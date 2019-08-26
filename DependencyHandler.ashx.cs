using System;
using System.Collections.Generic;
using System.Web;
using System.Collections.Specialized;
using System.Text;
using System.IO;

namespace DNNGo.Modules.PowerForms
{
    /// <summary>
    /// 用来串联模块调用的JS
    /// </summary>
    public class DependencyHandler : IHttpHandler
    {

        #region "==属性=="
        /// <summary>
        /// 模块编号
        /// </summary>
        private Int32 ModuleId = WebHelper.GetIntParam(HttpContext.Current.Request, "ModuleId", 0);
        /// <summary>
        /// 模块KTY
        /// </summary>
        private String ModuleKey = WebHelper.GetStringParam(HttpContext.Current.Request, "ModuleKey","");

        #endregion

        #region "==事件=="

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                BingDataItem(context);
            }
            catch(Exception ex)
            {
                throw new ArgumentException("Could not parse the type set in the request");
            }
        }

        #endregion


        #region "==方法=="
        /// <summary>
        /// 绑定数据类
        /// </summary>
        public void BingDataItem(HttpContext context)
        {
            String CacheKey = String.Format("CacheKey_JS_{0}_ModuleId{1}",ModuleKey, ModuleId);

             //读取当前存储的JS列表
            if (FrameWorkCache.Instance().ContainsKey(CacheKey))
            {
                StringBuilder JScontent = new StringBuilder();


              
                //将JS列表中存储的内容取出，拼接
                List<KeyValueEntity> JsNameValue = FrameWorkCache.Instance()[CacheKey] as List<KeyValueEntity>;
                //NameValueCollection JsNameValue = FrameWorkCache.Instance()[CacheKey] as NameValueCollection;
                //循环找到JS中的内容
                foreach (KeyValueEntity Item in JsNameValue)
                {
                    //输出JS列表的内容
                    String JsValue = Convert.ToString( Item.Value);
                    String JsPath = context.Server.MapPath(JsValue);
                    FileInfo jsFile = new FileInfo(JsPath);
                    if (jsFile.Exists)
                    {
                        using (StreamReader sr = jsFile.OpenText())
                        {
                            JScontent.AppendFormat("//++** {0} **++//", Item.Key).AppendLine();
                            JScontent.Append(sr.ReadToEnd()).AppendLine().AppendLine();
                        }
                    }
                }

                context.Response.Write(JScontent);
                context.Response.End();

                //设置当前页面的缓存
                //SetCaching(context,"",
            }

        }





        /// <summary>
        /// Sets the output cache parameters and also the client side caching parameters
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fileName">The name of the file that has been saved to disk</param>
        /// <param name="fileset">The Base64 encoded string supplied in the query string for the handler</param>
        /// <param name="compressionType"></param>
        private void SetCaching(HttpContext context, string fileName, string fileset, CompressionType compressionType)
        {


            //This ensures OutputCaching is set for this handler and also controls
            //client side caching on the browser side. Default is 10 days.
            var duration = TimeSpan.FromDays(10);
            var cache = context.Response.Cache;
            cache.SetCacheability(HttpCacheability.Public);

            cache.SetExpires(xUserTime.LocalTime().Add(duration));
            cache.SetMaxAge(duration);
            cache.SetValidUntilExpires(true);
            cache.SetLastModified(xUserTime.LocalTime());


            cache.SetETag("\"" + (fileset + compressionType.ToString()) + "\"");
            //set server OutputCache to vary by our params

    

            // in any case, cache already varies by pathInfo (build-in) so for path formats, we do not need anything
            // just add params for querystring format, just in case...
            cache.VaryByParams["t"] = true;
            cache.VaryByParams["s"] = true;
            cache.VaryByParams["cdv"] = true;

            //ensure the cache is different based on the encoding specified per browser
            cache.VaryByContentEncodings["gzip"] = true;
            cache.VaryByContentEncodings["deflate"] = true;

            //don't allow varying by wildcard
            cache.SetOmitVaryStar(true);
            //ensure client browser maintains strict caching rules
            cache.AppendCacheExtension("must-revalidate, proxy-revalidate");

            //This is the only way to set the max-age cachability header in ASP.Net!
            //FieldInfo maxAgeField = cache.GetType().GetField("_maxAge", BindingFlags.Instance | BindingFlags.NonPublic);
            //maxAgeField.SetValue(cache, duration);

            //make this output cache dependent on the file if there is one.
            if (!string.IsNullOrEmpty(fileName))
                context.Response.AddFileDependency(fileName);
        }


        #endregion


        #region "==其他=="

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        #endregion
    }

    public enum CompressionType
    {
        deflate, gzip, none
    }
}