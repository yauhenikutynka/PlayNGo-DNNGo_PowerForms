using System;
using System.Net;

namespace DNNGo.Modules.PowerForms
{
    /// <summary>
    /// 扩展的Web客户端
    /// </summary>
    public class WebClientX : WebClient
    {
        #region 为了Cookie而重写
        private CookieContainer _Cookie;
        /// <summary>Cookie容器</summary>
        public CookieContainer Cookie
        {
            get { return _Cookie ?? (_Cookie = new CookieContainer()); }
            set { _Cookie = value; }
        }

        /// <summary>
        /// 重写获取请求
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);

            if (request is HttpWebRequest) (request as HttpWebRequest).CookieContainer = Cookie;

            if (Timeout > 0) request.Timeout = Timeout;

            return request;
        }

        /// <summary>
        /// 重写获取响应
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response = base.GetWebResponse(request);

            if (response is HttpWebResponse) Cookie.Add((response as HttpWebResponse).Cookies);

            return response;
        }
        #endregion

        #region 属性
        ///// <summary>可接受类型</summary>
        //public String Accept
        //{
        //    get { return Headers[HttpRequestHeader.Accept]; }
        //    set { Headers[HttpRequestHeader.Accept] = value; }
        //}

        private Int32 _Timeout;
        /// <summary>超时，毫秒</summary>
        public Int32 Timeout
        {
            get { return _Timeout; }
            set { _Timeout = value; }
        }
        #endregion

        #region 构造
        /// <summary>
        /// 初始化常用的东西
        /// </summary>
        public WebClientX()
        {
            Headers[HttpRequestHeader.Accept] = "image/jpeg, image/gif, */*";
            Headers[HttpRequestHeader.AcceptLanguage] = "en-US";
            Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
            Headers[HttpRequestHeader.UserAgent] = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)";
        }

        /// <summary>
        /// 初始化常用的东西
        /// </summary>
        /// <param name="BPM">模块的基类</param>
        public WebClientX(basePortalModule BPM)
        {

            Headers[HttpRequestHeader.Accept] = BPM.Settings["PowerForms_RequestHeader_Accept"] != null && !string.IsNullOrEmpty(BPM.Settings["PowerForms_RequestHeader_Accept"].ToString()) ? Convert.ToString(BPM.Settings["PowerForms_RequestHeader_Accept"]) : "text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
            Headers[HttpRequestHeader.AcceptLanguage] = BPM.Settings["PowerForms_RequestHeader_AcceptLanguage"] != null && !string.IsNullOrEmpty(BPM.Settings["PowerForms_RequestHeader_AcceptLanguage"].ToString()) ? Convert.ToString(BPM.Settings["PowerForms_RequestHeader_AcceptLanguage"]) : "en-US";
            Headers[HttpRequestHeader.AcceptEncoding] = BPM.Settings["PowerForms_RequestHeader_AcceptEncoding"] != null && !string.IsNullOrEmpty(BPM.Settings["PowerForms_RequestHeader_AcceptEncoding"].ToString()) ? Convert.ToString(BPM.Settings["PowerForms_RequestHeader_AcceptEncoding"]) : "gzip, deflate";
            Headers[HttpRequestHeader.UserAgent] = BPM.Settings["PowerForms_RequestHeader_UserAgent"] != null && !string.IsNullOrEmpty(BPM.Settings["PowerForms_RequestHeader_UserAgent"].ToString()) ? Convert.ToString(BPM.Settings["PowerForms_RequestHeader_UserAgent"]) : "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)";
        }

        #endregion
    }
}