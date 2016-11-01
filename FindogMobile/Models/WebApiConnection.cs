
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

namespace FindogMobile.Models
{
    class WebApiConnection
    {
        public string ConnectionString { get; set; }

        #region Singleton

        private static WebApiConnection _instance = new WebApiConnection();

        //Constructor is marked as private   
        //so that the instance cannot be created   
        //from outside of the class  
        private WebApiConnection()
        {
            //ConnectionString = @"http://192.168.1.102:8086/";
            //ConnectionString = @"http://192.168.1.64:8086/";
            ConnectionString = @"http://192.168.1.7:8086/";
        }

        //Static method which allows the instance creation  
        static internal WebApiConnection Instance()
        {
            //All you need to do it is just return the  
            //already initialized which is thread safe  
            return _instance;
        }
        #endregion
    }
}