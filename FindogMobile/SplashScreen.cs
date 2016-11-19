using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using BusinessLogic.Models;
using Android.Preferences;
using FindogMobile.Models;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using FindogMobile.Services;

namespace FindogMobile
{
    [Activity(Label = "FinDog", MainLauncher = true, Icon = "@drawable/Dog", Theme = "@style/Theme.AppCompat.Light.NoActionBar.Splash", NoHistory = true)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }

        protected override void OnResume()
        {
            base.OnResume();

            Task startupWork = new Task(() =>
            {
                App.IsUserAlreadyRegistered = UserAlreadyRegistered();
                StartService(new Intent(this, typeof(DogService)));
            });

            startupWork.ContinueWith(t =>
            {
                StartActivity(new Intent(Application.Context, typeof(Login)));
            }, TaskScheduler.FromCurrentSynchronizationContext());

            startupWork.Start();
        }

        private bool UserAlreadyRegistered()
        {
            try
            {
                ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);

                User user = new User()
                {
                    // Date = prefs.GetString("Date", DateTime.Now.ToString()),
                    EmailAddress = prefs.GetString("Email", ""),
                    Name = prefs.GetString("Name", ""),
                    PhoneNumber = prefs.GetString("Phone", ""),
                    Id = prefs.GetString("Id", ""),
                    Password = prefs.GetString("Password", ""),
                };
                MobileUser.Instance().User = user;

                CachedData.Instance().Users = GetUsersFromDb();
                CachedData.Instance().FoundAnimals = GetFoundAnimalsFromDb();
                CachedData.Instance().WantedAnimals = GetWantedAnimalsFromDb();

                foreach (var u in CachedData.Instance().Users)
                {
                    if (u.Name.Equals(user.Name) && u.PhoneNumber.Equals(user.PhoneNumber))
                    {
                        MobileUser.Instance().User = u;
                        return true;
                    }
                }

            }
            catch (Exception)
            {
                Toast.MakeText(this, "Cannot connect to the server", ToastLength.Short).Show();

                return false;
            }


            return false;
        }

        private List<User> GetUsersFromDb()
        {
            string responseFromServer = String.Empty;
            List<User> users = new List<User>();

            try
            {
                // Create a request for the URL. 		
                WebRequest request = WebRequest.Create(WebApiConnection.Instance().ConnectionString + @"user/users");
                // If required by the server, set the credentials.
                request.Credentials = CredentialCache.DefaultCredentials;
                // Get the response.
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    // Get the stream containing content returned by the server.
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        // Read the content.
                        using (StreamReader reader = new StreamReader(dataStream))
                        {
                            responseFromServer = reader.ReadToEnd();
                        }
                    }
                }

                JArray jUsers = JArray.Parse(responseFromServer);

                foreach (var u in jUsers)
                {
                    User user = new User()
                    {
                        //Date = DateTime.Now,
                        EmailAddress = u["emailAddress"].ToString(),
                        Name = u["name"].ToString(),
                        PhoneNumber = u["phoneNumber"].ToString(),
                        Id = u["id"].ToString(),
                        Password = u["password"].ToString(),
                    };

                    users.Add(user);
                }
            }
            catch (Exception)
            {
                Toast.MakeText(this, "Cannot connect to the server", ToastLength.Short).Show();
            }

            return users;
        }

        private List<Animal> GetWantedAnimalsFromDb()
        {
            List<Animal> dogs = new List<Animal>();
            try
            {
                string responseFromServer = String.Empty;
                // Create a request for the URL. 		
                WebRequest request = WebRequest.Create(WebApiConnection.Instance().ConnectionString + @"animal/wantedanimals");
                // If required by the server, set the credentials.
                request.Credentials = CredentialCache.DefaultCredentials;
                // Get the response.
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    // Get the stream containing content returned by the server.
                    using (System.IO.Stream dataStream = response.GetResponseStream())
                    {
                        // Read the content.
                        using (StreamReader reader = new StreamReader(dataStream))
                        {
                            responseFromServer = reader.ReadToEnd();
                        }
                    }
                }

                JArray animals = JArray.Parse(responseFromServer);

                foreach (var animal in animals)
                {
                    Animal dog = new Animal();
                    dog.AnimalIdToObjectId(animal["animalId"].ToString());
                    dog.UserId = animal["userId"].ToString();
                    dog.Breed = animal["breed"].ToString();
                    dog.Description = animal["description"].ToString();
                    dog.Date = animal["date"].ToObject<DateTime>();
                    dog.Image = animal["image"].ToObject<byte[]>();
                    dog.Latitude = animal["latitude"].ToObject<double>();
                    dog.Longitude = animal["longitude"].ToObject<double>();
                    dogs.Add(dog);
                }
            }
            catch (Exception)
            {
                Toast.MakeText(this, "Cannot connect to the server", ToastLength.Short).Show();
            }


            return dogs;
        }

        private List<Animal> GetFoundAnimalsFromDb()
        {
            List<Animal> dogs = new List<Animal>();
            try
            {
                string responseFromServer = String.Empty;
                // Create a request for the URL. 		
                WebRequest request = WebRequest.Create(WebApiConnection.Instance().ConnectionString + @"animal/foundanimals");
                // If required by the server, set the credentials.
                request.Credentials = CredentialCache.DefaultCredentials;
                // Get the response.
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    // Get the stream containing content returned by the server.
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        // Read the content.
                        using (StreamReader reader = new StreamReader(dataStream))
                        {
                            responseFromServer = reader.ReadToEnd();
                        }
                    }
                }

                JArray animals = JArray.Parse(responseFromServer);

                foreach (var animal in animals)
                {
                    Animal dog = new Animal();
                    dog.UserId = animal["userId"].ToString();
                    dog.Breed = animal["breed"].ToString();
                    dog.Description = animal["description"].ToString();
                    dog.Date = animal["date"].ToObject<DateTime>();
                    dog.Image = animal["image"].ToObject<byte[]>();
                    dog.Latitude = animal["latitude"].ToObject<double>();
                    dog.Longitude = animal["longitude"].ToObject<double>();
                    dogs.Add(dog);
                }
            }
            catch (Exception)
            {
                Toast.MakeText(this, "Cannot connect to the server", ToastLength.Short).Show();
            }


            return dogs;
        }
    }
}