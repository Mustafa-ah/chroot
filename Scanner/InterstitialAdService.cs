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
using System.Threading.Tasks;
using Android.Gms.Ads;

namespace Scanner
{
    [Service]
    public class InterstitialAdService : IntentService
    {
        // bool isRunning;
       // protected InterstitialAd mInterstitialAd;

        //public override void OnCreate()
        //{
        //    base.OnCreate();
        //    //mInterstitialAd = new InterstitialAd(ApplicationContext);
        //    //mInterstitialAd.AdUnitId = ApplicationContext.GetString(Resource.String.interstital_ad_unit_id_home);

        //}
        protected override void OnHandleIntent(Intent intent)
        {
            //throw new NotImplementedException();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {

            new Task(() =>
            {
                while (true)
                {
                    //try
                    //{
                    //   // RequestNewInterstitial();
                    //    System.Threading.Thread.Sleep(50000);

                    //    if (!mInterstitialAd.IsLoaded)
                    //    {
                    //        RequestNewInterstitial();
                    //    }
                    //    else
                    //    {
                    //        mInterstitialAd.Show();
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    Android.Util.Log.Error("AdsReceiver", ex.Message);
                    //}

                    System.Threading.Thread.Sleep(14000000);// 8 hours => 28800000
                    Intent newIntent = new Intent();
                    newIntent.SetAction("com.chroot.ads");
                    // newIntent.PutExtra("AssignedOrders", OrdersJson);
                    SendBroadcast(newIntent);
                }
            }).Start();

            return StartCommandResult.Sticky;
        }

        //public void RequestNewInterstitial()
        //{
        //    var adRequest = new AdRequest.Builder().Build();
        //    mInterstitialAd.LoadAd(adRequest);
        //}
    }
}