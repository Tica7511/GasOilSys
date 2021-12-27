<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GasCIPS.aspx.cs" Inherits="WebPage_GasCIPS" %>

<!DOCTYPE html>

<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta http-equiv="X-UA-Compatible" content="IE=11; IE=10; IE=9; IE=8" />
	<meta name="viewport" content="width=device-width, initial-scale=1">
	<meta name="keywords" content="關鍵字內容" />
	<meta name="description" content="描述" />
	<!--告訴搜尋引擎這篇網頁的內容或摘要。-->
	<meta name="generator" content="Notepad" />
	<!--告訴搜尋引擎這篇網頁是用什麼軟體製作的。-->
	<meta name="author" content="工研院 資訊處" />
	<!--告訴搜尋引擎這篇網頁是由誰製作的。-->
	<meta name="copyright" content="本網頁著作權所有" />
	<!--告訴搜尋引擎這篇網頁是...... -->
	<meta name="revisit-after" content="3 days" />
	<!--告訴搜尋引擎3天之後再來一次這篇網頁，也許要重新登錄。-->
	<title>天然氣事業輸儲設備查核及檢測資訊系統</title>
	<!--#include file="Head_Include.html"-->
	<script type="text/javascript">
		$(document).ready(function () {
			getData();
		}); // end js

		function getData() {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetGasCIPS.aspx",
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
								tabstr += '<td nowrap="nowrap">' + $(this).children("同時檢測管線數量").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("最近一次執行年月").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("報告產出年月").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("檢測長度").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("合格標準請參照填表說明2").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("立即改善_數量").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("立即改善_改善完成數量").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("排程改善_數量").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("排程改善_改善完成數量").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("需監控點_數量").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("備註").text().trim() + '</td>';
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="12">查詢無資料</td></tr>';
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
			<div id="preloader">
				<div id="status">
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
														<th rowspan="2">長途管線<br />識別碼</th>
														<th rowspan="2">同時檢測管線數量</th>
														<th rowspan="2">最近一次執行<br />年/月</th>
														<th rowspan="2">報告產出<br />年/月</th>
														<th rowspan="2">檢測長度<br />(公里)</th>
														<th rowspan="2">合格標準<br />請參照<br />填表說明(2)</th>
														<th colspan="2">立即改善</th>
														<th colspan="2">排程改善</th>
														<th>需監控點</th>
														<th rowspan="2">備註</th>
													</tr>
													<tr>
														<th>數量</th>
														<th>改善完成數量</th>
														<th>數量</th>
														<th>改善完成數量</th>
														<th>數量</th>
													</tr>
												</thead>
												<tbody></tbody>
											</table>
										</div><!-- stripeMe -->
										<div class="margin5TB font-size2">
											(1) 合格標準：請依據該管線檢測報告判定結果時，所引用之標準，請填入相對應之數字， 1. 通電電位< -850mVCSE  2.極化電位< -850mVCSE  3.極化量>100mV  4.其他<br>
											(2) 訊號異常點_數量：依據公司之檢測合格標準，所判定訊號異常的點數。<br>
											(3) 訊號異常點_確認數量：排除箱涵、水泥遮蔽等訊號所剩數量。<br>
											(4) 訊號異常點_改善完成數量：確定已改善完成的數量。<br>
                                            (5) 備註：若檢測時之管線數量2條以上(含)，請以同一代號註明同一管束，如：以A、B…區別。
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
