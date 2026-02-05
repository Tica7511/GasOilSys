<%@ Page Language="C#" AutoEventWireup="true" CodeFile="edit_VerificationTest.aspx.cs" Inherits="WebPage_edit_VerificationTest" %>

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
    <title>查核與檢測資料系統</title>
    <!--#include file="Head_Include.html"-->
    <script type="text/javascript">
        $(document).ready(function () {
            $(".superfishmenu").html("\
<ul>\
<li><a href='Entrance.aspx' target='_self'>首頁</a></li>\
<li><a href='VerificationTest.aspx' target='_self'>查核與檢測資料</a></li>\
</ul>\
");

            getDDL('028', 'sel_type');
            //getDDL('029', 'sel_situation');
            $("#sellist").val(getTaiwanDate());
            getData();
            getDataFileRelation();

            if ($.getQueryString("guid") != '') {
                $("#sel_type").prop("disabled", true);
                $("#btn_object").hide();
                $("#btn_object_delete").hide();
            }
            else {
                $("#filediv").empty();
                $("#filediv").append('<input name="fileNameCheck" type="file" />');
                $("#sel_type").prop("disabled", false);
                $("#btn_object").show();
                $("#btn_object_delete").show();
            }

            //選擇類別
            $(document).on("change", "#sel_type", function () {
                $("#tObjectGuid").val('');
                $("#txt_object").val('');
                $("#tType").val($("#sel_type option:selected").val());
            });

            //對象請選擇按鈕
            $(document).on("click", "#btn_object", function () {
                if ($("#tType").val() != '') {
                    getObjectData();
                }
                else {
                    alert("請選擇【類別】");
                    return false;
                }

                doOpenMagPopup();
            });

            $(document).on("click", "a[name='objectbtn']", function () {

                $("#tObjectGuid").val($(this).attr('aid'));
                $("#txt_object").val($(this).attr('cname'));
                $.magnificPopup.close();
            });

            //選擇查核日期後更新該年度之場次(僅限新增時)
            $(document).on("change", "#txt_timeBegin", function () {
                if ($.getQueryString("guid") == "") {
                    //if (($("#sel_type option:selected").val() != '') && ($("#txt_object").val() != '')) {
                        
                    //}      

                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../Handler/GetVerificationTest.aspx",
                        data: {
                            type: "session",
                            //sType: $("#sel_type option:selected").val(),
                            //tobject: $("#tObjectGuid").val(),
                            timeBegin: $("#txt_timeBegin").val()
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
                                $("#txt_session").val($("Response", data).text());
                            }
                        }
                    });
                }
            });

            //刪除查核檢測報告
            $(document).on("click", "#delfileCheck", function () {
                var isDel = confirm("確定刪除檔案嗎?");
                if (isDel) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../Handler/DelVerificationTestFile.aspx",
                        data: {
                            guid: $(this).attr("aid"),
                            type: $(this).attr("atype"),
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
                                $("#filediv").empty();
                                $("#filediv").append('<input name="fileNameCheck" type="file" />');
                            }
                        }
                    });
                }
            });

            //刪除相關報告
            $(document).on("click", "a[name='delbtnFile']", function () {
                var isDel = confirm("確定刪除檔案嗎?");
                if (isDel) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../Handler/DelVerificationTestFile.aspx",
                        data: {
                            guid: $(this).attr("aid"),
                            sn: $(this).attr("asn"),
                            type: $(this).attr("atype"),
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
                                getDataFileRelation();
                            }
                        }
                    });
                }
            });

            //對象開窗取消按鈕
            //$(document).on("click", "#Ocancelbtn", function () {
            //    $.magnificPopup.close();
            //});

            //圖片編輯按鈕
            $(document).on("click", "#editbtn", function () {
                location.href = 'edit_OilLongPipeline.aspx?cp=' + $.getQueryString("cp") + '&guid=' + $("#nGuid").val();
            });

            //清除對象內文字
            $(document).on("click", "#btn_object_delete", function () {
                $("#tObjectGuid").val('');
                $("#txt_object").val('');
            });

            //場次個位數防呆用
            $(document).on("blur", "#txt_session", function () {
                var v = this.value;

                while (v.length < 2) {
                    v = '0' + v;
                }
                this.value = v;

                if (parseInt(this.value) == 0) {
                    this.value = '01';
                }
            });

            //儲存按鍵
            $(document).on("click", "#subbtn", function () {
                var msg = '';

                if ($("#sel_type").val() == '')
                    msg += "請選擇【類別】\n";
                if ($("#txt_object").val() == '')
                    msg += "請選擇【對象】\n";
                if ($("#txt_timeBegin").val() == '')
                    msg += "請輸入【查核日期(起)】\n";
                if ($("#txt_timeEnd").val() == '')
                    msg += "請輸入【查核日期(迄)】\n";
                if (($("#txt_timeBegin").val() != '') && ($("#txt_timeEnd").val() != ''))
                    if ($("#txt_timeBegin").val() > $("#txt_timeEnd").val())
                        msg += "【查核日期(起)】不可晚於【查核日期(迄)】，請重新輸入\n";
                if ($("#txt_session").val() == '')
                    msg += "請輸入【場次】\n";
                //if ($("#sel_situation").val() == '')
                //    msg += "請輸入【改善情形】\n";

                if (msg != "") {
                    alert("Error message: \n" + msg);
                    return false;
                }

                var isCheck = '';

                // Get form
                var form = $('#form1')[0];

                // Create an FormData object 
                var data = new FormData(form);

                var mode = ($.getQueryString("guid") == "") ? "new" : "edit";

                if ($("input[name='fileNameCheck']").val() == '')
                    isCheck = '';
                else
                    isCheck = 'Y';

                var beginYear = $("#txt_timeBegin").val();

                // If you want to add an extra field for the FormData
                data.append("guid", $.getQueryString("guid"));
                data.append("mode", encodeURIComponent(mode));
                data.append("year", encodeURIComponent(beginYear.substring(0, 3)));
                data.append("isCheck", encodeURIComponent(isCheck));
                data.append("type", encodeURIComponent($("#sel_type").val()));
                data.append("objectName", encodeURIComponent($("#txt_object").val()));
                data.append("cp", encodeURIComponent($("#tObjectGuid").val()));
                data.append("timeBegin", encodeURIComponent($("#txt_timeBegin").val()));
                data.append("timeEnd", encodeURIComponent($("#txt_timeEnd").val()));
                data.append("session", encodeURIComponent($("#txt_session").val()));
                //data.append("situation", encodeURIComponent($("#sel_situation").val()));
                //$.each($("#fileUpload")[0].files, function (i, file) {
                //    data.append('file', file);
                //});

                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "../handler/AddVerificationTest.aspx",
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

                            //location.href = "edit_VerificationTest.aspx?guid=" + $("vguid", data).text();
                            location.href = "VerificationTest.aspx";
                        }
                    }
                });
            });

            //顯示相關報告檔案列表
            $(document).on("change", "input[name='fileNameReport']", function () {
                $("#filelist").empty();
                var fp = $("input[name='fileNameReport']");
                var lg = fp[0].files.length; // get length
                var items = fp[0].files;
                var fragment = "";

                if (lg > 0) {
                    for (var i = 0; i < lg; i++) {
                        var fileName = items[i].name; // get file name

                        // append li to UL tag to display File info
                        fragment += "<label>" + (i + 1) + ". " + fileName + "</label></br>";
                    }

                    $("#filelist").append(fragment);
                }
            });

            //取消按鈕
            $(document).on("click", "#cancelbtn", function () {
                var isDel = confirm("尚未儲存的部分將不會更改，確定返回嗎?");
                if (isDel) {
                    location.href = "VerificationTest.aspx";
                }
            });

            $(".pickDate").datepick({
                dateFormat: 'yymmdd',
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: '../images/calendar.gif',
                yearRange: 'c-60:c+10'
            }).BootStrap(); //BootStrap() 產生符合 BootStrap 的樣式內容
        }); // end js

        function getData() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/GetVerificationTest.aspx",
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
                                $("#sel_type").val($(this).children("類別").text().trim());
                                $("#txt_object").val($(this).children("對象").text().trim());
                                $("#tObjectGuid").val($(this).children("業者guid").text().trim());
                                $("#txt_timeBegin").val($(this).children("查核日期起").text().trim());
                                $("#txt_timeEnd").val($(this).children("查核日期迄").text().trim());
                                $("#txt_session").val($(this).children("場次").text().trim());
                                //$("#sel_situation").val($(this).children("改善情形").text().trim());

                                if (($(this).children("新檔名").text().trim() == '') || ($(this).children("新檔名").text().trim() == null)) {
                                    $("#filediv").empty();
                                    $("#filediv").append('<input name="fileNameCheck" type="file" />');
                                }
                                else {
                                    $("#filediv").empty();
                                    $("#filediv").append('<a href=../DOWNLOAD.aspx?category=VerificationTest&details=10&type=Check&v=' + $(this).children("guid").text().trim() + '&sn=' + $(this).children("排序").text().trim() + '>' +
                                        $(this).children("原檔名").text().trim() + $(this).children("附檔名").text().trim() + '</a> <a id="delfileCheck" aid="' + $(this).children("guid").text().trim() + '" atype="10" class="genbtn" >刪除</a>');
                                }
                            });
                        }
                    }
                }
            });
        }

        //取得相關報告檔案列表
        function getDataFileRelation() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/GetFile.aspx",
                data: {
                    guid: $.getQueryString("guid"),
                    type: "11",
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
                                tabstr += '<td nowrap><a href="../DOWNLOAD.aspx?category=VerificationTest&type=Relation&details=11&sn=' + $(this).children("排序").text().trim() +
                                    '&v=' + $(this).children("guid").text().trim() + '">' + filename + fileextension + '</a></td>';
                                tabstr += '<td nowrap>' + $(this).children("上傳日期").text().trim() + '</td>';
                                tabstr += '<td name="td_editFile" nowrap="" align="center"><a href="javascript:void(0);" name="delbtnFile" aid="' + $(this).children("guid").text().trim() +
                                    '" asn="' + $(this).children("排序").text().trim() + '" atype="11">刪除</a></td>';
                                tabstr += '</tr>';
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="3">查詢無資料</td></tr>';
                        $("#tablistFile tbody").append(tabstr);
                    }
                }
            });
        }

        //取得對象列表
        function getObjectData() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetVerificationTest.aspx",
                data: {
                    dataType: $("#tType").val(),
                    type: "object",
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
                        $("#div_objecttable").empty();
                        var tabstr = '';
                        if (($("#tType").val() != '4') && ($("#tType").val() != '5')) {
                            tabstr += '<table width="100%" border="0" cellspacing="0" cellpadding="0"><thead><tr><th align="center" nowrap="nowrap">公司名稱</th><th align="center" nowrap="nowrap">事業部</th>' +
                                '<th align="center" nowrap="nowrap">營業處廠</th><th align="center" nowrap="nowrap">中心庫區儲運課工場</th><th align="center" nowrap="nowrap" width="100">功能</th>';
                        }
                        else {
                            tabstr += '<table width="100%" border="0" cellspacing="0" cellpadding="0"><thead><tr><th align="center" nowrap="nowrap">對象</th><th align="center" nowrap="nowrap">場站/位置</th>' +
                                '<th align="center" nowrap="nowrap" width="100">功能</th>';
                        }
                        tabstr += '</tr></thead><tbody>';

                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                tabstr += '<tr>';
                                if (($("#tType").val() != '4') && ($("#tType").val() != '5')) {
                                    tabstr += '<td nowrap="nowrap">' + $(this).children("公司名稱").text().trim() + '</td>';
                                    tabstr += '<td nowrap="nowrap">' + $(this).children("事業部").text().trim() + '</td>';
                                    tabstr += '<td nowrap="nowrap">' + $(this).children("營業處廠").text().trim() + '</td>';
                                    tabstr += '<td nowrap="nowrap">' + $(this).children("中心庫區儲運課工場").text().trim() + '</td>';
                                    tabstr += '<td name="td_edit" nowrap="" align="center"><a class="genbtn" href="javascript:void(0);" name="objectbtn" aid="'
                                        + $(this).children("guid").text().trim() + '" cname="' + $(this).children("CompanyFullName").text().trim() + '">確認對象</a></td>';
                                    tabstr += '</tr>';
                                }
                                else {
                                    tabstr += '<td nowrap="nowrap">' + $(this).children("公司名稱").text().trim() + '</td>';
                                    tabstr += '<td nowrap="nowrap">' + $(this).children("場佔位置").text().trim() + '</td>';
                                    tabstr += '<td name="td_edit" nowrap="" align="center"><a class="genbtn" href="javascript:void(0);" name="objectbtn" aid="'
                                        + $(this).children("guid").text().trim() + '" cname="' + $(this).children("CompanyFullName").text().trim() + '">確認對象</a></td>';
                                    tabstr += '</tr>';
                                }
                            });
                        }
                        else
                            tabstr += '<tr><td colspan="5">查詢無資料</td></tr>';
                        $("#div_objecttable").append(tabstr);

                        //確認權限&按鈕顯示或隱藏
                        //if (($("#Competence").val() == '01') || ($("#Competence").val() == '04') || ($("#Competence").val() == '05') || ($("#Competence").val() == '06')) {
                        //    $("#newbtn").hide();
                        //    $("#th_edit").hide();
                        //    $("td[name='td_edit']").hide();
                        //}
                        //else {
                        //    $("#newbtn").show();
                        //    $("#th_edit").show();
                        //    $("td[name='td_edit']").show();
                        //}
                    }
                }
            });
        }

        //取得代碼檔列表
        function getDDL(gNo, id) {
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

                        $("#" + id).empty();
                        $("#" + id).append(ddlstr);
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
<body class="bgP">
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
        <!--#include file="VerificationHeader.html"-->
        <input type="hidden" id="Competence" value="<%= competence %>" />
        <input type="hidden" id="nGuid" />
        <input type="hidden" id="tType" />
        <input type="hidden" id="tObjectGuid" />
        <div id="ContentWrapper">
            <div class="container margin15T">
                <div class="padding10ALL">
                    <div class="filetitlewrapper">
                        <span class="filetitle font-size7">查核與檢測資料系統</span>
                    </div>
                    <br />

                    <div class="OchiTrasTable width100 font-size3 TitleLength05">
                            <div class="OchiRow">
                                <div class="OchiHalf">
                                    <div class="OchiCell OchiTitle TitleSetWidth">類別</div>
                                    <div class="OchiCell width100"> 
                                        <select id="sel_type" class="inputex width100"></select>
                                    </div>
                                </div><!-- OchiHalf -->
                                <div class="OchiHalf">
                                    <div class="OchiCell OchiTitle TitleSetWidth">對象</div>
                                    <div class="OchiCell width100">
                                        <input id="txt_object" type="text" class="inputex width70" disabled /> 
                                        <a id="btn_object" href="javascript:void(0);" title="請選擇" class="grebtn font-size3">請選擇</a>
                                        <a id="btn_object_delete" href="javascript:void(0);" title="清除" class="grebtn font-size3">清除</a>
                                    </div>
                                </div><!-- OchiHalf -->
                            </div><!-- OchiRow -->
                        </div><!-- OchiTrasTable -->

                        <div class="OchiTrasTable width100 font-size3 TitleLength05">
                            <div class="OchiRow">
                                <div class="OchiHalf">
                                    <div class="OchiCell OchiTitle TitleSetWidth">查核日期(起)</div>
                                    <div class="OchiCell width100">
                                        <input id="txt_timeBegin" type="text" class="inputex pickDate width30" disabled>
                                    </div>
                                </div><!-- OchiHalf -->
                                <div class="OchiHalf">
                                    <div class="OchiCell OchiTitle TitleSetWidth">查核日期(迄)</div>
                                    <div class="OchiCell width100">
                                        <input id="txt_timeEnd" type="text" class="inputex pickDate width30" disabled> 
                                    </div>
                                </div><!-- OchiHalf -->
                            </div><!-- OchiRow -->
                        </div><!-- OchiTrasTable -->

                        <div class="OchiTrasTable width100 font-size3 TitleLength05">
                            <div class="OchiRow">
                                <div class="OchiHalf">
                                    <div class="OchiCell OchiTitle TitleSetWidth">場次</div>
                                    <div class="OchiCell width100">
                                        <input id="txt_session" type="number" min="0" max="99" class="inputex width15" />
                                    </div>
                                </div><!-- OchiHalf -->
                                <div class="OchiHalf">
                                    <div class="OchiCell OchiTitle TitleSetWidth">查核/檢測報告</div>
                                    <div id="filediv" class="OchiCell width100"></div>                         
                                </div>
                                <%--<div class="OchiHalf">
                                    <div class="OchiCell OchiTitle TitleSetWidth">改善情形</div>
                                    <div class="OchiCell width100">
                                        <select id="sel_situation" class="inputex width100"></select>
                                    </div>                         
                                </div>--%>
                                <!-- OchiHalf -->
                            </div><!-- OchiRow -->
                        </div><!-- OchiTrasTable -->

                        <%--<div class="OchiTrasTable width100 font-size3 TitleLength05">
                            <div class="OchiRow">
                                <div class="OchiHalf">
                                    <div class="OchiCell OchiTitle TitleSetWidth">查核/檢測報告</div>
                                    <div id="filediv" class="OchiCell width100"></div>                         
                                </div>
                            </div><!-- OchiRow -->
                        </div><!-- OchiTrasTable -->--%>

                        <div class="OchiTrasTable width100 font-size3 TitleLength05">
                            <div class="OchiRow">
                                <div class="OchiHalf">
                                    <div class="OchiCell OchiTitle TitleSetWidth">相關報告</div>
                                    <div class="OchiCell width100">
                                        <input name="fileNameRelation" type="file" multiple>
                                        
                                        <br /><br />
                                        <div class="stripeMeP tbover">
                                            <table id="tablistFile" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                <thead>
		                                            	<tr>
		                                            		<th nowrap="nowrap" align="center" width="50%">檔案名稱</th>
		                                            		<th nowrap="nowrap" align="center" width="30%">上傳日期</th>
		                                            		<th id="thFunc" nowrap="nowrap" align="center" width="10%">功能</th>
		                                            	</tr>
                                                </thead>
                                                <tbody></tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div><!-- OchiHalf -->
                            </div><!-- OchiRow -->
                        </div><!-- OchiTrasTable -->

                        <div class="textright margin10T">
                            <a id="cancelbtn" href="javascript:void(0);" title="返回" class="genbtn" >取消</a>
                            <a id="subbtn" href="javascript:void(0);" title="儲存" class="genbtn" >儲存</a>
                        </div>

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
<div id="sidebar-wrapper">
   
</div><!-- sidebar-wrapper -->

</form>
</div>
<!-- 結尾用div:修正mmenu form bug -->

<!-- Magnific Popup -->
<div id="messageblock" class="magpopup magSizeL mfp-hide">
  <div class="magpopupTitle">對象列表</div>
  <div class="padding10ALL">
      <div class="twocol">
          <div class="left font-size5 ">

          </div>
          <div class="right">

          </div>
      </div><br />
      <div id="div_objecttable" class="stripeMeP tbover">

      </div>
      <br />

      <%--<div class="twocol">
          <div class="left font-size5 ">
              
          </div>
          <div class="right">
              <a href="javascript:void(0);" id="Ocancelbtn" class="genbtn">取消</a>
              <a href="javascript:void(0);" id="Osubbtn" class="genbtn">儲存</a>
          </div>
      </div>--%>

  </div><!-- padding10ALL -->

</div><!--magpopup -->

<!-- 本頁面使用的JS -->
    <script type="text/javascript">
        $(document).ready(function () {

        });
    </script>
    <script type="text/javascript" src="../js/GenCommon.js"></script><!-- UIcolor JS -->
    <script type="text/javascript" src="../js/PageCommon.js"></script><!-- 系統共用 JS -->
    <%--<script type="text/javascript" src="../js/MenuVerificationTest.js"></script>--%><!-- 系統共用 JS -->
    <script type="text/javascript" src="../js/autoHeight.js"></script><!-- 高度不足頁面的絕對置底footer -->
</body>
</html>


