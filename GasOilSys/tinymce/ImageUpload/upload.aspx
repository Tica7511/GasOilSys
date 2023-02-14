<%@ Page Language="C#" AutoEventWireup="true" CodeFile="upload.aspx.cs" Inherits="tinymce_ImageUpload_upload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<script src="jquery-3.4.1.min.js"></script>
<script src="../../js/NickCommon.js"></script>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/javascript">
		$(document).ready(function () {

			$("#Ncategory").val($.getQueryString("category"));
			$("#Ntype").val($.getQueryString("type"));
            $("#NcpName").val($.getQueryString("cpName"));
            $("#Ncpguid").val($.getQueryString("cpguid"));

            $("#okbtn").click(function () {
                // Get form
                var form = $('#form2')[0];

                // Create an FormData object 
				var data = new FormData(form);

                data.append('category', $("#Ncategory").val());
                data.append('type', $("#Ntype").val());
                data.append('cpName', $("#NcpName").val());
                data.append('cpguid', $("#Ncpguid").val());
                data.append('file', $("#myfile").get(0).files[0]);

				$.ajax({
					type: "POST",
					async: false, //在沒有返回值之前,不會執行下一步動作
					url: "imgUpload.aspx",
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
                            var ReturnValue = '<img src="' + $("Response", data).text() + '&category=' + $("category", data).text() + '&type=' + $("type", data).text() + '&cpName=' + $("cpName", data).text() + '" alt="" />';
							top.tinymce.activeEditor.insertContent(ReturnValue);
							top.tinymce.activeEditor.windowManager.close();
						}
					}
				});
            });
        });
    </script>
	<style>
		.btn {
			background-color: #e7e7e7;
			border: none;
			color: black;
			padding: 5px 10px;
			text-align: center;
			text-decoration: none;
			display: inline-block;
			font-size: 16px;
			border-radius: 10%;
			cursor:pointer;
		}

		.inputex{border:1px solid #c5c5c5;transition: all ease-in-out 0.2s;}
	</style>
</head>
<body>
    <form id="form2" runat="server">
		<input id="Ncategory" type="hidden" />
		<input id="Ntype" type="hidden" />
		<input id="NcpName" type="hidden" />
		<input id="Ncpguid" type="hidden" />
        <div>
            <table width="100%">
                <tr>
                    <td nowrap="nowrap">附檔名限制<span style="color: red;"> .jpg、.jpeg、.png、.bmp、.gif</span></td>
                </tr>
                <tr>
                    <td><input type="file" id="myfile" name="myfile" class="inputex" /></td>
                </tr>
                <tr>
                    <td align="right"><input type="button" id="okbtn" value="確定" class="btn" /></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
