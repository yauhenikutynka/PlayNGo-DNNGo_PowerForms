<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Manager_Settings_Anti-Spam.ascx.cs" Inherits="DNNGo.Modules.PowerForms.Manager_Settings_Anti_Spam" %>
<!-- start: PAGE HEADER -->
<div class="row">
    <div class="col-sm-12">
        <!-- start: PAGE TITLE & BREADCRUMB -->
        
        <div class="page-header">
            <h1>
                <i class="fa fa-wrench"></i>
                <%=ViewResourceText("Header_Title", "Anti-Spam Settings")%></h1>
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
                <%=ViewTitle("lblCaptchaSettings", "reCAPTCHA v3 Settings")%>
                <div class="panel-tools">
                    <a href="#" class="btn btn-xs btn-link panel-collapse collapses"></a>
                </div>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <%=ViewControlTitle("lblCaptchaEnable", "reCAPTCHA Enable", "cbCaptchaEnable", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-8">
                            <div class="checkbox-inline">
                               <asp:CheckBox ID="cbCaptchaEnable" runat="server" CssClass="auto" />
                                 <%=ViewHelp("lblCaptchaEnable","To use reCAPTCHA ( v3 ), you need to <a href='http://www.google.com/recaptcha/admin' target='_blank'>sign up for an API key pair</a> for your site. The key pair consists of a <b>site key</b> and <b>secret</b>.") %>
                            </div>
                        </div>
                    </div>

                    <div class="form-group">
                        <%=ViewControlTitle("lblCaptchaSiteKey", "Site key", "txtCaptchaSiteKey", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-8">
                            <div class="control-inline">
                               <asp:TextBox runat="server" ID="txtCaptchaSiteKey" Width="350px"  CssClass="form-control"></asp:TextBox>
                                <%=ViewHelp("lblCaptchaSiteKey","Use this in the code your site serves to users.") %>
                            </div>
                        </div>
                    </div>

                   <div class="form-group">
                        <%=ViewControlTitle("lblCaptchaSecretKey", "Secret key", "txtCaptchaSecretKey", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-8">
                            <div class="control-inline">
                               <asp:TextBox runat="server" ID="txtCaptchaSecretKey" Width="350px"  CssClass="form-control"></asp:TextBox>
                                <%=ViewHelp("lblCaptchaSecretKey","Use this for communication between your site and Google. Be sure to keep it a secret.") %>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <%=ViewControlTitle("lblCaptchaSocial", "Social", "txtLimitSocial", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-8">
                            <div class="control-inline">
                               <asp:TextBox runat="server" ID="txtLimitSocial" Width="50px"  CssClass="form-control  validate[required,custom[number]]"></asp:TextBox>
                                <%=ViewHelp("lblCaptchaSocial","0.1 - 1, The smaller the numerical value, the looser the restriction. Limit unanswered friend requests from abusive users") %>
                            </div>
                        </div>
                    </div>

<%--                    <div class="form-group">
                        <%=ViewControlTitle("lblCaptchaTabindex", "Tabindex", "txtCaptchaTabindex", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-8">
                            <div class="control-inline">
                               <asp:TextBox runat="server" ID="txtCaptchaTabindex" Width="50px"  CssClass="form-control  validate[required,custom[integer]]"></asp:TextBox>
                                <%=ViewHelp("lblCaptchaTabindex","The tabindex of the widget and challenge. If other elements in your page use tabindex, it should be set to make user navigation easier.") %>
                            </div>
                        </div>
                    </div>

                   <div class="form-group">
                        <%=ViewControlTitle("lblCaptchaTheme", "Theme", "ddlCaptchaTheme", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-8">
                            <div class="control-inline">
                                <asp:DropDownList ID="ddlCaptchaTheme" runat="server" CssClass="form-control" Width="200">
                                    <asp:ListItem Text="light" Value="light"></asp:ListItem>
                                    <asp:ListItem Text="dark" Value="dark"></asp:ListItem>
                                </asp:DropDownList>
                         
                                <%=ViewHelp("lblCaptchaTheme","The color theme of the widget.") %>
                            </div>
                        </div>
                    </div>
                       <div class="form-group">
                        <%=ViewControlTitle("lblCaptchaType", "Type", "ddlCaptchaType", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-8">
                            <div class="control-inline">
                                <asp:DropDownList ID="ddlCaptchaType" runat="server" CssClass="form-control" Width="200">
                                    <asp:ListItem Text="image" Value="image"></asp:ListItem>
                                    <asp:ListItem Text="audio" Value="audio"></asp:ListItem>
                                </asp:DropDownList>
                         
                                <%=ViewHelp("lblCaptchaType","The type of CAPTCHA to serve.") %>
                            </div>
                        </div>
                    </div>
                       <div class="form-group">
                        <%=ViewControlTitle("lblCaptchaSize", "Size", "ddlCaptchaSize", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-8">
                            <div class="control-inline">
                                <asp:DropDownList ID="ddlCaptchaSize" runat="server" CssClass="form-control" Width="200">
                                    <asp:ListItem Text="normal" Value="normal"></asp:ListItem>
                                    <asp:ListItem Text="compact" Value="compact"></asp:ListItem>
                                </asp:DropDownList>
                                <%=ViewHelp("lblCaptchaSize","The size of the widget.") %>
                            </div>
                        </div>
                    </div>--%>


                  
                </div>
            </div>
        </div>


        <div class="panel panel-default">
            <div class="panel-heading">
                <i class="fa fa-external-link-square"></i>
                <%=ViewTitle("lblEncryptSettings", "Hidden fields encrypt Settings")%>
                <div class="panel-tools">
                    <a href="#" class="btn btn-xs btn-link panel-collapse collapses"></a>
                </div>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <%=ViewControlTitle("lblHiddenfieldsEnable", "Hidden fields", "cbHiddenfieldsEnable", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="checkbox-inline">
                                <asp:CheckBox ID="cbHiddenfieldsEnable" runat="server" CssClass="auto"/>
                             </div>
                        </div>
                    </div>



                   <div class="form-group">
                        <%=ViewControlTitle("lblEncryptionKey", "Encryption key", "txtEncryptionKey", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="control-inline">
                                <asp:TextBox runat="server" ID="txtEncryptionKey" Width="150px" CssClass="form-control validate[required,maxSize[20],minSize[8]]"></asp:TextBox>
                             </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <%=ViewControlTitle("lblVerifyStringLength", "Verify String Length", "txtVerifyStringLength", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="control-inline">
                                <asp:TextBox runat="server" ID="txtVerifyStringLength" Width="80px" CssClass="form-control validate[required,max[30],min[5]]"></asp:TextBox>
                             </div>
                        </div>
                    </div>


                    <div class="form-group">
                        <%=ViewControlTitle("lblVerifyIntervalTime", "Verify Interval Time", "txtVerifyIntervalTime", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="control-inline">
                                <asp:TextBox runat="server" ID="txtVerifyIntervalTime" Width="80px" CssClass="form-control validate[required,max[60],min[1]]"></asp:TextBox>
                                  <%=ViewHelp("lblVerifyIntervalTime", "Minute")%>
                                
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

