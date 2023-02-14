<%@ Page Language="C#" AutoEventWireup="true" CodeFile="edit_OilAreaLongPipeline.aspx.cs" Inherits="WebPage_edit_OilAreaLongPipeline" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=11; IE=10; IE=9; IE=8" />
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="keywords" content="關鍵字內容" />
    <meta name="description" content="描述" /><!--告訴搜尋引擎這篇網頁的內容或摘要。--> 
    <meta name="generator" content="Notepad" /><!--告訴搜尋引擎這篇網頁是用什麼軟體製作的。--> 
    <meta name="author" content="工研院 資訊處" /><!--告訴搜尋引擎這篇網頁是由誰製作的。-->
    <meta name="copyright" content="本網頁著作權所有" /><!--告訴搜尋引擎這篇網頁是...... --> 
    <meta name="revisit-after" content="3 days" /><!--告訴搜尋引擎3天之後再來一次這篇網頁，也許要重新登錄。-->
    <title>石油業輸儲設備查核及檢測資訊系統</title>
    <!--#include file="Head_Include.html"-->
    <%--<link href="../wysiwyg-editor-2/css/froala_editor.min.css" rel="stylesheet" type="text/css" />
    <link href="../wysiwyg-editor-2/css/third_party/font_awesome.min.css" rel="stylesheet" type="text/css" />
    <script src="../wysiwyg-editor-2/js/froala_editor.min.js"></script>--%>
    <script src="../tinymce/tinymce.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="./tinymce/jscripts/tiny_mce/tiny_mce.js"></script>--%>

    <script type="text/javascript">

        //var vWebUrl; //宣告網站站台 URL 路徑變數

        //tinyMCE.init({
        //    // General options
        //    mode: "exact",
        //    selector: "#nContent",
        //    theme: "advanced",
        //    plugins: "autolink,lists,pagebreak,advhr,advimage,advlink,emotions,iespell,inlinepopups,preview,media,searchreplace,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,wordcount,advlist,autosave,imgupd,example2,table",

        //    //整理要傳入TinyMCE的參數
        //    //*1,呼叫 ./tinymce/jscripts/tiny_mce/utils/GetWebRootURL.ashx
        //    //   取得目前的網站站台 URL 路徑
        //    setup: function (ed) {
        //        $.get('./tinymce/jscripts/tiny_mce/utils/GetWebRootURL.ashx', function (data) {
        //            vWebUrl = data;
        //        });
        //    },

        //    // Theme options
        //    theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,fontsizeselect,imgupd,|,forecolor,backcolor,code,|,tablecontrols",
        //    theme_advanced_buttons2: "",
        //    theme_advanced_buttons3: "",
        //    theme_advanced_buttons4: "",
        //    theme_advanced_toolbar_location: "top",
        //    theme_advanced_toolbar_align: "left",
        //    theme_advanced_statusbar_location: "bottom",  //顯示狀態列
        //    theme_advanced_resizing: true,  //是否允許調整編輯器大小位置

        //    // Example content CSS (should be your site CSS)
        //    content_css: "css/content.css",

        //    // Drop lists for link/image/media/template dialogs
        //    template_external_list_url: "lists/template_list.js",
        //    external_link_list_url: "lists/link_list.js",
        //    external_image_list_url: "lists/image_list.js",
        //    media_external_list_url: "lists/media_list.js",

        //    // Style formats
        //    style_formats: [
        //        { title: 'Bold text', inline: 'b' },
        //        { title: 'Red text', inline: 'span', styles: { color: '#ff0000' } },
        //        { title: 'Red header', block: 'h1', styles: { color: '#ff0000' } },
        //        { title: 'Example 1', inline: 'span', classes: 'example1' },
        //        { title: 'Example 2', inline: 'span', classes: 'example2' },
        //        { title: 'Table styles' },
        //        { title: 'Table row 1', selector: 'tr', classes: 'tablerow1' }
        //    ],

        //    invalid_elements: "script,object,applet,iframe,style", //避免 uset 輸入惡意語法
        //    relative_urls: false, //設定圖片顯示正常路徑  step 1
        //    remove_script_host: false, //設定圖片顯示正常路徑  step 2        
        //    document_base_url: vWebUrl   //設定圖片顯示正常路徑  step 3

        //});




        //$(function () {
        //    $('#edit').froalaEditor({
        //        inlineMode: false,

        //        events: {
        //            'image.uploaded': function (response) {
        //                var img_url = '../../tinymce/ImageUpload/imgUpload.aspx?category=Oil_Upload&type=arealongpipeline&cpName=' + $("#companyName").text();
        //                editor.image.insert(img_url, false, null, editor.image.get(), response);
        //            },
        //        }
        //    })
        //});
        var aaa = '../../tinymce/ImageUpload/imgUpload.aspx?category=Oil_Upload&type=arealongpipeline&cpName=油品行銷事業部台南營業處豐德供油服務中心';

        tinymce.init({
            
            selector: "textarea",
            language: "zh_TW",
            menubar: 'edit', //上方工具列顯示or隱藏
            images_upload_url: aaa,
            images_file_types: 'jpeg, jpg, png, gif, bmp, webp',
            //file_picker_callback: function (ed) {
            //    var fileInput = $('<input id="tinymce-uploader" type="file" name="pic" accept="image/*" style="display:none">');
            //    $(ed.getElement()).parent().append(fileInput);

            //    fileInput.on("change", function () {
            //        var file = this.files[0];
            //        var reader = new FileReader();
            //        var data = new FormData();
            //        var files = file;

            //        data.append('file', files);
            //        data.append('category', 'Oil_Upload');
            //        data.append('type', 'arealongpipeline');
            //        data.append('cpName', $("#companyName").text());

            //        $.ajax({
            //            type: "POST",
            //            async: false, //在沒有返回值之前,不會執行下一步動作
            //            url: "../../tinymce/ImageUpload/imgUpload.aspx",
            //            data: data,
            //            processData: false,
            //            contentType: false,
            //            cache: false,
            //            error: function (xhr) {
            //                alert("Error: " + xhr.status);
            //                console.log(xhr.responseText);
            //            },
            //            success: function (data) {
            //                if ($(data).find("Error").length > 0) {
            //                    alert($(data).find("Error").attr("Message"));
            //                }
            //                else {
            //                    var ReturnValue = '<img src="' + $("Response", data).text() + '&category=' + $("category", data).text() + '&type=' + $("type", data).text() + '&cpName=' + $("cpName", data).text() + '" alt="" />';
            //                    ed.insertContent(ReturnValue);
            //                }
            //            }
            //        });

            //        reader.readAsDataURL(file);
            //    });
            //},
            file_browser_callback: function (field_name, url, type, win) {
                if (type == "image") {
                    tinymce.activeEditor.windowManager.close();
                    tinymce.activeEditor.windowManager.open({
                        title: "圖片上傳",
                        url: '../../tinymce/ImageUpload/upload.aspx?category=Oil_Upload&type=arealongpipeline&cpName=' + $("#companyName").text() + '&cpguid=' + $.getQueryString("cp"),
                        width: 380,
                        height: 140
                    });
                }
            },
            plugins: ["advlist autolink lists image link charmap print preview searchreplace visualblocks code fullscreen insertdatetime table contextmenu paste pagebreak textcolor image"],
            font_formats: "新細明體=新細明體;標楷體=標楷體;微軟正黑體=微軟正黑體;Arial=arial,helvetica,sans-serif;Arial Black=arial black,avant garde;Comic Sans MS=comic sans ms,sans-serif;Times New Roman=times new roman,times;",
            pagebreak_separator: "<!--pagebreak-->",
            image_advtab: true, //圖片進階選項
            relative_urls: false,
            remove_script_host: false,
            convert_urls: true,
            toolbar1: "undo redo | bold italic underline | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link pagebreak table image | forecolor backcolor | fontselect fontsizeselect | paste pastetext",
            paste_data_images: true,
            paste_block_drop: true,
            
        });


        //$(function () {
        //    $("#edit").froalaEditor({
        //        autosave: false, // Enable autosave option. Enabling autosave helps preventing data loss.
        //        autosaveInterval: 1000, // Time in milliseconds to define when the autosave should be triggered. 
        //        saveURL: null, // Defines where to post the data when save is triggered. The editor will initialize a POST request to the specified URL passing the editor content in the body parameter of the HTTP request.
        //        blockTags: ["n", "p", "blockquote", "pre", "h1", "h2", "h3", "h4", "h5", "h6"], // Defines what tags list to format a paragraph and their order. 
        //        borderColor: "#252528", // Customize the appearance of the editor by changing the border color.
        //        buttons: ["bold", "italic", "underline", "strikeThrough", "fontSize", "color", "sep", "formatBlock", "align", "insertOrderedList", "insertUnorderedList", "outdent", "indent", "sep", "selectAll", "createLink", "insertImage", "undo", "redo", "html"], // Defines the list of buttons that are available in the editor. 
        //        crossDomain: false, // Make AJAX requests using CORS. 
        //        direction: "ltr", // Sets the direction of the text.
        //        editorClass: "", // Set a custom class for the editor element.
        //        height: "auto", // Set a custom height for the editor element.
        //        imageMargin: 20, // Define a custom margin for image. It will be visible on the margin of the image when float left or right is active.
        //        imageErrorCallback: false,
        //        imageUploadParam: "file", // Customize the name of the param that has the image file in the upload request.
        //        imageUploadURL: 'imgUpload.aspx?category=Oil_Upload&type=arealongpipeline&cpName=' + $("#companyName").text(), // A custom URL where to save the uploaded image.
        //        inlineMode: true, // Enable or disable inline mode.
        //        placeholder: "Type something", // Set a custom placeholder to be used when the editor body is empty.
        //        shortcuts: true, // Enable shortcuts. The shortcuts are visible when you hover a button in the editor.
        //        spellcheck: false, // Enables spellcheck.
        //        typingTimer: 250, // Time in milliseconds to define how long the typing pause may be without the change to be saved in the undo stack.
        //        width: "auto" // Set a custom width for the editor element.
        //    })
        //});

        $(document).ready(function () {
            getData();

            //取消按鍵
            $(document).on("click", "#cancelbtn", function () {
                var str = confirm('尚未儲存的部分將不會更改，確定返回嗎?');

                if (str)
                    location.href = "OilAreaLongPipeline.aspx?cp=" + $.getQueryString("cp");
            });

            //儲存按鍵
            $(document).on("click", "#subbtn", function () {

                var mode = ($.getQueryString("guid") == "") ? "new" : "edit";

                // '<' & '>' 做全形處理
                tinyMCE.activeEditor.dom.addClass(tinyMCE.activeEditor.dom.select('img'), 'img-responsive');
                var content_tmp = tinyMCE.get("nContent").getContent().trim().replace(/&lt;/g, "＜").replace(/&gt;/g, "＞");

                // Get form
                var form = $('#form1')[0];

                // Create an FormData object 
                var data = new FormData(form);

                // If you want to add an extra field for the FormData
                data.append("cp", $.getQueryString("cp"));
                data.append("guid", $.getQueryString("guid"));
                data.append("mode", encodeURIComponent(mode));
                data.append("year", encodeURIComponent(getTaiwanDate()));
                data.append("nContent", encodeURIComponent(content_tmp));

                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "../handler/AddOilAreaLongPipeline.aspx",
                    data: data,
                    processData: false,
                    contentType: false,
                    cache: false,
                    error: function (xhr) {
                        alert("Error: " + xhr.status);
                        console.log(xhr.responseText);
                    },
                    success: function (data) {
                        if ($(data).find("Error").length > 0) {
                            alert($(data).find("Error").attr("Message"));
                        }
                        else {
                            alert($("Response", data).text());

                            location.href = "OilAreaLongPipeline.aspx?cp=" + $.getQueryString("cp");
                        }
                    }
                });
            });
		}); // end js

        function getData() {
            $("#errMsg").empty();
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetOilAreaLongPipeline.aspx",
				data: {
                    guid: $.getQueryString("guid"),
                    type: "data"
				},
				error: function (xhr) {
                    $("#errMsg").html("Error: " + xhr.status);
					console.log(xhr.responseText);
				},
				success: function (data) {
					if ($(data).find("Error").length > 0) {
                        $("#errMsg").html($(data).find("Error").attr("Message"));
					}
					else {
						if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                var content_tmp = $(this).children("內容").text().trim();
                                if ($("#tmpBrowser").val() == "internetexplorer")
                                    tinymce.activeEditor.setContent(content_tmp);
                                else
                                    $("#nContent").val(content_tmp);
                                    if ($("#tmpBrowser").val() == "internetexplorer")
                                        tinymce.activeEditor.setContent(content_tmp);
                                    else
                                        $("#nContent").val(content_tmp);
                                //$('#edit').froalaEditor('html.set', $(this).children("內容").text().trim());
							});
						}
					}
				}
			});
        }

        function getDDL(year) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/GetOilStorageTankBWT.aspx",
                data: {
                    cpid: $.getQueryString("cp"),
                    year: year,
                    type: "list",
                },
                error: function (xhr) {
                    alert("Error: " + xhr.status);
                    console.log(xhr.responseText);
                },
                success: function (data) {
                    if ($(data).find("Error").length > 0) {
                        alert($(data).find("Error").attr("Message"));
                    }
                    else {
                        var ddlstr = '<option value="">請選擇</option>';
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                ddlstr += '<option value="' + $(this).children("轄區儲槽編號").text().trim() + '">' + $(this).children("轄區儲槽編號").text().trim() + '</option>';
                            });
                        }

                        $("#txt1").empty();
                        $("#txt1").append(ddlstr);
                    }
                }
            });
        }

        function splitYearMonth(arrylenth, fulldate) {

            if (fulldate != '') {
                var farray = new Array();
                farray = fulldate.split("/");
                var twdate = farray[arrylenth];

                return twdate;
            }
            else {
                return '';
            }

        }

        function getDate(fulldate) {

            if (fulldate != '') {
                var twdate = '';
                var farray = new Array();
                farray = fulldate.split("/");

                if (farray.length > 1) {
                    twdate = farray[0] + farray[1] + farray[2];
                }
                else {
                    twdate = fulldate;
                }

                return twdate;
            }
            else {
                return '';
            }

        }

        function getTaiwanDate() {
            var nowDate = new Date();

            var nowYear = nowDate.getFullYear();
            var nowTwYear = (nowYear - 1911);

            return nowTwYear;
        }

        //function getTaiwanDate() {
        //    var nowDate = new Date();

        //    var nowYear = nowDate.getFullYear();
        //    var nowTwYear = (nowYear - 1911);

        //    var ddlstr = '<option value="">請選擇</option>';

        //    for (var i = 10; i >= 0; i--) {
        //        ddlstr += '<option value="' + (nowTwYear - i).toString() + '">' + (nowTwYear - i).toString() + '</option>';
        //    }

        //    for (var j = 1; j <= 10; j++) {
        //        ddlstr += '<option value="' + (nowTwYear + j).toString() + '">' + (nowTwYear + j).toString() + '</option>';
        //    }

        //    $("#sellist").empty();
        //    $("#sellist").append(ddlstr);
        //}
    </script>
</head>
<body class="bgB">
<!-- 開頭用div:修正mmenu form bug -->
<div>
<form>
<!-- Preloader -->
<div id="preloader" >
	<div id="status" >
<div id="CSS3loading">
<!-- css3 loading -->
<div class="sk-three-bounce">
      <div class="sk-child sk-bounce1"></div>
      <div class="sk-child sk-bounce2"></div>
      <div class="sk-child sk-bounce3"></div>
</div> 
<!-- css3 loading -->
<span id="loadingword">資料讀取中，請稍待...</span> 
</div><!-- CSS3loading -->  
    </div><!-- status -->
</div><!-- preloader -->
<div class="container BoxBgWa BoxShadowD">
<div class="WrapperBody" id="WrapperBody">
        <!--#include file="OilHeader.html"-->
        <div id="ContentWrapper">
            <div class="container margin15T">
                <div class="padding10ALL">
                    <div class="filetitlewrapper"><!--#include file="OilBreadTitle.html"--></div>

                    <div class="row margin20T">
                        <div class="col-lg-3 col-md-4 col-sm-5">
                            <div id="navmenuV"><!--#include file="OilLeftMenu.html"--></div>
                        </div>
                        <div class="col-lg-9 col-md-8 col-sm-7">
                            <div class="twocol">
                                <div class="right">
                                    <a id="cancelbtn" href="javascript:void(0);" title="返回" class="genbtn" >取消</a>
                                    <a id="subbtn" href="javascript:void(0);" title="儲存" class="genbtn" >儲存</a>
                                </div>
                            </div><br />

                            <%--<section id="editor">
                                <textarea id="edit">

                                </textarea>
                            </section>--%>


                            <textarea id="nContent" rows="50" cols="" class="width100 TB_Content"></textarea>
                            <div id="errMsg" style="color:red;"></div>

                        </div><!-- col -->
                    </div><!-- row -->


                </div>
            </div><!-- container -->
        </div><!-- ContentWrapper -->



<div class="container-fluid">
<div class="backTop"><a href="#" class="backTotop">TOP</a></div>
</div>        
</div><!-- WrapperBody -->

        <!--#include file="Footer.html"-->

</div><!-- BoxBgWa -->
<!-- 側邊選單內容:動態複製主選單內容 -->
<div id="sidebar-wrapper">
   
</div><!-- sidebar-wrapper -->

</form>
</div>
<!-- 結尾用div:修正mmenu form bug -->

<!-- colorbox -->
<div style="display:none;">
    <div id="workitem">
        <div class="margin35T padding5RL">
            <div class="OchiTrasTable width100 TitleLength08 font-size3">
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">工作項次</div>
                    <div class="OchiCell width100">
                        <input type="number" class="inputex width10">﹒<input type="number" class="inputex width10">﹒<input type="number" class="inputex width10">
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">預定日期</div>
                    <div class="OchiCell width100"><input type="text" class="inputex Jdatepicker width30"> </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">預定完成執行內容</div>
                    <div class="OchiCell width100"><textarea rows="5" cols="" class="inputex width100"></textarea></div>
                </div><!-- OchiRow -->
            </div><!-- OchiTrasTable -->
        </div>

        <div class="twocol margin10T">
            <div class="right">
                <a href="#" class="genbtn closecolorbox">取消</a>
                <a href="#" class="genbtn">儲存</a>
            </div>
        </div>
        <br /><br />
    </div>
</div>

<!-- 本頁面使用的JS -->
    <script type="text/javascript">
        $(document).ready(function(){
        
        });
    </script>
    <script type="text/javascript" src="../js/GenCommon.js"></script><!-- UIcolor JS -->
    <script type="text/javascript" src="../js/PageCommon.js"></script><!-- 系統共用 JS -->
    <script type="text/javascript" src="../js/MenuOil.js"></script><!-- 系統共用 JS -->
    <script type="text/javascript" src="../js/SubMenuOilA.js"></script><!-- 內頁選單 -->
    <script type="text/javascript" src="../js/autoHeight.js"></script><!-- 高度不足頁面的絕對置底footer -->
</body>
</html>






