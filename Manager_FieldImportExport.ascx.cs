using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DotNetNuke.Services.Localization;

namespace DNNGo.Modules.PowerForms
{
    public partial class Setting_ManagerField_ImportExport : basePortalModule
    {


        #region "==属性=="

        /// <summary>
        /// 提示操作类
        /// </summary>
        MessageTips mTips = new MessageTips();

        #endregion




        #region "==方法=="







        #endregion





        #region "==事件=="

        /// <summary>
        /// 页面加载
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //如果查询到当前有字段时，需要给用户提示会清除掉原有的字段
                cmdImportFormXml.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("ImportField",this.LocalResourceFile) + "');");

            }
        }

        /// <summary>
        /// 导出数据到XML
        /// </summary>
        protected void cmdExportToXml_Click(object sender, EventArgs e)
        {

            ImportExportHelper ieHelper = new ImportExportHelper();
            ieHelper.ModuleID = ModuleId;

            //查询字段的数据,填充待导出的XML实体
            QueryParam qp = new QueryParam();
            Int32 RecordCount = 0;
            qp.Orderfld = DNNGo_PowerForms_Field._.Sort;
            qp.OrderType = 0;
            qp.Where.Add(new SearchParam("ModuleId", ModuleId, SearchType.Equal));
            List<DNNGo_PowerForms_Field> fieldList = DNNGo_PowerForms_Field.FindAll(qp, out RecordCount);
            List<FieldEntity> xmlFieldList = new List<FieldEntity>();
            List<GallerySettingsEntity> xmlSettingList = new List<GallerySettingsEntity>();
            foreach (DNNGo_PowerForms_Field fieldItem in fieldList)
            {
                xmlFieldList.Add(ieHelper.EntityToXml(fieldItem));
            }

            //查询出所有的配置项
            List<SettingEntity> EffectSettingDB = ieHelper.Setting_EffectSettingDB;
            if (EffectSettingDB != null && EffectSettingDB.Count > 0)
            {
                foreach (SettingEntity SettingItem in EffectSettingDB)
                {
                    String SettingValue = ieHelper.ViewXmlSetting(SettingItem.Name, SettingItem.DefaultValue).ToString();
                    xmlSettingList.Add(new GallerySettingsEntity(ieHelper.EffectSettingsFormat(ieHelper.Settings_EffectName, SettingItem.Name), SettingValue));
                }

                foreach (String key in ieHelper.PowerForms_Settings.Keys)
                {
                    if (!xmlSettingList.Exists(r1 => r1.SettingName == key) && key.IndexOf("Gallery") != 0)
                    {
                        xmlSettingList.Add(new GallerySettingsEntity(key, Convert.ToString(ieHelper.PowerForms_Settings[key])));
                    }
                }
            }

            XmlFormat xf = new XmlFormat(MapPath(String.Format("{0}Resource/xml/FieldEntity.xml", ModulePath)));
            //将字段列表转换成XML的实体
            String XmlContent = xf.ToXml<FieldEntity>(xmlFieldList, xmlSettingList);
            String XmlFilePath = FileSystemUtils.SaveXmlToFile(String.Format("FieldListEntity_{0}_{1}.xml", ModuleId, xUserTime.UtcTime().ToString("yyyyMMddHHmmssffff")), XmlContent, this);
            FileSystemUtils.DownloadFile(XmlFilePath, "FieldListEntity.xml");
            
        }

        /// <summary>
        /// 从XML导入数据
        /// </summary>
        protected void cmdImportFormXml_Click(object sender, EventArgs e)
        {

            try
            {
                HttpPostedFile hpfile = fuImportFormXml.PostedFile;

                if (hpfile.ContentLength > 0)
                {

                    if (Path.GetExtension(hpfile.FileName).IndexOf(".xml", StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {

                        ImportExportHelper ieHelper = new ImportExportHelper();
                        ieHelper.ModuleID = ModuleId;
                        ieHelper.UserId = UserId;

                        //先清除原有的字段列表
                        QueryParam qp = new QueryParam();
                        Int32 RecordCount = 0;
                        qp.Where.Add(new SearchParam("ModuleId", ModuleId, SearchType.Equal));
                        List<DNNGo_PowerForms_Field> fieldList = DNNGo_PowerForms_Field.FindAll(qp, out RecordCount);
                        foreach (DNNGo_PowerForms_Field fieldItem in fieldList)
                        {
                            fieldItem.Delete();
                        }

                        //构造需要上传的路径
                        String XmlFilePath = String.Format("{0}PowerForms\\Import\\{1}_{2}", PortalSettings.HomeDirectoryMapPath, xUserTime.UtcTime().ToString("yyyyMMddHHmmssffff"), fuImportFormXml.FileName);
                        FileInfo XmlFile = new FileInfo(XmlFilePath);
                        //判断文件夹是否存在
                        if (!XmlFile.Directory.Exists) XmlFile.Directory.Create();
                        //保存文件
                        fuImportFormXml.SaveAs(XmlFilePath);

                        XmlFormat xf = new XmlFormat(XmlFilePath);

                        List<FieldEntity> XmlFieldList = xf.ToList<FieldEntity>();


                        Int32 InsertResult = 0;
                        foreach (FieldEntity XmlField in XmlFieldList)
                        {
                            DNNGo_PowerForms_Field FieldItem = ieHelper.XmlToEntity(XmlField);


                            FieldItem.CreateUser = UserId;
                            FieldItem.CreateTime = xUserTime.UtcTime();

                            FieldItem.ModuleId = ModuleId;
                            FieldItem.PortalId = PortalId;

                            FieldItem.LastIP = WebHelper.UserHost;
                            FieldItem.LastTime = xUserTime.UtcTime();
                            FieldItem.LastUser = UserId;

                            if (FieldItem.Insert() > 0) InsertResult++;

                        }

                        //提示
                        mTips.LoadMessage("ImportFieldSuccess", EnumTips.Success, this, new String[] { InsertResult.ToString() });

                        //跳转
                        Response.Redirect(xUrl("FieldList"));
                    }
                    else
                    {
                        //上传文件的后缀名错误
                        mTips.IsPostBack = true;
                        mTips.LoadMessage("UploadFieldExtensionError", EnumTips.Warning, this, new String[] { "xml" });
                    }
                }
                else
                {
                    //为上传任何数据
                    mTips.IsPostBack = true;
                    mTips.LoadMessage("ImportFieldNullError", EnumTips.Success, this, new String[] { "" });
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