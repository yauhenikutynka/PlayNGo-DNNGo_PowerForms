using System;
using System.Collections.Generic;
using System.Web;
using DotNetNuke.Entities.Users;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Host;

using DotNetNuke.Entities.Tabs;
using System.IO;
using DotNetNuke.Services.Localization;
using System.Collections;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Common;
using DotNetNuke.Security.Permissions;
using DotNetNuke.Security;
using DotNetNuke.UI.Skins;
using System.Web.UI.WebControls;
using DotNetNuke.Web.Client.ClientResourceManagement;
using DotNetNuke.Framework;
using DotNetNuke.Entities.Controllers;

namespace DNNGo.Modules.PowerForms
{
    public class BasePage : DotNetNuke.Framework.PageBase
    {


        #region "获取DNN对象"

        /// <summary>
        /// 模块编号
        /// </summary>
        public Int32 ModuleId = WebHelper.GetIntParam(HttpContext.Current.Request, "ModuleId", 0);

        public Int32 PortalId = WebHelper.GetIntParam(HttpContext.Current.Request, "PortalId", 0);
        public Int32 TabId = WebHelper.GetIntParam(HttpContext.Current.Request, "TabId", 0);


        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo UserInfo
        {
            get { return UserController.GetCurrentUserInfo(); }
        }
        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserId
        {
            get
            {
                if (HttpContext.Current.Request.IsAuthenticated)
                {
                    return UserInfo.UserID;
                }
                else
                {
                    return Null.NullInteger;
                }
            }
        }



        private PortalSettings _portalSettings;
        /// <summary>
        /// 站点设置
        /// </summary>
        public PortalSettings PortalSettings
        {
            get
            {
                if (!(_portalSettings != null && _portalSettings.PortalId != Null.NullInteger))
                {
                    PortalAliasInfo objPortalAliasInfo = new PortalAliasInfo();
                    objPortalAliasInfo.PortalID = PortalId;
                    _portalSettings = new PortalSettings(TabId, objPortalAliasInfo);
                }
                return _portalSettings;
            }
        }



        private TabInfo _tabInfo;
        /// <summary>
        /// 页面信息
        /// </summary>
        public TabInfo TabInfo
        {
            get
            {
                if (!(_tabInfo != null && _tabInfo.TabID > 0) && TabId > 0)
                {
                    TabController tc = new TabController();
                    _tabInfo = tc.GetTab(TabId);

                }

                return _tabInfo;


            }
        }


        private ModuleInfo _ModuleConfiguration = new ModuleInfo();
        /// <summary>
        /// 模块信息
        /// </summary>
        public ModuleInfo ModuleConfiguration
        {
            get
            {
                if (!(_ModuleConfiguration != null && _ModuleConfiguration.ModuleID > 0) && ModuleId > 0)
                {
                    ModuleController mc = new ModuleController();
                    _ModuleConfiguration = mc.GetModule(ModuleId, TabId);

                }
                return _ModuleConfiguration;
            }
        }


        private String _BaseModuleName = String.Empty;
        /// <summary>
        /// 基础模块名
        /// </summary>
        public String BaseModuleName
        {
            get
            {
                if (String.IsNullOrEmpty(_BaseModuleName))
                {
                    _BaseModuleName = ModuleProperty("ModuleName");
                }
                return _BaseModuleName;
            }
            set { _BaseModuleName = value; }
        }



        /// <summary>
        /// 模块地址
        /// </summary>
        public string ModulePath
        {
            get { return this.TemplateSourceDirectory + "/"; }
        }

        public String QueryString
        {
            get { return String.Format("{0}&ModulePath={1}", WebHelper.GetScriptNameQueryString, HttpUtility.UrlEncode(ModulePath)); }
        }


        private Hashtable _settings = new Hashtable();
        /// <summary>
        /// 模块设置
        /// </summary>
        public Hashtable Settings
        {
            get
            {
                ModuleController controller = new ModuleController();
                if (!(_settings != null && _settings.Count > 0))
                {
                    _settings = new Hashtable(controller.GetModuleSettings(ModuleId));
                }
                return _settings;
            }
        }






        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public Boolean IsAdministrator
        {
            get { return UserInfo.IsSuperUser || UserInfo.IsInRole("Administrators"); }
        }

        /// <summary>
        /// 管理员锁
        /// (检索目录下有无admindisplay.lock)
        /// </summary>
        public Boolean AdministratorLock
        {
            get { return File.Exists(MapPath(String.Format("{0}admindisplay.lock", ModulePath))); }
        }
        /// <summary>
        /// 显示管理员选项
        /// </summary>
        public Boolean DisplayAdminOption
        {
            get
            {
                Boolean display = true;
                if (AdministratorLock && !IsAdministrator)
                {
                    display = false;
                }
                return display;
            }
        }




        /// <summary>
        /// 语言
        /// </summary>
        public String language
        {
            get { return WebHelper.GetStringParam(Request, "language", PortalSettings.DefaultLanguage); }
        }



        /// <summary>
        /// 验证登陆状态(没有登陆跳转到登陆页面)
        /// </summary>
        public void VerificationLogin()
        {
            //没有登陆的用户
            if (!(UserId > 0))
            {
                Response.Redirect(Globals.NavigateURL(PortalSettings.LoginTabId, "Login", "returnurl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl)),false);

            }
        }

        /// <summary>
        /// 验证作者状态(不是作者跳转到登陆页面)
        /// </summary>
        public void VerificationAuthor()
        {
            //没有登陆的用户
            if (!(UserId > 0))
            {
                Response.Redirect(Globals.NavigateURL(PortalSettings.LoginTabId, "Login", "returnurl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl)),true);
            }
            else if (!ModulePermissionController.HasModuleAccess(SecurityAccessLevel.Edit, "CONTENT", ModuleConfiguration))
            {
                Response.Redirect(Globals.NavigateURL(TabId),true);
            }




        }

        #endregion

        #region "新的后台URL"

        /// <summary>
        /// URL转换默认名
        /// </summary>
        /// <returns></returns>
        public String xUrlToken()
        {
            return "ManagerList";
        }


        public string xUrl()
        {
            return xUrl("", "", xUrlToken());
        }
        public string xUrl(string ControlKey)
        {
            return xUrl("", "", ControlKey);
        }
        public string xUrl(string KeyName, string KeyValue)
        {
            return xUrl(KeyName, KeyValue, xUrlToken());
        }
        public string xUrl(string KeyName, string KeyValue, string ControlKey)
        {
            string[] parameters = new string[] { };
            return xUrl(KeyName, KeyValue, ControlKey, parameters);
        }
        public string xUrl(string KeyName, string KeyValue, string ControlKey, params string[] AddParameters)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            String language = WebHelper.GetStringParam(Request, "language", PortalSettings.DefaultLanguage);

            sb.AppendFormat("{0}Index_Manager.aspx?PortalId={1}&TabId={2}&ModuleId={3}&language={4}", ModulePath, PortalId, TabId, ModuleId, language);

            string key = ControlKey;
            if (string.IsNullOrEmpty(key))
            {
                sb.AppendFormat("&Token={0}", xUrlToken());
            }
            else
            {
                sb.AppendFormat("&Token={0}", key);
            }
            if (!string.IsNullOrEmpty(KeyName) && !string.IsNullOrEmpty(KeyValue))
            {
                sb.AppendFormat("&{0}={1}", KeyName, KeyValue);
            }

            if (AddParameters != null && AddParameters.Length > 0)
            {
                foreach (String parameter in AddParameters)
                {
                    sb.AppendFormat("&{0}", parameter);
                }
            }
            return sb.ToString();

        }





        #endregion


        #region "基础配置属性"




        /// <summary>
        /// 获取绑定的效果名称
        /// </summary>
        public String Settings_EffectName
        {
            get { return Settings["PowerForms_EffectName"] != null ? Convert.ToString(Settings["PowerForms_EffectName"]) : GetDefaultEffectName(); }
        }


        /// <summary>
        /// 获取绑定的效果主题名称
        /// </summary>
        public String Settings_EffectThemeName
        {
            get { return Settings["PowerForms_EffectThemeName"] != null ? Convert.ToString(Settings["PowerForms_EffectThemeName"]) : GetDefaultThemeName(); }
        }

        public Boolean designMode
        {
            get { return DesignMode; }
        }



        private EffectDB _Setting_EffectDB = new EffectDB();
        /// <summary>
        /// 获取绑定效果内容
        /// </summary>
        public EffectDB Setting_EffectDB
        {
            get
            {
                if (!(_Setting_EffectDB != null && !String.IsNullOrEmpty(_Setting_EffectDB.Name)))
                {
                    String EffectDBPath = Server.MapPath(String.Format("{0}Effects/{1}/EffectDB.xml", ModulePath, Settings_EffectName));
                    if (File.Exists(EffectDBPath))
                    {
                        XmlFormat xf = new XmlFormat(EffectDBPath);
                        _Setting_EffectDB = xf.ToItem<EffectDB>();
                    }
                }
                return _Setting_EffectDB;
            }
        }


        private List<SettingEntity> _Setting_EffectSettingDB = new List<SettingEntity>();
        /// <summary>
        /// 获取绑定效果设置项
        /// </summary>
        public List<SettingEntity> Setting_EffectSettingDB
        {
            get
            {
                if (!(_Setting_EffectSettingDB != null && _Setting_EffectSettingDB.Count > 0))
                {
                    String EffectSettingDBPath = Server.MapPath(String.Format("{0}Effects/{1}/EffectSetting.xml", ModulePath, Settings_EffectName));
                    if (File.Exists(EffectSettingDBPath))
                    {
                        XmlFormat xf = new XmlFormat(EffectSettingDBPath);
                        _Setting_EffectSettingDB = xf.ToList<SettingEntity>();
                    }
                }
                return _Setting_EffectSettingDB;
            }
        }

        //public List<SettingEntity> GetSetting_EffectSettingDB(String )




        private List<SettingEntity> _Setting_ItemSettingDB = new List<SettingEntity>();
        /// <summary>
        /// 获取绑定数据设置项(非效果)
        /// </summary>
        public List<SettingEntity> Setting_ItemSettingDB
        {
            get
            {
                if (!(_Setting_ItemSettingDB != null && _Setting_ItemSettingDB.Count > 0))
                {
                    String ItemSettingDBPath = Server.MapPath(String.Format("{0}Effects/{1}/ItemSetting.xml", ModulePath, Settings_EffectName));
                    if (File.Exists(ItemSettingDBPath))
                    {
                        XmlFormat xf = new XmlFormat(ItemSettingDBPath);
                        _Setting_ItemSettingDB = xf.ToList<SettingEntity>();
                    }
                }
                return _Setting_ItemSettingDB;
            }
        }



        private string _CrmVersion = String.Empty;
        /// <summary>
        /// 引用文件版本
        /// </summary>
        public string CrmVersion
        {
            get
            {
                if (string.IsNullOrEmpty(_CrmVersion))
                {
                    var ModuleVersion = ModuleProperty("Version");
                    string setting = GetHostSetting("CrmVersion");
                    if (!string.IsNullOrEmpty(setting))
                    {
                        _CrmVersion = String.Format("{0}.{1}", ModuleVersion, setting);
                    }
                }
                return _CrmVersion;
            }
        }

        private string GetHostSetting(string key, string defaultValue = "")
        {
            return HostController.Instance.GetString(key, defaultValue); ;
        }






        /// <summary>
        /// 获取绑定的自定义效果名称
        /// </summary>
        public String Settings_CustomEffectName
        {
            get { return Settings["PowerForms_CustomEffectName"] != null ? Convert.ToString(Settings["PowerForms_CustomEffectName"]) : GetCustomDefaultEffectName(); }
        }


        /// <summary>
        /// 获取绑定的自定义效果主题名称
        /// </summary>
        public String Settings_CustomEffectThemeName
        {
            get { return Settings["PowerForms_CustomEffectThemeName"] != null ? Convert.ToString(Settings["PowerForms_CustomEffectThemeName"]) : GetCustomDefaultThemeName(); }
        }


        /// <summary>
        /// 获取绑定的结果名称
        /// </summary>
        public String Settings_ResultName
        {
            get { return Settings["PowerForms_ResultName"] != null ? Convert.ToString(Settings["PowerForms_ResultName"]) : GetDefaultResultName(); }
        }


        /// <summary>
        /// 获取绑定的结果主题名称
        /// </summary>
        public String Settings_ResultThemeName
        {
            get { return Settings["PowerForms_ResultThemeName"] != null ? Convert.ToString(Settings["PowerForms_ResultThemeName"]) : GetDefaultResultThemeName(); }
        }



        #endregion




        #region "设置默认的效果与主题"

        /// <summary>
        /// 获取默认的效果名
        /// </summary>
        /// <returns></returns>
        public String GetDefaultEffectName()
        {
            //构造效果存放路径
            String EffectDirPath = String.Format("{0}Effects/", Server.MapPath(ModulePath));
            DirectoryInfo EffectDir = new DirectoryInfo(EffectDirPath);
            if (!EffectDir.Exists) EffectDir.Create();//不存在就创建
            //获取当前所有的目录
            DirectoryInfo[] EffectDirs = EffectDir.GetDirectories();

            if (EffectDirs != null && EffectDirs.Length > 0)
            {
                UpdateModuleSetting("PowerForms_EffectName", EffectDirs[0].Name);
                return EffectDirs[0].Name;
            }

            return String.Empty;

        }

        /// <summary>
        /// 获取默认的主题名
        /// </summary>
        /// <returns></returns>
        public String GetDefaultThemeName()
        {

            //绑定效果的主题
            String EffectDirPath = String.Format("{0}Effects/{1}/Themes/", Server.MapPath(ModulePath), Settings_EffectName);
            DirectoryInfo EffectDir = new DirectoryInfo(EffectDirPath);
            if (!EffectDir.Exists) EffectDir.Create();
            DirectoryInfo[] ThemeDirs = EffectDir.GetDirectories();
            if (ThemeDirs != null && ThemeDirs.Length > 0)
            {
                UpdateModuleSetting("PowerForms_EffectThemeName", ThemeDirs[0].Name);
                return ThemeDirs[0].Name;
            }

            return String.Empty;
        }



        /// <summary>
        /// 获取默认的效果名
        /// </summary>
        /// <returns></returns>
        public String GetCustomDefaultEffectName()
        {
            //构造效果存放路径
            String EffectDirPath = String.Format("{0}CustomEffects/", Server.MapPath(ModulePath));
            DirectoryInfo EffectDir = new DirectoryInfo(EffectDirPath);
            if (!EffectDir.Exists) EffectDir.Create();//不存在就创建
            //获取当前所有的目录
            DirectoryInfo[] EffectDirs = EffectDir.GetDirectories();

            if (EffectDirs != null && EffectDirs.Length > 0)
            {
                UpdateModuleSetting("PowerForms_CustomEffectName", EffectDirs[0].Name);
                return EffectDirs[0].Name;
            }

            return String.Empty;

        }

        /// <summary>
        /// 获取默认的主题名
        /// </summary>
        /// <returns></returns>
        public String GetCustomDefaultThemeName()
        {

            //绑定效果的主题
            String EffectDirPath = String.Format("{0}CustomEffects/{1}/Themes/", Server.MapPath(ModulePath), Settings_CustomEffectName);
            DirectoryInfo EffectDir = new DirectoryInfo(EffectDirPath);
            if (!EffectDir.Exists) EffectDir.Create();
            DirectoryInfo[] ThemeDirs = EffectDir.GetDirectories();
            if (ThemeDirs != null && ThemeDirs.Length > 0)
            {
                UpdateModuleSetting("PowerForms_CustomEffectThemeName", ThemeDirs[0].Name);
                return ThemeDirs[0].Name;
            }

            return String.Empty;
        }



        /// <summary>
        /// 获取默认的效果名
        /// </summary>
        /// <returns></returns>
        public String GetDefaultResultName()
        {
            //构造效果存放路径
            String EffectDirPath = String.Format("{0}Results/", Server.MapPath(ModulePath));
            DirectoryInfo EffectDir = new DirectoryInfo(EffectDirPath);
            if (!EffectDir.Exists) EffectDir.Create();//不存在就创建
            //获取当前所有的目录
            DirectoryInfo[] EffectDirs = EffectDir.GetDirectories();

            if (EffectDirs != null && EffectDirs.Length > 0)
            {
                UpdateModuleSetting("PowerForms_ResultName", EffectDirs[0].Name);
                return EffectDirs[0].Name;
            }

            return String.Empty;

        }

        /// <summary>
        /// 获取默认的主题名
        /// </summary>
        /// <returns></returns>
        public String GetDefaultResultThemeName()
        {

            //绑定效果的主题
            String EffectDirPath = String.Format("{0}Results/{1}/Themes/", Server.MapPath(ModulePath), Settings_ResultName);
            DirectoryInfo EffectDir = new DirectoryInfo(EffectDirPath);
            if (!EffectDir.Exists) EffectDir.Create();
            DirectoryInfo[] ThemeDirs = EffectDir.GetDirectories();
            if (ThemeDirs != null && ThemeDirs.Length > 0)
            {
                UpdateModuleSetting("PowerForms_ResultThemeName", ThemeDirs[0].Name);
                return ThemeDirs[0].Name;
            }

            return String.Empty;
        }



        #endregion

        #region "更新模块设置"


        /// <summary>
        /// 更新当前模块的设置
        /// </summary>
        /// <param name="SettingName"></param>
        /// <param name="SettingValue"></param>
        public void UpdateModuleSetting(string SettingName, string SettingValue)
        {
            UpdateModuleSetting(ModuleId, SettingName, SettingValue);
        }


        /// <summary>
        /// 更新模块设置
        /// </summary>
        /// <param name="ModuleId"></param>
        /// <param name="SettingName"></param>
        /// <param name="SettingValue"></param>
        public void UpdateModuleSetting(int _ModuleId, string SettingName, string SettingValue)
        {
            ModuleController controller = new ModuleController();

            controller.UpdateModuleSetting(_ModuleId, SettingName, SettingValue);


        }

        /// <summary>
        /// 效果参数保存名称格式化
        /// </summary>
        /// <param name="EffectName">效果名</param>
        /// <param name="ThemeName">主题名</param>
        /// <returns></returns>
        public String EffectSettingsFormat(String EffectName, String ThemeName)
        {
            return String.Format("Gallery{0}_{1}", EffectName, ThemeName);
        }

        #endregion

        #region "绑定页面标题和帮助"

        /// <summary>
        /// 显示标题
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public String ViewTitle(String Title, String DefaultValue)
        {
            return ViewTitle(Title, DefaultValue, "");
        }

        /// <summary>
        /// 显示标题
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewTitle(String Title, String DefaultValue, String ControlName)
        {
            return ViewTitle(Title, DefaultValue, ControlName, "");
        }

        /// <summary>
        /// 显示标题
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public String ViewTitle(String Title, String DefaultValue, String ControlName, String ClassName)
        {
            String Content = ViewResourceText(Title, DefaultValue);
            return ViewSpan(Content, ControlName, ClassName);
        }

        /// <summary>
        /// 显示帮助
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public String ViewHelp(String Title, String DefaultValue)
        {
            String Content = ViewResourceText(Title, DefaultValue, "Help");
            return ViewSpan(Content, "", "span_help");
        }

        /// <summary>
        /// 显示内容框
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="ControlName"></param>
        /// <param name="ClassName"></param>
        /// <returns></returns>
        public String ViewSpan(String Content, String ControlName, String ClassName)
        {
            if (!String.IsNullOrEmpty(ControlName))
            {
                System.Web.UI.Control c = FindControl(ControlName);
                if (c != null && !String.IsNullOrEmpty(c.ClientID))
                {
                    ControlName = c.ClientID;
                }
                else
                {
                    ControlName = String.Empty;
                }
            }

            return String.Format("<label  {2}><span {1} >{0}</span></label>",
                Content,
                !String.IsNullOrEmpty(ClassName) ? String.Format("class=\"{0}\"", ClassName) : "",
              !String.IsNullOrEmpty(ControlName) ? String.Format("for=\"{0}\"", ControlName) : ""
                );
        }




        /// <summary>
        /// 显示资源文件内容
        /// </summary>
        /// <param name="Title"></param>
        /// <returns></returns>
        public String ViewResourceText(String Title)
        {
            return ViewResourceText(Title, "");
        }

        /// <summary>
        /// 显示资源文件内容
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public String ViewResourceText(String Title, String DefaultValue)
        {
            return ViewResourceText(Title, DefaultValue, "Text");
        }

        /// <summary>
        /// 显示资源文件内容
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <param name="TextType"></param>
        /// <returns></returns>
        public String ViewResourceText(String Title, String DefaultValue, String TextType)
        {
            String _Title = Localization.GetString(String.Format("{0}.{1}", Title, TextType), this.LocalResourceFile);
            if (String.IsNullOrEmpty(_Title))
            {
                _Title = DefaultValue;
            }
            return _Title;
        }




        /// <summary>
        /// 计算页面执行的时间
        /// </summary>
        /// <param name="TimeStart">开始时间</param>
        public String InitTimeSpan(DateTime TimeStart)
        {
            //查询数据库所花的时间
            System.DateTime endTime = xUserTime.UtcTime();
            System.TimeSpan ts = endTime - TimeStart;
            String RunTime = string.Format("{0}秒{1}毫秒", ts.Seconds, ts.Milliseconds);
            TimeStart = endTime = xUserTime.UtcTime();
            return RunTime;
        }






        /// <summary>
        /// 显示字段标题
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="ClassName"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewTitleSpan(String Content, String ClassName, String ControlName)
        {

            return String.Format("<label  {2}><span {1} >{0}</span></label>",
                        Content,
                        !String.IsNullOrEmpty(ClassName) ? String.Format("class=\"{0}\"", ClassName) : "",
                      !String.IsNullOrEmpty(ControlName) ? String.Format("for=\"{0}\"", ControlName) : ""
                        );
        }
        #endregion

        #region "加载样式表"

        /// <summary>
        /// 绑定样式表文件
        /// </summary>
        /// <param name="Name"></param>
        public void BindStyleFile(String Name, String FileName)
        {
            BindStyleFile(Name, FileName, 50);
        }

        /// <summary>
        /// 绑定样式表文件
        /// </summary>
        /// <param name="Name"></param>
        public void BindStyleFile(String Name, String FileName, int priority)
        {
            if (HttpContext.Current.Items[Name] == null)
            {
                HttpContext.Current.Items.Add(Name, "true");

                String PageUrl = WebHelper.GetScriptFileName;
                if (!String.IsNullOrEmpty(PageUrl) && PageUrl.IndexOf("View_iFrame.aspx", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    System.Web.UI.Control objCSS = this.Page.FindControl("CSS");
                    if ((objCSS != null))
                    {
                        Literal litLink = new Literal();
                        litLink.Text = String.Format("<link  rel=\"stylesheet\" type=\"text/css\" href=\"{0}?cdv={1}\" />", FileName, CrmVersion);
                        objCSS.Controls.Add(litLink);

                    }
                }
                else
                {
                    ClientResourceManager.RegisterStyleSheet(Page, FileName);
                }
            }
        }

        /// <summary>
        /// 绑定脚本文件
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="FileName"></param>
        public void BindJavaScriptFile(String Name, String FileName)
        {
            BindJavaScriptFile(Name, FileName, 50);
        }

        /// <summary>
        /// 绑定脚本文件
        /// </summary>
        /// <param name="ThemeName"></param>
        public void BindJavaScriptFile(String Name, String FileName, int priority)
        {
            if (HttpContext.Current.Items[Name] == null)
            {
                HttpContext.Current.Items.Add(Name, "true");

                String PageUrl = WebHelper.GetScriptFileName;
                if (!String.IsNullOrEmpty(PageUrl) && PageUrl.IndexOf("View_iFrame.aspx", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    if (FileName.IndexOf("?") >= 0)
                    {
                        Page.ClientScript.RegisterClientScriptInclude(Name, FileName);
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptInclude(Name, ResolveUrl(String.Format("{0}?cdv={1}", FileName, CrmVersion)));
                    }


                }
                else
                {
                    ClientResourceManager.RegisterScript(Page, FileName, priority);
                }
            }
        }


        #endregion

        #region "加载界面脚本样式表"

        /// <summary>
        /// 加载系统的jquery
        /// </summary>
        public void LoadSystemJQuery(System.Web.UI.Control objCSS)
        {


            string ContentSrc = ResolveUrl("~/admin/Skins/jQuery.ascx");
            if (File.Exists(Server.MapPath(ContentSrc)))
            {
                SkinObjectBase ManageContent = new SkinObjectBase();
                ManageContent = (SkinObjectBase)LoadControl(ContentSrc);
                //ManageContent.ModuleControl = this.ModuleConfiguration.;
                objCSS.Controls.Add(ManageContent);

            }
        }





        /// <summary>
        /// 加载显示界面脚本样式表
        /// </summary>
        public void LoadViewScript()
        {
            System.Web.UI.Control objCSS = this.Page.FindControl("CSS");
            if ((objCSS != null))
            {
                //LoadScriptForJqueryAndUI(ModulePath);


                LoadSystemJQuery(objCSS);

                BindJavaScriptFile("jquery.validationEngine-en.js", ViewValidationEngineLanguage(), 1);
                BindJavaScriptFile("jquery.validationEngine.js", String.Format("{0}Resource/js/jquery.validationEngine.js", ModulePath), 2);

                BindJavaScriptFile("sisyphus.min.js", String.Format("{0}Resource/js/sisyphus.min.js", ModulePath), 10);

                BindJavaScriptFile("tinymce.min.js", String.Format("{0}Resource/plugins/tinymce/tinymce.min.js", ModulePath), 11);

                BindStyleFile("jquery.datepick.css", String.Format("{0}Resource/plugins/jquery-datepick/jquery.datepick.css", ModulePath), 12);

                BindJavaScriptFile("jquery.plugin.min.js", String.Format("{0}Resource/plugins/jquery-datepick/jquery.plugin.min.js", ModulePath), 13);
                BindJavaScriptFile("jquery.datepick.min.js", String.Format("{0}Resource/plugins/jquery-datepick/jquery.datepick.min.js", ModulePath), 14);
                BindJavaScriptFile("jquery.datepick.lang.js", ViewDatepickLanguage(), 15);




                //载入多文件上传脚本
                //LoadPluploadScript();


                if (ViewSettingT<Boolean>("PowerForms_Recaptcha_v3_Enable", false))
                {
                    BindJavaScriptFile("google.recaptcha", String.Format("https://www.google.com/recaptcha/api.js?&render={0}&hl={1}", ViewSettingT<String>("PowerForms_Recaptcha_v3_SiteKey", ""), language));
                    //if (HttpContext.Current.Items["onloadFormsCallback"] == null)
                    //{
                    //    HttpContext.Current.Items.Add("onloadFormsCallback", "true");

                    //    System.Text.StringBuilder onloadFormsCallback = new System.Text.StringBuilder();
                    //    onloadFormsCallback.Append("<script type=\"text/javascript\">").AppendLine();
                    //    onloadFormsCallback.Append("var onloadformscallback = function () {").AppendLine();
                    //    onloadFormsCallback.Append("$('.g-recaptcha-dnn').each(function (i) {").AppendLine();
                    //    onloadFormsCallback.Append("grecaptcha.render(this.id, {").AppendLine();
                    //    onloadFormsCallback.AppendFormat("hl: '{0}',", language).AppendLine();
                    //    onloadFormsCallback.Append("sitekey: $(this).data(\"sitekey\"),").AppendLine();
                    //    onloadFormsCallback.Append("theme: $(this).data(\"theme\"),").AppendLine();
                    //    onloadFormsCallback.Append("type: $(this).data(\"type\"),").AppendLine();
                    //    onloadFormsCallback.Append("size: $(this).data(\"size\"),").AppendLine();
                    //    onloadFormsCallback.Append("tabindex: $(this).data(\"tabindex\"),").AppendLine();
                    //    onloadFormsCallback.Append("callback: eval($(this).data(\"callback\")),").AppendLine();
                    //    onloadFormsCallback.Append("'expired-callback':eval( $(this).data(\"expired-callback\"))").AppendLine();
                    //    onloadFormsCallback.Append("});").AppendLine();
                    //    onloadFormsCallback.Append("});").AppendLine();




                    //    onloadFormsCallback.Append("};").AppendLine();
                    //    onloadFormsCallback.Append("</script>").AppendLine();
                    //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "onloadFormsCallback", onloadFormsCallback.ToString());
                    //}
                }

            }
        }



        /// <summary>
        /// 载入多文件上传脚本
        /// </summary>
        public void LoadPluploadScript()
        {
            BindStyleFile("jquery.ui.plupload.css", String.Format("{0}Resource/plugins/plupload/jquery.ui.plupload/css/jquery.ui.plupload.css", ModulePath));
            BindJavaScriptFile("plupload.full.min.js", String.Format("{0}Resource/plugins/plupload/plupload.full.min.js", ModulePath), 16);
            BindJavaScriptFile("jquery.ui.plupload.js", String.Format("{0}Resource/plugins/plupload/jquery.ui.plupload/jquery.ui.plupload.js", ModulePath), 17);
            BindJavaScriptFile("jquery.plupload-lang.js", ViewPluploadLanguage(), 18);
        }


        /// <summary>
        /// 获取当前验证引擎语言文件的URL
        /// </summary>
        /// <returns></returns>
        public String ViewValidationEngineLanguage()
        {
            //String VEL = String.Format("{0}Resource/js/jquery.validationEngine-en.js?cdv={1}", ModulePath, CrmVersion);
            String VEL = String.Format("{0}Resource/js/jquery.validationEngine-en.js", ModulePath);
            String language = WebHelper.GetStringParam(Request, "language", PortalSettings.DefaultLanguage).ToLower(); ;
            if (!String.IsNullOrEmpty(language) && language != "en-us")
            {
                //先判断这个语言文件是否存在
                String webJS = String.Format("{0}Resource/plugins/validation/jquery.validationEngine-{1}.js", ModulePath, language);
                String serverJS = MapPath(webJS);
                if (File.Exists(serverJS))
                {
                    //VEL = String.Format("{0}?cdv={1}", webJS, CrmVersion);
                    VEL = webJS;
                }
                else if (language.IndexOf("-") >= 0)
                {
                    String lTemp = language.Remove(language.IndexOf("-"));
                    webJS = String.Format("{0}Resource/plugins/validation/jquery.validationEngine-{1}.js", ModulePath, lTemp);
                    serverJS = MapPath(webJS);
                    if (File.Exists(serverJS))
                    {
                        // VEL = String.Format("{0}?cdv={1}", webJS, CrmVersion);
                        VEL = webJS;
                    }
                }
            }
            return VEL;
        }


        /// <summary>
        /// 获取当前日期语言文件的URL
        /// </summary>
        /// <returns></returns>
        public String ViewDatepickLanguage()
        {
            //String VEL = String.Format("{0}Resource/plugins/jquery-datepick/jquery.datepick.lang.js?cdv={1}", ModulePath, CrmVersion);
            String VEL = String.Format("{0}Resource/plugins/jquery-datepick/jquery.datepick.lang.js", ModulePath);
            String language = WebHelper.GetStringParam(Request, "language", PortalSettings.DefaultLanguage).ToLower(); ;
            if (!String.IsNullOrEmpty(language) && language != "en-us")
            {
                //先判断这个语言文件是否存在
                String webJS = String.Format("{0}Resource/plugins/jquery-datepick/jquery.datepick-{1}.js", ModulePath, language);
                String serverJS = MapPath(webJS);
                if (File.Exists(serverJS))
                {
                    //VEL = String.Format("{0}?cdv={1}", webJS, CrmVersion);
                    VEL = webJS;
                }
                else if (language.IndexOf("-") >= 0)
                {
                    String lTemp = language.Remove(language.IndexOf("-"));
                    webJS = String.Format("{0}Resource/plugins/jquery-datepick/jquery.datepick-{1}.js", ModulePath, lTemp);
                    serverJS = MapPath(webJS);
                    if (File.Exists(serverJS))
                    {
                        //VEL = String.Format("{0}?cdv={1}", webJS, CrmVersion);
                        VEL = webJS;
                    }
                }
            }
            return VEL;
        }


        /// <summary>
        /// 多文件上传的多语言
        /// </summary>
        /// <returns></returns>
        public String ViewPluploadLanguage()
        {
            //String VEL = String.Format("{0}Resource/plugins/jquery-datepick/jquery.datepick.lang.js?cdv={1}", ModulePath, CrmVersion);
            String VEL = String.Format("{0}Resource/plugins/plupload/i18n/en.js", ModulePath);
            String language = WebHelper.GetStringParam(Request, "language", PortalSettings.DefaultLanguage).ToLower(); ;
            if (!String.IsNullOrEmpty(language) && language != "en-us")
            {
                //先判断这个语言文件是否存在
                String webJS = String.Format("{0}Resource/plugins/plupload/i18n/{1}.js", ModulePath, language);
                String serverJS = MapPath(webJS);
                if (File.Exists(serverJS))
                {
                    //VEL = String.Format("{0}?cdv={1}", webJS, CrmVersion);
                    VEL = webJS;
                }
                else if (language.IndexOf("-") >= 0)
                {
                    String lTemp = language.Remove(language.IndexOf("-"));
                    webJS = String.Format("{0}Resource/plugins/plupload/i18n/{1}.js", ModulePath, lTemp);
                    serverJS = MapPath(webJS);
                    if (File.Exists(serverJS))
                    {
                        //VEL = String.Format("{0}?cdv={1}", webJS, CrmVersion);
                        VEL = webJS;
                    }
                }
            }
            return VEL;
        }

        #endregion


        #region "XML参数"
        /// <summary>
        /// 搜索条件格式化
        /// </summary>
        /// <param name="Search">搜索条件</param>
        /// <returns></returns>
        public String SearchFormat(String Search)
        {
            return String.Format("{0}-{1}-{2}-{3}", Search, ModuleId, ClientID, TabId);
        }


        /// <summary>
        /// 读取XML参数
        /// </summary>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public object ViewXmlSetting(String Name, object DefaultValue)
        {
            return ViewXmlSetting(Settings_EffectName, Name, DefaultValue);

        }

        public object ViewXmlSetting(String EffectName, String Name, object DefaultValue)
        {
            String SettingKey = EffectSettingsFormat(EffectName, Name);
            object XmlContent = Settings[SettingKey] != null ? ConvertTo.FormatValue(Settings[SettingKey].ToString(), DefaultValue.GetType()) : DefaultValue;
            if (XmlContent != null && !String.IsNullOrEmpty(Convert.ToString(XmlContent)))
            {
                return XmlContent;
            }
            return DefaultValue;

        }


        public object ViewResultSetting(String Name, object DefaultValue)
        {
            return ViewXmlSetting(Settings_ResultName, Name, DefaultValue);
        }



        /// <summary>
        /// 读取参数
        /// </summary>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public object ViewSetting(String Name, object DefaultValue)
        {
            return Settings[Name] != null ? ConvertTo.FormatValue(Settings[Name].ToString(), DefaultValue.GetType()) : DefaultValue;
        }

        /// <summary>
        /// 读取参数
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public T ViewSettingT<T>(String Name, object DefaultValue)
        {
            var o = ViewSetting(Name, DefaultValue);
            return (T)Convert.ChangeType(o, typeof(T));
        }

        #endregion

        #region "获取文件后缀名和路径"

        /// <summary>
        /// 根据后缀名显示图标
        /// </summary>
        /// <param name="FileExtension">文件后缀</param>
        /// <param name="FilePath">文件路径</param>
        /// <returns></returns>
        public String GetPhotoExtension(String FileExtension, String FilePath)
        {
            FileExtension = FileExtension.ToLower();

            //先判断是否是图片格式的
            if (FileExtension == "jpg")
                return GetPhotoPath(FilePath);
            else if (FileExtension == "png")
                return GetPhotoPath(FilePath);
            else if (FileExtension == "jpeg")
                return GetPhotoPath(FilePath);
            else if (FileExtension == "gif")
                return GetPhotoPath(FilePath);
            else if (FileExtension == "bmp")
                return GetPhotoPath(FilePath);
            else if (FileExtension == "mp4")
                return GetFileIcon("video.jpg");
            else if (FileExtension == "ogv")
                return GetFileIcon("video.jpg");
            else if (FileExtension == "webm")
                return GetFileIcon("video.jpg");
            else if (FileExtension == "mp3")
                return GetFileIcon("audio.jpg");
            else if (FileExtension == "wma")
                return GetFileIcon("audio.jpg");
            else if (FileExtension == "zip")
                return GetFileIcon("zip.jpg");
            else if (FileExtension == "rar")
                return GetFileIcon("zip.jpg");
            else if (FileExtension == "7z")
                return GetFileIcon("zip.jpg");
            else if (FileExtension == "xls")
                return GetFileIcon("Document.jpg");
            else if (FileExtension == "txt")
                return GetFileIcon("text.jpg");
            else if (FileExtension == "cs")
                return GetFileIcon("code.jpg");
            else if (FileExtension == "html")
                return GetFileIcon("code.jpg");
            else if (FileExtension == "pdf")
                return GetFileIcon("pdf.jpg");
            else if (FileExtension == "doc")
                return GetFileIcon("Document.jpg");
            else if (FileExtension == "docx")
                return GetFileIcon("Document.jpg");
            else
                return GetFileIcon("Unknown type.jpg");
        }

        /// <summary>
        /// 获取图片的路径
        /// </summary>
        /// <param name="FilePath">图片路径</param>
        /// <returns></returns>
        public String GetPhotoPath(String FilePath)
        {
            return String.Format("{0}{1}", PortalSettings.HomeDirectory, FilePath);
        }

        /// <summary>
        /// 获取文件图标
        /// </summary>
        /// <param name="IconName">图标文件</param>
        /// <returns></returns>
        public String GetFileIcon(String IconName)
        {
            return String.Format("{0}Resource/images/crystal/{1}", ModulePath, IconName);
        }

        #endregion

        #region "Page_Init 权限验证"
        /// <summary>
        /// 关于权限验证
        /// </summary>
        protected virtual void Page_Init(System.Object sender, System.EventArgs e)
        {

            //如果不是此模块,则会抛出异常,提示非法入侵
            if (!(("DNNGo.PowerForms").IndexOf(BaseModuleName, StringComparison.CurrentCultureIgnoreCase) >= 0))
            {
                Response.Redirect(Globals.NavigateURL(TabId), true);
            }

            //没有登陆的用户
            if (!(UserId > 0))
            {
                Response.Redirect(Globals.NavigateURL(PortalSettings.LoginTabId, "Login", "returnurl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl)), true);
            }
            else if (!ModulePermissionController.HasModuleAccess(SecurityAccessLevel.Edit, "CONTENT", ModuleConfiguration))
            {
                Response.Redirect(Globals.NavigateURL(TabId), true);
            }
        }
        #endregion

        #region "获取模块信息属性DNN920"

        /// <summary>
        /// 获取模块信息属性DNN920
        /// </summary>
        /// <param name="m">模块信息</param>
        /// <param name="Name">属性名</param>
        /// <returns></returns>
        public String ModuleProperty(ModuleInfo m, String Name)
        {
            bool propertyNotFound = false;
            return m.GetProperty(Name, "", null, UserInfo, DotNetNuke.Services.Tokens.Scope.DefaultSettings, ref propertyNotFound);
        }

        /// <summary>
        /// 获取模块信息属性DNN920
        /// </summary>
        /// <param name="Name">属性名</param>
        /// <returns></returns>
        public String ModuleProperty(String Name)
        {
            return ModuleProperty(ModuleConfiguration, Name);
        }


        #endregion

    }
}