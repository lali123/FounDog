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
using Android.Gms.Maps;
using Android.Support.V7.App;
using Android.Gms.Maps.Model;
using BusinessLogic.Models;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using FindogMobile.Models;
using static Android.Gms.Maps.GoogleMap;
using Android.Graphics;

namespace FindogMobile
{
    [Activity(Label = "DogsOnMap")]
    public class DogsOnMap : Activity, IOnMapReadyCallback, IInfoWindowAdapter
    {
        GoogleMap map;
        List<Animal> Dogs;

        public void OnMapReady(GoogleMap googleMap)
        {
            LatLng location = new LatLng(47.5316049, 21.6273123);
            foreach (var dog in Dogs)
            {
                LatLng dogLocation = new LatLng(dog.Latitude, dog.Longitude);
                MarkerOptions markerOptions = new MarkerOptions();
                markerOptions.SetPosition(new LatLng(dogLocation.Latitude, dogLocation.Longitude));
                markerOptions.SetTitle("Found dog");
                markerOptions.SetSnippet(dog.AnimalIdToString());

                Marker marker = googleMap.AddMarker(markerOptions);
                
            }
            
            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(location);
            builder.Zoom(16);
            CameraPosition cameraPosition = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

            googleMap.AnimateCamera(cameraUpdate);
            googleMap.MyLocationEnabled = true;
            googleMap.SetInfoWindowAdapter(this);

            map = googleMap;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MapLayout);
            // Create your application here

            var result = FetchAnimalsAsync(@"animal/foundanimals");
            var result2 = FetchAnimalsAsync(@"animal/wantedanimals");
            Dogs = new List<Animal>();
            Dogs.AddRange(result);
            Dogs.AddRange(result2);

            var _mapFragment = FragmentManager.FindFragmentByTag("map") as MapFragment;

            if (_mapFragment == null)
            {
                GoogleMapOptions mapOptions = new GoogleMapOptions()
                    .InvokeMapType(GoogleMap.MapTypeHybrid)
                    .InvokeZoomControlsEnabled(true)
                    .InvokeCompassEnabled(true);

                FragmentTransaction fragTx = FragmentManager.BeginTransaction();
                _mapFragment = MapFragment.NewInstance(mapOptions);
                fragTx.Add(Resource.Id.map, _mapFragment, "map");
                fragTx.Commit();
                map = _mapFragment.Map;
                _mapFragment.GetMapAsync(this);
            }
        }

        private List<Animal> FetchAnimalsAsync(string requestString)
        {
            List<Animal> dogs = new List<Animal>();
            try
            {
                string responseFromServer = String.Empty;
                // Create a request for the URL. 		
                WebRequest request = WebRequest.Create(WebApiConnection.Instance().ConnectionString + requestString);
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

        public View GetInfoContents(Marker marker)
        {
            return null;
        }

        public View GetInfoWindow(Marker marker)
        {
            View view = LayoutInflater.Inflate(Resource.Layout.MarkerInfo, null, false);
            var animal = Dogs.Where(d => d.AnimalIdToString().Equals(marker.Snippet)).FirstOrDefault();
            TextView breed = view.FindViewById<TextView>(Resource.Id.markerBreed);
            TextView description = view.FindViewById<TextView>(Resource.Id.markerDescription);
            ImageView image = view.FindViewById<ImageView>(Resource.Id.markerImage);

            breed.Text = animal.Breed;
            description.Text = animal.Description;

            Bitmap bm = BitmapFactory.DecodeByteArray(animal.Image, 0, animal.Image.Length);
            image.SetImageBitmap(bm);

            return view;
        }
    }
}