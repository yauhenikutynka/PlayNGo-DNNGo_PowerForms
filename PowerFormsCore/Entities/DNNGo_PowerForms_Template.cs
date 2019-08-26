using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace DNNGo.Modules.PowerForms
{
	/// <summary>
	/// 模版
	/// </summary>
	[Serializable]
	[DataObject]
	[Description("模版")]
	[BindTable("DNNGo_PowerForms_Template", Description = "模版", ConnName = "SiteSqlServer")]
	public partial class DNNGo_PowerForms_Template : Entity<DNNGo_PowerForms_Template>
	{
		#region 属性
		private Int32 _ID;
		/// <summary>
		/// 模版编号
		/// </summary>
		[Description("模版编号")]
		[DataObjectField(true, true, false, 10)]
		[BindColumn("ID", Description = "模版编号", DefaultValue = "", Order = 1)]
		public Int32 ID
		{
			get { return _ID; }
			set { if (OnPropertyChange("ID", value)) _ID = value; }
		}

		private String _ReceiversSubject = String.Empty;
		/// <summary>
		/// 接收邮件标题
		/// </summary>
		[Description("接收邮件标题")]
		[DataObjectField(false, false, false, 512)]
		[BindColumn("ReceiversSubject", Description = "接收邮件标题", DefaultValue = "", Order = 2)]
		public String ReceiversSubject
		{
			get { return _ReceiversSubject; }
			set { if (OnPropertyChange("ReceiversSubject", value)) _ReceiversSubject = value; }
		}

        private String _ReceiversTemplate = String.Empty;
		/// <summary>
		/// 接收邮件模版
		/// </summary>
		[Description("接收邮件模版")]
		[DataObjectField(false, false, false, 1073741823)]
		[BindColumn("ReceiversTemplate", Description = "接收邮件模版", DefaultValue = "", Order = 3)]
		public String ReceiversTemplate
		{
			get { return _ReceiversTemplate; }
			set { if (OnPropertyChange("ReceiversTemplate", value)) _ReceiversTemplate = value; }
		}

        private String _ReplySubject = String.Empty;
		/// <summary>
		/// 回复邮件标题
		/// </summary>
		[Description("回复邮件标题")]
		[DataObjectField(false, false, false, 512)]
		[BindColumn("ReplySubject", Description = "回复邮件标题", DefaultValue = "", Order = 4)]
		public String ReplySubject
		{
			get { return _ReplySubject; }
			set { if (OnPropertyChange("ReplySubject", value)) _ReplySubject = value; }
		}

        private String _ReplyTemplate = String.Empty;
		/// <summary>
		/// 回复邮件模版
		/// </summary>
		[Description("回复邮件模版")]
		[DataObjectField(false, false, false, 1073741823)]
		[BindColumn("ReplyTemplate", Description = "回复邮件模版", DefaultValue = "", Order = 5)]
		public String ReplyTemplate
		{
			get { return _ReplyTemplate; }
			set { if (OnPropertyChange("ReplyTemplate", value)) _ReplyTemplate = value; }
		}

        private Int32 _ModuleId = 0;
		/// <summary>
		/// 模块编号
		/// </summary>
		[Description("模块编号")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("ModuleId", Description = "模块编号", DefaultValue = "", Order = 6)]
		public Int32 ModuleId
		{
			get { return _ModuleId; }
			set { if (OnPropertyChange("ModuleId", value)) _ModuleId = value; }
		}

        private Int32 _LastUser = 0;
		/// <summary>
		/// 最后更新用户
		/// </summary>
		[Description("最后更新用户")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("LastUser", Description = "最后更新用户", DefaultValue = "", Order = 7)]
		public Int32 LastUser
		{
			get { return _LastUser; }
			set { if (OnPropertyChange("LastUser", value)) _LastUser = value; }
		}

        private String _LastIP = String.Empty;
		/// <summary>
		/// 最后更新IP
		/// </summary>
		[Description("最后更新IP")]
		[DataObjectField(false, false, false, 50)]
		[BindColumn("LastIP", Description = "最后更新IP", DefaultValue = "", Order = 8)]
		public String LastIP
		{
			get { return _LastIP; }
			set { if (OnPropertyChange("LastIP", value)) _LastIP = value; }
		}

        private DateTime _LastTime = xUserTime.UtcTime();
		/// <summary>
		/// 最后更新时间
		/// </summary>
		[Description("最后更新时间")]
		[DataObjectField(false, false, false, 23)]
		[BindColumn("LastTime", Description = "最后更新时间", DefaultValue = "", Order = 9)]
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
					case "ReceiversSubject" : return _ReceiversSubject;
					case "ReceiversTemplate" : return _ReceiversTemplate;
					case "ReplySubject" : return _ReplySubject;
					case "ReplyTemplate" : return _ReplyTemplate;
					case "ModuleId" : return _ModuleId;
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
					case "ReceiversSubject" : _ReceiversSubject = Convert.ToString(value); break;
					case "ReceiversTemplate" : _ReceiversTemplate = Convert.ToString(value); break;
					case "ReplySubject" : _ReplySubject = Convert.ToString(value); break;
					case "ReplyTemplate" : _ReplyTemplate = Convert.ToString(value); break;
					case "ModuleId" : _ModuleId = Convert.ToInt32(value); break;
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
		/// 取得模版字段名的快捷方式
		/// </summary>
		public class _
		{
			///<summary>
			/// 模版编号
			///</summary>
			public const String ID = "ID";

			///<summary>
			/// 接收邮件标题
			///</summary>
			public const String ReceiversSubject = "ReceiversSubject";

			///<summary>
			/// 接收邮件模版
			///</summary>
			public const String ReceiversTemplate = "ReceiversTemplate";

			///<summary>
			/// 回复邮件标题
			///</summary>
			public const String ReplySubject = "ReplySubject";

			///<summary>
			/// 回复邮件模版
			///</summary>
			public const String ReplyTemplate = "ReplyTemplate";

			///<summary>
			/// 模块编号
			///</summary>
			public const String ModuleId = "ModuleId";

			///<summary>
			/// 最后更新用户
			///</summary>
			public const String LastUser = "LastUser";

			///<summary>
			/// 最后更新IP
			///</summary>
			public const String LastIP = "LastIP";

			///<summary>
			/// 最后更新时间
			///</summary>
			public const String LastTime = "LastTime";
		}
		#endregion
	}
}