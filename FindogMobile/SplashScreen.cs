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
using System.Threading.Tasks;
using BusinessLogic.Models;
using Android.Preferences;
using FindogMobile.Models;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace FindogMobile
{
    [Activity(Label = "FinDog", MainLauncher = true, Icon = "@drawable/Dog", Theme = "@style/Theme.AppCompat.Light.NoActionBar.Splash", NoHistory = true)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }

        protected override void OnResume()
        {
            base.OnResume();

            Task startupWork = new Task(() =>
            {
                App.IsUserAlreadyRegistered = UserAlreadyRegistered();
            });

            startupWork.ContinueWith(t =>
            {
                StartActivity(new Intent(Application.Context, typeof(Login)));
            }, TaskScheduler.FromCurrentSynchronizationContext());

            startupWork.Start();
        }


        private bool UserAlreadyRegistered()
        {
            try
            {
                ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);

                User user = new User()
                {
                    // Date = prefs.GetString("Date", DateTime.Now.ToString()),
                    EmailAddress = prefs.GetString("Email", ""),
                    Name = prefs.GetString("Name", ""),
                    PhoneNumber = prefs.GetString("Phone", ""),
                    Id = prefs.GetString("Id", ""),
                    Password = prefs.GetString("Password", "")
                };
                MobileUser.Instance().User = user;

                List<User> users = GetUsersFromDb();
                foreach (var u in users)
                {
                    if (u.Name.Equals(user.Name) && u.PhoneNumber.Equals(user.PhoneNumber))
                    {
                        return true;
                    }
                }

            }
            catch (Exception)
            {
                Toast.MakeText(this, "Cannot connect to the server", ToastLength.Short).Show();

                return false;
            }


            return false;
        }


        private List<User> GetUsersFromDb()
        {
            string responseFromServer = String.Empty;
            List<User> users = new List<User>();

            try
            {
                // Create a request for the URL. 		
                WebRequest request = WebRequest.Create(WebApiConnection.Instance().ConnectionString + @"user/users");
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
                        EmailAddress = u["emailAddress"].ToString(),
                        Name = u["name"].ToString(),
                        PhoneNumber = u["phoneNumber"].ToString(),
                        Id = u["id"].ToString(),
                    };

                    users.Add(user);
                }
            }
            catch (Exception)
            {
                Toast.MakeText(this, "Cannot connect to the server", ToastLength.Short).Show();
            }

            return users;
        }

    }
}