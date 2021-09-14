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
            IRestResponse response = default;
            try
            {
                //Get request from json server
                RestRequest request = new RestRequest("/employees", Method.GET);
                //Requesting server and execute , getting response
                response = client.Execute(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return response;
        }
        /// <summary>
        /// Test method to get all employee details
        /// </summary>
        [TestMethod]
        public void TestMethodToGetAllEmployees()
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }



        }
        /// <summary>
        /// Method to add a json object to json server
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IRestResponse AddToJsonServer(JsonObject json)
        {
            IRestResponse response = default;
            try
            {
                RestRequest request = new RestRequest("/employees", Method.POST);
                //adding type as json in request and pasing the json object as a body of request
                request.AddParameter("application/json", json, ParameterType.RequestBody);

                //Execute the request
                response = client.Execute(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;

        }
        /// <summary>
        /// Test method to add a new employee to the json server
        /// </summary>
        [TestMethod]
        public void TestMethodToAddEmployeeToJsonServerFile()
        {
            try
            {
                //Setting rest rquest to url and setiing method to post
                RestRequest request = new RestRequest("/employees", Method.POST);
                //object for json
                JsonObject json = new JsonObject();
                //Adding new employee details to json object
                json.Add("name", "Raju");
                json.Add("salary", 13000);

                //calling method to add to server
                IRestResponse response = AddToJsonServer(json);
                //deserialize json objject to employee class  object
                var res = JsonConvert.DeserializeObject<Employee>(response.Content);

                //Checking the response statuscode 201-created
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

                //checking object values
                Assert.AreEqual("Omi", res.name);
                Console.WriteLine($"id = {res.id} , name = {res.name} , salary = {res.salary}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// test Method to add multiple data to the json server
        /// </summary>
        [TestMethod]
        public void TestForAddMultipleDataToJsonServerFile()
        {
            try
            {
                //list for storing multiple employeee data json objects
                List<JsonObject> employeeList = new List<JsonObject>();
                JsonObject json = new JsonObject();
                json.Add("name", "Shankar");
                json.Add("salary", 40000);
                //add object to list
                employeeList.Add(json);
                JsonObject json1 = new JsonObject();
                json1.Add("name", "Alex");
                json1.Add("salary", 35000);
                employeeList.Add(json1);

                employeeList.ForEach((x) =>
                {
                    AddToJsonServer(x);
                });
                //Check by gettting all employee details
                IRestResponse response = GetAllEmployee();
                //convert json object to employee object
                var res = JsonConvert.DeserializeObject<List<Employee>>(response.Content);

                res.ForEach((x) =>
                {
                    Console.WriteLine($"id = {x.id} , name = {x.name} , salary = {x.salary}");
                });
                //Checking the response statuscode 200-ok
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

    }
}