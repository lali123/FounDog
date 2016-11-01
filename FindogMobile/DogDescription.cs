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

namespace FindogMobile
{
    [Activity(Label = "DogDescription")]
    public class DogDescription : Activity
    {
        TextView descriptionTextView, breedTextView, ownerNameTextView, ownerEmailTextView;
        ImageView dogImageView; 

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
            Animal animal = new Animal();
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
        }
    }
}