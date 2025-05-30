﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OilCompanyList.aspx.cs" Inherits="WebPage_OilCompanyList" %>

<!DOCTYPE html>

<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta http-equiv="X-UA-Compatible" content="IE=11; IE=10; IE=9; IE=8" />
	<meta name="viewport" content="width=device-width, initial-scale=1" />
	<meta name="keywords" content="關鍵字內容" />
	<meta name="description" content="描述" /><!--告訴搜尋引擎這篇網頁的內容或摘要。--> 
	<meta name="generator" content="Notepad" /><!--告訴搜尋引擎這篇網頁是用什麼軟體製作的。--> 
	<meta name="author" content="工研院 資訊處" /><!--告訴搜尋引擎這篇網頁是由誰製作的。-->
	<meta name="copyright" content="本網頁著作權所有" /><!--告訴搜尋引擎這篇網頁是...... --> 
	<meta name="revisit-after" content="3 days" /><!--告訴搜尋引擎3天之後再來一次這篇網頁，也許要重新登錄。-->
	<title>石油業輸儲設備查核及檢測資訊系統</title>
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
                url: "../Handler/GetOilCompanyList.aspx",
                data: {
                    year: getTaiwanDate(),
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
								tabstr += '<td nowrap="nowrap">' + $(this).children("公司名稱").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("處").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("事業部").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("營業處廠").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("組").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("中心庫區儲運課工場").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("資料是否確認").text().trim() + '</td>';
								tabstr += '<td align="center" nowrap="nowrap" class="font-normal"><a href="OilInfo.aspx?cp=' + $(this).children("guid").text().trim() + '">檢視</a>';
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="8">查詢無資料</td></tr>';
						$("#tablist tbody").append(tabstr);
					}
				}
			});
        }

        //取得現在時間之民國年
        function getTaiwanDate() {
            var nowDate = new Date();

            var nowYear = nowDate.getFullYear();
            var nowTwYear = (nowYear - 1911);

            return nowTwYear;
        }
	</script>
</head>
<body class="bgB">
	<!-- 開頭用div:修正mmenu form bug -->
	<div>
	<form id="form1">
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
				<!--#include file="OilHeader.html"-->

				<div id="ContentWrapper">
            <div class="container margin15T">
                <div class="padding10ALL">
                    <div class="filetitlewrapper">
                        <span class="filetitle font-size7">石油業者列表</span>
                        <span class="btnright"></span>
                    </div>
                    

                    <div class="stripeMeB font-size3 margin10T">
                        <table id="tablist" width="100%" border="0" cellspacing="0" cellpadding="0">
                            <thead>
								<tr>
									<th nowrap="nowrap">公司名稱</th>
									<th nowrap="nowrap">處</th>
									<th nowrap="nowrap">事業部</th>
									<th nowrap="nowrap">營業處廠</th>
									<th nowrap="nowrap">組</th>
									<th nowrap="nowrap">中心庫區儲運課工場</th>
									<th nowrap="nowrap" width="130">資料是否確認</th>
									<th nowrap="nowrap" width="100">功能</th>
								</tr>
                            </thead>
                            <tbody></tbody>
                        </table>
						<div class="margin10B margin10T textcenter">
							<div id="pageblock"></div>
						</div>
                    </div><!-- stripeMe -->


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

	<!-- colorbox -->
	<div style="display:none;">
		<div id="checklistedit">
			<div class="margin35T padding5RL">
				<div class="OchiTrasTable width100 TitleLength03 font-size3">
					<div class="OchiRow">
						<div class="OchiCell OchiTitle IconCe TitleSetWidth">備註</div>
						<div class="OchiCell width100">
							<textarea id="psStr" rows="8" cols="" class="inputex width100"></textarea>
						</div>
					</div><!-- OchiRow -->
				</div><!-- OchiTrasTable -->
			</div>

			<div class="twocol margin10T">
				<div class="right">
					<a href="javascript:void(0);" class="genbtn closecolorbox">取消</a>
					<a href="javascript:void(0);" id="ps_savebtn" class="genbtn">儲存</a>
				</div>
			</div>
			<br /><br />
		</div>
	</div>


		<script type="text/javascript" src="../js/GenCommon.js"></script><!-- UIcolor JS -->
		<script type="text/javascript" src="../js/PageCommon.js"></script><!-- 系統共用 JS -->
		<script type="text/javascript" src="../js/MenuGas.js"></script><!-- 系統共用 JS -->
		<script type="text/javascript" src="../js/autoHeight.js"></script><!-- 高度不足頁面的絕對置底footer -->
	</body>
</html>
