<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OilControl.aspx.cs" Inherits="WebPage_OilControl" %>

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
            $(".pickDate").datepick({
                dateFormat: 'yymmdd',
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: '../images/calendar.gif',
                yearRange: 'c-60:c+10'
            }).BootStrap(); //BootStrap() 產生符合 BootStrap 的樣式內容

            $(".datepick-trigger").hide();

            getYearList();
            $("#sellist").val(getTaiwanDate());
            getData(getTaiwanDate());

            //選擇年份
            $(document).on("change", "#sellist", function () {
                getData($("#sellist option:selected").val());
            });

            //新增按鈕
            $(document).on("click", "#newbtn", function () {
                location.href = "edit_OilControl.aspx?cp=" + $.getQueryString("cp");
            });

            //編輯按鈕
            $(document).on("click", "#editbtn", function () {
                $("#mode").val("edit");
                $("#editbtn").hide();
                $("#cancelbtn").show();
                $("#subbtn").show();
                $("#sellist").attr("disabled", true);
                $(".datepick-trigger").show();
                setDisplayed(false);
            });

            //返回按鈕
            $(document).on("click", "#cancelbtn", function () {
                var str = confirm('尚未儲存的部分將不會更改，確定返回嗎?');

                if (str) {
                    location.href = "OilControl.aspx?cp=" + $.getQueryString("cp");
                }
            });

            //儲存按鍵
            $(document).on("click", "#subbtn", function () {
                var msg = '';

                if ($("#docName").val() == '')
                    msg += "請輸入【依據文件名稱】\n";
                if ($("#docNo").val() == '')
                    msg += "請輸入【文件編號】\n";
                if ($("#docDate").val() == '')
                    msg += "請輸入【文件日期】\n";
                if ($("#pressureHz").val() == '')
                    msg += "請輸入【壓力計校正頻率】\n";
                if ($("#pressureRecentTime").val() == '')
                    msg += "請輸入【最近一次校正時間】\n";
                if ($("#flowHz").val() == '')
                    msg += "請輸入【流量計校正頻率】\n";
                if ($("#flowRecentTime").val() == '')
                    msg += "請輸入【最近一次校正時間】\n";
                if ($("#monitorTime").val() == '')
                    msg += "請輸入【為使監控中心之時鐘、電腦系統、監視器時間一致，定期調整之週期】\n";
                if ($("#TotalOperator").val() == '')
                    msg += "請輸入【合格操作人員總數】\n";
                if (!$("input[name='rbShift']").is(":checked"))
                    msg += "請輸入【輪班制度】\n";
                if ($("#classPerson").val() == '')
                    msg += "請輸入【每班人數】\n";
                if (!$("input[name='rbClassTime']").is(":checked"))
                    msg += "請輸入【每班時數】\n";

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
                data.append("docName", encodeURIComponent($("#docName").val()));
                data.append("docNo", encodeURIComponent($("#docNo").val()));
                data.append("docDate", encodeURIComponent($("#docDate").val()));
                data.append("pressureHz", encodeURIComponent($("#pressureHz").val()));
                data.append("pressureRecentTime", encodeURIComponent($("#pressureRecentTime").val()));
                data.append("flowHz", encodeURIComponent($("#flowHz").val()));
                data.append("flowRecentTime", encodeURIComponent($("#flowRecentTime").val()));
                data.append("monitorTime", encodeURIComponent($("#monitorTime").val()));
                data.append("TotalOperator", encodeURIComponent($("#TotalOperator").val()));
                data.append("classPerson", encodeURIComponent($("#classPerson").val()));

                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "../handler/AddOilControl.aspx",
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

                            location.href = "OilControl.aspx?cp=" + $.getQueryString("cp");
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
                        url: "../handler/DelOilControl.aspx",
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

        function setDisplayed(status) {
            $("#docName").attr("disabled", status);
            $("#docNo").attr("disabled", status);
            $("#pressureHz").attr("disabled", status);
            $("#flowHz").attr("disabled", status);
            $("#monitorTime").attr("disabled", status);
            $("#TotalOperator").attr("disabled", status);
            $("input[name='rbShift']").attr("disabled", status);
            $("#classPerson").attr("disabled", status);
            $("input[name='rbClassTime']").attr("disabled", status);
        }

		function getData(year) {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetOilControl.aspx",
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
                                $("#docName").val($(this).children("依據文件名稱").text().trim());
                                $("#docNo").val($(this).children("文件編號").text().trim());
                                $("#docDate").val($(this).children("文件日期").text().trim());
                                $("#pressureHz").val($(this).children("壓力計校正頻率").text().trim());
                                $("#pressureRecentTime").val($(this).children("壓力計校正_最近一次校正時間").text().trim());
                                $("#flowHz").val($(this).children("流量計校正頻率").text().trim());
                                $("#flowRecentTime").val($(this).children("流量計校正_最近一次校正時間").text().trim());
                                $("#monitorTime").val($(this).children("監控中心定期調整之週期").text().trim());
                                $("#TotalOperator").val($(this).children("合格操作人員總數").text().trim());
                                $("input[name='rbShift'][value='" + $(this).children("輪班制度").text().trim() + "']").prop("checked", true);
                                $("#classPerson").val($(this).children("每班人數").text().trim());
                                $("input[name='rbClassTime'][value='" + $(this).children("每班時數").text().trim() + "']").prop("checked", true);
                            });
                        }
                        else {
                            $("#docName").val('');
                            $("#docNo").val('');
                            $("#docDate").val('');
                            $("#pressureHz").val('');
                            $("#pressureRecentTime").val('');
                            $("#flowHz").val('');
                            $("#flowRecentTime").val('');
                            $("#monitorTime").val('');
                            $("#TotalOperator").val('');
                            $("input[name='rbShift']").prop("checked", false);
                            $("#classPerson").val('');
                            $("input[name='rbClassTime']").prop("checked", false);
                        }

                        $("#tablist tbody").empty();
						var tabstr = '';
						if ($(data).find("data_item2").length > 0) {
							$(data).find("data_item2").each(function (i) {
								tabstr += '<tr>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("轄區儲槽編號").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("控制室名稱").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("液位監測方式").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("液位監測靈敏度").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("高液位警報設定基準").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("前一年度高液位警報發生頻率").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("液位異常下降警報設定基準").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("前一年度異常下降警報發生頻率").text().trim() + '</td>';
                                tabstr += '<td name="td_edit" nowrap="" align="center"><a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("guid").text().trim() + '">刪除</a>';
                                tabstr += ' <a href="edit_OilControl.aspx?cp=' + $.getQueryString("cp") + '&guid=' + $(this).children("guid").text().trim() + '" name="editbtn">編輯</a></td>';
                                tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="9">查詢無資料</td></tr>';
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
					}
				}
			});
        }

        //取得民國年份之下拉選單
        function getYearList() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetOilControl.aspx",
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
                            <div class="OchiTrasTable width100 TitleLength09 font-size3">

                                <div class="OchiRow">
                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">依據文件名稱</div>
                                    <div class="OchiCell width100"><input type="text"  id="docName" class="inputex width99" disabled></div>
                                </div><!-- OchiRow -->

                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">文件編號</div>
                                        <div class="OchiCell width100"><input type="text" id="docNo" class="inputex width100" disabled></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">文件日期</div>
                                        <div class="OchiCell width100"><input type="text" id="docDate" class="inputex width40 pickDate" disabled></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->

                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">壓力計校正頻率</div>
                                        <div class="OchiCell width100"><input type="number" min="1" id="pressureHz" class="inputex width80" disabled> 次/年</div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">最近一次校正時間</div>
                                        <div class="OchiCell width100"><input type="text" id="pressureRecentTime" class="inputex width40 pickDate" disabled></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->

                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">流量計校正頻率</div>
                                        <div class="OchiCell width100"><input type="number" min="0" id="flowHz" class="inputex width80" disabled> 次/年</div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">最近一次校正時間</div>
                                        <div class="OchiCell width100"><input type="text" id="flowRecentTime" class="inputex width40 pickDate" disabled></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->

                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">為使監控中心之時鐘、電腦系統、監視器時間一致，定期調整之週期</div>
                                        <div class="OchiCell width100"><input type="text" id="monitorTime" class="inputex width99" disabled></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">合格操作人員總數</div>
                                        <div class="OchiCell width100"><input type="number" min="0" id="TotalOperator" class="inputex width80" disabled> 人</div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->

                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">輪班制度</div>
                                        <div class="OchiCell width100"><input type="radio" name="rbShift" value="01" disabled> 三班二輪 ；<input type="radio" name="rbShift" value="02" disabled > 四班三輪</div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">每班人數</div>
                                        <div class="OchiCell width100"><input type="number" min="0" id="classPerson" class="inputex width80" disabled> 人</div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->

                                <div class="OchiRow">

                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">每班時數</div>
                                    <div class="OchiCell width100"><input type="radio" name="rbClassTime" value="01" disabled> 8小時  ;  <input type="radio" name="rbClassTime" value="02" disabled>12小時 ; <input type="radio" name="rbClassTime" value="03" disabled>其他 </div>
                                </div><!-- OchiRow -->


                            </div><!-- OchiTrasTable -->
                            </br>

                            <div class="twocol">
                                <div class="left font-size4 margin10T font-bold">儲槽泵送/接收資料</div>
                                <div class="right">
                                <a id="newbtn" href="javascript:void(0);" title="新增" class="genbtn">新增</a>
                                </div>
                            </div><br />
                            <div class="stripeMeB tbover">
                                <table id="tablist" width="100%" border="0" cellspacing="0" cellpadding="0" width="0">
                                    <thead>
                                        <tr>
                                            <th >轄區儲槽編號 </th>
                                            <th  >負責泵送或接收之控制室名稱 </th>
                                            <th nowrap >液位監測方式 <br>
                                                1.機械 <br>
                                                2.超音波 <br>
                                                3.雷達 <br>
                                                4.RF transmitter<br>
                                                5.其他 </th>
                                            <th  >液位監測靈敏度 <br>
                                                (mm)</th>
                                            <th  >高液位警報 <br>
                                                設定基準(mm)</th>
                                            <th  >前一年度高液位警報發生頻率 <br>
                                                次/年 </th>
                                            <th  >液位異常下降警報設定基準(mm)</th>
                                            <th  >前一年度異常下降警報發生頻率 <br>
                                                次/年 </th>
                                            <th id="th_edit" >功能</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div>

                            <div class="margin5TB font-size2">
                                (1) 請依各儲槽分別填寫。<br>
                                (2) 「液位監測方式」：請依單位使用之系統，填寫對應之數字，若有2種以上，請都填寫。<br>
                                (3) 「液位監測靈敏度」：請對應「液位監測方式」填寫的數字，填寫該方式的靈敏度。

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

