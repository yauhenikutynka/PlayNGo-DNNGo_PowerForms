using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Services.Scheduling;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Common;
using DotNetNuke.Entities.Host;
using System.IO;

namespace DNNGo.Modules.PowerForms
{
    /// <summary>
    /// 调度器管理类
    /// </summary>
    public class SchedulerHelper : DotNetNuke.Services.Scheduling.SchedulerClient
    {


                /// <summary>
        /// 默认构造
        /// </summary>
        public SchedulerHelper() { }

        public SchedulerHelper(DotNetNuke.Services.Scheduling.ScheduleHistoryItem objScheduleHistoryItem)
            : base()
        {
            this.ScheduleHistoryItem = objScheduleHistoryItem;
        }

        ModuleController objModules = new ModuleController();

   

        #region "调度器的执行"
        /// <summary>
        /// 调度器执行
        /// </summary>
        public override void DoWork()
        {
            try
            {
                this.Progressing();

                //这里写上需要执行的任务

                ExecutionTasks();


               
                this.ScheduleHistoryItem.Succeeded = true;
                this.ScheduleHistoryItem.AddLogNote(String.Format("work to completed. time:{0}",DateTime.Now.ToString()));
            }
            catch (Exception exc)
            {
                this.ScheduleHistoryItem.Succeeded = false;
                this.ScheduleHistoryItem.AddLogNote("work to failed." + exc.ToString());
                this.Errored(ref exc);
                DotNetNuke.Services.Exceptions.Exceptions.LogException(exc);
            }

        }
        #endregion


        #region "任务的执行方法"

        /// <summary>
        /// 执行多个任务
        /// </summary>
        public void ExecutionTasks()
        {
            //找出所有需要执行的模块
            List<DNNGo_PowerForms_Scheduler> SchedulerList = DNNGo_PowerForms_Scheduler.FindAllSettings().Distinct<DNNGo_PowerForms_Scheduler>().ToList<DNNGo_PowerForms_Scheduler>();
            for (int i = 0; i < SchedulerList.Count; i++)
            {
               DNNGo_PowerForms_Scheduler SchedulerItem = SchedulerList[i];
               int index = i + 1;
                
               ModuleInfo mItem = objModules.GetModule(SchedulerItem.ModuleId);//验证当前模块是否存在
               if (mItem != null && mItem.ModuleID > 0 && !mItem.IsDeleted)
               {
                   //this.ScheduleHistoryItem.AddLogNote(String.Format("正在执行导出第{0}个任务,请稍后. time:{1}<br />", index, DateTime.Now.ToString()));
                   ExecutionTask(SchedulerItem, index, mItem);
                   //this.ScheduleHistoryItem.AddLogNote(String.Format("成功完成第{0}个任务,请稍后. time:{1}<br />", index, DateTime.Now.ToString()));
               }
               else
               {
                   this.ScheduleHistoryItem.AddLogNote(String.Format("task {0} error,Has ignored. time:{1}<br />", index, DateTime.Now.ToString()));
               }
            }
        }
        /// <summary>
        /// 执行单个任务
        /// </summary>
        public void ExecutionTask(DNNGo_PowerForms_Scheduler SchedulerItem, Int32 index, ModuleInfo mItem)
        {
            String Attachment = ExportExcel(SchedulerItem, index);
            if (!String.IsNullOrEmpty(Attachment) && File.Exists(Attachment))
            {
                SendEmail(SchedulerItem, Attachment, index, mItem);
            }
        }

        /// <summary>
        /// 导出表格
        /// </summary>
        /// <param name="SchedulerItem"></param>
        /// <returns></returns>
        public String ExportExcel(DNNGo_PowerForms_Scheduler SchedulerItem, Int32 index)
        {
            String Attachment = String.Empty;
            List<DNNGo_PowerForms_Content> DataList = DNNGo_PowerForms_Content.FindAllByModuleId(SchedulerItem.ModuleId);
            List<DNNGo_PowerForms_Field> FieldList = DNNGo_PowerForms_Field.FindAllByModuleId(SchedulerItem.ModuleId);
            if (DataList != null && DataList.Count > 0)
            {
                if (FieldList != null && FieldList.Count > 0)
                {
                    String FileName = SchedulerItem.ExcelName;
                    FileName = Common.ReplaceNoCase(FileName, "{yyyy}", DateTime.Now.ToString("yyyy"));
                    FileName = Common.ReplaceNoCase(FileName, "{mm}", DateTime.Now.ToString("MM"));
                    FileName = Common.ReplaceNoCase(FileName, "{dd}", DateTime.Now.ToString("dd"));
                    FileName = Common.ReplaceNoCase(FileName, "{time}", DateTime.Now.ToString("HHmmss"));
                    FileName = Common.ReplaceNoCase(FileName, "{ModuleID}", SchedulerItem.ModuleId.ToString());

                    Attachment = String.Format("{0}temp\\PowerForms\\{1}.xls", HttpRuntime.AppDomainAppPath, FileName);

                    try
                    {
                        CsvHelper.SaveAsToFile(FieldList, DataList, Attachment, false);
                    }
                    catch (Exception exc)
                    {
                        this.ScheduleHistoryItem.AddLogNote(exc.Source);
                    }

                    


                }
                else
                {
                    this.ScheduleHistoryItem.AddLogNote(String.Format("task {0},field list is empty,Has ignored. time:{1}<br />", index, DateTime.Now.ToString()));
                }
            }
            else
            {
                this.ScheduleHistoryItem.AddLogNote(String.Format("task {0},History records is empty,Has ignored. time:{1}<br />", index, DateTime.Now.ToString()));
            }
            return Attachment;
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="SchedulerItem"></param>
        /// <param name="Attachment"></param>
        /// <returns></returns>
        public void SendEmail(DNNGo_PowerForms_Scheduler SchedulerItem, String Attachment, Int32 index, ModuleInfo mItem)
        {
            EmailInfo eInfo = new EmailInfo();
            eInfo.Settings = new ModuleController().GetModule(mItem.ModuleID).ModuleSettings;
            //eInfo.SMTPEnableSSL = SMTPEnableSSL(mItem.PortalID);
            eInfo.PushSettings();
            eInfo.Attachments = Attachment;
            eInfo.MailTo = SchedulerItem.SenderEmail;
            eInfo.Content = eInfo.Subject = String.Format("PowerForms Excel. Module Title:{0} , time:{1}", mItem.ModuleTitle, DateTime.Now.ToString());
          

            try
            {
                String status =  NetHelper.SendMail(eInfo);


                //this.ScheduleHistoryItem.AddLogNote(String.Format("SMTPServer:{0}; SMTPUsername:{1}; EnableSMTPSSL:{2}; status:{3};<br />", Host.SMTPServer, Host.SMTPUsername, Host.EnableSMTPSSL, status));

                this.ScheduleHistoryItem.AddLogNote(String.Format("task {0},Send mail to completed. time:{1}<br />", index,  DateTime.Now.ToString()));
            }
            catch (Exception exc)
            {
                this.ScheduleHistoryItem.AddLogNote(exc.Source);
            }

            //MailScheduler.AssignMessage(eInfo);
          
        }



        public bool SMTPEnableSSL(Int32 PortalID)
        {
            Dictionary<string, string> Portal_Settings = PortalController.GetPortalSettingsDictionary(PortalID);
           bool SMTPmode =  Portal_Settings["SMTPmode"] != null && Convert.ToString(Portal_Settings["SMTPmode"]) == "p";

            if (SMTPmode)
            {
                return  Portal_Settings["SMTPEnableSSL"] != null && Convert.ToString(Portal_Settings["SMTPEnableSSL"]) == "Y";
            }
            else
            {
                return  Host.EnableSMTPSSL;
            }
        }



        #endregion




        #region "调度器的设置"
        /// <summary>
        /// 更新调度器
        /// </summary>
        public void UpdateScheduler( basePortalModule bpm)
        {



            ScheduleItem objScheduleItem = SchedulingProvider.Instance().GetSchedule("DNNGo.Modules.PowerForms.SchedulerHelper,DNNGo.Modules.PowerForms", Null.NullString);
            if (!(objScheduleItem != null && objScheduleItem.ScheduleID > 0))
            {
                //这里需要创建新的调度器
                Int32 ScheduleID = AddScheduler();
                objScheduleItem = SchedulingProvider.Instance().GetSchedule(ScheduleID);
            }


            //if (objScheduleItem != null && objScheduleItem.ScheduleID > 0)
            //{
            //    UpdateSchedule(objScheduleItem);
            //}



        }





        public Int32 AddScheduler()
        {
            ScheduleItem item = new ScheduleItem();
 
            item = CreateSchedule(item);

            return AddScheduler(item);

        }

        /// <summary>
        /// 增加调度器
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Int32 AddScheduler(ScheduleItem item)
        {
            return SchedulingProvider.Instance().AddSchedule(item);
        }


 


        /// <summary>
        /// 更新调度器
        /// </summary>
        /// <param name="item"></param>
        public void UpdateSchedule(ScheduleItem item)
        {
            item = CreateSchedule(item);
            SchedulingProvider.Instance().UpdateSchedule(item);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public ScheduleItem CreateSchedule(ScheduleItem item)
        {
            item.TypeFullName = "DNNGo.Modules.PowerForms.SchedulerHelper,DNNGo.Modules.PowerForms";
            item.TimeLapse = 24;
            item.TimeLapseMeasurement = "h";
            item.RetryTimeLapse = 60;
            item.RetryTimeLapseMeasurement = "m";
            item.RetainHistoryNum = 100;
            item.CatchUpEnabled = false;
            item.Enabled = true;
            item.ObjectDependencies = "";//对象的依赖关系

            Type t = typeof(ScheduleItem);
            if (t.GetProperty("FriendlyName") != null)
            {
                t.GetProperty("FriendlyName").SetValue(item, "DNNGo.PowerForms Bulk Sender Email", null);
            }
            return item;
        }
        #endregion
    }
}