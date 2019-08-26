using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Services.Scheduling;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using System.Collections;
using DotNetNuke.Entities.Portals;

namespace DNNGo.Modules.PowerForms
{
    /// <summary>
    /// 调度器管理类
    /// </summary>
    public class CleanupScheduler : DotNetNuke.Services.Scheduling.SchedulerClient
    {


                /// <summary>
        /// 默认构造
        /// </summary>
        public CleanupScheduler() { }

        public CleanupScheduler(DotNetNuke.Services.Scheduling.ScheduleHistoryItem objScheduleHistoryItem)
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

            //遍历每个站点
            DotNetNuke.Entities.Portals.PortalController pc = new DotNetNuke.Entities.Portals.PortalController();
            ModuleController objModules = new ModuleController();
            Int32 AllRecordCount = 0;
            //遍历所有站点
            ArrayList Portals = pc.GetPortals();
            List<Int32> ModuleIDs = new List<int>();
            foreach (PortalInfo p in Portals)
            {
                if (p != null && p.PortalID >= 0)
                {

                    //遍历所有模块
                    ArrayList Modules = objModules.GetModulesByDefinition(p.PortalID, "DNNGo.PowerForms");
                    foreach (ModuleInfo m in Modules)
                    {
                        if (m != null && m.ModuleID > 0 && !ModuleIDs.Exists(r => r == m.ModuleID))
                        {
                            Hashtable ModuleSettings = m.ModuleSettings; 
                            Boolean Cleanup_Enable = ModuleSettings["PowerForms_Cleanup_Enable"] != null ? Convert.ToBoolean(ModuleSettings["PowerForms_Cleanup_Enable"]) : false;
                            if (Cleanup_Enable)//开启了清除
                            {
                                AllRecordCount += ExecutionTask(ModuleSettings, m);
                            }
                            ModuleIDs.Add(m.ModuleID);
                        }
                    }


                }
            }
            this.ScheduleHistoryItem.AddLogNote(String.Format("It cleared a total of {0} histroy records for all modules. time:{1}<br />", AllRecordCount, DateTime.Now.ToString()));

        }
        /// <summary>
        /// 执行单个任务
        /// </summary>
        public Int32 ExecutionTask(Hashtable ModuleSettings, ModuleInfo mInfo)
        {
            Int32 ModuleRecordCount = 0;

            int DaysBefore = ModuleSettings["PowerForms_Cleanup_DaysBefore"] != null ? Convert.ToInt32(ModuleSettings["PowerForms_Cleanup_DaysBefore"]) : 30;
            int MaxFeedback = ModuleSettings["PowerForms_Cleanup_MaxFeedback"] != null ? Convert.ToInt32(ModuleSettings["PowerForms_Cleanup_MaxFeedback"]) : 1000;

            Int32 RecordCount = 0;
            QueryParam qp = new QueryParam();
            qp.Where.Add(new SearchParam(DNNGo_PowerForms_Content._.LastTime, DateTime.Now.AddDays(-DaysBefore), SearchType.Lt));
            qp.Where.Add(new SearchParam(DNNGo_PowerForms_Content._.ModuleId, mInfo.ModuleID, SearchType.Equal));
            qp.Orderfld = DNNGo_PowerForms_Content._.ID;
            qp.OrderType = 0;

            List<DNNGo_PowerForms_Content> DeleteList = DNNGo_PowerForms_Content.FindAll(qp, out RecordCount);

            for (int i = 0; i < DeleteList.Count && i < MaxFeedback; i++)
            {
                if (DeleteList[i].Delete() > 0)
                {
                    ModuleRecordCount++;
                }
            }
            this.ScheduleHistoryItem.AddLogNote(String.Format("It cleared {1} history records. time:{2} .Module:{0}<br />", mInfo.ModuleID, ModuleRecordCount, DateTime.Now.ToString()));


            return ModuleRecordCount;

        }

 
 
        #endregion




        #region "调度器的设置"
        /// <summary>
        /// 更新调度器
        /// </summary>
        public void UpdateScheduler( basePortalModule bpm)
        {



            ScheduleItem objScheduleItem = SchedulingProvider.Instance().GetSchedule("DNNGo.Modules.PowerForms.CleanupScheduler,DNNGo.Modules.PowerForms", Null.NullString);
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
            item.TypeFullName = "DNNGo.Modules.PowerForms.CleanupScheduler,DNNGo.Modules.PowerForms";
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
                t.GetProperty("FriendlyName").SetValue(item, "DNNGo.PowerForms Cleanup Scheduler", null);
            }
            return item;
        }
        #endregion
    }
}