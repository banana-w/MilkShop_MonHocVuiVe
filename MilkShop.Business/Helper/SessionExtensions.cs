using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkShop.Business.Helper
{
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.Set(key, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            byte[] value;
            if (session.TryGetValue(key, out value))
            {
                return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(value));
            }
            else
            {
                return default(T);
            }
        }
    }
}