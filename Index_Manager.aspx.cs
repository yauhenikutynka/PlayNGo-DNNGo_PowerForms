using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Localization;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Security.Permissions;
using DotNetNuke.Security;
using DotNetNuke.Common;


namespace DNNGo.Modules.PowerForms
{
    public partial class Index_Manager : BasePage
    {

        #region "==属性=="

        /// <summary>
        /// 当前标签
        /// </summary>
        public String Token
        {
            get { return WebHelper.GetStringParam(HttpContext.Current.Request, "Token", "History").ToLower(); }
        }

        /// <summary>
        /// 文章编号
        /// </summary>
        public Int32 ArticleID = WebHelper.GetIntParam(HttpContext.Current.Request, "ArticleID", 0);

        /// <summary>
        /// 评论编号
        /// </summary>
        public Int32 CommentID = WebHelper.GetIntParam(HttpContext.Current.Request, "CommentID", 0);


        /// <summary>
        /// 分类编号
        /// </summary>
        private Int32 CategoryID = WebHelper.GetIntParam(HttpContext.Current.Request, "CategoryID", 0);

        /// <summary>
        /// 标签编号
        /// </summary>
        private Int32 TagID = WebHelper.GetIntParam(HttpContext.Current.Request, "TagID", 0);


        /// <summary>
        /// 提示操作类
        /// </summary>
        MessageTips mTips = new MessageTips();

 
 


        #endregion


        #region "==方法=="

        private MenuTabCollection _InitMenuTabCollection = new MenuTabCollection();
        /// <summary>
        /// 初始化标签集合
        /// </summary>
        public MenuTabCollection InitMenuTabCollection
        {
            get
            {
                if (_InitMenuTabCollection == null || _InitMenuTabCollection.Count < 1)
                {
                    //靠XML地址切换菜单配置文件的地址
                    String MenuPath = MapPath(String.Format("{0}Resource/xml/MenuTabs.xml", ModulePath));
 
                    XmlFormat xf = new XmlFormat(MenuPath);

                    List<MenuTabItem> tabs = xf.ToList<MenuTabItem>();

                    foreach (MenuTabItem item in tabs)
                    {
                        _InitMenuTabCollection.Add(item);
                    }

                    //如果没有分组的效果需要去掉分组
                    EffectDB EffectDB = Setting_EffectDB;
                    if (!(EffectDB != null && String.IsNullOrEmpty(EffectDB.Name)))
                    {
                        if (!EffectDB.Group) _InitMenuTabCollection.Remove("groups");

                        if(!ViewSettingT<bool>("PowerForms_SaveRecords", true)) _InitMenuTabCollection.Remove("history");
                    }

                    //BindMenuTabCollection();//绑定标签集合参数
                }

                return _InitMenuTabCollection;
            }
        }




 


        /// <summary>
        /// 绑定左菜单
        /// </summary>
        public void BindLeftMenu()
        {
            if (InitMenuTabCollection != null && InitMenuTabCollection.Count > 0)
            {
                List<MenuTabItem> TokenItems = InitMenuTabCollection.ToList();

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i=0;i< TokenItems.Count;i++ )
                {
                    if (String.IsNullOrEmpty(TokenItems[i].Parent) && TokenItems[i].Visible)
                    {
                         
                        MenuTabItem item = TokenItems[i];

                        if (!item.IsAdministrator || IsAdministrator)
                        {

                            String ChildHtml = String.Empty;
                            bool is_active = false;
                            bool is_Child = false;

                            List<MenuTabItem> ChildList = InitMenuTabCollection.FindByParent(TokenItems[i].Token);
                            if (ChildList.Count > 0)
                            {
                                ChildHtml = BindLeftMenuBySubmenu(ChildList, out is_active);
                                is_Child = true;
                            }

                            if (item.Token.ToLower() == Token.ToLower())
                            {
                                is_active = true;
                            }

                            String active_class = is_active ? "active open" : "";
                            String language = WebHelper.GetStringParam(Request, "language", PortalSettings.DefaultLanguage);
                            String item_url = (ChildList != null && ChildList.Count > 0) || String.IsNullOrEmpty(item.Src) ? "javascript:;" : String.Format("{0}Index_Manager.aspx?PortalId={1}&TabId={2}&ModuleId={3}&Token={4}&language={5}{6}", ModulePath, PortalId, TabId, ModuleId, item.Token,language, ConvertParameter(item.Parameter)); ;
                            String item_icon = string.IsNullOrEmpty(item.Icon) ? "" : String.Format("<i class=\"{0}\"></i>", item.Icon);

                            sb.AppendFormat("<li class=\"{0}\">", active_class).AppendLine();

                            sb.AppendFormat("<a href=\"{0}\">{1} <span class=\"{2}\"> {2} </span> <span class=\"selected\"></span>", item_url, item_icon, ViewMenuText(item));
                            if (is_Child)
                            {
                                sb.Append("<i class=\"icon-arrow\"></i> ");
                            }

                            sb.Append("</a>").AppendLine();
                            sb.Append(ChildHtml).AppendLine();

                            sb.Append("</li>").AppendLine();
                        }
                    }
                }
                liLeftMenu.Text = sb.ToString();


            }
        }




        /// <summary>
        /// 绑定左边子菜单
        /// </summary>
        public String BindLeftMenuBySubmenu(List<MenuTabItem> TokenItems, out Boolean is_active)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            is_active = false;
            sb.Append("<ul class=\"sub-menu\">").AppendLine();
            foreach (MenuTabItem item in TokenItems)
            {
                
                    String active_class = String.Empty;
                    if (item.Token.ToLower() == Token.ToLower() || (!String.IsNullOrEmpty(item.Link) &&   item.Link.ToLower() == Token.ToLower()))
                    {
                        is_active = true;
                        active_class = "active open";
                    }

                    if (item.Visible)
                    {
                        if ( !item.IsAdministrator || IsAdministrator)
                        {

                            String language = WebHelper.GetStringParam(Request, "language", PortalSettings.DefaultLanguage);
                            String item_url = String.Format("{0}Index_Manager.aspx?PortalId={1}&TabId={2}&ModuleId={3}&Token={4}&language={5}{6}", ModulePath, PortalId, TabId, ModuleId, item.Token, language, ConvertParameter(item.Parameter)); ;
                            String item_icon = string.IsNullOrEmpty(item.Icon) ? "" : String.Format("<i class=\"{0}\"></i>", item.Icon);

                            sb.AppendFormat("<li class=\"{0}\">", active_class).AppendLine();

                            sb.AppendFormat("<a href=\"{0}\">{1} <span class=\"title\"> {2} </span> </a>", item_url, item_icon, ViewMenuText(item)).AppendLine();

                            sb.Append("</li>").AppendLine();
                        }
                    }
            }
            sb.Append("</ul>").AppendLine();

            return sb.ToString();
        }


        public String ViewMenuText(MenuTabItem item)
        {
            String ResourceName = String.Format("Menu_{0}",item.Token);
            return ViewResourceText(ResourceName, item.Title);
        }


        public String ConvertParameter(String parameter)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (!String.IsNullOrEmpty(parameter))
            {
                List<String> ss = Common.GetList(parameter);
                foreach (String s in ss)
                {
                    sb.AppendFormat("&{0}",s);
                }
            }
            return sb.ToString();
        
        }



        /// <summary>
        /// 绑定控件到容器
        /// </summary>
        public void BindContainer()
        {


            MenuTabItem _MenuTabItem = InitMenuTabCollection.ContainsKey(Token) ? InitMenuTabCollection[Token] : new MenuTabItem();

               if (_MenuTabItem != null && !String.IsNullOrEmpty(_MenuTabItem.Token) && !this.DesignMode)
               {
                   //判断是否为管理员菜单，当前用户是否为管理员
                   if (_MenuTabItem.IsAdministrator && !IsAdministrator)
                   {
                       phContainer.Visible = false;
                       mTips.MsgType = EnumTips.Warning;
                       mTips.Content = ViewResourceText("HasModuleAccess", "You are not permitted to access this page! :(");
                       mTips.Put();
                   }
                   else
                   {
                       //加载相应的控件
                       basePortalModule ManageContent = new basePortalModule();
                       string ContentSrc = ResolveClientUrl(string.Format("{0}/{1}", this.TemplateSourceDirectory, _MenuTabItem.Src));

                       if (System.IO.File.Exists(MapPath(ContentSrc)))
                       {
                           ManageContent = (basePortalModule)LoadControl(ContentSrc);
                           ManageContent.ModuleConfiguration = ModuleConfiguration;
                           ManageContent.ID = _MenuTabItem.Token;
                           ManageContent.LocalResourceFile = Localization.GetResourceFile(this, string.Format("{0}.resx", _MenuTabItem.Src));
                           phContainer.Controls.Add(ManageContent);
                       }
                   }

                   //标题
                   Page.Title = String.Format("{0} - {1} - {2}", ViewMenuText(_MenuTabItem), ModuleConfiguration.ModuleTitle, PortalSettings.ActiveTab.LocalizedTabName);
           
               }
               else if (!String.IsNullOrEmpty(Token) && Token.ToLower() == "error")
               {
                   //加载相应的控件
                   basePortalModule ManageContent = new basePortalModule();
                   string ContentSrc = ResolveClientUrl(string.Format("{0}/{1}", this.TemplateSourceDirectory, "Manager_ErrorCatch.ascx"));

                   if (System.IO.File.Exists(MapPath(ContentSrc)))
                   {
                       ManageContent = (basePortalModule)LoadControl(ContentSrc);
                       ManageContent.ModuleConfiguration = ModuleConfiguration;
                       ManageContent.ID = "ErrorCatch";
                       ManageContent.LocalResourceFile = Localization.GetResourceFile(this, string.Format("{0}.resx", "Manager_ErrorCatch.ascx"));
                       phContainer.Controls.Add(ManageContent);
                   }
                   //标题
                   Page.Title = String.Format("{0} - {1}", "Error", ModuleConfiguration.ModuleTitle);
          
               }

               //首页地址
               hlHome.NavigateUrl = Globals.NavigateURL(TabId);


             
        }



        #endregion


        #region "==事件=="
 

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MessageTips mt = new MessageTips();
            lblMessage.Text = mt.PostHtml(this);
            if (!String.IsNullOrEmpty(lblMessage.Text))
                lblMessage.Visible = true;
            else
                lblMessage.Visible = false;

              Literal LiBreadcrumb = FindControl("LiBreadcrumb") as Literal;
              if (LiBreadcrumb != null)
              {
                  System.Text.StringBuilder sb = new System.Text.StringBuilder();

                  sb.Append("		<ol class=\"breadcrumb\">").AppendLine();
                  sb.Append("         <li>").AppendLine();
                  sb.Append("				<i class=\"fa clip-leaf\"></i>").AppendLine();
                  sb.AppendFormat("				<a href=\"{1}\">{0}</a>", Settings_EffectName, xUrl("EffectList")).AppendLine();
                  sb.Append("			</li>").AppendLine();
                  sb.AppendFormat("         <li class=\"active\"><a href=\"{1}\">{0}</a></li>", Settings_EffectThemeName, xUrl("EffectOptions")).AppendLine();
                  sb.Append("		</ol>").AppendLine();



                  LiBreadcrumb.Text = sb.ToString();
              }
           
        }



        protected override void Page_Init(System.Object sender, System.EventArgs e)
        {
            //调用基类Page_Init，主要用于权限验证
            base.Page_Init(sender, e);
 
            try
            {
                if (!IsPostBack)
                {
                    //VerificationAuthor();


                    DotNetNuke.Entities.Modules.ModuleInfo m = ModuleConfiguration;
                    litModuleTitle.Text = ModuleProperty(m, "ModuleName");
                    litModuleVersion.Text = ModuleProperty(m, "Version");

                    String Downloadlink = String.Empty;
                    String latestversion = String.Empty;
                    //litUpdateVersion.Text = Common.LoadUpdateVersionBy2(m.ModuleName, m.Version, out Downloadlink, out latestversion);
                    hlModuleLink.NavigateUrl = Downloadlink;
                    hlModuleLink.Attributes.Add("data-original-title", String.Format(ViewResourceText("latest_version", "Click to download the latest version:{0}"), latestversion));
                }



                //LoadManagerScript();

                //绑定Tabs和容器中的控件
                BindContainer();


                // 绑定左菜单
                BindLeftMenu();

                 

            }
            catch (Exception exc) //Module failed to load
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

   








        
    }
}