<%@ Page Language="C#" AutoEventWireup="true" CodeFile="edit_GasCIPS.aspx.cs" Inherits="WebPage_edit_GasCIPS" %>

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
            getDDL(getTaiwanDate());
            getData();

            //座標列表開窗
            $(document).on("click", "#xymgbox", function () {
                if ($.getQueryString("guid") == "") {
                    alert("請先儲存此頁再新增異常點尚未改善完成之座標");
                    return false;
                }

                $("#CoGguid").val($.getQueryString("guid"));
                getCoordinate();
                doOpenMagPopup2();
            });

            //新增座標
            $(document).on("click", "#newbtnxy", function () {
                $("#Gguid").val("");
                $("#typeName").html("新增座標");
                $("#xy1").val("");
                $("#xy2").val("");
                $("#xy3").val("");
                doOpenMagPopup3();
            });

            //編輯座標
            $(document).on("click", "a[name='editbtnxy']", function () {
                $("#Gguid").val($(this).attr("aid"));
                $("#typeName").html("編輯座標");
                getCoordinateData();
                doOpenMagPopup3();
            });

            //取消 新增/編輯座標
            $(document).on("click", "#cancelbtn2", function () {
                $("#CoGguid").val();
                getCoordinate();
                doOpenMagPopup2();
            });

            //座標儲存
            $(document).on("click", "#subbtn2", function () {
                var msg = '';

                if ($("#xy1").val() == '')
                    msg += "請輸入【x座標】\n";
                if ($("#xy2").val() == '')
                    msg += "請輸入【y座標】\n";
                if ($("#xy3").val() == '')
                    msg += "請輸入【級距】\n";

                if (msg != "") {
                    alert("Error message: \n" + msg);
                    return false;
                }

                // Get form
                var form = $('#form1')[0];

                // Create an FormData object 
                var data = new FormData(form);

                var mode = ($.getQueryString("guid") == "") ? "new" : "edit";

                // If you want to add an extra field for the FormData
                data.append("cp", $.getQueryString("cp"));
                data.append("pGuid", $.getQueryString("guid"));
                data.append("guid", $("#Gguid").val());
                data.append("mode", encodeURIComponent(mode));
                data.append("year", encodeURIComponent(getTaiwanDate()));
                data.append("txt1", encodeURIComponent($("#xy1").val()));
                data.append("txt2", encodeURIComponent($("#xy2").val()));
                data.append("txt3", encodeURIComponent($("#xy3").val()));

                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "../handler/AddGasCIPSxy.aspx",
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

                            $("#CoGguid").val();
                            getCoordinate();
                            doOpenMagPopup2();
                        }
                    }
                });
            });

            //刪除座標
            $(document).on("click", "a[name='delbtn2']", function () {
                if (confirm("確定刪除?")) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../handler/DelGasCIPSxy.aspx",
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
                                getCoordinate();
                            }
                        }
                    });
                }
            });

            //取消按鍵
            $(document).on("click", "#cancelbtn", function () {
                var str = confirm('尚未儲存的部分將不會更改，確定返回嗎?');

                if (str)
                    location.href = "GasCIPS.aspx?cp=" + $.getQueryString("cp");
            });

            //儲存按鍵
            $(document).on("click", "#subbtn", function () {
                var msg = '';

                if ($("#txt1").val() == '')
                    msg += "請選擇【長途管線識別碼】\n";
                if ($("#txt2").val() == '')
                    msg += "請選擇【同時檢測管線數量】\n";
                if (($("#txt3_1").val()) == '' || ($("#txt3_2").val() == ''))
                    msg += "請輸入【最近一次執行 年/月】\n";
                if (($("#txt4_1").val()) == '' || ($("#txt4_2").val() == ''))
                    msg += "請輸入【報告產出 年/月】\n";
                if ($("#txt5").val() == '')
                    msg += "請選擇【檢測長度】\n";
                if ($("#txt6").val() == '')
                    msg += "請選擇【合格標準】\n";
                if ($("#txt7").val() == '')
                    msg += "請選擇【立即改善 數量】\n";
                if ($("#txt8").val() == '')
                    msg += "請選擇【立即改善 改善完成數量】\n";
                if ($("#txt9").val() == '')
                    msg += "請選擇【排程改善 數量】\n";
                if ($("#txt10").val() == '')
                    msg += "請選擇【排程改善 改善完成數量】\n";
                if ($("#txt11").val() == '')
                    msg += "請選擇【需監控點 數量】\n";
                if ($("#txt12").val() == '')
                    msg += "請輸入【x座標】\n";
                if ($("#txt13").val() == '')
                    msg += "請輸入【y座標】\n";

                if (msg != "") {
                    alert("Error message: \n" + msg);
                    return false;
                }

                // Get form
                var form = $('#form1')[0];

                // Create an FormData object 
                var data = new FormData(form);

                var mode = ($.getQueryString("guid") == "") ? "new" : "edit";

                // If you want to add an extra field for the FormData
                data.append("cp", $.getQueryString("cp"));
                data.append("guid", $.getQueryString("guid"));
                data.append("mode", encodeURIComponent(mode));
                data.append("year", encodeURIComponent(getTaiwanDate()));
                data.append("txt1", encodeURIComponent($("#txt1").val()));
                data.append("txt2", encodeURIComponent($("#txt2").val()));
                data.append("txt3_1", encodeURIComponent($("#txt3_1").val()));
                data.append("txt3_2", encodeURIComponent($("#txt3_2").val()));
                data.append("txt4_1", encodeURIComponent($("#txt4_1").val()));
                data.append("txt4_2", encodeURIComponent($("#txt4_2").val()));
                data.append("txt5", encodeURIComponent($("#txt5").val()));
                data.append("txt6", encodeURIComponent($("#txt6").val()));
                data.append("txt7", encodeURIComponent($("#txt7").val()));
                data.append("txt8", encodeURIComponent($("#txt8").val()));
                data.append("txt9", encodeURIComponent($("#txt9").val()));
                data.append("txt10", encodeURIComponent($("#txt10").val()));
                data.append("txt11", encodeURIComponent($("#txt11").val()));
                data.append("txt12", encodeURIComponent($("#txt12").val()));
                data.append("txt13", encodeURIComponent($("#txt13").val()));
                data.append("txt14", encodeURIComponent($("#txt14").val()));
                $.each($("#fileUpload")[0].files, function (i, file) {
                    data.append('file', file);
                });

                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "../handler/AddGasCIPS.aspx",
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

                            location.href = "GasCIPS.aspx?cp=" + $.getQueryString("cp");
                        }
                    }
                });
            });

            //上傳前的附件列表
            $(document).on("change", "#fileUpload", function () {
                $("#filelist").empty();
                var fp = $("#fileUpload");
                var lg = fp[0].files.length; // get length
                var items = fp[0].files;
                var fragment = "";

                if (lg > 0) {
                    for (var i = 0; i < lg; i++) {
                        var fileName = items[i].name; // get file name

                        // append li to UL tag to display File info
                        fragment += "<label>" + (i + 1) + ". " + fileName + "</label></br>";
                    }

                    $("#filelist").append(fragment);
                }
            });

            $(".pickDate").datepick({
                dateFormat: 'yymmdd',
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: '../images/calendar.gif',
                yearRange: 'c-60:c+10'
            }).BootStrap(); //BootStrap() 產生符合 BootStrap 的樣式內容
        }); // end js

        function getData() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetGasCIPS.aspx",
                data: {
                    guid: $.getQueryString("guid"),
                    type: "data",
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
                                $("#txt1").val($(this).children("長途管線識別碼").text().trim());
                                $("#txt2").val($(this).children("同時檢測管線數量").text().trim());
                                $("#txt3_1").val(splitYearMonth(0, $(this).children("最近一次執行年月").text().trim()));
                                $("#txt3_2").val(splitYearMonth(1, $(this).children("最近一次執行年月").text().trim()));
                                $("#txt4_1").val(splitYearMonth(0, $(this).children("報告產出年月").text().trim()));
                                $("#txt4_2").val(splitYearMonth(1, $(this).children("報告產出年月").text().trim()));
                                $("#txt5").val($(this).children("檢測長度").text().trim());
                                $("#txt6").val($(this).children("合格標準請參照填表說明2").text().trim());
                                $("#txt7").val($(this).children("立即改善_數量").text().trim());
                                $("#txt8").val($(this).children("立即改善_改善完成數量").text().trim());
                                $("#txt9").val($(this).children("排程改善_數量").text().trim());
                                $("#txt10").val($(this).children("排程改善_改善完成數量").text().trim());
                                $("#txt11").val($(this).children("需監控點_數量").text().trim());
                                $("#txt14").val($(this).children("備註").text().trim());
                            });
                        }
                    }
                }
            });
        }

        function getDDL(year) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/GetGasTubeInfo.aspx",
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
                        var ddlstr = '<option value="">請選擇</option>';
                        if ($(data).find("data_item3").length > 0) {
                            $(data).find("data_item3").each(function (i) {
                                ddlstr += '<option value="' + $(this).children("長途管線識別碼").text().trim() + '">' + $(this).children("長途管線識別碼").text().trim() + '</option>';
                            });
                        }

                        $("#txt1").empty();
                        $("#txt1").append(ddlstr);
                    }
                }
            });
        }

        function getCoordinate() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetGasCIPSxy.aspx",
                data: {
                    pGuid: $.getQueryString("guid"),
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
                        $("#tablistcoordinate tbody").empty();
                        var tabstr = '';
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                tabstr += '<tr>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("x座標").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("y座標").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("級距").text().trim() + '</td>';
                                tabstr += '<td name="td_editCoordinate" nowrap="" align="center"><a href="javascript:void(0);" name="delbtn2" aid="' + $(this).children("guid").text().trim() + '">刪除</a> ';
                                tabstr += '<a href="javascript:void(0);" name="editbtnxy" mid="edit" aid="' + $(this).children("guid").text().trim() + '">編輯</a></td>'
                                tabstr += '</tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="4">查詢無資料</td></tr>';
                        $("#tablistcoordinate tbody").append(tabstr);

                        //確認權限&按鈕顯示或隱藏
                        if (($("#Competence").val() == '01') || ($("#Competence").val() == '03')) {
                            $("#thFunc2").show();
                            $("td[name='td_editCoordinate']").show();
                        }
                        else {
                            $("#thFunc2").hide();
                            $("td[name='td_editCoordinate']").hide();
                        }
                    }
                }
            });
        }

        function getCoordinateData() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetGasCIPSxy.aspx",
                data: {
                    guid: $("#Gguid").val(),
                    type: "data",
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
                                $("#xy1").val($(this).children("x座標").text().trim());
                                $("#xy2").val($(this).children("y座標").text().trim());
                                $("#xy3").val($(this).children("級距").text().trim());
                            });
                        }
                    }
                }
            });
        }

        function splitYearMonth(arrylenth, fulldate) {

            if (fulldate != '') {
                var farray = new Array();
                farray = fulldate.split("/");
                var twdate = farray[arrylenth];

                return twdate;
            }
            else {
                return '';
            }

        }

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

                return twdate;
            }
            else {
                return '';
            }

        }

        function getTaiwanDate() {
            var nowDate = new Date();

            var nowYear = nowDate.getFullYear();
            var nowTwYear = (nowYear - 1911);

            return nowTwYear;
        }

        function doOpenMagPopup2() {
            $.magnificPopup.open({
                items: {
                    src: '#messageblock2'
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

        function doOpenMagPopup3() {
            $.magnificPopup.open({
                items: {
                    src: '#messageblock3'
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
        <input type="hidden" id="CoGguid" />
        <input type="hidden" id="Gguid" />
        <input type="hidden" id="Sno" />
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
                                <div class="left font-size4" style="width:50%">
                                    <i class="fa fa-chevron-circle-right IconCa" aria-hidden="true"></i> 
                                    長途管線識別碼 : <select id="txt1" class="width40 inputex" ></select>
                                </div>
                                <div class="right">
                                    <a id="cancelbtn" href="javascript:void(0);" title="返回" class="genbtn" >取消</a>
                                    <a id="subbtn" href="javascript:void(0);" title="儲存" class="genbtn" >儲存</a>
                                </div>
                            </div><br />
                            <div class="OchiTrasTable width100 TitleLength09 font-size3">
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">同時檢測管線數量</div>
                                        <div class="OchiCell width100"><input type="number" min="0" id="txt2" class="inputex width100"></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">最近一次執行 年/月</div>
                                        <div class="OchiCell width100">
                                            民國 <input type="number" min="1" id="txt3_1" placeholder="請填寫民國年" class="inputex width40"> 年 
                                            <select id="txt3_2" class="width25 inputex" >
                                                <option value="">請選擇</option>
                                                <option value="01">1</option>
                                                <option value="02">2</option>
                                                <option value="03">3</option>
                                                <option value="04">4</option>
                                                <option value="05">5</option>
                                                <option value="06">6</option>
                                                <option value="07">7</option>
                                                <option value="08">8</option>
                                                <option value="09">9</option>
                                                <option value="10">10</option>
                                                <option value="11">11</option>
                                                <option value="12">12</option>
                                            </select> 月
                                        </div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">報告產出 年/月</div>
                                        <div class="OchiCell width100">
                                            民國 <input type="number" min="1" id="txt4_1" placeholder="請填寫民國年" class="inputex width40"> 年 
                                            <select id="txt4_2" class="width25 inputex" >
                                                <option value="">請選擇</option>
                                                <option value="01">1</option>
                                                <option value="02">2</option>
                                                <option value="03">3</option>
                                                <option value="04">4</option>
                                                <option value="05">5</option>
                                                <option value="06">6</option>
                                                <option value="07">7</option>
                                                <option value="08">8</option>
                                                <option value="09">9</option>
                                                <option value="10">10</option>
                                                <option value="11">11</option>
                                                <option value="12">12</option>
                                            </select> 月
                                        </div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">檢測長度</div>
                                        <div class="OchiCell width100"><input type="text" id="txt5" class="inputex width50"> KM</div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">合格標準</div>
                                        <div class="OchiCell width100">
                                            <select id="txt6" class="width100 inputex" >
                                                <option value="">請選擇</option>
                                                <option value="通電電位< -850mVCSE">通電電位< -850mVCSE</option>
                                                <option value="極化電位< -850mVCSE">極化電位< -850mVCSE</option>
                                                <option value="極化量>100mV">極化量>100mV</option>
                                                <option value="其他">其他</option>
                                            </select>
                                        </div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                </br>
                                <div class="OchiRow">
                                    <div class="margin5TB font-size4 font-bold" style="text-align:center">立即改善</div>
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">數量</div>
                                        <div class="OchiCell width100"><input type="number" min="0" id="txt7" class="inputex width100"></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">改善完成數量</div>
                                        <div class="OchiCell width100"><input type="number" min="0" id="txt8" class="inputex width100"></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                </br>
                                <div class="OchiRow">
                                    <div class="margin5TB font-size4 font-bold" style="text-align:center">排程改善</div>
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">數量</div>
                                        <div class="OchiCell width100"><input type="number" min="0" id="txt9" class="inputex width100"></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">改善完成數量</div>
                                        <div class="OchiCell width100"><input type="number" min="0" id="txt10" class="inputex width100"></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                </br>
                                <div class="OchiRow">
                                    <div class="margin5TB font-size4 font-bold" style="text-align:center">需監控點</div>
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">數量</div>
                                        <div class="OchiCell width100"><input type="number" min="0" id="txt11" class="inputex width100"></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">異常點尚未改善完成之座標</div>
                                        <div class="OchiCell width100"><a href="javascript:void(0);" id="xymgbox" class="genbtn">座標列表</a></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                </br>
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">備註</div>
                                        <div class="OchiCell width100"><input type="text" id="txt14" class="inputex width100"></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <br />
                                <div class="OchiRow">
                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">附件上傳</div>
                                    <div class="OchiCell width100">
                                        <input type="file" id="fileUpload" multiple="multiple" />
                                        <div id="filelist"></div>
                                    </div>
                                </div><!-- OchiRow -->
                            </div><!-- OchiTrasTable -->
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
<div id="messageblock2" class="magpopup magSizeS mfp-hide">
  <div class="magpopupTitle">異常點尚未改善完成之座標</div>
  <div class="padding10ALL">
      <div class="twocol">
          <div class="right">
            <a id="newbtnxy" href="javascript:void(0);" title="新增" class="genbtn">新增</a>
          </div>
      </div><br />
      <div class="stripeMeB tbover">
          <table id="tablistcoordinate" border="0" cellspacing="0" cellpadding="0" width="100%">
              <thead>
		        	<tr>
		        		<th nowrap="nowrap" align="center">x座標</th>
		        		<th nowrap="nowrap" align="center">y座標</th>
		        		<th nowrap="nowrap" align="center">級距</th>
		        		<th id="thFunc2" nowrap="nowrap" align="center" width="10%">功能</th>
		        	</tr>
              </thead>
              <tbody></tbody>
          </table>
      </div>

  </div><!-- padding10ALL -->

</div><!--magpopup -->

<!-- Magnific Popup -->
<div id="messageblock3" class="magpopup magSizeS mfp-hide">
  <div class="magpopupTitle"><span id="typeName"></span></div>
  <div class="padding10ALL">
      <div class="OchiTrasTable width100 TitleLength08 font-size3">
          <div class="OchiRow">
              <div class="OchiHalf">
                  <div class="OchiCell OchiTitle IconCe TitleSetWidth">x座標</div>
                  <div class="OchiCell width100"><input id="xy1" type="text" class="inputex width100"></div>
              </div><!-- OchiHalf -->
              <div class="OchiHalf">
                  <div class="OchiCell OchiTitle IconCe TitleSetWidth">y座標</div>
                  <div class="OchiCell width100"><input id="xy2" type="text" class="inputex width100"></div>
              </div><!-- OchiHalf -->
          </div><!-- OchiRow -->
          <div class="OchiRow">
              <div class="OchiHalf">
                  <div class="OchiCell OchiTitle IconCe TitleSetWidth">級距</div>
                  <div class="OchiCell width100"><input id="xy3" type="text" class="inputex width100"></div>
              </div><!-- OchiHalf -->
          </div><!-- OchiRow -->
      </div><!-- OchiTrasTable -->

      <div class="twocol margin10T">
            <div class="right">
                <a id="cancelbtn2" href="javascript:void(0);" class="genbtn closecolorbox">取消</a>
                <a id="subbtn2" href="javascript:void(0);" class="genbtn">儲存</a>
            </div>
        </div>

  </div><!-- padding10ALL -->

</div><!--magpopup -->

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

<div style="display:none;">
    <div id="datesetting">
        <div class="margin35T padding5RL">
            <div class="OchiTrasTable width100 TitleLength04 font-size3">
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">開始日期</div>
                    <div class="OchiCell width100"><input type="text" class="inputex Jdatepicker width100"></div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">結束日期</div>
                    <div class="OchiCell width100"><input type="text" class="inputex Jdatepicker width100"></div>
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
	<script type="text/javascript" src="../js/GenCommon.js"></script><!-- UIcolor JS -->
	<script type="text/javascript" src="../js/PageCommon.js"></script><!-- 系統共用 JS -->
	<script type="text/javascript" src="../js/MenuGas.js"></script><!-- 系統共用 JS -->
	<script type="text/javascript" src="../js/SubMenuGasA.js"></script><!-- 內頁選單 -->
	<script type="text/javascript" src="../js/autoHeight.js"></script><!-- 高度不足頁面的絕對置底footer -->
</body>
</html>




