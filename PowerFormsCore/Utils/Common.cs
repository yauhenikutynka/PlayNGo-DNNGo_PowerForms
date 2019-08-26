﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using System.Xml;
using System.Globalization;
using System.Xml.Serialization;
using DotNetNuke.Entities.Modules;

namespace DNNGo.Modules.PowerForms
{
    /// <summary>
    /// 通用类
    /// </summary>
    public class Common
    {
        #region "按当前日期和时间生成随机数"
        /// <summary>
        /// 按当前日期和时间生成随机数
        /// </summary>
        /// <param name="Num">附加随机数长度</param>
        /// <returns></returns>
        public static string sRndNum(int Num)
        {
            string sTmp_Str = System.DateTime.Today.Year.ToString() + System.DateTime.Today.Month.ToString("00") + System.DateTime.Today.Day.ToString("00") + System.DateTime.Now.Hour.ToString("00") + System.DateTime.Now.Minute.ToString("00") + System.DateTime.Now.Second.ToString("00");
            return sTmp_Str + RndNum(Num);
        }
        #endregion

        #region 生成0-9随机数
        /// <summary>
        /// 生成0-9随机数
        /// </summary>
        /// <param name="VcodeNum">生成长度</param>
        /// <returns></returns>
        public static string RndNum(int VcodeNum)
        {
            StringBuilder sb = new StringBuilder(VcodeNum);
            Random rand = new Random();
            for (int i = 1; i < VcodeNum + 1; i++)
            {
                int t = rand.Next(9);
                sb.AppendFormat("{0}", t);
            }
            return sb.ToString();

        }
        #endregion

        #region "通过RNGCryptoServiceProvider 生成随机数 0-9"
        /// <summary>
        /// 通过RNGCryptoServiceProvider 生成随机数 0-9 
        /// </summary>
        /// <param name="length">随机数长度</param>
        /// <returns></returns>
        public static string RndNumRNG(int length)
        {
            byte[] bytes = new byte[16];
            RNGCryptoServiceProvider r = new RNGCryptoServiceProvider();
            StringBuilder sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                r.GetBytes(bytes);
                sb.AppendFormat("{0}", (int)((decimal)bytes[0] / 256 * 10));
            }
            return sb.ToString();

        }

        #endregion


        #region "按字符串位数补0"
        /// <summary>
        /// 按字符串位数补0
        /// </summary>
        /// <param name="CharTxt">字符串</param>
        /// <param name="CharLen">字符长度</param>
        /// <returns></returns>
        public static string FillZero(string CharTxt, int CharLen)
        {
            if (CharTxt.Length < CharLen)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(CharTxt);
                for (int i = 0; i < CharLen - CharTxt.Length; i++)
                {
                    sb.Append("0");
                }
                return sb.ToString();
            }
            else
            {
                return CharTxt;
            }
        }

        #endregion


        #region "月份转换"
       /// <summary>
       /// 数字转换为英文月份
       /// </summary>
       /// <param name="m"></param>
       /// <returns></returns>
        public static String ENmonth(String m)
        {
            switch (m)
            {
                case "1":
                    return "January";
                case "2":
                    return "February";
                case "3":
                    return "March";
                case "4":
                    return "April";
                case "5":
                    return "May";
                case "6":
                    return "June";
                case "7":
                    return "July";
                case "8":
                    return "Aguest";
                case "9":
                    return "September";
                case "10":
                    return "October";
                case "11":
                    return "November";
                case "12":
                    return "December";
            }
            return "";
        }

        #endregion
 
        #region "过滤HTML"

        /// <summary>
        /// 过滤字符串中的html代码
        /// </summary>
        /// <param name="Str"></param>
        /// <returns>返回过滤之后的字符串</returns>
        public static string LostHTML(string Str)
        {
            string Re_Str = "";

            if (!String.IsNullOrEmpty(Str))
            {
                string Pattern = "<\\/*[^<>]*>";
                Re_Str = Regex.Replace(HttpUtility.HtmlDecode(Str), Pattern, "");
            }

            return (Re_Str.Replace("\\r\\n", "")).Replace("\\r", "");
        }
        #endregion


        #region "屏蔽XSS攻击关键字符串"

        /// <summary>
        /// 屏蔽XSS攻击关键字符串
        /// </summary>
        /// <param name="Str"></param>
        /// <returns>返回过滤之后的字符串</returns>
        public static string LostXSS(string Str)
        {
            string Re_Str = Str;

            if (!String.IsNullOrEmpty(Str))
            {
                string Pattern = "<\\/*[^<>]*>";
                Re_Str = Regex.Replace(HttpUtility.HtmlDecode(Str), Pattern, "");
                Re_Str = (Re_Str.Replace("\r\n", "")).Replace("\r", "");
                Re_Str = Common.ReplaceNoCase(Re_Str, "<", "");
                Re_Str = Common.ReplaceNoCase(Re_Str, ">", "");
                Re_Str = Common.ReplaceNoCase(Re_Str, "javascript", "");
                Re_Str = Common.ReplaceNoCase(Re_Str, "script", "");
                Re_Str = Common.ReplaceNoCase(Re_Str, "cookie", "");
                Re_Str = Common.ReplaceNoCase(Re_Str, "iframe", "");
                Re_Str = Common.ReplaceNoCase(Re_Str, "expression", "");

                //关于一些JS事件的过滤
                Re_Str = Common.ReplaceNoCase(Re_Str, "onabort", ""); //图像加载被中断
                Re_Str = Common.ReplaceNoCase(Re_Str, "onblur", ""); // 元素失去焦点
                Re_Str = Common.ReplaceNoCase(Re_Str, "onchange", ""); // 用户改变域的内容
                Re_Str = Common.ReplaceNoCase(Re_Str, "onclick", ""); // 鼠标点击某个对象
                Re_Str = Common.ReplaceNoCase(Re_Str, "ondblclick", ""); // 鼠标双击某个对象
                Re_Str = Common.ReplaceNoCase(Re_Str, "onerror", ""); // 当加载文档或图像时发生某个错误
                Re_Str = Common.ReplaceNoCase(Re_Str, "onfocus", ""); // 元素获得焦点
                Re_Str = Common.ReplaceNoCase(Re_Str, "onkeydown", ""); // 某个键盘的键被按下
                Re_Str = Common.ReplaceNoCase(Re_Str, "onkeypress", ""); // 某个键盘的键被按下或按住
                Re_Str = Common.ReplaceNoCase(Re_Str, "onkeyup", ""); // 某个键盘的键被松开
                Re_Str = Common.ReplaceNoCase(Re_Str, "onload", ""); // 某个页面或图像被完成加载
                Re_Str = Common.ReplaceNoCase(Re_Str, "onmousedown", ""); // 某个鼠标按键被按下
                Re_Str = Common.ReplaceNoCase(Re_Str, "onmousemove", ""); // 鼠标被移动
                Re_Str = Common.ReplaceNoCase(Re_Str, "onmouseout", ""); // 鼠标从某元素移开
                Re_Str = Common.ReplaceNoCase(Re_Str, "onmouseover", ""); // 鼠标被移到某元素之上
                Re_Str = Common.ReplaceNoCase(Re_Str, "onmouseup", ""); // 某个鼠标按键被松开
                Re_Str = Common.ReplaceNoCase(Re_Str, "onreset", ""); // 重置按钮被点击
                Re_Str = Common.ReplaceNoCase(Re_Str, "onresize", ""); // 窗口或框架被调整尺寸
                Re_Str = Common.ReplaceNoCase(Re_Str, "onselect", ""); // 文本被选定
                Re_Str = Common.ReplaceNoCase(Re_Str, "onsubmit", ""); // 提交按钮被点击
                Re_Str = Common.ReplaceNoCase(Re_Str, "onunload", ""); // 用户退出页面


            }
            return Re_Str;
        }
        #endregion

        #region "转换版本号为数字"
        /// <summary>
        /// 转换版本号为数字
        /// </summary>
        /// <param name="Version"></param>
        /// <returns></returns>
        public static Int32 ConvertVersion(String Version)
        {
            Int32 Result = 0;
           if(int.TryParse( Version.Replace(".", ""),out Result))
           {

           }
            return Result;
        }



        #endregion

        #region "MD5加密"
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">加密字符</param>
        /// <param name="code">加密位数16/32</param>
        /// <returns></returns>
        public static string md5(string str, int code)
        {
            string strEncrypt = string.Empty;
            if (code == 16)
            {
                strEncrypt = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").Substring(8, 16);
            }

            if (code == 32)
            {
                strEncrypt = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
            }

            return strEncrypt;
        }
        #endregion

        #region "检测新版本"
        /// <summary>
        /// 检测新版本
        /// </summary>
        /// <param name="ModuleName">模块名</param>
        /// <param name="Version">模块版本</param>
        /// <returns></returns>
        public static String LoadUpdateVersion(String ModuleName, String Version)
        {
            try
            {
                //Import
                XmlDocument allXml = new XmlDocument();
                string xmlPath = "http://www.dnngo.net/ModuleVersion.xml";
                allXml.Load(xmlPath);
                XmlNodeList xmlList = allXml.DocumentElement.SelectNodes("//module");

                string latestversion = "";
                string downloadlink = "";

                for (int i = 0; i < xmlList.Count; i++)
                {
                    string name = "";
                    if (xmlList[i].SelectSingleNode("name") != null)
                    {
                        name = xmlList[i].SelectSingleNode("name").InnerText;
                    }

                    if (name.ToLower().Trim() == ModuleName.ToLower().Trim())
                    {
                        if (xmlList[i].SelectSingleNode("latestversion") != null)
                        {
                            latestversion = xmlList[i].SelectSingleNode("latestversion").InnerText;
                        }
                        if (xmlList[i].SelectSingleNode("downloadlink") != null)
                        {
                            downloadlink = xmlList[i].SelectSingleNode("downloadlink").InnerText;
                        }
                        break;
                    }
                }

                if (Convert.ToInt32(latestversion.Replace(".", "")) > Convert.ToInt32(Version.Replace(".", "")))
                {
                    return "<a href=\"" + downloadlink + "\" target=\"_blank\" style=\"font-size:12px;color;red\">" + latestversion + " Updates Available</a>";
                }
            }
            catch
            {

            }
            return "";
        }

        /// <summary>
        /// 检测新版本
        /// </summary>
        /// <param name="ModuleName">模块名</param>
        /// <param name="Version">模块版本</param>
        /// <returns></returns>
        public static String LoadUpdateVersionBy2(String ModuleName, String Version, out String Downloadlink, out String latestversion)
        {
            Downloadlink = "http://www.dnngo.net/";
            latestversion = Version;
            try
            {
                //Import
                XmlDocument allXml = new XmlDocument();
                string xmlPath = "http://www.dnngo.net/ModuleVersion.xml";
                allXml.Load(xmlPath);
                XmlNodeList xmlList = allXml.DocumentElement.SelectNodes("//module");

                for (int i = 0; i < xmlList.Count; i++)
                {
                    string name = "";
                    if (xmlList[i].SelectSingleNode("name") != null)
                    {
                        name = xmlList[i].SelectSingleNode("name").InnerText;
                    }

                    if (name.ToLower().Trim() == ModuleName.ToLower().Trim())
                    {
                        if (xmlList[i].SelectSingleNode("latestversion") != null)
                        {
                            latestversion = xmlList[i].SelectSingleNode("latestversion").InnerText;
                        }
                        if (xmlList[i].SelectSingleNode("downloadlink") != null)
                        {
                            Downloadlink = xmlList[i].SelectSingleNode("downloadlink").InnerText;
                        }
                        break;
                    }
                }



                if (Convert.ToInt32(latestversion.Replace(".", "")) > Convert.ToInt32(Version.Replace(".", "")))
                {
                    //return String.Format("<span class=\"badge\" title=\"{0}\" > Updates </span>", latestversion);
                    return String.Format("<span class=\"badge\"  > Updates </span>", latestversion);
                }
            }
            catch
            {

            }
            return "";
        }


        #endregion
 
        #region "过滤URL中的值"

        /// <summary>
        /// 创建友好连接
        /// </summary>
        /// <param name="pagename"></param>
        /// <returns></returns>
        public static string CreateFriendlySlug(string pagename)
        {
            pagename = pagename.Replace("'", string.Empty);
            pagename = pagename.Replace("\"", string.Empty);
            pagename = pagename.Replace("~", "-");
            pagename = pagename.Replace("!", "-");
            pagename = pagename.Replace("@", "-");
            pagename = pagename.Replace("#", "-");
            pagename = pagename.Replace("$", "-");
            pagename = pagename.Replace("%", "-");
            pagename = pagename.Replace("^", "-");
            pagename = pagename.Replace("&", "-");
            pagename = pagename.Replace("*", "-");
            pagename = pagename.Replace("(", "-");
            pagename = pagename.Replace(")", "-");
            pagename = pagename.Replace("+", "-");
            pagename = pagename.Replace("=", "-");
            pagename = pagename.Replace("`", "-");
            pagename = pagename.Replace(@"\", "-");
            pagename = pagename.Replace("/", "-");
            pagename = pagename.Replace(".", "-");
            pagename = pagename.Replace(",", "-");
            pagename = pagename.Replace("{", "-");
            pagename = pagename.Replace("}", "-");
            pagename = pagename.Replace("[", "-");
            pagename = pagename.Replace("]", "-");
            pagename = pagename.Replace("?", "-");
            pagename = pagename.Replace("<", "-");
            pagename = pagename.Replace(">", "-");
            pagename = pagename.Replace(" ", "-");
            pagename = pagename.Replace("_", "-");
            pagename = pagename.Replace("|", "-");
            pagename = pagename.Replace(";", "-");
            pagename = pagename.Replace(":", "-");
            pagename = pagename + ".aspx";
            pagename = pagename.Replace("------", "-");
            pagename = pagename.Replace("-----", "-");
            pagename = pagename.Replace("----", "-");
            pagename = pagename.Replace("---", "-");
            pagename = pagename.Replace("--", "-");
            if (pagename.EndsWith("-.aspx")) pagename = pagename.Replace("-.aspx", ".aspx");
            return pagename;
        }

        /// <summary>
        /// 返回当前语言
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentCulture()
        {
            return CultureInfo.CurrentCulture.ToString();
        }
        #endregion
 
        #region "在内存中分页"



        /// <summary>
        /// 列表在内存中分页
        /// </summary>
        /// <param name="list">需要分页的数据</param>
        /// <param name="PageIndex">页码</param>a
        /// <param name="PageSize">每页数</param>
        /// <returns>分页好的数据</returns>
        public static List<T> Split<T>(T[] list, Int32 PageIndex, Int32 PageSize)
        {
            //总数
            Int32 RecordCount = list.Length;
            //创建开始索引
            Int32 StartIndex = startRowIndex(PageIndex, PageSize, RecordCount);
            //创建结束索引
            Int32 EndIndex = StartIndex + PageSize - 1;
            EndIndex = EndIndex < RecordCount ? EndIndex : list.Length - 1;//修正结束索引

            List<T> Temp = new List<T>();
            for (Int32 i = StartIndex; i <= EndIndex; i++)
            {
                Temp.Add(list[i]);
            }
            return Temp;
        }

        /// <summary>
        /// 列表在内存中分页
        /// </summary>
        /// <param name="list">需要分页的数据</param>
        /// <param name="PageIndex">页码</param>a
        /// <param name="PageSize">每页数</param>
        /// <returns>分页好的数据</returns>
        public static List<T> Split<T>(List<T> list, Int32 PageIndex, Int32 PageSize)
        {
            return Split<T>(list.ToArray(), PageIndex, PageSize);
        }

        /// <summary>
        /// 开始行数
        /// </summary>
        public static int startRowIndex(Int32 PageIndex, Int32 PageSize, Int32 RecordCount)
        {
            //第一页时,行集索引从零开始 || 实际的行数不够分一页时
            if (PageIndex == 1 || RecordCount <= PageSize) return 0;

            //计算出总页数
            int MaxPage = RecordCount / PageSize + (RecordCount % PageSize > 0 ? 1 : 0);

            //页数不能超过实际最大页面的判断
            if (PageIndex > MaxPage) PageIndex = MaxPage;

            //起始值
            return (PageIndex - 1) * PageSize;
        }
        #endregion
 
        #region "XML序列化反序列化"

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="XmlData"></param>
        /// <returns></returns>
        public static String Serialize<T>(T XmlData)
        {
            XmlSerializer serial = new XmlSerializer(typeof(T));

            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
                serial.Serialize(writer, XmlData);
                writer.Close();
                String XmlString = Encoding.UTF8.GetString(stream.ToArray());
                if (!String.IsNullOrEmpty(XmlString))
                {
                    XmlString = XmlString.Replace("﻿<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                    XmlString = XmlString.Replace("﻿<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
                }
                return XmlString;
            }
        }

        /// <summary>
        /// 范序列化
        /// </summary>
        /// <param name="XmlData"></param>
        /// <returns></returns>
        public static T Deserialize<T>(String XmlData)
            where T : new()
        {
            if (!String.IsNullOrEmpty(XmlData))
            {
                XmlData = XmlData.Replace("﻿<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                XmlData = XmlData.Replace("﻿<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
                try
                {
                    XmlSerializer serial = new XmlSerializer(typeof(T));
                    using (StringReader reader = new StringReader(XmlData))
                    {
                        return (T)serial.Deserialize(reader);
                    }
                }
                catch { }
            }
            return new T();
        }



        #endregion
 
        #region "替换字符串(不区分大小写)"
        /// <summary>
        /// 替换字符串(不区分大小写)
        /// </summary>
        /// <param name="realaceValue"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static string ReplaceNoCase(string realaceValue, string oldValue, string newValue)  //不区分大小写替换
        {
            return Microsoft.VisualBasic.Strings.Replace(realaceValue, oldValue, newValue, 1, -1, Microsoft.VisualBasic.CompareMethod.Text);
        }
        #endregion


        #region "HTML / 文本 互转"
        /// <summary>
        /// 转换纯文本内容为 HTML 内容
        /// </summary>
        /// <param name="Text">纯文本内容</param>
        /// <returns>转换后的 HTML 内容</returns>
        public static string ToHTML(string Text)
        {
            return Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace("<p>" + Text + "</p>", "\r\n", "</p><p>"), "\r", "</p><p>"), "\n", "<br />"), "\t", "    "), "  ", "  ");
        }
        /// <summary>
        /// 转换 Html 内容为纯文本内容
        /// </summary>
        /// <param name="HTML">HTML 内容</param>
        /// <returns>转换后的纯文本内容</returns>
        public static string ToText(string HTML)
        {
            string input = HTML;
            return Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(input, @"(?m)<script[^>]*>(\w|\W)*?</script[^>]*>", "", RegexOptions.Multiline | RegexOptions.IgnoreCase), @"(?m)<style[^>]*>(\w|\W)*?</style[^>]*>", "", RegexOptions.Multiline | RegexOptions.IgnoreCase), @"(?m)<select[^>]*>(\w|\W)*?</select[^>]*>", "", RegexOptions.Multiline | RegexOptions.IgnoreCase), @"(?m)<a[^>]*>(\w|\W)*?</a[^>]*>", "", RegexOptions.Multiline | RegexOptions.IgnoreCase), "(<[^>]+?>)| ", "", RegexOptions.Multiline | RegexOptions.IgnoreCase), @"(\s)+", "", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        }
        #endregion

        #region "分隔字符串为列表"
        /// <summary>
        /// 根据字符串，利用“,”分隔获取集合
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public static List<string> GetList(string strs)
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(strs))
            {
                strs = strs.Replace("，", ",");
                strs = strs.TrimStart(',').TrimEnd(',');
                list = GetList(strs, ",");
            }
            return list;
        }
        /// <summary>
        /// 分隔字符串为列表
        /// </summary>
        /// <param name="Strs"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static List<string> GetList(String Strs, String separator)
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(Strs))
            {
                string[] strArray = Strs.Split(new string[] { separator }, StringSplitOptions.None);
                list.AddRange(strArray);
            }
            return list;
        }

        /// <summary>
        /// 列表转换成逗号相隔的字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static String GetStringByList(String[] list,String suffix = ",")
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Length > 0)
            {
                foreach (String str in list)
                {
                    if (String.IsNullOrEmpty(sb.ToString()))
                    {
                        sb.Append(str);
                    }
                    else
                    {
                        sb.AppendFormat("{1}{0}", str, suffix);
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 列表转换成逗号相隔的字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static String GetStringByList(List<String> list, String suffix = ",")
        {
            return GetStringByList(list.ToArray(), suffix);
           
        }

        #endregion

        #region "浏览器类型"
        /// <summary>
        /// 获取浏览器类型
        /// </summary>
        /// <returns></returns>
        public static string GetBrowserType()
        {
            System.Web.HttpBrowserCapabilities browser =HttpContext.Current. Request.Browser;
            return browser.Browser;
        }

        #endregion





        #region "隐藏部分IP地址"

        /// <summary>
        /// 隐藏部分IP地址
        /// </summary>
        /// <param name="IpAddress"></param>
        /// <returns></returns>
        public static String HideIpAddress(String IpAddress)
        {
            if (!String.IsNullOrEmpty(IpAddress))
            {
                if (IpAddress.IndexOf(":") >= 0)
                {
                    //IPV6
                    String[] ips = IpAddress.Split(':');
                    if (ips != null && ips.Length > 2)
                    {
                        IpAddress = String.Format("{0}:{1}:***", ips[0], ips[1]);
                    }
                }
                else if (IpAddress.IndexOf(".") >= 0)
                {
                    //IPV4
                    String[] ips = IpAddress.Split('.');
                    if (ips != null && ips.Length > 2)
                    {
                        IpAddress = String.Format("{0}.{1}.***", ips[0], ips[1]);
                    }


                }
            }
            return IpAddress;
        }


        #endregion


    }
}