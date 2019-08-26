using System;
using System.Collections.Generic;
using System.Web;
using DotNetNuke.Entities.Modules;

namespace DNNGo.Modules.PowerForms
{
    /// <summary>
    /// 效果展示数据
    /// </summary>
    [XmlEntityAttributes("DNNGo_PowerForms//EffectDB")]
    public class EffectDB
    {

        private String _Name = String.Empty;
        /// <summary>
        /// 效果名称
        /// </summary>
        public String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }


        private String _Description = String.Empty;
        /// <summary>
        /// 效果描述
        /// </summary>
        public String Description
        {
            get { return _Description; }
            set { _Description = value; }
        }


        private String _Version = "01.00.00";
        /// <summary>
        /// 版本号
        /// </summary>
        public String Version
        {
            get { return _Version; }
            set { _Version = value; }
        }


        private String _Thumbnails = String.Empty;
        /// <summary>
        /// 缩略图
        /// </summary>
        public String Thumbnails
        {
            get { return _Thumbnails; }
            set { _Thumbnails = value; }
        }


        private String _EffectScript = String.Empty;
        /// <summary>
        /// 效果附带脚本
        /// </summary>
        public String EffectScript
        {
            get { return _EffectScript; }
            set { _EffectScript = value; }
        }


        private String _GlobalScript = String.Empty;
        /// <summary>
        /// 全局附带脚本
        /// </summary>
        public String GlobalScript
        {
            get { return _GlobalScript; }
            set { _GlobalScript = value; }
        }

 
        private String _DemoUrl = String.Empty;
        /// <summary>
        /// 演示地址
        /// </summary>
        public String DemoUrl
        {
            get { return _DemoUrl; }
            set { _DemoUrl = value; }
        }

        private Boolean _Group = false;
        /// <summary>
        /// 是否有分组
        /// </summary>
        public Boolean Group
        {
            get { return _Group; }
            set { _Group = value; }
        }

        private Boolean _Responsive = false;
        /// <summary>
        /// 是否响应式
        /// </summary>
        public Boolean Responsive
        {
            get { return _Responsive; }
            set { _Responsive = value; }
        }


        private Boolean _iFrame = false;
        /// <summary>
        /// 是否框架
        /// </summary>
        public Boolean iFrame
        {
            get { return _iFrame; }
            set { _iFrame = value; }
        }


        private Boolean _Custom = false;
        /// <summary>
        /// 自定义模版
        /// </summary>
        public Boolean Custom
        {
            get { return _Custom; }
            set { _Custom = value; }
        }


        private Boolean _IsDetail = true;
        /// <summary>
        /// 是否详情(只在结果模版中有效)
        /// </summary>
        public Boolean IsDetail
        {
            get { return _IsDetail; }
            set { _IsDetail = value; }
        }

        /// <summary>
        /// 输出XML
        /// </summary>
        /// <returns></returns>
        public String ToXml(basePortalModule pmb)
        {
            //读取XML的模版
            XmlFormat xf = new XmlFormat(pmb.MapPath(String.Format("{0}Resource/xml/EffectDB.xml", pmb.ModulePath)));
            //将字段列表转换成XML的实体
            return xf.ToXml<EffectDB>(this);
        }


    }
}