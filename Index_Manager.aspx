﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index_Manager.aspx.cs" Inherits="DNNGo.Modules.PowerForms.Index_Manager" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Common.Controls" Assembly="DotNetNuke" %>
 <!DOCTYPE html>
<asp:literal id="skinDocType" runat="server"></asp:literal>
<!-- Template Name: Clip-One - Responsive Admin Template build with Twitter Bootstrap 3 Version: 1.0 Author: ClipTheme -->
<!--[if IE 8]><html class="ie8 no-js" lang="en"><![endif]-->
<!--[if IE 9]><html class="ie9 no-js" lang="en"><![endif]-->
<!--[if !IE]><!-->
<html lang="en" class="no-js">
	<!--<![endif]-->
<!-- start: HEAD --> 
<head id="Head" runat="server">
    <title></title>
    <!-- start: META -->
    <!--[if IE]><meta http-equiv='X-UA-Compatible' content="IE=edge,IE=9,IE=8,chrome=1" /><![endif]-->
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimum-scale=1.0, maximum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta content="text/html; charset=UTF-8" http-equiv="Content-Type"/>
    <meta content="text/javascript" http-equiv="Content-Script-Type"/>
    <meta content="text/css" http-equiv="Content-Style-Type"/>
    <meta id="MetaRefresh" runat="Server" http-equiv="Refresh" name="Refresh" />
    <meta id="MetaDescription" runat="Server" name="DESCRIPTION" />
    <meta id="MetaKeywords" runat="Server" name="KEYWORDS" />
    <meta id="MetaCopyright" runat="Server" name="COPYRIGHT" />
    <meta id="MetaGenerator" runat="Server" name="GENERATOR" />
    <meta id="MetaAuthor" runat="Server" name="AUTHOR" />
    <meta name="RESOURCE-TYPE" content="DOCUMENT" />
    <meta name="DISTRIBUTION" content="GLOBAL" />
    <meta id="MetaRobots" runat="server" name="ROBOTS" />
    <meta name="REVISIT-AFTER" content="1 DAYS" />
    <meta name="RATING" content="GENERAL" />
    <meta http-equiv="PAGE-ENTER" content="RevealTrans(Duration=0,Transition=1)" />
    <style type="text/css" id="StylePlaceholder" runat="server"></style>
    <asp:placeholder id="CSS" runat="server" />
      <script type="text/javascript">
          var Module = {ModulePath:"<%=ModulePath %>",ModuleId: <%=ModuleId %>, TabId: <%=TabId %>, PortalId: <%=PortalId %>,QueryString:"<%=QueryString %>"}
    </script>
</head>
<body id="Body" runat="server" >

    <!-- start: MAIN CSS -->
    <link rel="stylesheet" href="<%=ModulePath %>Resource/plugins/bootstrap/css/bootstrap.min.css?cdv=<%=CrmVersion %>"  media="screen" />
    <link rel="stylesheet" href="<%=ModulePath %>Resource/plugins/font-awesome/css/font-awesome.min.css?cdv=<%=CrmVersion %>" />
    <link rel="stylesheet" href="<%=ModulePath %>Resource/css/font-icon.css?cdv=<%=CrmVersion %>" />
    <link rel="stylesheet" href="<%=ModulePath %>Resource/fonts/style.css?cdv=<%=CrmVersion %>" />
    <link rel="stylesheet" href="<%=ModulePath %>Resource/css/main.css?cdv=<%=CrmVersion %>" />
    <link rel="stylesheet" href="<%=ModulePath %>Resource/css/main-responsive.css?cdv=<%=CrmVersion %>" />
 	<link rel="stylesheet" href="<%=ModulePath %>Resource/plugins/iCheck/skins/all.css?cdv=<%=CrmVersion %>">
    <link rel="stylesheet" href="<%=ModulePath %>Resource/plugins/perfect-scrollbar/src/perfect-scrollbar.css?cdv=<%=CrmVersion %>" />
    <link rel="stylesheet" href="<%=ModulePath %>Resource/plugins/bootstrap-modal/css/bootstrap-modal-bs3patch.css?cdv=<%=CrmVersion %>" />
    <link rel="stylesheet" href="<%=ModulePath %>Resource/plugins/bootstrap-modal/css/bootstrap-modal.css?cdv=<%=CrmVersion %>" />
    <link rel="stylesheet" href="<%=ModulePath %>Resource/plugins/select2/select2.min.css?cdv=<%=CrmVersion %>">
    <link rel="stylesheet" href="<%=ModulePath %>Resource/plugins/datepicker/css/datepicker.css?cdv=<%=CrmVersion %>">
    <link rel="stylesheet" href="<%=ModulePath %>Resource/plugins/bootstrap-timepicker/css/bootstrap-timepicker.min.css?cdv=<%=CrmVersion %>">
    <link rel="stylesheet" href="<%=ModulePath %>Resource/plugins/bootstrap-switch/static/stylesheets/bootstrap-switch.css?cdv=<%=CrmVersion %>">
    <link rel="stylesheet" href="<%=ModulePath %>Resource/plugins/bootstrap-colorpicker/css/bootstrap-colorpicker.css?cdv=<%=CrmVersion %>">
    <link rel="stylesheet" href="<%=ModulePath %>Resource/css/theme_light.css?cdv=<%=CrmVersion %>" type="text/css" id="skin_color" />
    <!--[if IE 7]>
	    <link rel="stylesheet" href="<%=ModulePath %>Resource/plugins/font-awesome/css/font-awesome-ie7.min.css?cdv=<%=CrmVersion %>" />
	<![endif]-->
    <!-- end: MAIN CSS -->
    <!-- start: CSS REQUIRED FOR THIS PAGE ONLY -->
    <link rel="stylesheet" href="<%=ModulePath %>Resource/plugins/bootstrap-social-buttons/social-buttons-3.css?cdv=<%=CrmVersion %>" />
    <!-- end: CSS REQUIRED FOR THIS PAGE ONLY -->
    <script src="<%=ModulePath %>Resource/js/jquery.min.js?cdv=<%=CrmVersion %>"></script>
    <script src="<%=ModulePath %>Resource/js/jquery-migrate.min.js?cdv=<%=CrmVersion %>"></script>
    <script src="<%=ModulePath %>Resource/js/jquery-ui.min.js?cdv=<%=CrmVersion %>"></script>
    <script src="<%=ModulePath %>Resource/js/jquery.tmpl.min.js?cdv=<%=CrmVersion %>"></script>

    <!-- start: MAIN JAVASCRIPTS -->
    <!--[if lt IE 9]>
		    <script src="<%=ModulePath %>Resource/plugins/respond.min.js?cdv=<%=CrmVersion %>"></script>
		    <script src="<%=ModulePath %>Resource/plugins/excanvas.min.js?cdv=<%=CrmVersion %>"></script>
    <![endif]-->

    <script src="<%=ModulePath %>Resource/plugins/bootstrap/js/bootstrap.min.js?cdv=<%=CrmVersion %>"></script>
    <script src="<%=ModulePath %>Resource/plugins/blockUI/jquery.blockUI.js?cdv=<%=CrmVersion %>"></script>
    <script src="<%=ModulePath %>Resource/plugins/iCheck/jquery.icheck.js?cdv=<%=CrmVersion %>"></script>
    <script src="<%=ModulePath %>Resource/plugins/perfect-scrollbar/src/jquery.mousewheel.js?cdv=<%=CrmVersion %>"></script>
    <script src="<%=ModulePath %>Resource/plugins/perfect-scrollbar/src/perfect-scrollbar.js?cdv=<%=CrmVersion %>"></script>
    <script src="<%=ModulePath %>Resource/plugins/less/less-1.5.0.min.js?cdv=<%=CrmVersion %>"></script>
    <script src="<%=ModulePath %>Resource/js/jquery.cookie.js?cdv=<%=CrmVersion %>"></script>
    <script src="<%=ModulePath %>Resource/plugins/bootstrap-modal/js/bootstrap-modal.js?cdv=<%=CrmVersion %>"></script>
    <script src="<%=ModulePath %>Resource/plugins/bootstrap-modal/js/bootstrap-modalmanager.js?cdv=<%=CrmVersion %>"></script>
    <script src="<%=ModulePath %>Resource/plugins/ladda-bootstrap/dist/spin.min.js?cdv=<%=CrmVersion %>"></script>
	<script src="<%=ModulePath %>Resource/plugins/ladda-bootstrap/dist/ladda.min.js?cdv=<%=CrmVersion %>"></script>
    <script src="<%=ModulePath %>Resource/plugins/bootstrap-paginator/src/bootstrap-paginator.js?cdv=<%=CrmVersion %>"></script>
    <script src="<%=ModulePath %>Resource/plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.js?cdv=<%=CrmVersion %>"></script>
    <script src="<%=ModulePath %>Resource/plugins/select2/select2.full.min.js?cdv=<%=CrmVersion %>"></script>
    <script src="<%=ModulePath %>Resource/plugins/bootstrap-datepicker/js/bootstrap-datepicker.js?cdv=<%=CrmVersion %>"></script>
    <script src="<%=ModulePath %>Resource/plugins/bootstrap-timepicker/js/bootstrap-timepicker.min.js?cdv=<%=CrmVersion %>"></script>
    <script src="<%=ModulePath %>Resource/plugins/bootstrap-switch/static/js/bootstrap-switch.min.js?cdv=<%=CrmVersion %>"></script>
    <script src="<%=ModulePath %>Resource/plugins/ddsort/ddsort.min.js?cdv=<%=CrmVersion %>"></script>
    <link rel="stylesheet" href="<%=ModulePath %>Resource/css/dropzone.css?cdv=<%=CrmVersion %>" />
    <script type="text/javascript" src="<%=ModulePath %>Resource/js/dropzone.js?cdv=<%=CrmVersion %>"></script>
    <script src="<%=ViewValidationEngineLanguage() %>"></script>
    <script src="<%=ModulePath %>Resource/js/jquery.validationEngine.js?cdv=<%=CrmVersion %>"></script>
    <!-- end: MAIN JAVASCRIPTS -->

    <script src="<%=ModulePath %>Resource/js/form-elements.js?cdv=<%=CrmVersion %>"></script>
    <script src="<%=ModulePath %>Resource/js/main.js?cdv=<%=CrmVersion %>"></script>
    <script src="<%=ModulePath %>Resource/plugins/tinymce/tinymce.min.js?cdv=<%=CrmVersion %>"></script>
    <script src="<%=ModulePath %>Resource/js/jquery.urls.js?cdv=<%=CrmVersion %>"></script>


<dnn:Form id="Form" runat="server" ENCTYPE="multipart/form-data" >
 <asp:Label ID="SkinError" runat="server" CssClass="NormalRed" Visible="False"></asp:Label>
<!-- start: HEADER -->
<div class="navbar navbar-inverse navbar-fixed-top"> 
  <!-- start: TOP NAVIGATION CONTAINER -->
  <div class="container">
    <div class="navbar-header"> 
    <!-- start: RESPONSIVE MENU TOGGLER -->
	<button data-target=".navbar-collapse" data-toggle="collapse" class="navbar-toggle" type="button">
		<span class="clip-list-2"></span>
	</button>
	<!-- end: RESPONSIVE MENU TOGGLER -->

      <!-- start: LOGO --> 
      <div class="navbar-brand">
            <asp:HyperLink ID="hlHome" runat="server" Text="<i class='fa fa-mail-reply-all'></i>" ToolTip="Back to home" CssClass="tooltips" data-placement="top" data-rel="tooltip" data-original-title="Back to view"></asp:HyperLink>
            <asp:Literal ID="litModuleTitle" runat="server"></asp:Literal>
            <asp:Literal ID="litModuleVersion" runat="server"></asp:Literal>
        <asp:HyperLink ID="hlModuleLink" runat="server" CssClass="tooltips hidden-xs" Target="_blank" data-placement="bottom" data-rel="tooltip" data-original-title="Click to download the latest version">
           <asp:Literal ID="litUpdateVersion" runat="server"></asp:Literal>
        </asp:HyperLink>
      </div>
      <!-- end: LOGO --> 
    </div>
    <div class="navbar-tools hidden-xs"> 
        <asp:PlaceHolder  ID="phLicense" runat="server"></asp:PlaceHolder>
        <asp:HyperLink ID="hlBuyIt" resourcekey="hlBuyIt" runat="server" Target="_blank" CssClass="btn btn-teal" NavigateUrl="http://www.dnngo.net/DNNStore/tabid/58/ProductID/471/Default.aspx" Text="<i class='fa clip-cart'></i> | Buy It" ToolTip="Buy It"></asp:HyperLink>
        <asp:HyperLink ID="hlDocumentation" resourcekey="hlDocumentation" runat="server" Target="_blank" CssClass="btn btn-teal" NavigateUrl="http://www.dnngo.net/doc/Powerforms/index.html" Text="<i class='fa clip-book'></i> | Documentation" ToolTip="Documentation"></asp:HyperLink>
        <asp:HyperLink ID="hlHelp" resourcekey="hlHelp" runat="server" Target="_blank" CssClass="btn btn-teal" NavigateUrl="http://www.DNNGo.net/Contactus/OnlineSupportTicket.aspx" Text="<i class='fa clip-question'></i> | Help" ToolTip="Help"></asp:HyperLink>
    </div>
  </div>
  <!-- end: TOP NAVIGATION CONTAINER --> 
</div>
<!-- end: HEADER -->

<div class="main-container">
  <div class="navbar-content"> 
    <!-- start: SIDEBAR -->
    <div class="main-navigation navbar-collapse collapse"> 
      <!-- start: MAIN NAVIGATION MENU -->
      <ul class="main-navigation-menu">
        <asp:Literal ID="liLeftMenu" runat="server"></asp:Literal>
      </ul>
      <!-- end: MAIN NAVIGATION MENU --> 
     

    </div>
    <!-- end: SIDEBAR --> 
  </div>
  <!-- start: PAGE -->
  <div class="main-content"> 
    
     <div id="PlaceHolder_container" class="container validationEngineContainer"> 
        <!-- start: PAGE HEADER -->
        <div class="row">
            <div class="col-sm-12">
                <!-- start: PAGE TITLE & BREADCRUMB -->
                <asp:Literal ID="LiBreadcrumb" runat="server"></asp:Literal>
            </div>
        </div>
        <!-- end: PAGE HEADER -->


        <!--message-->
        <asp:Label ID="lblMessage"  runat="server" CssClass="LI_Message"></asp:Label>
        <!--load UserControl-->
        <asp:PlaceHolder  ID="phContainer" runat="server"></asp:PlaceHolder>
    </div>
    <!-- end: PAGE --> 
  </div>
</div>

	<!-- start: FOOTER -->
		<div class="footer clearfix">
			<div class="footer-inner">
				2013 &copy; PowerForms by <a href="http://www.dnngo.net/" target="_blank">dnngo.net</a>.
			</div>
			<div class="footer-items">
				<span class="go-top"><i class="clip-chevron-up"></i></span>
			</div>
		</div>
		<!-- end: FOOTER -->



<script id="scriptLibrary" type="text/x-jquery-tmpl">
<li class="list-item dd-item dd3-item" data-id="${ID}" data-sort="${Sort}">
        <div class="row">
            <div class="col-xs-1">${ID}</div>
            <div class="col-xs-3">{{html Thumbnail}}</div>
            <div class="col-xs-4">${Name}</div>
            <div class="col-xs-2">${Extension}</div>
            <div class="col-xs-2 last">
                <a href="javascript:;" data-id="${ID}"  data-MediaID="MediaID=${ID}" data-src="${ThumbnailUrl}" class="Select_Thumbnail btn btn-xs btn-bricky tooltips" data-placement="top" data-original-title="Select"><i class="fa fa-plus"></i></a>
                <div id="div_json${ID}" style="display:none;">${Json}</div>
            </div>
        </div>
</li>
</script>
<script id="scriptImage" type="text/x-jquery-tmpl">
<div class="wrap-image" data-id="${ID}" data-url="${ThumbnailUrl}" data-name="${Name}">
    <a class="group${ID} cboxElement" title="${Name}">
        <img class="imgUrlLink img-responsive" id="imgUrlLink" src="${ThumbnailUrl}" /> 
    </a>
    <div class="tools tools-bottom" style="bottom:0;">
        <a>${Name} ${Extension}</a>
	    <a href="${FileUrl}" target="_blank" class="right">
		    <i class="clip-link-4"></i>
	    </a>
	    <a href="javascript:;" class="hlRemoveUrlLink right">
		    <i class="fa fa-trash-o"></i>
	    </a>
    </div>
</div>
</script>

 <input id="ScrollTop" runat="server" name="ScrollTop" type="hidden" />
<input id="__dnnVariable" runat="server" name="__dnnVariable" type="hidden" />
 
</dnn:Form>
</body>


<script type="text/javascript">
    jQuery(function (q) {
        FormElements.init();
        Main.init();




        $("#PlaceHolder_container").validationEngine({
            promptPosition: "topRight"
        });

        $("#PlaceHolder_container input[data-verify='Submit']").click(function () {
            if (!$('#PlaceHolder_container').validationEngine('validate')) {
                return false;
            }
        });

        tinymce.init({
            selector: "textarea.tinymce",
            convert_urls: false,
            plugins: [
		        "advlist autolink link image lists charmap print preview hr anchor pagebreak spellchecker",
		        "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
		        "save table contextmenu directionality template paste textcolor"
	        ]
        });
    });

    function CancelValidation() {
        $('#Form').validationEngine('detach');
    }
</script>
<script src="<%=ModulePath %>Resource/js/jquery.conditionize.min.js?cdv=<%=CrmVersion %>"></script>

<!-- end: BODY -->
</html>

 
