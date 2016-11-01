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
    public class DogMenu
    {
        public int Icon { get; internal set; }
        public string Title { get; set; }
        public bool IsGroupHeader { get; set; }
    }
}