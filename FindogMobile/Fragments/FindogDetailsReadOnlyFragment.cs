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
    public class FindogDetailsReadOnlyFragment : Android.Support.V4.App.Fragment
    {
        const string ARG_PAGE = "ARG_PAGE";
        private int mPage;
        public ImageView photoPreview;
        public TextView tvBreed, tvDescription, tvUploaderName, tvUploaderPhone, tvUploaderEmail;
        static Animal mDog;

        public static FindogDetailsReadOnlyFragment NewInstance(int page, Animal dog)
        {
            mDog = dog;
            var args = new Bundle();

            args.PutInt(ARG_PAGE, page);
            var fragment = new FindogDetailsReadOnlyFragment();
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
            var view = inflater.Inflate(Resource.Layout.FindogDetailsReadOnlyPage, container, false);

            photoPreview = view.FindViewById<ImageView>(Resource.Id.photoPreview);
            tvBreed = view.FindViewById<TextView>(Resource.Id.breedReadOnly);
            tvDescription = view.FindViewById<TextView>(Resource.Id.descriptionReadOnly);
            tvUploaderName = view.FindViewById<TextView>(Resource.Id.uploaderName);
            tvUploaderEmail = view.FindViewById<TextView>(Resource.Id.uploaderEmail);
            tvUploaderPhone = view.FindViewById<TextView>(Resource.Id.uploaderPhone);

            Bitmap bm = BitmapFactory.DecodeByteArray(mDog.Image, 0, mDog.Image.Length);
            photoPreview.SetImageBitmap(bm);
            tvBreed.Text = mDog.Breed;
            tvDescription.Text = mDog.Description;

            return view;
        }
    }
}