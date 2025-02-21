<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Entrance.aspx.cs" Inherits="WebPage_Entrance" %>

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
    <link rel="icon" href="../images/boe-logo-2-icon.ico" type="image/x-icon" />
	<link href="../css/bootstrap.css" rel="stylesheet" /><!-- normalize & bootstrap's grid system -->
    <link href="../css/magnific-popup.css" rel="stylesheet" type="text/css" /><!-- popup dialog -->
	<link href="../css/font-awesome.min.css" rel="stylesheet" /><!-- css icon -->
	<link href="../css/OchiColor.css" rel="stylesheet" type="text/css" />
	<link href="../css/OchiLayout.css" rel="stylesheet" type="text/css" />
	<link href="../css/OchiRWD.css" rel="stylesheet" type="text/css" /><!-- ochsion layout RWD -->
	<link href="../css/login.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/jquery-1.11.2.min.js"></script>
<script src="http://code.jquery.com/jquery-migrate-1.2.1.js"></script>
<script type="text/javascript" src="../js/jquery.magnific-popup.min.js"></script><!-- popup dialog -->
	<script type="text/javascript">
        $(document).ready(function () {
            //doOpenMagPopup();

            let idleTime = 0;
            const idleLimit = 3600; // 閒置時間限制（秒）

            function resetTimer() {
                idleTime = 0; // 重置閒置時間
            }

            function checkIdleTime() {
                idleTime++;
                if (idleTime >= idleLimit) {
                    window.location.href = 'TimeOutPage.aspx'; // 跳轉到靜態頁面
                }
            }

            // 監控用戶的各種操作
            $(this).mousemove(resetTimer);
            $(this).keypress(resetTimer);
            $(this).click(resetTimer);
            $(this).scroll(resetTimer);
            $(document).on('mousemove keypress click scroll touchstart', resetTimer);

            function checkLoginStatus() {
                $.ajax({
                    type: "POST",
                    async: true, //在沒有返回值之前,不會執行下一步動作
                    url: "../Handler/CheckLoginStatus.aspx",
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
                            if ($("Response", data).text() == 'Y') {
                                alert("已有相同的帳號登入或登入時間逾期，請重新登入");
                                location.href = "../Handler/SignOut.aspx";
                            }                                                           
                        }
                    }
                });
            }

            setInterval(checkIdleTime, 1000); // 每秒檢查一次閒置時間
            //2024.08.16 暫時註解來防止登入時驗證碼錯誤的問題
            //setInterval(checkLoginStatus, 5000) // 每5秒檢查一次是否有相同帳號登入


            $("#tdProjectManagement").hide();
            switch ($("#Competence").val()) {
                case "01":
                    $("#tdWeekReport").hide();                    
                    $("#tdStatistics").hide();
                    $("#tdPublicGas").hide();
                    switch ($("#EnterCtrl").val()) {
                        case "oil":
                            $("#tdGas").hide();
                            break;
                        case "gas":
                            $("#tdOil").hide();
                            break;
                    }
                    break;
                case "02":
                    $("#tdStatistics").hide();
                    $("#tdCheckCounseling").hide();
                    break;
                case "03":
                    switch ($("#EnterCtrl").val()) {
                        case "oil":
                            $("#tdGas").hide();
                            break;
                        case "gas":
                            $("#tdOil").hide();
                            break;
                    }
                    break;
                case "04":
                    $("#tdStatistics").hide();
                    $("#tdWeekReport").hide();
                    $("#tdPublicGas").hide();
                    break;
                //中油長官
                case "05":
                    $("#tdCheckCounseling").hide();
                    $("#tdStatistics").hide();
                    $("#tdWeekReport").hide();
                    $("#tdPublicGas").hide();
                    break;
                //台塑長官
                case "06":
                    $("#tdStatistics").hide();
                    $("#tdCheckCounseling").hide();
                    $("#tdGas").hide();
                    $("#tdWeekReport").hide();
                    $("#tdPublicGas").hide();
                    break;
            }

            function doOpenMagPopup() {
                $.magnificPopup.open({
                    items: {
                        src: '#importantDialog'
                    },
                    type: 'inline',
                    midClick: false, // 是否使用滑鼠中鍵
                    closeOnBgClick: true,//點擊背景關閉視窗
                    showCloseBtn: true,//隱藏關閉按鈕
                    fixedContentPos: true,//彈出視窗是否固定在畫面上
                    mainClass: 'mfp-fade',//加入CSS淡入淡出效果
                    tClose: '關閉',//翻譯字串
                });
            }
        });
    </script>
</head>
<body>
	<input type="hidden" id="Competence" value="<%= identity %>" />
	<input type="hidden" id="EnterCtrl" value="<%= EnterCtrl %>" />
    <div class="loginwrapper padding10RL">
    <div class="enterblock">
        <div class="loginheader textcenter">
            <%--<img src="<%= ResolveUrl("~/images/boe-logo.png") %>" class="imgcenter" />--%>
            <img src="<%= ResolveUrl("~/images/boe-logo-2.png") %>" class="imgcenter" />
            <div class="font-size8 font-shadowA font-bold">石油與天然氣輸儲設備查核及檢測雲端平台</div>
        </div>
    	<div class="padding10ALL">
            <table width="100%" class="entertable">
                <tr>
                    <td width="33%" id="tdOil">
                        <a href="OilCompanyList.aspx" id="" target="_blank" class="enerbtn">
                            <i class="fa fa-code-fork font-sizeIcon" aria-hidden="true"></i>
                            <div class="font-size5 font-bold">石油業輸儲設備</div>
                            <div class="font-size3">現場查核</div>
                        </a>
                    </td>
                    <td width="33%" id="tdGas">
                        <a href="GasCompanyList.aspx" target="_blank" class="enerbtn">
                            <i class="fa fa-fire font-sizeIcon" aria-hidden="true"></i>
                            <div class="font-size5 font-bold">天然氣事業輸儲設備</div>
                            <div class="font-size3">現場查核</div>
                        </a>
                    </td>
                   <td width="33%" id="tdPublicGas">
                        <a href="PublicGasInfo.aspx" target="_blank" class="enerbtn">
                            <i class="fa fa-cloud font-sizeIcon" aria-hidden="true"></i>
                            <div class="font-size5 font-bold">公用天然氣事業輸儲設備</div>
                            <div class="font-size3">現場查核</div>
                        </a>
                    </td>
                </tr>
                <tr>
                    <td width="33%" id="tdCheckCounseling">
                        <a href="VerificationTest.aspx" target="_blank" class="enerbtn">
                            <i class="fa fa-check-square-o font-sizeIcon" aria-hidden="true"></i>
                            <div class="font-size5 font-bold">查核與檢測資料</div>
                            <div class="font-size3">查詢系統</div>
                        </a>
                    </td>
                    <td width="33%" id="tdStatistics">
                        <a href="StatisticsPipeAndTank.aspx" target="_blank" class="enerbtn">
                            <i class="fa fa-pie-chart font-sizeIcon" aria-hidden="true"></i>
                            <div class="font-size5 font-bold">統計查詢</div>
                            <div class="font-size3">查詢系統</div>
                        </a>
                    </td>
                    <td width="33%" id="tdWeekReport">
                        <a href="WeekIndex.aspx" target="_blank" class="enerbtn">
                            <i class="fa fa-calendar font-sizeIcon" aria-hidden="true"></i>
                            <div class="font-size5 font-bold">週報、季報、月報</div>
                            <div class="font-size3">管理系統</div>
                        </a>
                    </td>
                    <td width="33%" id="tdProjectManagement">
                        <a href="week-index.html" target="_blank" class="enerbtn">
                            <i class="fa fa-briefcase font-sizeIcon" aria-hidden="true"></i>
                            <div class="font-size5 font-bold">計劃管理文件</div>
                        </a>
                    </td>
                </tr>
            </table>
            <div class="textcenter margin10T">
				版權所有©2021 工研院材化所｜ 建議瀏覽解析度1024x768以上<br />
			</div>
        </div>
    </div>
	</div>
    <!-- Magnific Popup -->
<div id="importantDialog" class="magpopup magSizeS mfp-hide">
  <div class="magpopupTitle" style="color:red"><b>重要公告</b></div>
<div class="padding10ALL">
      <div class="center ">
          <div class="font-size5" ><b>行政院關心大家,請大家務必參考以下資訊:</b></div><br />
          <div class="font-size5">
              <i class="fa fa-file-pdf-o IconCc" aria-hidden="true"></i> <a href="../doc/高氣溫戶外作業勞工熱危害預防指引-112-6-1修正.pdf" target="_blank">高氣溫戶外作業勞工熱危害預防</a>
          </div>
      </div>

      <%--<div class="twocol margin10T">
            <div class="right">
                <a id="importCancelbtn" href="javascript:void(0);" class="genbtn closemagnificPopup">取消</a>
            </div>
        </div>--%>

  </div><!-- padding10ALL -->

</div><!--magpopup -->
<script type="text/javascript" src="../js/GenCommon.js"></script><!-- UIcolor JS -->   
    <script type="text/javascript" src="../js/PageCommon.js"></script><!-- 系統共用 JS -->
</body>
</html>
