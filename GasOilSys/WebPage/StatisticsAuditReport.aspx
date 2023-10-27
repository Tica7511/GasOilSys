<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StatisticsAuditReport.aspx.cs" Inherits="WebPage_StatisticsAuditReport" %>

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
    <title>石油與天然氣事業輸儲設備查核及檢測資訊系統</title>
	<!--#include file="Head_Include.html"-->
	<script type="text/javascript">
        $(document).ready(function () {
            $("#div_table").hide();
            getDDL('028', 'sel_type');
            getDDL('029', 'sel_situation');

            //查詢按鈕
            $(document).on("click", "#querybtn", function () {
                getData();
                $("#div_table").show();
            });

            //選擇類別
            $(document).on("change", "#sel_type", function () {
                $("#tObjectGuid").val('');
                $("#txt_object").val('');
                $("#tType").val($("#sel_type option:selected").val());
            });

            //對象請選擇按鈕
            $(document).on("click", "#btn_object", function () {
                if ($("#tType").val() != '') {
                    getObjectData();
                }
                else {
                    alert("請選擇【類別】");
                    return false;
                }

                doOpenMagPopup();
            });

            $(document).on("click", "a[name='objectbtn']", function () {

                $("#tObjectGuid").val($(this).attr('aid'));
                $("#txt_object").val($(this).attr('cname'));
                $.magnificPopup.close();
            });

            //查核檢測報告開窗
            $(document).on("click", "a[name='fileCheckBtn']", function () {
                $("#CheckReportGuid").val($(this).attr("aid"));
                $("#CheckReportCPGuid").val($(this).attr("cid"));
                getDataCheckFile();
                doOpenPopup();
            });

            //相關報告開窗
            $(document).on("click", "a[name='fileRelationBtn']", function () {
                $("#RelationReportGuid").val($(this).attr("aid"));
                $("#RelationReportCPGuid").val($(this).attr("cid"));
                getDataRelationFile();
                doOpenPopup2();
            });

            //清除對象內文字
            $(document).on("click", "#btn_object_delete", function () {
                $("#tObjectGuid").val('');
                $("#txt_object").val('');
            });

            $(".pickDate").datepick({
                dateFormat: 'yymmdd',
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: '../images/calendar.gif',
                yearRange: 'c-60:c+10'
            }).BootStrap(); //BootStrap() 產生符合 BootStrap 的樣式內容
        });

        //查詢資料
        function getData() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetVerificationTest.aspx",
                data: {
                    sType: $("#sel_type").val(),
                    tobject: $("#tObjectGuid").val(),
                    timeBegin: $("#txt_timeBegin").val(),
                    timeEnd: $("#txt_timeEnd").val(),
                    reportNum: $("#txt_reportNum").val(),
                    situation: $("#sel_situation").val(),
                    type: "statistics",
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
                                tabstr += '<td nowrap="nowrap">' + $(this).children("報告編號").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("類別_V").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + getDate($(this).children("查核日期起").text().trim()) + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("對象").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("改善情形_V").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap" align="center"><a href="javascript:void(0);" name="fileCheckBtn" aid="'
                                    + $(this).children("guid").text().trim() + '" cid="' + $(this).children("業者guid").text().trim() + '">'
                                    + $(this).children("查核報告總和").text().trim() + '</a></td>';
                                tabstr += '<td nowrap="nowrap" align="center"><a href="javascript:void(0);" name="fileRelationBtn" aid="'
                                    + $(this).children("guid").text().trim() + '" cid="' + $(this).children("業者guid").text().trim() + '">'
                                    + $(this).children("相關報告總和").text().trim() + '</a></td>';
                                tabstr += '<td nowrap="nowrap" align="center">' + $(this).children("報告總和").text().trim() + '</td>';
                                tabstr += '</tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="8">查詢無資料</td></tr>';
                        $("#tablist tbody").append(tabstr);

                        $("#sp_Total").text('共' + $("total", data).text() + '份報告');
                    }
                }
            });
        }

        //取得對象列表
        function getObjectData() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetVerificationTest.aspx",
                data: {
                    dataType: $("#tType").val(),
                    type: "object",
                    year: getTaiwanDate()
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
                        $("#div_objecttable").empty();
                        var tabstr = '';
                        if ($("#tType").val() != '4') {
                            tabstr += '<table width="100%" border="0" cellspacing="0" cellpadding="0"><thead><tr><th align="center" nowrap="nowrap">公司名稱</th><th align="center" nowrap="nowrap">事業部</th>' +
                                '<th align="center" nowrap="nowrap">營業處廠</th><th align="center" nowrap="nowrap">中心庫區儲運課工場</th><th align="center" nowrap="nowrap" width="100">功能</th>';
                        }
                        else {
                            tabstr += '<table width="100%" border="0" cellspacing="0" cellpadding="0"><thead><tr><th align="center" nowrap="nowrap">對象</th><th align="center" nowrap="nowrap">場站/位置</th>' +
                                '<th align="center" nowrap="nowrap" width="100">功能</th>';
                        }
                        tabstr += '</tr></thead><tbody>';

                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                tabstr += '<tr>';
                                if ($("#tType").val() != '4') {
                                    tabstr += '<td nowrap="nowrap">' + $(this).children("公司名稱").text().trim() + '</td>';
                                    tabstr += '<td nowrap="nowrap">' + $(this).children("事業部").text().trim() + '</td>';
                                    tabstr += '<td nowrap="nowrap">' + $(this).children("營業處廠").text().trim() + '</td>';
                                    tabstr += '<td nowrap="nowrap">' + $(this).children("中心庫區儲運課工場").text().trim() + '</td>';
                                    tabstr += '<td name="td_edit" nowrap="" align="center"><a class="genbtn" href="javascript:void(0);" name="objectbtn" aid="'
                                        + $(this).children("guid").text().trim() + '" cname="' + $(this).children("CompanyFullName").text().trim() + '">確認對象</a></td>';
                                    tabstr += '</tr>';
                                }
                                else {
                                    tabstr += '<td nowrap="nowrap">' + $(this).children("公司名稱").text().trim() + '</td>';
                                    tabstr += '<td nowrap="nowrap">' + $(this).children("場佔位置").text().trim() + '</td>';
                                    tabstr += '<td name="td_edit" nowrap="" align="center"><a class="genbtn" href="javascript:void(0);" name="objectbtn" aid="'
                                        + $(this).children("guid").text().trim() + '" cname="' + $(this).children("CompanyFullName").text().trim() + '">確認對象</a></td>';
                                    tabstr += '</tr>';
                                }
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="5">查詢無資料</td></tr>';
                        $("#div_objecttable").append(tabstr);

                        //確認權限&按鈕顯示或隱藏
                        //if (($("#Competence").val() == '01') || ($("#Competence").val() == '04') || ($("#Competence").val() == '05') || ($("#Competence").val() == '06')) {
                        //    $("#newbtn").hide();
                        //    $("#th_edit").hide();
                        //    $("td[name='td_edit']").hide();
                        //}
                        //else {
                        //    $("#newbtn").show();
                        //    $("#th_edit").show();
                        //    $("td[name='td_edit']").show();
                        //}
                    }
                }
            });
        }

        //取得查核檢測報告列表
        function getDataCheckFile() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/GetFile.aspx",
                data: {
                    guid: $("#CheckReportGuid").val(),
                    type: '10',
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
                        $("#tablistCheckFile tbody").empty();
                        var tabstr = '';
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                var filename = $(this).children("新檔名").text().trim();
                                var fileextension = $(this).children("附檔名").text().trim();
                                tabstr += '<tr>'
                                tabstr += '<td nowrap><a href="../DOWNLOAD.aspx?category=VerificationTest&type=Check&sn=' + $(this).children("排序").text().trim() +
                                    '&v=' + $(this).children("guid").text().trim() + '">' + filename + fileextension + '</a></td>';
                                tabstr += '<td nowrap>' + $(this).children("上傳日期").text().trim() + '</td>';
                                tabstr += '</tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="3">查詢無資料</td></tr>';
                        $("#tablistCheckFile tbody").append(tabstr);
                    }
                }
            });
        }

        //取得查核檢測報告列表
        function getDataRelationFile() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/GetFile.aspx",
                data: {
                    guid: $("#RelationReportGuid").val(),
                    type: '11',
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
                        $("#tablistRelationFile tbody").empty();
                        var tabstr = '';
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                var filename = $(this).children("新檔名").text().trim();
                                var fileextension = $(this).children("附檔名").text().trim();
                                tabstr += '<tr>'
                                tabstr += '<td nowrap><a href="../DOWNLOAD.aspx?category=VerificationTest&type=Relation&sn=' + $(this).children("排序").text().trim() +
                                    '&v=' + $(this).children("guid").text().trim() + '">' + filename + fileextension + '</a></td>';
                                tabstr += '<td nowrap>' + $(this).children("上傳日期").text().trim() + '</td>';
                                tabstr += '</tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="3">查詢無資料</td></tr>';
                        $("#tablistRelationFile tbody").append(tabstr);
                    }
                }
            });
        }

        function getDDL(gNo, id) {
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
                        $("#" + id).empty();
                        $("#" + id).append(ddlstr);
                    }
                }
            });
        }

        //對象開窗
        function doOpenMagPopup() {
            $.magnificPopup.open({
                items: {
                    src: '#messageblock'
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

        //查核/檢測報告列表 開窗
        function doOpenPopup() {
            $.magnificPopup.open({
                items: {
                    src: '#popupCheckFile'
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

        //相關報告列表 開窗
        function doOpenPopup2() {
            $.magnificPopup.open({
                items: {
                    src: '#popupRelationFile'
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

        //取得現在時間之民國年
        function getTaiwanDate() {
            var nowDate = new Date();

            var nowYear = nowDate.getFullYear();
            var nowTwYear = (nowYear - 1911);

            return nowTwYear;
        }

        function getDate(fulldate) {
            if (fulldate != '') {
                var year = fulldate.substring("0", "3");
                var month = fulldate.substring("3", "5");
                var date = fulldate.substring("5", "7");

                return year + "/" + month + "/" + date;
            }
            else {
                return fulldate;
            }
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
		<!--#include file="CommonHeader.html"-->
        <input type="hidden" id="Competence" value="<%= competence %>" />
        <input type="hidden" id="nGuid" />
        <input type="hidden" id="tType" />
        <input type="hidden" id="tObjectGuid" />
        <input type="hidden" id="CheckReportGuid" />
        <input type="hidden" id="CheckReportCPGuid" />
        <input type="hidden" id="RelationReportGuid" />
        <input type="hidden" id="RelationReportCPGuid" />
        <div id="ContentWrapper">
            <div class="container margin15T">
                <div class="padding10ALL">
                    <%--<div class="filetitlewrapper"><!--#include file="GasBreadTitle.html"--></div>--%>

                    <div class="row margin20T">
                        <div class="col-lg-3 col-md-4 col-sm-5">
                            <div id="navmenuV"><!--#include file="StatisticsLeftMenu.html"--></div>
                        </div>
                        <div class="col-lg-9 col-md-8 col-sm-7">
                            <div class="BoxBgWa BoxRadiusA BoxBorderSa padding10ALL margin10T">
                                <div class="OchiTrasTable width100 font-size3 TitleLength05">
                            <div class="OchiRow">
                                <div class="OchiHalf">
                                    <div class="OchiCell OchiTitle TitleSetWidth">類別</div>
                                    <div class="OchiCell width100"> 
                                        <select id="sel_type" class="inputex width100"></select>
                                    </div>
                                </div><!-- OchiHalf -->
                                <div class="OchiHalf">
                                    <div class="OchiCell OchiTitle TitleSetWidth">改善情形</div>
                                    <div class="OchiCell width100"> 
                                        <select id="sel_situation" class="inputex width100"></select>
                                    </div>
                                </div><!-- OchiHalf -->
                            </div><!-- OchiRow -->
                        </div><!-- OchiTrasTable -->
                        <div class="OchiTrasTable width100 font-size3 TitleLength05">
                            <div class="OchiRow">
                                <div class="OchiCell OchiTitle TitleSetWidth">對象</div>
                                <div class="OchiCell width100">
                                    <input id="txt_object" type="text" class="inputex width70" disabled /> 
                                    <a id="btn_object" href="javascript:void(0);" title="請選擇" class="grebtn font-size3">請選擇</a>
                                    <a id="btn_object_delete" href="javascript:void(0);" title="清除" class="grebtn font-size3">清除</a>
                                </div>
                            </div>
                        </div>

                        <div class="OchiTrasTable width100 font-size3 TitleLength05">
                            <div class="OchiRow">
                                <div class="OchiHalf">
                                    <div class="OchiCell OchiTitle TitleSetWidth">查核期間</div>
                                    <div class="OchiCell width100"><input id="txt_timeBegin" type="text" class="inputex pickDate width30" disabled> ~ 
                                        <input id="txt_timeEnd" type="text" class="inputex pickDate width30" disabled> 
                                    </div>
                                </div><!-- OchiHalf -->
                                <div class="OchiHalf">
                                    <div class="OchiCell OchiTitle TitleSetWidth">報告編號</div>
                                    <div class="OchiCell width100"><input id="txt_reportNum" type="text" class="inputex width100"></div>
                                </div><!-- OchiHalf -->
                            </div><!-- OchiRow -->
                        </div><!-- OchiTrasTable -->
                                <br />
                                <div class="twocol">
                                    <div class="left">
                                        <span id="sp_Total" style="color:red" class="font-size5"></span>
                                    </div>
                                    <div class="right">
                                        <a id="querybtn" href="javascript:void(0);" title="查詢" class="genbtn" >查詢</a>
                                    </div>
                                </div>
                            </div>
                            <br />

                            <div id="div_table" class="stripeMeB tbover">
                                <table id="tablist" width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th nowrap="nowrap" width="8%">報告編號</th>
                                            <th nowrap="nowrap" width="10%">類別</th>
                                            <th nowrap="nowrap" width="5%">查核日期</th>
                                            <th nowrap="nowrap">對象</th>
                                            <th nowrap="nowrap" width="7%">改善情形</th>
                                            <th nowrap="nowrap" width="7%">查核/檢測報告</th>
                                            <th nowrap="nowrap" width="7%">相關報告</th>
                                            <th nowrap="nowrap" width="7%">總計</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                            </div><!-- stripeMe -->
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
	
		<!--#include file="Footer.html"-->

</div><!-- BoxBgWa -->
<!-- 側邊選單內容:動態複製主選單內容 -->
<div id="sidebar-wrapper"></div><!-- sidebar-wrapper -->

</form>
</div>
<!-- 結尾用div:修正mmenu form bug -->

<!-- Magnific Popup -->
<div id="messageblock" class="magpopup magSizeL mfp-hide">
  <div class="magpopupTitle">對象列表</div>
  <div class="padding10ALL">
      <div class="twocol">
          <div class="left font-size5 ">

          </div>
          <div class="right">

          </div>
      </div><br />
      <div id="div_objecttable" class="stripeMeB tbover">

      </div>
      <br />

  </div><!-- padding10ALL -->

</div><!--magpopup -->

<div id="popupCheckFile" class="magpopup magSizeM mfp-hide">
    <div class="magpopupTitle">查核/檢測報告列表</div>
    <div class="padding10ALL">
        <div class="twocol">
          <div class="left font-size4"></div>
          <div class="right">

          </div>
        </div>
        </br>
        <div class="overtablewidth margin5T">
           <table class="overtablewidthS" width="100%">
             <tr>
               <td>
                 <!-- start -->
                 <div class="stripeMeB">
                   <table id="tablistCheckFile" cellspacing="0" cellpadding="0" width="100%">
                       <thead>
                           <tr>
                             <th align="center" nowrap>文件名稱</th>
                             <th align="center" nowrap>上傳日期</th>
                           </tr>
                       </thead>
                       <tbody></tbody>
                   </table>
                 </div><!-- stripeMe -->
                 <!-- end -->
               </td>
             </tr>
           </table>
        </div><!-- overtablewidth -->
    </div>
    <!-- padding10ALL -->
</div>

<div id="popupRelationFile" class="magpopup magSizeM mfp-hide">
    <div class="magpopupTitle">相關報告列表</div>
    <div class="padding10ALL">
        <div class="twocol">
          <div class="left font-size4"></div>
          <div class="right">

          </div>
        </div>
        </br>
        <div class="overtablewidth margin5T">
           <table class="overtablewidthS" width="100%">
             <tr>
               <td>
                 <!-- start -->
                 <div class="stripeMeB">
                   <table id="tablistRelationFile" cellspacing="0" cellpadding="0" width="100%">
                       <thead>
                           <tr>
                             <th align="center" nowrap>文件名稱</th>
                             <th align="center" nowrap>上傳日期</th>
                           </tr>
                       </thead>
                       <tbody></tbody>
                   </table>
                 </div><!-- stripeMe -->
                 <!-- end -->
               </td>
             </tr>
           </table>
        </div><!-- overtablewidth -->
    </div>
    <!-- padding10ALL -->
</div>

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



