<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OilInternalAudit.aspx.cs" Inherits="WebPage_OilInternalAudit" %>

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
    <script type="text/javascript">
        $(document).ready(function () {
			getData();
            getExtension();

            $(document).on("keyup", "body", function (e) {
				if (e.keyCode == 13)
					$("#savebtn").click();
			});

            // 開啟檔案上傳開窗
            $(document).on("click", "input[name='uploadbtn']", function () {
                $("#iGuid").val($(this).attr("aid"));
                doOpenDialog();
            });

            // 儲存檔案
            $(document).on("click", "#savebtn", function () {
                var msg = '';

				if ($("#fileUpload").val() == "")
                    msg += "請先選擇檔案再上傳";
                if (msg != "") {
                    alert(msg);
					return false;
				}

				// Create an FormData object 
				var data = new FormData();

				// If you want to add an extra field for the FormData
                data.append("guid", $("#iGuid").val());
                data.append("cpid", $.getQueryString("cp"));
                data.append("category", "oil");
                data.append("type", "storageinspect");
                data.append('fileUpload', $("#fileUpload").get(0).files[0]);

                $.ajax({
				    type: "POST",
				    async: false, //在沒有返回值之前,不會執行下一步動作
				    url: "../Handler/AddDownload.aspx",
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
                            getData();
                            getExtension();
                            parent.$.colorbox.close();
				    	}
				    }
			    });
            });

            // 刪除檔案
            $(document).on("click", "input[name='delbtn']", function () {
                var isDel = confirm("確定刪除檔案嗎?");
                if (isDel) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../Handler/DelOilStorageInspect.aspx",
                        data: {
                            guid: $(this).attr("aid"),
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
                                alert($("Response", data).text());
                                getData();
                                getExtension();                                
                            }
                        }
                    });
                }
            });
		}); // end js

		function getData() {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetOilInternalAudit.aspx",
				data: {
					cpid: $.getQueryString("cp")
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
						$("#tablist tbody").empty();
						var tabstr = '';
						if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                var filename = $(this).children("佐證資料檔名").text().trim();
                                var fileextension = $(this).children("佐證資料副檔名").text().trim();
								tabstr += '<tr>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("內部稽核日期").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("執行單位").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("稽核範圍").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("缺失改善執行狀況").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("佐證資料").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">';                                
                                tabstr += '<img width="200px" height="200px" name="img_' + $(this).children("guid").text().trim() + '" src="../DOWNLOAD.aspx?category=Oil&type=storageinspect&v=' + $(this).children("guid").text().trim() +
                                    '" alt="' + filename + fileextension + '" style="display:none" >';                                
                                tabstr += '<a name="a_' + $(this).children("guid").text().trim() + '" href="../DOWNLOAD.aspx?category=Oil&type=storageinspect&v=' + $(this).children("guid").text().trim() +
                                    '" style="display:none" >' + filename + fileextension + '</a>';
                                tabstr += '</td>';
                                tabstr += '<td name="uploadtd" align="center" nowrap="nowrap">';
                                tabstr += '<input type="button" value="上傳檔案" name="uploadbtn" class="genbtn" aid="' + $(this).children("guid").text().trim() + '" /></br></br>';
                                tabstr += '<input type="button" value="刪除檔案" name="delbtn" class="genbtn" aid="' + $(this).children("guid").text().trim() + '" />';
                                tabstr += '</td>';
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="8">查詢無資料</td></tr>';
                        $("#tablist tbody").append(tabstr);
                        switch ($("#Competence").val()) {
                            case "01":
                                $("td[name='uploadtd']").hide();
                                $("td[name='deltd']").hide();
                                $("#uploadth").hide();
                                break;
                             case "04":
                                $("td[name='uploadtd']").hide();
                                $("td[name='deltd']").hide();
                                $("#uploadth").hide();
                                break;
                            case "05":
                                $("td[name='uploadtd']").hide();
                                $("td[name='deltd']").hide();
                                $("#uploadth").hide();
                                break;
                            case "06":
                                $("td[name='uploadtd']").hide();
                                $("td[name='deltd']").hide();
                                $("#uploadth").hide();
                                break;
                        }
					}
				}
			});
        }

        function getExtension() {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetOilInternalAudit.aspx",
				data: {
					cpid: $.getQueryString("cp")
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
						if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                var fileextension = $(this).children("佐證資料副檔名").text().trim();
								if (fileextension == ".jpg" || fileextension == ".jpeg" || fileextension == ".png") {
                                    $("img[name='img_" + $(this).children("guid").text().trim() + "']").show();
                                }
                                else {
                                    $("a[name='a_" + $(this).children("guid").text().trim() + "']").show();
                                }
							});
						}
					}
				}
			});
        }

        function doOpenDialog() {
            var WinHeight = $("html").height();
            var ColHeight = WinHeight * 0.8;
            $.colorbox({ inline: true, href: "#uploaddialog", width: "100%", maxWidth: "600", maxHeight: ColHeight, opacity: 0.5 });
        }
    </script>
</head>
<body class="bgB">
<!-- 開頭用div:修正mmenu form bug -->
<div>
<form id="form1">
<input type="hidden" id="iGuid" />
<input type="hidden" id="Competence" value="<%= usercompetence %>" />
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

                            <div class="stripeMeB tbover">
                                <table id="tablist" width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th nowrap>日期 </th>
                                            <th nowrap>執行單位 </th>
                                            <th nowrap>稽核範圍 </th>
                                            <th nowrap>缺失改善執行狀況 </th>
                                            <th nowrap>佐證資料說明 </th>
                                            <th nowrap>佐證檔案 </th>
                                            <th id="uploadth" nowrap></th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div><!-- stripeMe -->
                            <div class="margin5TB font-size2">
                                (1) 過去3年長途管線內部稽核執行紀錄(公司內相關單位)<br>
                                (2) 稽核範圍：請填稽核廠區範圍。


                            </div>
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
    <div id="uploaddialog">
        <div class="margin35T padding5RL">
            <div class="OchiTrasTable width100 TitleLength08 font-size3">
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">選擇檔案</div>
                    <div class="OchiCell width100">
                        <input type="file" id="fileUpload" name="fileUpload" />
                    </div>
                </div><!-- OchiRow -->
            </div><!-- OchiTrasTable -->
        </div>

        <div class="twocol margin10T">
            <div class="right">
                <input type="button" id="closebtn" value="取消" class="genbtn closecolorbox" />
                <input type="button" id="savebtn" value="儲存" class="genbtn" />
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

