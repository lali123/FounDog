using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using BusinessLogic.Models;
using FindogMobile.Helpers;
using System;

namespace FindogMobile.Fragments
{
    public class FindogDetailsUpdateFragment :Android.Support.V4.App.Fragment
    {
        const string ARG_PAGE = "ARG_PAGE";
        private int mPage;

        public EditText descriptionEditView, breedEditView;
        public TextView latitudeTextView, longitudeTextView;
        public ImageView dogImageView;
        static Animal mDog;

        public static FindogDetailsUpdateFragment NewInstance(int page, Animal dog)
        {
            mDog = dog;
            var args = new Bundle();
            args.PutInt(ARG_PAGE, page);
            var fragment = new FindogDetailsUpdateFragment();
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
            View view = inflater.Inflate(Resource.Layout.FindogDetailsUpdate, container, false);
            descriptionEditView = view.FindViewById<EditText>(Resource.Id.etDescriptionUpdate);
            breedEditView = view.FindViewById<EditText>(Resource.Id.etBreedUpdate);
            dogImageView = view.FindViewById<ImageView>(Resource.Id.imageViewDogPictureUpdate);
            latitudeTextView = view.FindViewById<TextView>(Resource.Id.latitudeReadOnlyUpdate);
            longitudeTextView = view.FindViewById<TextView>(Resource.Id.longitudeReadOnlyUpdate);
            latitudeTextView.Text = mDog.Latitude.ToString();
            longitudeTextView.Text = mDog.Longitude.ToString();
            descriptionEditView.Text = mDog.Description;
            breedEditView.Text = mDog.Breed;
            Bitmap bm = BitmapFactory.DecodeByteArray(mDog.Image, 0, mDog.Image.Length);
            dogImageView.SetImageBitmap(bm);

            descriptionEditView.TextChanged += (s, e) =>
            {
                if (Activity is UpdateFindog)
                {
                    (Activity as UpdateFindog).Description = descriptionEditView.Text;
                }
            };

            breedEditView.TextChanged += (s, e) =>
            {
                if (Activity is UpdateFindog)
                {
                    (Activity as UpdateFindog).Breed = breedEditView.Text;
                }
            };
            return view;
        }

        public void RefreshImage()
        {
            try
            {
                int height = Resources.DisplayMetrics.HeightPixels;
                int width = dogImageView.Height;
                App.bitmap = App._file.Path.LoadAndResizeBitmap(width, height);
                if (App.bitmap != null)
                {
                    dogImageView.SetImageBitmap(App.bitmap);
                    App.bitmap = null;
                }
            }
            catch (Exception)
            {

            }
        }
    }
}