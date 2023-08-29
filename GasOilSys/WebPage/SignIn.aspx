<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SignIn.aspx.cs" Inherits="WebPage_SignIn" %>

<!DOCTYPE html>

<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta name="viewport" content="width=device-width, initial-scale=1" />
	<meta name="keywords" content="關鍵字內容" />
	<meta name="description" content="描述" /><!--告訴搜尋引擎這篇網頁的內容或摘要。-->
	<meta name="generator" content="Notepad" /><!--告訴搜尋引擎這篇網頁是用什麼軟體製作的。-->
	<meta name="author" content="工研院 資科中心" /><!--告訴搜尋引擎這篇網頁是由誰製作的。-->
	<meta name="copyright" content="本網頁著作權所有" /><!--告訴搜尋引擎這篇網頁是...... -->
	<meta name="revisit-after" content="3 days" /><!--告訴搜尋引擎3天之後再來一次這篇網頁，也許要重新登錄。-->
	<title>石油與天然氣輸儲設備查核及檢測雲端平台</title>
	<!--#include file="Head_Include.html"-->
	<link href="../css/login.css" rel="stylesheet" type="text/css" />

	<script type="text/javascript">
		$(document).ready(function () {

			$(document).on("keyup", "body", function (e) {
				if (e.keyCode == 13)
					$("#lgbtn").click();
			});

			$(document).on("click", "#lgbtn", function () {
				$("#errMsg").empty();
				$.ajax({
					type: "POST",
					async: true, //在沒有返回值之前,不會執行下一步動作
					url: "../Handler/Login.aspx",
					data: {
						acc: $("#acStr").val(),
						pw: encodeURIComponent($("#pStr").val()),
						vCode: $("#codeStr").val()
					},
					error: function (xhr) {
						$("#errMsg").html("Error: " + xhr.status);
						console.log(xhr.responseText);
					},
					success: function (data) {
						if ($(data).find("Error").length > 0)
							$("#errMsg").html($(data).find("Error").attr("Message"));
						else {
							location.href = "Entrance.aspx";
						}
					}
				});
			});
		});

		function changeCode() {
			$("#imgCode").attr("src", "../handler/ValidationCode.aspx?" + Math.random());
        }

        function doOpenDialog() {
            var WinHeight = $("html").height();
            var ColHeight = WinHeight * 0.8;
            $.colorbox({ inline: true, href: "#passwordDia", width: "100%", maxWidth: "600", maxHeight: ColHeight, opacity: 0.5 });
        }
    </script>
</head>
<body>
	<form id="form1">
		<div class="loginwrapper padding10RL">
			<div class="loginblock">
				<div class="loginheader textcenter">
					<%--<img src="<%= ResolveUrl("~/images/boe-logo.png") %>" class="imgcenter">--%>
					<div class="font-size6 font-shadowA font-bold">石油與天然氣輸儲設備查核及檢測雲端平台</div>
				</div>

				<div class="padding10ALL">
					<div class="OchiFixTable width100 TitleLength03 font-size3 margin10T">
						<div class="OchiRow">
							<div class="OchiCell OchiTitle TitleSetWidth font-size3">帳號</div>
							<div class="OchiCell width100"><input type="text" id="acStr" class="width99 inputex" /></div>
						</div>
						<!-- OchiRow -->
						<div class="OchiRow">
							<div class="OchiCell OchiTitle TitleSetWidth font-size3">密碼</div>
							<div class="OchiCell width100"><input type="password" id="pStr" class="width99 inputex" /></div>
						</div>
						<!-- OchiRow -->
						<div class="OchiRow">
							<div class="OchiCell OchiTitle TitleSetWidth font-size3">驗證碼</div>
							<div class="OchiCell width100">
								<input type="text" id="codeStr" class="inputex" size="6" />
								<img src="../handler/ValidationCode.aspx" alt="驗證碼" id="imgCode" height="25" />
								<a href="javascript:void(0);" onclick="changeCode()"><img src="<%= ResolveUrl("~/images/refresh.png") %>" alt="變更驗證碼" height="20" /></a>
                                <a onclick="doOpenDialog()" href="javascript:void(0);">忘記密碼</a>
							</div>                            
						</div><!-- OchiRow -->                        
					</div><!-- OchiFixTable -->
				</div>
				<div id="errMsg" style="color: red; text-align: center;"></div>
				<div class="margin20T textcenter"><a id="lgbtn" href="javascript:void(0);" class="fullbtn loginbtn">登入</a></div>
			</div>
		</div>
	</form>

    <!-- colorbox -->
    <div style="display:none;">
        <div id="passwordDia">
            <div class="margin35T padding5RL">
                <span class="font-size6 font-shadowA font-bold" style="color:red">請聯絡賴斐瓔小姐</span><br />
                <span class="font-size6 font-shadowA font-bold" style="color:red">信箱: laputa@itri.org.tw</span><br />
                <span class="font-size6 font-shadowA font-bold" style="color:red">電話: 0912533858</span>
            </div>
        </div>
    </div>

	<script type="text/javascript" src="js/GenCommon.js"></script>
	<!-- UIcolor JS -->
</body>
</html>
