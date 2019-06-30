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
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Scanner.Adapters;
using Scanner.Models;
using Scanner.Repository;
using System.Threading.Tasks;
using Android.Util;
using Android.Support.V7.Widget.Helper;
using Scanner.Fragments;
using Java.Util;
using Android.Gms.Ads;
using Android.Views.InputMethods;
using Android.Content.PM;
using Scanner.Utility;

namespace Scanner.Activities
{
    [Activity(Label = "", Theme = "@style/Theme.DesignDemo")]
    public class HomeActivity : AppCompatActivity//, RecyclerView.IOnItemTouchListener
    {
        DrawerLayout drawerLayout;
        NavigationView navigationView;

        V7Toolbar toolbar;

        ImageView imagePlus;

        ProgressBar loading;

        RecyclerView recyclerView;
        RecyclerView.LayoutManager layoutManager;
        CardAdapter adapter;
        List<Card> _cards;
        Card _selectedCard;
        UserSettings _userSetting;
        XmlDb xmlDb;

        CardRepository cardrepo;

        protected InterstitialAd mInterstitialAd;
        AdView adView;

        //public event EventHandler CardsChanged;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Activity_Home);

            xmlDb = new XmlDb(this);
            cardrepo = new CardRepository();

            Button bt = FindViewById<Button>(Resource.Id.buttonCharge);

            //https://www.c-sharpcorner.com/article/xamarin-android-create-android-navigation-drawer-layout-using-support-design/
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            // Create ActionBarDrawerToggle button and add it to the toolbar  
            toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.drawer_open, Resource.String.drawer_close);
           // drawerLayout.SetDrawerListener(drawerToggle);
            drawerLayout.AddDrawerListener(drawerToggle);
            drawerToggle.SyncState();
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            setupDrawerContent(navigationView); //Calling Function  

            imagePlus = toolbar.FindViewById<ImageView>(Resource.Id.imagePlus);

            loading = FindViewById<ProgressBar>(Resource.Id.progressBarhome);


            recyclerView = FindViewById<RecyclerView>(Resource.Id.mvxRecyclerViewProducts);

            // recyclerView.AddOnItemTouchListener(this);


            #region  ads
            adView = FindViewById<AdView>(Resource.Id.adView);
             var adRequest = new AdRequest.Builder().Build();
            //var adRequestTst = new AdRequest.Builder().AddTestDevice("4E811BB7106DD61A6306B379A856A2B1");
            adView.LoadAd(adRequest);

            mInterstitialAd = new InterstitialAd(this);
            mInterstitialAd.AdUnitId = GetString(Resource.String.interstital_ad_unit_id_home);

            mInterstitialAd.AdListener = new InterstitailAdListener(this);

           //AdRequest.Builder().
            #endregion



            bt.Click += Bt_Click;
            imagePlus.Click += ImagePlus_Click;
        }

        public void RequestNewInterstitial()
        {
            var adRequest = new AdRequest.Builder().Build();
            mInterstitialAd.LoadAd(adRequest);
        }

        protected override async void OnResume()
        {
            base.OnResume();
            try
            {
                var local = Resources.Configuration.Locale;

                loading.Visibility = ViewStates.Visible;
                _userSetting = await GetUser();

                if (_userSetting.IsFirstLaunch)
                {
                    _cards = cardrepo.GetDefaultCardList(local.Country);
                    AllowSettings.StartPowerSaverIntent(this);
                }
                else
                {
                    _cards = await GetSavedCardList();
                    if (!mInterstitialAd.IsLoaded)
                    {
                        RequestNewInterstitial();
                    }
                }

               
                if (local.Language == "ar")
                {
                    _cards = TranslateCardListToArabic(_cards);
                }
                SetListAdapter();
               // CardsChanged?.Invoke(this, new EventArgs());
                loading.Visibility = ViewStates.Gone;

                #region ads Service
                bool isRunning = false;
                var manager = (ActivityManager)GetSystemService(ActivityService);

                var services = manager.GetRunningServices(int.MaxValue).Select(
                    service => service.Service.ClassName).ToList();
                foreach (var srv in services)
                {
                    if (srv.Contains("InterstitialAdService"))
                    {
                        isRunning = true;
                    }
                }
                if (!isRunning)
                {
                    Intent intentServie = new Intent(this, typeof(InterstitialAdService));
                    StartService(intentServie);
                }
                #endregion
                
            }
            catch (Exception ex)
            {
                loading.Visibility = ViewStates.Visible;
                Log.Error("GetUserSetting", ex.Message);
            }
        }

        private void SetListAdapter()
        {
            
           
            adapter = new CardAdapter(this, _cards, recyclerView);

            recyclerView.SetAdapter(adapter);

            layoutManager = new LinearLayoutManager(this, LinearLayoutManager.Vertical, false);
            recyclerView.SetLayoutManager(layoutManager);

            ItemTouchHelper.Callback callback = new CardItemTouchHelperCallback(
             adapter, recyclerView, this, _cards);

            ItemTouchHelper itemTouchHelper = new ItemTouchHelper(callback);
            itemTouchHelper.AttachToRecyclerView(recyclerView);
            //adapter.ItemClicked -= Adapter_ItemClicked;
            //adapter.ItemClicked += Adapter_ItemClicked;
        }

        private void Adapter_ItemClicked(object sender, EventArgs e)
        {
            _selectedCard = sender as Card;
        }

        protected override void OnPause()
        {
            base.OnPause();
            if (adView != null)
            {
                adView.Pause();
            }
            SaveUserSetting();
            SaveCards();
        }
        protected override void OnDestroy()
        {
            if (adView != null)
            {
                adView.Destroy();
            }
            base.OnDestroy();
        }
        private void ImagePlus_Click(object sender, EventArgs e)
        {
            try
            {
                if (mInterstitialAd.IsLoaded)
                {
                    mInterstitialAd.Show();
                }
                else
                {
                    ShowAddFragment();
                }
                   
            }
            catch (Exception ex)
            {
                Log.Error("GetUserSetting", ex.Message);
            }
        }

        private void AddDailog_CardAdded(object sender, providerEventArgs e)
        {
            _cards = _cards.Select(c => { c.IsSelected = false; return c; }).ToList();
            _cards.Add(e.AddedCard);
            //SetListAdapter();
            // CardsChanged?.Invoke(this, new EventArgs());
            
            HideKeyboard();
           // SaveCards();
            Finish();
            StartActivity(new Intent(this, typeof(HomeActivity)));
           
        }

        private void Bt_Click(object sender, EventArgs e)
        {
            _selectedCard = _cards.Find(c => c.IsSelected);
            string msg = GetString(Resource.String.home_chose);
            string ok = GetString(Resource.String.home_ok);
            if (_selectedCard == null)
            {
                var builder = new Android.Support.V7.App.AlertDialog.Builder(this);
                builder.SetIconAttribute
                    (Android.Resource.Attribute.AlertDialogIcon);
                builder.SetTitle("");
                builder.SetMessage(msg);
                builder.SetPositiveButton(ok, delegate { });
                builder.Create().Show();
            }
            else
            {
                StartActivity(new Intent(this, typeof(CameraActivity)));
            }
            
        }

        void setupDrawerContent(NavigationView navigationView)
        {
            navigationView.NavigationItemSelected += (sender, e) =>
            {
                e.MenuItem.SetChecked(true);
                drawerLayout.CloseDrawers();
                switch (e.MenuItem.ItemId)
                {
                    case Resource.Id.nav_chargs:
                        StartActivity(new Intent(this, typeof(ChargsActivity)));
                        break;
                    case Resource.Id.nav_setting:
                        StartActivity(new Intent(this, typeof(SettingActivity)));
                        break;
                    case Resource.Id.nav_serv:
                        StartActivity(new Intent(this, typeof(ServicesActivity)));
                        break;
                    case Resource.Id.nav_share:
                        ShareAppView();
                        break;
                    default:
                        break;
                }
            };
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            navigationView.InflateMenu(Resource.Menu.nav_menu); //Navigation Drawer Layout Menu Creation  
            return true;
        }


        private async Task<UserSettings> GetUser()
        {
            UserSettings user = new UserSettings();
            try
            {

                string userJson = xmlDb.getSavedUserSetting();
                user = await cardrepo.GetUserSetting(userJson);
            }
            catch (Exception ex)
            {
                Log.Error("GetUserSetting", ex.Message);
            }
            return user;
        }


        private List<Card> TranslateCardListToArabic(List<Card> lst)
        {
            Dictionary<string, string> langdic = new Dictionary<string, string>()
            {
                {"Etisalat","اتصالات"},
                {"Vodafone","فودافون"},
                {"Orange","اورانج"},
                {"We","وي"},
                {"STC","الاتصالات السعودية"},
                {"Zain","زين"}
            };
            foreach (Card card in lst)
            {
                if (langdic.ContainsKey(card.ProiderName))
                {
                    card.ProiderName = langdic[card.ProiderName];
                }
            }
            return lst;
        }
        private async Task<List<Card>> GetSavedCardList()
        {
            //one time execution
            List<Card> lst = new List<Card>();
            try
            {
                string cardsJsone = xmlDb.GetCardsList();
                lst = await cardrepo.GetCardList(cardsJsone);
            }
            catch (Exception ex)
            {
                Log.Error("GetUserSetting", ex.Message);
            }
            return lst;
        }

        public async void SaveCards()
        {
            string cardJson = await cardrepo.SerializeObject(_cards);
            xmlDb.SaveCardsList(cardJson);
        }

        public async void SaveUserSetting()
        {
            _userSetting.IsFirstLaunch = false;
            _selectedCard = _cards.Find(c => c.IsSelected);
            _userSetting.UserCard = _selectedCard;
            string userJson = await cardrepo.SerializeObject(_userSetting);
            xmlDb.SaveUserSetting(userJson);
        }

        private Intent ShareAppView()
        {
            using (var shareIntent = new Intent(Intent.ActionSend))
            {
                shareIntent.SetType("text/plain");
                shareIntent.PutExtra(Intent.ExtraSubject, "@string/ApplicationName");
                shareIntent.PutExtra(Intent.ExtraText, "https://play.google.com/store/apps/details?id=com.codina.chroot");
                StartActivity(Intent.CreateChooser(shareIntent, "Share via"));
                return shareIntent;
            }
        }

        internal void ShowAddFragment()
        {
            try
            {
                AddProviderDailogFragment addDailog = new AddProviderDailogFragment();
                Bundle dialogBundel = new Bundle();
                int max = _cards.Max(c => c.Position);
                dialogBundel.PutInt("pos", max + 1);
                addDailog.Arguments = dialogBundel;
                addDailog.CardAdded += AddDailog_CardAdded;
                FragmentTransaction ft = FragmentManager.BeginTransaction();
                ft.Add(addDailog, "addProvider");
                ft.CommitAllowingStateLoss();
                // addDailog.Show(FragmentManager, "addProvider");
            }
            catch (Exception ex)
            {
                Log.Error("GetUserSetting", ex.Message);
            }
           
        }
        protected override void OnSaveInstanceState(Bundle outState)
        {
            // base.OnSaveInstanceState(outState);
        }
        public void HideKeyboard()
        {
            try
            {
                InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
                inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
            }
            catch
            {

            }
        }

        // allow background service in some devices
        private void AllowAutoStart()
        {
            try
            {
                Intent intent = new Intent();
                if (PackageManager.ResolveActivity(intent, PackageInfoFlags.MatchDefaultOnly) != null)
                {
                    StartActivity(intent);
                }
            }
            catch
            {

            }
        }
    }
}