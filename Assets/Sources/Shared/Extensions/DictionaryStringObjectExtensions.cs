using System;
using System.Collections.Generic;

namespace GGJ2017.Shared.Extensions
{
    public static class DictionaryStringObjectExtensions
    {
        public static float GetFloat(this Dictionary<string, object> dict, string key, float defaultValue = 0f)
        {
            float val = defaultValue;
            object valObj = null;

            if (dict.TryGetValue(key, out valObj) && valObj!= null)
            {
                float.TryParse(valObj.ToString(), out val);
            }

            return val;
        }

        public static int GetInt(this Dictionary<string, object> dict, string key, int defaultValue = 0)
        {
            int val = defaultValue;
            object valObj = null;

            if (dict.TryGetValue(key, out valObj) && valObj != null)
            {
                int.TryParse(valObj.ToString(), out val);
            }

            return val;
        }

        public static bool GetBool(this Dictionary<string, object> dict, string key, bool defaultValue = false)
        {
            bool val = defaultValue;
            object valObj = null;

            if (dict.TryGetValue(key, out valObj) && valObj != null)
            {
                bool.TryParse(valObj.ToString(), out val);
            }

            return val;
        }

        public static string GetString(this Dictionary<string, object> dict, string key, string defaultValue = "")
        {
            string val = defaultValue;
            object valObj = null;

            if (dict.TryGetValue(key, out valObj) && valObj != null)
            {
                val = valObj.ToString();
            }

            return val;
        }

        public static List<T> GetList<T>(this Dictionary<string, object> dict, string key)
        {
            var val = new List<T>();
            object valObj = null;

            bool isInt = typeof(T) == typeof(int);
            bool isFloat = typeof(T) == typeof(float);

            if (dict.TryGetValue(key, out valObj))
            {
                var listObj = valObj as List<object>;

                if (listObj != null)
                {
                    foreach (var obj in listObj)
                    {
                        object objTmp = obj;

                        if (isInt)
                        {
                            objTmp = int.Parse(obj.ToString());
                        }
                        else if (isFloat)
                        {
                            objTmp = float.Parse(obj.ToString());
                        }

                        val.Add((T)objTmp);
                    }
                }
            }

            return val;
        }

        public static List<T> ParseList<T>(this Dictionary<string, object> dict, string key, Func<Dictionary<string, object>, T> parser)
        {
            var val = new List<T>();
            object valObj = null;

            if (dict.TryGetValue(key, out valObj))
            {
                var listObj = valObj as List<object>;

                if (listObj != null)
                {
                    foreach (var obj in listObj)
                    {
                        val.Add(parser(obj as Dictionary<string, object>));
                    }
                }
            }

            return val;
        }
    }
}
