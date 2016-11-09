using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using BusinessLogic.Models;
using FindogMobile.Models;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using Android.Graphics;
using FindogMobile.Helpers;
using System.IO;

namespace FindogMobile.Fragments
{
    public class UpdateFragment : Fragment
    {
        Button updateDog;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.UpdateDog, container, false);
            updateDog = view.FindViewById<Button>(Resource.Id.btnUpdateDog);

            updateDog.Click += (s, e) =>
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
                Description = (Activity as UpdateFindog).Description,
                Latitude = (Activity as UpdateFindog).Latitude,
                Longitude = (Activity as UpdateFindog).Longitude,
                Breed = (Activity as UpdateFindog).Breed,
                Image = byteArray,
            };
            string postString = "";
            if (App.Tag.Equals("found"))
            {
                postString = "animal/updatefoundanimal/";
            }
            else if (App.Tag.Equals("lost"))
            {
                postString = "animal/wantedanimals/";
            }

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