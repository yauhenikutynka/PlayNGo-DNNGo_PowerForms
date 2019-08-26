<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="DNNGo.Modules.PowerForms.Settings" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Security.Permissions.Controls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="dnn" TagName="URL" Src="~/controls/URLControl.ascx" %>

<div class="options_views">
    <h2 class="setting_page_title">
        <%=ViewTitle("lblModuleTitle", "Module Settings")%></h2>
    <div class="choose_tags form_field handlediv">
        <h3 class="hndle">
            <%=ViewTitle("lblBasicSetting", "Basic Settings")%></h3>
        <div class="inside">
        
 
           <div class="form_field">
                <h4>
                    <%=ViewTitle("lblPageSize", "Lists per page", "txtPageSize")%>:</h4>
                <asp:TextBox runat="server" ID="txtPageSize" Width="100px" CssClass="input_text validate[required,custom[integer]]"></asp:TextBox>
            </div>
<!--            <div class="form_field">
                <h4>
                    <%=ViewTitle("lblRedirectType", "Redirect", "rblRedirectType")%>:</h4>
                    <asp:RadioButtonList  RepeatDirection="Horizontal" ID="rbleRedirectType" runat="server"></asp:RadioButtonList>
            </div>-->
           <div class="form_field">
                <h4>
                    <%=ViewTitle("lblRedirectPage", "Redirect results page", "ucRedirectPage")%>:</h4>
                    <dnn:URL ID="ucRedirectPage" runat="server" ShowTabs="true" UrlType="N" ShowNewWindow="false"
                    ShowNone="true" Visible="true" ShowSecure="false" ShowDatabase="false" ShowLog="false"
                    ShowTrack="false" ShowFiles="true" ShowUrls="true" />
            </div>
            <div class="form_field">
                <h4>
                    <%=ViewTitle("lblExtraTracking", "Extra Tracking", "cbExtraTracking")%>:</h4>
                    <asp:CheckBox ID="cbExtraTracking" runat="server" />
            </div>
        </div>
    </div>

      <div class="choose_tags form_field handlediv">
        <h3 class="hndle">
            <%=ViewTitle("lblViewUserSetting", "View User Setting")%></h3>
        <div class="inside">
            <div class="form_field">
                <h4>
                    <%=ViewTitle("lblViewUserRoles", "View User Roles", "cblViewUserRoles")%>:</h4>
                    <asp:CheckBoxList ID="cblViewUserRoles" runat="server"></asp:CheckBoxList>
                    <dnn:modulepermissionsgrid id="dgPermissions" runat="server"/>
            </div>
           

        </div>
    </div>


    

    
 
  
    <p style="text-align: center;">
        <asp:Button CssClass="input_button" data-verify="Submit" ID="cmdUpdate" resourcekey="cmdUpdate"
            runat="server" Text="Update" OnClick="cmdUpdate_Click"></asp:Button>&nbsp;
        <asp:Button CssClass="input_button" ID="cmdCancel" resourcekey="cmdCancel" runat="server"
            Text="Cancel" CausesValidation="False" OnClick="cmdCancel_Click"  OnClientClick="CancelValidation();"></asp:Button>&nbsp;
    </p>
</div>

