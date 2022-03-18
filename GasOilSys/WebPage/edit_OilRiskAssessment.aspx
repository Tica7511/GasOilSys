<%@ Page Language="C#" AutoEventWireup="true" CodeFile="edit_OilRiskAssessment.aspx.cs" Inherits="WebPage_edit_OilRiskAssessment" %>

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
            getDDL();
            getData();

            //取消按鍵
            $(document).on("click", "#cancelbtn", function () {
                var str = confirm('尚未儲存的部分將不會更改，確定返回嗎?');

                if (str)
                    location.href = "OilRiskAssessment.aspx?cp=" + $.getQueryString("cp");
            });

            //儲存按鍵
            $(document).on("click", "#subbtn", function () {
                var msg = '';

                if ($("#txt1").val() == '')
                    msg += "請選擇【長途管線識別碼】\n";
                //if (($("#txt2_1").val()) == '' || ($("#txt2_2").val() == ''))
                //    msg += "請輸入【最近一次執行日期】\n";
                //if ($("#txt3").val() == '')
                //    msg += "請選擇【再評估時機】\n";
                //if ($("#txt4").val() == '')
                //    msg += "請選擇【管線長度】\n";
                //if ($("#txt5").val() == '')
                //    msg += "請選擇【分段數量】\n";
                //if ($("#txt6").val() == '')
                //    msg += "請選擇【已納入ILI結果】\n";
                //if ($("#txt7").val() == '')
                //    msg += "請選擇【已納入CIPS結果】\n";
                //if ($("#txt8").val() == '')
                //    msg += "請選擇【已納入巡管結果】\n";
                //if ($("#txt9").val() == '')
                //    msg += "請選擇【各等級風險管段數量_高】\n";
                //if ($("#txt10").val() == '')
                //    msg += "請選擇【各等級風險管段數量_中】\n";
                //if ($("#txt11").val() == '')
                //    msg += "請選擇【各等級風險管段數量_低】\n";
                //if ($("#txt13").val() == '')
                //    msg += "請選擇【改善後風險等級】\n";

                if (msg != "") {
                    alert("Error message: \n" + msg);
                    return false;
                }

                // Get form
                var form = $('#form1')[0];

                // Create an FormData object 
                var data = new FormData(form);

                var mode = ($.getQueryString("guid") == "") ? "new" : "edit";

                // If you want to add an extra field for the FormData
                data.append("cp", $.getQueryString("cp"));
                data.append("guid", $.getQueryString("guid"));
                data.append("mode", encodeURIComponent(mode));
                data.append("year", encodeURIComponent(getTaiwanDate()));
                data.append("txt1", encodeURIComponent($("#txt1").val()));
                data.append("txt2_1", encodeURIComponent($("#txt2_1").val()));
                data.append("txt2_2", encodeURIComponent($("#txt2_2").val()));
                data.append("txt3", encodeURIComponent($("#txt3").val()));
                data.append("txt4", encodeURIComponent($("#txt4").val()));
                data.append("txt5", encodeURIComponent($("#txt5").val()));
                data.append("txt6", encodeURIComponent($("#txt6").val()));
                data.append("txt7", encodeURIComponent($("#txt7").val()));
                data.append("txt8", encodeURIComponent($("#txt8").val()));
                data.append("txt9", encodeURIComponent($("#txt9").val()));
                data.append("txt10", encodeURIComponent($("#txt10").val()));
                data.append("txt11", encodeURIComponent($("#txt11").val()));
                data.append("txt12", encodeURIComponent($("#txt12").val()));
                data.append("txt13", encodeURIComponent($("#txt13").val()));
                data.append("txt14", encodeURIComponent($("#txt14").val()));

                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "../handler/AddOilRiskAssessment.aspx",
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

                            location.href = "OilRiskAssessment.aspx?cp=" + $.getQueryString("cp");
                        }
                    }
                });
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
                url: "../Handler/GetOilRiskAssessment.aspx",
				data: {
                    guid: $.getQueryString("guid"),
                    type: "data"
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
                                $("#txt1").val($(this).children("長途管線識別碼").text().trim());
                                $("#txt2_1").val(splitYearMonth(0, $(this).children("最近一次執行日期").text().trim()));
                                $("#txt2_2").val(splitYearMonth(1, $(this).children("最近一次執行日期").text().trim()));
                                $("#txt3").val($(this).children("再評估時機").text().trim());
                                $("#txt4").val($(this).children("管線長度").text().trim());
                                $("#txt5").val($(this).children("分段數量").text().trim());
                                $("#txt6").val($(this).children("已納入ILI結果").text().trim());
                                $("#txt7").val($(this).children("已納入CIPS結果").text().trim());
                                $("#txt8").val($(this).children("已納入巡管結果").text().trim());
                                $("#txt9").val($(this).children("各等級風險管段數量_高").text().trim());
                                $("#txt10").val($(this).children("各等級風險管段數量_中").text().trim());
                                $("#txt11").val($(this).children("各等級風險管段數量_低").text().trim());
                                $("#txt12").val($(this).children("文件名稱").text().trim());
                                $("#txt13").val($(this).children("改善後風險等級").text().trim());
                                $("#txt14").val($(this).children("備註").text().trim());
							});
						}
					}
				}
			});
        }

        function getDDL() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/GetOilTubeInfo.aspx",
                data: {
                    cpid: $.getQueryString("cp"),
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
                        var ddlstr = '<option value="">請選擇</option>';
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                ddlstr += '<option value="' + $(this).children("長途管線識別碼").text().trim() + '">' + $(this).children("長途管線識別碼").text().trim() + '</option>';
                            });
                        }

                        $("#txt1").empty();
                        $("#txt1").append(ddlstr);
                    }
                }
            });
        }

        function splitYearMonth(arrylenth, fulldate) {

            if (fulldate != '') {
                var farray = new Array();
                farray = fulldate.split("/");
                var twdate = farray[arrylenth];

                return twdate;
            }
            else {
                return '';
            }

        }

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

                return twdate;
            }
            else {
                return '';
            }

        }

        function getTaiwanDate() {
            var nowDate = new Date();

            var nowYear = nowDate.getFullYear();
            var nowTwYear = (nowYear - 1911);

            return nowTwYear;
        }

        //function getTaiwanDate() {
        //    var nowDate = new Date();

        //    var nowYear = nowDate.getFullYear();
        //    var nowTwYear = (nowYear - 1911);

        //    var ddlstr = '<option value="">請選擇</option>';

        //    for (var i = 10; i >= 0; i--) {
        //        ddlstr += '<option value="' + (nowTwYear - i).toString() + '">' + (nowTwYear - i).toString() + '</option>';
        //    }

        //    for (var j = 1; j <= 10; j++) {
        //        ddlstr += '<option value="' + (nowTwYear + j).toString() + '">' + (nowTwYear + j).toString() + '</option>';
        //    }

        //    $("#sellist").empty();
        //    $("#sellist").append(ddlstr);
        //}
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
                                <div class="left font-size4" style="width:50%">
                                    <i class="fa fa-chevron-circle-right IconCa" aria-hidden="true"></i> 
                                    長途管線識別碼 : <select id="txt1" class="width40 inputex" ></select>
                                </div>
                                <div class="right">
                                    <a id="cancelbtn" href="javascript:void(0);" title="返回" class="genbtn" >取消</a>
                                    <a id="subbtn" href="javascript:void(0);" title="儲存" class="genbtn" >儲存</a>
                                </div>
                            </div><br />
                            <div class="OchiTrasTable width100 TitleLength09 font-size3">
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">最近一次執行日期</div>
                                        <div class="OchiCell width100">
                                            民國 <input type="number" min="1" id="txt2_1" placeholder="請填寫民國年" class="inputex width40"> 年 
                                            <select id="txt2_2" class="width25 inputex" >
                                                <option value="">請選擇</option>
                                                <option value="01">1</option>
                                                <option value="02">2</option>
                                                <option value="03">3</option>
                                                <option value="04">4</option>
                                                <option value="05">5</option>
                                                <option value="06">6</option>
                                                <option value="07">7</option>
                                                <option value="08">8</option>
                                                <option value="09">9</option>
                                                <option value="10">10</option>
                                                <option value="11">11</option>
                                                <option value="12">12</option>
                                            </select> 月
                                        </div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">再評估時機</div>
                                        <div class="OchiCell width100">
                                            <select id="txt3" class="inputex width100">
                                                <option value="">請選擇</option>
                                                <option value="定期(5年)">定期(5年)</option>
                                                <option value="風險因子異動">風險因子異動</option>
                                            </select>
                                        </div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">管線長度</div>
                                        <div class="OchiCell width100"><input type="text" id="txt4" class="inputex width50"> KM</div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">分段數量</div>
                                        <div class="OchiCell width100"><input type="number" min="0" id="txt5" class="inputex width50"></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">已納入ILI結果</div>
                                        <div class="OchiCell width100"><input type="text" id="txt6" maxlength="2" class="inputex width100"></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">已納入CIPS結果</div>
                                        <div class="OchiCell width100"><input type="text" id="txt7" maxlength="2" class="inputex width100"></div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">已納入巡管結果</div>
                                        <div class="OchiCell width100">
                                            <select id="txt8" class="inputex width100">
                                                <option value="">請選擇</option>
                                                <option value="是">是</option>
                                                <option value="否">否</option>
                                            </select>
                                        </div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                </br>
                                <div class="OchiRow">
                                    <div class="margin5TB font-size4 font-bold" style="text-align:center">各等級風險管段數量</div>
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">高</div>
                                    <div class="OchiCell width100"><input type="number" min="0" id="txt9" class="inputex width30"> 
                                        中:<input type="number" min="0" id="txt10" class="inputex width30"> 
                                        低:<input type="number" min="0" id="txt11" class="inputex width30"></div>
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">降低中高風險管段之相關作為文件名稱</div>
                                        <div class="OchiCell width100"><input type="text" id="txt12" class="inputex width100"></div>
                                    </div><!-- OchiHalf -->
                                    <div class="OchiHalf">
                                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">改善後風險等級</div>
                                        <div class="OchiCell width100">
                                            <select id="txt13" class="inputex width100">
                                                <option value="">請選擇</option>
                                                <option value="低">低</option>
                                                <option value="中">中</option>
                                                <option value="中">高</option>
                                                <option value="NA">NA</option>
                                            </select>
                                        </div>
                                    </div><!-- OchiHalf -->
                                </div><!-- OchiRow -->
                                <div class="OchiRow">
                                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">備註</div>
                                    <div class="OchiCell width100"><input type="text" id="txt14" class="inputex width100"></div>
                                </div><!-- OchiRow -->
                            </div><!-- OchiTrasTable -->
                            <br />
                            <div class="margin5TB font-size2">
                                (1) 風險評估相關教育訓練：包含公司內部自行辦理或至其他機構辦理之風險評估教育訓練。<br>
                                (2) 訓練課程屬於內部訓練者，請填1；外部訓練者，請填2。<br>
                                (3) 再評估時機：最近一次所執行之評估是公司定期規劃(例：每5年一次)，或因風險評估之因子有所異動 (例：遷管、換管)而執行。<br>
                                (4) 執行該管線風險評估時，已將ILI檢測結果納入評估參數，請填寫檢測時間，若尚未考量ILI檢測結果，或該管線尚未執行ILI檢測者，請填NA。<br>
                                (5) 執行該管線風險評估時，已將CIPS檢測結果納入評估參數，請填檢測時間，若尚未考量CIPS檢測結果者，請填NA。<br>
                                (6) 執行該管線風險評估時，已將巡管結果(如：未會勘而開挖頻度)納入評估參數，請填「是」，若尚未考量巡管結果者，請填「否」。<br>
                                (7) 各等級風險管段數量：請分別填入高、中、低風險之管段數量。<br>
                                (8) 若評估結果有中高風險管段，應於「降低中高風險管段之作為」欄位註明相對應之作為或其作為相關文件名稱，並於「改善後風險等級」欄位中，填入改善後之風險等級(高、中、低)。


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





