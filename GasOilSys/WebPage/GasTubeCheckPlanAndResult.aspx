<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GasTubeCheckPlanAndResult.aspx.cs" Inherits="WebPage_GasTubeCheckPlanAndResult" %>

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
            $(".pickDate").datepick({
                dateFormat: 'yymmdd',
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: '../images/calendar.gif',
                yearRange: 'c-60:c+10'
            }).BootStrap(); //BootStrap() 產生符合 BootStrap 的樣式內容

            getYearList();
            $("#sellist").val(getTaiwanDate());
            getData(getTaiwanDate());

            //選擇年份
            $(document).on("change", "#sellist", function () {
                getData($("#sellist option:selected").val());
			});

            //編輯按鈕
            $(document).on("click", "#editbtn", function () {
                $("#editbtn").hide();
                $("#cancelbtn").show();
                $("#subbtn").show();
                $("#sellist").attr("disabled", true);
                $("input[name='usercheck']").attr("disabled", false);
            });

            //返回按鈕
            $(document).on("click", "#cancelbtn", function () {
                var str = confirm('尚未儲存的部分將不會更改，確定返回嗎?');

                if (str) {
                    location.href = "GasTubeCheckPlanAndResult.aspx?cp=" + $.getQueryString("cp");
                }
            });

            //儲存按鍵
            $(document).on("click", "#subbtn", function () {

                // Get form
                var form = $('#form1')[0];

                // Create an FormData object 
                var data = new FormData(form);

                // If you want to add an extra field for the FormData
                data.append("cp", $.getQueryString("cp"));
                data.append("year", encodeURIComponent(getTaiwanDate()));

                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "../handler/AddGasTubeCheckPlanAndResult.aspx",
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

                            location.href = "GasTubeCheckPlanAndResult.aspx?cp=" + $.getQueryString("cp");
                        }
                    }
                });
            });

            //刪除按鈕
            $(document).on("click", "a[name='delbtn']", function () {
                if (confirm("確定刪除?")) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../handler/DelGasTubeCheckPlanAndResult.aspx",
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
                                location.href = "GasTubeCheckPlanAndResult.aspx?cp=" + $.getQueryString("cp");
                            }
                        }
                    });
                }
            });

            //新增開窗
            $(document).on("click", "#newbtn", function () {
                $("#Gguid").val('');

                $("#txt1").val('');
                $("input[name='txt2'][value='01']").prop("checked", true);
                $("input[name='txt3'][value='01']").prop("checked", true);

                doOpenMagPopup();
            });

            //編輯開窗
            $(document).on("click", "a[name='editbtnU']", function () {
                $("#Gguid").val($(this).attr("aid"));

                var txt2 = $("input[name='rd1_" + $("#Gguid").val() + "']:checked").val();
                var txt3 = $("input[name='rd2_" + $("#Gguid").val() + "']:checked").val();

                $("#txt1").val($("span[name='sp_" + $("#Gguid").val() + "']").text());
                $("input[name='txt2'][value='" + txt2 + "']").prop("checked", true);
                $("input[name='txt3'][value='" + txt3 + "']").prop("checked", true);

                doOpenMagPopup();
            });

            //開窗儲存
            $(document).on("click", "#cSubbtn", function () {
                // Get form
                var form = $('#form1')[0];

                // Create an FormData object 
                var data = new FormData(form);

                var mode = ($("#Gguid").val() == "") ? "new" : "edit";

                // If you want to add an extra field for the FormData
                data.append("cp", $.getQueryString("cp"));
                data.append("guid", $("#Gguid").val());
                data.append("mode", encodeURIComponent(mode));
                data.append("year", encodeURIComponent(getTaiwanDate()));
                data.append("txt1", encodeURIComponent($("#txt1").val()));
                data.append("txt2", encodeURIComponent($("input[name='txt2']:checked").val()));
                data.append("txt3", encodeURIComponent($("input[name='txt3']:checked").val()));

                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "../handler/AddGasTubeCheckPlanAndResult2.aspx",
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
                            parent.$.colorbox.close();

                            location.href = "GasTubeCheckPlanAndResult.aspx?cp=" + $.getQueryString("cp");
                        }
                    }
                });
            });
		}); // end js

        function getData(year) {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetGasTubeCheckPlanAndResult.aspx",
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
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                var arychecktool = $(this).children("管線檢查").text().trim().split(',');
                                $("input[name='usercheck']").prop("checked", false);
                                $.each(arychecktool, function (key, value) {
                                    $("input[name='usercheck'][value='" + value + "']").prop("checked", true);
                                });
                            });
                        }
                        else {
                            $("input[name='usercheck']").prop("checked", false);
                        }

						$("#tablist tbody").empty();
						var tabstr = '';
						if ($(data).find("data_item2").length > 0) {
							$(data).find("data_item2").each(function (i) {
								//if (i == 0) {
								//	var UserCheck = ($(this).children("用戶管線定期檢查").text().trim() == "Y") ? true : false;
								//	var OutCheck = ($(this).children("委外檢查").text().trim() == "Y") ? true : false;
								//	$("#usercheck").prop("checked", UserCheck);
								//	$("#outsidecheck").prop("checked", OutCheck);
								//}

								tabstr += '<tr>';
								tabstr += '<td>' + $(this).children("itemNo").text().trim() + '</td>';
                                tabstr += '<td><span name="sp_' + $(this).children("guid").text().trim() + '">' + $(this).children("用戶名稱").text().trim() + '</span></td>';

								var ckdate01, ckdate02;
                                ckdate01 = ($(this).children("檢查期限是否符合").text().trim() == "01") ? '<input name="rd1_' + $(this).children("guid").text().trim() + '" type="radio" disabled="disabled" value="01" checked="checked" />' : '<input name="rd1_' + $(this).children("guid").text().trim() + '" type="radio" disabled="disabled" value="01" />';
                                ckdate02 = ($(this).children("檢查期限是否符合").text().trim() == "02") ? '<input name="rd1_' + $(this).children("guid").text().trim() + '" type="radio" disabled="disabled" value="02" checked="checked" />' : '<input name="rd1_' + $(this).children("guid").text().trim() + '" type="radio" disabled="disabled" value="02" />';
								tabstr += '<td>' + ckdate01 + '符合</td>';
								tabstr += '<td>' + ckdate02 + '不符合</td>';

								var ckresult01, ckresult02,ckresult03;
                                ckresult01 = ($(this).children("檢查結果是否符合").text().trim() == "01") ? '<input name="rd2_' + $(this).children("guid").text().trim() + '" type="radio" disabled="disabled" value="01" checked="checked" />' : '<input name="rd2_' + $(this).children("guid").text().trim() + '" type="radio" disabled="disabled" value="01" />';
                                ckresult02 = ($(this).children("檢查結果是否符合").text().trim() == "02") ? '<input name="rd2_' + $(this).children("guid").text().trim() + '" type="radio" disabled="disabled" value="02" checked="checked" />' : '<input name="rd2_' + $(this).children("guid").text().trim() + '" type="radio" disabled="disabled" value="02" />';
                                ckresult03 = ($(this).children("檢查結果是否符合").text().trim() == "03") ? '<input name="rd2_' + $(this).children("guid").text().trim() + '" type="radio" disabled="disabled" value="03" checked="checked" />' : '<input name="rd2_' + $(this).children("guid").text().trim() + '" type="radio" disabled="disabled" value="03" />';
								tabstr += '<td>' + ckresult01 + '合格</td>';
								tabstr += '<td>' + ckresult02 + '不合格</td>';
								tabstr += '<td>' + ckresult03 + '拒檢</td>';
                                tabstr += '<td name="td_edit" nowrap="" align="center"><a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("guid").text().trim() + '">刪除</a>';
                                tabstr += ' <a href="javascript:void(0);" title="編輯" name="editbtnU" aid="' + $(this).children("guid").text().trim() + '" >編輯</a></td>';
                                tabstr += '</tr>';
							});
						}
						else
                            tabstr += '<tr><td colspan="8">查詢無資料</td></tr>';
                        $("#tablist tbody").append(tabstr);

                        //確認權限&按鈕顯示或隱藏
                        if ($("#sellist").val() != getTaiwanDate()) {
                            $("#editbtn").hide();
                            $("#newbtn").hide();
                            $("#th_edit").hide();
                            $("td[name='td_edit']").hide();
                        }
                        else {
                            if (($("#Competence").val() == '01') || ($("#Competence").val() == '04') || ($("#Competence").val() == '05') || ($("#Competence").val() == '06')) {
                                $("#editbtn").hide();
                                $("#newbtn").hide();
                                $("#th_edit").hide();
                                $("td[name='td_edit']").hide();
                            }
                            else {
                                $("#editbtn").show();
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
                url: "../Handler/GetGasTubeCheckPlanAndResult.aspx",
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
                                    <a id="editbtn" href="javascript:void(0);" title="編輯" class="genbtn">編輯</a>
                                    <a id="cancelbtn" href="javascript:void(0);" title="返回" class="genbtn" style="display:none">返回</a>
                                    <a id="subbtn" href="javascript:void(0);" title="儲存" class="genbtn" style="display:none">儲存</a>
                                </div>
                            </div><br />
                            <div class="font-size3"><input type="checkbox" name="usercheck" value="01" disabled="disabled"> 用戶管線定期檢查：<input type="checkbox" name="usercheck" value="02" disabled="disabled"> 委外檢查</div><br />

                            <div class="twocol">
                                <div class="left font-size4 margin10T font-bold">用戶管線定期檢查計畫及檢查結果:</div>
                                <div class="right">
                                    <a id="newbtn" href="javascript:void(0);" title="新增" class="genbtn">新增</a>
                                </div>
                            </div><br />
                            <div class="stripeMeG margin5T tbover">
                                <table id="tablist" width="100%" border="0" cellspacing="0" cellpadding="0">
									<thead>
										<tr>
											<th>項目 </th>
											<th width="45%">用戶名稱</th>
											<th  colspan="2">檢查期限</th>
											<th  colspan="3">檢查結果</th>
                                            <th id="th_edit">功能</th>
										</tr>
									</thead>
									<tbody></tbody>
                                </table>
                            </div><!-- stripeMe -->
                        </div><!-- col -->
                    </div><!-- row -->
                </div>
            </div><!-- container -->
        </div>



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
<div id="messageblock" class="magpopup magSizeM mfp-hide">
  <div class="magpopupTitle">用戶管線定期檢查計畫及檢查結果</div>
  <div class="padding10ALL">

      <div class="margin35T padding5RL">
            <div class="OchiTrasTable width100 TitleLength08 font-size3">
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">用戶名稱</div>
                    <div class="OchiCell width100">
                        <input id="txt1" type="text" class="inputex width100">
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">檢查期限</div>
                    <div class="OchiCell width100">
                        <input name="txt2" type="radio" value="01" /> 符合<input name="txt2" type="radio" value="02" /> 不符合
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">檢查結果</div>
                    <div class="OchiCell width100">
                        <input name="txt3" type="radio" value="01" /> 合格<input name="txt3" type="radio" value="02" /> 不合格<input name="txt3" type="radio" value="03" /> 拒檢
                    </div>
                </div><!-- OchiRow -->
            </div><!-- OchiTrasTable -->
        </div>

      <div class="twocol margin10T">
            <div class="right">
                <a id="cCancelbtn" href="javascript:void(0);" class="genbtn closemagnificPopup">取消</a>
                <a id="cSubbtn" href="javascript:void(0);" class="genbtn">儲存</a>
            </div>
        </div>

  </div><!-- padding10ALL -->

</div><!--magpopup -->

<!-- colorbox -->
<%--<div style="display:none;">
    <div id="workitem">
        <div class="margin35T padding5RL">
            <div class="OchiTrasTable width100 TitleLength08 font-size3">
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">用戶名稱</div>
                    <div class="OchiCell width100">
                        <input id="txt1" type="text" class="inputex width100">
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">檢查期限</div>
                    <div class="OchiCell width100">
                        <input name="txt2" type="radio" value="01" /> 符合<input name="txt2" type="radio" value="02" /> 不符合
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">檢查結果</div>
                    <div class="OchiCell width100">
                        <input name="txt3" type="radio" value="01" /> 合格<input name="txt3" type="radio" value="02" /> 不合格<input name="txt3" type="radio" value="03" /> 拒檢
                    </div>
                </div><!-- OchiRow -->
            </div><!-- OchiTrasTable -->
        </div>

        <div class="twocol margin10T">
            <div class="right">
                <a id="cCancelbtn" href="javascript:void(0);" class="genbtn closecolorbox">取消</a>
                <a id="cSubbtn" href="javascript:void(0);" class="genbtn">儲存</a>
            </div>
        </div>
        <br /><br />
    </div>
</div>--%>

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
