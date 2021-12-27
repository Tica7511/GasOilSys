<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Error" %>

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
	<!--#include file="../WebPage/Head_Include.html"-->
	<link href="../css/login.css" rel="stylesheet" type="text/css" />

	<script type="text/javascript">
		$(document).ready(function () {
			$(document).on("keyup", "body", function (e) {
				if (e.keyCode == 13)
					$("#lgbtn").click();
            });
		});
	</script>
</head>
<body>
	<form id="form1">
		<div class="loginwrapper padding10RL">
			<div class="loginblock">
				<div class="loginheader textcenter">
					<img src="<%= ResolveUrl("~/images/boe-logo.png") %>" class="imgcenter">
					<div class="font-size6 font-shadowA font-bold">石油與天然氣輸儲設備查核及檢測雲端平台</div>
				</div>

				<div class="padding10ALL">
					<div class="OchiFixTable width100 TitleLength03 font-size3 margin10T">
						<div class="OchiRow">
							<span class="font-size5 font-shadowA font-bold" style="color:red">系統出現錯誤!!!</span><br />
						</div><!-- OchiRow -->
						<div class="OchiRow">
							<span class="font-size4 font-shadowA font-bold" style="color:red">請聯絡系統管理人 賴斐瓔小姐</span><br />
						</div><!-- OchiRow -->
						<div class="OchiRow">
							<span class="font-size4 font-shadowA font-bold" style="color:red">信箱: laputa@itri.org.tw</span><br />                           
						</div><!-- OchiRow -->   
                        <div class="OchiRow">
							<span class="font-size4 font-shadowA font-bold" style="color:red">電話: 0912533858</span>                           
						</div><!-- OchiRow -->
					</div><!-- OchiFixTable -->
				</div>
				<div class="margin20T textcenter"><a id="lgbtn" href="../Handler/SignOut.aspx" class="fullbtn loginbtn">返回首頁</a></div>
			</div>
		</div>
	</form>

	<script type="text/javascript" src="js/GenCommon.js"></script>
	<!-- UIcolor JS -->
</body>
</html>

