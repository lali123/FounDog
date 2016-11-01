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
        [Route("{name}")]
        [HttpGet]
        public List<Animal> GetAnimal(string name)
        {
            return ReadFromDatabase.ReadAnimalFromDatabase();
        }

        [Route("savefound")]
        public HttpResponseMessage SaveAnimals(Object model)
        {
            try
            {
                var jsonString = model.ToString();
                JToken animal = JToken.Parse(jsonString);

                Animal dog = new Animal();
                dog.Breed = animal["breed"].ToString();
                dog.Description = animal["description"].ToString();
                dog.Date = animal["date"] == null ? DateTime.Now : animal["date"].ToObject<DateTime>();
                dog.Latitude = animal["latitude"] == null ? 0 : animal["latitude"].ToObject<double>();
                dog.Longitude = animal["longitude"] == null ? 0 : animal["longitude"].ToObject<double>();
                dog.Image = animal["image"] == null ? null : animal["image"].ToObject<byte[]>();

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

                File.WriteAllText(@"C:\Users\Lajos\Desktop\debug.txt", sb.ToString());

                return Request.CreateResponse(System.Net.HttpStatusCode.ExpectationFailed);
            }

        }
    }
}