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
using System.Net;
using System.IO;
using FindogMobile.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using Android.Preferences;

namespace FindogMobile
{
    [Activity(Label = "UpdateProfile")]
    public class UpdateProfile : Activity
    {
        EditText etName, etPhone, etEmail, etPassword;
        Button btnUpdate, btnDelete;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.UpdateProfile);

            etName = FindViewById<EditText>(Resource.Id.etNameUpdate);
            etPhone = FindViewById<EditText>(Resource.Id.etPhoneNumberUpdate);
            etEmail = FindViewById<EditText>(Resource.Id.etEmailUpdate);
            etPassword = FindViewById<EditText>(Resource.Id.etPasswodUpdate);
            btnUpdate = FindViewById<Button>(Resource.Id.btnUpdateProfile);
            btnDelete = FindViewById<Button>(Resource.Id.btnDeleteProfile);

            etName.Text = MobileUser.Instance().User.Name;
            etPhone.Text = MobileUser.Instance().User.PhoneNumber;
            etEmail.Text = MobileUser.Instance().User.EmailAddress;
            etPassword.Text = MobileUser.Instance().User.Password;

            btnUpdate.Click += (s, e) =>
            {
                UpdateProfileSettings();
            };


            btnDelete.Click += (s, e) =>
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Delete");
                alert.SetMessage("Are you sure you want to delete your profile");
                alert.SetPositiveButton("Delete", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Delete!", ToastLength.Short).Show();
                    //DeleteProfileSettings();
                    if (DeleteProfileSettings())
                    {
                        Restart();
                    }
                });

                alert.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Cancel!", ToastLength.Short).Show();
                });

                Dialog dialog = alert.Create();
                dialog.Show();
            };
        }

        private void Restart()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.Remove("Id");
            editor.Remove("Name");
            editor.Remove("Email");
            editor.Remove("Phone");
            editor.Remove("Password");
            editor.Apply();
            App.IsUserAlreadyRegistered = false;

            BaseContext.PackageManager.GetLaunchIntentForPackage(BaseContext.PackageName);
            Intent i = new Intent(this, typeof(SplashScreen));
            i.AddFlags(ActivityFlags.ClearTop);
            i.AddFlags(ActivityFlags.ClearTask);
            i.AddFlags(ActivityFlags.NewTask);
            this.StartActivity(i);
        }

        private bool DeleteProfileSettings()
        {
            try
            {
                string responseFromServer = String.Empty;
                // Create a request for the URL. 		


                WebRequest request = WebRequest.Create(WebApiConnection.Instance().ConnectionString + @"user/deleteuser/" + MobileUser.Instance().User.Id);
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

                return true;
            }
            catch (Exception)
            {
                Toast.MakeText(this, "Cannot connect to the server", ToastLength.Short).Show();
                return false;
            }
        }

        private async void UpdateProfileSettings()
        {
            MobileUser.Instance().User.Name = etName.Text;
            MobileUser.Instance().User.PhoneNumber = etPhone.Text;
            MobileUser.Instance().User.EmailAddress = etEmail.Text;
            MobileUser.Instance().User.Password = etPassword.Text;
            try
            {

                var settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(WebApiConnection.Instance().ConnectionString);

                    var json = JsonConvert.SerializeObject(MobileUser.Instance().User, settings);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    // HTTP POST

                    HttpResponseMessage response = await client.PostAsync("user/updateuser/" + MobileUser.Instance().User.Id, content);
                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();
                    }
                }

                ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
                ISharedPreferencesEditor editor = prefs.Edit();

                editor.PutString("Id", MobileUser.Instance().User.Id);
                editor.PutString("Name", MobileUser.Instance().User.Name);
                editor.PutString("Email", MobileUser.Instance().User.EmailAddress);
                editor.PutString("Phone", MobileUser.Instance().User.PhoneNumber);
                editor.PutString("Password", MobileUser.Instance().User.Password);
            }
            catch (Exception)
            {
                Toast.MakeText(this, "Cannot connect to the server", ToastLength.Short).Show();
            }
        }
    }
}