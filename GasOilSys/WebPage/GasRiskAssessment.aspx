﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GasRiskAssessment.aspx.cs" Inherits="WebPage_GasRiskAssessment" %>

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
            getYearList();
            $("#sellist").val(getTaiwanDate());
            getData(getTaiwanDate());
            $("#exportbtn").attr("href", "../Gas_EXPORTEXCEL.aspx?cpid=" + $.getQueryString("cp") + "&year=" + getTaiwanDate() + "&category=riskassessment");

            //選擇年份
            $(document).on("change", "#sellist", function () {
                getData($("#sellist option:selected").val());
                $("#exportbtn").attr("href", "../Gas_EXPORTEXCEL.aspx?cpid=" + $.getQueryString("cp") + "&year=" + $("#sellist option:selected").val() + "&category=riskassessment");
            });

            //新增按鈕
            $(document).on("click", "#newbtn", function () {
                location.href = "edit_GasRiskAssessment.aspx?cp=" + $.getQueryString("cp");
            });

            //刪除按鈕
            $(document).on("click", "a[name='delbtn']", function () {
                if (confirm("確定刪除?")) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../handler/DelGasRiskAssessment.aspx",
                        data: {
                            guid: $(this).attr("aid"),
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
                                alert($("Response", data).text());
                                getData($("#sellist").val());
                            }
                        }
                    });
                }
            });
		}); // end js

		function getData(year) {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetGasRiskAssessment.aspx",
				data: {
                    cpid: $.getQueryString("cp"),
                    year: year,
                    type: "list",
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
								tabstr += '<td>' + $(this).children("降低中高風險管段之相關作為文件名稱").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("改善後風險等級高中低").text().trim() + '</td>';
                                tabstr += '<td name="td_edit" nowrap="" align="center"><a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("guid").text().trim() + '">刪除</a>';
                                tabstr += ' <a href="edit_GasRiskAssessment.aspx?cp=' + $.getQueryString("cp") + '&guid=' + $(this).children("guid").text().trim() + '" name="editbtn">編輯</a></td>';
                                tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="14">查詢無資料</td></tr>';
						$("#tablist tbody").append(tabstr);

                        //確認權限&按鈕顯示或隱藏
                        if ($("#sellist").val() != getTaiwanDate()) {
                            $("#newbtn").hide();
                            $("#th_edit").hide();
                            $("td[name='td_edit']").hide();
                        }
                        else {
                            if (($("#Competence").val() == '01') || ($("#Competence").val() == '04') || ($("#Competence").val() == '05') || ($("#Competence").val() == '06')) {
                                $("#newbtn").hide();
                                $("#th_edit").hide();
                                $("td[name='td_edit']").hide();
                            }
                            else {
                                $("#newbtn").show();
                                $("#th_edit").show();
                                $("td[name='td_edit']").show();
                            }
                        }

                        getConfirmedStatus();
					}
				}
			});
        }

        //確認資料是否完成
        function getConfirmedStatus() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetCompanyName.aspx",
                data: {
                    type: "Gas",
                    cpid: $.getQueryString("cp"),
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
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                var dataConfirm = $(this).children("資料是否確認").text().trim();

                                if ($("#Competence").val() != '03') {
                                    if (dataConfirm == "是") {
                                        $("#newbtn").hide();
                                        $("#editbtn").hide();
                                        $("#th_edit").hide();
                                        $("td[name='td_edit']").hide();
                                    }
                                }                                
                            });
                        }
                    }
                }
            });
        }

        //取得民國年份之下拉選單
        function getYearList() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetGasRiskAssessment.aspx",
                data: {
                    cpid: $.getQueryString("cp"),
                    year: getTaiwanDate(),
                    type: "list",
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
                        $("#sellist").empty();
                        var ddlstr = '';
                        if ($(data).find("data_item2").length > 0) {
                            $(data).find("data_item2").each(function (i) {
                                ddlstr += '<option value="' + $(this).children("年度").text().trim() + '">' + $(this).children("年度").text().trim() + '</option>'
                            });
                        }
                        else {
                            ddlstr += '<option>請選擇</option>'
                        }
                        $("#sellist").append(ddlstr);
                    }
                }
            });
        }

        //年月日格式=> yyyy/mm/dd
        function getDate(fulldate) {

            if (fulldate != '') {
                var twdate = '';

                var farray = new Array();
                farray = fulldate.split("/");

                if (farray.length > 1) {
                    twdate = farray[0] + farray[1] + farray[2];
                }
                else {
                    twdate = fulldate;
                }

                if (twdate.length > 6) {
                    twdate = twdate.substring(0, 3) + "/" + twdate.substring(3, 5) + "/" + twdate.substring(5, 7);
                }
                else {
                    twdate = twdate.substring(0, 2) + "/" + twdate.substring(2, 4) + "/" + twdate.substring(4, 6);
                }

                return twdate;
            }
            else {
                return '';
            }

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
        <input type="hidden" id="Competence" value="<%= competence %>" />
        <div id="ContentWrapper">
            <div class="container margin15T">
                <div class="padding10ALL">
                    <div class="filetitlewrapper"><!--#include file="GasBreadTitle.html"--></div>

                    <div class="row margin20T">
                        <div class="col-lg-3 col-md-4 col-sm-5">
                            <div id="navmenuV"><!--#include file="GasLeftMenu.html"--></div>
                        </div>
                        <div class="col-lg-9 col-md-8 col-sm-7">
                            <div class="twocol">
                                <div class="left font-size5 "><i class="fa fa-chevron-circle-right IconCa" aria-hidden="true"></i> 
                                    <select id="sellist" class="inputex">
                                    </select> 年
                                </div>
                                <div class="right">
                                    <a id="exportbtn" href="javascript:void(0);" title="匯出" class="genbtn">匯出</a>
                                    <a id="newbtn" href="javascript:void(0);" title="新增" class="genbtn">新增</a>
                                </div>
                            </div><br />
                            <div class="stripeMeG tbover">
                                <table id="tablist" width="100%" border="0" cellspacing="0" cellpadding="0">
									<thead>
										<tr>
											<th  rowspan="2">長途管線 <br>識別碼 </th>
											<th  rowspan="2">最近一次 <br>執行日期 <br>(年/月)</th>
											<th  rowspan="2">再評估時機 <br>1.定期(5年)<br>2.風險因子異動 </th>
											<th  rowspan="2">管線長度 <br>(公里)</th>
											<th  rowspan="2">分段數量 </th>
											<th  rowspan="2">已納入 <br>ILI結果 <br>(2)</th>
											<th  rowspan="2">已納入CIPS結果 <br>(3)</th>
											<th  rowspan="2">已納入 <br>巡管結果 <br>1.是 <br>2.否 <br>(4)</th>
											<th width="85" colspan="3">各等級風險 <br>管段數量 </th>
											<th  rowspan="2">降低中高風險管段之相關作為文件名稱 </th>
											<th  rowspan="2">改善後 <br>風險等級 <br>高、中、低 </th>
                                            <th  rowspan="2" id="th_edit">功能</th>
										</tr>
										<tr>
											<th >高 </th>
											<th >中 </th>
											<th >低 </th>
										</tr>
									</thead>
									<tbody></tbody>
                                </table>
                            </div><!-- stripeMe --><br>
                            <div class="margin5TB font-size2">
                                填表說明：<br />
                                (1) 再評估時機：最近一次所執行之評估是公司定期規劃(例：每5年一次)，或因風險評估之因子有所異動 (例：遷管、換管)而執行。<br>
                                (2) 執行該管線風險評估時，已將ILI檢測結果納入評估參數，請填寫檢測時間，若尚未考量ILI檢測結果，或該管線尚未執行ILI檢測者，請填NA。<br>
                                (3) 執行該管線風險評估時，已將CIPS檢測結果納入評估參數，請填檢測時間，若尚未考量CIPS檢測結果者，請填NA。<br>
                                (4) 執行該管線風險評估時，已將巡管結果(如：未會勘而開挖頻度)納入評估參數，請填「1」，若尚未考量巡管結果者，請填「2」。<br>
                                (5) 各等級風險管段數量：請分別填入高、中、低風險之管段數量。<br>
                                (6) 若評估結果有中高風險管段，應於「降低中高風險管段之作為」欄位註明相對應之作為或其作為相關文件名稱，並於「改善後風險等級」欄位中，填入改善後之風險等級(高、中、低)。

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
