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
using Scanner.Repository;
using Android.Gms.Ads;
using Android.Support.V7.Widget;
using Scanner.Models;
using System.Threading.Tasks;
using Android.Util;
using Scanner.Adapters;

namespace Scanner.Activities
{
    [Activity(Label = "ChargsActivity")]
    public class ChargsActivity : Activity
    {
        XmlDb xmlDb;

        CardRepository cardrepo;

        RecyclerView recyclerView;
        RecyclerView.LayoutManager layoutManager;
        ChargedAdapter adapter;
        List<Card> _cards;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Activity_chargedCards);

            xmlDb = new XmlDb(this);
            cardrepo = new CardRepository();

            #region banner ads
            var adView = FindViewById<AdView>(Resource.Id.adView);
            var adRequest = new AdRequest.Builder().Build();
            adView.LoadAd(adRequest);
            #endregion

            recyclerView = FindViewById<RecyclerView>(Resource.Id.mvxRecyclerViewChargs);

             _cards = await GetSavedCardList();

            adapter = new ChargedAdapter(this, _cards, recyclerView);

            recyclerView.SetAdapter(adapter);

            layoutManager = new LinearLayoutManager(this, LinearLayoutManager.Vertical, false);
            recyclerView.SetLayoutManager(layoutManager);
        }


        private async Task<List<Card>> GetSavedCardList()
        {

            List<Card> lst = new List<Card>();
            try
            {
                string cardsJsone = xmlDb.GetChargsList();
                lst = await cardrepo.GetChargedList(cardsJsone);
            }
            catch (Exception ex)
            {
                Log.Error("GetUserSetting", ex.Message);
            }
            return lst;
        }

    }
}