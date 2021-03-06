﻿using BusinessLogic.Models;
using BusinessLogic.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace FindogWeb.Controllers
{
    [RoutePrefix("user")]
    public class UserController : ApiController
    {
        // GET api/Users
        [Route("users")]
        [HttpGet]
        public List<User> GetUsers()
        {
            return ReadFromDatabase.ReadUsersFromDatabase();
        }

        // GET api/User
        [Route("{name}")]
        [HttpGet]
        public List<User> GetUser(string name)
        {
            return ReadFromDatabase.ReadUsersFromDatabase(name);
        }

        // GET api/User
        [Route("id/{id}")]
        [HttpGet]
        public List<User> GetUserById(string id)
        {
            return ReadFromDatabase.ReadUsersFromDatabaseById(id);
        }

        [Route("registeruser")]
        public HttpResponseMessage SaveUser(Object model)
        {
            try
            {
                var jsonString = model.ToString();
                
                JToken jUser = JToken.Parse(jsonString);

                User user = new User();
                user.Id = jUser["id"].ToString();
                user.Date = jUser["date"] == null ? DateTime.Now : jUser["date"].ToObject<DateTime>();
                user.EmailAddress = jUser["emailAddress"].ToString();
                user.Name = jUser["name"].ToString();
                user.PhoneNumber = jUser["phoneNumber"].ToString();
                user.Password = jUser["password"].ToString();

                WriteToDatabase.WriteUserToDatabase(user);

                var response = Request.CreateResponse<User>(System.Net.HttpStatusCode.Created, user);

                return response;
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.ToString());
                sb.AppendLine(ex.StackTrace);
                sb.AppendLine("MESSAGE: " + ex.Message);
                sb.AppendLine("SOURCE: " + ex.Source);
                sb.AppendLine(model.ToString());

                File.WriteAllText(@"C:\Users\Lajos\Desktop\debug.txt", sb.ToString());

                return Request.CreateResponse(System.Net.HttpStatusCode.ExpectationFailed);
            }

        }
        
        // GET: Animal
        [Route("deleteuser/{id}")]
        [HttpGet]
        public HttpResponseMessage DeleteUser(string id)
        {
            try
            {
                var response = Request.CreateResponse<string>(System.Net.HttpStatusCode.Accepted, id);
                DeleteFromDatabase.DeleteUser(id);
                return response;
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, id);
            }
        }

        // GET: Animal
        [Route("updateuser/{id}")]
        [HttpPost]
        public HttpResponseMessage UpdateUser(object model, string id)
        {
            try
            {
                var jsonString = model.ToString();

                JToken jUser = JToken.Parse(jsonString);

                User user = new User();
                user.Id = jUser["id"].ToString();
                user.Date = jUser["date"] == null ? DateTime.Now : jUser["date"].ToObject<DateTime>();
                user.EmailAddress = jUser["emailAddress"].ToString();
                user.Name = jUser["name"].ToString();
                user.PhoneNumber = jUser["phoneNumber"].ToString();
                user.Password = jUser["password"].ToString();

                var result = UpdateDatabase.UpdateUser(user);
                if (result)
                {
                    return Request.CreateResponse<User>(System.Net.HttpStatusCode.Accepted, user);
                }
                else
                {
                    return Request.CreateResponse<User>(System.Net.HttpStatusCode.NotAcceptable, user);
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.ToString());
                sb.AppendLine(ex.StackTrace);
                sb.AppendLine("MESSAGE: " + ex.Message);
                sb.AppendLine("SOURCE: " + ex.Source);
                sb.AppendLine(model.ToString());

                File.WriteAllText(@"C:\Users\Lajos\Desktop\debug.txt", sb.ToString());

                return Request.CreateResponse(System.Net.HttpStatusCode.ExpectationFailed);
            }
        }
    }
}
