/*
       Copyright (C) 2008 supesoft.com,All Rights Reserved						    
       File:																		
 				BingZiOnline.cs                                              			*
       Description:																
 				BingZi������
       Author:																													
 				http://www.supesoft.com												
       Finish DateTime:															
 				2008��10��12��														
       History:																	
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace DNNGo.Modules.PowerForms
{
    /// <summary>
    /// BingZi������
    /// </summary>
    public class FrameWorkCache
    {
        private static ICache _ICache = null;

        static FrameWorkCache()
        {
            _ICache = (ICache)Activator.CreateInstance("DNNGo.Modules.PowerForms", "DNNGo.Modules.PowerForms.HttpWebCache").Unwrap();
        }

        /// <summary>
        /// �����û��ӿ�
        /// </summary>
        /// <returns>IBingZiOnlineʵ����</returns>
        public static ICache Instance()
        {
            return _ICache;
        }
    }
}
