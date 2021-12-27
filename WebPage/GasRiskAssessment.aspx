<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GasRiskAssessment.aspx.cs" Inherits="WebPage_GasRiskAssessment" %>

<!DOCTYPE html>

<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta http-equiv="X-UA-Compatible" content="IE=11; IE=10; IE=9; IE=8" />
	<meta name="viewport" content="width=device-width, initial-scale=1">
	<meta name="keywords" content="關鍵字內容" />
	<meta name="description" content="描述" /><!--告訴搜尋引擎這篇網頁的內容或摘要。--> 
	<meta name="generator" content="Notepad" /><!--告訴搜尋引擎這篇網頁是用什麼軟體製作的。--> 
	<meta name="author" content="工研院 資訊處" /><!--告訴搜尋引擎這篇網頁是由誰製作的。-->
	<meta name="copyright" content="本網頁著作權所有" /><!--告訴搜尋引擎這篇網頁是...... --> 
	<meta name="revisit-after" content="3 days" /><!--告訴搜尋引擎3天之後再來一次這篇網頁，也許要重新登錄。-->
    <title>天然氣事業輸儲設備查核及檢測資訊系統</title>
	<!--#include file="Head_Include.html"-->
	<script type="text/javascript">
		$(document).ready(function () {
		    $(".container").css("max-width","1800px");
			getData();
		}); // end js

		function getData() {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetGasRiskAssessment.aspx",
				data: {
					cpid: $.getQueryString("cp")
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
						$("#tablist tbody").empty();
						var tabstr = '';
						if ($(data).find("data_item").length > 0) {
							$(data).find("data_item").each(function (i) {
								tabstr += '<tr>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("長途管線識別碼").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("最近一次執行日期").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("再評估時機").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("管線長度").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("分段數量").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("已納入ILI結果").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("已納入CIPS結果").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("已納入巡管結果").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("各等級風險管段數量_高").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("各等級風險管段數量_中").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("各等級風險管段數量_低").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("降低中高風險管段之相關作為文件名稱").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("改善後風險等級高中低").text().trim() + '</td>';
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="13">查詢無資料</td></tr>';
						$("#tablist tbody").append(tabstr);
					}
				}
			});
		}
	</script>
</head>
<body class="bgG">
<!-- 開頭用div:修正mmenu form bug -->
<div>
<form>
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
		<!--#include file="GasHeader.html"-->

        <div id="ContentWrapper">
            <div class="container margin15T">
                <div class="padding10ALL">
                    <div class="filetitlewrapper"><!--#include file="GasBreadTitle.html"--></div>

                    <div class="row margin20T">
                        <div class="col-lg-3 col-md-4 col-sm-5">
                            <div id="navmenuV"><!--#include file="GasLeftMenu.html"--></div>
                        </div>
                        <div class="col-lg-9 col-md-8 col-sm-7">

                            <div class="stripeMeG tbover">
                                <table id="tablist" width="100%" border="0" cellspacing="0" cellpadding="0">
									<thead>
										<tr>
											<th  rowspan="2">長途管線 <br>識別碼 </th>
											<th  rowspan="2">最近一次 <br>執行日期 <br>(年/月)</th>
											<th  rowspan="2">再評估時機 <br>1.定期(5年)<br>2.風險因子異動 </th>
											<th  rowspan="2">管線長度 <br>(公里)</th>
											<th  rowspan="2">分段數量 </th>
											<th  rowspan="2">已納入 <br>ILI結果 <br>(4)</th>
											<th  rowspan="2">已納入CIPS結果 <br>(5)</th>
											<th  rowspan="2">已納入 <br>巡管結果 <br>1.是 <br>2.否 <br>(6)</th>
											<th width="85" colspan="3">各等級風險 <br>管段數量 </th>
											<th  rowspan="2">降低中高風險管段之相關作為文件名稱 </th>
											<th  rowspan="2">改善後 <br>風險等級 <br>高、中、低 </th>
										</tr>
										<tr>
											<th >高 </th>
											<th >中 </th>
											<th >低 </th>
										</tr>
									</thead>
									<tbody></tbody>
                                </table>
                            </div><!-- stripeMe -->
                            <div class="margin5TB font-size2">
                                (1) 風險評估相關教育訓練：包含公司內部自行辦理或至其他機構辦理之風險評估教育訓練。<br>
                                (2) 訓練課程屬於內部訓練者，請填1；外部訓練者，請填2。<br>
                                (3) 再評估時機：最近一次所執行之評估是公司定期規劃(例：每5年一次)，或因風險評估之因子有所異動 (例：遷管、換管)而執行。<br>
                                (4) 執行該管線風險評估時，已將ILI檢測結果納入評估參數，請填寫檢測時間，若尚未考量ILI檢測結果，或該管線尚未執行ILI檢測者，請填NA。<br>
                                (5) 執行該管線風險評估時，已將CIPS檢測結果納入評估參數，請填檢測時間，若尚未考量CIPS檢測結果者，請填NA。<br>
                                (6) 執行該管線風險評估時，已將巡管結果(如：未會勘而開挖頻度)納入評估參數，請填「1」，若尚未考量巡管結果者，請填「2」。<br>
                                (7) 各等級風險管段數量：請分別填入高、中、低風險之管段數量。<br>
                                (8) 若評估結果有中高風險管段，應於「降低中高風險管段之作為」欄位註明相對應之作為或其作為相關文件名稱，並於「改善後風險等級」欄位中，填入改善後之風險等級(高、中、低)。

                            </div>


                        </div><!-- col -->
                    </div><!-- row -->



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
<!-- 本頁面使用的JS -->
	<script type="text/javascript" src="../js/GenCommon.js"></script><!-- UIcolor JS -->
	<script type="text/javascript" src="../js/PageCommon.js"></script><!-- 系統共用 JS -->
	<script type="text/javascript" src="../js/MenuGas.js"></script><!-- 系統共用 JS -->
	<script type="text/javascript" src="../js/SubMenuGasA.js"></script><!-- 內頁選單 -->
	<script type="text/javascript" src="../js/autoHeight.js"></script><!-- 高度不足頁面的絕對置底footer -->
</body>
</html>
