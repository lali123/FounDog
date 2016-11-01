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

namespace FindogMobile
{
    [Activity(Label = "Profile")]
    public class Profile : Activity
    {
        TextView nameTextView, emailTextView, phoneTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Profile);
            // Create your application here

            nameTextView = FindViewById<TextView>(Resource.Id.tvUserName);
            emailTextView = FindViewById<TextView>(Resource.Id.tvUserEmail);
            phoneTextView = FindViewById<TextView>(Resource.Id.tvUserPhone);

            nameTextView.Text = MobileUser.Instance().User.Name;
            emailTextView.Text = MobileUser.Instance().User.EmailAddress;
            phoneTextView.Text = MobileUser.Instance().User.PhoneNumber;

        }
    }
}