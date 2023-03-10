<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GasCIPS.aspx.cs" Inherits="WebPage_GasCIPS" %>

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
            getYearList();
            $("#sellist").val(getTaiwanDate());
            getData(getTaiwanDate());
            $("#exportbtn").attr("href", "../Gas_EXPORTEXCEL.aspx?cpid=" + $.getQueryString("cp") + "&year=" + getTaiwanDate() + "&category=CIPS");

            //選擇年份
            $(document).on("change", "#sellist", function () {
                getData($("#sellist option:selected").val());
                $("#exportbtn").attr("href", "../Gas_EXPORTEXCEL.aspx?cpid=" + $.getQueryString("cp") + "&year=" + $("#sellist option:selected").val() + "&category=CIPS");
            });

            //新增按鈕
            $(document).on("click", "#newbtn", function () {
                location.href = "edit_GasCIPS.aspx?cp=" + $.getQueryString("cp");
            });

            //刪除按鈕
            $(document).on("click", "a[name='delbtn']", function () {
                if (confirm("確定刪除?")) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../handler/DelGasCIPS.aspx",
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

            //刪除附件
            $(document).on("click", "a[name='delbtnFile']", function () {
                var isDel = confirm("確定刪除檔案嗎?");
                if (isDel) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../Handler/DelGasCIPSFile.aspx",
                        data: {
                            cpid: $.getQueryString("cp"),
                            sn: $(this).attr("sn"),
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
                                alert("刪除完成");

                                GetFileList();
                                getExtension();
                            }
                        }
                    });
                }
            });

            //附件列表開窗
            $(document).on("click", "a[name='fileListBtn']", function () {
                $("#CGguid").val($(this).attr("aid"));

                GetFileList();
                getExtension();
                doOpenMagPopup();
            });

            //座標列表開窗
            $(document).on("click", "a[name='coordinateBtn']", function () {
                $("#CoGguid").val($(this).attr("aid"));
                getCoordinate();
                doOpenMagPopup2();
            });

            //新增座標
            $(document).on("click", "#newbtnxy", function () {
                $("#Gguid").val("");
                $("#typeName").html("新增座標");
                $("#txt1").val("");
                $("#txt2").val("");
                $("#txt3").val("");
                doOpenMagPopup3();
            });

            //編輯座標
            $(document).on("click", "a[name='editbtnxy']", function () {
                $("#Gguid").val($(this).attr("aid"));
                $("#typeName").html("編輯座標");
                getCoordinateData();
                doOpenMagPopup3();
            });

            //取消 新增/編輯座標
            $(document).on("click", "#cancelbtn", function () {
                $("#CoGguid").val();
                getCoordinate();
                doOpenMagPopup2();
            });

            //座標儲存
            $(document).on("click", "#subbtn", function () {
                var msg = '';

                if ($("#txt1").val() == '')
                    msg += "請輸入【x座標】\n";
                if ($("#txt2").val() == '')
                    msg += "請輸入【y座標】\n";
                if ($("#txt3").val() == '')
                    msg += "請輸入【級距】\n";

                if (msg != "") {
                    alert("Error message: \n" + msg);
                    return false;
                }

                // Get form
                var form = $('#form1')[0];

                // Create an FormData object 
                var data = new FormData(form);

                var mode = ($("#Gguid").val() == "") ? "new" : "edit";

                // If you want to add an extra field for the FormData
                data.append("cp", $.getQueryString("cp"));
                data.append("pGuid", $("#CoGguid").val());
                data.append("guid", $("#Gguid").val());
                data.append("mode", encodeURIComponent(mode));
                data.append("year", encodeURIComponent(getTaiwanDate()));
                data.append("txt1", encodeURIComponent($("#txt1").val()));
                data.append("txt2", encodeURIComponent($("#txt2").val()));
                data.append("txt3", encodeURIComponent($("#txt3").val()));

                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "../handler/AddGasCIPSxy.aspx",
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

                            $("#CoGguid").val();
                            getCoordinate();
                            doOpenMagPopup2();
                        }
                    }
                });
            });

            //刪除座標
            $(document).on("click", "a[name='delbtn2']", function () {
                if (confirm("確定刪除?")) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../handler/DelGasCIPSxy.aspx",
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
                                getCoordinate();
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
				url: "../Handler/GetGasCIPS.aspx",
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
								tabstr += '<td nowrap="nowrap">' + $(this).children("同時檢測管線數量").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("最近一次執行年月").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("報告產出年月").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("檢測長度").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("合格標準請參照填表說明2").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("立即改善_數量").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("立即改善_改善完成數量").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("排程改善_數量").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("排程改善_改善完成數量").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("需監控點_數量").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("備註").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap" align="center"><a href="javascript:void(0);" name="fileListBtn" class="grebtn" aid="' + $(this).children("guid").text().trim() + '">附件列表</a></td>';
                                tabstr += '<td nowrap="nowrap" align="center"><a href="javascript:void(0);" name="coordinateBtn" class="grebtn" aid="' + $(this).children("guid").text().trim() + '">座標列表</a></td>';
                                tabstr += '<td name="td_edit" nowrap="" align="center"><a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("guid").text().trim() + '">刪除</a>';
                                tabstr += ' <a href="edit_GasCIPS.aspx?cp=' + $.getQueryString("cp") + '&guid=' + $(this).children("guid").text().trim() + '" name="editbtn">編輯</a></td>';
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
                url: "../Handler/GetGasCIPS.aspx",
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

        //附件列表
        function GetFileList() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/GetFile.aspx",
                data: {
                    cpid: $.getQueryString("cp"),
                    guid: $("#CGguid").val(),
                    year: getTaiwanDate(),
                    type: "09",
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
                        $("#tablistFile tbody").empty();
                        var tabstr = '';
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                var filename = $(this).children("新檔名").text().trim();
                                var fileextension = $(this).children("附檔名").text().trim();
                                tabstr += '<tr>';
                                tabstr += '<td nowrap="nowrap">';
                                tabstr += '<img width="200px" height="200px" name="img_' + $(this).children("guid").text().trim() + $(this).children("排序").text().trim() + '" src="../DOWNLOAD.aspx?category=Gas&type=CIPS&sn=' + $(this).children("排序").text().trim() +
                                    '&v=' + $(this).children("guid").text().trim() + '" alt="' + filename + fileextension + '" style="display:none" >';
                                tabstr += '<a name="a_' + $(this).children("guid").text().trim() + $(this).children("排序").text().trim() + '" href="../DOWNLOAD.aspx?category=Gas&type=CIPS&sn=' + $(this).children("排序").text().trim() +
                                    '&v=' + $(this).children("guid").text().trim() + '" style="display:none" >' + filename + fileextension + '</a>';
                                tabstr += '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("上傳日期").text().trim() + '</td>';
                                tabstr += '<td name="td_editFile" nowrap="" align="center"><a href="javascript:void(0);" name="delbtnFile" aid="' + $(this).children("guid").text().trim() +
                                    '" sn="' + $(this).children("排序").text().trim() + '">刪除</a></td>';
                                tabstr += '</tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="3">查詢無資料</td></tr>';
                        $("#tablistFile tbody").append(tabstr);

                        //確認權限&按鈕顯示或隱藏
                        if (($("#Competence").val() == '01') || ($("#Competence").val() == '03')) {
                            $("#thFunc").show();
                            $("td[name='td_editFile']").show();
                        }
                        else {
                            $("#thFunc").hide();
                            $("td[name='td_editFile']").hide();
                        }
                    }
                }
            });
        }

        function getCoordinate() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetGasCIPSxy.aspx",
                data: {
                    pGuid: $("#CoGguid").val(),
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
                        $("#tablistcoordinate tbody").empty();
                        var tabstr = '';
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                tabstr += '<tr>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("x座標").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("y座標").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("級距").text().trim() + '</td>';
                                tabstr += '<td name="td_editCoordinate" nowrap="" align="center"><a href="javascript:void(0);" name="delbtn2" aid="' + $(this).children("guid").text().trim() + '">刪除</a> ';
                                tabstr += '<a href="javascript:void(0);" name="editbtnxy" mid="edit" aid="' + $(this).children("guid").text().trim() + '">編輯</a></td>'
                                tabstr += '</tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="4">查詢無資料</td></tr>';
                        $("#tablistcoordinate tbody").append(tabstr);

                        //確認權限&按鈕顯示或隱藏
                        if (($("#Competence").val() == '01') || ($("#Competence").val() == '03')) {
                            $("#thFunc2").show();
                            $("td[name='td_editCoordinate']").show();
                        }
                        else {
                            $("#thFunc2").hide();
                            $("td[name='td_editCoordinate']").hide();
                        }
                    }
                }
            });
        }

        function getCoordinateData() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetGasCIPSxy.aspx",
                data: {
                    guid: $("#Gguid").val(),
                    type: "data",
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
                                $("#txt1").val($(this).children("x座標").text().trim());
                                $("#txt2").val($(this).children("y座標").text().trim());
                                $("#txt3").val($(this).children("級距").text().trim());
                            });
                        }
                    }
                }
            });
        }

        function getExtension() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/GetFile.aspx",
                data: {
                    cpid: $.getQueryString("cp"),
                    guid: $("#CGguid").val(),
                    year: getTaiwanDate(),
                    type: "09",
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
                                var fileextension = $(this).children("附檔名").text().trim();
                                if (fileextension == ".jpg" || fileextension == ".jpeg" || fileextension == ".png") {
                                    $("img[name='img_" + $(this).children("guid").text().trim() + $(this).children("排序").text().trim() + "']").show();
                                }
                                else {
                                    $("a[name='a_" + $(this).children("guid").text().trim() + $(this).children("排序").text().trim() + "']").show();
                                }
                            });
                        }
                    }
                }
            });
        }

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

        function doOpenMagPopup2() {
            $.magnificPopup.open({
                items: {
                    src: '#messageblock2'
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

        function doOpenMagPopup3() {
            $.magnificPopup.open({
                items: {
                    src: '#messageblock3'
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
        <input type="hidden" id="CGguid" />
        <input type="hidden" id="CoGguid" />
        <input type="hidden" id="Gguid" />
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
                                <div class="left font-size5 "><i class="fa fa-chevron-circle-right IconCa" aria-hidden="true"></i> 
                                    <select id="sellist" class="inputex">
                                    </select> 年
                                </div>
                                <div class="right">
                                    <a id="exportbtn" href="javascript:void(0);" title="匯出" class="genbtn">匯出</a>
                                    <a id="newbtn" href="javascript:void(0);" title="新增" class="genbtn">新增</a>
                                </div>
                            </div><br />
                            <div class="stripeMeG tbover">
								<table id="tablist" width="100%" border="0" cellspacing="0" cellpadding="0">
									<thead>
										<tr>
                                        <th rowspan="2">長途管線<br />識別碼</th>
										<th rowspan="2">同時檢測管線數量</th>
										<th rowspan="2">折線圖產出日期<br />年/月</th>
										<th rowspan="2">複判會議日期<br />年/月</th>
										<th rowspan="2">檢測長度<br />(公里)</th>
										<th rowspan="2">合格標準<br />請參照<br />填表說明(2)</th>
										<th colspan="2">立即改善</th>
										<th colspan="2">排程改善</th>
										<th>需監控點</th>
										<th rowspan="2">備註</th>
                                        <th  rowspan="2">附件 </th>
                                        <th  rowspan="2">異常點尚未改善完成之座標 </th>
										<th id="th_edit" rowspan="2">功能 </th>
                                    </tr>
                                    <tr>
                                        <th>數量</th>
										<th>改善完成數量</th>
										<th>數量</th>
										<th>改善完成數量</th>
										<th>數量</th>
                                    </tr>
									</thead>
									<tbody></tbody>
								</table>
                            </div><!-- stripeMe --><br />
                            <div class="margin5TB font-size2">
                                填表說明：<br />
                                (1) 有執行過CIPS的管線才須填寫<br>
                                (2) 同時檢測管線數量：進行緊密電位檢測時，同時檢測到的管線數量（管束管線數量）。<br>
                                (3) 折線圖產出日期(年/月)：收到折線圖之日期<br>
                                (4) 複判會議日期(年/月)：該管線最後一次複判會議之日期<br>
                                (5) 合格標準：請依據該管線檢測報告判定結果時，所引用之標準，請填入相對應之數字，<br>
                                    1. 通電電位< -850mVCSE  2.極化電位< -850mVCSE  3.極化量>100mV  4.其他 <br>
                                (6) 立即改善、排程改善<br>
                                    A.數量：依據公司之檢測合格標準，最後一次複判會議所判定立即改善(排程改善)的點數。<br>
                                    B.改善完成數量：依據公司之檢測合格標準，須立即改善(排程改善)且已完成改善之點數。<br>
                                (7) 需監控點數量：依據公司之檢測合格標準，最後一次複判會議須監控之數量。<br>
                                (8) 異常點尚未改善之座標：僅須填寫立即改善及排程改善尚未改善完成點之座標，若2點以上，<br>
                                    請逐列列出，或以附件方式上傳(雲端平台)。異常點座標可以是檢測報告電子檔所附之座標、<br>
                                    若報告無提供座標，請於現場利用手機定位。(座標格式TWI97)<br>
                                (9) 備註：若檢測時之管線數量2條以上(含)，請以同一代號註明同一管束，如：以A、B…區別。
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
<div id="messageblock" class="magpopup magSizeS mfp-hide">
  <div class="magpopupTitle">附件列表</div>
  <div class="padding10ALL">
      <div class="stripeMeB tbover">
          <table id="tablistFile" border="0" cellspacing="0" cellpadding="0" width="100%">
              <thead>
		        	<tr>
		        		<th nowrap="nowrap" align="center" width="50%">檔案名稱</th>
		        		<th nowrap="nowrap" align="center" width="30%">上傳日期</th>
		        		<th id="thFunc" nowrap="nowrap" align="center" width="10%">功能</th>
		        	</tr>
              </thead>
              <tbody></tbody>
          </table>
      </div>

  </div><!-- padding10ALL -->

</div><!--magpopup -->

<!-- Magnific Popup -->
<div id="messageblock2" class="magpopup magSizeS mfp-hide">
  <div class="magpopupTitle">異常點尚未改善完成之座標</div>
  <div class="padding10ALL">
      <div class="twocol">
          <div class="right">
            <a id="newbtnxy" href="javascript:void(0);" title="新增" class="genbtn">新增</a>
          </div>
      </div><br />
      <div class="stripeMeB tbover">
          <table id="tablistcoordinate" border="0" cellspacing="0" cellpadding="0" width="100%">
              <thead>
		        	<tr>
		        		<th nowrap="nowrap" align="center">x座標</th>
		        		<th nowrap="nowrap" align="center">y座標</th>
		        		<th nowrap="nowrap" align="center">級距</th>
		        		<th id="thFunc2" nowrap="nowrap" align="center" width="10%">功能</th>
		        	</tr>
              </thead>
              <tbody></tbody>
          </table>
      </div>

  </div><!-- padding10ALL -->

</div><!--magpopup -->

<!-- Magnific Popup -->
<div id="messageblock3" class="magpopup magSizeS mfp-hide">
  <div class="magpopupTitle"><span id="typeName"></span></div>
  <div class="padding10ALL">
      <div class="OchiTrasTable width100 TitleLength08 font-size3">
          <div class="OchiRow">
              <div class="OchiHalf">
                  <div class="OchiCell OchiTitle IconCe TitleSetWidth">x座標</div>
                  <div class="OchiCell width100"><input id="txt1" type="text" class="inputex width100"></div>
              </div><!-- OchiHalf -->
              <div class="OchiHalf">
                  <div class="OchiCell OchiTitle IconCe TitleSetWidth">y座標</div>
                  <div class="OchiCell width100"><input id="txt2" type="text" class="inputex width100"></div>
              </div><!-- OchiHalf -->
          </div><!-- OchiRow -->
          <div class="OchiRow">
              <div class="OchiHalf">
                  <div class="OchiCell OchiTitle IconCe TitleSetWidth">級距</div>
                  <div class="OchiCell width100"><input id="txt3" type="text" class="inputex width100"></div>
              </div><!-- OchiHalf -->
          </div><!-- OchiRow -->
      </div><!-- OchiTrasTable -->

      <div class="twocol margin10T">
            <div class="right">
                <a id="cancelbtn" href="javascript:void(0);" class="genbtn closecolorbox">取消</a>
                <a id="subbtn" href="javascript:void(0);" class="genbtn">儲存</a>
            </div>
        </div>

  </div><!-- padding10ALL -->

</div><!--magpopup -->

<!-- 本頁面使用的JS -->
	<script type="text/javascript" src="../js/GenCommon.js"></script><!-- UIcolor JS -->
	<script type="text/javascript" src="../js/PageCommon.js"></script><!-- 系統共用 JS -->
	<script type="text/javascript" src="../js/MenuGas.js"></script><!-- 系統共用 JS -->
	<script type="text/javascript" src="../js/SubMenuGasA.js"></script><!-- 內頁選單 -->
	<script type="text/javascript" src="../js/autoHeight.js"></script><!-- 高度不足頁面的絕對置底footer -->
</body>
</html>
