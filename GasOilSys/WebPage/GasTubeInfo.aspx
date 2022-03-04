<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GasTubeInfo.aspx.cs" Inherits="WebPage_GasTubeInfo" %>

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
    <style>
        td:first-child, th:first-child {
         position:sticky;
         left:0; /* 首行永遠固定於左 */
         z-index:1;
        }
        
        thead tr th {
         position:sticky;
         top:0; /* 列首永遠固定於上 */
        }
        
        th:first-child{
         z-index:2;
        }
    </style>
	<script type="text/javascript">
        $(document).ready(function () {
            $(".container").css("max-width", "1800px");
            //getYearList();
            //$("#sellist").val(getTaiwanDate());
            //$("#taiwanYear").val(getTaiwanDate());
			getData(0);

            //選擇年份
            $(document).on("change", "#sellist", function () {
                $("#taiwanYear").val($("#sellist option:selected").val());
                getData(0);
            });

            //新增按鈕
            $(document).on("click", "#newbtn", function () {
                location.href = "edit_GasTubeInfo.aspx?cp=" + $.getQueryString("cp");
            });

            //刪除按鈕
            $(document).on("click", "a[name='delbtn']", function () {
                if (confirm("確定刪除?")) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../handler/DelGasTubeInfo.aspx",
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
                                getData(0);
                            }
                        }
                    });
                }
            });
		}); // end js

        function getData(p) {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetGasTubeInfo.aspx",
				data: {
                    cpid: $.getQueryString("cp"),
                    type: "list",
                    PageNo: p,
                    PageSize: Page.Option.PageSize,
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
								tabstr += '<td nowrap="nowrap">' + $(this).children("轄區長途管線名稱_公司").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("銜接管線識別碼_上游").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("銜接管線識別碼_下游").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("起點").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("迄點").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("管徑").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("厚度").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("管材").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("包覆材料").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("轄管長度").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("內容物").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("緊急遮斷閥").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("建置年").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("設計壓力").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("使用壓力").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("使用狀態").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("附掛橋樑數量").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("管線穿越箱涵數量").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("活動斷層敏感區").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("土壤液化區").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("土石流潛勢區").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("淹水潛勢區").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap"><pre>' + $(this).children("備註").text().trim() + '</pre></td>';
                                tabstr += '<td name="td_edit" nowrap="" align="center"><a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("guid").text().trim() + '">刪除</a>';
                                tabstr += ' <a href="edit_GasTubeInfo.aspx?cp=' + $.getQueryString("cp") + '&guid=' + $(this).children("guid").text().trim() + '" name="editbtn">編輯</a></td>';
                                tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="24">查詢無資料</td></tr>';
                        $("#tablist tbody").append(tabstr);
                        Page.Option.Selector = "#pageblock";
                        Page.Option.FunctionName = "getData";
                        Page.CreatePage(p, $("total", data).text());

                        //確認權限&按鈕顯示或隱藏
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
                    type: "Gas",
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
                url: "../Handler/GetGasTubeInfo.aspx",
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
<body class="bgG">
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
		<!--#include file="GasHeader.html"-->
        <input type="hidden" id="Competence" value="<%= competence %>" />
        <input type="hidden" id="taiwanYear" />
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
                                <%--<div class="left font-size5 "><i class="fa fa-chevron-circle-right IconCa" aria-hidden="true"></i> 
                                    <select id="sellist" class="inputex">
                                    </select> 年
                                </div>--%>
                                <div class="right">
                                <a id="newbtn" href="javascript:void(0);" title="新增" class="genbtn">新增</a>
                                </div>
                            </div><br />
							<div class="stripeMeG tbover">
								<table id="tablist" width="100%" border="0" cellspacing="0" cellpadding="0">
									<thead>
										<tr>
											<th nowrap>長途管線識別碼 </th>
											<th nowrap>轄區長途管線名稱<br>(公司)</th>
											<th nowrap>銜接管線識別碼<br>(上游) </th>
											<th nowrap>銜接管線識別碼<br>(下游) </th>
											<th nowrap>起點 </th>
											<th nowrap>迄點 </th>
											<th nowrap>管徑<br>吋 </th>
											<th nowrap>厚度<br>(mm)</th>
											<th nowrap>管材<br>(詳細規格)</th>
											<th nowrap>包覆材料 </th>
											<th nowrap>轄管長度<br>(公尺)</th>
											<th nowrap>內容物 </th>
											<th nowrap>緊急遮斷閥<br>(處)</th>
											<th nowrap>建置年<br>(民國年月)</th>
											<th nowrap>設計壓力<br>(Kg/cm<sup>2</sup>)</th>
											<th nowrap>使用<br>壓力<br>(Kg/cm<sup>2</sup>)</th>
											<th nowrap>使用狀態<br>1.使用中<br>2.停用<br>3.備用 </th>
											<th nowrap>附掛<br>橋樑<br>數量 </th>
                                            <th nowrap>管線穿越<br>箱涵數量</th>
                                            <th nowrap>活動斷層敏感區<br>1.有<br>2.無 </th>
                                            <th nowrap>土壤液化區<br>1.有<br>2.無 </th>
                                            <th nowrap>土石流潛勢區<br>1.有<br>2.無 </th>
                                            <th nowrap>淹水潛勢區<br>1.有<br>2.無 </th>
                                            <th nowrap>備註 </th>
                                            <th id="th_edit">功能</th>
										</tr>
									</thead>
									<tbody></tbody>
								</table>
							</div><!-- stripeMe -->
                            <div class="margin10B margin10T textcenter">
	                            <div id="pageblock"></div>
	                        </div>

							<div class="margin5TB font-size2">
								註1：厚度請填寫到小數點後兩位，請依據ASME B36.10M Welded and Seamless Wrought Steel Pipe填寫公稱厚度，例如25.40 mm。<br>
								註2：管線長度單位公里，請填寫到小數點後三位，例如5.140公里。<br>
								註3：停用管線之內容物，請填如(氮封)、(空管)、(水)。<br>
								註4：緊急遮斷閥請填寫有幾處。<br>
								註5：設計壓力與使用壓力(請填寫近年內之MOP)單位請採用kg/cm<sup>2</sup>。<br>
								註6：請填寫同一管線附掛橋樑的數量。<br>
								註7：廢棄管線與非中油公司資產管線，請勿列入。<br>
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
