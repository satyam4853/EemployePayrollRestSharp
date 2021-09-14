using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmployeePayrollRestSharp;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System;

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
        /// <summary>
        /// Method to get all employee details from server
        /// </summary>
        /// <returns></returns>
        public IRestResponse GetAllEmployee()
        {
            //Get request from json server
            RestRequest request = new RestRequest("/employees", Method.GET);
            //Requesting server and execute , getting response
            IRestResponse response = client.Execute(request);

            return response;
        }
        /// <summary>
        /// Test method to get all employee details
        /// </summary>
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
        /// <summary>
        /// Method to add a json object to json server
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IRestResponse AddToJsonServer(JsonObject json)
        {
            RestRequest request = new RestRequest("/employees", Method.POST);
            //adding type as json in request and pasing the json object as a body of request
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            //Execute the request
            IRestResponse response = client.Execute(request);
            return response;

        }
        /// <summary>
        /// Test method to add a new employee to the json server
        /// </summary>
        [TestMethod]
        public void TestMethodToAddEmployeeToJsonServerFile()
        {
            //Setting rest rquest to url and setiing method to post
            RestRequest request = new RestRequest("/employees", Method.POST);
            //object for json
            JsonObject json = new JsonObject();
            //Adding new employee details to json object
            json.Add("name", "Neymar");
            json.Add("salary", 13000);

            //calling method to add to server
            IRestResponse response = AddToJsonServer(json);
            //deserialize json objject to employee class  object
            var res = JsonConvert.DeserializeObject<Employee>(response.Content);

            //Checking the response statuscode 201-created
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            //checking object values
            Assert.AreEqual("Messi", res.name);
            Console.WriteLine($"id = {res.id} , name = {res.name} , salary = {res.salary}");
        }


    }
}