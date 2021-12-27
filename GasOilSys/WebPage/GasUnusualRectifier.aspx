<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GasUnusualRectifier.aspx.cs" Inherits="WebPage_GasUnusualRectifier" %>

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
	<title>天然氣事業輸儲設備查核及檢測資訊系統</title><!--#include file="Head_Include.html"-->
	<script type="text/javascript">
		$(document).ready(function () {
			getData();
		}); // end js

		function getData() {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetGasUnusualRectifier.aspx",
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
								tabstr += '<td nowrap="nowrap">' + $(this).children("異常整流站名稱").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("異常起始日期年月").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("異常狀況").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("整流站修復進度").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("影響長途管線識別碼").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("預計完成日期").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("備註").text().trim() + '</td>';
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="7">查詢無資料</td></tr>';
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
														<th>異常整流站<br>名稱</th>
														<th>異常起始日期<br>(年/月)</th>
														<th>異常狀況</th>
														<th>整流站修復進度<br>1.公司報修<br>2.設計中<br>3.向地方主管機關提出申請中<br>4.修復中</th>
														<th>影響長途管線識別碼</th>
														<th>預計完成日期</th>
														<th>備註</th>
													</tr>
												</thead>
												<tbody></tbody>
											</table>
										</div><!-- stripeMe -->
										<div class="margin5TB font-size2">
											(1) 管壁減薄請依腐蝕位置(內部、外部)、減薄量30%、40%、50%及變形量>12%分別填寫數量。<br>
											(2) 開挖確認數量：已依檢測結果進行開挖確認的數量。<br>
											(3) 改善完成數量：經開挖確認後，進行改善(例：銲補、換管、貼補等)。<br>
											(4) 若ILI執行檢測之管線，有多段管線編號，若無法分段統計管壁減薄數量，則擇一段管線編號填寫全線數量，其他段之管線，則於備註欄註明同一檢測管線之編號。
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
