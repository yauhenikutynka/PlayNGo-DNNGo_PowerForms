<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View_PaymentConfirm.ascx.cs" Inherits="DNNGo.Modules.PowerForms.View_PaymentConfirm" %>
<div id="form_div_<%=ModuleId %>">
 
    <asp:Literal ID="liContent" runat="server"></asp:Literal>
    <asp:Button runat="server" ID="butCheckOut" OnClick="butCheckOut_Click" Visible="false" />
    <asp:Button runat="server" ID="butReturn" OnClick="butReturn_Click" Visible="false" />

</div>

