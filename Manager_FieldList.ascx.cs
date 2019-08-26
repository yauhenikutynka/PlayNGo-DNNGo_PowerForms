using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Localization;

namespace DNNGo.Modules.PowerForms
{
    public partial class Manager_FieldList : basePortalModule
    {


        #region "属性"

        /// <summary>
        /// 提示操作类
        /// </summary>
        MessageTips mTips = new MessageTips();

        /// <summary>
        /// 当前页码
        /// </summary>
        public Int32 PageIndex = WebHelper.GetIntParam(HttpContext.Current.Request, "PageIndex", 1);

        /// <summary>
        /// 文章状态
        /// </summary>
        public Int32 FieldStatus = WebHelper.GetIntParam(HttpContext.Current.Request, "Status", (Int32)EnumStatus.Activation);



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

                //if (FieldStatus >= 0)
                //{
                    urls.Add(String.Format("Status={0}", FieldStatus));
                //}

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

                return xUrl("", "", "FieldList", urls.ToArray());
            }
        }


        /// <summary>
        /// 排序字段
        /// </summary>
        public string Orderfld = WebHelper.GetStringParam(HttpContext.Current.Request, "sort_f", "");


        /// <summary>
        /// 排序类型 1:降序 0:升序
        /// </summary>
        public int OrderType = WebHelper.GetIntParam(HttpContext.Current.Request, "sort_t", 0);



        #endregion



        #region "方法"

        /// <summary>
        /// 绑定列表
        /// </summary>
        private void BindDataList()
        {
            QueryParam qp = new QueryParam();
            qp.OrderType = OrderType;
            if (!String.IsNullOrEmpty(Orderfld))
            {
                qp.Orderfld = Orderfld;
            }
            else
            {
                qp.Orderfld = DNNGo_PowerForms_Field._.Sort;
            }

            #region "分页的一系列代码"


            int RecordCount = 0;
            int pagesize = qp.PageSize = 10;
            qp.PageIndex = PageIndex;


            #endregion

            //查询的方法
            qp.Where = BindSearch();

            List<DNNGo_PowerForms_Field> Fields = DNNGo_PowerForms_Field.FindAll(qp, out RecordCount);
            qp.RecordCount = RecordCount;
            RecordPages = qp.Pages;
            lblRecordCount.Text = String.Format("{0} {2} / {1} {3}", RecordCount, RecordPages, ViewResourceText("Title_Items", "Items"), ViewResourceText("Title_Pages", "Pages"));

            hlAllFields.Text = String.Format("{1} ({0})", DNNGo_PowerForms_Field.FindCountByStatus(ModuleId, -1), ViewResourceText("hlAllFields", "All"));
            hlActivationField.Text = String.Format("{1} ({0})", DNNGo_PowerForms_Field.FindCountByStatus(ModuleId, (Int32)EnumStatus.Activation), ViewResourceText("hlActivationField", "Activation"));
            hlHideField.Text = String.Format("{1} ({0})", DNNGo_PowerForms_Field.FindCountByStatus(ModuleId, (Int32)EnumStatus.Hide), ViewResourceText("hlHideField", "Hide"));
          


            //ctlPagingControl.TotalRecords = RecordCount;

            //if (RecordCount <= pagesize)
            //{
            //    ctlPagingControl.Visible = false;

            //}

            gvFieldList.DataSource = Fields;
            gvFieldList.DataBind();
            BindGridViewEmpty<DNNGo_PowerForms_Field>(gvFieldList, new DNNGo_PowerForms_Field());
            gvFieldList.Columns[7].Visible = false;//屏蔽排序的列
            gvFieldList.Columns[2].Visible = Setting_EffectDB.Group;//分组展现

        }



        /// <summary>
        /// 绑定页面项
        /// </summary>
        private void BindPageItem()
        {

            hlLinkAddField.NavigateUrl = xUrl("AddNewField");
            hlLinkSortFields.NavigateUrl = xUrl("FieldSort");


            hlAllFields.NavigateUrl = xUrl("Status","-1", "FieldList");
            hlActivationField.NavigateUrl = xUrl("Status", ((Int32)EnumStatus.Activation).ToString(), "FieldList");
            hlHideField.NavigateUrl = xUrl("Status", ((Int32)EnumStatus.Hide).ToString(), "FieldList");
            

            switch (FieldStatus)
            {
                case -1: hlAllFields.CssClass = "btn btn-default active"; break;
                case (Int32)EnumStatus.Activation: hlActivationField.CssClass = "btn btn-default active"; break;
                case (Int32)EnumStatus.Hide: hlHideField.CssClass = "btn btn-default active"; break;
                default: hlAllFields.CssClass = "btn btn-default active"; break;
            }


        }


        /// <summary>
        /// 绑定查询的方法
        /// </summary>
        private List<SearchParam> BindSearch()
        {
            List<SearchParam> Where = new List<SearchParam>();
            Where.Add(new SearchParam("ModuleId", ModuleId, SearchType.Equal));


            //筛选文章的状态
            if (FieldStatus >= 0)
            {
                Where.Add(new SearchParam(DNNGo_PowerForms_Field._.Status, FieldStatus, SearchType.Equal));
            }


            if (!String.IsNullOrEmpty(Search_Title))
            {
                txtSearch.Text = HttpUtility.UrlDecode(Search_Title);
                Where.Add(new SearchParam(DNNGo_PowerForms_Field._.Alias, HttpUtility.UrlDecode(Search_Title), SearchType.Like));
            }






            return Where;
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
                    BindDataList();
                    BindPageItem();
                }
            }
            catch (Exception ex)
            {
                ProcessModuleLoadException( ex);
            }

        }

        /// <summary>
        /// 列表行创建
        /// </summary>
        protected void gvFieldList_RowCreated(object sender, GridViewRowEventArgs e)
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
        protected void gvFieldList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //还原出数据
                DNNGo_PowerForms_Field Fielditem = e.Row.DataItem as DNNGo_PowerForms_Field;

                if (Fielditem != null && Fielditem.ID > 0)
                {
                    #region "编辑&删除按钮"
                    HyperLink hlEdit = e.Row.FindControl("hlEdit") as HyperLink;
                    HyperLink hlMobileEdit = e.Row.FindControl("hlMobileEdit") as HyperLink;
                    LinkButton btnRemove = e.Row.FindControl("btnRemove") as LinkButton;
                    LinkButton btnMobileRemove = e.Row.FindControl("btnMobileRemove") as LinkButton;
                    //设置按钮的CommandArgument
                    btnRemove.CommandArgument = btnMobileRemove.CommandArgument = Fielditem.ID.ToString();
                    //设置删除按钮的提示
                    //if (Field.Status == (Int32)EnumFieldStatus.Recycle)
                    //{
                    btnRemove.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');");
                    btnMobileRemove.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');");
                    //}
                    //else
                    //{
                    //    btnRemove.Attributes.Add("onClick", "javascript:return confirm('" + ViewResourceText("DeleteRecycleItem", "Are you sure to move it to recycle bin?") + "');");
                    //    btnMobileRemove.Attributes.Add("onClick", "javascript:return confirm('" + ViewResourceText("DeleteRecycleItem", "Are you sure to move it to recycle bin?") + "');");
                    //}

                    hlEdit.NavigateUrl = hlMobileEdit.NavigateUrl = xUrl("FieldID", Fielditem.ID.ToString(), "AddNewField");


                    #endregion


                    #region "移动分类按钮"
                    LinkButton lbSortUp = e.Row.FindControl("lbSortUp") as LinkButton;
                    LinkButton lbSortDown = e.Row.FindControl("lbSortDown") as LinkButton;
                    LinkButton lbMobileSortUp = e.Row.FindControl("lbMobileSortUp") as LinkButton;
                    LinkButton lbMobileSortDown = e.Row.FindControl("lbMobileSortDown") as LinkButton;
                    lbSortUp.CommandArgument =
                         lbSortDown.CommandArgument =
                          lbMobileSortUp.CommandArgument =
                           lbMobileSortDown.CommandArgument = Fielditem.ID.ToString();
                    #endregion


                    e.Row.Cells[4].Text = EnumHelper.GetEnumTextVal(Fielditem.FieldType, typeof(EnumViewControlType));

                    //获取用户名称
                    DotNetNuke.Entities.Users.UserInfo uInfo = new DotNetNuke.Entities.Users.UserController().GetUser(PortalId, Fielditem.LastUser);
                    e.Row.Cells[7].Text = uInfo != null && uInfo.UserID> 0 ?  uInfo.Username:"--";

                    e.Row.Cells[8].Text = EnumHelper.GetEnumTextVal(Fielditem.Status, typeof(EnumStatus));

                    if (Fielditem.GroupID > 0)
                    {
                        DNNGo_PowerForms_Group Group = DNNGo_PowerForms_Group.FindByKeyForEdit(Fielditem.GroupID);
                        if (Group != null && Group.ID > 0)
                            e.Row.Cells[3].Text = Group.Name;
                        else
                            e.Row.Cells[3].Text = String.Empty;
                    }
                    else
                    {
                        e.Row.Cells[3].Text = String.Empty;
                    }

                }
            }
        }

        /// <summary>
        /// 列表排序
        /// </summary>
        protected void gvFieldList_Sorting(object sender, GridViewSortEventArgs e)
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
            Response.Redirect(CurrentUrl);
        }


        /// <summary>
        /// 列表上的项删除事件
        /// </summary>
        protected void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {

                LinkButton btnRemove = (LinkButton)sender;

                if (btnRemove != null && !String.IsNullOrEmpty(btnRemove.CommandArgument))
                {

                 

                    DNNGo_PowerForms_Field Field = DNNGo_PowerForms_Field.FindByKeyForEdit(btnRemove.CommandArgument);

                    if (Field != null && Field.ID > 0)
                    {
                        mTips.IsPostBack = true;
                        if (Field.Delete() > 0)
                        {
                            //操作成功
                            mTips.LoadMessage("DeleteFieldSuccess", EnumTips.Success, this, new String[] { Field.Alias });
                        }
                        else
                        {
                            //操作失败
                            mTips.LoadMessage("DeleteFieldError", EnumTips.Success, this, new String[] { Field.Alias });
                        }

                        BindDataList();
                    }
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
                ProcessModuleLoadException( ex);
            }
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
                    mTips.IsPostBack = true;
                    for (int i = 0; i < Checkbox_Value_Array.Length; i++)
                    {
                        if (Int32.TryParse(Checkbox_Value_Array[i], out IDX))
                        {
                            DNNGo_PowerForms_Field Field = DNNGo_PowerForms_Field.FindByKeyForEdit(IDX);
                            if (Field != null && Field.ID > 0)
                            {
                                Field.Status = Status;
                                    if (Field.Update() > 0)
                                    {
                                        //操作成功
                                        mTips.LoadMessage("UpdateFieldSuccess", EnumTips.Success, this, new String[] { Field.Alias });
                                    }
                                    else
                                    {
                                        //操作失败
                                        mTips.LoadMessage("UpdateFieldError", EnumTips.Success, this, new String[] { Field.Alias });
                                    }
                                
                            }
                        }
                    }

                    BindDataList();

                   
                    
                }
            }
            catch (Exception ex)
            {
                ProcessModuleLoadException( ex);
            }
        }


        protected void lbSort_Click(object sender, EventArgs e)
        {
            LinkButton ImgbutSort = (LinkButton)sender;
            if (ImgbutSort != null)
            {
                //查出当前要排序的字段
                DNNGo_PowerForms_Field objC = DNNGo_PowerForms_Field.FindByKeyForEdit(ImgbutSort.CommandArgument);

                mTips.IsPostBack = true;//回发时就要触发
                if (ImgbutSort.ToolTip == "up")
                {
                    DNNGo_PowerForms_Field.MoveField(objC, EnumMoveType.Up, ModuleId);
                    //字段上移成功
                    mTips.LoadMessage("UpMoveFieldSuccess", EnumTips.Success, this, new String[] { "" });

                }
                else
                {
                    DNNGo_PowerForms_Field.MoveField(objC, EnumMoveType.Down, ModuleId);
                    //字段下移成功

                    mTips.LoadMessage("DownMoveFieldSuccess", EnumTips.Success, this, new String[] { "" });
                }
                //绑定一下
                BindDataList();
            }


        }



        #endregion



















    }
}