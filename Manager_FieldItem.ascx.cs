using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Localization;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Globalization;

namespace DNNGo.Modules.PowerForms
{
    public partial class Manager_FieldItem : basePortalModule
    {
        /// <summary>
        /// 提示操作类
        /// </summary>
        MessageTips mTips = new MessageTips();


        #region "属性"

        /// <summary>
        /// 字段编号
        /// </summary>
        public Int32 FieldID = WebHelper.GetIntParam(HttpContext.Current.Request, "FieldID", 0);


        private DNNGo_PowerForms_Field _FieldItem;
        /// <Description>
        /// 文章项
        /// </Description>
        public DNNGo_PowerForms_Field FieldItem
        {
            get
            {
                if (!(_FieldItem != null && _FieldItem.ID > 0))
                {
                    if (FieldID > 0)
                        _FieldItem = DNNGo_PowerForms_Field.FindByKeyForEdit(FieldID);
                    else
                        _FieldItem = new DNNGo_PowerForms_Field();
                }
                return _FieldItem;
            }
        }


        private List<KeyValueEntity> _ItemSettings;
        /// <Description>
        /// 封装的参数集合
        /// </Description>
        public List<KeyValueEntity> ItemSettings
        {
            get
            {
                if (!(_ItemSettings != null && _ItemSettings.Count > 0))
                {
                    if (FieldItem != null && FieldItem.ID > 0 && !String.IsNullOrEmpty(FieldItem.Options))
                    {
                        try
                        {
                            _ItemSettings = ConvertTo.Deserialize<List<KeyValueEntity>>(FieldItem.Options);
                        }
                        catch
                        {
                            _ItemSettings = new List<KeyValueEntity>();
                        }
                    }
                    else
                        _ItemSettings = new List<KeyValueEntity>();
                }
                return _ItemSettings;
            }
        }

        #endregion


        #region "事件"

        /// <summary>
        /// 页面加载
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (FieldID > 0)
                    {
                        cmdDelete.Enabled = true;
                        cmdDelete.Attributes.Add("onClick", "javascript:if(confirm('" + Localization.GetString("DeleteItem") + "')){ CancelValidation();return true;}else{return false;}");
                    }


                    // 绑定方案项
                    BindDataItem();
                }

                //绑定设置参数到页面
                BindGroupsToPage("Left");
                BindGroupsToPage("Right");
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {

                Int32 ArticleStatus = -1;
                DNNGo_PowerForms_Field fieldItem = new DNNGo_PowerForms_Field();
                Int32 SaveResult = SetDataItem(ArticleStatus, out fieldItem);



                if (SaveResult > 0)
                {
                    mTips.LoadMessage("SaveFieldSuccess", EnumTips.Success, this, new String[] { fieldItem.Name });
                    Response.Redirect(xUrl("FieldID", fieldItem.ID.ToString(), "AddNewField"), false);
                }
                else if (SaveResult == -1)
                {
                    mTips.IsPostBack = true;
                    mTips.LoadMessage("AddFieldOverlap", EnumTips.Error, this, new String[] { fieldItem.Name });
                }
                else
                {
                    mTips.IsPostBack = true;
                    mTips.LoadMessage("SaveFieldError", EnumTips.Error, this, new String[] { "" });
                }
 

    

            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            try
            {

                Response.Redirect(xUrl("FieldList"));
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {


                //查出相关的信息,然后修改状态
                DNNGo_PowerForms_Field ExtendFieldItem = DNNGo_PowerForms_Field.FindByKeyForEdit(FieldID);
                if (ExtendFieldItem != null && ExtendFieldItem.ID > 0)
                {
                    if (ExtendFieldItem.Delete() > 0)
                    {
                        //字段删除成功
                        mTips.LoadMessage("DeleteFieldSuccess", EnumTips.Success, this, new String[] { ExtendFieldItem.Name });
                    }
                    else
                    {
                        //字段删除失败
                        mTips.LoadMessage("DeleteFieldError", EnumTips.Error, this, new String[] { ExtendFieldItem.Name });
                    }

                }



                Response.Redirect(xUrl("FieldList"));
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

    




        protected void RepeaterOptions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SettingEntity ThemeSetting = e.Item.DataItem as SettingEntity;

                Boolean IsRightLayout = !String.IsNullOrEmpty(ThemeSetting.Layout) && ThemeSetting.Layout.IndexOf("Right", StringComparison.CurrentCultureIgnoreCase) >= 0;

                KeyValueEntity KeyValue = ItemSettings.Find(r1 => r1.Key == ThemeSetting.Name);
                if (KeyValue != null && !String.IsNullOrEmpty(KeyValue.Key))
                {
                    ThemeSetting.DefaultValue = KeyValue.Value.ToString();
                }

                //构造输入控件
                PlaceHolder ThemePH = e.Item.FindControl("ThemePH") as PlaceHolder;

                #region "创建控件"


                ControlHelper ctl = new ControlHelper(this);

                ThemePH.Controls.Add((Control)ctl.ViewControl(ThemeSetting));
                #endregion

                Literal liTitle = e.Item.FindControl("liTitle") as Literal;


                liTitle.Text = String.Format("<label class=\"col-sm-{2} control-label\" for=\"{1}\">{0}:</label>", !String.IsNullOrEmpty(ThemeSetting.Alias) ? ThemeSetting.Alias : ThemeSetting.Name, ctl.ViewControlID(ThemeSetting), IsRightLayout ? 5 : 3);

                if (!String.IsNullOrEmpty(ThemeSetting.Description))
                {
                    Literal liHelp = e.Item.FindControl("liHelp") as Literal;
                    liHelp.Text = String.Format("<span class=\"help-block\"><i class=\"fa fa-info-circle\"></i> {0}</span>", ThemeSetting.Description);
                }
            }
        }


        /// <Description>
        /// 分组绑定事件
        /// </Description>
        protected void RepeaterGroup_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater RepeaterOptions = e.Item.FindControl("RepeaterOptions") as Repeater;
                KeyValueEntity GroupItem = e.Item.DataItem as KeyValueEntity;
                int OptionCount = 0;
                BindOptionsToPage(RepeaterOptions, GroupItem.Key, out OptionCount);

                if (OptionCount == 0)
                {
                    e.Item.Visible = false;
                }

            }
        }

        #endregion

        #region "方法"

        /// <summary>
        /// 绑定方案项
        /// </summary>
        private void BindDataItem()
        {


            //取出当前的方案实体,并绑定到相应的控件上
            DNNGo_PowerForms_Field fieldItem = FieldItem;

            if (fieldItem == null)
            {
                fieldItem = new DNNGo_PowerForms_Field();
            }

            if (fieldItem.ID > 0)
            {
                //更新方案
                //需要将几个控件设置为不可操作状态
                txtName.Enabled = false;
                ddlControlType.Enabled = false;
            }
            else
            {
                //新增方案
            }

            //绑定字段类型
            //BindFieldTypeView(fieldItem.FieldType); 


            EffectDB EffectDB = Setting_EffectDB;
            divGroup.Visible = EffectDB.Group;
            if (EffectDB.Group)
            {

                BindTreeGroups(fieldItem);
            }

            //增加权限用户
            DotNetNuke.Security.Roles.RoleController rc = new DotNetNuke.Security.Roles.RoleController();
            WebHelper.BindList(cblPermissionsRoles, rc.GetPortalRoles(PortalId), "RoleName", "RoleName");
            WebHelper.SelectedListMultiByValue(cblPermissionsRoles, fieldItem.Per_Roles);

            cbPermissionsAllUsers.Checked = fieldItem.Per_AllUsers == 0 ? true : false;

            WebHelper.BindList(ddlControlType, typeof(EnumViewControlType));
            WebHelper.SelectedListByValue(ddlControlType, fieldItem.FieldType);

            WebHelper.BindList(ddlVerification, typeof(EnumVerification));
            WebHelper.SelectedListByValue(ddlVerification, fieldItem.Verification);

            WebHelper.BindList(rblFTDirection, typeof(EnumControlDirection));
            WebHelper.SelectedListByValue(rblFTDirection, fieldItem.Direction);



            List<DNNGo_PowerForms_Field> FieldControls = DNNGo_PowerForms_Field.FindAllByModuleId(ModuleId);

            WebHelper.BindList<DNNGo_PowerForms_Field>(ddlFTEqualsControl, FieldControls, "Name", "ID");
            WebHelper.BindItem(ddlFTEqualsControl,ViewResourceText("lblDDRSelect", "==Please select=="),"0");
            WebHelper.SelectedListByValue(ddlFTEqualsControl, fieldItem.EqualsControl);

            WebHelper.BindList<DNNGo_PowerForms_Field>(ddlFTAssociatedControl, FieldControls.FindAll(r=>r.FieldType == (Int32)EnumViewControlType.DropDownList_Country), "Name", "ID");
            WebHelper.BindItem(ddlFTAssociatedControl, ViewResourceText("lblDDRSelect", "==Please select=="), "0");
            WebHelper.SelectedListByValue(ddlFTAssociatedControl, fieldItem.AssociatedControl);


            txtName.Text = fieldItem.Name;
            txtDescription.Text = fieldItem.Description;
            txtAlias.Text = fieldItem.Alias;
            txtToolTip.Text = fieldItem.ToolTip;
            txtFTDefaultValue.Text = fieldItem.DefaultValue;
            txtTinymceDefaultValue.Text = fieldItem.DefaultValue;
            txtFTListCollection.Text = fieldItem.FiledList;
            txtFTRows.Text = fieldItem.Rows.ToString();
            WebHelper.SelectedListByValue(rblFTDirection, fieldItem.Direction);
            txtFTListColumn.Text = fieldItem.ListColumn.ToString();

            txtFTInputLength.Text = fieldItem.InputLength.ToString();
         
            txtFTWidth.Text = fieldItem.Width.ToString();
            WebHelper.BindList(ddlFTWidth, typeof(EnumWidthSuffix));
            WebHelper.SelectedListByValue(ddlFTWidth, fieldItem.WidthSuffix);
           

          
 


            //文章状态
            cbStatus.Checked = fieldItem.Status == (Int32)EnumStatus.Activation;
            //是否必填
            cbRequired.Checked = fieldItem.Required == 1;

            //发布时间和结束时间
            if (FieldID > 0 && fieldItem != null && fieldItem.ID > 0)
            {
                liStartDateTime.Text = fieldItem.StartTime.ToString("MM/dd/yyyy hh:mm tt", new CultureInfo("en-US", false));//Thread.CurrentThread.CurrentCulture
                liDisableDateTime.Text = fieldItem.EndTime.ToString("MM/dd/yyyy hh:mm tt", new CultureInfo("en-US", false));//Thread.CurrentThread.CurrentCulture
            }
            txtStartDate.Text = fieldItem.StartTime.ToString("MM/dd/yyyy", new CultureInfo("en-US", false));
            txtStartTime.Text = fieldItem.StartTime.ToString("hh:mm tt", new CultureInfo("en-US", false));

            txtDisableDate.Text = fieldItem.EndTime.ToString("MM/dd/yyyy", new CultureInfo("en-US", false));
            txtDisableTime.Text = fieldItem.EndTime.ToString("hh:mm tt", new CultureInfo("en-US", false));


        }
  


        /// <Description>
        /// 绑定树分类
        /// </Description>
        private void BindTreeGroups(DNNGo_PowerForms_Field Article)
        {
            List<Int32> SelectList = new List<Int32>();


            //绑定所有分类到页面
            QueryParam qp = new QueryParam();
            qp.Orderfld = DNNGo_PowerForms_Group._.Sort;
            qp.OrderType = 0;
            int RecordCount = 0;
            qp.Where.Add(new SearchParam(DNNGo_PowerForms_Group._.ModuleId, ModuleId, SearchType.Equal));
            List<DNNGo_PowerForms_Group> lst = DNNGo_PowerForms_Group.FindAll(qp, out RecordCount);

            StringBuilder GroupListContent = new StringBuilder();
            foreach (var Group in lst)
            {
                GroupListContent.AppendFormat("{0}:{1}", Group.Name, Group.ID).AppendLine();
            }

            //拼接顶级分类的方法
            ControlHelper ctl = new ControlHelper(this);

            SettingEntity GroupControl = new SettingEntity();
            GroupControl.Name = "GroupControl";
            GroupControl.Alias = "GroupControl";
            GroupControl.ControlType = EnumViewControlType.RadioButtonList.ToString();
            GroupControl.DefaultValue = Article.GroupID.ToString();
            GroupControl.ListContent = GroupListContent.ToString(); 

            PHGroups.Controls.Add((Control)ctl.ViewControl(GroupControl));
   



        }







        /// <summary>
        /// 设置方案项
        /// </summary>
        private Int32 SetDataItem(Int32 ArticleStatus, out DNNGo_PowerForms_Field fieldItem)
        {
            fieldItem = FieldItem;

            //权限
            fieldItem.Per_AllUsers = cbPermissionsAllUsers.Checked ? 0 : 1;

            String textStr, idStr = String.Empty;
            WebHelper.GetSelected(cblPermissionsRoles, out textStr, out idStr);
            fieldItem.Per_Roles = idStr;


            if (ddlControlType.Enabled) fieldItem.FieldType = WebHelper.GetIntParam(Request, ddlControlType.UniqueID, (Int32)EnumViewControlType.TextBox);

            fieldItem.ToolTip = txtToolTip.Text;
            fieldItem.Alias = txtAlias.Text;
            fieldItem.Description = txtDescription.Text;
            fieldItem.Required = cbRequired.Checked ? 1 : 0;



            fieldItem.Options = SetItemSettings();


            if (divGroup.Visible)
            {
                SettingEntity GroupControl = new SettingEntity();
                GroupControl.Name = "GroupControl";
                Int32 GroupID = 0;
                if (int.TryParse(ControlHelper.GetWebFormValue(GroupControl, this), out GroupID))
                {
                }
                fieldItem.GroupID = GroupID;
            }

            fieldItem.FiledList = txtFTListCollection.Text;

            if (fieldItem.FieldType == (Int32)EnumViewControlType.Html || fieldItem.FieldType == (Int32)EnumViewControlType.RichTextBox)
            {
                fieldItem.DefaultValue = txtTinymceDefaultValue.Text;
            }
            else
            {
                fieldItem.DefaultValue = txtFTDefaultValue.Text;
            }
            fieldItem.Rows = WebHelper.GetIntParam(Request, txtFTRows.UniqueID, 1);
            fieldItem.Verification = WebHelper.GetIntParam(Request, ddlVerification.UniqueID, 0);
            fieldItem.Direction = WebHelper.GetIntParam(Request, rblFTDirection.UniqueID, 0);
            fieldItem.ListColumn = WebHelper.GetIntParam(Request, txtFTListColumn.UniqueID, 1);

            fieldItem.Width = WebHelper.GetIntParam(Request, txtFTWidth.UniqueID, 100);
            fieldItem.WidthSuffix = WebHelper.GetIntParam(Request, ddlFTWidth.UniqueID, 0);

            fieldItem.EqualsControl = WebHelper.GetIntParam(Request, ddlFTEqualsControl.UniqueID, 0);
            fieldItem.AssociatedControl = WebHelper.GetIntParam(Request, ddlFTAssociatedControl.UniqueID, 0);
            fieldItem.InputLength = WebHelper.GetIntParam(Request, txtFTInputLength.UniqueID, 2000);

            fieldItem.LastTime = xUserTime.UtcTime();
            fieldItem.LastUser = UserId;
            fieldItem.LastIP = WebHelper.UserHost;

            //发布状态和时间
            DateTime oTime = xUserTime.LocalTime();
            string[] expectedFormats = { "G", "g", "f", "F" };
            string StartDate = WebHelper.GetStringParam(Request, txtStartDate.UniqueID, oTime.ToString("MM/dd/yyyy"));
            string StartTime = WebHelper.GetStringParam(Request, txtStartTime.UniqueID, oTime.ToString("hh:mm tt"));
            if (DateTime.TryParseExact(String.Format("{0} {1}", StartDate, StartTime), "MM/dd/yyyy hh:mm tt", new CultureInfo("en-US", false), DateTimeStyles.AllowWhiteSpaces, out oTime))
            {
                fieldItem.StartTime = oTime;
            }
            //发布状态和时间
            DateTime EndTime = xUserTime.LocalTime().AddYears(10);
            string DisableDate = WebHelper.GetStringParam(Request, txtDisableDate.UniqueID, EndTime.ToString("MM/dd/yyyy"));
            string DisableTime = WebHelper.GetStringParam(Request, txtDisableTime.UniqueID, EndTime.ToString("hh:mm tt"));
            if (DateTime.TryParseExact(String.Format("{0} {1}", DisableDate, DisableTime), "MM/dd/yyyy hh:mm tt", new CultureInfo("en-US", false), DateTimeStyles.AllowWhiteSpaces, out EndTime))
            {
                fieldItem.EndTime = EndTime;
            }

            if (ArticleStatus == -1)//如果没有指定状态就取控件的
            {
                fieldItem.Status = cbStatus.Checked ? (Int32)EnumStatus.Activation : (Int32)EnumStatus.Hide;
            }
            else
            {
                fieldItem.Status = ArticleStatus;
            }


            Int32 SaveResult = 0;
            if (fieldItem.ID > 0)
            {
                SaveResult = fieldItem.Update();
            }
            else
            {
                fieldItem.Name = txtName.Text;

                QueryParam qp = new QueryParam();
                qp.Where.Add(new SearchParam(DNNGo_PowerForms_Field._.ModuleId, ModuleId, SearchType.Equal));
                qp.Where.Add(new SearchParam(DNNGo_PowerForms_Field._.Name, fieldItem.Name, SearchType.Equal));
                if (DNNGo_PowerForms_Field.FindCount(qp) == 0)
                {

                    fieldItem.ModuleId = ModuleId;
                    fieldItem.PortalId = PortalId;


                    QueryParam Sqp = new QueryParam();
                    Sqp.ReturnFields = Sqp.Orderfld = DNNGo_PowerForms_Field._.Sort;
                    Sqp.OrderType = 1;
                    Sqp.Where.Add(new SearchParam(DNNGo_PowerForms_Field._.ModuleId, ModuleId, SearchType.Equal));
                    fieldItem.Sort = Convert.ToInt32(DNNGo_PowerForms_Field.FindScalar(Sqp)) + 2;



                    //构造默认的验证方式
                    if (fieldItem.FieldType == (Int32)EnumViewControlType.DatePicker)
                    {
                        fieldItem.Verification = (Int32)EnumVerification.date;
                    }
                    else if (fieldItem.FieldType == (Int32)EnumViewControlType.FileUpload)
                    {

                    }
                    else if (fieldItem.FieldType == (Int32)EnumViewControlType.TextBox_Email)
                    {
                        fieldItem.Verification = (Int32)EnumVerification.email;
                    }


                    SaveResult = fieldItem.Insert();
                }
                else
                {
                    SaveResult = -1;

                }
            }

            return SaveResult;
        }


        /// <Description>
        /// 绑定选项分组框到页面
        /// </Description>
        private void BindGroupsToPage(String Layout)
        {

            Repeater RepeaterGroup = FindControl(String.Format("RepeaterGroup_{0}", Layout)) as Repeater;
            HtmlGenericControl divOptions = FindControl(String.Format("divOptions_{0}", Layout)) as HtmlGenericControl;

            if (RepeaterGroup != null && divOptions != null)
            {
                //获取效果参数
                List<SettingEntity> ItemSettingDB = Setting_ItemSettingDB.FindAll(r => r.Layout == Layout);
                if (Layout != "Right")
                {
                    ItemSettingDB = Setting_ItemSettingDB.FindAll(r => r.Layout != "Right");
                }


                if (ItemSettingDB != null && ItemSettingDB.Count > 0)
                {

                    List<KeyValueEntity> Items = new List<KeyValueEntity>();
                    foreach (SettingEntity ItemSetting in ItemSettingDB)
                    {
                        if (!Items.Exists(r1 => r1.Key == ItemSetting.Group))
                        {
                            Items.Add(new KeyValueEntity(ItemSetting.Group, ""));
                        }
                    }

                    if (Items != null && Items.Count > 0)
                    {
                        //绑定参数项
                        RepeaterGroup.DataSource = Items;
                        RepeaterGroup.DataBind();
                    }
                    divOptions.Visible = true;
                }
            }
        }




        /// <Description>
        /// 绑定选项集合到页面
        /// </Description>
        private void BindOptionsToPage(Repeater RepeaterOptions, String Group, out int OptionCount)
        {
            OptionCount = 0;
            //获取效果参数
            List<SettingEntity> ItemSettingDB = Setting_ItemSettingDB;

            if (ItemSettingDB != null && ItemSettingDB.Count > 0)
            {
                ItemSettingDB = ItemSettingDB.FindAll(r1 => r1.Group == Group);
                OptionCount = ItemSettingDB.Count;
                //绑定参数项
                RepeaterOptions.DataSource = ItemSettingDB;
                RepeaterOptions.DataBind();
            }
        }

        /// <Description>
        /// 拼接数据项的设置参数
        /// </Description>
        /// <returns></returns>
        public String SetItemSettings()
        {
            //获取效果参数
            List<SettingEntity> ItemSettingDB = Setting_ItemSettingDB;
            List<KeyValueEntity> list = new List<KeyValueEntity>();

            if (ItemSettingDB != null && ItemSettingDB.Count > 0)
            {
                ControlHelper ControlItem = new ControlHelper(ModuleId);

                foreach (SettingEntity ri in ItemSettingDB)
                {
                    KeyValueEntity item = new KeyValueEntity();
                    item.Key = ri.Name;
                    item.Value = ControlHelper.GetWebFormValue(ri, this);
                    list.Add(item);
                }
            }
            return ConvertTo.Serialize<List<KeyValueEntity>>(list);
        }

        #endregion

    }
}