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

            //新增按鈕
            $(document).on("click", "#newbtn", function () {
                location.href = "edit_OilSuggestionImport.aspx?filetype=new";
            });

            //版本差異開窗
            $(document).on("click", "a[name='historybtn']", function () {
                $("#cGuid").val($(this).attr("aid"));
                getFileData();
                doOpenMagPopup();
            });

            //範本新增開窗
            $(document).on("click", "#newDemobtn", function () {
                getFileDemoData();
                doOpenMagPopup2();
            });

            //刪除按鈕
            $(document).on("click", "a[name='delbtnFile']", function () {
                if (confirm("確定刪除?")) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../handler/DelOnlyOfficeFile.aspx",
                        data: {
                            type: "data",
                            guid: $(this).attr("aid"),
                            pguid: $(this).attr("pid"),
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
                                getData(getTaiwanDate());
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
                                tabstr += '<td nowrap>' + filename + fileextension + '</td>';
                                //tabstr += '<td nowrap><a href="../DOWNLOAD.aspx?category=Oil&type=suggestionimport&cpid=' + $(this).children("業者guid").text().trim() +
                                //    '&v=' + encodeURIComponent(filename + fileextension) + '">' + filename + fileextension + '</a></td>';
                                tabstr += '<td nowrap align="center"><a href="javascript:void(0);" aid="' + $(this).children("guid").text().trim() + '" class="grebtn font-size3" name="historybtn">開啟</a></td>';
                                tabstr += '<td nowrap>' + $(this).children("上傳日期").text().trim() + '</td>';
                                tabstr += '<td name="td_editFile" nowrap="" align="center"><a href="javascript:void(0);" name="delbtnFile" aid="' + $(this).children("業者guid").text().trim() +
                                    '" pid="' + $(this).children("guid").text().trim() + '" >刪除</a> ';
                                tabstr += '<a href="edit_OilSuggestionImport.aspx?fguid=' + $(this).children("guid").text().trim() + '&nguid=' + $(this).children("業者guid").text().trim() + '">編輯</a></td>'
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

        function getFileData() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/GetFile.aspx",
                data: {
                    type: "00",
                    filetype: "data",
                    guid: $("#cGuid").val()
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
                                var year = $(this).children("年份").text().trim();
                                var month = $(this).children("月份").text().trim();
                                var day = $(this).children("日期").text().trim();
                                tabstr += '<tr>'
                                tabstr += '<td nowrap>' + (i+1) + '</td>';
                                tabstr += '<td nowrap>' + $(this).children("用印文件名稱").text().trim() + '</td>';
                                tabstr += '<td nowrap>' + $(this).children("用途").text().trim() + '</td>';
                                tabstr += '<td nowrap>' + $(this).children("件數").text().trim() + '</td>';
                                tabstr += '<td nowrap>' + $(this).children("印信名稱").text().trim() + '</td>';
                                tabstr += '<td nowrap>' + $(this).children("主管").text().trim() + '</td>';
                                tabstr += '<td nowrap>' + $(this).children("部門主管").text().trim() + '</td>';
                                tabstr += '<td nowrap>' + $(this).children("申請人").text().trim() + '</td>';
                                tabstr += '<td nowrap>民國 ' + year + '年 ' + month + '月 ' + day + '日</td>';
                                tabstr += '<td nowrap>' + $(this).children("姓名").text().trim() + '</td>';
                                tabstr += '<td nowrap>' + $(this).children("上傳日期").text().trim() + '</td>';
                                tabstr += '</tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="11">查詢無資料</td></tr>';
                        $("#tablist2 tbody").append(tabstr);
                    }
                }
            });
        }

        function getFileDemoData() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/GetDDLlist.aspx",
                data: {
                    gNo: "047",
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
                        $("#tablist3 tbody").empty();
                        var tabstr = '';
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                tabstr += '<tr>'
                                tabstr += '<td nowrap>' + $(this).children("項目名稱").text().trim() + '</td>';
                                tabstr += '<td nowrap align="center"><a href="edit_OilSuggestionImport.aspx?filetype=demo&filecategory=' + $(this).children("項目代碼").text().trim() + '" class="grebtn font-size3">新增</a></td>';
                                tabstr += '</tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="2">查詢無資料</td></tr>';
                        $("#tablist3 tbody").append(tabstr);
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
                                $("#txt2").empty();
                                $("#txt2").append(ddlstr);
                                break;
                            case '002':
                                $("#txt3").empty();
                                $("#txt3").append(ddlstr);
                                break;
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
    </script>
</head>
<body class="bgB">
	<!-- 開頭用div:修正mmenu form bug -->
	<div>
	<form id="form1">
        <input id="cGuid" type="hidden" />
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
					        <a id="newbtn" href="javascript:void(0);" title="新增" class="genbtn">新增</a> 
                            <a id="newDemobtn" href="javascript:void(0);" title="範本新增" class="genbtn">範本新增</a> 
					    </div>
					</div><br />
                    <div class="stripeMeB font-size3 margin10T">
                        <table id="tablist" width="100%" border="0" cellspacing="0" cellpadding="0">
                            <thead>
								<tr>
									<th nowrap="nowrap">年度</th>
									<th nowrap="nowrap">檔案名稱</th>
									<th nowrap="nowrap">版本差異</th>
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

	<!-- Magnific Popup -->
    <div id="messageblock" class="magpopup magSizeL mfp-hide">
      <div class="magpopupTitle">版本差異</div>
      <div class="padding10ALL">
          <div class="stripeMeB tbover">
              <table id="tablist2" border="0" cellspacing="0" cellpadding="0" width="100%">
                  <thead>
    		        	<tr>
    		        		<th nowrap="nowrap" align="center" width="5%">版本</th>
    		        		<th nowrap="nowrap" align="center">用印文件名稱</th>
    		        		<th nowrap="nowrap" align="center">用途</th>
    		        		<th nowrap="nowrap" align="center">件數</th>
    		        		<th nowrap="nowrap" align="center">印信名稱</th>
    		        		<th nowrap="nowrap" align="center">主管</th>
    		        		<th nowrap="nowrap" align="center">部門主管</th>
    		        		<th nowrap="nowrap" align="center">申請人</th>
    		        		<th nowrap="nowrap" align="center" width="10%">日期</th>
    		        		<th nowrap="nowrap" align="center" width="5%">修改者</th>
    		        		<th nowrap="nowrap" align="center" width="10%">修改日期</th>
    		        	</tr>
                  </thead>
                  <tbody></tbody>
              </table>
          </div>
    
      </div><!-- padding10ALL -->
    
    </div><!--magpopup -->

    <div id="messageblock2" class="magpopup magSizeS mfp-hide">
      <div class="magpopupTitle">文件範本</div>
      <div class="padding10ALL">
          <div class="stripeMeB tbover">
              <table id="tablist3" border="0" cellspacing="0" cellpadding="0" width="100%">
                  <thead>
       		        	<tr>
       		        		<th nowrap="nowrap" align="center" width="85%">範本名稱</th>
       		        		<th nowrap="nowrap" align="center" width="15%">功能</th>
       		        	</tr>
                  </thead>
                  <tbody></tbody>
              </table>
          </div>
    
      </div><!-- padding10ALL -->
    
    </div><!--magpopup -->


		<script type="text/javascript" src="../js/GenCommon.js"></script><!-- UIcolor JS -->
		<script type="text/javascript" src="../js/PageCommon.js"></script><!-- 系統共用 JS -->
		<script type="text/javascript" src="../js/MenuGas.js"></script><!-- 系統共用 JS -->
		<script type="text/javascript" src="../js/autoHeight.js"></script><!-- 高度不足頁面的絕對置底footer -->
	</body>
</html>
