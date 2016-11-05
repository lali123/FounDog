using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using BusinessLogic.Models;
using Android.Graphics;

namespace FindogMobile.Fragments
{
    public class PhotoFragment : Android.Support.V4.App.Fragment
    {
        const string ARG_PAGE = "ARG_PAGE";
        private int mPage;
        public ImageView photo;
        static Animal mdog;

        public static PhotoFragment NewInstance(int page, Animal dog)
        {
            var args = new Bundle();
            args.PutInt(ARG_PAGE, page);
            mdog = dog;
            var fragment = new PhotoFragment();
            fragment.Arguments = args;

            return fragment;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mPage = Arguments.GetInt(ARG_PAGE);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            var view = inflater.Inflate(Resource.Layout.PhotoPageReadOnly, container, false);
            Bitmap bm = BitmapFactory.DecodeByteArray(mdog.Image, 0, mdog.Image.Length);
            photo = view.FindViewById<ImageView>(Resource.Id.ivPhotoReadOnly);
            photo.SetImageBitmap(bm);
            return view;
        }
    }
}