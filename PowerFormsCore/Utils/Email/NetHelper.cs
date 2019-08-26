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
using DotNetNuke.Services.Mail;
using DotNetNuke.Common.Utilities;
using System.Text;
using System.Net.Mail;
using System.Collections;
using System.Collections.Generic;
using DotNetNuke.Entities.Host;

namespace DNNGo.Modules.PowerForms
{
    /// <summary>
    /// 网络相关公用类
    /// </summary>
    public class NetHelper
    {

        #region "邮件发送"
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        /// <param name="toemail"></param>
        /// <param name="mailFrom"></param>
        /// <returns></returns>
        public static string SendMail(string subject, string content, string toemail, string mailFrom, Hashtable settings)
        {
            EmailInfo mailInfo = new EmailInfo(toemail, content, subject, mailFrom, settings);

            return SendMail(mailInfo);
        }
 


        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailInfo"></param>
        /// <returns></returns>
        public static string SendMail(EmailInfo mailInfo)
        {
            Hashtable Settings = mailInfo.Settings;

            //string SMTPServer = null;
            //SMTPServer = (string)Globals.HostSettings["SMTPServer"].ToString();

            //string SMTPAuthentication = null;
            //SMTPAuthentication = (string)Globals.HostSettings["SMTPAuthentication"].ToString();

            //string SMTPUsername = null;
            //SMTPUsername = (string)Globals.HostSettings["SMTPUsername"].ToString();

            //string SMTPPassword = null;
            //SMTPPassword = (string)Globals.HostSettings["SMTPPassword"].ToString();

            Boolean SMTPEnableSSL = mailInfo.SMTPEnableSSL;


            string HostEmail = Settings["PowerForms_SenderEmail"] != null && Mail.IsValidEmailAddress(Convert.ToString(Settings["PowerForms_SenderEmail"]), Null.NullInteger) ? Convert.ToString(Settings["PowerForms_SenderEmail"]) : Host.HostEmail;
          



            String MailFrom = String.IsNullOrEmpty(mailInfo.MailFrom) ? HostEmail : mailInfo.MailFrom;

            string status = null;

            if (!String.IsNullOrEmpty(mailInfo.MailTo))//if (!String.IsNullOrEmpty(mailInfo.MailTo) && Mail.IsValidEmailAddress(mailInfo.MailTo, Null.NullInteger))
            {

                if (!String.IsNullOrEmpty(mailInfo.ReplyTo))
                {
                    /** 因为要加入ReplyTo，故只有一个参数可以使用,这里需要多测试了  2015.12.21 **/

                    List<System.Net.Mail.Attachment> AttachmentList = new List<System.Net.Mail.Attachment>();
                    if (!String.IsNullOrEmpty(mailInfo.Attachments))
                    {
                        foreach (String filename in WebHelper.GetList(mailInfo.Attachments))
                        {
                            AttachmentList.Add(new Attachment(filename));
                        }
                    }

                    status = Mail.SendMail(MailFrom, mailInfo.MailTo, "", "", mailInfo.ReplyTo, DotNetNuke.Services.Mail.MailPriority.Normal, mailInfo.Subject, MailFormat.Html, Encoding.UTF8, mailInfo.Content, AttachmentList, mailInfo.SMTPServer, mailInfo.SMTPAuthentication, mailInfo.SMTPUsername, mailInfo.SMTPPassword, SMTPEnableSSL);

                }
                else
                {

                    if (SMTPEnableSSL)
                    {
                        if (!String.IsNullOrEmpty(mailInfo.Attachments))
                        {

                            status = Mail.SendMail(MailFrom, mailInfo.MailTo, "", "", DotNetNuke.Services.Mail.MailPriority.Normal, mailInfo.Subject, MailFormat.Html, Encoding.UTF8, mailInfo.Content, mailInfo.Attachments, mailInfo.SMTPServer, mailInfo.SMTPAuthentication, mailInfo.SMTPUsername, mailInfo.SMTPPassword, SMTPEnableSSL);

                        }
                        else
                        {

                            status = Mail.SendMail(MailFrom, mailInfo.MailTo, "", "", "", DotNetNuke.Services.Mail.MailPriority.Normal, mailInfo.Subject, MailFormat.Html, Encoding.UTF8, mailInfo.Content, new List<System.Net.Mail.Attachment>(), mailInfo.SMTPServer, mailInfo.SMTPAuthentication, mailInfo.SMTPUsername, mailInfo.SMTPPassword, SMTPEnableSSL);
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(mailInfo.Attachments))
                        {
                            status = Mail.SendMail(MailFrom, mailInfo.MailTo, "", mailInfo.Subject, mailInfo.Content, mailInfo.Attachments, "html", mailInfo.SMTPServer, mailInfo.SMTPAuthentication, mailInfo.SMTPUsername, mailInfo.SMTPPassword);
                        }
                        else
                        {
                            status = Mail.SendMail(MailFrom, mailInfo.MailTo, "", mailInfo.Subject, mailInfo.Content, "", "html", mailInfo.SMTPServer, mailInfo.SMTPAuthentication, mailInfo.SMTPUsername, mailInfo.SMTPPassword);
                        }

                        //statue = Mail.SendMail(MailFrom, mailInfo.MailTo, "", mailInfo.Subject, mailInfo.Content, mailInfo.Attachments, "html", "", "", "", "");
                    }
                }

               
            }
            return status;

        }

 
        #endregion

    }
}
