<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GasReportUpload.aspx.cs" Inherits="WebPage_GasReportUpload" %>

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
            $("#filediv").hide();

            $(document).on("click", "#filebtn", function () {
                $("#filebtn").hide();
                $("#filediv").show();
            });

            $(document).on("click", "#cancelbtn", function () {
                getData();
                $("#filediv").hide();
                $("#filebtn").show();
            });

            $(document).on("click", "input[name='delbtn']", function () {
                var isDel = confirm("確定刪除簡報嗎?");
                if (isDel) {
                    $.ajax({
				        type: "POST",
				        async: false, //在沒有返回值之前,不會執行下一步動作
				        url: "../Handler/DelGasReportUpload.aspx",
                        data: {
                            rid : $(this).attr("aid"),
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
                                getData();
				        	}
				        }
			        });
                }                
            });

            $(document).on("click", "#savebtn", function () {
                var msg = '';

				if ($("#fileUpload").val() == "")
                    msg += "請先選擇檔案再上傳";
                if (msg != "") {
                    alert(msg);
					return false;
				}

                // Get form
				var form = $('#form1')[0];

				// Create an FormData object 
				var data = new FormData(form);

				// If you want to add an extra field for the FormData
                data.append("cpid", $.getQueryString("cp"));
                data.append("category", "gas");
                data.append("type", "report");

                $.ajax({
				    type: "POST",
				    async: false, //在沒有返回值之前,不會執行下一步動作
				    url: "../Handler/AddDownload.aspx",
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
                            getData();
                            $("#filediv").hide();
                            $("#filebtn").show();
				    	}
				    }
			    });
            });
		}); // end js

		function getData() {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetGasReportUpload.aspx",
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
						$("#tablist tbody").empty();
						var tabstr = '';
						if ($(data).find("data_item").length > 0) {
							$(data).find("data_item").each(function (i) {
								tabstr += '<tr>';
                                tabstr += '<td nowrap="nowrap" align="center">';
                                tabstr += '<a href="../DOWNLOAD.aspx?category=Gas&type=report&rid=' + $(this).children("guid").text().trim() + '"';
                                tabstr += '>'+ $(this).children("檔案名稱").text().trim() + '</a>';
                                tabstr += '</td>';
                                tabstr += '<td nowrap="nowrap" align="center">' + $(this).children("年度").text().trim() + '</td>';
                                tabstr += '<td nowrap="nowrap" align="center">' + $(this).children("上傳日期").text().trim() + '</td>';
                                tabstr += '<td name="ftd" nowrap="nowrap" align="center">';
                                tabstr += '<input type="button" value="刪除" class="genbtn" name="delbtn" aid="' + $(this).children("guid").text().trim() + '" />';
                                tabstr += '</td>';
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="4">查詢無資料</td></tr>';
                        $("#tablist tbody").append(tabstr);
                        if ($("#Competence").val() == "01" || $("#Competence").val() == "04" || $("#Competence").val() == "05") {
                            $("#fileall").hide();
                            $("#thFunc").hide();
                            $("td[name='ftd']").hide();
                        }
					}
				}
			});
		}

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
        <input type="hidden" id="Competence" value="<%= usercompetence %>" />
		<form id="form1">
			<!-- Preloader -->
			<div id="preloader">
				<div id="status">
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
                                        <div id="fileall" align="right">
                                        <input type="button" id="filebtn" name="filebtn" value="上傳檔案" class="genbtn" />
                                        <div id="filediv">
                                            <input type="file" id="fileUpload" name="fileUpload" />
                                            <input type="button" id="savebtn" value="上傳" class="genbtn" />
                                            <input type="button" id="cancelbtn" value="取消" class="genbtn" />
                                        </div>                                
                                        </div><br />
                                        <div class="stripeMeG tbover">
                                            <table id="tablist" width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <thead>
                                                    <tr>
                                                        <th nowrap width="50%">檔案名稱 </th>
                                                        <th nowrap width="10%">年度 </th>
                                                        <th nowrap width="30%">上傳日期 </th>
                                                        <th id="thFunc" nowrap width="10%">功能 </th>
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
			<div id="sidebar-wrapper"></div><!-- sidebar-wrapper -->
		</form>
	</div>
	<!-- 結尾用div:修正mmenu form bug -->

	<!-- 本頁面使用的JS -->
	<script type="text/javascript" src="../js/GenCommon.js"></script><!-- UIcolor JS -->
	<script type="text/javascript" src="../js/PageCommon.js"></script><!-- 系統共用 JS -->
	<script type="text/javascript" src="../js/MenuGas.js"></script><!-- 系統共用 JS -->
	<script type="text/javascript" src="../js/SubMenuGasA.js"></script><!-- 內頁選單 -->
	<script type="text/javascript" src="../js/autoHeight.js"></script><!-- 高度不足頁面的絕對置底footer -->
</body>
</html>

