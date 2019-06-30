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

namespace Scanner.Models
{
    public class UserSettings
    {
        public Card UserCard { get; set; }
        public bool IsFirstLaunch { get; set; }
        public bool ShowRedBox { get; set; }
        public bool ShowFirst { get; set; }//display charg number on screen befor dail
    }
}