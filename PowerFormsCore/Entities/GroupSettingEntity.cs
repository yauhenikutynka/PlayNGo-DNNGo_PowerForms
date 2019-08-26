using System;
using System.Collections.Generic;
using System.Web;
using System.ComponentModel;

namespace DNNGo.Modules.PowerForms
{


    /// <summary>
    /// 参数分组的实体
    /// </summary>
    [Serializable]
    [DataObject]
    [Description("参数分组的实体")]
    [XmlEntityAttributes("DNNGo_PowerForms//Groups//Group")]
    public class GroupEntity
    {
        private String _Name = String.Empty;
        /// <summary>
        /// 参数名
        /// </summary>
        public String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
 
 
        private String _Description = String.Empty;
        /// <summary>
        /// 参数描述
        /// </summary>
        public String Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
         


        private String _Categories = "Basic Options";
        /// <summary>
        /// 类别
        /// </summary>
        public String Categories
        {
            get { return _Categories; }
            set { _Categories = value; }
        }



        private String _Layout = "Left";
        /// <summary>
        /// 布局(Left,Right)
        /// </summary>
        public String Layout
        {
            get { return _Layout; }
            set { _Layout = value; }
        }



        private String _Condition = "";
        /// <summary>
        /// 逻辑语句
        /// </summary>
        public String Condition
        {
            get { return _Condition; }
            set { _Condition = value; }
        }


        public GroupEntity Clone()
        {
            return this.MemberwiseClone() as GroupEntity;
        }

    }
}