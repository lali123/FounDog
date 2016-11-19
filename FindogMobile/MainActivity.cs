using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Threading.Tasks;
using System.Net;
using System.Xml;
using Android.Media;
using Android.Content;
using Java.IO;
using Android.Graphics;
using FindogMobile.Adapters;
using FindogMobile.Models;
using System.Collections.Generic;
using BusinessLogic.Models;
using Newtonsoft.Json.Linq;
using System.IO;
using Android.Net;
using System.Linq;

namespace FindogMobile
{
    [Activity(Label = "FindogMobile")]
    public class MainActivity : Activity
    {
        ListView mainMenuListView;

        private List<DogMenu> mainMenuList;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            mainMenuList = GetMenuList();
            mainMenuListView = FindViewById<ListView>(Resource.Id.MainMenuList);
            mainMenuListView.Adapter = new MenuAdapter(this, mainMenuList);
            mainMenuListView.ItemClick += (s, e) =>
              {
                  var listView = s as ListView;
                  var menu = mainMenuList[e.Position];

                  switch (menu.Title)
                  {
                      case "Found a dog":
                          Intent intentFind = new Intent(this, typeof(FindDog));
                          App.Tag = "found";
                          StartActivity(intentFind);
                          break;
                      case "Lost my dog":
                          Intent intentLost = new Intent(this, typeof(FindDog));
                          App.Tag = "lost";
                          StartActivity(intentLost);
                          break;
                      case "Found dogs":
                          Intent intentFound = new Intent(this, typeof(FoundDog));
                          StartActivity(intentFound);
                          break;
                      case "Wanted dogs":
                          Intent intentWanted = new Intent(this, typeof(WantedDogs));
                          StartActivity(intentWanted);
                          break;
                      case "Dogs on map":
                          Intent intentMap = new Intent(this, typeof(DogsOnMap));
                          StartActivity(intentMap);
                          break;
                      case "Log off":
                          LogOff();
                          break;
                      case "Exit":
                          Finish(); FinishAffinity();
                          System.Environment.Exit(0);
                          break;
                      default:
                          Intent intentProfile = new Intent(this, typeof(Profile));
                          StartActivity(intentProfile);
                          break;
                  }
              };

            CheckNotification();
        }

        private void LogOff()
        {
            App.IsUserAlreadyRegistered = false;

            ConnectivityManager cm = (ConnectivityManager)this.GetSystemService(Context.ConnectivityService);
            NetworkInfo info = cm.ActiveNetworkInfo;
            if (info != null && info.IsConnected)
            {
                ISharedPreferences prefs = Android.Preferences.PreferenceManager.GetDefaultSharedPreferences(this);
                ISharedPreferencesEditor editor = prefs.Edit();
                editor.Remove("Id");
                editor.Remove("Name");
                editor.Remove("Email");
                editor.Remove("Phone");
                editor.Remove("Password");
                editor.Apply();
            }

            BaseContext.PackageManager.GetLaunchIntentForPackage(BaseContext.PackageName);
            Intent i = new Intent(this, typeof(SplashScreen));
            i.AddFlags(ActivityFlags.ClearTop);
            i.AddFlags(ActivityFlags.ClearTask);
            i.AddFlags(ActivityFlags.NewTask);
            this.StartActivity(i);
        }

        private List<DogMenu> GetMenuList()
        {
            return new List<DogMenu>
            {
                new DogMenu { IsGroupHeader = false, Title=MobileUser.Instance().User.Name, Icon = Resource.Drawable.User},
                new DogMenu { IsGroupHeader = false, Title="Found a dog", Icon=Resource.Drawable.Dog},
                new DogMenu { IsGroupHeader = false, Title="Lost my dog", Icon=Resource.Drawable.Search},
                new DogMenu { IsGroupHeader = false, Title="Found dogs", Icon=Resource.Drawable.DogHouse},
                new DogMenu { IsGroupHeader = false, Title="Wanted dogs", Icon=Resource.Drawable.DogLeash},
                new DogMenu { IsGroupHeader = false, Title="Dogs on map", Icon=Resource.Drawable.MapMarker},
                new DogMenu { IsGroupHeader = false, Title="Log off", Icon=Resource.Drawable.denied},
                new DogMenu { IsGroupHeader = false, Title="Exit", Icon=Resource.Drawable.Cancel},
            };
        }

        private void CheckNotification()
        {
            var wantedDogs = CachedData.Instance().WantedAnimals.Where(d => d.UserId == MobileUser.Instance().User.Id).ToList();
            foreach (var animal in wantedDogs)
            {
                if ((DateTime.Now - animal.Date).Days > 30)
                {
                    ShowNotification(animal);
                }
            }
        }

        private void ShowNotification(Animal animal)
        {
            App.Tag = "lost";
            Intent resultIntent = new Intent(this, typeof(UpdateFindog));
            Bundle bundle = new Bundle();
            bundle.PutString("AnimalId", animal.AnimalIdToString());
            bundle.PutString("userId", animal.UserId.ToString());
            bundle.PutString("breed", animal.Breed);
            bundle.PutDouble("latitude", animal.Latitude);
            bundle.PutString("description", animal.Description);
            bundle.PutByteArray("image", animal.Image);
            bundle.PutDouble("longitude", animal.Longitude);
            bundle.PutDouble("latitude", animal.Latitude);
            resultIntent.PutExtra("bundle", bundle);

            TaskStackBuilder stackBuilder = TaskStackBuilder.Create(this);
            stackBuilder.AddNextIntent(resultIntent);

            // Create the PendingIntent with the back stack:            
            PendingIntent resultPendingIntent = stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);

            Android.Support.V4.App.NotificationCompat.Builder builder = new Android.Support.V4.App.NotificationCompat.Builder(this)
            .SetAutoCancel(true)                        // Dismiss from the notif. area when clicked
            .SetContentIntent(resultPendingIntent)    // Start 2nd activity when the intent is clicked.
            .SetContentTitle("Dog out of date")               // Set its title
            .SetSmallIcon(Resource.Drawable.WhiteDog2)        // Display this icon
            .SetLargeIcon(BitmapFactory.DecodeResource(Resources, Resource.Drawable.Dog))
            .SetContentText(String.Format(
            "{0} should extend.", animal.Breed));

            NotificationManager notificationManager =
            (NotificationManager)GetSystemService(Activity.NotificationService);
            notificationManager.Notify(1000, builder.Build());
        }

        private List<Animal> GetAnimalsAsync()
        {
            List<Animal> dogs = new List<Animal>();
            try
            {
                string responseFromServer = String.Empty;
                // Create a request for the URL. 		
                WebRequest request = WebRequest.Create(WebApiConnection.Instance().ConnectionString + @"animal/wantedanimals/" + MobileUser.Instance().User.Id);
                // If required by the server, set the credentials.
                request.Credentials = CredentialCache.DefaultCredentials;
                // Get the response.
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    // Get the stream containing content returned by the server.
                    using (System.IO.Stream dataStream = response.GetResponseStream())
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
    }

    public static class App
    {
        public static Java.IO.File _file;
        public static Java.IO.File _dir;
        public static Bitmap bitmap;
        public static string Tag;
        public static bool IsUserAlreadyRegistered;
    }
}

