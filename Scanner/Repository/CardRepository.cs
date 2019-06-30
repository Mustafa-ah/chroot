using System;
using System.Collections.Generic;
using Scanner.Models;
using Android.Util;
using System.Threading.Tasks;

namespace Scanner.Repository
{
    public class CardRepository : BaseRepository
    {
        public async Task<UserSettings> GetUserSetting(string userJson)
        {
            UserSettings set = new UserSettings();
            try
            {
                if (userJson == null)
                {
                    set.IsFirstLaunch = true;
                    set.ShowFirst = false;
                    return set;
                }
                set = await DeserializeTObject<UserSettings>(userJson);

            }
            catch (Exception ex)
            {
                Log.Error("GetUserSetting", ex.Message);
            }
            return set;
        }

        public async Task<List<Card>> GetCardList(string cardJson)
        {
            List<Card> cards = new List<Card>();
            try
            {
                if (cardJson != null)
                {
                    cards = await DeserializeTObject<List<Card>>(cardJson);
                }
                
            }
            catch (Exception ex)
            {
                Log.Error("GetUserSetting", ex.Message);
            }
            return cards;
        }

        public async Task<List<Card>> GetChargedList(string cargJson)
        {
            List<Card> cards = new List<Card>();
            try
            {
                if (cargJson != null)
                {
                    cards = await DeserializeTObject<List<Card>>(cargJson);
                }

            }
            catch (Exception ex)
            {
                Log.Error("GetUserSetting", ex.Message);
            }
            return cards;
        }

        public List<Card> GetDefaultCardList(string country)
        {
            //one time execution
            List<Card> lst = new List<Card>();

            Card etisalat = new Card();
            etisalat.Position = 0;
            etisalat.ProiderName = "Etisalat";
            etisalat.Prefix = "*556*";
            etisalat.Suffix = "#";
            
            lst.Add(etisalat);

            Card vodafone = new Card();
            vodafone.Position = 1;
            vodafone.ProiderName = "Vodafone";
            vodafone.Prefix = "*858*";
            vodafone.Suffix = "#";

            lst.Add(vodafone);

            Card orange = new Card();
            orange.Position = 2;
            orange.ProiderName = "Orange";
            orange.Prefix = "#102*";
            orange.Suffix = "#";
           
            lst.Add(orange);

            Card we = new Card();
            we.Position = 3;
            we.ProiderName = "We";
            we.Prefix = "*555*";
            we.Suffix = "#";
           
            lst.Add(we);

            Card stc = new Card();
            stc.Position = 4;
            stc.ProiderName = "STC";
            stc.Prefix = "*155*";
            stc.Suffix = "#";

            Card zain = new Card();
            zain.Position = 5;
            zain.ProiderName = "Zain";
            zain.Prefix = "*141*";
            zain.Suffix = "#";


            if (country == "SA")
            {
                lst.Insert(0, stc);
                lst.Insert(1, zain);
            }
            else
            {
                lst.Add(stc);
                lst.Add(zain);
            }

            return lst;
        }

        public List<Card> GetDefaultCardServiceList(string country)
        {
            //one time execution
            List<Card> lst = new List<Card>();

            Card etisalat = new Card();
            etisalat.Position = 0;
            etisalat.ProiderName = "Etisalat";
            etisalat.Prefix = "*556*";
            etisalat.Suffix = "#";

            List<Card> etisalatSrvices = new List<Card>();
            #region etisalat Services

            Card etisalatServ1 = new Card();
            etisalatServ1.Position = 0;
            etisalatServ1.Description = "للاستعلام عن الرصيد";
            etisalatServ1.Prefix = "*555";
            etisalatServ1.Suffix = "#";
            etisalatSrvices.Add(etisalatServ1);

            Card etisalatServ2 = new Card();
            etisalatServ2.Position = 0;
            etisalatServ2.Description = "لمعرفة استهلاك دقايق اتصالات";
            etisalatServ2.Prefix = "*558*";
            etisalatServ2.Suffix = "1#";
            etisalatSrvices.Add(etisalatServ2);

            Card etisalatServ3 = new Card();
            etisalatServ3.Position = 0;
            etisalatServ3.Description = "الغاء خدمة الكول تون";
            etisalatServ3.Prefix = "*15*";
            etisalatServ3.Suffix = "5#";
            etisalatSrvices.Add(etisalatServ3);

            etisalatSrvices.Add(GenarateCard(0, 0, "تجديد باقة المكالمات", "*807", "#"));
            etisalatSrvices.Add(GenarateCard(0, 0, "الغاء باقة النت", "*566*", "1357#"));
            etisalatSrvices.Add(GenarateCard(0, 0, "لمعرفة ميعاد تجديد باقة النت ", "*566*", "1341#"));
            etisalatSrvices.Add(GenarateCard(0, 0, "لمعرفة ميعاد تجديد باقة المكالمات ", "*852", "#"));
            etisalatSrvices.Add(GenarateCard(0, 0, "لمعرفة استهلاك باقة النت", "*558*", "5#"));
            etisalatSrvices.Add(GenarateCard(0, 0, " الاستعلام عن رقم الخط", "*947", "#"));
            etisalatSrvices.Add(GenarateCard(0, 0, "  سلفنى  شكرا	", "*911", "#"));


            #endregion
            etisalat.Services = etisalatSrvices;
            lst.Add(etisalat);

            Card vodafone = new Card();
            vodafone.Position = 1;
            vodafone.ProiderName = "Vodafone";
            vodafone.Prefix = "*858*";
            vodafone.Suffix = "#";

            List<Card> vodafoneSrvices = new List<Card>();
            #region vodafone Services

            vodafoneSrvices.Add(GenarateCard(0, 0, "معرفة استهلاك الفليكسات والفليكسات المبتقية", "*60", "#"));
            vodafoneSrvices.Add(GenarateCard(0, 0, "الغاء باقة النت  ", "*2000*", "0#"));
            vodafoneSrvices.Add(GenarateCard(0, 0, "الاستعلام عن موعد تجديد باقة فودافون ", "*225", "#"));
            vodafoneSrvices.Add(GenarateCard(0, 0, " الاستعلام عن رقم الخط", "*878", "#"));
            vodafoneSrvices.Add(GenarateCard(0, 0, "  لاستعلام عن رصيد فودافون	", "*868*", "1#"));
            vodafoneSrvices.Add(GenarateCard(0, 0, "  الغاء الكول تون ", "*055*", "000000#"));
            vodafoneSrvices.Add(GenarateCard(0, 0, "  رقم خدمة عملاء فودافون", "888", ""));

            #endregion
            vodafone.Services = vodafoneSrvices;
            lst.Add(vodafone);

            Card orange = new Card();
            orange.Position = 2;
            orange.ProiderName = "Orange";
            orange.Prefix = "#102*";
            orange.Suffix = "#";

            List<Card> orangeSrvices = new List<Card>();
            #region orange Services

            orangeSrvices.Add(GenarateCard(0, 0, "الغاء باقة النت  ", "*100*", "414611#"));
            orangeSrvices.Add(GenarateCard(0, 0, "الاستعلام عن موعد تجديد باقة النت ", "#100*", "413#"));
            orangeSrvices.Add(GenarateCard(0, 0, " الاستعلام عن رقم الخط", "#119", "#"));
            orangeSrvices.Add(GenarateCard(0, 0, "  لاستعلام عن رصيد اورانج	", "#100#", "3#"));
            orangeSrvices.Add(GenarateCard(0, 0, "  الغاء الكول تون ", "9999", ""));
            orangeSrvices.Add(GenarateCard(0, 0, "  رقم خدمة عملاء اورانج", "110", ""));


            #endregion
            orange.Services = orangeSrvices;
            lst.Add(orange);

            Card we = new Card();
            we.Position = 3;
            we.ProiderName = "We";
            we.Prefix = "*555*";
            we.Suffix = "#";

            List<Card> weSrvices = new List<Card>();
            #region we Services

            weSrvices.Add(GenarateCard(0, 0, "لمعرفة رصيدك", "*322", "#"));
            weSrvices.Add(GenarateCard(0, 0, "معرفة رقم الهاتف", "*688", "#"));
            weSrvices.Add(GenarateCard(0, 0, "خدمة سلفني شكراً", "*04", "#"));
            weSrvices.Add(GenarateCard(0, 0, " معلومات و خدمات الدليل في مصر", "140", ""));
            weSrvices.Add(GenarateCard(0, 0, " استعلام عن الاستخدام	", "*414", "#"));

            #endregion
            we.Services = weSrvices;
            lst.Add(we);

            Card stc = new Card();
            stc.Position = 4;
            stc.ProiderName = "STC";
            stc.Prefix = "*155*";
            stc.Suffix = "#";
            List<Card> stcSrvices = new List<Card>();
            #region STC Services

            stcSrvices.Add(GenarateCard(0, 0, "خدمة العملاء", "900", ""));

            #endregion
            stc.Services = stcSrvices;

            Card zain = new Card();
            zain.Position = 5;
            zain.ProiderName = "Zain";
            zain.Prefix = "*141*";
            zain.Suffix = "#";
            List<Card> zaincSrvices = new List<Card>();
            #region zain Services

            zaincSrvices.Add(GenarateCard(0, 0, "لمعرفة رصيدك", "*142", "#"));
            zaincSrvices.Add(GenarateCard(0, 0, "اخر ثلاث مكالمات", "*143", "#"));
            zaincSrvices.Add(GenarateCard(0, 0, "خدمة البريد الصوتى", "*1700", ""));
            zaincSrvices.Add(GenarateCard(0, 0, "لتفعيل خدمة انتظار المكالمات", "*43", "#"));
            zaincSrvices.Add(GenarateCard(0, 0, "الغاء خدمة انتظار المكالمات", "#43", "#"));
            zaincSrvices.Add(GenarateCard(0, 0, "تفعيل نغمات المتصل أو فيديو المتصل مثل ( رنان ) و ( صدى )", "*1718", ""));

            #endregion
            zain.Services = zaincSrvices;


            if (country == "SA")
            {
                lst.Insert(0, stc);
                lst.Insert(1, zain);
            }
            else
            {
                lst.Add(stc);
                lst.Add(zain);
            }

            return lst;
        }


        private Card GenarateCard(int position, int id, string description, string prefix, string suffix)
        {
            Card card = new Card();
            card.CardId = id;
            card.Position = position;
            card.Description = description;
            card.Prefix = prefix;
            card.Suffix = suffix;
            return card;
        }
    }
}