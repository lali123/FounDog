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
using BusinessLogic.Models;
using Android.Graphics;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using FindogMobile.Models;

namespace FindogMobile.Fragments
{
    public class FindogDetailsReadOnlyFragment : Android.Support.V4.App.Fragment
    {
        const string ARG_PAGE = "ARG_PAGE";
        private int mPage;
        public ImageView photoPreview, ivCallNumber, ivWriteEmail;
        public TextView tvBreed, tvDescription, tvUploaderName, tvUploaderPhone, tvUploaderEmail;
        static Animal mDog;
        Button btnWriteEmail, btnCallNumber;

        public static FindogDetailsReadOnlyFragment NewInstance(int page, Animal dog)
        {
            mDog = dog;
            var args = new Bundle();

            args.PutInt(ARG_PAGE, page);
            var fragment = new FindogDetailsReadOnlyFragment();
            fragment.Arguments = args;

            return fragment;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mPage = Arguments.GetInt(ARG_PAGE);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            var view = inflater.Inflate(Resource.Layout.FindogDetailsReadOnlyPage, container, false);

            photoPreview = view.FindViewById<ImageView>(Resource.Id.photoPreview);
            tvBreed = view.FindViewById<TextView>(Resource.Id.breedReadOnly);
            tvDescription = view.FindViewById<TextView>(Resource.Id.descriptionReadOnly);
            tvUploaderName = view.FindViewById<TextView>(Resource.Id.uploaderName);
            tvUploaderEmail = view.FindViewById<TextView>(Resource.Id.uploaderEmail);
            tvUploaderPhone = view.FindViewById<TextView>(Resource.Id.uploaderPhone);
            ivCallNumber = view.FindViewById<ImageView>(Resource.Id.callNumber);
            ivWriteEmail = view.FindViewById<ImageView>(Resource.Id.writeEmail);
            btnCallNumber = view.FindViewById<Button>(Resource.Id.btnCallPhone);
            btnWriteEmail = view.FindViewById<Button>(Resource.Id.btnWriteEmail);

            Bitmap bm = BitmapFactory.DecodeByteArray(mDog.Image, 0, mDog.Image.Length);
            photoPreview.SetImageBitmap(bm);
            tvBreed.Text = mDog.Breed;
            tvDescription.Text = mDog.Description;

            var users = GetUsersFromDb();

            var mobileUser = MobileUser.Instance().User;
            User dogUser = new User();
            foreach (var user in users)
            {
                if (user.Id.Equals(mDog.UserId))
                {
                    dogUser = user;
                    tvUploaderName.Text = user.Name;
                    tvUploaderEmail.Text = user.EmailAddress;
                    tvUploaderPhone.Text = user.PhoneNumber;
                }
            }
            btnCallNumber.Click += (s, e) => 
            {
                Intent phone = new Intent(Intent.ActionCall, Android.Net.Uri.Parse("tel:" + dogUser.PhoneNumber));
                StartActivity(phone);
            };

            ivCallNumber.Click += (s, e) =>
            {
                Intent phone = new Intent(Intent.ActionCall, Android.Net.Uri.Parse("tel:" + dogUser.PhoneNumber));
                StartActivity(phone);
            };

            btnWriteEmail.Click += (s, e) =>
            {                
                Intent i = new Intent(Intent.ActionSend);
                i.SetType("message/rfc822");
                i.PutExtra(Intent.ExtraEmail, new String[] { dogUser.EmailAddress });
                i.PutExtra(Intent.ExtraSubject, "subject of email");
                i.PutExtra(Intent.ExtraText, "body of email");
                try
                {
                    StartActivity(Intent.CreateChooser(i, "Send mail"));
                }
                catch (Android.Content.ActivityNotFoundException ex)
                {
                    Toast.MakeText(this.Activity, "There are no email applications installed.", ToastLength.Short).Show();
                }
            };

            ivWriteEmail.Click += (s, e) =>
            {
                Intent i = new Intent(Intent.ActionSend);
                i.SetType("message/rfc822");
                i.PutExtra(Intent.ExtraEmail, new String[] { dogUser.EmailAddress });
                i.PutExtra(Intent.ExtraSubject, "subject of email");
                i.PutExtra(Intent.ExtraText, "body of email");
                try
                {
                    StartActivity(Intent.CreateChooser(i, "Send mail"));
                }
                catch (Android.Content.ActivityNotFoundException ex)
                {
                    Toast.MakeText(this.Activity, "There are no email applications installed.", ToastLength.Short).Show();
                }
            };

            return view;
        }

        private List<User> GetUsersFromDb()
        {
            string responseFromServer = String.Empty;
            List<User> users = new List<User>();

            try
            {
                // Create a request for the URL. 		
                WebRequest request = WebRequest.Create(WebApiConnection.Instance().ConnectionString + @"user/id/" + mDog.UserId);
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
                        Id = u["id"].ToString(),
                        EmailAddress = u["emailAddress"].ToString(),
                        Name = u["name"].ToString(),
                        PhoneNumber = u["phoneNumber"].ToString(),
                    };

                    users.Add(user);
                }
            }
            catch (Exception)
            {
                Toast.MakeText(this.Activity, "Cannot connect to the server", ToastLength.Short).Show();
            }

            return users;
        }
    }
}