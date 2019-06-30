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
using Scanner.Activities;

namespace Scanner.Adapters
{
    class InterstitailAdListener : Android.Gms.Ads.AdListener
    {
        HomeActivity home;

        public InterstitailAdListener(HomeActivity t)
        {
            home = t;
        }

        public override void OnAdClosed()
        {
            home.RequestNewInterstitial();
            home.ShowAddFragment();
        }
    }

}