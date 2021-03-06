using Android.OS;
using Android.Views;
using Android.Widget;
using FindogMobile.Helpers;
using System;

namespace FindogMobile.Fragments
{
    public class FindogDetailsFragment : Android.Support.V4.App.Fragment
    {
        const string ARG_PAGE = "ARG_PAGE";
        private int mPage;

        public EditText descriptionEditView, breedEditView;
        public TextView latitudeTextView, longitudeTextView;
        public ImageView dogImageView;

        public static FindogDetailsFragment NewInstance(int page)
        {
            var args = new Bundle();
            args.PutInt(ARG_PAGE, page);
            var fragment = new FindogDetailsFragment();
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
            View view = inflater.Inflate(Resource.Layout.FindogDetailsFragmet, container, false);
            descriptionEditView = view.FindViewById<EditText>(Resource.Id.etDescription);
            breedEditView = view.FindViewById<EditText>(Resource.Id.etBreed);
            dogImageView = view.FindViewById<ImageView>(Resource.Id.imageViewDogPicture);
            latitudeTextView = view.FindViewById<TextView>(Resource.Id.latitudeReadOnly);
            longitudeTextView = view.FindViewById<TextView>(Resource.Id.longitudeReadOnly);
            latitudeTextView.Text = 47.5316049.ToString();
            longitudeTextView.Text = 21.6273123.ToString();



            descriptionEditView.TextChanged += (s, e) =>
            {
                if (Activity is FindDog)
                {
                    (Activity as FindDog).Description = descriptionEditView.Text;
                }
            };

            breedEditView.TextChanged += (s, e) =>
            {
                if (Activity is FindDog)
                {
                    (Activity as FindDog).Breed = breedEditView.Text;
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