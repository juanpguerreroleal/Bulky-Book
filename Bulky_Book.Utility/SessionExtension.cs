using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bulky_Book.Utility
{
    public static class SessionExtension
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        public static T GetObject<T>(this ISession session, string key)
        {
            var jsonObject = session.GetString(key);
            return jsonObject == null ? default(T) : JsonConvert.DeserializeObject<T>(jsonObject);
        }
    }
}
