<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StatisticsPipeAndTank.aspx.cs" Inherits="WebPage_StatisticsPipeAndTank" %>

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
    <title>石油與天然氣事業輸儲設備查核及檢測資訊系統</title>
	<!--#include file="Head_Include.html"-->
    <style>
        td:first-child, th:first-child {
         position:sticky;
         left:0; /* 首行永遠固定於左 */
         z-index:1;
        }
        
        thead tr th {
         position:sticky;
         top:0; /* 列首永遠固定於上 */
        }
        
        th:first-child{
         z-index:2;
        }
    </style>
	<script type="text/javascript">
        $(document).ready(function () {
            $("#div_table").hide();
            getDDL('002', 'txt1');

            //查詢按鈕
            $(document).on("click", "#querybtn", function () {
                getData();
                $("#div_table").show();
            });

            //變更類別
            $(document).on("change", "#txt1", function () {
                if ($("#txt1 option:selected").val() != '') {
                    getCompanyDDL('01', $("#txt1 option:selected").val(), '', '', '', 'txt2');
                }
                else {
                    $("#txt2").empty();
                    $("#txt2").append('<option value="">請選擇</option>');
                }                
                $("#txt3").empty();
                $("#txt3").append('<option value="">請選擇</option>');
                $("#txt4").empty();
                $("#txt4").append('<option value="">請選擇</option>');
                $("#txt5").empty();
                $("#txt5").append('<option value="">請選擇</option>');
            });

            //變更公司名稱
            $(document).on("change", "#txt2", function () {
                if ($("#txt2 option:selected").val() != '') {
                    getCompanyDDL('02', $("#txt1 option:selected").val(), $("#txt2 option:selected").val(), '', '', 'txt3');
                }
                else {
                    $("#txt3").empty();
                    $("#txt3").append('<option value="">請選擇</option>');
                }
                $("#txt4").empty();
                $("#txt4").append('<option value="">請選擇</option>');
                $("#txt5").empty();
                $("#txt5").append('<option value="">請選擇</option>');
            });

            //變更事業部
            $(document).on("change", "#txt3", function () {
                if ($("#txt3 option:selected").val() != '') {
                    getCompanyDDL('03', $("#txt1 option:selected").val(), $("#txt2 option:selected").val(), $("#txt3 option:selected").val(), '', 'txt4');
                }
                else {
                    $("#txt4").empty();
                    $("#txt4").append('<option value="">請選擇</option>');
                }
                $("#txt5").empty();
                $("#txt5").append('<option value="">請選擇</option>');
            });

            //變更營業處廠
            $(document).on("change", "#txt4", function () {
                if ($("#txt4 option:selected").val() != '') {
                    getCompanyDDL('04', $("#txt1 option:selected").val(), $("#txt2 option:selected").val(), $("#txt3 option:selected").val(), $("#txt4 option:selected").val(), 'txt5');
                }
                else {
                    $("#txt5").empty();
                    $("#txt5").append('<option value="">請選擇</option>');
                }
            });

            //管線數量開窗
            $(document).on("click", "a[name='pipetotal']", function () {
                $("#CpGuid").val($(this).attr("aid"));
                if ($(this).attr("atype") == '01') {
                    doOpenPopupOilPipe();
                    $("#sp_oilpipeCpname").html($("span[name='sp1_" + $("#CpGuid").val() + "']").text() + $("span[name='sp2_" + $("#CpGuid").val() + "']").text() + 
                        $("span[name='sp3_" + $("#CpGuid").val() + "']").text() + $("span[name='sp4_" + $("#CpGuid").val() + "']").text());
                    getOilPipeData(0);
                }
                else {
                    doOpenPopupGasPipe();
                    $("#sp_gaspipeCpname").html($("span[name='sp1_" + $("#CpGuid").val() + "']").text() + $("span[name='sp2_" + $("#CpGuid").val() + "']").text() +
                        $("span[name='sp3_" + $("#CpGuid").val() + "']").text() + $("span[name='sp4_" + $("#CpGuid").val() + "']").text());
                    getGasPipeData(0);
                }
            });

            //儲槽數量開窗
            $(document).on("click", "a[name='tanktotal']", function () {
                $("#CpGuid").val($(this).attr("aid"));
                if ($(this).attr("atype") == '01') {
                    doOpenPopupOilTank();
                    $("#sp_oiltankCpname").html($("span[name='sp1_" + $("#CpGuid").val() + "']").text() + $("span[name='sp2_" + $("#CpGuid").val() + "']").text() +
                        $("span[name='sp3_" + $("#CpGuid").val() + "']").text() + $("span[name='sp4_" + $("#CpGuid").val() + "']").text());
                    getOilTankData(0);
                }
                else {
                    doOpenPopupGasTank();
                    $("#sp_gastankCpname").html($("span[name='sp1_" + $("#CpGuid").val() + "']").text() + $("span[name='sp2_" + $("#CpGuid").val() + "']").text() +
                        $("span[name='sp3_" + $("#CpGuid").val() + "']").text() + $("span[name='sp4_" + $("#CpGuid").val() + "']").text());
                    getGasTankData(0);
                }
            });
        });

        //查詢資料
        function getData() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetStatisticsPipeAndTank.aspx",
                data: {
                    type: "list",
                    stype: $("#txt1").val(),
                    cpname: $("#txt2").val(),
                    businessOrg: $("#txt3").val(),
                    factory: $("#txt4").val(),
                    workshop: $("#txt5").val(),
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
                                tabstr += '<td nowrap="nowrap"><span name="sp1_' + $(this).children("guid").text().trim() + '">' + $(this).children("公司名稱").text().trim() + '</span></td>';
                                tabstr += '<td nowrap="nowrap"><span name="sp2_' + $(this).children("guid").text().trim() + '">' + $(this).children("事業部").text().trim() + '</span></td>';
                                tabstr += '<td nowrap="nowrap"><span name="sp3_' + $(this).children("guid").text().trim() + '">' + $(this).children("營業處廠").text().trim() + '</span></td>';
                                tabstr += '<td nowrap="nowrap"><span name="sp4_' + $(this).children("guid").text().trim() + '">' + $(this).children("中心庫區儲運課工場").text().trim() + '</span></td>';
                                tabstr += '<td align="center" nowrap="nowrap"><a name="pipetotal" aid="' + $(this).children("guid").text().trim() +
                                    '" atype="' + $(this).children("類別").text().trim() + '" href="javascript:void(0);">' + $(this).children("pipeCount").text().trim() + '</a></td>';
                                tabstr += '<td align="center" nowrap="nowrap"><a name="tanktotal" aid="' + $(this).children("guid").text().trim() +
                                    '" atype="' + $(this).children("類別").text().trim() + '" href="javascript:void(0);">' + $(this).children("tankCount").text().trim() + '</a></td>';
                                tabstr += '</tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="6">查詢無資料</td></tr>';
                        $("#tablist tbody").append(tabstr);

                        //查詢紅字統計數量
                        if ($(data).find("data_item2").length > 0) {
                            $(data).find("data_item2").each(function (i) {
                                $("#sp_totalText").html('儲槽共' + $(this).children("儲槽數量總合").text().trim() + '座，儲槽容量' +
                                    $(this).children("儲槽容量總合").text().trim() + '公秉，管線共' + $(this).children("管線數量總合").text().trim() +
                                    '條，管線長度共' + $(this).children("管線長度總合").text().trim() + '公里');
                            });
                        }
                        else {
                            $("#sp_totalText").html('儲槽共0座，儲槽容量0公秉，管線共0條，管線長度共0公里');
                        }
                    }
                }
            });
        }

        //石油天然氣下拉選單查詢列表
        function getCompanyDDL(dlltype, stype, cpname, businessOrg, factory, id) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/GetStatisticsPipeAndTank.aspx",
                data: {
                    dlltype: dlltype,
                    type: 'dll',
                    stype: stype,
                    cpname: cpname,
                    businessOrg: businessOrg,
                    factory: factory,
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
                        var ddlstr = '<option value="">請選擇</option>';
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                ddlstr += '<option value="' + $(this).children("showName").text().trim() + '">' + $(this).children("showName").text().trim() + '</option>';
                            });
                        }
                        $("#" + id).empty();
                        $("#" + id).append(ddlstr);
                    }
                }
            });
        }

        //取得石油管線資料
        function getOilPipeData(p) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetOilTubeInfo.aspx",
                data: {
                    cpid: $("#CpGuid").val(),
                    type: "list",
                    PageNo: p,
                    PageSize: Page.Option.PageSize,
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
                        $("#tablistOilPipe tbody").empty();
                        var tabstr = '';
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                tabstr += '<tr>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("長途管線識別碼").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("轄區長途管線名稱").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("銜接管線識別碼_上游").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("銜接管線識別碼_下游").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("起點").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("迄點").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("管徑吋").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("厚度").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("管材").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("包覆材料").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("轄管長度").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("內容物").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("緊急遮斷閥").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("建置年").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("設計壓力").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("使用壓力").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("使用狀態").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("附掛橋樑數量").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("管線穿越箱涵數量").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("活動斷層敏感區").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("土壤液化區").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("土石流潛勢區").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("淹水潛勢區").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap"><pre>' + $(this).children("備註").text().trim() + '</pre></td>';
                                tabstr += '</tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="24">查詢無資料</td></tr>';
                        $("#tablistOilPipe tbody").append(tabstr);
                        Page.Option.Selector = "#pageblockOilPipe";
                        Page.Option.FunctionName = "getOilPipeData";
                        Page.CreatePage(p, $("total", data).text());
                    }
                }
            });
        }

        //取得石油儲槽資料
        function getOilTankData(p) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetOilStorageTankInfo.aspx",
                data: {
                    cpid: $("#CpGuid").val(),
                    type: "list",
                    PageNo: p,
                    PageSize: Page.Option.PageSize,
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
                        $("#tablistOilTank tbody").empty();
                        var tabstr = '';
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                tabstr += '<tr>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("轄區儲槽編號").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("能源局編號").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("容量").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("內徑").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("內容物").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("油品種類").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("形式").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("啟用日期").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("代行檢查_代檢機構1").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + getDate($(this).children("代行檢查_外部日期1").text().trim()) + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("代行檢查_代檢機構2").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + getDate($(this).children("代行檢查_外部日期2").text().trim()) + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("狀態").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("延長開放年限").text().trim() + '</td>';
                                tabstr += '</tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="14">查詢無資料</td></tr>';
                        $("#tablistOilTank tbody").append(tabstr);
                        Page.Option.Selector = "#pageblockOilTank";
                        Page.Option.FunctionName = "getOilTankData";
                        Page.CreatePage(p, $("total", data).text());
                    }
                }
            });
        }

        //取得天然氣管線資料
        function getGasPipeData(p) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetGasTubeInfo.aspx",
                data: {
                    cpid: $("#CpGuid").val(),
                    type: "list",
                    PageNo: p,
                    PageSize: Page.Option.PageSize,
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
                        $("#tablistGaspipe tbody").empty();
                        var tabstr = '';
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                tabstr += '<tr>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("長途管線識別碼").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("轄區長途管線名稱_公司").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("銜接管線識別碼_上游").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("銜接管線識別碼_下游").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("起點").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("迄點").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("管徑").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("厚度").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("管材").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("包覆材料").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("轄管長度").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("內容物").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("緊急遮斷閥").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("建置年").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("設計壓力").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("使用壓力").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("使用狀態").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("附掛橋樑數量").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("管線穿越箱涵數量").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("活動斷層敏感區").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("土壤液化區").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("土石流潛勢區").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("淹水潛勢區").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap"><pre>' + $(this).children("備註").text().trim() + '</pre></td>';
                                tabstr += '</tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="24">查詢無資料</td></tr>';
                        $("#tablistGaspipe tbody").append(tabstr);
                        Page.Option.Selector = "#pageblockGasPipe";
                        Page.Option.FunctionName = "getGasPipeData";
                        Page.CreatePage(p, $("total", data).text());
                    }
                }
            });
        }

        //取得天然氣儲槽資料
        function getGasTankData() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetGasStorageTankInfo.aspx",
                data: {
                    cpid: $("#CpGuid").val(),
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
                        $("#tablistGastank tbody").empty();
                        var tabstr = '';
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                tabstr += '<tr>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("液化天然氣廠").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("儲槽編號").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("容量").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("外徑").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("高度").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("形式").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + getDate($(this).children("啟用日期").text().trim()) + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("狀態").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("勞動部檢查").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("代行檢查機構").text().trim() + '</td>';
                                tabstr += '</tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="10">查詢無資料</td></tr>';
                        $("#tablistGastank tbody").append(tabstr);
                    }
                }
            });
        }

        function getDDL(gNo, id) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/GetDDLlist.aspx",
                data: {
                    gNo: gNo,
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
                        var ddlstr = '<option value="">請選擇</option>';
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                ddlstr += '<option value="' + $(this).children("項目代碼").text().trim() + '">' + $(this).children("項目名稱").text().trim() + '</option>';
                            });
                        }
                        $("#" + id).empty();
                        $("#" + id).append(ddlstr);
                    }
                }
            });
        }

        //石油管線數量 開窗
        function doOpenPopupOilPipe() {
            $.magnificPopup.open({
                items: {
                    src: '#OilPipePopUp'
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

        //石油儲槽數量 開窗
        function doOpenPopupOilTank() {
            $.magnificPopup.open({
                items: {
                    src: '#OilTankPopUp'
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

        //天然氣管線數量 開窗
        function doOpenPopupGasPipe() {
            $.magnificPopup.open({
                items: {
                    src: '#GasPipePopUp'
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

        //天然氣儲槽數量 開窗
        function doOpenPopupGasTank() {
            $.magnificPopup.open({
                items: {
                    src: '#GasTankPopUp'
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
    </script>
</head>
<body class="bgC">
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
		<!--#include file="CommonHeader.html"-->
        <input type="hidden" id="Competence" value="<%= competence %>" />
        <input type="hidden" id="CGguid" />
        <input type="hidden" id="CpGuid" />
        <div id="ContentWrapper">
            <div class="container margin15T">
                <div class="padding10ALL">
                    <%--<div class="filetitlewrapper"><!--#include file="GasBreadTitle.html"--></div>--%>

                    <div class="row margin20T">
                        <div class="col-lg-3 col-md-4 col-sm-5">
                            <div id="navmenuV"><!--#include file="StatisticsLeftMenu.html"--></div>
                        </div>
                        <div class="col-lg-9 col-md-8 col-sm-7">
                            <div class="twocol">
                                <div class="left font-size5 ">查詢:</div>
                                <div class="right">

                                </div>
                            </div>
                            <div class="OchiTrasTable width100 TitleLength09 font-size3">
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">類別</div>
                                        <div class="OchiCell width100">
                                            <select id="txt1" class="width100 inputex" >
                                            </select>
                                        </div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">公司名稱</div>
                                        <div class="OchiCell width100">
                                            <select id="txt2" class="width100 inputex" >
                                                <option value="">請選擇</option>
                                            </select>
                                        </div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">事業部</div>
                                        <div class="OchiCell width100">
                                            <select id="txt3" class="width100 inputex" >
                                                <option value="">請選擇</option>
                                            </select>
                                        </div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">營業處廠</div>
                                        <div class="OchiCell width100">
                                            <select id="txt4" class="width100 inputex" >
                                                <option value="">請選擇</option>
                                            </select>
                                        </div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">中心庫區儲運課工場</div>
                                        <div class="OchiCell width100">
                                            <select id="txt5" class="width100 inputex" >
                                                <option value="">請選擇</option>
                                            </select>
                                        </div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                            </div><!-- OchiTrasTable -->
                            <br />
                            <div class="twocol">
                                <div class="left">
                                    <span id="sp_totalText" style="color:red" class="font-size3"></span>
                                </div>
                                <div class="right">
                                    <a id="querybtn" href="javascript:void(0);" title="查詢" class="genbtn" >查詢</a>
                                </div>
                            </div>
                            <br />
                            <div id="div_table" class="stripeMeB tbover">
                                <table id="tablist" border="0" cellspacing="0" cellpadding="0" width="100%">
                                    <thead>
							        	<tr>
							        		<th nowrap="nowrap">公司名稱</th>
							        		<th nowrap="nowrap">事業部</th>
							        		<th nowrap="nowrap">營業處廠</th>
                                            <th nowrap="nowrap">中心庫區儲運課工場</th>
							        		<th nowrap="nowrap">管線數量</th>
							        		<th nowrap="nowrap">儲槽數量</th>
							        	</tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div>
                            <div class="margin10B margin10T textcenter">
	                            <div id="pageblock"></div>
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

<!-- Magnific Popup -->
<div id="OilPipePopUp" class="magpopup magSizeL mfp-hide">
  <div class="magpopupTitle"><span id="sp_oilpipeCpname"></span> 管線列表</div>
  <div class="padding10ALL">
      <div class="stripeMeB tbover">
          <table id="tablistOilPipe" border="0" cellspacing="0" cellpadding="0" width="100%">
              <thead>
		        	<tr>
		        		<th nowrap>長途管線識別碼 </th>
                        <th nowrap>轄區長途管線名稱<br>(公司)</th>
                        <th nowrap>銜接管線識別碼<br>(上游)</th>
                        <th nowrap>銜接管線識別碼<br>(下游)</th>
                        <th nowrap>起點 </th>
                        <th nowrap>迄點 </th>
                        <th nowrap>管徑<br>吋</th>
                        <th nowrap>厚度<br>(mm)</th>
                        <th nowrap>管材<br>(詳細規格)</th>
                        <th nowrap>包覆<br>材料 </th>
                        <th nowrap>轄管長度<br>(公里)</th>
                        <th nowrap>內容物 </th>
                        <th nowrap>緊急遮斷閥<br>(處)</th>
                        <th nowrap>建置年</th>
                        <th nowrap>設計壓力<br>(Kg/cm<sup>2</sup>)</th>
                        <th nowrap>使用壓力<br>(Kg/cm<sup>2</sup>)</th>
                        <th nowrap>使用狀態<br>
                            1.使用中<br>
                            2.停用<br>
                            3.備用 </th>
                        <th nowrap>附掛<br>橋樑<br>數量</th>
                        <th nowrap>管線穿越<br>箱涵數量</th>
                        <th nowrap>活動<br>斷層<br>敏感區</th>
                        <th nowrap>土壤<br>液化區</th>
                        <th nowrap>土石流<br>潛勢區</th>
                        <th nowrap>淹水<br>潛勢區</th>
                        <th nowrap>備註 </th>
		        	</tr>
              </thead>
              <tbody></tbody>
          </table>
      </div>
      <div class="margin10B margin10T textcenter">
	      <div id="pageblockOilPipe"></div>
	  </div>

  </div><!-- padding10ALL -->

</div><!--magpopup -->

<div id="OilTankPopUp" class="magpopup magSizeL mfp-hide">
  <div class="magpopupTitle"><span id="sp_oiltankCpname"></span> 儲槽列表</div>
  <div class="padding10ALL">
      <div class="stripeMeB tbover">
          <table id="tablistOilTank" width="100%" border="0" cellspacing="0" cellpadding="0">
              <thead>
                  <tr>
                      <th nowrap  rowspan="2">轄區儲槽編號 </th>
                      <th nowrap rowspan="2">能源局編號 </th>
                      <th nowrap rowspan="2">容量 <br>
                          （公秉） </th>
                      <th nowrap rowspan="2">內徑 <br>
                          (公尺） </th>
                      <th nowrap rowspan="2">內容物 </th>
                      <th nowrap rowspan="2">油品種類 </th>
                      <th nowrap rowspan="2">形式 <br>
                          1.錐頂 <br>
                          2.內浮頂 <br>
                          3.外浮頂 <br>
                          4.掩體式 </th>
                      <th nowrap rowspan="2">啟用日期 <br>
                          年/月 </th>
                      <th nowrap colspan="4">代行檢查有效期限 </th>
                      <th nowrap rowspan="2" valign="top">狀態 <br>
                          1.使用中 <br>
                          2.開放中 <br>
                          3.停用 <br>
                          4.其他 </th>
                      <th nowrap rowspan="2">延長開 <br>
                          放年限 <br>
                          多?年 </th>
                  </tr>
                  <tr>
                      <th >代檢機構 <br>
                          (填表說明) </th>
                      <th >外部 <br>
                          年/月/日 </th>
                      <th >代檢機構 <br>
                          (填表說明) </th>
                      <th >內部 <br>
                          年/月/日 </th>
                  </tr>
              </thead>
              <tbody></tbody>
          </table>
      </div>
      <div class="margin10B margin10T textcenter">
	      <div id="pageblockOilTank"></div>
	  </div>

  </div><!-- padding10ALL -->

</div><!--magpopup -->

<div id="GasPipePopUp" class="magpopup magSizeL mfp-hide">
  <div class="magpopupTitle"><span id="sp_gaspipeCpname"></span> 管線列表</div>
  <div class="padding10ALL">
      <div class="stripeMeB tbover">
          <table id="tablistGaspipe" width="100%" border="0" cellspacing="0" cellpadding="0">
              <thead>
                  <tr>
					<th nowrap>長途管線識別碼 </th>
					<th nowrap>轄區長途管線名稱<br>(公司)</th>
					<th nowrap>銜接管線識別碼<br>(上游) </th>
					<th nowrap>銜接管線識別碼<br>(下游) </th>
					<th nowrap>起點 </th>
					<th nowrap>迄點 </th>
					<th nowrap>管徑<br>吋 </th>
					<th nowrap>厚度<br>(mm)</th>
					<th nowrap>管材<br>(詳細規格)</th>
					<th nowrap>包覆材料 </th>
					<th nowrap>轄管長度<br>(公里)</th>
					<th nowrap>內容物 </th>
					<th nowrap>緊急遮斷閥<br>(處)</th>
					<th nowrap>建置年<br>(民國年月)</th>
					<th nowrap>設計壓力<br>(Kg/cm<sup>2</sup>)</th>
					<th nowrap>使用<br>壓力<br>(Kg/cm<sup>2</sup>)</th>
					<th nowrap>使用狀態<br>1.使用中<br>2.停用<br>3.備用 </th>
					<th nowrap>附掛<br>橋樑<br>數量 </th>
                    <th nowrap>管線穿越<br>箱涵數量</th>
                    <th nowrap>活動斷層敏感區<br>1.有<br>2.無 </th>
                    <th nowrap>土壤液化區<br>1.有<br>2.無 </th>
                    <th nowrap>土石流潛勢區<br>1.有<br>2.無 </th>
                    <th nowrap>淹水潛勢區<br>1.有<br>2.無 </th>
                    <th nowrap>備註 </th>
				</tr>
              </thead>
              <tbody></tbody>
          </table>
      </div>
      <div class="margin10B margin10T textcenter">
	      <div id="pageblockGasPipe"></div>
	  </div>

  </div><!-- padding10ALL -->

</div><!--magpopup -->

<div id="GasTankPopUp" class="magpopup magSizeL mfp-hide">
  <div class="magpopupTitle"><span id="sp_gastankCpname"></span> 儲槽列表</div>
  <div class="padding10ALL">
      <div class="stripeMeB tbover">
          <table id="tablistGastank" width="100%" border="0" cellspacing="0" cellpadding="0">
              <thead>
                  <tr>
                      <th nowrap>液化天然氣廠 </th>
                      <th nowrap>儲槽編號 </th>
                      <th nowrap>容量 <br>（萬公秉） </th>
                      <th nowrap>外徑 <br>(公尺） </th>
                      <th nowrap>高度 <br>(公尺)</th>
                      <th nowrap>形式 </th>
                      <th nowrap>啟用日期 </th>
                      <th nowrap>狀態 <br>(使用中/ 開放中/ 停用)</th>
                      <th nowrap>勞動部檢查<br>合格證及有效期限 </th>
                      <th nowrap>代行/檢查機構 </th>
                  </tr>
              </thead>
              <tbody></tbody>
          </table>
      </div>
      <div class="margin10B margin10T textcenter">
	      <div id="pageblockGasTank"></div>
	  </div>

  </div><!-- padding10ALL -->

</div><!--magpopup -->

<!-- 本頁面使用的JS -->
	<script type="text/javascript">
        $(document).ready(function () {

        });
    </script>
	<script type="text/javascript" src="../js/GenCommon.js"></script><!-- UIcolor JS -->
	<script type="text/javascript" src="../js/PageCommon.js"></script><!-- 系統共用 JS -->
	<script type="text/javascript" src="../js/MenuGas.js"></script><!-- 系統共用 JS -->
	<script type="text/javascript" src="../js/SubMenuManage.js"></script><!-- 內頁選單 -->
	<script type="text/javascript" src="../js/autoHeight.js"></script><!-- 高度不足頁面的絕對置底footer -->
</body>
</html>


