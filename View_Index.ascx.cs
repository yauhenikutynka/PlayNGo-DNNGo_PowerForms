using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Localization;
using DotNetNuke.Entities.Modules;

using DotNetNuke.Common;

 

namespace DNNGo.Modules.PowerForms
{
    public partial class View_Index : basePortalModule, DotNetNuke.Entities.Modules.IActionable
    {
        #region "属性"




        /// <summary>
        /// 检测是否安装过默认字段
        /// </summary>
        public Boolean InstallationDefault
        {
            get { return Settings["PowerForms_InstallationDefault"] != null && !string.IsNullOrEmpty(Settings["PowerForms_InstallationDefault"].ToString()) ? Convert.ToBoolean(Settings["PowerForms_InstallationDefault"]) : false; }
        }

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


        #region "事件"

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                //加载脚本
                LoadViewScript();

                if (!IsPostBack)
                {
                    //这里是网页跟踪的两个参数，不需要理会
                    String _OriginalReferrer = Tracking_OriginalReferrer;
                    String _LandingPage = Tracking_LandingPage;



                   
                    //加载Debug信息
                    LoadDebug();
                }
 
               

            }
            catch (Exception exc) //Module failed to load
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, exc);
            }
        }


        protected void Page_Init(System.Object sender, System.EventArgs e)
        {
            try
            {





                if (!IsPostBack)
                {
                    //检测是否安装过默认字段 显示名,邮箱，消息
                    if (!InstallationDefault)//没有安装过进入
                    {
                        UpdateModuleSetting("PowerForms_InstallationDefault", "true");

                        //并且里面不存在任何字段的
                        if (DNNGo_PowerForms_Field.FindCount(DNNGo_PowerForms_Field._.ModuleId, ModuleId) == 0)
                        {

                            DNNGo_PowerForms_Field.InstallField("Name", this);
                            DNNGo_PowerForms_Field.InstallField("Email", this);
                            DNNGo_PowerForms_Field.InstallField("Messages", this);
                            Response.Redirect(Globals.NavigateURL());
                        }
                    }

                }


             
                    //绑定容器内的子控件
                    BindContainer();

               


            }
            catch (Exception exc) //Module failed to load
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, exc);
            }
        }


        #endregion

        #region "方法"

 

        /// <summary>
        /// 绑定列表数据到容器
        /// </summary>
        private void BindContainer()
        {
            //加载相应的控件
            basePortalModule ManageContent = new basePortalModule();
            String ModuleSrc = String.Empty;
            if (Token.IndexOf(String.Format("Result{0}",ModuleId), StringComparison.CurrentCultureIgnoreCase) >= 0)
                ModuleSrc = "View_Result.ascx";
            else
                ModuleSrc = "View_Form.ascx";
 
            ManageContent.ID = Token;
            String ContentSrc = ResolveClientUrl(string.Format("{0}/{1}", this.TemplateSourceDirectory, ModuleSrc));
            ManageContent = (basePortalModule)LoadControl(ContentSrc);
            ManageContent.ModuleConfiguration = this.ModuleConfiguration;
            ManageContent.LocalResourceFile = Localization.GetResourceFile(this, string.Format("{0}.resx", ModuleSrc));
            phContainer.Controls.Add(ManageContent);
        }






        #endregion

  

        #region Optional Interfaces
        public ModuleActionCollection ModuleActions
        {
            get
            {
                ModuleActionCollection Actions = new ModuleActionCollection();

                Actions.Add(this.GetNextActionID(), Localization.GetString("NewField", this.LocalResourceFile), ModuleActionType.AddContent, "", "settings.gif", xUrl("AddNewField"), false, SecurityAccessLevel.Admin, true, false);
                Actions.Add(this.GetNextActionID(), Localization.GetString("Manager", this.LocalResourceFile), ModuleActionType.AddContent, "", "settings.gif", xUrl("History"), false, SecurityAccessLevel.Edit, true, false);
                Actions.Add(this.GetNextActionID(), Localization.GetString("Options", this.LocalResourceFile), ModuleActionType.AddContent, "", "settings.gif", xUrl("EffectOptions"), false, SecurityAccessLevel.Admin, true, false);

            

                return Actions;
            }
        }
        #endregion
    }
}