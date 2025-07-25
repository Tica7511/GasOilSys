﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OilInfo.aspx.cs" Inherits="WebPage_OilInfo" %>

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
    <title>石油業輸儲設備查核及檢測資訊系統</title>
    <!--#include file="Head_Include.html"-->
    <script type="text/javascript">
        $(document).ready(function () {
            //doOpenMagPopup();

            getYearList();
            getCompanyNameTitile();
            $("#sellist").val(getTaiwanDate());
            getData(getTaiwanDate());

            if (($("#Competence").val() != '02') && ($("#Competence").val() != '03'))
                $("#editbtn").hide();
            else
                $("#editbtn").show();

            getConfirmedStatus();

            //本年度聯絡資料詳細內容開窗
            $(document).on("click", "#contactbtn", function () {
                $("#mgsellist").val($("#sellist option:selected").val());
                getContactDetailData($("#sellist option:selected").val());
                doOpenMagPopupConfirmData();
            });

            //本年度聯絡資料詳細內容儲存按鈕
            $(document).on("click", "#mgsubbtn", function () {
                var msg = '';

                if ($("#mgtxt1").val() == '')
                    msg += "請輸入【本年度查核聯絡窗口 姓名】\n";
                if ($("#mgtxt2").val() == '')
                    msg += "請輸入【本年度查核聯絡窗口 職稱】\n";
                if ($("#mgtxt3").val() == '')
                    msg += "請輸入【本年度查核聯絡窗口 分機】\n";
                if ($("#mgtxt4").val() == '')
                    msg += "請輸入【本年度查核聯絡窗口 email】\n";
                if ($("#mgtxt5").val() == '')
                    msg += "請輸入【本年度檢測聯絡窗口 姓名】\n";
                if ($("#mgtxt6").val() == '')
                    msg += "請輸入【本年度檢測聯絡窗口 職稱】\n";
                if ($("#mgtxt7").val() == '')
                    msg += "請輸入【本年度檢測聯絡窗口 分機】\n";
                if ($("#mgtxt8").val() == '')
                    msg += "請輸入【本年度檢測聯絡窗口 email】\n";

                if (msg != "") {
                    alert("Error message: \n" + msg);
                    return false;
                }

                // Create an FormData object
                var data = new FormData();

                // If you want to add an extra field for the FormData
                data.append("type", "Oil");
                data.append("cpid", $.getQueryString("cp"));
                data.append("year", $("#sellist option:selected").val());
                data.append("category", "contact");
                data.append("txt1", encodeURIComponent($("#mgtxt1").val()));
                data.append("txt2", encodeURIComponent($("#mgtxt2").val()));
                data.append("txt3", encodeURIComponent($("#mgtxt3").val()));
                data.append("txt4", encodeURIComponent($("#mgtxt4").val()));
                data.append("txt5", encodeURIComponent($("#mgtxt5").val()));
                data.append("txt6", encodeURIComponent($("#mgtxt6").val()));
                data.append("txt7", encodeURIComponent($("#mgtxt7").val()));
                data.append("txt8", encodeURIComponent($("#mgtxt8").val()));

                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "../handler/AddCompanyCheck.aspx",
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
                            $.magnificPopup.close();
                        }
                    }
                });
            });

            //選擇年份
            $(document).on("change", "#sellist", function () {
                getData($("#sellist option:selected").val());
            });

            //編輯按鈕
            $(document).on("click", "#editbtn", function () {
                $("#sellist").attr("disabled", true);
                btnDisable(false);
            });

            //返回按鈕
            $(document).on("click", "#backbtn", function () {
                var str = confirm('尚未儲存的部分將不會更改，確定返回嗎?');

                if (str) {
                    btnDisable(true);
                    $("#sellist").attr("disabled", false);
                    getData($("#sellist option:selected").val());
                }
            });

            $(document).on("click", "#exportbtn", function () {

                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "../Handler/AddWord.aspx",
                    data: {

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
                        }
                    }
                });
            });

            //儲存按鈕
            $(document).on("click", "#subbtn", function () {
                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "../Handler/AddOilInfo.aspx",
                    data: {
                        cid: $.getQueryString("cp"),
                        year: $("#sellist option:selected").val(),
                        ctel: encodeURIComponent($("#ctel").val()),
                        caddr: encodeURIComponent($("#caddr").val()),
                        storagetank: encodeURIComponent($("#storagetank").val()),
                        storagetankcapacity: encodeURIComponent($("#storagetankcapacity").val()),
                        pipeline: encodeURIComponent($("#pipeline").val()),
                        pipelinelength: encodeURIComponent($("#pipelinelength").val()),
                        //report: $("#report").val(),
                        checkdate: encodeURIComponent($("#checkdate").val()),
                        txt1: encodeURIComponent($("#txt1").val()),
                        txt2: encodeURIComponent($("#txt2").val()),
                        txt3: encodeURIComponent($("#txt3").val()),
                        txt4: encodeURIComponent($("#txt4").val()),
                        txt5: encodeURIComponent($("#txt5").val()),
                        txt6: encodeURIComponent($("#txt6").val()),
                        txt7: encodeURIComponent($("#txt7").val()),
                        txt8: encodeURIComponent($("#txt8").val()),
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
                            location.href = "OilInfo.aspx?cp=" + $.getQueryString("cp");
                        }
                    }
                });
            });
        }); // end js

        function btnDisable(status) {
            if (status == true) {
                $("#editbtn").show();
                $("#backbtn").hide();
                $("#subbtn").hide();
            }
            else {
                $("#editbtn").hide();
                $("#backbtn").show();
                $("#subbtn").show();
            }

            $("#ctel").attr('disabled', status);
            $("#caddr").attr('disabled', status);
            $("#storagetank").attr('disabled', status);
            $("#storagetankcapacity").attr('disabled', status);
            $("#pipeline").attr('disabled', status);
            $("#pipelinelength").attr('disabled', status);
            //$("#report").attr('disabled', false);
            $("#checkdate").attr('disabled', status);
            $("#txt1").attr('disabled', status);
            $("#txt2").attr('disabled', status);
            $("#txt3").attr('disabled', status);
            $("#txt4").attr('disabled', status);
            $("#txt5").attr('disabled', status);
            $("#txt6").attr('disabled', status);
            $("#txt7").attr('disabled', status);
            $("#txt8").attr('disabled', status);
        }

        function getCompanyNameTitile() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetCompanyName.aspx",
                data: {
                    type: "Oil",
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
                                $("#cname").val($(this).children("cpname").text().trim());
                            });
                        }
                    }
                }
            });
        }

        function getData(year) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetOilInfo.aspx",
                data: {
                    cpid: $.getQueryString("cp"),
                    year: year
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
                            $(data).find("data_item").each(function () {
                                $("#ctel").val($(this).children("電話").text().trim());
                                $("#caddr").val($(this).children("地址").text().trim());
                                $("#storagetank").val($(this).children("儲槽數量").text().trim());
                                $("#storagetankcapacity").val($(this).children("儲槽容量").text().trim());
                                $("#pipeline").val($(this).children("管線數量").text().trim());
                                $("#pipelinelength").val($(this).children("管線長度").text().trim());
                                //$("#report").val($(this).children("維運計畫書及成果報告").text().trim());
                                $("#checkdate").val($(this).children("曾執行過查核日期").text().trim());
                                $("#txt1").val($(this).children("年度查核姓名").text().trim());
                                $("#txt2").val($(this).children("年度查核職稱").text().trim());
                                $("#txt3").val($(this).children("年度查核分機").text().trim());
                                $("#txt4").val($(this).children("年度查核email").text().trim());
                                $("#txt5").val($(this).children("年度檢測姓名").text().trim());
                                $("#txt6").val($(this).children("年度檢測職稱").text().trim());
                                $("#txt7").val($(this).children("年度檢測分機").text().trim());
                                $("#txt8").val($(this).children("年度檢測email").text().trim());
                            });
                        }
                        else {
                            $("#ctel").val($(this).children("").text().trim());
                            $("#caddr").val($(this).children("").text().trim());
                            $("#storagetank").val($(this).children("").text().trim());
                            $("#storagetankcapacity").val($(this).children("").text().trim());
                            $("#pipeline").val($(this).children("").text().trim());
                            $("#pipelinelength").val($(this).children("").text().trim());
                            $("#checkdate").val($(this).children("").text().trim());
                            $("#txt1").val($(this).children("").text().trim());
                            $("#txt2").val($(this).children("").text().trim());
                            $("#txt3").val($(this).children("").text().trim());
                            $("#txt4").val($(this).children("").text().trim());
                            $("#txt5").val($(this).children("").text().trim());
                            $("#txt6").val($(this).children("").text().trim());
                            $("#txt7").val($(this).children("").text().trim());
                            $("#txt8").val($(this).children("").text().trim());
                        }

                        //確認權限&按鈕顯示或隱藏
                        if ($("#sellist").val() != getTaiwanDate()) {
                            $("#div_contact").hide();
                            $("#editbtn").hide();
                        }
                        else {
                            if (($("#Competence").val() == '01') || ($("#Competence").val() == '04') || ($("#Competence").val() == '05') || ($("#Competence").val() == '06')) {
                                $("#div_contact").hide();
                                $("#editbtn").hide();
                            }
                            else {
                                $("#div_contact").show();
                                $("#editbtn").show();
                            }
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
                url: "../Handler/GetOilInfo.aspx",
                data: {
                    cpid: $.getQueryString("cp"),
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

        //確認資料是否完成
        function getConfirmedStatus() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetCompanyName.aspx",
                data: {
                    type: "Oil",
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
                                    }
                                }
                            });
                        }
                    }
                }
            });
        }

        //取得本年度聯絡資料
        function getContactDetailData(year) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetOilInfo.aspx",
                data: {
                    cpid: $.getQueryString("cp"),
                    year: year
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
                            $(data).find("data_item").each(function () {
                                name1 = $(this).children("年度查核姓名").text().trim();
                                name2 = $(this).children("年度檢測姓名").text().trim();

                                if (name1 != '') {
                                    $("#mgtxt1").val(getPartName(name1));
                                    //$("#mgtxt1").val(name1);
                                }
                                else {
                                    $("#mgtxt1").val('');
                                }

                                if (name2 != '') {
                                    $("#mgtxt5").val(getPartName(name2));
                                    //$("#mgtxt5").val(name2);
                                }
                                else {
                                    $("#mgtxt5").val('');
                                }

                                $("#mgtxt2").val("");
                                $("#mgtxt3").val("");
                                $("#mgtxt4").val("");
                                $("#mgtxt6").val("");
                                $("#mgtxt7").val("");
                                $("#mgtxt8").val("");
                            });
                        }
                        else {
                            $("#mgtxt1").val("");
                            $("#mgtxt2").val("");
                            $("#mgtxt3").val("");
                            $("#mgtxt4").val("");
                            $("#mgtxt5").val("");
                            $("#mgtxt6").val("");
                            $("#mgtxt7").val("");
                            $("#mgtxt8").val("");
                        }
                    }
                }
            });
        }

        function getPartName(name) {
            var partname = '';

            if (name.length == 1) {
                partname = name;
            }
            else if (name.length == 2) {
                partname = name.substring(0, name.length - 1) + "X";
            }
            else {
                partname = name.substring(0, name.length - 2) + "XX";
            }

            return partname;
        }

        //取得現在時間之民國年
        function getTaiwanDate() {
            var nowDate = new Date();

            var nowYear = nowDate.getFullYear();
            var nowTwYear = (nowYear - 1911);

            return nowTwYear;
        }

        function doOpenMagPopup() {
            $.magnificPopup.open({
                items: {
                    src: '#importantDialog'
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

        function doOpenMagPopup2() {
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

        function doOpenMagPopupConfirmData() {
            $.magnificPopup.open({
                items: {
                    src: '#messageblockConfirm'
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
<body class="bgB">
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
                    </div>
                    <!-- CSS3loading -->
                </div>
                <!-- status -->
            </div>
            <!-- preloader -->
            <div class="container BoxBgWa BoxShadowD">
                <div class="WrapperBody" id="WrapperBody">
                    <!--#include file="OilHeader.html"-->
                    <input type="hidden" id="Competence" value="<%= competence %>" />
                    <div id="ContentWrapper">
                        <div class="container margin15T">
                            <div class="padding10ALL">
                                <div class="filetitlewrapper">
                                    <!--#include file="OilBreadTitle.html"-->
                                </div>

                                <div class="row margin20T">
                                    <div class="col-lg-3 col-md-4 col-sm-5">
                                        <div id="navmenuV">
                                            <!--#include file="OilLeftMenu.html"-->
                                        </div>
                                    </div>
                                    <div class="col-lg-9 col-md-8 col-sm-7">
                                        <div class="twocol">
                                            <div class="left font-size5 ">
                                                <i class="fa fa-chevron-circle-right IconCa" aria-hidden="true"></i>
                                                <select id="sellist" class="inputex">
                                                </select>
                                                年
                                            </div>
                                            <div id="fileall" class="right">
                                                <%--<a id="exportbtn" href="javascript:void(0);" class="genbtn">匯入</a>--%>
                                                <a id="editbtn" href="javascript:void(0);" title="編輯" class="genbtn">編輯</a>
                                                <a id="backbtn" href="javascript:void(0);" title="返回" class="genbtn" style="display: none">返回</a>
                                                <a id="subbtn" href="javascript:void(0);" class="genbtn" style="display: none">儲存</a>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="OchiTrasTable width100 TitleLength09 font-size3">
                                            <!-- OchiRow -->
                                            <div class="OchiRow">
                                                <div class="OchiCell OchiTitle IconCe TitleSetWidth">事業名稱</div>
                                                <div class="OchiCell width100">
                                                    <input type="text" id="cname" class="inputex width99" disabled>
                                                </div>
                                            </div>
                                            <!-- OchiRow -->
                                            <div class="OchiRow">
                                                <div class="OchiCell OchiTitle IconCe TitleSetWidth">地址</div>
                                                <div class="OchiCell width100">
                                                    <input type="text" id="caddr" class="inputex width99" disabled>
                                                </div>
                                            </div>
                                            <!-- OchiRow -->
                                            <div class="OchiRow">
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">電話</div>
                                                    <div class="OchiCell width100">
                                                        <input type="text" id="ctel" class="inputex width100" disabled>
                                                    </div>
                                                </div>
                                                <!-- OchiHalf -->
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">儲槽數量</div>
                                                    <div class="OchiCell width100">
                                                        <input type="text" id="storagetank" class="inputex width100" disabled>
                                                    </div>
                                                </div>
                                                <!-- OchiHalf -->
                                            </div>
                                            <!-- OchiRow -->
                                            <div class="OchiRow">
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">儲槽容量(公秉)</div>
                                                    <div class="OchiCell width100">
                                                        <input type="text" id="storagetankcapacity" class="inputex width100" disabled>
                                                    </div>
                                                </div>
                                                <!-- OchiHalf -->
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">管線數量</div>
                                                    <div class="OchiCell width100">
                                                        <input type="text" id="pipeline" class="inputex width100" disabled>
                                                    </div>
                                                </div>
                                                <!-- OchiHalf -->
                                            </div>
                                            <!-- OchiRow -->
                                            <div class="">
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">管線長度(公里)</div>
                                                    <div class="OchiCell width100">
                                                        <input type="text" id="pipelinelength" class="inputex width100" disabled>
                                                    </div>
                                                </div>
                                                <!-- OchiHalf -->
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">前一次查核日期</div>
                                                    <div class="OchiCell width100">
                                                        <input type="text" id="checkdate" class="inputex width100" disabled>
                                                    </div>
                                                </div>
                                                <!-- OchiHalf -->
                                            </div>
                                            <!-- OchiRow -->
                                            <div id="div_contact" class="OchiRow">
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">本年度聯絡窗口</div>
                                                    <div class="OchiCell width100">
                                                        <a id="contactbtn" href="javascript:void(0);" title="詳細資料" class="grebtn">詳細資料</a>
                                                    </div>
                                                </div>
                                                <!-- OchiHalf -->
                                            </div>

                                            </br>
                                            <%--<div class="OchiRow">
                                                <div class="margin5TB font-size4" style="text-align:center">本年度查核聯絡窗口</div>
                                            </div><!-- OchiRow -->

                                            <div class="OchiRow">
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">姓名</div>
                                                    <div class="OchiCell width100"><input type="text" id="txt1" class="inputex width100" disabled></div>
                                                </div><!-- OchiHalf -->
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">職稱</div>
                                                    <div class="OchiCell width100"><input type="text" id="txt2" class="inputex width100" disabled></div>
                                                </div><!-- OchiHalf -->
                                            </div><!-- OchiRow -->
                                            <div class="OchiRow">
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">分機</div>
                                                    <div class="OchiCell width100"><input type="text" id="txt3" class="inputex width100" disabled></div>
                                                </div><!-- OchiHalf -->
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">email</div>
                                                    <div class="OchiCell width100"><input type="text" id="txt4" class="inputex width100" disabled></div>
                                                </div><!-- OchiHalf -->
                                            </div><!-- OchiRow -->

                                            </br>
                                            <div class="OchiRow">
                                                <div class="margin5TB font-size4" style="text-align:center">本年度檢測聯絡窗口</div>
                                            </div><!-- OchiRow -->

                                            <div class="OchiRow">
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">姓名</div>
                                                    <div class="OchiCell width100"><input type="text" id="txt5" class="inputex width100" disabled></div>
                                                </div><!-- OchiHalf -->
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">職稱</div>
                                                    <div class="OchiCell width100"><input type="text" id="txt6" class="inputex width100" disabled></div>
                                                </div><!-- OchiHalf -->
                                            </div><!-- OchiRow -->
                                            <div class="OchiRow">
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">分機</div>
                                                    <div class="OchiCell width100"><input type="text" id="txt7" class="inputex width100" disabled></div>
                                                </div><!-- OchiHalf -->
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">email</div>
                                                    <div class="OchiCell width100"><input type="text" id="txt8" class="inputex width100" disabled></div>
                                                </div><!-- OchiHalf -->
                                            </div><!-- OchiRow -->--%>

                                            <%--<div class="OchiRow">
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">維運計畫書及成果報告</div>
                                                    <div class="OchiCell width100"><input type="text" id="report" class="inputex width100" disabled></div>
                                                </div><!-- OchiHalf -->
                                            </div><!-- OchiRow -->--%>
                                        </div>
                                        <!-- OchiTrasTable -->
                                    </div>
                                    <!-- col -->
                                </div>
                                <!-- row -->
                            </div>
                        </div>
                        <!-- container -->
                    </div>
                    <!-- ContentWrapper -->



                    <div class="container-fluid">
                        <div class="backTop"><a href="#" class="backTotop">TOP</a></div>
                    </div>
                </div>
                <!-- WrapperBody -->

                <!--#include file="Footer.html"-->

            </div>
            <!-- BoxBgWa -->
            <!-- 側邊選單內容:動態複製主選單內容 -->
            <div id="sidebar-wrapper">
            </div>
            <!-- sidebar-wrapper -->

        </form>
    </div>
    <!-- 結尾用div:修正mmenu form bug -->

    <!-- Magnific Popup -->
    <div id="importantDialog" class="magpopup magSizeS mfp-hide">
        <div class="magpopupTitle" style="color: red"><b>重要公告</b></div>
        <div class="padding10ALL">
            <div class="center ">
                <div class="font-size5"><b>行政院關心大家,請大家務必參考以下資訊:</b></div>
                <br />
                <div class="font-size5">
                    <i class="fa fa-file-pdf-o IconCc" aria-hidden="true"></i><a href="../doc/高氣溫戶外作業勞工熱危害預防指引-112-6-1修正.pdf" target="_blank">高氣溫戶外作業勞工熱危害預防</a>
                </div>
            </div>

            <%--<div class="twocol margin10T">
            <div class="right">
                <a id="importCancelbtn" href="javascript:void(0);" class="genbtn closemagnificPopup">取消</a>
            </div>
        </div>--%>
        </div>
        <!-- padding10ALL -->

    </div>
    <!--magpopup -->

    <!-- Magnific Popup -->
    <div id="messageblockConfirm" class="magpopup magSizeM mfp-hide">
        <div class="magpopupTitle">本年度聯絡詳細資料</div>
        <div class="padding10ALL">
            <%--<div class="twocol">
                <div class="left font-size5 ">
                    <i class="fa fa-chevron-circle-right IconCa" aria-hidden="true"></i>
                    <select id="mgsellist" class="inputex" disabled>
                    </select>
                    年
                </div>
                <div class="right">
                    <span class="font-size5" style="color: red">請填寫本年度各聯絡窗口資訊，填妥後請點選儲存來確認資料</span>
                </div>
            </div>
            <br />--%>
            <div class="OchiTrasTable width100 TitleLength08 font-size3">
                <div class="OchiRow">
                    <div class="margin5TB font-size4" style="text-align: center">本年度查核聯絡窗口</div>
                </div>
                <!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiHalf">
                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">姓名</div>
                        <div class="OchiCell width100">
                            <input id="mgtxt1" type="text" class="inputex width100">
                        </div>
                    </div>
                    <!-- OchiHalf -->
                    <div class="OchiHalf">
                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">職稱</div>
                        <div class="OchiCell width100">
                            <input id="mgtxt2" type="text" class="inputex width100">
                        </div>
                    </div>
                    <!-- OchiHalf -->
                </div>
                <!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiHalf">
                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">分機</div>
                        <div class="OchiCell width100">
                            <input id="mgtxt3" type="text" class="inputex width100">
                        </div>
                    </div>
                    <!-- OchiHalf -->
                    <div class="OchiHalf">
                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">email</div>
                        <div class="OchiCell width100">
                            <input id="mgtxt4" type="text" class="inputex width100">
                        </div>
                    </div>
                    <!-- OchiHalf -->
                </div>
                <!-- OchiRow -->
                </br>
            <div class="OchiRow">
                <div class="margin5TB font-size4" style="text-align: center">本年度檢測聯絡窗口</div>
            </div>
                <!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiHalf">
                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">姓名</div>
                        <div class="OchiCell width100">
                            <input type="text" id="mgtxt5" class="inputex width100">
                        </div>
                    </div>
                    <!-- OchiHalf -->
                    <div class="OchiHalf">
                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">職稱</div>
                        <div class="OchiCell width100">
                            <input type="text" id="mgtxt6" class="inputex width100">
                        </div>
                    </div>
                    <!-- OchiHalf -->
                </div>
                <!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiHalf">
                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">分機</div>
                        <div class="OchiCell width100">
                            <input type="text" id="mgtxt7" class="inputex width100">
                        </div>
                    </div>
                    <!-- OchiHalf -->
                    <div class="OchiHalf">
                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">email</div>
                        <div class="OchiCell width100">
                            <input type="text" id="mgtxt8" class="inputex width100">
                        </div>
                    </div>
                    <!-- OchiHalf -->
                </div>
                <!-- OchiRow -->
            </div>
            <!-- OchiTrasTable -->

            <div class="twocol margin10T">
                <div class="right">
                    <a id="mgcancelbtn" href="javascript:void(0);" class="genbtn closecolorbox">取消</a>
                    <a id="mgsubbtn" href="javascript:void(0);" class="genbtn">儲存</a>
                </div>
            </div>

        </div>
        <!-- padding10ALL -->

    </div>
    <!--magpopup -->

    <!-- colorbox -->
    <div style="display: none;">
        <div id="workitem">
            <div class="margin35T padding5RL">
                <div class="OchiTrasTable width100 TitleLength08 font-size3">
                    <div class="OchiRow">
                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">工作項次</div>
                        <div class="OchiCell width100">
                            <input type="number" class="inputex width10">﹒<input type="number" class="inputex width10">﹒<input type="number" class="inputex width10">
                        </div>
                    </div>
                    <!-- OchiRow -->
                    <div class="OchiRow">
                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">預定日期</div>
                        <div class="OchiCell width100">
                            <input type="text" class="inputex Jdatepicker width30">
                        </div>
                    </div>
                    <!-- OchiRow -->
                    <div class="OchiRow">
                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">預定完成執行內容</div>
                        <div class="OchiCell width100">
                            <textarea rows="5" cols="" class="inputex width100"></textarea>
                        </div>
                    </div>
                    <!-- OchiRow -->
                </div>
                <!-- OchiTrasTable -->
            </div>

            <div class="twocol margin10T">
                <div class="right">
                    <a href="#" class="genbtn closecolorbox">取消</a>
                    <a href="#" class="genbtn">儲存</a>
                </div>
            </div>
            <br />
            <br />
        </div>
    </div>

    <!-- 本頁面使用的JS -->
    <script type="text/javascript">
        $(document).ready(function () {

        });
    </script>
    <script type="text/javascript" src="../js/GenCommon.js"></script>
    <!-- UIcolor JS -->
    <script type="text/javascript" src="../js/PageCommon.js"></script>
    <!-- 系統共用 JS -->
    <script type="text/javascript" src="../js/MenuOil.js"></script>
    <!-- 系統共用 JS -->
    <script type="text/javascript" src="../js/SubMenuOilA.js"></script>
    <!-- 內頁選單 -->
    <script type="text/javascript" src="../js/autoHeight.js"></script>
    <!-- 高度不足頁面的絕對置底footer -->
</body>
</html>

