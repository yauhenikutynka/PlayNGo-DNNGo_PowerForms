using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace DNNGo.Modules.PowerForms
{
    public partial class Resource_DropzoneUpload : basePortalModule
    {

        public String QueryString
        {
            get { return String.Format("{0}&ModulePath={1}", WebHelper.GetScriptNameQueryString, HttpUtility.UrlEncode(ModulePath)); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
            }
        }
    }
}