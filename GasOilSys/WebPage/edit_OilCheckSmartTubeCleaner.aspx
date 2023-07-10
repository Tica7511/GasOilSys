<%@ Page Language="C#" AutoEventWireup="true" CodeFile="edit_OilCheckSmartTubeCleaner.aspx.cs" Inherits="WebPage_edit_OilCheckSmartTubeCleaner" %>

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

            //取消按鍵
            $(document).on("click", "#cancelbtn", function () {
                var str = confirm('尚未儲存的部分將不會更改，確定返回嗎?');

                if (str)
                    location.href = "OilCheckSmartTubeCleaner.aspx?cp=" + $.getQueryString("cp");
            });

            //儲存按鍵
            $(document).on("click", "#subbtn", function () {
                var msg = '';

                if ($("#txt1").val() == '')
                    msg += "請選擇【長途管線識別碼】\n";
                //if ($("#txt2").val() == '')
                //    msg += "請選擇【檢測方法】\n";
                //if (($("#txt3_1").val()) == '' || ($("#txt3_2").val() == ''))
                //    msg += "請輸入【最近一次執行 年/月】\n";
                //if (($("#txt4_1").val()) == '' || ($("#txt4_2").val() == ''))
                //    msg += "請輸入【報告產出 年/月】\n";
                //if ($("#txt5").val() == '')
                //    msg += "請選擇【檢測長度】\n";
                //if ($("#txt6").val() == '')
                //    msg += "請選擇【管壁減薄30%-40%數量 內部腐蝕數量】\n";
                //if ($("#txt7").val() == '')
                //    msg += "請選擇【管壁減薄30%-40%數量 內部開挖確認數量】\n";
                //if ($("#txt8").val() == '')
                //    msg += "請選擇【管壁減薄30%-40%數量 外部腐蝕數量】\n";
                //if ($("#txt9").val() == '')
                //    msg += "請選擇【管壁減薄30%-40%數量 外部開挖確認數量】\n";
                //if ($("#txt10").val() == '')
                //    msg += "請選擇【管壁減薄40%-50%數量 內部腐蝕數量】\n";
                //if ($("#txt11").val() == '')
                //    msg += "請選擇【管壁減薄40%-50%數量 內部開挖確認數量】\n";
                //if ($("#txt12").val() == '')
                //    msg += "請選擇【管壁減薄40%-50%數量 外部腐蝕數量】\n";
                //if ($("#txt13").val() == '')
                //    msg += "請選擇【管壁減薄40%-50%數量 外部開挖確認數量】\n";
                //if ($("#txt14").val() == '')
                //    msg += "請選擇【管壁減薄50%以上數量 內部腐蝕數量】\n";
                //if ($("#txt15").val() == '')
                //    msg += "請選擇【管壁減薄50%以上數量 內部開挖確認數量】\n";
                //if ($("#txt16").val() == '')
                //    msg += "請選擇【管壁減薄50%以上數量 外部腐蝕數量】\n";
                //if ($("#txt17").val() == '')
                //    msg += "請選擇【管壁減薄50%以上數量 外部開挖確認數量】\n";
                //if ($("#txt18").val() == '')
                //    msg += "請選擇【Dent 變形量>12%數量】\n";
                //if ($("#txt19").val() == '')
                //    msg += "請選擇【Dent 開挖確認數量】\n";

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
                data.append("txt15", encodeURIComponent($("#txt15").val()));
                data.append("txt16", encodeURIComponent($("#txt16").val()));
                data.append("txt17", encodeURIComponent($("#txt17").val()));
                data.append("txt18", encodeURIComponent($("#txt18").val()));
                data.append("txt19", encodeURIComponent($("#txt19").val()));
                data.append("txt20", encodeURIComponent($("#txt20").val()));
                data.append("txt21", encodeURIComponent($("#txt21").val()));

                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "../handler/AddOilCheckSmartTubeCleaner.aspx",
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

                            location.href = "OilCheckSmartTubeCleaner.aspx?cp=" + $.getQueryString("cp");
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
                url: "../Handler/GetOilCheckSmartTubeCleaner.aspx",
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
                                $("#txt1").val($(this).children("長途管線識別碼").text().trim());
                                $("#txt2").val($(this).children("檢測方法").text().trim());
                                $("#txt3_1").val(splitYearMonth(0, $(this).children("最近一次執行年月").text().trim()));
                                $("#txt3_2").val(splitYearMonth(1, $(this).children("最近一次執行年月").text().trim()));
                                $("#txt4_1").val(splitYearMonth(0, $(this).children("報告產出年月").text().trim()));
                                $("#txt4_2").val(splitYearMonth(1, $(this).children("報告產出年月").text().trim()));
                                $("#txt5").val($(this).children("檢測長度公里").text().trim());
                                $("#txt6").val($(this).children("減薄數量_內1").text().trim());
                                $("#txt7").val($(this).children("減薄數量_內_開挖確認1").text().trim());
                                $("#txt8").val($(this).children("減薄數量_外").text().trim());
                                $("#txt9").val($(this).children("減薄數量_外_開挖確認1").text().trim());
                                $("#txt10").val($(this).children("減薄數量_內2").text().trim());
                                $("#txt11").val($(this).children("減薄數量_內_開挖確認2").text().trim());
                                $("#txt12").val($(this).children("減薄數量_外2").text().trim());
                                $("#txt13").val($(this).children("減薄數量_外_開挖確認2").text().trim());
                                $("#txt14").val($(this).children("減薄數量_內3").text().trim());
                                $("#txt15").val($(this).children("減薄數量_內_開挖確認3").text().trim());
                                $("#txt16").val($(this).children("減薄數量_外3").text().trim());
                                $("#txt17").val($(this).children("減薄數量_外_開挖確認3").text().trim());
                                $("#txt18").val($(this).children("Dent").text().trim());
                                $("#txt19").val($(this).children("Dent_開挖確認").text().trim());
                                $("#txt20").val($(this).children("備註").text().trim());
                                $("#txt21").val($(this).children("外部腐蝕保護電位符合標準要求數量").text().trim());
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
                url: "../handler/GetOilTubeInfo.aspx",
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
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">檢測方法</div>
                                        <div class="OchiCell width100">
                                            <select id="txt2" class="width100 inputex" >
                                                <option value="">請選擇</option>
                                                <option value="MFL">MFL</option>
                                                <option value="UT">UT</option>
                                            </select>
                                        </div>
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
                                </br>
                                <div class="OchiRow">
                                    <div class="margin5TB font-size4 font-bold" style="text-align:center">管壁減薄30%-40%數量</div>
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">內部腐蝕數量</div>
                                        <div class="OchiCell width100"><input type="number" min="0" id="txt6" class="inputex width40"></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">內部開挖確認數量</div>
                                        <div class="OchiCell width100"><input type="number" min="0" id="txt7" class="inputex width40"></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">外部腐蝕數量</div>
                                        <div class="OchiCell width100"><input type="number" min="0" id="txt8" class="inputex width40"></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">外部開挖確認數量</div>
                                        <div class="OchiCell width100"><input type="number" min="0" id="txt9" class="inputex width40"></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                </br>
                                <div class="OchiRow">
                                    <div class="margin5TB font-size4 font-bold" style="text-align:center">管壁減薄40%-50%數量</div>
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">內部腐蝕數量</div>
                                        <div class="OchiCell width100"><input type="number" min="0" id="txt10" class="inputex width40"></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">內部開挖確認數量</div>
                                        <div class="OchiCell width100"><input type="number" min="0" id="txt11" class="inputex width40"></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">外部腐蝕數量</div>
                                        <div class="OchiCell width100"><input type="number" min="0" id="txt12" class="inputex width40"></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">外部開挖確認數量</div>
                                        <div class="OchiCell width100"><input type="number" min="0" id="txt13" class="inputex width40"></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                </br>
                                <div class="OchiRow">
                                    <div class="margin5TB font-size4 font-bold" style="text-align:center">管壁減薄50%以上數量</div>
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">內部腐蝕數量</div>
                                        <div class="OchiCell width100"><input type="number" min="0" id="txt14" class="inputex width40"></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">內部開挖確認數量</div>
                                        <div class="OchiCell width100"><input type="number" min="0" id="txt15" class="inputex width40"></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">外部腐蝕數量</div>
                                        <div class="OchiCell width100"><input type="number" min="0" id="txt16" class="inputex width40"></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">外部開挖確認數量</div>
                                        <div class="OchiCell width100"><input type="number" min="0" id="txt17" class="inputex width40"></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                </br>
                                <div class="OchiRow">
                                    <div class="margin5TB font-size4 font-bold" style="text-align:center">Dent</div>
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">變形量>12%數量</div>
                                        <div class="OchiCell width100"><input type="number" min="0" id="txt18" class="inputex width40"></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">開挖確認數量</div>
                                        <div class="OchiCell width100"><input type="number" min="0" id="txt19" class="inputex width40"></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                </br>
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">外部腐蝕保護電位不符合標準要求數量</div>
                                        <div class="OchiCell width100"><input type="number" min="0" id="txt21" class="inputex width40"></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">備註</div>
                                        <div class="OchiCell width100"><input type="text" id="txt20" class="inputex width100"></div>
                                    </div><!-- OchiHalf -->
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




