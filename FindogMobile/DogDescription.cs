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
using BusinessLogic.Models;
using Android.Graphics;
using FindogMobile.Models;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;

namespace FindogMobile
{
    [Activity(Label = "DogDescription")]
    public class DogDescription : Activity, IOnMapReadyCallback
    {
        TextView descriptionTextView, breedTextView, ownerNameTextView, ownerEmailTextView;
        ImageView dogImageView;
        Animal animal;

        public void OnMapReady(GoogleMap googleMap)
        {
            LatLng location = new LatLng(animal.Latitude, animal.Longitude);
            MarkerOptions markerOptions = new MarkerOptions();
            markerOptions.SetPosition(new LatLng(location.Latitude, location.Longitude));
            markerOptions.SetTitle("Found dog");
            
            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(location);
            builder.Zoom(16);
            CameraPosition cameraPosition = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

            googleMap.AnimateCamera(cameraUpdate);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DogDescription);

            descriptionTextView = FindViewById<TextView>(Resource.Id.tvDogDescription);
            ownerNameTextView = FindViewById<TextView>(Resource.Id.tvOwnerName);
            ownerEmailTextView = FindViewById<TextView>(Resource.Id.tvOwnerEmail);
            breedTextView = FindViewById<TextView>(Resource.Id.tvDogBreed);
            dogImageView = FindViewById<ImageView>(Resource.Id.ivDogDescription);

            Intent intent = Intent;
            Bundle bundle = intent.Extras.GetBundle("bundle");
            animal = new Animal();
            animal.Image= bundle.GetByteArray("image");
            animal.Breed = bundle.GetString("breed");
            animal.Description = bundle.GetString("description");
            animal.Latitude = bundle.GetDouble("latitude");
            animal.Longitude = bundle.GetDouble("longitude");
            // Create your application here


            ownerNameTextView.Text = MobileUser.Instance().User.Name;
            ownerEmailTextView.Text = MobileUser.Instance().User.EmailAddress;

            descriptionTextView.Text = animal.Description;
            breedTextView.Text = animal.Breed;

            Bitmap bm = BitmapFactory.DecodeByteArray(animal.Image, 0, animal.Image.Length);
            dogImageView.SetImageBitmap(bm);

            var _mapFragment = FragmentManager.FindFragmentByTag("mapDescriptions") as MapFragment;

            if (_mapFragment == null)
            {
                GoogleMapOptions mapOptions = new GoogleMapOptions()
                    .InvokeMapType(GoogleMap.MapTypeHybrid)
                    .InvokeZoomControlsEnabled(true)
                    .InvokeCompassEnabled(true);

                FragmentTransaction fragTx = FragmentManager.BeginTransaction();
                _mapFragment = MapFragment.NewInstance(mapOptions);
                fragTx.Add(Resource.Id.map, _mapFragment, "mapDescriptions");
                fragTx.Commit();

                _mapFragment.GetMapAsync(this);
            }
        }
    }
}