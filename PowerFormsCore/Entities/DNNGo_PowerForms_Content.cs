using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace DNNGo.Modules.PowerForms
{
	/// <summary>
	/// 表单内容
	/// </summary>
	[Serializable]
	[DataObject]
	[Description("表单内容")]
	[BindTable("DNNGo_PowerForms_Content", Description = "表单内容", ConnName = "SiteSqlServer")]
	public partial class DNNGo_PowerForms_Content : Entity<DNNGo_PowerForms_Content>
	{
		#region 属性
		private Int32 _ID;
		/// <summary>
		/// 表单编号
		/// </summary>
		[Description("表单编号")]
		[DataObjectField(true, true, false, 10)]
		[BindColumn("ID", Description = "表单编号", DefaultValue = "", Order = 1)]
		public Int32 ID
		{
			get { return _ID; }
			set { if (OnPropertyChange("ID", value)) _ID = value; }
		}

        private String _UserName = "Anonymous";
		/// <summary>
		/// 用户名
		/// </summary>
		[Description("用户名")]
		[DataObjectField(false, false, false, 100)]
		[BindColumn("UserName", Description = "用户名", DefaultValue = "", Order = 2)]
		public String UserName
		{
			get { return _UserName; }
			set { if (OnPropertyChange("UserName", value)) _UserName = value; }
		}

        private String _Email = "Anonymous e-mail";
		/// <summary>
		/// 邮箱
		/// </summary>
		[Description("邮箱")]
		[DataObjectField(false, false, false, 256)]
		[BindColumn("Email", Description = "邮箱", DefaultValue = "", Order = 3)]
		public String Email
		{
			get { return _Email; }
			set { if (OnPropertyChange("Email", value)) _Email = value; }
		}

        private String _CultureInfo = System.Globalization.CultureInfo.CurrentCulture.Name;
		/// <summary>
		/// 区域信息
		/// </summary>
		[Description("区域信息")]
		[DataObjectField(false, false, false, 50)]
		[BindColumn("CultureInfo", Description = "区域信息", DefaultValue = "", Order = 4)]
		public String CultureInfo
		{
			get { return _CultureInfo; }
			set { if (OnPropertyChange("CultureInfo", value)) _CultureInfo = value; }
		}

		private String _ContentValue = String.Empty;
		/// <summary>
		/// 收集内容
		/// </summary>
		[Description("收集内容")]
		[DataObjectField(false, false, false, 1073741823)]
		[BindColumn("ContentValue", Description = "收集内容", DefaultValue = "", Order = 5)]
		public String ContentValue
		{
			get { return _ContentValue; }
			set { if (OnPropertyChange("ContentValue", value)) _ContentValue = value; }
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

		private Int32 _PortalId = 0;
		/// <summary>
		/// 站点编号
		/// </summary>
		[Description("站点编号")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("PortalId", Description = "站点编号", DefaultValue = "", Order = 7)]
		public Int32 PortalId
		{
			get { return _PortalId; }
			set { if (OnPropertyChange("PortalId", value)) _PortalId = value; }
		}

		private Int32 _Status = 0;
		/// <summary>
		/// 状态
		/// </summary>
		[Description("状态")]
		[DataObjectField(false, false, false, 3)]
		[BindColumn("Status", Description = "状态", DefaultValue = "", Order = 8)]
		public Int32 Status
		{
			get { return _Status; }
			set { if (OnPropertyChange("Status", value)) _Status = value; }
		}

		private Int32 _LastUser = 0;
		/// <summary>
		/// 更新用户
		/// </summary>
		[Description("更新用户")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("LastUser", Description = "更新用户", DefaultValue = "", Order = 9)]
		public Int32 LastUser
		{
			get { return _LastUser; }
			set { if (OnPropertyChange("LastUser", value)) _LastUser = value; }
		}

		private String _LastIP = String.Empty;
		/// <summary>
		/// 更新IP
		/// </summary>
		[Description("更新IP")]
		[DataObjectField(false, false, false, 50)]
		[BindColumn("LastIP", Description = "更新IP", DefaultValue = "", Order = 10)]
		public String LastIP
		{
			get { return _LastIP; }
			set { if (OnPropertyChange("LastIP", value)) _LastIP = value; }
		}

		private DateTime _LastTime = xUserTime.UtcTime();
		/// <summary>
		/// 更新时间
		/// </summary>
		[Description("更新时间")]
		[DataObjectField(false, false, false, 23)]
		[BindColumn("LastTime", Description = "更新时间", DefaultValue = "", Order = 11)]
		public DateTime LastTime
		{
            get { return xUserTime.LocalTime(_LastTime); }
            set { if (OnPropertyChange("LastTime", value)) _LastTime = xUserTime.ServerTime(value); }
        }



        private String _PaymentStatus = String.Empty;
        /// <summary>
        /// 付款状态
        /// </summary>
        [Description("付款状态")]
        [DataObjectField(false, false, true, 200)]
        [BindColumn("PaymentStatus", Description = "付款状态", DefaultValue = "", Order = 12)]
        public String PaymentStatus
        {
            get { return _PaymentStatus; }
            set { if (OnPropertyChange("PaymentStatus", value)) _PaymentStatus = value; }
        }

        private DateTime _PaymentTime = xUserTime.UtcTime();
        /// <summary>
        /// 付款时间
        /// </summary>
        [Description("付款时间")]
        [DataObjectField(false, false, true, 23)]
        [BindColumn("PaymentTime", Description = "付款时间", DefaultValue = "", Order = 13)]
        public DateTime PaymentTime
        {
            get { return _PaymentTime; }
            set { if (OnPropertyChange("PaymentTime", value)) _PaymentTime = value; }
        }

        private String _PaymentLink = String.Empty;
        /// <summary>
        /// 付款链接
        /// </summary>
        [Description("付款链接")]
        [DataObjectField(false, false, true, 500)]
        [BindColumn("PaymentLink", Description = "付款链接", DefaultValue = "", Order = 14)]
        public String PaymentLink
        {
            get { return _PaymentLink; }
            set { if (OnPropertyChange("PaymentLink", value)) _PaymentLink = value; }
        }

        private String _TransactionID = String.Empty;
        /// <summary>
        /// 付款编号
        /// </summary>
        [Description("付款编号")]
        [DataObjectField(false, false, true, 500)]
        [BindColumn("TransactionID", Description = "付款编号", DefaultValue = "", Order = 15)]
        public String TransactionID
        {
            get { return _TransactionID; }
            set { if (OnPropertyChange("TransactionID", value)) _TransactionID = value; }
        }

        private String _VerifyString = String.Empty;
        /// <summary>
        /// 验证字符串
        /// </summary>
        [Description("验证字符串")]
        [DataObjectField(false, false, true, 30)]
        [BindColumn("VerifyString", Description = "验证字符串", DefaultValue = "", Order = 16)]
        public String VerifyString
        {
            get { return _VerifyString; }
            set { if (OnPropertyChange("VerifyString", value)) _VerifyString = value; }
        }


        private String _FormVersion = String.Empty;
        /// <summary>
        /// 表单版本
        /// </summary>
        [Description("表单版本")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn("FormVersion", Description = "表单版本", DefaultValue = "", Order = 17)]
        public String FormVersion
        {
            get { return _FormVersion; }
            set { if (OnPropertyChange("FormVersion", value)) _FormVersion = value; }
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
					case "UserName" : return _UserName;
					case "Email" : return _Email;
					case "CultureInfo" : return _CultureInfo;
					case "ContentValue" : return _ContentValue;
					case "ModuleId" : return _ModuleId;
					case "PortalId" : return _PortalId;
					case "Status" : return _Status;
					case "LastUser" : return _LastUser;
					case "LastIP" : return _LastIP;
					case "LastTime" : return _LastTime;
                    case "PaymentStatus": return _PaymentStatus;
                    case "PaymentTime": return _PaymentTime;
                    case "PaymentLink": return _PaymentLink;
                    case "TransactionID": return _TransactionID;
                    case "VerifyString": return _VerifyString;
                    case "FormVersion": return _FormVersion;
                    default: return base[name];
				}
			}
			set
			{
				switch (name)
				{
					case "ID" : _ID = Convert.ToInt32(value); break;
					case "UserName" : _UserName = Convert.ToString(value); break;
					case "Email" : _Email = Convert.ToString(value); break;
					case "CultureInfo" : _CultureInfo = Convert.ToString(value); break;
					case "ContentValue" : _ContentValue = Convert.ToString(value); break;
					case "ModuleId" : _ModuleId = Convert.ToInt32(value); break;
					case "PortalId" : _PortalId = Convert.ToInt32(value); break;
					case "Status" : _Status = Convert.ToInt32(value); break;
					case "LastUser" : _LastUser = Convert.ToInt32(value); break;
					case "LastIP" : _LastIP = Convert.ToString(value); break;
					case "LastTime" : _LastTime = Convert.ToDateTime(value); break;
                    case "PaymentStatus": _PaymentStatus = Convert.ToString(value); break;
                    case "PaymentTime": _PaymentTime = Convert.ToDateTime(value); break;
                    case "PaymentLink": _PaymentLink = Convert.ToString(value); break;
                    case "TransactionID": _TransactionID = Convert.ToString(value); break;
                    case "VerifyString": _VerifyString = Convert.ToString(value); break;
                    case "FormVersion": _FormVersion = Convert.ToString(value); break;
                    default: base[name] = value; break;
				}
			}
		}
		#endregion

		#region 字段名
		/// <summary>
		/// 取得表单内容字段名的快捷方式
		/// </summary>
        public class _
        {
            ///<summary>
            /// 表单编号
            ///</summary>
            public const String ID = "ID";

            ///<summary>
            /// 用户名
            ///</summary>
            public const String UserName = "UserName";

            ///<summary>
            /// 邮箱
            ///</summary>
            public const String Email = "Email";

            ///<summary>
            /// 区域信息
            ///</summary>
            public const String CultureInfo = "CultureInfo";

            ///<summary>
            /// 收集内容
            ///</summary>
            public const String ContentValue = "ContentValue";

            ///<summary>
            /// 模块编号
            ///</summary>
            public const String ModuleId = "ModuleId";

            ///<summary>
            /// 站点编号
            ///</summary>
            public const String PortalId = "PortalId";

            ///<summary>
            /// 状态
            ///</summary>
            public const String Status = "Status";

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

            ///<summary>
            /// 付款状态
            ///</summary>
            public const String PaymentStatus = "PaymentStatus";

            ///<summary>
            /// 付款时间
            ///</summary>
            public const String PaymentTime = "PaymentTime";

            ///<summary>
            /// 付款链接
            ///</summary>
            public const String PaymentLink = "PaymentLink";

            ///<summary>
            /// 付款编号
            ///</summary>
            public const String TransactionID = "TransactionID";

            ///<summary>
            /// 验证字符串
            ///</summary>
            public const String VerifyString = "VerifyString";

            ///<summary>
			/// 表单版本
			///</summary>
			public const String FormVersion = "FormVersion";

        }
		#endregion
	}
}