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
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using FindogMobile.Models;
using BusinessLogic.Models;
using FindogMobile.Adapters;

namespace FindogMobile
{
    [Activity(Label = "MyLostDog")]
    public class MyLostDog : Activity
    {
        ListView uploadList;
        List<Animal> result;
        public Animal SelectedAnimal { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MyLosts);
            // Create your application here
            result = FetchAnimalsAsync();

            var adapter = new DogAdapter(this, result);
            uploadList = FindViewById<ListView>(Resource.Id.MyLostsList);
            uploadList.Adapter = adapter;
            uploadList.ItemClick += (s, e) =>
            {
                var listView = s as ListView;
                var t = result[e.Position];
                SelectedAnimal =t;
                Toast.MakeText(this, t.Breed, ToastLength.Short).Show();
                App.Tag = "lost";
                Intent intent = new Intent(this, typeof(UpdateFindog));
                Bundle bundle = new Bundle();
                bundle.PutString("AnimalId", t.AnimalIdToString());
                bundle.PutString("userId", t.UserId.ToString());
                bundle.PutString("breed", t.Breed);
                bundle.PutDouble("latitude", t.Latitude);
                bundle.PutString("description", t.Description);
                bundle.PutByteArray("image", t.Image);
                bundle.PutDouble("longitude", t.Longitude);
                bundle.PutDouble("latitude", t.Latitude);
                intent.PutExtra("bundle", bundle);
                StartActivity(intent);
            };
            uploadList.ItemLongClick += (s, e) =>
            {
                var listView = s as ListView;
                var animal = result[e.Position];
                PopupMenu menu = new PopupMenu(this, uploadList);
                menu.Inflate(Resource.Menu.PopupMenu);
                menu.MenuItemClick += (se, ev) =>
                {
                    RemoveAnimal(animal.AnimalIdToString());
                    result.Remove(animal);

                    adapter.NotifyDataSetChanged();
                    Toast.MakeText(this, "Deleted", ToastLength.Short).Show();
                };
                menu.Show();
            };
        }


        private void RemoveAnimal(string id)
        {
            try
            {
                string responseFromServer = String.Empty;
                // Create a request for the URL. 		


                WebRequest request = WebRequest.Create(WebApiConnection.Instance().ConnectionString + @"animal/deletewantedanimal/" + id);
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
            }
            catch (Exception)
            {
                Toast.MakeText(this, "Cannot connect to the server", ToastLength.Short).Show();
            }
        }

        private List<Animal> FetchAnimalsAsync()
        {
            List<Animal> dogs = new List<Animal>();
            try
            {
                string responseFromServer = String.Empty;
                // Create a request for the URL. 		
                WebRequest request = WebRequest.Create(WebApiConnection.Instance().ConnectionString + @"animal/wantedanimals/" + MobileUser.Instance().User.Id);
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
    }
}