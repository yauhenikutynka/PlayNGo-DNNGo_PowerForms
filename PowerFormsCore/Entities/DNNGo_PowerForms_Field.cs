using System;
using System.Collections.Generic;
using System.ComponentModel;
 

namespace DNNGo.Modules.PowerForms
{
    /// <summary>
    /// 表单字段
    /// </summary>
    [Serializable]
    [DataObject]
    [Description("表单字段")]
    [BindTable("DNNGo_PowerForms_Field", Description = "表单字段", ConnName = "SiteSqlServer")]
    public partial class DNNGo_PowerForms_Field : Entity<DNNGo_PowerForms_Field>
    {
        #region 属性
        private Int32 _ID = 0;
        /// <summary>
        /// 字段编号
        /// </summary>
        [Description("字段编号")]
        [DataObjectField(true, true, false, 10)]
        [BindColumn("ID", Description = "字段编号", DefaultValue = "", Order = 1)]
        public Int32 ID
        {
            get { return _ID; }
            set { if (OnPropertyChange("ID", value)) _ID = value; }
        }

        private Int32 _GroupID = 0;
        /// <summary>
        /// 分组编号
        /// </summary>
        [Description("分组编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn("GroupID", Description = "分组编号", DefaultValue = "", Order = 2)]
        public Int32 GroupID
        {
            get { return _GroupID; }
            set { if (OnPropertyChange("GroupID", value)) _GroupID = value; }
        }

        private String _Name = String.Empty;
        /// <summary>
        /// 字段名
        /// </summary>
        [Description("字段名")]
        [DataObjectField(false, false, false, 100)]
        [BindColumn("Name", Description = "字段名", DefaultValue = "", Order = 3)]
        public String Name
        {
            get { return _Name; }
            set { if (OnPropertyChange("Name", value)) _Name = value; }
        }

        private String _Alias = String.Empty;
        /// <summary>
        /// 别名
        /// </summary>
        [Description("别名")]
        [DataObjectField(false, false, false, 100)]
        [BindColumn("Alias", Description = "别名", DefaultValue = "", Order = 4)]
        public String Alias
        {
            get { return _Alias; }
            set { if (OnPropertyChange("Alias", value)) _Alias = value; }
        }

        private String _ToolTip = String.Empty;
        /// <summary>
        /// 提示
        /// </summary>
        [Description("提示")]
        [DataObjectField(false, false, true, 256)]
        [BindColumn("ToolTip", Description = "提示", DefaultValue = "", Order = 5)]
        public String ToolTip
        {
            get { return _ToolTip; }
            set { if (OnPropertyChange("ToolTip", value)) _ToolTip = value; }
        }

        private String _Description = String.Empty;
        /// <summary>
        /// 描述
        /// </summary>
        [Description("描述")]
        [DataObjectField(false, false, true, 512)]
        [BindColumn("Description", Description = "描述", DefaultValue = "", Order = 6)]
        public String Description
        {
            get { return _Description; }
            set { if (OnPropertyChange("Description", value)) _Description = value; }
        }

        private String _DefaultValue=String.Empty;
        /// <summary>
        /// 默认值
        /// </summary>
        [Description("默认值")]
        [DataObjectField(false, false, true, 5120000)]
        [BindColumn("DefaultValue", Description = "默认值", DefaultValue = "", Order = 7)]
        public String DefaultValue
        {
            get { return _DefaultValue; }
            set { if (OnPropertyChange("DefaultValue", value)) _DefaultValue = value; }
        }




        private Int32 _FieldType = (Int32)EnumControlType.TextBox;
        /// <summary>
        /// 字段类型
        /// </summary>
        [Description("字段类型")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn("FieldType", Description = "字段类型", DefaultValue = "", Order = 8)]
        public Int32 FieldType
        {
            get { return _FieldType; }
            set { if (OnPropertyChange("FieldType", value)) _FieldType = value; }
        }


        private Int32 _Direction = (Int32)EnumControlDirection.Vertical;
        /// <summary>
        /// 控件布局方向
        /// </summary>
        [Description("控件布局方向")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn("Direction", Description = "控件布局方向", DefaultValue = "", Order = 9)]
        public Int32 Direction
        {
            get { return _Direction; }
            set { if (OnPropertyChange("Direction", value)) _Direction = value; }
        }


        private Int32 _Width = 100;
        /// <summary>
        /// 宽度
        /// </summary>
        [Description("宽度")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn("Width", Description = "宽度", DefaultValue = "", Order = 10)]
        public Int32 Width
        {
            get { return _Width; }
            set { if (OnPropertyChange("Width", value)) _Width = value; }
        }

        private Int32 _Rows = 1;
        /// <summary>
        /// 行数
        /// </summary>
        [Description("行数")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn("Rows", Description = "行数", DefaultValue = "", Order = 11)]
        public Int32 Rows
        {
            get { return _Rows; }
            set { if (OnPropertyChange("Rows", value)) _Rows = value; }
        }

        private String _FiledList = String.Empty;
        /// <summary>
        /// 列表字段值
        /// </summary>
        [Description("列表字段值")]
        [DataObjectField(false, false, true, 1073741823)]
        [BindColumn("FiledList", Description = "列表字段值", DefaultValue = "", Order = 12)]
        public String FiledList
        {
            get { return _FiledList; }
            set { if (OnPropertyChange("FiledList", value)) _FiledList = value; }
        }

        private Int32 _Required = 0;
        /// <summary>
        /// 是否必填
        /// </summary>
        [Description("是否必填")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn("Required", Description = "是否必填", DefaultValue = "", Order = 13)]
        public Int32 Required
        {
            get { return _Required; }
            set { if (OnPropertyChange("Required", value)) _Required = value; }
        }

        private Int32 _Verification = (Int32)EnumVerification.optional;
        /// <summary>
        /// 验证类型
        /// </summary>
        [Description("验证类型")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn("Verification", Description = "验证类型", DefaultValue = "", Order = 14)]
        public Int32 Verification
        {
            get { return _Verification; }
            set { if (OnPropertyChange("Verification", value)) _Verification = value; }
        }

        private Int32 _Sort = 99;
        /// <summary>
        /// 排序
        /// </summary>
        [Description("排序")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn("Sort", Description = "排序", DefaultValue = "", Order = 15)]
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
        [BindColumn("ModuleId", Description = "模块编号", DefaultValue = "", Order = 16)]
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
        [BindColumn("PortalId", Description = "站点编号", DefaultValue = "", Order = 17)]
        public Int32 PortalId
        {
            get { return _PortalId; }
            set { if (OnPropertyChange("PortalId", value)) _PortalId = value; }
        }

        private Int32 _Status = (Int32)EnumStatus.Activation;
        /// <summary>
        /// 状态
        /// </summary>
        [Description("状态")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn("Status", Description = "状态", DefaultValue = "", Order = 18)]
        public Int32 Status
        {
            get { return _Status; }
            set { if (OnPropertyChange("Status", value)) _Status = value; }
        }

        private Int32 _LastUser = 0;
        /// <summary>
        /// 更新用户
        /// </summary>
        [Description("更新用户")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn("LastUser", Description = "更新用户", DefaultValue = "", Order = 19)]
        public Int32 LastUser
        {
            get { return _LastUser; }
            set { if (OnPropertyChange("LastUser", value)) _LastUser = value; }
        }

        private String _LastIP= String.Empty;
        /// <summary>
        /// 更新IP
        /// </summary>
        [Description("更新IP")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn("LastIP", Description = "更新IP", DefaultValue = "", Order = 20)]
        public String LastIP
        {
            get { return _LastIP; }
            set { if (OnPropertyChange("LastIP", value)) _LastIP = value; }
        }

        private DateTime _LastTime = xUserTime.UtcTime();
        /// <summary>
        /// 更新时间
        /// </summary>
        [Description("更新时间")]
        [DataObjectField(false, false, false, 23)]
        [BindColumn("LastTime", Description = "更新时间", DefaultValue = "", Order = 21)]
        public DateTime LastTime
        {
            get { return _LastTime; }
            set { if (OnPropertyChange("LastTime", value)) _LastTime = value; }
        }

        private Int32 _WidthSuffix = 1;
        /// <summary>
        /// 宽度后缀(px / %)
        /// </summary>
        [Description("宽度后缀(px / %)")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn("WidthSuffix", Description = "宽度后缀(px / %)", DefaultValue = "", Order = 22)]
        public Int32 WidthSuffix
        {
            get { return _WidthSuffix; }
            set { if (OnPropertyChange("WidthSuffix", value)) _WidthSuffix = value; }
        }

        private Int32 _ListColumn = 1;
        /// <summary>
        /// 列表控件分列
        /// </summary>
        [Description("列表控件分列")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn("ListColumn", Description = "列表控件分列", DefaultValue = "", Order = 23)]
        public Int32 ListColumn
        {
            get { return _ListColumn; }
            set { if (OnPropertyChange("ListColumn", value)) _ListColumn = value; }
        }

        private String _Options;
        /// <summary>
        /// 选项集合
        /// </summary>
        [Description("选项集合")]
        [DataObjectField(false, false, true, 1073741823)]
        [BindColumn("Options", Description = "选项集合", DefaultValue = "", Order = 24)]
        public String Options
        {
            get { return _Options; }
            set { if (OnPropertyChange("Options", value)) _Options = value; }
        }



        private Int32 _CreateUser;
        /// <summary>
        /// 创建者
        /// </summary>
        [Description("创建者")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn("CreateUser", Description = "创建者", DefaultValue = "", Order = 25)]
        public Int32 CreateUser
        {
            get { return _CreateUser; }
            set { if (OnPropertyChange("CreateUser", value)) _CreateUser = value; }
        }

        private DateTime _CreateTime;
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        [DataObjectField(false, false, false, 23)]
        [BindColumn("CreateTime", Description = "创建时间", DefaultValue = "", Order = 26)]
        public DateTime CreateTime
        {
            get { return _CreateTime; }
            set { if (OnPropertyChange("CreateTime", value)) _CreateTime = value; }
        }

        private DateTime _StartTime = xUserTime.UtcTime();
        /// <summary>
        /// 开始时间
        /// </summary>
        [Description("开始时间")]
        [DataObjectField(false, false, false, 23)]
        [BindColumn("StartTime", Description = "开始时间", DefaultValue = "getdate()", Order = 27)]
        public DateTime StartTime
        {
            get { return xUserTime.LocalTime(_StartTime); }
            set { if (OnPropertyChange("StartTime", value)) _StartTime = xUserTime.ServerTime(value); }
        }

        private DateTime _EndTime =  xUserTime.UtcTime().AddYears(10);
        /// <summary>
        /// 结束时间
        /// </summary>
        [Description("结束时间")]
        [DataObjectField(false, false, false, 23)]
        [BindColumn("EndTime", Description = "结束时间", DefaultValue = "dateadd(year,(10),getdate())", Order = 28)]
        public DateTime EndTime
        {
            get { return xUserTime.LocalTime(_EndTime); }
            set { if (OnPropertyChange("EndTime", value)) _EndTime = xUserTime.ServerTime(value); }
        }

        private Int32 _Per_AllUsers = 0;
        /// <summary>
        /// 所有用户可见(默认可见0)
        /// </summary>
        [Description("所有用户可见(默认可见0)")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn("Per_AllUsers", Description = "所有用户可见(默认可见0)", DefaultValue = "0", Order = 29)]
        public Int32 Per_AllUsers
        {
            get { return _Per_AllUsers; }
            set { if (OnPropertyChange("Per_AllUsers", value)) _Per_AllUsers = value; }
        }

        private String _Per_Roles = String.Empty;
        /// <summary>
        /// 可见权限角色集合
        /// </summary>
        [Description("可见权限角色集合")]
        [DataObjectField(false, false, true, 1073741823)]
        [BindColumn("Per_Roles", Description = "可见权限角色集合", DefaultValue = "", Order = 30)]
        public String Per_Roles
        {
            get { return _Per_Roles; }
            set { if (OnPropertyChange("Per_Roles", value)) _Per_Roles = value; }
        }

        private Int32 _InputLength = 2000;
        /// <summary>
        /// 输入字符限制
        /// </summary>
        [Description("输入限制")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn("InputLength", Description = "输入限制", DefaultValue = "2000", Order = 31)]
        public Int32 InputLength
        {
            get { return _InputLength; }
            set { if (OnPropertyChange("InputLength", value)) _InputLength = value; }
        }


        private Int32 _EqualsControl = 0;
        /// <summary>
        /// 相等的控件
        /// </summary>
        [Description("相等的控件")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn("EqualsControl", Description = "相等的控件", DefaultValue = "0", Order = 32)]
        public Int32 EqualsControl
        {
            get { return _EqualsControl; }
            set { if (OnPropertyChange("EqualsControl", value)) _EqualsControl = value; }
        }


        private Int32 _AssociatedControl = 0;
        /// <summary>
        /// 相关联控件
        /// </summary>
        [Description("相关联控件")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn("AssociatedControl", Description = "相关联控件", DefaultValue = "0", Order = 33)]
        public Int32 AssociatedControl
        {
            get { return _AssociatedControl; }
            set { if (OnPropertyChange("AssociatedControl", value)) _AssociatedControl = value; }
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
                    case "GroupID": return _GroupID;
                    case "Name": return _Name;
                    case "Alias": return _Alias;
                    case "ToolTip": return _ToolTip;
                    case "Description": return _Description;
                    case "DefaultValue": return _DefaultValue;
                    case "FieldType": return _FieldType;
                    case "Direction": return _Direction;
                    case "Width": return _Width;
                    case "Rows": return _Rows;
                    case "FiledList": return _FiledList;
                    case "Required": return _Required;
                    case "Verification": return _Verification;
                    case "Sort": return _Sort;
                    case "ModuleId": return _ModuleId;
                    case "PortalId": return _PortalId;
                    case "Status": return _Status;
                    case "LastUser": return _LastUser;
                    case "LastIP": return _LastIP;
                    case "LastTime": return _LastTime;
                    case "WidthSuffix": return _WidthSuffix;
                    case "ListColumn": return _ListColumn;
                    case "Options": return _Options;
                    case "CreateUser": return _CreateUser;
                    case "CreateTime": return _CreateTime;
                    case "StartTime": return _StartTime;
                    case "EndTime": return _EndTime;
                    case "Per_AllUsers": return _Per_AllUsers;
                    case "Per_Roles": return _Per_Roles;
                    case "InputLength": return _InputLength;
                    case "EqualsControl": return _EqualsControl;
                    case "AssociatedControl": return _AssociatedControl;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case "ID": _ID = Convert.ToInt32(value); break;
                    case "GroupID": _GroupID = Convert.ToInt32(value); break;
                    case "Name": _Name = Convert.ToString(value); break;
                    case "Alias": _Alias = Convert.ToString(value); break;
                    case "ToolTip": _ToolTip = Convert.ToString(value); break;
                    case "Description": _Description = Convert.ToString(value); break;
                    case "DefaultValue": _DefaultValue = Convert.ToString(value); break;
                    case "FieldType": _FieldType = Convert.ToInt32(value); break;
                    case "Direction": _Direction = Convert.ToInt32(value); break;
                    case "Width": _Width = Convert.ToInt32(value); break;
                    case "Rows": _Rows = Convert.ToInt32(value); break;
                    case "FiledList": _FiledList = Convert.ToString(value); break;
                    case "Required": _Required = Convert.ToInt32(value); break;
                    case "Verification": _Verification = Convert.ToInt32(value); break;
                    case "Sort": _Sort = Convert.ToInt32(value); break;
                    case "ModuleId": _ModuleId = Convert.ToInt32(value); break;
                    case "PortalId": _PortalId = Convert.ToInt32(value); break;
                    case "Status": _Status = Convert.ToInt32(value); break;
                    case "LastUser": _LastUser = Convert.ToInt32(value); break;
                    case "LastIP": _LastIP = Convert.ToString(value); break;
                    case "LastTime": _LastTime = Convert.ToDateTime(value); break;
                    case "WidthSuffix": _WidthSuffix = Convert.ToInt32(value); break;
                    case "ListColumn": _ListColumn = Convert.ToInt32(value); break;
                    case "CreateUser": _CreateUser = Convert.ToInt32(value); break;
                    case "CreateTime": _CreateTime = Convert.ToDateTime(value); break;
                    case "StartTime": _StartTime = Convert.ToDateTime(value); break;
                    case "EndTime": _EndTime = Convert.ToDateTime(value); break;
                    case "Options": _Options = Convert.ToString(value); break;
                    case "Per_AllUsers": _Per_AllUsers = Convert.ToInt32(value); break;
                    case "Per_Roles": _Per_Roles = Convert.ToString(value); break;
                    case "InputLength": _InputLength = Convert.ToInt32(value); break;
                    case "EqualsControl": _EqualsControl = Convert.ToInt32(value); break;
                    case "AssociatedControl": _AssociatedControl = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>
        /// 取得表单字段字段名的快捷方式
        /// </summary>
        public class _
        {
            ///<summary>
            /// 字段编号
            ///</summary>
            public const String ID = "ID";

            ///<summary>
            /// 分组编号
            ///</summary>
            public const String GroupID = "GroupID";

            ///<summary>
            /// 字段名
            ///</summary>
            public const String Name = "Name";

            ///<summary>
            /// 别名
            ///</summary>
            public const String Alias = "Alias";

            ///<summary>
            /// 提示
            ///</summary>
            public const String ToolTip = "ToolTip";

            ///<summary>
            /// 描述
            ///</summary>
            public const String Description = "Description";

            ///<summary>
            /// 默认值
            ///</summary>
            public const String DefaultValue = "DefaultValue";

            ///<summary>
            /// 字段类型
            ///</summary>
            public const String FieldType = "FieldType";

            ///<summary>
            /// 控件布局方向
            ///</summary>
            public const String Direction = "Direction";

            ///<summary>
            /// 宽度
            ///</summary>
            public const String Width = "Width";

            ///<summary>
            /// 行数
            ///</summary>
            public const String Rows = "Rows";

            ///<summary>
            /// 列表字段值
            ///</summary>
            public const String FiledList = "FiledList";

            ///<summary>
            /// 是否必填
            ///</summary>
            public const String Required = "Required";

            ///<summary>
            /// 验证类型
            ///</summary>
            public const String Verification = "Verification";

            ///<summary>
            /// 排序
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

            ///<summary>
            /// 宽度后缀(px / %)
            ///</summary>
            public const String WidthSuffix = "WidthSuffix";

            ///<summary>
            /// 列表控件分列
            ///</summary>
            public const String ListColumn = "ListColumn";

            ///<summary>
            /// 选项集合
            ///</summary>
            public const String Options = "Options";

            ///<summary>
            /// 创建者
            ///</summary>
            public const String CreateUser = "CreateUser";

            ///<summary>
            /// 创建时间
            ///</summary>
            public const String CreateTime = "CreateTime";

            ///<summary>
            /// 开始时间
            ///</summary>
            public const String StartTime = "StartTime";

            ///<summary>
            /// 结束时间
            ///</summary>
            public const String EndTime = "EndTime";

            ///<summary>
            /// 所有用户可见(默认可见0)
            ///</summary>
            public const String Per_AllUsers = "Per_AllUsers";

            ///<summary>
            /// 可见权限角色集合
            ///</summary>
            public const String Per_Roles = "Per_Roles";

            /// <summary>
            /// 输入限制字符数
            /// </summary>
            public const String InputLength = "InputLength";


            ///<summary>
            /// 相等的控件
            ///</summary>
            public const String EqualsControl = "EqualsControl";

            ///<summary>
            /// 相关联控件
            ///</summary>
            public const String AssociatedControl = "AssociatedControl";

        }
        #endregion
    }
}