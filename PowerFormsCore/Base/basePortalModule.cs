using System;
using System.Collections.Generic;
using System.Web;
using DotNetNuke.Entities.Modules;

using System.Web.UI.WebControls;
using DotNetNuke.Services.Localization;
using System.IO;
using System.Collections;
using DotNetNuke.Entities.Host;

using DotNetNuke.UI.Skins;
using DotNetNuke.UI.Modules;
using System.Web.UI;
using DotNetNuke.Web.Client.ClientResourceManagement;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Common;
using DotNetNuke.Entities.Controllers;

namespace DNNGo.Modules.PowerForms
{
    /// <summary>
    /// 模块基类
    /// </summary>
    public class basePortalModule : PortalModuleBase 
    {

        #region "基础配置属性"







        /// <summary>
        /// 语言
        /// </summary>
        public String language
        {
            get { return WebHelper.GetStringParam(Request, "language", ((DotNetNuke.Framework.PageBase)Page).PageCulture.Name); }
        }

        /// <summary>
        /// 为当前 Web 请求获取与服务器控件关联的 System.Web.HttpContext 对象
        /// </summary>
        public HttpContext HttpContext
        {
            get { return Context; }
        }

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

        public  Boolean designMode
        {
            get { return DesignMode; }
        }



        private EffectDB _Setting_EffectDB = new EffectDB();
        /// <summary>
        /// 获取绑定效果内容
        /// </summary>
        public EffectDB Setting_EffectDB
        {
            get {
                if (!(_Setting_EffectDB != null && !String.IsNullOrEmpty(_Setting_EffectDB.Name)))
                {
                      String EffectDBPath = Server.MapPath( String.Format("{0}Effects/{1}/EffectDB.xml", ModulePath, Settings_EffectName));
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
                if (!(_Setting_EffectSettingDB != null && _Setting_EffectSettingDB.Count >0))
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

        private List<GroupEntity> __EffectGroups = new List<GroupEntity>();
        /// <summary>
        /// 页面分组列表
        /// </summary>
        public List<GroupEntity> Setting_EffectGroupsDB
        {
            get
            {
                if (!(__EffectGroups != null && __EffectGroups.Count > 0))
                {
                    String EffectSettingDBPath = Server.MapPath(String.Format("{0}Effects/{1}/EffectSetting.xml", ModulePath, Settings_EffectName));
                    if (File.Exists(EffectSettingDBPath))
                    {
                        XmlFormat xf = new XmlFormat(EffectSettingDBPath);
                        __EffectGroups = xf.ToList<GroupEntity>();
                    }
          
                }
                return __EffectGroups;
            }
        }



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

                    //全局的数据项
                    String ItemSettingGlobalPath = Server.MapPath(String.Format("{0}Resource/xml/ItemSetting.xml", ModulePath));
                    if (File.Exists(ItemSettingGlobalPath))
                    {
                        XmlFormat xf = new XmlFormat(ItemSettingGlobalPath);
                        _Setting_ItemSettingDB.AddRange(xf.ToList<SettingEntity>());
                    }
                }
                return _Setting_ItemSettingDB;
            }
        }





        private EffectDB _Setting_ResultDB = new EffectDB();
        /// <summary>
        /// 获取绑定结果效果内容
        /// </summary>
        public EffectDB Setting_ResultDB
        {
            get
            {
                if (!(_Setting_ResultDB != null && !String.IsNullOrEmpty(_Setting_ResultDB.Name)))
                {
                    String EffectDBPath = Server.MapPath(String.Format("{0}Results/{1}/EffectDB.xml", ModulePath, Settings_ResultName));
                    if (File.Exists(EffectDBPath))
                    {
                        XmlFormat xf = new XmlFormat(EffectDBPath);
                        _Setting_ResultDB = xf.ToItem<EffectDB>();
                    }
                }
                return _Setting_ResultDB;
            }
        }


        private List<SettingEntity> _Setting_ResultSettingDB = new List<SettingEntity>();
        /// <summary>
        /// 获取绑定效果设置项
        /// </summary>
        public List<SettingEntity> Setting_ResultSettingDB
        {
            get
            {
                if (!(_Setting_ResultSettingDB != null && _Setting_ResultSettingDB.Count > 0))
                {
                    String EffectSettingDBPath = Server.MapPath(String.Format("{0}Results/{1}/EffectSetting.xml", ModulePath, Settings_ResultName));
                    if (File.Exists(EffectSettingDBPath))
                    {
                        XmlFormat xf = new XmlFormat(EffectSettingDBPath);
                        _Setting_ResultSettingDB = xf.ToList<SettingEntity>();
                    }
                }
                return _Setting_ResultSettingDB;
            }
        }


        private List<GroupEntity> __ResultGroups = new List<GroupEntity>();
        /// <summary>
        /// 页面分组列表
        /// </summary>
        public List<GroupEntity> Setting_ResultGroupsDB
        {
            get
            {
                if (!(__ResultGroups != null && __ResultGroups.Count > 0))
                {
                    String EffectSettingDBPath = Server.MapPath(String.Format("{0}Results/{1}/EffectSetting.xml", ModulePath, Settings_ResultName));
                    if (File.Exists(EffectSettingDBPath))
                    {
                        XmlFormat xf = new XmlFormat(EffectSettingDBPath);
                        __ResultGroups = xf.ToList<GroupEntity>();
                    }

                }
                return __ResultGroups;
            }
        }



        private List<SettingEntity> _Setting_CategorySettingDB = new List<SettingEntity>();
        /// <summary>
        /// 获取绑定数据设置项(分类)
        /// </summary>
        public List<SettingEntity> Setting_CategorySettingDB
        {
            get
            {
                if (!(_Setting_CategorySettingDB != null && _Setting_CategorySettingDB.Count > 0))
                {
                    //皮肤的数据项
                    String ItemSettingSkinPath = Server.MapPath(String.Format("{0}Effects/{1}/CategorySetting.xml", ModulePath, Settings_EffectName));
                    if (File.Exists(ItemSettingSkinPath))
                    {
                        XmlFormat xf = new XmlFormat(ItemSettingSkinPath);
                        _Setting_CategorySettingDB.AddRange(xf.ToList<SettingEntity>());
                    }

                    //全局的数据项
                    String ItemSettingGlobalPath = Server.MapPath(String.Format("{0}Resource/xml/CategorySetting.xml", ModulePath));
                    if (File.Exists(ItemSettingGlobalPath))
                    {
                        XmlFormat xf = new XmlFormat(ItemSettingGlobalPath);
                        _Setting_CategorySettingDB.AddRange(xf.ToList<SettingEntity>());
                    }
                }
                return _Setting_CategorySettingDB;
            }
        }



        /// <summary>
        /// 每页数量
        /// </summary>
        public Int32 Settings_PageSize
        {
            get { return Settings["PowerForms_PageSize"] != null ? Convert.ToInt32(Settings["PowerForms_PageSize"]) : 10; }
        }


        /// <summary>
        /// 跳转方式（枚举EnumRedirectType）
        /// </summary>
        public Int32 Settings_RedirectType
        {
            get { return Settings["PowerForms_RedirectType"] != null ? Convert.ToInt32(Settings["PowerForms_RedirectType"]) : (Int32)EnumRedirectType.Results; }
        }

        /// <summary>
        /// 跳转页面(跳转方式为指定页面时)
        /// </summary>
        public String Settings_RedirectPage
        {
            get { return Settings["PowerForms_RedirectPage"] != null ? Convert.ToString(Settings["PowerForms_RedirectPage"]) : String.Empty; }
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

        public string GetHostSetting(string key, string defaultValue = "")
        {
            return HostController.Instance.GetString(key, defaultValue); ;
        }







        /// <summary>
        /// 是否开启SSL
        /// </summary>
        public Boolean IsSSL
        {
            get { return PortalSettings.ActiveTab.IsSecure || WebHelper.GetSSL; }
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
        /// 框架的结构
        /// </summary>
        public String iFrame = HttpContext.Current.Request.QueryString["iFrame"] != null  ? HttpContext.Current.Request.QueryString["iFrame"] :"";


        #endregion

        #region "跟踪的设置与属性"

        /// <summary>
        /// 启用扩展跟踪
        /// </summary>
        public Boolean Settings_ExtraTracking
        {
            get { return Settings["PowerForms_ExtraTracking"] != null ? Convert.ToBoolean(Settings["PowerForms_ExtraTracking"]) : false; }
        }


        /// <summary>
        /// 跟踪-进入页面前的地址
        /// </summary>
        public String Tracking_OriginalReferrer
        {
            get
            {
                String _OriginalReferrer = WebHelper.GetCookie("OriginalReferrer");
                if (String.IsNullOrEmpty(_OriginalReferrer))
                {
                    _OriginalReferrer = Request.UrlReferrer != null ?  Request.UrlReferrer.ToString():"--";
                    //if (String.IsNullOrEmpty(_OriginalReferrer))
                    //{
                    //    _OriginalReferrer = "--";
                    //}
                    WebHelper.SaveCookie("OriginalReferrer", _OriginalReferrer, 24);
                }
                return _OriginalReferrer;
            }
        }
        /// <summary>
        /// 第一次用户访问网页的地址
        /// </summary>
        public String Tracking_LandingPage
        {
            get
            {
                String _LandingPage = WebHelper.GetCookie("LandingPage");
                if (String.IsNullOrEmpty(_LandingPage))
                {
                    _LandingPage = String.Format("http://{0}{1}", WebHelper.GetHomeUrl(), Request.RawUrl);
                    WebHelper.SaveCookie("LandingPage", _LandingPage, 24);
                }
                return _LandingPage;
            }
        }

        #endregion

        #region "打印模块的名称版本等信息"
        private Boolean? _Debug;
        /// <summary>
        /// 是否调试模式
        /// </summary>
        public Boolean Debug
        {
            get
            {
                if (!_Debug.HasValue)
                {
                    String sDebug = WebHelper.GetStringParam(Request, "Debug", "false").ToLower();
                    _Debug = !String.IsNullOrEmpty(sDebug) && sDebug.IndexOf("true") >= 0;
                }
                return _Debug.Value;
            }
        }

        /// <summary>
        /// 载入Debug信息
        /// </summary>
        public void LoadDebug()
        {
            if (Debug)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine();
                sb.Append("<!--**********************************************************************************").AppendLine();
                sb.AppendFormat("Module ModuleID:{0}       ", ModuleConfiguration.ModuleID).AppendLine();
                sb.AppendFormat("Module Name:{0}           ", ModuleProperty(ModuleConfiguration, "ModuleName")).AppendLine();
                sb.AppendFormat("Module Cache Time:{0}     ", ModuleConfiguration.CacheTime).AppendLine();
                sb.AppendFormat("Module Version:{0}        ", ModuleProperty(ModuleConfiguration, "Version")).AppendLine();

                sb.AppendFormat("Date Format:{0}           ", DateTime.Now.ToString()).AppendLine();

                sb.Append("**********************************************************************************-->").AppendLine();
                Controls.AddAt(0, new LiteralControl(sb.ToString()));
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

        #region "jQuery配置属性"

        /// <summary>
        /// 开始模块jQuery
        /// </summary>
        public Boolean Settings_jQuery_Enable
        {
            get { return Settings["PowerForms_jQuery_Enable"] != null && !string.IsNullOrEmpty(Settings["PowerForms_jQuery_Enable"].ToString()) ? Convert.ToBoolean(Settings["PowerForms_jQuery_Enable"]) : false; }
        }

        /// <summary>
        /// 使用jQuery库
        /// </summary>
        public Boolean Settings_jQuery_UseHosted
        {
            get { return Settings["PowerForms_jQuery_UseHosted"] != null && !string.IsNullOrEmpty(Settings["PowerForms_jQuery_UseHosted"].ToString()) ? Convert.ToBoolean(Settings["PowerForms_jQuery_UseHosted"]) : false; }
        }

        /// <summary>
        /// jQuery库的地址
        /// </summary>
        public String Settings_jQuery_HostedjQuery
        {
            get { return Settings["PowerForms_jQuery_HostedjQuery"] != null && !string.IsNullOrEmpty(Settings["PowerForms_jQuery_HostedjQuery"].ToString()) ? Convert.ToString(Settings["PowerForms_jQuery_HostedjQuery"]) : "https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"; }
        }

        /// <summary>
        /// jQueryUI库的地址
        /// </summary>
        public String Settings_jQuery_HostedjQueryUI
        {
            get { return Settings["PowerForms_jQuery_HostedjQueryUI"] != null && !string.IsNullOrEmpty(Settings["PowerForms_jQuery_HostedjQueryUI"].ToString()) ? Convert.ToString(Settings["PowerForms_jQuery_HostedjQueryUI"]) : "https://ajax.googleapis.com/ajax/libs/jqueryui/1.10.4/jquery-ui.min.js"; }
        }


        #endregion

        #region "邮件相关的配置属性"

        /// <summary>
        /// 管理员邮箱
        /// </summary>
        public String Settings_AdminEmail
        {
            get { return Settings["PowerForms_AdminEmail"] != null ? Convert.ToString(Settings["PowerForms_AdminEmail"]) : Host.HostEmail; }
        }

        /// <summary>
        /// 提交用户邮箱字段
        /// </summary>
        public String Settings_SubmitUserEmail
        {
            get { return Settings["PowerForms_SubmitUserEmail"] != null ? Convert.ToString(Settings["PowerForms_SubmitUserEmail"]) : ""; }
        }


        /// <summary>
        /// 是否发邮件给管理员
        /// </summary>
        public Boolean Settings_SendToAdmin
        {
            get { return Settings["PowerForms_SendToAdmin"] != null ? Convert.ToBoolean(Settings["PowerForms_SendToAdmin"]) : true; }
        }

        /// <summary>
        /// 是否发邮件给提交用户
        /// </summary>
        public Boolean Settings_SendToSubmitUser
        {
            get { return Settings["PowerForms_SendToSubmitUser"] != null ? Convert.ToBoolean(Settings["PowerForms_SendToSubmitUser"]) : true; }
        }




        private DNNGo_PowerForms_Template _Settings_Template = new DNNGo_PowerForms_Template();
        /// <summary>
        /// 邮件模版
        /// </summary>
        public DNNGo_PowerForms_Template Settings_EmailTemplate
        {
            get {
                if (!(_Settings_Template != null && _Settings_Template.ID > 0))
                {
                    _Settings_Template = DNNGo_PowerForms_Template.FindByModuleId(ModuleId);

                    if (!(_Settings_Template != null && _Settings_Template.ID > 0))
                    {
                        _Settings_Template = new DNNGo_PowerForms_Template();
                        _Settings_Template.ReceiversSubject = Localization.GetString("PowerForms_ReceiversSubject", Localization.GetResourceFile(this, "Manager_Settings_Email.ascx.resx"));
                        _Settings_Template.ReceiversTemplate = Localization.GetString("PowerForms_ReceiversTemplate", Localization.GetResourceFile(this, "Manager_Settings_Email.ascx.resx"));
                        _Settings_Template.ReplySubject = Localization.GetString("PowerForms_ReplySubject", Localization.GetResourceFile(this, "Manager_Settings_Email.ascx.resx"));
                        _Settings_Template.ReplyTemplate = Localization.GetString("PowerForms_ReplyTemplate", Localization.GetResourceFile(this, "Manager_Settings_Email.ascx.resx"));
                        _Settings_Template.ModuleId = ModuleId;
                        _Settings_Template.Insert();
                    }
                
                }
                return _Settings_Template;
            }
            
        }

        

         



        #region "隐藏域防提交设置"


        /// <summary>
        /// 隐藏域验证开关
        /// </summary>
        public Boolean Settings_Hiddenfields_Enable
        {
            get { return Settings["PowerForms_Hiddenfields_Enable"] != null ? Convert.ToBoolean(Settings["PowerForms_Hiddenfields_Enable"]) : true; }
        }
        /// <summary>
        /// 隐藏域密钥
        /// </summary>
        public String Settings_Hiddenfields_EncryptionKey
        {
            get { return Settings["PowerForms_Hiddenfields_EncryptionKey"] != null ? Convert.ToString(Settings["PowerForms_Hiddenfields_EncryptionKey"]) : String.Format("powerforms#{0}&{1}",PortalId, ModuleId); }
        }
        /// <summary>
        /// 隐藏域验证字符串长度
        /// </summary>
        public Int32 Settings_Hiddenfields_VerifyStringLength
        {
            get { return Settings["PowerForms_Hiddenfields_VerifyStringLength"] != null ? Convert.ToInt32(Settings["PowerForms_Hiddenfields_VerifyStringLength"]) : 12; }
        }
        /// <summary>
        /// 隐藏域验证间隔时间
        /// </summary>
        public Int32 Settings_Hiddenfields_VerifyIntervalTime
        {
            get { return Settings["PowerForms_Hiddenfields_VerifyIntervalTime"] != null ? Convert.ToInt32(Settings["PowerForms_Hiddenfields_VerifyIntervalTime"]) : 10; }
        }
 
        #endregion





        #endregion

        #region "加载样式表"

        /// <summary>
        /// 绑定样式表文件
        /// </summary>
        /// <param name="Name"></param>
        public void BindStyleFile(String Name, String FileName)
        {
            BindStyleFile(Name, FileName,50);
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
            BindJavaScriptFile(Name, FileName, 50,true);
        }

        /// <summary>
        /// 绑定脚本文件
        /// </summary>
        /// <param name="ThemeName"></param>
        public void BindJavaScriptFile(String Name, String FileName, int priority,Boolean IsClientResourceManager = true)
        {
        

            if (HttpContext.Current.Items[Name] == null)
            {
                HttpContext.Current.Items.Add(Name, "true");

                String PageUrl = WebHelper.GetScriptFileName;
                if ((!String.IsNullOrEmpty(PageUrl) && PageUrl.IndexOf("View_iFrame.aspx", StringComparison.CurrentCultureIgnoreCase) >= 0) || !IsClientResourceManager)
                {
                    Page.ClientScript.RegisterClientScriptInclude(Name, ResolveUrl(String.Format("{0}?cdv={1}", FileName, CrmVersion)));
                }
                else
                {
                    ClientResourceManager.RegisterScript(Page, FileName, priority);
                }
            }
        }

        



        #endregion

        #region "加载XML配置文件中的脚本与样式表"
        /// <summary>
        /// XmlDB
        /// </summary>
        /// <param name="XmlDB">配置文件</param>
        /// <param name="XmlName">效果/皮肤</param>
        public void BindXmlDBToPage(EffectDB XmlDB, String XmlName)
        {

            int priority = 50;

            //绑定全局附带的脚本
            if (!String.IsNullOrEmpty(XmlDB.GlobalScript))
            {
                List<String> GlobalScripts = WebHelper.GetList(XmlDB.GlobalScript);

                foreach (String Script in GlobalScripts)
                {
                    if (!String.IsNullOrEmpty(Script))
                    {
                        if (Script.IndexOf(".css", StringComparison.CurrentCultureIgnoreCase) > 0)
                        {
                            String FullFileName = String.Format("{0}Resource/css/{1}", ModulePath, Script);
                            BindStyleFile(Script, FullFileName, priority);
                        }
                        else //if (Script.IndexOf(".js", StringComparison.CurrentCultureIgnoreCase) > 0)
                        {
                            String FullFileName = String.Format("{0}Resource/js/{1}", ModulePath, Script);
                            BindJavaScriptFile(Script, FullFileName, priority);
                        }
                        priority++;
                    }
                }
            }
            //绑定效果附带的脚本
            if (!String.IsNullOrEmpty(XmlDB.EffectScript))
            {
                List<String> EffectScripts = WebHelper.GetList(XmlDB.EffectScript);

                foreach (String Script in EffectScripts)
                {
                    if (!String.IsNullOrEmpty(Script))
                    {
                        if (Script.IndexOf(".css", StringComparison.CurrentCultureIgnoreCase) > 0)
                        {
                            String FullFileName = String.Format("{0}{1}s/{2}/css/{3}", ModulePath, XmlName, XmlDB.Name, Script);
                            BindStyleFile(Script, FullFileName, priority);
                        }
                        else
                        {
                            String FullFileName = String.Format("{0}{1}s/{2}/js/{3}", ModulePath, XmlName, XmlDB.Name, Script);
                            BindJavaScriptFile(Script, FullFileName, priority);
                        }
                        priority++;
                    }
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
                ManageContent.ModuleControl = this;
                objCSS.Controls.Add(ManageContent);//具有编辑权限才能看到模块

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




            

                //检测是否开启了验证码功能
                //if (Settings_Recaptcha_Enable)
                //{
                //    //加载验证码插件
                //    if (HttpContext.Current.Items["recaptcha_ajax_js"] == null)
                //    {
                //        //Literal litLink = new Literal();
                //        //litLink.Text =
                //        //     Microsoft.VisualBasic.Constants.vbCrLf + "<script type=\"text/javascript\" src=\"http://www.google.com/recaptcha/api/js/recaptcha_ajax.js\"></script>" +
                //        //    Microsoft.VisualBasic.Constants.vbCrLf;
                //        //HttpContext.Current.Items.Add("recaptcha_ajax_js", "true");
                //        //objCSS.Controls.Add(litLink);

                //        HttpContext.Current.Items.Add("recaptcha_ajax_js", "true");
                //        DotNetNuke.Framework.AJAX.AddScriptManager(this.Page);
                //        Page.ClientScript.RegisterClientScriptInclude("recaptcha_ajax_js", "http://www.google.com/recaptcha/api/js/recaptcha_ajax.js");
                //    }
                //}


 
         

 



                //if (HttpContext.Current.Items["DNNGo_PowerForms_Modules_css"] == null)
                //{
                //    Literal litLink = new Literal();
                //    litLink.Text = "<link  rel=\"stylesheet\" type=\"text/css\" href=\"" + ModulePath + "Resource/css/Modules.css\" />";

                //    HttpContext.Current.Items.Add("DNNGo_PowerForms_Modules_css", "true");
                //    objCSS.Controls.Add(litLink);
                //}


                BindJavaScriptFile("jquery.validationEngine-en.js", ViewValidationEngineLanguage(),1);
                BindJavaScriptFile("jquery.validationEngine.js", String.Format("{0}Resource/js/jquery.validationEngine.js", ModulePath),2);

      
                //BindJavaScriptFile("jquery.tmpl.min.js", String.Format("{0}Resource/js/jquery.tmpl.min.js", ModulePath), 9);
                

                BindJavaScriptFile("sisyphus.min.js", String.Format("{0}Resource/js/sisyphus.min.js", ModulePath),10);
 
                BindJavaScriptFile("tinymce.min.js", String.Format("{0}Resource/plugins/tinymce/tinymce.min.js", ModulePath),11,false);
               

                BindStyleFile("jquery.datepick.css", String.Format("{0}Resource/plugins/jquery-datepick/jquery.datepick.css", ModulePath),12);


                BindJavaScriptFile("jquery.plugin.min.js", String.Format("{0}Resource/plugins/jquery-datepick/jquery.plugin.min.js", ModulePath), 13);
                BindJavaScriptFile("jquery.datepick.min.js", String.Format("{0}Resource/plugins/jquery-datepick/jquery.datepick.min.js", ModulePath),14);
                BindJavaScriptFile("jquery.datepick.lang.js", ViewDatepickLanguage(),15);




                //载入多文件上传脚本
                //LoadPluploadScript();







                if (ViewSettingT<Boolean>("PowerForms_Recaptcha_v3_Enable", false))
                {
                    BindJavaScriptFile("google.recaptcha", String.Format("https://www.google.com/recaptcha/api.js?render={0}&hl={1}", ViewSettingT<String>("PowerForms_Recaptcha_v3_SiteKey", ""), language));
                    //if (HttpContext.Current.Items["onloadFormsCallback"] == null)
                    //{
                    //    HttpContext.Current.Items.Add("onloadFormsCallback", "true");

                    //    System.Text.StringBuilder onloadFormsCallback = new System.Text.StringBuilder();
                    //    onloadFormsCallback.Append("<script type=\"text/javascript\">").AppendLine();
                    //    onloadFormsCallback.Append("var onloadformscallback = function () {").AppendLine();
                    //    onloadFormsCallback.Append("$('.g-recaptcha-dnn').each(function (i) {").AppendLine();
                    //    onloadFormsCallback.Append("grecaptcha.render(this.id, {").AppendLine();
                    //    onloadFormsCallback.AppendFormat("hl: '{0}',",language).AppendLine();
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
        /// 加载管理界面脚本样式表
        /// </summary>
        public void LoadManagerScript()
        {
            System.Web.UI.Control objCSS = this.Page.FindControl("CSS");
            if ((objCSS != null))
            {
               // LoadScriptForJqueryAndUI(ModulePath);

                //if (HttpContext.Current.Items["thickbox_CSS"] == null)
                //{
                //    Literal litLink = new Literal();
                //    litLink.Text = String.Format("<link  rel=\"stylesheet\" type=\"text/css\" href=\"{0}Resource/css/thickbox.css?cdv={1}\" />", ModulePath, CrmVersion);

                //    HttpContext.Current.Items.Add("thickbox_CSS", "true");
                //    objCSS.Controls.Add(litLink);
                //}

    






 



                //if (HttpContext.Current.Items["jquery-ui-CSS"] == null)
                //{
                //    Literal litLink = new Literal();
                //    litLink.Text = String.Format("<link  rel=\"stylesheet\" type=\"text/css\" href=\"{0}Resource/css/jquery-ui-1.7.custom.css?cdv={1}\" />", ModulePath, CrmVersion);

                //    HttpContext.Current.Items.Add("jquery-ui-CSS", "true");
                //    objCSS.Controls.Add(litLink);
                //}





                //if (HttpContext.Current.Items["DNNGo_PowerForms_Modules_css"] == null)
                //{
                //    Literal litLink = new Literal();
                //    litLink.Text = String.Format("<link  rel=\"stylesheet\" type=\"text/css\" href=\"{0}Resource/css/Modules.css?cdv={1}\" />", ModulePath, CrmVersion);

                //    HttpContext.Current.Items.Add("DNNGo_PowerForms_Modules_css", "true");
                //    objCSS.Controls.Add(litLink);
                //}





                ////if (HttpContext.Current.Items["Vanadium_js"] == null)
                ////{
                ////    Literal litLink = new Literal();
                ////    litLink.Text =
                ////         Microsoft.VisualBasic.Constants.vbCrLf + "<script type=\"text/javascript\" src=\"" + ModulePath + "Resource/js/vanadium.js\"></script>" +
                ////        Microsoft.VisualBasic.Constants.vbCrLf;
                ////    HttpContext.Current.Items.Add("Vanadium_js", "true");
                ////    objCSS.Controls.Add(litLink);
                ////}


                //if (HttpContext.Current.Items["jquery.validationEngine-en.js"] == null)
                //{

                //    HttpContext.Current.Items.Add("jquery.validationEngine-en.js", "true");
                //    DotNetNuke.Framework.AJAX.AddScriptManager(this.Page);
                //    Page.ClientScript.RegisterClientScriptInclude("jquery.validationEngine-en.js", String.Format("{0}Resource/js/jquery.validationEngine-en.js?cdv={1}", ModulePath, CrmVersion));
                //}


                //if (HttpContext.Current.Items["jquery.validationEngine.js"] == null)
                //{

                //    HttpContext.Current.Items.Add("jquery.validationEngine.js", "true");
                //    DotNetNuke.Framework.AJAX.AddScriptManager(this.Page);
                //    Page.ClientScript.RegisterClientScriptInclude("jquery.validationEngine.js", String.Format("{0}Resource/js/jquery.validationEngine.js?cdv={1}", ModulePath, CrmVersion));
                //}
 

             
 
            }
        }


        /// <summary>
        /// 加载脚本
        /// </summary>
        public void LoadScriptForJqueryAndUI(string modulePath)
        {
            System.Web.UI.Control objCSS = this.Page.FindControl("CSS");
            if ((objCSS != null))
            {
                String jQueryUrl = String.Format("{0}Resource/js/jquery.min.js?cdv={1}", ModulePath, CrmVersion);
                String jQueryUIUrl = String.Format("{0}Resource/js/jquery-ui.min.js?cdv={1}", ModulePath, CrmVersion);
                if (Settings_jQuery_UseHosted)//使用指定的jQuery库的地址
                {
                    jQueryUrl = Settings_jQuery_HostedjQuery;
                    jQueryUIUrl = Settings_jQuery_HostedjQueryUI;
                }





                if ((Settings_jQuery_Enable && !HttpContext.Current.Items.Contains("jQueryUIRequested")) || (Settings_jQuery_Enable && !HttpContext.Current.Items.Contains("DNNGo_jQueryUI")))
                {
                    Literal litLink = new Literal();
                    litLink.Text = String.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", jQueryUIUrl);
                    //if (!Settings_jQuery_Enable)
                    //{

                    if (!HttpContext.Current.Items.Contains("jQueryUIRequested")) HttpContext.Current.Items.Add("jQueryUIRequested", "true");
                    //}
                    if (!HttpContext.Current.Items.Contains("DNNGo_jQueryUI")) HttpContext.Current.Items.Add("DNNGo_jQueryUI", "true");
                    objCSS.Controls.AddAt(0, litLink);
                }

                if ((Settings_jQuery_Enable && !HttpContext.Current.Items.Contains("jquery_registered") && !HttpContext.Current.Items.Contains("jQueryRequested")) || (Settings_jQuery_Enable && !HttpContext.Current.Items.Contains("DNNGo_jQuery")))
                {
                    Literal litLink = new Literal();
                    litLink.Text = String.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", jQueryUrl);
                    //if (!Settings_jQuery_Enable)
                    //{
                    if (!HttpContext.Current.Items.Contains("jquery_registered")) HttpContext.Current.Items.Add("jquery_registered", "true");
                    if (!HttpContext.Current.Items.Contains("jQueryRequested")) HttpContext.Current.Items.Add("jQueryRequested", "true");
                    //}
                    if (!HttpContext.Current.Items.Contains("DNNGo_jQuery")) HttpContext.Current.Items.Add("DNNGo_jQuery", "true");

                    objCSS.Controls.AddAt(0, litLink);

                    litLink = new Literal();
                    litLink.Text = String.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", String.Format("{0}Resource/js/jquery-migrate.min.js?cdv={1}", ModulePath, CrmVersion));
                    objCSS.Controls.AddAt(1, litLink);
                }
            }
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

        #region "加载提示语句"

        /// <summary>
        /// 显示未绑定模版的语句
        /// </summary>
        /// <returns></returns>
        public String ViewNoTemplate()
        {
            String NoTemplate = Localization.GetString("NoTemplate.Message", Localization.GetResourceFile(this, "Message.resx"));


            return NoTemplate + ViewThemeGoUrl();
        }
        /// <summary>
        /// 显示未绑定主题时的跳转链接
        /// </summary>
        /// <returns></returns>
        public String ViewThemeGoUrl()
        {
            String ThemeGoUrl = String.Empty;
            //有编辑权限的时候，显示跳转到模版加载页
            if (IsEditable)
            {
                ThemeGoUrl = Localization.GetString("ThemeGoUrl.Message", Localization.GetResourceFile(this, "Message.resx"));
                ThemeGoUrl = ThemeGoUrl.Replace("[ThemeUrl]", EditUrl("Token", "Themes", "Manager"));
            }
            return ThemeGoUrl;
        }

        /// <summary>
        /// 未设置模块的绑定
        /// </summary>
        /// <returns></returns>
        public String ViewNoSettingBind()
        {
            return Localization.GetString("NoModuleSetting.Message", Localization.GetResourceFile(this, "Message.resx"));
        }



        /// <summary>
        /// 显示列表无数据的提示
        /// </summary>
        /// <returns></returns>
        public String ViewGridViewEmpty()
        {
            return Localization.GetString("GridViewEmpty.Message", Localization.GetResourceFile(this, "Message.resx"));
        }


        /// <summary>
        /// 绑定GridView的空信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gvList"></param>
        public void BindGridViewEmpty<T>(GridView gvList)
             where T : new()
        {
            BindGridViewEmpty<T>(gvList, new T());
        }

        /// <summary>
        /// 绑定GridView的空信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gvList"></param>
        /// <param name="t"></param>
        public void BindGridViewEmpty<T>(GridView gvList, T t)
        {
            String EmptyDataText = ViewGridViewEmpty();
            if (gvList.Rows.Count == 0)
            {
                List<T> ss = new List<T>();
                ss.Add(t);
                gvList.DataSource = ss;
                gvList.DataBind();

                gvList.Rows[0].Cells.Clear();
                gvList.Rows[0].Cells.Add(new TableCell());
                gvList.Rows[0].Cells[0].ColumnSpan = gvList.Columns.Count;
                gvList.Rows[0].Cells[0].Text = EmptyDataText;
                gvList.Rows[0].Cells[0].Style.Add("text-align", "center");
            }
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
            UpdateModuleSetting(this.ModuleId, SettingName, SettingValue);
        }


        /// <summary>
        /// 更新模块设置
        /// </summary>
        /// <param name="ModuleId"></param>
        /// <param name="SettingName"></param>
        /// <param name="SettingValue"></param>
        public void UpdateModuleSetting(int ModuleId, string SettingName, string SettingValue)
        {
            ModuleController controller = new ModuleController();

            controller.UpdateModuleSetting(ModuleId, SettingName, SettingValue);

            //refresh cache
            SynchronizeModule();
        }

        /// <summary>
        /// 效果参数保存名称格式化
        /// </summary>
        /// <param name="EffectName">效果名</param>
        /// <param name="ThemeName">主题名</param>
        /// <returns></returns>
        public String EffectSettingsFormat(String EffectName, String ThemeName)
        {
            /*2015.09.21 由于key只能存50的长度，这里修正一下*/
            return WebHelper.leftx(String.Format("Gallery{0}_{1}", EffectName, ThemeName),50);
        }

        #endregion


        #region "绑定模版文件"
 

        /// <summary>
        /// 显示模版
        /// </summary>
        /// <param name="Theme"></param>
        /// <param name="ThemeFile"></param>
        /// <param name="Puts"></param>
        /// <returns></returns>
        public String ViewTemplate(EffectDB Theme, String ThemeFile, Hashtable Puts)
        {
            TemplateFormat xf = new TemplateFormat(this);
            return ViewTemplate(Theme, ThemeFile, Puts, xf);
        }

        /// <summary>
        /// 显示模版
        /// </summary>
        /// <param name="Theme"></param>
        /// <param name="xf"></param>
        /// <param name="Puts"></param>
        /// <returns></returns>
        public String ViewTemplate(EffectDB Theme, String ThemeFile, Hashtable Puts, TemplateFormat xf, String _path = "Effect")
        {
            VelocityHelper vltContext = new VelocityHelper(this, Theme, _path);


            vltContext.Put("xf", xf);//模版格式化共用方法
            vltContext.Put("ModuleID", ModuleId);//绑定的主模块编号
            vltContext.Put("TabID", TabId);//绑定的主模块页面编号

            if (Puts != null && Puts.Count > 0)
            {
                foreach (String key in Puts.Keys)
                {
                    vltContext.Put(key, Puts[key]);
                }
            }
            return vltContext.Display(ThemeFile);
        }



 

        #endregion

        #region "绑定页面标题和帮助"

        /// <summary>
        /// 显示控件标题
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <param name="ControlName"></param>
        /// <param name="Suffix"></param>
        /// <param name="ClassName"></param>
        /// <returns></returns>
        public String ViewControlTitle(String Title, String DefaultValue, String ControlName, String Suffix, String ClassName)
        {
            String Content = ViewResourceText(Title, DefaultValue);
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

            return String.Format("<label  {2} {1}>{0}{3}</label>",
                Content,
                !String.IsNullOrEmpty(ClassName) ? String.Format("class=\"{0}\"", ClassName) : "",
              !String.IsNullOrEmpty(ControlName) ? String.Format("for=\"{0}\"", ControlName) : "",
              Suffix
                );
        }




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
            return ViewHelp(Title, DefaultValue, "");
        }

        /// <summary>
        /// 显示帮助
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewHelp(String Title, String DefaultValue, String ControlName)
        {
            String Content = ViewResourceText(Title, DefaultValue, "Help");
            //return ViewSpan(Content, ControlName, "span_help");
            return String.Format("<span class=\"help-block\" for=\"{1}\"><i class=\"fa fa-info-circle\"></i> {0}</span>", Content, ControlName);
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

            return String.Format("<label  {2} {1}><span {1} >{0}</span></label>",
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
        /// 显示菜单的文本
        /// </summary>
        /// <param name="MenuItem">菜单项</param>
        /// <returns></returns>
        public String ShowMenuText(TokenItem MenuItem)
        {
            return ViewResourceText(MenuItem.Token, MenuItem.Title, "MenuText");
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


        #region "关于表单逻辑"



        public Boolean IsCondition(object Condition)
        {
            return Condition != null && !String.IsNullOrEmpty(Convert.ToString(Condition));
        }

        public String ConditionCSS(object Condition)
        {
            return IsCondition(Condition) ? "conditional" : "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Condition"></param>
        /// <returns></returns>
        public String ConditionDATA(object Condition)
        {
            return IsCondition(Condition) ? String.Format("data-condition=\"{0}\"", Condition) : "";
        }

        #endregion


        #region "错误捕获"

        /// <summary>
        /// 错误捕获
        /// </summary>
        /// <param name="exc">错误</param>
        public void ProcessModuleLoadException(Exception exc)
        {
            if (HttpContext.Current.Session["Exception"] != null)
            {
                HttpContext.Current.Session.Remove("Exception");
            }
            //增加当前序列化的内容到Session
            HttpContext.Current.Session.Add("Exception", exc);

            if (WebHelper.GetStringParam(Request, "Token", "").ToLower() != "error")
            {
                Response.Redirect(xUrl("ReturnUrl", HttpUtility.UrlEncode(WebHelper.GetScriptUrl), "Error"), false);
            }

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
                if (Settings != null && Settings.Count > 0)
                {
                    UpdateModuleSetting("PowerForms_EffectName", EffectDirs[0].Name);
                }
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
                if (Settings != null && Settings.Count > 0)
                {
                    UpdateModuleSetting("PowerForms_EffectThemeName", ThemeDirs[0].Name);
                }
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


        #region "DNN 920 的支持"

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
            return m.GetProperty(Name, "", System.Globalization.CultureInfo.CurrentCulture, UserInfo, DotNetNuke.Services.Tokens.Scope.DefaultSettings, ref propertyNotFound);
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

        #region "模块路径"
        /// <summary>
        /// 模块路径
        /// </summary>
        public String ModulePath
        {
            get { return ControlPath; }
        }

        #endregion

        #endregion

    }
}