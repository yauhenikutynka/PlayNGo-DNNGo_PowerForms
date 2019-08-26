using System;
using System.Text;
using System.Collections.Generic;
using System.Web;
using DotNetNuke.Common;
using DotNetNuke.Services.Localization;
using System.Web.UI.WebControls;
using System.Collections;

using DotNetNuke.Entities.Modules;
using System.IO;
using System.Web.UI;
using DotNetNuke.Common.Lists;
using System.Web.UI.HtmlControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Users;

namespace DNNGo.Modules.PowerForms
{
    public class TemplateFormat
    {


        #region "属性"
        /// <summary>
        /// 模块基类
        /// </summary>
        private basePortalModule bpm = new basePortalModule();



        private Button _CtlButton;
        /// <summary>
        /// 触发按钮
        /// </summary>
        public Button CtlButton
        {
            get { return _CtlButton; }
            set { _CtlButton = value; }
        }



        private String _ThemeXmlName = String.Empty;
        /// <summary>
        /// 主题XML名称
        /// </summary>
        public String ThemeXmlName
        {
            get { return _ThemeXmlName; }
            set { _ThemeXmlName = value; }
        }

        private PlaceHolder _PhContent = new PlaceHolder();

        public PlaceHolder PhContent
        {
            get { return _PhContent; }
            set { _PhContent = value; }
        }

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
                    _FieldList = DNNGo_PowerForms_Field.FindAllByView(bpm.ModuleId, bpm.UserInfo);
                }
                return _FieldList;
            }
            set { _FieldList = value; }
        }
        #endregion



        #region "方法"

        #region "--关于内容与标题--"

        /// <summary>
        /// 显示标题(通过资源文件)
        /// </summary>
        /// <param name="Title">标题</param>
        /// <param name="DefaultValue">资源文件未定义时默认值</param>
        /// <returns>返回值</returns>
        public String ViewTitle(String Title, String DefaultValue)
        {
            return ViewResourceText(Title, DefaultValue);
        }

        /// <summary>
        /// 显示内容
        /// </summary>
        public String ViewContent(String FieldName, DNNGo_PowerForms_Content DataItem)
        {
            if (DataItem != null && DataItem.ID > 0)
            {
                if ( DataItem[FieldName] != null)
                {
                    return Convert.ToString(DataItem[FieldName]);//找出一般属性
                }
            }
            return string.Empty;
        }

      

        /// <summary>
        /// 显示内容并截取数据
        /// </summary>
        /// <param name="FieldName">字段名</param>
        /// <param name="DataItem">数据项</param>
        /// <param name="Lenght">显示长度</param>
        /// <returns></returns>
        public String ViewContent(String FieldName, DNNGo_PowerForms_Content DataItem, Int32 Lenght)
        {
            return ViewContent(FieldName, DataItem, Lenght, "...");
        }


        /// <summary>
        /// 显示内容并截取数据
        /// </summary>
        /// <param name="FieldName">字段名</param>
        /// <param name="DataItem">数据项</param>
        /// <param name="Lenght">显示长度</param>
        /// <param name="Suffix">终止符号</param>
        /// <returns></returns>
        public String ViewContent(String FieldName, DNNGo_PowerForms_Content DataItem, Int32 Lenght, String Suffix)
        {
            String Content = ViewContent(FieldName, DataItem);//先取内容
            return WebHelper.leftx(Content, Lenght, Suffix);
        }

        /// <summary>
        ///  显示时间
        /// </summary>
        /// <param name="FieldName">字段名</param>
        /// <param name="DataItem">数据项</param>
        /// <param name="TimeFormat">时间格式</param>
        /// <returns></returns>
        public String ViewDateTime(String FieldName, DNNGo_PowerForms_Content DataItem, String TimeFormat)
        { 
            String Content = ViewContent(FieldName, DataItem);//先取内容
            DateTime Temp = xUserTime.UtcTime();
            if (DateTime.TryParse(Content, out Temp))
                return Temp.ToString(TimeFormat);
            else
                return String.Empty;

        }












        /// <summary>
        /// 显示URL控件存放的值
        /// </summary>
        /// <param name="UrlValue"></param>
        /// <returns></returns>
        public String ViewLinkUrl(String UrlValue, String DefaultValue, int PortalId)
        {
            if (!String.IsNullOrEmpty(UrlValue) && UrlValue != "0")
            {
                if (UrlValue.IndexOf("FileID=", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    int FileID = 0;
                    if (int.TryParse(UrlValue.Replace("FileID=", ""), out FileID) && FileID > 0)
                    {
                        var fi = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(FileID);
                        if (fi != null && fi.FileId > 0)
                        {
                            DefaultValue = string.Format("{0}{1}{2}", bpm.PortalSettings.HomeDirectory, fi.Folder, bpm.Server.UrlPathEncode(fi.FileName));
                        }
                    }
                }
                else if (UrlValue.IndexOf("MediaID=", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {


                    int MediaID = 0;
                    if (int.TryParse(UrlValue.Replace("MediaID=", ""), out MediaID) && MediaID > 0)
                    {
                        DNNGo_PowerForms_Files Multimedia = DNNGo_PowerForms_Files.FindByID(MediaID);
                        if (Multimedia != null && Multimedia.ID > 0)
                        {
                            DefaultValue = bpm.Server.UrlPathEncode(bpm.GetPhotoPath(Multimedia.FilePath));// String.Format("{0}{1}", bpm.MemberGroup_PortalSettings.HomeDirectory, Multimedia.FilePath);
                        }

                        if (!String.IsNullOrEmpty(DefaultValue))
                        {
                            if (DefaultValue.ToLower().IndexOf("http://") < 0 && DefaultValue.ToLower().IndexOf("https://") < 0)
                            {
                                DefaultValue = string.Format("{2}://{0}{1}", WebHelper.GetHomeUrl(), DefaultValue, bpm.PortalSettings.SSLEnabled ? "https" : "http");
                            }
                           

                        }
                    }
                }
                else if (UrlValue.IndexOf("TabID=", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {

                    DefaultValue = Globals.NavigateURL(Convert.ToInt32(UrlValue.Replace("TabID=", "")), false, bpm.PortalSettings, Null.NullString, "", "");

                }
                else
                {
                    DefaultValue = UrlValue;
                }
            }
            return DefaultValue;

        }


        public String ViewLinkUrl(String UrlValue, String DefaultValue)
        {
            return ViewLinkUrl(UrlValue, DefaultValue, bpm.PortalId);
        }


        public String ViewLinkUrl(String UrlValue)
        {
            String DefaultValue = String.Empty;
            if (UrlValue.IndexOf("MediaID=", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                DefaultValue = String.Format("{0}Resource/images/no_image.png", bpm.ModulePath);
            }
            return ViewLinkUrl(UrlValue, DefaultValue, bpm.PortalId);
        }



       
        #endregion

        #region "--关于图片--"

        /// <summary>
        /// 显示图片地址
        /// </summary>
        /// <param name="DataItem"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public String PictureUrl(DNNGo_PowerForms_Field DataItem, String FieldName, String DefaultValue)
        {
            String _PictureUrl = ViewItemSettingT<String>(DataItem, FieldName, "");

            return ViewLinkUrl(_PictureUrl, DefaultValue);
        }

        /// <summary>
        /// 显示图片地址
        /// </summary>
        /// <param name="DataItem"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public String PictureUrl(String FieldName, String DefaultValue)
        {
            String _PictureUrl = ViewXmlSettingT<String>( FieldName, "");

            return ViewLinkUrl(_PictureUrl, DefaultValue);
        }




        #endregion

        #region "--关于数据筛选--"

        /// <summary>
        /// 在字段列表中查找单个字段
        /// </summary>
        /// <param name="FieldList"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public DNNGo_PowerForms_Field FindFieldItem(List<DNNGo_PowerForms_Field> FieldList, String FieldName)
        {
            return FieldList.Find(r => r.Name == FieldName);
        }


        #endregion


        #region "--关于链接跳转--"

        /// <summary>
        /// 返回到列表
        /// </summary>
        /// <returns></returns>
        public String GoUrl()
        {
            return Globals.NavigateURL(bpm.TabId);
        }


 


 

        /// <summary>
        /// 跳转到登录页面
        /// </summary>
        /// <returns></returns>
        public String GoLogin()
        {
            return  Globals.NavigateURL(bpm.PortalSettings.LoginTabId, "Login", "returnurl=" +  HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl));
        }
 

        /// <summary>
        /// 填充为完整的URL
        /// </summary>
        public String GoFullUrl(String goUrl)
        {
            return FullPortalUrl(goUrl);
        }
        /// <summary>
        /// 填充为完整的URL
        /// </summary>
        public String GoFullUrl()
        {
            return String.Format("{1}://{0}", WebHelper.GetHomeUrl(), bpm.PortalSettings.SSLEnabled ? "https" : "http");
        }

 

        private String _PortalUrl = String.Empty;
        /// <summary>
        /// 站点URL (可以在绑定的时候用到)
        /// </summary>
        public String PortalUrl
        {
            get
            {
                if (String.IsNullOrEmpty(_PortalUrl))
                {
                 
                   _PortalUrl = String.Format("{0}://{1}", bpm. IsSSL ? "https" : "http", WebHelper.GetHomeUrl());

                }
                return _PortalUrl;
            }
        }

        /// <summary>
        /// 填充目标的URL
        /// </summary>
        /// <param name="_Url"></param>
        /// <returns></returns>
        public String FullPortalUrl(String _Url)
        {
            if (!String.IsNullOrEmpty(_Url))
            {
                if (_Url.ToLower().IndexOf("http://") < 0 && _Url.ToLower().IndexOf("https://") < 0)
                {
                    _Url = string.Format("{0}{1}", PortalUrl, _Url);
                }
            }
            return _Url;
        }


        #endregion


        #region "--关于模版内容格式化--"

        /// <summary>
        /// 显示内容值
        /// </summary>
        /// <param name="ContentItem"></param>
        /// <returns></returns>
        public String ViewContentValue(DNNGo_PowerForms_ContentItem ContentItem)
        {
            String LiContentValue = String.Empty;
            if (!String.IsNullOrEmpty(ContentItem.ContentValue))
            {
                DNNGo_PowerForms_Field fielditem = FieldList.Find(r => r.ID == ContentItem.FieldID);

                if (fielditem != null && fielditem.ID > 0)
                {
                    if (fielditem.FieldType == (Int32)EnumViewControlType.TextBox)
                    {
                        LiContentValue = HttpUtility.HtmlEncode(ContentItem.ContentValue);

                    }
                    else if (fielditem.FieldType == (Int32)EnumViewControlType.TextBox_Email)
                    {
                        LiContentValue = String.Format("<a href=\"mailto:{0}\">{0}</a>", ContentItem.ContentValue);

                    }
                    else if (fielditem.FieldType == (Int32)EnumViewControlType.MultipleFilesUpload || fielditem.FieldType == (Int32)EnumViewControlType.FileUpload)
                    {
                        if (ContentItem.ContentValue.IndexOf("Url://") >= 0)
                        {
                            StringBuilder fileBuilder = new StringBuilder();
                            List<String> files = WebHelper.GetList(ContentItem.ContentValue, "<|>");
                            if (files != null && files.Count > 0)
                            {
                                foreach (var file in files)
                                {
                                    if (!string.IsNullOrEmpty(file) && file.IndexOf("Url://", StringComparison.CurrentCultureIgnoreCase) >= 0)
                                    {
                                        //DotNetNuke.Entities.Users.
                                        
                                        String FileUrl = String.Format("{0}Resource_Service.aspx?Token=downLoadformfile&PortalId={1}&TabId={2}&ModuleId={3}&file={4}", bpm.ModulePath, bpm.PortalId,bpm.TabId,  bpm.ModuleId, bpm.Server.UrlEncode(  CryptionHelper.Base64Encode(  CryptionHelper.EncryptString( file.Replace("Url://", "")))));

                                        if (fileBuilder.Length > 0) fileBuilder.Append("<br/>");

                                        fileBuilder.AppendFormat("<a href=\"{0}\" target=\"_blank\">{1}</a>", GoFullUrl(FileUrl), file.Replace("Url://", "")).AppendLine();



                                        //String FileUrl = bpm.ResolveUrl(String.Format("~/Portals/{0}/PowerForms/{1}/{2}", bpm.PortalId, bpm.ModuleId, file.Replace("Url://", "")));

                                        //if (fileBuilder.Length > 0) fileBuilder.Append("<br/>");

                                        //fileBuilder.AppendFormat("<a href=\"{0}\" target=\"_blank\">{1}</a>", GoFullUrl(FileUrl), file.Replace("Url://", "")).AppendLine();

                                    }
                                }
                            }
                            LiContentValue = fileBuilder.ToString();
                        }

                    }
                    else
                    {
                        LiContentValue = ContentItem.ContentValue;
                    }
                } else
                {
                    LiContentValue = ContentItem.ContentValue;
                }


                
                
               
            }
            return LiContentValue;
        }

        /// <summary>
        /// 显示内容值
        /// </summary>
        /// <param name="ContentItem"></param>
        /// <returns></returns>
        public String ViewContentValue2(DNNGo_PowerForms_ContentItem ContentItem)
        {
            String LiContentValue = String.Empty;
            if (!String.IsNullOrEmpty(ContentItem.ContentValue))
            {
                DNNGo_PowerForms_Field fielditem = FieldList.Find(r => r.ID == ContentItem.FieldID);

                if (fielditem != null && fielditem.ID > 0 && fielditem.FieldType == (Int32)EnumViewControlType.TextBox)
                {
                    LiContentValue = HttpUtility.HtmlEncode(ContentItem.ContentValue);

                }
                else if (ContentItem.ContentValue.IndexOf("Url://") >= 0)
                {

                    LiContentValue = bpm.ResolveUrl(String.Format("~/Portals/{0}/PowerForms/{1}/{2}", bpm.PortalId, bpm.ModuleId, ContentItem.ContentValue.Replace("Url://", "")));

                    //看看是否需要加http
                    if (LiContentValue.ToLower().IndexOf("http://") < 0 && LiContentValue.ToLower().IndexOf("https://") < 0)
                    {
                        LiContentValue = string.Format("{2}://{0}{1}", WebHelper.GetHomeUrl(), LiContentValue, bpm.PortalSettings.SSLEnabled ? "https" : "http");
                    }

                
                }
                else
                {
                    LiContentValue = ContentItem.ContentValue;
                }

            }
            return LiContentValue;
        }


        /// <summary>
        /// 根据字段名查询值
        /// </summary>
        /// <param name="Items"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public String ViewSearchContentValue(List<DNNGo_PowerForms_ContentItem> Items,String FieldName)
        {
            String ContentValue = String.Empty;
            if (Items != null && Items.Count > 0 && Items.Exists(r => r.FieldName == FieldName))
            {
                var item = Items.Find(r => r.FieldName == FieldName);
                if (item != null && !String.IsNullOrEmpty(item.FieldName))
                {
                    ContentValue = item.ContentValue;
                }
            }
            return ContentValue;
        }

        /// <summary>
        /// 根据分组查询相应的字段
        /// </summary>
        /// <param name="FieldList">字段列表</param>
        /// <param name="GroupName">分组项</param>
        /// <returns></returns>
        public List<DNNGo_PowerForms_Field> ViewSearchFieldList(List<DNNGo_PowerForms_Field> FieldList, String GroupName)
        {
            List<DNNGo_PowerForms_Field> list = new List<DNNGo_PowerForms_Field>();
            DNNGo_PowerForms_Group GroupItem = DNNGo_PowerForms_Group.FindByName(GroupName, bpm.ModuleId);
            if(GroupItem!= null && GroupItem.ID > 0)
            {
                list = ViewSearchFieldList(FieldList, GroupItem);
            }
            return list;
        }

        /// <summary>
        /// 根据分组查询相应的字段
        /// </summary>
        /// <param name="FieldList">字段列表</param>
        /// <param name="GroupItem">分组项</param>
        /// <returns></returns>
        public List<DNNGo_PowerForms_Field> ViewSearchFieldList(List<DNNGo_PowerForms_Field> FieldList, DNNGo_PowerForms_Group GroupItem)
        {
            return FieldList.FindAll(r1=>r1.GroupID == GroupItem.ID);
        }


        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="DataItem"></param>
        /// <returns></returns>
        public List<DNNGo_PowerForms_ContentItem> Conversion(DNNGo_PowerForms_Content DataItem)
        {
            List<DNNGo_PowerForms_ContentItem> list = new List<DNNGo_PowerForms_ContentItem>();
            if (DataItem != null && DataItem.ID > 0 && !String.IsNullOrEmpty(DataItem.ContentValue))
            {
                list = Common.Deserialize<List<DNNGo_PowerForms_ContentItem>>(DataItem.ContentValue);
            }
            return list;
        }

        /// <summary>
        /// 查找你所需要的字段
        /// </summary>
        /// <param name="FieldList">字段列表</param>
        /// <param name="FieldName">字段名</param>
        /// <returns></returns>
        public DNNGo_PowerForms_Field ViewFieldItem(List<DNNGo_PowerForms_Field> FieldList, String FieldName)
        {
            DNNGo_PowerForms_Field FieldValue = new DNNGo_PowerForms_Field();
            if (FieldList != null && FieldList.Count > 0)
            {
                DNNGo_PowerForms_Field FieldItem = FieldList.Find(r1 => r1.Name == FieldName);
                if (FieldItem != null && FieldItem.ID > 0)
                {
                    FieldValue = FieldItem;
                }
            }
            return FieldValue;
        }


        #endregion



        #region "--关于控件格式化--"

        /// <summary>
        /// 生成控件
        /// </summary>
        /// <param name="FieldList">字段列表</param>
        /// <param name="FieldName">字段名</param>
        /// <returns></returns>
        public String ViewControl(List<DNNGo_PowerForms_Field> FieldList, String FieldName)
        {
            String ControlValue = String.Empty;
            if (FieldList != null && FieldList.Count > 0)
            {
                DNNGo_PowerForms_Field FieldItem = FieldList.Find(r1 => r1.Name == FieldName);
                if (FieldItem != null && FieldItem.ID > 0)
                {
                    ControlValue = ViewControl(FieldItem);
                }
            }
            return ControlValue;
        }



        /// <summary>
        /// 生成控件
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <returns></returns>
        public String ViewControl(DNNGo_PowerForms_Field FieldItem)
        {
            String ControlName = ViewControlName(FieldItem);
            String ControlID = ViewControlID(FieldItem);
            String ControlHtml = String.Empty;//控件的HTML
            if (FieldItem.FieldType == (Int32)EnumViewControlType.CheckBox)
                ControlHtml = ViewCreateCheckBox(FieldItem, ControlName, ControlID);
            else if (FieldItem.FieldType == (Int32)EnumViewControlType.Confirm)
                ControlHtml = ViewCreateConfirm(FieldItem, ControlName, ControlID);
            else if (FieldItem.FieldType == (Int32)EnumViewControlType.CheckBoxList)
                ControlHtml = ViewCreateCheckBoxList(FieldItem, ControlName, ControlID);
            else if (FieldItem.FieldType == (Int32)EnumViewControlType.DatePicker)
                ControlHtml = ViewCreateDatePicker(FieldItem, ControlName, ControlID);
            else if (FieldItem.FieldType == (Int32)EnumViewControlType.DropDownList)
                ControlHtml = ViewCreateDropDownList(FieldItem, ControlName, ControlID);
            else if (FieldItem.FieldType == (Int32)EnumViewControlType.FileUpload)
                ControlHtml = ViewCreateFileUpload(FieldItem, ControlName, ControlID);
            else if (FieldItem.FieldType == (Int32)EnumViewControlType.MultipleFilesUpload)
                ControlHtml = ViewCreateMultipleFileUpload(FieldItem, ControlName, ControlID);
            else if (FieldItem.FieldType == (Int32)EnumViewControlType.Label)
                ControlHtml = ViewCreateLabel(FieldItem, ControlName, ControlID);
            else if (FieldItem.FieldType == (Int32)EnumViewControlType.Html)
                ControlHtml = ViewCreateHtml(FieldItem, ControlName, ControlID);
            else if (FieldItem.FieldType == (Int32)EnumViewControlType.ListBox)
                ControlHtml = ViewCreateListBox(FieldItem, ControlName, ControlID);
            else if (FieldItem.FieldType == (Int32)EnumViewControlType.RadioButtonList)
                ControlHtml = ViewCreateRadioButtonList(FieldItem, ControlName, ControlID);
            else if (FieldItem.FieldType == (Int32)EnumViewControlType.RichTextBox)
                ControlHtml = ViewCreateRichTextBox(FieldItem, ControlName, ControlID);
            else if (FieldItem.FieldType == (Int32)EnumViewControlType.TextBox)
                ControlHtml = ViewCreateTextBox(FieldItem, ControlName, ControlID);
            else if (FieldItem.FieldType == (Int32)EnumViewControlType.TextBox_Email)
                ControlHtml = ViewCreateTextBoxByEmail(FieldItem, ControlName, ControlID);
            else if (FieldItem.FieldType == (Int32)EnumViewControlType.TextBox_DisplayName)
                ControlHtml = ViewCreateTextBoxByDisplayName(FieldItem, ControlName, ControlID);
            else if (FieldItem.FieldType == (Int32)EnumViewControlType.DropDownList_Country)
                ControlHtml = ViewCreateDropDownListByCountry(FieldItem, ControlName, ControlID);
            else if (FieldItem.FieldType == (Int32)EnumViewControlType.DropDownList_Region)
                ControlHtml = ViewCreateDropDownListByRegion(FieldItem, ControlName, ControlID);
            else if (FieldItem.FieldType == (Int32)EnumViewControlType.DropDownList_SendEmail)
                ControlHtml = ViewCreateDropDownList(FieldItem, ControlName, ControlID);
         
            return ControlHtml;


           
        }


        #region "创建HTML控件方法集合"
        /// <summary>
        /// 创建TextBox
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <returns></returns>
        public String ViewCreateTextBox(DNNGo_PowerForms_Field FieldItem, String ControlName, String ControlID)
        {
            
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML
 
           //看行数决定控件的是什么控件
            if (FieldItem.Rows > 1)
            {
                ControlHtml.AppendFormat("<textarea  name=\"{0}\" id=\"{1}\"", ControlName, ControlID);

                if (!String.IsNullOrEmpty(FieldItem.ToolTip)) ControlHtml.AppendFormat(" placeholder=\"{0}\" title=\"{0}\"", FieldItem.ToolTip);
                //if (!String.IsNullOrEmpty(FieldItem.ToolTip)) ControlHtml.AppendFormat("title=\"{0}\"", FieldItem.ToolTip);

                ControlHtml.AppendFormat(" class=\"{0} {1} {2}\"", ViewVerification(FieldItem), FieldItem.Required == 1 ? "required" : "", FieldItem.FieldType == (Int32)EnumViewControlType.RichTextBox ? "tinymce" : "");

                ControlHtml.AppendFormat(" style=\"width:{0}{1};height:{2}px;\" rows=\"{3}\"", FieldItem.Width, ViewWidthSuffix(FieldItem), FieldItem.Rows * 40, FieldItem.Rows);


                if (FieldItem.InputLength > 0)
                {
                    //ControlHtml.AppendFormat(" maxlength=\"{0}\"", FieldItem.InputLength);
                }

                ControlHtml.Append(" >");

                //默认值
                if (!String.IsNullOrEmpty(FieldItem.DefaultValue)) ControlHtml.Append( FieldItem.DefaultValue);
               
                ControlHtml.Append("</textarea>");
                 

            }
            else
            {
                ControlHtml.AppendFormat("<input type=\"text\" name=\"{0}\" id=\"{1}\"", ControlName, ControlID);

                if (!String.IsNullOrEmpty(FieldItem.ToolTip)) ControlHtml.AppendFormat(" placeholder=\"{0}\" title=\"{0}\"", FieldItem.ToolTip);
                //if (!String.IsNullOrEmpty(FieldItem.ToolTip)) ControlHtml.AppendFormat(" title=\"{0}\"", FieldItem.ToolTip);

                ControlHtml.AppendFormat(" class=\"{0} {1}\"", ViewVerification(FieldItem), FieldItem.Required == 1 ? "required" : "");

                if (FieldItem.InputLength > 0)
                {
                    //ControlHtml.AppendFormat(" maxlength=\"{0}\"", FieldItem.InputLength);
                }

                ControlHtml.AppendFormat(" style=\"width:{0}{1};\"", FieldItem.Width, ViewWidthSuffix(FieldItem));

                if (!String.IsNullOrEmpty(FieldItem.DefaultValue)) ControlHtml.AppendFormat(" value=\"{0}\"", FieldItem.DefaultValue);

                ControlHtml.Append(" />");
            }
 
            return ControlHtml.ToString();
        }


        public String ViewCreateTextBoxByEmail(DNNGo_PowerForms_Field FieldItem, String ControlName, String ControlID)
        {
            if (bpm.UserId > 0)
            {

                FieldItem.DefaultValue = bpm.UserInfo.Email;
            }
            else
            {
                //FieldItem.DefaultValue = "Please enter your e-mail";
                FieldItem.DefaultValue = String.Empty;
            }
 
            return ViewCreateTextBox(FieldItem, ControlName, ControlID);
        }

        public String ViewCreateTextBoxByDisplayName(DNNGo_PowerForms_Field FieldItem, String ControlName, String ControlID)
        {
            if (bpm.UserId > 0)
            {

                FieldItem.DefaultValue = bpm.UserInfo.DisplayName;
            }
            else
            {
                //FieldItem.DefaultValue = "Anonymous";
                FieldItem.DefaultValue = String.Empty;
            }

            return ViewCreateTextBox(FieldItem, ControlName, ControlID);
        }



        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateCheckBox(DNNGo_PowerForms_Field FieldItem, String ControlName, String ControlID)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML


            ControlHtml.AppendFormat("<input type=\"checkbox\" name=\"{0}\" id=\"{1}\"", ControlName, ControlID);

            ControlHtml.AppendFormat(" class=\"{0}\"", ViewVerification(FieldItem));

            if (!String.IsNullOrEmpty(FieldItem.ToolTip)) ControlHtml.AppendFormat(" title=\"{0}\"", FieldItem.ToolTip);
            
            if (!String.IsNullOrEmpty(FieldItem.DefaultValue))
            {
                Boolean DefaultValue,b;DefaultValue =b = false;
                if (FieldItem.DefaultValue == "1" || FieldItem.DefaultValue.Equals(Boolean.TrueString, StringComparison.CurrentCultureIgnoreCase))
                    DefaultValue = true;
                else if (FieldItem.DefaultValue == "0" || FieldItem.DefaultValue.Equals(Boolean.FalseString, StringComparison.CurrentCultureIgnoreCase))
                    DefaultValue = false;
                else if (Boolean.TryParse(FieldItem.DefaultValue.ToLower(), out b))
                    DefaultValue = b;

               if(DefaultValue) ControlHtml.Append(" checked=\"checked\"");
            }
 
            ControlHtml.Append(" />");

            //提示的关键字用作是后面的描述
            if (!String.IsNullOrEmpty(FieldItem.ToolTip))
            {
                ControlHtml.AppendFormat(" <label for=\"{0}\" title=\"{1}\" style=\"display:inline;\">{1}</label>", ControlID, FieldItem.ToolTip);
            }
 
            return ControlHtml.ToString();
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateConfirm(DNNGo_PowerForms_Field FieldItem, String ControlName, String ControlID)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML


            ControlHtml.AppendFormat("<input type=\"checkbox\" name=\"{0}\" id=\"{1}\"", ControlName, ControlID);

            ControlHtml.AppendFormat(" class=\"form-confirm validate[required]\"");
 

            if (!String.IsNullOrEmpty(FieldItem.ToolTip)) ControlHtml.AppendFormat(" data-errormessage=\"{0}\"", FieldItem.ToolTip);

            if (!String.IsNullOrEmpty(FieldItem.DefaultValue))
            {
                Boolean DefaultValue, b; DefaultValue = b = false;
                if (FieldItem.DefaultValue == "1" || FieldItem.DefaultValue.Equals(Boolean.TrueString, StringComparison.CurrentCultureIgnoreCase))
                    DefaultValue = true;
                else if (FieldItem.DefaultValue == "0" || FieldItem.DefaultValue.Equals(Boolean.FalseString, StringComparison.CurrentCultureIgnoreCase))
                    DefaultValue = false;
                else if (Boolean.TryParse(FieldItem.DefaultValue.ToLower(), out b))
                    DefaultValue = b;

                if (DefaultValue) ControlHtml.Append(" checked=\"checked\"");
            }

            ControlHtml.Append(" />");

            //提示的关键字用作是后面的描述
            if (!String.IsNullOrEmpty(FieldItem.Description))
            {
                ControlHtml.AppendFormat(" <label for=\"{0}\">{1}</label>", ControlID, FieldItem.Description);
            }

            return ControlHtml.ToString();
        }
        






        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateCheckBoxList(DNNGo_PowerForms_Field FieldItem, String ControlName, String ControlID)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML
            ControlHtml.AppendFormat("<span id=\"{1}\" name=\"{0}\" ", ControlName, ControlID);

            if (!String.IsNullOrEmpty(FieldItem.ToolTip)) ControlHtml.AppendFormat(" title=\"{0}\"", FieldItem.ToolTip);

            ControlHtml.Append(">");

            if (!String.IsNullOrEmpty(FieldItem.FiledList))
            {
                List<String> list = WebHelper.GetList(FieldItem.FiledList.Replace("\r\n", "{|}").Replace("\r", "{|}"), "{|}");
                List<String> DefaultListValue = WebHelper.GetList(FieldItem.DefaultValue);


                if (FieldItem.Direction == (Int32)EnumControlDirection.Horizontal && FieldItem.ListColumn > 1)
                {

                    QueryParam qp = new QueryParam();
                    qp.PageSize = FieldItem.ListColumn;
                    qp.RecordCount = list.Count;
                    HtmlTable table = new HtmlTable();
                    table.Attributes.Add("role", "presentation");
                    for (int i = 1;i <= qp.Pages;i++)
                    {
                        List<String> rowLis = Common.Split<String>(list, i, qp.PageSize);
                        HtmlTableRow row = new HtmlTableRow();
                        
                        foreach (String rowS in rowLis)
                        {
                            HtmlTableCell cell = new HtmlTableCell();
                            cell.InnerText = ViewCreateCheckBoxListOption(FieldItem, ControlName, ControlID, rowS, list.IndexOf(rowS));

                            row.Cells.Add(cell);
                        }
                        table.Rows.Add(row);
                    }


                    StringBuilder sb = new StringBuilder();
                    table.RenderControl(new HtmlTextWriter(new StringWriter(sb)));
                    ControlHtml.Append(sb.ToString());


                }
                else
                {
                    for (Int32 i = 0; i < list.Count; i++)
                    {
                        ControlHtml.Append(ViewCreateCheckBoxListOption(FieldItem, ControlName, ControlID, list[i],i));

                        if (FieldItem.Direction == (Int32)EnumControlDirection.Vertical) ControlHtml.Append("<br />");
                    }
                }
            }

            ControlHtml.Append(" </span>");
            return ControlHtml.ToString();
        }

        public String ViewCreateCheckBoxListOption(DNNGo_PowerForms_Field FieldItem, String ControlName, String ControlID,String list_i,int i)
        {
            List<String> DefaultListValue = WebHelper.GetList(FieldItem.DefaultValue);

            StringBuilder ControlHtml = new StringBuilder();//控件的HTML

            String TextBoxString = String.Empty;
            if (list_i.IndexOf("[TextBox]", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                list_i = Common.ReplaceNoCase(list_i, "[TextBox]", "");
                TextBoxString = String.Format("<input type=\"text\" class=\"other_textbox\" data-forid=\"{1}_{2}\" name=\"{0}ot{2}\" id=\"{1}ot{2}\" />", ControlName, ControlID, i);
            }

            String CheckedStr = DefaultListValue.Count > 0 && DefaultListValue.Contains(list_i) ? " checked=\"checked\"" : "";

            ControlHtml.AppendFormat("<input id=\"{1}_{2}\" type=\"checkbox\" name=\"{0}\"", ControlName, ControlID, i);

            if (FieldItem.Required == 1) ControlHtml.AppendFormat(" class=\"{0}\" ", " validate[minCheckbox[1]]");

            ControlHtml.AppendFormat(" value=\"{0}\" data-value=\"{0}\"  {1} />", list_i, CheckedStr);

            ControlHtml.AppendFormat(" <label for=\"{0}_{1}\" style=\"display:inline;\">{2} {3}</label>  ", ControlID, i, list_i, TextBoxString);

            return ControlHtml.ToString();
        }




        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateDatePicker(DNNGo_PowerForms_Field FieldItem, String ControlName, String ControlID)
        {

            FieldItem.Verification = (Int32)EnumVerification.optional;//临时将验证方式去掉,这样在各种时间格式时均支持。

            StringBuilder ControlHtml = new StringBuilder();//控件的HTML

            ControlHtml.AppendFormat("<input type=\"text\" name=\"{0}\" id=\"{1}\"", ControlName, ControlID);

            if (!String.IsNullOrEmpty(FieldItem.ToolTip)) ControlHtml.AppendFormat(" placeholder=\"{0}\" title=\"{0}\"", FieldItem.ToolTip);

            ControlHtml.AppendFormat(" class=\"PowerForms_Calendar jquery-datepick {0} {1}\"", ViewVerification(FieldItem), FieldItem.Required == 1 ? "required" : "");
            
            ControlHtml.AppendFormat(" style=\"width:{0}{1};\"", FieldItem.Width, ViewWidthSuffix(FieldItem));

            if (!String.IsNullOrEmpty(FieldItem.DefaultValue)) ControlHtml.AppendFormat(" value=\"{0}\"", FieldItem.DefaultValue);

            //ControlHtml.AppendFormat(" onclick=\"{0}\"", Calendar.InvokePopupCal(ControlID,bpm.Page));

            //ControlHtml.AppendFormat(" data-date-format=\"{0}\" data-date-viewmode=\"years\"", System.Globalization.DateTimeFormatInfo.InvariantInfo.ShortDatePattern);
            


            ControlHtml.Append(" />");



        
 
            //ControlHtml.Append(ViewCreateTextBox(FieldItem,ControlName,ControlID));

            //ControlHtml.Append("<script type=\"text/javascript\">");
            //ControlHtml.Append("jQuery(function($) {").AppendLine();
            //ControlHtml.AppendFormat("		    $(\"#{0}\").datepicker({{changeMonth: true,changeYear: true}});", ControlID).AppendLine();
            //ControlHtml.AppendFormat("          $('#{0}').change( function() {{ ", ControlID).AppendLine();
            //ControlHtml.AppendFormat("              $('#{0}').validationEngine('validate');",ControlID).AppendLine();
            //ControlHtml.Append("               });").AppendLine();
            //ControlHtml.Append("});").AppendLine();
            //ControlHtml.Append("</script>");
            return ControlHtml.ToString();
        }

        /// <summary>
        /// 创建下拉列表
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateDropDownList(DNNGo_PowerForms_Field FieldItem, String ControlName, String ControlID,String CssClass = "")
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML
            ControlHtml.AppendFormat("<select name=\"{0}\" id=\"{1}\" ", ControlName, ControlID);


            if (FieldItem.AssociatedControl > 0)
            {
                DNNGo_PowerForms_Field AssociatedControl = DNNGo_PowerForms_Field.FindByKeyForEdit(FieldItem.AssociatedControl);
                if (AssociatedControl != null && AssociatedControl.ID > 0)
                {
                    ControlHtml.AppendFormat(" data-for=\"{0}\" ", ViewControlID(AssociatedControl));
                }
            }


            if (!String.IsNullOrEmpty(FieldItem.ToolTip)) ControlHtml.AppendFormat(" title=\"{0}\"", FieldItem.ToolTip);

            ControlHtml.AppendFormat(" style=\"width:{0}{1};\"", FieldItem.Width, ViewWidthSuffix(FieldItem));

            ControlHtml.AppendFormat(" class=\"{0} {1} {2}\">", ViewVerification(FieldItem), FieldItem.Required == 1 ? "required" : "", CssClass);

            //加一个默认选择符
            ControlHtml.AppendFormat("<option value=\"\">{0}</option>", bpm.ViewResourceText("lblGroupSelect", "==Please select=="));

            if (!String.IsNullOrEmpty(FieldItem.FiledList))
            {
                List<String> list = WebHelper.GetList(FieldItem.FiledList.Replace("\r\n", "{|}").Replace("\r", "{|}"), "{|}");
                for (Int32 i = 0; i < list.Count; i++)
                {
                    
                    //判断是否包含有键值对,将键值对分离开
                    if (list[i].IndexOf(":") > 0)
                    {
                        List<String> ItemKeyValue = WebHelper.GetList(list[i],":");
                        String CheckedStr = FieldItem.DefaultValue.IndexOf(ItemKeyValue[1], StringComparison.CurrentCultureIgnoreCase) >= 0 ? "selected=\"selected\"" : "";
                        ControlHtml.AppendFormat("<option {0} value=\"{1}\">{2}</option>", CheckedStr, ItemKeyValue[1], ItemKeyValue[0]);
                    }
                    else
                    {
                        String CheckedStr = FieldItem.DefaultValue.IndexOf(list[i], StringComparison.CurrentCultureIgnoreCase) >= 0 ? " selected=\"selected\"" : "";
                        ControlHtml.AppendFormat("<option {0} value=\"{1}\">{1}</option>", CheckedStr, list[i]);
                    }
                }
            }
            ControlHtml.Append(" </select>");
            return ControlHtml.ToString();
        }


        /// <summary>
        /// 创建下拉列表by 国家
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <param name="ControlID"></param>
        /// <returns></returns>
        public String ViewCreateDropDownListByCountry(DNNGo_PowerForms_Field FieldItem, String ControlName, String ControlID)
        {
            //构造国家数据
            StringBuilder sb = new StringBuilder(); 
            ListEntryInfoCollection entryCollection = new ListController().GetListEntryInfoCollection("Country");
            if (entryCollection != null && entryCollection.Count > 0)
            {
                foreach (ListEntryInfo info in entryCollection)
                {
                    if (info != null && !String.IsNullOrEmpty(info.Text))
                    {
                        sb.AppendFormat("\r\n{0}:{1}", info.Text, info.Value);
                    }
                }
                FieldItem.FiledList = sb.ToString();
            }

            //构造当前用户默认国家
            if (bpm.UserId > 0 && bpm.UserInfo.Profile != null && !String.IsNullOrEmpty(bpm.UserInfo.Profile.Country))
            {
                FieldItem.DefaultValue = bpm.UserInfo.Profile.Country;
            }
     
            return ViewCreateDropDownList(FieldItem, ControlName, ControlID, "country");
        }


        public String ViewCreateDropDownListByRegion(DNNGo_PowerForms_Field FieldItem, String ControlName, String ControlID)
        {
        
            return ViewCreateDropDownList(FieldItem, ControlName, ControlID, "region");
        }



        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateFileUpload(DNNGo_PowerForms_Field FieldItem, String ControlName, String ControlID)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML
            ControlHtml.AppendFormat("<input type=\"file\" name=\"{0}\" id=\"{1}\"", ControlName, ControlID);

            if (!String.IsNullOrEmpty(FieldItem.ToolTip)) ControlHtml.AppendFormat(" title=\"{0}\"", FieldItem.ToolTip);

            ControlHtml.AppendFormat(" class=\"ajaxUpload validate[{0}{1}maxSize[999]] {0}\"", FieldItem.Required == 1 ? "required" : "", FieldItem.Required == 1 ? "," : "");


            ControlHtml.AppendFormat(" style=\"width:{0}{1};\"", FieldItem.Width, ViewWidthSuffix(FieldItem));

            ControlHtml.Append(" />");


            ControlHtml.AppendFormat(" <img id=\"png_upload_{0}\" src=\"\" style=\"display:none\" />", ControlID);
            return ControlHtml.ToString();
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateMultipleFileUpload(DNNGo_PowerForms_Field FieldItem, String ControlName, String ControlID)
        {
      

            StringBuilder ControlHtml = new StringBuilder();//控件的HTML

            ControlHtml.AppendFormat("<div id=\"plupload{0}\" class=\"plupload\" data-id=\"{0}\"><p>Your browser doesn't have Flash, Silverlight or HTML5 support.</p></div>", ControlID);
            ControlHtml.AppendFormat("<input type=\"hidden\" id=\"{0}\" name=\"{1}\" />", ControlID, ControlName);
            

            return ControlHtml.ToString();
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateLabel(DNNGo_PowerForms_Field FieldItem, String ControlName, String ControlID)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML

             ControlHtml.AppendFormat("<span  name=\"{0}\" id=\"{1}\"", ControlName, ControlID);

             
             if (!String.IsNullOrEmpty(FieldItem.ToolTip)) ControlHtml.AppendFormat(" title=\"{0}\"", FieldItem.ToolTip);

             ControlHtml.AppendFormat(">{0}</span>", FieldItem.DefaultValue);
            return ControlHtml.ToString();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateHtml(DNNGo_PowerForms_Field FieldItem, String ControlName, String ControlID)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML

            ControlHtml.AppendFormat("<div  name=\"{0}\" id=\"{1}\"", ControlName, ControlID);
            if (!String.IsNullOrEmpty(FieldItem.ToolTip)) ControlHtml.AppendFormat(" title=\"{0}\"", FieldItem.ToolTip);

            ControlHtml.AppendFormat(">{0}</div>", FieldItem.DefaultValue);
            return ControlHtml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateListBox(DNNGo_PowerForms_Field FieldItem, String ControlName, String ControlID)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML
            ControlHtml.AppendFormat("<select name=\"{0}\" id=\"{1}\"", ControlName, ControlID);

            if (!String.IsNullOrEmpty(FieldItem.ToolTip)) ControlHtml.AppendFormat(" title=\"{0}\"", FieldItem.ToolTip);

            ControlHtml.AppendFormat(" class=\"{0}\"", ViewVerification(FieldItem));

            ControlHtml.AppendFormat(" style=\"width:{0}{1};\"", FieldItem.Width, ViewWidthSuffix(FieldItem));

            ControlHtml.AppendFormat(" size=\"{0}\" multiple=\"multiple\">", FieldItem.Rows);
 
            if (!String.IsNullOrEmpty(FieldItem.FiledList))
            {
                List<String> list = WebHelper.GetList(FieldItem.FiledList.Replace("\r\n", "{|}").Replace("\r", "{|}"), "{|}");
                List<String> DefaultListValue = WebHelper.GetList(FieldItem.DefaultValue);
                for (Int32 i = 0; i < list.Count; i++)
                {
                    String CheckedStr = DefaultListValue.Count > 0 && DefaultListValue.Contains(list[i]) ? " selected=\"selected\"" : "";
                    ControlHtml.AppendFormat("<option {0} value=\"{1}\">{1}</option>", CheckedStr, list[i]);
                }
            }
            ControlHtml.Append(" </select>");
            return ControlHtml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateRadioButtonList(DNNGo_PowerForms_Field FieldItem, String ControlName, String ControlID)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML
            ControlHtml.AppendFormat("<span id=\"{0}\" ",  ControlID);

            if (!String.IsNullOrEmpty(FieldItem.ToolTip)) ControlHtml.AppendFormat(" title=\"{0}\"", FieldItem.ToolTip);
            ControlHtml.Append(" >");
            //ControlHtml.AppendFormat(" class=\"{0}\" >", ViewVerification(FieldItem));

            if (!String.IsNullOrEmpty(FieldItem.FiledList))
            {
                List<String> list = WebHelper.GetList(FieldItem.FiledList.Replace("\r\n", "{|}").Replace("\r", "{|}"), "{|}");


                if (FieldItem.Direction == (Int32)EnumControlDirection.Horizontal && FieldItem.ListColumn > 1)
                {
                    QueryParam qp = new QueryParam();
                    qp.PageSize = FieldItem.ListColumn;
                    qp.RecordCount = list.Count;
                    HtmlTable table = new HtmlTable();
                    for (int i = 1; i <= qp.Pages; i++)
                    {
                        List<String> rowLis = Common.Split<String>(list, i, qp.PageSize);
                        HtmlTableRow row = new HtmlTableRow();

                        foreach (String rowS in rowLis)
                        {
                            HtmlTableCell cell = new HtmlTableCell();
                            cell.InnerText = ViewCreateRadioButtonListOption(FieldItem, ControlName, ControlID, rowS, list.IndexOf(rowS));

                            row.Cells.Add(cell);
                        }
                        table.Rows.Add(row);
                    }

                    StringBuilder sb = new StringBuilder();
                    table.RenderControl(new HtmlTextWriter(new System.IO.StringWriter(sb)));
                    ControlHtml.Append(sb.ToString());

                }
                else
                {
                    for (Int32 i = 0; i < list.Count; i++)
                    {
                        ControlHtml.Append(ViewCreateRadioButtonListOption(FieldItem, ControlName, ControlID, list[i], i));

                        if (FieldItem.Direction == (Int32)EnumControlDirection.Vertical) ControlHtml.Append("<br /> ");
                    }
                }
            }

            ControlHtml.Append(" </span>");
            return ControlHtml.ToString();
        }

        public String ViewCreateRadioButtonListOption(DNNGo_PowerForms_Field FieldItem, String ControlName, String ControlID, String list_i, int i)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML
            if (!String.IsNullOrEmpty(list_i))
            {
                String TextBoxString = String.Empty;
                if (list_i.IndexOf("[TextBox]", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    list_i = Common.ReplaceNoCase(list_i, "[TextBox]", "");
                    TextBoxString = String.Format("<input type=\"text\" class=\"other_textbox\" data-forid=\"{1}_{2}\" name=\"{0}ot{2}\" id=\"{1}ot{2}\" />", ControlName, ControlID, i);
                }


                String CheckedStr = FieldItem.DefaultValue.IndexOf(list_i, StringComparison.CurrentCultureIgnoreCase) >= 0 ? " checked=\"checked\"" : "";
                ControlHtml.AppendFormat("<input id=\"{1}_{2}\" type=\"radio\" name=\"{0}\" value=\"{3}\"  data-value=\"{3}\" {4} class=\"{5}\"  /> <label for=\"{1}_{2}\" style=\"display:inline;\">{3} {6}</label> ", ControlName, ControlID, i, list_i, CheckedStr, ViewVerification(FieldItem), TextBoxString);
              
            
            }
            return ControlHtml.ToString();
        }







        /// <summary>
        /// 
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateRichTextBox(DNNGo_PowerForms_Field FieldItem, String ControlName, String ControlID)
        {
            if (FieldItem.Rows < 3) FieldItem.Rows = 3;

            StringBuilder ControlHtml = new StringBuilder();//控件的HTML


            ControlHtml.AppendFormat("<div style=\"width:{0}{1};\">", FieldItem.Width,EnumHelper.GetEnumTextVal( FieldItem.WidthSuffix,typeof(EnumWidthSuffix))  ).AppendLine();

            ControlHtml.Append(ViewCreateTextBox(FieldItem, ControlName, ControlID)).AppendLine();

            ControlHtml.AppendFormat("</div>").AppendLine();

           
 

            return ControlHtml.ToString();
        }

        /// <summary>
        /// 验证字符
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <returns></returns>
        public String ViewVerification(DNNGo_PowerForms_Field FieldItem)
        {
            String VerificationStr = String.Empty;

            String custom = String.Empty;
            //检查是否需要字符验证
            if (FieldItem.InputLength > 0)
            {
                custom = String.Format("maxSize[{0}]", FieldItem.InputLength);
            }

            //检查是否要相同
            if (FieldItem.EqualsControl > 0 )
            {
               DNNGo_PowerForms_Field EqualsField = DNNGo_PowerForms_Field.FindByKeyForEdit(FieldItem.EqualsControl);
               if (EqualsField != null && EqualsField.ID > 0)
               {
                   custom = String.Format("{0}{1}equals[{2}]", custom, !String.IsNullOrEmpty(custom) ? "," : "", ViewControlID(EqualsField));
               }
            }


            //验证类型
            if(FieldItem.Verification != (Int32)EnumVerification.optional)
            {
                custom = String.Format("custom[{0}]{1}{2}", EnumHelper.GetEnumTextVal(FieldItem.Verification, typeof(EnumVerification)), String.IsNullOrEmpty(custom) ? "" : ",", custom);
            }

            


            if(!String.IsNullOrEmpty(custom))
            {
                if(FieldItem.Required == 0)
                    VerificationStr = String.Format("validate[{0}]", custom);
                else
                    VerificationStr = String.Format("validate[required,{0}]", custom);
            }
            else if (FieldItem.Required != 0)
            {
                VerificationStr = "validate[required]";
            }

            return VerificationStr;
 
        }



        /// <summary>
        /// 输出控件默认值的脚本
        /// </summary>
        /// <param name="FieldList"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public String ViewInputDefault(List<DNNGo_PowerForms_Field> FieldList, String FieldName)
        {
            String DefaultValue = String.Empty;
            if (FieldList != null && FieldList.Count > 0)
            {
                DNNGo_PowerForms_Field FieldItem = FieldList.Find(r1 => r1.Name == FieldName);
                if (FieldItem != null && FieldItem.ID > 0)
                {
                    DefaultValue = ViewInputDefault(FieldItem);
                }
            }
            return DefaultValue;
        }






        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String ViewInputDefault(DNNGo_PowerForms_Field FieldItem)
        {
            //String ControlName = ViewControlName(FieldItem);
            String ControlID = ViewControlID(FieldItem);


            StringBuilder ControlHtml = new StringBuilder();
            ControlHtml.Append("<script type=\"text/javascript\">").AppendLine();
            ControlHtml.Append("    jQuery(function ($) {").AppendLine();

            if (FieldItem.FieldType == (Int32)EnumViewControlType.TextBox)
            {
                ControlHtml.AppendFormat("$('#{0}').inputDefault();", ControlID).AppendLine();
            }
            else if (FieldItem.FieldType == (Int32)EnumViewControlType.DatePicker)
            {
                ControlHtml.AppendFormat("$('#{0}').inputDefault({{ModuleId:{1}}});", ControlID, bpm.ModuleId).AppendLine();
            }
            else if (FieldItem.FieldType == (Int32)EnumViewControlType.TextBox_DisplayName)
            {
                ControlHtml.AppendFormat("$('#{0}').inputDefault();", ControlID).AppendLine();
            }
            else if (FieldItem.FieldType == (Int32)EnumViewControlType.TextBox_Email)
            {
                ControlHtml.AppendFormat("$('#{0}').inputDefault({{ModuleId:{1}}});", ControlID, bpm.ModuleId).AppendLine();
                //ControlHtml.AppendFormat("$('#{0}').inputDefault();", ControlID).AppendLine();
            }
            ControlHtml.Append("    });").AppendLine();
            ControlHtml.Append("</script>").AppendLine();

           return ControlHtml.ToString();
        }


        /// <summary>
        /// 是否HTML控件
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <returns></returns>
        public Boolean IsHtmlControl(DNNGo_PowerForms_Field FieldItem)
        {
            return FieldItem.FieldType == (Int32)EnumViewControlType.Html;
        }



        #endregion

        /// <summary>
        /// 显示标题控件
        /// </summary>
        /// <param name="FieldList">字段列表</param>
        /// <param name="FieldName">显示字段</param>
        /// <param name="Suffix">后缀名</param>
        /// <returns></returns>
        public String ViewLable(List<DNNGo_PowerForms_Field> FieldList,String FieldName, String Suffix)
        {
            String LableValue = String.Empty;
            if (FieldList != null && FieldList.Count > 0)
            {
                DNNGo_PowerForms_Field FieldItem = FieldList.Find(r1 => r1.Name == FieldName);
                if (FieldItem != null && FieldItem.ID > 0)
                {
                    LableValue = ViewLable(FieldItem, Suffix);
                }
            }
           return LableValue;
        }

        /// <summary>
        /// 显示标题控件
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <returns></returns>
        public String ViewLable(List<DNNGo_PowerForms_Field> FieldList, String FieldName)
        {
            return ViewLable(FieldList, FieldName,"");
        }


        /// <summary>
        /// 显示标题控件
        /// </summary>
        /// <param name="FieldItem">显示字段</param>
        /// <param name="Suffix">后缀名</param>
        /// <returns></returns>
        public String ViewLable(DNNGo_PowerForms_Field FieldItem,String Suffix)
        {
            String ControlName = ViewControlID(FieldItem);
            return String.Format("<label for=\"{0}\">{1}{2}</label>", ControlName, FieldItem.Alias, Suffix);
        }
        /// <summary>
        /// 显示标题控件
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <returns></returns>
        public String ViewLable(DNNGo_PowerForms_Field FieldItem)
        {
            return ViewLable(FieldItem, "");
        }

        /// <summary>
        /// 显示控件名
        /// </summary>
        /// <param name="FieldItem">字段</param>
        /// <returns></returns>
        public String ViewControlName(DNNGo_PowerForms_Field FieldItem)
        {
            return String.Format("Ctl${0}${1}", FieldItem.Name, FieldItem.ModuleId);
        }

        public String ViewControlID(DNNGo_PowerForms_Field FieldItem)
        {
            return String.Format("Ctl_{0}_{1}", FieldItem.Name, FieldItem.ModuleId);
        }




        /// <summary>
        /// 生成提交按钮
        /// </summary>
        /// <returns></returns>
        public String ViewButton()
        {
            return ViewButton("CommandButton");
        }

        /// <summary>
        /// 生成提交按钮
        /// </summary>
        /// <param name="ClassName"></param>
        /// <returns></returns>
        public String ViewButton(String ClassName)
        {
            //return ViewButton(ViewResourceText("Submit", "Submit"), ClassName);
            return ViewButton(Convert.ToString(ViewXmlSetting("SubmitButtonText", ViewResourceText("Submit", "Submit"))), ClassName);
        }


        /// <summary>
        /// 生成提交按钮
        /// </summary>
        /// <param name="ButtonText">按钮名称</param>
        /// <param name="ClassName">样式名称</param>
        /// <returns></returns>
        public String ViewButton(String ButtonText, String ClassName)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("<input type=\"submit\" name=\"{0}\" id=\"{1}\" data-verify=\"Submit\"  onclick=\"{2}\" title=\"{3}\" ", CtlButton.UniqueID, CtlButton.ClientID, CtlButton.Attributes["onclick"], ButtonText);
            String SubmitButtonImage = Convert.ToString(PictureUrl("SubmitButtonImage", ""));
            if (!String.IsNullOrEmpty(SubmitButtonImage))
            {
                sb.AppendFormat(" value=\"\" style=\"width:{0}px; height:{1}px;background:url('{2}') left top no-repeat;\" class=\"SubmitButton{4} submit_image {3}\" />", ViewXmlSetting("buttonwidth", 75), ViewXmlSetting("buttonheight", 30), SubmitButtonImage, ClassName, bpm.ModuleId);
            }
            else
            {
                sb.AppendFormat(" value=\"{0}\"  class=\"SubmitButton{2} btn {1}\" />", ButtonText, ClassName, bpm.ModuleId);
            }
            
            
            return sb.ToString();
        }

        /// <summary>
        /// 生成重置按钮
        /// </summary>
        /// <returns></returns>
        public String ViewResetButton()
        {
            return ViewResetButton("CommandButton");
        }

        /// <summary>
        /// 生成重置按钮
        /// </summary>
        /// <param name="ClassName">样式名称</param>
        /// <returns></returns>
        public String ViewResetButton(String ClassName)
        {
            //return ViewResetButton(ViewResourceText("Reset", "Reset"), ClassName);
            return ViewResetButton(Convert.ToString(ViewXmlSetting("ResetButtonText", ViewResourceText("Reset", "Reset"))), ClassName);
        }


        /// <summary>
        /// 生成重置按钮
        /// </summary>
        /// <param name="ButtonText">按钮名称</param>
        /// <param name="ClassName">样式名称</param>
        /// <returns></returns>
        public String ViewResetButton(String ButtonText, String ClassName)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("<input type=\"reset\" data-verify=\"reset\" title=\"{0}\"", ButtonText);

            String ResetButtonImage = Convert.ToString(PictureUrl("ResetButtonImage", ""));
            if (!String.IsNullOrEmpty(ResetButtonImage))
            {
                sb.AppendFormat(" value=\"\" style=\"width:{0}px; height:{1}px;background:url('{2}') left top no-repeat;\"  class=\"submit_image {3}\" />", ViewXmlSetting("buttonwidth", 78), ViewXmlSetting("buttonheight", 32), ResetButtonImage, ClassName);
            }
            else
            {
                sb.AppendFormat(" value=\"{0}\"   class=\"btn {1}\" />", ButtonText, ClassName);
            }
            return sb.ToString();
        }


        /// <summary>
        /// 生成重置按钮
        /// </summary>
        /// <returns></returns>
        public String ViewReturnButton()
        {
            return ViewReturnButton("btn");
        }

        /// <summary>
        /// 生成重置按钮
        /// </summary>
        /// <param name="ClassName">样式名称</param>
        /// <returns></returns>
        public String ViewReturnButton(String ClassName)
        {
            return ViewReturnButton(Convert.ToString(ViewResultSetting("ReturnButtonText", ViewResourceText("Return", "Return"))), ClassName);
        }


        /// <summary>
        /// 生成重置按钮
        /// </summary>
        /// <param name="ButtonText">按钮名称</param>
        /// <param name="ClassName">样式名称</param>
        /// <returns></returns>
        public String ViewReturnButton(String ButtonText, String ClassName)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            //这里需要设置结果页面返回按钮的URL


            String ReturnUrl = GoUrl();
            String ReturnUrlScript = String.Empty;
            //String ReturnUrlStript = String.Empty;
            //返回的地址
            if (bpm.iFrame.IndexOf("iFrame", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                ReturnUrl = ViewIFrame();
            }

            //String ReturnRedirect = ViewResultSettingT<String>("ReturnRedirect", "Default");
            //if (ReturnRedirect == "Custom")
            //{
            //    String settingReturnUrl = Convert.ToString(ViewLinkUrl(ViewResultSettingT<String>("ReturnUrl", ""), ""));
            //    if (!String.IsNullOrEmpty(settingReturnUrl))
            //    {
            //        if (bpm.iFrame.IndexOf("iFrame", StringComparison.CurrentCultureIgnoreCase) >= 0)
            //        {
            //            ReturnUrl =  settingReturnUrl;
            //            ReturnUrlScript = String.Format(" onclick=\"window.parent.location = '{0}'\"", settingReturnUrl);
            //        }
            //        else
            //        {
            //            ReturnUrl = settingReturnUrl;
            //        }


            //    }
            //}
            String ReturnRedirect = ViewSettingT<String>("PowerForms_ReturnUrl", "");
            if (!String.IsNullOrEmpty(ReturnRedirect))
            {
                if (ReturnRedirect.IndexOf("TabID=", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    ReturnUrl = Globals.NavigateURL(Convert.ToInt32(ReturnRedirect.Replace("TabID=", "")));
                }
                else if (ReturnRedirect.IndexOf("sFileID=", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    int FileID = 0;
                    if (int.TryParse(ReturnRedirect.Replace("sFileID=", ""), out FileID) && FileID > 0)
                    {
                        var fi = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(FileID);
                        if (fi != null && fi.FileId > 0)
                        {
                            ReturnUrl = string.Format("{0}{1}{2}", bpm.PortalSettings.HomeDirectory, fi.Folder, bpm.Server.UrlPathEncode(fi.FileName));
                        }
                    }
                }
                else
                {
                    ReturnUrl = ReturnRedirect;
                }
            }




            String ReturnButtonImage = Convert.ToString(ViewLinkUrl(ViewResultSettingT<String>("ReturnButtonImage", ""), ""));
            if (!String.IsNullOrEmpty(ReturnButtonImage))
            {
                sb.AppendFormat("<a title=\"{0}\" href=\"{1}\" {2}>", ButtonText, ReturnUrl, ReturnUrlScript);
                sb.AppendFormat("<img src=\"{2}\" style=\"width:{0}px; height:{1}px;\" />", ViewResultSetting("buttonwidth", 78), ViewResultSetting("buttonheight", 32), ReturnButtonImage);
                
            }
            else
            {
                sb.AppendFormat("<a title=\"{0}\" href=\"{1}\" class=\"{2}\" {3}>", ButtonText, ReturnUrl, ClassName,ReturnUrlScript);
                sb.AppendFormat("{0}", ButtonText);
                 
            }
            sb.Append("</a>");
            
            return sb.ToString();
        }

        /// <summary>
        /// 显示浮动框架地址
        /// </summary>
        /// <returns></returns>
        public String ViewIFrame()
        {
            String language = WebHelper.GetStringParam(bpm.Request, "language", bpm.PortalSettings.DefaultLanguage);
            String View_iFrame_Url = String.Format("{0}View_iFrame.aspx?PortalId={1}&TabId={2}&ModuleId={3}&language={4}&iFrame=iFrame", bpm.ModulePath, bpm.PortalId, bpm.TabId, bpm.ModuleId, language);


            return View_iFrame_Url;

            //return GoFullUrl(View_iFrame_Url);
        }



        #endregion



        #region "--关于验证码--"
        /// <summary>
        /// 显示验证码
        /// </summary>
        /// <returns></returns>
        public String ViewCaptcha()
        {
            StringBuilder sb = new StringBuilder();

            //sb.AppendFormat("<div class=\"g-recaptcha-dnn\" data-sitekey=\"{0}\"", bpm.ViewSettingT<String>("PowerForms_Recaptcha_SiteKey", "6LevB4AUAAAAAP4HX6l8NuvjndB_sBgdKdCbfJM7"));

            //sb.AppendFormat(" id=\"g-recaptcha-{0}\"", bpm.ModuleId);
            //sb.AppendFormat(" data-theme=\"{0}\"", bpm.ViewSettingT<String>("PowerForms_Recaptcha_Theme", "light"));//dark light
            //sb.AppendFormat(" data-type=\"{0}\"", bpm.ViewSettingT<String>("PowerForms_Recaptcha_Type", "image"));//audio image
            //sb.AppendFormat(" data-size=\"{0}\"", bpm.ViewSettingT<String>("PowerForms_Recaptcha_Size", "normal"));//compact normal
            //sb.AppendFormat(" data-tabindex=\"{0}\"", bpm.ViewSettingT<String>("PowerForms_Recaptcha_Tabindex", "0"));
            //sb.AppendFormat(" data-callback=\"GRecaptchaVerifyCallback{0}\"", bpm.ModuleId);
            //sb.AppendFormat(" data-expired-callback=\"GRecaptchaExpiredCallback{0}\"", bpm.ModuleId);
            //sb.Append("></div>").AppendLine(); 
            sb.AppendFormat("<div class=\"g-recaptcha-msg msg-hide\" id=\"g-recaptcha-msg-{0}\"  style=\"display: none;\">{1}</div>", bpm.ModuleId, ViewResourceText("GRecaptchaMsg", "Please verify you are not a robot!"));



            //sb.Append("<script type=\"text/javascript\">").AppendLine();
            //sb.Append("    jQuery(function ($) {").AppendLine();
            //sb.AppendFormat("grecaptcha.render('g-recaptcha-{0}', {{", bpm.ModuleId).AppendLine();
            //sb.AppendFormat(" 'sitekey' : $('#g-recaptcha-{0}').data('sitekey'),", bpm.ModuleId).AppendLine();
            ////sb.AppendFormat("'callback' : verifyCallback{0},", bpm.ModuleId).AppendLine();
            //sb.AppendFormat("'theme' : '{0}'", "dark").AppendLine();
            //sb.Append("});").AppendLine();
            //sb.Append("});").AppendLine();
            //sb.Append("</script>").AppendLine();









            //sb.AppendFormat("<input type=\"text\" id=\"InputCaptcha{0}\" name=\"InputCaptcha{0}\" class=\"validate[required] required\" style=\"width:120px;\" placeholder=\"{1}\" value=\"\" />", bpm.ModuleId, ViewLanguage("Captcha", "Captcha")).AppendLine();
            //sb.AppendFormat("<img  lang=\"{0}ajaxCaptcha.aspx\" src=\"{0}ajaxCaptcha.aspx?ModuleID={1}\"  align=\"absmiddle\" id=\"ImageCheck{1}\" style=\"cursor: pointer\"  onclick=\"this.src=this.lang+'?ModuleID={1}&r='+ (((1+Math.random())*0x10000000)|0).toString(16)\" title=\"{2}\" alt=\"{2}\" />", bpm.ModulePath, bpm.ModuleId, Localization.GetString("lblRefreshCaptcha.Text", bpm.LocalResourceFile)).AppendLine();
            //sb.AppendFormat("<script type=\"text/javascript\">jQuery(window).load(function () {{ jQuery(\"#ImageCheck{0}\").attr(\"src\",jQuery(\"#ImageCheck{0}\").attr(\"lang\")+'?ModuleID={0}&r='+ (((1+Math.random())*0x10000000)|0).toString(16));   }});</script>", bpm.ModuleId).AppendLine();//$(this).validationEngine(\"validate\");



            return sb.ToString();



            //System.IO.StringWriter w = new System.IO.StringWriter();
            //HtmlTextWriter a = new HtmlTextWriter(w);
            //return GenerateCaptcha(a);
        }


  


        #endregion

        





        #region "--关于用户--"

        /// <summary>
        /// 显示用户信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public String ViewUser(Int32 UserID, String FieldName)
        {
            return ViewUser(UserID, FieldName, String.Empty);
        }

        /// <summary>
        /// 显示用户信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public String ViewUser(Int32 UserID, String FieldName, String DefaultValue)
        {
            UserInfo uInfo = new UserController().GetUser(bpm.PortalId, UserID);
            return ViewUser(uInfo, FieldName, DefaultValue);
        }


        /// <summary>
        /// 显示用户信息
        /// </summary>
        /// <param name="uInfo"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public String ViewUser(DotNetNuke.Entities.Users.UserInfo uInfo, String FieldName)
        {
            return ViewUser(uInfo, FieldName, String.Empty);
        }


        /// <summary>
        /// 显示用户信息
        /// </summary>
        /// <param name="uInfo"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public String ViewUser(DotNetNuke.Entities.Users.UserInfo uInfo, String FieldName, String DefaultValue)
        {
            String FieldValue = DefaultValue;
            if (uInfo != null && uInfo.UserID > 0 && !String.IsNullOrEmpty(FieldName))
            {

                switch (FieldName.ToLower())
                {
                    case "username": FieldValue = uInfo.Username; break;
                    case "email": FieldValue = uInfo.Email; break;
                    case "firstName": FieldValue = uInfo.FirstName; break;
                    case "lastname": FieldValue = uInfo.LastName; break;
                    case "displayname": FieldValue = uInfo.DisplayName; break;
                    default: FieldValue = DefaultValue; break;
                }
            }
            return FieldValue;
        }


 


 


 

 



        #endregion

 

        #region "--XML参数读取--"

 


        /// <summary>
        /// 读取XML参数
        /// </summary>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public object ViewXmlSetting(String Name, object DefaultValue)
        {
            return bpm.ViewXmlSetting(Name, DefaultValue);
        }
        /// <summary>
        /// 读取XML参数
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public T ViewXmlSettingT<T>(String Name, object DefaultValue)
        {
            return (T)Convert.ChangeType(ViewXmlSetting(Name, DefaultValue), typeof(T));
        }


        /// <summary>
        /// 读取XML参数
        /// </summary>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public object ViewResultSetting(String Name, object DefaultValue)
        {
            return bpm.ViewResultSetting(Name, DefaultValue);
        }
        /// <summary>
        /// 读取XML参数
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public T ViewResultSettingT<T>(String Name, object DefaultValue)
        {
            return (T)Convert.ChangeType(ViewResultSetting(Name, DefaultValue), typeof(T));
        }



        /// <summary>
        /// 读取数据项参数
        /// </summary>
        /// <param name="DataItem">数据项</param>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public object ViewItemSetting(DNNGo_PowerForms_Field DataItem, String Name, object DefaultValue)
        {
            return TemplateFormat.ViewItemSettingByStatic(DataItem, Name, DefaultValue);
        }

        /// <summary>
        /// 读取XML参数
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public T ViewItemSettingT<T>(DNNGo_PowerForms_Field DataItem, String Name, object DefaultValue)
        {
            return (T)Convert.ChangeType(ViewItemSetting(DataItem, Name, DefaultValue), typeof(T));
        }

        /// <summary>
        /// 读取数据项参数
        /// </summary>
        /// <param name="DataItem">数据项</param>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public static object ViewItemSettingByStatic(DNNGo_PowerForms_Field DataItem, String Name, object DefaultValue)
        {
            object o = DefaultValue;
            if (DataItem != null && DataItem.ID > 0 && !String.IsNullOrEmpty(DataItem.Options))
            {
                try
                {
                    List<KeyValueEntity> ItemSettings = ConvertTo.Deserialize<List<KeyValueEntity>>(DataItem.Options);
                    KeyValueEntity KeyValue = ItemSettings.Find(r1 => r1.Key.ToLower() == Name.ToLower());
                    if (KeyValue != null && !String.IsNullOrEmpty(KeyValue.Key))
                    {
                        o = KeyValue.Value;
                    }

                }
                catch
                {

                }
            }
            return o;
        }

        /// <summary>
        /// 读取参数
        /// </summary>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public object ViewSetting(String Name, object DefaultValue)
        {
            return bpm.ViewSetting( Name,  DefaultValue);
        }

        /// <summary>
        /// 读取参数
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public T ViewSettingT<T>(String Name, object DefaultValue)
        {
            return bpm.ViewSettingT<T>(Name, DefaultValue);
        }






        /// <summary>
        /// 读取数据项参数
        /// </summary>
        /// <param name="DataItem">数据项</param>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public object ViewCategorySetting(DNNGo_PowerForms_Group DataItem, String Name, object DefaultValue)
        {
            return TemplateFormat.ViewCategorySettingByStatic(DataItem, Name, DefaultValue);
        }

        /// <summary>
        /// 读取XML参数
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public T ViewCategorySettingT<T>(DNNGo_PowerForms_Group DataItem, String Name, object DefaultValue)
        {
            return (T)Convert.ChangeType(ViewCategorySetting(DataItem, Name, DefaultValue), typeof(T));
        }


        /// <summary>
        /// 读取数据项参数
        /// </summary>
        /// <param name="DataItem">数据项</param>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public static object ViewCategorySettingByStatic(DNNGo_PowerForms_Group   DataItem, String Name, object DefaultValue)
        {
            object o = DefaultValue;
            if (DataItem != null && DataItem.ID > 0 && !String.IsNullOrEmpty(DataItem.Options))
            {
                try
                {
                    List<KeyValueEntity> ItemSettings = ConvertTo.Deserialize<List<KeyValueEntity>>(DataItem.Options);
                    KeyValueEntity KeyValue = ItemSettings.Find(r1 => r1.Key.ToLower() == Name.ToLower());
                    if (KeyValue != null && !String.IsNullOrEmpty(KeyValue.Key))
                    {
                        o = KeyValue.Value;
                    }

                }
                catch
                {

                }
            }
            return o;
        }




        #endregion


        #region "--关于模版--"
        /// <summary>
        /// 引用脚本文件
        /// </summary>
        /// <param name="Name">JS名称</param>
        /// <param name="FileName">JS文件(已包含主题路径)</param>
        public void IncludeScript(String Name, String FileName)
        {
            String FullFileName = String.Format("{0}{1}", ThemePath, FileName);

            bpm.BindJavaScriptFile(Name, FullFileName);
        }

        /// <summary>
        /// 显示宽度后缀(像素/百分比)
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <returns></returns>
        public String ViewWidthSuffix(DNNGo_PowerForms_Field FieldItem)
        {
            return EnumHelper.GetEnumTextVal(FieldItem.WidthSuffix, typeof(EnumWidthSuffix));
        }


        #endregion


        #region "--关于模版--"

        private String _ThemePath = String.Empty;
        /// <summary>
        /// 当前模版路径
        /// </summary>
        public String ThemePath
        {
            get {
                if (String.IsNullOrEmpty(_ThemePath))
                {


                    _ThemePath = String.Format("{0}Effects/{1}/", bpm.ModulePath, bpm.Settings_EffectName);

                }
                return _ThemePath;
            }
        }


        #endregion

        #region "语言翻译"

        /// <summary>
        /// 显示多语言
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public String ViewLanguage(String Title, String DefaultValue)
        {
            //String LocalResourceFile = String.Format("{0}App_LocalResources/{1}.ascx.resx", bpm.ModulePath, bpm.Settings_SkinName);

            return ViewResourceText(Title, DefaultValue, "Text", bpm.LocalResourceFile);
        }



        /// <summary>
        /// 显示资源文件内容
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public String ViewResourceText(String Title, String DefaultValue)
        {
            return ViewResourceText(Title, DefaultValue, "Text", bpm.LocalResourceFile);
        }


        /// <summary>
        /// 显示资源文件内容
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <param name="Extension"></param>
        /// <param name="LocalResourceFile"></param>
        /// <returns></returns>
        public String ViewResourceText(String Title, String DefaultValue, String Extension, String LocalResourceFile)
        {
            String _Title = Localization.GetString(String.Format("{0}.{1}", Title, Extension), LocalResourceFile);
            if (String.IsNullOrEmpty(_Title))
            {
                _Title = DefaultValue;
            }
            return _Title;
        }

        #endregion

        #endregion



        #region "构造"


        public TemplateFormat()
        { }

        public TemplateFormat(basePortalModule _bpm)
        {
            bpm = _bpm;
        }

        #endregion

    }
}