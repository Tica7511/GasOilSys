﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="edit_OilCathodicProtection.aspx.cs" Inherits="WebPage_edit_OilCathodicProtection" %>

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
    <title>石油業輸儲設備查核及檢測資訊系統</title>
    <!--#include file="Head_Include.html"-->
    <script type="text/javascript">
        $(document).ready(function () {
            getDDL(getTaiwanDate());
            getData();

            $(document).on("change", "#txt2", function () {
                if ($("#txt2").val() == '有') {
                    $("#txt3").attr("disabled", false);
                }
                else {
                    $("#txt3").attr("disabled", true);
                    $("#txt3").val("");
                }
            });

            //取消按鍵
            $(document).on("click", "#cancelbtn", function () {
                var str = confirm('尚未儲存的部分將不會更改，確定返回嗎?');

                if (str)
                    location.href = "OilCathodicProtection.aspx?cp=" + $.getQueryString("cp");
            });

            //儲存按鍵
            $(document).on("click", "#subbtn", function () {
                var msg = '';

                if ($("#txt1").val() == '')
                    msg += "請選擇【轄區儲槽編號】\n";
                if ($("#txt2").val() == '')
                    msg += "請選擇【設置】\n";
                if ($("#txt2").val() == '有')
                    if ($("#txt3").val() == '')
                        msg += "請選擇【整流站名稱】\n";
                if ($("#txt4").val() == '')
                    msg += "請輸入【合格標準】\n";
                if ($("#txt5").val() == '')
                    msg += "請輸入【整流站狀態】\n";
                if ($("#txt6").val() == '')
                    msg += "請選擇【系統狀態】\n";
                if ($("#txt7").val() == '')
                    msg += "請選擇【設置長效型參考電極種類】\n";
                if ($("#txt8").val() == '')
                    msg += "請選擇【測試點數量】\n";
                if ($("#txt9").val() == '')
                    msg += "請選擇【陽極地床種類】\n";

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

                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "../handler/AddOilCathodicProtectionList.aspx",
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

                            location.href = "OilCathodicProtection.aspx?cp=" + $.getQueryString("cp");
                        }
                    }
                });
            });

            $(".pickDate").datepick({
                dateFormat: 'yymmdd',
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: '../images/calendar.gif',
                yearRange: 'c-6:c+6'
            }).BootStrap(); //BootStrap() 產生符合 BootStrap 的樣式內容
		}); // end js

		function getData() {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetOilCathodicProtection.aspx",
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
                                $("#txt1").val($(this).children("轄區儲槽編號").text().trim());
                                $("#txt2").val($(this).children("設置").text().trim());
                                $("#txt3").val($(this).children("整流站名稱").text().trim());
                                if (($("#txt2").val() != '') && ($("#txt2").val() != '無'))
                                    $("#txt3").attr("disabled", false);
                                else
                                    $("#txt3").attr("disabled", true);
                                $("#txt4").val($(this).children("合格標準").text().trim());
                                $("#txt5").val($(this).children("整流站狀態").text().trim());
                                $("#txt6").val($(this).children("系統狀態").text().trim());
                                $("#txt7").val($(this).children("設置長效型參考電極種類").text().trim());
                                $("#txt8").val($(this).children("測試點數量").text().trim());
                                $("#txt9").val($(this).children("陽極地床種類").text().trim());
                                $("#txt10").val($(this).children("備註").text().trim());
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
                url: "../handler/GetOilStorageTankInfo.aspx",
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

        //function getTaiwanDate() {
        //    var nowDate = new Date();

        //    var nowYear = nowDate.getFullYear();
        //    var nowTwYear = (nowYear - 1911);

        //    var ddlstr = '<option value="">請選擇</option>';

        //    for (var i = 10; i >= 0; i--) {
        //        ddlstr += '<option value="' + (nowTwYear - i).toString() + '">' + (nowTwYear - i).toString() + '</option>';
        //    }

        //    for (var j = 1; j <= 10; j++) {
        //        ddlstr += '<option value="' + (nowTwYear + j).toString() + '">' + (nowTwYear + j).toString() + '</option>';
        //    }

        //    $("#sellist").empty();
        //    $("#sellist").append(ddlstr);
        //}
    </script>
</head>
<body class="bgB">
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
        <!--#include file="OilHeader.html"-->
        <div id="ContentWrapper">
            <div class="container margin15T">
                <div class="padding10ALL">
                    <div class="filetitlewrapper"><!--#include file="OilBreadTitle.html"--></div>

                    <div class="row margin20T">
                        <div class="col-lg-3 col-md-4 col-sm-5">
                            <div id="navmenuV"><!--#include file="OilLeftMenu.html"--></div>
                        </div>
                        <div class="col-lg-9 col-md-8 col-sm-7">
                            <div class="twocol">
                                <div class="left font-size4" style="width:50%">
                                    <i class="fa fa-chevron-circle-right IconCa" aria-hidden="true"></i> 
                                    轄區儲槽編號 : <select id="txt1" class="width40 inputex" ></select>
                                </div>
                                <div class="right">
                                    <a id="cancelbtn" href="javascript:void(0);" title="返回" class="genbtn" >取消</a>
                                    <a id="subbtn" href="javascript:void(0);" title="儲存" class="genbtn" >儲存</a>
                                </div>
                            </div><br />
                            <div class="OchiTrasTable width100 TitleLength09 font-size3">
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">設置</div>
                                        <div class="OchiCell width100">
                                            <select id="txt2" class="width100 inputex" >
                                                <option value="">請選擇</option>
                                                <option value="有">1.有</option>
                                                <option value="無">2.無</option>
                                            </select>
                                        </div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">整流站名稱</div>
                                        <div class="OchiCell width100"><input type="text" id="txt3" class="inputex width100"></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">合格標準</div>
                                        <div class="OchiCell width100"><input type="text" id="txt4" class="inputex width100 "></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">整流站狀態</div>
                                        <div class="OchiCell width100">
                                            <select id="txt5" class="width100 inputex" >
                                                <option value="">請選擇</option>
                                                <option value="正常">1.正常</option>
                                                <option value="異常">2.異常</option>
                                            </select>
                                        </div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">系統狀態</div>
                                        <div class="OchiCell width100">
                                            <select id="txt6" class="width100 inputex" >
                                                <option value="">請選擇</option>
                                                <option value="正常">1.正常</option>
                                                <option value="異常">2.異常</option>
                                            </select>
                                        </div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">設置長效型參考電極種類</div>
                                        <div class="OchiCell width100">
                                            <select id="txt7" class="width100 inputex" >
                                                <option value="">請選擇</option>
                                                <option value="鋅">1.鋅</option>
                                                <option value="飽和硫酸銅">2.飽和硫酸銅</option>
                                                <option value="無">3.無</option>
                                            </select>
                                        </div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">測試點數量</div>
                                        <div class="OchiCell width100"><input type="number" id="txt8" min="0" class="inputex width40 "></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">陽極地床種類</div>
                                        <div class="OchiCell width100">
                                            <select id="txt9" class="width100 inputex" >
                                                <option value="">請選擇</option>
                                                <option value="深井">1.深井</option>
                                                <option value="淺井">2.淺井</option>
                                            </select>
                                        </div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">備註</div>
                                    <div class="OchiCell width100"><textarea id="txt10" rows="5" cols="" class="inputex width100 "></textarea></div>
                                </div><!-- OchiRow -->
                            </div><!-- OchiTrasTable -->
                            </br>
                            <div class="margin5TB font-size2">
                                (1) 若「設置」欄位填寫1，請於「整流站名稱」填寫負責該儲槽陰極防蝕系統之整流站名稱。<br>
                                (2) 合格標準：請依據該管線檢測報告判定結果時，所引用之標準，請填入相對應之數字，<br>
                                1. 通電電位< -850mVCSE  2.極化電位< -850mVCSE  3.極化量>100mV 4.其他<br>
                                (3) 設置長效型性參考電極種類：若有設置長效型參考電極，請依種類選填1或2；若無設置長效型參考電極，請填3。<br>
                                (4) 陽極地床種類：請依實際設置情形，選填1或2。
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
<div id="sidebar-wrapper">
   
</div><!-- sidebar-wrapper -->

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

<!-- 本頁面使用的JS -->
    <script type="text/javascript">
        $(document).ready(function(){
        
        });
    </script>
    <script type="text/javascript" src="../js/GenCommon.js"></script><!-- UIcolor JS -->
    <script type="text/javascript" src="../js/PageCommon.js"></script><!-- 系統共用 JS -->
    <script type="text/javascript" src="../js/MenuOil.js"></script><!-- 系統共用 JS -->
    <script type="text/javascript" src="../js/SubMenuOilA.js"></script><!-- 內頁選單 -->
    <script type="text/javascript" src="../js/autoHeight.js"></script><!-- 高度不足頁面的絕對置底footer -->
</body>
</html>



