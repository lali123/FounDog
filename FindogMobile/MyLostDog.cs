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
        DogAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MyLosts);
            // Create your application here
            result = FetchAnimalsAsync();

            adapter = new DogAdapter(this, result);
            uploadList = FindViewById<ListView>(Resource.Id.MyLostsList);
            uploadList.Adapter = adapter;
            RegisterForContextMenu(uploadList);
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
            
        }
        
        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            if (v.Id == Resource.Id.MyLostsList)
            {
                var menuItems = Resources.GetStringArray(Resource.Array.menu);
                for (var i = 0; i < menuItems.Length; i++)
                    menu.Add(Menu.None, i, i, menuItems[i]);
            }
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            var info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
            var menuItemIndex = item.ItemId;
            var menuItems = Resources.GetStringArray(Resource.Array.menu);
            var menuItemName = menuItems[menuItemIndex];
            var animal = result[info.Position];
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("One month");
            alert.SetMessage("Are you sure you want to delete " + animal.Breed + " ?");
            alert.SetPositiveButton("Cancel", (senderAlert, args) => {
            });

            alert.SetNegativeButton("Delete", (senderAlert, args) => {
                RemoveAnimal(animal.AnimalIdToString());
                result.Remove(animal);
                adapter.NotifyDataSetChanged();
                Toast.MakeText(this, string.Format("{0} deleted", animal.Breed), ToastLength.Short).Show();
            });

            Dialog dialog = alert.Create();
            dialog.Show();

            return true;
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