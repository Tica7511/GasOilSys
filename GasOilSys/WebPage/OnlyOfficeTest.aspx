<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OnlyOfficeTest.aspx.cs" Inherits="WebPage_OnlyOfficeTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
	<script src="http://172.20.10.5:8088/web-apps/apps/api/documents/api.js"></script>
    <title></title>
	<style>
		html, body {
		  margin: 0;
		  padding: 0;
		  height: 100%;
		}

		.editor {
		  width: 100%;
		  height: 100vh; /* 100% 視窗高度 */
		  border: none;
		}
  </style>
    <!--#include file="Head_Include.html"-->
    <script type="text/javascript">
        $(document).ready(function () {
            var docEditor = new DocsAPI.DocEditor("placeholder", {
                document: {
                    fileType: "docx",
                    key: "test1key",
                    title: "test1.docx",
                    url: "http://172.20.10.5:8088/example/files/test1.docx"
                },
                documentType: "word",
                editorConfig: {
                    mode: "view",
                    lang: "zh-TW"
                }
            });
        });
    </script>
</head>
<body>
    <div id="placeholder" class="editor" ></div>
</body>
</html>
