using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

namespace DNNGo.Modules.PowerForms
{
    public partial class View_Result : basePortalModule
    {


        #region "扩展属性"
        /// <summary>
        /// 表单编号
        /// </summary>
        private Int32 FormID = WebHelper.GetIntParam(HttpContext.Current.Request, "FormID", 0);

        /// <summary>
        /// 页码
        /// </summary>
        public Int32 PageIndex
        {
            get { return WebHelper.GetIntParam(HttpContext.Current.Request, String.Format("PageIndex{0}", ModuleId), 1); }
        }
        #endregion


        #region "方法"

        /// <summary>
        /// 绑定数据项到前台
        /// </summary>
        public void BindDataItem(EffectDB EffectDB)
        {

            Hashtable Puts = new Hashtable();
            TemplateFormat xf = new TemplateFormat(this);
        

            //读取需要载入的参数
            Puts.Add("EffectName", Settings_ResultName);
            Puts.Add("ThemeName", Settings_ResultThemeName);
 
            //读取当前Form的结果
            DNNGo_PowerForms_Content DataItem = DNNGo_PowerForms_Content.FindByKeyForEdit(FormID);
            Puts.Add("DataItem", DataItem);

             
            if (DataItem != null && DataItem.ID > 0 && !String.IsNullOrEmpty(DataItem.ContentValue))
            {
                List<DNNGo_PowerForms_ContentItem> ContentList = Common.Deserialize<List<DNNGo_PowerForms_ContentItem>>(DataItem.ContentValue);
                Puts.Add("ContentList", ContentList);
            }

            //查看是否是结果列表页
            if (!EffectDB.IsDetail)
            {
                Int32 RecordCount = 0;
                QueryParam qp = new QueryParam();
                qp.Orderfld = DNNGo_PowerForms_Content._.ID;
                qp.OrderType = 1;
                qp.PageSize = 10;
                qp.PageIndex = PageIndex;
                qp.Where.Add(new SearchParam("ModuleId", ModuleId, SearchType.Equal));
                List<DNNGo_PowerForms_Content> Results = DNNGo_PowerForms_Content.FindAll(qp, out RecordCount);
                Puts.Add("ResultList", Results);

                if (RecordCount > qp.PageSize)
                {
                    Puts.Add("Pager", new Pager(qp.PageIndex, qp.PageSize, ModuleId, RecordCount, EnumPageType.DnnURL,false).CreateHtml());//分页
                }
                else
                {
                    Puts.Add("Pager", "");
                }
            }



            liContent.Text = HttpUtility.HtmlDecode(ViewTemplate(EffectDB, "Effect.html", Puts, xf, "Result"));


        }





        #endregion



        #region "事件"

        /// <summary>
        /// 页面加载
        /// </summary>
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(Settings_ResultName))
                {

                    if (!String.IsNullOrEmpty(Settings_ResultThemeName))
                    {
                        EffectDB EffectDB = Setting_ResultDB;
                        if (!IsPostBack)
                        {
                            //绑定数据项到前台
                            BindDataItem(EffectDB);

                        }

                        //需要载入当前设置效果的主题CSS文件
                        String ThemeName = String.Format("{0}_{1}", Settings_ResultName, Settings_ResultThemeName);
                        String ThemePath = String.Format("{0}Results/{1}/Themes/{2}/Style.css", ModulePath, Settings_ResultName, Settings_ResultThemeName);
                        BindStyleFile(ThemeName, ThemePath);

                        BindXmlDBToPage(EffectDB, "Result");

                         




                    }
                    else
                    {
                        //未定义效果对应的主题
                        liContent.Text = "";
                    }
                }
                else
                {
                    //未绑定效果
                    liContent.Text = "";
                }


            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

 


 
 
        #endregion




    }
}