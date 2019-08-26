using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;


using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search;
using DotNetNuke.Common.Utilities;


namespace DNNGo.Modules.PowerForms
{
    public class baseController :IPortable
    {
        #region "Optional Interfaces"
 
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ExportModule implements the IPortable ExportModule Interface
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="ModuleID">The Id of the module to be exported</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------

        public string ExportModule(int ModuleID)
        {
            string strXML = String.Empty;

            ImportExportHelper ieHelper = new ImportExportHelper();
            ieHelper.ModuleID = ModuleID;

            //查询字段的数据,填充待导出的XML实体
            QueryParam qp = new QueryParam();
            Int32 RecordCount = 0;
            qp.Orderfld = DNNGo_PowerForms_Field._.Sort;
            qp.OrderType = 0;
            qp.Where.Add(new SearchParam("ModuleId", ModuleID, SearchType.Equal));
            List<DNNGo_PowerForms_Field> fieldList = DNNGo_PowerForms_Field.FindAll(qp, out RecordCount);
            List<FieldEntity> xmlFieldList = new List<FieldEntity>();
            List<GallerySettingsEntity> xmlSettingList = new List<GallerySettingsEntity>();
            foreach (DNNGo_PowerForms_Field fieldItem in fieldList)
            {
                xmlFieldList.Add(ieHelper.EntityToXml(fieldItem));
            }

            if (xmlFieldList != null && xmlFieldList.Count > 0)
            {
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


                XmlFormat xf = new XmlFormat(HttpContext.Current.Server.MapPath(String.Format("{0}Resource/xml/FieldEntity.xml", "~/DesktopModules/DNNGo_PowerForms/")));
                strXML = xf.ToXml<FieldEntity>(xmlFieldList, xmlSettingList);
            }
            else
            {
            }

            return strXML;

        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ImportModule implements the IPortable ImportModule Interface
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="ModuleID">The ID of the Module being imported</param>
        /// <param name="Content">The Content being imported</param>
        /// <param name="Version">The Version of the Module Content being imported</param>
        /// <param name="UserID">The UserID of the User importing the Content</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------

        public void ImportModule(int ModuleID, string Content, string Version, int UserId)
        {

            //先清除原有的字段列表
            QueryParam qp = new QueryParam();
            Int32 RecordCount = 0;
            qp.Where.Add(new SearchParam("ModuleId", ModuleID, SearchType.Equal));
            List<DNNGo_PowerForms_Field> fieldList = DNNGo_PowerForms_Field.FindAll(qp, out RecordCount);
            foreach (DNNGo_PowerForms_Field fieldItem in fieldList)
            {
                fieldItem.Delete();
            }

 
            ImportExportHelper ieHelper = new ImportExportHelper();
            ieHelper.ModuleID = ModuleID;
            ieHelper.UserId = UserId;

            if (!String.IsNullOrEmpty(Content))
            {

                //将XML转换为实体
                XmlFormat xf = new XmlFormat();
                xf.XmlDoc.LoadXml(Content);
                List<FieldEntity> XmlFieldList = xf.ToList<FieldEntity>();
                List<GallerySettingsEntity> XmlSettingList = xf.ToList<GallerySettingsEntity>();

                Int32 InsertResult = 0;
                foreach (FieldEntity XmlField in XmlFieldList)
                {
                    DNNGo_PowerForms_Field FieldItem = ieHelper.XmlToEntity(XmlField);

                    FieldItem.CreateUser = UserId;
                    FieldItem.CreateTime = DateTime.Now;

                    FieldItem.LastIP = WebHelper.UserHost;
                    FieldItem.LastTime = DateTime.Now;
                    FieldItem.LastUser = UserId;

                    if (FieldItem.Insert() > 0) InsertResult++;

                }

                //插入设置的记录
                foreach (GallerySettingsEntity XmlSettingItem in XmlSettingList)
                {
                    if (!String.IsNullOrEmpty(XmlSettingItem.SettingName) && !String.IsNullOrEmpty(XmlSettingItem.SettingValue))
                    {
                        ieHelper.UpdateModuleSetting(XmlSettingItem.SettingName, XmlSettingItem.SettingValue);
                    }
                }

            }
        }

        #endregion



   


    }
}