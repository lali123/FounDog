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
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using FindogMobile.Adapters;
using BusinessLogic.Models;

namespace FindogMobile
{
    [Activity(Label = "Profile")]
    public class Profile : Activity
    {
        ListView uploadList;
        private List<DogMenu> menuList;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Profile);
            // Create your application here
            menuList = GetMenuList();
            
            var adapter = new MenuAdapter(this, menuList);
            uploadList = FindViewById<ListView>(Resource.Id.MyUploads);
            uploadList.Adapter = adapter;
            uploadList.ItemClick += (s, e) =>
            {
                var listView = s as ListView;
                var menu = menuList[e.Position];

                switch (menu.Title)
                {
                    case "Found dogs by me":
                        Intent intentFind = new Intent(this, typeof(MyFoundDog));                        
                        StartActivity(intentFind);
                        break;
                    case "Lost dogs by me":
                        Intent intentLost = new Intent(this, typeof(MyLostDog));
                        StartActivity(intentLost);
                        break;
                    case "Profile settings":
                        Intent intentUpdateProfile = new Intent(this, typeof(UpdateProfile));
                        StartActivity(intentUpdateProfile);
                        break;
                    default:
                        break;
                }
            };
        }

        private List<DogMenu> GetMenuList()
        {
            return new List<DogMenu>
            {
                new DogMenu { IsGroupHeader = false, Title=MobileUser.Instance().User.Name, Icon=Resource.Drawable.User},
                new DogMenu { IsGroupHeader = false, Title=MobileUser.Instance().User.PhoneNumber, Icon=Resource.Drawable.Phone},
                new DogMenu { IsGroupHeader = false, Title=MobileUser.Instance().User.EmailAddress, Icon=Resource.Drawable.Email},
                new DogMenu { IsGroupHeader = false, Title="Found dogs by me", Icon=Resource.Drawable.DogHouse},
                new DogMenu { IsGroupHeader = false, Title="Lost dogs by me", Icon=Resource.Drawable.DogLeash},
                new DogMenu { IsGroupHeader = false, Title="Profile settings", Icon=Resource.Drawable.Settings},
            };
        }

        private void ShowAllertIfItNeed(List<Animal> result)
        {
            foreach (var dog in result)
            {
                var time = (DateTime.Now - dog.Date);
                if (time.Days > 30)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("One month");
                    alert.SetMessage(dog.Breed+" has already lost for one month. Do you want to extend the search or delete.");
                    alert.SetPositiveButton("Extend", (senderAlert, args) => {
                        Toast.MakeText(this, "Extend!", ToastLength.Short).Show();
                    });

                    alert.SetNegativeButton("Delete", (senderAlert, args) => {
                        Toast.MakeText(this, "Delete!", ToastLength.Short).Show();
                    });

                    Dialog dialog = alert.Create();
                    dialog.Show();
                }
            }
        }
    }
}