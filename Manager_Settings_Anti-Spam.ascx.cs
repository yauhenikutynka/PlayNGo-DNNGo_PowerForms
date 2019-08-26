using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;

namespace DNNGo.Modules.PowerForms
{
    public partial class Manager_Settings_Anti_Spam : basePortalModule
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

            #region "隐藏域验证设置"
            cbHiddenfieldsEnable.Checked = Settings["PowerForms_Hiddenfields_Enable"] != null && !string.IsNullOrEmpty(Settings["PowerForms_Hiddenfields_Enable"].ToString()) ? Convert.ToBoolean(Settings["PowerForms_Hiddenfields_Enable"]) : true;

            txtEncryptionKey.Text = Settings["PowerForms_Hiddenfields_EncryptionKey"] != null && !string.IsNullOrEmpty(Settings["PowerForms_Hiddenfields_EncryptionKey"].ToString()) ? Convert.ToString(Settings["PowerForms_Hiddenfields_EncryptionKey"]) : String.Format("powerforms#{0}&{1}", PortalId, ModuleId);
            txtVerifyStringLength.Text = Settings["PowerForms_Hiddenfields_VerifyStringLength"] != null && !string.IsNullOrEmpty(Settings["PowerForms_Hiddenfields_VerifyStringLength"].ToString()) ? Convert.ToString(Settings["PowerForms_Hiddenfields_VerifyStringLength"]) : "12";
            txtVerifyIntervalTime.Text = Settings["PowerForms_Hiddenfields_VerifyIntervalTime"] != null && !string.IsNullOrEmpty(Settings["PowerForms_Hiddenfields_VerifyIntervalTime"].ToString()) ? Convert.ToString(Settings["PowerForms_Hiddenfields_VerifyIntervalTime"]) : "10";

            #endregion



            #region "验证码功能"
            cbCaptchaEnable.Checked = ViewSettingT<Boolean>("PowerForms_Recaptcha_v3_Enable", false);
            txtCaptchaSiteKey.Text = ViewSettingT<String>("PowerForms_Recaptcha_v3_SiteKey", "");
            txtCaptchaSecretKey.Text = ViewSettingT<String>("PowerForms_Recaptcha_v3_SecretKey", "");
            txtLimitSocial.Text = ViewSettingT<String>("PowerForms_Recaptcha_v3_Social", "0.1");
            //txtCaptchaTabindex.Text = ViewSettingT<String>("PowerForms_Recaptcha_Tabindex", "0");

            //WebHelper.SelectedListByValue(ddlCaptchaTheme, ViewSettingT<String>("PowerForms_Recaptcha_Theme", "light"));
            //WebHelper.SelectedListByValue(ddlCaptchaSize, ViewSettingT<String>("PowerForms_Recaptcha_Size", "normal"));
            //WebHelper.SelectedListByValue(ddlCaptchaType, ViewSettingT<String>("PowerForms_Recaptcha_Type", "image"));

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
            #region "隐藏域验证设置"

            UpdateModuleSetting("PowerForms_Hiddenfields_Enable", cbHiddenfieldsEnable.Checked.ToString());
            UpdateModuleSetting("PowerForms_Hiddenfields_EncryptionKey", txtEncryptionKey.Text.Trim());
            UpdateModuleSetting("PowerForms_Hiddenfields_VerifyStringLength", txtVerifyStringLength.Text.Trim());
            UpdateModuleSetting("PowerForms_Hiddenfields_VerifyIntervalTime", txtVerifyIntervalTime.Text.Trim());

            #endregion


            #region "验证码相关设置"
            UpdateModuleSetting("PowerForms_Recaptcha_v3_Enable", cbCaptchaEnable.Checked.ToString());
            UpdateModuleSetting("PowerForms_Recaptcha_v3_SiteKey", txtCaptchaSiteKey.Text);
            UpdateModuleSetting("PowerForms_Recaptcha_v3_SecretKey", txtCaptchaSecretKey.Text);
            UpdateModuleSetting("PowerForms_Recaptcha_v3_Social", txtLimitSocial.Text);

            //UpdateModuleSetting("PowerForms_Recaptcha_Tabindex", txtCaptchaTabindex.Text);
            //UpdateModuleSetting("PowerForms_Recaptcha_Theme", ddlCaptchaTheme.Items[ddlCaptchaTheme.SelectedIndex].Value);
            //UpdateModuleSetting("PowerForms_Recaptcha_Type", ddlCaptchaType.Items[ddlCaptchaType.SelectedIndex].Value);
            //UpdateModuleSetting("PowerForms_Recaptcha_Size", ddlCaptchaSize.Items[ddlCaptchaSize.SelectedIndex].Value);

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

                Response.Redirect(xUrl("Anti-Spam"), true);
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