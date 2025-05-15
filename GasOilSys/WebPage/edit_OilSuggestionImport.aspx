<%@ Page Language="C#" AutoEventWireup="true" CodeFile="edit_OilSuggestionImport.aspx.cs" Inherits="WebPage_edit_OilSuggestionImport" %>

<!DOCTYPE html>

<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta http-equiv="X-UA-Compatible" content="IE=11; IE=10; IE=9; IE=8" />
	<meta name="viewport" content="width=device-width, initial-scale=1" />
	<meta name="keywords" content="關鍵字內容" />
	<meta name="description" content="描述" /><!--告訴搜尋引擎這篇網頁的內容或摘要。--> 
	<meta name="generator" content="Notepad" /><!--告訴搜尋引擎這篇網頁是用什麼軟體製作的。--> 
	<meta name="author" content="工研院 資訊處" /><!--告訴搜尋引擎這篇網頁是由誰製作的。-->
	<meta name="copyright" content="本網頁著作權所有" /><!--告訴搜尋引擎這篇網頁是...... --> 
	<meta name="revisit-after" content="3 days" /><!--告訴搜尋引擎3天之後再來一次這篇網頁，也許要重新登錄。-->
	<title>石油查核建議管理系統</title>
	<style>
		.editor {
		  width: 100%;
		  height: 100vh; /* 100% 視窗高度 */
		  border: none;
		}
	</style>
	<!--#include file="Head_Include.html"-->
	<script src="http://myapp.local:8088/web-apps/apps/api/documents/api.js"></script>
	<script type="text/javascript">
        $(document).ready(function () {

            $(document).on("click", "#subbtn", function () {
                var msg = '';

                if ($("#fileUpload").val() == "")
                    msg += "請先選擇檔案再上傳";
                if (msg != "") {
                    alert(msg);
                    return false;
                }

                // Get form
                var form = $('#form1')[0];

                // Create an FormData object 
                var data = new FormData(form);

                // If you want to add an extra field for the FormData
                data.append("cpid", "");
                data.append("category", "oil");
                data.append("year", getTaiwanDate());
                data.append("type", "suggestionimport");
                data.append("details", "17");
                $.each($("#fileUpload")[0].files, function (i, file) {
                    data.append('file', file);
                });

                $.ajax({
                    type: "POST",
                    async: true, //在沒有返回值之前,不會執行下一步動作
                    url: "../Handler/AddDownload.aspx",
                    data: data,
                    processData: false,
                    contentType: false,
                    cache: false,
                    error: function (xhr) {
                        alert("Error: " + xhr.status);
                        console.log(xhr.responseText);
                    },
                    beforeSend: function () {
                        $("#alertText").show();
                        $("#subbtn").prop("disabled", true);
                    },
                    complete: function () {
                        $("#alertText").hide();
                        $("#subbtn").prop("disabled", false);
                    },
                    success: function (data) {
                        if ($(data).find("Error").length > 0) {
                            alert($(data).find("Error").attr("Message"));
                        }
                        else {
                            console.log("後端回傳檔案名：", $("fileName", data).text());
                            initOnlyOfficeViewer($("fileName", data).text());
                        }
                    }
                });
			});

            function initOnlyOfficeViewer(fileName) {
                console.log("Initializing OnlyOffice Viewer for:", fileName);

                new DocsAPI.DocEditor("div_word", {
                    "document": {
                        "fileType": "docx",
                        "key": "test1_" + Date.now(),
                        "title": fileName,
                        "url": "http://myapp.local:54315/DOWNLOAD.aspx?category=Oil&type=suggestionimport&v=" + encodeURIComponent(fileName)
                    },
                    "editorConfig": {
                        "mode": "edit",
                        "lang": "zh-TW"
                    },
                    "documentType": "word",
                    "events": {
                        "onAppReady": function () {
                            console.log("OnlyOffice Editor Ready ✅");
                        },
                        "onError": function (event) {
                            console.error("OnlyOffice 發生錯誤 ❌:", event);
                            alert("OnlyOffice 錯誤：" + JSON.stringify(event));
                        }
                    }
                });
            }


		}); // end js

        //取得現在時間之民國年
        function getTaiwanDate() {
            var nowDate = new Date();

            var nowYear = nowDate.getFullYear();
            var nowTwYear = (nowYear - 1911);

            return nowTwYear;
        }
    </script>
</head>
<body class="bgB">
	<!-- 開頭用div:修正mmenu form bug -->
	<div>
	<form id="form1">
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
				<!--#include file="OilSuggestionImportHeader.html"-->

				<div id="ContentWrapper">
            <div class="container margin15T">
                <div class="padding10ALL">
                    <div class="filetitlewrapper">
                        <span class="filetitle font-size7">檔案匯入</span>
                        <span class="btnright"></span>
                    </div><br />
                    
					<div class="twocol">
					    <div class="left font-size3 "><i class="fa fa-chevron-circle-right IconCa" aria-hidden="true"></i> 
					        <input id="fileUpload" type="file" />
							<a href="javascript:void(0);" id="subbtn" class="genbtn">上傳</a>
					    </div>
					    <div class="right">

					    </div>
					</div><br />
                        <div id="div_word" style="height: 600px; width: 100%; border: 1px solid #ccc;" class="">

</div><!-- stripeMe -->
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
		<div id="sidebar-wrapper"></div><!-- sidebar-wrapper -->
	</form>
    
	</div>
	<!-- 結尾用div:修正mmenu form bug -->

	<!-- colorbox -->
	<div style="display:none;">
		<div id="checklistedit">
			<div class="margin35T padding5RL">
				<div class="OchiTrasTable width100 TitleLength03 font-size3">
					<div class="OchiRow">
						<div class="OchiCell OchiTitle IconCe TitleSetWidth">備註</div>
						<div class="OchiCell width100">
							<textarea id="psStr" rows="8" cols="" class="inputex width100"></textarea>
						</div>
					</div><!-- OchiRow -->
				</div><!-- OchiTrasTable -->
			</div>

			<div class="twocol margin10T">
				<div class="right">
					<a href="javascript:void(0);" class="genbtn closecolorbox">取消</a>
					<a href="javascript:void(0);" id="ps_savebtn" class="genbtn">儲存</a>
				</div>
			</div>
			<br /><br />
		</div>
	</div>


		<script type="text/javascript" src="../js/GenCommon.js"></script><!-- UIcolor JS -->
		<script type="text/javascript" src="../js/PageCommon.js"></script><!-- 系統共用 JS -->
		<script type="text/javascript" src="../js/MenuGas.js"></script><!-- 系統共用 JS -->
		<script type="text/javascript" src="../js/autoHeight.js"></script><!-- 高度不足頁面的絕對置底footer -->
	</body>
</html>

