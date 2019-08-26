using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace DNNGo.Modules.PowerForms
{
	/// <summary>
	/// 调度任务配置
	/// </summary>
	[Serializable]
	[DataObject]
	[Description("调度任务配置")]
	[BindTable("DNNGo_PowerForms_Scheduler", Description = "调度任务配置", ConnName = "SiteSqlServer")]
	public partial class DNNGo_PowerForms_Scheduler : Entity<DNNGo_PowerForms_Scheduler>
	{
		#region 属性
		private Int32 _ID;
		/// <summary>
		/// 调度编号
		/// </summary>
		[Description("调度编号")]
		[DataObjectField(true, true, false, 10)]
		[BindColumn("ID", Description = "调度编号", DefaultValue = "", Order = 1)]
		public Int32 ID
		{
			get { return _ID; }
			set { if (OnPropertyChange("ID", value)) _ID = value; }
		}

		private String _SenderEmail;
		/// <summary>
		/// 邮件接受者
		/// </summary>
		[Description("邮件接受者")]
		[DataObjectField(false, false, false, 500)]
		[BindColumn("SenderEmail", Description = "邮件接受者", DefaultValue = "", Order = 2)]
		public String SenderEmail
		{
			get { return _SenderEmail; }
			set { if (OnPropertyChange("SenderEmail", value)) _SenderEmail = value; }
		}

        private String _ExcelName = "Bulk_{yyyy}_{mm}_{dd}_{time}_{ModuleID}";
		/// <summary>
		/// 文档命名
		/// </summary>
		[Description("文档命名")]
		[DataObjectField(false, false, false, 500)]
		[BindColumn("ExcelName", Description = "文档命名", DefaultValue = "", Order = 3)]
		public String ExcelName
		{
			get { return _ExcelName; }
			set { if (OnPropertyChange("ExcelName", value)) _ExcelName = value; }
		}

		private Int32 _Enable = 0;
		/// <summary>
		/// 是否启用
		/// </summary>
		[Description("是否启用")]
		[DataObjectField(false, false, false, 3)]
		[BindColumn("Enable", Description = "是否启用", DefaultValue = "", Order = 4)]
		public Int32 Enable
		{
			get { return _Enable; }
			set { if (OnPropertyChange("Enable", value)) _Enable = value; }
		}

		private Int32 _ModuleId;
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

		private Int32 _PortalId;
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

		private Int32 _LastUser;
		/// <summary>
		/// 更新用户
		/// </summary>
		[Description("更新用户")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("LastUser", Description = "更新用户", DefaultValue = "", Order = 7)]
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
		[BindColumn("LastIP", Description = "更新IP", DefaultValue = "", Order = 8)]
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
		[BindColumn("LastTime", Description = "更新时间", DefaultValue = "", Order = 9)]
		public DateTime LastTime
		{
			get { return _LastTime; }
			set { if (OnPropertyChange("LastTime", value)) _LastTime = value; }
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
					case "ID" : return _ID;
					case "SenderEmail" : return _SenderEmail;
					case "ExcelName" : return _ExcelName;
					case "Enable" : return _Enable;
					case "ModuleId" : return _ModuleId;
					case "PortalId" : return _PortalId;
					case "LastUser" : return _LastUser;
					case "LastIP" : return _LastIP;
					case "LastTime" : return _LastTime;
					default: return base[name];
				}
			}
			set
			{
				switch (name)
				{
					case "ID" : _ID = Convert.ToInt32(value); break;
					case "SenderEmail" : _SenderEmail = Convert.ToString(value); break;
					case "ExcelName" : _ExcelName = Convert.ToString(value); break;
					case "Enable" : _Enable = Convert.ToInt32(value); break;
					case "ModuleId" : _ModuleId = Convert.ToInt32(value); break;
					case "PortalId" : _PortalId = Convert.ToInt32(value); break;
					case "LastUser" : _LastUser = Convert.ToInt32(value); break;
					case "LastIP" : _LastIP = Convert.ToString(value); break;
					case "LastTime" : _LastTime = Convert.ToDateTime(value); break;
					default: base[name] = value; break;
				}
			}
		}
		#endregion

		#region 字段名
		/// <summary>
		/// 取得调度任务配置字段名的快捷方式
		/// </summary>
		public class _
		{
			///<summary>
			/// 调度编号
			///</summary>
			public const String ID = "ID";

			///<summary>
			/// 邮件接受者
			///</summary>
			public const String SenderEmail = "SenderEmail";

			///<summary>
			/// 文档命名
			///</summary>
			public const String ExcelName = "ExcelName";

			///<summary>
			/// 是否启用
			///</summary>
			public const String Enable = "Enable";

			///<summary>
			/// 模块编号
			///</summary>
			public const String ModuleId = "ModuleId";

			///<summary>
			/// 站点编号
			///</summary>
			public const String PortalId = "PortalId";

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
		}
		#endregion
	}
}