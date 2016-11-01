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
using Android.Locations;
using System.Threading.Tasks;
using Android.Provider;

namespace FindogMobile.Fragments
{
    public class GpsFragment : Fragment, ILocationListener
    {
        Button setGpsButton, addPointOnMapButton;
        TextView gpsTextView, addressTextView;
        LocationManager locationManager;
        Location currentLocation;
        string locationProvider;
        static readonly string TAG = "X:" + typeof(FindDog).Name;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            InitializeLocationManager();
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.GetLocation, container, false);
            setGpsButton = view.FindViewById<Button>(Resource.Id.btnGPS);
            addPointOnMapButton = view.FindViewById<Button>(Resource.Id.btnPointOnMap);
            gpsTextView = view.FindViewById<TextView>(Resource.Id.tvGps);
            addressTextView = view.FindViewById<TextView>(Resource.Id.tvAddress);

            locationManager = (LocationManager)this.Activity.GetSystemService(Context.LocationService);
            
            setGpsButton.Click += (s, e) =>
            {
                if (locationManager.IsProviderEnabled(LocationManager.GpsProvider) == false)
                {
                    Intent gpsSettingIntent = new Intent(Settings.ActionLocationSourceSettings);
                    this.StartActivity(gpsSettingIntent);
                }

                if (currentLocation == null)
                {
                    addressTextView.Text = "Can't determine the current address. Try again in a few minutes.";
                    return;
                }

            };

            InitializeLocationManager();
            return view;
        }

        #region Set GPS

        private void InitializeLocationManager()
        {
            locationManager = (LocationManager)this.Activity.GetSystemService(Context.LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
            IList<string> acceptableLocationProviders = locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                locationProvider = string.Empty;
            }
            Log.Debug(TAG, "Using " + locationProvider + ".");
        }

        public override void OnResume()
        {
            base.OnResume();
            if (locationManager.IsProviderEnabled(LocationManager.GpsProvider))
            {
                if (locationProvider != null)
                {
                    locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);
                }
            }
        }

        public override void OnPause()
        {
            base.OnPause();
            locationManager.RemoveUpdates(this);
        }

        public void OnLocationChanged(Location location)
        {
            currentLocation = location;
            if (currentLocation == null)
            {
                gpsTextView.Text = "Unable to determine your location. Try again in a short while.";
            }
            else
            {
                (this.Activity as FindDog).Latitude = currentLocation.Latitude;
                (this.Activity as FindDog).Longitude = currentLocation.Longitude;

                gpsTextView.Text = string.Format("{0:f6},{1:f6}", currentLocation.Latitude, currentLocation.Longitude);
            }
        }

        public void OnProviderDisabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}