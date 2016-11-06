using BusinessLogic.Models;
using BusinessLogic.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FindogTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //WriteAUser();
            //ReadUsers();

            //WriteAnAnimal();
            //ReadAnAnimal();
            //ReadAnAnimalByCoordinates();

            //TestPost();

            TestUpdate();

            Console.ReadKey();
        }

        private static async void TestUpdate()
        {
            var path = Directory.GetCurrentDirectory() + "\\Resources\\dogee.jpeg";
            var image = new Bitmap(path);

            Image img = Image.FromFile(path);
            byte[] arr;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                arr = ms.ToArray();
            }

            Animal animal = new Animal()
            {
                UserId = new Guid(),
                Date = DateTime.Now,
                Description = "test",
                Latitude = 47.5371291,
                Longitude = 21.623794299999986,
                Breed = "test",
                Image = arr,
            };

            var settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8086/");
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var json = JsonConvert.SerializeObject(animal, settings);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                // HTTP POST
                HttpResponseMessage response = await client.PostAsync("animal/updatefoundanimal/581f9d62a314a70524a39d10", content);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    //animal = JsonConvert.DeserializeObject<Animal>(data);
                }
                Console.WriteLine(response.StatusCode);
            }
        }

        class PostClass
        {
            public string first { get; set; }
            public int second { get; set; }
            public bool third { get; set; }
        }

        private static async void TestPost()
        {
            var path = Directory.GetCurrentDirectory() + "\\Resources\\dogee.jpeg";
            var image = new Bitmap(path);

            Image img = Image.FromFile(path);
            byte[] arr;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                arr = ms.ToArray();
            }

            Animal animal = new Animal()
            {
                UserId = new Guid(),
                Date = DateTime.Now,
                Description = "Keverék",
                Latitude = 47.5371291,
                Longitude = 21.623794299999986,
                Breed = "Keverék",
                Image = arr,
            };

            var settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8086/");
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var json = JsonConvert.SerializeObject(animal, settings);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                // HTTP POST
                HttpResponseMessage response = await client.PostAsync("animal/savefound", content);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    //animal = JsonConvert.DeserializeObject<Animal>(data);
                }
            }
        }


        //private static void ReadAnAnimalByCoordinates()
        //{
        //    var coordinate = new GeoCoordinate() { Latitude = 0.00001, Longitude = 0.000001 };
        //    var list = ReadFromDatabase.ReadAnimalFromDatabaseByLocation(coordinate);
        //    Console.WriteLine("By location");
        //    foreach (var item in list)
        //    {
        //        Console.WriteLine(item);
        //    }
        //}

        private static void ReadUsers()
        {
            var list = ReadFromDatabase.ReadUsersFromDatabase();
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }

        private static void WriteAUser()
        {
            var user = new User()
            {
                Name = "Turi Lajos",
                EmailAddress = "turi.lajos1@gmail.com",
                PhoneNumber = "06703286699",
                Date = DateTime.Now,
            };
            WriteToDatabase.WriteUserToDatabase(user);
        }

        private static void ReadAnAnimal()
        {
            var list = ReadFromDatabase.ReadAnimalFromDatabase();
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }

        private static void WriteAnAnimal()
        {
            var path = Directory.GetCurrentDirectory() + "\\Resources\\dogee.jpeg";
            var image = new Bitmap(path);

            Image img = Image.FromFile(path);
            byte[] arr;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                arr = ms.ToArray();
            }

            Animal animal = new Animal()
            {
                Date = DateTime.Now,
                Description = "Keverék",
                //Coordinates = new GeoCoordinate() { Latitude = 47.5371291, Longitude = 21.623794299999986 },
                Image = arr,
            };
            WriteToDatabase.WriteFoundAnimalToDatabase(animal);
            WriteToDatabase.WriteWantedAnimalToDatabase(animal);
        }
    }
}
