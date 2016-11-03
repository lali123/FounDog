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
using System.Json;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using BusinessLogic.Models;
using System.Net.Http.Headers;
using Android.Graphics;
using Java.IO;
using Newtonsoft.Json.Serialization;
using Android.Graphics.Drawables;
using FindogMobile.Helpers;
using FindogMobile.Models;

namespace FindogMobile.Fragments
{
    public class SaveFragment : Fragment
    {
        Button saveDog;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.SaveFounDog, container, false);
            saveDog = view.FindViewById<Button>(Resource.Id.btnSaveDog);

            saveDog.Click += (s, e) =>
           {
               FetchAnimalsAsync();
           };

            return view;
        }

        private async void FetchAnimalsAsync()
        {
            Bitmap bmp = null;
            byte[] byteArray = null;
            if (App._file != null)
            {
                bmp = App._file.Path.LoadAndResizeBitmap(100, 100);
            }
            else
            {
                bmp = BitmapFactory.DecodeResource(Resources, Resource.Drawable.Dog);
            }

            using (var stream = new MemoryStream())
            {
                bmp.Compress(Bitmap.CompressFormat.Png, 0, stream);
                byteArray = stream.ToArray();
            }

            Animal animal = new Animal()
            {
                UserId = MobileUser.Instance().User.Id,
                Date = DateTime.Now,
                Description = (Activity as FindDog).Description,
                Latitude = (Activity as FindDog).Latitude,
                Longitude = (Activity as FindDog).Longitude,
                Breed = (Activity as FindDog).Breed,
                Image = byteArray,
            };

            try
            {
                var settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(WebApiConnection.Instance().ConnectionString);

                    var json = JsonConvert.SerializeObject(animal, settings);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    // HTTP POST
                    string postString = "";
                    if (App.UploadAnimal.Equals("found"))
                    {
                        postString = "animal/savefound";
                    }
                    else if (App.UploadAnimal.Equals("lost"))
                    {
                        postString = "animal/savewanted";
                    }
                    HttpResponseMessage response = await client.PostAsync(postString, content);
                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception)
            {
                Toast.MakeText(this.Activity, "Cannot connect to the server", ToastLength.Short).Show();
            }
           
        }
    }
}