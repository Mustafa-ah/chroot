using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Support.V7.App;
using Android.Content.PM;

namespace Scanner
{
    [Activity(Label = "@string/ApplicationName", 
        MainLauncher = true, Theme = "@style/SplashTheme",
        NoHistory =true,
        Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
             SetContentView (Resource.Layout.Activity_Splash);

            string id = "ca-app-pub-7735761310721955~5871423062";
            Android.Gms.Ads.MobileAds.Initialize(ApplicationContext, id);

            //bool isXiaomi =  string.Equals(Build.Manufacturer, "xiaomi", System.StringComparison.OrdinalIgnoreCase);
            //if (isXiaomi)
            //{
            //    Intent intent = new Intent();
            //    intent.SetComponent(new ComponentName("com.miui.securitycenter", "com.miui.permcenter.autostart.AutoStartManagementActivity"));
                
            //    if (PackageManager.ResolveActivity(intent, PackageInfoFlags.MatchDefaultOnly) != null)
            //    {
            //        StartActivity(intent);
            //    }
            //}

            System.Threading.Thread.Sleep(300);


            StartActivity(new Intent(this, typeof(Activities.HomeActivity)));

            Finish();
        }

    }
}

