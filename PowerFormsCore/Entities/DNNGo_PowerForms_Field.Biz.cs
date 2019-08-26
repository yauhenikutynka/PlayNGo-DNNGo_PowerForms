using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using DotNetNuke.Entities.Users;

namespace DNNGo.Modules.PowerForms
{
	/// <summary>
	/// 表单字段
	/// </summary>
	public partial class DNNGo_PowerForms_Field : Entity<DNNGo_PowerForms_Field>
	{
		#region 对象操作
		//基类Entity中包含三个对象操作：Insert、Update、Delete
		//你可以重载它们，以改变它们的行为
		//如：
		/*
		/// <summary>
		/// 已重载。把该对象插入到数据库。这里可以做数据插入前的检查
		/// </summary>
		/// <returns>影响的行数</returns>
		public override Int32 Insert()
		{
			return base.Insert();
		}
		 * */
		#endregion
		
		#region 扩展属性
		//TODO: 本类与哪些类有关联，可以在这里放置一个属性，使用延迟加载的方式获取关联对象

		/*
		private Category _Category;
		/// <summary>该商品所对应的类别</summary>
		public Category Category
		{
			get
			{
				if (_Category == null && CategoryID > 0 && !Dirtys.ContainKey("Category"))
				{
					_Category = Category.FindByKey(CategoryID);
					Dirtys.Add("Category", true);
				}
				return _Category;
			}
			set { _Category = value; }
		}
		 * */
		#endregion

		#region 扩展查询


        /// <summary>
        /// 前台查询所有字段的方法
        /// </summary>
        /// <param name="ModuleId"></param>
        /// <returns></returns>
        public static List<DNNGo_PowerForms_Field> FindAllByView(Int32 ModuleId,UserInfo uInfo)
        {
            QueryParam qp = new QueryParam();
            int RecordCount = 0;
            qp.Orderfld = DNNGo_PowerForms_Field._.Sort;
            qp.OrderType = 0;
            qp.Where.Add(new SearchParam(DNNGo_PowerForms_Field._.ModuleId, ModuleId, SearchType.Equal));
            qp.Where.Add(new SearchParam(DNNGo_PowerForms_Field._.Status, (Int32)EnumStatus.Activation, SearchType.Equal));
            qp.Where.Add(new SearchParam(DNNGo_PowerForms_Field._.StartTime, xUserTime.UtcTime(), SearchType.LtEqual));
            qp.Where.Add(new SearchParam(DNNGo_PowerForms_Field._.EndTime, xUserTime.UtcTime(), SearchType.GtEqual));


            //需要根据条件来查找相应的权限
            if (uInfo.UserID > 0)
            {
                if (!uInfo.IsSuperUser)//超级管理员不限制
                {

                    qp.WhereSql.Append(" ( ");
                    //公开的
                    qp.WhereSql.Append(new SearchParam(DNNGo_PowerForms_Field._.Per_AllUsers, 0, SearchType.Equal).ToSql());

                    //有角色的
                    if (uInfo.Roles != null && uInfo.Roles.Length > 0)
                    {
                        qp.WhereSql.Append(" OR ");
                        qp.WhereSql.Append(" ( ");

                        Int32 RoleIndex = 0;
                        foreach (var r in uInfo.Roles)
                        {
                            if (RoleIndex > 0)
                            {
                                qp.WhereSql.Append("OR");
                            }

                            qp.WhereSql.Append(new SearchParam(DNNGo_PowerForms_Field._.Per_Roles, String.Format(",{0},", r), SearchType.Like).ToSql());

                            qp.WhereSql.Append(" OR ");

                            qp.WhereSql.Append(new SearchParam(DNNGo_PowerForms_Field._.Per_Roles, r, SearchType.Like).ToSql());

                            RoleIndex++;
                        }
                        qp.WhereSql.Append(" ) ");
                    }


                    qp.WhereSql.Append(" ) ");
                }
            }
            else
            {
                qp.Where.Add(new SearchParam(DNNGo_PowerForms_Field._.Per_AllUsers, 0, SearchType.Equal));
            }



            return FindAll(qp, out RecordCount);
        }

        /// <summary>
        /// 前台查询所有字段的方法
        /// </summary>
        /// <param name="ModuleId"></param>
        /// <param name="Verification"></param>
        /// <returns></returns>
        public static List<DNNGo_PowerForms_Field> FindAllByView(Int32 ModuleId, EnumVerification Verification)
        {
            QueryParam qp = new QueryParam();
            int RecordCount = 0;
            qp.Orderfld = String.Format("{0} asc,{1}", DNNGo_PowerForms_Field._.Sort, DNNGo_PowerForms_Field._.ID);
            qp.OrderType = 0;
            qp.Where.Add(new SearchParam(DNNGo_PowerForms_Field._.Verification, (Int32)Verification, SearchType.Equal));
            qp.Where.Add(new SearchParam(DNNGo_PowerForms_Field._.ModuleId, ModuleId, SearchType.Equal));
            qp.Where.Add(new SearchParam(DNNGo_PowerForms_Field._.Status, (Int32)EnumStatus.Activation, SearchType.Equal));
            return FindAll(qp, out RecordCount);
        }



        /// <summary>
        /// 根据方案编号查询该方案下所有的字段
        /// </summary>
        /// <param name="ProjectID">方案编号</param>
        /// <returns></returns>
        public static List<DNNGo_PowerForms_Field> FindAllByModuleID(object ModuleID)
        {
            QueryParam qp = new QueryParam();
            int RecordCount = 0;
            qp.Orderfld = String.Format("{0} asc,{1}", DNNGo_PowerForms_Field._.Sort, DNNGo_PowerForms_Field._.ID);
            qp.OrderType = 0;
            qp.Where.Add(new SearchParam(_.ModuleId, ModuleID, SearchType.Equal));
            //  qp.Where.Add(new SearchParam(DNNGo_xForm_ExtendField._.IsDelete.ColumnName, (Int32)EnumIsDelete.Normal, SearchType.Equal));
            return FindAll(qp, out RecordCount);
        }

		/// <summary>
		/// 根据主键查询一个表单字段实体对象用于表单编辑
		/// </summary>
		///<param name="__ID">字段编号</param>
		/// <returns></returns>
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static DNNGo_PowerForms_Field FindByKeyForEdit(Int32 __ID)
		{
			DNNGo_PowerForms_Field entity=Find(new String[]{_.ID}, new Object[]{__ID});
			if (entity == null)
			{
				entity = new DNNGo_PowerForms_Field();
			}
			return entity;
		}     

		/// <summary>
		/// 根据字段编号查找
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static DNNGo_PowerForms_Field FindByID(Int32 id)
		{
			return Find(_.ID, id);
			// 实体缓存
			//return Meta.Cache.Entities.Find(_.ID, id);
			// 单对象缓存
			//return Meta.SingleCache[id];
		}

		/// <summary>
		/// 根据字段名查找
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static DNNGo_PowerForms_Field FindByName(String name)
		{
			return Find(_.Name, name);
			// 实体缓存
			//return Meta.Cache.Entities.Find(_.Name, name);
			// 单对象缓存
			//return Meta.SingleCache[name];
		}
		#endregion

		#region 高级查询
		/// <summary>
		/// 查询满足条件的记录集，分页、排序
		/// </summary>
		/// <param name="key">关键字</param>
		/// <param name="orderClause">排序，不带Order By</param>
		/// <param name="startRowIndex">开始行，0开始</param>
		/// <param name="maximumRows">最大返回行数</param>
		/// <returns>实体集</returns>
		[DataObjectMethod(DataObjectMethodType.Select, true)]
		public static List<DNNGo_PowerForms_Field> Search(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
		{
		    return FindAll(SearchWhere(key), orderClause, null, startRowIndex, maximumRows);
		}

		/// <summary>
		/// 查询满足条件的记录总数，分页和排序无效，带参数是因为ObjectDataSource要求它跟Search统一
		/// </summary>
		/// <param name="key">关键字</param>
		/// <param name="orderClause">排序，不带Order By</param>
		/// <param name="startRowIndex">开始行，0开始</param>
		/// <param name="maximumRows">最大返回行数</param>
		/// <returns>记录数</returns>
		public static Int32 SearchCount(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
		{
		    return FindCount(SearchWhere(key), null, null, 0, 0);
		}

		/// <summary>
		/// 构造搜索条件
		/// </summary>
		/// <param name="key">关键字</param>
		/// <returns></returns>
		private static String SearchWhere(String key)
		{
            if (String.IsNullOrEmpty(key)) return null;
            key = key.Replace("'", "''");
            String[] keys = key.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

		    StringBuilder sb = new StringBuilder();
		    sb.Append("1=1");

            //if (!String.IsNullOrEmpty(name)) sb.AppendFormat(" And {0} like '%{1}%'", _.Name, name.Replace("'", "''"));

            for (int i = 0; i < keys.Length; i++)
            {
                sb.Append(" And ");

                if (keys.Length > 1) sb.Append("(");
                Int32 n = 0;
                foreach (FieldItem item in Meta.Fields)
                {
                    if (item.Property.PropertyType != typeof(String)) continue;
                    // 只要前五项
                    if (++n > 5) break;

                    if (n > 1) sb.Append(" Or ");
                    sb.AppendFormat("{0} like '%{1}%'", item.Name, keys[i]);
                }
                if (keys.Length > 1) sb.Append(")");
            }

            if (sb.Length == "1=1".Length)
                return null;
            else
                return sb.ToString();
		}





        /// <summary>
        /// 根据状态统计数量
        /// </summary>
        /// <param name="ModuleId"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public static Int32 FindCountByStatus(Int32 ModuleId, Int32 Status)
        {
            QueryParam qp = new QueryParam();
            qp.Where.Add(new SearchParam(_.ModuleId, ModuleId, SearchType.Equal));
            if (Status >= 0)
            {
                qp.Where.Add(new SearchParam(_.Status, Status, SearchType.Equal));
            }
            return FindCount(qp);
        }

        /// <summary>
        /// 根据模块编号查看
        /// </summary>
        /// <param name="ModuleId"></param>
        /// <returns></returns>
        public static List<DNNGo_PowerForms_Field> FindAllByModuleId(Int32 ModuleId)
        {
            QueryParam qp = new QueryParam();
            Int32 RecordCount = 0;
            qp.Orderfld = DNNGo_PowerForms_Field._.Sort;
            qp.OrderType = 0;
            qp.Where.Add(new SearchParam(DNNGo_PowerForms_Field._.ModuleId, ModuleId, SearchType.Equal));
            qp.Where.Add(new SearchParam(DNNGo_PowerForms_Field._.Status, (Int32)EnumStatus.Activation, SearchType.Equal));
            return DNNGo_PowerForms_Field.FindAll(qp, out RecordCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ModuleId"></param>
        /// <param name="uInfo"></param>
        /// <returns></returns>
        public Boolean IncludeMultipleFileUpload(Int32 ModuleId, UserInfo uInfo)
        {
            return FindAllByView(ModuleId, uInfo).Exists(r => r.FieldType == (Int32)EnumViewControlType.MultipleFilesUpload);
        }
   


        #endregion

        #region 扩展操作

        /// <summary>
        /// 移动字段
        /// </summary>
        /// <param name="objTab">待移动的字段</param>
        /// <param name="type">移动类型</param>
        /// <param name="ProjectID"></param>
        public static void MoveField(DNNGo_PowerForms_Field objTab, EnumMoveType type, object ModuleID)
        {
            List<DNNGo_PowerForms_Field> siblingTabs = FindAllByModuleID(ModuleID);
            int siblingCount = siblingTabs.Count;
            int tabIndex = 0;
            UpdateTabOrder(siblingTabs, 2);
            switch (type)
            {
                case EnumMoveType.Up:
                    tabIndex = GetIndexOfTab(objTab, siblingTabs);
                    if (tabIndex > 0)
                    {
                        DNNGo_PowerForms_Field swapTab = siblingTabs[tabIndex - 1];
                        SwapAdjacentTabs(objTab, swapTab);
                    }
                    break;
                case EnumMoveType.Down:
                    tabIndex = GetIndexOfTab(objTab, siblingTabs);
                    if (tabIndex < siblingCount - 1)
                    {
                        DNNGo_PowerForms_Field swapTab = siblingTabs[tabIndex + 1];
                        SwapAdjacentTabs(swapTab, objTab);
                    }
                    break;
            }

        }

        private static void SwapAdjacentTabs(DNNGo_PowerForms_Field firstTab, DNNGo_PowerForms_Field secondTab)
        {
            firstTab.Sort -= 2;
            firstTab.Update();
            secondTab.Sort += 2;
            secondTab.Update();
        }


        private static void UpdateTabOrder(List<DNNGo_PowerForms_Field> tabs, int increment)
        {
            int tabOrder = 1;
            for (int index = 0; index <= tabs.Count - 1; index++)
            {
                DNNGo_PowerForms_Field objTab = tabs[index];
                objTab.Sort = tabOrder;
                objTab.Update();
                tabOrder += increment;
            }
        }


        private static void UpdateTabOrder(List<DNNGo_PowerForms_Field> tabs, int startIndex, int endIndex, int increment)
        {
            for (int index = startIndex; index <= endIndex; index++)
            {
                DNNGo_PowerForms_Field objTab = tabs[index];
                objTab.Sort += increment;
                objTab.Update();
            }
        }

        private static int GetIndexOfTab(DNNGo_PowerForms_Field objTab, List<DNNGo_PowerForms_Field> tabs)
        {
            int tabIndex = -1;// Null.NullInteger;
            for (int index = 0; index <= tabs.Count - 1; index++)
            {
                if (tabs[index].ID == objTab.ID)
                {
                    tabIndex = index;
                    break;
                }
            }
            return tabIndex;
        }
        #endregion

		#region 业务
		#endregion

        #region 默认字段的增加

        /// <summary>
        /// 安装字段
        /// </summary>
        /// <param name="FieldName"></param>
        /// <param name="bpm"></param>
        public static Int32 InstallField(String FieldName, basePortalModule bpm)
        {
            DNNGo_PowerForms_Field fieldItem = new DNNGo_PowerForms_Field();
            fieldItem.Name = fieldItem.Alias = fieldItem.ToolTip = FieldName;

            if (FieldName.IndexOf("Name", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                fieldItem.FieldType = (Int32)EnumViewControlType.TextBox_DisplayName;
                fieldItem.Required = 1;
            }
            else if (FieldName.IndexOf("Email", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                fieldItem.FieldType = (Int32)EnumViewControlType.TextBox_Email;
                fieldItem.Verification = (Int32)EnumVerification.email;
                fieldItem.Required = 1;
            }
            else if (FieldName.IndexOf("Messages", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                fieldItem.FieldType = (Int32)EnumViewControlType.TextBox;
                fieldItem.Rows = 4;
            }
 

            QueryParam qp = new QueryParam();
            qp.Where.Add(new SearchParam(DNNGo_PowerForms_Field._.ModuleId, bpm.ModuleId, SearchType.Equal));
            qp.Where.Add(new SearchParam(DNNGo_PowerForms_Field._.Name, fieldItem.Name, SearchType.Equal));
            if (DNNGo_PowerForms_Field.FindCount(qp) == 0)
            {
                fieldItem.ModuleId = bpm.ModuleId;
                fieldItem.PortalId = bpm.PortalId;
 
                fieldItem.Sort = DNNGo_PowerForms_Field.FindCount(DNNGo_PowerForms_Field._.ModuleId, bpm.ModuleId) + 1;
            
                fieldItem.Status = 1;

                fieldItem.LastTime = xUserTime.UtcTime();
                fieldItem.LastUser = bpm.UserId;
                fieldItem.LastIP = WebHelper.UserHost;
                return fieldItem.Insert();
 
            }
            return 0;
        }


        




        #endregion


    }
}