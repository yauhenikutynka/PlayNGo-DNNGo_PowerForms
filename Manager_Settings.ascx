<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Manager_Settings.ascx.cs" Inherits="DNNGo.Modules.PowerForms.Manager_Settings" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Security.Permissions.Controls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" TagName="URL" Src="~/controls/URLControl.ascx" %>

<!-- start: PAGE HEADER -->
<div class="row">
    <div class="col-sm-12">
        <!-- start: PAGE TITLE & BREADCRUMB -->
        
        <div class="page-header">
            <h1>
                <i class="fa fa-wrench"></i>
                <%=ViewResourceText("Header_Title", "General Settings")%></h1>
        </div>
        <!-- end: PAGE TITLE & BREADCRUMB -->
    </div>
</div>
<!-- end: PAGE HEADER -->
<!-- start: PAGE CONTENT -->
<div class="row">
    <div class="col-sm-12">
        <div class="panel panel-default">
            <div class="panel-heading">
                <i class="fa fa-external-link-square"></i>
                <%=ViewTitle("lblBaseSettings", "Base Settings")%>
                <div class="panel-tools">
                    <a href="#" class="btn btn-xs btn-link panel-collapse collapses"></a>
                </div>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <%=ViewControlTitle("lblPageSize", "Lists per page", "txtPageSize", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="control-inline">
                                <asp:TextBox runat="server" ID="txtPageSize" Width="100px" CssClass="input_text validate[required,custom[integer]]"></asp:TextBox>
                             </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <%=ViewControlTitle("lblRedirectPage", "Redirect results page", "ucRedirectPage", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="control-inline">
                                <dnn:URL ID="ucRedirectPage" runat="server" ShowTabs="true" UrlType="N" ShowNewWindow="false"
                                    ShowNone="true" Visible="true" ShowSecure="false" ShowDatabase="false" ShowLog="false"
                                    ShowTrack="false" ShowFiles="true" ShowUrls="true" />
                                
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <%=ViewControlTitle("lblReturnUrl", "Customize Return Url", "ucReturnUrl", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="control-inline">
                                <dnn:URL ID="ucReturnUrl" runat="server" ShowTabs="true" UrlType="N" ShowNewWindow="false"
                                    ShowNone="true" Visible="true" ShowSecure="false" ShowDatabase="false" ShowLog="false"
                                    ShowTrack="false" ShowFiles="true" ShowUrls="true" />
                                
                            </div>
                        </div>
                    </div>
                    
                      <div class="form-group">
                        <%=ViewControlTitle("lblAnchorLink", "Anchor link", "txtAnchorLink", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="control-inline">
                                <asp:TextBox runat="server" ID="txtAnchorLink" Width="150px" CssClass="input_text"></asp:TextBox>
                             </div>
                             <%=ViewHelp("lblAnchorLink", "Anchor position for redirecting to result page can be set, no # is needed.")%> 
                        </div>
                    </div>



                   <div class="form-group">
                        <%=ViewControlTitle("lblDatePattern", "Date Pattern", "txtDatePattern", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="control-inline">
                                <asp:TextBox runat="server" ID="txtDatePattern" Width="150px" CssClass="input_text validate[required]"></asp:TextBox>
                             </div>
                             <%=ViewHelp("lblDatePattern", "The default date format is mm/dd/yyyy, of course you can customize it to the one you need, such as, yyyy-mm-dd")%> 
                        </div>
                    </div>
                   <div class="form-group">
                        <%=ViewControlTitle("lblHideIp", "Hide IP address", "cbHideIp", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="checkbox-inline">
                                <asp:CheckBox ID="cbHideIp" runat="server" CssClass="auto" />
                                </div>
                             <%=ViewHelp("lblHideIp", "192.168.***. Not to store the full IP address, hide the last two pieces of it.")%> 
                        </div>
                    </div>
                     
                      <div class="form-group">
                        <%=ViewControlTitle("lblFormVersion", "Form Version", "txtFormVersion", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="control-inline">
                                <asp:TextBox runat="server" ID="txtFormVersion" Width="150px" CssClass="input_text validate[custom[integer]]"></asp:TextBox>
                             </div>
                             <%=ViewHelp("lblFormVersion", "It will only be allowed to fill out the form once for each version if you set version identifier. Only numbers are allowed to be version identifiers.")%> 
                        </div>
                    </div>
                    <div class="form-group">
                        <%=ViewControlTitle("lblFormStorage", "Form Storage", "cbFormStorage", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="checkbox-inline">
                                <asp:CheckBox ID="cbFormStorage" runat="server" CssClass="auto" />
                                </div>
                             <%=ViewHelp("lblFormStorage", "Save the filled content of the form temporarily before submitting.")%> 
                        </div>
                    </div>
                       <div class="form-group">
                        <%=ViewControlTitle("lblLoginUserDisplay", "Login User Display", "cbLoginUserDisplay", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="checkbox-inline">
                                <asp:CheckBox ID="cbLoginUserDisplay" runat="server" CssClass="auto" />
                                </div>
                             <%=ViewHelp("lblLoginUserDisplay", "Form is only visible to users who are logged in.")%> 
                        </div>
                    </div>

                    <div class="form-group">
                        <%=ViewControlTitle("lblLoginUserDownload", "Login User Download Attachment", "cbLoginUserDownload", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="checkbox-inline">
                                <asp:CheckBox ID="cbLoginUserDownload" runat="server" CssClass="auto" />
                                </div>
                             <%=ViewHelp("lblLoginUserDownload", "Only logon users are allowed to download attachment.")%> 
                        </div>
                    </div>

                    
                      <div class="form-group">
                        <%=ViewControlTitle("lblPromptNotLogged", "Prompt Not Logged", "txtPromptNotLogged", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="control-inline">
                                <asp:TextBox runat="server" ID="txtPromptNotLogged" Width="450px" CssClass="tinymce  input_text" TextMode="MultiLine"></asp:TextBox>
                             </div>
                             <%=ViewHelp("lblPromptNotLogged", "Prompt that users have not logged it.")%> 
                        </div>
                    </div>

                    
                      <div class="form-group">
                        <%=ViewControlTitle("lblPromptAlreadySubmitted", "Prompt Already Submitted", "txtPromptAlreadySubmitted", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="control-inline">
                                <asp:TextBox runat="server" ID="txtPromptAlreadySubmitted" Width="450px" CssClass="tinymce  input_text" TextMode="MultiLine"></asp:TextBox>
                             </div>
                             <%=ViewHelp("lblPromptAlreadySubmitted", "Prompt that users have already submitted the form")%> 
                        </div>
                    </div>



                </div>
            </div>
        </div>


            <div class="panel panel-default">
            <div class="panel-heading">
                <i class="fa fa-external-link-square"></i>
                     <%=ViewTitle("lblMultipleSettings", "Multiple FileUpload Settings")%>
                <div class="panel-tools">
                    <a href="#" class="btn btn-xs btn-link panel-collapse collapses"></a>
                </div>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    
                    <div class="form-group">
                         <%=ViewControlTitle("lblMaxFileSize", "Max file size", "txtMaxFileSize", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="control-inline">
                                <asp:TextBox runat="server" ID="txtMaxFileSize" Width="100px" CssClass="input_text validate[required,custom[integer],min[1]]"></asp:TextBox>
                                <%=ViewHelp("lblMaxFileSize", "KB")%> 
                                
                             </div>
                        </div>
                    </div>
                   <div class="form-group">
                        <div class="col-sm-3"></div>
                        <div class="col-sm-5">
                            <div class="control-inline">
                                 <asp:Button CssClass="btn btn-default" ID="cmdClearfiles" resourcekey="cmdClearfiles" runat="server"  Text="Clear temporary files" CausesValidation="False" OnClick="cmdClearfiles_Click" OnClientClick="CancelValidation();"></asp:Button>
                             <%=ViewHelp("lblClearfiles", "You can clear the useless files of the temporary folder on multiple file upload controls with this button.")%> 
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>


    <div class="panel panel-default">
            <div class="panel-heading">
                <i class="fa fa-external-link-square"></i>
                <%=ViewTitle("lblHistoryRecords", "History Records")%>
                <div class="panel-tools">
                    <a href="#" class="btn btn-xs btn-link panel-collapse collapses"></a>
                </div>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <%=ViewControlTitle("lblSaveRecords", "Store History Records", "cbSaveRecords", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="checkbox-inline">
                                <asp:CheckBox ID="cbSaveRecords" runat="server" CssClass="auto" />
                             </div>
                        </div>
                    </div>
         
                    
                    <div class="form-group">
                        <%=ViewControlTitle("lblExtraTracking", "Extra Tracking", "cbExtraTracking", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="checkbox-inline">
                                <asp:CheckBox ID="cbExtraTracking" runat="server" CssClass="auto" />
                                
                                </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <%=ViewControlTitle("lblExportExtraTracking", "Export Extra Tracking", "cbExtraTracking", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="checkbox-inline">
                                <asp:CheckBox ID="cbExportExtraTracking" runat="server" CssClass="auto" />
                                
                                </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <%=ViewControlTitle("lblExportExtension", "Export Extension", "rblExportExtension", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="checkbox-inline">
                                <asp:RadioButtonList ID="rblExportExtension" runat="server" CssClass="auto"></asp:RadioButtonList>
                            </div>
                        </div>
                    </div>




                </div>
            </div>
        </div>
           

        <div class="panel panel-default">
            <div class="panel-heading">
                <i class="fa fa-external-link-square"></i>
                <%=ViewTitle("lblCleanupSettings", "Cleanup Settings")%>
                <div class="panel-tools">
                    <a href="#" class="btn btn-xs btn-link panel-collapse collapses"></a>
                </div>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <%=ViewControlTitle("lblScheduleEnable", "Schedule Enable", "cbScheduleEnable", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="checkbox-inline">
                                <asp:CheckBox ID="cbScheduleEnable" runat="server" CssClass="auto" />
                             </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <%=ViewControlTitle("lblDaysBefore", "Days Before", "txtDaysBefore", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="control-inline">
                                <asp:TextBox runat="server" ID="txtDaysBefore" Width="100px" CssClass="input_text validate[required,custom[integer],min[7]]"></asp:TextBox>
                             </div>
                        </div>
                    </div>
                   <div class="form-group">
                        <%=ViewControlTitle("lblMaxFeedback", "Max Feedback", "txtMaxFeedback", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="control-inline">
                                <asp:TextBox runat="server" ID="txtMaxFeedback" Width="100px" CssClass="input_text validate[required,custom[integer],max[10000]]"></asp:TextBox>
                             </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>


    </div>
</div>
<!-- end: PAGE CONTENT-->
<div class="row">
    <div class="col-sm-2">
    </div>
    <div class="col-sm-10">
        <asp:Button CssClass="btn btn-primary" data-verify="Submit" ID="cmdUpdate" resourcekey="cmdUpdate"
            runat="server" Text="Update" OnClick="cmdUpdate_Click"></asp:Button>&nbsp;
        <asp:Button CssClass="btn btn-default" ID="cmdCancel" resourcekey="cmdCancel" runat="server"
            Text="Cancel" CausesValidation="False" OnClick="cmdCancel_Click" OnClientClick="CancelValidation();">
        </asp:Button>&nbsp;
    </div>
</div>

