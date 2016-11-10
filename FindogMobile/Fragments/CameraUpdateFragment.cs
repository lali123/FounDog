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
using Android.Provider;
using Android.Content.PM;
using FindogMobile.Helpers;
using Java.IO;
using BusinessLogic.Models;
using Android.Graphics;
using System.IO;

namespace FindogMobile.Fragments
{
    public class CameraUpdateFragment : Android.Support.V4.App.Fragment
    {

        const string ARG_PAGE = "ARG_PAGE";
        private int mPage;
        public ImageView takePictureImageView;
        static Animal mDog;

        public static CameraUpdateFragment NewInstance(int page, Animal dog)
        {
            mDog = dog;
            var args = new Bundle();
            args.PutInt(ARG_PAGE, page);
            var fragment = new CameraUpdateFragment();
            fragment.Arguments = args;

            return fragment;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mPage = Arguments.GetInt(ARG_PAGE);
            Activity.Window.SetSoftInputMode(SoftInput.StateHidden);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            var view = inflater.Inflate(Resource.Layout.CameraUpdatePage, container, false);

            Activity.Window.SetSoftInputMode(SoftInput.StateHidden);
            takePictureImageView = view.FindViewById<ImageView>(Resource.Id.imgTakePictureUpdate);
            Bitmap bm = BitmapFactory.DecodeByteArray(mDog.Image, 0, mDog.Image.Length);
            takePictureImageView.SetImageBitmap(bm);

            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();

                takePictureImageView.Click += (s, e) =>
                {
                    Intent intent = new Intent(MediaStore.ActionImageCapture);
                    App._file = new Java.IO.File(App._dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
                    intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(App._file));
                    StartActivityForResult(intent, 0);
                };
            }

            return view;
        }

        #region Take a picture with camera

        private void CreateDirectoryForPictures()
        {
            App._dir = new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "Findog");
            if (!App._dir.Exists())
            {
                App._dir.Mkdirs();
            }
        }

        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities = Application.Context.PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            Android.Net.Uri contentUri = Android.Net.Uri.FromFile(App._file);
            mediaScanIntent.SetData(contentUri);
            Activity.SendBroadcast(mediaScanIntent);

            // Display in ImageView. We will resize the bitmap to fit the display.
            // Loading the full sized image will consume to much memory
            // and cause the application to crash.

            int height = Resources.DisplayMetrics.HeightPixels;
            int width = takePictureImageView.Height;
            App.bitmap = App._file.Path.LoadAndResizeBitmap(width, height);
            if (App.bitmap != null)
            {
                takePictureImageView.SetImageBitmap(App.bitmap);
                var bmp = App.bitmap;
                using (var stream = new MemoryStream())
                {
                    bmp.Compress(Bitmap.CompressFormat.Png, 0, stream);

                    (Activity as UpdateFindog).Image = stream.ToArray();
                }

                App.bitmap = null;

            }

            // Dispose of the Java side bitmap.
            GC.Collect();
        }


        #endregion
    }
}