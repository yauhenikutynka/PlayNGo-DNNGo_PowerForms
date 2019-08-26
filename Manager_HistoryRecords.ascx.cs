using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DotNetNuke.Services.Localization;
using System.Globalization;


namespace DNNGo.Modules.PowerForms
{
    public partial class Manager_HistoryRecords : basePortalModule
    {


        #region "==属性=="


        /// <summary>
        /// 提示操作类
        /// </summary>
        MessageTips mTips = new MessageTips();


        /// <summary>
        /// 文章搜索_标题
        /// </summary>
        public String Search_Title = WebHelper.GetStringParam(HttpContext.Current.Request, "SearchText", "");


        /// <summary>
        /// [开始时间]搜索
        /// </summary>
        public DateTime Search_BeginDate =WebHelper.GetDateTimeParam(HttpContext.Current.Request, "BeginDate", DateTime.MinValue);



        /// <summary>
        /// [结束时间]搜索
        /// </summary>
        public DateTime Search_EndDate=WebHelper.GetDateTimeParam(HttpContext.Current.Request, "EndDate", DateTime.MinValue);


        /// <summary>
        /// 当前页码
        /// </summary>
        public Int32 PageIndex = WebHelper.GetIntParam(HttpContext.Current.Request, "PageIndex", 1);

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


                if (Search_BeginDate != DateTime.MinValue)
                {
                    urls.Add(String.Format("BeginDate={0}", HttpUtility.UrlEncode(Search_BeginDate.ToShortDateString())));
                }

                if (Search_EndDate != DateTime.MinValue)
                {
                    urls.Add(String.Format("EndDate={0}", HttpUtility.UrlEncode(Search_EndDate.ToShortDateString())));
                }


                return xUrl("", "", "History", urls.ToArray());
            }
        }


        /// <summary>
        /// 排序字段
        /// </summary>
        public string Orderfld = WebHelper.GetStringParam(HttpContext.Current.Request, "sort_f", "");


        /// <summary>
        /// 排序类型 1:降序 0:升序
        /// </summary>
        public int OrderType = WebHelper.GetIntParam(HttpContext.Current.Request, "sort_t", 1);

        private List<DNNGo_PowerForms_Field> _FieldList = new List<DNNGo_PowerForms_Field>();
        /// <summary>
        /// 字段列表
        /// </summary>
        public List<DNNGo_PowerForms_Field> FieldList
        {
            get
            {
                if (!(_FieldList != null && _FieldList.Count > 0))
                {
                    _FieldList = DNNGo_PowerForms_Field.FindAllByModuleId(ModuleId);
                }
                return _FieldList;
            }
        }

        #endregion


        #region "==方法=="

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindDataToPage()
        {

            //tdControl.Visible = IsAdministrator;
            cmdDelete.Visible = IsAdministrator;
            cmdCleanup.Attributes.Add("onClick", "javascript:return confirm('" + ViewResourceText("CleanupConfirm", "") + "');");

            QueryParam qp = new QueryParam();
            qp.OrderType = OrderType;
            if (!String.IsNullOrEmpty(Orderfld))
            {
                qp.Orderfld = Orderfld;
            }
            else
            {
                qp.Orderfld = DNNGo_PowerForms_Content._.ID;
            }

            #region "分页的一系列代码"


            int RecordCount = 0;
            int pagesize = qp.PageSize = Settings_PageSize;
            qp.PageIndex = PageIndex;

    

            #endregion

            //查询的方法
            qp.Where = BindSearch();

            List<DNNGo_PowerForms_Content> Articles = DNNGo_PowerForms_Content.FindAll(qp, out RecordCount);
            qp.RecordCount = RecordCount;
            RecordPages = qp.Pages;
 
            repeaterContent.DataSource = Articles;
            repeaterContent.DataBind();
             
             

        }

        /// <summary>
        /// 绑定查询的方法
        /// </summary>
        private List<SearchParam> BindSearch()
        {
            Boolean IsViewCancel = false;

            List<SearchParam> Where = new List<SearchParam>();
            Where.Add(new SearchParam("ModuleId", ModuleId, SearchType.Equal));


            if (Search_BeginDate != DateTime.MinValue)
            {
                txtBeginDate.Text = Search_BeginDate.ToString("MM/dd/yyyy");

                Where.Add(new SearchParam(DNNGo_PowerForms_Content._.LastTime, Search_BeginDate, SearchType.GtEqual));
             
            }

            if (Search_EndDate != DateTime.MinValue)
            {
                txtEndDate.Text = Search_EndDate.ToString("MM/dd/yyyy");

                Where.Add(new SearchParam(DNNGo_PowerForms_Content._.LastTime, Search_EndDate, SearchType.LtEqual));
            
            }
 


            return Where;
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
        /// 搜索按钮
        /// </summary>
        protected void lbSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime BeginDate = DateTime.Now;
                if (!String.IsNullOrEmpty(txtBeginDate.Text) && DateTime.TryParseExact(txtBeginDate.Text, "MM/dd/yyyy", new CultureInfo("en-US", false), DateTimeStyles.AllowWhiteSpaces, out BeginDate))
                {
                    Search_BeginDate = BeginDate;
                }

                DateTime EndDate = DateTime.Now;
                if (!String.IsNullOrEmpty(txtEndDate.Text) && DateTime.TryParseExact(txtEndDate.Text, "MM/dd/yyyy", new CultureInfo("en-US", false), DateTimeStyles.AllowWhiteSpaces, out EndDate))
                {
                    Search_EndDate = EndDate;
                }


                Response.Redirect(CurrentUrl, false);
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        /// <summary>
        /// 取消搜索按钮
        /// </summary>
        protected void lbCancel_Click(object sender, EventArgs e)
        {
            try
            {


                Search_BeginDate = DateTime.MinValue;
                Search_EndDate = DateTime.MinValue;
                txtBeginDate.Text = String.Empty;
                txtEndDate.Text = String.Empty;

                BindDataToPage();
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
        }




        protected void repeaterContent_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                DNNGo_PowerForms_Content ContentItem = e.Item.DataItem as DNNGo_PowerForms_Content;


                Label lblUserName = e.Item.FindControl("lblUserName") as Label;
                Label lblEmail = e.Item.FindControl("lblEmail") as Label;
                Label lblFormVersion = e.Item.FindControl("lblFormVersion") as Label;
                

                lblUserName.Text = String.IsNullOrEmpty(ContentItem.UserName) ? Localization.GetString("Anonymous", this.LocalResourceFile) : ContentItem.UserName;
                lblEmail.Text = String.IsNullOrEmpty(ContentItem.Email) ? Localization.GetString("Anonymous", this.LocalResourceFile) : ContentItem.Email;
                lblFormVersion.Text =  ContentItem.FormVersion;


                GridView gvItemList = e.Item.FindControl("gvItemList") as GridView;
                //读取存储的表单内容
                gvItemList.DataSource = Common.Deserialize<List<DNNGo_PowerForms_ContentItem>>(ContentItem.ContentValue);
                gvItemList.DataBind();
                gvItemList.Columns[3].Visible = Setting_EffectDB.Group;
                BindGridViewEmpty<DNNGo_PowerForms_ContentItem>(gvItemList);

            }
        }



        protected void gvItemList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DNNGo_PowerForms_ContentItem ContentItem = e.Row.DataItem as DNNGo_PowerForms_ContentItem;

                if (ContentItem.Extra && !Settings_ExtraTracking)
                {
                    e.Row.Visible = false; //如果是关闭了追踪,就停止显示
                }
                else
                {
                    Literal LiContentValue = e.Row.FindControl("LiContentValue") as Literal;

                    TemplateFormat xf = new TemplateFormat(this);
                    xf.FieldList = FieldList;
                    LiContentValue.Text =  xf.ViewContentValue(ContentItem);
                }

            }
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {

                string Checkbox_Value = WebHelper.GetStringParam(Request, "Checkbox", "");
                string[] Checkbox_Value_Array = Checkbox_Value.Split(',');
                Int32 IDX = 0;
                for (int i = 0; i < Checkbox_Value_Array.Length; i++)
                {
                    if (Int32.TryParse(Checkbox_Value_Array[i], out IDX))
                    {
                        DNNGo_PowerForms_Content ContentItem = DNNGo_PowerForms_Content.FindByKeyForEdit(IDX);

                        if (ContentItem != null && ContentItem.ID > 0 && !String.IsNullOrEmpty(ContentItem.ContentValue))
                        {

                            foreach (DNNGo_PowerForms_ContentItem item in Common.Deserialize<List<DNNGo_PowerForms_ContentItem>>(ContentItem.ContentValue))
                            {

                                if (!String.IsNullOrEmpty(item.ContentValue))
                                {
                                    List<String> files = WebHelper.GetList(item.ContentValue, "<|>");
                                    if (files != null && files.Count > 0)
                                    {
                                        foreach (var filename in files)
                                        {
                                            if (!String.IsNullOrEmpty(filename) && filename.IndexOf("Url://") >= 0)
                                            {

                                                try
                                                {
                                                    FileInfo formFile = new FileInfo(MapPath(String.Format("~/Portals/{0}/PowerForms/{1}/{2}", PortalId, ModuleId, filename.Replace("Url://", ""))));
                                                    if (formFile.Exists) formFile.Delete();

                                                }
                                                catch
                                                { }

                                            }

                                        }


                                    }




                                }


                            }

                        }


                        if (ContentItem.Delete() > 0)
                        {
                            //删除历史记录成功
                            mTips.IsPostBack = true;
                            mTips.LoadMessage("DeleteHistoryRecordsSuccess", EnumTips.Success, this, new String[] { "" });
                        }
                    }
                }

                BindDataToPage();


            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        protected void cmdExport_Click(object sender, EventArgs e)
        {
            try
            {

                string Checkbox_Value = WebHelper.GetStringParam(Request, "Checkbox", "");
                string[] Checkbox_Value_Array = Checkbox_Value.Split(',');
                Int32 IDX = 0;
                
                List<DNNGo_PowerForms_Content> DataList = new List<DNNGo_PowerForms_Content>();
                for (int i = 0; i < Checkbox_Value_Array.Length; i++)
                {
                    if (Int32.TryParse(Checkbox_Value_Array[i], out IDX))
                    {
                        DNNGo_PowerForms_Content ContentItem = DNNGo_PowerForms_Content.FindByKeyForEdit(IDX);

                        if (ContentItem != null && ContentItem.ID > 0 && !String.IsNullOrEmpty(ContentItem.ContentValue))
                        {
                            //DataList.Add(  Common.Deserialize<List<DNNGo_PowerForms_ContentItem>>(ContentItem.ContentValue));
                            DataList.Add(ContentItem);
                        }

                    }
                }
                if (DataList.Count > 0)
                {
 
                    try
                    {
                        String FullName = String.Empty;
                        if (CsvHelper.SaveAsToFile(FieldList, DataList, out FullName, this))
                        {

                
                            FileSystemUtils.DownloadFile(FullName, Path.GetFileName(FullName));
                            //Response.Write(FullName);
                        }
                        else
                        {
                            mTips.LoadMessage("SaveExportRecordsError", EnumTips.Error, this, new String[] { FullName });
                        }



                    }
                    catch (Exception ex)
                    {
                        DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
                    }


                }

            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        protected void cmdExportAll_Click(object sender, EventArgs e)
        {
            try
            {
                QueryParam qp = new QueryParam();
                qp.OrderType = 1;
                qp.Orderfld = DNNGo_PowerForms_Content._.ID;
                qp.PageSize = int.MaxValue;
                qp.Where = BindSearch();
                Int32 RecordCount = 0;
                List<DNNGo_PowerForms_Content> DataList = DNNGo_PowerForms_Content.FindAll(qp, out RecordCount);
           

                if (DataList != null && DataList.Count > 0)
                {
                    try
                    {

                        String FullName = String.Empty;
                        if (CsvHelper.SaveAsToFile(FieldList, DataList, out FullName, this))
                        {

                            FileSystemUtils.DownloadFile(FullName, Path.GetFileName(FullName));
                        
                            //Response.Write(FullName);
                        }
                        else
                        {
                            //导出记录失败
                            mTips.IsPostBack = true;
                            mTips.LoadMessage("SaveExportRecordsError", EnumTips.Error, this, new String[] { FullName });

                        }

                    }
                    catch (Exception ex)
                    {
                        DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
                    }


                }

            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        /// <summary>
        /// 清除所有历史记录
        /// </summary>
        protected void cmdCleanup_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 RecordCount = 0;
                 QueryParam qp = new QueryParam();
                 qp.Where.Add(new SearchParam("ModuleId", ModuleId, SearchType.Equal));

                 List<DNNGo_PowerForms_Content> CleanupList = DNNGo_PowerForms_Content.FindAll(qp, out RecordCount);
                 Int32 CleanupCount = 0;
                 foreach (var CleanupItem in CleanupList)
                 {
                     if (CleanupItem.Delete() > 0)
                     {
                         CleanupCount++;
                     }
                 }

                 if (CleanupCount > 0)
                 {
                     BindDataToPage();
                 }

                    

                 mTips.IsPostBack = true;
                 mTips.LoadMessage("CleanupHistoryRecordsSuccess", EnumTips.Success, this, new String[] { CleanupCount.ToString() });

            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        #endregion








    }
}