using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using System.Web.Script.Serialization;

namespace DNNGo.Modules.PowerForms
{
    public partial class Manager_FieldSort : basePortalModule
    {


        #region "==属性=="

        /// <summary>
        /// 模块操作类
        /// </summary>
        private static ModuleController controller = new ModuleController();

        private ModuleInfo _module = new ModuleInfo();
        /// <summary>
        /// 模块信息
        /// </summary>
        public ModuleInfo Module
        {
            get
            {
                if (!(_module != null && _module.ModuleID > 0))
                {
                    _module = controller.GetModule(ModuleId, TabId, false);
                }

                return _module;
            }

        }
        /// <summary>
        /// 得到当前模块的字段列表
        /// </summary>
        public List<DNNGo_PowerForms_Field> FieldList
        {
            get {
                QueryParam qp = new QueryParam();
                qp.OrderType = 0;
                qp.Orderfld = DNNGo_PowerForms_Field._.Sort;
                qp.Where.Add(new SearchParam("ModuleId", ModuleId, SearchType.Equal));
                qp.Where.Add(new SearchParam(DNNGo_PowerForms_Field._.Status, (Int32)EnumStatus.Activation, SearchType.Equal));
                Int32 RecordCount = 0;
               return DNNGo_PowerForms_Field.FindAll(qp, out RecordCount);
            
            }
        }

        /// <summary>提示操作类</summary>
        MessageTips mTips = new MessageTips();

        #endregion


        #region "==方法=="

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindDataToPage()
        {

            RepeaterFields.DataSource = FieldList;
            RepeaterFields.DataBind();
        }
 

        /// <summary>
        /// 设置数据项
        /// </summary>
        private void SetDataItem()
        {
          
            //查询出当前字段的列表
            String jsonFields = WebHelper.GetStringParam(Request, nestable_output.UniqueID, "");
            if (!String.IsNullOrEmpty(jsonFields))
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
               List<DNNGo_PowerForms_Field> json_fields =  json.Deserialize<List<DNNGo_PowerForms_Field>>(jsonFields);
               if (json_fields != null && json_fields.Count > 0)
               {

                   List<DNNGo_PowerForms_Field> FieldLists = FieldList;

                   for (int i = 0; i < json_fields.Count; i++)
                   {
                       DNNGo_PowerForms_Field DBField = FieldLists.Find(r => r.ID == json_fields[i].ID);
                       if (DBField != null && DBField.ID > 0 && DBField.Sort != i)
                       {
                           DBField.Sort = i;
                           DBField.Update();

                       }


                   }
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
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
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

                Response.Redirect(xUrl("FieldSort"), true);
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
        }
        /// <summary>
        /// 返回
        /// </summary>
        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            try
            {

                Response.Redirect(xUrl(), true);
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        /// <summary>
        /// 字段绑定事件
        /// </summary>
        protected void RepeaterFields_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DNNGo_PowerForms_Field FieldItem = e.Item.DataItem as DNNGo_PowerForms_Field;

                if (FieldItem != null && FieldItem.ID > 0)
                {

                    Literal liFieldType = e.Item.FindControl("liFieldType") as Literal;
                    liFieldType.Text = EnumHelper.GetEnumTextVal(FieldItem.FieldType, typeof(EnumViewControlType));

                    Literal liName = e.Item.FindControl("liName") as Literal;
                    liName.Text = WebHelper.leftx(FieldItem.Name, 30, "...");

                    if (FieldItem.GroupID > 0)
                    {
                        DNNGo_PowerForms_Group Group = DNNGo_PowerForms_Group.FindByKeyForEdit(FieldItem.GroupID);
                        if (Group != null && Group.ID > 0)
                        {
                            Literal liGroup = e.Item.FindControl("liGroup") as Literal;
                            liGroup.Text = WebHelper.leftx(Group.Name, 20, "...");
                        }
                    }
                }

             
            }
        }


        #endregion









    }
}