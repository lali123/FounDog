using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.App;

namespace FindogMobile.Fragments
{
    public class MapFragment : Android.Support.V4.App.Fragment, IOnMapReadyCallback
    {
        const string ARG_PAGE = "ARG_PAGE";
        private int mPage;
        GoogleMap map;
        string markerId;

        public static MapFragment NewInstance(int page)
        {
            var args = new Bundle();
            args.PutInt(ARG_PAGE, page);
            var fragment = new MapFragment();
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
            var view = inflater.Inflate(Resource.Layout.MapFragmentPage, container, false);
            var _mapFragment = FragmentManager.FindFragmentByTag("map") as SupportMapFragment;

            if (_mapFragment == null)
            {
                GoogleMapOptions mapOptions = new GoogleMapOptions()
                    .InvokeMapType(GoogleMap.MapTypeHybrid)
                    .InvokeZoomControlsEnabled(true)
                    .InvokeCompassEnabled(true);

                Android.Support.V4.App.FragmentTransaction fragTx = FragmentManager.BeginTransaction();
                _mapFragment = SupportMapFragment.NewInstance(mapOptions);
                fragTx.Add(Resource.Id.map, _mapFragment, "map");
                fragTx.Commit();
                map = _mapFragment.Map;
                _mapFragment.GetMapAsync(this);
            }

            return view;
        }
        
        public void OnMapReady(GoogleMap googleMap)
        {
            LatLng location = new LatLng(47.5316049, 21.6273123);
            MarkerOptions markerOptions = new MarkerOptions();
            markerOptions.SetPosition(new LatLng(location.Latitude, location.Longitude));
            markerOptions.SetTitle("Found dog");
            var marker = googleMap.AddMarker(markerOptions);
            markerId = marker.Id;
            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(location);
            builder.Zoom(16);
            CameraPosition cameraPosition = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

            googleMap.AnimateCamera(cameraUpdate);
            googleMap.MyLocationEnabled = true;
            map = googleMap;
            map.MapClick += MapClick;
        }

        private void MapClick(object sender, GoogleMap.MapClickEventArgs e)
        {
            (sender as GoogleMap).Clear();
            MarkerOptions markerOptions = new MarkerOptions();
            markerOptions.SetPosition(new LatLng(e.Point.Latitude, e.Point.Longitude));
            markerOptions.SetTitle("Found dog");
            map.AddMarker(markerOptions);
            if (Activity is FindDog)
            {
                (Activity as FindDog).Longitude = e.Point.Longitude;
                (Activity as FindDog).Latitude = e.Point.Latitude;
            }
        }
    }
}