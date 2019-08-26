/*
       Copyright (C) 2008 supesoft.com,All Rights Reserved						    
       File:																		
 				BingZiOnline.cs                                              			*
       Description:																
 				BingZi缓存类
       Author:																													
 				http://www.supesoft.com												
       Finish DateTime:															
 				2008年10月12日														
       History:																	
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace DNNGo.Modules.PowerForms
{
    /// <summary>
    /// BingZi缓存类
    /// </summary>
    public class FrameWorkCache
    {
        private static ICache _ICache = null;

        static FrameWorkCache()
        {
            _ICache = (ICache)Activator.CreateInstance("DNNGo.Modules.PowerForms", "DNNGo.Modules.PowerForms.HttpWebCache").Unwrap();
        }

        /// <summary>
        /// 在线用户接口
        /// </summary>
        /// <returns>IBingZiOnline实现类</returns>
        public static ICache Instance()
        {
            return _ICache;
        }
    }
}
