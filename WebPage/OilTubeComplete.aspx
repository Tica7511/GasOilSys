<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OilTubeComplete.aspx.cs" Inherits="WebPage_OilTubeComplete" %>

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
                location.href = "edit_OilTubeComplete.aspx?cp=" + $.getQueryString("cp");
            });

            //刪除按鈕
            $(document).on("click", "a[name='delbtn']", function () {
                if (confirm("確定刪除?")) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../handler/DelOilTubeComplete.aspx",
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
				url: "../Handler/GetOilTubeComplete.aspx",
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
						$("#tablist tbody").empty();
						var tabstr = '';
						if ($(data).find("data_item").length > 0) {
							$(data).find("data_item").each(function (i) {
								tabstr += '<tr>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("長途管線識別碼").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("風險評估年月").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("智慧型通管器ILI可行性").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("耐壓強度試驗TP可行性").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("緊密電位CIPS年月").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("電磁包覆PCM年月").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("智慧型通管器ILI年月").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("耐壓強度試驗TP年月").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("耐壓強度試驗TP介質").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("試壓壓力與MOP壓力倍數").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("耐壓強度試驗TP持壓時間").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("受雜散電流影響").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("洩漏偵測系統").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("強化作為").text().trim() + '</td>';
                                tabstr += '<td name="td_edit" nowrap="" align="center"><a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("guid").text().trim() + '">刪除</a>';
                                tabstr += ' <a href="edit_OilTubeComplete.aspx?cp=' + $.getQueryString("cp") + '&guid=' + $(this).children("guid").text().trim() + '" name="editbtn">編輯</a></td>';
                                tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="14">查詢無資料</td></tr>';
                        $("#tablist tbody").append(tabstr);

                        //確認權限&按鈕顯示或隱藏
                        if ($("#sellist").val() != getTaiwanDate()) {
                            $("#newbtn").hide();
                            $("#th_edit").hide();
                            $("td[name='td_edit']").hide();
                        }
                        else {
                            if (($("#Competence").val() == '01') || ($("#Competence").val() == '04') || ($("#Competence").val() == '05') || ($("#Competence").val() == '06')) {
                                $("#newbtn").hide();
                                $("#th_edit").hide();
                                $("td[name='td_edit']").hide();
                            }
                            else {
                                $("#newbtn").show();
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
                url: "../Handler/GetOilTubeComplete.aspx",
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
                                <a id="newbtn" href="javascript:void(0);" title="新增" class="genbtn">新增</a>
                                </div>
                            </div><br />
                            <div class="stripeMeB tbover">
                                <table id="tablist" border="0" cellspacing="0" cellpadding="0" width="100%">
                                    <thead>
                                        <tr>
                                            <th  rowspan="2">長途管線識別碼 </th>
                                            <th >風險評估 <br>
                                                年/月 </th>
                                            <th >智慧型通管器(ILI) 可行性 </th>
                                            <th >耐壓強度試驗 <br>
                                                (TP)<br>
                                                可行性 </th>
                                            <th >緊密電位(CIPS)<br>
                                                年/月 </th>
                                            <th >電磁包覆(PCM)<br>
                                                年/月 </th>
                                            <th >智慧型通管器(ILI)<br>
                                                年/月 </th>
                                            <th >耐壓強度試驗 <br>
                                                (TP)<br>
                                                年/月 </th>
                                            <th >耐壓強度試驗 <br>
                                                (TP)<br>
                                                介質 </th>
                                            <th >試壓壓力與MOP壓力倍數 </th>
                                            <th >耐壓強度試驗 <br>
                                                (TP)<br>
                                                持壓時間 <br>
                                                (小時)</th>
                                            <th >受雜散 <br>
                                                電流影響 </th>
                                            <th >洩漏偵測系統 <br>
                                                (LLDS)</th>
                                            <th  rowspan="2">強化作為 </th>
                                            <th id="th_edit"  rowspan="2">功能 </th>
                                        </tr>
                                        <tr>
                                            <th >&nbsp;</th>
                                            <th >1.可 <br>
                                                2.無法 </th>
                                            <th >1.可 <br>
                                                2.無法 </th>
                                            <th >&nbsp;</th>
                                            <th >&nbsp;</th>
                                            <th >&nbsp;</th>
                                            <th >&nbsp;</th>
                                            <th >&nbsp;</th>
                                            <th >&nbsp;</th>
                                            <th >&nbsp;</th>
                                            <th >1.有 <br>
                                                2.無 </th>
                                            <th >1.有 <br>
                                                2.無 </th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>                                
                            </div>
                            <div class="margin5TB font-size2">
                                填表說明：<br>
                                (1) 請依各管線分別填寫。<br>
                                (2) 智慧型通管器(ILI) 可行性：請依據實際情形填寫該管線是否可執行ILI檢測，若可以，則選「可」，若有困難(如：三通、異徑等)無法執行，請選「無法」。<br>
                                (3) 耐壓強度試驗(TP)可行性：請依據實際情形填寫該管線是否可執行耐壓試驗，若可以，則選「可」，若有困難無法執行，請選「無法」。<br>
                                (4) 若有執行上述之檢測方法，請選最近一次檢測時間：年/月，ILI檢測則填寫第4階段之年/月，若在執行第1至3階段，請填寫執行中，若無檢測則選 “NA”。<br>
                                (5) 耐壓強度試驗(TP)介質、壓力倍數、持壓時間：若有執行耐壓強度試驗(TP)才須填寫，若無執行，則免填。<br>
                                (6) 受雜散電流影響：若管線有受雜散電流影響，請選有；反之，若無，請選無。<br>
                                (7) 強化作為：若該管線有強化作為，請簡述強化作為內容。
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

