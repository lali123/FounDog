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
using System.Threading;
using Android.Util;
using BusinessLogic.Models;

namespace FindogMobile.Services
{
    [Service]
    public class DogService : IntentService
    {
        IBinder binder;
        private List<Animal> founDogList = new List<Animal>();
        private List<Animal> wantedDogList = new List<Animal>();
        private List<User> userList = new List<User>();

        protected override void OnHandleIntent(Intent intent)
        {
            founDogList = UpdateFounDogList();
            wantedDogList = UpdateWantedDogList();
            userList = UpdateUserList();
        }

        private List<Animal> UpdateFounDogList()
        {
            return new List<Animal> { new Animal { Breed = "Recieved", Date = DateTime.Now, Description = "Recieved description", Latitude = 42, Longitude = 21 } };
        }

        private List<Animal> UpdateWantedDogList()
        {
            return new List<Animal> { new Animal { Breed = "Recieved", Date = DateTime.Now, Description = "Recieved description", Latitude = 42, Longitude = 21 } };
        }

        private List<User> UpdateUserList()
        {
            return new List<User> { new User { Date = DateTime.Now, EmailAddress = "asdf123@gmail.com", Name = "Reciever", Password = "test", PhoneNumber = "06208765432" } };
        }

        public override IBinder OnBind(Intent intent)
        {
            binder = new DogServiceBinder(this);
            return binder;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }

    public class DogServiceBinder : Binder
    {
        DogService service;

        public DogServiceBinder(DogService service)
        {
            this.service = service;
        }

        public DogService GetStockService()
        {
            return service;
        }
    }
}