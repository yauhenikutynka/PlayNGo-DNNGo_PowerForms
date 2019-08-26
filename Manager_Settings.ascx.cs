using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using System.IO;

namespace DNNGo.Modules.PowerForms
{
    public partial class Manager_Settings : basePortalModule
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


            #region "基本设置"
            WebHelper.BindList(rblExportExtension, typeof(EnumExport));
            WebHelper.SelectedListByValue(rblExportExtension, Settings["PowerForms_ExportExtension"] != null && !string.IsNullOrEmpty(Settings["PowerForms_ExportExtension"].ToString()) ? Convert.ToInt32(Settings["PowerForms_ExportExtension"]) : (Int32)EnumExport.Excel);

          

            txtPageSize.Text = Settings["PowerForms_PageSize"] != null && !string.IsNullOrEmpty(Settings["PowerForms_PageSize"].ToString()) ? Convert.ToString(Settings["PowerForms_PageSize"]) : "10";
            txtDatePattern.Text = Settings["PowerForms_DatePattern"] != null && !string.IsNullOrEmpty(Settings["PowerForms_DatePattern"].ToString()) ? Convert.ToString(Settings["PowerForms_DatePattern"]) : "mm/dd/yyyy";
            cbExtraTracking.Checked = Settings["PowerForms_ExtraTracking"] != null && !string.IsNullOrEmpty(Settings["PowerForms_ExtraTracking"].ToString()) ? Convert.ToBoolean(Settings["PowerForms_ExtraTracking"]) : false;
            cbExportExtraTracking.Checked = Settings["PowerForms_ExportExtraTracking"] != null && !string.IsNullOrEmpty(Settings["PowerForms_ExportExtraTracking"].ToString()) ? Convert.ToBoolean(Settings["PowerForms_ExportExtraTracking"]) : false;
            cbHideIp.Checked = Settings["PowerForms_HideIp"] != null && !string.IsNullOrEmpty(Settings["PowerForms_HideIp"].ToString()) ? Convert.ToBoolean(Settings["PowerForms_HideIp"]) : false;
            cbSaveRecords.Checked = Settings["PowerForms_SaveRecords"] != null && !string.IsNullOrEmpty(Settings["PowerForms_SaveRecords"].ToString()) ? Convert.ToBoolean(Settings["PowerForms_SaveRecords"]) : true;

            txtMaxFileSize.Text = Settings["PowerForms_MaxFileSize"] != null  ? Convert.ToString(Settings["PowerForms_MaxFileSize"]) :"10240";

            txtFormVersion.Text = Settings["PowerForms_FormVersion"] != null ? Convert.ToString(Settings["PowerForms_FormVersion"]) : "";
            cbLoginUserDisplay.Checked = Settings["PowerForms_LoginUserDisplay"] != null && !string.IsNullOrEmpty(Settings["PowerForms_LoginUserDisplay"].ToString()) ? Convert.ToBoolean(Settings["PowerForms_LoginUserDisplay"]) : false;
            cbLoginUserDownload.Checked = Settings["PowerForms_LoginUserDownload"] != null && !string.IsNullOrEmpty(Settings["PowerForms_LoginUserDownload"].ToString()) ? Convert.ToBoolean(Settings["PowerForms_LoginUserDownload"]) : false;

            cbFormStorage.Checked = Settings["PowerForms_FormStorage"] != null && !string.IsNullOrEmpty(Settings["PowerForms_FormStorage"].ToString()) ? Convert.ToBoolean(Settings["PowerForms_FormStorage"]) : false;

            txtAnchorLink.Text = Settings["PowerForms_RedirectPageAnchorLink"] != null && !string.IsNullOrEmpty(Settings["PowerForms_RedirectPageAnchorLink"].ToString()) ? Convert.ToString(Settings["PowerForms_RedirectPageAnchorLink"]) : "";

            txtPromptNotLogged.Text = Settings["PowerForms_PromptNotLogged"] != null ? Convert.ToString(Settings["PowerForms_PromptNotLogged"]) : "You need to log in to submit forms.";
            txtPromptAlreadySubmitted.Text = Settings["PowerForms_PromptAlreadySubmitted"] != null ? Convert.ToString(Settings["PowerForms_PromptAlreadySubmitted"]) : "You have already submitted the form.";


            String Url = Settings["PowerForms_RedirectPage"] != null && !string.IsNullOrEmpty(Settings["PowerForms_RedirectPage"].ToString()) ? Convert.ToString(Settings["PowerForms_RedirectPage"]) : String.Empty;
            ucRedirectPage.UrlType = ViewUrlType(Url, ucRedirectPage.UrlType);
            ucRedirectPage.Url = Url;
            if (ucRedirectPage.UrlType == "T")
            {
                ucRedirectPage.Url = Url.Replace("TabID=", "");
            }
            else if (ucRedirectPage.UrlType == "F")
            {
                ucRedirectPage.Url = Url.Replace("sFileID=", "FileID=");
            }


            String ReturnUrl = Settings["PowerForms_ReturnUrl"] != null && !string.IsNullOrEmpty(Settings["PowerForms_ReturnUrl"].ToString()) ? Convert.ToString(Settings["PowerForms_ReturnUrl"]) : String.Empty;
            ucReturnUrl.UrlType = ViewUrlType(ReturnUrl, ucReturnUrl.UrlType);
            ucReturnUrl.Url = ReturnUrl;
            if (ucReturnUrl.UrlType == "T")
            {
                ucReturnUrl.Url = ReturnUrl.Replace("TabID=", "");
            }
            else if (ucReturnUrl.UrlType == "F")
            {
                ucReturnUrl.Url = ReturnUrl.Replace("sFileID=", "FileID=");
            }


            #endregion


            #region "定时清除历史记录"
            cbScheduleEnable.Checked = Settings["PowerForms_Cleanup_Enable"] != null ? Convert.ToBoolean(Settings["PowerForms_Cleanup_Enable"]) : false;
            txtDaysBefore.Text = Settings["PowerForms_Cleanup_DaysBefore"] != null ? Convert.ToString(Settings["PowerForms_Cleanup_DaysBefore"]) : "30";
            txtMaxFeedback.Text = Settings["PowerForms_Cleanup_MaxFeedback"] != null  ? Convert.ToString(Settings["PowerForms_Cleanup_MaxFeedback"]) : "1000";
            if (cbScheduleEnable.Checked)//开启或创建调度器
            {
                CleanupScheduler ClearS = new CleanupScheduler();
                ClearS.UpdateScheduler(this);
            }

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
            #region "基本设置"
 


            UpdateModuleSetting("PowerForms_PageSize", txtPageSize.Text.Trim());
            UpdateModuleSetting("PowerForms_DatePattern", txtDatePattern.Text.Trim());
            UpdateModuleSetting("PowerForms_ExtraTracking", cbExtraTracking.Checked.ToString());
            UpdateModuleSetting("PowerForms_ExportExtraTracking", cbExportExtraTracking.Checked.ToString());
            UpdateModuleSetting("PowerForms_SaveRecords", cbSaveRecords.Checked.ToString());
            UpdateModuleSetting("PowerForms_HideIp", cbHideIp.Checked.ToString());
            UpdateModuleSetting("PowerForms_ExportExtension", rblExportExtension.Items[rblExportExtension.SelectedIndex].Value);
          

            UpdateModuleSetting("PowerForms_MaxFileSize", txtMaxFileSize.Text.Trim());


            UpdateModuleSetting("PowerForms_FormVersion", txtFormVersion.Text.Trim());
            UpdateModuleSetting("PowerForms_LoginUserDisplay", cbLoginUserDisplay.Checked.ToString());
            UpdateModuleSetting("PowerForms_LoginUserDownload", cbLoginUserDownload.Checked.ToString());
            UpdateModuleSetting("PowerForms_FormStorage", cbFormStorage.Checked.ToString());

            UpdateModuleSetting("PowerForms_RedirectPageAnchorLink", txtAnchorLink.Text.Trim());

 
            UpdateModuleSetting("PowerForms_PromptNotLogged", txtPromptNotLogged.Text.Trim());
            UpdateModuleSetting("PowerForms_PromptAlreadySubmitted", txtPromptAlreadySubmitted.Text.Trim());

            String Url = ucRedirectPage.Url;
            if (ucRedirectPage.UrlType == "T")
            {
                Url = String.Format("TabID={0}", ucRedirectPage.Url);
            }
            else if (ucRedirectPage.UrlType == "F")
            {
                Url = String.Format("s{0}", ucRedirectPage.Url);
            }

            UpdateModuleSetting("PowerForms_RedirectPage", Url);


            String ReturnUrl = ucReturnUrl.Url;
            if (ucReturnUrl.UrlType == "T")
            {
                ReturnUrl = String.Format("TabID={0}", ucReturnUrl.Url);
            }
            else if (ucRedirectPage.UrlType == "F")
            {
                ReturnUrl = String.Format("s{0}", ucReturnUrl.Url);
            }

            UpdateModuleSetting("PowerForms_ReturnUrl", ReturnUrl);

            #endregion


            #region "定时清除历史记录"
            UpdateModuleSetting("PowerForms_Cleanup_Enable", cbScheduleEnable.Checked.ToString());
            UpdateModuleSetting("PowerForms_Cleanup_DaysBefore", txtDaysBefore.Text.Trim());
            UpdateModuleSetting("PowerForms_Cleanup_MaxFeedback", txtMaxFeedback.Text.Trim());

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

                Response.Redirect(xUrl("Settings"), true);
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


        /// <summary>
        /// 清除临时文件
        /// </summary>
        protected void cmdClearfiles_Click(object sender, EventArgs e)
        {
            try
            {
                String FileWebPath = String.Format("{0}PowerForms/Multiplefiles/", PortalSettings.HomeDirectory);
                String SaveFilePath = MapPath(FileWebPath);
                DirectoryInfo dir = new DirectoryInfo(SaveFilePath);
                if (dir.Exists)
                {
                    DeleteFolder(SaveFilePath);
                }
 
                mTips.LoadMessage("ClearfilesSuccess", EnumTips.Success, this, new String[] { "" });

                Response.Redirect(xUrl("Settings"), true);
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="path"></param>
        public  void DeleteFolder(string path)
        {
            string[] strTemp;

            //先删除该目录下的文件
            strTemp = System.IO.Directory.GetFiles(path);
            foreach (string str in strTemp)
            {
                System.IO.File.Delete(str);
            }
            //删除子目录，递归
            strTemp = System.IO.Directory.GetDirectories(path);
            foreach (string str in strTemp)
            {
                DeleteFolder(str);
            }
            //删除该目录
            System.IO.Directory.Delete(path);
        }

        #endregion









    }
}