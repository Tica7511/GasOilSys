<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OilCathodicProtection.aspx.cs" Inherits="WebPage_OilCathodicProtection" %>

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
            getYearList();
            $("#sellist").val(getTaiwanDate());
            getData(getTaiwanDate());

            //選擇年份
            $(document).on("change", "#sellist", function () {
                getData($("#sellist option:selected").val());
            });

            //新增按鈕
            $(document).on("click", "#newbtn", function () {
                location.href = "edit_OilCathodicProtection.aspx?cp=" + $.getQueryString("cp");
            });

            //編輯按鈕
            $(document).on("click", "#editbtn", function () {
                $("#mode").val("edit");
                $("#editbtn").hide();
                $("#cancelbtn").show();
                $("#subbtn").show();
                $("#PMPeriod").attr("disabled", false);
                $("#PSMPeriod").attr("disabled", false);
                $("input[name='checkUnit']").attr("disabled", false);
                $("#sellist").attr("disabled", true);

            });

            //返回按鈕
            $(document).on("click", "#cancelbtn", function () {
                var str = confirm('尚未儲存的部分將不會更改，確定返回嗎?');

                if (str) {
                    $("#mode").val("noedit");
                    $("#editbtn").show();
                    $("#cancelbtn").hide();
                    $("#subbtn").hide();
                    $("#PMPeriod").attr("disabled", true);
                    $("#PSMPeriod").attr("disabled", true);
                    $("input[name='checkUnit']").attr("disabled", true);
                    $("#sellist").attr("disabled", false);

                    location.href = "OilCathodicProtection.aspx?cp=" + $.getQueryString("cp");
                }
            });

            //儲存按鍵
            $(document).on("click", "#subbtn", function () {
                var msg = '';

                if ($("#PMPeriod").val() == '')
                    msg += "請輸入【儲槽陰極防蝕系統電位量測週期】\n";
                if ($("#PSMPeriod").val() == '')
                    msg += "請輸入【整流站量測週期】\n";
                if ($("input[name='checkUnit']").val() == '')
                    msg += "請選擇【儲槽陰極防蝕系統電位量測單位】\n";

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
                data.append("PMPeriod", encodeURIComponent($("#PMPeriod").val()));
                data.append("PSMPeriod", encodeURIComponent($("#PSMPeriod").val()));

                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "../handler/AddOilCathodicProtection.aspx",
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

            //刪除按鈕
            $(document).on("click", "a[name='delbtn']", function () {
                if (confirm("確定刪除?")) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../handler/DelOilCathodicProtection.aspx",
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
		}); // end js

        function getData(year) {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetOilCathodicProtection.aspx",
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
                                $("#PMPeriod").val($(this).children("電位量測週期").text().trim());
                                $("#PSMPeriod").val($(this).children("整流站量測週期").text().trim());
                                // 電位量測單位
                                var arychecktool = $(this).children("電位量測單位").text().trim().split(',');
                                $("input[name='checkUnit']").prop("checked", false);
                                $.each(arychecktool, function (key, value) {
                                    $("input[name='checkUnit'][value='" + value + "']").prop("checked", true);
                                });
                            });
                        }
                        else {
                            $("#PMPeriod").val('');
                            $("#PSMPeriod").val('');
                            $("input[name='checkUnit']").prop("checked", false);
                        }

						$("#tablist tbody").empty();
						var tabstr = '';
						if ($(data).find("data_item2").length > 0) {
							$(data).find("data_item2").each(function (i) {
								tabstr += '<tr>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("轄區儲槽編號").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("設置").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("整流站名稱").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("合格標準").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("整流站狀態").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("系統狀態").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("設置長效型參考電極種類").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("測試點數量").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("陽極地床種類").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("備註").text().trim() + '</td>';
                                tabstr += '<td name="td_edit" nowrap="" align="center"><a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("guid").text().trim() + '">刪除</a>';
                                tabstr += ' <a href="edit_OilCathodicProtection.aspx?cp=' + $.getQueryString("cp") + '&guid=' + $(this).children("guid").text().trim() + '" name="editbtn">編輯</a></td>';
                                tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="11">查詢無資料</td></tr>';
                        $("#tablist tbody").append(tabstr);

                        //確認權限&按鈕顯示或隱藏
                        if ($("#sellist").val() != getTaiwanDate()) {
                            $("#editbtn").hide();
                            $("#newbtn").hide();
                            $("#th_edit").hide();
                            $("td[name='td_edit']").hide();
                        }
                        else {
                            if (($("#Competence").val() == '01') || ($("#Competence").val() == '04') || ($("#Competence").val() == '05') || ($("#Competence").val() == '06')) {
                                $("#newbtn").hide();
                                $("#editbtn").hide();
                                $("#th_edit").hide();
                                $("td[name='td_edit']").hide();
                            }
                            else {
                                $("#newbtn").show();
                                $("#editbtn").show();
                                $("#th_edit").show();
                                $("td[name='td_edit']").show();
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
                                        $("#newbtn").hide();
                                        $("#editbtn").hide();
                                        $("#th_edit").hide();
                                        $("td[name='td_edit']").hide();
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
                url: "../Handler/GetOilCathodicProtection.aspx",
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
                        if ($(data).find("data_item3").length > 0) {
                            $(data).find("data_item3").each(function (i) {
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
        <input type="hidden" id="Competence" value="<%= competence %>" />
        <input type="hidden" id="mode" />
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
                                <div class="left font-size5 "><i class="fa fa-chevron-circle-right IconCa" aria-hidden="true"></i> 
                                    <select id="sellist" class="inputex">
                                    </select> 年
                                </div>
                                <div class="right">
                                    <a id="editbtn" href="javascript:void(0);" title="編輯" class="genbtn">編輯</a>
                                    <a id="cancelbtn" href="javascript:void(0);" title="返回" class="genbtn" style="display:none">返回</a>
                                    <a id="subbtn" href="javascript:void(0);" title="儲存" class="genbtn" style="display:none">儲存</a>
                                </div>
                            </div><br />
                            <div class="font-size3 lineheight03">
                                1.儲槽陰極防蝕系統電位量測週期：
                                <input type="number" min="0" id="PMPeriod" class="inputex width10" value="" disabled> 次/年<br>
                                2.整流站量測週期：
                                <input type="number" min="0" id="PSMPeriod" class="inputex width10" value="" disabled> 次/年<br>
                                3.儲槽陰極防蝕系統電位量測單位：
                                <input type="checkbox" name="checkUnit" value="01" disabled> 公司員工；
                                <input type="checkbox" name="checkUnit" value="02" disabled > 委外辦理
                            </div><br />

                            <div class="twocol">
                                <div class="right">
                                <a id="newbtn" href="javascript:void(0);" title="新增" class="genbtn">新增</a>
                                </div>
                            </div><br />
                            <div class="stripeMeB tbover">
                                <table id="tablist" width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th nowrap rowspan="3">轄區儲槽編號 </th>
                                            <th nowrap colspan="9">陰極防蝕系統 </th>
                                            <th id="th_edit" nowrap rowspan="3">功能 </th>
                                        </tr>
                                        <tr>
                                            <th nowrap valign="top">設置 </th>
                                            <th nowrap valign="top">整流站 <br>名稱 </th>
                                            <th nowrap valign="top">合格標準 </th>
                                            <th nowrap valign="top">整流站 <br>狀態 </th>
                                            <th nowrap valign="top">系統 <br>狀態 </th>
                                            <th nowrap valign="top">設置長效型參考電極種類 </th>
                                            <th nowrap valign="top">測試點數量 </th>
                                            <th nowrap valign="top">陽極地床種類 </th>
                                            <th nowrap valign="top">備註 </th>

                                        </tr>
                                        <tr>
                                            <th nowrap>1.有 <br>
                                                2.無 </th>
                                            <th nowrap valign="top">&nbsp;</th>
                                            <th nowrap>請參照 <br>
                                                填表說明(2) </th>
                                            <th nowrap>1.正常 <br>
                                                2.異常 </th>
                                            <th nowrap>1.正常 <br>
                                                2.異常 </th>
                                            <th nowrap>1.鋅 <br>
                                                2.飽和硫酸銅 <br>
                                                3.無 </th>
                                            <th nowrap>&nbsp;</th>
                                            <th nowrap>1.深井 <br>
                                                2.淺井 </th>
                                            <th nowrap valign="top">&nbsp;</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div><!-- stripeMe -->

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

