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
    public class MobileUser
    {
        public User User { get; set; }

        #region Singleton

        private static MobileUser _instance = new MobileUser();

        //Constructor is marked as private   
        //so that the instance cannot be created   
        //from outside of the class  
        private MobileUser()
        {
            User = new User();
        }

        //Static method which allows the instance creation  
        static internal MobileUser Instance()
        {
            //All you need to do it is just return the  
            //already initialized which is thread safe  
            return _instance;
        }
        #endregion
    }
}