﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GasTubeCheck.aspx.cs" Inherits="WebPage_GasTubeCheck" %>

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
            $(".pickDate").datepick({
                dateFormat: 'yymmdd',
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: '../images/calendar.gif',
                yearRange: 'c-60:c+10'
            }).BootStrap(); //BootStrap() 產生符合 BootStrap 的樣式內容

            $(".datepick-trigger").hide();
            $("#spLastYear2").html(getTaiwanDate() - 2);
            $("#spLastYear1").html(getTaiwanDate() - 1);

            getYearList();
            $("#sellist").val(getTaiwanDate());
            getData(getTaiwanDate());
            $("#exportbtn").attr("href", "../Gas_EXPORTEXCEL.aspx?cpid=" + $.getQueryString("cp") + "&year=" + getTaiwanDate() + "&category=tubecheck");

            //選擇年份
            $(document).on("change", "#sellist", function () {
                getData($("#sellist option:selected").val());
                $("#exportbtn").attr("href", "../Gas_EXPORTEXCEL.aspx?cpid=" + $.getQueryString("cp") + "&year=" + $("#sellist option:selected").val() + "&category=tubecheck");
            });

            //新增按鈕(依據文件資料)
            $(document).on("click", "#newbtnFile", function () {
                location.href = "edit_GasTubeCheck_File.aspx?cp=" + $.getQueryString("cp");
            });

            //新增按鈕(異常情形統計資料)
            $(document).on("click", "#newbtnUnusual", function () {
                location.href = "edit_GasTubeCheck_Unusual.aspx?cp=" + $.getQueryString("cp");
            });

            //編輯按鈕
            $(document).on("click", "#editbtn", function () {
                $("#mode").val("edit");
                $("#editbtn").hide();
                $("#cancelbtn").show();
                $("#subbtn").show();
                $("#sellist").attr("disabled", true);
                $(".datepick-trigger").show();
                setDisplayed(false);
            });

            //返回按鈕
            $(document).on("click", "#cancelbtn", function () {
                var str = confirm('尚未儲存的部分將不會更改，確定返回嗎?');

                if (str) {
                    location.href = "GasTubeCheck.aspx?cp=" + $.getQueryString("cp");
                }
            });

            //儲存按鍵
            $(document).on("click", "#subbtn", function () {
                var msg = '';

                if (!$("input[name='checkCount']").is(":checked"))
                    msg += "請選擇【每日巡檢次數】\n";
                if ($("#checkPerson").val() == '')
                    msg += "請輸入【巡管人數】\n";
                if ($("#checkPersonOutSider").val() == '')
                    msg += "請輸入【巡管外包人數】\n";
                if (!$("input[name='checkTool']").is(":checked"))
                    msg += "請輸入【巡管工具】\n";
                if (!$("input[name='managerCheck']").is(":checked"))
                    msg += "請輸入【主管監督查核】\n";
                if (!$("input[name='checkStrengthen']").is(":checked"))
                    msg += "請輸入【是否有加強巡檢點】\n";

                if (msg != "") {
                    alert("Error message: \n" + msg);
                    return false;
                }

                // Get form
                var form = $('#form1')[0];

                // Create an FormData object 
                var data = new FormData(form);

                // If you want to add an extra field for the FormData
                data.append("cp", $.getQueryString("cp"));
                data.append("year", encodeURIComponent(getTaiwanDate()));
                data.append("checkPerson", encodeURIComponent($("#checkPerson").val()));
                data.append("checkPersonOutSider", encodeURIComponent($("#checkPersonOutSider").val()));
                data.append("checkToolOther", encodeURIComponent($("#checkToolOther").val()));
                data.append("ManagerCount", encodeURIComponent($("#ManagerCount").val()));
                data.append("StrengthenTxt", encodeURIComponent($("#StrengthenTxt").val()));

                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "../handler/AddGasTubeCheck.aspx",
                    data: data,
                    processData: false,
                    contentType: false,
                    cache: false,
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

                            location.href = "GasTubeCheck.aspx?cp=" + $.getQueryString("cp");
                        }
                    }
                });
            });

            //刪除按鈕(依據文件資料)
            $(document).on("click", "a[name='delbtnFile']", function () {
                if (confirm("確定刪除?")) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../handler/DelGasTubeCheckFile.aspx",
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

            //刪除按鈕(異常情形統計資料)
            $(document).on("click", "a[name='delbtnUnusual']", function () {
                if (confirm("確定刪除?")) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../handler/DelGasTubeCheckUnusual.aspx",
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

            //巡管工具 其他勾選時事件
            $(document).on("change", "input[name='checkTool'][value='03']", function () {
                if (this.checked) {
                    $("#checkToolOther").val($("#checkToolch").val());
                    $("#checkToolOther").attr("disabled", false);
                }
                else {
                    $("#checkToolch").val($("#checkToolOther").val());
                    $("#checkToolOther").val('');
                    $("#checkToolOther").attr("disabled", true);
                }
            });

            //主管監督查核 有勾選時事件
            $(document).on("change", "input[name='managerCheck']", function () {
                if (this.value == 'Y') {
                    $("#ManagerCount").val($("#managerCheckrd").val());
                    $("#ManagerCount").attr("disabled", false);
                }
                else if (this.value == 'N') {
                    $("#managerCheckrd").val($("#ManagerCount").val());
                    $("#ManagerCount").val('');
                    $("#ManagerCount").attr("disabled", true);
                }
            });

            //是否有加強巡檢點 有勾選時事件
            $(document).on("change", "input[name='checkStrengthen']", function () {
                if (this.value == 'Y') {
                    $("#StrengthenTxt").val($("#checkStrengthenrd").val());
                    $("#StrengthenTxt").attr("disabled", false);
                }
                else if (this.value == 'N') {
                    $("#checkStrengthenrd").val($("#StrengthenTxt").val());
                    $("#StrengthenTxt").val('');
                    $("#StrengthenTxt").attr("disabled", true);
                }
            });
        }); // end js

        function setDisplayed(status) {
            $("input[name='checkCount']").attr("disabled", status);
            $("input[name='checkTool']").attr("disabled", status);
            $("#checkPerson").attr("disabled", status);
            $("#checkPersonOutSider").attr("disabled", status);
            if ($("input[name='checkTool'][value='03']").is(":checked"))
                $("#checkToolOther").attr("disabled", status);
            else
                $("#checkToolOther").attr("disabled", !status);
            $("input[name='managerCheck']").attr("disabled", status);
            if ($("input[name='managerCheck'][value='Y']").is(":checked"))
                $("#ManagerCount").attr("disabled", status);
            else
                $("#ManagerCount").attr("disabled", !status);
            $("input[name='checkStrengthen']").attr("disabled", status);
            if ($("input[name='checkStrengthen'][value='Y']").is(":checked"))
                $("#StrengthenTxt").attr("disabled", status);
            else
                $("#StrengthenTxt").attr("disabled", !status);

        }

		function getData(year) {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetGasTubeCheck.aspx",
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
						if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
								$("input[name='checkCount'][value='" + $(this).children("每日巡檢次數").text().trim() + "']").prop("checked", true);
								$("#checkPerson").val($(this).children("巡管人數").text().trim());
                                $("#checkPersonOutSider").val($(this).children("巡管外包人數").text().trim());
								// 巡管工具
								var othertool = false;
                                var arychecktool = $(this).children("巡管工具").text().trim().split(',');
                                $("input[name='checkTool']").prop("checked", false);
								$.each(arychecktool, function (key, value) {
									$("input[name='checkTool'][value='" + value + "']").prop("checked", true);
								});
                                $("#checkToolOther").val($(this).children("巡管工具其他").text().trim());

								// 主管監督查核
								$("input[name='managerCheck'][value='" + $(this).children("主管監督查核").text().trim() + "']").prop("checked", true);
                                $("#ManagerCount").val($(this).children("主管監督查核次").text().trim());

								// 是否有加強巡檢點
								$("input[name='checkStrengthen'][value='" + $(this).children("是否有加強巡檢點").text().trim() + "']").prop("checked", true);
                                $("#StrengthenTxt").val($(this).children("是否有加強巡檢點敘述").text().trim());
							});
                        }
                        else
                        {
                            $("input[name='checkCount']").prop("checked", false);
                            $("#checkPerson").val('');
                            $("#checkPersonOutSider").val('');
                            $("input[name='checkTool']").prop("checked", false);
                            $("#checkToolOther").val('');
                            $("input[name='managerCheck']").prop("checked", false);
                            $("#ManagerCount").val('');
                            $("input[name='checkStrengthen']").prop("checked", false);
                            $("#StrengthenTxt").val('');
                        }

                        $("#tablist tbody").empty();
                        var tabstr = '';
                        if ($(data).find("data_item2").length > 0) {
                            $(data).find("data_item2").each(function (i) {
                                tabstr += '<tr>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("依據文件名稱").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("文件編號").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + getDate($(this).children("文件日期").text().trim()) + '</td>';
                                tabstr += '<td name="td_editFile" nowrap="" align="center"><a href="javascript:void(0);" name="delbtnFile" aid="' + $(this).children("guid").text().trim() + '">刪除</a>';
                                tabstr += ' <a href="edit_GasTubeCheck_File.aspx?cp=' + $.getQueryString("cp") + '&guid=' + $(this).children("guid").text().trim() + '" name="editbtnFile">編輯</a></td>';
                                tabstr += '</tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="4">查詢無資料</td></tr>';
                        $("#tablist tbody").append(tabstr);

						$("#tablist2 tbody").empty();
						var tabstr = '';
						if ($(data).find("data_item3").length > 0) {
							$(data).find("data_item3").each(function (i) {
								tabstr += '<tr>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("管線巡檢情形").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("前兩年").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("前一年").text().trim() + '</td>';
                                tabstr += '<td name="td_editUnusual" nowrap="" align="center"><a href="javascript:void(0);" name="delbtnUnusual" aid="' + $(this).children("guid").text().trim() + '">刪除</a>';
                                tabstr += ' <a href="edit_GasTubeCheck_Unusual.aspx?cp=' + $.getQueryString("cp") + '&guid=' + $(this).children("guid").text().trim() + '" name="editbtnUnusual">編輯</a></td>';
                                tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="4">查詢無資料</td></tr>';
                        $("#tablist2 tbody").append(tabstr);

                        //確認權限&按鈕顯示或隱藏
                        if ($("#sellist").val() != getTaiwanDate()) {
                            $("#editbtn").hide();
                            $("#newbtnFile").hide();
                            $("#newbtnUnusual").hide();
                            $("#th_editFile").hide();
                            $("#th_editUnusual").hide();
                            $("td[name='td_editFile']").hide();
                            $("td[name='td_editUnusual']").hide();
                        }
                        else {
                            if (($("#Competence").val() == '01') || ($("#Competence").val() == '04') || ($("#Competence").val() == '05') || ($("#Competence").val() == '06')) {
                                $("#editbtn").hide();
                                $("#newbtnFile").hide();
                                $("#newbtnUnusual").hide();
                                $("#th_editFile").hide();
                                $("#th_editUnusual").hide();
                                $("td[name='td_editFile']").hide();
                                $("td[name='td_editUnusual']").hide();
                            }
                            else {
                                $("#editbtn").show();
                                $("#newbtnFile").show();
                                $("#newbtnUnusual").show();
                                $("#th_editFile").show();
                                $("#th_editUnusual").show();
                                $("td[name='td_editFile']").show();
                                $("td[name='td_editUnusual']").show();
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
                                        $("#editbtn").hide();
                                        $("#newbtnFile").hide();
                                        $("#newbtnUnusual").hide();
                                        $("#th_editFile").hide();
                                        $("#th_editUnusual").hide();
                                        $("td[name='td_editFile']").hide();
                                        $("td[name='td_editUnusual']").hide();
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
                url: "../Handler/GetGasTubeCheck.aspx",
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
                        if ($(data).find("data_item4").length > 0) {
                            $(data).find("data_item4").each(function (i) {
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
		<!--#include file="GasHeader.html"-->
        <input type="hidden" id="Competence" value="<%= competence %>" />
        <input type="hidden" id="checkToolch" />
        <input type="hidden" id="managerCheckrd" />
        <input type="hidden" id="checkStrengthenrd" />
        <input type="hidden" id="mode" />
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
                                    <a id="editbtn" href="javascript:void(0);" title="編輯" class="genbtn">編輯</a>
                                    <a id="cancelbtn" href="javascript:void(0);" title="返回" class="genbtn" style="display:none">返回</a>
                                    <a id="subbtn" href="javascript:void(0);" title="儲存" class="genbtn" style="display:none">儲存</a>
                                </div>
                            </div><br />
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
                                            員工<input type="number" min="0" id="checkPerson" class="inputex width20" disabled> 人;
                                            外包<input type="number" min="0" id="checkPersonOutSider" class="inputex width20" disabled> 人
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
                                        <div class="OchiCell width100"><input type="radio" name="managerCheck" value="Y" disabled> 有  至少 <input type="number" min="0" id="ManagerCount" class="inputex width40" disabled > 次/月(季)  <input type="radio" name="managerCheck" value="N" disabled> 無</div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->

                                <div class="OchiRow">
                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">是否有加強巡檢點？</div>
                                    <div class="OchiCell width100">
                                        <input type="radio" name="checkStrengthen" value="Y" disabled> 有 <input type="text" id="StrengthenTxt" class="inputex width40" disabled>  <input type="radio" name="checkStrengthen" value="N" disabled> 無
                                    </div>
                                </div><!-- OchiRow -->

                            </div><!-- OchiTrasTable -->

                            <br />
                            <div class="twocol">
                                <div class="left font-size4 margin10T font-bold">依據文件資料:</div>
                                <div class="right">
                                    <a id="newbtnFile" href="javascript:void(0);" title="新增" class="genbtn">新增</a>
                                </div>
                            </div><br />
                            <div class="stripeMeG tbover margin5T">
                                <table id="tablist" width="100%" border="0" cellspacing="0" cellpadding="0">
									<thead>
										<tr>
											<th >依據文件名稱 </th>
											<th >文件編號 </th>
											<th >文件日期 </th>
                                            <th id="th_editFile" width="10%" >功能 </th>
										</tr>
									</thead>
									<tbody></tbody>
                                </table>
                            </div><!-- stripeMe -->

                            <br />
                            <div class="twocol">
                                <div class="left font-size4 margin10T font-bold">異常情形統計資料:</div>
                                <div class="right">
                                    <a id="newbtnUnusual" href="javascript:void(0);" title="新增" class="genbtn">新增</a>
                                </div>
                            </div><br />
                            <div class="stripeMeG tbover margin5T">
                                <table id="tablist2" width="100%" border="0" cellspacing="0" cellpadding="0">
									<thead>
										<tr>
											<th >管線巡檢情形 </th>
											<th >前二年 </th>
											<th >前一年 </th>
                                            <th id="th_editUnusual" width="10%" >功能 </th>
										</tr>
									</thead>
									<tbody></tbody>
                                </table>
                            </div><!-- stripeMe --><br>

                            <div class="margin5TB font-size2">
                                填表說明：<br>
                                (1) 前二年：<span id="spLastYear2"></span>年；前一年：<span id="spLastYear1"></span>年
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
