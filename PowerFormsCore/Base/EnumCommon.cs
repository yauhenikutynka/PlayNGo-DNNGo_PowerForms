using System;
using System.Collections.Generic;
using System.Web;

namespace DNNGo.Modules.PowerForms
{


    /// <summary>
    /// 提示类型
    /// </summary>
    public enum EnumTips
    {
        /// <summary>
        /// Success
        /// </summary>
        [Text("Success")]
        Success = 0,
        /// <summary>
        /// Warning
        /// </summary>
        [Text("Warning")]
        Warning = 1,
        /// <summary>
        /// Error
        /// </summary>
        [Text("Error")]
        Error = 2,
        /// <summary>
        /// Alert
        /// </summary>
        [Text("Alert")]
        Alert = 3
    }

    /// <summary>
    /// 显示类型(正常/模块)
    /// </summary>
    public enum EnumViewType
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Text("Normal")]
        Normal = 0,
        /// <summary>
        /// 模块
        /// </summary>
        [Text("Module")]
        Module = 1
    }

    /// <summary>
    /// 状态
    /// </summary>
    public enum EnumStatus
    {
        /// <summary>
        /// 隐藏
        /// </summary>
        [Text("Hide")]
        Hide = 0,
        /// <summary>
        /// 激活
        /// </summary>
        [Text("Active")]
        Activation = 1
    }


    /// <summary>
    /// 移动类型
    /// </summary>
    public enum EnumMoveType
    {
        /// <summary>
        /// Up
        /// </summary>
        [Text("Up")]
        Up,
        /// <summary>
        /// Down
        /// </summary>
        [Text("Down")]
        Down,
        /// <summary>
        /// Top
        /// </summary>
        [Text("Top")]
        Top,
        /// <summary>
        /// Bottom
        /// </summary>
        [Text("Bottom")]
        Bottom,
        /// <summary>
        /// Promote
        /// </summary>
        [Text("Promote")]
        Promote,
        /// <summary>
        /// Demote
        /// </summary>
        [Text("Demote")]
        Demote
    }

    /// <summary>
    /// 
    /// </summary>
    public enum EnumLinkTarget
    {
        /// <summary>
        /// _self
        /// </summary>
        [Text("_self")]
        self = 0,
        /// <summary>
        /// _blank
        /// </summary>
        [Text("_blank")]
        blank = 1
    }

    /// <summary>
    /// 控件类型
    /// </summary>
    public enum EnumControlType
    {
        /// <summary>
        /// TextBox
        /// </summary>
        [Text("TextBox")]
        TextBox = 1,
        /// <summary>
        /// RichTextBox
        /// </summary>
        [Text("RichTextBox")]
        RichTextBox = 2,
        /// <summary>
        /// DropDownList
        /// </summary>
        [Text("DropDownList")]
        DropDownList = 3,
        /// <summary>
        /// DropDownList
        /// </summary>
        [Text("DropDownList Group")]
        DropDownList_Group = 30,
        /// <summary>
        /// ListBox
        /// </summary>
        [Text("ListBox")]
        ListBox = 4,
        /// <summary>
        /// RadioButtonList
        /// </summary>
        [Text("RadioButtonList")]
        RadioButtonList = 5,
        /// <summary>
        /// FileUpload
        /// </summary>
        [Text("FileUpload")]
        FileUpload = 6,
        /// <summary>
        /// Urls
        /// </summary>
        [Text("Urls")]
        Urls = 60,
        /// <summary>
        /// CheckBox
        /// </summary>
        [Text("CheckBox")]
        CheckBox = 7,
        /// <summary>
        /// CheckBoxList
        /// </summary>
        [Text("CheckBoxList")]
        CheckBoxList = 8,
        /// <summary>
        /// DatePicker
        /// </summary>
        [Text("DatePicker")]
        DatePicker = 9,
        /// <summary>
        /// ColorPicker
        /// </summary>
        [Text("ColorPicker")]
        ColorPicker = 90,
        /// <summary>
        /// IconPicker
        /// </summary>
        [Text("IconPicker")]
        IconPicker = 91,
        /// <summary>
        /// Label
        /// </summary>
        [Text("Label")]
        Label = 10
    }


    /// <summary>
    /// 控件类型
    /// </summary>
    public enum EnumViewControlType
    {
        /// <summary>
        /// TextBox
        /// </summary>
        [Text("TextBox")]
        TextBox = 1,
        /// <summary>
        /// RichTextBox
        /// </summary>
        [Text("RichTextBox")]
        RichTextBox = 2,
        /// <summary>
        /// DropDownList
        /// </summary>
        [Text("DropDownList")]
        DropDownList = 3,
        /// <summary>
        /// ListBox
        /// </summary>
        [Text("ListBox")]
        ListBox = 4,
        /// <summary>
        /// RadioButtonList
        /// </summary>
        [Text("RadioButtonList")]
        RadioButtonList = 5,
        /// <summary>
        /// FileUpload
        /// </summary>
        [Text("FileUpload")]
        FileUpload = 6,
        /// <summary>
        /// Multiple Files Upload
        /// </summary>
        [Text("MultipleFilesUpload")]
        MultipleFilesUpload = 61,
        /// <summary>
        /// CheckBox
        /// </summary>
        [Text("CheckBox")]
        CheckBox = 7,
        /// <summary>
        /// CheckBoxList
        /// </summary>
        [Text("CheckBoxList")]
        CheckBoxList = 8,
        /// <summary>
        /// DatePicker
        /// </summary>
        [Text("DatePicker")]
        DatePicker = 9,
        /// <summary>
        /// Label
        /// </summary>
        [Text("Label")]
        Label = 10,
        /// <summary>
        /// Html
        /// </summary>
        [Text("Html")]
        Html = 11,
        /// <summary>
        /// TextBox ( DisplayName )
        /// </summary>
        [Text("TextBox ( DisplayName )")]
        TextBox_DisplayName = 111,
        /// <summary>
        /// TextBox ( Email )
        /// </summary>
        [Text("TextBox ( Email )")]
        TextBox_Email = 112,
        /// <summary>
        /// DropDownList ( Country )
        /// </summary>
        [Text("DropDownList ( Country )")]
        DropDownList_Country = 131,
        /// <summary>
        /// DropDownList ( Region )
        /// </summary>
        [Text("DropDownList ( Region )")]
        DropDownList_Region = 130,
        /// <summary>
        /// DropDownList ( SendEmail ) 可以选择键值对的方式,这个字段的值是会接受到邮件
        /// </summary>
        [Text("DropDownList ( SendEmail )")]
        DropDownList_SendEmail = 132,
        /// <summary>
        /// Confirm
        /// </summary>
        [Text("Confirm")]
        Confirm = 140,
    }


    /// <summary>
    /// 列表控件方向
    /// </summary>
    public enum EnumControlDirection
    {
        
        /// <summary>
        /// 横向
        /// </summary>
        [Text("Horizontal")]
        Horizontal = 1,
      
        /// <summary>
        /// 垂直
        /// </summary>
        [Text("Vertical")]
        Vertical = 0
    }



    /// <summary>
    /// 验证类型
    /// </summary>
    public enum EnumVerification
    {
        /// <summary>
        /// 表示可选项。若不输入，不要求必填，若有输入，则验证其是否符合要求。
        /// </summary>
        [Text("optional")]
        optional = 0,
        /// <summary>
        /// 验证整数
        /// </summary>
        [Text("integer")]
         integer = 1,
        /// <summary>
        /// 验证数字
        /// </summary>
        [Text("number")]
         number = 2,
        /// <summary>
        /// 验证日期，格式为 YYYY/MM/DD、YYYY/M/D、YYYY-MM-DD、YYYY-M-D
        /// </summary>
        [Text("dateFormat")]
         date = 3,
        /// <summary>
        /// 验证 Email 地址
        /// </summary>
        [Text("email")]
         email = 4,
        /// <summary>
        /// 验证电话号码
        /// </summary>
        [Text("phone")]
         phone = 5,
        /// <summary>
        /// 验证 ipv4 地址
        /// </summary>
        [Text("ipv4")]
        ipv4 = 6,
        /// <summary>
        /// 验证 url 地址，需以 http://、https:// 或 ftp:// 开头
        /// </summary>
        [Text("url")]
        url = 7,
        /// <summary>
        /// 只接受填数字和空格
        /// </summary>
        [Text("onlyNumberSp")]
        onlyNumberSp = 8,
        /// <summary>
        /// 只接受填英文字母（大小写）和单引号(')
        /// </summary>
        [Text("onlyLetterSp")]
        onlyLetterSp = 9,
        /// <summary>
        /// 只接受数字和英文字母
        /// </summary>
        [Text("onlyLetterNumber")]
        onlyLetterNumber = 10 

        

    }

    /// <summary>
    /// 跳转类型
    /// </summary>
    public enum EnumRedirectType
    {
        /// <summary>
        /// 结果页面
        /// </summary>
        [Text("Results page")]
        Results = 0,
        /// <summary>
        /// 指定页面
        /// </summary>
        [Text("Specify page")]
        Specify = 1
      }


    /// <summary>
    /// 验证码主题
    /// </summary>
    public enum EnumCaptchaTheme
    {
        /// <summary>
        /// 红色
        /// </summary>
        [Text("default")]
        Default = 1,
        /// <summary>
        /// 白色
        /// </summary>
        [Text("white")]
        white = 2,
        /// <summary>
        /// 黑色
        /// </summary>
        [Text("blackglass")]
        blackglass = 3,
        /// <summary>
        /// 干净
        /// </summary>
        [Text("clean")]
        clean = 4
    }

    /// <summary>
    /// 邮件的认证方式
    /// </summary>
    public enum EnumEmailAuthentication
    {
        /// <summary>
        /// Anonymous
        /// </summary>
        [Text("Anonymous")]
        Anonymous = 0,
        /// <summary>
        /// Basic
        /// </summary>
        [Text("Basic")]
        Basic = 1,
        /// <summary>
        /// NTLM
        /// </summary>
        [Text("NTLM")]
        NTLM = 2
 
    }

    /// <summary>
    /// 宽度后缀(px / %)
    /// </summary>
    public enum EnumWidthSuffix
    {
        /// <summary>
        /// 像素
        /// </summary>
        [Text("px")]
        px = 0,
        /// <summary>
        /// 百分比
        /// </summary>
        [Text("%")]
        Percentage = 1
    }

    /// <summary>
    /// 表单提交的方法
    /// </summary>
    public enum EnumFormMethod
    {
        /// <summary>
        /// post
        /// </summary>
        [Text("post")]
        POST = 0,
        /// <summary>
        /// get
        /// </summary>
        [Text("get")]
        GET = 1
    }


    /// <summary>
    /// 链接控件枚举
    /// </summary>
    public enum EnumUrlControls
    {
        /// <summary>
        /// URL
        /// </summary>
        [Text("URL ( A Link To An External Resource )")]
        Url = 1,
        /// <summary>
        /// 页面
        /// </summary>
        [Text("Page ( A Page On Your Site )")]
        Page = 2,
        /// <summary>
        /// 文件
        /// </summary>
        [Text("Files ( From the media library )")]
        Files = 3

    }




    /// <summary>
    /// 文件类型
    /// </summary>
    public enum EnumFileMate
    {
        /// <summary>
        /// Image
        /// </summary>
        [Text("Image")]
        Image = 0,
        /// <summary>
        /// Video
        /// </summary>
        [Text("Video")]
        Video = 1,
        /// <summary>
        /// Audio
        /// </summary>
        [Text("Audio")]
        Audio = 2,
        /// <summary>
        /// Zip
        /// </summary>
        [Text("Zip")]
        Zip = 3,
        /// <summary>
        /// Doc
        /// </summary>
        [Text("Doc")]
        Doc = 4,
        /// <summary>
        /// Other
        /// </summary>
        [Text("Other")]
        Other = 9,



    }


    /// <summary>
    /// 多媒体文件状态(未审核、正常、删除/回收站)
    /// </summary>
    public enum EnumFileStatus
    {
        /// <summary>
        /// 未处理
        /// </summary>
        [Text("Pending")]
        Pending = 0,

        /// <summary>
        /// 正常
        /// </summary>
        [Text("Approved")]
        Approved = 1,
        /// <summary>
        /// 回收站
        /// </summary>
        [Text("Recycle")]
        Recycle = 2
    }

    /// <summary>
    /// 导出类型枚举
    /// </summary>
    public enum EnumExport
    {

   
        /// <summary>
        /// Excel
        /// </summary>
        [Text("Excel")]
        Excel = 0,
        /// <summary>
        /// CSV
        /// </summary>
        [Text("CSV")]
        CSV = 1,
        /// <summary>
        /// Doc
        /// </summary>
        [Text("Doc")]
        Doc = 2,
        /// <summary>
        /// Html
        /// </summary>
        [Text("Html")]
        Html = 4,
        /// <summary>
        /// Xml
        /// </summary>
        [Text("Xml")]
        Xml = 6,
        /// <summary>
        /// TextFile
        /// </summary>
        [Text("TextFile")]
        TextFile = 8 


 
    }

}