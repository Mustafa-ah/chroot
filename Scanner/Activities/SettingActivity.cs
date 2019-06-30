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
using Scanner.Repository;
using Android.Util;
using Android.Gms.Ads;

namespace Scanner.Activities
{
    [Activity(Label = "SettingActivity")]
    public class SettingActivity : Activity
    {
        RadioGroup dailGrou;
        CheckBox redBoxCheck;
        UserSettings _userSetting;
        XmlDb xmlDb;

        CardRepository cardrepo;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_setting);

            xmlDb = new XmlDb(this);
            cardrepo = new CardRepository();

            #region banner ads
            var adView = FindViewById<AdView>(Resource.Id.adView);
            var adRequest = new AdRequest.Builder().Build();
            adView.LoadAd(adRequest);
            #endregion

            dailGrou = FindViewById<RadioGroup>(Resource.Id.radioGroupDial);
            redBoxCheck = FindViewById<CheckBox>(Resource.Id.checkBoxRed);

            dailGrou.CheckedChange += DailGrou_CheckedChange;
            redBoxCheck.CheckedChange += RedBoxCheck_CheckedChange;
        }

        private async void RedBoxCheck_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            try
            {
                _userSetting.ShowRedBox = e.IsChecked;
                string userJson = await cardrepo.SerializeObject(_userSetting);
                xmlDb.SaveUserSetting(userJson);
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
                if (_userSetting.ShowFirst)
                {
                    dailGrou.Check(Resource.Id.radioButtonShow);
                }
                else
                {
                    dailGrou.Check(Resource.Id.radioButtondirect);
                }
                redBoxCheck.Checked = _userSetting.ShowRedBox;
            }
            catch (Exception ex)
            {
                Log.Error("GetUserSetting", ex.Message);
            }
        }

        private async void DailGrou_CheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            try
            {
                switch (e.CheckedId)
                {
                    case Resource.Id.radioButtonShow:
                        _userSetting.ShowFirst = true;
                        break;
                    case Resource.Id.radioButtondirect:
                        _userSetting.ShowFirst = false;
                        break;
                    default:
                        break;
                }

                string userJson = await cardrepo.SerializeObject(_userSetting);
                xmlDb.SaveUserSetting(userJson);
            }
            catch (Exception ex)
            {
                Log.Error("GetUserSetting", ex.Message);
            }
        }
    }
}