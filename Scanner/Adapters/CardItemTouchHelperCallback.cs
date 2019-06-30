using Android.Support.V7.Widget.Helper;
using Android.Support.V7.Widget;
using Android.Graphics;
using Android.Util;
using Android.Support.Design.Widget;
using Android.Content;
using Android.Widget;
using System;

using Android.Views;
using Scanner.Models;
using System.Collections.Generic;
using Android.App;
using Scanner.Activities;

namespace Scanner.Adapters
{
    class CardItemTouchHelperCallback : ItemTouchHelper.Callback
    {
        private readonly RecyclerView.Adapter _adapter;
        List<Card> _cards;
        RecyclerView _view;

        Card _deletedCard;
        HomeActivity _activity;

        public CardItemTouchHelperCallback(RecyclerView.Adapter adapter,
            RecyclerView view, HomeActivity activity, List<Card> cards)
        {
            _adapter = adapter;
            _view = view;
            _cards = cards;
            _activity = activity;
           // _activity.CardsChanged += _activity_CardsChanged;
        }

        //private void _activity_CardsChanged(object sender, EventArgs e)
        //{
        //    _cards = _activity._cards;
        //}

        public override int GetMovementFlags(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder)
        {
            int swipeFlags = 0;
            int dragFlags = 0;

            swipeFlags = ItemTouchHelper.Start | ItemTouchHelper.End;
            dragFlags = ItemTouchHelper.Up | ItemTouchHelper.Down;

            return MakeMovementFlags(dragFlags, swipeFlags);
        }

        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
        {
            _adapter?.NotifyItemMoved(viewHolder.AdapterPosition, target.AdapterPosition);
            return true;
        }

        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
        {
            try
            {
                int pos_ = viewHolder.AdapterPosition;

                _deletedCard = _cards[pos_];
                _cards.Remove(_deletedCard);
                _adapter?.NotifyItemRemoved(pos_);
                ShowSnackbar(pos_);
            }
            catch (Exception ex)
            {
                Log.Error("CartItemTouchHelperCallback ", ex.Message);
            }
        }

        private void ShowSnackbar(int position)
        {
            try
            {
                string undoMsg = _activity.GetString(Resource.String.undoDelet);
                string undo = _activity.GetString(Resource.String.undo);
                //https://blog.xamarin.com/add-beautiful-material-design-with-the-android-support-design-library/
                Snackbar snackbar = Snackbar.Make(_view, undoMsg, Snackbar.LengthLong)
                   .SetAction(undo, (v) =>
                   {
                       if (_deletedCard != null)
                       {
                           _cards.Insert(position, _deletedCard);
                           _adapter.NotifyItemInserted(position);
                       }
                   });

                snackbar.SetActionTextColor(Color.Yellow);

                // Button snackViewButton = dd.View.FindViewById<Button>(Resource.Id.snackbar_action);
                TextView snackViewText = snackbar.View.FindViewById<TextView>(Resource.Id.snackbar_text);
                //snackViewText.Gravity = GravityFlags.Center;
                snackViewText.SetTextColor(Color.White);
                snackbar.Show();
            }
            catch (Exception ex)
            {
                Log.Error("CartItemTouchHelperCallback ", ex.Message);
            }
        }
    }
}