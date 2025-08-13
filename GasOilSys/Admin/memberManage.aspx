<%@ Page Language="C#" AutoEventWireup="true" CodeFile="memberManage.aspx.cs" Inherits="Admin_memberManage" %>

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
            getDDL('001');
            getDDL('002');
            getData(0);

            //停用帳號
            $(document).on("click", "a[name='stopBtn']", function () {
                SaveAccountStatus($(this).attr("aid"), 'D');
            });

            //啟用帳號
            $(document).on("click", "a[name='openBtn']", function () {
                SaveAccountStatus($(this).attr("aid"), 'A');
            });

            function SaveAccountStatus(guid, status) {
                var confirmType = status;

                if (status == 'A')
                    confirmType = '確定啟用嗎?';
                else
                    confirmType = '確定停用嗎?';

                if (confirm(confirmType)) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "BackEnd/AddMemberStatus.aspx",
                        data: {
                            status: status,
                            guid: guid,
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
                                getData($("#pageNumber").val());
                            }
                        }
                    });
                }
            }

            //選擇帳號類別
            $(document).on("change", "#txt1", function () {
                if ($("#txt1 option:selected").val() == '02') {
                    $("#txt2").attr('disabled', false);
                    $("#txt3").attr('disabled', false);
                }
                else if ($("#txt1 option:selected").val() == '') {
                    $("#txt2").val('');
                    $("#txt3").val('');
                    $("#txt3").empty();
                    $("#txt3").append('<option value="">請選擇</option>');
                    $("#txt2").attr('disabled', false);
                    $("#txt3").attr('disabled', false);
                }
                else {
                    $("#txt2").val('');
                    $("#txt3").val('');
                    $("#txt3").empty();
                    $("#txt3").append('<option value="">請選擇</option>');
                    $("#txt2").attr('disabled', true);
                    $("#txt3").attr('disabled', true);
                }
            });

            //選擇網站類別
            $(document).on("change", "#txt2", function () {
                if ($("#txt2 option:selected").val() == '01') {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../Handler/GetCompanyName.aspx",
                        data: {
                            type: "Oil",
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
                                        ddlstr += '<option value="' + $(this).children("guid").text().trim() + '">' + $(this).children("cpname").text().trim() + '</option>';
                                    });
                                }
                                $("#txt3").empty();
                                $("#txt3").append(ddlstr);
                            }
                        }
                    });
                }
                else if ($("#txt2 option:selected").val() == '02') {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../Handler/GetCompanyName.aspx",
                        data: {
                            type: "Gas",
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
                                        ddlstr += '<option value="' + $(this).children("guid").text().trim() + '">' + $(this).children("cpname").text().trim() + '</option>';
                                    });
                                }
                                $("#txt3").empty();
                                $("#txt3").append(ddlstr);
                            }
                        }
                    });
                }
                else {
                    $("#txt3").empty();
                    $("#txt3").append('<option value="">請選擇</option>');
                }
            });

            //刪除按鈕
            $(document).on("click", "a[name='delbtn']", function () {
                if (confirm("刪除後將無法復原，確定刪除此人員資料?")) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "BackEnd/DelMember.aspx",
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
                                getData($("#pageNumber").val());
                            }
                        }
                    });
                }
            });

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
                url: "BackEnd/GetMember.aspx",
                data: {
                    type: "list",
                    guid: "",
                    PageNo: p,
                    PageSize: Page.Option.PageSize,
                    txt1: $("#txt1").val(),
                    txt2: $("#txt2").val(),
                    txt3: $("#txt3").val(),
                    txt5: $("#txt5").val(),
                    SearchStr: $("#txt4").val(),
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
                                tabstr += '<td nowrap="nowrap">' + $(this).children("姓名").text().trim() + '</td>';
                                var dataStatus = $(this).children("資料狀態").text().trim();
                                if (dataStatus == 'A') {
                                    dataStatus = '啟用中';
                                    tabstr += '<td align="center" nowrap="nowrap">' + dataStatus + ' <a href="javascript:void(0);" name="stopBtn" class="genbtnS" aid="' +
                                        $(this).children("guid").text().trim() + '">停用</a>' + '</td>';
                                }
                                else {
                                    dataStatus = '停用中';
                                    tabstr += '<td align="center" nowrap="nowrap"><span style="color:red">' + dataStatus + '</span> <a href="javascript:void(0);" class="grebtn font-size2" name="openBtn" aid="' +
                                        $(this).children("guid").text().trim() + '">啟用</a>' + '</td>';
                                }
                                    
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
                                var webType = $(this).children("網站類別").text().trim();
                                if (webType == '01')
                                    webType = '石油';
                                else if (webType == '02')
                                    webType = '天然氣';
								tabstr += '<td nowrap="nowrap">' + accountType + '</td>';
                                tabstr += '<td nowrap="nowrap">' + webType + '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("cName").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("使用者帳號").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("使用者密碼").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("mail").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("電話").text().trim() + '</td>';
								tabstr += '<td nowrap="nowrap">' + $(this).children("單位名稱").text().trim() + '</td>';
                                tabstr += '<td name="td_edit" nowrap="" align="center"><a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("guid").text().trim() + '">刪除</a>';
                                tabstr += ' <a href="edit_memberManage.aspx?guid=' + $(this).children("guid").text().trim() + '" name="editbtn">編輯</a></td>';
                                tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="11">查詢無資料</td></tr>';
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
                            case '034':
                                $("#txt5").empty();
                                $("#txt5").append(ddlstr);
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
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">帳號狀態</div>
                                        <div class="OchiCell width100">
                                            <select id="txt5" class="width100 inputex" >
                                                <option value="">請選擇</option>
                                                <option value="A">啟用</option>
                                                <option value="D">停用</option>
                                            </select>
                                        </div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">網站類別</div>
                                        <div class="OchiCell width100"><select id="txt2" class="width100 inputex" ></select></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">業者名稱</div>
                                        <div class="OchiCell width100">
                                            <select id="txt3" class="width100 inputex" >
                                                <option value="">請選擇</option>
                                            </select>
                                        </div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">姓名</div>
                                        <div class="OchiCell width100"><input type="text" id="txt4" class="inputex width100 "></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiHalf -->
                            </div><!-- OchiTrasTable -->
                            <br />
                            <div class="twocol">
                                <div class="left">
                                    
                                </div>
                                <div class="right">
                                    <a id="newbtn" href="edit_memberManage.aspx" title="新增" class="genbtn" >新增</a> <a id="subbtn" href="javascript:void(0);" title="查詢" class="genbtn" >查詢</a>
                                </div>
                            </div>
                            <br />
                            <div class="stripeMeB tbover">
                                <table id="tablist" border="0" cellspacing="0" cellpadding="0" width="100%">
                                    <thead>
							        	<tr>
							        		<th nowrap="nowrap">姓名</th>
							        		<th nowrap="nowrap">帳號類別</th>
							        		<th nowrap="nowrap">帳號狀態</th>
							        		<th nowrap="nowrap">網站類別</th>
                                            <th nowrap="nowrap">業者名稱</th>
							        		<th nowrap="nowrap">帳號</th>
							        		<th nowrap="nowrap">密碼</th>
							        		<th nowrap="nowrap">email</th>
							        		<th nowrap="nowrap">電話</th>
							        		<th nowrap="nowrap">單位名稱</th>
							        		<th nowrap="nowrap" width="100">功能</th>
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


