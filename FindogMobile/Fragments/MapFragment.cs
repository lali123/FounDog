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

                _mapFragment.GetMapAsync(this);
            }
            //fragment.GetMapAsync(this);

            return view;
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            MarkerOptions markerOptions = new MarkerOptions();
            markerOptions.SetPosition(new LatLng(47, 46));
            markerOptions.SetTitle("Found dog");
            googleMap.AddMarker(markerOptions);
        }
    }
}