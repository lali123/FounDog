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

namespace FindogMobile.Models
{
    public class CachedData
    {
        public List<Animal> FoundAnimals { get; set; }
        public List<Animal> WantedAnimals { get; set; }
        public List<User> Users { get; set; }

        #region Singleton

        private static CachedData _instance = new CachedData();

        //Constructor is marked as private   
        //so that the instance cannot be created   
        //from outside of the class  
        private CachedData()
        {
            FoundAnimals = new List<Animal>();
            WantedAnimals = new List<Animal>();
            Users = new List<User>();
        }

        //Static method which allows the instance creation  
        static internal CachedData Instance()
        {
            //All you need to do it is just return the  
            //already initialized which is thread safe  
            return _instance;
        }
        #endregion
    }
}