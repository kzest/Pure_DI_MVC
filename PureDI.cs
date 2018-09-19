using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Pure_DI
{
    public class PureDI : System.Web.Mvc.DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            var result = InjectControllerDependency(controllerType);

            if (result != null)
                return result;
            else
                return base.GetControllerInstance(requestContext, controllerType);
        }


        public static string ConnectionString { get; set; }

        public IController InjectControllerDependency(Type controllerType)
        {
            List<ConstructorInfo> constructorInfos = controllerType.GetConstructors().ToList();

            if (constructorInfos.Count == 1 && (constructorInfos[0].GetParameters()).Length == 0)
            {
                return (IController)constructorInfos[0].Invoke(new object[0]);
            }
            else
            {
                var constructorInfo = constructorInfos.Where(x => x.GetParameters().Length > 0).FirstOrDefault();

                List<ParameterInfo> parameterInfos = constructorInfo.GetParameters().ToList();
                List<object> args = new List<object>();
                foreach(ParameterInfo paramInfo in parameterInfos)
                {
                    if(paramInfo.Name.Equals("connectionString", StringComparison.InvariantCultureIgnoreCase))
                    {
                        args.Add(ConnectionString);
                        continue;
                    }

                    ObjectGraph objectGraph = new ObjectGraph();
                    Type paramInstanceType = null;
                    objectGraph.ContractInstanceGraph.TryGetValue(paramInfo.ParameterType, out paramInstanceType);

                    if (paramInstanceType == null)
                    {
                        return null;
                    }
                    else
                    {
                        args.Add(InjectDependency(paramInstanceType));
                    }
                }

                return (IController)constructorInfo.Invoke(args.ToArray());
            }
        }

        public object InjectDependency (Type instanceType)
        {
            List<ConstructorInfo> constructorInfos = instanceType.GetConstructors().ToList();

            if (constructorInfos.Count == 1 && (constructorInfos[0].GetParameters()).Length == 0)
            {
                return constructorInfos[0].Invoke(new object[0]);
            }
            else
            {
                var constructorInfo = constructorInfos.Where(x => x.GetParameters().Length > 0).FirstOrDefault();

                List<ParameterInfo> parameterInfos = constructorInfo.GetParameters().ToList();
                List<object> args = new List<object>();
                foreach (ParameterInfo paramInfo in parameterInfos)
                {
                    if (paramInfo.Name.Equals("connectionString", StringComparison.InvariantCultureIgnoreCase))
                    {
                        args.Add(ConnectionString);
                        continue;
                    }

                    ObjectGraph objectGraph = new ObjectGraph();
                    Type paramInstanceType = null;
                    objectGraph.ContractInstanceGraph.TryGetValue(paramInfo.ParameterType, out paramInstanceType);

                    if (paramInstanceType == null)
                    {
                        throw new Exception("Type not added in Object Graph");
                    }
                    else
                    {
                        args.Add(InjectDependency(paramInstanceType));
                    }
                }

                return constructorInfo.Invoke(args.ToArray());
            }
        }

    }
}
