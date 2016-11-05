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
using FindogMobile.Models;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using FindogMobile.Adapters;
using BusinessLogic.Models;

namespace FindogMobile
{
    [Activity(Label = "Profile")]
    public class Profile : Activity
    {
        TextView nameTextView, emailTextView, phoneTextView;
        ListView uploadList;
        List<Animal> result;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Profile);
            // Create your application here

            result = FetchAnimalsAsync();
            var adapter = new DogAdapter(this, result);
            nameTextView = FindViewById<TextView>(Resource.Id.tvUserName);
            emailTextView = FindViewById<TextView>(Resource.Id.tvUserEmail);
            phoneTextView = FindViewById<TextView>(Resource.Id.tvUserPhone);
            uploadList = FindViewById<ListView>(Resource.Id.MyUploads);
            uploadList.Adapter = adapter;
            uploadList.ItemClick += (s, e) =>
            {
                var listView = s as ListView;
                var t = result[e.Position];
                Toast.MakeText(this, t.Breed, ToastLength.Short).Show();
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
                    Toast.MakeText(this, "Remove", ToastLength.Short).Show();
                };
                menu.Show();
            };

            nameTextView.Text = MobileUser.Instance().User.Name;
            emailTextView.Text = MobileUser.Instance().User.EmailAddress;
            phoneTextView.Text = MobileUser.Instance().User.PhoneNumber;

        }

        private void RemoveAnimal(string id)
        {
            try
            {
                string responseFromServer = String.Empty;
                // Create a request for the URL. 		
                WebRequest request = WebRequest.Create(WebApiConnection.Instance().ConnectionString + @"animal/deletefoundanimal/" + id);
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
                WebRequest request = WebRequest.Create(WebApiConnection.Instance().ConnectionString + @"animal/foundanimal/" + MobileUser.Instance().User.Id.ToString());
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
                    dog.UserId = new Guid(animal["userId"].ToString());
                    dog.Breed = animal["breed"].ToString();
                    dog.Description = animal["description"].ToString();
                    dog.Date = animal["date"].ToObject<DateTime>();
                    dog.Image = animal["image"].ToObject<byte[]>();
                    dog.Latitude = animal["latitude"].ToObject<int>();
                    dog.Longitude = animal["longitude"].ToObject<int>();
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