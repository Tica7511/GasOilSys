﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="edit_GasTubeInfo.aspx.cs" Inherits="WebPage_edit_GasTubeInfo" %>

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

            //長途管線識別碼 認證有無重複序號
            $(document).on("blur", "#txt1", function () {
                var Sno = $("#Sno").val();

                if ($.getQueryString("guid") != '') {
                    if ($("#txt1").val() != '')
                        if (Sno != $("#txt1").val())
                            compareSno();
                }
                else {
                    if ($("#txt1").val() != '')
                        compareSno();
                }
            });

            //取消按鍵
            $(document).on("click", "#cancelbtn", function () {
                var str = confirm('尚未儲存的部分將不會更改，確定返回嗎?');

                if (str)
                    location.href = "GasTubeInfo.aspx?cp=" + $.getQueryString("cp");
            });

            //儲存按鍵
            $(document).on("click", "#subbtn", function () {
                var msg = '';

                if ($("#txt1").val() == '')
                    msg += "請選擇【長途管線識別碼】\n";
                if ($("#txt2").val() == '')
                    msg += "請選擇【轄區長途管線名稱(公司)】\n";
                //if ($("#txt3").val() == '')
                //    msg += "請輸入【銜接管線識別碼(上游)】\n";
                //if ($("#txt4").val() == '')
                //    msg += "請輸入【銜接管線識別碼(下游)】\n";
                //if ($("#txt5").val() == '')
                //    msg += "請選擇【起點】\n";
                //if ($("#txt6").val() == '')
                //    msg += "請選擇【迄點】\n";
                //if ($("#txt7").val() == '')
                //    msg += "請選擇【管徑】\n";
                //if ($("#txt8").val() == '')
                //    msg += "請選擇【厚度】\n";
                //if ($("#txt9").val() == '')
                //    msg += "請選擇【管材】\n";
                //if ($("#txt10").val() == '')
                //    msg += "請選擇【包覆材料】\n";
                //if ($("#txt11").val() == '')
                //    msg += "請選擇【轄管長度】\n";
                //if ($("#txt12").val() == '')
                //    msg += "請選擇【內容物】\n";
                //if ($("#txt13").val() == '')
                //    msg += "請選擇【緊急遮斷閥】\n";
                //if ($("#txt14").val() == '')
                //    msg += "請選擇【建置年】\n";
                //if ($("#txt15").val() == '')
                //    msg += "請選擇【設計壓力】\n";
                //if ($("#txt16").val() == '')
                //    msg += "請選擇【使用壓力】\n";
                //if ($("#txt17").val() == '')
                //    msg += "請選擇【使用狀態】\n";
                //if ($("#txt18").val() == '')
                //    msg += "請選擇【附掛橋樑數量】\n";
                //if ($("#txt19").val() == '')
                //    msg += "請選擇【活動斷層敏感區】\n";
                //if ($("#txt20").val() == '')
                //    msg += "請選擇【土壤液化區】\n";
                //if ($("#txt21").val() == '')
                //    msg += "請選擇【土石流潛勢區】\n";
                //if ($("#txt22").val() == '')
                //    msg += "請選擇【淹水潛勢區】\n";
                //if ($("#txt23").val() == '')
                //    msg += "請選擇【管線穿越箱涵數量】\n";

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
                data.append("txt3", encodeURIComponent($("#txt3").val()));
                data.append("txt4", encodeURIComponent($("#txt4").val()));
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
                data.append("txt15", encodeURIComponent($("#txt15").val()));
                data.append("txt16", encodeURIComponent($("#txt16").val()));
                data.append("txt17", encodeURIComponent($("#txt17").val()));
                data.append("txt18", encodeURIComponent($("#txt18").val()));
                data.append("txt19", encodeURIComponent($("#txt19").val()));
                data.append("txt20", encodeURIComponent($("#txt20").val()));
                data.append("txt21", encodeURIComponent($("#txt21").val()));
                data.append("txt22", encodeURIComponent($("#txt22").val()));
                data.append("txt23", encodeURIComponent($("#txt23").val()));
                data.append("txt24", encodeURIComponent($("#txt24").val()));

                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "../handler/AddGasTubeInfo.aspx",
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

                            location.href = "GasTubeInfo.aspx?cp=" + $.getQueryString("cp");
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

        function compareSno() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetGasTubeInfo.aspx",
                data: {
                    cpid: $.getQueryString("cp"),
                    year: getTaiwanDate(),
                    Sno: $("#txt1").val(),
                    type: "list"
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
                            alert('已有相同的長途管線識別碼，請重新輸入一次');
                            $("#txt1").val($("#Sno").val());
                            return false;
                        }
                    }
                }
            });

            return status;
        }

        function getData() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetGasTubeInfo.aspx",
                data: {
                    guid: $.getQueryString("guid"),
                    type: "data"
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
                                $("#Sno").val($(this).children("長途管線識別碼").text().trim());
                                $("#txt1").val($(this).children("長途管線識別碼").text().trim());
                                $("#txt2").val($(this).children("轄區長途管線名稱_公司").text().trim());
                                $("#txt3").val($(this).children("銜接管線識別碼_上游").text().trim());
                                $("#txt4").val($(this).children("銜接管線識別碼_下游").text().trim());
                                $("#txt5").val($(this).children("起點").text().trim());
                                $("#txt6").val($(this).children("迄點").text().trim());
                                $("#txt7").val($(this).children("管徑").text().trim());
                                $("#txt8").val($(this).children("厚度").text().trim());
                                $("#txt9").val($(this).children("管材").text().trim());
                                $("#txt10").val($(this).children("包覆材料").text().trim());
                                $("#txt11").val($(this).children("轄管長度").text().trim());
                                $("#txt12").val($(this).children("內容物").text().trim());
                                $("#txt13").val($(this).children("緊急遮斷閥").text().trim());
                                $("#txt14").val($(this).children("建置年").text().trim());
                                $("#txt15").val($(this).children("設計壓力").text().trim());
                                $("#txt16").val($(this).children("使用壓力").text().trim());
                                $("#txt17").val($(this).children("使用狀態").text().trim());
                                $("#txt18").val($(this).children("附掛橋樑數量").text().trim());
                                $("#txt19").val($(this).children("活動斷層敏感區").text().trim());
                                $("#txt20").val($(this).children("土壤液化區").text().trim());
                                $("#txt21").val($(this).children("土石流潛勢區").text().trim());
                                $("#txt22").val($(this).children("淹水潛勢區").text().trim());
                                $("#txt23").val($(this).children("管線穿越箱涵數量").text().trim());
                                $("#txt24").val($(this).children("備註").text().trim());
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
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                ddlstr += '<option value="' + $(this).children("轄區儲槽編號").text().trim() + '">' + $(this).children("轄區儲槽編號").text().trim() + '</option>';
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
                                <div class="right">
                                    <a id="cancelbtn" href="javascript:void(0);" title="返回" class="genbtn" >取消</a>
                                    <a id="subbtn" href="javascript:void(0);" title="儲存" class="genbtn" >儲存</a>
                                </div>
                            </div><br />
							<div class="OchiTrasTable width100 TitleLength09 font-size3">
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">長途管線識別碼</div>
                                        <div class="OchiCell width100"><input type="text" id="txt1" class="inputex width100"></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">轄區長途管線名稱(公司)</div>
                                        <div class="OchiCell width100"><input type="text" id="txt2" class="inputex width100"></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">銜接管線識別碼(上游)</div>
                                        <div class="OchiCell width100"><input type="text" id="txt3" class="inputex width100" ></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">銜接管線識別碼(下游)</div>
                                        <div class="OchiCell width100"><input type="text" id="txt4" class="inputex width100" ></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">起點</div>
                                        <div class="OchiCell width100"><input type="text" id="txt5" class="inputex width100" ></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">迄點</div>
                                        <div class="OchiCell width100"><input type="text" id="txt6" class="inputex width100" ></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">管徑(吋)</div>
                                        <div class="OchiCell width100"><input type="text" id="txt7" class="inputex width100" ></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">厚度(mm)</div>
                                        <div class="OchiCell width100"><input type="text" id="txt8" class="inputex width100" ></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">管材(詳細規格)</div>
                                        <div class="OchiCell width100"><input type="text" id="txt9" class="inputex width100" ></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">包覆材料</div>
                                        <div class="OchiCell width100"><input type="text" id="txt10" class="inputex width100" ></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">轄管長度(公里)</div>
                                        <div class="OchiCell width100"><input type="text" id="txt11" class="inputex width100" ></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">內容物</div>
                                        <div class="OchiCell width100"><input type="text" id="txt12" class="inputex width100" ></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">緊急遮斷閥(處)</div>
                                        <div class="OchiCell width100"><input type="text" id="txt13" class="inputex width100" ></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">建置年</div>
                                        <div class="OchiCell width100"><input type="text" id="txt14" class="inputex width100" ></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">設計壓力(Kg/cm2)</div>
                                        <div class="OchiCell width100"><input type="text" id="txt15" class="inputex width100" ></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">使用壓力(Kg/cm2)</div>
                                        <div class="OchiCell width100"><input type="text" id="txt16" class="inputex width100" ></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">使用狀態</div>
                                        <div class="OchiCell width100">
                                            <select id="txt17" class="width100 inputex" >
                                                <option value="">請選擇</option>
                                                <option value="使用中">使用中</option>
                                                <option value="停用">停用</option>
                                                <option value="備用">備用</option>
                                            </select>
                                        </div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">附掛橋樑數量</div>
                                        <div class="OchiCell width100"><input type="text" id="txt18" class="inputex width100" ></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">活動斷層敏感區</div>
                                        <div class="OchiCell width100">
                                            <select id="txt19" class="width100 inputex" >
                                                <option value="">請選擇</option>
                                                <option value="有">有</option>
                                                <option value="無">無</option>
                                            </select>
                                        </div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">土壤液化區</div>
                                        <div class="OchiCell width100">
                                            <select id="txt20" class="width100 inputex" >
                                                <option value="">請選擇</option>
                                                <option value="有">有</option>
                                                <option value="無">無</option>
                                            </select>
                                        </div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">土石流潛勢區</div>
                                        <div class="OchiCell width100">
                                            <select id="txt21" class="width100 inputex" >
                                                <option value="">請選擇</option>
                                                <option value="有">有</option>
                                                <option value="無">無</option>
                                            </select>
                                        </div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">淹水潛勢區</div>
                                        <div class="OchiCell width100">
                                            <select id="txt22" class="width100 inputex" >
                                                <option value="">請選擇</option>
                                                <option value="有">有</option>
                                                <option value="無">無</option>
                                            </select>
                                        </div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">管線穿越箱涵數量</div>
                                        <div class="OchiCell width100"><input type="number" min="0" id="txt23" class="inputex width40" ></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">備註</div>
                                    <div class="OchiCell width100"><textarea id="txt24" rows="5" cols="" class="inputex width100" ></textarea></div>
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

