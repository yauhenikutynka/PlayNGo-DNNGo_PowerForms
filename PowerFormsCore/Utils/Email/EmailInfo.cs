using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DotNetNuke.Common;
using System.Collections;
using System.Collections.Generic;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Host;

namespace DNNGo.Modules.PowerForms
{
 
    /// <summary>
    /// 邮箱实体
    /// </summary>
    public class EmailInfo
    {

        private Hashtable _Settings = new Hashtable();
        /// <summary>
        /// 模块配置
        /// </summary>
        public Hashtable Settings
        {
            get { return _Settings; }
            set { _Settings = value; }
        }

        private Boolean _SMTPEnableSSL = false;
        /// <summary>
        /// 保存SMTP的属性
        /// </summary>
        public bool SMTPEnableSSL
        {
            get
            {
                return _SMTPEnableSSL;
            }

            set
            {
                _SMTPEnableSSL = value;
            }
        }


        public string SMTPServer
        {
            get;
            set;
        }


        public string SMTPAuthentication
        {
            get;
            set;
        }

        public string SMTPUsername
        {
            get;
            set;
        }

        public string SMTPPassword
        {
            get;
            set;
        }


        





        /// <summary>
        /// 发送给**邮箱
        /// </summary>
        public string MailTo
        {
            get;
            set;
        }

        /// <summary>
        /// 回复给谁
        /// </summary>
        public string ReplyTo
        {
            get;
            set;
        }
        

        /// <summary>
        /// 邮件内容
        /// </summary>
        public string Content
        {
            get;
            set;
        }
        /// <summary>
        /// 邮件标题
        /// </summary>
        public string Subject
        {
            get;
            set;
        }
        /// <summary>
        /// 发件人
        /// </summary>
        public string MailFrom
        {
            get;
            set;
        }


        private String _Attachments = "";
        /// <summary>
        /// 附件
        /// </summary>
        public String Attachments
        {
            get { return _Attachments; }
            set { _Attachments = value; }
        }


        /// <summary>
        /// 初始化邮件配置
        /// </summary>
        /// <param name="baseModule"></param>
        public EmailInfo()
        {




        }


        public void PushSettings()
        {

            SMTPEnableSSL = Host.EnableSMTPSSL;
            SMTPServer = Host.SMTPServer;
            SMTPAuthentication = Host.SMTPAuthentication;
            SMTPUsername = Host.SMTPUsername;
            SMTPPassword = Host.SMTPPassword;
        }

        /// <summary>
        /// 推送参数到这里
        /// </summary>
        /// <param name="baseModule"></param>
        public void PushSettings(basePortalModule baseModule)
        {

            SMTPEnableSSL = Host.EnableSMTPSSL;
            SMTPServer = Host.SMTPServer;
            SMTPAuthentication = Host.SMTPAuthentication;
            SMTPUsername = Host.SMTPUsername;
            SMTPPassword = Host.SMTPPassword;



            //if (baseModule.SMTPmode)
            //{

            //    if (baseModule.Portal_Settings["SMTPmode"] != null)
            //    {

            //    }else
            //    {
            //        var Portal_Settings = baseModule.Portal_Settings;

            //        SMTPEnableSSL = Portal_Settings["SMTPEnableSSL"] != null && Convert.ToString(Portal_Settings["SMTPEnableSSL"]) == "Y";

            //        SMTPServer = Portal_Settings["SMTPServer"] != null && !String.IsNullOrEmpty(Portal_Settings["SMTPServer"].ToString()) ? Portal_Settings["SMTPServer"].ToString() : "";
            //        SMTPAuthentication = Portal_Settings["SMTPAuthentication"] != null && !String.IsNullOrEmpty(Portal_Settings["SMTPAuthentication"].ToString()) ? Portal_Settings["SMTPAuthentication"].ToString() : "";
            //        SMTPUsername = Portal_Settings["SMTPUsername"] != null && !String.IsNullOrEmpty(Portal_Settings["SMTPUsername"].ToString()) ? Portal_Settings["SMTPUsername"].ToString() : "";
            //        SMTPPassword = Portal_Settings["SMTPPassword"] != null && !String.IsNullOrEmpty(Portal_Settings["SMTPPassword"].ToString()) ? Portal_Settings["SMTPPassword"].ToString() : "";
            //    }






            //} else
            //{
            //    var Host_Settings = Globals.HostSettings;

            //    SMTPEnableSSL = Host.EnableSMTPSSL;
            //    SMTPServer = Host.SMTPServer;
            //    SMTPAuthentication = Host.SMTPAuthentication;
            //    SMTPUsername = Host.SMTPUsername;
            //    SMTPPassword = Host.SMTPPassword;
            //}

            //System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //sb.AppendFormat("SMTPmode:{0}", baseModule.SMTPmode).AppendLine();
            //sb.AppendFormat("SMTPEnableSSL:{0}", SMTPEnableSSL).AppendLine();
            //sb.AppendFormat("SMTPServer:{0}", SMTPServer).AppendLine();
            //sb.AppendFormat("SMTPAuthentication:{0}", SMTPAuthentication).AppendLine();
            //sb.AppendFormat("SMTPUsername:{0}", SMTPUsername).AppendLine();
            //sb.AppendFormat("SMTPPassword:{0}", SMTPPassword).AppendLine();
   
            //Trace.WriteLine(sb.ToString());


        }




        public EmailInfo(string mailTo, string content, string subject, string mailFrom, Hashtable settings)
        {
            MailTo = mailTo;
            Content = content;
            Subject = subject;
            MailFrom = mailFrom;
            Settings = settings;
        }

        public EmailInfo(string mailTo, string content, string subject)
        {
            MailTo = mailTo;
            Content = content;
            Subject = subject;
        }

        public EmailInfo(EmailInfo mInfo)
        {
            MailTo = mInfo.MailTo;
            Content = mInfo.Content;
            Subject = mInfo.Subject;
            MailFrom = mInfo.MailFrom;
        }


       
    }
}
