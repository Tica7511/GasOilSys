<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="WebPage_ChangePassword" %>

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
					$("#savebtn").click();
			});

			//儲存變更後密碼
            $(document).on("click", "#savebtn", function () {
                $("#errMsg").empty();
                var msg = '';
                if ($("#pStr").val() == "")
                    msg += "請輸入【新密碼】<br>";
                else {
                    //20210315 IRENE 修改特殊符號語法，避免底線_無法輸入
                    //var ValidPw = new RegExp(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W)[A-Za-z\d\W]{8,}$/);
                    var ValidPw = new RegExp(/^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_]).*$/);
                    if (!ValidPw.test($("#pStr").val()))
                        msg += "【新密碼】需至少8碼，含英文大小寫 & 數字 & 特殊符號<br>";
                }

                if ($("#cpStr").val() == "")
                    msg += "請輸入【確認新密碼】<br>";
                else {
                    if ($("#pStr").val() != $("#cpStr").val())
                        msg += "【新密碼】與【確認新密碼】不相符<br>";
                }

                if (msg != "") {
                    $("#errMsg").html(msg);
                    return false;
                }

                $.ajax({
                    type: "POST",
                    async: true, //在沒有返回值之前,不會執行下一步動作
                    url: "../Handler/ModPwd.aspx",
                    data: {
                        pw: encodeURIComponent($("#pStr").val()),
                    },
                    error: function (xhr) {
                        $("#errMsg").html("Error: " + xhr.status);
                        console.log(xhr.responseText)
                    },
                    success: function (data) {
                        if ($(data).find("Error").length > 0) {
                            $("#errMsg").html($(data).find("Error").attr("Message"));
                        }
                        else {
                            alert($("Response", data).text());
                            location.href = "../Handler/SignOut.aspx";
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
					<img src="<%= ResolveUrl("~/images/boe-logo.png") %>" class="imgcenter">
					<div class="font-size6 font-shadowA font-bold">石油與天然氣輸儲設備查核及檢測雲端平台</br>變更密碼</div>
				</div>

				<div class="padding10ALL">
					<div class="OchiFixTable width100 TitleLength03 font-size3 margin10T">
						<div class="OchiRow">
							<div class="OchiCell OchiTitle TitleSetWidth font-size3">新密碼</div>
							<div class="OchiCell width100"><input type="text" id="pStr" class="width99 inputex" /></div>
						</div>
						<!-- OchiRow -->
						<div class="OchiRow">
							<div class="OchiCell OchiTitle TitleSetWidth font-size3">確認</br>新密碼</div>
							<div class="OchiCell width100"><input type="password" id="cpStr" class="width99 inputex" /></div>
						</div>
						<!-- OchiRow -->                        
					</div><!-- OchiFixTable -->
				</div>
				<div id="errMsg" style="color: red; text-align: center;"></div>
				<div class="margin20T textcenter"><a id="savebtn" href="javascript:void(0);" class="fullbtn loginbtn">變更密碼</a></div>
			</div>
		</div>
	</form>

	<script type="text/javascript" src="../js/GenCommon.js"></script>
	<!-- UIcolor JS -->
</body>
</html>

