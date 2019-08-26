using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using System.IO;

namespace DNNGo.Modules.PowerForms
{
    public partial class Manager_EffectOptions : basePortalModule
    {


        #region "==属性=="

        /// <summary>
        /// 模块操作类
        /// </summary>
        private static ModuleController controller = new ModuleController();




        /// <summary>提示操作类</summary>
        MessageTips mTips = new MessageTips();

        #endregion


        #region "==方法=="

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindDataToPage()
        {
            EffectDB XmlDB = Setting_EffectDB;
            if (!(XmlDB != null && String.IsNullOrEmpty(XmlDB.Name)))
            {
                lblEffectName.Text = XmlDB.Name;
                lblEffectDescription.Text = XmlDB.Description;

                //绑定效果的主题
                String EffectDirPath = String.Format("{0}Effects/{1}/Themes/", Server.MapPath(ModulePath), XmlDB.Name);
                DirectoryInfo EffectDir = new DirectoryInfo(EffectDirPath);
                if (!EffectDir.Exists) EffectDir.Create();
                DirectoryInfo[] ThemeDirs = EffectDir.GetDirectories();
                List<KeyValueEntity> dirs = new List<KeyValueEntity>();
                if (ThemeDirs != null && ThemeDirs.Length > 0)
                {
                    foreach (DirectoryInfo dir in ThemeDirs)
                    {
                        KeyValueEntity dirEntity = new KeyValueEntity();
                        dirEntity.Key = dir.Name;

                        FileInfo imgFile = new FileInfo(MapPath(String.Format("{0}Effects/{1}/Themes/{2}/image.jpg", ModulePath, XmlDB.Name, dir.Name)));
                        if (imgFile.Exists)
                            dirEntity.Value = String.Format("{0}Effects/{1}/Themes/{2}/image.jpg", ModulePath, XmlDB.Name, dir.Name);
                        else
                            dirEntity.Value = String.Format("http://www.dnngo.net/DesktopModules/DNNGo_PowerForms/Effects/{0}/Themes/{1}/image.jpg", XmlDB.Name, dir.Name);


                        dirs.Add(dirEntity);
                    }
                    WebHelper.BindList<KeyValueEntity>(ddlThemeName, dirs, "Key", "Value");
                }
                WebHelper.SelectedListByText(ddlThemeName, Settings_EffectThemeName);


                // hfThemeThumbnails.Value = String.Format("{0}Effects/{1}/Themes/[EffectThemeName]/image.jpg", ModulePath, XmlDB.Name);
                if (!String.IsNullOrEmpty(Settings_EffectThemeName))
                {
                    imgThemeThumbnails.Attributes.Add("onError", String.Format("this.src='{0}Resource/images/no_image.png'", ModulePath));
                    imgThemeThumbnails.ToolTip = Settings_EffectThemeName;
                    KeyValueEntity dirEntity = dirs.Find(r1 => r1.Key.IndexOf(Settings_EffectThemeName, StringComparison.CurrentCultureIgnoreCase) >= 0);
                    imgThemeThumbnails.ImageUrl = dirEntity != null ? dirEntity.Value.ToString() : "";
                    imgThemeThumbnails.Visible = true;
                }


            }


            ////获取效果参数
            //List<SettingEntity> EffectSettingDB = Setting_SkinEffectSettingDB;

            //if (EffectSettingDB != null && EffectSettingDB.Count > 0)
            //{
            //    //绑定参数项
            //    RepeaterTheme.DataSource = EffectSettingDB;
            //    RepeaterTheme.DataBind();
            //}
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


        /// <summary>
        /// 设置数据项
        /// </summary>
        private void SetDataItem()
        {
            SetThemeSettings();


            UpdateModuleSetting("PowerForms_EffectThemeName", ddlThemeName.Items[ddlThemeName.SelectedIndex].Text);

        }




        /// <summary>
        /// 设置模版参数的值
        /// </summary>
        private void SetThemeSettings()
        {


            //获取效果参数
            List<SettingEntity> EffectSettingDB = Setting_EffectSettingDB;

            if (EffectSettingDB != null && EffectSettingDB.Count > 0)
            {
                ControlHelper ControlItem = new ControlHelper(ModuleId);

                foreach (SettingEntity ri in EffectSettingDB)
                {

                    controller.UpdateModuleSetting(this.ModuleId, EffectSettingsFormat(lblEffectName.Text, ri.Name), ControlHelper.GetWebFormValue(ri, this));
                }
            }

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
                BindGroupsToPage();
            }
            catch (Exception ex)
            {
                ProcessModuleLoadException(ex);
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

                Response.Redirect(xUrl("EffectOptions"), false);
            }
            catch (Exception ex)
            {
                ProcessModuleLoadException(ex);
            }
        }
        /// <summary>
        /// 返回
        /// </summary>
        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            try
            {

                Response.Redirect(xUrl("EffectList"), false);
            }
            catch (Exception ex)
            {
                ProcessModuleLoadException(ex);
            }
        }

        ///// <summary>
        ///// 行数据绑定
        ///// </summary>
        //protected void RepeaterTheme_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        SettingEntity ThemeSetting = e.Item.DataItem as SettingEntity;


        //        String SettingKey = SettingsFormat(lblEffectName.Text, ThemeSetting.Name);
        //        ThemeSetting.DefaultValue = Settings[SettingKey] != null ? Convert.ToString(Settings[SettingKey]) : ThemeSetting.DefaultValue;


        //        //构造输入控件
        //        PlaceHolder ThemePH = e.Item.FindControl("ThemePH") as PlaceHolder;

        //        #region "创建控件"
        //        Literal liControl = new Literal();

        //        ControlHelper ctl = new ControlHelper(ModuleId);
        //        liControl.Text = ctl.ViewControl(ThemeSetting);

        //        ThemePH.Controls.Add(liControl);
        //        #endregion

        //        Literal liTitle = e.Item.FindControl("liTitle") as Literal;
        //        liTitle.Text = ViewControlTitle(ThemeSetting.Name, !String.IsNullOrEmpty(ThemeSetting.Alias) ? ThemeSetting.Alias : ThemeSetting.Name, ctl.ViewControlID(ThemeSetting), ":", "col-sm-3 control-label");// ViewTitleSpan(!String.IsNullOrEmpty(ThemeSetting.Alias) ? ThemeSetting.Alias : ThemeSetting.Name, "", ClientID);

        //        Literal liHelp = e.Item.FindControl("liHelp") as Literal;
        //        //liHelp.Text = ViewHelp(String.Format("lbh{0}", ThemeSetting.Name), ThemeSetting.Description.Trim());
        //        liHelp.Text = ThemeSetting.Description.Trim();
        //    }
        //}




        /// <summary>
        /// 绑定选项分组框到页面
        /// </summary>
        private void BindGroupsToPage()
        {

            //获取效果参数
            List<GroupEntity> ItemSettingDB = Setting_EffectGroupsDB;

            if (!(ItemSettingDB != null && ItemSettingDB.Count > 0))
            {
                //当分组都没配置的时候，需要取出参数里面的分组
               
                foreach (var ItemSetting in Setting_EffectSettingDB)
                {
                    if (!ItemSettingDB.Exists(r1 => r1.Name == ItemSetting.Group))
                    {
                        ItemSettingDB.Add(new GroupEntity { Name = ItemSetting.Group, Layout = ItemSetting.Layout });
                    }
                }
            }
   

            if (ItemSettingDB != null && ItemSettingDB.Count > 0)
            {

   
                if (ItemSettingDB != null && ItemSettingDB.Count > 0)
                {

                    //绑定参数项
                    RepeaterGroup.DataSource = ItemSettingDB;
                    RepeaterGroup.DataBind();
                }
                divOptions.Visible = true;

            }





            ////获取效果参数
            //List<SettingEntity> ItemSettingDB = Setting_EffectSettingDB;

            //if (ItemSettingDB != null && ItemSettingDB.Count > 0)
            //{

            //    List<KeyValueEntity> Items = new List<KeyValueEntity>();
            //    foreach (SettingEntity ItemSetting in ItemSettingDB)
            //    {
            //        if (!Items.Exists(r1 => r1.Key == ItemSetting.Group))
            //        {
            //            Items.Add(new KeyValueEntity(ItemSetting.Group, ""));
            //        }
            //    }

            //    if (Items != null && Items.Count > 0)
            //    {
            //        //绑定参数项
            //        RepeaterGroup.DataSource = Items;
            //        RepeaterGroup.DataBind();
            //    }
            //    divOptions.Visible = true;
            //}
        }




        /// <summary>
        /// 绑定选项集合到页面
        /// </summary>
        private void BindOptionsToPage(Repeater RepeaterOptions, String Group, out int OptionCount)
        {
            OptionCount = 0;
            //获取效果参数
            List<SettingEntity> ItemSettingDB = Setting_EffectSettingDB;

            if (ItemSettingDB != null && ItemSettingDB.Count > 0)
            {
                ItemSettingDB = ItemSettingDB.FindAll(r1 => r1.Group == Group);
                OptionCount = ItemSettingDB.Count;
                //绑定参数项
                RepeaterOptions.DataSource = ItemSettingDB;
                RepeaterOptions.DataBind();
            }
        }



        protected void RepeaterOptions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SettingEntity ThemeSetting = e.Item.DataItem as SettingEntity;

                String SettingKey = EffectSettingsFormat(lblEffectName.Text, ThemeSetting.Name);
                ThemeSetting.DefaultValue = Settings[SettingKey] != null ? Convert.ToString(Settings[SettingKey]) : ThemeSetting.DefaultValue;


                //构造输入控件
                PlaceHolder ThemePH = e.Item.FindControl("ThemePH") as PlaceHolder;

                #region "创建控件"
                ControlHelper ctl = new ControlHelper(this);
                ctl.IconPath = "Effects";
                ThemePH.Controls.Add((Control)ctl.ViewControl(ThemeSetting));
                #endregion

                Literal liTitle = e.Item.FindControl("liTitle") as Literal;
                liTitle.Text = String.Format("<label class=\"col-sm-3 control-label\" for=\"{1}\">{0}:</label>", !String.IsNullOrEmpty(ThemeSetting.Alias) ? ThemeSetting.Alias : ThemeSetting.Name, ctl.ViewControlID(ThemeSetting));

                if (!String.IsNullOrEmpty(ThemeSetting.Description))
                {
                    Literal liHelp = e.Item.FindControl("liHelp") as Literal;
                    liHelp.Text = String.Format("<span class=\"help-block\"><i class=\"fa fa-info-circle\"></i> {0}</span>", ThemeSetting.Description);
                }
            }
        }


        /// <summary>
        /// 分组绑定事件
        /// </summary>
        protected void RepeaterGroup_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater RepeaterOptions = e.Item.FindControl("RepeaterOptions") as Repeater;
                GroupEntity GroupItem = e.Item.DataItem as GroupEntity;
                int OptionCount = 0;
                BindOptionsToPage(RepeaterOptions, GroupItem.Name, out OptionCount);

                if (OptionCount == 0)
                {
                    e.Item.Visible = false;
                }

            }
        }


        #endregion









    }
}
