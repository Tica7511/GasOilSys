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
	<link href="../css/bootstrap.css" rel="stylesheet" /><!-- normalize & bootstrap's grid system -->
	<link href="../css/font-awesome.min.css" rel="stylesheet" /><!-- css icon -->
	<link href="../css/OchiColor.css" rel="stylesheet" type="text/css" />
	<link href="../css/OchiLayout.css" rel="stylesheet" type="text/css" />
	<link href="../css/OchiRWD.css" rel="stylesheet" type="text/css" /><!-- ochsion layout RWD -->
	<link href="../css/login.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript" src="../js/jquery-3.4.1.min.js"></script>
	<script type="text/javascript">
        $(document).ready(function () {

            switch ($("#Competence").val()) {
                case "01":
                    $("#tdWeekReport").hide();
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
                    $("#tdWeekReport").hide();
                    break;
                //中油長官
                case "05":
                    $("#tdWeekReport").hide();
                    break;
                //台塑長官
                case "06":
                    $("#tdGas").hide();
                    $("#tdWeekReport").hide();
                    break;
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
            <img src="<%= ResolveUrl("~/images/boe-logo.png") %>" class="imgcenter" />
            <div class="font-size8 font-shadowA font-bold">石油與天然氣輸儲設備查核及檢測雲端平台</div>
        </div>
    	<div class="padding10ALL">
            <table width="100%" class="entertable">
                <tr>
                    <td width="33%" id="tdOil">
                        <a href="OilCompanyList.aspx" id="" target="_blank" class="enerbtn">
                            <i class="fa fa-code-fork font-sizeIcon" aria-hidden="true"></i>
                            <div class="font-size5 font-bold">石油業輸儲設備</div>
                            <div class="font-size3">查核及檢測資訊系統</div>
                        </a>
                    </td>
                    <td width="33%" id="tdGas">
                        <a href="GasCompanyList.aspx" target="_blank" class="enerbtn">
                            <i class="fa fa-fire font-sizeIcon" aria-hidden="true"></i>
                            <div class="font-size5 font-bold">天然氣事業輸儲設備</div>
                            <div class="font-size3">查核及檢測資訊系統</div>
                        </a>
                    </td>
                    <td width="33%" id="tdWeekReport">
                        <a href="WeekIndex.aspx" target="_blank" class="enerbtn">
                            <i class="fa fa-calendar font-sizeIcon" aria-hidden="true"></i>
                            <div class="font-size5 font-bold">週報、季報、月報</div>
                            <div class="font-size3">管理系統</div>
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
<script type="text/javascript" src="js/GenCommon.js"></script><!-- UIcolor JS -->    
</body>
</html>
