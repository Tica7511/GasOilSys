<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VerificationTest.aspx.cs" Inherits="WebPage_VerificationTest" %>

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
    <title>查核與檢測資料系統</title>
    <!--#include file="Head_Include.html"-->
    <script type="text/javascript">
        $(document).ready(function () {
            getDDL('028', 'sel_type');
            getData();

            $("#CheckFiles").show();
            $("#subbtnCheckFile").show();
            $("#RelationFiles").show();
            $("#subbtnRelationFile").show();

            //查詢按鈕
            $(document).on("click", "#querybtn", function () {
                getData();
            });

            //選擇類別
            $(document).on("change", "#sel_type", function () {
                $("#tObjectGuid").val('');
                $("#txt_object").val('');
                $("#tType").val($("#sel_type option:selected").val());
            });

            //對象請選擇按鈕
            $(document).on("click", "#btn_object", function () {
                if ($("#tType").val() != '') {
                    getObjectData();
                }
                else {
                    alert("請選擇【類別】");
                    return false;
                }

                doOpenMagPopup();
            });

            $(document).on("click", "a[name='objectbtn']", function () {

                $("#tObjectGuid").val($(this).attr('aid'));
                $("#txt_object").val($(this).attr('cname'));
                $.magnificPopup.close();
            });

            //查核檢測報告開窗
            $(document).on("click", "a[name='fileCheckBtn']", function () {
                $("#CheckFiles").val('');
                $("#CheckReportGuid").val($(this).attr("aid"));
                $("#CheckReportCPGuid").val($(this).attr("cid"));
                getDataCheckFile();
                doOpenPopup();
            });

            //相關報告開窗
            $(document).on("click", "a[name='fileRelationBtn']", function () {
                $("#RelationFiles").val('');
                $("#RelationReportGuid").val($(this).attr("aid"));
                $("#RelationReportCPGuid").val($(this).attr("cid"));
                getDataRelationFile();
                doOpenPopup2();
            });

            //查核檢測報告儲存
            $(document).on("click", "#subbtnCheckFile", function () {
                var msg = '';

                if ($("#CheckFiles").val() == "")
                    msg += "請先選擇檔案再上傳";
                if (msg != "") {
                    alert(msg);
                    return false;
                }

                var files = $("#CheckFiles").get(0).files;

                // Get form
                var form = $('#form1')[0];

                // Create an FormData object 
                var data = new FormData(form);

                // If you want to add an extra field for the FormData
                data.append("guid", $("#CheckReportGuid").val());
                data.append("cpid", $("#CheckReportCPGuid").val());
                data.append("type", "10");
                data.append("year", getTaiwanDate());
                for (var i = 0; i < files.length; i++) {
                    data.append("files", files[i]);
                }

                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "../handler/AddVerificationTestFile.aspx",
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
                            $("#CheckFiles").val('');
                            getDataCheckFile();
                        }
                    }
                });
            });

            //刪除資料
            $(document).on("click", "a[name='delbtn']", function () {
                var isDel = confirm("確定刪除資料嗎?");
                if (isDel) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../Handler/DelVerificationTest.aspx",
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
                                getData();
                            }
                        }
                    });
                }
            });

            //相關報告儲存
            $(document).on("click", "#subbtnRelationFile", function () {
                var msg = '';

                if ($("#RelationFiles").val() == "")
                    msg += "請先選擇檔案再上傳";
                if (msg != "") {
                    alert(msg);
                    return false;
                }

                var files = $("#RelationFiles").get(0).files;

                // Get form
                var form = $('#form1')[0];

                // Create an FormData object 
                var data = new FormData(form);

                // If you want to add an extra field for the FormData
                data.append("guid", $("#RelationReportGuid").val());
                data.append("cpid", $("#RelationReportCPGuid").val());
                data.append("type", "11");
                data.append("year", getTaiwanDate());
                for (var i = 0; i < files.length; i++) {
                    data.append("files", files[i]);
                }

                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "../handler/AddVerificationTestFile.aspx",
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
                            $("#RelationFiles").val('');
                            getDataRelationFile();
                        }
                    }
                });
            });

            //刪除查核檢測報告
            $(document).on("click", "a[name='delbtnCheckFile']", function () {
                var isDel = confirm("確定刪除檔案嗎?");
                if (isDel) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../Handler/DelVerificationTestFile.aspx",
                        data: {
                            guid: $(this).attr("aid"),
                            type: $(this).attr("atype"),
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
                                getDataCheckFile();
                            }
                        }
                    });
                }
            });

            //刪除相關報告
            $(document).on("click", "a[name='delbtnRelationFile']", function () {
                var isDel = confirm("確定刪除檔案嗎?");
                if (isDel) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../Handler/DelVerificationTestFile.aspx",
                        data: {
                            guid: $(this).attr("aid"),
                            sn: $(this).attr("asn"),
                            type: $(this).attr("atype"),
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
                                getDataRelationFile();
                            }
                        }
                    });
                }
            });

            //對象開窗取消按鈕
            //$(document).on("click", "#Ocancelbtn", function () {
            //    $.magnificPopup.close();
            //});

            //圖片編輯按鈕
            $(document).on("click", "#editbtn", function () {
                location.href = 'edit_OilLongPipeline.aspx?cp=' + $.getQueryString("cp") + '&guid=' + $("#nGuid").val();
            });

            //清除對象內文字
            $(document).on("click", "#btn_object_delete", function () {
                $("#tObjectGuid").val('');
                $("#txt_object").val('');
            });

            $(".pickDate").datepick({
                dateFormat: 'yymmdd',
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: '../images/calendar.gif',
                yearRange: 'c-60:c+10'
            }).BootStrap(); //BootStrap() 產生符合 BootStrap 的樣式內容
        }); // end js

        //查詢資料
        function getData() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetVerificationTest.aspx",
                data: {
                    sType: $("#sel_type").val(),
                    tobject: $("#tObjectGuid").val(),
                    timeBegin: $("#txt_timeBegin").val(),
                    timeEnd: $("#txt_timeEnd").val(),
                    reportNum: $("#txt_reportNum").val(),
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
                                tabstr += '<td nowrap="nowrap">' + $(this).children("報告編號").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("類別_V").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + getDate($(this).children("查核日期起").text().trim()) + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("對象").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap" align="center"><a href="javascript:void(0);" name="fileCheckBtn" class="grebtn" aid="'
                                    + $(this).children("guid").text().trim() + '" cid="' + $(this).children("業者guid").text().trim() + '">附件列表</a></td>';
                                tabstr += '<td nowrap="nowrap" align="center"><a href="javascript:void(0);" name="fileRelationBtn" class="grebtn" aid="'
                                    + $(this).children("guid").text().trim() + '" cid="' + $(this).children("業者guid").text().trim() + '">附件列表</a></td>';
                                tabstr += '<td name="td_edit" nowrap="" align="center"><a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("guid").text().trim() + '">刪除</a>';
                                tabstr += ' <a href="edit_VerificationTest.aspx?guid=' + $(this).children("guid").text().trim() + '" name="editbtn">編輯</a></td>';
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

        //取得對象列表
        function getObjectData() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetVerificationTest.aspx",
                data: {
                    dataType: $("#tType").val(),
                    type: "object",
                    year: getTaiwanDate()
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
                        $("#div_objecttable").empty();
                        var tabstr = '';
                        if ($("#tType").val() != '4') {
                            tabstr += '<table width="100%" border="0" cellspacing="0" cellpadding="0"><thead><tr><th align="center" nowrap="nowrap">公司名稱</th><th align="center" nowrap="nowrap">事業部</th>' +
                                '<th align="center" nowrap="nowrap">營業處廠</th><th align="center" nowrap="nowrap">中心庫區儲運課工場</th><th align="center" nowrap="nowrap" width="100">功能</th>';
                        }
                        else {
                            tabstr += '<table width="100%" border="0" cellspacing="0" cellpadding="0"><thead><tr><th align="center" nowrap="nowrap">對象</th><th align="center" nowrap="nowrap">場站/位置</th>' +
                                '<th align="center" nowrap="nowrap" width="100">功能</th>';
                        }
                        tabstr += '</tr></thead><tbody>';

                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                tabstr += '<tr>';
                                if ($("#tType").val() != '4') {
                                    tabstr += '<td nowrap="nowrap">' + $(this).children("公司名稱").text().trim() + '</td>';
                                    tabstr += '<td nowrap="nowrap">' + $(this).children("事業部").text().trim() + '</td>';
                                    tabstr += '<td nowrap="nowrap">' + $(this).children("營業處廠").text().trim() + '</td>';
                                    tabstr += '<td nowrap="nowrap">' + $(this).children("中心庫區儲運課工場").text().trim() + '</td>';
                                    tabstr += '<td name="td_edit" nowrap="" align="center"><a class="genbtn" href="javascript:void(0);" name="objectbtn" aid="'
                                        + $(this).children("guid").text().trim() + '" cname="' + $(this).children("CompanyFullName").text().trim() + '">確認對象</a></td>';
                                    tabstr += '</tr>';
                                }
                                else {
                                    tabstr += '<td nowrap="nowrap">' + $(this).children("公司名稱").text().trim() + '</td>';
                                    tabstr += '<td nowrap="nowrap">' + $(this).children("場佔位置").text().trim() + '</td>';
                                    tabstr += '<td name="td_edit" nowrap="" align="center"><a class="genbtn" href="javascript:void(0);" name="objectbtn" aid="'
                                        + $(this).children("guid").text().trim() + '" cname="' + $(this).children("CompanyFullName").text().trim() + '">確認對象</a></td>';
                                    tabstr += '</tr>';
                                }
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="5">查詢無資料</td></tr>';
                        $("#div_objecttable").append(tabstr);

                        //確認權限&按鈕顯示或隱藏
                        //if (($("#Competence").val() == '01') || ($("#Competence").val() == '04') || ($("#Competence").val() == '05') || ($("#Competence").val() == '06')) {
                        //    $("#newbtn").hide();
                        //    $("#th_edit").hide();
                        //    $("td[name='td_edit']").hide();
                        //}
                        //else {
                        //    $("#newbtn").show();
                        //    $("#th_edit").show();
                        //    $("td[name='td_edit']").show();
                        //}
                    }
                }
            });
        }

        //取得查核檢測報告列表
        function getDataCheckFile() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/GetFile.aspx",
                data: {
                    guid: $("#CheckReportGuid").val(),
                    type: '10',
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
                        $("#tablistCheckFile tbody").empty();
                        var tabstr = '';
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                var filename = $(this).children("新檔名").text().trim();
                                var fileextension = $(this).children("附檔名").text().trim();
                                tabstr += '<tr>'
                                tabstr += '<td nowrap><a href="../DOWNLOAD.aspx?category=VerificationTest&type=Check&sn=' + $(this).children("排序").text().trim() +
                                    '&v=' + $(this).children("guid").text().trim() + '">' + filename + fileextension + '</a></td>';
                                tabstr += '<td nowrap>' + $(this).children("上傳日期").text().trim() + '</td>';
                                tabstr += '<td name="td_editFile" nowrap="" align="center"><a href="javascript:void(0);" name="delbtnCheckFile" aid="' + $(this).children("guid").text().trim() +
                                    '" atype="10" >刪除</a></td>';
                                tabstr += '</tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="3">查詢無資料</td></tr>';
                        $("#tablistCheckFile tbody").append(tabstr);
                    }
                }
            });
        }

        //取得查核檢測報告列表
        function getDataRelationFile() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/GetFile.aspx",
                data: {
                    guid: $("#RelationReportGuid").val(),
                    type: '11',
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
                        $("#tablistRelationFile tbody").empty();
                        var tabstr = '';
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                var filename = $(this).children("新檔名").text().trim();
                                var fileextension = $(this).children("附檔名").text().trim();
                                tabstr += '<tr>'
                                tabstr += '<td nowrap><a href="../DOWNLOAD.aspx?category=VerificationTest&type=Relation&sn=' + $(this).children("排序").text().trim() +
                                    '&v=' + $(this).children("guid").text().trim() + '">' + filename + fileextension + '</a></td>';
                                tabstr += '<td nowrap>' + $(this).children("上傳日期").text().trim() + '</td>';
                                tabstr += '<td name="td_editFile" nowrap="" align="center"><a href="javascript:void(0);" name="delbtnRelationFile" aid="' + $(this).children("guid").text().trim() +
                                    '" asn="' + $(this).children("排序").text().trim() + '"  atype="11" >刪除</a></td>';
                                tabstr += '</tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="3">查詢無資料</td></tr>';
                        $("#tablistRelationFile tbody").append(tabstr);
                    }
                }
            });
        }

        //取得代碼檔列表
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

        //取得現在時間之民國年
        function getTaiwanDate() {
            var nowDate = new Date();

            var nowYear = nowDate.getFullYear();
            var nowTwYear = (nowYear - 1911);

            return nowTwYear;
        }

        function getDate(fulldate) {
            if (fulldate != '') {
                var year = fulldate.substring("0", "3");
                var month = fulldate.substring("3", "5");
                var date = fulldate.substring("5", "7");

                return year + "/" + month + "/" + date;
            }
            else {
                return fulldate;
            }
        }

        function doOpenMagPopup() {
            $.magnificPopup.open({
                items: {
                    src: '#messageblock'
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

        //查核/檢測報告列表 開窗
        function doOpenPopup() {
            $.magnificPopup.open({
                items: {
                    src: '#popupCheckFile'
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

        //相關報告列表 開窗
        function doOpenPopup2() {
            $.magnificPopup.open({
                items: {
                    src: '#popupRelationFile'
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
    </script>
</head>
<body class="bgP">
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
        <!--#include file="VerificationHeader.html"-->
        <input type="hidden" id="Competence" value="<%= competence %>" />
        <input type="hidden" id="nGuid" />
        <input type="hidden" id="tType" />
        <input type="hidden" id="tObjectGuid" />
        <input type="hidden" id="CheckReportGuid" />
        <input type="hidden" id="CheckReportCPGuid" />
        <input type="hidden" id="RelationReportGuid" />
        <input type="hidden" id="RelationReportCPGuid" />
        <div id="ContentWrapper">
            <div class="container margin15T">
                <div class="padding10ALL">
                    <div class="filetitlewrapper">
                        <span class="filetitle font-size7">查核與檢測資料系統</span>
                    </div>

                    <div class="BoxBgWa BoxRadiusA BoxBorderSa padding10ALL margin10T">
                        <div class="OchiTrasTable width100 font-size3 TitleLength05">
                            <div class="OchiRow">
                                <div class="OchiHalf">
                                    <div class="OchiCell OchiTitle TitleSetWidth">類別</div>
                                    <div class="OchiCell width100"> 
                                        <select id="sel_type" class="inputex width100"></select>
                                    </div>
                                </div><!-- OchiHalf -->
                                <div class="OchiHalf">
                                    <div class="OchiCell OchiTitle TitleSetWidth">對象</div>
                                    <div class="OchiCell width100">
                                        <input id="txt_object" type="text" class="inputex width70" disabled /> 
                                        <a id="btn_object" href="javascript:void(0);" title="請選擇" class="grebtn font-size3">請選擇</a>
                                        <a id="btn_object_delete" href="javascript:void(0);" title="清除" class="grebtn font-size3">清除</a>
                                    </div>
                                </div><!-- OchiHalf -->
                            </div><!-- OchiRow -->
                        </div><!-- OchiTrasTable -->

                        <div class="OchiTrasTable width100 font-size3 TitleLength05">
                            <div class="OchiRow">
                                <div class="OchiHalf">
                                    <div class="OchiCell OchiTitle TitleSetWidth">查核期間</div>
                                    <div class="OchiCell width100"><input id="txt_timeBegin" type="text" class="inputex pickDate width30" disabled> ~ 
                                        <input id="txt_timeEnd" type="text" class="inputex pickDate width30" disabled> 
                                    </div>
                                </div><!-- OchiHalf -->
                                <div class="OchiHalf">
                                    <div class="OchiCell OchiTitle TitleSetWidth">報告編號</div>
                                    <div class="OchiCell width100"><input id="txt_reportNum" type="text" class="inputex width100"></div>
                                </div><!-- OchiHalf -->
                            </div><!-- OchiRow -->
                        </div><!-- OchiTrasTable -->

                        <div class="textright margin10T">
                            <a id="querybtn" href="javascript:void(0);" class="genbtn">查詢</a>
                            <a id="newbtn" href="edit_VerificationTest.aspx" class="genbtn">新增</a>
                        </div>
                    </div>

                    <div class="twocol margin10T">
                        <div class="left font-size5 "><i class="fa fa-chevron-circle-right IconCa" aria-hidden="true"></i> 查詢結果</div>
                        <div class="right font-normal font-size3">
                        </div>
                    </div>

                    <div id="div_table" class="stripeMeP font-size3 margin10T">
                        <table id="tablist" width="100%" border="0" cellspacing="0" cellpadding="0">
                            <thead>
                                <tr>
                                    <th nowrap="nowrap" width="8%">報告編號</th>
                                    <th nowrap="nowrap" width="10%">類別</th>
                                    <th nowrap="nowrap" width="5%">查核日期</th>
                                    <th nowrap="nowrap">對象</th>
                                    <th nowrap="nowrap" width="7%">查核/檢測報告</th>
                                    <th nowrap="nowrap" width="7%">相關報告</th>
                                    <th nowrap="nowrap" width="100">功能</th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
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
<div id="sidebar-wrapper">
   
</div><!-- sidebar-wrapper -->

</form>
</div>
<!-- 結尾用div:修正mmenu form bug -->

<!-- Magnific Popup -->
<div id="messageblock" class="magpopup magSizeL mfp-hide">
  <div class="magpopupTitle">對象列表</div>
  <div class="padding10ALL">
      <div class="twocol">
          <div class="left font-size5 ">

          </div>
          <div class="right">

          </div>
      </div><br />
      <div id="div_objecttable" class="stripeMeP tbover">

      </div>
      <br />

      <%--<div class="twocol">
          <div class="left font-size5 ">
              
          </div>
          <div class="right">
              <a href="javascript:void(0);" id="Ocancelbtn" class="genbtn">取消</a>
              <a href="javascript:void(0);" id="Osubbtn" class="genbtn">儲存</a>
          </div>
      </div>--%>

  </div><!-- padding10ALL -->

</div><!--magpopup -->

<div id="popupCheckFile" class="magpopup magSizeM mfp-hide">
    <div class="magpopupTitle">查核/檢測報告列表</div>
    <div class="padding10ALL">
        <div class="twocol">
          <div class="left font-size4"></div>
          <div class="right">
              <input style="display:none" id="CheckFiles" type="file" class="inputex width80"  /> 
              <a style="display:none" id="subbtnCheckFile" href="javascript:void(0);" class="genbtn">儲存</a>
          </div>
        </div>
        </br>
        <div class="overtablewidth margin5T">
           <table class="overtablewidthS" width="100%">
             <tr>
               <td>
                 <!-- start -->
                 <div class="stripeMeP">
                   <table id="tablistCheckFile" cellspacing="0" cellpadding="0" width="100%">
                       <thead>
                           <tr>
                             <th align="center" nowrap>文件名稱</th>
                             <th align="center" nowrap>上傳日期</th>
                             <th align="center" id="th_editCheck" width="120" nowrap="nowrap">功能</th>
                           </tr>
                       </thead>
                       <tbody></tbody>
                   </table>
                 </div><!-- stripeMe -->
                 <!-- end -->
               </td>
             </tr>
           </table>
        </div><!-- overtablewidth -->
    </div>
    <!-- padding10ALL -->
</div>

<div id="popupRelationFile" class="magpopup magSizeM mfp-hide">
    <div class="magpopupTitle">相關報告列表</div>
    <div class="padding10ALL">
        <div class="twocol">
          <div class="left font-size4"></div>
          <div class="right">
              <input style="display:none" id="RelationFiles" type="file" multiple class="inputex width80"  /> 
              <a style="display:none" id="subbtnRelationFile" href="javascript:void(0);" class="genbtn">儲存</a>
          </div>
        </div>
        </br>
        <div class="overtablewidth margin5T">
           <table class="overtablewidthS" width="100%">
             <tr>
               <td>
                 <!-- start -->
                 <div class="stripeMeP">
                   <table id="tablistRelationFile" cellspacing="0" cellpadding="0" width="100%">
                       <thead>
                           <tr>
                             <th align="center" nowrap>文件名稱</th>
                             <th align="center" nowrap>上傳日期</th>
                             <th align="center" id="th_editRelation" width="120" nowrap="nowrap">功能</th>
                           </tr>
                       </thead>
                       <tbody></tbody>
                   </table>
                 </div><!-- stripeMe -->
                 <!-- end -->
               </td>
             </tr>
           </table>
        </div><!-- overtablewidth -->
    </div>
    <!-- padding10ALL -->
</div>

<!-- colorbox -->
<div style="display:none;">
    <div id="workitem">
        <div class="margin35T padding5RL">
            <div class="OchiTrasTable width100 TitleLength08 font-size3">
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">工作項次</div>
                    <div class="OchiCell width100">
                        <input type="number" class="inputex width10">﹒<input type="number" class="inputex width10">﹒<input type="number" class="inputex width10">
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">預定日期</div>
                    <div class="OchiCell width100"><input type="text" class="inputex Jdatepicker width30"> </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">預定完成執行內容</div>
                    <div class="OchiCell width100"><textarea rows="5" cols="" class="inputex width100"></textarea></div>
                </div><!-- OchiRow -->
            </div><!-- OchiTrasTable -->
        </div>

        <div class="twocol margin10T">
            <div class="right">
                <a href="#" class="genbtn closecolorbox">取消</a>
                <a href="#" class="genbtn">儲存</a>
            </div>
        </div>
        <br /><br />
    </div>
</div>

<!-- 本頁面使用的JS -->
    <script type="text/javascript">
        $(document).ready(function(){
        
        });
    </script>
    <script type="text/javascript" src="../js/GenCommon.js"></script><!-- UIcolor JS -->
    <script type="text/javascript" src="../js/PageCommon.js"></script><!-- 系統共用 JS -->
    <script type="text/javascript" src="../js/MenuOil.js"></script><!-- 系統共用 JS -->
    <script type="text/javascript" src="../js/autoHeight.js"></script><!-- 高度不足頁面的絕對置底footer -->
</body>
</html>

