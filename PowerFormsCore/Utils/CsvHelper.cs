using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.OleDb;


namespace DNNGo.Modules.PowerForms
{
    public class CsvHelper
    {



        public static bool SaveAsToFile(List<DNNGo_PowerForms_Field> fieldList, List<DNNGo_PowerForms_Content> DataList, String FilePath,bool ExtraTracking)
        {
          return SaveAsToFile(fieldList, DataList, FilePath, (Int32)EnumExport.Excel, ExtraTracking);
        }



        public static bool SaveAsToFile(List<DNNGo_PowerForms_Field> fieldList, List<DNNGo_PowerForms_Content> DataList,out string FullName,  basePortalModule bpm)
        {
            Int32 EnumExportExtension = bpm.Settings["PowerForms_ExportExtension"] != null && !string.IsNullOrEmpty(bpm.Settings["PowerForms_ExportExtension"].ToString()) ? Convert.ToInt32(bpm.Settings["PowerForms_ExportExtension"]) : (Int32)EnumExport.Excel;
            String FileExtension = "xls";
            switch (EnumExportExtension)
            {
                case (Int32)EnumExport.CSV: FileExtension = "csv"; break;
                case (Int32)EnumExport.Doc: FileExtension = "doc"; break;
                case (Int32)EnumExport.Html: FileExtension = "html"; break;
                case (Int32)EnumExport.TextFile: FileExtension = "txt"; break;
                case (Int32)EnumExport.Xml: FileExtension = "xml"; break;
                default: FileExtension = "xls"; break;
            }

            FullName = bpm.Server.MapPath(String.Format("{0}PowerForms/Export/PowerForms_{1}_{2}.{3}", bpm.PortalSettings.HomeDirectory, bpm.ModuleId, DateTime.Now.ToString("yyyyMMddHHmmssffff"), FileExtension));

            bool ExtraTracking = bpm.ViewSettingT<bool>("PowerForms_ExportExtraTracking", false);

            return SaveAsToFile(fieldList, DataList, FullName, EnumExportExtension, ExtraTracking);
        }





        public static Boolean SaveAsToFile(List<DNNGo_PowerForms_Field> fieldList, List<DNNGo_PowerForms_Content> DataList, String FullName, Int32 EnumExportExtension,bool ExtraTracking)
        {
            bool flag = false;
            try
            {
                ExportDotNet excel = new ExportDotNet();
                excel.Title = "HistoryRecords";
                excel.ExportFileName = FullName;
             

                FileInfo Exportfile = new FileInfo(FullName);
                if (!Exportfile.Directory.Exists)
                {
                    Exportfile.Directory.Create();
                }


                DataTable dt = new DataTable(excel.Title);
                dt = ConvertDataTable(dt, fieldList, DataList, ExtraTracking);



                if (EnumExportExtension == (Int32)EnumExport.Excel)
                {
                    flag = excel.ExportToExcel(dt);
                }
                else if (EnumExportExtension == (Int32)EnumExport.CSV)
                {
                    flag = excel.ExportToCSV(dt);
                }
                else if (EnumExportExtension == (Int32)EnumExport.Doc)
                {
                    flag = excel.ExportToDoc(dt);
                }
                else if (EnumExportExtension == (Int32)EnumExport.Html)
                {
                    flag = excel.ExportToHtml(dt);
                }
                else if (EnumExportExtension == (Int32)EnumExport.TextFile)
                {
                    flag = excel.ExportToTextFile(dt);
                }
                else if (EnumExportExtension == (Int32)EnumExport.Xml)
                {
                    flag = excel.ExportToXml(dt);
                }
                else
                {
                    flag = excel.ExportToExcel(dt);
                }


            }
            catch(Exception ex)
            {
                throw ex;
            }
            return flag;
        }


        public static DataTable ConvertDataTable(DataTable dt, List<DNNGo_PowerForms_Field> fieldList, List<DNNGo_PowerForms_Content> DataList, bool ExtraTracking)
        {
            for (int i = 0; i < fieldList.Count; i++)
            {
                dt.Columns.Add(fieldList[i].Name);
            }

            if (!dt.Columns.Contains("UserName")) dt.Columns.Add("UserName");
            if (!dt.Columns.Contains("Time")) dt.Columns.Add("Time");
            if (!dt.Columns.Contains("IP")) dt.Columns.Add("IP");

            if (ExtraTracking)
            {
                if (!dt.Columns.Contains("Tracking_PageURL")) dt.Columns.Add("Tracking_PageURL");
                if (!dt.Columns.Contains("Tracking_OriginalReferrer")) dt.Columns.Add("Tracking_OriginalReferrer");
                if (!dt.Columns.Contains("Tracking_LandingPage")) dt.Columns.Add("Tracking_LandingPage");
                if (!dt.Columns.Contains("Tracking_UserIP")) dt.Columns.Add("Tracking_UserIP");
                if (!dt.Columns.Contains("Tracking_UserAgent")) dt.Columns.Add("Tracking_UserAgent");
            }

            if (DataList != null && DataList.Count > 0)
            {
                string strModel = string.Empty;

                for (int i = 0; i < DataList.Count; i++)
                {
                    DNNGo_PowerForms_Content ContentItem = DataList[i];

                    if (ContentItem != null && !String.IsNullOrEmpty(ContentItem.ContentValue))
                    {
                        List<DNNGo_PowerForms_ContentItem> DataItem = Common.Deserialize<List<DNNGo_PowerForms_ContentItem>>(ContentItem.ContentValue);

                        if (DataItem != null && DataItem.Count > 0)
                        {

                            DataRow dr = dt.NewRow();



                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                string PropertyName = dt.Columns[j].ColumnName;
                                DNNGo_PowerForms_ContentItem ItemInfo = DataItem.Find(r1 => r1.FieldName == PropertyName);
                                if (ItemInfo != null)
                                {
                                    dr[PropertyName] = ViewContentValue(fieldList, ItemInfo, ContentItem);

                                }
                                else
                                {
                                    if (PropertyName == "UserName")
                                    {
                                        dr["UserName"] = ContentItem.UserName;
                                    }
                                    else if (PropertyName == "Time")
                                    {
                                        dr["Time"] = ContentItem.LastTime.ToString();
                                    }
                                    else if (PropertyName == "IP")
                                    {
                                        dr["IP"] = ContentItem.LastIP;
                                    }

                                }

                            }

                            dt.Rows.Add(dr);
                        }
                    }
                }


            }
            return dt;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Value"></param>
        /// <returns></returns>
        public static String FromatValue(String _Value)
        {
            String result = String.Empty;
            if (!String.IsNullOrEmpty(_Value))
            {
                result = _Value.Replace("\"", "\"\"");//替换英文冒号 英文冒号需要换成两个冒号

                if (result.Contains(",") || result.Contains("\"") ) //含逗号 冒号 换行符的需要放到引号中
                {

                    result = string.Format("\"{0}\"", result);

                }

                if (result.Contains("\r") || result.Contains("\n")) //含逗号 冒号 换行符的需要放到引号中
                {

                    result = result.Replace("\r","").Replace("\n","");

                }


            }
            return result;
        }



        /// <summary>
        /// 显示内容值
        /// </summary>
        /// <param name="ContentItem"></param>
        /// <returns></returns>
        public static String ViewContentValue(List<DNNGo_PowerForms_Field> fieldList,DNNGo_PowerForms_ContentItem ItemInfo,DNNGo_PowerForms_Content ContentItem)
        {

           

            
            if (!String.IsNullOrEmpty(ItemInfo.ContentValue))
            {
                String LiContentValue = ItemInfo.ContentValue;

                DNNGo_PowerForms_Field fielditem = fieldList.Find(r => r.ID == ItemInfo.FieldID);

                if (fielditem != null && fielditem.ID > 0 && fielditem.FieldType == (Int32)EnumViewControlType.TextBox )
                {
                    LiContentValue = HttpUtility.HtmlEncode(ItemInfo.ContentValue);
                }
                else if (ItemInfo.ContentValue.IndexOf("Url://") >= 0)
                {

                    LiContentValue = String.Format("/Portals/{0}/PowerForms/{1}/{2}", ContentItem.PortalId, ContentItem.ModuleId, ItemInfo.ContentValue.Replace("Url://", ""));

                    //看看是否需要加http
                    if (LiContentValue.ToLower().IndexOf("http://") < 0 && LiContentValue.ToLower().IndexOf("https://") < 0)
                    {
                        LiContentValue = string.Format("http://{0}{1}", WebHelper.GetHomeUrl(), LiContentValue);
                    }

                }
                else
                {
                    LiContentValue = ItemInfo.ContentValue;
                }
                return LiContentValue;
            }
            return String.Empty;
        }

         

    }
}