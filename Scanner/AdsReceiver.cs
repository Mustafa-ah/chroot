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
using Android.Gms.Ads;

namespace Scanner
{
    [BroadcastReceiver]
    [IntentFilter(new string[] { "com.chroot.ads" })]
    public class AdsReceiver : BroadcastReceiver
    {

        protected InterstitialAd mInterstitialAd;

      //  public bool AddClosed { get; set; }

        public override async void OnReceive(Context context, Intent intent)
        {
            try
            {
              //  Toast.MakeText(context, "Received intent!", ToastLength.Long).Show();
                mInterstitialAd = new InterstitialAd(context);
                mInterstitialAd.AdUnitId = context.GetString(Resource.String.interstital_ad_unit_id_home);
                RequestNewInterstitial();
                await System.Threading.Tasks.Task.Delay(200000); // half hour
               // Toast.MakeText(context, "Ads Loded!", ToastLength.Short).Show();
                if (!mInterstitialAd.IsLoaded)
                {
                    RequestNewInterstitial();
                }
                else
                {
                    mInterstitialAd.Show();
                }
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("AdsReceiver", ex.Message);
            }
        }

        public void RequestNewInterstitial()
        {
            var adRequest = new AdRequest.Builder().Build();
          //  mInterstitialAd.AdListener = new RInterstitailAdListener(this);
            //var ddd = new AdRequest.Builder().AddTestDevice("4E811BB7106DD61A6306B379A856A2B1");
            //var adRequest = ddd.Build();
            mInterstitialAd.LoadAd(adRequest);
        }

    }
    /*
    class RInterstitailAdListener : AdListener
    {
        AdsReceiver receiver;

        public RInterstitailAdListener(AdsReceiver re)
        {
            receiver = re;
        }

        public override void OnAdClosed()
        {
            receiver.AddClosed = true;
        }

        public override void OnAdLoaded()
        {
            base.OnAdLoaded();
            receiver.AddClosed = false;
        }
    }
    */
}