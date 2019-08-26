<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Manager_FieldItem.ascx.cs" Inherits="DNNGo.Modules.PowerForms.Manager_FieldItem" %>
<script src="<%=ModulePath %>Resource/plugins/tinymce/tinymce.min.js"></script>
<!-- start: PAGE HEADER -->
      <div class="row">
        <div class="col-sm-12"> 
          <!-- start: PAGE TITLE & BREADCRUMB -->
          
          <div class="page-header">
            <h1><i class="fa fa-plus"></i> <%=ViewResourceText("Header_Title", "New add & Edit Field")%></h1>
          </div>
          <!-- end: PAGE TITLE & BREADCRUMB --> 
        </div>
      </div>
      <!-- end: PAGE HEADER --> 


      
      <!-- start: PAGE CONTENT -->
      
      <div class="row">
        <div class="col-sm-8">
          <div class="row">
            <div class="form-horizontal">
              <div class="form-group">
                <%=ViewControlTitle("lblName", "Name", "txtName", ":", "col-sm-3 control-label")%>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtName" runat="server" Width="330" CssClass="form-control NoWrapTextBox validate[required,custom[onlyLetterNumber],maxSize[100]]"></asp:TextBox>
                    <i id="iName" class="fa clip-circle-small " style="color:Red"></i>
                    <%--<img id="png_captcha" src="" style="display:none" alt="" />--%>
                    <%=ViewHelp("lblName", "This is a field name and not used to display on the form. Only characters from a-Z and 0-9 can be allowed.")%>
                </div>
              </div>
     
              <div class="form-group">
                <%=ViewControlTitle("lblAlias", "Alias", "txtAlias", ":", "col-sm-3 control-label")%>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtAlias" runat="server" Width="330" CssClass="form-control NoWrapTextBox validate[required,maxSize[100]]"></asp:TextBox>
                    <i class="fa clip-circle-small " style="color:Red"></i>
                    <%=ViewHelp("lblAlias", "This is a filed name and only used to display on the form.")%>
                </div>
              </div>


              <div class="form-group">
                <%=ViewControlTitle("lblToolTip", "ToolTip", "txtToolTip", ":", "col-sm-3 control-label")%>
                <div class="col-sm-9">
                    <div class="control-inline">
                        <asp:TextBox ID="txtToolTip" runat="server" Width="330" TextMode="MultiLine" Rows="2" CssClass=" validate[maxSize[200]]"></asp:TextBox>
                    </div>
                </div>
              </div> 
              <div class="form-group">
                <%=ViewControlTitle("lblDescription", "Description", "txtDescription", ":", "col-sm-3 control-label")%>
                <div class="col-sm-9">
                 <div class="control-inline">
                    <asp:TextBox ID="txtDescription" TextMode="MultiLine" Rows="4" runat="server" Width="330" CssClass=" validate[maxSize[512]]"></asp:TextBox>
                 </div>
                </div>
              </div> 
              
              
            </div>
          </div>
  


          <div class="panel panel-default">
            <div class="panel-heading"> <i class="fa fa-external-link-square"></i> <%=ViewResourceText("Title_ControlSettings", "Control Settings")%>
              <div class="panel-tools"> <a href="#" class="btn btn-xs btn-link panel-collapse collapses"> </a> </div>
            </div>
            <div class="panel-body">
              <div class="row">
                <div class="form-horizontal">
                     <div class="form-group">
                        <%=ViewControlTitle("lblFieldType", "Control Type", "ddlControlType", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-9">
                            <asp:DropDownList ID="ddlControlType" runat="server" CssClass="form-control form_default">
                            </asp:DropDownList>
                        </div>
                      </div>
                     <div class="form-group ControlHide" id="trFTDirection">
                        <%=ViewControlTitle("lblFTDirection", "Direction", "rblDirection", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-9">
                             <asp:RadioButtonList ID="rblFTDirection" runat="server" RepeatDirection="Vertical" CssClass="auto  radio-inline"></asp:RadioButtonList>
                        </div>
                      </div>

                    <div class="form-group ControlHide" id="trFTListColumn">
                        <%=ViewControlTitle("lblFTListColumn", "Column", "txtFTListColumn", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-9">
 
                              <asp:TextBox ID="txtFTListColumn" runat="server" Width="100" CssClass="form-control NoWrapTextBox validate[required]"></asp:TextBox>
                              <%=ViewHelp("lblFTListColumn", "This is for you to set number of columns for the control and it is valid only when Horizontal above is selected.")%>
                        </div>
                      </div>

                     <div class="form-group ControlHide" id="trFTWidth">
                        <%=ViewControlTitle("lblFTWidth", "Width", "txtFTWidth", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtFTWidth" runat="server" CssClass="form-control NoWrapTextBox validate[required,custom[integer]]" MaxLength="4" Width="80">
                            </asp:TextBox><asp:DropDownList ID="ddlFTWidth" runat="server"  CssClass="form-control form_default"></asp:DropDownList>
                        </div>
                      </div>
                        <div class="form-group ControlHide"  id="trFTRows">
                        <%=ViewControlTitle("lblFTRows", "Rows", "txtFTRows", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtFTRows" runat="server" CssClass="NormalTextBox form-control validate[required,custom[integer]]" MaxLength="1" Width="80"></asp:TextBox>
                            <%=ViewHelp("lblFTRows", "line")%>
                        </div>
                      </div>
          
                        <div class="form-group ControlHide" id="trFTListCollection">
                        <%=ViewControlTitle("lblFTListCollection", "List Collection", "txtFTListCollection", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtFTListCollection" runat="server" CssClass="NormalTextBox validate[required]"
                            Width="330" Rows="5" TextMode="MultiLine"></asp:TextBox>
                            <div class="ControlHide ListCollection_Help">
                            <%=ViewHelp("lblFTListCollection", "Please enter the item colection of List Control here, only one item for each line.")%>
                            </div>
                            <div class="ControlHide ListCollection_SendMail_Help">
                                <%=ViewHelp("lblFTListCollection", "You can enter multiline texts in List Collection, the text format must be written as below:")%>
                                <%=ViewHelp("lblFTListCollection_2", "sales: admin@dnngo.net")%>
                                <%=ViewHelp("lblFTListCollection_3", "support: dnnskindev@gmail.com")%>
                            </div>
                            
                        </div>
                      </div>
                        <div class="form-group ControlHide" id="trFTDefaultValue">
                        <%=ViewControlTitle("lblFTDefaultValue", "Default Value", "txtFTDefaultValue", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtFTDefaultValue" runat="server" CssClass="TxtDefaultValue ControlHide NormalTextBox form-control validate[maxSize[512]]" Width="330"></asp:TextBox>
                           <div class="HtmlDefaultValue ControlHide"> <asp:TextBox ID="txtTinymceDefaultValue" runat="server" CssClass=" NormalTextBox tinymce form-control validate[maxSize[512]]" Width="90%" TextMode="MultiLine"></asp:TextBox></div>
                            <div class="ControlHide DefaultValue_Help_1"><%=ViewHelp("lblFTDefaultValue", "You can enter Default Value for thr control here.")%></div>
                            <div class="ControlHide DefaultValue_Help_2"><%=ViewHelp("lblFTDefaultValue_Radio", "For DropDownList and RadioButtonList, you can enter one value of List Collection as Default Value.")%>
                            </div>
                            <div class="ControlHide DefaultValue_Help_3"><%=ViewHelp("lblFTDefaultValue_Multiple", "For CheckBoxList and ListBox, you can enter one or mutiple value(s) of List Collection as Default Value(s) , mutiple default values should be separated by ','.")%></div>
                            <div class="ControlHide DefaultValue_Help_4"><%=ViewHelp("lblFTDefaultValue_CheckBox", "For CheckBox, you can enter true or false as Default Value.")%></div>
                            <div class="ControlHide DefaultValue_Help_5"><%=ViewHelp("lblFTDefaultValue_Confirm", "You can enter true or false as Default Value.")%></div>
                        </div>
                      </div>
                       <div class="form-group ControlHide" id="trFTVerification">
                        <%=ViewControlTitle("lblVerification", "Verification", "ddlVerification", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-9">
                           <asp:DropDownList ID="ddlVerification" runat="server" CssClass="form-control" Width="250"></asp:DropDownList>
                        </div>
                      </div>

                      <div class="form-group ControlHide" id="trFTEquals">
                        <%=ViewControlTitle("lblFTEqualsControl", "Equals Control", "ddlFTEqualsControl", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-9">
                             <asp:DropDownList ID="ddlFTEqualsControl" runat="server" CssClass="form-control" Width="250"></asp:DropDownList>
                        </div>
                      </div>
                      <div class="form-group ControlHide" id="trFTAssociatedControl">
                        <%=ViewControlTitle("lblFTAssociatedControl", "Associated Control", "ddlFTAssociatedControl", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-9">
                             <asp:DropDownList ID="ddlFTAssociatedControl" runat="server" CssClass="form-control" Width="250"></asp:DropDownList>
                        </div>
                      </div>


                      <div class="form-group ControlHide" id="trFTInputLength">
                        <%=ViewControlTitle("lblFTInputLength", "Max Length", "txtFTInputLength", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtFTInputLength" runat="server" CssClass="form-control validate[required,custom[integer]]" MaxLength="5" Width="80"></asp:TextBox>
                        </div>
                      </div>
                      

                       <div class="form-group  ControlHide"  id="trFTRequired">
                        <%=ViewControlTitle("lblRequired", "Required", "cbRequired", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-9">
                           <div class="checkbox-inline"><asp:CheckBox ID="cbRequired" runat="server"  CssClass="auto" /></div> 
                        </div>
                      </div>
         

               

                </div>
              </div>
            </div>
          </div>

          
          <div runat="server" id="divOptions_Left">
            <asp:Repeater ID="RepeaterGroup_Left" runat="server" OnItemDataBound="RepeaterGroup_ItemDataBound">
                <ItemTemplate>
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <i class="fa fa-external-link-square"></i>
                            <%#Eval("key")%>
                            <div class="panel-tools">
                                <a href="#" class="btn btn-xs btn-link panel-collapse collapses"></a>
                            </div>
                        </div>
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <asp:Repeater ID="RepeaterOptions" runat="server" OnItemDataBound="RepeaterOptions_ItemDataBound">
                                    <ItemTemplate>
                                        <div class="form-group">
                                            <asp:Literal ID="liTitle" runat="server"></asp:Literal>
                                            <div class="col-sm-9">
                                                <asp:PlaceHolder ID="ThemePH" runat="server"></asp:PlaceHolder>
                                                <asp:Literal ID="liHelp" runat="server"></asp:Literal>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        <!-- end: TEXT AREA PANEL -->
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        </div>

         <div class="col-sm-4">
          <!-- start: SELECT BOX PANEL -->
        <!--Start-->
        <div class="panel panel-default">
            <div class="panel-heading">
                <i class="fa fa-external-link-square"></i>
                <%=ViewResourceText("Title_Publish", "Publish")%>
                <div class="panel-tools">
                    <a href="#" class="btn btn-xs btn-link panel-collapse collapses"></a>
                </div>
            </div>
            <div class="panel-body buttons-widget">
                <ul class="Edit_List" id="accordion">
                    <li>
                        <p>
                            <div class="row form-group">
                                 <div class="col-sm-4"><i class="fa clip-grid-5"></i>&nbsp;&nbsp;<%=ViewResourceText("Title_Active", "Active")%>:</div>
                                <div class="col-sm-8">
                                    <asp:CheckBox ID="cbStatus" runat="server"  CssClass="auto" />
                                </div>
                            </div>
                         </p>
                    </li>
           
                    <li>
                        <p>
                            <i class="fa clip-calendar-3"></i>&nbsp;&nbsp;<%=ViewResourceText("Title_Start", "Start")%>:
                            <b>
                                <asp:Label ID="liStartDateTime" runat="server" Text="immediately"></asp:Label></b>&nbsp;<a
                                    href="#Start" data-toggle="collapse" data-parent="#accordion"><i class="fa fa-pencil"></i>[<%=ViewResourceText("Title_Edit", "Edit")%>]</a></p>
                        <div class="panel-collapse collapse" id="Start">
                            <div class="row form-group">
                                <div class="col-md-6 input-group">
                                    <asp:TextBox ID="txtStartDate" runat="server" data-date-format="mm/dd/yyyy" data-date-viewmode="years"
                                        CssClass="form-control date-picker"></asp:TextBox>
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                </div>
                                <div class="col-md-5 input-group input-append bootstrap-timepicker">
                                    <asp:TextBox ID="txtStartTime" runat="server" CssClass="form-control time-picker"></asp:TextBox>
                                    <span class="input-group-addon add-on"><i class="fa fa-clock-o"></i></span>
                                </div>
                            </div>
                            <div class="form-group">
                                <a id="link_StartDateTime" class="btn btn-default btn-ms2" href="#Start" data-toggle="collapse"
                                    data-parent="#accordion">
                                    <%=ViewResourceText("Title_OK", "OK")%>
                                </a>&nbsp;<a href="#Start" data-toggle="collapse" data-parent="#accordion"><%=ViewResourceText("Title_Cancel", "Cancel")%></a>
                            </div>
                        </div>
                    </li>
                    <li>
                        <p>
                            <i class="clip-stopwatch"></i>&nbsp;&nbsp;<%=ViewResourceText("Title_Disable", "Disable")%>:
                            <b>
                                <asp:Label ID="liDisableDateTime" runat="server" Text="None"></asp:Label></b>&nbsp;<a
                                    href="#DisableDateTime" data-toggle="collapse" data-parent="#accordion"><i class="fa fa-pencil"></i>[<%=ViewResourceText("Title_Edit", "Edit")%>]</a></p>
                        <div class="panel-collapse collapse" id="DisableDateTime">
                            <div class="row form-group">
                                <div class="col-md-6 input-group">
                                    <asp:TextBox ID="txtDisableDate" runat="server" data-date-format="mm/dd/yyyy" data-date-viewmode="years"
                                        CssClass="form-control date-picker"></asp:TextBox>
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                </div>
                                <div class="col-md-5 input-group input-append bootstrap-timepicker">
                                    <asp:TextBox ID="txtDisableTime" runat="server" CssClass="form-control time-picker"></asp:TextBox>
                                    <span class="input-group-addon add-on"><i class="fa fa-clock-o"></i></span>
                                </div>
                            </div>
                            <div class="form-group">
                                <a id="link_DisableDateTime" class="btn btn-default btn-ms2" href="#DisableDateTime"
                                    data-toggle="collapse" data-parent="#accordion">
                                    <%=ViewResourceText("Title_OK", "OK")%>
                                </a>&nbsp;<a href="#DisableDateTime" data-toggle="collapse" data-parent="#accordion"><%=ViewResourceText("Title_Cancel", "Cancel")%></a>
                            </div>
                        </div>
                    </li>
                </ul>
                <div class="row">
                    <br />
                    <div class="col-sm-5">
                        <asp:Button CssClass="btn btn-light-grey btn-sm" ID="cmdDelete" resourcekey="cmdDelete"
                            runat="server" Text="Delete" CausesValidation="False" OnClick="cmdDelete_Click"
                            OnClientClick="CancelValidation();"></asp:Button>
                    </div>
                    <div class="col-sm-7 text_right">
                        <asp:Button CssClass="btn btn-primary btn-sm"  data-verify="Submit"  ID="cmdUpdate" resourcekey="cmdUpdate"
                            runat="server" Text="Update" OnClick="cmdUpdate_Click"></asp:Button>&nbsp;
                        <asp:Button CssClass="btn btn-primary btn-sm" ID="cmdCancel" resourcekey="cmdCancel"
                            runat="server" Text="Cancel" CausesValidation="False" OnClick="cmdCancel_Click"
                            OnClientClick="CancelValidation();"></asp:Button>&nbsp;
                    </div>
                </div>
            </div>
        </div>

        <!--Categories-->
        <div class="panel panel-default" runat="server" id="divGroup" visible="false">
            <div class="panel-heading">
                <i class="fa fa-external-link-square"></i>
                <%=ViewResourceText("Title_Groups", "Groups")%>
                <div class="panel-tools">
                    <a href="#" class="btn btn-xs btn-link panel-collapse collapses"></a>
                </div>
            </div>
            <div class="panel-body buttons-widget">
                <asp:PlaceHolder ID="PHGroups" runat="server"></asp:PlaceHolder>
            </div>
        </div>

        <div class="panel panel-default"  >
            <div class="panel-heading">
                <i class="fa fa-external-link-square"></i>
                <%=ViewResourceText("Title_Permissions", "Permissions")%>
                <div class="panel-tools">
                    <a href="#" class="btn btn-xs btn-link panel-collapse collapses"></a>
                </div>
            </div>
            <div class="panel-body buttons-widget form-horizontal">
                  <div class="form-group">
                       <%=ViewControlTitle("lblPermissionsAllUsers", "All Users", "cbPermissionsAllUsers", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-9">
                             <div class="checkbox-inline">
                                <asp:CheckBox ID="cbPermissionsAllUsers" runat="server" CssClass="auto"/>
                            </div>
                        </div>
                  </div>

                  <div class="form-group">
                       <%=ViewControlTitle("lblPermissionsRoles", "Permission Roles", "cblPermissionsRoles", ":", "col-sm-3 control-label")%>
                        <div class="col-sm-9">
                            <div class="checkbox-inline">
                                <asp:CheckBoxList ID="cblPermissionsRoles" runat="server" CssClass="auto"></asp:CheckBoxList>
                            </div>
                        </div>
                  </div>
            </div>
        </div>

         <!-- Layout Right Options -->
        <div runat="server" id="divOptions_Right">
            <asp:Repeater ID="RepeaterGroup_Right" runat="server" OnItemDataBound="RepeaterGroup_ItemDataBound">
                <ItemTemplate>
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <i class="fa fa-external-link-square"></i>
                            <%#Eval("key")%>
                            <div class="panel-tools">
                                <a href="#" class="btn btn-xs btn-link panel-collapse collapses"></a>
                            </div>
                        </div>
                        <div class="panel-body buttons-widget form-horizontal">
                            <asp:Repeater ID="RepeaterOptions" runat="server" OnItemDataBound="RepeaterOptions_ItemDataBound">
                                <ItemTemplate>
                                    <div class="form-group">
         
                                            <asp:Literal ID="liTitle" runat="server"></asp:Literal>
                                            <div class="col-sm-7">
                                                <asp:PlaceHolder ID="ThemePH" runat="server"></asp:PlaceHolder>
                                                <asp:Literal ID="liHelp" runat="server"></asp:Literal>
                                            </div>
                 
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>


         <!-- end: SELECT BOX PANEL -->
         
         
         
         </div>

      </div>
      
      <!-- end: PAGE CONTENT--> 

      <script type="text/javascript">
     jQuery(function ($) {

        
        var ControlTypeLoad = function(ControlType)
        {
            $(".ControlHide").hide();
            if(ControlType == 2)
            {
                $("#trFTDefaultValue,#trFTRows,#trFTWidth,.HtmlDefaultValue,.DefaultValue_Help_1").show();
            }else if(ControlType == 3)
            {
                $("#trFTDefaultValue,#trFTListCollection,.ListCollection_Help,#trFTWidth,.TxtDefaultValue,.DefaultValue_Help_2,#trFTRequired").show();
            }else if(ControlType == 4)
            {
                $("#trFTDefaultValue,#trFTListCollection,.ListCollection_Help,#trFTWidth,#trFTRows,.TxtDefaultValue,.DefaultValue_Help_3,#trFTRequired").show();
            }
            else if(ControlType == 5)
            {
                $("#trFTDefaultValue,#trFTListCollection,.ListCollection_Help,#trFTWidth,#trFTListColumn,#trFTDirection,.TxtDefaultValue,.DefaultValue_Help_2,#trFTRequired").show();
            }
            else if(ControlType == 6)
            {
              $("#trFTWidth,#trFTRequired").show();
            }
            else if(ControlType == 7)
            {
              $("#trFTDefaultValue,.TxtDefaultValue,.DefaultValue_Help_4,#trFTRequired").show();
            }
            else if(ControlType == 8)
            {
                $("#trFTDefaultValue,#trFTListCollection,.ListCollection_Help,#trFTWidth,#trFTListColumn,#trFTDirection,.TxtDefaultValue,.DefaultValue_Help_3,#trFTRequired").show();
            }
            else if(ControlType == 9)
            {
              $("#trFTWidth,#trFTRequired").show();
            }
            else if(ControlType == 10)
            {
               $("#trFTDefaultValue,.TxtDefaultValue").show();
            }
            else if(ControlType == 11)
            {
              $("#trFTDefaultValue,.HtmlDefaultValue").show();
            }
            else if(ControlType == 111)
            {
              $("#trFTWidth,#trFTRequired").show();
            }
            else if(ControlType == 112)
            {
              $("#trFTWidth,#trFTRequired").show();
            }
            else if (ControlType == 130) {
                $("#trFTWidth,#trFTRequired,#trFTAssociatedControl").show();
            }
            else if(ControlType == 131)
            {
              $("#trFTWidth,#trFTRequired").show();
            }  else if(ControlType == 132)
            {
                $("#trFTDefaultValue,#trFTWidth,#trFTListCollection,.TxtDefaultValue,.ListCollection_SendMail_Help,#trFTRequired").show();
            } else if (ControlType == 140) {
                $("#trFTDefaultValue,.TxtDefaultValue,.DefaultValue_Help_5").show();
            } else if (ControlType == 1)
            {
              $("#trFTDefaultValue,#trFTRows,#trFTWidth,#trFTVerification,.TxtDefaultValue,.DefaultValue_Help_1,#trFTRequired,#trFTInputLength,#trFTEquals").show();
            }

        };


         $("#<%=ddlControlType.ClientID %>").change(function (e) {
            ControlTypeLoad($(this).find("option:selected").val());
         });
         ControlTypeLoad($("#<%=ddlControlType.ClientID %>").find("option:selected").val());




<%--
        <%if(txtName.Enabled)
        { %>
         $("#<%=cmdUpdate.ClientID %>").click(function (event) {
            


           $.ajax({
                 async: false,
                 url: "<%=ModulePath %>ajaxValidate.aspx",
                 cache: false,
                 data: "ModuleID=<%=ModuleId %>&type=AjaxCustomFieldName&fieldValue=" + $("#<%=txtName.ClientID %>").val(),
                 success: function (Result) {
                    if (!$('#PlaceHolder_container').validationEngine('validate')) {
                         event.preventDefault(); 
                    } 

                    if($("#<%=txtName.ClientID %>").val() !="")
                    {
                        if (Result == "true") {

                           

                             $("#iName").attr("class", "clip-checkmark-circle");
                             $("#Form").submit(function () {return Result == "true";});
                           //$("#Form").one("submit", function () { return Result == "true"; });
                           //console.log("提交：", Result == "true");
                         } else {
                              $("#iName").attr("class", "clip-cancel-circle");
                               event.preventDefault(); 
                         }
                     }
         
                    
                 }
             })
         });
         $("#<%=txtName.ClientID %>").change(function () {
             $("#iName").attr("class", "fa clip-circle-small");
         });
         <%} %>--%>

         tinymce.init({
            selector: "textarea.tinymce",
            entity_encoding: "raw",
            convert_urls: false,
	        plugins: [
		        "advlist autolink link image lists charmap print preview hr anchor pagebreak",
		        "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
		        "save table contextmenu directionality template paste textcolor"
	        ]  
        });


     });
</script>