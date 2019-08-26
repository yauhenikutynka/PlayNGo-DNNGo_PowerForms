using System;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Common;
 

using DotNetNuke.Services.Localization;
using DotNetNuke.Entities.Host;

namespace DNNGo.Modules.PowerForms
{
    public partial class Settings : basePortalModule
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
            //WebHelper.BindList(rbleRedirectType, typeof(EnumRedirectType));
            //WebHelper.SelectedListByValue(rbleRedirectType, Settings["PowerForms_RedirectType"] != null && !string.IsNullOrEmpty(Settings["PowerForms_RedirectType"].ToString()) ? Convert.ToInt32(Settings["PowerForms_RedirectType"]) : (Int32)EnumRedirectType.Results);

            txtPageSize.Text = Settings["PowerForms_PageSize"] != null && !string.IsNullOrEmpty(Settings["PowerForms_PageSize"].ToString()) ? Convert.ToString(Settings["PowerForms_PageSize"]) : "10";
            cbExtraTracking.Checked = Settings["PowerForms_ExtraTracking"] != null && !string.IsNullOrEmpty(Settings["PowerForms_ExtraTracking"].ToString()) ? Convert.ToBoolean(Settings["PowerForms_ExtraTracking"]) : false;

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

           
            #endregion


       


            dgPermissions.InheritViewPermissionsFromTab = Module.InheritViewPermissions;
            dgPermissions.TabId = PortalSettings.ActiveTab.TabID;
            dgPermissions.ModuleID = ModuleId;

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
            UpdateModuleSetting("PowerForms_ExtraTracking",cbExtraTracking.Checked.ToString());
            //UpdateModuleSetting("PowerForms_RedirectType", rbleRedirectType.Items[rbleRedirectType.SelectedIndex].Value);


            String Url = ucRedirectPage.Url;
            if (ucRedirectPage.UrlType == "T")
            {
                Url = String.Format("TabID={0}", ucRedirectPage.Url);
            }else if (ucRedirectPage.UrlType == "F")
            {
                Url = String.Format("s{0}", ucRedirectPage.Url);
            }

            UpdateModuleSetting("PowerForms_RedirectPage", Url);
            #endregion

 

            //设置作者权限
            Module.ModulePermissions.Clear();
            Module.ModulePermissions = dgPermissions.Permissions;
            controller.UpdateModule(Module);

           

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

                Response.Redirect(EditUrl("", "", "Manager", "Token=Settings"), true);
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

                Response.Redirect(EditUrl("", "", "Manager"), true);
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        #endregion









    }
}