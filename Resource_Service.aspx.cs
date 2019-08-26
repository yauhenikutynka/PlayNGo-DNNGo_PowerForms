
using System;
using System.Web;
using System.Collections.Generic;
using DotNetNuke.Entities.Users;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using System.IO;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Xml;
using DotNetNuke.Common;
using System.Web.Script.Serialization;
using DotNetNuke.Common.Lists;

namespace DNNGo.Modules.PowerForms
{
    /// <summary>
    /// 资源文件(主要用于一些请求的服务)
    /// 1.文件上传
    /// </summary>
    public partial class Resource_Service : BasePage
    {

        #region "属性"
        /// <summary>
        /// 功能
        /// 文件上传 FileUpload
        /// </summary>
        private String Token = WebHelper.GetStringParam(HttpContext.Current.Request, "Token", "").ToLower();
 

 

      


 

        #endregion


        /// <summary>
        /// Page_Init 主要用于权限的验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Page_Init(System.Object sender, System.EventArgs e)
        {
            //调用基类Page_Init，主要用于权限验证
            if (!String.IsNullOrEmpty(Token))
            {
                switch (Token.ToLower())
                {
                    case "picturelist": base.Page_Init(sender, e); break;
                    case "pictureitem": base.Page_Init(sender, e); break;
                    default: break;
                }
            }
        }

        /// <summary>
        /// 页面加载
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
           

            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Token))
                {

                    if (Token.ToLower() == "picturelist")
                    {
                        PushPictureList();
                    }
                    else if (Token.ToLower() == "pictureitem")
                    {
                        PushPictureItem();
                    }
                    else if (Token.ToLower() == "multiplefiles")
                    {
                        SaveMultiplefiles();
                    }
                    else if (Token.ToLower() == "country.region")
                    {
                        PushCountryRegion();
                    }
                    else if (Token.ToLower() == "downloadformfile")
                    {
                        DownLoadFormFile();
                    }
              





                }
            }
            
        }


 

 

        #region "Urls控件用的方法"



        /// <summary>
        /// 推送图片列表数据
        /// </summary>
        public void PushPictureList()
        {
            QueryParam qp = new QueryParam();
            qp.Orderfld = DNNGo_PowerForms_Files._.ID;

            qp.PageIndex = WebHelper.GetIntParam(Request, "PageIndex", 1);
            qp.PageSize = WebHelper.GetIntParam(Request, "PageSize", Int32.MaxValue);
            qp.Where.Add(new SearchParam(DNNGo_PowerForms_Files._.Status, (Int32)EnumStatus.Activation, SearchType.Equal));


            int RecordCount = 0;
            List<DNNGo_PowerForms_Files> fileList = DNNGo_PowerForms_Files.FindAll(qp, out RecordCount);

            Dictionary<String, Object> jsonLayers = new Dictionary<string, Object>();

            TemplateFormat xf = new TemplateFormat();

            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();

            foreach (var fileItem in fileList)
            {
                int index = fileList.IndexOf(fileItem); //index 为索引值

                Dictionary<String, Object> jsonLayer = new Dictionary<String, Object>();

                jsonLayer.Add("Pages", qp.Pages);


                jsonLayer.Add("ID", fileItem.ID);

                jsonLayer.Add("CreateTime", fileItem.LastTime);

                jsonLayer.Add("Name", WebHelper.leftx(fileItem.Name, 20, "..."));
                jsonLayer.Add("Extension", fileItem.FileExtension);


                String ThumbnailUrl = ViewLinkUrl(String.Format("MediaID={0}", fileItem.ID));
                jsonLayer.Add("ThumbnailUrl", ThumbnailUrl);
                jsonLayer.Add("FileUrl", GetPhotoPath(fileItem.FilePath));

                jsonLayer.Add("Thumbnail", String.Format("<img style=\"border-width:0px; max-height:60px;max-width:80px;\"  src=\"{0}\"  /> ", ThumbnailUrl));


                jsonLayer.Add("Json", jsSerializer.Serialize(jsonLayer));

                jsonLayers.Add(index.ToString(), jsonLayer);

            }

            //转换数据为json

            Response.Clear();
            Response.Write(jsSerializer.Serialize(jsonLayers));
            Response.End();
        }


        /// <summary>
        /// 推送图片列表数据
        /// </summary>
        public void PushPictureItem()
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            Dictionary<String, Object> jsonLayer = new Dictionary<String, Object>();

            Int32 MediaID = WebHelper.GetIntParam(Request, "MediaID", 0);
            if (MediaID > 0)
            {

                DNNGo_PowerForms_Files PictureItem = DNNGo_PowerForms_Files.FindByKeyForEdit(MediaID);
                if (PictureItem != null && PictureItem.ID > 0)
                {
                    jsonLayer.Add("ID", PictureItem.ID);
                    jsonLayer.Add("CreateTime", PictureItem.LastTime);
                    jsonLayer.Add("Name", PictureItem.Name);
                    jsonLayer.Add("Extension", PictureItem.FileExtension);
                    String ThumbnailUrl = ViewLinkUrl(String.Format("MediaID={0}", PictureItem.ID));
                    jsonLayer.Add("ThumbnailUrl", ThumbnailUrl);
                    jsonLayer.Add("FileUrl", GetPhotoPath(PictureItem.FilePath));


                    jsonLayer.Add("Thumbnail", String.Format("<img style=\"border-width:0px; max-height:60px;max-width:80px;\"  src=\"{0}\"  /> ", ThumbnailUrl));
                }

            }

            //转换数据为json
            Response.Clear();
            Response.Write(jsSerializer.Serialize(jsonLayer));
            Response.End();
        }

        /// <summary>
        /// 显示URL控件存放的值
        /// </summary>
        /// <param name="UrlValue"></param>
        /// <returns></returns>
        public String ViewLinkUrl(String UrlValue)
        {
            return ViewLinkUrl(UrlValue, true);
        }

        /// <summary>
        /// 显示URL控件存放的值
       /// </summary>
       /// <param name="UrlValue"></param>
       /// <param name="IsPhotoExtension">是否显示扩展名图片</param>
       /// <returns></returns>
        public String ViewLinkUrl(String UrlValue, Boolean IsPhotoExtension)
        {
            String DefaultValue = String.Empty;
            if (!String.IsNullOrEmpty(UrlValue) && UrlValue != "0")
            {
                if (UrlValue.IndexOf("FileID=", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    int FileID = 0;
                    if (int.TryParse(UrlValue.Replace("FileID=", ""), out FileID) && FileID > 0)
                    {
                        var fi = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(FileID);
                        if (fi != null && fi.FileId > 0)
                        {
                            DefaultValue = string.Format("{0}{1}{2}", PortalSettings.HomeDirectory, fi.Folder, Server.UrlPathEncode(fi.FileName));
                        }
                    }
                }
                else if (UrlValue.IndexOf("MediaID=", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    DefaultValue = String.Format("{0}Resource/images/no_image.png", ModulePath);

                    int MediaID = 0;
                    if (int.TryParse(UrlValue.Replace("MediaID=", ""), out MediaID) && MediaID > 0)
                    {
                        DNNGo_PowerForms_Files Multimedia = DNNGo_PowerForms_Files.FindByID(MediaID);
                        if (Multimedia != null && Multimedia.ID > 0)
                        {
                            if (IsPhotoExtension)
                            {
                                DefaultValue = Server.UrlPathEncode(GetPhotoExtension(Multimedia.FileExtension, Multimedia.FilePath));// String.Format("{0}{1}", bpm.PowerForms_PortalSettings.HomeDirectory, Multimedia.FilePath);
                            }
                            else
                            {
                                DefaultValue = Server.UrlPathEncode(GetPhotoPath( Multimedia.FilePath));
                            }
                        }
                    }
                }
                else if (UrlValue.IndexOf("TabID=", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {

                    DefaultValue = Globals.NavigateURL(Convert.ToInt32(UrlValue.Replace("TabID=", "")), false, PortalSettings, Null.NullString, "", "");

                }
                else
                {
                    DefaultValue = UrlValue;
                }
            }
            return DefaultValue;

        }

        #endregion


        #region "保存多文件上传的方法"
        /// <summary>
        /// 多文件上传之暂存办法
        /// </summary>
        public void SaveMultiplefiles()
        {
            Dictionary<String, Object> jsonFils = new Dictionary<string, Object>();

            String ControlName = WebHelper.GetStringParam(Request, "ControlName", "");
            if (!String.IsNullOrEmpty(ControlName))
            {
                if (Request.Files.Count > 0 && Request.Files[ControlName] != null)
                {
                    HttpPostedFile Httpfiles = Request.Files[ControlName];
                    if (Httpfiles != null && Httpfiles.ContentLength > 0)
                    {

                        //To verify that if the suffix name of the uploaded files meets the DNN HOST requirements
                        bool retValue = FileSystemUtils.CheckValidFileName(Httpfiles.FileName);
                        if (retValue)
                        {
                            //构造文件的存放目录
                            String FileName = Httpfiles.FileName;
                            String FileWebPath = String.Format("{0}PowerForms/Multiplefiles/{1}/{2}/{3}", PortalSettings.HomeDirectory, ModuleId, DateTime.Now.ToString("yyyyMMdd"), Httpfiles.FileName);
                            String FileServerPath = MapPath(FileWebPath);
                            String Extension = Path.GetExtension(FileServerPath);
                            String leftName = FileName.Replace("." + Extension, "");
                            //构造文件对象
                            FileInfo Savefile = new FileInfo(FileServerPath);
                            //文件夹构造
                            if (!Savefile.Directory.Exists)
                            {
                                Savefile.Directory.Create();
                            }
                            //文件是否存在||存在的话需要重新构造名称filename_2.txt
                            Int32 FileIndex = 2;
                            while (Savefile.Exists)
                            {
                                FileWebPath = String.Format("{0}PowerForms/Multiplefiles/{1}/{2}/{3}_{4}.{5}", PortalSettings.HomeDirectory, ModuleId, DateTime.Now.ToString("yyyyMMdd"), leftName, FileIndex, Extension);
                                FileServerPath = MapPath(FileWebPath);
                                Savefile = new FileInfo(FileServerPath);
                                FileIndex++;
                            }



                            //构造指定存储路径
                            Httpfiles.SaveAs(Savefile.FullName);
                        }

                    }
                    else
                    {
                        //文件大小为空或者是0
                    }


                }
                else
                {
                    //没有上传文件或
                }
                
            }
            else
            {
                //控件没传过来
            }

            //转换数据为json
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            Response.Clear();
            Response.Write(jsSerializer.Serialize(jsonFils));
            Response.End();

        }


        #endregion


        #region "推送州/区域的数据"
        /// <summary>
        /// 推送州/区域的数据
        /// </summary>
        public void PushCountryRegion()
        {

            Dictionary<String, Object> jsonRegions = new Dictionary<string, Object>();

            String CountryName = WebHelper.GetStringParam(Request, "Country", "");
            if (!String.IsNullOrEmpty(CountryName))
            {

                ListController listController = new ListController();

                //ListEntryInfo thisCountry = new ListEntryInfo();

                //var Countrys= listController.GetListEntryInfoItems("Country");
        
                //foreach (ListEntryInfo Country in Countrys)
                //{
                //    if (!String.IsNullOrEmpty(Country.Value) && CountryName == Country.Value)
                //    {
                //        thisCountry = Country;
                //        break;
                //    }


                //}



                //if (thisCountry != null && thisCountry.EntryID > 0)
                //{

                    //var entryCollection = listController.GetListEntryInfoItems("Region", String.Format("Country.{0}", thisCountry.Value));
                    var entryCollection = listController.GetListEntryInfoItems("Region", String.Format("Country.{0}", CountryName));
                    foreach (ListEntryInfo Country in entryCollection)
                    {
                        if (!String.IsNullOrEmpty(Country.Text))
                        {
                            Dictionary<String, Object> jsonRegion = new Dictionary<string, Object>();
                            jsonRegion.Add("Text", Country.Text);
                            jsonRegions.Add(Country.EntryID.ToString(), jsonRegion);
                        }
                    }

                //}
            }

            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            Response.Clear();
            Response.Write(jsSerializer.Serialize(jsonRegions));
            Response.End();
        }


        #endregion


        #region "获取加密的下载地址"

        /// <summary>
        /// 下载表单文件
        /// </summary>
        public void DownLoadFormFile()
        {

           bool  LoginUserDownload = ViewSettingT<bool>("PowerForms_LoginUserDownload", false);
            if ((LoginUserDownload && UserInfo.UserID > 0) || !LoginUserDownload)
            {
                String filepath = WebHelper.GetStringParam(Request, "file", "");
                if (!String.IsNullOrEmpty(filepath))
                {
                    try
                    {
                        filepath =   CryptionHelper.DecryptString(CryptionHelper.Base64Decode(filepath));
                        if (!String.IsNullOrEmpty(filepath))
                        {
                            String FileUrl = MapPath(String.Format("~/Portals/{0}/PowerForms/{1}/{2}", PortalId, ModuleId, filepath));
                            if (File.Exists(FileUrl))
                            {
                                Response.Write(" <script>window.opener=null;window.close(); </script>");
                                FileSystemUtils.DownloadFile(FileUrl,Path.GetFileName(FileUrl));
                            }
                        }
                    }
                    catch
                    { }

                }

            }
            else
            {


                String file = WebHelper.GetStringParam(Request, "file", "");
                String RedirectFile = String.Format("{0}Resource_Service.aspx?Token=downloadformfile&PortalId={1}&ModuleId={2}&file={3}", ModulePath, PortalId, ModuleId, Server.UrlEncode(file));

                Response.Redirect(Globals.NavigateURL(PortalSettings.LoginTabId, "Login", "returnurl=" + Server.UrlEncode(RedirectFile)), false);

                //String file = WebHelper.GetStringParam(Request, "file", "");

                //String RedirectFile = Globals.NavigateURL("", "a=b","Token=abc","bb=cc", "PortalId="+ PortalId.ToString(), "TabId="+ TabId.ToString(), "ModuleId="+ ModuleId.ToString(), "file="+ Server.UrlEncode(file));//    String.Format("{0}Resource_Service.aspx?Token=RedirectFormFile&PortalId={1}&ModuleId={2}&file={3}", ModulePath, PortalId, ModuleId, Server.UrlEncode(file));

                //Response.Redirect(Globals.NavigateURL(PortalSettings.LoginTabId, "Login", "returnurl=" + Server.UrlEncode(RedirectFile)),false);
            }

           


        }


     




        #endregion

    }
}