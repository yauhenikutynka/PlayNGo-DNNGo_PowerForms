using System;
using System.Collections.Generic;
using System.Web;

using System.IO;

using System.Text;
using CNVelocity.App;
using CNVelocity.Context;
using Commons.Collections;
using CNVelocity;
using CNVelocity.Runtime;
using DotNetNuke.Entities.Controllers;

namespace DNNGo.Modules.PowerForms
{
    /// <summary>
    /// CNVelocity模板工具类 VelocityHelper
    /// </summary>
    public class VelocityHelper
    {
        private VelocityEngine velocity = null;
        private IContext context = null;
        private basePortalModule bpm = new basePortalModule();
        private EffectDB Theme = new EffectDB();
   

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pmb">集成模块的对象</param>
        public VelocityHelper(basePortalModule _bpm, String _path = "Effect")
        {
            bpm = _bpm;
            Init(_bpm, _path);
        }

        public VelocityHelper(basePortalModule _bpm, EffectDB _Theme, String _path = "Effect")
        {
            Theme = _Theme;
            bpm = _bpm;

            Init(_bpm, _path);
        }
 

        /// <summary>
        /// 无参数构造函数
        /// </summary>
        public VelocityHelper() { }

        /// <summary>
        /// 初始话CNVelocity模块
        /// </summary>
        public void Init(basePortalModule _bpm, String _path = "Effect")
        {
            //创建VelocityEngine实例对象
            velocity = new VelocityEngine();
 
         
            //使用设置初始化VelocityEngine
            ExtendedProperties props = new ExtendedProperties();

            props.AddProperty(RuntimeConstants.RESOURCE_LOADER, "file");

            if (_path != "Effect")
            {

                props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, HttpContext.Current.Server.MapPath(String.Format("{0}{1}s/{2}/", _bpm.ModulePath, _path, _bpm.Settings_ResultName)));
            }
            else
            {
                props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, HttpContext.Current.Server.MapPath(String.Format("{0}{1}s/{2}/", _bpm.ModulePath, _path, _bpm.Settings_EffectName)));
            }
                //props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, Path.GetDirectoryName(HttpContext.Current.Request.PhysicalPath));
            props.AddProperty(RuntimeConstants.INPUT_ENCODING, "utf-8");
            props.AddProperty(RuntimeConstants.OUTPUT_ENCODING, "utf-8");

            //模板的缓存设置
            props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_CACHE, false);              //是否缓存
            props.AddProperty("file.resource.loader.modificationCheckInterval", (Int64)600);    //缓存时间(秒)

            velocity.Init(props);

            //为模板变量赋值
            context = new VelocityContext();
        }

        /// <summary>
        /// 给模板变量赋值
        /// </summary>
        /// <param name="key">模板变量</param>
        /// <param name="value">模板变量值</param>
        public void Put(string key, object value)
        {
            if (context == null)
                context = new VelocityContext();
            context.Put(key, value);
        }

        /// <summary>
        /// 显示模版
        /// </summary>
        /// <param name="templatFileName"></param>
        /// <returns></returns>
        public String Display(String templatFileName)
        {
            //检测是否存在此文件，若不存在将创建
            FileSystemUtils.CreateText(HttpContext.Current.Server.MapPath(String.Format("{0}Effects/{1}/{2}",bpm.ModulePath, bpm.Settings_EffectName, templatFileName)));
  
            //从文件中读取模板
            Template template = velocity.GetTemplate(templatFileName);
            //添加共用变量
             context.Put("Module", bpm); 
             context.Put("ModuleConfiguration", bpm.ModuleConfiguration); 
             context.Put("UserInfo", bpm.UserInfo);
             context.Put("Portal", bpm.PortalSettings);
            context.Put("Host", HostController.Instance.GetSettingsDictionary());

            StringWriter writer = new StringWriter();
             lock (this)
             {
                 //合并模板
                 template.Merge(context, writer);
             }
            return writer.ToString();
        }

    
        /// <summary>
        /// 根据模板生成静态页面
        /// </summary>
        /// <param name="templatFileName"></param>
        /// <param name="htmlpath"></param>
        public void CreateHtml(string templatFileName, string htmlpath)
        {
            //从文件中读取模板
            Template template = velocity.GetTemplate(templatFileName);
            //合并模板
            StringWriter writer = new StringWriter(); 
            template.Merge(context, writer);
            using (StreamWriter write2 = new StreamWriter(HttpContext.Current.Server.MapPath(htmlpath), false, Encoding.UTF8, 200))
            {
                write2.Write(writer);
                write2.Flush();
                write2.Close();
            }

        }

        /// <summary>
        /// 根据模板生成静态页面
        /// </summary>
        /// <param name="templatFileName"></param>
        /// <param name="htmlpath"></param>
        public void CreateJS(string templatFileName, string htmlpath)
        {
            //从文件中读取模板
            Template template = velocity.GetTemplate(templatFileName);
            //合并模板
            StringWriter writer = new StringWriter();
            template.Merge(context, writer);
            using (StreamWriter write2 = new StreamWriter(HttpContext.Current.Server.MapPath(htmlpath), false, Encoding.UTF8, 200))
            {
                write2.Write(writer.ToString());
                write2.Flush();
                write2.Close();
            }

        }
    }

}




