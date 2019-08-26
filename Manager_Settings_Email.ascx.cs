using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using DotNetNuke.Entities.Host;

namespace DNNGo.Modules.PowerForms
{
    public partial class Manager_Settings_Email : basePortalModule
    {


        #region "==属性=="

        /// <summary>
        /// 模块操作类
        /// </summary>
        private static ModuleController controller = new ModuleController();




        /// <summary>提示操作类</summary>
        MessageTips mTips = new MessageTips();

        #endregion


        #region "==方法=="

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindDataToPage()
        {

            //发件人邮箱
            txtSenderEmail.Text = Settings["PowerForms_SenderEmail"] != null && !string.IsNullOrEmpty(Settings["PowerForms_SenderEmail"].ToString()) ? Convert.ToString(Settings["PowerForms_SenderEmail"]) : Host.HostEmail;

            #region "邮件设置"
            txtAdminEmail.Text = Settings["PowerForms_AdminEmail"] != null && !string.IsNullOrEmpty(Settings["PowerForms_AdminEmail"].ToString()) ? Convert.ToString(Settings["PowerForms_AdminEmail"]) : Host.HostEmail;
            cbSendToAdmin.Checked = Settings["PowerForms_SendToAdmin"] != null && !string.IsNullOrEmpty(Settings["PowerForms_SendToAdmin"].ToString()) ? Convert.ToBoolean(Settings["PowerForms_SendToAdmin"]) : true;
            cbSendToSubmitUser.Checked = Settings["PowerForms_SendToSubmitUser"] != null && !string.IsNullOrEmpty(Settings["PowerForms_SendToSubmitUser"].ToString()) ? Convert.ToBoolean(Settings["PowerForms_SendToSubmitUser"]) : true;
            //cbReplaceSender.Checked = Settings["PowerForms_ReplaceSender"] != null && !string.IsNullOrEmpty(Settings["PowerForms_ReplaceSender"].ToString()) ? Convert.ToBoolean(Settings["PowerForms_ReplaceSender"]) : false;
            cbReplyTo.Checked = Settings["PowerForms_ReplyTo"] != null ? Convert.ToBoolean(Settings["PowerForms_ReplyTo"]) : true;

            WebHelper.BindList<DNNGo_PowerForms_Field>(ddlSubmitUserEmail, DNNGo_PowerForms_Field.FindAllByView(ModuleId, EnumVerification.email), DNNGo_PowerForms_Field._.Alias, DNNGo_PowerForms_Field._.Name);
            WebHelper.SelectedListByValue(ddlSubmitUserEmail, Settings["PowerForms_SubmitUserEmail"] != null && !string.IsNullOrEmpty(Settings["PowerForms_SubmitUserEmail"].ToString()) ? Convert.ToString(Settings["PowerForms_SubmitUserEmail"]) : "Email");


            DotNetNuke.Security.Roles.RoleController rc = new DotNetNuke.Security.Roles.RoleController();
            WebHelper.BindList(cblAdminEmailRoles, rc.GetPortalRoles(PortalId), "RoleName", "RoleName");
            WebHelper.SelectedListMultiByValue(cblAdminEmailRoles, Settings["PowerForms_AdminEmailRoles"] != null ? Convert.ToString(Settings["PowerForms_AdminEmailRoles"]) : "");


            //需要读取模版的设置
            DNNGo_PowerForms_Template Template = DNNGo_PowerForms_Template.FindByModuleId(ModuleId);

            txtSendAdminTitle.Text = !String.IsNullOrEmpty(Template.ReceiversSubject) ? Template.ReceiversSubject : Localization.GetString("PowerForms_ReceiversSubject", this.LocalResourceFile);
            txtSendAdminContent.Text = !String.IsNullOrEmpty(Template.ReceiversTemplate) ? Server.HtmlDecode( Template.ReceiversTemplate) : Localization.GetString("PowerForms_ReceiversTemplate", this.LocalResourceFile);
            txtSendSubmitUserTitle.Text = !String.IsNullOrEmpty(Template.ReplySubject) ? Template.ReplySubject : Localization.GetString("PowerForms_ReplySubject", this.LocalResourceFile);
            txtSendSubmitUserContent.Text = !String.IsNullOrEmpty(Template.ReplyTemplate) ? Server.HtmlDecode( Template.ReplyTemplate) : Localization.GetString("PowerForms_ReplyTemplate", this.LocalResourceFile);
            #endregion

            #region "定时邮件设置"
            DNNGo_PowerForms_Scheduler SchedulerItem = DNNGo_PowerForms_Scheduler.FindSettings(this);
            txtScheduleSenderEmail.Text = !String.IsNullOrEmpty(SchedulerItem.SenderEmail) ? SchedulerItem.SenderEmail: Host.HostEmail;
            txtExcelName.Text = !String.IsNullOrEmpty(SchedulerItem.ExcelName) ? SchedulerItem.ExcelName : "Bulk_{yyyy}_{mm}_{dd}_{time}_{ModuleID}";
            cbScheduleEnable.Checked = SchedulerItem.Enable != 0;

            #endregion

        }

        /// <summary>
        /// 查找Eamil字段
        /// </summary>
        /// <param name="FieldList"></param>
        /// <returns></returns>
        public String ViewEmailField(List<DNNGo_PowerForms_Field> FieldList)
        {

            //验证类型为邮箱的
            DNNGo_PowerForms_Field fieldItem = FieldList.Find(r1 => r1.ID > 0 && (r1.Verification == (Int32)EnumVerification.email || r1.FieldType == (Int32)EnumViewControlType.DropDownList_SendEmail));
            if (fieldItem != null && fieldItem.ID > 0)
            {
                return fieldItem.Name;
            }

            //先找出首选项为email的
            fieldItem = FieldList.Find(r1 => r1.Name.IndexOf("Email", StringComparison.CurrentCultureIgnoreCase) >= 0);
            if (fieldItem != null && fieldItem.ID > 0 && fieldItem.Verification == (Int32)EnumVerification.email)
            {
                return "Email";
            }


            return "";
        }




        /// <summary>
        /// 设置数据项
        /// </summary>
        private void SetDataItem()
        {



            #region "邮件设置"
            UpdateModuleSetting("PowerForms_AdminEmail", txtAdminEmail.Text.Trim());
            UpdateModuleSetting("PowerForms_SendToAdmin", cbSendToAdmin.Checked.ToString());
            UpdateModuleSetting("PowerForms_SendToSubmitUser", cbSendToSubmitUser.Checked.ToString());
            //UpdateModuleSetting("PowerForms_ReplaceSender", cbReplaceSender.Checked.ToString());
            UpdateModuleSetting("PowerForms_ReplyTo", cbReplyTo.Checked.ToString());

            if (ddlSubmitUserEmail.SelectedIndex >= 0)
            {
                UpdateModuleSetting("PowerForms_SubmitUserEmail", ddlSubmitUserEmail.Items[ddlSubmitUserEmail.SelectedIndex].Value);
            }


            String textStr, idStr = String.Empty;
            WebHelper.GetSelected(cblAdminEmailRoles, out textStr, out idStr);
            UpdateModuleSetting("PowerForms_AdminEmailRoles", idStr);
           
            

            //发件人邮箱
            UpdateModuleSetting("PowerForms_SenderEmail", txtSenderEmail.Text.Trim());

            DNNGo_PowerForms_Template Template = DNNGo_PowerForms_Template.FindByModuleId(ModuleId);
            Template.ReceiversSubject = txtSendAdminTitle.Text.Trim();
            Template.ReceiversTemplate = txtSendAdminContent.Text.Trim();
            Template.ReplySubject = txtSendSubmitUserTitle.Text.Trim();
            Template.ReplyTemplate = txtSendSubmitUserContent.Text.Trim();

            Template.LastIP = WebHelper.UserHost;
            Template.LastTime = xUserTime.UtcTime();
            Template.LastUser = UserId;

            if (Template != null && Template.ID > 0)
            {
                Template.Update();
            }
            else
            {
                Template.ModuleId = ModuleId;
                Template.Insert();
            }

            #endregion


            #region "定时邮件设置"
            DNNGo_PowerForms_Scheduler SchedulerItem = DNNGo_PowerForms_Scheduler.FindSettings(this);
            SchedulerItem.Enable = cbScheduleEnable.Checked ? 1 : 0;
            SchedulerItem.ExcelName = txtExcelName.Text.Trim();
            SchedulerItem.SenderEmail = txtScheduleSenderEmail.Text.Trim();
            DNNGo_PowerForms_Scheduler.UpdateSettings(SchedulerItem, this);
            

            //UpdateModuleSetting("PowerForms_Schedule_SenderEmail", txtScheduleSenderEmail.Text.Trim());
            //UpdateModuleSetting("PowerForms_Schedule_ExcelName", txtExcelName.Text.Trim());
            //UpdateModuleSetting("PowerForms_Schedule_Enable", cbScheduleEnable.Checked.ToString());


            if (cbScheduleEnable.Checked)//开启或创建调度器
            {
                SchedulerHelper sh = new SchedulerHelper();
                sh.UpdateScheduler(this);
            }


            #endregion

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

                Response.Redirect(xUrl("EmailSettings"), true);
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


        #endregion









    }
}