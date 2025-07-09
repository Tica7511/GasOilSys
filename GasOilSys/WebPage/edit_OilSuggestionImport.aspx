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
    <script src="http://172.20.10.5:8080/web-apps/apps/api/documents/api.js"></script>
	<title>石油查核建議管理系統</title>
	<style>
		.editor {
		  width: 100%;
		  height: 100vh; /* 100% 視窗高度 */
		  border: none;
		}
	</style>
	<!--#include file="Head_Include.html"-->
	<script type="text/javascript">
        var docEditor = null; 
        var lastEditorConfig = null; 

        $(document).ready(function () {
            if ($.getQueryString("fguid") != "") {
                $("#div_container").empty();
                getData();
            }

            //上傳檔案
            $(document).on("change", "#fileUpload", function () {
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
                            $("#div_container").empty();
                            console.log("後端回傳檔案名：", $("fileName", data).text());
                            const fileName = $("fileName", data).text();
                            const fileNewName = $("fileNewName", data).text();
                            const authToken = $("token", data).text();
                            const onlyofficeguid = $("onlyofficeguid", data).text();
                            $("#cGuid").val($("cGuid", data).text());
                            $("#pGuid").val($("onlyofficeguid", data).text());
                            const mGuid = $("mGuid", data).text();
                            const mName = $("mName", data).text();
                            localStorage.setItem('authToken', authToken);
                            initOnlyOfficeViewer(fileName, fileNewName, authToken, onlyofficeguid, mGuid, mName);
                        }
                    }
                });
            });

            //取得OnlyOffice檔案資料
            function initOnlyOfficeViewer(fileName, fileNewName, authToken, onlyofficeguid, mGuid, mName) {
                console.log("Initializing OnlyOffice Viewer for:", fileName, "\rToken:", authToken);

                var editorConfig = {
                    "document": {
                        "fileType": "docx",
                        "key": onlyofficeguid,
                        "title": fileName,
                        "url": "http://172.20.10.5:54315/DOWNLOAD.aspx?category=Oil&type=suggestionimport&cpid=" + onlyofficeguid +"&v=" + encodeURIComponent(fileNewName)
                    },
                    "documentType": "word",
                    "editorConfig": {
                        "mode": "edit",
                        "lang": "zh-TW",
                        "callbackUrl": "http://172.20.10.5:54315/Handler/SaveCallback.aspx",
                        "customization": {
                            "forcesave": true,
                            "autosave": true
                            /*"trackChanges": true*/
                        },
                        "user": {
                            "id": mGuid,
                            "name": mName
                        },
                        "history": {
                            "serverVersion": true
                        }
                    },
                    "permissions": {
                        "edit": true,
                        /*"review": true,*/  // 開啟追蹤修訂按鈕
                        "comment": true,
                        "print": false,
                        "download": false
                    },
                    "token": authToken,
                    "height": "100%",
                    "width": "100%",
                    "type": "desktop",
                    "events": {
                        onRequestHistory: handleRequestHistory,
                        onRequestHistoryData: handleRequestHistoryData,
                        onRequestHistoryClose: handleRequestHistoryClose,
                        onRequestRestore: handleRequestRestore
                    }
                };

                docEditor = new DocsAPI.DocEditor("placeholder", editorConfig);
            }


            function handleRequestHistory() {
                console.log("onRequestHistory triggered");

                // 確保 docEditor 已經完成初始化
                if (!docEditor || typeof docEditor.refreshHistory !== "function") {
                    console.warn("docEditor not ready");
                    return;
                }

                const onlyofficeguid = $("#cGuid").val();

                $.ajax({
                    type: "GET",
                    url: "http://172.20.10.5:54315/Handler/DocumentHistoryHandler.aspx",
                    data: {
                        type: "01",
                        guid: onlyofficeguid
                    },
                    dataType: "json",
                    success: function (historyResponse) {
                        console.log("Received history response:", historyResponse);
                        docEditor.refreshHistory(historyResponse);
                        console.log("refreshHistory called with:", historyResponse);
                    },
                    error: function (xhr) {
                        console.error("Failed to get history data:", xhr);
                    }
                });
            }

            function handleRequestHistoryData(event) {
                console.log("onRequestHistoryData event triggered. Fetching history...");

                // 確保 docEditor 已初始化
                if (!docEditor || typeof docEditor.refreshHistory !== "function") {
                    console.warn("docEditor not ready");
                    return;
                }

                const version = event.data;
                const onlyofficeguid = $("#cGuid").val();

                $.ajax({
                    type: "GET",
                    url: "http://172.20.10.5:54315/Handler/DocumentHistoryHandler.aspx",
                    data: {
                        type: "02",
                        guid: onlyofficeguid,
                        version: version
                    },
                    dataType: "json",
                    success: function (historyResponse) {
                        console.log("Received history response:", historyResponse);

                        if (docEditor && typeof docEditor.refreshHistory === "function") {
                            console.log("Calling docEditor.setHistoryData");
                            docEditor.setHistoryData(historyResponse);
                            console.log("complete docEditor.setHistoryData");
                        } else {
                            console.error("setHistoryData is not a function or docEditor not ready");
                        }
                    },
                    error: function (xhr) {
                        console.error("Failed to get history data:", xhr);
                    }
                });
            }

            function handleRequestHistoryClose() {
                location.href = "http://172.20.10.5:54315/WebPage/edit_OilSuggestionImport.aspx?fguid=" + $("#cGuid").val();
                //console.log("關閉歷史檢視，將重新啟動編輯器");
                //if (lastEditorConfig) {
                //    initEditor(lastEditorConfig); // 重啟編輯器，不 reload
                //} else {
                //    console.warn("找不到之前的 editor config");
                //}
            }

            function handleRequestRestore(event) {
                const version = event.data.version;
                const fileUrl = event.data.url;
                const fileType = event.data.fileType;
                const changes = event.data.changes;
                const onlyofficeguid = $("#cGuid").val();

                console.log("guid:", $("#cGuid").val());
                console.log("使用者請求還原版本:", version);
                console.log("還原檔案連結:", fileUrl);
                console.log("變更摘要:", changes);

                $.ajax({
                    type: "GET",
                    url: "http://172.20.10.5:54315/Handler/DocumentHistoryHandler.aspx",
                    data: {
                        type: "03",
                        guid: onlyofficeguid,
                        version: version
                    },
                    dataType: "json",
                    success: function () {
                        alert("成功還原版本，重新載入最新內容");
                        location.href = "http://172.20.10.5:54315/WebPage/edit_OilSuggestionImport.aspx?fguid=" + onlyofficeguid;
                    },
                    error: function (xhr) {
                        console.error("Failed to get history data:", xhr);
                    }
                });
            }
        });

        //取得現在時間之民國年
        function getTaiwanDate() {
            var nowDate = new Date();

            var nowYear = nowDate.getFullYear();
            var nowTwYear = (nowYear - 1911);

            return nowTwYear;
        }

        //取得資料庫檔案
        function getData() {
            $.ajax({
                type: "POST",
                async: true,
                url: "../Handler/GetFile.aspx",
                data: {
                    guid: $.getQueryString("fguid"),
                    filetype: "data",
                    type: "17"
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
                                console.log("後端回傳檔案名：", $("fileName", data).text());
                                const fileName = $("fileName", data).text();
                                const fileNewName = $("fileNewName", data).text();
                                const authToken = $("token", data).text();
                                const cGuid = $("cGuid", data).text();
                                const onlyofficeguid = $("onlyofficeguid", data).text();
                                $("#cGuid").val($("cGuid", data).text());
                                $("#pGuid").val($("onlyofficeguid", data).text());
                                const mGuid = $("mGuid", data).text();
                                const mName = $("mName", data).text();
                                localStorage.setItem('authToken', authToken);

                                console.log("Initializing OnlyOffice Viewer for:", fileName, "\rToken:", authToken);

                                var editorConfig = {
                                    "document": {
                                        "fileType": "docx",
                                        "key": onlyofficeguid,
                                        "title": fileName,
                                        "url": "http://172.20.10.5:54315/DOWNLOAD.aspx?category=Oil&type=suggestionimport&cpid=" + onlyofficeguid +"&v=" + encodeURIComponent(fileNewName)
                                    },
                                    "documentType": "word",
                                    "editorConfig": {
                                        "mode": "edit",
                                        "lang": "zh-TW",
                                        "canUseHistory" : true,
                                        "callbackUrl": "http://172.20.10.5:54315/Handler/SaveCallback.aspx",
                                        "customization": {
                                            "forcesave": true,
                                            "autosave": true,
                                            /*"trackChanges": true,*/
                                            "buttons": {
                                                "print": false,
                                                "download": false
                                            }
                                        },
                                        "user": {
                                            "id": mGuid,
                                            "name": mName
                                        },
                                        "history": {
                                            "serverVersion": true 
                                        }
                                    },
                                    "permissions": {
                                        "edit": true,
                                        /*"review": true,*/  // 開啟追蹤修訂按鈕
                                        "comment": true,
                                        "print": false,
                                        "download": false
                                    },
                                    "token": authToken,
                                    "height": "100%",
                                    "width": "100%",
                                    "type": "desktop",
                                    "events": {
                                        onRequestHistory: handleRequestHistory,
                                        onRequestHistoryData: handleRequestHistoryData,
                                        onRequestHistoryClose: handleRequestHistoryClose,
                                        onRequestRestore: handleRequestRestore
                                    }
                                };

                                lastEditorConfig = editorConfig;
                                docEditor = new DocsAPI.DocEditor("placeholder", editorConfig);
                            });
                        }
                    }
                }
            });
        }

        function initEditor(config) {
            if (docEditor && typeof docEditor.destroyEditor === "function") {
                docEditor.destroyEditor(); // 清掉原本的 editor
            }
            docEditor = new DocsAPI.DocEditor("placeholder", config);
        }

        function handleRequestHistory() {
            console.log("onRequestHistory triggered");

            // 確保 docEditor 已經完成初始化
            if (!docEditor || typeof docEditor.refreshHistory !== "function") {
                console.warn("docEditor not ready");
                return;
            }

            const onlyofficeguid = $("#cGuid").val();

            $.ajax({
                type: "GET",
                url: "http://172.20.10.5:54315/Handler/DocumentHistoryHandler.aspx",
                data: {
                    type: "01",
                    guid: onlyofficeguid
                },
                dataType: "json",
                success: function (historyResponse) {
                    console.log("Received history response:", historyResponse);
                    docEditor.refreshHistory(historyResponse);
                    console.log("refreshHistory called with:", historyResponse);
                },
                error: function (xhr) {
                    console.error("Failed to get history data:", xhr);
                }
            });
        }

        function handleRequestHistoryData(event) {
            console.log("onRequestHistoryData event triggered. Fetching history...");

            // 確保 docEditor 已初始化
            if (!docEditor || typeof docEditor.refreshHistory !== "function") {
                console.warn("docEditor not ready");
                return;
            }

            const version = event.data;
            const onlyofficeguid = $("#cGuid").val();

            $.ajax({
                type: "GET",
                url: "http://172.20.10.5:54315/Handler/DocumentHistoryHandler.aspx",
                data: {
                    type: "02",
                    guid: onlyofficeguid,
                    version: version
                },
                dataType: "json",
                success: function (historyResponse) {
                    console.log("Received history response:", historyResponse);

                    if (docEditor && typeof docEditor.refreshHistory === "function") {
                        console.log("Calling docEditor.setHistoryData");
                        docEditor.setHistoryData(historyResponse);
                        console.log("complete docEditor.setHistoryData");
                    } else {
                        console.error("setHistoryData is not a function or docEditor not ready");
                    }
                },
                error: function (xhr) {
                    console.error("Failed to get history data:", xhr);
                }
            });
        }

        function handleRequestHistoryClose() {
            document.location.reload();
            //console.log("關閉歷史檢視，將重新啟動編輯器");
            //if (lastEditorConfig) {
            //    initEditor(lastEditorConfig); // 重啟編輯器，不 reload
            //} else {
            //    console.warn("找不到之前的 editor config");
            //}
        }

        function handleRequestRestore(event) {
            const version = event.data.version;
            const fileUrl = event.data.url;
            const fileType = event.data.fileType;
            const changes = event.data.changes; 
            const onlyofficeguid = $("#cGuid").val();

            console.log("guid:", $("#cGuid").val());
            console.log("使用者請求還原版本:", version);
            console.log("還原檔案連結:", fileUrl);
            console.log("變更摘要:", changes);

            $.ajax({
                type: "GET",
                url: "http://172.20.10.5:54315/Handler/DocumentHistoryHandler.aspx",
                data: {
                    type: "03",
                    guid: onlyofficeguid,
                    version: version
                },
                dataType: "json",
                success: function () {
                    alert("成功還原版本，重新載入最新內容");
                    document.location.reload(); 
                },
                error: function (xhr) {
                    console.error("Failed to get history data:", xhr);
                }
            });
        }
    </script>
</head>
<body class="bgB">
	<!-- 開頭用div:修正mmenu form bug -->
	<form id="form1">
        <input type="hidden" id="cGuid" />
        <input type="hidden" id="pGuid" />
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
        <div id="div_container" class="container BoxBgWa BoxShadowD">
            <div class="WrapperBody" id="WrapperBody">            
                <!--#include file="OilSuggestionImportHeader.html"-->            
                <div id="ContentWrapper">
                    <div class="container margin15T">  
                        <div class="padding10ALL">
                            <div class="filetitlewrapper">
                                <span class="filetitle font-size7">文件上傳</span>
                                <span class="btnright"></span>
                            </div>
                            <div id="drag-and-drop-zone" class="dm-uploader margin10T">
                                <div class="margin10T"><i class="fa fa-cloud-upload font-size15 IconCb" aria-hidden="true"></i></div>
                                <div class="margin20B">
                                    請利用範本填寫資料並上傳系統
                                </div>
                                <div class="textcenter witdh100 margin20B">
                                <div class="btn btn-block">
                                    <span class="btntype">上傳</span>
                                    <input id="fileUpload" type="file" title='上傳' />
                                </div>
                                </div>
                            </div><!-- /uploader -->
                        </div>
                    </div><!-- container -->
                </div><!-- ContentWrapper -->
        <div class="container-fluid">
        <div class="backTop"><a href="#" class="backTotop">TOP</a></div>
        </div>        
        </div><!-- WrapperBody -->
        
        <div class="WrapperFooter">
            <div class="footerblock container font-normal">
                版權所有©2021 工研院材化所｜ 建議瀏覽解析度1024x768以上<br />
            </div><!--{* footerblock *}-->
        </div><!-- WrapperFooter -->
        
        </div><!-- BoxBgWa -->

        <%--<div class="twocol">
            <div id="div_file" class="left font-size3 "><i class="fa fa-chevron-circle-right IconCa" aria-hidden="true"></i> 
                <input id="fileUpload" type="file" />
            	<a href="javascript:void(0);" id="subbtn" class="genbtn">上傳</a>
            </div>
            <div class="right">
            </div>
	    </div>--%>
		<!-- 側邊選單內容:動態複製主選單內容 -->
		<div id="sidebar-wrapper"></div><!-- sidebar-wrapper -->
        
	</form>
	<!-- 結尾用div:修正mmenu form bug -->
    <div id="placeholder" class="editor" ></div>


		<script type="text/javascript" src="../js/GenCommon.js"></script><!-- UIcolor JS -->
		<script type="text/javascript" src="../js/PageCommon.js"></script><!-- 系統共用 JS -->
		<%--<script type="text/javascript" src="../js/MenuGas.js"></script><!-- 系統共用 JS -->--%>
		<script type="text/javascript" src="../js/autoHeight.js"></script><!-- 高度不足頁面的絕對置底footer -->
	</body>
</html>

