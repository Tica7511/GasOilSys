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
			getData();
		}); // end js

		function getData() {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetGasStorageTankInfo.aspx",
				data: {
					cpid: $.getQueryString("cp")
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
                                // 庫區特殊區域
								var othercheck = false;
								var arycheck = $(this).children("庫區特殊區域").text().trim().split(',');
								$.each(arycheck, function (key, value) {
									$("input[name='checkArea'][value='" + value + "']").prop("checked", true);

									if (value == "05")
										othercheck = true;
								});
								if (othercheck)
                                    $("#checkAreaOther").val($(this).children("庫區特殊區域_其他").text().trim());

                                $("#content").html($(this).children("內容").text().trim());
							});
                        }

						$("#tablist tbody").empty();
						var tabstr = '';
						if ($(data).find("data_item2").length > 0) {
							$(data).find("data_item2").each(function (i) {
								tabstr += '<tr>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("液化天然氣廠").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("儲槽編號").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("容量").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("外徑").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("高度").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("形式").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("啟用日期").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("狀態").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("勞動部檢查").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("代行檢查機構").text().trim() + '</td>';
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="10">查詢無資料</td></tr>';
                        $("#tablist tbody").append(tabstr);

                        $("#tablist2 tbody").empty();
						var tabstr = '';
						if ($(data).find("data_item3").length > 0) {
							$(data).find("data_item3").each(function (i) {
								tabstr += '<tr>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("儲氣設備").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("查核項目").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("業者填寫").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("佐證資料").text().trim() + '</td>';
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="4">查詢無資料</td></tr>';
                        $("#tablist2 tbody").append(tabstr);
					}
				}
			});
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
        <div id="ContentWrapper">
            <div class="container margin15T">



                <div class="padding10ALL">
                    <div class="filetitlewrapper"><!--#include file="GasBreadTitle.html"--></div>

                    <div class="row margin20T">
                        <div class="col-lg-3 col-md-4 col-sm-5">
                            <div id="navmenuV"><!--#include file="GasLeftMenu.html"--></div>
                        </div>
                        <div class="col-lg-9 col-md-8 col-sm-7">
                            <div class="font-size3">(一)儲槽基本資料表</div>
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
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div><!-- stripeMe -->

                            <div class="font-size3 margin10T">(二)廠區是否有屬於下列特殊區域？有者請打勾</div>
                            <div class="font-size3">
                                <div class="inlineitem"><input type="checkbox" name="checkArea" value="01" disabled> 活動斷層敏感區</div>
                                <div class="inlineitem"><input type="checkbox" name="checkArea" value="02" disabled> 土壤液化區</div>
                                <div class="inlineitem"><input type="checkbox" name="checkArea" value="03" disabled> 土石流潛勢區</div>
                                <div class="inlineitem"><input type="checkbox" name="checkArea" value="04" disabled> 淹水潛勢區</div>
                                <div class="inlineitem"><input type="checkbox" name="checkArea" value="05" disabled> 其他 <input type="text" id="checkAreaOther" class="inputex" disabled></div>
                                <div class="inlineitem"><input type="checkbox" name="checkArea" value="06" disabled> 以上皆無</div>
                            </div>

                            <div id="content">

                            </div>

                            <div class="font-size3 margin10T">(三)儲槽設備查核資料</div>
                            <div class="stripeMeG margin5T tbover">
                                <table id="tablist2" width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th  rowspan="5">儲氣設備</th>
                                            <th >查  核  項  目 </th>
                                            <th >業者填寫 </th>
                                            <th  valign="top">佐證資料/紀錄/指導書/作業程序 </th>
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

