using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System;
using EmployeePayrollRestSharp;


namespace PayrollTest
{
    [TestClass]
    public class PayrollRestSharpTest
    {
        //Initializing the restclient 
        RestClient client;

        [TestInitialize]

        public void Setup()
        {
            client = new RestClient("http://localhost:3000");
        }

        public IRestResponse GetAllEmployee()
        {
            //Get request from json server
            RestRequest request = new RestRequest("/employees", Method.GET);
            //Requesting server and execute , getting response
            IRestResponse response = client.Execute(request);

            return response;
        }

        [TestMethod]
        public void TestMethodToGetAllEmployees()
        {
            //calling get all employee method 
            IRestResponse response = GetAllEmployee();
            //converting response to list og objects
            var res = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            //Check whether all contents are received or not
            Assert.AreEqual(3, res.Count);
            //Checking the response statuscode 200-ok
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            res.ForEach((x) =>
            {
                Console.WriteLine($"id = {x.id} , name = {x.name} , salary = {x.salary}");
            });


        }
    }
}
