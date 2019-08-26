using System;
using System.Collections.Generic;
using System.Web;
using DotNetNuke.Common;
using System.IO;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
using System.Collections;
using DotNetNuke.Common.Utilities;

namespace DNNGo.Modules.PowerForms
{
    public class ImportExportHelper
    {

        #region "==属性=="
        /// <summary>
        /// 导入时的图片列表
        /// </summary>
        private List<KeyValueEntity> ImportPictureList = new List<KeyValueEntity>();

        /// <summary>
        /// 模块编号
        /// </summary>
        public Int32 ModuleID
        {
            get;
            set;
        }

        /// <summary>
        /// 用户编号
        /// </summary>
        public Int32 UserId
        {
            get;
            set;
        }

        

        private ModuleInfo _moduleInfo = new ModuleInfo();
        /// <summary>
        /// 模块信息
        /// </summary>
        public ModuleInfo ModuleInfo
        {
            get {
                if (!(_moduleInfo != null && _moduleInfo.ModuleID > 0) && ModuleID >0)
                {
                    ModuleController mc= new ModuleController();
                    _moduleInfo = mc.GetModule(ModuleID);
                }
                return _moduleInfo; }
        }

        private PortalInfo _portalInfo = new PortalInfo();
        /// <summary>
        /// 站点信息
        /// </summary>
        public PortalInfo portalInfo
        {
            get
            {
                if (!(_portalInfo != null && _portalInfo.PortalID > 0) && ModuleID > 0)
                {
                    PortalController pc = new PortalController();
                    _portalInfo = pc.GetPortal(ModuleInfo.PortalID);
                    
                }
                return _portalInfo;
            }
        }
        private PortalSettings _PowerForms_PortalSettings = new PortalSettings();
        /// <summary>
        /// 获取站点配置
        /// </summary>
        public PortalSettings PowerForms_PortalSettings
        {
            get
            {
                if (!(_PowerForms_PortalSettings != null && _PowerForms_PortalSettings.PortalId > 0))
                {

                    _PowerForms_PortalSettings = new PortalSettings(portalInfo.PortalID);

                        DotNetNuke.Entities.Portals.PortalAliasController pac = new PortalAliasController();
                        ArrayList PortalAlias = pac.GetPortalAliasArrayByPortalID(portalInfo.PortalID);
                        if (PortalAlias != null && PortalAlias.Count > 0)
                        {
                            _PowerForms_PortalSettings.PortalAlias = (PortalAliasInfo)PortalAlias[0];
                        }
                        else
                        {

                            _PowerForms_PortalSettings.PortalAlias = new PortalAliasInfo();
                            _PowerForms_PortalSettings.PortalAlias.PortalID = portalInfo.PortalID;
                        }
                }
                return _PowerForms_PortalSettings;
            }
        }



        private Hashtable _PowerForms_Settings = new Hashtable();
        /// <summary>
        /// 获取模块配置(可以获取其他模块配置)
        /// </summary>
        public Hashtable PowerForms_Settings
        {
            get
            {
                if (!(_PowerForms_Settings != null && _PowerForms_Settings.Count > 0))
                {
                    _PowerForms_Settings = new ModuleController().GetModule(ModuleID).ModuleSettings;
                }
                return _PowerForms_Settings;
            }
        }

        /// <summary>
        /// 模块路径
        /// </summary>
        public String ModulePath
        {
            get { return "~/DesktopModules/DNNGo_PowerForms/"; }
        }
        


        private List<SettingEntity> _Setting_EffectSettingDB = new List<SettingEntity>();
        /// <summary>
        /// 获取绑定效果设置项
        /// </summary>
        public List<SettingEntity> Setting_EffectSettingDB
        {
            get
            {
                if (!(_Setting_EffectSettingDB != null && _Setting_EffectSettingDB.Count > 0))
                {
                    String EffectSettingDBPath = HttpContext.Current.Server.MapPath(String.Format("{0}Effects/{1}/EffectSetting.xml", ModulePath, Settings_EffectName));
                    if (File.Exists(EffectSettingDBPath))
                    {
                        XmlFormat xf = new XmlFormat(EffectSettingDBPath);
                        _Setting_EffectSettingDB = xf.ToList<SettingEntity>();
                    }
                }
                return _Setting_EffectSettingDB;
            }
        }


        /// <summary>
        /// 获取绑定的效果名称
        /// </summary>
        public String Settings_EffectName
        {
            get { return PowerForms_Settings["PowerForms_EffectName"] != null ? Convert.ToString(PowerForms_Settings["PowerForms_EffectName"]) : "Effect_01_Basic_Normal"; }
        }


        #endregion 


        #region "==公用方法=="

        #endregion

     


        #region "数据转换XML & Entity"




        /// <summary>
        /// 自定义字段转XML实体
        /// </summary>
        /// <param name="fieldItem"></param>
        /// <returns></returns>
        public  FieldEntity EntityToXml(DNNGo_PowerForms_Field fieldItem)
        {
            FieldEntity filexml = new FieldEntity();
            filexml.Name = fieldItem.Name;
            filexml.Alias = fieldItem.Alias;
            filexml.DefaultValue = fieldItem.DefaultValue;
            filexml.Description = fieldItem.Description;
            filexml.Direction = fieldItem.Direction;
            filexml.FieldType = fieldItem.FieldType;
            filexml.FiledList = fieldItem.FiledList;
            filexml.Required = fieldItem.Required;
            filexml.Rows = fieldItem.Rows;
            filexml.Sort = fieldItem.Sort;
            filexml.Status = fieldItem.Status;
            filexml.ToolTip = fieldItem.ToolTip;
            filexml.Verification = fieldItem.Verification;
            filexml.Width = fieldItem.Width;

            filexml.WidthSuffix = fieldItem.WidthSuffix;
            filexml.ListColumn = fieldItem.ListColumn;
            filexml.Options = fieldItem.Options;
            filexml.StartTime = fieldItem.StartTime;
            filexml.EndTime = fieldItem.EndTime;
            filexml.Per_AllUsers = fieldItem.Per_AllUsers;
            filexml.Per_Roles = fieldItem.Per_Roles;


            if (fieldItem.GroupID > 0)
            {
                DNNGo_PowerForms_Group Group = DNNGo_PowerForms_Group.FindByID(fieldItem.GroupID);

                if(Group!= null && Group.ID > 0)
                    filexml.Group = Group.Name;
            }

            return filexml;



        }


        /// <summary>
        /// 自定义字段转XML实体
        /// </summary>
        /// <param name="fieldItem"></param>
        /// <returns></returns>
        public  DNNGo_PowerForms_Field XmlToEntity(FieldEntity fieldItem)
        {
            DNNGo_PowerForms_Field fileEntity = new DNNGo_PowerForms_Field();
            fileEntity.Name = fieldItem.Name;
            fileEntity.Alias = fieldItem.Alias;
            fileEntity.DefaultValue = fieldItem.DefaultValue;
            fileEntity.Description = fieldItem.Description;
            fileEntity.Direction = fieldItem.Direction;
            fileEntity.FieldType = fieldItem.FieldType;
            fileEntity.FiledList = fieldItem.FiledList;
            fileEntity.Required = fieldItem.Required;
            fileEntity.Rows = fieldItem.Rows;
            fileEntity.Sort = fieldItem.Sort;
            fileEntity.Status = fieldItem.Status;
            fileEntity.ToolTip = fieldItem.ToolTip;
            fileEntity.Verification = fieldItem.Verification;
            fileEntity.Width = fieldItem.Width;

            fileEntity.WidthSuffix = fieldItem.WidthSuffix;
            fileEntity.ListColumn = fieldItem.ListColumn;
            fileEntity.Options = fieldItem.Options;
            fileEntity.StartTime = fieldItem.StartTime;
            fileEntity.EndTime = fieldItem.EndTime;
            fileEntity.Per_AllUsers = fieldItem.Per_AllUsers;
            fileEntity.Per_Roles = fieldItem.Per_Roles;



            if (!String.IsNullOrEmpty(fieldItem.Group))
            {


                DNNGo_PowerForms_Group GroupItem = DNNGo_PowerForms_Group.FindByName(fieldItem.Group, ModuleID);
                if (!(GroupItem != null && GroupItem.ID > 0))
                {
                    GroupItem = new DNNGo_PowerForms_Group();

                    GroupItem.Name = fieldItem.Group;

                    GroupItem.Status = fileEntity.Status;
                    GroupItem.Sort = fileEntity.Sort;

                    GroupItem.LastIP = WebHelper.UserHost;
                    GroupItem.LastTime = DateTime.Now;
                    GroupItem.LastUser = UserId;

                    GroupItem.ModuleId = ModuleID;
                    GroupItem.PortalId = portalInfo.PortalID;

                    GroupItem.Insert();
                }

                fileEntity.GroupID = GroupItem.ID;
            }

            fileEntity.ModuleId = ModuleID;
            fileEntity.PortalId = portalInfo.PortalID;

            return fileEntity;



        }

 



        #endregion

        #region "更新模块设置"


        /// <summary>
        /// 更新当前模块的设置
        /// </summary>
        /// <param name="SettingName"></param>
        /// <param name="SettingValue"></param>
        public void UpdateModuleSetting(string SettingName, string SettingValue)
        {
            UpdateModuleSetting(ModuleID, SettingName, SettingValue);
        }


        /// <summary>
        /// 更新模块设置
        /// </summary>
        /// <param name="ModuleId"></param>
        /// <param name="SettingName"></param>
        /// <param name="SettingValue"></param>
        public void UpdateModuleSetting(int ModuleId, string SettingName, string SettingValue)
        {
            ModuleController controller = new ModuleController();

            controller.UpdateModuleSetting(ModuleId, SettingName, SettingValue);
        }

        /// <summary>
        /// 效果参数保存名称格式化
        /// </summary>
        /// <param name="EffectName">效果名</param>
        /// <param name="ThemeName">主题名</param>
        /// <returns></returns>
        public String EffectSettingsFormat(String EffectName, String ThemeName)
        {
            return String.Format("Gallery{0}_{1}", EffectName, ThemeName);
        }

        /// <summary>
        /// 读取XML参数
        /// </summary>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public object ViewXmlSetting(String Name, object DefaultValue)
        {
            String SettingKey = EffectSettingsFormat(Settings_EffectName, Name);
            return PowerForms_Settings[SettingKey] != null ? ConvertTo.FormatValue(PowerForms_Settings[SettingKey].ToString(), DefaultValue.GetType()) : DefaultValue;
        }

        #endregion

    }
}