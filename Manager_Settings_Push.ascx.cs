using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;

namespace DNNGo.Modules.PowerForms
{
    public partial class Manager_Settings_Push : basePortalModule
    {


        #region "==属性=="

        /// <summary>
        /// 模块操作类
        /// </summary>
        private static ModuleController controller = new ModuleController();

        private ModuleInfo _module = new ModuleInfo();
        /// <summary>
        /// 模块信息
        /// </summary>
        public ModuleInfo Module
        {
            get
            {
                if (!(_module != null && _module.ModuleID > 0))
                {
                    _module = controller.GetModule(ModuleId, TabId, false);
                }

                return _module;
            }

        }



        /// <summary>提示操作类</summary>
        MessageTips mTips = new MessageTips();

        #endregion


        #region "==方法=="

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindDataToPage()
        {

            #region "数据推送设置"
            cbPushEnable.Checked = Settings["PowerForms_Push_Enable"] != null && !string.IsNullOrEmpty(Settings["PowerForms_Push_Enable"].ToString()) ? Convert.ToBoolean(Settings["PowerForms_Push_Enable"]) : false;
            cbAsynchronous.Checked = Settings["PowerForms_Push_Asynchronous"] != null && !string.IsNullOrEmpty(Settings["PowerForms_Push_Asynchronous"].ToString()) ? Convert.ToBoolean(Settings["PowerForms_Push_Asynchronous"]) : true;

            txtTransferUrl.Text = Settings["PowerForms_Push_TransferUrl"] != null && !string.IsNullOrEmpty(Settings["PowerForms_Push_TransferUrl"].ToString()) ? Convert.ToString(Settings["PowerForms_Push_TransferUrl"]) : "http://www.dnngo.net/OurModules/PowerForms/FormPush.aspx";
            txtQueryString.Text = Settings["PowerForms_Push_QueryString"] != null && !string.IsNullOrEmpty(Settings["PowerForms_Push_QueryString"].ToString()) ? Convert.ToString(Settings["PowerForms_Push_QueryString"]) : "";

            WebHelper.BindList(ddlFormMethod, typeof(EnumFormMethod));
            WebHelper.SelectedListByValue(ddlFormMethod, Settings["PowerForms_Push_FormMethod"] != null && !string.IsNullOrEmpty(Settings["PowerForms_Push_FormMethod"].ToString()) ? Convert.ToInt32(Settings["PowerForms_Push_FormMethod"]) : (Int32)EnumFormMethod.POST);


            txtAppVerify.Text = Settings["PowerForms_Push_AppVerify"] != null && !string.IsNullOrEmpty(Settings["PowerForms_Push_AppVerify"].ToString()) ? Convert.ToString(Settings["PowerForms_Push_AppVerify"]) : Guid.NewGuid().ToString("N");
            hlVerifyUrl.Text = hlVerifyUrl.NavigateUrl = String.Format("http://www.dnngo.net/OurModules/PowerForms/FormPush.aspx?go={0}", txtAppVerify.Text);

            #endregion

            #region "数据推送时请求基类的设置"

            txtAccept.Text = Settings["PowerForms_RequestHeader_Accept"] != null && !string.IsNullOrEmpty(Settings["PowerForms_RequestHeader_Accept"].ToString()) ? Convert.ToString(Settings["PowerForms_RequestHeader_Accept"]) : "text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
            txtAcceptLanguage.Text = Settings["PowerForms_RequestHeader_AcceptLanguage"] != null && !string.IsNullOrEmpty(Settings["PowerForms_RequestHeader_AcceptLanguage"].ToString()) ? Convert.ToString(Settings["PowerForms_RequestHeader_AcceptLanguage"]) : "en-US";
            txtAcceptEncoding.Text = Settings["PowerForms_RequestHeader_AcceptEncoding"] != null && !string.IsNullOrEmpty(Settings["PowerForms_RequestHeader_AcceptEncoding"].ToString()) ? Convert.ToString(Settings["PowerForms_RequestHeader_AcceptEncoding"]) : "gzip, deflate";
            txtUserAgent.Text = Settings["PowerForms_RequestHeader_UserAgent"] != null && !string.IsNullOrEmpty(Settings["PowerForms_RequestHeader_UserAgent"].ToString()) ? Convert.ToString(Settings["PowerForms_RequestHeader_UserAgent"]) : "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)";

            #endregion


        }


        /// <summary>
        /// 显示URL控件类型
        /// </summary>
        /// <param name="_UrlValue"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        private String ViewUrlType(String _UrlValue, String DefaultValue)
        {
            if (!String.IsNullOrEmpty(_UrlValue))
            {
                if (_UrlValue.IndexOf("sFileID", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    DefaultValue = "F";
                }
                else if (_UrlValue.IndexOf("TabID", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    DefaultValue = "T";
                }
                else
                {
                    DefaultValue = "U";
                }
            }
            return DefaultValue;
        }

        /// <summary>
        /// 设置数据项
        /// </summary>
        private void SetDataItem()
        {
            #region "数据推送设置"

            UpdateModuleSetting("PowerForms_Push_Enable", cbPushEnable.Checked.ToString());
            UpdateModuleSetting("PowerForms_Push_Asynchronous", cbAsynchronous.Checked.ToString());
            UpdateModuleSetting("PowerForms_Push_FormMethod", ddlFormMethod.Items[ddlFormMethod.SelectedIndex].Value);
            UpdateModuleSetting("PowerForms_Push_TransferUrl", txtTransferUrl.Text.Trim());
            UpdateModuleSetting("PowerForms_Push_QueryString", txtQueryString.Text.Trim());
            UpdateModuleSetting("PowerForms_Push_AppVerify", txtAppVerify.Text.Trim());

            #endregion

            #region "数据推送时请求基类的设置"

            UpdateModuleSetting("PowerForms_RequestHeader_Accept", txtAccept.Text.Trim());
            UpdateModuleSetting("PowerForms_RequestHeader_AcceptLanguage", txtAcceptLanguage.Text.Trim());
            UpdateModuleSetting("PowerForms_RequestHeader_AcceptEncoding", txtAcceptEncoding.Text.Trim());
            UpdateModuleSetting("PowerForms_RequestHeader_UserAgent", txtUserAgent.Text.Trim());


            #endregion



        }








        #endregion


        #region "==事件=="


        /// <summary>
        /// 页面加载事件
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //绑定数据
                    BindDataToPage();
                }
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        /// <summary>
        /// 更新绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // 设置需要绑定的方案项
                SetDataItem();

                mTips.LoadMessage("UpdateSettingsSuccess", EnumTips.Success, this, new String[] { "" });

                //refresh cache
                SynchronizeModule();

                Response.Redirect(xUrl("PushSettings"), true);
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
        }
        /// <summary>
        /// 返回
        /// </summary>
        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            try
            {

                Response.Redirect(xUrl(), true);
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        #endregion









    }
}