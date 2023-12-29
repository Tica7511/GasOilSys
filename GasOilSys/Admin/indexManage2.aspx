<%@ Page Language="C#" AutoEventWireup="true" CodeFile="indexManage2.aspx.cs" Inherits="Admin_indexManage2" %>

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
	<script type="text/javascript">
        $(document).ready(function () {
            getData();

            //編輯開窗
            $(document).on("click", "a[name='editbtn']", function () {
                $("#CGguid").val($(this).attr("aid"));

                var cguid = $("#CGguid").val();
                
                var sp2 = $("span[name='sp2_" + $("#CGguid").val() + "']").text();
                var sp3 = $("span[name='sp3_" + cguid + "']").text();
                var sp4 = $("span[name='sp4_" + cguid + "']").text();
                var sp5 = $("span[name='sp5_" + cguid + "']").text();
                var sp6 = $("span[name='sp6_" + cguid + "']").text();
                var txt1 = $("input[name='rd_" + cguid + "']:checked").val();

                $("#cpNameIsConfirm").html(sp2 + sp3 + sp4 + sp5 + sp6);
                $("input[name='txt1'][value='" + txt1 + "']").prop("checked", true);

                doOpenMagPopup();
            });

            //開窗儲存
            $(document).on("click", "#cSubbtn", function () {
                // Get form
                var form = $('#form1')[0];

                // Create an FormData object 
                var data = new FormData(form);

                // If you want to add an extra field for the FormData
                data.append("guid", $("#CGguid").val());
                data.append("type", encodeURIComponent('Oil'));
                data.append("txt1", encodeURIComponent($("input[name='txt1']:checked").val()));

                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "BackEnd/AddIsConfirm.aspx",
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
                            $.magnificPopup.close();
                        }
                    }
                });
            });

            $(document).ajaxStart(function () {
                //$("input[name='rds_539DBDA9-4163-4475-952A-C2D8F99C5B6B']").attr("disabled", true);
            });

            $(document).on("click", "input[type='radio']", function () {
                if (confirm('確認更新資料?')) {
                    var confirmType = $(this).attr("tid");
                    var guid = $(this).attr("aid");
                    var Cvalue = $(this).val();

                    switch (confirmType) {
                        case "01":
                            Cvalue = $(this).val();
                            break;
                        case "02":
                            if (Cvalue == "是")
                                Cvalue = getTaiwanDate();
                            else
                                Cvalue = 'N';
                            break;
                        case "03":
                            if (Cvalue == "是")
                                Cvalue = getTaiwanDate();
                            else
                                Cvalue = 'N';
                            break;
                    }

                    // Get form
                    var form = $('#form1')[0];

                    // Create an FormData object 
                    var data = new FormData(form);


                    // If you want to add an extra field for the FormData
                    data.append("guid", guid);
                    data.append("type", encodeURIComponent('Oil'));
                    data.append("confirmType", encodeURIComponent(confirmType));
                    data.append("txt1", encodeURIComponent(Cvalue));

                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "BackEnd/AddIsConfirm.aspx",
                        data: data,
                        processData: false,
                        contentType: false,
                        cache: false,
                        error: function (xhr) {
                            alert("Error: " + xhr.status);
                            console.log(xhr.responseText);
                        },
                        beforeSend: function () {
                            //$("input[name='rds_539DBDA9-4163-4475-952A-C2D8F99C5B6B']").prop("disabled", true);
                        },
                        complete: function () {
                            //$("input[name='rds_539DBDA9-4163-4475-952A-C2D8F99C5B6B']").attr("disabled", false);
                        },
                        success: function (data) {
                            if ($(data).find("Error").length > 0) {
                                alert($(data).find("Error").attr("Message"));
                            }
                            else {
                                getData();
                                alert($("Response", data).text());
                            }
                        }
                    });
                }
                else {
                    return false;
                }
            });

        }); // end js

        function getData() {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetOilCompanyList.aspx",
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
                        isConfirm01 = '';
                        isConfirm02 = '';
                        isStorageConfirm01 = '';
                        isStorageConfirm02 = '';
                        isStorageLiqConfirm01 = '';
                        isStorageLiqConfirm02 = '';
						if ($(data).find("data_item").length > 0) {
							$(data).find("data_item").each(function (i) {
								tabstr += '<tr>';
								tabstr += '<td nowrap="nowrap"><span name="sp1_' + $(this).children("guid").text().trim() + '">' + $(this).children("公司名稱").text().trim() + '</span></td>';
								tabstr += '<td nowrap="nowrap"><span name="sp2_' + $(this).children("guid").text().trim() + '">' + $(this).children("處").text().trim() + '</span></td>';
								tabstr += '<td nowrap="nowrap"><span name="sp3_' + $(this).children("guid").text().trim() + '">' + $(this).children("事業部").text().trim() + '</span></td>';
								tabstr += '<td nowrap="nowrap"><span name="sp4_' + $(this).children("guid").text().trim() + '">' + $(this).children("營業處廠").text().trim() + '</span></td>';
								tabstr += '<td nowrap="nowrap"><span name="sp5_' + $(this).children("guid").text().trim() + '">' + $(this).children("組").text().trim() + '</span></td>';
                                tabstr += '<td nowrap="nowrap"><span name="sp6_' + $(this).children("guid").text().trim() + '">' + $(this).children("中心庫區儲運課工場").text().trim() + '</span></td>';
                                isConfirm01 = ($(this).children("資料是否確認").text().trim() == "是") ? '<input name="rd_' + $(this).children("guid").text().trim() + '" aid="' + $(this).children("guid").text().trim() + '" tid="01" type="radio" value="是" checked="checked" />' : '<input name="rd_' + $(this).children("guid").text().trim() + '" aid="' + $(this).children("guid").text().trim() + '" tid="01" type="radio" value="是" />';
                                isConfirm02 = ($(this).children("資料是否確認").text().trim() == "否") ? '<input name="rd_' + $(this).children("guid").text().trim() + '" aid="' + $(this).children("guid").text().trim() + '" tid="01" type="radio" value="否" checked="checked" />' : '<input name="rd_' + $(this).children("guid").text().trim() + '" aid="' + $(this).children("guid").text().trim() + '" tid="01" type="radio" value="否" />';
								tabstr += '<td>' + isConfirm01 + '是</td>';
                                tabstr += '<td>' + isConfirm02 + '否</td>';
                                isStorageConfirm01 = ($(this).children("年度儲槽確認").text().trim() == getTaiwanDate()) ? '<input name="rds_' + $(this).children("guid").text().trim() + '" aid="' + $(this).children("guid").text().trim() + '" tid="02" type="radio" value="是" checked="checked" />' : '<input name="rds_' + $(this).children("guid").text().trim() + '" aid="' + $(this).children("guid").text().trim() + '" tid="02" type="radio" value="是" />';
                                isStorageConfirm02 = ($(this).children("年度儲槽確認").text().trim() != getTaiwanDate()) ? '<input name="rds_' + $(this).children("guid").text().trim() + '" aid="' + $(this).children("guid").text().trim() + '" tid="02" type="radio" value="否" checked="checked" />' : '<input name="rds_' + $(this).children("guid").text().trim() + '" aid="' + $(this).children("guid").text().trim() + '" tid="02" type="radio" value="否" />';
                                tabstr += '<td>' + isStorageConfirm01 + '是</td>';
                                tabstr += '<td>' + isStorageConfirm02 + '否</td>';
                                isStorageLiqConfirm01 = ($(this).children("年度液化石油氣儲槽確認").text().trim() == getTaiwanDate()) ? '<input name="rdsl_' + $(this).children("guid").text().trim() + '" aid="' + $(this).children("guid").text().trim() + '" tid="03" type="radio" value="是" checked="checked" />' : '<input name="rdsl_' + $(this).children("guid").text().trim() + '" aid="' + $(this).children("guid").text().trim() + '" tid="03" type="radio" value="是" />';
                                isStorageLiqConfirm02 = ($(this).children("年度液化石油氣儲槽確認").text().trim() != getTaiwanDate()) ? '<input name="rdsl_' + $(this).children("guid").text().trim() + '" aid="' + $(this).children("guid").text().trim() + '" tid="03" type="radio" value="否" checked="checked" />' : '<input name="rdsl_' + $(this).children("guid").text().trim() + '" aid="' + $(this).children("guid").text().trim() + '" tid="03" type="radio" value="否" />';
                                tabstr += '<td>' + isStorageLiqConfirm01 + '是</td>';
                                tabstr += '<td>' + isStorageLiqConfirm02 + '否</td>';
								//tabstr += '<td align="center" nowrap="nowrap" class="font-normal"><a name="editbtn" href="javascript:void(0);" aid="' + $(this).children("guid").text().trim() + '">編輯</a>';
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="12">查詢無資料</td></tr>';
						$("#tablist tbody").append(tabstr);
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

        function doOpenMagPopupImg() {
            $.magnificPopup.open({
                items: {
                    src: '#messageblockLoading'
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
                                <div class="right">

                                </div>
                            </div><br />
                            <div class="stripeMeB tbover">
                                <table id="tablist" border="0" cellspacing="0" cellpadding="0" width="100%">
                                    <thead>
							        	<tr>
								        	<th nowrap="nowrap">公司名稱</th>
								        	<th nowrap="nowrap">處</th>
								        	<th nowrap="nowrap">事業部</th>
								        	<th nowrap="nowrap">營業處廠</th>
								        	<th nowrap="nowrap">組</th>
								        	<th nowrap="nowrap">中心庫區儲運課工場</th>
								        	<th colspan="2" width="130">資料是否確認</th>
								        	<th colspan="2" width="130">年度儲槽確認</th>
								        	<th colspan="2" width="130">年度液化石油氣儲槽確認</th>
								        	<%--<th nowrap="nowrap" width="100">功能</th>--%>
								        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
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

<!-- Magnific Popup -->
<div id="messageblock" class="magpopup magSizeS mfp-hide">
  <div class="magpopupTitle"><span id="cpNameIsConfirm"></span></div>
  <div class="padding10ALL">

      <div class="margin35T padding5RL">
            <div class="OchiTrasTable width100 TitleLength08 font-size3">
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">資料是否確認</div>
                    <div class="OchiCell width100">
                        <input name="txt1" type="radio" value="是" /> 是 <input name="txt1" type="radio" value="否" /> 否
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

<!-- Magnific Popup -->
<div id="messageblockLoading" class="magpopup magSizeS mfp-hide">
  <div class="magpopupTitle"></div>
  <div class="padding10ALL">

      <div class="margin35T padding5RL">
          <img src="../images/loading.gif" />  
      </div>

  </div><!-- padding10ALL -->

</div><!--magpopup -->

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


