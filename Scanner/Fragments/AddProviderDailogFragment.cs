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
using Scanner.Models;

namespace Scanner.Fragments
{
    public class AddProviderDailogFragment : DialogFragment
    {

        int width = 0;
        int height = 0;
        DisplayMetrics metrics;
        Card _card;

        EditText txtProvider;
        EditText txtPrefix;
        EditText txtSuffix;
        Button btnAdd;
        public event EventHandler<providerEventArgs> CardAdded;
        int position;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            metrics = Activity.Resources.DisplayMetrics;

            position = Arguments.GetInt("pos", 0);
           // position = 0;
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View dailogView =  inflater.Inflate(Resource.Layout.fragment_add_provider, container, false);

            btnAdd = dailogView.FindViewById<Button>(Resource.Id.buttonAdd);
            txtProvider = dailogView.FindViewById<EditText>(Resource.Id.editTextProvider); ;
            txtPrefix = dailogView.FindViewById<EditText>(Resource.Id.editTextPrefix); ;
            txtSuffix = dailogView.FindViewById<EditText>(Resource.Id.editTextSuffix); ;

            btnAdd.Click += BtnAdd_Click;

            return dailogView;// base.OnCreateView(inflater, container, savedInstanceState);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtProvider.Text))
                {
                    txtProvider.Error = Context.GetString(Resource.String.fragment_msg);
                    return;
                }
                _card = new Card();
                _card.ProiderName = txtProvider.Text;
                _card.Prefix = txtPrefix.Text;
                _card.Suffix = txtSuffix.Text;
                _card.IsSelected = true;
                _card.Position = position;

                providerEventArgs args = new providerEventArgs(_card);
                CardAdded?.Invoke(this, args);

                this.Dismiss();
            }
            catch (Exception ex)
            {
                Log.Error("GetUserSetting", ex.Message);
            }
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            // remove title
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            Dialog.Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);
            Dialog.SetCanceledOnTouchOutside(true);
            Dialog.SetCancelable(true);
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Android.Resource.Style.AnimationDialog;
        }
        /*
        public override void OnStart()
        {
            base.OnStart();
            try
            {
                // set screen size
                width = metrics.WidthPixels - (metrics.WidthPixels / 5);
                height = metrics.HeightPixels - (metrics.HeightPixels / 3);
                this.Dialog?.Window.SetLayout(width, height);

            }
            catch (Exception ex)
            {
                Log.Error("GetUserSetting", ex.Message);
            }

        }
        */
    }

    public class providerEventArgs : EventArgs
    {
        public Card AddedCard { get; set; }
        public providerEventArgs(Card added)
        {
            AddedCard = added;
        }
    }
}