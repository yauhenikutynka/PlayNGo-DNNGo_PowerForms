using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules.Actions;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using DotNetNuke.Common;
using DotNetNuke.Services.Mail;
using DotNetNuke.Common.Utilities;
using System.Web.Script.Serialization;
using DotNetNuke.Entities.Users;
using DotNetNuke.Common.Lists;
using DotNetNuke.Entities.Controllers;

namespace DNNGo.Modules.PowerForms
{
    public partial class View_Form : basePortalModule
    {
        private JavaScriptSerializer jsSerializer = new JavaScriptSerializer();

        #region "扩展属性"

        private Boolean _EffectGroup=false;
        /// <summary>
        /// 分组框效果
        /// </summary>
        public Boolean EffectGroup
        {
            get { return _EffectGroup; }
            set { _EffectGroup = value; }
        }


        private Boolean _Fancybox = false;
        /// <summary>
        /// 弹出框效果
        /// </summary>
        public Boolean Fancybox
        {
            get { return _Fancybox; }
            set { _Fancybox = value; }
        }
 
        /// <summary>提示操作类</summary>
        MessageTips mTips = new MessageTips();


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
                    _FieldList = DNNGo_PowerForms_Field.FindAllByView(ModuleId, UserInfo);
                }
                return _FieldList;
            }
        }

        /// <summary>
        /// 是否包含多文件上传控件
        /// </summary>
        public Boolean IncludeMultipleFileUpload
        {
            get { return FieldList.Exists(r => r.FieldType == (Int32)EnumViewControlType.MultipleFilesUpload); }
        }
        /// <summary>
        /// Allowable File Extensions
        /// </summary>
        public String AllowableFileExtensions
        {
            get { return HostController.Instance.GetString("FileExtensions"); }
        }

      
        


        #endregion


        #region "方法"

        /// <summary>
        /// 绑定数据项到前台
        /// </summary>
        public void BindDataItem(EffectDB EffectDB, String ErrorMessage)
        {
          
            
            //防止重复提交之用
            //SubmitButton.Attributes["onclick"] = Page.GetPostBackEventReference(this.SubmitButton) ;

            Hashtable Puts = new Hashtable();
            TemplateFormat xf = new TemplateFormat(this);
            xf.CtlButton = SubmitButton;

            //读取需要载入的参数
            Puts.Add("FieldList", FieldList);

            //读取需要载入的分组
            List<DNNGo_PowerForms_Group> GroupList = new List<DNNGo_PowerForms_Group>();
            if (Setting_EffectDB.Group)
            {
                GroupList = DNNGo_PowerForms_Group.FindAllByView(ModuleId);
                if (GroupList.Count == 0)
                {
                    DNNGo_PowerForms_Group defultGroup = new DNNGo_PowerForms_Group();
                    defultGroup.ID = 0;
                    defultGroup.Name = "Default Group";
                    GroupList.Add(defultGroup);
                }
            }
            Puts.Add("GroupList", GroupList);
           
            Puts.Add("EffectName", Settings_EffectName);
            Puts.Add("ThemeName", Settings_EffectThemeName);
            Puts.Add("ErrorMessage", ErrorMessage);
            Puts.Add("captchaErrorMessage", ErrorMessage);

            //判断表单版本，用来判断是否控制每个人只能填写一次
            Puts.Add("DisplayForm", DisplayForm());
            

            if (!String.IsNullOrEmpty(iFrame) && iFrame.IndexOf("iFrame", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                liContent.Text = HttpUtility.HtmlDecode(ViewTemplate(EffectDB, "iFrame.html", Puts, xf));
            }
            else
            {

                liContent.Text = HttpUtility.HtmlDecode(ViewTemplate(EffectDB, "Effect.html", Puts, xf));
            }


        }

        /// <summary>
        /// 显示表单信息
        /// </summary>
        /// <returns></returns>
        public Boolean DisplayForm()
        {
            Boolean _DisplayForm = true;


            Boolean LoginUserDisplay = ViewSettingT<Boolean>("PowerForms_LoginUserDisplay", false);
            if (LoginUserDisplay)
            {
                if (UserId > 0)
                {
                    String FormVersion = ViewSettingT<String>("PowerForms_FormVersion", "");
                    if (!String.IsNullOrEmpty(FormVersion))
                    {
                        //根据条件判断该用户是否提交过了。
                        QueryParam qp = new QueryParam();
                        qp.Where.Add(new SearchParam(DNNGo_PowerForms_Content._.ModuleId, ModuleId, SearchType.Equal));
                        qp.Where.Add(new SearchParam(DNNGo_PowerForms_Content._.FormVersion, FormVersion, SearchType.Equal));
                        qp.Where.Add(new SearchParam(DNNGo_PowerForms_Content._.LastUser, UserId, SearchType.Equal));

                        if (DNNGo_PowerForms_Content.FindCount(qp) > 0)
                        {
                            _DisplayForm = false;//如果已经提交过表单，则不显示
                        }
                    }
                }
                else
                {
                    _DisplayForm = false;//需要登陆且没有登陆时不显示表单
                }
            }


            

            //检查是否需要登陆用户可见
           
            




            return _DisplayForm;
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
                if(IncludeMultipleFileUpload)
                {
                    //载入多文件上传脚本
                    LoadPluploadScript();
                }


                String SubmitPostAction = WebHelper.GetStringParam(Request, SubmitButton.UniqueID, "");

                //if (!String.IsNullOrEmpty(SubmitPostAction))
                //{
                //    SubmitButton_Click(null, null);
                //}


                if (!String.IsNullOrEmpty(Settings_EffectName))
                {

                    if (!String.IsNullOrEmpty(Settings_EffectThemeName))
                    {
                        EffectDB EffectDB = Setting_EffectDB;
                        EffectGroup = EffectDB.Group;//指示前台当前效果是否带分组




                        //String captchaErrorMessage = mTips.Post(this);

                        if (!IsPostBack)// || !String.IsNullOrEmpty(captchaErrorMessage))
                        {
                            //绑定数据项到前台
                            BindDataItem(EffectDB, "");
                            //BindDataItem(EffectDB, captchaErrorMessage);

                            if (EffectDB.Name.IndexOf("Effect_04_FancyBox") >= 0 || EffectDB.Name.IndexOf("Effect_05_HoverFancyBox") >= 0)
                            {
                                _Fancybox = true;
                            }
                            

                            
                        }

                        if (!IsPostBack)
                        {
                            //绑定隐藏控件域(防止提交垃圾)
                            BindHiddenField();
                        }


                        //需要载入当前设置效果的主题CSS文件
                        String ThemeName = String.Format("{0}_{1}", Settings_EffectName, Settings_EffectThemeName);
                        String ThemePath = String.Format("{0}Effects/{1}/Themes/{2}/Style.css", ModulePath, Settings_EffectName, Settings_EffectThemeName);
                        BindStyleFile(ThemeName, ThemePath);

                        BindXmlDBToPage(EffectDB, "Effect");
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

        ///<summary>
        ///注册按钮的事件
        ///PS:防止发生验证不一致
        ///</summary>
        protected override void Render(HtmlTextWriter writer)
        {
            Page.ClientScript.RegisterForEventValidation(SubmitButton.UniqueID);
            base.Render(writer);
        }

        /// <summary>
        /// 表单提交按钮
        /// </summary>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            Boolean Verify = true;
            String ErrorMessage = String.Empty;

            //验证码验证
            if (ViewSettingT<Boolean>("PowerForms_Recaptcha_v3_Enable", false))
            {
                string EncodedResponse = WebHelper.GetStringParam(Request, hfGoogleRecaptchaResponse.UniqueID, "",false);
                if (!String.IsNullOrEmpty(EncodedResponse))
                {
                    Dictionary<String, Object> captchaResponse = ReCaptchaClass.Validate(EncodedResponse, this);
                    if (captchaResponse != null && captchaResponse.Count >0)
                    {
                        Verify = (captchaResponse.ContainsKey("success") ? captchaResponse["success"].ToString() : "false").ToLower() == "true" ? true : false;
                        if (!Verify)//验证不通过时,需要打印出错误
                        {

                            Verify = false;

                            if (captchaResponse.ContainsKey("error-codes"))
                            {
                                String error_codes = captchaResponse.ContainsKey("error-codes") ? (captchaResponse["error-codes"] as ArrayList)[0].ToString() : "";

                                switch (error_codes)
                                {
                                    case "missing-input-secret": ErrorMessage = String.Format("[{0}]:{1}", error_codes, "The secret parameter is missing."); break;
                                    case "invalid-input-secret": ErrorMessage = String.Format("[{0}]:{1}", error_codes, "The secret parameter is invalid or malformed."); break;
                                    case "missing-input-response": ErrorMessage = String.Format("[{0}]:{1}", error_codes, "The response parameter is missing."); break;
                                    case "invalid-input-response": ErrorMessage = String.Format("[{0}]:{1}", error_codes, "The response parameter is invalid or malformed."); break;
                                    default: ErrorMessage = String.Format("[{0}]:{1}", error_codes, "The response parameter is invalid or malformed..."); break;
                                }
                            }
                            else if (captchaResponse.ContainsKey("error-net"))
                            {
                                ErrorMessage = Convert.ToString(captchaResponse["error-net"]);
                            }




                        }
                        else if(captchaResponse.ContainsKey("social") && !String.IsNullOrEmpty(captchaResponse["social"].ToString()))
                        {
                            //成功了,需要判断下阈值
                            float Social = 1;
                            if (float.TryParse(captchaResponse["social"].ToString(), out Social))
                            {
                                //设置的阈值
                                var Recaptcha_Social = ViewSettingT<float>("PowerForms_Recaptcha_v3_Social", 0.1);
                                if (Social > Recaptcha_Social)
                                {
                                    //你的访问行为评分[{0}]过低，为了防止机器人提交，已经屏蔽此次提交!
                                    ErrorMessage = String.Format( ViewResourceText("RecaptchaScoreOverrun", "Your visit behavior rating [{0}] is too low. To prevent the submit from robot, this submission has been blocked!"), Social);
                                    Verify = false;
                                }

                            }

                        }
                    }
                    else
                    {
                        ErrorMessage = ViewResourceText("UnableConnect", "Unable to connect to the remote server");
                        Verify = false;
                    }
             
                    
                }
                else
                {
                    ErrorMessage = ViewResourceText("CaptchaError", "");
                    Verify = false;
                }
            }


            //隐藏域验证
            if (Settings_Hiddenfields_Enable)
            {
                //验证重复提交表单的代码
                Boolean RepeatSubmitted = true;
                //验证的步骤
                Boolean HiddenfieldsVerify = VerificationHiddenfields(ref RepeatSubmitted);
                //没有验证通过时进入
                if (!HiddenfieldsVerify)
                {
                    //没有通过验证
                    ErrorMessage = ViewResourceText("HiddenfieldsVerifyError", "Hiddenfields Verify Error");
                    Verify = false;
                }
                if (!RepeatSubmitted)
                {
                    //没有通过验证
                    ErrorMessage = ViewResourceText("RepeatSubmittedError","Hiddenfields Verify Error - 2");
                    Verify = false;
                }
            }


            //通过验证才能将表单提交到数据库
            if (Verify)
            {
                //提交表单到数据库
                SubmitFormToDB();
            }
            else
            {
                BindDataItem(Setting_EffectDB, ErrorMessage);
            }

        }

        /// <summary>
        /// 提交表单信息到数据库
        /// </summary>
        public void SubmitFormToDB()
        {
            DNNGo_PowerForms_Content SubmitContent = new DNNGo_PowerForms_Content();

            //读取需要载入的参数
            List<DNNGo_PowerForms_ContentItem> ContentList = new List<DNNGo_PowerForms_ContentItem>();


            Boolean SubmitValue = false;
            if (FieldList != null && FieldList.Count > 0)
            {
                foreach (DNNGo_PowerForms_Field fieldItem in FieldList)
                {
                    DNNGo_PowerForms_ContentItem ContentItem = new DNNGo_PowerForms_ContentItem();
                    ContentItem.FieldID = fieldItem.ID;
                    ContentItem.FieldName = fieldItem.Name;
                    ContentItem.FieldAlias = fieldItem.Alias;
                    ContentItem.Sort = fieldItem.Sort;
                    ContentItem.ContentValue = GetWebFormValue(fieldItem);
                    ContentItem.Group = DNNGo_PowerForms_Group.FindNameByKeyForEdit(fieldItem.GroupID);
                    if (!String.IsNullOrEmpty(ContentItem.ContentValue))
                    {
                        SubmitValue = true;
                    }

                    ContentList.Add(ContentItem);
                }
            }

            //是否添加额外跟踪属性
            if (Settings_ExtraTracking)
            {
                ContentList.AddRange(GetExtraTracking());
            }


            if (SubmitValue)
            {
                //判断是否需要隐藏IP
                String UserHost = WebHelper.UserHost;
                Boolean HideIp = Settings["PowerForms_HideIp"] != null ? Convert.ToBoolean(Settings["PowerForms_HideIp"]) : false;
                if (HideIp)
                {
                    UserHost = Common.HideIpAddress(UserHost);
                }

                SubmitContent.VerifyString = WebHelper.GetStringParam(Request, hfVerifyString.UniqueID, "");
                SubmitContent.LastIP = UserHost;
                SubmitContent.LastTime = xUserTime.LocalTime();
                SubmitContent.LastUser = UserId;
                SubmitContent.ModuleId = ModuleId;
                SubmitContent.PortalId = PortalId;
                SubmitContent.CultureInfo = System.Globalization.CultureInfo.CurrentCulture.Name;
                SubmitContent.FormVersion = ViewSettingT<String>("PowerForms_FormVersion", "");


                //默认是当前登录用户的邮箱
                if (UserId > 0 && UserInfo != null && !String.IsNullOrEmpty(UserInfo.Email) && Mail.IsValidEmailAddress(UserInfo.Email, Null.NullInteger))
                {
                    SubmitContent.UserName = UserInfo.Username;
                    SubmitContent.Email = UserInfo.Email;
                }


                if (!String.IsNullOrEmpty(Settings_SubmitUserEmail))
                {
                    //如果没有填写邮箱时
                    DNNGo_PowerForms_ContentItem EmailItem = ContentList.Find(r1 => r1.FieldName == Settings_SubmitUserEmail );
                    //if (EmailItem != null && !String.IsNullOrEmpty(EmailItem.ContentValue) && Mail.IsValidEmailAddress(EmailItem.ContentValue, Null.NullInteger))
                    if (EmailItem != null && !String.IsNullOrEmpty(EmailItem.ContentValue))
                    {
                        SubmitContent.Email = EmailItem.ContentValue;
                    }
                }


                //如果没有填写姓名时
                DNNGo_PowerForms_Field DisplayNameField = FieldList.Find(r => r.FieldType == (Int32)EnumViewControlType.TextBox_DisplayName);
                if (DisplayNameField != null && DisplayNameField.ID > 0)
                {
                    DNNGo_PowerForms_ContentItem DisplayNameItem = ContentList.Find(r1 => r1.FieldName == DisplayNameField.Name);
                    if (DisplayNameItem != null && !String.IsNullOrEmpty(DisplayNameItem.ContentValue))
                    {
                        SubmitContent.UserName = DisplayNameItem.ContentValue;
                    }
                }



                //序列化收集到的提交值列表
                if (ContentList != null && ContentList.Count > 0)
                {
                    SubmitContent.ContentValue = Common.Serialize<List<DNNGo_PowerForms_ContentItem>>(ContentList);
                }

                SubmitContent.Status = (Int32)EnumStatus.Activation;

                //是否需要保存记录
                Boolean SaveRecords = ViewSettingT<bool>("PowerForms_SaveRecords", true);
                //不保存记录 或 保存记录
                if (!SaveRecords || (SaveRecords && SubmitContent.Insert() > 0))
                {
                    //提交成功发邮件的方法
                    SendMail(SubmitContent, ContentList, FieldList);

                    //推送数据到第三方URL
                    Boolean Push_Enable = Settings["PowerForms_Push_Enable"] != null && !string.IsNullOrEmpty(Settings["PowerForms_Push_Enable"].ToString()) ? Convert.ToBoolean(Settings["PowerForms_Push_Enable"]) : false;
                    if (Push_Enable)
                    {
                        PushForm push = new PushForm(this);
                        push.ContentList = ContentList;
                        push.SubmitContent = SubmitContent;
                        push.Push();
                    }


                    if (iFrame.IndexOf("iFrame", StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        if (!String.IsNullOrEmpty(Settings_RedirectPage))
                        {
                            Response.Write(String.Format("<script> window.parent.location = '{0}';</script>", RedirectPage(SubmitContent)));
                        }
                        else
                        {
                            //提交成功跳转的页面
                            Response.Redirect(RedirectPage(SubmitContent));
                        }
                    }
                    else
                    {
                        //提交成功跳转的页面
                        Response.Redirect(RedirectPage(SubmitContent));
                    }
                }
                else
                {
                    //新增表单失败
                }
            }
            else
            {
                //提交的内容是空的，没任何信息
            }
        }

 



        /// <summary>
        /// 提交成功发送邮件
        /// 1.构造管理员邮件并发送
        /// 2.构造提交者的邮件并发送
        /// </summary>
        /// <param name="SubmitContent"></param>
        public void SendMail(DNNGo_PowerForms_Content SubmitContent, List<DNNGo_PowerForms_ContentItem> ContentList, List<DNNGo_PowerForms_Field> FieldList)
        {


            DNNGo_PowerForms_Template Template = Settings_EmailTemplate;

            //if (String.IsNullOrEmpty(SubmitContent.Email) || SubmitContent.Email.IndexOf("Anonymous e-mail", StringComparison.CurrentCultureIgnoreCase) >= 0)
            //{

            //    DNNGo_PowerForms_ContentItem ContentItem = ContentList.Find(r1 => r1.FieldName.IndexOf(Settings_SubmitUserEmail, StringComparison.CurrentCultureIgnoreCase) >= 0);

            //    if (ContentItem != null && !String.IsNullOrEmpty(ContentItem.ContentValue))
            //    {
            //        SubmitContent.Email = ContentItem.ContentValue;
            //    }
            //}

            EmailInfo Email = new EmailInfo();
            Email.Settings = Settings;
            Email.PushSettings(this);

            //1.构造管理员邮件并发送
            if (Settings_SendToAdmin && !String.IsNullOrEmpty(Settings_AdminEmail))//发邮件||管理员邮箱不为空
            {

                List<String> SendMailAddress = new List<string>();

                //Boolean ReplaceSender = Settings["PowerForms_ReplaceSender"] != null && !string.IsNullOrEmpty(Settings["PowerForms_ReplaceSender"].ToString()) ? Convert.ToBoolean(Settings["PowerForms_ReplaceSender"]) : false;
                //if (ReplaceSender && !String.IsNullOrEmpty(SubmitContent.Email) && Mail.IsValidEmailAddress(SubmitContent.Email, Null.NullInteger))
                //{
                //    Email.ReplyTo = SubmitContent.Email;//替换发件人地址为提交用户
                //}

                /** 因为要加入ReplyTo，这个需要记录下，屏蔽掉之前的代码  2015.12.21 **/
                String EmailReplyTo = String.Empty;
                Boolean ReplyTo = Settings["PowerForms_ReplyTo"] != null ? Convert.ToBoolean(Settings["PowerForms_ReplyTo"]) : true;
                if (ReplyTo && !String.IsNullOrEmpty(SubmitContent.Email) && Mail.IsValidEmailAddress(SubmitContent.Email, Null.NullInteger))
                {
                    EmailReplyTo = SubmitContent.Email;//替换发件人地址为提交用户
                }

                //添加管理员邮件到代发列表
                if (!String.IsNullOrEmpty(Settings_AdminEmail))
                {
                    if (Settings_AdminEmail.IndexOf(";") >= 0)
                    {
                        List<String> adminMailTos = WebHelper.GetList(Settings_AdminEmail, ";");
                        foreach (var adminMailTo in adminMailTos)
                        {
                            if (!String.IsNullOrEmpty(adminMailTo) && Mail.IsValidEmailAddress(adminMailTo, Null.NullInteger))
                            {
                                SendMailAddress.Add(adminMailTo);
                            }
                        }
                    }
                    else
                    {
                        SendMailAddress.Add(Settings_AdminEmail);
                    }
                }





                //检索勾选的待发送角色
                String AdminEmailRoles = Settings["PowerForms_AdminEmailRoles"] != null ? Convert.ToString(Settings["PowerForms_AdminEmailRoles"]) : "";
                if (!String.IsNullOrEmpty(AdminEmailRoles))
                {
                    List<String> RoleNames = Common.GetList(AdminEmailRoles);
                    if (RoleNames != null && RoleNames.Count > 0)
                    {

                        foreach (var RoleName in RoleNames)
                        {
                            if (!String.IsNullOrEmpty(RoleName))
                            {
                                //找出角色相关的用户信息
                                DotNetNuke.Security.Roles.RoleController roleController = new DotNetNuke.Security.Roles.RoleController();
                                ArrayList users = roleController.GetUsersByRoleName(PortalId, RoleName);
                                if (users != null && users.Count > 0)
                                {
                                    foreach (UserInfo user in users)
                                    {
                                        //判断邮件地址是否符合
                                        if (!String.IsNullOrEmpty(user.Email) && Mail.IsValidEmailAddress(user.Email, Null.NullInteger) && !(SendMailAddress.IndexOf(user.Email) >= 0))
                                        {
                                            SendMailAddress.Add(user.Email);
                                        }
                                    }
                                }

                            }
                        }

                    }
                }


            

                //发送给所有的管理用户
                foreach (var SendMail in SendMailAddress)
                {
                    Email = new EmailInfo();
                    Email.Settings = Settings;
                    Email.PushSettings(this);

                    if (!String.IsNullOrEmpty(EmailReplyTo))
                    {
                        Email.ReplyTo = EmailReplyTo;
                    }

                    //构造邮件的主题和内容
                    Email.Subject = FormatContent(SubmitContent, Template.ReceiversSubject, ContentList);
                    Email.Content = FormatContent(SubmitContent, Template.ReceiversTemplate, ContentList);

                    Email.MailTo = SendMail;
                    MailScheduler.AssignMessage(Email);//加到待发队列
                }
            }

            //2.构造提交者的邮件并发送
            if (Settings_SendToSubmitUser)
            {
                if (!String.IsNullOrEmpty(SubmitContent.Email) && SubmitContent.Email.IndexOf("Anonymous e-mail", StringComparison.CurrentCultureIgnoreCase) < 0)
                {
                    Email = new EmailInfo();
                    Email.Settings = Settings;
                    Email.PushSettings(this);
                    Email.MailTo = SubmitContent.Email;

                    //构造邮件的主题和内容
                    Email.Subject = FormatContent(SubmitContent, Template.ReplySubject, ContentList);
                    Email.Content = FormatContent(SubmitContent, Template.ReplyTemplate, ContentList);

                    MailScheduler.AssignMessage(Email);//加到待发队列
                    //NetHelper.SendMail(Email);
                }
            }

            //3.检查有无下拉列表发邮件的字段
            foreach (DNNGo_PowerForms_Field fieldItem in FieldList)
            {
                if (fieldItem.FieldType == (Int32)EnumViewControlType.DropDownList_SendEmail)
                {
                    DNNGo_PowerForms_ContentItem contentItem = ContentList.Find(r1 => r1.FieldName == fieldItem.Name);
                    if (contentItem != null && !String.IsNullOrEmpty(contentItem.FieldName))
                    {
                        Email = new EmailInfo();
                        Email.Settings = Settings;
                        Email.PushSettings(this);
                        Email.MailTo = contentItem.ContentValue;

                        //构造邮件的主题和内容
                        Email.Subject = FormatContent(SubmitContent, Template.ReceiversSubject, ContentList);
                        Email.Content = FormatContent(SubmitContent, Template.ReceiversTemplate, ContentList);

                        MailScheduler.AssignMessage(Email);//加到待发队列
                        //NetHelper.SendMail(Email);

                    }
                }
            }
 
        }

        /// <summary>
        /// 格式化内容模版
        /// </summary>
        /// <param name="SubmitContent">提交的内容实体</param>
        /// <param name="Template">模版</param>
        /// <returns></returns>
        public String FormatContent(DNNGo_PowerForms_Content SubmitContent,String Template, List<DNNGo_PowerForms_ContentItem> ContentList)
        {
            TemplateFormat xf = new TemplateFormat(this);
            xf.FieldList = FieldList;

            if (!String.IsNullOrEmpty(Template))
            { 
                //为了节约效率，需要先判断模版内有无需要替换的标签
                if (Template.IndexOf("[UserName]", StringComparison.CurrentCultureIgnoreCase) >= 0) Template = Common.ReplaceNoCase(Template, "[UserName]", SubmitContent.UserName);

                if (Template.IndexOf("[CultureInfo]", StringComparison.CurrentCultureIgnoreCase) >= 0) Template = Common.ReplaceNoCase(Template, "[CultureInfo]", SubmitContent.CultureInfo);

                if (Template.IndexOf("[SubmitTime]", StringComparison.CurrentCultureIgnoreCase) >= 0) Template = Common.ReplaceNoCase(Template, "[SubmitTime]", SubmitContent.LastTime.ToString());

                if (Template.IndexOf("[SubmitIP]", StringComparison.CurrentCultureIgnoreCase) >= 0) Template = Common.ReplaceNoCase(Template, "[SubmitIP]", SubmitContent.LastIP);

                if (Template.IndexOf("[Email]", StringComparison.CurrentCultureIgnoreCase) >= 0) Template = Common.ReplaceNoCase(Template, "[Email]", SubmitContent.Email);
 
                //if (Template.IndexOf("[]", StringComparison.CurrentCultureIgnoreCase) >= 0) Template = Common.ReplaceNoCase(Template, "[]", "");

                //2014.4.30 新增更多用户的Token,但需要是用户登陆时提交的才行.
                if (SubmitContent.LastUser > 0)
                {
                    
                    DotNetNuke.Entities.Users.UserInfo uInfo = DotNetNuke.Entities.Users.UserController.GetUserById(PortalId, SubmitContent.LastUser);
                    if (uInfo != null && uInfo.UserID > 0)
                    {
                        if (Template.IndexOf("[DisplayName]", StringComparison.CurrentCultureIgnoreCase) >= 0) Template = Common.ReplaceNoCase(Template, "[DisplayName]", uInfo.DisplayName);
                        if (Template.IndexOf("[LastName]", StringComparison.CurrentCultureIgnoreCase) >= 0) Template = Common.ReplaceNoCase(Template, "[LastName]", uInfo.LastName);
                        if (Template.IndexOf("[FirstName]", StringComparison.CurrentCultureIgnoreCase) >= 0) Template = Common.ReplaceNoCase(Template, "[FirstName]", uInfo.FirstName);
                        if (Template.IndexOf("[UserRole]", StringComparison.CurrentCultureIgnoreCase) >= 0) Template = Common.ReplaceNoCase(Template, "[UserRole]", Common.GetStringByList( uInfo.Roles));
                 
                        
                    }
                    else
                    {
                        if (Template.IndexOf("[DisplayName]", StringComparison.CurrentCultureIgnoreCase) >= 0) Template = Common.ReplaceNoCase(Template, "[DisplayName]", "Anonymous users");
                        if (Template.IndexOf("[LastName]", StringComparison.CurrentCultureIgnoreCase) >= 0) Template = Common.ReplaceNoCase(Template, "[LastName]", "--");
                        if (Template.IndexOf("[FirstName]", StringComparison.CurrentCultureIgnoreCase) >= 0) Template = Common.ReplaceNoCase(Template, "[FirstName]", "--");
                        if (Template.IndexOf("[UserRole]", StringComparison.CurrentCultureIgnoreCase) >= 0) Template = Common.ReplaceNoCase(Template, "[UserRole]", "--");
                    }
                }
                else
                {
                    if (Template.IndexOf("[DisplayName]", StringComparison.CurrentCultureIgnoreCase) >= 0) Template = Common.ReplaceNoCase(Template, "[DisplayName]", "Anonymous users");
                    if (Template.IndexOf("[LastName]", StringComparison.CurrentCultureIgnoreCase) >= 0) Template = Common.ReplaceNoCase(Template, "[LastName]", "--");
                    if (Template.IndexOf("[FirstName]", StringComparison.CurrentCultureIgnoreCase) >= 0) Template = Common.ReplaceNoCase(Template, "[FirstName]", "--");
                    if (Template.IndexOf("[UserRole]", StringComparison.CurrentCultureIgnoreCase) >= 0) Template = Common.ReplaceNoCase(Template, "[UserRole]", "--");
                }


                //循环打印所有的字段值
                foreach (DNNGo_PowerForms_ContentItem item in ContentList)
                {
                    String item_key = String.Format("[{0}]", item.FieldName);
                    if (Template.IndexOf(item_key, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        Template = Common.ReplaceNoCase(Template, item_key, xf.ViewContentValue(item));
                    }
                }


                if (Template.IndexOf("[Content]", StringComparison.CurrentCultureIgnoreCase) >= 0)
                { 
                    EffectDB EffectDB = Setting_EffectDB;

                    Hashtable Puts = new Hashtable();

                    String FormUrl = Globals.NavigateURL(TabId);
                    if (FormUrl.ToLower().IndexOf("http://") < 0 && FormUrl.ToLower().IndexOf("https://") < 0)
                    {
                        FormUrl = string.Format("{2}://{0}{1}", WebHelper.GetHomeUrl(), FormUrl, PortalSettings.SSLEnabled ? "https" : "http");
                    }

                    Puts.Add("ContentList", ContentList);
                    Puts.Add("FieldList", FieldList);
                    Puts.Add("EffectName", Settings_EffectName);
                    Puts.Add("ThemeName", Settings_EffectThemeName);
                    Puts.Add("Group", EffectDB.Group);//有分组的时候才会显示分组
                    Puts.Add("FormUrl", FormUrl);
                    Puts.Add("FormTitle",ModuleConfiguration.ModuleTitle);
                    Puts.Add("ExtraTracking", Settings_ExtraTracking);//是否启用跟踪

                   Template = Common.ReplaceNoCase(Template, "[Content]", ViewTemplate(EffectDB, "EmailTable.html", Puts, xf));
                 
                }



            }
            return Template;
        }








        /// <summary>
        /// 提交成功跳转页面
        /// </summary>
        /// <param name="SubmitContent"></param>
        /// <returns></returns>
        public String RedirectPage(DNNGo_PowerForms_Content SubmitContent)
        {
            //默认跳转到结果页
            String UrlValue = Globals.NavigateURL("",String.Format( "Token=Result{0}",ModuleId), "FormID=" + SubmitContent.ID.ToString());
            //这里判断跳转的类型(指定页面)
            if (!String.IsNullOrEmpty(Settings_RedirectPage))
            {
                if (Settings_RedirectPage.IndexOf("TabID=", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    UrlValue = Globals.NavigateURL(Convert.ToInt32(Settings_RedirectPage.Replace("TabID=", "")),"",String.Format("FormID{0}={1}", ModuleId, SubmitContent.ID));
                }
                else if (Settings_RedirectPage.IndexOf("sFileID=", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    var fi = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(Convert.ToInt32(Settings_RedirectPage.Replace("sFileID=", "")));
                    if (fi != null && fi.FileId > 0)
                    {
                        String filepath = MapPath( string.Format("{0}{1}{2}", PortalSettings.HomeDirectory, fi.Folder, Server.UrlPathEncode(fi.FileName)));
                        FileSystemUtils.DownloadFile(filepath, fi.FileName);
                    }
                  
                
                }
                else
                {
                    UrlValue = Settings_RedirectPage;
                }
            }
            else
            {
                if (iFrame.IndexOf("iFrame", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    String language = WebHelper.GetStringParam(Request, "language", PortalSettings.DefaultLanguage);
                    UrlValue = String.Format("{0}View_iFrame.aspx?PortalId={1}&TabId={2}&ModuleId={3}&language={4}&Token=Result{3}&FormID={5}&iFrame=iFrame", ModulePath, PortalId, TabId, ModuleId, language, SubmitContent.ID);
                }
            }

            String RedirectPageAnchorLink = ViewSettingT<String>("PowerForms_RedirectPageAnchorLink", "");
            if (!String.IsNullOrEmpty(RedirectPageAnchorLink))
            {
                UrlValue = String.Format("{0}#{1}", UrlValue, RedirectPageAnchorLink);
            }

            return UrlValue;
        }




        /// <summary>
        /// 获取Form传值
        /// </summary>
        /// <param name="fieldItem"></param>
        /// <returns></returns>
        public String GetWebFormValue(DNNGo_PowerForms_Field fieldItem)
        {
            String WebFormValue = String.Empty;

            //创建控件的Name和ID
            TemplateFormat xf = new TemplateFormat(this);
            String ControlName = xf.ViewControlName(fieldItem);
            String ControlID = xf.ViewControlID(fieldItem);


            if (fieldItem.FieldType == (Int32)EnumViewControlType.CheckBox)
            {
                WebFormValue = WebHelper.GetStringParam(Request, ControlName, "");
                WebFormValue = !String.IsNullOrEmpty(WebFormValue) && WebFormValue == "on" ? "true" : "false";
            }
            else if (fieldItem.FieldType == (Int32)EnumViewControlType.FileUpload)
            {
                if (Request.Files != null && Request.Files.Count > 0)
                {


                    HttpPostedFile hpFile = Request.Files.Get(ControlName);
                    if (hpFile != null && hpFile.ContentLength > 0)
                    {
                        //To verify that if the suffix name of the uploaded files meets the DNN HOST requirements
                        Boolean retValue = FileSystemUtils.CheckValidFileName(hpFile.FileName);
                        if (retValue)
                        {
                            WebFormValue = String.Format("Url://{0}", FileSystemUtils.UploadFile(hpFile, this));//存放到目录中，并返回
                        }
                    }
                }
            }
            else if (fieldItem.FieldType == (Int32)EnumViewControlType.MultipleFilesUpload)
            {
                String WebUploads = WebHelper.GetStringParam(Request, ControlName, "");
                if (!String.IsNullOrEmpty(WebUploads) && WebUploads != "[]")
                {
                    List<Resource_FilesStatus> Uploads =  jsSerializer.Deserialize<List<Resource_FilesStatus>>(WebUploads);
                    if (Uploads != null && Uploads.Count > 0)
                    {
                        List<String> fileurls = new List<string>();
                        foreach (var UploadFile in Uploads)
                        {
                            fileurls.Add(String.Format("Url://{0}", FileSystemUtils.CopyFile(UploadFile, this)));
                        }
                        WebFormValue = Common.GetStringByList(fileurls,"<|>");
                    }
                }
            }
            else if (fieldItem.FieldType == (Int32)EnumViewControlType.DropDownList_Country)
            {
                var tempWebFormValue = WebHelper.GetStringParam(HttpContext.Current.Request, ControlName, "");
                
                var Countrys =   new ListController().GetListEntryInfoItems("Country");
                foreach (var Country in Countrys)
                {
                    if (Country.Value == tempWebFormValue)
                    {
                        WebFormValue = Country.Text;
                        break;
                    }
                }
            }
            else
            {
                WebFormValue = WebHelper.GetStringParam(Request, ControlName, "");

                if (!(fieldItem.FieldType == (Int32)EnumViewControlType.RichTextBox))
                {
                    //非富文本框时，需要过滤掉XSS特殊字符
                    WebFormValue = Common.LostXSS(WebFormValue);
                }

                //如果提示的值和输入的值一样的情况，就过滤掉该值 *** 有点争议的地方
                if (WebFormValue == fieldItem.ToolTip && fieldItem.DefaultValue != WebFormValue) WebFormValue = string.Empty;
            }

            return WebFormValue;

        }



        /// <summary>
        /// 绑定隐藏控件域(防止提交垃圾)
        /// </summary>
        public void BindHiddenField()
        {
          
                if (Settings_Hiddenfields_Enable)
                {
                    //生成认证密钥10位
                    String VerifyString = WebHelper.leftx(Guid.NewGuid().ToString("N"), Settings_Hiddenfields_VerifyStringLength);
                    String VerifyEncrypt = CryptionHelper.EncryptString1(VerifyString, Settings_Hiddenfields_EncryptionKey);// CryptionHelper.EncryptString(VerifyString, Settings_Hiddenfields_EncryptionKey);

                    hfVerifyString.Value = VerifyString;
                    hfVerifyEncrypt.Value = VerifyEncrypt;
                }
            
        }


        /// <summary>
        /// 验证隐藏域
        /// </summary>
        /// <returns></returns>
        public Boolean VerificationHiddenfields(ref Boolean RepeatSubmitted)
        {
            Boolean HiddenfieldsVerify = true;
            String VerifyEncrypt = WebHelper.GetStringParam(Request, hfVerifyEncrypt.UniqueID, "",false);
            String VerifyString = WebHelper.GetStringParam(Request, hfVerifyString.UniqueID, "",false);
            if (!String.IsNullOrEmpty(VerifyEncrypt) && !String.IsNullOrEmpty(VerifyString))
            {
                try
                {
                    String DecryptString = CryptionHelper.DecryptString1(VerifyEncrypt, Settings_Hiddenfields_EncryptionKey);//CryptionHelper.DecryptString(VerifyEncrypt, Settings_Hiddenfields_EncryptionKey);
                    if (!String.IsNullOrEmpty(DecryptString) && VerifyString == DecryptString)
                    {
                        //查询该信息是否为重复提交的
                        QueryParam qp = new QueryParam();
                        qp.Where.Add(new SearchParam(DNNGo_PowerForms_Content._.VerifyString, VerifyString, SearchType.Equal));
                        qp.Where.Add(new SearchParam(DNNGo_PowerForms_Content._.LastTime, xUserTime.UtcTime().AddMinutes(-Settings_Hiddenfields_VerifyIntervalTime), SearchType.GtEqual));
                        if (DNNGo_PowerForms_Content.FindCount(qp) > 0)
                        {
                            RepeatSubmitted = false;
                        }
                        
                    }
                    else
                    {
                        HiddenfieldsVerify = false;
                    }
                }
                catch
                {
                    HiddenfieldsVerify = false;
                }
            }
            else
            {
                HiddenfieldsVerify = false;
            }
            return HiddenfieldsVerify;
        }

        /// <summary>
        /// 获取扩展跟踪信息
        /// </summary>
        /// <returns></returns>
        public List<DNNGo_PowerForms_ContentItem> GetExtraTracking()
        {
            List<DNNGo_PowerForms_ContentItem> list = new List<DNNGo_PowerForms_ContentItem>();
            list.Add(GetExtraTrackingItem("Tracking_PageURL", "Form Page URL", String.Format("http://{0}{1}", WebHelper.GetHomeUrl(), Request.RawUrl), 101));
            list.Add(GetExtraTrackingItem("Tracking_OriginalReferrer", "Original Referrer", Tracking_OriginalReferrer, 102));
            list.Add(GetExtraTrackingItem("Tracking_LandingPage", "Landing Page", Tracking_LandingPage, 103));
            list.Add(GetExtraTrackingItem("Tracking_UserIP", "User IP", WebHelper.UserHost, 104));
            list.Add(GetExtraTrackingItem("Tracking_UserAgent", "User Agent / Browser", Request.UserAgent, 105));

            return list;
        }

        /// <summary>
        /// 创建单个扩展信息
        /// </summary>
        /// <param name="FieldName"></param>
        /// <param name="FieldAlias"></param>
        /// <param name="ContentValue"></param>
        /// <param name="Sort"></param>
        /// <returns></returns>
        public DNNGo_PowerForms_ContentItem GetExtraTrackingItem(String FieldName, String FieldAlias, String ContentValue, Int32 Sort)
        {
            DNNGo_PowerForms_ContentItem ContentItem = new DNNGo_PowerForms_ContentItem();
            ContentItem.Group = "Tracking";
            ContentItem.FieldID = 0;
            ContentItem.FieldName = FieldName;
            ContentItem.FieldAlias = FieldAlias;
            ContentItem.Sort = Sort;
            ContentItem.ContentValue = ContentValue;
            ContentItem.Extra = true;
            return ContentItem;
        }


  

        #endregion




    }
}