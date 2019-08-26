<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Manager_Settings_Push.ascx.cs" Inherits="DNNGo.Modules.PowerForms.Manager_Settings_Push" %>
<!-- start: PAGE HEADER -->
<div class="row">
    <div class="col-sm-12">
        <!-- start: PAGE TITLE & BREADCRUMB -->
        
        <div class="page-header">
            <h1>
                <i class="fa fa-wrench"></i>
                <%=ViewResourceText("Header_Title", "Push Settings")%></h1>
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
                <%=ViewTitle("lblDataSettings", "Push Form Data Settings")%>
                <div class="panel-tools">
                    <a href="#" class="btn btn-xs btn-link panel-collapse collapses"></a>
                </div>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">

                    <div class="form-group">
                        <%=ViewControlTitle("lblPushEnable", "Push Enable", "cbPushEnable", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-9">
                            <div class="checkbox-inline">
                                <asp:CheckBox ID="cbPushEnable" runat="server" CssClass="auto" />
                             </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <%=ViewControlTitle("lblAsynchronous", "Asynchronous", "cbAsynchronous", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-9">
                            <div class="checkbox-inline">
                                <asp:CheckBox ID="cbAsynchronous" runat="server" CssClass="auto" />
                             </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <%=ViewControlTitle("lblFormMethod", "Form Method", "ddlFormMethod", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-9">
                            <div class="control-inline">
                                <asp:DropDownList ID="ddlFormMethod" runat="server" CssClass="form-control validate[required]" Width="100"></asp:DropDownList>
                             </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <%=ViewControlTitle("lblTransferUrl", "Transfer Url", "txtTransferUrl", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-9">
                            <div class="control-inline">
                                <asp:TextBox runat="server" ID="txtTransferUrl" Width="400" CssClass="input_text validate[required,maxSize[1024],custom[url]]"></asp:TextBox><br />
                                <%=ViewHelp("lblTransferUrl", "Default test page:http://www.dnngo.net/OurModules/PowerForms/FormPush.aspx")%>
                             </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <%=ViewControlTitle("lblQueryString", "Query String", "txtQueryString", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-9">
                            <div class="control-inline">
                                <asp:TextBox runat="server" ID="txtQueryString" Width="400" TextMode="MultiLine" Rows="5" CssClass="input_text validate[maxSize[1024]]"></asp:TextBox>
                             </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <%=ViewControlTitle("lblAppVerify", "App Verify", "txtAppVerify", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-9">
                            <div class="control-inline">
                                <asp:TextBox runat="server" ID="txtAppVerify" Width="400" CssClass="input_text validate[required,maxSize[32],minSize[32]]"></asp:TextBox><br />
                                <%=ViewHelp("lblAppVerify", "Query String: AppVerify=XXXXXXXXXXXXX ")%>
                             </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <%=ViewControlTitle("lblVerifyUrl", "Verify Url", "hlVerifyUrl", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-9">
                            <div class="control-inline">
                                <asp:HyperLink ID="hlVerifyUrl" runat="server" Target="_blank"  Text="http://www.dnngo.net/OurModules/PowerForms/FormPush.aspx"></asp:HyperLink><br />
                                <%=ViewHelp("lblVerifyUrl", "Click to open the test page.")%>
                             </div>
                        </div>
                    </div>


                </div>
            </div>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading">
                <i class="fa fa-external-link-square"></i>
                <%=ViewTitle("lblDataFilter", "Request Header Settings")%>
                <div class="panel-tools">
                    <a href="#" class="btn btn-xs btn-link panel-collapse collapses"></a>
                </div>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                   <div class="form-group">
                        <%=ViewControlTitle("lblAcceptLanguage", "Accept Language", "txtAcceptLanguage", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="control-inline">
                                <asp:TextBox runat="server" ID="txtAcceptLanguage" Width="200" CssClass="input_text validate[required,maxSize[1024]]"></asp:TextBox><br />
                                <%=ViewHelp("lblAcceptLanguage", "The Accept-Langauge header, which specifies that natural languages that are preferred for the response.")%>
                             </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <%=ViewControlTitle("lblAcceptEncoding", "Accept Encoding", "txtAcceptEncoding", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="control-inline">
                                <asp:TextBox runat="server" ID="txtAcceptEncoding" Width="200" CssClass="input_text validate[required,maxSize[1024]]"></asp:TextBox><br />
                                <%=ViewHelp("lblAcceptEncoding", "The Accept-Encoding header, which specifies the content encodings that are acceptable for the response.")%>
                             </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <%=ViewControlTitle("lblAccept", "Accept", "txtAccept", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="control-inline">
                                <asp:TextBox runat="server" ID="txtAccept" Width="90%" CssClass="input_text validate[required,maxSize[1024]]"></asp:TextBox><br />
                                <%=ViewHelp("lblAccept", "The Accept header, which specifies the MIME types that are acceptable for the response.")%>
                             </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <%=ViewControlTitle("lblUserAgent", "User Agent", "txtUserAgent", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-5">
                            <div class="control-inline">
                                <asp:TextBox runat="server" ID="txtUserAgent" Width="90%" CssClass="input_text validate[required,maxSize[1024]]"></asp:TextBox><br />
                                <%=ViewHelp("lblUserAgent", "The User-Agent header, which specifies information about the client agent.")%>
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

