using System;
using System.Collections.Generic;
using System.Web;
using System.Collections.Specialized;
using System.Reflection;
using System.Threading;

namespace DNNGo.Modules.PowerForms
{
    public class PushForm : WebClientX
    {
        private basePortalModule BPM = new basePortalModule();
        private WebClientX webX = new WebClientX();


        private List<DNNGo_PowerForms_ContentItem> _ContentList = new List<DNNGo_PowerForms_ContentItem>();
        /// <summary>
        /// 提交的表单字段内容
        /// </summary>
        public List<DNNGo_PowerForms_ContentItem> ContentList
        {
            get { return _ContentList; }
            set { _ContentList = value; }
        }

        private DNNGo_PowerForms_Content _SubmitContent = new DNNGo_PowerForms_Content();
        /// <summary>
        /// 提交的表单
        /// </summary>
        public DNNGo_PowerForms_Content SubmitContent
        {
            get { return _SubmitContent; }
            set { _SubmitContent = value; }
        }

        private NameValueCollection _QueryStrings = new NameValueCollection();

        public NameValueCollection QueryStrings
        {
            get { return _QueryStrings; }
            set { _QueryStrings = value; }
        }

        /// <summary>
        /// 请求地址
        /// </summary>
        public String TransferUrl
        {
            get { return BPM.Settings["PowerForms_Push_TransferUrl"] != null && !string.IsNullOrEmpty(BPM.Settings["PowerForms_Push_TransferUrl"].ToString()) ? Convert.ToString(BPM.Settings["PowerForms_Push_TransferUrl"]) : "http://www.dnngo.net/OurModules/PowerForms/FormPush.aspx"; }
        }
            
          

        /// <summary>
        /// 封装构造
        /// </summary>
        public PushForm(basePortalModule _BPM)
        {
            BPM = _BPM;
            webX = new WebClientX(_BPM);
        }

        /// <summary>
        /// 推送数据到目标地址
        /// </summary>
        public void Push()
        {
            //取出设置
            String QueryString = BPM.Settings["PowerForms_Push_QueryString"] != null && !string.IsNullOrEmpty(BPM.Settings["PowerForms_Push_QueryString"].ToString()) ? Convert.ToString(BPM.Settings["PowerForms_Push_QueryString"]) : "";
            Boolean Asynchronous = BPM.Settings["PowerForms_Push_Asynchronous"] != null && !string.IsNullOrEmpty(BPM.Settings["PowerForms_Push_Asynchronous"].ToString()) ? Convert.ToBoolean(BPM.Settings["PowerForms_Push_Asynchronous"]) : true;
            Int32 FormMethod = BPM.Settings["PowerForms_Push_FormMethod"] != null && !string.IsNullOrEmpty(BPM.Settings["PowerForms_Push_FormMethod"].ToString()) ? Convert.ToInt32(BPM.Settings["PowerForms_Push_FormMethod"]) : (Int32)EnumFormMethod.POST;

            if (!String.IsNullOrEmpty(TransferUrl))//推送的目标地址不能为空
            {
                //构造查询的字符串
                QueryStrings = CreateQueryStrings();

                //将值转换到键值对中存储
                if (ContentList != null && ContentList.Count > 0)
                {
                    TemplateFormat xf = new TemplateFormat(BPM);
                    foreach (DNNGo_PowerForms_ContentItem item in ContentList)
                    {
                        if (String.IsNullOrEmpty(QueryStrings.Get(item.FieldName)))//不存在该键值时才能进入
                        {
                            QueryStrings.Add(item.FieldName, xf.ViewContentValue2(item));
                        }
                    }

                }
 
      

                //设置中设置的查询参数
                if (!String.IsNullOrEmpty(QueryString))
                {
                    List<String> ListQuery = WebHelper.GetList(QueryString, "\r\n");
                    foreach (String query in ListQuery)
                    {
                        List<String> nvs = WebHelper.GetList(query, "=");
                        if (nvs != null && nvs.Count == 2 && !String.IsNullOrEmpty(nvs[0]) && !String.IsNullOrEmpty(nvs[1]))
                        {
                            if (String.IsNullOrEmpty(QueryStrings.Get(nvs[0])))//不存在该键值时才能进入
                            {
                                QueryStrings.Add(nvs[0], nvs[1]);
                            }
                        }
                    }
                }


                //将提交的表单实体转换
                Type t = typeof(DNNGo_PowerForms_Content);
                PropertyInfo[] Propertys = t.GetProperties();
                //循环字段列表
                foreach (PropertyInfo Property in Propertys)
                {
                    if (String.IsNullOrEmpty(_QueryStrings.Get(Property.Name)))//不存在该键值时才能进入
                    {
                        if (Property.Name != "Item" && Property.Name != "ContentValue")
                        {
                            object o = Property.GetValue(SubmitContent, null);
                            if (Property.Name == "UserName")
                            {
                                _QueryStrings.Add("DisplayName", o.ToString());
                            }
                            else
                            {
                                _QueryStrings.Add(Property.Name, o.ToString());
                            }
                        }
                    }
                }


                //移除模块编号和页面编号
                if(_QueryStrings[DNNGo_PowerForms_Content._.ModuleId] !=null) _QueryStrings.Remove(DNNGo_PowerForms_Content._.ModuleId);
                if (_QueryStrings[DNNGo_PowerForms_Content._.PortalId] != null) _QueryStrings.Remove(DNNGo_PowerForms_Content._.PortalId);
        





                if (Asynchronous)
                {
                    //异步推送
                    ManagedThreadPool.QueueUserWorkItem(new WaitCallback(ThreadUploadValues), this);
                }
                else
                {
                    //webX.UploadValues(new Uri(TransferUrl), EnumHelper.GetEnumTextVal(FormMethod, typeof(EnumFormMethod)), QueryStrings);
                    PushData();
                }


            }
        }

        public void PushData()
        {
         
            Int32 FormMethod = BPM.Settings["PowerForms_Push_FormMethod"] != null && !string.IsNullOrEmpty(BPM.Settings["PowerForms_Push_FormMethod"].ToString()) ? Convert.ToInt32(BPM.Settings["PowerForms_Push_FormMethod"]) : (Int32)EnumFormMethod.POST;

            try
            {
                if (FormMethod == (Int32)EnumFormMethod.GET)
                {
                    //查询参数通过Get方式发送
                    //webX.QueryString = QueryStrings;
                    //webX.UploadValues(new Uri(CreateQueryUrl(QueryStrings)), EnumHelper.GetEnumTextVal(FormMethod, typeof(EnumFormMethod)), new NameValueCollection());
                   //String s= webX.UploadString(new Uri(CreateQueryUrl(QueryStrings),true), "get", "");
                   
                   webX.DownloadString(new Uri(CreateQueryUrl(QueryStrings), true));
                }
                else
                {
                    webX.UploadValues(new Uri(TransferUrl), "post", QueryStrings);
                }
            }
            catch { }
        }


        /// <summary>
        /// 创建需要提交的记录(主要是提交表单的基础信息)
        /// </summary>
        /// <returns></returns>
        public NameValueCollection CreateQueryStrings()
        {
            NameValueCollection _QueryStrings = new NameValueCollection();

            String AppVerify = BPM.Settings["PowerForms_Push_AppVerify"] != null && !string.IsNullOrEmpty(BPM.Settings["PowerForms_Push_AppVerify"].ToString()) ? Convert.ToString(BPM.Settings["PowerForms_Push_AppVerify"]) : "";
            _QueryStrings.Add("AppVerify", AppVerify);
 
            return _QueryStrings;
        }

        /// <summary>
        /// 创建get查询的URL
        /// </summary>
        /// <param name="__QueryStrings"></param>
        /// <returns></returns>
        public String CreateQueryUrl(NameValueCollection __QueryStrings)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(TransferUrl);
          

            foreach (String key in __QueryStrings.AllKeys)
            {
                sb.Append(sb.ToString().IndexOf("?") >= 0 ? "&" : "?");
                sb.AppendFormat("{0}={1}", key,HttpUtility.UrlEncode( __QueryStrings[key]));
            }
            return sb.ToString();
        }



        public void ThreadUploadValues(object o)
        {
            PushForm push = o as PushForm;
            push.PushData();
        }

    }
}