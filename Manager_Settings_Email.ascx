<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Manager_Settings_Email.ascx.cs" Inherits="DNNGo.Modules.PowerForms.Manager_Settings_Email" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<!-- start: PAGE HEADER -->
<div class="row">
    <div class="col-sm-12">
        <!-- start: PAGE TITLE & BREADCRUMB -->
        
        <div class="page-header">
            <h1>
                <i class="fa fa-wrench"></i>
                <%=ViewResourceText("Header_Title", "Email Settings")%></h1>
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
                <%=ViewTitle("lblEmailSettings", "Email ON / OFF")%>
                <div class="panel-tools">
                    <a href="#" class="btn btn-xs btn-link panel-collapse collapses"></a>
                </div>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <%=ViewControlTitle("lblSenderEmail", "Custom 'From' email address", "SenderEmail", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="control-inline">
                                <asp:TextBox runat="server" ID="txtSenderEmail" Width="300px" CssClass="form-control validate[required,custom[email]]"></asp:TextBox>
                              
                             </div>
                        </div>
                    </div>
                   <div class="form-group">
                        <%=ViewControlTitle("lblSendAdmin", "Send to admin", "cbSendToAdmin", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="checkbox-inline">
                                <asp:CheckBox ID="cbSendToAdmin" runat="server" CssClass="auto" />
                            </div>
                            <%=ViewHelp("SendToAdmin_Help", "Administrator will receive emails after checking this.")%> 
                             
                        </div>
                    </div>
                    <div class="form-group">
                        <%=ViewControlTitle("lblAdminEmail", "Administrator Email", "txtAdminEmail", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="control-inline">
                                <asp:TextBox runat="server" ID="txtAdminEmail" Width="300px" CssClass="form-control validate[required]"></asp:TextBox>
                             </div>
                                <%=ViewHelp("AdminEmail_Help", "Set Administrator emails which need receiving reminder email, multiple email addresses separated by ; ")%>
                            
                        </div>
                    </div>

                   <div class="form-group">
                        <%=ViewControlTitle("lblAdminEmailRoles", "Administrator Roles Email", "cblAdminEmailRoles", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="checkbox-inline">
                                <asp:CheckBoxList ID="cblAdminEmailRoles" Width="300px"    CssClass="auto" runat="server"></asp:CheckBoxList>
                             </div>
                              <%=ViewHelp("AdminEmailRoles_Help", "Administrator Roles to receive notification email. ")%>
                            
                        </div>
                    </div>


                    <div class="form-group">
                        <%=ViewControlTitle("lblSendSubmitUser", "Send to submit user", "cbSendToSubmitUser", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="checkbox-inline">
                               <asp:CheckBox ID="cbSendToSubmitUser" runat="server"  CssClass="auto" />
                             </div>
                               <%=ViewHelp("SendToSubmitUser_Help", "Submit user will receive emails after checking this.")%>  
                           
                        </div>
                    </div>
                    <div class="form-group">
                        <%=ViewControlTitle("lblSubmitUserEmail", "Submit User Email", "ddlSubmitUserEmail", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="control-inline">
                               <asp:DropDownList ID="ddlSubmitUserEmail" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                               <%=ViewHelp("SubmitUserEmail_Help", "Choose form fields as email address of submit user.")%>
                            
                        </div>
                    </div>
                     <div class="form-group" style="display:none;">
                        <%=ViewControlTitle("lblReplaceSender", "Replace the sender", "cbReplaceSender", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="checkbox-inline">
                                <asp:CheckBox ID="cbReplaceSender" runat="server" CssClass="auto" />
                            </div>
                            <%=ViewHelp("ReplaceSender_Help", "Sender of the administrator email will be replaced by submit user after checking this. This feature may not work properly due to some ISP.")%> 
                            
                        </div>
                    </div>
                     <div class="form-group">
                        <%=ViewControlTitle("lblReplyTo", "Reply To", "cbReplyTo", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="checkbox-inline">
                                <asp:CheckBox ID="cbReplyTo" runat="server" CssClass="auto" />
                            </div>
                            <%=ViewHelp("ReplyTo_Help", "Email received by the Administrator can be replied to the form submitter directly. This feature may not work properly due to some ISP.")%> 
                            
                        </div>
                    </div>



                </div>
            </div>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading">
                <i class="fa fa-external-link-square"></i>
                <%=ViewTitle("lblEmailTemplate", "Email Content Template")%>
                <div class="panel-tools">
                    <a href="#" class="btn btn-xs btn-link panel-collapse collapses"></a>
                </div>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <%=ViewControlTitle("lblSendAdminTitle", "Receivers' Email Subject", "txtSendAdminTitle", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-8">
                            <div class="control-inline">
                               <asp:TextBox runat="server" ID="txtSendAdminTitle" Width="500px" CssClass="form-control validate[required,maxSize[200]]"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <%=ViewControlTitle("lblSendAdminContent", "Receivers' Email Template", "txtSendAdminContent", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-8">
                            <div class="control-inline">
                               <%--<dnn:TextEditor id="txtSendAdminContent" runat="server" height="400" width="500"></dnn:TextEditor>--%>
                                <asp:TextBox ID="txtSendAdminContent" CssClass="tinymce form-control" runat="server" Height="300" Width="100%" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                     <div class="form-group">
                        <%=ViewControlTitle("lblSendSubmitUserTitle", "Auto Reply Email Subject", "txtSendSubmitUserTitle", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-8">
                            <div class="control-inline">
                               <asp:TextBox runat="server" ID="txtSendSubmitUserTitle" Width="500px" CssClass="form-control validate[required,maxSize[200]]"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                     <div class="form-group">
                        <%=ViewControlTitle("lblSendSubmitUserContent", "Auto Reply Email Template", "txtSendSubmitUserContent", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-8">
                            <div class="control-inline">
                               <%--<dnn:TextEditor id="txtSendSubmitUserContent" runat="server" height="400" width="500"></dnn:TextEditor>--%>
                                <asp:TextBox ID="txtSendSubmitUserContent" CssClass="tinymce form-control" runat="server" Height="300" Width="100%" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                     <div class="form-group">
                        <%=ViewControlTitle("", "", "", "", "col-sm-3 control-label")%>
                         <div class="col-sm-8">
                             <h4><%=ViewHelp("lblEmailTemplateTitle", "Template Token:")%></h4>
                              <%=ViewHelp("lblEmailTemplate_UserName", "[UserName]:Submission username.")%> 
                                <%=ViewHelp("lblEmailTemplate_CultureInfo", "[CultureInfo]: Submission user's language info.")%> 
                              <%=ViewHelp("lblEmailTemplate_SubmitTime", "[SubmitTime]: Submission date for the form.")%> 
                               <%=ViewHelp("lblEmailTemplate_SubmitIP", "[SubmitIP]:Submission user's IP address.")%> 
                                <%=ViewHelp("lblEmailTemplate_Email", "[Email]:Submission user's email address.")%> 
                               <%=ViewHelp("lblEmailTemplate_Content", "[Content]:Submission details for the form.")%> 
                                <%=ViewHelp("lblEmailTemplate_DisplayName", "[DisplayName]:Submission display name.")%> 
                                 <%=ViewHelp("lblEmailTemplate_LastName", "[LastName]:Submission last name.")%> 
                                 <%=ViewHelp("lblEmailTemplate_FirstName", "[FirstName]:Submission first name.")%>
                                 <%=ViewHelp("lblEmailTemplate_UserRole", "[UserRole]:Submission roles.")%>  
                                 <%=ViewHelp("lblEmailTemplate_Content", "[Content]:Users’submission results will be displayed with the list.")%> 
                                 <%=ViewHelp("lblEmailTemplate_FieldName", "[FieldName]:Display the field contents that the users submitted. [FieldName] is any field that you set, for example: [Name],[Email]")%> 
                         </div>
                    </div>
                   
                  
                </div>
            </div>
        </div>


        <div class="panel panel-default">
            <div class="panel-heading">
                <i class="fa fa-external-link-square"></i>
                <%=ViewTitle("lblTimingSettings", "Timing the sending records")%>
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
                        <%=ViewControlTitle("lblScheduleAdminEmail", "Administrator Email", "txtScheduleSenderEmail", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="control-inline">
                                <asp:TextBox runat="server" ID="txtScheduleSenderEmail" Width="300px" CssClass="form-control validate[required]"></asp:TextBox>
                             </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <%=ViewControlTitle("lblExcelName", "Excel Name", "txtExcelName", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="control-inline">
                                <asp:TextBox runat="server" ID="txtExcelName" Width="300px" CssClass="form-control validate[required]"></asp:TextBox>
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
