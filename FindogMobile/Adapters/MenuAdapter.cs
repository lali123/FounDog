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
using FindogMobile.Models;
using Android.Graphics;

namespace FindogMobile.Adapters
{
    public class MenuAdapter : BaseAdapter<DogMenu>
    {
        List<DogMenu> items;
        Activity context;
        public MenuAdapter(Activity context, List<DogMenu> items)
       : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override DogMenu this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            
            if (view == null) // no view to re-use, create new
            {
                if (item.IsGroupHeader)
                {
                    view = context.LayoutInflater.Inflate(Resource.Layout.HeaderItem, null);
                    view.FindViewById<TextView>(Resource.Id.tvHeader).Text = item.Title;
                    Bitmap bmp = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.User);
                    if (bmp != null)
                    {
                        view.FindViewById<ImageView>(Resource.Id.headerIcon).SetImageBitmap(bmp);
                    }
                }
                else
                {
                    view = context.LayoutInflater.Inflate(Resource.Layout.MenuListItemRow, null);
                    view.FindViewById<ImageView>(Resource.Id.ivIcon).SetImageResource(item.Icon);
                    view.FindViewById<TextView>(Resource.Id.tvTitle).Text = item.Title;
                }
            }
            //view.FindViewById<ImageView>(Resource.Id.Image).SetImageBitmap(item.Image);
            return view;
        }
    }
}