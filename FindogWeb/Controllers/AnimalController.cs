using BusinessLogic.Models;
using BusinessLogic.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace FindogWeb.Controllers
{
    [RoutePrefix("animal")]
    public class AnimalController : ApiController
    {
        // GET: Animals
        [Route("foundanimals")]
        [HttpGet]
        public List<Animal> GetFoundAnimals()
        {
            return ReadFromDatabase.ReadAnimalFromDatabase();
        }

        // GET: Animals
        [Route("wantedanimals")]
        [HttpGet]
        public List<Animal> GetWantedAnimals()
        {
            return ReadFromDatabase.ReadWantedAnimalFromDatabase();
        }

        // GET: Animal
        [Route("foundanimals/{userId}")]
        [HttpGet]
        public List<Animal> GetFoundAnimal(string userId)
        {
            return ReadFromDatabase.ReadAnimalFromDatabase(userId);
        }

        // GET: Animal
        [Route("wantedanimals/{userId}")]
        [HttpGet]
        public List<Animal> GetWantedAnimals(string userId)
        {
            return ReadFromDatabase.ReadWantedAnimalFromDatabase(userId);
        }

        [Route("savefound")]
        [HttpPost]
        public HttpResponseMessage SaveFoundAnimals(Object model)
        {
            try
            {
                var jsonString = model.ToString();
                JToken animal = JToken.Parse(jsonString);

                Animal dog = new Animal();
                dog.AnimalIdToObjectId(animal["animalId"].ToString());
                dog.UserId = animal["userId"].ToString();
                dog.Breed = animal["breed"].ToString();
                dog.Description = animal["description"].ToString();
                dog.Date = animal["date"] == null ? DateTime.Now : animal["date"].ToObject<DateTime>();
                dog.Latitude = animal["latitude"] == null ? 0 : animal["latitude"].ToObject<double>();
                dog.Longitude = animal["longitude"] == null ? 0 : animal["longitude"].ToObject<double>();
                dog.Image = animal["image"] == null ? null : animal["image"].ToObject<byte[]>();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(dog.UserId.ToString());

                File.WriteAllText(@"C:\Users\Lajos\Desktop\debug.txt", sb.ToString());
                WriteToDatabase.WriteFoundAnimalToDatabase(dog);

                var response = Request.CreateResponse<Animal>(System.Net.HttpStatusCode.Created, dog);

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

        [Route("savewanted")]
        [HttpPost]
        public HttpResponseMessage SaveWantedAnimals(Object model)
        {
            try
            {
                var jsonString = model.ToString();
                JToken animal = JToken.Parse(jsonString);

                Animal dog = new Animal();
                //dog.AnimalIdToObjectId(animal["animalId"].ToObject<ObjectId>);
                dog.UserId = animal["userId"].ToString();
                dog.Breed = animal["breed"].ToString();
                dog.Description = animal["description"].ToString();
                dog.Date = animal["date"] == null ? DateTime.Now : animal["date"].ToObject<DateTime>();
                dog.Latitude = animal["latitude"] == null ? 0 : animal["latitude"].ToObject<double>();
                dog.Longitude = animal["longitude"] == null ? 0 : animal["longitude"].ToObject<double>();
                dog.Image = animal["image"] == null ? null : animal["image"].ToObject<byte[]>();

                WriteToDatabase.WriteWantedAnimalToDatabase(dog);

                var response = Request.CreateResponse<Animal>(System.Net.HttpStatusCode.Created, dog);

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
        [Route("deletefoundanimal/{id}")]
        [HttpGet]
        public HttpResponseMessage DeleteFoundAnimal(string id)
        {
            try
            {
                var response = Request.CreateResponse<string>(System.Net.HttpStatusCode.Accepted, id);
                DeleteFromDatabase.DeleteFoundAnimal(id);
                return response;
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, id);
            }
        }

        // GET: Animal
        [Route("deletewantedanimal/{id}")]
        [HttpGet]
        public HttpResponseMessage DeleteWantedAnimal(string id)
        {
            try
            {
                var response = Request.CreateResponse<string>(System.Net.HttpStatusCode.Accepted, id);
                DeleteFromDatabase.DeleteWantedAnimal(id);
                return response;
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, id);
            }
        }

        // GET: Animal
        [Route("updatefoundanimal/{id}")]
        [HttpPost]
        public HttpResponseMessage UpdateAnimal(object model, string id)
        {
            try
            {
                var jsonString = model.ToString();
                JToken animal = JToken.Parse(jsonString);

                Animal dog = new Animal();
                dog.AnimalIdToObjectId(id);
                dog.UserId = animal["userId"].ToString();
                dog.Breed = animal["breed"].ToString();
                dog.Description = animal["description"].ToString();
                dog.Date = animal["date"] == null ? DateTime.Now : animal["date"].ToObject<DateTime>();
                dog.Latitude = animal["latitude"] == null ? 0 : animal["latitude"].ToObject<double>();
                dog.Longitude = animal["longitude"] == null ? 0 : animal["longitude"].ToObject<double>();
                dog.Image = animal["image"] == null ? null : animal["image"].ToObject<byte[]>();

                var result = UpdateDatabase.UpdateFoundAnimal(dog);
                if (result)
                {
                    return Request.CreateResponse<Animal>(System.Net.HttpStatusCode.Accepted, dog);
                }
                else
                {
                    return Request.CreateResponse<Animal>(System.Net.HttpStatusCode.NotAcceptable, dog);
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