<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OilSuggestionImport.aspx.cs" Inherits="WebPage_OilSuggestionImport" %>

<!DOCTYPE html>

<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta http-equiv="X-UA-Compatible" content="IE=11; IE=10; IE=9; IE=8" />
	<meta name="viewport" content="width=device-width, initial-scale=1" />
	<meta name="keywords" content="關鍵字內容" />
	<meta name="description" content="描述" /><!--告訴搜尋引擎這篇網頁的內容或摘要。--> 
	<meta name="generator" content="Notepad" /><!--告訴搜尋引擎這篇網頁是用什麼軟體製作的。--> 
	<meta name="author" content="工研院 資訊處" /><!--告訴搜尋引擎這篇網頁是由誰製作的。-->
	<meta name="copyright" content="本網頁著作權所有" /><!--告訴搜尋引擎這篇網頁是...... --> 
	<meta name="revisit-after" content="3 days" /><!--告訴搜尋引擎3天之後再來一次這篇網頁，也許要重新登錄。-->
	<title>石油查核建議管理系統</title>
	<!--#include file="Head_Include.html"-->
	<script type="text/javascript">
		$(document).ready(function () {
            getYearList();
            $("#sellist").val(getTaiwanDate());
            getData(getTaiwanDate());
		}); // end js

        function getData(year) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/GetFile.aspx",
                data: {
                    type: "17",
                    filetype: "list",
                    year: year,
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
                                var filename = $(this).children("原檔名").text().trim();
                                var fileextension = $(this).children("附檔名").text().trim();
                                tabstr += '<tr>'
								tabstr += '<td nowrap>' + $(this).children("年度").text().trim() + '</td>';
								tabstr += '<td nowrap>' + $(this).children("業者簡稱").text().trim() + '</td>';
                                tabstr += '<td nowrap><a href="../DOWNLOAD.aspx?category=VerificationTest&type=Relation&details=11&sn=' + $(this).children("排序").text().trim() +
                                    '&v=' + $(this).children("guid").text().trim() + '">' + filename + fileextension + '</a></td>';
                                tabstr += '<td nowrap>' + $(this).children("上傳日期").text().trim() + '</td>';
                                tabstr += '<td name="td_editFile" nowrap="" align="center"><a href="javascript:void(0);" name="delbtnFile" aid="' + $(this).children("guid").text().trim() +
                                    '" asn="' + $(this).children("排序").text().trim() + '" atype="11">刪除</a></td>';
                                tabstr += '</tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="5">查詢無資料</td></tr>';
                        $("#tablist tbody").append(tabstr);
                    }
                }
            });
		}

        //取得民國年份之下拉選單
        function getYearList() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetFile.aspx",
				data: {
                    type: "17",
                    filetype: "list",
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

        //取得現在時間之民國年
        function getTaiwanDate() {
            var nowDate = new Date();

            var nowYear = nowDate.getFullYear();
            var nowTwYear = (nowYear - 1911);

            return nowTwYear;
        }
    </script>
</head>
<body class="bgB">
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
				<!--#include file="OilSuggestionImportHeader.html"-->

				<div id="ContentWrapper">
            <div class="container margin15T">
                <div class="padding10ALL">
                    <div class="filetitlewrapper">
                        <span class="filetitle font-size7">年度列表</span>
                        <span class="btnright"></span>
                    </div><br />
                    
					<div class="twocol">
					    <div class="left font-size5 "><i class="fa fa-chevron-circle-right IconCa" aria-hidden="true"></i> 
					        <select id="sellist" class="inputex">
					        </select> 年
					    </div>
					    <div class="right">
					        <a id="newbtn" href="edit_OilSuggestionImport.aspx" title="新增" class="genbtn">新增</a>
					    </div>
					</div><br />
                    <div class="stripeMeB font-size3 margin10T">
                        <table id="tablist" width="100%" border="0" cellspacing="0" cellpadding="0">
                            <thead>
								<tr>
									<th nowrap="nowrap">年度</th>
									<th nowrap="nowrap">業者簡稱</th>
									<th nowrap="nowrap">檔案名稱</th>
									<th nowrap="nowrap">上傳日期</th>
									<th nowrap="nowrap" width="100">功能</th>
								</tr>
                            </thead>
                            <tbody></tbody>
                        </table>
						<div class="margin10B margin10T textcenter">
							<div id="pageblock"></div>
						</div>
                    </div><!-- stripeMe -->


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

	<!-- colorbox -->
	<div style="display:none;">
		<div id="checklistedit">
			<div class="margin35T padding5RL">
				<div class="OchiTrasTable width100 TitleLength03 font-size3">
					<div class="OchiRow">
						<div class="OchiCell OchiTitle IconCe TitleSetWidth">備註</div>
						<div class="OchiCell width100">
							<textarea id="psStr" rows="8" cols="" class="inputex width100"></textarea>
						</div>
					</div><!-- OchiRow -->
				</div><!-- OchiTrasTable -->
			</div>

			<div class="twocol margin10T">
				<div class="right">
					<a href="javascript:void(0);" class="genbtn closecolorbox">取消</a>
					<a href="javascript:void(0);" id="ps_savebtn" class="genbtn">儲存</a>
				</div>
			</div>
			<br /><br />
		</div>
	</div>


		<script type="text/javascript" src="../js/GenCommon.js"></script><!-- UIcolor JS -->
		<script type="text/javascript" src="../js/PageCommon.js"></script><!-- 系統共用 JS -->
		<script type="text/javascript" src="../js/MenuGas.js"></script><!-- 系統共用 JS -->
		<script type="text/javascript" src="../js/autoHeight.js"></script><!-- 高度不足頁面的絕對置底footer -->
	</body>
</html>
