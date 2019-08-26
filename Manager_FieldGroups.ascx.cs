using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Localization;

namespace DNNGo.Modules.PowerForms
{
    public partial class Manager_FieldGroups : basePortalModule
    {


        #region "属性"
        /// <summary>
        /// 标签编号
        /// </summary>
        private Int32 TagID = WebHelper.GetIntParam(HttpContext.Current.Request, "ID", 0);


        /// <summary>
        /// 当前页码
        /// </summary>
        public Int32 PageIndex = WebHelper.GetIntParam(HttpContext.Current.Request, "PageIndex", 1);

        /// <summary>
        /// 文章状态
        /// </summary>
        public Int32 ArticleStatus = WebHelper.GetIntParam(HttpContext.Current.Request, "Status", -1);



        /// <summary>
        /// 文章搜索_标题
        /// </summary>
        public String Search_Title = WebHelper.GetStringParam(HttpContext.Current.Request, "SearchText", "");

        /// <summary>
        /// 总页码数
        /// </summary>
        public Int32 RecordPages
        {
            get;
            set;
        }

        /// <summary>
        /// 当前页面URL(不包含分页)
        /// </summary>
        public String CurrentUrl
        {
            get
            {

                List<String> urls = new List<String>();

                if (ArticleStatus >= 0)
                {
                    urls.Add(String.Format("Status={0}", ArticleStatus));
                }

                if (!String.IsNullOrEmpty(Orderfld))
                {
                    urls.Add(String.Format("sort_f={0}", Orderfld));
                }

                if (OrderType > 0)
                {
                    urls.Add(String.Format("sort_t={0}", OrderType));
                }

                if (!String.IsNullOrEmpty(Search_Title))
                {
                    urls.Add(String.Format("SearchText={0}", Search_Title));
                }

                return xUrl("", "", "Groups", urls.ToArray());
            }
        }


        /// <summary>
        /// 排序字段
        /// </summary>
        public string Orderfld = WebHelper.GetStringParam(HttpContext.Current.Request, "sort_f", "Sort");


        /// <summary>
        /// 排序类型 1:降序 0:升序
        /// </summary>
        public int OrderType = WebHelper.GetIntParam(HttpContext.Current.Request, "sort_t", 0);

        /// <summary>
        /// 提示操作类
        /// </summary>
        MessageTips mTips = new MessageTips();




        private DNNGo_PowerForms_Group _GroupItem;
        /// <summary>
        /// 分类项
        /// </summary>
        public DNNGo_PowerForms_Group GroupItem
        {
            get
            {
                if (!(_GroupItem != null && _GroupItem.ID > 0))
                {
                    if (TagID > 0)
                        _GroupItem = DNNGo_PowerForms_Group.FindByKeyForEdit(TagID);
                    else
                        _GroupItem = new DNNGo_PowerForms_Group();
                }
                return _GroupItem;
            }
        }




        private List<KeyValueEntity> _ItemSettings;
        /// <summary>
        /// 封装的参数集合
        /// </summary>
        public List<KeyValueEntity> ItemSettings
        {
            get
            {
                if (!(_ItemSettings != null && _ItemSettings.Count > 0))
                {
                    if (GroupItem != null && GroupItem.ID > 0 && !String.IsNullOrEmpty(GroupItem.Options))
                    {
                        try
                        {
                            _ItemSettings = ConvertTo.Deserialize<List<KeyValueEntity>>(GroupItem.Options);
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


        #region "方法"


        /// <summary>
        /// 绑定查询的方法
        /// </summary>
        private List<SearchParam> BindSearch()
        {
            List<SearchParam> Where = new List<SearchParam>();
            Where.Add(new SearchParam(DNNGo_PowerForms_Group._.ModuleId, ModuleId, SearchType.Equal));

            if (!String.IsNullOrEmpty(Search_Title))
            {
                txtSearch.Text = HttpUtility.UrlDecode(Search_Title);
                Where.Add(new SearchParam(DNNGo_PowerForms_Group._.Name, HttpUtility.UrlDecode(Search_Title), SearchType.Like));
            }






            return Where;
        }

        /// <summary>
        /// 绑定右边列表
        /// </summary>
        private void BindDataList()
        {
            QueryParam qp = new QueryParam();
            qp.PageSize = 10;
            qp.PageIndex = PageIndex;
            qp.Orderfld = Orderfld;
            qp.OrderType = OrderType;
            int RecordCount = 0;


            //查询的方法
            qp.Where = BindSearch();

            List<DNNGo_PowerForms_Group> lst = DNNGo_PowerForms_Group.FindAll(qp, out RecordCount);

            qp.RecordCount = RecordCount;
            RecordPages = qp.Pages;
            lblRecordCount.Text = String.Format("{0} {2} / {1} {3}", RecordCount, RecordPages, ViewResourceText("Title_Items", "Items"), ViewResourceText("Title_Pages", "Pages"));

            gvArticleList.DataSource = lst;
            gvArticleList.DataBind();


        }






        /// <summary>
        /// 绑定左边项
        /// </summary>
        private void BindDataItem()
        {


            if (TagID > 0)
            {
                cmdDelete.Visible = true;
                cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');");

                DNNGo_PowerForms_Group item = DNNGo_PowerForms_Group.FindByKeyForEdit(TagID);
                txtName.Text = item.Name;



                txtContentText.Text = item.Description;
            }
            else
            {

                //新增界面
                cmdDelete.Visible = false;
                txtName.Text = "";

                txtContentText.Text = "";

            }



        }



        /// <summary>
        /// 绑定选项分组框到页面
        /// </summary>
        private void BindGroupsToPage()
        {
            //获取效果参数
            List<SettingEntity> ItemSettingDB = Setting_CategorySettingDB;

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




        /// <summary>
        /// 绑定选项集合到页面
        /// </summary>
        private void BindOptionsToPage(Repeater RepeaterOptions, String Group, out int OptionCount)
        {
            OptionCount = 0;
            //获取效果参数
            List<SettingEntity> ItemSettingDB = Setting_CategorySettingDB;

            if (ItemSettingDB != null && ItemSettingDB.Count > 0)
            {
                ItemSettingDB = ItemSettingDB.FindAll(r1 => r1.Group == Group);
                OptionCount = ItemSettingDB.Count;
                //绑定参数项
                RepeaterOptions.DataSource = ItemSettingDB;
                RepeaterOptions.DataBind();
            }
        }



        /// <summary>
        /// 拼接数据项的设置参数
        /// </summary>
        /// <returns></returns>
        public String SetItemSettings()
        {
            //获取效果参数
            List<SettingEntity> ItemSettingDB = Setting_CategorySettingDB;
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



        #region "事件"
        protected void Page_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                }
            }
            catch (Exception exc) //Module failed to load
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, exc);
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (!IsPostBack)
                {
                    BindDataItem();
                    BindDataList();
                }
                //绑定设置参数到页面
                BindGroupsToPage();
            }
            catch (Exception exc) //Module failed to load
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, exc);
            }
        }







        /// <summary>
        /// 列表上的项删除事件
        /// </summary>
        protected void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {

                LinkButton btnRemove = (LinkButton)sender;

                DNNGo_PowerForms_Group item = DNNGo_PowerForms_Group.FindByKeyForEdit(btnRemove.CommandArgument);
                mTips.IsPostBack = true;
                if (item.ID > 0 && item.Delete() > 0)
                {
                    mTips.LoadMessage("DeleteGroupSuccess", EnumTips.Success, this, new String[] { item.Name });
                }
                else
                {
                    mTips.LoadMessage("DeleteGroupError", EnumTips.Success, this, new String[] { item.Name });
                }
                BindDataList();

            }
            catch (Exception ex)
            {
                ProcessModuleLoadException(ex);
            }

        }


        protected void cmdDeleteCategory_Click(object sender, EventArgs e)
        {
            try
            {
                DNNGo_PowerForms_Group item = DNNGo_PowerForms_Group.FindByKeyForEdit(TagID);
                if (item.ID > 0 && item.Delete() > 0)
                {
                    mTips.LoadMessage("DeleteGroupSuccess", EnumTips.Success, this, new String[] { item.Name });
                }
                else
                {
                    mTips.LoadMessage("DeleteGroupError", EnumTips.Success, this, new String[] { item.Name });
                }
                Response.Redirect(xUrl("Groups"), false);

            }
            catch (Exception ex)
            {
                ProcessModuleLoadException(ex);
            }

        }


        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {

                DNNGo_PowerForms_Group item = DNNGo_PowerForms_Group.FindByKeyForEdit(TagID);
                item.Description = txtContentText.Text;
                item.Name = txtName.Text;
                if (divOptions.Visible) item.Options = SetItemSettings();

                item.LastUser = UserId;
                item.LastTime = xUserTime.UtcTime();
                item.LastIP = WebHelper.UserHost;

                if (item.ID > 0)
                {

                }
                else
                {

                    item.ModuleId = ModuleId;
                    item.PortalId = PortalId;
                    item.Status = (Int32)EnumStatus.Activation;

                }

                int Resultitem = 0;

                if (item.ID > 0)
                    Resultitem = item.Update();
                else
                    Resultitem = item.Insert();



                if (Resultitem > 0)
                {
                    mTips.LoadMessage("SaveGroupSuccess", EnumTips.Success, this, new String[] { item.Name });
                }
                else
                {
                    //保存失败
                    mTips.LoadMessage("SaveGroupError", EnumTips.Success, this, new String[] { item.Name });
                }
                Response.Redirect(xUrl("Groups"), false);
            }
            catch (Exception ex)
            {
                ProcessModuleLoadException(ex);
            }

        }



        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(xUrl("Groups"), false);

            }
            catch (Exception ex)
            {
                ProcessModuleLoadException(ex);
            }

        }




        /// <summary>
        /// 列表行创建
        /// </summary>
        protected void gvArticleList_RowCreated(object sender, GridViewRowEventArgs e)
        {

            Int32 DataIDX = 0;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //增加check列头全选
                TableCell cell = new TableCell();
                cell.Width = Unit.Pixel(5);
                cell.Text = "<label> <input id='CheckboxAll' value='0' type='checkbox' class='input_text' onclick='SelectAll()'/></label>";
                e.Row.Cells.AddAt(0, cell);


                foreach (TableCell var in e.Row.Cells)
                {
                    if (var.Controls.Count > 0 && var.Controls[0] is LinkButton)
                    {
                        string Colume = ((LinkButton)var.Controls[0]).CommandArgument;
                        if (Colume == Orderfld)
                        {
                            LinkButton l = (LinkButton)var.Controls[0];
                            l.Text += string.Format("<i class=\"fa {0}{1}\"></i>", Orderfld == "Title" ? "fa-sort-alpha-" : "fa-sort-amount-", (OrderType == 0) ? "asc" : "desc");
                        }
                    }
                }

            }
            else
            {
                //增加行选项
                DataIDX = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ID"));
                TableCell cell = new TableCell();
                cell.Width = Unit.Pixel(5);
                cell.Text = string.Format("<label> <input name='Checkbox' id='Checkbox' value='{0}' type='checkbox' type-item=\"true\" class=\"input_text\" /></label>", DataIDX);
                e.Row.Cells.AddAt(0, cell);

            }


        }

        /// <summary>
        /// 列表行绑定
        /// </summary>
        protected void gvArticleList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //还原出数据
                DNNGo_PowerForms_Group Categorie = e.Row.DataItem as DNNGo_PowerForms_Group;

                //移动分类按钮
                LinkButton lbSortUp = e.Row.FindControl("lbSortUp") as LinkButton;
                LinkButton lbSortDown = e.Row.FindControl("lbSortDown") as LinkButton;
                LinkButton lbMobileSortUp = e.Row.FindControl("lbMobileSortUp") as LinkButton;
                LinkButton lbMobileSortDown = e.Row.FindControl("lbMobileSortDown") as LinkButton;
                lbSortUp.CommandArgument =
                     lbSortDown.CommandArgument =
                      lbMobileSortUp.CommandArgument =
                       lbMobileSortDown.CommandArgument = Categorie.ID.ToString();

                //编辑按钮
                HyperLink hlMobileEdit = e.Row.FindControl("hlMobileEdit") as HyperLink;
                HyperLink hlEdit = e.Row.FindControl("hlEdit") as HyperLink;
                hlMobileEdit.NavigateUrl = hlEdit.NavigateUrl = xUrl("ID", Categorie.ID.ToString(), "Groups");

                //删除按钮
                LinkButton btnRemove = e.Row.FindControl("btnRemove") as LinkButton;
                LinkButton btnMobileRemove = e.Row.FindControl("btnMobileRemove") as LinkButton;
                btnRemove.CommandArgument = btnMobileRemove.CommandArgument = Categorie.ID.ToString();
                btnRemove.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');");
                btnMobileRemove.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');");



            }
        }

        /// <summary>
        /// 列表排序
        /// </summary>
        protected void gvArticleList_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Orderfld == e.SortExpression)
            {
                if (OrderType == 0)
                {
                    OrderType = 1;
                }
                else
                {
                    OrderType = 0;
                }
            }
            Orderfld = e.SortExpression;
            //BindDataList();
            Response.Redirect(CurrentUrl, false);
        }


        /// <summary>
        /// 状态应用按钮事件
        /// </summary>
        protected void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 Status = WebHelper.GetIntParam(Request, ddlStatus.UniqueID, -1);

                if (Status >= 0)
                {
                    string Checkbox_Value = WebHelper.GetStringParam(Request, "Checkbox", "");
                    string[] Checkbox_Value_Array = Checkbox_Value.Split(',');
                    Int32 IDX = 0;
                    for (int i = 0; i < Checkbox_Value_Array.Length; i++)
                    {
                        if (Int32.TryParse(Checkbox_Value_Array[i], out IDX))
                        {
                            DNNGo_PowerForms_Group item = DNNGo_PowerForms_Group.FindByKeyForEdit(IDX);
                            if (item != null && item.ID > 0)
                            {
                                mTips.IsPostBack = true;
                                if (item.Delete() > 0)
                                {
                                    //mTips.LoadMessage("DeleteCategorySuccess", EnumTips.Success, this, new String[] { item.ContentText });
                                }
                                else
                                {
                                    //mTips.LoadMessage("DeleteCategoryError", EnumTips.Success, this, new String[] { item.ContentText });
                                }


                            }
                        }
                    }
                    BindDataList();
                }
            }
            catch (Exception ex)
            {
                ProcessModuleLoadException(ex);
            }
        }


        /// <summary>
        /// 搜索按钮事件
        /// </summary>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Search_Title = HttpUtility.UrlEncode(txtSearch.Text.Trim());
                Response.Redirect(CurrentUrl, false);
            }
            catch (Exception ex)
            {
                ProcessModuleLoadException(ex);
            }
        }


        protected void lbSort_Click(object sender, EventArgs e)
        {
            LinkButton ImgbutSort = (LinkButton)sender;
            if (ImgbutSort != null)
            {
                //查出当前要排序的字段
                DNNGo_PowerForms_Group objC = DNNGo_PowerForms_Group.FindByKeyForEdit(ImgbutSort.CommandArgument);

                mTips.IsPostBack = true;//回发时就要触发
                if (ImgbutSort.ToolTip == "up")
                {
                    DNNGo_PowerForms_Group.MoveField(objC, EnumMoveType.Up, ModuleId);
                    //字段上移成功
                    mTips.LoadMessage("UpMoveGroupSuccess", EnumTips.Success, this, new String[] { "" });

                }
                else
                {
                    DNNGo_PowerForms_Group.MoveField(objC, EnumMoveType.Down, ModuleId);
                    //字段下移成功

                    mTips.LoadMessage("DownMoveGroupSuccess", EnumTips.Success, this, new String[] { "" });
                }
                //绑定一下
                BindDataList();
            }


        }






        protected void RepeaterOptions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SettingEntity ThemeSetting = e.Item.DataItem as SettingEntity;

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













    }
}