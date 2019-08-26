using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections;
using DotNetNuke.Entities.Users;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Tabs;
using System.Drawing;
using DotNetNuke.Entities.Modules;

namespace DNNGo.Modules.PowerForms
{
    public class Resource_TempSaves : IHttpHandler
    {
		private readonly JavaScriptSerializer js = new JavaScriptSerializer();


        #region "获取DNN对象"

        public Int32 PortalId = WebHelper.GetIntParam(HttpContext.Current.Request, "PortalId", 0);
        /// <summary>
        /// 模块编号
        /// </summary>
        public Int32 ModuleId = WebHelper.GetIntParam(HttpContext.Current.Request, "ModuleId", 0);
        /// <summary>
        /// 页面编号
        /// </summary>
        public Int32 TabId = WebHelper.GetIntParam(HttpContext.Current.Request, "TabId", 0);

        /// <summary>
        /// 路径
        /// </summary>
        public String ModulePath = WebHelper.GetStringParam(HttpContext.Current.Request, "ModulePath", "");


        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo UserInfo
        {
            get { return UserController.GetCurrentUserInfo(); }
        }



        /// <summary>
        /// 是否具有该模块的编辑权限
        /// </summary>
        public Boolean IsEdit
        {
            get { return UserInfo != null && UserInfo.UserID > 0 && (IsAdministrator || DotNetNuke.Security.Permissions.ModulePermissionController.HasModuleAccess(DotNetNuke.Security.SecurityAccessLevel.Edit, "CONTENT", ModuleConfiguration)); }
            //get { return UserId > 0 && (IsAdministrator ||  PortalSecurity.HasEditPermissions(ModuleId,TabId)); }
        }


        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public Boolean IsAdministrator
        {
            get { return UserInfo.IsSuperUser || UserInfo.IsInRole("Administrators"); }
        }

        private ModuleInfo _ModuleConfiguration = new ModuleInfo();
        /// <summary>
        /// 模块信息
        /// </summary>
        public ModuleInfo ModuleConfiguration
        {
            get
            {
                if (!(_ModuleConfiguration != null && _ModuleConfiguration.ModuleID > 0) && ModuleId > 0)
                {
                    ModuleController mc = new ModuleController();
                    _ModuleConfiguration = mc.GetModule(ModuleId, TabId);

                }
                return _ModuleConfiguration;
            }
        }


        private PortalSettings _portalSettings;
        /// <summary>
        /// 站点设置
        /// </summary>
        public PortalSettings PortalSettings
        {
            get
            {
                if (!(_portalSettings != null && _portalSettings.PortalId != Null.NullInteger))
                {
                    PortalAliasInfo objPortalAliasInfo = new PortalAliasInfo();
                    objPortalAliasInfo.PortalID = PortalId;
                    _portalSettings = new PortalSettings(TabId, objPortalAliasInfo);
                }
                return _portalSettings;
            }
        }



        private TabInfo _tabInfo;
        /// <summary>
        /// 页面信息
        /// </summary>
        public TabInfo TabInfo
        {
            get
            {
                if (!(_tabInfo != null && _tabInfo.TabID > 0) && TabId > 0)
                {
                    TabController tc = new TabController();
                    _tabInfo = tc.GetTab(TabId);

                }

                return _tabInfo;


            }
        }

        #endregion

 



 
		public bool IsReusable { get { return false; } }

		public void ProcessRequest (HttpContext context) {
			context.Response.AddHeader("Pragma", "no-cache");
			context.Response.AddHeader("Cache-Control", "private, no-cache");
			HandleMethod(context);
           
		}
        
		// Handle request based on method
		private void HandleMethod (HttpContext context) {

         

            switch (context.Request.HttpMethod) {
				case "HEAD":
				case "GET":
					if (GivenFilename(context)) DeliverFile(context);
					else ListCurrentFiles(context);
					break;

				case "POST":
				case "PUT":
					UploadFile(context);
					break;

				case "DELETE":
					DeleteFile(context);
					break;

				case "OPTIONS":
					ReturnOptions(context);
					break;

				default:
					context.Response.ClearHeaders();
					context.Response.StatusCode = 405;
					break;
			}
		}

		private static void ReturnOptions(HttpContext context) {
			context.Response.AddHeader("Allow", "DELETE,GET,HEAD,POST,PUT,OPTIONS");
			context.Response.StatusCode = 200;
		}

		// Delete file from the server
		private void DeleteFile (HttpContext context) {
            String FileID = WebHelper.GetStringParam(context.Request, "id", "");
            if (!String.IsNullOrEmpty(FileID))
            {
                String FileWebPath = String.Format("{0}PowerForms/Multiplefiles/{1}/{2}/", PortalSettings.HomeDirectory, ModuleId, DateTime.Now.ToString("yyyyMMdd"));
                String SaveFilePath = context.Request.MapPath(FileWebPath);
                DirectoryInfo dir = new DirectoryInfo(SaveFilePath);
                if (dir.Exists)
                {
                   FileInfo[] SearchFiles =   dir.GetFiles(String.Format("*-[{0}].*", FileID));
                    if (SearchFiles != null && SearchFiles.Length > 0)
                    {
                        SearchFiles[0].Delete();
                    }


                }

                
            }


            
		}

        /// <summary>
        /// 获取图片的路径
        /// </summary>
        /// <param name="media">媒体文件的实体</param>
        /// <returns></returns>
        public String GetPhotoPath(DNNGo_PowerForms_Files media)
        {
            String PhotoPath  = String.Empty;
            if (media != null && media.ID > 0)
            {
               PhotoPath = GetPhotoPath(media.FilePath);
            }
            return PhotoPath;

        }


        /// <summary>
        /// 获取图片的路径
        /// </summary>
        /// <param name="FilePath">图片路径</param>
        /// <returns></returns>
        public String GetPhotoPath(String FilePath)
        {
            return String.Format("{0}{1}", PortalSettings.HomeDirectory, FilePath);
        }

		// Upload file to the server
		private void UploadFile (HttpContext context) {
            var statuses = new List<Resource_FilesStatus>();
			var headers = context.Request.Headers;

            String a= WebHelper.GetStringParam(context.Request,"type","");

            if (!String.IsNullOrEmpty(a) && a == "DELETE")
            {
               

                DeleteFile(context);
            }
            else
            {

                if (string.IsNullOrEmpty(headers["X-File-Name"]))
                {
                    UploadWholeFile(context, statuses);
                }
                else
                {
                    UploadPartialFile(headers["X-File-Name"], context, statuses);
                }

                WriteJsonIframeSafe(context, statuses);
            }
		}

		// Upload partial file
        private void UploadPartialFile(string fileName, HttpContext context, List<Resource_FilesStatus> statuses)
        {
			if (context.Request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
            var file = context.Request.Files[0];

            if (file != null && !String.IsNullOrEmpty(file.FileName) && file.ContentLength > 0)
            {
                Resource_FilesStatus Resource = UploadFile(context, file);
                statuses.Add(Resource);
            }
             
		}

		// Upload entire file
        private void UploadWholeFile(HttpContext context, List<Resource_FilesStatus> statuses)
        {
			for (int i = 0; i < context.Request.Files.Count; i++) {
				var file = context.Request.Files[i];

                if (file != null && !String.IsNullOrEmpty(file.FileName) && file.ContentLength > 0)
                {
                    Resource_FilesStatus Resource = UploadFile(context,file);
                    statuses.Add(Resource);
                }
                
			}
		}


        private Resource_FilesStatus UploadFile(HttpContext context,HttpPostedFile Httpfiles)
        {
            Resource_FilesStatus Resource = new Resource_FilesStatus();

            if (Httpfiles != null && Httpfiles.ContentLength > 0)
            {

                //To verify that if the suffix name of the uploaded files meets the DNN HOST requirements
                Boolean retValue = FileSystemUtils.CheckValidFileName(Httpfiles.FileName);
                if (retValue)
                {

                    //构造文件的存放目录
                    String FileName = Httpfiles.FileName;
                    String tempName = WebHelper.GetStringParam(context.Request, "id", "");
                    String Extension = Path.GetExtension(FileName);
                    String leftName = FileName.Replace(Extension, "");
                    String FileWebPath = String.Format("{0}PowerForms/Multiplefiles/{1}/{2}/{3}-[{4}]{5}", PortalSettings.HomeDirectory, ModuleId, DateTime.Now.ToString("yyyyMMdd"), leftName, tempName, Extension);
                    String FileServerPath = context.Request.MapPath(FileWebPath);

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
                        FileWebPath = String.Format("{0}PowerForms/Multiplefiles/{1}/{2}/{3}_{4}-[{5}]{6}", PortalSettings.HomeDirectory, ModuleId, DateTime.Now.ToString("yyyyMMdd"), leftName, FileIndex, tempName, Extension);
                        FileServerPath = context.Request.MapPath(FileWebPath);
                        Savefile = new FileInfo(FileServerPath);
                        FileIndex++;
                    }
                    //构造需要返回的信息
                    Resource.name = FileName;
                    Resource.size = Httpfiles.ContentLength;
                    Resource.url = FileWebPath;
                    Resource.type = Httpfiles.ContentType;


                    //构造指定存储路径
                    Httpfiles.SaveAs(Savefile.FullName);
                }

            }
            else
            {
                //文件大小为空或者是0
            }

            return Resource;
        }


        private void WriteJsonIframeSafe(HttpContext context, List<Resource_FilesStatus> statuses)
        {
			context.Response.AddHeader("Vary", "Accept");
			try {
				if (context.Request["HTTP_ACCEPT"].Contains("application/json"))
					context.Response.ContentType = "application/json";
				else
					context.Response.ContentType = "text/plain";
			} catch {
				context.Response.ContentType = "text/plain";
			}

			var jsonObj = js.Serialize(statuses.ToArray());
			context.Response.Write(jsonObj);
		}

		private static bool GivenFilename (HttpContext context) {
            return !string.IsNullOrEmpty(context.Request["PhotoID"]);
		}

		private void DeliverFile (HttpContext context) {

            DNNGo_PowerForms_Files DataItem = DNNGo_PowerForms_Files.FindByKeyForEdit(WebHelper.GetStringParam(context.Request, "PhotoID", "0"));
            if (DataItem != null && DataItem.ID > 0)
            {
                String Picture = GetPhotoPath(DataItem);

                if (!String.IsNullOrEmpty(Picture) && File.Exists(context.Server.MapPath(Picture)))
               {
                   context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + DataItem.FileName + "\"");
                   context.Response.ContentType = "application/octet-stream";
                   context.Response.ClearContent();
                   context.Response.WriteFile(context.Server.MapPath(Picture));
               }
               else
                   context.Response.StatusCode = 404;
            }else
                context.Response.StatusCode = 404;



            //var filename = context.Request["PhotoID"];
            //var filePath = StorageRoot + filename;

            //if (File.Exists(filePath)) {
            //    context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\"");
            //    context.Response.ContentType = "application/octet-stream";
            //    context.Response.ClearContent();
            //    context.Response.WriteFile(filePath);
            //} else
            //    context.Response.StatusCode = 404;
		}

		private void ListCurrentFiles (HttpContext context) {
 
            //QueryParam qp = new QueryParam();
            //qp.PageSize = 0;

            //int RecordCount = 0;
            //qp.Where.Add(new SearchParam(DNNGo_PowerForms_Files._.AlbumID, WebHelper.GetStringParam(context.Request, "AlbumID", "0"), SearchType.Equal));

            //var files = DNNGo_PowerForms_Files.FindAll(qp, out RecordCount).Select(f => new Resource_FilesStatus(f, PortalSettings)).ToArray();
            var files = new List<Resource_FilesStatus>();
 
            string jsonObj = js.Serialize(files);
            context.Response.AddHeader("Content-Disposition", "inline; filename=\"files.json\"");
            context.Response.Write(jsonObj);
            context.Response.ContentType = "application/json";
		}
	}
}