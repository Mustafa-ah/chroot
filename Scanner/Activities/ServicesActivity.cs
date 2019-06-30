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
using Scanner.Models;
using Scanner.Adapters;
using Scanner.Repository;
using Android.Util;
using System.Threading.Tasks;

namespace Scanner.Activities
{
    [Activity(Label = "ServicesActivity")]
    public class ServicesActivity : Activity
    {
        ExpandableListView servicesListView;
        List<Card> serviceis;
        ServicesExpanableAdapter servicesadapter;

        UserSettings _userSetting;
        XmlDb xmlDb;
        CardRepository cardrepo;

        private bool _isBusy;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Activity_services);

            xmlDb = new XmlDb(this);
            cardrepo = new CardRepository();

            servicesListView = FindViewById<ExpandableListView>(Resource.Id.expandableListViewSer);

            var local = Resources.Configuration.Locale;
            serviceis = cardrepo.GetDefaultCardServiceList(local.Country);
            servicesadapter = new ServicesExpanableAdapter(this, serviceis);
            servicesListView.SetAdapter(servicesadapter);

            servicesListView.ChildClick += ServicesListView_ChildClick;
        }

        private void ServicesListView_ChildClick(object sender, ExpandableListView.ChildClickEventArgs e)
        {
            try
            {
                Card clickedCard = serviceis[e.GroupPosition].Services[e.ChildPosition];
                MakeDial(clickedCard);
            }
            catch (Exception ex)
            {
                Log.Error("GetUserSetting", ex.Message);
            }
        }

        protected async override void OnResume()
        {
            base.OnResume();
            try
            {
                string userJson = xmlDb.getSavedUserSetting();
                _userSetting = await cardrepo.GetUserSetting(userJson);
                //var local = Resources.Configuration.Locale;
                //serviceis = cardrepo.GetDefaultCardServiceList(local.Country);
                //servicesadapter = new ServicesExpanableAdapter(this, serviceis);
                //servicesListView.SetAdapter(servicesadapter);
            }
            catch (Exception ex)
            {
                Log.Error("GetUserSetting", ex.Message);
            }
        }

        //private async Task<List<Card>> GetSavedCardList()
        //{
        //    //one time execution
        //    List<Card> lst = new List<Card>();
        //    try
        //    {
        //        string cardsJsone = xmlDb.GetCardsList();
        //        lst = await cardrepo.GetCardList(cardsJsone);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error("GetUserSetting", ex.Message);
        //    }
        //    return lst;
        //}


        public void MakeDial(Card card)
        {
            try
            {
                if (_isBusy)
                {
                    return;
                }
                _isBusy = true;

                Intent call;
                //I found a solution for this issue by replacing # in 
                string preix = card.Prefix.Replace("#", "%23");
                string suffix = card.Suffix.Replace("#", "%23");

                string formatedNumber = $"tel:{preix}{suffix}";
                Android.Net.Uri num = Android.Net.Uri.Parse(formatedNumber);

                if (_userSetting.ShowFirst)
                {
                    call = new Intent(Intent.ActionDial, num);
                }
                else
                {
                    call = new Intent(Intent.ActionCall, num);
                }

                StartActivity(call);
                _isBusy = false;
            }
            catch (Exception es)
            {
                _isBusy = false;
                Log.Info("Release", "txtRecognizer.SetProcessor " + es.Message);
            }
        }
    }
}