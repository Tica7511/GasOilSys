<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OilContactDetail.aspx.cs" Inherits="Admin_OilContactDetail" %>

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
    <title>石油與天然氣事業輸儲設備查核及檢測資訊系統後台管理</title>
	<!--#include file="Head_Include_Manage.html"-->
	<script type="text/javascript">
        $(document).ready(function () {
            getData();

            //選擇年份
            $(document).on("change", "#mgsellist", function () {
                getContactDetailData($("#mgsellist option:selected").val());
            });

            //取消視窗
            $(document).on("click", "#mgcancelbtn", function () {
                $.magnificPopup.close();
            });

            //編輯開窗
            $(document).on("click", "a[name='editbtn']", function () {
                $("#CGguid").val($(this).attr("aid"));

                var cguid = $("#CGguid").val();

                var sp1 = $("span[name='sp1_" + cguid + "']").text();
                var sp2 = $("span[name='sp2_" + cguid + "']").text();
                var sp3 = $("span[name='sp3_" + cguid + "']").text();
                var sp4 = $("span[name='sp4_" + cguid + "']").text();
                var sp5 = $("span[name='sp5_" + cguid + "']").text();
                var sp6 = $("span[name='sp6_" + cguid + "']").text();

                if (sp2 == "" && sp3 == "" && sp4 == "" && sp5 == "" && sp6 == "") {
                    $("#cpNameIsConfirm").html(sp1);
                }
                else {
                    $("#cpNameIsConfirm").html(sp2 + sp3 + sp4 + sp5 + sp6);
                }

                getYearList();
                $("#mgsellist").val(getTaiwanDate());
                getContactDetailData(getTaiwanDate());
                doOpenMagPopupConfirmData();
            });

            //儲存按鈕
            $(document).on("click", "#mgsubbtn", function () {
                var msg = '';

                if ($("#mgtxt1").val() == '')
                    msg += "請輸入【本年度查核聯絡窗口 姓名】\n";
                if ($("#mgtxt2").val() == '')
                    msg += "請輸入【本年度查核聯絡窗口 職稱】\n";
                if ($("#mgtxt3").val() == '')
                    msg += "請輸入【本年度查核聯絡窗口 分機】\n";
                if ($("#mgtxt4").val() == '')
                    msg += "請輸入【本年度查核聯絡窗口 email】\n";
                if ($("#mgtxt5").val() == '')
                    msg += "請輸入【本年度檢測聯絡窗口 姓名】\n";
                if ($("#mgtxt6").val() == '')
                    msg += "請輸入【本年度檢測聯絡窗口 職稱】\n";
                if ($("#mgtxt7").val() == '')
                    msg += "請輸入【本年度檢測聯絡窗口 分機】\n";
                if ($("#mgtxt8").val() == '')
                    msg += "請輸入【本年度檢測聯絡窗口 email】\n";

                if (msg != "") {
                    alert("Error message: \n" + msg);
                    return false;
                }

                // Create an FormData object
                var data = new FormData();

                // If you want to add an extra field for the FormData
                data.append("type", "Oil");
                data.append("cpid", $("#CGguid").val());
                data.append("year", $("#mgsellist option:selected").val());
                data.append("category", "contact");
                data.append("txt1", encodeURIComponent($("#mgtxt1").val()));
                data.append("txt2", encodeURIComponent($("#mgtxt2").val()));
                data.append("txt3", encodeURIComponent($("#mgtxt3").val()));
                data.append("txt4", encodeURIComponent($("#mgtxt4").val()));
                data.append("txt5", encodeURIComponent($("#mgtxt5").val()));
                data.append("txt6", encodeURIComponent($("#mgtxt6").val()));
                data.append("txt7", encodeURIComponent($("#mgtxt7").val()));
                data.append("txt8", encodeURIComponent($("#mgtxt8").val()));

                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "../handler/AddCompanyCheck.aspx",
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
                            $.magnificPopup.close();
                        }
                    }
                });
            });

        }); // end js

        function getData() {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetOilCompanyList.aspx",
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
                        isConfirm01 = '';
                        isConfirm02 = '';
                        isStorageConfirm01 = '';
                        isStorageConfirm02 = '';
                        isStorageLiqConfirm01 = '';
                        isStorageLiqConfirm02 = '';
						if ($(data).find("data_item").length > 0) {
							$(data).find("data_item").each(function (i) {
								tabstr += '<tr>';
								tabstr += '<td nowrap="nowrap"><span name="sp1_' + $(this).children("guid").text().trim() + '">' + $(this).children("公司名稱").text().trim() + '</span></td>';
								tabstr += '<td nowrap="nowrap"><span name="sp2_' + $(this).children("guid").text().trim() + '">' + $(this).children("處").text().trim() + '</span></td>';
								tabstr += '<td nowrap="nowrap"><span name="sp3_' + $(this).children("guid").text().trim() + '">' + $(this).children("事業部").text().trim() + '</span></td>';
								tabstr += '<td nowrap="nowrap"><span name="sp4_' + $(this).children("guid").text().trim() + '">' + $(this).children("營業處廠").text().trim() + '</span></td>';
								tabstr += '<td nowrap="nowrap"><span name="sp5_' + $(this).children("guid").text().trim() + '">' + $(this).children("組").text().trim() + '</span></td>';
                                tabstr += '<td nowrap="nowrap"><span name="sp6_' + $(this).children("guid").text().trim() + '">' + $(this).children("中心庫區儲運課工場").text().trim() + '</span></td>';
                                tabstr += '<td align="center" nowrap="nowrap" class="font-normal"><a name="editbtn" href="javascript:void(0);" aid="' + $(this).children("guid").text().trim() + '">聯絡資訊</a>';
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="12">查詢無資料</td></tr>';
						$("#tablist tbody").append(tabstr);
					}
				}
			});
        }

        function getContactDetailData(year) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetOilInfo.aspx",
                data: {
                    cpid: $("#CGguid").val(),
                    year: year
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
                            $(data).find("data_item").each(function () {
                                $("#mgtxt1").val($(this).children("年度查核姓名").text().trim());
                                $("#mgtxt2").val($(this).children("年度查核職稱").text().trim());
                                $("#mgtxt3").val($(this).children("年度查核分機").text().trim());
                                $("#mgtxt4").val($(this).children("年度查核email").text().trim());
                                $("#mgtxt5").val($(this).children("年度檢測姓名").text().trim());
                                $("#mgtxt6").val($(this).children("年度檢測職稱").text().trim());
                                $("#mgtxt7").val($(this).children("年度檢測分機").text().trim());
                                $("#mgtxt8").val($(this).children("年度檢測email").text().trim());
                            });
                        }
                        else {
                            $("#mgtxt1").val("");
                            $("#mgtxt2").val("");
                            $("#mgtxt3").val("");
                            $("#mgtxt4").val("");
                            $("#mgtxt5").val("");
                            $("#mgtxt6").val("");
                            $("#mgtxt7").val("");
                            $("#mgtxt8").val("");
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
                url: "../Handler/GetOilInfo.aspx",
                data: {
                    cpid: $("#CGguid").val(),
                    year: getTaiwanDate(),
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
                        $("#mgsellist").empty();
                        var ddlstr = '';
                        if ($(data).find("data_item2").length > 0) {
                            $(data).find("data_item2").each(function (i) {
                                ddlstr += '<option value="' + $(this).children("年度").text().trim() + '">' + $(this).children("年度").text().trim() + '</option>'
                            });
                        }
                        else {
                            ddlstr += '<option>請選擇</option>'
                        }
                        $("#mgsellist").append(ddlstr);
                    }
                }
            });
        }

        //取得現在時間之民國年
        function getTaiwanDate() {
            var nowDate = new Date();

            var nowYear = nowDate.getFullYear();
            var nowTwYear = (nowYear - 1911);

            return nowTwYear;
        }

        //取得現在時間之民國年
        function getTaiwanDate() {
            var nowDate = new Date();

            var nowYear = nowDate.getFullYear();
            var nowTwYear = (nowYear - 1911);

            return nowTwYear;
        }

        function doOpenMagPopupConfirmData() {
            $.magnificPopup.open({
                items: {
                    src: '#messageblockConfirm'
                },
                type: 'inline',
                midClick: false, // 是否使用滑鼠中鍵
                closeOnBgClick: true,//點擊背景關閉視窗
                showCloseBtn: true,//隱藏關閉按鈕
                fixedContentPos: true,//彈出視窗是否固定在畫面上
                mainClass: 'mfp-fade',//加入CSS淡入淡出效果
                tClose: '關閉',//翻譯字串
            });
        }
    </script>
</head>
<body class="bgC">
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
		<!--#include file="ManageHeader.html"-->
        <input type="hidden" id="Competence" value="<%= competence %>" />
        <input type="hidden" id="CGguid" />
        <div id="ContentWrapper">
            <div class="container margin15T">
                <div class="padding10ALL">
                    <%--<div class="filetitlewrapper"><!--#include file="GasBreadTitle.html"--></div>--%>

                    <div class="row margin20T">
                        <div class="col-lg-3 col-md-4 col-sm-5">
                            <div id="navmenuV"><!--#include file="ManageLeftMenu.html"--></div>
                        </div>
                        <div class="col-lg-9 col-md-8 col-sm-7">
                            <div class="twocol">
                                <div class="right">

                                </div>
                            </div><br />
                            <div class="stripeMeB tbover">
                                <table id="tablist" border="0" cellspacing="0" cellpadding="0" width="100%">
                                    <thead>
							        	<tr>
								        	<th nowrap="nowrap">公司名稱</th>
								        	<th nowrap="nowrap">處</th>
								        	<th nowrap="nowrap">事業部</th>
								        	<th nowrap="nowrap">營業處廠</th>
								        	<th nowrap="nowrap">組</th>
								        	<th nowrap="nowrap">中心庫區儲運課工場</th>
								        	<th nowrap="nowrap" width="100">功能</th>
								        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
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
	
		<!--#include file="ManageFooter.html"-->

</div><!-- BoxBgWa -->
<!-- 側邊選單內容:動態複製主選單內容 -->
<div id="sidebar-wrapper"></div><!-- sidebar-wrapper -->

</form>
</div>
<!-- 結尾用div:修正mmenu form bug -->

<!-- Magnific Popup -->
<div id="messageblockConfirm" class="magpopup magSizeM mfp-hide">
    <div class="magpopupTitle"><span id="cpNameIsConfirm"></span></div>
    <div class="padding10ALL">
        <div class="twocol">
            <div class="left font-size5 ">
                <i class="fa fa-chevron-circle-right IconCa" aria-hidden="true"></i>
                <select id="mgsellist" class="inputex">
                </select> 年
            </div>
            <div class="right">
                <%--<span class="font-size5" style="color:red">請填寫本年度各聯絡窗口資訊，填妥後請點選儲存來確認資料</span>--%>
            </div>
        </div><br />
        <div class="OchiTrasTable width100 TitleLength08 font-size3">
            <div class="OchiRow">
                <div class="margin5TB font-size4" style="text-align:center">本年度查核聯絡窗口</div>
            </div><!-- OchiRow -->
            <div class="OchiRow">
                <div class="OchiHalf">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">姓名</div>
                    <div class="OchiCell width100"><input id="mgtxt1" type="text" class="inputex width100"></div>
                </div><!-- OchiHalf -->
                <div class="OchiHalf">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">職稱</div>
                    <div class="OchiCell width100"><input id="mgtxt2" type="text" class="inputex width100"></div>
                </div><!-- OchiHalf -->
            </div><!-- OchiRow -->
            <div class="OchiRow">
                <div class="OchiHalf">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">分機</div>
                    <div class="OchiCell width100"><input id="mgtxt3" type="text" class="inputex width100"></div>
                </div><!-- OchiHalf -->
                <div class="OchiHalf">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">email</div>
                    <div class="OchiCell width100"><input id="mgtxt4" type="text" class="inputex width100"></div>
                </div><!-- OchiHalf -->
            </div><!-- OchiRow -->
            </br>
            <div class="OchiRow">
                <div class="margin5TB font-size4" style="text-align:center">本年度檢測聯絡窗口</div>
            </div><!-- OchiRow -->
            <div class="OchiRow">
                <div class="OchiHalf">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">姓名</div>
                    <div class="OchiCell width100"><input type="text" id="mgtxt5" class="inputex width100"></div>
                </div><!-- OchiHalf -->
                <div class="OchiHalf">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">職稱</div>
                    <div class="OchiCell width100"><input type="text" id="mgtxt6" class="inputex width100"></div>
                </div><!-- OchiHalf -->
            </div><!-- OchiRow -->
            <div class="OchiRow">
                <div class="OchiHalf">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">分機</div>
                    <div class="OchiCell width100"><input type="text" id="mgtxt7" class="inputex width100"></div>
                </div><!-- OchiHalf -->
                <div class="OchiHalf">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">email</div>
                    <div class="OchiCell width100"><input type="text" id="mgtxt8" class="inputex width100"></div>
                </div><!-- OchiHalf -->
            </div><!-- OchiRow -->
        </div><!-- OchiTrasTable -->

        <div class="twocol margin10T">
            <div class="right">
                <a id="mgcancelbtn" href="javascript:void(0);" class="genbtn closecolorbox">取消</a>
                <a id="mgsubbtn" href="javascript:void(0);" class="genbtn">儲存</a>
            </div>
        </div>

    </div><!-- padding10ALL -->

</div><!--magpopup -->

<!-- 本頁面使用的JS -->
	<script type="text/javascript">
        $(document).ready(function () {

		});
    </script>
	<script type="text/javascript" src="../js/GenCommon.js"></script><!-- UIcolor JS -->
	<script type="text/javascript" src="../js/PageCommon.js"></script><!-- 系統共用 JS -->
	<script type="text/javascript" src="../js/MenuGas.js"></script><!-- 系統共用 JS -->
	<script type="text/javascript" src="../js/SubMenuManage.js"></script><!-- 內頁選單 -->
	<script type="text/javascript" src="../js/autoHeight.js"></script><!-- 高度不足頁面的絕對置底footer -->
</body>
</html>



