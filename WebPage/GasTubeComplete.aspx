<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GasTubeComplete.aspx.cs" Inherits="WebPage_GasTubeComplete" %>

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
			$(".container").css("max-width", "1800px");
			getData();
		}); // end js

		function getData() {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetGasTubeComplete.aspx",
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
						// 幹線及環線管線
						$("#tablist tbody").empty();
						var tabstr = '';
						if ($(data).find("data_item").length > 0) {
							$(data).find("data_item").each(function (i) {
								tabstr += '<tr>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("長途管線識別碼").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("風險評估年月").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("智慧型通管器ILI可行性").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("耐壓強度試驗TP可行性").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("緊密電位CIPS年月").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("電磁包覆PCM年月").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("智慧型通管器ILI年月").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("耐壓強度試驗TP年月").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("耐壓強度試驗TP介質").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("試壓壓力與MOP壓力倍數").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("耐壓強度試驗TP持壓時間").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("受雜散電流影響").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("洩漏偵測系統LLDS").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("強化作為").text().trim() + '</td>';
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="14">查詢無資料</td></tr>';
						$("#tablist tbody").append(tabstr);

						// 幹線及環線管線以外
						$("#tablist_out tbody").empty();
						tabstr = '';
						if ($(data).find("data_item_out").length > 0) {
							$(data).find("data_item_out").each(function (i) {
								tabstr += '<tr>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("長途管線識別碼").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("風險評估年月").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("智慧型通管器ILI可行性").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("耐壓強度試驗TP可行性").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("緊密電位CIPS年月").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("電磁包覆PCM年月").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("智慧型通管器ILI年月").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("耐壓強度試驗TP年月").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("耐壓強度試驗TP介質").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("試壓壓力與MOP壓力倍數").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("耐壓強度試驗TP持壓時間").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("受雜散電流影響").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("洩漏偵測系統LLDS").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("強化作為").text().trim() + '</td>';
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="14">查詢無資料</td></tr>';
						$("#tablist_out tbody").append(tabstr);
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
                            <div class="font-size3">幹線及環線管線</div>
                            <div class="stripeMeG tbover">
                                <table id="tablist" width="100%" border="0" cellspacing="0" cellpadding="0">
									<thead>
										<tr>
											<th nowrap rowspan="2">長途管線識別碼 </th>
											<th nowrap>風險評估 <br>年/月 </th>
											<th nowrap>智慧型通管器<br>(ILI) 可行性 </th>
											<th nowrap>耐壓強度試驗 <br>(TP)<br>可行性 </th>
											<th nowrap>緊密電位<br>(CIPS)<br>年/月 </th>
											<th nowrap>電磁包覆<br>(PCM)<br>年/月 </th>
											<th nowrap>智慧型通管器<br>(ILI)<br>年/月 </th>
											<th nowrap>耐壓強度試驗<br>(TP)<br>年/月 </th>
											<th nowrap>耐壓強度試驗<br>(TP)<br>介質 </th>
											<th nowrap>試壓壓力與<br>MOP壓力倍數 </th>
											<th nowrap>耐壓強度試驗 <br>(TP)<br>持壓時間 <br>(小時)</th>
											<th nowrap>受雜散 <br>電流影響 </th>
											<th nowrap>洩漏偵測系統 <br>(LLDS)</th>
											<th nowrap rowspan="2">強化作為 </th>
										</tr>
									</thead>
									<tbody></tbody>
                                </table>
                            </div><!-- stripeMe -->

                            <div class="font-size3 margin10T">幹線及環線管線以外</div>
                            <div class="stripeMeG tbover">
                                <table id="tablist_out" width="100%" border="0" cellspacing="0" cellpadding="0">
									<thead>
										<tr>
											<th nowrap rowspan="2">長途管線識別碼 </th>
											<th nowrap>風險評估 <br>年/月 </th>
											<th nowrap>智慧型通管器<br>(ILI) 可行性 </th>
											<th nowrap>耐壓強度試驗 <br>(TP)<br>可行性 </th>
											<th nowrap>緊密電位<br>(CIPS)<br>年/月 </th>
											<th nowrap>電磁包覆<br>(PCM)<br>年/月 </th>
											<th nowrap>智慧型通管器<br>(ILI)<br>年/月 </th>
											<th nowrap>耐壓強度試驗<br>(TP)<br>年/月 </th>
											<th nowrap>耐壓強度試驗<br>(TP)<br>介質 </th>
											<th nowrap>試壓壓力與<br>MOP壓力倍數 </th>
											<th nowrap>耐壓強度試驗 <br>(TP)<br>持壓時間 <br>(小時)</th>
											<th nowrap>受雜散 <br>電流影響 </th>
											<th nowrap>洩漏偵測系統 <br>(LLDS)</th>
											<th nowrap rowspan="2">強化作為 </th>
										</tr>
									</thead>
									<tbody></tbody>
                                </table>
                            </div><!-- stripeMe -->

                            <div class="margin5TB font-size2">
                                (1) 請依各管線分別填寫。<br>
                                (2) 請提供轄區所有「幹線及環線/幹線及環線以外」管線之歷年執行之重要檢測資料。<br>
                                (3) 智慧型通管器(ILI) 可行性：請依據實際情形填寫該管線是否可執行ILI檢測，若可以，則選「可」，若有困難(如：三通、異徑等)無法執行，請選「無法」。<br>
                                (4) 耐壓強度試驗(TP)可行性：請依據實際情形填寫該管線是否可執行耐壓試驗，若可以，則選「可」，若有困難無法執行，請選「無法」。<br>
                                (5) 若有執行上述之檢測方法，請選最近一次檢測時間：年/月，若無檢測則選 “NA”。<br>
                                (6) 耐壓強度試驗(TP)介質、壓力倍數、持壓時間：若有執行耐壓強度試驗(TP)才須填寫，若無執行，則免填。<br>
                                (7) 受雜散電流影響：若管線有受雜散電流影響，請選有；反之，若無，請選無。<br>
                                (8) 強化作為：若該管線有強化作為，請簡述強化作為內容。
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
