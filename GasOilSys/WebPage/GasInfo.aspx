<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GasInfo.aspx.cs" Inherits="WebPage_GasInfo" %>

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
	<script type="text/javascript">
		$(document).ready(function () {
            getYearList();
            $("#sellist").val(getTaiwanDate());
            getData(getTaiwanDate());

            $("a[name='delbtn']").hide();

            //選擇年份
            $(document).on("change", "#sellist", function () {
                getData($("#sellist option:selected").val());
            });

            //編輯按鈕
            $(document).on("click", "#editbtn", function () {
                $("#editbtn").hide();
                $("#backbtn").show();
                $("#subbtn").show();

                disabled(false);
            });

            //場站新增按鈕
            $(document).on("click", "#newbtn", function () {
                $("#sc_type").val('');
                $("#txt_name").val('');
                doOpenMagPopup();
            });

            //場站編輯按鈕
            $(document).on("click", "#editbtn2", function () {
                $("a[name='delbtn']").show();
                $("#editbtn2").hide();
                $("#cancelbtn").show();
            });

            //場站取消按鈕
            $(document).on("click", "#cancelbtn", function () {
                $("a[name='delbtn']").hide();
                $("#editbtn2").show();
                $("#cancelbtn").hide();
            });

            //場站取消按鈕
            $(document).on("click", "a[name='delbtn']", function () {
                if (confirm("確定刪除?")) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../handler/DelGasInfoState.aspx",
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

                                location.href = "GasInfo.aspx?cp=" + $.getQueryString("cp");
                            }
                        }
                    });
                }
            });

            //場站彈出視窗取消按鈕
            $(document).on("click", "#cancelbtn2", function () {
                $.magnificPopup.close();
            });

            //返回按鈕
            $(document).on("click", "#backbtn", function () {
                var str = confirm('尚未儲存的部分將不會更改，確定返回嗎?');

                if (str) {
                    $("#editbtn").show();
                    $("#backbtn").hide();
                    $("#subbtn").hide();

                    disabled(true);

                    getData();
                }
            });

            //儲存按鈕
            $(document).on("click", "#subbtn", function () {
                // Get form
                var form = $('#form1')[0];

                // Create an FormData object 
                var data = new FormData(form);

                // If you want to add an extra field for the FormData
                data.append("cid", $.getQueryString("cp"));
                data.append("year", $("#sellist option:selected").val());
                data.append("txt1", encodeURIComponent($("#cname").val()));
                data.append("txt2", encodeURIComponent($("#caddr").val()));
                data.append("txt3", encodeURIComponent($("#ctel").val()));
                data.append("txt4", encodeURIComponent($("#mainline").val()));
                data.append("txt5", encodeURIComponent($("#cycleline").val()));
                data.append("txt28", encodeURIComponent($("#businessline").val()));
                data.append("txt6", encodeURIComponent($("#specialline").val()));
                data.append("txt7", encodeURIComponent($("#finishline").val()));
                data.append("txt8", encodeURIComponent($("#sealine").val()));
                data.append("txt9", encodeURIComponent($("#LNGline").val()));
                data.append("txt10", encodeURIComponent($("#BOGline").val()));
                data.append("txt11", encodeURIComponent($("#NGline").val()));
                data.append("txt12", encodeURIComponent($("#supplycity").val()));
                data.append("txt13", encodeURIComponent($("#ov1").val()));
                data.append("txt14", encodeURIComponent($("#ov2").val()));
                data.append("txt15", encodeURIComponent($("#ov3").val()));
                data.append("txt16", encodeURIComponent($("#ov4").val()));
                data.append("txt17", encodeURIComponent($("#ov5").val()));
                data.append("txt18", encodeURIComponent($("#ov6").val()));
                data.append("txt19", encodeURIComponent($("#ov7").val()));
                data.append("txt20", encodeURIComponent($("#ov8").val()));
                data.append("txt21", encodeURIComponent($("#ov9").val()));
                data.append("txt22", encodeURIComponent($("#ov10").val()));
                data.append("txt23", encodeURIComponent($("#ov11").val()));
                data.append("txt24", encodeURIComponent($("#txt1").val()));
                data.append("txt25", encodeURIComponent($("#txt2").val()));
                data.append("txt26", encodeURIComponent($("#txt3").val()));
                data.append("txt27", encodeURIComponent($("#txt4").val()));

                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "../handler/AddGasInfo.aspx",
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

                            location.href = "GasInfo.aspx?cp=" + $.getQueryString("cp");
                        }
                    }
                });
            });

            //場站中心儲存按鈕
            $(document).on("click", "#subbtn2", function () {

                var msg = '';
                var status = true;

                if ($("#sc_type option:selected").val() == '')
                    msg += "請選擇【場站類別】\n";
                if ($("#txt_name").val() == '')
                    msg += "請輸入【中心名稱】\n";

                if (msg != "") {
                    alert("Error message: \n" + msg);
                    return false;
                }

                $.ajax({
			    	type: "POST",
			    	async: false, //在沒有返回值之前,不會執行下一步動作
			    	url: "../Handler/GetGasInfo.aspx",
                    data: {
                        type: "data",
                        cpid: $.getQueryString("cp"),
                        year: $("#sellist option:selected").val(),
                        centralType: $("#sc_type option:selected").val(),
                        centralName: $("#txt_name").val()
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
                            if ($("dtCount", data).text() != '0') {
                                alert('已有相同的場站名稱!請重新輸入');
                                status = false;
                            }
                        }
			    	}
                });

                if (status == false)
                    return false;

                // Get form
                var form = $('#form1')[0];

                // Create an FormData object 
                var data = new FormData(form);

                // If you want to add an extra field for the FormData
                data.append("cid", $.getQueryString("cp"));
                data.append("year", $("#sellist option:selected").val());
                data.append("txt1", $("#sc_type option:selected").val());
                data.append("txt2", encodeURIComponent($("#txt_name").val()));

                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "../handler/AddGasInfoState.aspx",
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

                            location.href = "GasInfo.aspx?cp=" + $.getQueryString("cp");
                        }
                    }
                });
            });
        }); // end js

        function disabled(status) {
            $("#cname").attr("disabled", status);
            $("#ctel").attr("disabled", status);
            $("#caddr").attr("disabled", status);
            $("#mainline").attr("disabled", status);
            $("#cycleline").attr("disabled", status);
            $("#businessline").attr("disabled", status);
            $("#specialline").attr("disabled", status);
            $("#finishline").attr("disabled", status);
            $("#sealine").attr("disabled", status);
            $("#LNGline").attr("disabled", status);
            $("#BOGline").attr("disabled", status);
            $("#NGline").attr("disabled", status);
            $("#supplycity").attr("disabled", status);
            $("input[name='supplygas']").attr("disabled", status);
            $("#ov1").attr("disabled", status);
            $("#ov2").attr("disabled", status);
            $("#ov3").attr("disabled", status);
            $("#ov4").attr("disabled", status);
            $("#ov5").attr("disabled", status);
            $("#ov6").attr("disabled", status);
            $("#ov7").attr("disabled", status);
            $("#ov8").attr("disabled", status);
            $("#ov9").attr("disabled", status);
            $("#ov10").attr("disabled", status);
            $("#ov11").attr("disabled", status);
            $("#txt1").attr("disabled", status);
            $("#txt2").attr("disabled", status);
            $("#txt3").attr("disabled", status);
            $("#txt4").attr("disabled", status);
        }

		function getData(year) {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetGasInfo.aspx",
                data: {
                    type: "list",
                    cpid: $.getQueryString("cp"),
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
                            $(data).find("data_item").each(function (i) {
                                $("#cname").val($(this).children("事業名稱").text().trim());
                                $("#ctel").val($(this).children("電話").text().trim());
                                $("#caddr").val($(this).children("地址").text().trim());
                                $("#mainline").val($.FormatThousandGroup($(this).children("輸氣幹線").text().trim()));
                                $("#cycleline").val($.FormatThousandGroup($(this).children("輸氣環線").text().trim()));
                                $("#businessline").val($.FormatThousandGroup($(this).children("營業線").text().trim()));
                                $("#specialline").val($.FormatThousandGroup($(this).children("配氣專管").text().trim()));
                                $("#finishline").val($.FormatThousandGroup($(this).children("場內成品線").text().trim()));
                                $("#sealine").val($.FormatThousandGroup($(this).children("海底管線").text().trim()));
                                $("#LNGline").val($.FormatThousandGroup($(this).children("LNG管線").text().trim()));
                                $("#BOGline").val($.FormatThousandGroup($(this).children("BOG管線").text().trim()));
                                $("#NGline").val($.FormatThousandGroup($(this).children("NG管線").text().trim()));
                                $("#supplycity").val($(this).children("供氣對象縣市").text().trim());
                                $("input[name='supplygas']").prop("checked", false);
                                var arySupplyGas = $(this).children("供應天然氣").text().trim().split(',');
                                $.each(arySupplyGas, function (key, value) {
                                    $("input[name='supplygas'][value='" + value + "']").prop("checked", true);
                                });
                                $("#ov1").val($(this).children("儲槽").text().trim());
                                $("#ov2").val($(this).children("注氣站").text().trim());
                                $("#ov3").val($(this).children("加壓站").text().trim());
                                $("#ov4").val($(this).children("配氣站").text().trim());
                                $("#ov5").val($(this).children("隔離站").text().trim());
                                $("#ov6").val($(this).children("開關站").text().trim());
                                $("#ov7").val($(this).children("清管站").text().trim());
                                $("#ov8").val($(this).children("整壓計量站").text().trim());
                                $("#ov9").val($(this).children("低壓排放塔").text().trim());
                                $("#ov10").val($(this).children("高壓排放塔").text().trim());
                                $("#ov11").val($(this).children("NG2摻配站").text().trim());
                                $("#txt1").val($(this).children("年度查核姓名").text().trim());
                                $("#txt2").val($(this).children("年度查核職稱").text().trim());
                                $("#txt3").val($(this).children("年度查核分機").text().trim());
                                $("#txt4").val($(this).children("年度查核email").text().trim());
                            });
                        }
                        else {
                            $("#cname").val('');
                            $("#ctel").val('');
                            $("#caddr").val('');
                            $("#mainline").val('');
                            $("#cycleline").val('');
                            $("#businessline").val('');
                            $("#specialline").val('');
                            $("#finishline").val('');
                            $("#sealine").val('');
                            $("#LNGline").val('');
                            $("#BOGline").val('');
                            $("#NGline").val('');
                            $("#supplycity").val('');
                            $("input[name='supplygas']").prop("checked", false);
                            $("#ov1").val('');
                            $("#ov2").val('');
                            $("#ov3").val('');
                            $("#ov4").val('');
                            $("#ov5").val('');
                            $("#ov6").val('');
                            $("#ov7").val('');
                            $("#ov8").val('');
                            $("#ov9").val('');
                            $("#ov10").val('');
                            $("#ov11").val('');
                            $("#txt1").val('');
                            $("#txt2").val('');
                            $("#txt3").val('');
                            $("#txt4").val('');
                        }

						$("#tablist tbody").empty();
						var tabstr = '';
						if ($(data).find("data_item2").length > 0) {
							$(data).find("data_item2").each(function (i) {
								tabstr += '<tr>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("戰場類別中心名稱").text().trim() + '</td>';

                                if (i == (parseInt($("dtCount", data).text()) - 1)) {
                                    tabstr += '<td nowrap="nowrap"><span id="sp_1">' + $(this).children("配氣站").text().trim() + '</span></td>';
                                    tabstr += '<td nowrap="nowrap"><span id="sp_2">' + $(this).children("開關站").text().trim() + '</span></td>';
                                    tabstr += '<td nowrap="nowrap"><span id="sp_3">' + $(this).children("隔離站").text().trim() + '</span></td>';
                                    tabstr += '<td nowrap="nowrap"><span id="sp_4">' + $(this).children("計量站").text().trim() + '</span></td>';
                                    tabstr += '<td nowrap="nowrap"><span id="sp_5">' + $(this).children("清管站").text().trim() + '</span></td>';
                                }
                                else {
                                    if ($(this).children("配氣站").text().trim() != '')
                                        if ($(this).children("年度").text().trim() == getTaiwanDate())
                                            tabstr += '<td nowrap="nowrap">' + $(this).children("配氣站").text().trim() + ' <a name="delbtn" href="javascript:void(0);" aid="'
                                                + $(this).children("配氣站guid").text().trim() + '" style="float:right">刪除</a>' + '</td>';
                                        else
                                            tabstr += '<td nowrap="nowrap">' + $(this).children("配氣站").text().trim() + '</td>';
                                    else
                                        tabstr += '<td nowrap="nowrap"></td>';

                                    if ($(this).children("開關站").text().trim() != '')
                                        if ($(this).children("年度").text().trim() == getTaiwanDate())
                                            tabstr += '<td nowrap="nowrap">' + $(this).children("開關站").text().trim() + ' <a name="delbtn" href="javascript:void(0);" aid="'
                                                + $(this).children("開關站guid").text().trim() + '" style="float:right">刪除</a>' + '</td>';
                                        else
                                            tabstr += '<td nowrap="nowrap">' + $(this).children("開關站").text().trim() + '</td>';                                    
                                    else
                                        tabstr += '<td nowrap="nowrap"></td>';

                                    if ($(this).children("隔離站").text().trim() != '')
                                        if ($(this).children("年度").text().trim() == getTaiwanDate())
                                            tabstr += '<td nowrap="nowrap">' + $(this).children("隔離站").text().trim() + ' <a name="delbtn" href="javascript:void(0);" aid="'
                                                + $(this).children("隔離站guid").text().trim() + '" style="float:right">刪除</a>' + '</td>';
                                        else
                                            tabstr += '<td nowrap="nowrap">' + $(this).children("隔離站").text().trim() + '</td>';                                    
                                    else
                                        tabstr += '<td nowrap="nowrap"></td>';

                                    if ($(this).children("計量站").text().trim() != '')
                                        if ($(this).children("年度").text().trim() == getTaiwanDate())
                                            tabstr += '<td nowrap="nowrap">' + $(this).children("計量站").text().trim() + ' <a name="delbtn" href="javascript:void(0);" aid="'
                                                + $(this).children("計量站guid").text().trim() + '" style="float:right">刪除</a>' + '</td>';
                                        else
                                            tabstr += '<td nowrap="nowrap">' + $(this).children("計量站").text().trim() + '</td>';                                    
                                    else
                                        tabstr += '<td nowrap="nowrap"></td>';

                                    if ($(this).children("清管站").text().trim() != '')
                                        if ($(this).children("年度").text().trim() == getTaiwanDate())
                                            tabstr += '<td nowrap="nowrap">' + $(this).children("清管站").text().trim() + ' <a name="delbtn" href="javascript:void(0);" aid="'
                                                + $(this).children("清管站guid").text().trim() + '" style="float:right">刪除</a>' + '</td>';
                                        else
                                            tabstr += '<td nowrap="nowrap">' + $(this).children("清管站").text().trim() + '</td>';                                    
                                    else
                                        tabstr += '<td nowrap="nowrap"></td>';
                                }
                                
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="6">查詢無資料</td></tr>';
                        $("#tablist tbody").append(tabstr);

                        //確認權限&按鈕顯示或隱藏
                        if ($("#sellist").val() != getTaiwanDate()) {
                            $("#editbtn").hide();
                            $("#editbtn2").hide();
                            $("#newbtn").hide();
                        }
                        else {
                            if (($("#Competence").val() == '01') || ($("#Competence").val() == '04') || ($("#Competence").val() == '05') || ($("#Competence").val() == '06')) {
                                $("#editbtn").hide();
                                $("#editbtn2").hide();
                                $("#newbtn").hide();
                            }
                            else {
                                $("#editbtn").show();
                                $("#editbtn2").show();
                                $("#newbtn").show();
                            }

                            if ($("#sp_1").text() == '0' && $("#sp_2").text() == '0' && $("#sp_3").text() == '0' && $("#sp_4").text() == '0' && $("#sp_5").text() == '0')
                                $("#editbtn2").hide();
                        }                        
                    }

                    getConfirmedStatus();
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
                url: "../Handler/GetGasInfo.aspx",
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
                        if ($(data).find("data_item3").length > 0) {
                            $(data).find("data_item3").each(function (i) {
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
                                <div id="fileall" class="right">
                                <a id="editbtn" href="javascript:void(0);" title="編輯" class="genbtn">編輯</a>
                                <a id="backbtn" href="javascript:void(0);" title="返回" class="genbtn" style="display:none">返回</a>
                                <a id="subbtn" href="javascript:void(0);" class="genbtn" style="display:none">儲存</a>
                                </div>
                            </div><br />
                            <div class="twocol">
                                <div class="right font-normal font-size3">
                                    <a href="javascript:void(0);" id="collapse1open">全部展開</a>
                                    <a href="javascript:void(0);" id="collapse1close">全部關閉</a>
                                </div><!-- right -->
                            </div><!-- twocol -->

                            <div id="collapse1">
                                <div>
                                    <div class="collapseTitle font-blackA font-size4">A.基本資料</div>
                                    <div class="BoxBorderDa BoxRadiusB padding5ALL">
                                        <!-- 管線資料start -->
                                        <div class="OchiTrasTable width100 TitleLength08 font-size3">

                                            <div class="OchiRow">
                                                <div class="OchiCell OchiTitle IconCe TitleSetWidth">事業名稱</div>
                                                <div class="OchiCell width100"><input type="text" id="cname" class="inputex width99" disabled></div>
                                            </div><!-- OchiRow -->
                                            <div class="OchiRow">
                                                <div class="OchiCell OchiTitle IconCe TitleSetWidth">地址</div>
                                                <div class="OchiCell width100"><input type="text" id="caddr" class="inputex width99" disabled></div>
                                            </div><!-- OchiRow -->
                                            <div class="OchiRow">
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">電話</div>
                                                    <div class="OchiCell width100"><input type="text" id="ctel" class="inputex width100" disabled></div>
                                                </div><!-- OchiHalf -->
                                            </div><!-- OchiRow -->

                                            </br>
                                            <div class="OchiRow">
                                                <div class="margin5TB font-size4" style="text-align:center">本年度查核聯絡窗口</div>
                                            </div><!-- OchiRow -->

                                            <div class="OchiRow">
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">姓名</div>
                                                    <div class="OchiCell width100"><input type="text" id="txt1" class="inputex width100" disabled></div>
                                                </div><!-- OchiHalf -->
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">職稱</div>
                                                    <div class="OchiCell width100"><input type="text" id="txt2" class="inputex width100" disabled></div>
                                                </div><!-- OchiHalf -->
                                            </div><!-- OchiRow -->
                                            <div class="OchiRow">
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">分機</div>
                                                    <div class="OchiCell width100"><input type="text" id="txt3" class="inputex width100" disabled></div>
                                                </div><!-- OchiHalf -->
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">email</div>
                                                    <div class="OchiCell width100"><input type="text" id="txt4" class="inputex width100" disabled></div>
                                                </div><!-- OchiHalf -->
                                            </div><!-- OchiRow -->

                                        </div><!-- OchiTrasTable -->
                                        <!-- 管線資料end -->
                                    </div>
                                </div>

                                <div class="margin10T">
                                    <div class="collapseTitle font-blackA font-size4">B.輸儲管線概況</div>
                                    <div class="BoxBorderDa BoxRadiusB padding5ALL">
                                        <!-- 管線資料start -->
                                        <div class="OchiTrasTable width100 TitleLength08 font-size3">
                                            <div class="OchiRow">
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">輸氣幹線</div>
                                                    <div class="OchiCell width100"><input type="text" id="mainline" class="inputex width80" disabled> 公里</div>
                                                </div><!-- OchiHalf -->
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">輸氣環線</div>
                                                    <div class="OchiCell width100"><input type="text" id="cycleline" class="inputex width80" disabled> 公里</div>
                                                </div><!-- OchiHalf -->
                                            </div><!-- OchiRow -->

                                            <div class="OchiRow">
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">營業線</div>
                                                    <div class="OchiCell width100"><input type="text" id="businessline" class="inputex width80" disabled> 公里</div>
                                                </div><!-- OchiHalf -->
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">配氣專管</div>
                                                    <div class="OchiCell width100"><input type="text" id="specialline" class="inputex width80" disabled> 公里</div>
                                                </div><!-- OchiHalf -->
                                            </div><!-- OchiRow -->

                                            <div class="OchiRow">
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">場內成品線</div>
                                                    <div class="OchiCell width100"><input type="text" id="finishline" class="inputex width80" disabled> 公里</div>
                                                </div><!-- OchiHalf -->
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">海底管線</div>
                                                    <div class="OchiCell width100"><input type="text" id="sealine" class="inputex width80" disabled> 公里</div>
                                                </div><!-- OchiHalf -->
                                            </div><!-- OchiRow -->

                                            <div class="OchiRow">
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">LNG 管線</div>
                                                    <div class="OchiCell width100"><input type="text" id="LNGline" class="inputex width80" disabled> 公里</div>
                                                </div><!-- OchiHalf -->
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">BOG 管線</div>
                                                    <div class="OchiCell width100"><input type="text" id="BOGline" class="inputex width80" disabled> 公里</div>
                                                </div><!-- OchiHalf -->
                                            </div><!-- OchiRow -->

                                            <div class="OchiRow">
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">NG 管線</div>
                                                    <div class="OchiCell width100"><input type="text" id="NGline" class="inputex width80" disabled> 公里</div>
                                                </div><!-- OchiHalf -->
                                            </div><!-- OchiRow -->

                                            <div class="OchiRow">
                                                <div class="OchiCell OchiTitle IconCe TitleSetWidth">供氣對象(縣市)</div>
                                                <div class="OchiCell width100"><input type="text" id="supplycity" class="inputex width99" disabled></div>
                                            </div><!-- OchiRow -->

                                            <div class="OchiRow">
                                                <div class="OchiCell OchiTitle IconCe TitleSetWidth">供應天然氣</div>
                                                <div class="OchiCell width100">
                                                    <div class="inlineitem"><input type="checkbox" name="supplygas" value="01" disabled="disabled"> NG1 進口天然氣與自產天然氣摻配</div>
                                                    <div class="inlineitem"><input type="checkbox" name="supplygas" value="02" disabled="disabled"> NG1自產天然氣</div>
                                                    <div class="inlineitem"><input type="checkbox" name="supplygas" value="03" disabled="disabled"> NG2進口天然氣</div>
                                                </div>
                                            </div><!-- OchiRow -->


                                        </div><!-- OchiTrasTable -->
                                        <!-- 管線資料end -->
                                    </div>
                                </div>

                                <div class="margin10T">
                                    <div class="collapseTitle font-blackA font-size4">C.場站概況</div>
                                    <div class="BoxBorderDa BoxRadiusB padding5ALL">
                                        <!-- 管線資料start -->
                                        <div class="OchiTrasTable width100 TitleLength08 font-size3">
                                            <div class="OchiRow">
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">儲槽</div>
                                                    <div class="OchiCell width100"><input type="text" id="ov1" class="inputex width100" disabled></div>
                                                </div><!-- OchiHalf -->
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">注氣站</div>
                                                    <div class="OchiCell width100"><input type="text" id="ov2"  class="inputex width100" disabled></div>
                                                </div><!-- OchiHalf -->
                                            </div><!-- OchiRow -->

                                            <div class="OchiRow">
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">加壓站</div>
                                                    <div class="OchiCell width100"><input type="text" id="ov3"  class="inputex width100" disabled></div>
                                                </div><!-- OchiHalf -->
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">配氣站</div>
                                                    <div class="OchiCell width100"><input type="text" id="ov4"  class="inputex width100" disabled></div>
                                                </div><!-- OchiHalf -->
                                            </div><!-- OchiRow -->

                                            <div class="OchiRow">
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">隔離站</div>
                                                    <div class="OchiCell width100"><input type="text" id="ov5"  class="inputex width100" disabled></div>
                                                </div><!-- OchiHalf -->
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">開關站</div>
                                                    <div class="OchiCell width100"><input type="text" id="ov6"  class="inputex width100" disabled></div>
                                                </div><!-- OchiHalf -->
                                            </div><!-- OchiRow -->

                                            <div class="OchiRow">
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">清管站</div>
                                                    <div class="OchiCell width100"><input type="text" id="ov7"  class="inputex width100" disabled></div>
                                                </div><!-- OchiHalf -->
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">整壓計量站</div>
                                                    <div class="OchiCell width100"><input type="text" id="ov8"  class="inputex width100" disabled></div>
                                                </div><!-- OchiHalf -->
                                            </div><!-- OchiRow -->

                                            <div class="OchiRow">
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">低壓排放塔</div>
                                                    <div class="OchiCell width100"><input type="text" id="ov9"  class="inputex width100" disabled></div>
                                                </div><!-- OchiHalf -->
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">高壓排放塔</div>
                                                    <div class="OchiCell width100"><input type="text" id="ov10"  class="inputex width100" disabled></div>
                                                </div><!-- OchiHalf -->
                                            </div><!-- OchiRow -->
                                            <div class="OchiRow">
                                                <div class="OchiHalf">
                                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">NG2 熱值 調整摻配站</div>
                                                    <div class="OchiCell width100"><input type="text" id="ov11"  class="inputex width100" disabled></div>
                                                </div><!-- OchiHalf -->
                                            </div><!-- OchiRow -->
                                        </div><!-- OchiTrasTable -->
                                        <!-- 管線資料end -->
                                    </div>
                                </div>
                                
								<div class="margin10T">
                                    <div class="collapseTitle font-blackA font-size4">D.天然氣進口事業轄區場站名稱</div>
                                    <div>
                                        <div class="twocol">
                                        <div class="left">
                                            
                                        </div>
                                        <div class="right">
                                            <a id="newbtn" href="javascript:void(0);" title="新增" class="genbtn">新增</a>
                                            <a id="editbtn2" href="javascript:void(0);" title="編輯" class="genbtn">編輯</a>
                                            <a id="cancelbtn" href="javascript:void(0);" title="取消" class="genbtn" style="display:none">取消</a>
                                        </div>
                                    </div>
                                    <div class="stripeMeG margin5T tbover">
                                            <table id="tablist" width="100%" border="0" cellspacing="0" cellpadding="0">
												<thead>
													<tr>
														<th  nowrap>場站類別<br>中心名稱</th>
														<th  nowrap>配氣站</th>
														<th  nowrap>開關站</th>
														<th  nowrap>隔離站</th>
														<th  nowrap>計量站</th>
														<th  nowrap>清管站</th>
													</tr>
												</thead>
												<tbody></tbody>
                                            </table>
                                        </div><!-- stripeMe -->
                                    </div>                                    
                                </div>
                            </div><!-- collapse1 -->
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
  <div class="magpopupTitle"><span id="cpNameIsConfirm2"></span>新增場站中心</div>
  <div class="padding10ALL">
      <div class="twocol">
      </div><br />
      <div class="stripeMeB tbover">
          <table id="tablist3" border="0" cellspacing="0" cellpadding="0" width="100%">
              <thead>
		        	<tr>
		        		<th nowrap="nowrap" align="center" width="50">場站類別</th>
		        		<th nowrap="nowrap" align="center" width="150">中心名稱</th>
		        	</tr>
              </thead>
              <tbody>
                  <td nowrap="nowrap" align="center">
                      <select id="sc_type" class="inputex width100">
                          <option value="">請選擇</option>
                          <option value="配氣站">配氣站</option>
                          <option value="開關站">開關站</option>
                          <option value="隔離站">隔離站</option>
                          <option value="計量站">計量站</option>
                          <option value="清管站">清管站</option>
                      </select>
                  </td>
                  <td>
                      <input id="txt_name" type="text" class="inputex width100" />
                  </td>
              </tbody>
          </table>
      </div>

      <div class="twocol margin5TB">
		<div class="left"><span id="errMsg" style="color:red;"></span></div>
			<div class="right">
				<a id="cancelbtn2" href="javascript:void(0);" class="genbtn">取消</a>
				<a id="subbtn2" href="javascript:void(0);" class="genbtn">儲存</a>
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
	<script type="text/javascript">
		$(document).ready(function () {
			$("#collapse1").collapse({
				query: 'div.collapseTitle',//收合標題樣式名
				persist: false,//是否記憶收合,需配合jquery.collapse_storage.js
				open: function () {
					this.slideDown(100);//動畫效果
				},
				close: function () {
					this.slideUp(100);//動畫效果
				},
			});

			$("#collapse1").trigger("open") // 預設全開啟
			//$("#collapse1").trigger("close") // 預設全關閉(default)
			$("#collapse1 div:nth-child(1) div.collapseTitle a").trigger("open") // 控制第幾個開啟

			//全部收合展開按鈕動作
			$("#collapse1open").click(function () {
				$("#collapse1").trigger("open")
			});
			$("#collapse1close").click(function () {
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
