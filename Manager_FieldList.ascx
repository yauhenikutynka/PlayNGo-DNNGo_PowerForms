<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Manager_FieldList.ascx.cs" Inherits="DNNGo.Modules.PowerForms.Manager_FieldList" %>
   <!-- start: PAGE HEADER -->
      <div class="row">
        <div class="col-sm-12"> 
          <!-- start: PAGE TITLE & BREADCRUMB -->
          
          <div class="page-header">
            <h1><i class="fa clip-list-4"></i> <%=ViewResourceText("Header_Title", "All Fields")%>
             
                <asp:HyperLink ID="hlLinkAddField" runat="server" CssClass="btn btn-xs btn-bricky" Text="<i class='fa fa-plus'></i> New Add Field" resourcekey="hlLinkAddField"></asp:HyperLink> 
              
                <asp:HyperLink ID="hlLinkSortFields" runat="server" CssClass="btn btn-xs btn-bricky" Text="<i class='fa fa-sort'></i> Sort Fields" resourcekey="hlLinkSortFields"></asp:HyperLink> 
            </h1>
          </div>
          <!-- end: PAGE TITLE & BREADCRUMB --> 
        </div>
      </div>
      <!-- end: PAGE HEADER --> 
      <!-- start: PAGE CONTENT -->
      
      <div class="row">
        <div class="col-sm-12">
          <div class="form-group">
            <div class="row">
              <div class="col-sm-8">
                  <div class="btn-group">
                    <asp:HyperLink runat="server" ID="hlAllFields" CssClass="btn btn-default" Text="All"></asp:HyperLink>
                    <asp:HyperLink runat="server" ID="hlActivationField" CssClass="btn btn-default" Text="Activation"></asp:HyperLink>
                    <asp:HyperLink runat="server" ID="hlHideField" CssClass="btn btn-default" Text="Hide"></asp:HyperLink>
                   
                  </div>
              </div>
                <div class="col-sm-4 input-group text_right">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" Width="100%" placeholder="Search Text Field" x-webkit-speech></asp:TextBox>
                <div class="input-group-btn">
                    <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-primary" 
                      Text="<i class='fa fa-search'></i>" onclick="btnSearch_Click" />
				</div>
                
              </div>
            </div>
          </div>
          <div class="form-group">
            <div class="row">
              <div class="col-sm-9">
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control form_default">
                    <asp:ListItem Value="-1" Text="Bulk Actions" resourcekey="ddlStatus_BulkActions"></asp:ListItem>
                    <asp:ListItem Value="1" Text="Activation" resourcekey="ddlStatus_Activation"></asp:ListItem>
                    <asp:ListItem Value="0" Text="Hide" resourcekey="ddlStatus_Hide"></asp:ListItem>
                  
                </asp:DropDownList>
                <asp:Button ID="btnApply" runat="server" CssClass="btn btn-default" Text="Apply" resourcekey="btnApply" onclick="btnApply_Click" OnClientClick="return ApplyStatus();" />

              </div>
              <div class="col-sm-3 text_right"> <br/>
                <asp:Label ID="lblRecordCount" runat="server"></asp:Label> </div>
            </div>
          </div>
          <div class="form-group">
            <asp:GridView ID="gvFieldList" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvFieldList_RowDataBound" OnRowCreated="gvFieldList_RowCreated" OnSorting="gvFieldList_Sorting" AllowSorting="true"
                        Width="100%" CellPadding="0" cellspacing="0" border="0" CssClass="table table-striped table-bordered table-hover"  GridLines="none" >
                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText="Name" /> 
                            <asp:BoundField DataField="Alias" HeaderText="Alias" /> 
                            <asp:BoundField DataField="GroupID" HeaderText="Group" HeaderStyle-Width="120" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs"/> 
                            <asp:BoundField DataField="FieldType" HeaderText="Control" HeaderStyle-Width="120" /> 
                            <asp:BoundField DataField="StartTime" HeaderText="Start Time"  HeaderStyle-Width="120" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs"/> 
                            <asp:BoundField DataField="EndTime" HeaderText="End Time"  HeaderStyle-Width="120" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs"/> 
                            <asp:BoundField DataField="CreateUser" HeaderText="Create User" HeaderStyle-Width="100" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs"/> 
                            <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-Width="50" /> 
                            <asp:TemplateField HeaderText="Sort" HeaderStyle-Width="80" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs" Visible="false" >
                                <ItemTemplate>
                                    <div class="visible-md visible-lg hidden-sm hidden-xs">
                                        <asp:LinkButton ID="lbSortUp" runat="server" ToolTip="up" CssClass="btn btn-xs btn-default tooltips" data-original-title="Sort up" data-placement="top" Text="<i class='fa fa-arrow-up'></i>" OnClick="lbSort_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lbSortDown" runat="server" ToolTip="down" CssClass="btn btn-xs btn-default tooltips" data-original-title="Sort down" data-placement="top" Text="<i class='fa fa-arrow-down'></i>" OnClick="lbSort_Click"></asp:LinkButton>
                                     </div>
                                    
                             </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="center" HeaderStyle-Width="80">
                                <ItemTemplate>
                                     <div class="visible-md visible-lg hidden-sm hidden-xs">
                                       <asp:HyperLink ID="hlEdit" runat="server" CssClass="btn btn-xs btn-teal tooltips" data-original-title="Edit" data-placement="top" Text="<i class='fa fa-edit'></i>"></asp:HyperLink>
                                        <asp:LinkButton ID="btnRemove" runat="server" CssClass="btn btn-xs btn-bricky tooltips" data-original-title="Remove" data-placement="top" Text="<i class='fa fa-times fa fa-white'></i>" OnClick="btnRemove_Click"></asp:LinkButton>
                                         
                                     </div>
                                    <div class="visible-xs visible-sm hidden-md hidden-lg">
                                      <div class="btn-group"> <a href="#" data-toggle="dropdown" class="btn btn-primary dropdown-toggle btn-sm"> <i class="fa fa-cog"></i> <span class="caret"></span> </a>
                                        <ul class="dropdown-menu pull-right" role="menu">
                                            <li role="presentation" style="display:none;"> <asp:LinkButton ID="lbMobileSortUp" runat="server" ToolTip="up" tabindex="-1" role="menuitem" data-placement="top" Text="<i class='fa clip-arrow-up-3'></i> up"  OnClick="lbSort_Click"></asp:LinkButton></li>
                                            <li role="presentation" style="display:none;"> <asp:LinkButton ID="lbMobileSortDown" runat="server" ToolTip="down" tabindex="-1" role="menuitem" data-placement="top" Text="<i class='fa clip-arrow-down-3'></i> down"  OnClick="lbSort_Click"></asp:LinkButton></li>
                                            <li role="presentation">  <asp:HyperLink ID="hlMobileEdit" runat="server" tabindex="-1" role="menuitem" Text="<i class='fa fa-edit'></i> Edit"></asp:HyperLink></li>
                                            <li role="presentation"> <asp:LinkButton ID="btnMobileRemove" runat="server" tabindex="-1" role="menuitem" data-placement="top" Text="<i class='fa fa-times'></i> Remove"  OnClick="btnRemove_Click"></asp:LinkButton></li>
                                        </ul>
                                      </div>
                                    </div>
                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                        </Columns>
                        <PagerSettings Visible="False" />
                    </asp:GridView>
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
      </div>


 <script type="text/javascript">
<!--

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
             if (e[i].type == "checkbox") {
                 e[i].checked = IsTrue;
             }
         }
     }
     function ApplyStatus() {
         var StatusSelected = $("#<%=ddlStatus.ClientID %>").find("option:selected").val();
         if (StatusSelected >= 0) {
             var checkok = false;
             $("#<%=gvFieldList.ClientID %> input[type='checkbox'][type-item='true']").each(function (i, n) {
                 if ($(this).prop('checked')) {
                     checkok = true;
                 }
             });

             if (checkok) {
                 return confirm('<%=ViewResourceText("Confirm_ApplyStatus", "Are you sure to operate the records you choose?") %>');
             }
             alert('<%=ViewResourceText("Alert_NoItems", "Please operate with one line of record selected at least.") %>');

         } else {
             alert('<%=ViewResourceText("Alert_NoActions", "Please choose the operation you need.") %>');
         }
         return false;
     }
   
 
// -->
    </script>