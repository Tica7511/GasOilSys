<%@ Page Language="C#" AutoEventWireup="true" CodeFile="memberLog.aspx.cs" Inherits="Admin_memberLog" %>

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
            $(".pickDate").datepick({
                dateFormat: 'yymmdd',
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: '../images/calendar.gif',
                yearRange: 'c-60:c+10'
            }).BootStrap(); //BootStrap() 產生符合 BootStrap 的樣式內容

            getDDL('001');
            getDDL('002');
            getData(0);

            //查詢
            $(document).on("click", "#subbtn", function () {
                getData(0);
            });
        }); // end js

        function getData(p) {
            $("#pageNumber").val(p);
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
                url: "BackEnd/GetMemberLog.aspx",
                data: {
                    PageNo: p,
                    PageSize: Page.Option.PageSize,
                    txt1: $("#txt1").val(),
                    txt2: $("#txt2").val(),
                    beginDate: $("#txt_begintime").val(),
                    endDate: $("#txt_endtime").val(),
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
                                tabstr += '<td nowrap="nowrap">' + $(this).children("帳號").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("姓名").text().trim() + '</td>';
                                var accountType = $(this).children("帳號類別").text().trim();
                                switch (accountType) {
                                    case "01":
                                        accountType = '委員';
                                        break;
                                    case "02":
                                        accountType = '業者';
                                        break;
                                    case "03":
                                        accountType = '管理員';
                                        break;
                                    case "04":
                                        accountType = '長官';
                                        break;
                                    case "05":
                                        accountType = '台灣中油長官';
                                        break;
                                    case "06":
                                        accountType = '台塑石化長官';
                                        break;
                                }
								tabstr += '<td nowrap="nowrap">' + accountType + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("登入日期").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">';
                                if ($(this).children("登入結果_V").text().trim() == '失敗')
                                    tabstr += '<span style="color: red">' + $(this).children("登入結果_V").text().trim() + '</span>';
                                else
                                    tabstr += '<span>' + $(this).children("登入結果_V").text().trim() + '</span>';
                                tabstr += '</td>';
                                tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="5">查詢無資料</td></tr>';
                        $("#tablist tbody").append(tabstr);
                        Page.Option.Selector = "#pageblock";
                        Page.Option.FunctionName = "getData";
                        Page.CreatePage(p, $("total", data).text());
					}
				}
			});
        }

        function getDDL(gNo) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/GetDDLlist.aspx",
                data: {
                    gNo: gNo,
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
                                ddlstr += '<option value="' + $(this).children("項目代碼").text().trim() + '">' + $(this).children("項目名稱").text().trim() + '</option>';
                            });
                        }
                        switch (gNo) {
                            case '001':
                                $("#txt1").empty();
                                $("#txt1").append(ddlstr);
                                break;
                            case '002':
                                $("#txt2").empty();
                                $("#txt2").append(ddlstr);
                                break;
                        }
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
        <input type="hidden" id="pageNumber" />
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
                                <div class="left font-size5 ">查詢:</div>
                                <div class="right">

                                </div>
                            </div>
                            <div class="OchiTrasTable width100 TitleLength09 font-size3">
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">帳號類別</div>
                                        <div class="OchiCell width100"><select id="txt1" class="width100 inputex" ></select></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">姓名</div>
                                        <div class="OchiCell width100"><input type="text" id="txt2" class="inputex width100 "></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">登入時間</div>
                                    <div class="OchiCell width100">
                                        起: <input id="txt_begintime" type="text" class="inputex pickDate width20" disabled> 迄: <input id="txt_endtime" type="text" class="inputex pickDate width20" disabled>
                                    </div>
                                </div><!-- OchiRow -->
                            </div><!-- OchiTrasTable -->
                            <br />
                            <div class="twocol">
                                <div class="left">
                                    
                                </div>
                                <div class="right">
                                    <a id="subbtn" href="javascript:void(0);" title="查詢" class="genbtn" >查詢</a>
                                </div>
                            </div>
                            <br />
                            <div class="stripeMeB tbover">
                                <table id="tablist" border="0" cellspacing="0" cellpadding="0" width="100%">
                                    <thead>
							        	<tr>
							        		<th nowrap="nowrap">帳號</th>
							        		<th nowrap="nowrap">姓名</th>
							        		<th nowrap="nowrap">帳號類別</th>
                                            <th nowrap="nowrap">登入時間</th>
							        		<th nowrap="nowrap">登入結果</th>
							        	</tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div>
                            <div class="margin10B margin10T textcenter">
	                            <div id="pageblock"></div>
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



