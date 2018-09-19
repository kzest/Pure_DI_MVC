using CodeAssignment1.EmployeeDomainBusiness;
using CodeAssignment1.Repository;
using CodeAssignment1.ServiceContract;
using CodeAssignment1.Services;
using EmployeeDomainBusinessContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pure_DI
{
    public class ObjectGraph
    {
        public Dictionary<Type, Type> ContractInstanceGraph { get; set; }

        public ObjectGraph()
        {
            //Initialization
            this.ContractInstanceGraph = new Dictionary<Type, Type>();

            //Employee Domain
            this.ContractInstanceGraph.Add(typeof(IEmployeeService), typeof(EmployeeService));
            this.ContractInstanceGraph.Add(typeof(IEmployeeDomainBusinessContract), typeof(EmployeeBusiness));
            this.ContractInstanceGraph.Add(typeof(IEmployeeRepositoryContract), typeof(EmployeeRepository));

            
        }
    }
}