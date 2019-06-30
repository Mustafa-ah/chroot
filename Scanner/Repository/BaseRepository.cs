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
using Newtonsoft.Json;

namespace Scanner.Repository
{
    public class BaseRepository
    {

        public async Task<T> DeserializeTObject<T>(string ObjString)
            where T : new()
        {
            T Obj = new T();
            try
            {
                await Task.Run(() => Obj = JsonConvert.DeserializeObject<T>(ObjString));
            }
            catch
            {
                Obj = default(T);
            }
            return Obj;
        }

        public async Task<string> SerializeObject(object obj)
        {
            string ObjJson = "";
            try
            {
                await Task.Run(() =>
                {
                    ObjJson = JsonConvert.SerializeObject(obj);
                });

            }
            catch
            {
                ObjJson = "";
            }
            return ObjJson;
        }
    }
}