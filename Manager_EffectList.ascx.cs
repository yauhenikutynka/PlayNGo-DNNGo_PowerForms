using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DotNetNuke.Services.Localization;

namespace DNNGo.Modules.PowerForms
{
    public partial class Manager_EffectList : basePortalModule
    {


        #region "==属性=="





        /// <summary>
        /// 提示操作类
        /// </summary>
        MessageTips mTips = new MessageTips();
        #endregion


        #region "==方法=="

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindDataToPage()
        {
            //构造效果存放路径
            String EffectDirPath = String.Format("{0}Effects/", Server.MapPath(ModulePath));
            DirectoryInfo EffectDir = new DirectoryInfo(EffectDirPath);
            if (!EffectDir.Exists) EffectDir.Create();//不存在就创建
            //获取当前所有的目录
            DirectoryInfo[] EffectDirs = EffectDir.GetDirectories();

  


            //绑定数据
            gvEffectList.DataSource = EffectDirs;
            gvEffectList.DataBind();


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
                ProcessModuleLoadException(ex);
            }
        }






        /// <summary>
        /// 效果列表行绑定
        /// </summary>
        protected void gvEffectList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DirectoryInfo EffectDir = e.Row.DataItem as DirectoryInfo;

                if (!EffectDir.Exists) EffectDir.Create();

                //获取效果数据的XML
                String EffectDBPath = String.Format("{0}\\EffectDB.xml", EffectDir.FullName);
                if (File.Exists(EffectDBPath))
                {
                    XmlFormat xf = new XmlFormat(EffectDBPath);

                    EffectDB EffectDB = xf.ToItem<EffectDB>();


                    //构造图片的路径
                    Image imgPicture = e.Row.FindControl("imgPicture") as Image;
                    imgPicture.Attributes.Add("onError", String.Format("this.src='{0}Resource/images/no_image.png'", ModulePath));
                    if (!String.IsNullOrEmpty(EffectDB.Thumbnails))
                    {
                        if (EffectDB.Thumbnails.IndexOf("http://", StringComparison.CurrentCultureIgnoreCase) >= 0)
                            imgPicture.ImageUrl = EffectDB.Thumbnails;
                        else
                            imgPicture.ImageUrl = String.Format("{0}Effects/{1}/{2}", ModulePath, EffectDB.Name, EffectDB.Thumbnails);
                    }

                    //构造效果标题描述
                    Label labName = e.Row.FindControl("labName") as Label;
                    Label labDescription = e.Row.FindControl("labDescription") as Label;
                    Label labVersion = e.Row.FindControl("labVersion") as Label;



                    labName.Text = EffectDB.Name;
                    labDescription.Text = EffectDB.Description;
                    labVersion.Text = EffectDB.Version;



                    //演示地址
                    if (!String.IsNullOrEmpty(EffectDB.DemoUrl))
                    {
                        Literal LiDemoUrl = e.Row.FindControl("LiDemoUrl") as Literal;
                        LiDemoUrl.Text = String.Format("<a href=\"{0}\" target=\"_blank\" class=\"btn btn-default btn-sm\"><i class=\"fa clip-link\"></i> {1}</a>", EffectDB.DemoUrl, ViewResourceText("labDemoUrl", "Demo Url"));
                    }

                    //响应式
                    if (EffectDB.Responsive)
                    {
                        Literal LiResponsive = e.Row.FindControl("LiResponsive") as Literal;
                        LiResponsive.Text = String.Format("<a  class=\"btn btn-default btn-sm\"><i class=\"fa clip-embed\"></i> {0}</a>", ViewResourceText("labResponsive", "Responsive"));
                    }



                    //应用效果的按钮
                    LinkButton btnApply = e.Row.FindControl("btnApply") as LinkButton;
                    HyperLink hlThemeName = e.Row.FindControl("hlThemeName") as HyperLink;
                    btnApply.CommandArgument = EffectDB.Name;
                    if (!String.IsNullOrEmpty(Settings_EffectName) && Settings_EffectName == EffectDB.Name)//判断设置效果是否为当前效果
                    {
                        btnApply.Text = String.Format("<i class=\"fa fa-stop\"></i> {1}", ModulePath, Localization.GetString("btnApply_Stop", this.LocalResourceFile));
                        btnApply.Enabled = false;
                        btnApply.CssClass = "btn btn-danger";


                        hlThemeName.Text = String.Format("<i class=\"fa clip-cogs\"></i> {0}", Settings_EffectThemeName);
                        hlThemeName.NavigateUrl = xUrl("EffectOptions");


                    }
                    else
                    {
                        btnApply.Text = String.Format("<i class=\"fa fa-play-circle-o\"></i> {1}", ModulePath, Localization.GetString("btnApply_Play", this.LocalResourceFile));
                        btnApply.CssClass = "btn btn-success";
                        hlThemeName.Visible = false;
                    }

                }



            }
        }

        /// <summary>
        /// 应用效果
        /// </summary>
        protected void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnApply = (LinkButton)sender;


                //要修改默认的主题样式
                String EffectDirPath = String.Format("{0}Effects/{1}/Themes/", Server.MapPath(ModulePath), btnApply.CommandArgument);
                DirectoryInfo EffectDir = new DirectoryInfo(EffectDirPath);
                if (!EffectDir.Exists) EffectDir.Create();
                DirectoryInfo[] ThemeDirs = EffectDir.GetDirectories();
                if (ThemeDirs != null && ThemeDirs.Length > 0)
                {
                    UpdateModuleSetting("PowerForms_EffectThemeName", ThemeDirs[0].Name);
                }


                UpdateModuleSetting("PowerForms_EffectName", btnApply.CommandArgument);

                mTips.LoadMessage("SaveEffectSuccess", EnumTips.Success, this, new String[] { btnApply.CommandArgument });

                Response.Redirect(xUrl("EffectList"), false);


            }
            catch (Exception ex)
            {
                ProcessModuleLoadException(ex);
            }
        }


        #endregion







    }
}
