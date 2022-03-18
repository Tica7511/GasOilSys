<%@ Page Language="C#" AutoEventWireup="true" CodeFile="edit_GasTubeComplete_Other.aspx.cs" Inherits="WebPage_edit_GasTubeComplete2" %>

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

            $(document).on("change", "input[name='rdYeae1']", function () {
                if ($("input[name='rdYeae1']:checked").val() == 'Y') {
                    $("#txt5_1").attr("disabled", false);
                    $("#txt5_2").attr("disabled", false);
                }
                else {
                    $("#txt5_1").attr("disabled", true);
                    $("#txt5_2").attr("disabled", true);
                    $("#txt5_1").val("");
                    $("#txt5_2").val("");
                }
            });

            $(document).on("change", "input[name='rdYeae2']", function () {
                if ($("input[name='rdYeae2']:checked").val() == 'Y') {
                    $("#txt6_1").attr("disabled", false);
                    $("#txt6_2").attr("disabled", false);
                }
                else {
                    $("#txt6_1").attr("disabled", true);
                    $("#txt6_2").attr("disabled", true);
                    $("#txt6_1").val("");
                    $("#txt6_2").val("");
                }
            });

            $(document).on("change", "input[name='rdYeae3']", function () {
                if ($("input[name='rdYeae3']:checked").val() == 'Y') {
                    $("#txt7_1").attr("disabled", false);
                    $("#txt7_2").attr("disabled", false);
                }
                else {
                    $("#txt7_1").attr("disabled", true);
                    $("#txt7_2").attr("disabled", true);
                    $("#txt7_1").val("");
                    $("#txt7_2").val("");
                }
            });

            $(document).on("change", "input[name='rdYeae4']", function () {
                if ($("input[name='rdYeae4']:checked").val() == 'Y') {
                    $("#txt8_1").attr("disabled", false);
                    $("#txt8_2").attr("disabled", false);
                }
                else {
                    $("#txt8_1").attr("disabled", true);
                    $("#txt8_2").attr("disabled", true);
                    $("#txt8_1").val("");
                    $("#txt8_2").val("");
                }
            });

            //取消按鍵
            $(document).on("click", "#cancelbtn", function () {
                var str = confirm('尚未儲存的部分將不會更改，確定返回嗎?');

                if (str)
                    location.href = "GasTubeComplete.aspx?cp=" + $.getQueryString("cp");
            });

            //儲存按鍵
            $(document).on("click", "#subbtn", function () {
                var msg = '';

                if ($("#txt1").val() == '')
                    msg += "請輸入【長途管線識別碼】\n";
                //if (($("#txt2_1").val()) == '' || ($("#txt2_2").val() == ''))
                //    msg += "請輸入【風險評估年/月】\n";
                //if ($("#txt3").val() == '')
                //    msg += "請輸入【智慧型通管器(ILI)可行性】\n";
                //if ($("#txt4").val() == '')
                //    msg += "請輸入【耐壓強度試驗(TP)可行性】\n";
                //if ($("input[name='rdYeae1']:checked").val() == '')
                //    msg += "請選擇【緊密電位(CIPS)年/月】\n";
                //else
                //    if ($("input[name='rdYeae1']:checked").val() == 'Y')
                //        if (($("#txt5_1").val()) == '' || ($("#txt5_2").val() == ''))
                //            msg += "請輸入【緊密電位(CIPS)年/月】\n";
                //if ($("input[name='rdYeae2']:checked").val() == '')
                //    msg += "請選擇【電磁包覆(PCM)年/月】\n";
                //else
                //    if ($("input[name='rdYeae2']:checked").val() == 'Y')
                //        if (($("#txt6_1").val()) == '' || ($("#txt6_2").val() == ''))
                //            msg += "請輸入【電磁包覆(PCM)年/月】\n";
                //if ($("input[name='rdYeae3']:checked").val() == '')
                //    msg += "請選擇【智慧型通管器(ILI)年/月】\n";
                //else
                //    if ($("input[name='rdYeae3']:checked").val() == 'Y')
                //        if (($("#txt7_1").val()) == '' || ($("#txt7_2").val() == ''))
                //            msg += "請輸入【智慧型通管器(ILI)年/月】\n";
                //if ($("input[name='rdYeae4']:checked").val() == '')
                //    msg += "請選擇【耐壓強度試驗(TP)年/月】\n";
                //else
                //    if ($("input[name='rdYeae4']:checked").val() == 'Y')
                //        if (($("#txt8_1").val()) == '' || ($("#txt8_2").val() == ''))
                //            msg += "請輸入【耐壓強度試驗(TP)年/月】\n";
                //if ($("#txt9").val() == '')
                //    msg += "請輸入【耐壓強度試驗(TP)介質】\n";
                //if ($("#txt10").val() == '')
                //    msg += "請輸入【試壓壓力與MOP壓力倍數】\n";
                //if ($("#txt11").val() == '')
                //    msg += "請輸入【耐壓強度試驗(TP)持壓時間(小時)】\n";
                //if ($("#txt12").val() == '')
                //    msg += "請輸入【受雜散電流影響】\n";
                //if ($("#txt13").val() == '')
                //    msg += "請輸入【洩漏偵測系統(LLDS)】\n";
                //if ($("#txt14").val() == '')
                //    msg += "請輸入【強化作為】\n";

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
                data.append("no", "2");
                data.append("year", encodeURIComponent(getTaiwanDate()));
                data.append("txt1", encodeURIComponent($("#txt1").val()));
                data.append("txt2_1", encodeURIComponent($("#txt2_1").val()));
                data.append("txt2_2", encodeURIComponent($("#txt2_2").val()));
                data.append("txt3", encodeURIComponent($("#txt3").val()));
                data.append("txt4", encodeURIComponent($("#txt4").val()));
                if ($("input[name='rdYeae1']:checked").val() == 'Y')
                    data.append("txt5", encodeURIComponent($("#txt5_1").val() + '/' + $("#txt5_2").val()));
                else
                    data.append("txt5", encodeURIComponent('NA'));
                if ($("input[name='rdYeae2']:checked").val() == 'Y')
                    data.append("txt6", encodeURIComponent($("#txt6_1").val() + '/' + $("#txt6_2").val()));
                else
                    data.append("txt6", encodeURIComponent('NA'));
                if ($("input[name='rdYeae3']:checked").val() == 'Y')
                    data.append("txt7", encodeURIComponent($("#txt7_1").val() + '/' + $("#txt7_2").val()));
                else
                    data.append("txt7", encodeURIComponent('NA'));
                if ($("input[name='rdYeae4']:checked").val() == 'Y')
                    data.append("txt8", encodeURIComponent($("#txt8_1").val() + '/' + $("#txt8_2").val()));
                else
                    data.append("txt8", encodeURIComponent('NA'));
                data.append("txt9", encodeURIComponent($("#txt9").val()));
                data.append("txt10", encodeURIComponent($("#txt10").val()));
                data.append("txt11", encodeURIComponent($("#txt11").val()));
                data.append("txt12", encodeURIComponent($("#txt12").val()));
                data.append("txt13", encodeURIComponent($("#txt13").val()));
                data.append("txt14", encodeURIComponent($("#txt14").val()));

                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "../handler/AddGasTubeComplete.aspx",
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

                            location.href = "GasTubeComplete.aspx?cp=" + $.getQueryString("cp");
                        }
                    }
                });
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
                url: "../Handler/GetGasTubeComplete.aspx",
                data: {
                    guid: $.getQueryString("guid"),
                    type: "data",
                    no: "2",
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
                                $("#txt2_1").val(splitYearMonth(0, $(this).children("風險評估年月").text().trim()));
                                $("#txt2_2").val(splitYearMonth(1, $(this).children("風險評估年月").text().trim()));
                                $("#txt3").val($(this).children("智慧型通管器ILI可行性").text().trim());
                                $("#txt4").val($(this).children("耐壓強度試驗TP可行性").text().trim());
                                if ($(this).children("緊密電位CIPS年月").text().trim() == 'NA') {
                                    $("input[name='rdYeae1'][value='N']").attr('checked', true);
                                    $("#txt5_1").attr("disabled", true);
                                    $("#txt5_2").attr("disabled", true);
                                }
                                else {
                                    $("input[name='rdYeae1'][value='Y']").attr('checked', true);
                                    $("#txt5_1").val(splitYearMonth(0, $(this).children("緊密電位CIPS年月").text().trim()));
                                    $("#txt5_2").val(splitYearMonth(1, $(this).children("緊密電位CIPS年月").text().trim()));
                                }
                                if ($(this).children("電磁包覆PCM年月").text().trim() == 'NA') {
                                    $("input[name='rdYeae2'][value='N']").attr('checked', true);
                                    $("#txt6_1").attr("disabled", true);
                                    $("#txt6_2").attr("disabled", true);
                                }
                                else {
                                    $("input[name='rdYeae2'][value='Y']").attr('checked', true);
                                    $("#txt6_1").val(splitYearMonth(0, $(this).children("電磁包覆PCM年月").text().trim()));
                                    $("#txt6_2").val(splitYearMonth(1, $(this).children("電磁包覆PCM年月").text().trim()));
                                }
                                if ($(this).children("智慧型通管器ILI年月").text().trim() == 'NA') {
                                    $("input[name='rdYeae3'][value='N']").attr('checked', true);
                                    $("#txt7_1").attr("disabled", true);
                                    $("#txt7_2").attr("disabled", true);
                                }
                                else {
                                    $("input[name='rdYeae3'][value='Y']").attr('checked', true);
                                    $("#txt7_1").val(splitYearMonth(0, $(this).children("智慧型通管器ILI年月").text().trim()));
                                    $("#txt7_2").val(splitYearMonth(1, $(this).children("智慧型通管器ILI年月").text().trim()));
                                }
                                if ($(this).children("耐壓強度試驗TP年月").text().trim() == 'NA') {
                                    $("input[name='rdYeae4'][value='N']").attr('checked', true);
                                    $("#txt8_1").attr("disabled", true);
                                    $("#txt8_2").attr("disabled", true);
                                }
                                else {
                                    $("input[name='rdYeae4'][value='Y']").attr('checked', true);
                                    $("#txt8_1").val(splitYearMonth(0, $(this).children("耐壓強度試驗TP年月").text().trim()));
                                    $("#txt8_2").val(splitYearMonth(1, $(this).children("耐壓強度試驗TP年月").text().trim()));
                                }
                                $("#txt9").val($(this).children("耐壓強度試驗TP介質").text().trim());
                                $("#txt10").val($(this).children("試壓壓力與MOP壓力倍數").text().trim());
                                $("#txt11").val($(this).children("耐壓強度試驗TP持壓時間").text().trim());
                                $("#txt12").val($(this).children("受雜散電流影響").text().trim());
                                $("#txt13").val($(this).children("洩漏偵測系統LLDS").text().trim());
                                $("#txt14").val($(this).children("強化作為").text().trim());
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
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">風險評估年/月</div>
                                        <div class="OchiCell width100">
                                            民國 <input type="number" min="1" id="txt2_1" placeholder="請填寫民國年" class="inputex width40"> 年 
                                            <select id="txt2_2" class="width25 inputex" >
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
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">智慧型通管器(ILI)</br>可行性</div>
                                        <div class="OchiCell width100">
                                             <select id="txt3" class="width100 inputex" >
                                                <option value="">請選擇</option>
                                                <option value="可">可</option>
                                                <option value="無法">無法</option>
                                            </select>
                                        </div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">耐壓強度試驗(TP)</br>可行性</div>
                                        <div class="OchiCell width100">
                                            <select id="txt4" class="width100 inputex" >
                                                <option value="">請選擇</option>
                                                <option value="可">可</option>
                                                <option value="無法">無法</option>
                                            </select>
                                        </div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">緊密電位(CIPS)年/月</div>
                                        <div class="OchiCell width100">
                                            <input type="radio" name="rdYeae1" value="N" /> NA<br />  
                                            <input type="radio" name="rdYeae1" value="Y" /> 
                                            民國 <input type="number" min="1" id="txt5_1" placeholder="請填寫民國年" class="inputex width40"> 年 
                                            <select id="txt5_2" class="width25 inputex" >
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
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">電磁包覆(PCM)年/月</div>
                                        <div class="OchiCell width100">
                                            <input type="radio" name="rdYeae2" value="N" /> NA<br />  
                                            <input type="radio" name="rdYeae2" value="Y" /> 
                                            民國 <input type="number" min="1" id="txt6_1" placeholder="請填寫民國年" class="inputex width40"> 年 
                                            <select id="txt6_2" class="width25 inputex" >
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
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">智慧型通管器(ILI)</br>年/月</div>
                                        <div class="OchiCell width100">
                                            <input type="radio" name="rdYeae3" value="N" /> NA<br />  
                                            <input type="radio" name="rdYeae3" value="Y" /> 
                                            民國 <input type="number" min="1" id="txt7_1" placeholder="請填寫民國年" class="inputex width40"> 年 
                                            <select id="txt7_2" class="width25 inputex" >
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
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">耐壓強度試驗(TP)</br>年/月</div>
                                        <div class="OchiCell width100">
                                            <input type="radio" name="rdYeae4" value="N" /> NA<br />  
                                            <input type="radio" name="rdYeae4" value="Y" /> 
                                            民國 <input type="number" min="1" id="txt8_1" placeholder="請填寫民國年" class="inputex width40"> 年 
                                            <select id="txt8_2" class="width25 inputex" >
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
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">耐壓強度試驗(TP)介質</div>
                                        <div class="OchiCell width100"><input type="text" id="txt9" class="inputex width100"></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">試壓壓力與MOP</br>壓力倍數</div>
                                        <div class="OchiCell width100"><input type="text" id="txt10" class="inputex width100"></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">耐壓強度試驗(TP)</br>持壓時間(小時)</div>
                                        <div class="OchiCell width100"><input min="0" type="number" id="txt11" class="inputex width40"></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">受雜散電流影響</div>
                                        <div class="OchiCell width100">
                                            <select id="txt12" class="width100 inputex" >
                                                <option value="">請選擇</option>
                                                <option value="有">有</option>
                                                <option value="無">無</option>
                                            </select>
                                        </div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">洩漏偵測系統(LLDS)</div>
                                        <div class="OchiCell width100">
                                            <select id="txt13" class="width100 inputex" >
                                                <option value="">請選擇</option>
                                                <option value="有">有</option>
                                                <option value="無">無</option>
                                            </select>
                                        </div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">強化作為</div>
                                        <div class="OchiCell width100"><input type="text" id="txt14" class="inputex width100"></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                            </div><!-- OchiTrasTable -->
                            </br>
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



