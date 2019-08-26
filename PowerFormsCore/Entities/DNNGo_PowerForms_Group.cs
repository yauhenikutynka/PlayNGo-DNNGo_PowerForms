using System;
using System.Collections.Generic;
using System.ComponentModel;
 

namespace DNNGo.Modules.PowerForms
{
    /// <summary>
    /// 表单分组
    /// </summary>
    [Serializable]
    [DataObject]
    [Description("表单分组")]
    [BindTable("DNNGo_PowerForms_Group", Description = "表单分组", ConnName = "SiteSqlServer")]
    public partial class DNNGo_PowerForms_Group : Entity<DNNGo_PowerForms_Group>
    {
        #region 属性
        private Int32 _ID;
        /// <summary>
        /// 分组编号
        /// </summary>
        [Description("分组编号")]
        [DataObjectField(true, true, false, 10)]
        [BindColumn("ID", Description = "分组编号", DefaultValue = "", Order = 1)]
        public Int32 ID
        {
            get { return _ID; }
            set { if (OnPropertyChange("ID", value)) _ID = value; }
        }

        private String _Name = String.Empty;
        /// <summary>
        /// 分组名称
        /// </summary>
        [Description("分组名称")]
        [DataObjectField(false, false, false, 128)]
        [BindColumn("Name", Description = "分组名称", DefaultValue = "", Order = 2)]
        public String Name
        {
            get { return _Name; }
            set { if (OnPropertyChange("Name", value)) _Name = value; }
        }

        private String _Description = String.Empty;
        /// <summary>
        /// 分组描述
        /// </summary>
        [Description("分组描述")]
        [DataObjectField(false, false, true, 512)]
        [BindColumn("Description", Description = "分组描述", DefaultValue = "", Order = 3)]
        public String Description
        {
            get { return _Description; }
            set { if (OnPropertyChange("Description", value)) _Description = value; }
        }

        private Int32 _Sort = 0;
        /// <summary>
        /// 分组排序
        /// </summary>
        [Description("分组排序")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn("Sort", Description = "分组排序", DefaultValue = "", Order = 4)]
        public Int32 Sort
        {
            get { return _Sort; }
            set { if (OnPropertyChange("Sort", value)) _Sort = value; }
        }

        private Int32 _ModuleId = 0;
        /// <summary>
        /// 模块编号
        /// </summary>
        [Description("模块编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn("ModuleId", Description = "模块编号", DefaultValue = "", Order = 5)]
        public Int32 ModuleId
        {
            get { return _ModuleId; }
            set { if (OnPropertyChange("ModuleId", value)) _ModuleId = value; }
        }

        private Int32 _PortalId = 0;
        /// <summary>
        /// 站点编号
        /// </summary>
        [Description("站点编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn("PortalId", Description = "站点编号", DefaultValue = "", Order = 6)]
        public Int32 PortalId
        {
            get { return _PortalId; }
            set { if (OnPropertyChange("PortalId", value)) _PortalId = value; }
        }

        private Int32 _Status = 0;
        /// <summary>
        /// 状态
        /// </summary>
        [Description("状态")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn("Status", Description = "状态", DefaultValue = "", Order = 7)]
        public Int32 Status
        {
            get { return _Status; }
            set { if (OnPropertyChange("Status", value)) _Status = value; }
        }

        private Int32 _LastUser;
        /// <summary>
        /// 更新用户
        /// </summary>
        [Description("更新用户")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn("LastUser", Description = "更新用户", DefaultValue = "", Order = 8)]
        public Int32 LastUser
        {
            get { return _LastUser; }
            set { if (OnPropertyChange("LastUser", value)) _LastUser = value; }
        }

        private String _LastIP;
        /// <summary>
        /// 更新IP
        /// </summary>
        [Description("更新IP")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn("LastIP", Description = "更新IP", DefaultValue = "", Order = 9)]
        public String LastIP
        {
            get { return _LastIP; }
            set { if (OnPropertyChange("LastIP", value)) _LastIP = value; }
        }

        private DateTime _LastTime;
        /// <summary>
        /// 更新时间
        /// </summary>
        [Description("更新时间")]
        [DataObjectField(false, false, false, 23)]
        [BindColumn("LastTime", Description = "更新时间", DefaultValue = "", Order = 10)]
        public DateTime LastTime
        {
            get { return _LastTime; }
            set { if (OnPropertyChange("LastTime", value)) _LastTime = value; }
        }



        private String _Options;
        /// <summary>选项集合</summary>
        [DisplayName("选项集合")]
        [Description("选项集合")]
        [DataObjectField(false, false, true, 1073741823)]
        [BindColumn(11, "Options", "选项集合", null, "ntext", 0, 0, true)]
        public virtual String Options
        {
            get { return _Options; }
            set { if (OnPropertyChange("Options", value)) { _Options = value; } }
        }

        #endregion

        #region 获取/设置 字段值
        /// <summary>
        /// 获取/设置 字段值。
        /// 一个索引，基类使用反射实现。
        /// 派生实体类可重写该索引，以避免反射带来的性能损耗
        /// </summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        public override Object this[String name]
        {
            get
            {
                switch (name)
                {
                    case "ID": return _ID;
                    case "Name": return _Name;
                    case "Description": return _Description;
                    case "Sort": return _Sort;
                    case "ModuleId": return _ModuleId;
                    case "PortalId": return _PortalId;
                    case "Status": return _Status;
                    case "LastUser": return _LastUser;
                    case "LastIP": return _LastIP;
                    case "LastTime": return _LastTime;
                    case "Options": return _Options;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case "ID": _ID = Convert.ToInt32(value); break;
                    case "Name": _Name = Convert.ToString(value); break;
                    case "Description": _Description = Convert.ToString(value); break;
                    case "Sort": _Sort = Convert.ToInt32(value); break;
                    case "ModuleId": _ModuleId = Convert.ToInt32(value); break;
                    case "PortalId": _PortalId = Convert.ToInt32(value); break;
                    case "Status": _Status = Convert.ToInt32(value); break;
                    case "LastUser": _LastUser = Convert.ToInt32(value); break;
                    case "LastIP": _LastIP = Convert.ToString(value); break;
                    case "LastTime": _LastTime = Convert.ToDateTime(value); break;
                    case "Options": _Options = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>
        /// 取得表单分组字段名的快捷方式
        /// </summary>
        public class _
        {
            ///<summary>
            /// 分组编号
            ///</summary>
            public const String ID = "ID";

            ///<summary>
            /// 分组名称
            ///</summary>
            public const String Name = "Name";

            ///<summary>
            /// 分组描述
            ///</summary>
            public const String Description = "Description";

            ///<summary>
            /// 分组排序
            ///</summary>
            public const String Sort = "Sort";

            ///<summary>
            /// 模块编号
            ///</summary>
            public const String ModuleId = "ModuleId";

            ///<summary>
            /// 站点编号
            ///</summary>
            public const String PortalId = "PortalId";

            ///<summary>
            /// 状态
            ///</summary>
            public const String Status = "Status";

            ///<summary>
            /// 更新用户
            ///</summary>
            public const String LastUser = "LastUser";

            ///<summary>
            /// 更新IP
            ///</summary>
            public const String LastIP = "LastIP";

            ///<summary>
            /// 更新时间
            ///</summary>
            public const String LastTime = "LastTime";



            ///<summary>选项集合</summary>
            public const String Options = ("Options");
        }
        #endregion
    }
}