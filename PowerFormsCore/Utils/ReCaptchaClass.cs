using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace DNNGo.Modules.PowerForms
{
    /// <summary>
    /// google验证类
    /// </summary>
    public class ReCaptchaClass
    {

        public static Dictionary<String, Object> Validate(string EncodedResponse,basePortalModule baseModule)
        {

            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            Dictionary<String, Object> captchaResponse = new Dictionary<string, Object>();

            
            
            string PrivateKey = baseModule.ViewSettingT<String>("PowerForms_Recaptcha_v3_SecretKey", "");

            try
            {
                using (var client = new System.Net.WebClient())
                {
                  
                    client.Headers[System.Net.HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                    //if (baseModule.IsSSL)
                    //{
                    //    client.Headers[System.Net.HttpRequestHeader.KeepAlive] = "true";
                    //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
                    //}

                    client.Headers[System.Net.HttpRequestHeader.KeepAlive] = "true";
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;



                    //var GoogleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}&remoteip={2}", PrivateKey, EncodedResponse, WebHelper.UserHost));
                    var GoogleReply = client.UploadString("https://www.google.com/recaptcha/api/siteverify","POST", string.Format("secret={0}&response={1}&remoteip={2}", PrivateKey, EncodedResponse, WebHelper.UserHost));
                    //captchaResponse.Add("error-net",  string.Format("secret={0}&response={1}&remoteip={2}", PrivateKey, EncodedResponse, WebHelper.UserHost));
                    captchaResponse = jsSerializer.Deserialize<Dictionary<String, Object>>(GoogleReply);
                    XTrace.WriteLine(GoogleReply);
                }
            }
            catch (Exception exc)
            {
                captchaResponse.Add("error-net", exc.Message);
                return captchaResponse;

            }

            return captchaResponse;

        }


   



    }
}