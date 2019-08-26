using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DNNGo.Modules.PowerForms
{
	/// <summary>
	/// 表单内容
	/// </summary>
	public partial class DNNGo_PowerForms_Content : Entity<DNNGo_PowerForms_Content>
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
		/// 根据主键查询一个表单内容实体对象用于表单编辑
		/// </summary>
		///<param name="__ID">表单编号</param>
		/// <returns></returns>
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static DNNGo_PowerForms_Content FindByKeyForEdit(Int32 __ID)
		{
			DNNGo_PowerForms_Content entity=Find(new String[]{_.ID}, new Object[]{__ID});
			if (entity == null)
			{
				entity = new DNNGo_PowerForms_Content();
			}
			return entity;
		}     

		/// <summary>
		/// 根据表单编号查找
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static DNNGo_PowerForms_Content FindByID(Int32 id)
		{
			return Find(_.ID, id);
			// 实体缓存
			//return Meta.Cache.Entities.Find(_.ID, id);
			// 单对象缓存
			//return Meta.SingleCache[id];
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
		public static List<DNNGo_PowerForms_Content> Search(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
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
        /// 根据模块编号查看
        /// </summary>
        /// <param name="ModuleId"></param>
        /// <returns></returns>
        public static List<DNNGo_PowerForms_Content> FindAllByModuleId(Int32 ModuleId)
        {
            Int32 RecordCount = 0;
            QueryParam qp = new QueryParam();
            qp.Where.Add(new SearchParam(DNNGo_PowerForms_Content._.ModuleId, ModuleId, SearchType.Equal));
            qp.Where.Add(new SearchParam(DNNGo_PowerForms_Content._.Status, (Int32)EnumStatus.Activation, SearchType.Equal));
            return DNNGo_PowerForms_Content.FindAll(qp, out RecordCount);
        }
		#endregion

		#region 扩展操作
		#endregion

		#region 业务
		#endregion
	}
}