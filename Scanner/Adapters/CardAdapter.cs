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
using Android.Support.V7.Widget;
using Scanner.Models;
using Scanner.Adapters;
using Scanner.Activities;

namespace Scanner.Adapters
{
    class CardAdapter : RecyclerView.Adapter
    {
        List<Card> _cards;
        Activity activity;
        RecyclerView _target;

       // public event EventHandler ItemClicked;

        public CardAdapter(Activity activity, List<Card> cards, RecyclerView target)
        {
            this._cards = cards;
            this.activity = activity;
            this._target = target;
        }

        public override int ItemCount
        {
            get
            {
                return _cards.Count;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            try
            {
                var item = _cards[position];

                // Replace the contents of the view with that element
                var viewHolder = holder as CardAdapterViewHolder;
                viewHolder.Caption.Text = item.ProiderName.ToString();
                if (item.IsSelected)
                {
                    viewHolder.Image.SetImageResource(Resource.Drawable.selected);
                }
                else
                {
                    viewHolder.Image.SetImageResource(Resource.Drawable.unSelected);
                }

                //Picasso.With(activity).Load(item.ImageLink).Into(holder.Image);
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("OnBindViewHolder", ex.Message);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //Setup and inflate your layout here
            var id = Resource.Layout.list_item_card2;
            var itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);
            itemView.Click += ItemView_Click;
            return new CardAdapterViewHolder(itemView);
        }

        private void ItemView_Click(object sender, EventArgs e)
        {
            try
            {
                View view = sender as View;
                int pos = _target.GetChildAdapterPosition(view);
                if (_cards[pos].IsSelected)
                {
                    Intent camIntent = new Intent(activity, typeof(CameraActivity));
                    camIntent.SetFlags(ActivityFlags.NewTask);
                    activity.StartActivity(camIntent);
                    return;
                }
                
                _cards = _cards.Select(c => { c.IsSelected = false; return c; }).ToList();
                _cards[pos].IsSelected = true;
                this.NotifyDataSetChanged();
               // ItemClicked?.Invoke(_cards[pos], new EventArgs());
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("ItemView_Click", ex.Message);
            }
        }
    }
}