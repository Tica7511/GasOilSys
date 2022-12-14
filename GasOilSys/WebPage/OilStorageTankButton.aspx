<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OilStorageTankButton.aspx.cs" Inherits="WebPage_OilStorageTankButton" %>

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
            $("#exportbtn").attr("href", "../Oil_EXPORTEXCEL.aspx?cpid=" + $.getQueryString("cp") + "&year=" + getTaiwanDate() + "&category=storagetankbutton");

            //選擇年份
            $(document).on("change", "#sellist", function () {
                getData($("#sellist option:selected").val());
                $("#exportbtn").attr("href", "../Oil_EXPORTEXCEL.aspx?cpid=" + $.getQueryString("cp") + "&year=" + $("#sellist option:selected").val() + "&category=storagetankbutton");
            });

            //新增按鈕
            $(document).on("click", "#newbtn", function () {
                location.href = "edit_OilStorageTankButton.aspx?cp=" + $.getQueryString("cp");
            });

            //刪除按鈕
            $(document).on("click", "a[name='delbtn']", function () {
                if (confirm("確定刪除?")) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../handler/DelOilStorageTankButton.aspx",
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
                url: "../Handler/GetOilStorageTankButton.aspx",
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
								tabstr += '<td nowrap="nowrap">' + $(this).children("轄區儲槽編號").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("執行MFL檢測").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("防蝕塗層").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("塗層全面重新施加日期").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("最近一次開放塗層維修情形").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("銲道腐蝕").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("局部變形").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("最近一次開放是否有維修").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("內容物側最小剩餘厚度").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("內容物側最大腐蝕速率").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("土壤側最小剩餘厚度").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("土壤側最大腐蝕速率").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("是否有更換過底板").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("綜合判定").text().trim() + '</td>';
                                tabstr += '<td name="td_edit" nowrap="" align="center"><a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("guid").text().trim() + '">刪除</a>';
                                tabstr += ' <a href="edit_OilStorageTankButton.aspx?cp=' + $.getQueryString("cp") + '&guid=' + $(this).children("guid").text().trim() + '" name="editbtn">編輯</a></td>';
                                tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="15">查詢無資料</td></tr>';
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
                url: "../Handler/GetOilStorageTankButton.aspx",
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
                                    <a id="exportbtn" href="javascript:void(0);" title="匯出" class="genbtn">匯出</a>
                                    <a id="newbtn" href="javascript:void(0);" title="新增" class="genbtn">新增</a>
                                </div>
                            </div><br />
                            <div class="stripeMeB tbover">
                                <table id="tablist" width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th nowrap rowspan="2">轄區<br>儲槽<br>編號</th>
                                            <th nowrap valign="top">執行<br>MFL<br>檢測 </th>
                                            <th nowrap valign="top">防蝕塗層</th>
                                            <th nowrap valign="top">塗層全面<br>重新施加日期 </th>
                                            <th nowrap valign="top">最近一次<br>開放塗層<br>維修情形 </th>
                                            <th nowrap valign="top">銲道腐蝕 </th>
                                            <th nowrap valign="top">局部 <br>變形 </th>
                                            <th nowrap valign="top">最近一次<br>開放是否<br>有維修 </th>
                                            <th nowrap valign="top">內容物側<br>最小剩餘<br>厚度 </th>
                                            <th nowrap valign="top">內容物側<br>最大腐蝕<br>速率 </th>
                                            <th nowrap valign="top">土壤側 <br>最小剩餘<br>厚度 </th>
                                            <th nowrap valign="top">土壤側 <br>最大腐蝕<br>速率 </th>
                                            <th nowrap valign="top">是否有<br>更換過<br>底板 <br>(6)</th>
                                            <th nowrap valign="top">綜合判定<br>(7)</th>
                                            <th id="th_edit" rowspan="2">功能</th>
                                        </tr>
                                        <tr>
                                            <th nowrap>1.全部 <br>
                                                2.部份 <br>
                                                3.無 </th>
                                            <th >1.無 <br>
                                                2.FRP<br>
                                                3.EPOXY<br>
                                                4.其他 </th>
                                            <th >年/月 </th>
                                            <th >1.全部 <br>
                                                2.部份 <br>
                                                3.無 </th>
                                            <th >1.有 <br>
                                                2.無 </th>
                                            <th nowrap>1.有 <br>
                                                2.無 </th>
                                            <th >1.有 <br>
                                                2.無 </th>
                                            <th >mm</th>
                                            <th >mm/yr </th>
                                            <th >mm</th>
                                            <th >mm/yr </th>
                                            <th >1.有 <br>
                                                2.無 </th>
                                            <th nowrap>1.良好 <br>
                                                2.須持續追蹤.</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div><!-- stripeMe --><br />

                            <div class="margin5TB font-size2">
                                填表說明：<br />
                                (1) 執行MFL檢測：填寫儲槽底板磁通漏檢測執行情形，請直接點選全部、部份、無。<br>
                                (2) 防蝕塗層：請直接點選儲槽底板塗層種類，如：無、FRP、EPOXY、其他。<br>
                                (3) 塗層全面重新施加日期：請填寫塗層全面重新施作完成之日期，非定期內部開放修補年/月。<br>
                                (4) 「最近一次開放是否有維修」：此欄位如勾選「有」，請於備註欄填寫相對應文件編號，並於現場提供相關詳細維修紀錄資料。<br>
                                (5) 「內容物側最大腐蝕速率」、「土壤側最大腐蝕速率」、「內容物側最小剩餘厚度」、「土壤側最小剩餘厚度」：有執行MFL檢測者，方須填寫此4欄位，若無執行MFL檢測者，則不須填寫此4欄位。<br>
                                (6) 「是否有更換過底板」欄位：若自建造以來，不管更換面積大小，只要曾經更換過(非貼板)，請選有，並於下方底板更換說明表格填寫更換日期、更換面積(若全面更換者，請填「全部」)及更換原因；若自建造以來，從未更換過，則選無。<br>
                                (7) 綜合判定：針對最近一次開放檢查結果，儲槽底板良好正常，無須特別留意者，請選良好，若有異常(如：腐蝕較明顯，雖未達維修標準，但須注意腐蝕情形；或已維修，但原因不確定已排除，須持續觀察者)，須持續追蹤者，請選「須持續追蹤」。

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

