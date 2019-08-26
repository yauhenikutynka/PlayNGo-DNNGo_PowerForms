using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Xml.Serialization;
using System.Globalization;

namespace DNNGo.Modules.PowerForms
{
    /// <summary>
    /// 转换
    /// </summary>
    public class ConvertTo
    {

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string Serialize<T>(T t)
        {
            using (StringWriter sw = new StringWriter())
            {
                XmlSerializer xz = new XmlSerializer(t.GetType());
                xz.Serialize(sw, t);
                return sw.ToString();
            }
        }
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T Deserialize<T>(T t, string s)
        {
            using (StringReader sr = new StringReader(s))
            {
                XmlSerializer xz = new XmlSerializer(t.GetType());

                return (T)xz.Deserialize(sr);
            }
        }


        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T Deserialize<T>( string s)
        {
            using (StringReader sr = new StringReader(s))
            {
                XmlSerializer xz = new XmlSerializer(typeof(T));

                return (T)xz.Deserialize(sr);
            }
        }

        /// <summary>
        /// 格式化
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static object FormatValue(string _value, Type t)
        {
            object o = new object();

            if (!String.IsNullOrEmpty(_value))
            {

                if (t == typeof(DateTime))
                {
                    string[] expectedFormats = { "G", "g", "f", "F" };

                    DateTime oTime = DateTime.Now;

                    //当前系统的语言
                    if (DateTime.TryParseExact(_value, expectedFormats, System.Globalization.CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out oTime))
                    {
                        o = oTime;
                    }
                    else if (DateTime.TryParseExact(_value, expectedFormats, new CultureInfo("en-US", true), DateTimeStyles.AllowWhiteSpaces, out oTime))//英语
                    {
                        o = oTime;
                    }
                    else
                    {
                        //德语、中文、法语、俄语、希腊语、西班牙语
                        string[] Cultures = { "de-DE", "zh-CN", "fr-FR", "ru-RU", "el-GR", "es-ES" };
                        foreach (String Culture in Cultures)
                        {
                            if (DateTime.TryParseExact(_value, expectedFormats, new CultureInfo(Culture, true), DateTimeStyles.AllowWhiteSpaces, out oTime))
                            {
                                o = oTime;
                            }
                        }
                    }
                }
                else if (t == typeof(Int32))
                {
                    o = int.Parse(_value);
                }
                else if (t == typeof(Double))
                {
                    o = Double.Parse(_value);
                }
                else if (t == typeof(Boolean))
                {
                    o = Boolean.Parse(_value);
                }
                else if (t == typeof(Enum))
                {
                    o = Enum.Parse(t, _value);
                }
                else
                {
                    o = _value;
                }
            }
            else
            {
                o = _value;
            }
            return o;
        }

 



    }
}