<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StatisticsStorageTankInfo.aspx.cs" Inherits="WebPage_StatisticsStorageTankInfo" %>

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
    <title>石油與天然氣事業輸儲設備統計查詢資訊系統</title>
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
            $("#div_content").hide();
            getDDL('002', 'txt1');
            getDDL('030', 'txt9');

            //查詢按鈕
            $(document).on("click", "#querybtn", function () {
                var msg = '';
                if ($("#txt1 option:selected").val() == '')
                    msg += '請選擇【類別】\n';
                if (($("#txt9").val() != '') && ($("#txt10").val() != ''))
                    if ($("#txt9").val() > $("#txt10").val())
                        msg += '【啟用日期(起)】不能大於【啟用日期(迄)】';
                if (msg != '') {
                    alert("Error message: \n" + msg);
                    return false;
                }

                getData(0);
                $("#div_table").show();
            });

            //變更類別
            $(document).on("change", "#txt1", function () {
                var ddlstr = '';

                if ($("#txt1 option:selected").val() != '') {
                    if ($("#txt1 option:selected").val() == '01') {
                        ddlstr += '<div class="OchiHalf"><div class="OchiCell OchiTitle IconCe TitleSetWidth">能源署編號</div>' +
                            '<div class="OchiCell width100"><input id="txt7" type="text" class="width100 inputex" /></div></div >' +
                            '<div class="OchiHalf"><div class="OchiCell OchiTitle IconCe TitleSetWidth">油品種類</div><div class="OchiCell width100">' +
                            '<select id="txt8" class="width100 inputex" >';
                        $.ajax({
                            type: "POST",
                            async: false, //在沒有返回值之前,不會執行下一步動作
                            url: "../handler/GetDDLlist.aspx",
                            data: {
                                gNo: '023',
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
                                    ddlstr += '<option value="">請選擇</option>';
                                    if ($(data).find("data_item").length > 0) {
                                        $(data).find("data_item").each(function (i) {
                                            ddlstr += '<option value="' + $(this).children("項目名稱").text().trim() + '">' + $(this).children("項目名稱").text().trim() + '</option>';
                                        });
                                    }
                                }
                            }
                        });
                        ddlstr += '</select></div></div>';
                        $("#div_content").empty();
                        $("#div_content").append(ddlstr);
                        $("#div_content").show();
                    }
                    else {
                        ddlstr += '<div class="OchiHalf"><div class="OchiCell OchiTitle IconCe TitleSetWidth">液化天然氣廠</div>' +
                            '<div class="OchiCell width100"><input id="txt7" type="text" class="width100 inputex" /></div></div >' +
                            '<div class="OchiHalf"><div class="OchiCell OchiTitle IconCe TitleSetWidth">狀態</div><div class="OchiCell width100">' +
                            '<select id="txt8" class="width100 inputex" >';
                        $.ajax({
                            type: "POST",
                            async: false, //在沒有返回值之前,不會執行下一步動作
                            url: "../handler/GetDDLlist.aspx",
                            data: {
                                gNo: '022',
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
                                    ddlstr += '<option value="">請選擇</option>';
                                    if ($(data).find("data_item").length > 0) {
                                        $(data).find("data_item").each(function (i) {
                                            ddlstr += '<option value="' + $(this).children("項目名稱").text().trim() + '">' + $(this).children("項目名稱").text().trim() + '</option>';
                                        });
                                    }
                                }
                            }
                        });
                        ddlstr += '</select></div></div>';
                        $("#div_content").empty();
                        $("#div_content").append(ddlstr);
                        $("#div_content").show();
                    }                        
                    getCompanyDDL('01', $("#txt1 option:selected").val(), '', '', '', 'txt2');
                }
                else {
                    $("#div_content").empty();
                    $("#div_oil").hide();
                    $("#txt2").empty();
                    $("#txt2").append('<option value="">請選擇</option>');
                }                
                $("#txt3").empty();
                $("#txt3").append('<option value="">請選擇</option>');
                $("#txt4").empty();
                $("#txt4").append('<option value="">請選擇</option>');
                $("#txt5").empty();
                $("#txt5").append('<option value="">請選擇</option>');
                $("#txt6").empty();
                $("#txt6").append('<option value="">請選擇</option>');
            });

            //變更公司名稱
            $(document).on("change", "#txt2", function () {
                if ($("#txt2 option:selected").val() != '') {
                    getPipeSn();
                    getCompanyDDL('02', $("#txt1 option:selected").val(), $("#txt2 option:selected").val(), '', '', 'txt3');
                }
                else {
                    $("#txt3").empty();
                    $("#txt3").append('<option value="">請選擇</option>');
                    $("#txt6").empty();
                    $("#txt6").append('<option value="">請選擇</option>');
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
                getPipeSn();
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
                getPipeSn();
            });

            //變更中心庫區儲運課工廠
            $(document).on("change", "#txt5", function () {
                getPipeSn();
            });

            $(".pickDate").datepick({
                dateFormat: 'yymmdd',
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: '../images/calendar.gif',
                yearRange: 'c-60:c+10'
            }).BootStrap(); //BootStrap() 產生符合 BootStrap 的樣式內容
        });

        //查詢資料
        function getData(p) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetStatisticsStorageTankInfo.aspx",
                data: {
                    type: "list",
                    ctype: $("#txt1").val(),
                    cpname: $("#txt2").val(),
                    businessOrg: $("#txt3").val(),
                    factory: $("#txt4").val(),
                    workshop: $("#txt5").val(),
                    ssn: $("#txt6").val(),
                    txt7: $("#txt7").val(),
                    txt8: $("#txt8").val(),
                    openDateBegin: $("#txt9").val(),
                    openDateEnd: $("#txt10").val(),
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
                        $("#div_table").empty();
                        var tabstr = '';
                        if ($("#txt1").val() == '01') {
                            tabstr += '<table width="100%" border="0" cellspacing="0" cellpadding="0"><thead><tr><th align="center" nowrap="nowrap" rowspan="2">轄區儲槽編號</th><th align="center" nowrap="nowrap" rowspan="2">業者簡稱</th><th align="center" nowrap="nowrap" rowspan="2">能源署編號</th>' +
                                '<th align="center" nowrap="nowrap" rowspan="2">容量<br>（公秉）</th><th align="center" nowrap="nowrap" rowspan="2">內徑<br>(公尺）</th><th align="center" nowrap="nowrap" rowspan="2">內容物</th>' +
                                '<th align="center" nowrap="nowrap" rowspan="2">油品種類</th><th align="center" nowrap="nowrap" rowspan="2">形式<br>1.錐頂 <br>2.內浮頂 <br>3.外浮頂 <br>4.掩體式 </th><th align="center" nowrap="nowrap" rowspan="2">啟用日期<br>年/月</th><th align="center" nowrap="nowrap" colspan="4">代行檢查有效期限</th>' +
                                '<th align="center" nowrap="nowrap" rowspan="2" valign="top">狀態<br>1.使用中 <br>2.開放中 <br>3.停用 <br>4.其他 </th><th align="center" nowrap="nowrap" rowspan="2">延長開 <br>放年限 <br>多?年 </th></tr><tr><th >代檢機構 <br>(填表說明) </th><th >外部 <br>年/月/日 </th><th >代檢機構 <br>' +
                                '(填表說明) </th><th >內部 <br>年/月/日 </th>';
                        }
                        else {
                            tabstr += '<table width="100%" border="0" cellspacing="0" cellpadding="0"><thead><tr><th nowrap>液化天然氣廠 </th><th nowrap>儲槽編號 </th><th nowrap>容量 <br>（萬公秉） </th>' +
                                '<th nowrap>外徑 <br>(公尺） </th><th nowrap>高度 <br>(公尺)</th><th nowrap>形式 </th><th nowrap>啟用日期 </th><th nowrap>狀態 <br>(使用中/ 開放中/ 停用)</th>' +
                                '<th nowrap>勞動部檢查<br>合格證及有效期限 </th><th nowrap>代行/檢查機構 </th>';
                        }
                        tabstr += '</tr></thead><tbody>';

                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                tabstr += '<tr>';
                                if ($("#txt1").val() == '01') {
                                    tabstr += '<td nowrap="nowrap">' + $(this).children("轄區儲槽編號").text().trim() + '</td>';
                                    tabstr += '<td nowrap="nowrap">' + $(this).children("業者簡稱").text().trim() + '</td>';
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
                                }
                                else {
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
                                }
                                tabstr += '</tr>';
                            });
                        }
                        else {
                            if ($("#txt1").val() == '01')
                                tabstr += '<tr><td colspan="15">查詢無資料</td></tr>';
                            else
                                tabstr += '<tr><td colspan="11">查詢無資料</td></tr>';
                        }
                            
                        $("#tablist tbody").append(tabstr);
                        Page.Option.Selector = "#pageblock";
                        Page.Option.FunctionName = "getData";
                        Page.CreatePage(p, $("total", data).text());
                        $("#sp_totalText").html('共' + $("total", data).text() + '個儲槽');
                        $("#div_table").append(tabstr);
                    }
                }
            });
        }

        function getPipeSn() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetStatisticsStorageTankInfo.aspx",
                data: {
                    type: "ddl",
                    ctype: $("#txt1").val(),
                    cpname: $("#txt2").val(),
                    businessOrg: $("#txt3").val(),
                    factory: $("#txt4").val(),
                    workshop: $("#txt5").val(),
                    ssn: $("#txt6").val(),
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
                                ddlstr += '<option value="' + $(this).children("長途管線識別碼").text().trim() + '">' + $(this).children("長途管線識別碼").text().trim() + '</option>';
                            });
                        }
                        $("#txt6").empty();
                        $("#txt6").append(ddlstr);
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
                            <div class="BoxBgWa BoxRadiusA BoxBorderSa padding10ALL margin10T">
                                <div class="OchiTrasTable width100 TitleLength09 font-size3">
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">類別</div>
                                        <div class="OchiCell width100">
                                            <select id="txt1" class="width100 inputex" >
                                                <option value="">請選擇</option>
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
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">轄區儲槽編號</div>
                                        <div class="OchiCell width100">
                                            <select id="txt6" class="width100 inputex" >
                                                <option value="">請選擇</option>
                                            </select>
                                        </div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div id="div_content" class="OchiRow">
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">啟用日期</div>
                                    <div class="OchiCell width100">
                                        <input id="txt9" type="text" class="width20 inputex pickDate " disabled /> ~ <input id="txt10" type="text" class="width20 inputex pickDate " disabled />
                                    </div>
                                </div><!-- OchiRow -->
                            </div><!-- OchiTrasTable -->
                                <br />
                                <div class="twocol">
                                    <div class="left">
                                        <span id="sp_totalText" style="color:red" class="font-size5"></span>
                                    </div>
                                    <div class="right">
                                        <a id="querybtn" href="javascript:void(0);" title="查詢" class="genbtn" >查詢</a>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div id="div_table" class="stripeMeB tbover">

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




