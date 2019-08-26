<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View_Form.ascx.cs"  Inherits="DNNGo.Modules.PowerForms.View_Form" %>

 <div>
    <asp:Literal ID="liContent" runat="server"></asp:Literal>
    <asp:HiddenField ID="hfGoogleRecaptchaResponse" runat="server" />
    <asp:HiddenField ID="hfVerifyEncrypt" runat="server" />
    <asp:HiddenField ID="hfVerifyString" runat="server" />
    <asp:Button runat="server" ID="SubmitButton" OnClick="SubmitButton_Click" Visible="false" />
</div>
 
 
<script type="text/javascript">

     

     function validationFile<%=ModuleId %>(){
        var b = true;
         jQuery(".form_div_<%=ModuleId %> input[type='file']").filter(".ajaxUpload").each(function(i){
            var fileid =  $(this).attr("id");
            jQuery.ajax({
            async:false,
            url: "<%=ModulePath %>ajaxValidate.aspx",
            cache: false,
            data: "ModuleID=<%=ModuleId %>&type=AjaxCustomUpload&fieldValue="+jQuery(this).val(),
            success: function(Result){
                        if(Result != "true")
                        {
                        jQuery(".form_div_<%=ModuleId %> #png_upload_"+fileid).attr("src","<%=ModulePath %>Resource/images/no.png").show();
                        }else
                        {
                            //jQuery(".form_div_<%=ModuleId %> #png_upload_"+ fileid).attr("src","<%=ModulePath %>Resource/images/yes.png").show();
                            jQuery(".form_div_<%=ModuleId %> #png_upload_"+ fileid).hide();
                        }
                          if( Result != "true") {    b= false; } 
                }
            });
        });
        return b;
     }

    <%if (ViewSettingT<Boolean>("PowerForms_Recaptcha_v3_Enable", false))
    {%>

    function setgrecaptcha<%=ModuleId %>() {
        grecaptcha.execute('<%=ViewSettingT<String>("PowerForms_Recaptcha_v3_SiteKey", "")%>', { action: 'page<%=TabId%>/module<%=ModuleId%>' }).then(function (token) {
            $("#<%=hfGoogleRecaptchaResponse.ClientID%>").val(token);
            //console.log("token-val:", $("#<%=hfGoogleRecaptchaResponse.ClientID%>").val());
        });
    }

    grecaptcha.ready(function () {
        setgrecaptcha<%=ModuleId %>();
        ref = setInterval(function () {
            setgrecaptcha<%=ModuleId %>();
            //console.log("setInterval-setgrecaptcha", new Date());
        }, 1000 * 60 * 2);
    });
     <%}%>
   

    jQuery(function ($) {

        <% if(ViewSettingT<Boolean>("PowerForms_FormStorage", false)) {%>
            var sisForm = $(".form_div_<%=ModuleId %>").sisyphus({ customKeyPrefix: 'bg', timeout: 0, autoRelease: true });
            $("#Form").submit(function () {sisForm.manuallyReleaseData();});
        <%  } %>
   

        $("input.other_textbox").bind("keyup change",function(){
           var forid = "#" + $(this).data("forid");
           $(forid).val($(forid).data("value")+ $(this).val());
        });


        tinymce.init({
            selector: "textarea.tinymce",
            entity_encoding: "raw",
            convert_urls: false,
             menubar : false
        });

        <% if(EffectGroup == true)
        { %>
        $(".form_div_<%=ModuleId %> .group_title").click(function () {
            $(this).parent().find("div[class='group_inside']").slideToggle(300);
            if ($(this).find("span").attr("class") === "min") { $(this).find("span").addClass("max").removeClass("min"); } else { $(this).find("span").addClass("min").removeClass("max"); }
        });

        <%} %>
       
        $(".form_div_<%=ModuleId %> input[data-verify='Submit']").click(function () { 

           
             //setgrecaptcha();
         

            if(!validationFile<%=ModuleId %>())
            {
                return false;
            }
             
            $(".form_div_<%=ModuleId %> .group_title span").addClass("min").removeClass("max");
            $(".form_div_<%=ModuleId %> div[class='group_inside']").show(function () {
               <%-- if (!$(".form_div_<%=ModuleId %>").validationEngine('validate'))
                {
                    return false;
                }--%>
            });
       });

        
        if ($(".form_div_<%=ModuleId %>  select.region").is("select") && $(".form_div_<%=ModuleId %>  select.region[data-for]").is("select"))
        {
          

            function loadregions<%=ModuleId %>($region, $country)
            {
                $region.empty();
                $region.append('<option value=""><%=ViewResourceText("lblGroupSelect", "==Please select==")%></option>');
                var select_country = $country.val();
                if (select_country !== undefined && select_country != '') {
                     
                    $.getJSON("<%=ModulePath %>Resource_Service.aspx?Token=country.region&ModuleId=<%=ModuleId%>&PortalId=<%=PortalId%>&TabId=<%=TabId%>&Country=" + encodeURIComponent(select_country), function (data) {
                        $.each(data, function (i, item) {
                            $region.append('<option value="' + item.Text + '">' + item.Text + '</option>');
                        });
                    });
                }
            }

           $(".form_div_<%=ModuleId %>  select.region").each(function (i) {
                var $region = $(this);
                var $country = $("#"+ $region.data("for"));
                $country.bind("change", function () {
                    loadregions<%=ModuleId %>($region, $country);
                });
                loadregions<%=ModuleId %>($region, $country);
            });
 
        }
  




        <%if (IncludeMultipleFileUpload){%>
            if (jQuery.type($.fn.button.noConflict) === "function")
            {
                var btn = $.fn.button.noConflict()
                $.fn.btn = btn;
            }
      
            $(".form_div_<%=ModuleId %> .plupload").each(function (i) {
                    var $plupload = $(this);
                    var $plupload_input = $("#" + $plupload.data("id"));
                    var json_files = [];
                    $plupload.plupload({
                        runtimes: 'html5,flash,silverlight,html4',
                        url: "<%=ModulePath %>Resource_TempSaves.ashx?ModuleId=<%=ModuleId%>&PortalId=<%=PortalId%>&TabId=<%=TabId%>",
                        unique_names: true,
                        rename: true,
                        sortable: true,
                        dragdrop: true,
                        views: { list: true,thumbs: true,active: 'thumbs'},
                        filters: {
                            max_file_size: '<%=ViewSettingT<Int32>("PowerForms_MaxFileSize",10240)%>kb',
                            mime_types: [{ title: "all files", extensions: "<%=AllowableFileExtensions%>" }]
                        },
                        flash_swf_url: '<%=ModulePath %>Resource/plugins/plupload/Moxie.swf',
                        silverlight_xap_url: '<%=ModulePath %>Resource/plugins/plupload/Moxie.xap',
                        preinit: {
                            UploadFile: function (up, file) {
                                up.setOption('multipart_params', { id: file.id });
                            }
                        },
                        init: {
                            FilesRemoved: function (up, files) {
                                plupload.each(files, function (file) {
                                    $.each(json_files, function (index, item) {
                                        if (item != undefined && item.id == file.id)
                                        {
                                            $.post("<%=ModulePath %>Resource_TempSaves.ashx?ModuleId=<%=ModuleId%>&PortalId=<%=PortalId%>&TabId=<%=TabId%>&type=DELETE&id=" + file.id + "", function (data) {});
                                            json_files.splice(parseInt(index), 1);
                                        }
                                    });
                                });
                                $plupload_input.val(JSON.stringify(json_files));
                                //console.log("$plupload_input delete:", $plupload_input.val());
                            },
                            FileUploaded: function (up, file, info) {
                                $.each(JSON.parse(info.response),function (index, item) { 
                                    item.id = file.id;
                                    json_files.push(item);
                                });
                                $plupload_input.val(JSON.stringify(json_files));
                                //console.log("$plupload_input add:", $plupload_input.val());
                            }

                        }
                    });
                });
        <%}%>
    


        

           
    });
</script>
