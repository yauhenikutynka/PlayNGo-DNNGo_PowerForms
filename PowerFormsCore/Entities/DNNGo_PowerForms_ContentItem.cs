using System;
using System.Collections.Generic;
using System.Web;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace DNNGo.Modules.PowerForms
{

    /// <summary>
    /// 表单数据项
    /// </summary>
    [Serializable]
    [DataObject]
    [Description("表单数据项")]
    public class DNNGo_PowerForms_ContentItem
    {
        private Int32 _FieldID = 0;
        /// <summary>
        /// 字段编号
        /// </summary>
        public Int32 FieldID
        {
            get { return _FieldID; }
            set { _FieldID = value; }
        }



        private String _FieldName = String.Empty;
        /// <summary>
        /// 字段名
        /// </summary>
        public String FieldName
        {
            get { return _FieldName; }
            set { _FieldName = value; }
        }

        private String _FieldAlias = String.Empty;
        /// <summary>
        /// 字段别名
        /// </summary>
        public String FieldAlias
        {
            get { return _FieldAlias; }
            set { _FieldAlias = value; }
        }


        private String _Group = String.Empty;
        /// <summary>
        /// 字段的分组
        /// </summary>
        public String Group
        {
            get { return _Group; }
            set { _Group = value; }
        }
 

        private String _ContentValue = String.Empty;
        /// <summary>
        /// 内容
        /// </summary>
        public String ContentValue
        {
            get { return _ContentValue; }
            set { _ContentValue = value; }
        }


        private Int32 _Sort = 0;
        /// <summary>
        /// 排序
        /// </summary>
        public Int32 Sort
        {
            get { return _Sort; }
            set { _Sort = value; }
        }

        private Boolean _Extra = false;
        /// <summary>
        /// 额外属性
        /// </summary>
        public Boolean Extra
        {
            get { return _Extra; }
            set { _Extra = value; }
        }
        
    }
}