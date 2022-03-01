<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GasCommitteeToCompany.aspx.cs" Inherits="Admin_OilCommitteeToCompany" %>

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

            //選擇年份
            $(document).on("change", "#sellist", function () {
                getCommittee($("#sellist option:selected").val());
            });

            //編輯開窗
            $(document).on("click", "a[name='editbtn']", function () {
                $("#CGguid").val($(this).attr("aid"));

                var cguid = $("#CGguid").val();
                
                var sp2 = $("span[name='sp2_" + $("#CGguid").val() + "']").text();
                var sp3 = $("span[name='sp3_" + cguid + "']").text();
                var sp4 = $("span[name='sp4_" + cguid + "']").text();

                $("#cpNameIsConfirm").html(sp2 + sp3 + sp4);

                getYearList();
                $("#sellist").val(getTaiwanDate());
                getCommittee(getTaiwanDate());
                doOpenMagPopup();
            });

            $(document).on("click", "#newbtn", function () {
                $("#cpNameSP").val($("#cpNameIsConfirm").text());
                $("#cpNameIsConfirm2").html($("#cpNameSP").val());
                getCommitteeTable();
                getCommitteeList();
                doOpenMagPopup2();
            });

            $(document).on("click", "a[name='addbtn']", function () {
				$("#errMsg").empty();
				if ($(this).closest("tr").find("select[name='committeeName']").val() == "") {
					$("#errMsg").html("請先新增委員");
				}
				else {
					$("#tablist3 tbody").append($(this).closest("tr")[0].outerHTML);
					$(this).attr("name", "delcommitteebtn");
					$(this).text("刪除");
				}
            });

            $(document).on("click", "a[name='delcommitteebtn']", function () {
				$(this).parent().parent().remove();
            });

            $(document).on("click", "#cancelbtn", function () {
                $("#cpNameIsConfirm").html($("#cpNameSP").val());
                getCommitteeTable();
                getCommitteeList();
                doOpenMagPopup();
            });

            $(document).on("click", "#subbtn", function () {
				$("#errMsg").empty();
				var msg = '';

				var xmlstr = document.createElement("result");
				$("#tablist3 tbody tr").each(function () {
                    if ($(this).find("select[name='committeeName']").val() != "") {

                        var child = document.createElement("item");
                        child.setAttribute("year", getTaiwanDate());
                        child.setAttribute("cpGuid", $("#CGguid").val());
                        child.setAttribute("cGuid", $(this).find("select[name='committeeName']").val());
                        child.setAttribute("cName", $(this).find("select[name='committeeName'] option:selected").text());
                        xmlstr.appendChild(child);
                    }
                    else {
                        msg = "請選擇委員";
						return false;
                    }
				});

				if (msg != "") {
					$("#errMsg").html(msg);
					return false;
				}

				$.ajax({
					type: "POST",
					async: false, //在沒有返回值之前,不會執行下一步動作
					url: "BackEnd/AddGasCommittee.aspx",
					data: {
						xStr: encodeURIComponent(xmlstr.outerHTML)
					},
					error: function (xhr) {
						$("#errMsg").html("Error: " + xhr.status);
						console.log(xhr.responseText);
					},
					success: function (data) {
						if ($(data).find("Error").length > 0) {
							$("#errMsg").html($(data).find("Error").attr("Message"));
						}
						else {
                            alert($("Response", data).text());
                            $("#cpNameIsConfirm").html($("#cpNameSP").val());
                            getCommittee(getTaiwanDate());
                            doOpenMagPopup();
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
                        url: "BackEnd/DelGasCommittee.aspx",
                        data: {
                            year: getTaiwanDate(),
                            cpguid: $("#CGguid").val(),
                            cguid: $(this).attr("aid")
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
                                getCommittee(getTaiwanDate());
                            }
                        }
                    });
                }
            });

        }); // end js

        function getData() {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetGasCompanyList.aspx",
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
								tabstr += '<td nowrap="nowrap"><span name="sp1_' + $(this).children("guid").text().trim() + '">' + $(this).children("公司名稱").text().trim() + '</span></td>';
								tabstr += '<td nowrap="nowrap"><span name="sp2_' + $(this).children("guid").text().trim() + '">' + $(this).children("事業部").text().trim() + '</span></td>';
								tabstr += '<td nowrap="nowrap"><span name="sp3_' + $(this).children("guid").text().trim() + '">' + $(this).children("營業處廠").text().trim() + '</span></td>';
                                tabstr += '<td nowrap="nowrap"><span name="sp4_' + $(this).children("guid").text().trim() + '">' + $(this).children("中心庫區儲運課工場").text().trim() + '</span></td>';
								tabstr += '<td align="center" nowrap="nowrap" class="font-normal"><a name="editbtn" href="javascript:void(0);" aid="' + $(this).children("guid").text().trim() + '">查核委員</a>';
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

        function getCommittee(year) {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
                url: "BackEnd/GetGasCommitteeList.aspx",
                data: {
                    type: "list",
                    cid: $("#CGguid").val(),
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
						$("#tablist2 tbody").empty();
						var tabstr = '';
						if ($(data).find("data_item").length > 0) {
							$(data).find("data_item").each(function (i) {
								tabstr += '<tr>';
								tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("itemNo").text().trim() + '</td>';
								tabstr += '<td align="center" nowrap="nowrap">' + $(this).children("委員姓名").text().trim() + '</td>';
								tabstr += '<td name="td_edit" align="center" nowrap="nowrap" class="font-normal"><a name="delbtn" href="javascript:void(0);" aid="' + $(this).children("委員guid").text().trim() + '">刪除</a>';
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="3">查詢無資料</td></tr>';
                        $("#tablist2 tbody").append(tabstr);

                        //確認權限&按鈕顯示或隱藏
                        if ($("#sellist").val() != getTaiwanDate()) {
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
				}
			});
        }

        //取得民國年份之下拉選單
        function getYearList() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "BackEnd/GetGasCommitteeList.aspx",
                data: {
                    cid: $("#CGguid").val(),
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

        function getCommitteeList() {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetCommitteeList.aspx",
				error: function (xhr) {
					alert("Error: " + xhr.status);
                    console.log(xhr.responseText);
				},
				success: function (data) {
					if ($(data).find("Error").length > 0) {
						alert($(data).find("Error").attr("Message"));
					}
					else {
						var ddlstr = '';
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                ddlstr += '<option value="' + $(this).children("guid").text().trim() + '">' + $(this).children("姓名").text().trim() + '</option>';
							});
						}
                        else {
                            ddlstr += '<option value="">目前暫無委員</option>';
                        }
                        $("select[name='committeeName']").append(ddlstr);
					}
				}
			});
        }

        function getCommitteeTable() {
			$("#errMsg").empty();
			$("#tablist3 tbody").empty();
			var tabstr = '';
			tabstr += '<tr>';
			tabstr += '<td align="center"><select name="committeeName" class="inputex width100"></select></td>';
			tabstr += '<td align="center" class="font-normal">';
			tabstr += '<a href="javascript:void(0);" name="addbtn">新增</a>';
			tabstr += '</td></tr>';
			$("#tablist3 tbody").append(tabstr);
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
        <input type="hidden" id="cpNameSP" />
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
							        		<th nowrap="nowrap">事業部</th>
							        		<th nowrap="nowrap">營業處廠</th>
							        		<th nowrap="nowrap">中心庫區儲運課工場</th>
							        		<th nowrap="nowrap" width="100">功能</th>
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
      <div class="twocol">
          <div class="left font-size5 "><i class="fa fa-chevron-circle-right IconCa" aria-hidden="true"></i> 
              <select id="sellist" class="inputex">
              </select> 年
          </div>
          <div class="right">
          <a id="newbtn" href="javascript:void(0);" title="新增" class="genbtn">新增</a>
          </div>
      </div><br />
      <div class="stripeMeB tbover">
          <table id="tablist2" border="0" cellspacing="0" cellpadding="0" width="100%">
              <thead>
		        	<tr>
		        		<th nowrap="nowrap" align="center" width="50">項目</th>
		        		<th nowrap="nowrap" align="center">委員名稱</th>
		        		<th id="th_edit" nowrap="nowrap" align="center" width="100">功能</th>
		        	</tr>
              </thead>
              <tbody></tbody>
          </table>
      </div>

  </div><!-- padding10ALL -->

</div><!--magpopup -->

<!-- Magnific Popup -->
<div id="messageblock2" class="magpopup magSizeS mfp-hide">
  <div class="magpopupTitle"><span id="cpNameIsConfirm2"></span> - 新增委員</div>
  <div class="padding10ALL">
      <div class="twocol">
      </div><br />
      <div class="stripeMeB tbover">
          <table id="tablist3" border="0" cellspacing="0" cellpadding="0" width="100%">
              <thead>
		        	<tr>
		        		<th nowrap="nowrap" align="center" width="200">委員名稱</th>
		        		<th nowrap="nowrap" align="center" width="50">功能</th>
		        	</tr>
              </thead>
              <tbody></tbody>
          </table>
      </div>

      <div class="twocol margin5TB">
		<div class="left"><span id="errMsg" style="color:red;"></span></div>
			<div class="right">
				<a id="cancelbtn" href="javascript:void(0);" class="genbtn">取消</a>
				<a id="subbtn" href="javascript:void(0);" class="genbtn">儲存</a>
			</div>
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


