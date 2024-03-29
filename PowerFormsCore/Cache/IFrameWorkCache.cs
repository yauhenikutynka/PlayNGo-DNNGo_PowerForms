/************************************************************************************
 *      Copyright (C) 2008 supesoft.com,All Rights Reserved						    *
 *      File:																		*
 *				IBingZiCache.cs                                              			*
 *      Description:																*
 *				 缓存数据接口                 								    *
 *      Author:																		*
 *				Lzppcc														        *
 *				Lzppcc@hotmail.com													*
 *				http://www.supesoft.com												*
 *      Finish DateTime:															*
 *				2008年5月2日														*
 *      History:																	*
 ***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace DNNGo.Modules.PowerForms
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 插入值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="o">值</param>
        void Insert(string key, object o);

        /// <summary>
        /// 移除值
        /// </summary>
        /// <param name="key">key</param>
        void Remove(string key);

        /// <summary>
        /// 根据key获得对应值key值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="o">值</param>
        /// <returns>是否存在key</returns>
        bool TryGetValue(string key, out object o);

        /// <summary>
        /// 检测key是否存在
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>是否存在</returns>
        bool ContainsKey(string key);

        /// <summary>
        /// 获得值
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>返回值</returns>
        object this[string key]
        {
            get;
        }

        /// <summary>
        /// 获得使用缓存总数
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// 获得剩余缓存大小
        /// </summary>
        string Remains
        {
            get;
        }
    }
}
