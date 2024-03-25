<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OilStorageTankInfo.aspx.cs" Inherits="WebPage_OilStorageTankInfo" %>

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
            //getYearList();
            //$("#sellist").val(getTaiwanDate());
            getData(0);
            getData2(0);
            $("#confirmbtn").attr("aid", "1");
            $("#confirmbtn2").attr("aid", "2");
            $("#exportbtn").attr("href", "../Oil_EXPORTEXCEL.aspx?cpid=" + $.getQueryString("cp") + "&category=storagetankinfo");
            $("#exportbtn2").attr("href", "../Oil_EXPORTEXCEL.aspx?cpid=" + $.getQueryString("cp") + "&category=storagetankinfoliquefaction");

            //選擇年份
            //$(document).on("change", "#sellist", function () {
            //    getData($("#sellist option:selected").val());
            //});

            //新增按鈕
            $(document).on("click", "#newbtn", function () {
                location.href = "edit_OilStorageTankInfo.aspx?cp=" + $.getQueryString("cp");
            });

            //新增按鈕-液化石油氣儲槽
            $(document).on("click", "#newbtn2", function () {
                location.href = "edit_OilStorageTankInfoLiquefaction.aspx?cp=" + $.getQueryString("cp");
            });

            //刪除按鈕
            $(document).on("click", "a[name='delbtn']", function () {
                if (confirm("確定刪除?")) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../handler/DelOilStorageTankInfo.aspx",
                        data: {
                            type: "data",
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
                                getData($("#pageint").val());
                            }
                        }
                    });
                }
            });

            //刪除按鈕-液化石油氣儲槽
            $(document).on("click", "a[name='delbtn2']", function () {
                if (confirm("確定刪除?")) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../handler/DelOilStorageTankInfoLiquefaction.aspx",
                        data: {
                            type: "data",
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
                                getData2($("#pageint2").val());
                            }
                        }
                    });
                }
            });

            //今年度全部資料刪除按鈕-常壓地上式儲槽
            $(document).on("click", "#delallbtn", function () {
                if (confirm("將會刪除本年度常壓地上式儲槽【" + getTaiwanDate() + "】年度之資料，確定刪除?")) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../handler/DelOilStorageTankInfo.aspx",
                        data: {
                            type: "all",
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

            //今年度全部資料刪除按鈕-液化石油氣儲槽
            $(document).on("click", "#delallbtn2", function () {
                if (confirm("將會刪除本年度液化石油氣儲槽【" + getTaiwanDate() + "】年度之資料，確定刪除?")) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../handler/DelOilStorageTankInfoLiquefaction.aspx",
                        data: {
                            type: "all",
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
                                getData2(0);
                            }
                        }
                    });
                }
            });

            //年度儲槽確認按鈕
            $(document).on("click", "a[name='confirmbtnName']", function () {
                var storageType = $(this).attr("aid");
                if (confirm("確認更新?")) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../handler/AddOilStorageTankInfoLiquefaction.aspx",
                        data: {
                            cp: $.getQueryString("cp"),
                            storageType: storageType,
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
                                alert($("Response", data).text());
                                if (storageType == '1') {
                                    $("#confirmbtn").hide();
                                    $("#sp_confirmbtn").show();
                                }
                                else {
                                    $("#confirmbtn2").hide();
                                    $("#sp_confirmbtn2").show();
                                }
                                    
                            }
                        }
                    });
                }
            });

            //匯入開窗-常壓地上式儲槽
            $(document).on("click", "#importbtn", function () {
                $("#importFile").val('');
                doOpenMagPopup();
            });

            //匯入按鈕-常壓地上式儲槽
            $(document).on("click", "#importSubbtn", function () {
                if (confirm('請確認上傳的檔案內資料格式是否與範例檔案相同?')) {
                    // Get form
                    var form = $('#form1')[0];

                    // Create an FormData object 
                    var data = new FormData(form);

                    // If you want to add an extra field for the FormData
                    data.append("cpid", $.getQueryString("cp"));
                    data.append("year", getTaiwanDate());
                    data.append("category", "storagetankinfo");
                    $.each($("#importFile")[0].files, function (i, file) {
                        data.append('file', file);
                    });

                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../handler/OilImport.aspx",
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
                                getData($("#pageint").val());
                                $.magnificPopup.close();
                            }
                        }
                    });
                }
            });

            //匯入開窗-液化石油氣儲槽
            $(document).on("click", "#importbtn2", function () {
                $("#importFile2").val('');
                doOpenMagPopup2();
            });

            //匯入按鈕-液化石油氣儲槽
            $(document).on("click", "#importSubbtn2", function () {
                if (confirm('請確認上傳的檔案內資料格式是否與範例檔案相同?')) {
                    // Get form
                    var form = $('#form1')[0];

                    // Create an FormData object 
                    var data = new FormData(form);

                    // If you want to add an extra field for the FormData
                    data.append("cpid", $.getQueryString("cp"));
                    data.append("year", getTaiwanDate());
                    data.append("category", "storagetankinfoliquefaction");
                    $.each($("#importFile2")[0].files, function (i, file) {
                        data.append('file', file);
                    });

                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../handler/OilImport.aspx",
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
                                getData2($("#pageint").val());
                                $.magnificPopup.close();
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
				url: "../Handler/GetOilStorageTankInfo.aspx",
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
								tabstr += '<td nowrap="nowrap">' + $(this).children("轄區儲槽編號").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("能源局編號").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("容量").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("內徑").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("內容物").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("油品種類").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("形式").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("啟用日期").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("代行檢查_代檢機構1").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + getDate($(this).children("代行檢查_外部日期1").text().trim()) + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("代行檢查_代檢機構2").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + getDate($(this).children("代行檢查_外部日期2").text().trim()) + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("狀態").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("延長開放年限").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("差異說明").text().trim() + '</td>';
                                if ($("#Competence").val() == '02') {
                                    tabstr += '<td name="td_edit" nowrap="" align="center"><a href="edit_OilStorageTankInfo.aspx?cp=' + $.getQueryString("cp") + '&guid=' + $(this).children("guid").text().trim() + '" name="editbtn">編輯</a></td>';
                                }
                                else {
                                    tabstr += '<td name="td_edit" nowrap="" align="center"><a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("guid").text().trim() + '">刪除</a>';
                                    tabstr += ' <a href="edit_OilStorageTankInfo.aspx?cp=' + $.getQueryString("cp") + '&guid=' + $(this).children("guid").text().trim() + '" name="editbtn">編輯</a></td>';
                                }                                
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="16">查詢無資料</td></tr>';
                        $("#tablist tbody").append(tabstr);
                        Page.Option.Selector = "#pageblock";
                        Page.Option.FunctionName = "getData";
                        Page.CreatePage(p, $("total", data).text());
                        $("#pageint").val(p);

                        //確認權限&按鈕顯示或隱藏
                        if (($("#Competence").val() == '01') || ($("#Competence").val() == '04') || ($("#Competence").val() == '05') || ($("#Competence").val() == '06')) {
                            $("#importbtn").hide();
                            $("#newbtn").hide();
                            $("#th_edit").hide();
                            $("td[name='td_edit']").hide();
                        }
                        else {
                            $("#importbtn").show();
                            $("#newbtn").show();
                            $("#th_edit").show();
                            $("td[name='td_edit']").show();
                        }

                        if (($("#Competence").val() == '02'))
                            $("#delallbtn").hide();

                        getConfirmedStatus();
					}
				}
			});
        }

        function getData2(p) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetOilStorageTankInfoLiquefaction.aspx",
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
                        $("#tablist2 tbody").empty();
                        var tabstr = '';
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                tabstr += '<tr>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("轄區儲槽編號").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("能源局編號").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("容量").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("內徑").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("內容物").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("油品種類").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("形式").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("啟用日期").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("狀態").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("差異說明").text().trim() + '</td>';
                                if ($("#Competence").val() == '02') {
                                    tabstr += '<td name="td_edit2" nowrap="" align="center"><a href="edit_OilStorageTankInfoLiquefaction.aspx?cp=' + $.getQueryString("cp") + '&guid=' + $(this).children("guid").text().trim() + '" name="editbtn2">編輯</a></td>';
                                }
                                else {
                                    tabstr += '<td name="td_edit2" nowrap="" align="center"><a href="javascript:void(0);" name="delbtn2" aid="' + $(this).children("guid").text().trim() + '">刪除</a>';
                                    tabstr += ' <a href="edit_OilStorageTankInfoLiquefaction.aspx?cp=' + $.getQueryString("cp") + '&guid=' + $(this).children("guid").text().trim() + '" name="editbtn2">編輯</a></td>';
                                }                                
                                tabstr += '</tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="11">查詢無資料</td></tr>';
                        $("#tablist2 tbody").append(tabstr);
                        Page.Option.Selector = "#pageblock2";
                        Page.Option.FunctionName = "getData2";
                        Page.CreatePage(p, $("total", data).text());
                        $("#pageint2").val(p);

                        //確認權限&按鈕顯示或隱藏
                        if (($("#Competence").val() == '01') || ($("#Competence").val() == '04') || ($("#Competence").val() == '05') || ($("#Competence").val() == '06')) {
                            $("#newbtn2").hide();
                            $("#importbtn2").hide();
                            $("#th_edit2").hide();
                            $("td[name='td_edit2']").hide();
                        }
                        else {
                            $("#newbtn2").show();
                            $("#importbtn2").show();
                            $("#th_edit2").show();
                            $("td[name='td_edit2']").show();
                        }

                        if (($("#Competence").val() == '02'))
                            $("#delallbtn2").hide();

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
                                var yearStorageConfirm = $(this).children("年度儲槽確認").text().trim();
                                var yearStorageLiquefactionConfirm = $(this).children("年度液化石油氣儲槽確認").text().trim();
                                var dataConfirm = $(this).children("資料是否確認").text().trim();

                                if ((yearStorageConfirm == '') || (yearStorageConfirm != getTaiwanDate())) {
                                    $("#confirmbtn").show();
                                    $("#sp_confirmbtn").hide();
                                }
                                else {
                                    $("#confirmbtn").hide();
                                    $("#sp_confirmbtn").show();
                                }                                    

                                if ((yearStorageLiquefactionConfirm == '') || (yearStorageLiquefactionConfirm != getTaiwanDate())) {
                                    $("#confirmbtn2").show();
                                    $("#sp_confirmbtn2").hide();
                                }
                                else {
                                    $("#confirmbtn2").hide();
                                    $("#sp_confirmbtn2").show();
                                }

                                if ($("#Competence").val() != '03') {
                                    if (dataConfirm == "是") {
                                        $("#newbtn").hide();
                                        $("#editbtn").hide();
                                        $("#th_edit").hide();
                                        $("td[name='td_edit']").hide();
                                        $("#newbtn2").hide();
                                        $("#editbtn2").hide();
                                        $("#th_edit2").hide();
                                        $("td[name='td_edit2']").hide();
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
                url: "../Handler/GetOilStorageTankInfo.aspx",
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

        function doOpenMagPopup() {
            $.magnificPopup.open({
                items: {
                    src: '#importDialog'
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

        function doOpenMagPopup2() {
            $.magnificPopup.open({
                items: {
                    src: '#importDialog2'
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
        <input type="hidden" id="pageint" />
        <input type="hidden" id="pageint2" />
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
                                <div class="left font-size5 ">
                                    <a id="confirmbtn" name="confirmbtnName" href="javascript:void(0);" title="年度儲槽確認" class="genbtn">年度儲槽確認</a>
                                    <span id="sp_confirmbtn" class="IconCb font-size2"><i class="fa fa-check-square-o" aria-hidden="true"></i>今年度已確認</span>
                                </div>
                                <div class="right">
                                    <a id="importbtn" href="javascript:void(0);" title="匯入" class="genbtn">匯入</a>
                                    <a id="exportbtn" href="javascript:void(0);" title="匯出" class="genbtn">匯出</a>
                                    <a id="delallbtn" href="javascript:void(0);" title="全部刪除" class="genbtn">全部刪除</a>
                                    <a id="newbtn" href="javascript:void(0);" title="新增" class="genbtn">新增</a>
                                </div>
                            </div><br />
                            <div class="font-size4 font-bold">常壓地上式儲槽</div>
                            <div class="stripeMeB tbover">
                                <table id="tablist" width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th nowrap  rowspan="2">轄區儲槽編號 </th>
                                            <th nowrap rowspan="2">能源署編號 </th>
                                            <th nowrap rowspan="2">設計容量 <br>
                                                （公秉） </th>
                                            <th nowrap rowspan="2">儲槽內徑 <br>
                                                (公尺） </th>
                                            <th nowrap rowspan="2">內容物 <br>
                                                (中文)<br>
                                                (1)
                                            </th>
                                            <th nowrap rowspan="2">油品種類 <br>
                                                (2)
                                            </th>
                                            <th nowrap rowspan="2">形式 <br>
                                                1.錐頂 <br>
                                                2.內浮頂 <br>
                                                3.外浮頂 <br>
                                                4.掩體式 <br>
                                                8.其他 </th>
                                            <th nowrap rowspan="2">啟用日期 <br>
                                                年/月 </th>
                                            <th nowrap colspan="4">代行檢查有效期限 <br>
                                                (3)
                                            </th>
                                            <th nowrap rowspan="2" valign="top">狀態 <br>
                                                1.使用中 <br>
                                                2.開放中 <br>
                                                3.停用 <br>
                                                4.其他 <br>
                                                (4)
                                            </th>
                                            <th nowrap rowspan="2">延長開 <br>
                                                放年限 <br>
                                                多?年 <br>
                                                (5)
                                            </th>
                                            <th nowrap rowspan="2">差異說明<br>
                                                (內容物名<br>
                                                稱/油品種<br>
                                                類...)
                                            </th>
                                            <th id="th_edit" rowspan="2">功能</th>
                                        </tr>
                                        <tr>
                                            <th >代檢機構 <br>
                                                (填表說明) </th>
                                            <th >外部 <br>
                                                年/月/日 </th>
                                            <th >代檢機構 <br>
                                                (填表說明) </th>
                                            <th >內部 <br>
                                                年/月/日 </th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div><!-- stripeMe -->
                            <div class="margin10B margin10T textcenter">
	                            <div id="pageblock"></div>
	                        </div>
                            <div class="margin5TB font-size2">
                                (1) 儲槽基本資料淺藍色欄位以儲運處提供每年1月2日之儲槽狀態匯入，匯入後之資料不可編輯，若與實際有差異，<br>
                                請於最右欄填寫差異，確認完成請按「年度儲槽確認」鍵。<br>
                                (2) 內容物：請填寫中文名稱，請勿填寫英文或縮寫。<br>
                                (3) 油品種類：1.原油，2.汽油，3.柴油，4.煤油，5.輕油，6.液化石油氣，7.航空燃油，8.燃料油，9.其他(水、空槽、潤滑油等)，<br>
                                請直接點選，油品定義請依照「石油製品認定基準」附表一。<i class="fa fa-file-pdf-o IconCc" aria-hidden="true"></i><a href="../doc/石油製品認定基準-附表一-1080117.pdf" target="_blank">下載</a><br>
                                (4)	代檢機構：1：中國石油學會；2.中華壓力容器協會；3.中華勞動學會；4.中華機械產業設備發展協會，請直接點選。<br>
                                (5)	代行檢查有效期限欄位於查核前再更新即可。<br>
                                (6)	延長開放年限：若儲槽有申請延長開放，請填入核可延長之年限，無則填寫0。<br>
                            </div>
                            <br />
                            <br />
                            <div class="twocol">
                                <div class="left font-size5 ">
                                    <a id="confirmbtn2" name="confirmbtnName" href="javascript:void(0);" title="年度儲槽確認" class="genbtn">年度儲槽確認</a>
                                    <span id="sp_confirmbtn2" class="IconCb font-size2"><i class="fa fa-check-square-o" aria-hidden="true"></i>今年度已確認</span>
                                </div>
                                <div class="right">
                                    <a id="importbtn2" href="javascript:void(0);" title="匯入" class="genbtn">匯入</a>
                                    <a id="exportbtn2" href="javascript:void(0);" title="匯出" class="genbtn">匯出</a>
                                    <a id="delallbtn2" href="javascript:void(0);" title="全部刪除" class="genbtn">全部刪除</a>
                                    <a id="newbtn2" href="javascript:void(0);" title="新增" class="genbtn">新增</a>
                                </div>
                            </div><br />
                            <div class="font-size4 font-bold">液化石油氣儲槽</div>
                            <div class="stripeMeB tbover">
                                <table id="tablist2" width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th nowrap  >轄區儲槽編號 </th>
                                            <th nowrap >能源署編號 </th>
                                            <th nowrap >設計容量 <br>
                                                （公秉） </th>
                                            <th nowrap >儲槽內徑 <br>
                                                (公尺） </th>
                                            <th nowrap >內容物 </th>
                                            <th nowrap >油品種類 <br>
                                                (1)
                                            </th>
                                            <th nowrap >形式 <br>
                                                5.ECT <br>
                                                6.球型壓力容器 <br>
                                                7.低溫儲槽 <br>
                                                8.其他 </th>
                                            <th nowrap >啟用日期 <br>
                                                年/月 </th>
                                            <th nowrap valign="top">狀態 <br>
                                                1.使用中 <br>
                                                2.開放中 <br>
                                                3.停用 <br>
                                                4.其他 <br>
                                                (4)
                                            </th>
                                            <th nowrap rowspan="2">差異說明<br>
                                                (內容物名<br>
                                                稱/油品種<br>
                                                類...)
                                            </th>
                                            <th id="th_edit2" >功能</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div><!-- stripeMe -->
                            <div class="margin10B margin10T textcenter">
	                            <div id="pageblock2"></div>
	                        </div>
                            <div class="margin5TB font-size2">
                                (1) 儲槽基本資料淺藍色欄位以儲運處提供每年1月2日之儲槽狀態匯入，匯入後之資料不可編輯，若與實際有差異，<br>
                                請於最右欄填寫差異，確認完成請按「年度儲槽確認」鍵。<br>
                                (2)	油品種類：1.原油，2.汽油，3.柴油，4.煤油，5.輕油，6.液化石油氣，7.航空燃油，8.燃料油，9.其他(水、空槽、潤滑油等)，<br>
                                請直接點選，油品定義除原油外請依照「石油製品認定基準」附表一。<i class="fa fa-file-pdf-o IconCc" aria-hidden="true"></i><a href="../doc/石油製品認定基準-附表一-1080117.pdf" target="_blank">下載</a>
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

<!-- Magnific Popup -->
<div id="importDialog" class="magpopup magSizeS mfp-hide">
  <div class="magpopupTitle">匯入資料</div>
<div class="padding10ALL">
      <div class="OchiTrasTable width100 TitleLength08 font-size3">
          <div class="OchiRow">
              <div class="OchiCell OchiTitle IconCe TitleSetWidth">範例</div>
              <div class="OchiCell width100">
                  <i class="fa fa-file-excel-o IconCc" aria-hidden="true"></i><a href="../doc/import/Oil/石油_儲槽基本資料.xls">下載</a>
              </div>
          </div><!-- OchiRow -->
          <div class="OchiRow">
              <div class="OchiCell OchiTitle IconCe TitleSetWidth">匯入檔案</div>
              <div class="OchiCell width100">
                  <input id="importFile" type="file" class="inputex width100 font-size2" />
              </div>
          </div><!-- OchiRow -->
      </div><!-- OchiTrasTable -->

      <div class="twocol margin10T">
            <div class="right">
                <a id="importCancelbtn" href="javascript:void(0);" class="genbtn closemagnificPopup">取消</a>
                <a id="importSubbtn" href="javascript:void(0);" class="genbtn">上傳</a>
            </div>
        </div>

  </div><!-- padding10ALL -->

</div><!--magpopup -->

<!-- Magnific Popup -->
<div id="importDialog2" class="magpopup magSizeS mfp-hide">
  <div class="magpopupTitle">匯入資料</div>
<div class="padding10ALL">
      <div class="OchiTrasTable width100 TitleLength08 font-size3">
          <div class="OchiRow">
              <div class="OchiCell OchiTitle IconCe TitleSetWidth">範例</div>
              <div class="OchiCell width100">
                  <i class="fa fa-file-excel-o IconCc" aria-hidden="true"></i><a href="../doc/import/Oil/石油_液化石油氣儲槽基本資料.xls">下載</a>
              </div>
          </div><!-- OchiRow -->
          <div class="OchiRow">
              <div class="OchiCell OchiTitle IconCe TitleSetWidth">匯入檔案</div>
              <div class="OchiCell width100">
                  <input id="importFile2" type="file" class="inputex width100 font-size2" />
              </div>
          </div><!-- OchiRow -->
      </div><!-- OchiTrasTable -->

      <div class="twocol margin10T">
            <div class="right">
                <a id="importCancelbtn2" href="javascript:void(0);" class="genbtn closemagnificPopup">取消</a>
                <a id="importSubbtn2" href="javascript:void(0);" class="genbtn">上傳</a>
            </div>
        </div>

  </div><!-- padding10ALL -->

</div><!--magpopup -->

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

