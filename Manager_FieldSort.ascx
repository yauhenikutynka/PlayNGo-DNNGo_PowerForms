<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Manager_FieldSort.ascx.cs"
    Inherits="DNNGo.Modules.PowerForms.Manager_FieldSort" %>
<script src="<%=ModulePath %>Resource/plugins/nestable/jquery.nestable.js"></script>
<!-- start: PAGE HEADER -->
<div class="row">
    <div class="col-sm-12">
        <!-- start: PAGE TITLE & BREADCRUMB -->
        <div class="page-header">
            <h1>
                <i class="fa fa-sort"></i>
                <%=ViewResourceText("Header_Title", "Sort Fields")%>
            </h1>
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
                <%=ViewResourceText("Title_SortFields", "Sort Fields")%>
                <div class="panel-tools">
                    <a href="#" class="btn btn-xs btn-link panel-collapse collapses"></a>
                </div>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="dd" id="nestable">
                        <ol class="dd-list ">
                            <asp:Repeater ID="RepeaterFields" runat="server" OnItemDataBound="RepeaterFields_ItemDataBound">
                                <ItemTemplate>
                                    <li class="dd-item dd3-item  sort_hight" data-ID="<%#Eval("ID")%>" data-level="0">
                                        <div class="dd-handle dd3-handle">
                                        </div>
                                        <div class="dd3-content">
                                           <div class="row">
                                                <div class="col-sm-4"><%=ViewResourceText("Title_Name", "Name")%>:<asp:Literal ID="liName" runat="server"></asp:Literal></div>
                                                <%-- <div class="col-sm-2 hidden-xs">Alias:<%#Eval("Alias")%></div>--%>
                                                <div class="col-sm-5 hidden-xs"><%=ViewResourceText("Title_Control", "Control")%>:<asp:Literal ID="liFieldType" runat="server"></asp:Literal></div>
                                                 <div class="col-sm-3 hidden-xs"><%=ViewResourceText("Title_Group", "Group")%>:<asp:Literal ID="liGroup" runat="server"></asp:Literal> </div>
                                           
                                           </div>
                                     
                                        </div>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ol>
                    </div>
                    <asp:HiddenField ID="nestable_output" runat="server"  />
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
<script type="text/javascript">
    <%--    var UINestable = function () {
        //function to initiate jquery.nestable
        var updateOutput = function (e) {
             var list = e.length ? e : $(e.target),
             output = list.data('output');
                var lst = [];
                $('#nestable li').each(function (i) {
                    if($(this).attr("data-sort") != (i+1)*2)
                    {
                        lst.push({ID:$(this).attr("data-id"),Sort:(i+1)*2});
                    }
                });
                output.val(window.JSON.stringify(lst));
        };
        var runNestable = function () {
            // activate Nestable for list 1
            $('#nestable').nestable({
                maxDepth: 1
            }).on('change', updateOutput);
            // output initial serialised data
            updateOutput($('#nestable').data('output', $('#<%=nestable_output.ClientID %>')));
          
        };
        return {
            //main function to initiate template pages
            init: function () {
                runNestable();
            }
        };
    } ();


    jQuery(document).ready(function () {
        UINestable.init();
    });--%>
    jQuery(document).ready(function () {

        var updateOutput = function (e) {
            var list = e.length ? e : $(e.target),
            output = list.data('output');
            var lst = [];
            $('#nestable li').each(function (i) {
                if ($(this).attr("data-sort") != (i + 1) * 2) {
                    lst.push({ ID: $(this).attr("data-id"), Sort: (i + 1) * 2 });
                }
            });
            output.val(window.JSON.stringify(lst));
        };
        $('#nestable').DDSort({
            up: function () {
                updateOutput($('#nestable').data('output', $('#<%=nestable_output.ClientID %>')));
            }
        });
    });



</script>
