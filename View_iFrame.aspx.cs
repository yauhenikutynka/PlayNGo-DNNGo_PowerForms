using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Localization;
using Microsoft.VisualBasic;
using DotNetNuke.Web.Client.ClientResourceManagement;
using DotNetNuke.UI.Utilities;
using DotNetNuke.Framework;
using DotNetNuke.Entities.Host;
using DotNetNuke.Services.Personalization;

namespace DNNGo.Modules.PowerForms
{
    /// <summary>
    /// 框架页面调取表单的。
    /// </summary>
    public partial class View_iFrame : BasePage, IClientAPICallbackEventHandler
    {

        #region "属性"


        /// <summary>
        /// 日期模式
        /// </summary>
        public String DatePattern
        {
            get { return Settings["PowerForms_DatePattern"] != null && !string.IsNullOrEmpty(Settings["PowerForms_DatePattern"].ToString()) ? Convert.ToString(Settings["PowerForms_DatePattern"]) : "mm/dd/yyyy"; }
        }

        /// <summary>
        /// 当前标签
        /// </summary>
        public String Token = WebHelper.GetStringParam(HttpContext.Current.Request, "Token", "Form").ToLower();
        #endregion



        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //加载脚本
                LoadViewScript();
            }
            catch (Exception exc) //Module failed to load
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {

        }

        protected override void Page_Init(System.Object sender, System.EventArgs e)
        {
            try
            {    //绑定Tabs和容器中的控件
                BindContainer();
            }
            catch (Exception exc) //Module failed to load
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, exc);
            }
        }


        #region IClientAPICallbackEventHandler Members

        public string RaiseClientAPICallbackEvent(string eventArgument)
        {
            var dict = ParsePageCallBackArgs(eventArgument);
            if (dict.ContainsKey("type"))
            {
                if (DNNClientAPI.IsPersonalizationKeyRegistered(dict["namingcontainer"] + ClientAPI.CUSTOM_COLUMN_DELIMITER + dict["key"]) == false)
                {
                    throw new Exception(string.Format("This personalization key has not been enabled ({0}:{1}).  Make sure you enable it with DNNClientAPI.EnableClientPersonalization", dict["namingcontainer"], dict["key"]));
                }
                switch ((DNNClientAPI.PageCallBackType)Enum.Parse(typeof(DNNClientAPI.PageCallBackType), dict["type"]))
                {
                    case DNNClientAPI.PageCallBackType.GetPersonalization:
                        return Personalization.GetProfile(dict["namingcontainer"], dict["key"]).ToString();
                    case DNNClientAPI.PageCallBackType.SetPersonalization:
                        Personalization.SetProfile(dict["namingcontainer"], dict["key"], dict["value"]);
                        return dict["value"];
                    default:
                        throw new Exception("Unknown Callback Type");
                }
            }
            return "";
        }


        //I realize the parsing of this is rather primitive.  A better solution would be to use json serialization
        //unfortunately, I don't have the time to write it.  When we officially adopt MS AJAX, we will get this type of 
        //functionality and this should be changed to utilize it for its plumbing.
        private Dictionary<string, string> ParsePageCallBackArgs(string strArg)
        {
            string[] aryVals = strArg.Split(new[] { ClientAPI.COLUMN_DELIMITER }, StringSplitOptions.None);
            var objDict = new Dictionary<string, string>();
            if (aryVals.Length > 0)
            {
                objDict.Add("type", aryVals[0]);
                switch (
                    (DNNClientAPI.PageCallBackType)Enum.Parse(typeof(DNNClientAPI.PageCallBackType), objDict["type"]))
                {
                    case DNNClientAPI.PageCallBackType.GetPersonalization:
                        objDict.Add("namingcontainer", aryVals[1]);
                        objDict.Add("key", aryVals[2]);
                        break;
                    case DNNClientAPI.PageCallBackType.SetPersonalization:
                        objDict.Add("namingcontainer", aryVals[1]);
                        objDict.Add("key", aryVals[2]);
                        objDict.Add("value", aryVals[3]);
                        break;
                }
            }
            return objDict;
        }

        #endregion

        /// <summary>
        /// 绑定列表数据到容器
        /// </summary>
        private void BindContainer()
        {
            //加载相应的控件
            basePortalModule ManageContent = new basePortalModule();
            String ModuleSrc = String.Empty;
            if (Token.IndexOf(String.Format("Result{0}", ModuleId), StringComparison.CurrentCultureIgnoreCase) >= 0)
                ModuleSrc = "View_Result.ascx";
            else
                ModuleSrc = "View_Form.ascx";

            ManageContent.ID = Token;
            String ContentSrc = ResolveClientUrl(string.Format("{0}/{1}", this.TemplateSourceDirectory, ModuleSrc));
            ManageContent = (basePortalModule)LoadControl(ContentSrc);
            ManageContent.ModuleConfiguration = this.ModuleConfiguration;
            ManageContent.LocalResourceFile = Localization.GetResourceFile(this, string.Format("{0}.resx", ModuleSrc));
            phPlaceHolder.Controls.Add(ManageContent);
        }




    }
}