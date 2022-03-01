<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GasStorageTankInfo.aspx.cs" Inherits="WebPage_GasStorageTankInfo" %>

<!DOCTYPE html>

<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta http-equiv="X-UA-Compatible" content="IE=11; IE=10; IE=9; IE=8" />
	<meta name="viewport" content="width=device-width, initial-scale=1">
	<meta name="keywords" content="關鍵字內容" />
	<meta name="description" content="描述" />
	<!--告訴搜尋引擎這篇網頁的內容或摘要。-->
	<meta name="generator" content="Notepad" />
	<!--告訴搜尋引擎這篇網頁是用什麼軟體製作的。-->
	<meta name="author" content="工研院 資訊處" />
	<!--告訴搜尋引擎這篇網頁是由誰製作的。-->
	<meta name="copyright" content="本網頁著作權所有" />
	<!--告訴搜尋引擎這篇網頁是...... -->
	<meta name="revisit-after" content="3 days" />
	<!--告訴搜尋引擎3天之後再來一次這篇網頁，也許要重新登錄。-->
	<title>天然氣事業輸儲設備查核及檢測資訊系統</title>
	<!--#include file="Head_Include.html"-->
	<script type="text/javascript">
		$(document).ready(function () {
            //getYearList();
            //$("#sellist").val(getTaiwanDate());
            getData();

            //選擇年份
            $(document).on("change", "#sellist", function () {
                getData($("#sellist option:selected").val());
            });

            //新增按鈕(儲槽基本資料表)
            $(document).on("click", "#newbtnInfo", function () {
                location.href = "edit_GasStorageTankInfo_Info.aspx?cp=" + $.getQueryString("cp");
            });

            //新增按鈕(儲槽設備查核資料)
            $(document).on("click", "#newbtnEvaluation", function () {
                location.href = "edit_GasStorageTankInfo_Evaluation.aspx?cp=" + $.getQueryString("cp");
            });

            //編輯按鈕
            $(document).on("click", "#editbtn", function () {
                $("#editbtn").hide();
                $("#cancelbtn").show();
                $("#subbtn").show();
                $("#sellist").attr("disabled", true);
                $("input[name='checkArea']").attr("disabled", false);
                if ($("input[name='checkArea'][value='05']").is(":checked"))
                    $("#checkAreaOther").attr("disabled", false);
                else
                    $("#checkAreaOther").attr("disabled", true);
            });

            //廠區特殊區域 其他勾選時事件
            $(document).on("change", "input[name='checkArea'][value='05']", function () {
                if (this.checked) {
                    $("#checkAreaOther").val($("#checkAreach").val());
                    $("#checkAreaOther").attr("disabled", false);
                }
                else {
                    $("#checkAreach").val($("#checkAreaOther").val());
                    $("#checkAreaOther").val('');
                    $("#checkAreaOther").attr("disabled", true);
                }
            });

            //返回按鈕
            $(document).on("click", "#cancelbtn", function () {
                var str = confirm('尚未儲存的部分將不會更改，確定返回嗎?');

                if (str) {
                    location.href = "GasStorageTankInfo.aspx?cp=" + $.getQueryString("cp");
                }
            });

            //儲存按鍵
            $(document).on("click", "#subbtn", function () {
                var msg = '';

                if ($("input[name='checkArea'][value='05']").is(":checked"))
                    if ($("#checkAreaOther").val() == '')
                        msg += "請填寫【其它】的內容\n";

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
                data.append("checkAreaOther", encodeURIComponent($("#checkAreaOther").val()));

                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "../handler/AddGasStorageTankInfo.aspx",
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

                            location.href = "GasStorageTankInfo.aspx?cp=" + $.getQueryString("cp");
                        }
                    }
                });
            });

            //刪除按鈕(儲槽基本資料表)
            $(document).on("click", "a[name='delbtnInfo']", function () {
                if (confirm("確定刪除?")) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../handler/DelGasStorageTankInfo.aspx",
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

            //刪除按鈕(儲槽設備查核資料)
            $(document).on("click", "a[name='delbtnEvaluation']", function () {
                if (confirm("確定刪除?")) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../handler/DelGasStorageTankEvaluation.aspx",
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

		function getData() {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetGasStorageTankInfo.aspx",
				data: {
                    cpid: $.getQueryString("cp"),
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
                                tabstr += '<td nowrap="nowrap">' + $(this).children("液化天然氣廠").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("儲槽編號").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("容量").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("外徑").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("高度").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("形式").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + getDate($(this).children("啟用日期").text().trim()) + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("狀態").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("勞動部檢查").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("代行檢查機構").text().trim() + '</td>';
                                tabstr += '<td name="td_editInfo" nowrap="" align="center"><a href="javascript:void(0);" name="delbtnInfo" aid="' + $(this).children("guid").text().trim() + '">刪除</a>';
                                tabstr += ' <a href="edit_GasStorageTankInfo_Info.aspx?cp=' + $.getQueryString("cp") + '&guid=' + $(this).children("guid").text().trim() + '" name="editbtnInfo">編輯</a></td>';
                                tabstr += '</tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="11">查詢無資料</td></tr>';
                        $("#tablist tbody").append(tabstr);

                        if ($(data).find("data_item2").length > 0) {
                            $(data).find("data_item2").each(function (i) {
                                // 庫區特殊區域
								var othercheck = false;
                                var arycheck = $(this).children("庫區特殊區域").text().trim().split(',');
                                $("input[name='checkArea']").prop("checked", false);
								$.each(arycheck, function (key, value) {
									$("input[name='checkArea'][value='" + value + "']").prop("checked", true);
								});
                                $("#checkAreaOther").val($(this).children("庫區特殊區域_其他").text().trim());

                                $("#content").html($(this).children("內容").text().trim());
							});
                        }

                        $("#tablist2 tbody").empty();
						var tabstr = '';
						if ($(data).find("data_item3").length > 0) {
							$(data).find("data_item3").each(function (i) {
								tabstr += '<tr>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("儲氣設備").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("查核項目").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("業者填寫").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap"><pre>' + $(this).children("佐證資料").text().trim() + '</pre></td>';
                                tabstr += '<td name="td_editEvaluation" nowrap="" align="center"><a href="javascript:void(0);" name="delbtnEvaluation" aid="' + $(this).children("guid").text().trim() + '">刪除</a>';
                                tabstr += ' <a href="edit_GasStorageTankInfo_Evaluation.aspx?cp=' + $.getQueryString("cp") + '&guid=' + $(this).children("guid").text().trim() + '" name="editbtnEvaluation">編輯</a></td>';
                                tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="5">查詢無資料</td></tr>';
                        $("#tablist2 tbody").append(tabstr);

                        //確認權限&按鈕顯示或隱藏
                        if (($("#Competence").val() == '01') || ($("#Competence").val() == '04') || ($("#Competence").val() == '05') || ($("#Competence").val() == '06')) {
                            $("#editbtn").hide();
                            $("#newbtnInfo").hide();
                            $("#newbtnEvaluation").hide();
                            $("#th_editInfo").hide();
                            $("#th_editEvaluation").hide();
                            $("td[name='td_editInfo']").hide();
                            $("td[name='td_editEvaluation']").hide();
                        }
                        else {
                            $("#editbtn").show();
                            $("#newbtnInfo").show();
                            $("#newbtnEvaluation").show();
                            $("#th_editInfo").show();
                            $("#th_editEvaluation").show();
                            $("td[name='td_editInfo']").show();
                            $("td[name='td_editEvaluation']").show();
                        }
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
                                        $("#editbtn").hide();
                                        $("#newbtnInfo").hide();
                                        $("#newbtnEvaluation").hide();
                                        $("#th_editInfo").hide();
                                        $("#th_editEvaluation").hide();
                                        $("td[name='td_editInfo']").hide();
                                        $("td[name='td_editEvaluation']").hide();
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
                url: "../Handler/GetGasStorageTankInfo.aspx",
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
                        if ($(data).find("data_item4").length > 0) {
                            $(data).find("data_item4").each(function (i) {
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
        <input type="hidden" id="checkAreach" />
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
                            </div><br />
                            <div class="twocol">
                                <div class="left font-size4 margin10T font-bold">(一)儲槽基本資料表</div>
                                <div class="right">
                                    <a id="newbtnInfo" href="javascript:void(0);" title="新增" class="genbtn">新增</a>
                                </div>
                            </div><br />
                            <div class="stripeMeG margin5T tbover">
                                <table id="tablist" width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <thead>                                        
                                        <tr>
                                            <th nowrap>液化天然氣廠 </th>
                                            <th nowrap>儲槽編號 </th>
                                            <th nowrap>容量 <br>（萬公秉） </th>
                                            <th nowrap>外徑 <br>(公尺） </th>
                                            <th nowrap>高度 <br>(公尺)</th>
                                            <th nowrap>形式 </th>
                                            <th nowrap>啟用日期 </th>
                                            <th nowrap>狀態 <br>(使用中/ 開放中/ 停用)</th>
                                            <th nowrap>勞動部檢查<br>合格證及有效期限 </th>
                                            <th nowrap>代行/檢查機構 </th>
                                            <th id="th_editInfo" nowrap>功能</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div><!-- stripeMe -->
                            </br>

                            <div class="twocol">
                                <div class="left font-size4 margin10T font-bold">(二)廠區特殊區域</div>
                                <div class="right">
                                    <a id="editbtn" href="javascript:void(0);" title="編輯" class="genbtn">編輯</a>
                                    <a id="cancelbtn" href="javascript:void(0);" title="返回" class="genbtn" style="display:none">返回</a>
                                    <a id="subbtn" href="javascript:void(0);" title="儲存" class="genbtn" style="display:none">儲存</a>
                                </div>
                            </div><br />
                            <div class="font-size3">
                                <div class="inlineitem"><input type="checkbox" name="checkArea" value="01" disabled> 活動斷層敏感區</div>
                                <div class="inlineitem"><input type="checkbox" name="checkArea" value="02" disabled> 土壤液化區</div>
                                <div class="inlineitem"><input type="checkbox" name="checkArea" value="03" disabled> 土石流潛勢區</div>
                                <div class="inlineitem"><input type="checkbox" name="checkArea" value="04" disabled> 淹水潛勢區</div>
                                <div class="inlineitem"><input type="checkbox" name="checkArea" value="05" disabled> 其他 <input type="text" id="checkAreaOther" class="inputex" disabled></div>
                                <div class="inlineitem"><input type="checkbox" name="checkArea" value="06" disabled> 以上皆無</div>
                            </div>

                            <div id="content">

                            </div><br />

                            <div class="twocol">
                                <div class="left font-size4 margin10T font-bold">(三)儲槽設備查核資料</div>
                                <div class="right">
                                    <a id="newbtnEvaluation" href="javascript:void(0);" title="新增" class="genbtn">新增</a>
                                </div>
                            </div><br />
                            <div class="stripeMeG margin5T tbover">
                                <table id="tablist2" width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th  nowrap>儲氣設備</th>
                                            <th  nowrap>查核項目 </th>
                                            <th  nowrap>業者填寫 </th>
                                            <th  nowrap>佐證資料/紀錄/指導書/作業程序 </th>
                                            <th id="th_editEvaluation">功能</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div><!-- stripeMe -->



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


<!-- 本頁面使用的JS -->
<script type="text/javascript">
$(document).ready(function(){
    $("#collapse1").collapse({
        query: 'div.collapseTitle',//收合標題樣式名
        persist: false,//是否記憶收合,需配合jquery.collapse_storage.js
        open: function() {
            this.slideDown(100);//動畫效果
        },
        close: function() {
            this.slideUp(100);//動畫效果
        },
    });

    $("#collapse1").trigger("open") // 預設全開啟
//$("#collapse1").trigger("close") // 預設全關閉(default)
    $("#collapse1 div:nth-child(1) div.collapseTitle a").trigger("open") // 控制第幾個開啟

//全部收合展開按鈕動作
    $("#collapse1open").click(function(){
        $("#collapse1").trigger("open")
    });
    $("#collapse1close").click(function(){
        $("#collapse1").trigger("close")
    });


});
</script>
<script type="text/javascript" src="../js/GenCommon.js"></script><!-- UIcolor JS -->
<script type="text/javascript" src="../js/PageCommon.js"></script><!-- 系統共用 JS -->
<script type="text/javascript" src="../js/MenuGas.js"></script><!-- 系統共用 JS -->
<script type="text/javascript" src="../js/SubMenuGasA.js"></script><!-- 內頁選單 -->
<script type="text/javascript" src="../js/autoHeight.js"></script><!-- 高度不足頁面的絕對置底footer -->
</body>
</html>

