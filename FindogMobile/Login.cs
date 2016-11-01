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
using Android.Preferences;
using Android.Text;
using BusinessLogic.Models;
using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using FindogMobile.Models;

namespace FindogMobile
{
    [Activity(Label = "Login", MainLauncher = true, Icon = "@drawable/Dog")]
    public class Login : Activity
    {
        EditText txtName;
        EditText txtPassword;
        EditText txtEmail;
        EditText txtPhoneNumber;
        Button btnRegister;
        Button btnLogin;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);
            // Create your application here

            if (UserAlreadyRegistered())
            {
                Intent intentMain = new Intent(this, typeof(MainActivity));
                StartActivity(intentMain);
            }
            else
            {
                txtName = FindViewById<EditText>(Resource.Id.input_name);
                txtPassword = FindViewById<EditText>(Resource.Id.input_password);
                txtEmail = FindViewById<EditText>(Resource.Id.input_email);
                txtPhoneNumber = FindViewById<EditText>(Resource.Id.input_phone);
                btnRegister = FindViewById<Button>(Resource.Id.btn_register);
                btnLogin = FindViewById<Button>(Resource.Id.btn_login);

                txtName.TextChanged += EditTextChanged;
                txtPassword.TextChanged += EditTextChanged;
                txtEmail.TextChanged += EditTextChanged;
                txtPhoneNumber.TextChanged += EditTextChanged;

                btnRegister.Click += RegisterUserClick;
                btnLogin.Click += LoginClick;
            }
        }

        private void EditTextChanged(object sender, TextChangedEventArgs e)
        {
            var editText = sender as EditText;
            if (editText != null)
            {
                if (string.IsNullOrEmpty(editText.Text))
                {
                    editText.Error = "This field is required";
                }
                else
                {
                    editText.Error = null;
                }
            }
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
                    };
                    users.Add(user);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return users;
        }

        private void RegisterUserClick(object sender, EventArgs e)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editor = prefs.Edit();

            if (!string.IsNullOrEmpty(txtName.Text) && !string.IsNullOrEmpty(txtEmail.Text) && !string.IsNullOrEmpty(txtPassword.Text) && !string.IsNullOrEmpty(txtPhoneNumber.Text))
            {
                string email = txtEmail.Text;
                editor.PutString("Name", txtName.Text);
                editor.PutString("Email", email);
                editor.PutString("Password", txtPassword.Text);
                editor.PutString("Phone", txtPhoneNumber.Text);

                User user = new User()
                {
                    Date = DateTime.Now,
                    EmailAddress = txtEmail.Text,
                    Name = txtName.Text,
                    PhoneNumber = txtEmail.Text,
                };

                editor.Apply();

                SaveUserToDb(user);
            }

            Toast.MakeText(this, "Registered", ToastLength.Short).Show();
            Intent intentMain = new Intent(this, typeof(MainActivity));
            StartActivity(intentMain);
        }

        private async void SaveUserToDb(User user)
        {
            try
            {
                var settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(WebApiConnection.Instance().ConnectionString);
                    //client.DefaultRequestHeaders.Accept.Clear();
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var json = JsonConvert.SerializeObject(user, settings);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    // HTTP POST
                    HttpResponseMessage response = await client.PostAsync("user/registeruser", content);
                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception)
            {
                Toast.MakeText(this, "Cannot connect to the server", ToastLength.Short).Show();
            }

        }

        private void LoginClick(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Login", ToastLength.Short).Show();
            Intent intentMain = new Intent(this, typeof(MainActivity));
            StartActivity(intentMain);
        }


    }
}