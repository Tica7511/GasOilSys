<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GasTubeCheck.aspx.cs" Inherits="WebPage_GasTubeCheck" %>

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
			getData();
		}); // end js

		function getData() {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetGasTubeCheck.aspx",
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
								tabstr += '<td nowrap="nowrap">' + $(this).children("依據文件名稱").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("文件編號").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("文件日期").text().trim() + '</td>';
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="3">查詢無資料</td></tr>';
						$("#tablist tbody").append(tabstr);

						if ($(data).find("data_item2").length > 0) {
                            $(data).find("data_item2").each(function (i) {
								$("input[name='checkCount'][value='" + $(this).children("每日巡檢次數").text().trim() + "']").prop("checked", true);
								$("#checkPerson").val($(this).children("巡管人數").text().trim());
								// 巡管工具
								var othertool = false;
								var arychecktool = $(this).children("巡管工具").text().trim().split(',');
								$.each(arychecktool, function (key, value) {
									$("input[name='checkTool'][value='" + value + "']").prop("checked", true);

									if (value == "03")
										othertool = true;
								});
								if (othertool)
									$("#checkToolOther").val($(this).children("巡管工具其他").text().trim());

								// 主管監督查核
								$("input[name='managerCheck'][value='" + $(this).children("主管監督查核").text().trim() + "']").prop("checked", true);
								if ($(this).children("主管監督查核").text().trim() == "Y")
									$("#ManagerCount").val($(this).children("主管監督查核次").text().trim());

								// 是否有加強巡檢點
								$("input[name='checkStrengthen'][value='" + $(this).children("是否有加強巡檢點").text().trim() + "']").prop("checked", true);
								if ($(this).children("是否有加強巡檢點").text().trim() == "Y")
									$("#StrengthenTxt").val($(this).children("是否有加強巡檢點敘述").text().trim());
							});
						}

						$("#tablist2 tbody").empty();
						var tabstr = '';
						if ($(data).find("data_item3").length > 0) {
							$(data).find("data_item3").each(function (i) {
								tabstr += '<tr>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("管線巡檢情形").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("前兩年").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("前一年").text().trim() + '</td>';
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="3">查詢無資料</td></tr>';
						$("#tablist2 tbody").append(tabstr);
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

                            <div class="OchiTrasTable width100 TitleLength08 font-size3">
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">每日巡檢次數</div>
                                        <div class="OchiCell width100">
                                            <input type="radio" name="checkCount" value="01" disabled> 1次  ； <input type="radio" name="checkCount" value="02" disabled> 2次  ；<input type="radio" name="checkCount" value="03" disabled> 3次(含)以上
                                        </div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">巡管人數</div>
                                        <div class="OchiCell width100">
                                            <input type="text" id="checkPerson" class="inputex" size="4" disabled> 人
                                        </div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">巡管工具</div>
                                        <div class="OchiCell width100">
                                            <input type="checkbox" name="checkTool" value="01" disabled> PDA <input type="checkbox" name="checkTool" value="02" disabled> 手機 <input type="checkbox" name="checkTool" value="03" disabled >  其他 <input type="text" id="checkToolOther" class="inputex" size="10" disabled />
                                        </div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">主管監督查核</div>
                                        <div class="OchiCell width100"><input type="radio" name="managerCheck" value="Y" disabled> 有  至少 <input type="text" id="ManagerCount" class="inputex" disabled size="4"> 次/月(季)  <input type="radio" name="managerCheck" value="N" disabled> 無</div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->

                                <div class="OchiRow">
                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">是否有加強巡檢點？</div>
                                    <div class="OchiCell width100">
                                        <input type="radio" name="checkStrengthen" value="Y" disabled> 有 <input type="text" id="StrengthenTxt" class="inputex width80" disabled>  <input type="radio" name="checkStrengthen" value="N" disabled> 無
                                    </div>
                                </div><!-- OchiRow -->

                            </div><!-- OchiTrasTable -->

                            <div class="font-size3 margin10T">依據文件資料:</div>
                            <div class="stripeMeG tbover margin5T">
                                <table id="tablist" width="100%" border="0" cellspacing="0" cellpadding="0">
									<thead>
										<tr>
											<th >依據文件名稱 </th>
											<th >文件編號 </th>
											<th >文件日期 </th>
										</tr>
									</thead>
									<tbody></tbody>
                                </table>
                            </div><!-- stripeMe -->

                            <div class="font-size3 margin10T">異常情形統計資料:</div>
                            <div class="stripeMeG tbover margin5T">
                                <table id="tablist2" width="100%" border="0" cellspacing="0" cellpadding="0">
									<thead>
										<tr>
											<th >管線巡檢情形 </th>
											<th >前二年 </th>
											<th >前一年 </th>
										</tr>
									</thead>
									<tbody></tbody>
                                </table>
                            </div><!-- stripeMe -->
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
