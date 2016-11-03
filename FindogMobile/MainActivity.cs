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
                          App.UploadAnimal = "found";
                          StartActivity(intentFind);
                          break;
                      case "Lost my dog":
                          Intent intentLost = new Intent(this, typeof(FindDog));
                          App.UploadAnimal = "lost";
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
                      case "Exit":
                          Finish();
                          break;
                      default:
                          Intent intentProfile = new Intent(this, typeof(Profile));
                          StartActivity(intentProfile);
                          break;
                  }
              };
        }

        private List<DogMenu> GetMenuList()
        {
            return new List<DogMenu>
            {
                new DogMenu { IsGroupHeader = false, Title=MobileUser.Instance().User.Name, Icon = Resource.Drawable.User},
                new DogMenu { IsGroupHeader = false, Title="Found a dog", Icon=Resource.Drawable.Dog},
                new DogMenu { IsGroupHeader = false, Title="Lost my dog", Icon=Resource.Drawable.Location},
                new DogMenu { IsGroupHeader = false, Title="Found dogs", Icon=Resource.Drawable.Search},
                new DogMenu { IsGroupHeader = false, Title="Wanted dogs", Icon=Resource.Drawable.denied},
                new DogMenu { IsGroupHeader = false, Title="Exit", Icon=Resource.Drawable.Cancel},
            };
        }
    }

    public static class App
    {
        public static File _file;
        public static File _dir;
        public static Bitmap bitmap;
        public static string UploadAnimal;
    }
}

