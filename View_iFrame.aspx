<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View_iFrame.aspx.cs" Inherits="DNNGo.Modules.PowerForms.View_iFrame" %>

<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Common.Controls" Assembly="DotNetNuke" %>
<!DOCTYPE HTML>
<!--
/*
 * jQuery File Upload Plugin HTML Example 5.0.5
 * https://github.com/blueimp/jQuery-File-Upload
 *
 * Copyright 2010, Sebastian Tschan
 * https://blueimp.net
 *
 * Licensed under the MIT license:
 * http://creativecommons.org/licenses/MIT/
 */
-->
<html lang="en" class="no-js">
<head>
    <meta charset="utf-8">
    <script src="<%=ModulePath %>Resource/js/jquery.min.js?cdv=<%=CrmVersion %>"></script>
    <script src="<%=ModulePath %>Resource/js/jquery-migrate.min.js?cdv=<%=CrmVersion %>"></script>
    <script src="<%=ModulePath %>Resource/js/jquery-ui.min.js?cdv=<%=CrmVersion %>"></script>
    <!--[if lt IE 9]>
    <script src="http://html5shim.googlecode.com/svn/trunk/html5.js?cdv=<%=CrmVersion %>"></script>
    <![endif]-->
    <style type="text/css" id="StylePlaceholder" runat="server"></style>
    <asp:placeholder id="CSS" runat="server" />
</head>
<body id="Body" runat="server" class="iframe_powerformes">
    <dnn:Form id="Form" runat="server" ENCTYPE="multipart/form-data">


         <script type="text/javascript">
             var GRecaptchaVerifyCallback<%=ModuleId %> = function(response) 
                 {
                     $("#g-recaptcha-<%=ModuleId %>").data("GRecaptcha", true);
                 };

                 var GRecaptchaExpiredCallback<%=ModuleId %> = function() 
                 {
                     $("#g-recaptcha-<%=ModuleId %>").data("GRecaptcha", false);
                 };
        </script>
        <asp:PlaceHolder ID="BodySCRIPTS" runat="server" />
            <div id="phContainer<%=ModuleId %>" class="validationEngineContainer form_div_<%=ModuleId %>">
                 <asp:PlaceHolder ID="phPlaceHolder" runat="server"></asp:PlaceHolder>
            </div>
            
        
    </dnn:Form> 
    
     <script type="text/javascript">
             jQuery(function (q) {
                  q(".form_div_<%=ModuleId %>").validationEngine({
                         promptPosition: '<%=ViewXmlSetting("promptPosition","topRight") %>',
                         ajaxFormValidationURL: '<%=ModulePath %>ajaxValidate.aspx?ModuleID=<%=ModuleId %>'
                     });

                 q(".form_div_<%=ModuleId %> input[data-verify='Submit']").click(function () {

                         if (!$('.form_div_<%=ModuleId %>').validationEngine('validate')) {
                             return false;
                         }

                         if($("#g-recaptcha-<%=ModuleId %>").is("div") )
                         {
                             if ( !$("#g-recaptcha-<%=ModuleId %>").data("GRecaptcha"))
                             {
                                 $("#g-recaptcha-msg-<%=ModuleId %>").show("fast",function(){
                                     $(this).fadeOut(5000);
                                 });
                                 return false;
                             }
                         }
             
             

                     });

                 q(".form_div_<%=ModuleId %> input[data-verify='reset']").click(function () {
                         $('.form_div_<%=ModuleId %>').validationEngine('hideAll');
                     });

                     q(".form_div_<%=ModuleId %> .jquery-datepick").datepick({ dateFormat: '<%=DatePattern %>' });
             });
    </script>
</body>
</html>
