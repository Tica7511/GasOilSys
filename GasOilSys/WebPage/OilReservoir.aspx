<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OilReservoir.aspx.cs" Inherits="WebPage_OilReservoir" %>

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
    <title>石油業輸儲設備查核及檢測資訊系統</title>
    <!--#include file="Head_Include.html"-->
    <script type="text/javascript">
        $(document).ready(function () {
            getYearList();
            $("#sellist").val(getTaiwanDate());
            getData(getTaiwanDate());

            //選擇年份
            $(document).on("change", "#sellist", function () {
                getData($("#sellist option:selected").val());
            });

            //編輯按鈕
            $(document).on("click", "#editbtn", function () {
                $("#sellist").attr('disabled', true);
                $("#editbtn").hide();
                $("#backbtn").show();
                $("#subbtn").show();
                $("#mode").val("edit");

                setDisplayed(false);

                if ($("input[name='checkArea'][value='06']").is(":checked"))
                    setNothing();
            });

            //圖片編輯按鈕
            $(document).on("click", "#editbtnContent", function () {
                location.href = 'edit_OilReservoir.aspx?cp=' + $.getQueryString("cp") + '&guid=' + $("#nGuid").val();
            });

            //返回按鈕
            $(document).on("click", "#backbtn", function () {
                var str = confirm('尚未儲存的部分將不會更改，確定返回嗎?');

                if (str) {
                    location.href = 'OilReservoir.aspx?cp=' + $.getQueryString("cp");
                }
            });

            //附件列表
            //$(document).on("change", "input[name='fileName']", function () {
            //    $("#filelist").empty();
            //    var fp = $("input[name='fileName']");
            //    var lg = fp[0].files.length; // get length
            //    var items = fp[0].files;
            //    var fragment = "";

            //    if (lg > 0) {
            //        for (var i = 0; i < lg; i++) {
            //            var fileName = items[i].name; // get file name
 
            //            // append li to UL tag to display File info
            //            fragment += "<label>" + (i+1) + ". " + fileName + "</label></br>";
            //        }
 
            //        $("#filelist").append(fragment);
            //    }
            //})

            //儲存
            $(document).on("click", "#subbtn", function () {

                var form = $('#form1')[0];

                // Create an FormData object
                var data = new FormData(form);

                data.append("cguid", $.getQueryString("cp"));
                data.append("mode", $("#mode").val());
                data.append("checkAreaOther", $("#checkAreaOther").val());

                $.ajax({
                    type: "POST",
                    async: true, //在沒有返回值之前,不會執行下一步動作
                    url: "../Handler/AddOilReservoir.aspx",
                    data: data,
                    processData: false,
                    contentType: false,
                    cache: false,
                    error: function (xhr) {
                        alert("Error: " + xhr.status);
                        console.log(xhr.responseText);
                    },
                    beforeSend: function () {
                        $("#subbtn").val("資料傳輸中...");
                        $("#subbtn").prop("disabled", true);
                    },
                    complete: function () {
                        $("#subbtn").val("儲存");
                        $("#subbtn").prop("disabled", false);
                    },
                    success: function (data) {
                        if ($(data).find("Error").length > 0) {
                            alert($(data).find("Error").attr("Message"));
                        }
                        else {
                            alert($("Response", data).text());
                            location.href='OilReservoir.aspx?cp=' + $.getQueryString("cp");
                        }
                    }
                });
            });

            //特殊區域 其他勾選時事件
            $(document).on("change", "input[name='checkArea'][value='05']", function () {
                if (this.checked) {
                    $("#checkAreaOther").val($("#checkAreach").val());
                    $("#checkAreaOther").attr("disabled", false);
                }
                else {
                    $("#checkAreach").val($("#checkAreaOther").val());
                    $("#checkAreaOther").val('');
                    $("#checkAreaOther").attr("disabled", true);
                }
            });

            //特殊區域 以上皆無
            $(document).on("change", "input[name='checkArea'][value='06']", function () {
                if (this.checked) {
                    setNothing();
                }
                else {
                    $("input[name='checkArea'][value='01']").attr("disabled", false);
                    $("input[name='checkArea'][value='02']").attr("disabled", false);
                    $("input[name='checkArea'][value='03']").attr("disabled", false);
                    $("input[name='checkArea'][value='04']").attr("disabled", false);
                    $("input[name='checkArea'][value='05']").attr("disabled", false);
                }
            });

        }); // end js

        function setDisplayed(status) {
            $("input[name='checkArea']").attr("disabled", status);
            if ($("input[name='checkArea'][value='05']").is(":checked"))
                $("#checkAreaOther").attr("disabled", status);
            else
                $("#checkAreaOther").attr("disabled", !status);
        }

        function setNothing() {
            $("input[name='checkArea'][value='01']").prop("checked", false);
            $("input[name='checkArea'][value='02']").prop("checked", false);
            $("input[name='checkArea'][value='03']").prop("checked", false);
            $("input[name='checkArea'][value='04']").prop("checked", false);
            $("input[name='checkArea'][value='05']").prop("checked", false);
            $("input[name='checkArea'][value='01']").attr("disabled", true);
            $("input[name='checkArea'][value='02']").attr("disabled", true);
            $("input[name='checkArea'][value='03']").attr("disabled", true);
            $("input[name='checkArea'][value='04']").attr("disabled", true);
            $("input[name='checkArea'][value='05']").attr("disabled", true);
            $("#checkAreaOther").attr("disabled", true);
            $("#checkAreaOther").val('');
            $("#checkAreach").val('');
        }

        function getData(year) {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetOilReservoir.aspx",
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
                        var constr = '';
                        $("#content").empty();

                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {

                                // 庫區特殊區域
                                var othercheck = false;
                                var arycheck = $(this).children("庫區特殊區域").text().trim().split(',');
                                $("input[name='checkArea']").prop("checked", false);
                                $.each(arycheck, function (key, value) {
                                    $("input[name='checkArea'][value='" + value + "']").prop("checked", true);
                                });
                                $("#checkAreaOther").val($(this).children("庫區特殊區域_其他").text().trim());

                                $("#nGuid").val($(this).children("guid").text().trim());

                                if ($(this).children("Content").text().trim() != '') {
                                    constr += $(this).children("Content").text().trim();
                                    $("#content").append(constr);
                                }
                                else {
                                    $("#content").append('<div id="notfound" class="BoxBorderSa BoxRadiusB padding5ALL textcenter"><div class="opa6 font-size3">目前無資料</div></div>');
                                }
                            });
                        }
                        else {
                            $("#content").append('<div id="notfound" class="BoxBorderSa BoxRadiusB padding5ALL textcenter"><div class="opa6 font-size3">目前無資料</div></div>');
                        }

                        //確認權限&按鈕顯示或隱藏
                        if ($("#sellist").val() != getTaiwanDate()) {
                            $("#editbtn").hide();
                            $("#editbtnContent").hide();
                        }
                        else {
                            if (($("#Competence").val() == '01') || ($("#Competence").val() == '04') || ($("#Competence").val() == '05') || ($("#Competence").val() == '06')) {
                                $("#editbtn").hide();
                                $("#editbtnContent").hide();
                            }
                            else {
                                $("#editbtn").show();
                                $("#editbtnContent").show();
                            }
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
                url: "../Handler/GetOilReservoir.aspx",
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
        <!--#include file="OilHeader.html"-->
        <div id="ContentWrapper">
            <input type="hidden" id="mode" />
            <input type="hidden" id="Competence" value="<%= competence %>" />
            <input type="hidden" id="checkAreach" />
            <input type="hidden" id="nGuid" />
            <div class="container margin15T">
                <div class="padding10ALL">
                    <div class="filetitlewrapper"><!--#include file="OilBreadTitle.html"--></div>

                    <div class="row margin20T">
                        <div class="col-lg-3 col-md-4 col-sm-5">
                            <div id="navmenuV"><!--#include file="OilLeftMenu.html"--></div>
                        </div>
                        <div class="col-lg-9 col-md-8 col-sm-7">
                            <div class="twocol">
                                <div class="left font-size5 "><i class="fa fa-chevron-circle-right IconCa" aria-hidden="true"></i> 
                                    <select id="sellist" class="inputex">
                                    </select> 年
                                </div>
                                <div class="right">
                                    <a id="editbtn" href="javascript:void(0);" title="編輯" class="genbtn">編輯</a>
                                    <a id="backbtn" href="javascript:void(0);" title="返回" class="genbtn" style="display:none">返回</a>
                                    <a id="subbtn" href="javascript:void(0);" class="genbtn" style="display:none">儲存</a>
                                </div>
                            </div><br />
                            <div class="font-size3 lineheight03">
                                1. 庫區是否有屬於下列特殊區域？<br>
                                <div>
                                    <input type="checkbox" name="checkArea" value="01" disabled> 活動斷層敏感區
                                    <input type="checkbox" name="checkArea" value="02" disabled> 土壤液化區
                                    <input type="checkbox" name="checkArea" value="03" disabled> 土石流潛勢區
                                    <input type="checkbox" name="checkArea" value="04" disabled> 淹水潛勢區
                                    <input type="checkbox" name="checkArea" value="05" disabled> 其他 <input type="text" id="checkAreaOther" class="inputex" disabled>
                                    <input type="checkbox" name="checkArea" value="06" disabled> 以上皆無
                                </div>
                            </div>
                            <div class="twocol">
                                <div class="right">
                                    <a id="editbtnContent" href="javascript:void(0);" title="編輯" class="genbtn">編輯</a>
                                </div>
                            </div>
                            <div class="font-size3 lineheight03">
                                2. 儲槽配置圖<br>
                                <%--<input type="file" name="fileName" style="display:none" multiple />
                                <div id="filelist"></div>--%>
                                <div id="content">
                                    
                                </div>
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

        <!--#include file="Footer.html"-->

</div><!-- BoxBgWa -->
<!-- 側邊選單內容:動態複製主選單內容 -->
<div id="sidebar-wrapper">
   
</div><!-- sidebar-wrapper -->

</form>
</div>
<!-- 結尾用div:修正mmenu form bug -->

<!-- colorbox -->
<div style="display:none;">
    <div id="workitem">
        <div class="margin35T padding5RL">
            <div class="OchiTrasTable width100 TitleLength08 font-size3">
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">工作項次</div>
                    <div class="OchiCell width100">
                        <input type="number" class="inputex width10">﹒<input type="number" class="inputex width10">﹒<input type="number" class="inputex width10">
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">預定日期</div>
                    <div class="OchiCell width100"><input type="text" class="inputex Jdatepicker width30"> </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">預定完成執行內容</div>
                    <div class="OchiCell width100"><textarea rows="5" cols="" class="inputex width100"></textarea></div>
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
    <script type="text/javascript">
        $(document).ready(function(){
        
        });
    </script>
    <script type="text/javascript" src="../js/GenCommon.js"></script><!-- UIcolor JS -->
    <script type="text/javascript" src="../js/PageCommon.js"></script><!-- 系統共用 JS -->
    <script type="text/javascript" src="../js/MenuOil.js"></script><!-- 系統共用 JS -->
    <script type="text/javascript" src="../js/SubMenuOilA.js"></script><!-- 內頁選單 -->
    <script type="text/javascript" src="../js/autoHeight.js"></script><!-- 高度不足頁面的絕對置底footer -->
</body>
</html>

