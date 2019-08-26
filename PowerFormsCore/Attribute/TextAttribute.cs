using System;
using System.Collections.Generic;
using System.Web;

namespace DNNGo.Modules.PowerForms
{
    /// <summary>
    /// TextAttribute
    /// </summary>
    public class TextAttribute : Attribute
    {
        string _Text;
        /// <summary>
        /// 显示的文本
        /// </summary>
        public string Text
        {
            get { return _Text; }
            set { _Text = value; }
        }


        string _Description;
        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="text"></param>
        public TextAttribute(string text)
        {
            this._Text = text;
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="text"></param>
        /// <param name="description"></param>
        public TextAttribute(string text, string description)
        {
            this._Text = text;
            this._Description = description;
        }

    }
}