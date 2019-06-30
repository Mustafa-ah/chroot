using Android.Content;
using Android.Preferences;
using Scanner.Models;

namespace Scanner.Repository
{
    public class XmlDb
    {
        Context _Ctx;
        ISharedPreferences prefs;

        public XmlDb(Context ctx)
        {
            _Ctx = ctx;
            prefs = PreferenceManager.GetDefaultSharedPreferences(_Ctx);
        }

        public bool SaveUserSetting(string userSetting)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(userSetting))
                {
                    ISharedPreferencesEditor editor = prefs.Edit();
                    editor.PutString(Constants.UserSettingKey, userSetting);
                    editor.Apply();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public string getSavedUserSetting()
        {
            try
            {
                return prefs.GetString(Constants.UserSettingKey, null);
            }
            catch
            {
                return null;
            }
        }

        public bool SaveCardsList(string cardsList)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(cardsList))
                {
                    ISharedPreferencesEditor editor = prefs.Edit();
                    editor.PutString(Constants.CardsListKey, cardsList);
                    editor.Apply();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public string GetCardsList()
        {
            try
            {
                return prefs.GetString(Constants.CardsListKey, null);
            }
            catch
            {
                return null;
            }
        }

        public bool SaveChargsList(string chargsList)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(chargsList))
                {
                    ISharedPreferencesEditor editor = prefs.Edit();
                    editor.PutString(Constants.ChargsListKey, chargsList);
                    editor.Apply();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public string GetChargsList()
        {
            try
            {
                return prefs.GetString(Constants.ChargsListKey, null);
            }
            catch
            {
                return null;
            }
        }
    }
}