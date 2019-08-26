using System;
using System.Collections.Generic;
using System.Web;

namespace DNNGo.Modules.PowerForms
{


    /// <summary>
    /// 效果设置参数
    /// </summary>
    [XmlEntityAttributes("DNNGo_PowerForms//FieldList//FieldItem")]
    public class FieldEntity
    {
        private String _Name = String.Empty;
        /// <summary>
        /// 字段名
        /// </summary>
        public String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }


        private String _Alias = String.Empty;
        /// <summary>
        /// 别名
        /// </summary>
        public String Alias
        {
            get { return _Alias; }
            set { _Alias = value; }
        }


        private String _ToolTip = String.Empty;
        /// <summary>
        /// 提示
        /// </summary>
        public String ToolTip
        {
            get { return _ToolTip; }
            set { _ToolTip = value; }
        }


        private String _Description = String.Empty;
        /// <summary>
        /// 描述
        /// </summary>
        public String Description
        {
            get { return _Description; }
            set { _Description = value; }
        }


        private String _DefaultValue = String.Empty;
        /// <summary>
        /// 默认值
        /// </summary>
        public String DefaultValue
        {
            get { return _DefaultValue; }
            set { _DefaultValue = value; }
        }


        private Int32 _FieldType = 0;
        /// <summary>
        /// 字段类型
        /// </summary>
        public Int32 FieldType
        {
            get { return _FieldType; }
            set { _FieldType = value; }
        }


        private Int32 _Direction = 0;
        /// <summary>
        /// 控件布局方向
        /// </summary>
        public Int32 Direction
        {
            get { return _Direction; }
            set { _Direction = value; }
        }


        private Int32 _Width = 100;
        /// <summary>
        /// 宽度
        /// </summary>
        public Int32 Width
        {
            get { return _Width; }
            set { _Width = value; }
        }

        private Int32 _WidthSuffix = 1;
        /// <summary>
        /// 宽度后缀
        /// </summary>
        public Int32 WidthSuffix
        {
            get { return _WidthSuffix; }
            set { _WidthSuffix = value; }
        }

        private Int32 _ListColumn = 1;
        /// <summary>
        /// 列表控件分列
        /// </summary>
        public Int32 ListColumn
        {
            get { return _ListColumn; }
            set {  _ListColumn = value; }
        }


        private Int32 _Rows = 1;
        /// <summary>
        /// 行数
        /// </summary>
        public Int32 Rows
        {
            get { return _Rows; }
            set { _Rows = value; }
        }


        private String _FiledList = String.Empty;
        /// <summary>
        /// 列表字段值
        /// </summary>
        public String FiledList
        {
            get { return _FiledList; }
            set { _FiledList = value; }
        }


        private Int32 _Required = 0;
        /// <summary>
        /// 是否必填
        /// </summary>
        public Int32 Required
        {
            get { return _Required; }
            set { _Required = value; }
        }


        private Int32 _Verification = 0;
        /// <summary>
        /// 验证类型
        /// </summary>
        public Int32 Verification
        {
            get { return _Verification; }
            set { _Verification = value; }
        }


        private Int32 _Sort = 99;
        /// <summary>
        /// 排序
        /// </summary>
        public Int32 Sort
        {
            get { return _Sort; }
            set { _Sort = value; }
        }


        private Int32 _Status = 1;
        /// <summary>
        /// 状态
        /// </summary>
        public Int32 Status
        {
            get { return _Status; }
            set { _Status = value; }
        }


        private String _Group = String.Empty;
        /// <summary>
        /// 分组名
        /// </summary>
        public String Group
        {
            get { return _Group; }
            set { _Group = value; }
        }


        private String _Options;
        /// <summary>
        /// 选项集合
        /// </summary>
        public String Options
        {
            get { return _Options; }
            set {  _Options = value; }
        }

        private DateTime _StartTime = DateTime.Now;
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime
        {
            get { return _StartTime; }
            set {  _StartTime = value; }
        }

        private DateTime _EndTime = DateTime.Now.AddYears(10);
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime
        {
            get { return _EndTime; }
            set {  _EndTime = value; }
        }


        private Int32 _Per_AllUsers = 0;
        /// <summary>
        /// 所有用户可见(默认可见0)
        /// </summary>
        public Int32 Per_AllUsers
        {
            get { return _Per_AllUsers; }
            set { _Per_AllUsers = value; }
        }

        private String _Per_Roles = String.Empty;
        /// <summary>
        /// 可见权限角色集合
        /// </summary>
        public String Per_Roles
        {
            get { return _Per_Roles; }
            set { _Per_Roles = value; }
        }


    }
}