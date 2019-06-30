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
using Scanner.ViewHolder;

namespace Scanner.Adapters
{
    public class ChargedAdapter : RecyclerView.Adapter
    {
        List<Card> _cards;
        Activity activity;
        RecyclerView _target;

        public ChargedAdapter(Activity activity, List<Card> cards, RecyclerView target)
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
            //CargedAdapterViewHolder
            try
            {
                var item = _cards[position];

                // Replace the contents of the view with that element
                var viewHolder = holder as CargedAdapterViewHolder;
                viewHolder.Date.Text = item.ChargingDate.ToShortDateString();
                viewHolder.Number.Text = item.CardNumber;
                viewHolder.Provider.Text = item.ProiderName;
                
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("OnBindViewHolder", ex.Message);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var id = Resource.Layout.list_item_chargs;
            var itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);
           // itemView.Click += ItemView_Click;
            return new CargedAdapterViewHolder(itemView);
        }
    }
}