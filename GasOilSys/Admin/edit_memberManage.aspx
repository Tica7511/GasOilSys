<%@ Page Language="C#" AutoEventWireup="true" CodeFile="edit_memberManage.aspx.cs" Inherits="Admin_edit_memberManager" %>
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
            if ($.getQueryString("guid") == "") {
                $("#div_mode_1").empty();
                $("#div_mode_1").append('<div class="OchiHalf"><div class="OchiCell OchiTitle IconCe TitleSetWidth">帳號</div><div class="OchiCell width100"><input type="text" id="txt5" class="inputex width100 "></div>' +
                    '</div><!-- OchiHalf --><div class="OchiHalf"><div class="OchiCell OchiTitle IconCe TitleSetWidth">密碼</div><div class="OchiCell width100"><input type="password" id="txt6" class="inputex width100 "></div>' +
                    '</div><!-- OchiHalf -->');
                $("#div_mode_2").empty();
                $("#div_mode_2").append('<div class="OchiHalf"><div class="OchiCell OchiTitle IconCe TitleSetWidth">email</div><div class="OchiCell width100"><input type="text" id="txt7" class="inputex width100 "></div>' +
                    '</div><!-- OchiHalf --><div class="OchiHalf"><div class="OchiCell OchiTitle IconCe TitleSetWidth">電話</div><div class="OchiCell width100"><input type="text" id="txt8" class="inputex width100 "></div>' +
                    '</div><!-- OchiHalf -->');
                $("#div_mode_3").empty();
                $("#div_mode_3").append('<div class="OchiHalf"><div class="OchiCell OchiTitle IconCe TitleSetWidth">單位名稱</div><div class="OchiCell width100"><input type="text" id="txt9" class="inputex width100 "></div>' +
                    '</div><!-- OchiHalf -->');

                $("#div_password").show();
            }
            else {
                $("#div_mode_1").empty();
                $("#div_mode_1").append('<div class="OchiHalf"><div class="OchiCell OchiTitle IconCe TitleSetWidth">帳號</div><div class="OchiCell width100"><input type="text" id="txt5" class="inputex width100 "></div>' +
                    '</div><!-- OchiHalf --><div class="OchiHalf"><div class="OchiCell OchiTitle IconCe TitleSetWidth">email</div><div class="OchiCell width100"><input type="text" id="txt7" class="inputex width100 "></div>' +
                    '</div><!-- OchiHalf -->');
                $("#div_mode_2").empty();
                $("#div_mode_2").append('<div class="OchiHalf"><div class="OchiCell OchiTitle IconCe TitleSetWidth">電話</div><div class="OchiCell width100"><input type="text" id="txt8" class="inputex width100 "></div>' +
                    '</div><!-- OchiHalf --><div class="OchiHalf"><div class="OchiCell OchiTitle IconCe TitleSetWidth">單位名稱</div><div class="OchiCell width100"><input type="text" id="txt9" class="inputex width100 "></div>' +
                    '</div><!-- OchiHalf -->');
                $("#div_mode_3").empty();

                $("#div_password").hide();
            }                

            getDDL('001');
            getDDL('002');
            getData();

            $(document).on("click", "#subbtn", function () {
                var mode = ($.getQueryString("guid") == "") ? "new" : "mod";

                var msg = '';
                var emailPattern = /^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,6}$/;
                if ($("#txt1").val() == "")
                    msg += "請輸入【姓名】\n";
                if ($("#txt2").val() == "")
                    msg += "請選擇【帳號類別】\n";
                else
                    if ($("#txt2").val() == '02') {
                        if ($("#txt3").val() == "")
                            msg += "請選擇【網站類別】\n";
                        if ($("#txt4").val() == "")
                            msg += "請選擇【業者名稱】\n";
                    }                        
                if ($("#txt5").val() == "")
                    msg += "請輸入【帳號】\n";
                if (mode == "new")
                    if ($("#txt6").val() == "")
                        msg += "請輸入【密碼】\n";                
                if ($("#txt7").val() != "")
                    if (!emailPattern.test($("#txt7").val()))
                        msg += "【email】請輸入有效的電子郵件地址\n";

                if (msg != "") {
                    alert("Error message: \n" + msg);
                    return false;
                }

                // Get form
                var form = $('#form1')[0];

                // Create an FormData object 
                var data = new FormData(form);

                // If you want to add an extra field for the FormData
                data.append("guid", $.getQueryString("guid"));
                data.append("txt1", encodeURIComponent($("#txt1").val()));
                data.append("txt2", encodeURIComponent($("#txt2").val()));
                data.append("txt3", encodeURIComponent($("#txt3").val()));
                data.append("txt4", encodeURIComponent($("#txt4").val()));
                data.append("txt5", encodeURIComponent($("#txt5").val()));
                data.append("txt6", encodeURIComponent($("#txt6").val()));
                data.append("txt7", encodeURIComponent($("#txt7").val()));
                data.append("txt8", encodeURIComponent($("#txt8").val()));
                data.append("txt9", encodeURIComponent($("#txt9").val()));
                data.append("mode", mode);

                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "BackEnd/AddMember.aspx",
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
                            if ($("relogin", data).text() == 'Y')
                                location.href = "../Handler/SignOut.aspx";
                            else
                                location.href = "memberManage.aspx";
                        }
                    }
                });
            });

            //選擇帳號類別
            $(document).on("change", "#txt2", function () {
                if ($("#txt2 option:selected").val() == '02') {
                    $("#txt3").attr('disabled', false);
                    $("#txt4").attr('disabled', false);
                }
                else if ($("#txt2 option:selected").val() == '') {
                    $("#txt3").val('');
                    $("#txt4").val('');
                    $("#txt4").empty();
                    $("#txt4").append('<option value="">請選擇</option>');
                    $("#txt3").attr('disabled', false);
                    $("#txt4").attr('disabled', false);
                }
                else {
                    $("#txt3").val('');
                    $("#txt4").val('');
                    $("#txt4").empty();
                    $("#txt4").append('<option value="">請選擇</option>');
                    $("#txt3").attr('disabled', true);
                    $("#txt4").attr('disabled', true);
                }
            });

            //選擇網站類別
            $(document).on("change", "#txt3", function () {
                if ($("#txt3 option:selected").val() == '01') {
                    getOilCompanyList();
                }
                else if ($("#txt3 option:selected").val() == '02') {
                    getGasCompanyList();
                }
                else {
                    $("#txt4").empty();
                    $("#txt4").append('<option value="">請選擇</option>');
                }
            });

            $(document).on("click", "#cancelbtn", function () {
                if (confirm("確定取消?")) {
                    location.href = "memberManage.aspx";
                }
            });
        }); // end js

        function getData() {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
                url: "BackEnd/GetMember.aspx",
                data: {
                    type: "data",
                    guid: $.getQueryString("guid"),
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
                                $("#txt1").val($(this).children("姓名").text().trim());
                                $("#txt2").val($(this).children("帳號類別").text().trim());
                                if ($(this).children("帳號類別").text().trim() != '02') {
                                    $("#txt3").val('');
                                    $("#txt3").attr("disabled", true);
                                    $("#txt4").attr("disabled", true);
                                }
                                else {
                                    $("#txt3").val($(this).children("網站類別").text().trim());

                                    if ($(this).children("網站類別").text().trim() == '01')
                                        getOilCompanyList($(this).children("網站類別").text().trim());
                                    else
                                        getGasCompanyList($(this).children("網站類別").text().trim());

                                    $("#txt4").val($(this).children("業者guid").text().trim());
                                }                                    
                                $("#txt5").val($(this).children("使用者帳號").text().trim());
                                $("#txt6").val($(this).children("使用者密碼").text().trim());
                                $("#txt7").val($(this).children("mail").text().trim());
                                $("#txt8").val($(this).children("電話").text().trim());
                                $("#txt9").val($(this).children("單位名稱").text().trim());
							});
						}
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

        function getOilCompanyList() {
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
                        $("#txt4").empty();
                        $("#txt4").append(ddlstr);
                    }
                }
            });
        }

        function getGasCompanyList() {
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
                        $("#txt4").empty();
                        $("#txt4").append(ddlstr);
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
                                <div class="left ">
                                </div>
                                <div class="right">
                                    <a id="cancelbtn" href="javascript:void(0);" title="返回" class="genbtn" >取消</a>
                                    <a id="subbtn" href="javascript:void(0);" title="儲存" class="genbtn" >儲存</a>
                                </div>
                            </div><br />
                            <div class="OchiTrasTable width100 TitleLength09 font-size3">
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">姓名</div>
                                        <div class="OchiCell width100"><input type="text" id="txt1" class="inputex width100 "></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">帳號類別</div>
                                        <div class="OchiCell width100"><select id="txt2" class="width100 inputex" ></select></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">網站類別</div>
                                        <div class="OchiCell width100"><select id="txt3" class="width100 inputex" ></select></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">業者名稱</div>
                                        <div class="OchiCell width100">
                                            <select id="txt4" class="width100 inputex" >
                                                <option value="">請選擇</option>
                                            </select>
                                        </div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div id="div_mode_1" class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">帳號</div>
                                        <div class="OchiCell width100"><input type="text" id="txt5" class="inputex width100 "></div>
                                    </div><!-- OchiHalf -->
                                    <div id="div_password" class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">密碼</div>
                                        <div class="OchiCell width100"><input type="password" id="txt6" class="inputex width100 "></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div id="div_mode_2" class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">email</div>
                                        <div class="OchiCell width100"><input type="text" id="txt7" class="inputex width100 "></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">電話</div>
                                        <div class="OchiCell width100"><input type="text" id="txt8" class="inputex width100 "></div>
                                    </div><!-- OchiHalf -->                                    
                                </div><!-- OchiRow -->
                                <div id="div_mode_3" class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">單位名稱</div>
                                        <div class="OchiCell width100"><input type="text" id="txt9" class="inputex width100 "></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                            </div><!-- OchiTrasTable -->
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



