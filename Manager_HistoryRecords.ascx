<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Manager_HistoryRecords.ascx.cs" Inherits="DNNGo.Modules.PowerForms.Manager_HistoryRecords" %>
<!-- start: PAGE HEADER -->
<div class="row">
    <div class="col-sm-12">
        <!-- start: PAGE TITLE & BREADCRUMB -->
        
        <div class="page-header">
            <h1>
                <i class="fa clip-history"></i> <%=ViewResourceText("Header_Title", "History Records")%></h1>
        </div>
        <!-- end: PAGE TITLE & BREADCRUMB -->
    </div>
</div>
<!-- end: PAGE HEADER -->


<!-- start: PAGE CONTENT -->
      
      <div class="row">
        <div class="col-sm-5"></div>

        <div class="col-sm-3   input-group text_right">
                  <div class="input-group input-append bootstrap-timepicker">
                    <asp:TextBox ID="txtBeginDate" runat="server" placeholder="Start Date" data-date-format="mm/dd/yyyy" data-date-viewmode="years" CssClass="form-control date-picker"></asp:TextBox>
                    <span class="input-group-addon"> <i class="fa fa-calendar"></i> </span> 
                 </div>
        </div>
        <div class="col-sm-3  input-group text_right">
                <div class="input-group input-append bootstrap-timepicker">
                  <asp:TextBox ID="txtEndDate" runat="server" placeholder="End Date" data-date-format="mm/dd/yyyy" data-date-viewmode="years" CssClass="form-control date-picker"></asp:TextBox>
                  <span class="input-group-addon"><i class="fa fa-calendar"></i></span> 
                </div>  
        </div>
         <div class="col-sm-1">
            <asp:Button ID="lbSearch" runat="server" CssClass="btn btn-primary btn-sm" resourcekey="lbSearch"  OnClick="lbSearch_Click" Text="Search"></asp:Button>
         </div> 
    </div>
    
    <div class="form-group">
      <div class="row">
        <div class="col-sm-9"> 



            <input name='CheckboxAll' id='CheckboxAll' type='checkbox' value='0' onclick="SelectAll()" /> <label
                for="CheckboxAll"><%=ViewResourceText("lblSelectAll", "Select All")%></label> 
                <asp:Button ID="cmdDelete" runat="server" CssClass="btn btn-default" resourcekey="cmdDelete"
                OnClick="cmdDelete_Click" Text="Delete" OnClientClick="CancelValidation();return deleteop();"></asp:Button>
            <asp:Button ID="cmdCleanup" runat="server" CssClass="btn btn-default" resourcekey="cmdCleanup"
                OnClick="cmdCleanup_Click" Text="Cleanup All" OnClientClick="CancelValidation();"></asp:Button>
            <asp:Button ID="cmdExport" runat="server" CssClass="btn btn-default" resourcekey="cmdExport"
                OnClick="cmdExport_Click" Text="Export" OnClientClick="CancelValidation();return exportop();"></asp:Button>
            <asp:Button ID="cmdExportAll" runat="server" CssClass="btn btn-default" resourcekey="cmdExportAll"
                OnClick="cmdExportAll_Click" Text="Export All" OnClientClick="CancelValidation();"></asp:Button>


         
        </div>
        <div class="col-sm-3 text_right" style="padding-top:8px;">
          <label for="CheckboxExpandAll"><%=ViewResourceText("lblExpandAll", "Expand All")%></label>  
          <input name='CheckboxExpandAll' id='CheckboxExpandAll' type='checkbox'  />
        </div>
      </div>
    </div>
    <div class="form-group">



         <asp:Repeater runat="server" ID="repeaterContent" OnItemDataBound="repeaterContent_ItemDataBound">
                        <ItemTemplate>
                        <!-- start: TEXT AREA PANEL -->
                        <div class="panel panel-default">
                            <div class="panel-heading">
                              <%--  <i class="fa fa-external-link-square"></i>--%>
                                <input name='Checkbox' id='Checkbox' lang="select" class="fa fa-external" value='<%#Eval("ID") %>' type='checkbox' />
                                <div class="row">
                                    <div class="col-sm-3">
                                         <%=ViewTitle("lblSumbitTime", "Time")%>:
                                                <%#Eval("LastTime") %>
                                    </div>
                                   
                                    <div class="col-sm-5">
                                           <%=ViewTitle("lblUser", "User")%>:
                                                <asp:Label ID="lblUserName" runat="server"></asp:Label> (<asp:Label ID="lblEmail" runat="server"></asp:Label>)
                                    </div>
                                     <div class="col-sm-3">
                                           <%=ViewTitle("lblFormVersion", "Version")%>:
                                           <asp:Label ID="lblFormVersion" runat="server"></asp:Label>
                                    </div>
                                </div>

                                <div class="panel-tools">
                                    <a href="#" class="btn btn-xs btn-link panel-collapse expand"></a>
                                </div>
                            </div>
                            <div class="panel-body" style="display: none;">
                                <div class="form-horizontal">
                                    <asp:GridView ID="gvItemList" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvItemList_RowDataBound"
                                        Width="100%" CellPadding="0" CellSpacing="0" border="0" CssClass="table table-bordered table-hover"
                                        GridLines="none">
                                        <RowStyle CssClass="td_row" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Number" ItemStyle-Width="50">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- <asp:BoundField DataField="FieldID" HeaderText="ID" HeaderStyle-Width="50" /> --%>
                                            <asp:BoundField DataField="FieldName" HeaderText="Name" HeaderStyle-Width="80" />
                                            <asp:BoundField DataField="FieldAlias" HeaderText="Alias" HeaderStyle-Width="80" />
                                            <asp:BoundField DataField="Group" HeaderText="Group" HeaderStyle-Width="120" />
                                            <%--<asp:BoundField DataField="ContentValue" HeaderText="Content" />--%>
                                             <asp:TemplateField HeaderText="Content"  >
                                                <ItemTemplate> 
                                                    <asp:Literal ID="LiContentValue" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Visible="False" />
                                        <FooterStyle />
                                        <PagerStyle />
                                        <SelectedRowStyle />
                                        <HeaderStyle />
                                        <AlternatingRowStyle CssClass="alternating_row" />
                                    </asp:GridView>


                                  </div>
                            </div>
                            <!-- end: TEXT AREA PANEL -->
                        </div>
                        </ItemTemplate>
                        <FooterTemplate>
                            <% 
                                if (repeaterContent.Items.Count == 0)
                                {
                            %>
                            <div class="submit_list_title">
                                <%=ViewGridViewEmpty() %></div>
                            <%} %>
                        </FooterTemplate>
                    </asp:Repeater>
                    <ul id="paginator-FieldList" class="pagination-purple"></ul>
                    <script type="text/javascript">
                        $(document).ready(function () {
                            $('#paginator-FieldList').bootstrapPaginator({
                                bootstrapMajorVersion: 3,
                                currentPage: <%=PageIndex %>,
                                totalPages: <%=RecordPages %>,
                                numberOfPages:7,
                                useBootstrapTooltip:true,
                                onPageClicked: function (e, originalEvent, type, page) {
                                    window.location.href='<%=CurrentUrl %>&PageIndex='+ page;
                                }
                            });
                        });
                    </script>

      
    </div>  
 </div>
  
  <!-- end: PAGE CONTENT--> 
 <script type="text/javascript">
     jQuery(function (q) {
         var dates = q("#<%=txtBeginDate.ClientID %>, #<%=txtEndDate.ClientID %>").datepicker({
             dateFormat: 'mm/dd/yy',
             showButtonPanel: true, changeMonth: true, changeYear: true,
             onSelect: function (selectedDate) {
                 var option = this.id == "<%=txtBeginDate.ClientID %>" ? "minDate" : "maxDate",
					instance = q(this).data("datepicker"),
					date = q.datepicker.parseDate(
						instance.settings.dateFormat ||
						q.datepicker._defaults.dateFormat,
						selectedDate, instance.settings);
                 dates.not(this).datepicker("option", option, date);
             }
         });
     });

     jQuery(function (q) {

         q("#CheckboxExpandAll").click(function () {
             var checked = q(this).prop('checked');

             q(".panel-tools .panel-collapse").each(function (i, n) {
                 var el = jQuery(this).parent().closest(".panel").children(".panel-body");
                 if (!checked) {
                     $(this).addClass("expand").removeClass("collapses");
                     el.slideUp(200);
                 } else {
                     $(this).addClass("collapses").removeClass("expand");
                     el.slideDown(200);
                 }
             });
             
         });
     });



     function SelectAll() {
         var e = document.getElementsByTagName("input");
         var IsTrue;
         if (document.getElementById("CheckboxAll").value == "0") {
             IsTrue = true;
             document.getElementById("CheckboxAll").value = "1"
         }
         else {
             IsTrue = false;
             document.getElementById("CheckboxAll").value = "0"
         }
         for (var i = 0; i < e.length; i++) {
             if (e[i].type == "checkbox" && e[i].lang == "select") {
                 e[i].checked = IsTrue;
             }
         }
     }

     function deleteop() {
         var checkok = false;
         var e = document.getElementsByTagName("input");
         for (var i = 0; i < e.length; i++) {
             if (e[i].type == "checkbox") {
                 if (e[i].checked == true) {
                     checkok = true;
                     break;
                 }
             }
         }
         if (checkok)
             return confirm('<%=ViewResourceText("lblcheckok", "This action can not be undone, are you sure to delete?")%>');
         else {

             alert('<%=ViewResourceText("lblcheckconfirm", "Please select the records needs to be deleted!")%>');
             return false;
         }
     }


     function exportop() {
         var checkok = false;
         var e = document.getElementsByTagName("input");
         for (var i = 0; i < e.length; i++) {
             if (e[i].type == "checkbox") {
                 if (e[i].checked == true) {
                     checkok = true;
                     break;
                 }
             }
         }
         if (checkok)
             return confirm('<%=ViewResourceText("lblcheckok_export","Are you sure to export?")%>');
         else {

             alert('<%=ViewResourceText("lblcheckconfirm_export", "Please select the records needs to be exported!")%>');
             return false;
         }
     }

</script>
