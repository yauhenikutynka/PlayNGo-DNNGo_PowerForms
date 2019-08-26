using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Globalization;

namespace DNNGo.Modules.PowerForms
{
    /// <summary>
    /// 确定表单的类
    /// </summary>
    public partial class View_PaymentConfirm : basePortalModule
    {
        //private int HistoryId = -1;
        //private bool IsSendEmail = false;
        //protected LinkButton lbCheckOut;
        //protected Label lblMessage;
        //protected LinkButton lbReturn;
        //protected Literal litPaymentDetail;
        //private string paymentFirstName = "";
        //private string paymentItemName = "";
        //private string paymentLastName = "";
        //private string PaypalReturnUrl = "";
        //protected RadioButtonList rblPaymentMethods;
 
        //private string TotalAmount = "0";
        //private XmlDocument xmlDoc = new XmlDocument();

        //private string GetPaymentStatus(string paramenter)
        //{
        //    if ((paramenter == "0") || (paramenter.Trim() == ""))
        //    {
        //        return DotNetNuke.Services.Localization.Localization.GetString("NotPaid", base.LocalResourceFile);
        //    }
        //    return DotNetNuke.Services.Localization.Localization.GetString("Paid", base.LocalResourceFile);
        //}

        //private void GoToPaypal()
        //{
        //    string paymentFirstName = this.paymentFirstName;
        //    string paymentLastName = this.paymentLastName;
        //    string paymentItemName = this.paymentItemName;
        //    string strPost = base.Settings["PayPalID"].ToString();
        //    string url = "https://www.paypal.com/cgi-bin/webscr?cmd=_ext-enter";
        //    if (((string)base.Settings["dpEn"]) == "sandbox")
        //    {
        //        url = "https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_ext-enter";
        //    }
        //    CultureInfo info = new CultureInfo("en-US");
        //    this.TotalAmount = string.Format(info.NumberFormat, "{0:#####0.00}", new object[] { this.TotalAmount });
        //    url = ((((((url + "&redirect_cmd=_xclick&business=" + Globals.HTTPPOSTEncode(strPost)) + "&item_name=" + Globals.HTTPPOSTEncode(paymentItemName)) + "&item_number=" + Globals.HTTPPOSTEncode(this.HistoryId.ToString())) + "&no_shipping=1&no_note=1&rm=2" + "&quantity=1") + "&amount=" + Globals.HTTPPOSTEncode(this.TotalAmount)) + "&currency_code=" + Globals.HTTPPOSTEncode(this.PaypalCurrency)) + "&custom=" + Globals.HTTPPOSTEncode(base.UserId.ToString());
        //    if (paymentFirstName != "")
        //    {
        //        url = url + "&first_name=" + Globals.HTTPPOSTEncode(paymentFirstName);
        //    }
        //    if (paymentLastName != "")
        //    {
        //        url = url + "&last_name=" + Globals.HTTPPOSTEncode(paymentLastName);
        //    }
        //    url = ((url + "&return=" + Globals.HTTPPOSTEncode(Globals.AddHTTP(this.PaypalReturnUrl))) + "&cancel_return=" + this.PaypalCancelUrl) + "&sra=1";
        //    base.Response.Redirect(url, true);
        //}

        //protected void lbCheckOut_Click(object sender, EventArgs e)
        //{
        //    this.GoToPaypal();
        //}

        //protected void lbReturn_Click(object sender, EventArgs e)
        //{
        //    base.Response.Redirect(Globals.NavigateURL());
        //}

        //private void LoadContent()
        //{
        //    string emailTemplate = this.EmailTemplate;
        //    string paymentTemplate = this.PaymentTemplate;
        //    HistoryInfo history = Common.GetHistory(this.HistoryId);
        //    string xml = history.FeildData.Replace("&", "&amp;");
        //    this.xmlDoc.LoadXml(xml);
        //    List<SuperFormInfo> pages = Common.GetPages(base.ModuleId);
        //    for (int i = 1; i <= pages.Count; i++)
        //    {
        //        SuperFormInfo info2 = pages[i - 1];
        //        List<FeildInfo> list2 = Common.FromXMLToList(info2.PageFeildXML);
        //        if (this.xmlDoc.DocumentElement.SelectNodes("Step")[i - 1].Attributes["Title"] != null)
        //        {
        //            paymentTemplate = Common.ReplaceNoCase(paymentTemplate, "[Step" + i.ToString() + "Name]", this.xmlDoc.DocumentElement.SelectNodes("Step")[i - 1].Attributes["Title"].Value);
        //            emailTemplate = Common.ReplaceNoCase(emailTemplate, "[Step" + i.ToString() + "Name]", this.xmlDoc.DocumentElement.SelectNodes("Step")[i - 1].Attributes["Title"].Value);
        //        }
        //        for (int j = 1; j <= list2.Count; j++)
        //        {
        //            if (this.xmlDoc.DocumentElement.SelectNodes("Step")[i - 1].SelectNodes("Feild")[j - 1].Attributes["Title"] != null)
        //            {
        //                paymentTemplate = Common.ReplaceNoCase(paymentTemplate, "[Step" + i.ToString() + "Title" + j.ToString() + "]", this.xmlDoc.DocumentElement.SelectNodes("Step")[i - 1].SelectNodes("Feild")[j - 1].Attributes["Title"].Value);
        //                emailTemplate = Common.ReplaceNoCase(emailTemplate, "[Step" + i.ToString() + "Title" + j.ToString() + "]", this.xmlDoc.DocumentElement.SelectNodes("Step")[i - 1].SelectNodes("Feild")[j - 1].Attributes["Title"].Value);
        //            }
        //            if (this.xmlDoc.DocumentElement.SelectNodes("Step")[i - 1].SelectNodes("Feild")[j - 1] != null)
        //            {
        //                paymentTemplate = Common.ReplaceNoCase(paymentTemplate, "[Step" + i.ToString() + "Content" + j.ToString() + "]", this.xmlDoc.DocumentElement.SelectNodes("Step")[i - 1].SelectNodes("Feild")[j - 1].InnerText);
        //                emailTemplate = Common.ReplaceNoCase(emailTemplate, "[Step" + i.ToString() + "Content" + j.ToString() + "]", this.xmlDoc.DocumentElement.SelectNodes("Step")[i - 1].SelectNodes("Feild")[j - 1].InnerText);
        //            }
        //        }
        //    }
        //    if (this.xmlDoc.DocumentElement.SelectSingleNode("TotalAmount") != null)
        //    {
        //        this.TotalAmount = this.xmlDoc.DocumentElement.SelectSingleNode("TotalAmount").InnerText;
        //        paymentTemplate = Common.ReplaceNoCase(paymentTemplate, "[TotalAmount]", this.xmlDoc.DocumentElement.SelectSingleNode("TotalAmount").InnerText);
        //        emailTemplate = Common.ReplaceNoCase(emailTemplate, "[TotalAmount]", this.xmlDoc.DocumentElement.SelectSingleNode("TotalAmount").InnerText);
        //    }
        //    string innerText = "0";
        //    if (this.xmlDoc.DocumentElement.SelectSingleNode("PaymentStatus") != null)
        //    {
        //        innerText = this.xmlDoc.DocumentElement.SelectSingleNode("PaymentStatus").InnerText;
        //        paymentTemplate = Common.ReplaceNoCase(paymentTemplate, "[PaymentStatus]", this.GetPaymentStatus(this.xmlDoc.DocumentElement.SelectSingleNode("PaymentStatus").InnerText));
        //        emailTemplate = Common.ReplaceNoCase(emailTemplate, "[PaymentStatus]", this.GetPaymentStatus(this.xmlDoc.DocumentElement.SelectSingleNode("PaymentStatus").InnerText));
        //    }
        //    if (innerText == "0")
        //    {
        //        paymentTemplate = Common.ReplaceNoCase(paymentTemplate, "[PaymentDate]", "");
        //        paymentTemplate = Common.ReplaceNoCase(paymentTemplate, "[TransactionID]", "");
        //    }
        //    else
        //    {
        //        if (this.xmlDoc.DocumentElement.SelectSingleNode("PaymentDate") != null)
        //        {
        //            paymentTemplate = Common.ReplaceNoCase(paymentTemplate, "[PaymentDate]", this.xmlDoc.DocumentElement.SelectSingleNode("PaymentDate").InnerText);
        //            emailTemplate = Common.ReplaceNoCase(emailTemplate, "[PaymentDate]", this.xmlDoc.DocumentElement.SelectSingleNode("PaymentDate").InnerText);
        //        }
        //        if (this.xmlDoc.DocumentElement.SelectSingleNode("TransactionID") != null)
        //        {
        //            paymentTemplate = Common.ReplaceNoCase(paymentTemplate, "[TransactionID]", this.xmlDoc.DocumentElement.SelectSingleNode("TransactionID").InnerText);
        //            emailTemplate = Common.ReplaceNoCase(emailTemplate, "[TransactionID]", this.xmlDoc.DocumentElement.SelectSingleNode("TransactionID").InnerText);
        //        }
        //        this.lbCheckOut.Visible = false;
        //    }
        //    if (this.xmlDoc.DocumentElement.SelectSingleNode("PaymentLink") != null)
        //    {
        //        paymentTemplate = Common.ReplaceNoCase(paymentTemplate, "[PaymentLink]", this.xmlDoc.DocumentElement.SelectSingleNode("PaymentLink").InnerText);
        //        emailTemplate = Common.ReplaceNoCase(emailTemplate, "[PaymentLink]", this.xmlDoc.DocumentElement.SelectSingleNode("PaymentLink").InnerText);
        //    }
        //    if (this.xmlDoc.DocumentElement.SelectSingleNode("PaymentFirstName") != null)
        //    {
        //        this.paymentFirstName = this.xmlDoc.DocumentElement.SelectSingleNode("PaymentFirstName").InnerText;
        //    }
        //    if (this.xmlDoc.DocumentElement.SelectSingleNode("PaymentLastName") != null)
        //    {
        //        this.paymentLastName = this.xmlDoc.DocumentElement.SelectSingleNode("PaymentLastName").InnerText;
        //    }
        //    if (this.xmlDoc.DocumentElement.SelectSingleNode("PaymentItemName") != null)
        //    {
        //        this.paymentItemName = this.xmlDoc.DocumentElement.SelectSingleNode("PaymentItemName").InnerText;
        //    }
        //    this.litPaymentDetail.Text = base.Server.HtmlDecode(paymentTemplate);
        //    emailTemplate = base.Server.HtmlDecode(emailTemplate);
        //    if (((history.PaymentStatus == "1") && !this.IsSendEmail) && ((base.Session["IsSendEmail"] == null) || ((base.Session["IsSendEmail"] != null) && (base.Session["IsSendEmail"].ToString() != this.HistoryId.ToString()))))
        //    {
        //        //Common.SendMail(this.EmailSubject, emailTemplate, this.EmailReceiver, base.PortalSettings.Email);
        //        history.Content = emailTemplate;
        //        //Common.UpdateHistory(history);
        //        base.Session["IsSendEmail"] = this.HistoryId.ToString();
        //        base.Response.Redirect(Globals.NavigateURL(base.TabId, "PaymentConfirm", new string[] { "mid=" + base.ModuleId.ToString(), "HistoryId=" + this.HistoryId.ToString(), "IsSendEmail=True" }));
        //    }
        //}

        //private void LoadPayment()
        //{
        //    bool flag = true;
        //    if ((base.Request.QueryString["PaypalReturn"] != null) && flag)
        //    {
        //        string str5 = base.Request.QueryString["PaypalReturn"].ToString().ToLower();
        //        if ((str5 != null) && !(str5 == "notify"))
        //        {
        //            if (str5 == "return")
        //            {
        //                HistoryInfo history = Common.GetHistory(this.HistoryId);
        //                if (history.PaymentStatus == "0")
        //                {
        //                    string strPost = "";
        //                    string txToken = base.Request.QueryString["tx"];
        //                    if (this.PaypalPDTVerified(txToken, ref strPost))
        //                    {
        //                        history.PaymentDate = DateTime.Now;
        //                        history.PaymentStatus = "1";
        //                        history.FeildData = Common.ReplaceNoCase(history.FeildData, "<PaymentStatus>0</PaymentStatus>", "<PaymentStatus>1</PaymentStatus>");
        //                        history.FeildData = Common.ReplaceNoCase(history.FeildData, "<PaymentStatus></PaymentStatus>", "<PaymentStatus>1</PaymentStatus>");
        //                        string[] strArray = strPost.Split(new char[] { '\n' });
        //                        for (int i = 1; i < (strArray.Length - 1); i++)
        //                        {
        //                            string[] strArray2 = strArray[i].Split(new char[] { '=' });
        //                            string str3 = strArray2[0];
        //                            string str4 = HttpUtility.UrlDecode(strArray2[1]);
        //                            str5 = str3;
        //                            if ((str5 != null) && (str5 == "txn_id"))
        //                            {
        //                                history.TransactionID = Convert.ToString(str4);
        //                                history.FeildData = Common.ReplaceNoCase(history.FeildData, "[TransactionID]", history.TransactionID);
        //                            }
        //                        }
        //                        Common.UpdateHistory(history);
        //                        this.lblMessage.Visible = true;
        //                        this.lblMessage.Text = DotNetNuke.Services.Localization.Localization.GetString("PaymentOKMsg", base.LocalResourceFile);
        //                    }
        //                    else
        //                    {
        //                        this.lblMessage.Visible = true;
        //                        this.lblMessage.Text = DotNetNuke.Services.Localization.Localization.GetString("PaymentFailMsg", base.LocalResourceFile);
        //                    }
        //                }
        //            }
        //            else if ((str5 == "cancel") && (base.Request.QueryString["ModuleId"].ToString() == base.ModuleId.ToString()))
        //            {
        //                base.Response.Redirect(Globals.NavigateURL(), true);
        //            }
        //        }
        //    }
        //}

        //private void LoadScript()
        //{
        //    Control control = this.Page.FindControl("CSS");
        //    if ((control != null) && (HttpContext.Current.Items["SuperForm_Module"] == null))
        //    {
        //        Literal child = new Literal();
        //        child.Text = "<link  rel=\"stylesheet\" type=\"text/css\" href=\"" + base.ModulePath + "Effects/" + this.SettingsEffect + "/Themes/" + this.SettingsTheme + "/Theme.css\" /><link  rel=\"stylesheet\" type=\"text/css\" href=\"" + base.ModulePath + "CSS/Module.css\" />";
        //        HttpContext.Current.Items.Add("SuperForm_Module", "true");
        //        control.Controls.Add(child);
        //    }
        //}

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (base.Request.QueryString["HistoryId"] != null)
        //    {
        //        this.HistoryId = Convert.ToInt32(base.Request.QueryString["HistoryId"]);
        //    }
        //    else if (base.Request.QueryString["item_number"] != null)
        //    {
        //        this.HistoryId = Convert.ToInt32(base.Request.QueryString["item_number"]);
        //    }
        //    this.PaypalReturnUrl = Globals.NavigateURL(base.TabId, "PaymentConfirm", new string[] { "mid=" + base.ModuleId.ToString(), "PaypalReturn=RETURN", "HistoryId=" + this.HistoryId.ToString() });
        //    if (base.Request.QueryString["IsSendEmail"] != null)
        //    {
        //        this.IsSendEmail = Convert.ToBoolean(base.Request.QueryString["IsSendEmail"]);
        //    }
        //    this.lblMessage.Visible = false;
        //    this.LoadScript();
        //    this.LoadPayment();
        //    this.LoadContent();
        //}

        //private bool PaypalPDTVerified(string txToken, ref string strPost)
        //{
        //    bool flag = true;
        //    string pDTIdentityToken = this.PDTIdentityToken;
        //    string str2 = string.Format("cmd=_notify-synch&tx={0}&at={1}", txToken, pDTIdentityToken);
        //    string requestUriString = "https://www.paypal.com/cgi-bin/webscr";
        //    if (((string)base.Settings["dpEn"]) == "sandbox")
        //    {
        //        requestUriString = "https://www.sandbox.paypal.com/cgi-bin/webscr";
        //    }
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUriString);
        //    request.Method = "POST";
        //    request.ContentType = "application/x-www-form-urlencoded";
        //    request.ContentLength = str2.Length;
        //    StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
        //    writer.Write(str2);
        //    writer.Close();
        //    StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
        //    strPost = reader.ReadToEnd();
        //    reader.Close();
        //    if (!strPost.StartsWith("SUCCESS"))
        //    {
        //        flag = false;
        //    }
        //    return flag;
        //}

        //public string EmailReceiver
        //{
        //    get
        //    {
        //        if (base.Settings["Receiver"] != null)
        //        {
        //            return base.Settings["Receiver"].ToString();
        //        }
        //        return base.PortalSettings.Email;
        //    }
        //}

        //private string EmailSubject
        //{
        //    get
        //    {
        //        return "";
        //    }
        //}

        //private string EmailTemplate
        //{
        //    get
        //    {
        //        return "";
        //    }
        //}

        //private string PaymentTemplate
        //{
        //    get
        //    {
        //        return "";
        //    }
        //}

        //private string PaypalCancelUrl
        //{
        //    get
        //    {
        //        if ((((string)base.Settings["PaypalCancelUrl"]) == null) || (((string)base.Settings["PaypalCancelUrl"]) == ""))
        //        {
        //            return Globals.AddHTTP(Globals.NavigateURL());
        //        }
        //        return (string)base.Settings["PaypalCancelUrl"];
        //    }
        //}

        //private string PaypalCurrency
        //{
        //    get
        //    {
        //        if ((base.Settings["PaypalCurrency"] == null) || string.IsNullOrEmpty(base.Settings["PaypalCurrency"].ToString()))
        //        {
        //            return "USD";
        //        }
        //        return base.Settings["PaypalCurrency"].ToString();
        //    }
        //}

        //private string PDTIdentityToken
        //{
        //    get
        //    {
        //        if ((((string)base.Settings["PDTIdentityToken"]) == null) || (((string)base.Settings["PDTIdentityToken"]) == ""))
        //        {
        //            return "";
        //        }
        //        return (string)base.Settings["PDTIdentityToken"];
        //    }
        //}

        //public string SettingsEffect
        //{
        //    get
        //    {
        //        if (base.Settings["Effect"] != null)
        //        {
        //            return base.Settings["Effect"].ToString();
        //        }
        //        return "S001_Normal";
        //    }
        //}

        //public string SettingsTheme
        //{
        //    get
        //    {
        //        if (base.Settings["Theme"] != null)
        //        {
        //            return base.Settings["Theme"].ToString();
        //        }
        //        return "S001_Theme_Default";
        //    }
        //}
    }
}

