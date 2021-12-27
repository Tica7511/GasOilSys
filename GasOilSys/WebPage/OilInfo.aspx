<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OilInfo.aspx.cs" Inherits="WebPage_OilInfo" %>

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
            getData();

            if (($("#Competence").val() != '02') && ($("#Competence").val() != '03'))
                $("#editbtn").hide();
            else
                $("#editbtn").show();

            //編輯按鈕
            $(document).on("click", "#editbtn", function () {
                $("#editbtn").hide();
                $("#backbtn").show();
                $("#subbtn").show();
                
                $("#ctel").attr('disabled', false);
                $("#caddr").attr('disabled', false);
                $("#storagetank").attr('disabled', false);
                $("#pipeline").attr('disabled', false);
                $("#report").attr('disabled', false);
                $("#checkdate").attr('disabled', false);
            });

            //返回按鈕
            $(document).on("click", "#backbtn", function () {
                var str = confirm('尚未儲存的部分將不會更改，確定返回嗎?');

                if (str) {
                    $("#editbtn").show();
                    $("#backbtn").hide();
                    $("#subbtn").hide();
                    
                    $("#ctel").attr('disabled', true);
                    $("#caddr").attr('disabled', true);
                    $("#storagetank").attr('disabled', true);
                    $("#pipeline").attr('disabled', true);
                    $("#report").attr('disabled', true);
                    $("#checkdate").attr('disabled', true);

                    getData();
                }
            });

            //儲存按鈕
            $(document).on("click", "#subbtn", function () {
                $.ajax({
			    	type: "POST",
			    	async: false, //在沒有返回值之前,不會執行下一步動作
			    	url: "../Handler/AddOilInfo.aspx",
			    	data: {
                        cid: $.getQueryString("cp"),
                        ctel: $("#ctel").val(),
                        caddr: $("#caddr").val(),
                        storagetank: $("#storagetank").val(),
                        pipeline: $("#pipeline").val(),
                        report: $("#report").val(),
                        checkdate: $("#checkdate").val(),
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
                            location.href = "OilInfo.aspx?cp=" + $.getQueryString("cp");
			    		}
			    	}
			    });
            });
        }); // end js

        function getData() {
            $.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetOilInfo.aspx",
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
						if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function () {
                                $("#cname").val($(this).children("事業名稱").text().trim());
								$("#ctel").val($(this).children("電話").text().trim());
								$("#caddr").val($(this).children("地址").text().trim());
								$("#storagetank").val($(this).children("儲槽數量").text().trim());
								$("#pipeline").val($(this).children("管線數量").text().trim());
								$("#report").val($(this).children("維運計畫書及成果報告").text().trim());
								$("#checkdate").val($(this).children("曾執行過查核日期").text().trim());
							});
						}
					}
				}
			});
        }
    </script>
</head>
<body class="bgB">
<!-- 開頭用div:修正mmenu form bug -->
<div>
<form>
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
    <input type="hidden" id="Competence" value="<%= competence %>" />
        <div id="ContentWrapper">
            <div class="container margin15T">
                <div class="padding10ALL">
                    <div class="filetitlewrapper"><!--#include file="OilBreadTitle.html"--></div>

                    <div class="row margin20T">
                        <div class="col-lg-3 col-md-4 col-sm-5">
                            <div id="navmenuV"><!--#include file="OilLeftMenu.html"--></div>
                        </div>
                        <div class="col-lg-9 col-md-8 col-sm-7">
                            <div class="twocol">
                                <div id="fileall" class="right">
                                <a id="editbtn" href="javascript:void(0);" title="編輯" class="genbtn">編輯</a>
                                <a id="backbtn" href="javascript:void(0);" title="返回" class="genbtn" style="display:none">返回</a>
                                <a id="subbtn" href="javascript:void(0);" class="genbtn" style="display:none">儲存</a>
                                </div>
                            </div><br />
                            <div class="OchiTrasTable width100 TitleLength09 font-size3">
                                <div class="OchiRow">
                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">事業名稱</div>
                                    <div class="OchiCell width100"><input type="text" id="cname" class="inputex width99" disabled></div>
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">地址</div>
                                    <div class="OchiCell width100"><input type="text" id="caddr" class="inputex width99" disabled></div>
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">電話</div>
                                        <div class="OchiCell width100"><input type="text" id="ctel" class="inputex width100" disabled></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">儲槽數量</div>
                                        <div class="OchiCell width100"><input type="text" id="storagetank" class="inputex width100" disabled></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">管線數量</div>
                                        <div class="OchiCell width100"><input type="text" id="pipeline" class="inputex width100" disabled></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">維運計畫書及成果報告</div>
                                        <div class="OchiCell width100"><input type="text" id="report" class="inputex width100" disabled></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">曾執行過查核日期</div>
                                        <div class="OchiCell width100"><input type="text" id="checkdate" class="inputex width100" disabled></div>
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

