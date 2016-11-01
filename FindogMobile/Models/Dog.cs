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
using Android.Graphics;

namespace FindogMobile.Models
{
    public class Dog
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int Latitude { get; set; }
        public int Longitude { get; set; }
        public string Breed { get; set; }
        public byte[] Image { get; set; }
    }
}