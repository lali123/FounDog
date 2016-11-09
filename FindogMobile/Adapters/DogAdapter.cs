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
using BusinessLogic.Models;

namespace FindogMobile.Adapters
{
    public class DogAdapter : BaseAdapter<Animal>
    {
        List<Animal> items;
        List<Animal> temp;
        Activity context;
        public DogAdapter(Activity context, List<Animal> items)
       : base()
        {
            temp = new List<Animal>();
            temp.AddRange(items);
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override Animal this[int position]
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
                view = context.LayoutInflater.Inflate(Resource.Layout.DogListItemRow, null);

            Bitmap bm = BitmapFactory.DecodeByteArray(item.Image, 0, item.Image.Length);
            view.FindViewById<TextView>(Resource.Id.Text1).Text = item.Breed;
            view.FindViewById<TextView>(Resource.Id.Text2).Text = item.Date.ToString();
            view.FindViewById<ImageView>(Resource.Id.Image).SetImageBitmap(bm);
            return view;
        }

        public void Filter(String charText)
        {
            charText = charText.ToLower();

            items.Clear();
            if (charText.Length == 0)
            {
                items.AddRange(temp);
            }
            else
            {
                foreach (Animal animal  in temp)
                {
                    if (charText.Length != 0 && animal.Breed.ToLower().Contains(charText))
                    {
                        items.Add(animal);
                    }
                    else if (charText.Length != 0 && animal.Description.ToLower().Contains(charText))
                    {
                        items.Add(animal);
                    }
                }
            }
            NotifyDataSetChanged();
        }
    }
}