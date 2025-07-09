<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PublicGasInfo.aspx.cs" Inherits="WebPage_PublicGasInfo" %>

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
	<title>公用天然氣事業輸儲設備現場查核系統</title>
	<!--#include file="Head_Include.html"-->
	<script type="text/javascript">
        $(document).ready(function () {
            $(".container").css("max-width", "1800px");

            getData();

            //報告開窗
            $(document).on("click", "a[name='reportbtn']", function () {
                doOpenMagPopup2();
            });

            //上傳檔案
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
                data.append("cpid", $("#CPID").val());
                data.append("category", "publicgas");
                data.append("year", getTaiwanDate());
                data.append("type", "info");
                data.append("details", $("#FType").val());
                $.each($("#fileUpload")[0].files, function (i, file) {
                    data.append('file', file);
                });

                $.ajax({
                    type: "POST",
                    async: true, //在沒有返回值之前,不會執行下一步動作
                    url: "../Handler/AddDownload.aspx",
                    data: data,
                    processData: false,
                    contentType: false,
                    cache: false,
                    error: function (xhr) {
                        alert("Error: " + xhr.status);
                        console.log(xhr.responseText);
                    },
                    beforeSend: function () {
                        $("#alertText").show();
                        $("#subbtn").prop("disabled", true);
                    },
                    complete: function () {
                        $("#alertText").hide();
                        $("#subbtn").prop("disabled", false);
                    },
                    success: function (data) {
                        if ($(data).find("Error").length > 0) {
                            alert($(data).find("Error").attr("Message"));
                        }
                        else {
                            alert($("Response", data).text());
                            getFileData($("#FType").val(), 'PublicGas', 'Info');
                            $("#filediv").hide();
                            $("#filebtn").show();
                            $("#fileUpload").val("");
                        }
                    }
                });
            });

            //刪除檔案
            $(document).on("click", "a[name='delbtn']", function () {
                var isDel = confirm("確定刪除檔案嗎?");
                if (isDel) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../Handler/DelFile.aspx",
                        data: {
                            guid: $(this).attr("aid"),
                            category: "PublicGas",
                            type: "Info",
                            details: $(this).attr("atype"),
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
                                getFileData($("#FType").val(), 'PublicGas', 'Info');
                            }
                        }
                    });
                }
            });

            //上傳檔案按鈕
            $(document).on("click", "#filebtn", function () {
                $("#filebtn").hide();
                $("#filediv").show();
            });

            //取消按鈕
            $(document).on("click", "#cancelbtn", function () {
                $("#fileUpload").val("");
                $("#filediv").hide();
                $("#filebtn").show();
            });

            //上傳查核報告開窗
            $(document).on("click", "a[name='checkreportbtn']", function () {
                $("#FType").val($(this).attr("atype"));
                $("#CPID").val($(this).attr("acpid"));
                getFileData($("#FType").val(), 'PublicGas', 'Info');
                $("#filediv").hide();
                $("#sp_fileName").html('上傳查核報告');
                doOpenMagPopup();
            });

            //上傳簡報開窗
            $(document).on("click", "a[name='reportbtn']", function () {
                $("#FType").val($(this).attr("atype"));
                $("#CPID").val($(this).attr("acpid"));
                getFileData($("#FType").val(), 'PublicGas', 'Info');
                $("#filediv").hide();
                $("#sp_fileName").html('上傳簡報');
                doOpenMagPopup();
            });

            //查核結果報告開窗
            $(document).on("click", "a[name='resultreportbtn']", function () {
                $("#FType").val($(this).attr("atype"));
                $("#CPID").val($(this).attr("acpid"));
                getFileData($("#FType").val(), 'PublicGas', 'Info');
                $("#sp_fileName").html('查核結果報告');
                $("#filediv").hide();
                doOpenMagPopup();
            });
        }); // end js

        function getData() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetPublicGasCompanyList.aspx",
                data: {
                    year: getTaiwanDate(),
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
                                //tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("排序編號").text().trim() + '</td>';
                                tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("公司名稱").text().trim() + '</td>';
                                //tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("人數").text().trim() + '</td>';
                                tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("地址").text().trim() + '</td>';
                                //tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("北中南分類").text().trim() + '</td>';
                                tabstr += '<td align="center" nowrap="nowrap" class="font-normal"><a name="reportbtn" href="javascript:void(0);"><i class="fa fa-paperclip" aria-hidden="true"></i></a>';
                                //tabstr += '<td align="center" nowrap="nowrap" class="font-normal"><a atype="14" acpid="' + $(this).children("guid").text().trim() +
                                //    '" name="checkreportbtn" href="javascript:void(0);"><i class="fa fa-paperclip" aria-hidden="true"></i></a>';
                                //tabstr += '<td align="center" nowrap="nowrap" class="font-normal"><a atype="15" acpid="' + $(this).children("guid").text().trim() +
                                //    '" name="reportbtn" href="javascript:void(0);"><i class="fa fa-paperclip" aria-hidden="true"></i></a>';
                                //tabstr += '<td align="center" nowrap="nowrap" class="font-normal"><a atype="16" acpid="' + $(this).children("guid").text().trim() +
                                //    '" name="resultreportbtn" href="javascript:void(0);"><i class="fa fa-paperclip" aria-hidden="true"></i></a>';
                                tabstr += '<td align="center" nowrap="nowrap" class="font-normal"><a href="PublicGasDetail.aspx?cp=' + $(this).children("guid").text().trim() + '">檢視</a>';
                                tabstr += '</tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="4">查詢無資料</td></tr>';
                        $("#tablist tbody").append(tabstr);
                    }
                }
            });
        }

        function getFileData(details, category, type) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/GetFile.aspx",
                data: {
                    cpid: $("#CPID").val(),
                    category: category,
                    type: details,
                    filetype: 'list'
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
                                var filename = $(this).children("原檔名").text().trim();
                                var fileextension = $(this).children("附檔名").text().trim();
                                tabstr += '<tr>'
                                tabstr += '<td nowrap><a href="../DOWNLOAD.aspx?cpid=' + $(this).children("guid").text().trim() + '&category=' + category + '&type=' + type + '&details=' + details + 
                                    '&v=' + $(this).children("guid").text().trim() + '">' + filename + fileextension + '</a></td>';
                                tabstr += '<td nowrap>' + $(this).children("上傳日期").text().trim() + '</td>';
                                tabstr += '<td name="td_editFile" nowrap="" align="center"><a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("guid").text().trim() +
                                    '" asn="' + $(this).children("排序").text().trim() + '" atype="' + $(this).children("檔案類型").text().trim() + '">刪除</a></td>';
                                tabstr += '</tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="3">查詢無資料</td></tr>';
                        $("#tablistFile tbody").append(tabstr);

                        $("#fileall").show();
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
        <input id="FType" type="hidden" />
        <input id="CPID" type="hidden" />
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
				<!--#include file="PublicGasHeader.html"-->

				<div id="ContentWrapper">
            <div class="container margin15T">
                <div class="padding10ALL">
                    <div class="filetitlewrapper">
                        <span class="filetitle font-size7">公用天然氣業者列表</span>
                        <span class="btnright"></span>
                    </div>

                    <%--<div class="BoxBgWa BoxRadiusA BoxBorderSa padding10ALL margin10T">
                        <div class="OchiTrasTable width100 font-size3 TitleLength05">
                            <div class="OchiRow">
                                <div class="OchiHalf">
                                    <div class="OchiCell OchiTitle TitleSetWidth">年度</div>
                                    <div class="OchiCell width100"><select class="inputex width100"><option>110</option></select></div>
                                </div><!-- OchiHalf -->
                                <div class="OchiHalf">
                                    <div class="OchiCell OchiTitle TitleSetWidth">單位名稱</div>
                                    <div class="OchiCell width100"><select class="inputex width100"><option>請選擇</option></select></div>
                                </div><!-- OchiHalf -->
                            </div><!-- OchiRow -->
                        </div><!-- OchiTrasTable -->

                        <div class="textright margin10T"><input type="submit" class="genbtn" value="查詢"> </div>
                    </div>


                    <div class="twocol margin10T">
                        <div class="left font-size5 "><i class="fa fa-chevron-circle-right IconCa" aria-hidden="true"></i> 查詢結果</div>
                        <div class="right font-normal font-size3">
                        </div>
                    </div>--%>

                    <div class="stripeMeG font-size3 margin10T">
                        <table id="tablist" width="100%" border="0" cellspacing="0" cellpadding="0">
                            <thead>
								<tr>
									<%--<th width="5%" nowrap="nowrap">序號</th>--%>
									<th width="40%" nowrap="nowrap">事業名稱</th>
									<%--<th width="5%" nowrap="nowrap">人數</th>--%>
									<th width="20%" nowrap="nowrap">公司地址</th>
									<th width="5%" nowrap="nowrap">報告</th>
									<%--<th width="5%" nowrap="nowrap">北中南分類</th>
									<th width="5%" nowrap="nowrap">上傳查核報告</th>
									<th width="5%" nowrap="nowrap">上傳簡報</th>
									<th width="5%" nowrap="nowrap">查核結果報告</th>--%>
									<th width="5%" nowrap="nowrap" width="100">功能</th>
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

    <!-- Magnific Popup -->
<div id="messageblock" class="magpopup magSizeM mfp-hide">
  <div class="magpopupTitle"><span id="cpNameIsConfirm2"></span><span id="sp_fileName"></span>列表</div>
  <div class="padding10ALL">
      <div class="twocol">
        <div class="left font-size5 ">
        </div>
        <div class="right">
            <div id="fileall" >
                <input type="button" id="filebtn" name="filebtn" value="上傳檔案" class="genbtn" />
            <div id="filediv">
                <input type="file" id="fileUpload" multiple="multiple" name="fileUpload" />
                <input type="button" id="savebtn" value="上傳" class="genbtn" />
                <input type="button" id="cancelbtn" value="取消" class="genbtn" />
            </div>                                
            </div>
        </div>
    </div><br>
      <div class="stripeMeB tbover">
          <table id="tablistFile" border="0" cellspacing="0" cellpadding="0" width="100%">
              <thead>
		        	<tr>
		        		<th nowrap="nowrap" align="center" width="200">檔案名稱</th>
		        		<th nowrap="nowrap" align="center" width="50">上傳日期</th>
		        		<th nowrap="nowrap" align="center" width="50">功能</th>
		        	</tr>
              </thead>
              <tbody>
              </tbody>
          </table>
      </div>
  </div><!-- padding10ALL -->

</div><!--magpopup -->

<div id="messageblock2" class="magpopup magSizeS mfp-hide">
  <div class="magpopupTitle"><span id="cpNameIsConfirm2"></span>報告列表</div>
  <div class="padding10ALL">
      <div class="twocol">
      </div><br />
      <div class="stripeMeB tbover">
          <table id="Rtablist" border="0" cellspacing="0" cellpadding="0" width="100%">
              <thead>
		        	<tr>
		        		<th nowrap="nowrap" align="center" width="200">檔案名稱</th>
		        		<th nowrap="nowrap" align="center" width="50">上傳日期</th>
		        		<th nowrap="nowrap" align="center" width="50">功能</th>
		        	</tr>
              </thead>
              <tbody>
                  <tr>
                      <td align="center"><a href="javascript:void(0);">欣隆天然氣_上傳報告.docx</a></td>
                      <td align="center">2025/01/31</td>
                      <td align="center"><a href="javascript:void(0);">刪除</a></td>
                  </tr>
                   <tr>
                       <td align="center"><a href="javascript:void(0);">欣隆天然氣_上傳報告_2.docx</a></td>
                       <td align="center">2025/01/31</td>
                       <td align="center"><a href="javascript:void(0);">刪除</a></td>
                   </tr>
              </tbody>
          </table>
      </div>
  </div><!-- padding10ALL -->

</div><!--magpopup -->
	<!-- 結尾用div:修正mmenu form bug -->


		<!-- 本頁面使用的JS -->
		<script type="text/javascript" src="../js/GenCommon.js"></script><!-- UIcolor JS -->
		<script type="text/javascript" src="../js/PageCommon.js"></script><!-- 系統共用 JS -->
		<script type="text/javascript" src="../js/MenuGas.js"></script><!-- 系統共用 JS -->
		<script type="text/javascript" src="../js/autoHeight.js"></script><!-- 高度不足頁面的絕對置底footer -->
	</body>
</html>


