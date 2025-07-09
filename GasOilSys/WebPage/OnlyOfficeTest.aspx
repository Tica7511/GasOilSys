<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OnlyOfficeTest.aspx.cs" Inherits="WebPage_OnlyOfficeTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
	<script src="http://172.20.10.5:8080/web-apps/apps/api/documents/api.js"></script>
    <title></title>
	<style>
		.editor {
          width: 100%;
          height: 100vh; /* 100% 視窗高度 */
          border: none;
        }
  </style>
    <!--#include file="Head_Include.html"-->
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
                            const fileName = $("fileName", data).text();
                            const authToken = $("token", data).text();
                            const onlyofficeguid = $("onlyofficeguid", data).text();
                            localStorage.setItem('authToken', authToken);
                            initOnlyOfficeViewer(fileName, authToken, onlyofficeguid);
                        }
                    }
                });
            });

            function initOnlyOfficeViewer(fileName, authToken, onlyofficeguid) {
                console.log("Initializing OnlyOffice Viewer for:", fileName, "\rToken:", authToken);

                new DocsAPI.DocEditor("placeholder", {
                    "document": {
                        "fileType": "docx",
                        "key": onlyofficeguid,
                        "title": fileName,
                        "url": "http://172.20.10.5:54315/DOWNLOAD.aspx?category=Oil&type=suggestionimport&v=" + encodeURIComponent(fileName)
                    },
                    "documentType": "word",
                    "editorConfig": {
                        "mode": "edit",
                        "lang": "zh-TW"
                    },
                    "token": authToken,
                    "height": "100%",
                    "width": "100%",
                    "type": "desktop"
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
    </script>
</head>
<body>
    <input id="fileUpload" type="file" /><a href="javascript:void(0);" id="subbtn" class="genbtn">上傳</a>
    <div id="placeholder" class="editor" ></div>
</body>
</html>
