/************************************************************************************
 *      Copyright (C) 2008 supesoft.com,All Rights Reserved						    *
 *      File:																		*
 *				HttpWebCache.cs                                              			*
 *      Description:																*
 *				 Asp.Net Web缓存                 								    *
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
using System.Web;
using System.Web.Caching;
namespace DNNGo.Modules.PowerForms
{
    /// <summary>
    /// Asp.Net Web缓存
    /// </summary>
    public class HttpWebCache : ICache
    {
        /// <summary>
        /// WebCache
        /// </summary>
        private Cache _Cache = HttpRuntime.Cache;

        /// <summary>
        /// 插入值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="o">值</param>
        public void Insert(string key, object o)
        {
            _Cache.Insert(key, o);
        }

        /// <summary>
        /// 移除值
        /// </summary>
        /// <param name="key">key</param>
        public void Remove(string key)
        {
            _Cache.Remove(key);
        }

        /// <summary>
        /// 获得值
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>返回值</returns>
        public object this[string key]
        {
            get {
                return _Cache[key];
            }
        }

        /// <summary>
        /// 获得使用缓存总数
        /// </summary>
        public int Count
        {
            get {
                return _Cache.Count;
            }
        }

        /// <summary>
        /// 获得剩余缓存大小
        /// </summary>
        public string Remains
        {
            get {
                return string.Format("{0}M",_Cache.EffectivePrivateBytesLimit / 1024 / 1024);
            }
        }

        /// <summary>
        /// 根据key获得值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool TryGetValue(string key, out object o)
        { 
            o=_Cache[key];
            return ContainsKey(key);
        }

        /// <summary>
        /// 检测是否存在当前key
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>存在/不存在</returns>
        public bool ContainsKey(string key)
        {
            if (_Cache[key] == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
