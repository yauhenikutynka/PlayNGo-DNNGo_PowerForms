using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Controllers;

namespace DNNGo.Modules.PowerForms
{
    public partial class ajaxValidate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string validateId = WebHelper.GetStringParam(Request, "fieldId", "");
                string validateValue = WebHelper.GetStringParam(Request, "fieldValue", "");
                string validateError = WebHelper.GetStringParam(Request, "extraData", "");
                string validateType = WebHelper.GetStringParam(Request, "type", "");

                String validateJSON = String.Empty;


                if (validateType.IndexOf("FieldName", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    validateJSON = ValidationFieldName(validateId, validateValue, validateError);
                }
                if (validateType.IndexOf("AjaxCustomFieldName", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    validateJSON = ValidationCustomFieldName(validateId, validateValue, validateError);
                }
                else if (validateType.IndexOf("FileUpload", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    validateJSON = ValidationUpload(validateId, validateValue, validateError);
                }
                else if (validateType.IndexOf("XmlUpload", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    validateJSON = ValidationXmlUpload(validateId, validateValue, validateError);
                }
                else if (validateType.IndexOf("AjaxCaptcha", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    validateJSON = ValidationCaptcha(validateId, validateValue, validateError);
                }
                else if (validateType.IndexOf("AjaxCustomCaptcha", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    validateJSON = ValidationCustomCaptcha(validateId, validateValue, validateError);
                }
                else if (validateType.IndexOf("AjaxCustomUpload", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    String FileExtensions = HostController.Instance.GetString("FileExtensions");
                    validateJSON = ValidationCustomUpload(validateId, validateValue, validateError, FileExtensions);
                }
 
                Response.Write(validateJSON);
 
            }
        }


        /// <summary>
        /// 验证字段名是否重复
        /// </summary>
        /// <returns></returns>
        private String ValidationCustomFieldName(string validateId, string validateValue, string validateError)
        {
            String validateJSON = "true";
 
            Int32 ModuleID = WebHelper.GetIntParam(Request, "ModuleID", 0);
  

            QueryParam qp = new QueryParam();
            qp.Where.Add(new SearchParam(DNNGo_PowerForms_Field._.Name, validateValue, SearchType.Equal));
            qp.Where.Add(new SearchParam(DNNGo_PowerForms_Field._.ModuleId, ModuleID, SearchType.Equal));

            if (DNNGo_PowerForms_Field.FindCount(qp) > 0)
            {
                validateJSON = "false";
            }
 
            return validateJSON;
        }



        /// <summary>
        /// 验证字段名是否重复
        /// </summary>
        /// <returns></returns>
        private String ValidationFieldName(string validateId, string validateValue, string validateError)
        {
            String validateJSON = String.Empty;

            Int32 ModuleID = WebHelper.GetIntParam(Request, "ModuleID", 0);

            QueryParam qp = new QueryParam();
            qp.Where.Add(new SearchParam(DNNGo_PowerForms_Field._.Name, validateValue, SearchType.Equal));
            qp.Where.Add(new SearchParam(DNNGo_PowerForms_Field._.ModuleId, ModuleID, SearchType.Equal));

            if (DNNGo_PowerForms_Field.FindCount(qp) > 0)
                validateJSON = "{\"jsonValidateReturn\":[\"" + validateId + "\",\"" + validateError + "\",false]}";//验证不通过
            else
                validateJSON = "{\"jsonValidateReturn\":[\"" + validateId + "\",\"" + validateError + "\",true]}";//验证通过



            return validateJSON;
        }



        private String ValidationXmlUpload(string validateId, string validateValue, string validateError)
        {
            String FileExtensions = ",xml,";
            return ValidationUpload(validateId, validateValue, validateError, FileExtensions);
        }


        private String ValidationUpload(string validateId, string validateValue, string validateError)
        {
            String FileExtensions = HostController.Instance.GetString("FileExtensions");
             return ValidationUpload(validateId, validateValue, validateError, FileExtensions);
        }


        /// <summary>
        /// 验证文件的后缀名是否合法
        /// </summary>
        /// <param name="validateId"></param>
        /// <param name="validateValue"></param>
        /// <param name="validateError"></param>
        /// <returns></returns>
        private String ValidationUpload(string validateId, string validateValue, string validateError, String FileExtensions)
        {
            String validateJSON = String.Empty;

            String Extension = Path.GetExtension(validateValue).Replace(".", "");

            if (!String.IsNullOrEmpty(validateValue) && String.IsNullOrEmpty(Extension))
            {
                //有文件路径，没后缀的情况
                validateJSON = "{\"jsonValidateReturn\":[\"" + validateId + "\",\"" + validateError + "\",false]}";//验证不通过
                return validateJSON;
            }

            if (!String.IsNullOrEmpty(FileExtensions) && FileExtensions.IndexOf(Extension, StringComparison.CurrentCultureIgnoreCase) >= 0)
                validateJSON = "{\"jsonValidateReturn\":[\"" + validateId + "\",\"" + validateError + "\",true]}";//验证通过
            else
                validateJSON = "{\"jsonValidateReturn\":[\"" + validateId + "\",\"" + validateError + "\",false]}";//验证不通过


            return validateJSON;
        }

        /// <summary>
        /// 表单验证码ajax验证
        /// </summary>
        /// <param name="validateId"></param>
        /// <param name="validateValue"></param>
        /// <param name="validateError"></param>
        /// <returns></returns>
        private String ValidationCaptcha(string validateId, string validateValue, string validateError)
        {
            String validateJSON = String.Empty;

            //检测
            Int32 ModuleID = WebHelper.GetIntParam(Request, "ModuleID", 0);
            String SessionCaptcha = Convert.ToString(HttpContext.Current.Session[String.Format("SessionCaptcha_{0}", ModuleID)]);


            if (!String.IsNullOrEmpty(validateValue) && !String.IsNullOrEmpty(SessionCaptcha) && validateValue.ToLower().Trim() == SessionCaptcha)
                 validateJSON = "{\"jsonValidateReturn\":[\"" + validateId + "\",\"" + validateError + "\",true]}";//验证通过
            else
                validateJSON = "{\"jsonValidateReturn\":[\"" + validateId + "\",\"" + validateError + "\",false]}";//验证不通过
           
               



            return validateJSON;
        }


        /// <summary>
        /// 表单验证码ajax验证
        /// </summary>
        /// <param name="validateId"></param>
        /// <param name="validateValue"></param>
        /// <param name="validateError"></param>
        /// <returns></returns>
        private String ValidationCustomCaptcha(string validateId, string validateValue, string validateError)
        {
            String validateJSON = "false";

            //检测
            Int32 ModuleID = WebHelper.GetIntParam(Request, "ModuleID", 0);
            HttpCookie ckSessionCaptcha = HttpContext.Current.Request.Cookies[String.Format("SessionCaptcha_{0}", ModuleID)];

            if (ckSessionCaptcha != null && !String.IsNullOrEmpty(ckSessionCaptcha.Name))
            {
                String SessionCaptcha = Convert.ToString(ckSessionCaptcha.Value);
                if (!String.IsNullOrEmpty(validateValue) && !String.IsNullOrEmpty(SessionCaptcha) && validateValue.ToLower().Trim() == SessionCaptcha)
                {
                    validateJSON = "true";
                }
            }
            return validateJSON;
        }


        private String ValidationCustomUpload(string validateId, string validateValue, string validateError, String FileExtensions)
        {
            String validateJSON = "false";

            String Extension = Path.GetExtension(validateValue).Replace(".", "");

            if (String.IsNullOrEmpty(validateValue) || String.IsNullOrEmpty(Extension))
            {
                validateJSON = "true";
            }
 
            if (!String.IsNullOrEmpty(FileExtensions) && FileExtensions.IndexOf(Extension, StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                validateJSON = "true";
            }
        
            return validateJSON;
        }


    }
}