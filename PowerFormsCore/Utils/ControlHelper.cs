using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Localization;

namespace DNNGo.Modules.PowerForms
{
    /// <summary>
    /// 控件操作类
    /// </summary>
    public class ControlHelper
    {

        private Int32 _ModuleId = 0;
        /// <summary>
        /// 模块编号
        /// </summary>
        public Int32 ModuleId
        {
            get { return _ModuleId; }
            set { _ModuleId = value; }
        }

        private String _IconPath = "Effects";
        /// <summary>
        /// 图标路径,默认是Effects
        /// </summary>
        public String IconPath
        {
            get { return _IconPath; }
            set { _IconPath = value; }
        }

        private basePortalModule _baseModule = new basePortalModule();
        /// <summary>
        /// 基础类
        /// </summary>
        public basePortalModule BaseModule
        {
            get { return _baseModule; }
            set { _baseModule = value; }
        }



        public ControlHelper(Int32 __ModuleId)
        {
            ModuleId = __ModuleId;
        }


        public ControlHelper(basePortalModule __baseModule)
        {
            ModuleId = __baseModule.ModuleId;
            BaseModule = __baseModule;
        }



        #region "--关于控件格式化--"

        /// <summary>
        /// 显示输入控件内容
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <returns></returns>
        public Control ViewControl(SettingEntity FieldItem)
        {
            String ControlName = ViewControlName(FieldItem);
            String ControlID = ViewControlID(FieldItem);
            Control CreateControl = new Control();
            String ControlHtml = String.Empty;//控件的HTML
            if (FieldItem.ControlType == EnumControlType.CheckBox.ToString())
                ControlHtml = ViewCreateCheckBox(FieldItem, ControlName, ControlID);
            else if (FieldItem.ControlType == EnumControlType.CheckBoxList.ToString())
                ControlHtml = ViewCreateCheckBoxList(FieldItem, ControlName, ControlID);
            else if (FieldItem.ControlType == EnumControlType.DatePicker.ToString())
                ControlHtml = ViewCreateDatePicker(FieldItem, ControlName, ControlID);
            else if (FieldItem.ControlType == EnumControlType.ColorPicker.ToString())
                ControlHtml = ViewCreateColorPicker(FieldItem, ControlName, ControlID);
            else if (FieldItem.ControlType == EnumControlType.IconPicker.ToString())
                ControlHtml = ViewCreateIconPicker(FieldItem, ControlName, ControlID);
            else if (FieldItem.ControlType == EnumControlType.DropDownList.ToString())
                ControlHtml = ViewCreateDropDownList(FieldItem, ControlName, ControlID);
            else if (FieldItem.ControlType == EnumControlType.DropDownList_Group.ToString())
                ControlHtml = ViewCreateDropDownListByGroup(FieldItem, ControlName, ControlID);
            else if (FieldItem.ControlType == EnumControlType.FileUpload.ToString())
                ControlHtml = ViewCreateFileUpload(FieldItem, ControlName, ControlID);
            else if (FieldItem.ControlType == EnumControlType.Urls.ToString())
                CreateControl = ViewCreateUrls(FieldItem, ControlName, ControlID);
            else if (FieldItem.ControlType == EnumControlType.Label.ToString())
                ControlHtml = ViewCreateLabel(FieldItem, ControlName, ControlID);
            else if (FieldItem.ControlType == EnumControlType.ListBox.ToString())
                ControlHtml = ViewCreateListBox(FieldItem, ControlName, ControlID);
            else if (FieldItem.ControlType == EnumControlType.RadioButtonList.ToString())
                ControlHtml = ViewCreateRadioButtonList(FieldItem, ControlName, ControlID);
            else if (FieldItem.ControlType == EnumControlType.RichTextBox.ToString())
                ControlHtml = ViewCreateRichTextBox(FieldItem, ControlName, ControlID);
            else if (FieldItem.ControlType == EnumControlType.TextBox.ToString() || FieldItem.ControlType.IndexOf("text") >= 0)
                ControlHtml = ViewCreateTextBox(FieldItem, ControlName, ControlID);

            if (!String.IsNullOrEmpty(ControlHtml))
            {
                Literal liHtml = new Literal();
                liHtml.Text = ControlHtml;
                CreateControl = liHtml;
            }

            return CreateControl;



        }


        #region "创建HTML控件方法集合"
        /// <summary>
        /// 创建TextBox
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <returns></returns>
        public String ViewCreateTextBox(SettingEntity FieldItem, String ControlName, String ControlID)
        {

            StringBuilder ControlHtml = new StringBuilder();//控件的HTML

            //看行数决定控件的是什么控件
            if (FieldItem.Rows > 1)
            {
                ControlHtml.AppendFormat("<textarea  name=\"{0}\" id=\"{1}\"", ControlName, ControlID);

                //if (!String.IsNullOrEmpty(FieldItem.ToolTip)) ControlHtml.AppendFormat(" title=\"{0}\"", FieldItem.ToolTip);

                ControlHtml.AppendFormat(" class=\"form-control input_text {0}\"", ViewVerification(FieldItem));

                ControlHtml.AppendFormat(" style=\"width:{0}px;height:{1}px;\" rows=\"{2}\"", FieldItem.Width, FieldItem.Rows * 20, FieldItem.Rows);

                ControlHtml.Append(" >");

                //默认值
                if (!String.IsNullOrEmpty(FieldItem.DefaultValue)) ControlHtml.Append(FieldItem.DefaultValue);

                ControlHtml.Append("</textarea>");
            }
            else
            {
                ControlHtml.AppendFormat("<input type=\"text\" name=\"{0}\" id=\"{1}\"", ControlName, ControlID);

                //if (!String.IsNullOrEmpty(FieldItem.ToolTip)) ControlHtml.AppendFormat(" title=\"{0}\"", FieldItem.ToolTip);

                ControlHtml.AppendFormat(" class=\"form-control  input_text {0}\"", ViewVerification(FieldItem));

                ControlHtml.AppendFormat(" style=\"width:{0}px;\"", FieldItem.Width);

                if (!String.IsNullOrEmpty(FieldItem.DefaultValue)) ControlHtml.AppendFormat(" value=\"{0}\"", FieldItem.DefaultValue);

                ControlHtml.Append(" />");
            }

            return ControlHtml.ToString();
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateCheckBox(SettingEntity FieldItem, String ControlName, String ControlID)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML

            ControlHtml.AppendFormat("<label class=\"checkbox-inline\" for=\"{0}\">", ControlID);

            ControlHtml.AppendFormat("<input type=\"checkbox\" name=\"{0}\" id=\"{1}\"", ControlName, ControlID);

            ControlHtml.AppendFormat(" class=\"square-green grey {0}\"", ViewVerification(FieldItem));

            if (!String.IsNullOrEmpty(FieldItem.DefaultValue))
            {
                Boolean DefaultValue, b; DefaultValue = b = false;
                if (FieldItem.DefaultValue == "1" || FieldItem.DefaultValue.Equals(Boolean.TrueString, StringComparison.CurrentCultureIgnoreCase))
                    DefaultValue = true;
                else if (FieldItem.DefaultValue == "0" || FieldItem.DefaultValue.Equals(Boolean.FalseString, StringComparison.CurrentCultureIgnoreCase))
                    DefaultValue = false;
                else if (Boolean.TryParse(FieldItem.DefaultValue.ToLower(), out b))
                    DefaultValue = b;

                if (DefaultValue) ControlHtml.Append("checked=\"checked\"");
            }


            ControlHtml.Append(" />");

            //提示的关键字用作是后面的描述
            //if (!String.IsNullOrEmpty(FieldItem.Description))
            //{
            //    ControlHtml.Append(FieldItem.Description);
            //}

            ControlHtml.Append("</label>");

            //提示的关键字用作是后面的描述
            //if (!String.IsNullOrEmpty(FieldItem.ToolTip))
            //{
            //    ControlHtml.AppendFormat("<label for=\"{0}\" title=\"{1}\" style=\"display:inline;\">{1}</label>", ControlID, FieldItem.ToolTip);
            //}
            //if (!String.IsNullOrEmpty(FieldItem.Description))
            //{
            //    ControlHtml.AppendFormat("<label for=\"{0}\" title=\"{1}\" style=\"display:inline;\">{1}</label>", ControlID, FieldItem.Description);
            //}

            //ControlHtml.Append("</div>");

            return ControlHtml.ToString();
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateCheckBoxList(SettingEntity FieldItem, String ControlName, String ControlID)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML
            ControlHtml.AppendFormat("<span id=\"{1}\" name=\"{0}\" class=\"auto\">", ControlName, ControlID);



            if (!String.IsNullOrEmpty(FieldItem.ListContent))
            {
                List<String> list = Common.GetList(FieldItem.ListContent.Replace(",", "|").Replace("\r\n", "|").Replace("\r", "|"), "|");
                List<String> DefaultListValue = Common.GetList(FieldItem.DefaultValue);
                for (Int32 i = 0; i < list.Count; i++)
                {
                    if (!String.IsNullOrEmpty(list[i]))
                    {
                        String OptionText = list[i];
                        String OptionValue = list[i];
                        //判断是否包含有键值对,将键值对分离开
                        if (list[i].IndexOf(":") >= 0)
                        {
                            List<String> ItemKeyValue = Common.GetList(list[i], ":");
                            OptionText = ItemKeyValue[0];
                            OptionValue = ItemKeyValue[1];

                            //如果有图标就检测,没有就忽略
                            OptionText = FormatIcon(OptionText);
                        }


                        ControlHtml.AppendFormat("<label  for=\"{0}_{1}\"  class=\"checkbox-inline\" >", ControlID, i);

                        String CheckedStr = DefaultListValue.Count > 0 && DefaultListValue.Contains(OptionValue) ? "checked=\"checked\"" : "";

                        ControlHtml.AppendFormat("<input id=\"{1}_{2}\" type=\"checkbox\" name=\"{0}\"", ControlName, ControlID, i);

                        if (FieldItem.Required) ControlHtml.AppendFormat(" class=\"square-green grey {0}\" ", "validate[minCheckbox[1]]");

                        ControlHtml.AppendFormat("value=\"{0}\" {1} />", OptionValue, CheckedStr);

                        ControlHtml.AppendFormat(" {0}</label>", OptionText);

                        if (FieldItem.Direction == EnumControlDirection.Vertical.ToString()) ControlHtml.Append("<br />");
                    }
                }
            }

            ControlHtml.Append(" </span>");
            return ControlHtml.ToString();
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateDatePicker(SettingEntity FieldItem, String ControlName, String ControlID)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML
            ControlHtml.Append(ViewCreateTextBox(FieldItem, ControlName, ControlID));

            ControlHtml.Append("<script type=\"text/javascript\">");
            ControlHtml.Append("jQuery(document).ready(function(){").AppendLine();
            ControlHtml.Append("	    jQuery(function() {").AppendLine();
            ControlHtml.AppendFormat("		    jQuery(\"#{0}\").datepicker({{showButtonPanel: true,changeMonth: true,changeYear: true}});", ControlID).AppendLine();
            ControlHtml.Append("	    });").AppendLine();
            ControlHtml.Append("});").AppendLine();
            ControlHtml.Append("</script>");
            return ControlHtml.ToString();
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateColorPicker(SettingEntity FieldItem, String ControlName, String ControlID)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML

            if (String.IsNullOrEmpty(FieldItem.DefaultValue))
            {
                FieldItem.DefaultValue = "#ffffff";
            }


            ControlHtml.AppendFormat("<div class=\"input-group\" style=\"width:{0}px;\">", FieldItem.Width);
            ControlHtml.AppendFormat("<label class=\"input-group-addon color-picker-box tooltips\" for=\"{0}\" style=\"background-color: {1};\"> <i class=\"clip-eyedropper-2 \" ></i> </label>", ControlID, FieldItem.DefaultValue);


            ControlHtml.AppendFormat("<input type=\"text\" name=\"{0}\" id=\"{1}\"", ControlName, ControlID);

            //if (!String.IsNullOrEmpty(FieldItem.ToolTip)) ControlHtml.AppendFormat(" title=\"{0}\"", FieldItem.ToolTip);

            ControlHtml.AppendFormat(" class=\"form-control color-picker-auto input_text {0}\"", ViewVerification(FieldItem));

            ControlHtml.AppendFormat(" style=\"width:{0}px;\"", FieldItem.Width - 40);

            if (!String.IsNullOrEmpty(FieldItem.DefaultValue)) ControlHtml.AppendFormat(" value=\"{0}\"", FieldItem.DefaultValue);

            if (!String.IsNullOrEmpty(FieldItem.ListContent)) ControlHtml.AppendFormat(" data-color-format=\"{0}\"", FieldItem.ListContent);


            ControlHtml.Append(" />");


            ControlHtml.Append("</div>");





            //ControlHtml.Append("<div class=\"input-group colorpicker-component\" data-color=\"rgb(81, 145, 185)\" data-color-format=\"rgb\">");
            //ControlHtml.Append(ViewCreateTextBox(FieldItem, ControlName, ControlID));
            //ControlHtml.Append("<span class=\"input-group-addon\"><i style=\"background-color: rgb(81, 145, 185)\"></i></span>");
            //ControlHtml.Append("</div>");
            return ControlHtml.ToString();
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateIconPicker(SettingEntity FieldItem, String ControlName, String ControlID)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML


            ControlHtml.AppendFormat("<select id=\"{0}\" name=\"{1}\" class=\"form-control search-select search-select-auto {2}\" ", ControlID, ControlName, ViewVerification(FieldItem));


            ControlHtml.AppendFormat(" style=\"width:{0}px;\" >", FieldItem.Width);


            List<OptionItem> Optins = new List<OptionItem>();

            if (String.IsNullOrEmpty(FieldItem.ListContent))//没有列表项的时候,需要加载默认的字体
            {
                String xml_url = BaseModule.Server.MapPath(String.Format("{0}Resource/xml/SelectIcons.xml", BaseModule.ModulePath));
                XmlFormat xf = new XmlFormat(xml_url);

                Optins = xf.ToList<OptionItem>();
            }
            else if (!String.IsNullOrEmpty(FieldItem.ListContent))
            {
                List<String> list = Common.GetList(FieldItem.ListContent.Replace(",", "|").Replace("\r\n", "|").Replace("\r", "|"), "|");

                for (Int32 i = 0; i < list.Count; i++)
                {
                    if (!String.IsNullOrEmpty(list[i]))
                    {
                        OptionItem Optin = new OptionItem();
                        Optin.Text = list[i];
                        Optin.Value = list[i];
                        //判断是否包含有键值对,将键值对分离开
                        if (list[i].IndexOf(":") >= 0)
                        {
                            List<String> ItemKeyValue = Common.GetList(list[i], ":");
                            Optin.Text = ItemKeyValue[0];
                            Optin.Value = ItemKeyValue[1];
                        }
                        Optins.Add(Optin);
                    }
                }
            }


            foreach (var Optin in Optins)
            {
                String DisabledStr = String.IsNullOrEmpty(Optin.Value) ? "disabled=\"disabled\"" : "";
                String CheckedStr = !String.IsNullOrEmpty(Optin.Value) && FieldItem.DefaultValue.IndexOf(Optin.Value, StringComparison.CurrentCultureIgnoreCase) >= 0 ? "selected=\"selected\"" : "";
                ControlHtml.AppendFormat("<option value=\"{0}\" {2} {3}> {1}</option>", Optin.Value, Optin.Text, CheckedStr, DisabledStr).AppendLine();
            }

            ControlHtml.Append("</select>");

            return ControlHtml.ToString();
        }





        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateDropDownList(SettingEntity FieldItem, String ControlName, String ControlID)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML
            ControlHtml.AppendFormat("<select name=\"{0}\" id=\"{1}\"", ControlName, ControlID);

            ControlHtml.AppendFormat(" style=\"width:{0}px;\"", FieldItem.Width);

            ControlHtml.AppendFormat(" class=\"form-control  input_text {0}\">", ViewVerification(FieldItem));

            if (!String.IsNullOrEmpty(FieldItem.ListContent))
            {
                List<String> list = Common.GetList(FieldItem.ListContent.Replace(",", "|").Replace("\r\n", "|").Replace("\r", "|"), "|");

                for (Int32 i = 0; i < list.Count; i++)
                {
                    if (!String.IsNullOrEmpty(list[i]))
                    {
                        String OptionText = list[i];
                        String OptionValue = list[i];
                        //判断是否包含有键值对,将键值对分离开
                        if (list[i].IndexOf(":") >= 0)
                        {
                            List<String> ItemKeyValue = Common.GetList(list[i], ":");
                            OptionText = ItemKeyValue[0];
                            OptionValue = ItemKeyValue[1];
                        }

                        String CheckedStr = FieldItem.DefaultValue.IndexOf(OptionValue, StringComparison.CurrentCultureIgnoreCase) >= 0 ? "selected=\"selected\"" : "";
                        ControlHtml.AppendFormat("<option {0} value=\"{1}\">{2}</option>", CheckedStr, OptionValue, OptionText);

                    }
                }
            }
            ControlHtml.Append(" </select>");
            return ControlHtml.ToString();
        }



        /// <summary>
        /// 创建下拉列表by 分组
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <param name="ControlID"></param>
        /// <returns></returns>
        public String ViewCreateDropDownListByGroup(SettingEntity FieldItem, String ControlName, String ControlID)
        {
            //构造国家数据
            StringBuilder sb = new StringBuilder(FieldItem.ListContent);

            List<DNNGo_PowerForms_Group> Groups = DNNGo_PowerForms_Group.FindAllByModuleID(ModuleId);

            if (Groups != null && Groups.Count > 0)
            {
                foreach (DNNGo_PowerForms_Group info in Groups)
                {
                    if (info != null && !String.IsNullOrEmpty(info.Name))
                    {
                        sb.AppendFormat("|{0}", info.Name);
                    }
                }
                FieldItem.ListContent = sb.ToString();
            }
            return ViewCreateDropDownList(FieldItem, ControlName, ControlID);
        }







        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateFileUpload(SettingEntity FieldItem, String ControlName, String ControlID)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML
            ControlHtml.AppendFormat("<input type=\"file\" name=\"{0}\" id=\"{1}\"", ControlName, ControlID);

            //if (!String.IsNullOrEmpty(FieldItem.ToolTip)) ControlHtml.AppendFormat(" title=\"{0}\"", FieldItem.ToolTip);

            ControlHtml.AppendFormat(" class=\"file-input validate[{0}ajax[ajaxUpload]]\"", FieldItem.Required ? "required," : "");

            ControlHtml.AppendFormat(" style=\"width:{0}px;\"", FieldItem.Width < 250 ? 250 : FieldItem.Width);

            ControlHtml.Append(" />");
            return ControlHtml.ToString();
        }

        /// <summary>
        /// 创建URLs
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <param name="ControlID"></param>
        /// <returns></returns>
        public Control ViewCreateUrls(SettingEntity FieldItem, String ControlName, String ControlID)
        {
            string ContentSrc = BaseModule.ResolveClientUrl(string.Format("{0}Resource_RichUrls.ascx", BaseModule.ModulePath));
            if (System.IO.File.Exists(BaseModule.MapPath(ContentSrc)))
            {
                Resource_RichUrls ManageContent = new Resource_RichUrls();
                ManageContent = (Resource_RichUrls)BaseModule.LoadControl(ContentSrc);
                ManageContent.ModuleConfiguration = BaseModule.ModuleConfiguration;
                ManageContent.ID = ControlID;
                ManageContent.LocalResourceFile = Localization.GetResourceFile(ManageContent.Control, string.Format("{0}.resx", "Resource_RichUrls.ascx"));
                ManageContent.FieldItem = FieldItem;
                return ManageContent;
            }
            return new Literal();
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateLabel(SettingEntity FieldItem, String ControlName, String ControlID)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML

            ControlHtml.AppendFormat("<span  name=\"{0}\" id=\"{1}\"", ControlName, ControlID);
            //if (!String.IsNullOrEmpty(FieldItem.ToolTip)) ControlHtml.AppendFormat(" title=\"{0}\"", FieldItem.ToolTip);

            if (!String.IsNullOrEmpty(FieldItem.DefaultValue) )
            {
                FieldItem.DefaultValue = Common.ReplaceNoCase( FieldItem.DefaultValue,"[ModuleId]",BaseModule.ModuleId.ToString());
            }

            ControlHtml.AppendFormat(">{0}</span>", FieldItem.DefaultValue);
            return ControlHtml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateListBox(SettingEntity FieldItem, String ControlName, String ControlID)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML
            ControlHtml.AppendFormat("<select name=\"{0}\" id=\"{1}\"", ControlName, ControlID);

            ControlHtml.AppendFormat(" class=\"form-control input_text {0}\"", ViewVerification(FieldItem));

            ControlHtml.AppendFormat(" style=\"width:{0}px;\"", FieldItem.Width);

            ControlHtml.AppendFormat(" size=\"{0}\" multiple=\"multiple\">", FieldItem.Rows);

            if (!String.IsNullOrEmpty(FieldItem.ListContent))
            {
                List<String> list = Common.GetList(FieldItem.ListContent.Replace(",", "|").Replace("\r\n", "|").Replace("\r", "|"), "|");
                List<String> DefaultListValue = Common.GetList(FieldItem.DefaultValue);
                for (Int32 i = 0; i < list.Count; i++)
                {
                    if (!String.IsNullOrEmpty(list[i]))
                    {

                        String OptionText = list[i];
                        String OptionValue = list[i];
                        //判断是否包含有键值对,将键值对分离开
                        if (list[i].IndexOf(":") >= 0)
                        {
                            List<String> ItemKeyValue = Common.GetList(list[i], ":");
                            OptionText = ItemKeyValue[0];
                            OptionValue = ItemKeyValue[1];
                        }


                        String CheckedStr = DefaultListValue.Count > 0 && DefaultListValue.Contains(OptionValue) ? "selected=\"selected\"" : "";
                        String DisabledStr = String.IsNullOrEmpty(OptionValue) ? "disabled=\"disabled\"" : "";
                        ControlHtml.AppendFormat("<option {0} {1} value=\"{2}\">{3}</option>", CheckedStr, DisabledStr, OptionValue, OptionText);
                    }
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
        public String ViewCreateRadioButtonList(SettingEntity FieldItem, String ControlName, String ControlID)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML
            ControlHtml.AppendFormat("<span id=\"{0}\"  class=\"{1} auto\" >", ControlID, ViewVerification(FieldItem));


            if (!String.IsNullOrEmpty(FieldItem.ListContent))
            {
                List<String> list = Common.GetList(FieldItem.ListContent.Replace(",", "|").Replace("\r\n", "|").Replace("\r", "|"), "|");
                for (Int32 i = 0; i < list.Count; i++)
                {
                    if (!String.IsNullOrEmpty(list[i]))
                    {
                        String OptionText = list[i];
                        String OptionValue = list[i];
                        String OptionImage = String.Empty;
                        //判断是否包含有键值对,将键值对分离开
                        if (list[i].IndexOf(":") >= 0)
                        {
                            List<String> ItemKeyValue = Common.GetList(list[i], ":");
                            OptionText = ItemKeyValue[0];
                            OptionValue = ItemKeyValue[1];

                            //如果有图标就检测,没有就忽略
                            OptionText = FormatIcon(OptionText);

                        }

                        String CheckedStr = FieldItem.DefaultValue.IndexOf(OptionValue, StringComparison.CurrentCultureIgnoreCase) >= 0 ? "checked=\"checked\"" : "";
                        ControlHtml.AppendFormat("<label  class=\"radio-inline\" for=\"{1}_{2}\" ><input id=\"{1}_{2}\"  type=\"radio\" name=\"{0}\" value=\"{3}\" {4} /> {5}</label>", ControlName, ControlID, i, OptionValue, CheckedStr, OptionText);
                        if (FieldItem.Direction == EnumControlDirection.Vertical.ToString()) ControlHtml.Append("<br />");

                    }
                }
            }

            ControlHtml.Append(" </span>");
            return ControlHtml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateRichTextBox(SettingEntity FieldItem, String ControlName, String ControlID)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML

            ControlHtml.AppendFormat("<textarea  name=\"{0}\" id=\"{1}\"", ControlName, ControlID);

            //if (!String.IsNullOrEmpty(FieldItem.ToolTip)) ControlHtml.AppendFormat(" title=\"{0}\"", FieldItem.ToolTip);

            ControlHtml.AppendFormat(" class=\"form-control tinymce input_text {0}\"", ViewVerification(FieldItem));

            ControlHtml.AppendFormat(" style=\"width:{0}px;height:{1}px;\" rows=\"{2}\"", FieldItem.Width, FieldItem.Rows * 20, FieldItem.Rows);

            ControlHtml.Append(" >");

            //默认值
            if (!String.IsNullOrEmpty(FieldItem.DefaultValue)) ControlHtml.Append(FieldItem.DefaultValue);

            ControlHtml.Append("</textarea>");


            //ControlHtml.Append(ViewCreateTextBox(FieldItem, ControlName, ControlID));


            

            //ControlHtml.Append("<script>");
            //ControlHtml.Append("var editor;").AppendLine();
            //ControlHtml.Append("KindEditor.ready(function (K) {").AppendLine();
            //ControlHtml.AppendFormat(" 	editor = K.create('#{0}', {{", ControlID).AppendLine();
            //ControlHtml.Append("		afterBlur: function(){this.sync();},allowPreviewEmoticons: false,allowImageUpload: false,").AppendLine();
            //ControlHtml.Append("	    items: [ 'source', '|','fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline','removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist','insertunorderedlist', '|', 'emoticons', 'image', 'link']").AppendLine();
            //ControlHtml.Append("        });").AppendLine();
            //ControlHtml.Append(" });").AppendLine();
            //ControlHtml.Append("</script>");

            return ControlHtml.ToString();
        }

        /// <summary>
        /// 验证字符
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <returns></returns>
        public String ViewVerification(SettingEntity FieldItem)
        {
            //无验证时退出
            //if (FieldItem.Verification == EnumVerification.optional.ToString() && !FieldItem.Required) return "";

            String custom = String.Empty;
            if (!String.IsNullOrEmpty(FieldItem.Verification) && FieldItem.Verification != EnumVerification.optional.ToString())
            {
                if (!(("DropDownList,ListBox,RadioButtonList,FileUpload,CheckBox,CheckBoxList,Label").IndexOf(FieldItem.ControlType) >= 0))
                {
                    custom = String.Format("custom[{0}]", FieldItem.Verification);
                }
            }
            if (!String.IsNullOrEmpty(custom))
            {
                if (!FieldItem.Required)
                    return String.Format("validate[{0}]", String.IsNullOrEmpty(custom) ? "optional" : custom);
                else
                    return String.Format("validate[required,{0}]", custom);
            }
            else
            {
                if (FieldItem.Required)
                    return "validate[required]";
                else
                    return "";
            }

        }


        #endregion


        #endregion


        #region 获取Form传值
        /// <summary>
        /// 获取Form传值
        /// </summary>
        /// <param name="fieldItem"></param>
        /// <returns></returns>
        public static String GetWebFormValue(SettingEntity fieldItem, basePortalModule bpm)
        {
            String WebFormValue = String.Empty;

            //创建控件的Name和ID
            ControlHelper ControlItem = new ControlHelper(bpm.ModuleId);
            String ControlName = ControlItem.ViewControlName(fieldItem);
            String ControlID = ControlItem.ViewControlID(fieldItem);

            if (fieldItem.ControlType == EnumControlType.CheckBox.ToString())
            {
                WebFormValue = WebHelper.GetStringParam(HttpContext.Current.Request, ControlName, "");
                WebFormValue = !String.IsNullOrEmpty(WebFormValue) && WebFormValue == "on" ? "true" : "false";
            }     
            else if (fieldItem.ControlType == EnumControlType.FileUpload.ToString())
            {
                HttpPostedFile hpFile = HttpContext.Current.Request.Files[ControlName];
                //To verify that if the suffix name of the uploaded files meets the DNN HOST requirements
                Boolean retValue = FileSystemUtils.CheckValidFileName(hpFile.FileName);
                if (retValue)
                {
                    if (hpFile != null && hpFile.ContentLength > 0) WebFormValue = String.Format("Url://{0}", FileSystemUtils.UploadFile(hpFile, bpm));                //存放到目录中，并返回
                }
            }
            else if (fieldItem.ControlType == EnumControlType.Urls.ToString())
            {
                String ClientName = GetRichUrlsName(fieldItem);
                if (bpm.HttpContext.Items.Contains(ClientName))
                {
                    WebFormValue = Convert.ToString(bpm.HttpContext.Items[ClientName]);
                }
            }
            else
            {
                WebFormValue = WebHelper.GetStringParam(HttpContext.Current.Request, ControlName, "");
            }
            return WebFormValue;

        }

        /// <summary>
        /// 获取URLs控件的名称
        /// </summary>
        /// <param name="fieldItem"></param>
        /// <returns></returns>
        public static String GetRichUrlsName(SettingEntity fieldItem)
        {
            return String.Format("RichUrls{0}", fieldItem.Name).Replace("_", "").Replace("-", "");
        }

        #endregion


        #region "辅助的方法"


        /// <summary>
        /// 显示标题控件
        /// </summary>
        /// <param name="FieldItem">显示字段</param>
        /// <param name="Suffix">后缀名</param>
        /// <returns></returns>
        public String ViewLable(SettingEntity FieldItem, String Suffix)
        {
            String ControlName = ViewControlID(FieldItem);
            return String.Format("<label for=\"{0}\">{1}{2}</label>", ControlName, FieldItem.Alias, Suffix);
        }
        /// <summary>
        /// 显示标题控件
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <returns></returns>
        public String ViewLable(SettingEntity FieldItem)
        {
            return ViewLable(FieldItem, ""); ;
        }

        /// <summary>
        /// 显示控件名
        /// </summary>
        /// <param name="FieldItem">字段</param>
        /// <returns></returns>
        public String ViewControlName(SettingEntity FieldItem)
        {
            return String.Format("Ctl${0}", FieldItem.Name);
            //return String.Format("Ctl${0}${1}", FieldItem.Name, ModuleId);
        }

        public String ViewControlID(SettingEntity FieldItem)
        {
            return String.Format("Ctl_{0}", FieldItem.Name);
            //return String.Format("Ctl_{0}_{1}", FieldItem.Name, ModuleId);
        }

        /// <summary>
        /// 检测并过滤图标
        /// </summary>
        /// <param name="OptionText"></param>
        /// <returns></returns>
        public String FormatIcon(String OptionText)
        {
            if (!String.IsNullOrEmpty(OptionText) && OptionText.IndexOf("[*]") >= 0)
            {
                List<String> TextImages = Common.GetList(OptionText, "[*]");
                OptionText = FormatIcon(TextImages[0], TextImages[1]);
            }
            return OptionText;
        }
        /// <summary>
        /// 过滤图标
        /// </summary>
        /// <param name="_Text"></param>
        /// <param name="_Icon"></param>
        /// <returns></returns>
        public String FormatIcon(String _Text, String _Icon)
        {
            return String.Format("<img src=\"{0}{4}/{1}/images/{2}\" class=\"img-responsive tooltips\" title=\"{3}\" data-placement=\"top\" data-rel=\"tooltip\" data-original-title=\"{3}\" />", BaseModule.ModulePath, BaseModule.Settings_EffectName, _Icon, _Text, IconPath);
        }



        #endregion
    }
}