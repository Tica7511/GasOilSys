<%@ Page Title="" Language="C#" MasterPageFile="~/WebPage/MasterPageWeek.master" AutoEventWireup="true" CodeFile="WeekReport_5_7.aspx.cs" Inherits="WebPage_WeekReport_5_7" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        #datepick-div{
            position:absolute;
            z-index:9999;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            getData1(0);
            getData2(0);

            $(".pickDate").datepick({
                dateFormat: 'yymmdd',
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: '../images/calendar.gif',
                yearRange: 'c-60:c+10'
            }).BootStrap(); //BootStrap() 產生符合 BootStrap 的樣式內容

            //新增開窗1
            $(document).on("click", "#newbtn1", function () {
                $("#HGuid").val('');
                var datenow = new Date();
                var year = datenow.toLocaleDateString().slice(0, 4);
                var month = (datenow.getMonth()+1<10 ? '0' : '')+(datenow.getMonth()+1);
                var date = (datenow.getDate()<10 ? '0' : '')+datenow.getDate();
                var fulldate = (parseInt(year) - 1911).toString() + month + date;

                $("#txt_company").val('');                
                $("#txt_time1").val(fulldate);
                $("#txt_time2").val(fulldate);
                $("#txt_time3").val(fulldate);
                $("#txt_time4").val(fulldate);
                $("#txt_other1").val('');
            });

            //新增開窗2
            $(document).on("click", "#newbtn2", function () {
                $("#HGuid").val('');
                var datenow = new Date();
                var year = datenow.toLocaleDateString().slice(0,4)
                var month = (datenow.getMonth()+1<10 ? '0' : '')+(datenow.getMonth()+1);
                var date = (datenow.getDate()<10 ? '0' : '')+datenow.getDate();
                var fulldate = (parseInt(year) - 1911).toString() + month + date;

                $("#txt_content").val('');                
                $("#txt_begintime").val(fulldate);
                $("#txt_endtime").val(fulldate);
                $("#txt_other2").val('');
            });

            // 編輯開窗1
            $(document).on("click", "a[name='editbtn1']", function () {
                $("#HGuid").val($(this).attr("aid"));

                $("#txt_company").val($("span[name='co_" + $("#HGuid").val() + "']").html());                
                $("#txt_time1").val($("span[name='tmd1_" + $("#HGuid").val() + "']").html());
                $("#txt_time2").val($("span[name='tmd2_" + $("#HGuid").val() + "']").html());
                $("#txt_time3").val($("span[name='tmd3_" + $("#HGuid").val() + "']").html());
                var condition = $("span[name='tmd4_" + $("#HGuid").val() + "']").html();
                if (condition != '')
                    $("#txt_time4").val(condition);
                else
                    $("#txt_time4").val(condition);
                $("#txt_other1").val($("span[name='ot1_" + $("#HGuid").val() + "']").html());

                doOpenDialog();
            });

            // 編輯開窗2
            $(document).on("click", "a[name='editbtn2']", function () {
                $("#HGuid").val($(this).attr("aid"));

                $("#txt_content").val($("span[name='cn_" + $("#HGuid").val() + "']").html());                
                $("#txt_begintime").val($("span[name='tmdb_" + $("#HGuid").val() + "']").html());
                $("#txt_endtime").val($("span[name='tmde_" + $("#HGuid").val() + "']").html());
                $("#txt_other2").val($("span[name='ot2_" + $("#HGuid").val() + "']").html());

                doOpenDialog2();
            });

            // 儲存開窗1
            $(document).on("click", "#subbtn1", function () {
				var msg = '';
				if ($("#txt_company").val() == "")
					msg += "請輸入【公用天然氣事業】 ";
				if ($("#txt_time1").val() == "")
                    msg += "請輸入【能源署發文日期】 ";
                if ($("#txt_time2").val() == "")
                    msg += "請輸入【業者繳交日期】 ";
                if ($("#txt_time3").val() == "")
                    msg += "請輸入【工研院審閱日期】 ";

				if (msg != "") {
					alert("錯誤訊息: " + msg);
					return false;
				}

                var mode = ($("#HGuid").val() == "") ? "new" : "mod";

                $.ajax({
			    	type: "POST",
			    	async: false, //在沒有返回值之前,不會執行下一步動作
			    	url: "../Handler/AddWeekReport_5_7.aspx",
                    data: {
                        type: "list1",
			    		mode: mode,
			    		rid: $("#HGuid").val(),
                        rpid: $.getQueryString("rpid"),
                        no: $("#reportno").html(),
			    		company: $("#txt_company").val(),
			    		time1: $("#txt_time1").val(),
			    		time2: $("#txt_time2").val(),
			    		time3: $("#txt_time3").val(),
			    		time4: $("#txt_time4").val(),
			    		other: $("#txt_other1").val(),
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
                            parent.$.colorbox.close();
                            location.href = "WeekReport_5_7.aspx?rid=" + $("#RGuid").val() + "&rpid=" + $("#RPGuid").val() + "&year=" + $.getQueryString("year");
			    		}
			    	}
			    });
            });

            // 儲存開窗2
            $(document).on("click", "#subbtn2", function () {
				var msg = '';
				if ($("#txt_content").val() == "")
					msg += "請輸入【執行內容】 ";
				if ($("#txt_begintime").val() == "")
                    msg += "請輸入【預定日期起】 ";
                if ($("#txt_endtime").val() == "")
                    msg += "請輸入【預定日期迄】 ";
                if (parseInt($("#txt_begintime").val()) > parseInt($("#txt_endtime").val()))
                    msg += "【預定日期迄】不可小於【預定日期起】 ";

				if (msg != "") {
					alert("錯誤訊息: " + msg);
					return false;
				}

                var mode = ($("#HGuid").val() == "") ? "new" : "mod";

                $.ajax({
			    	type: "POST",
			    	async: false, //在沒有返回值之前,不會執行下一步動作
			    	url: "../Handler/AddWeekReport_5_7.aspx",
                    data: {
                        type: "list2",
			    		mode: mode,
			    		rid: $("#HGuid").val(),
                        rpid: $.getQueryString("rpid"),
                        no: $("#reportno").html(),
			    		content: $("#txt_content").val(),
			    		begintime: $("#txt_begintime").val(),
			    		endtime: $("#txt_endtime").val(),
			    		other: $("#txt_other2").val(),
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
                            parent.$.colorbox.close();
                            location.href = "WeekReport_5_7.aspx?rid=" + $("#RGuid").val() + "&rpid=" + $("#RPGuid").val() + "&year=" + $.getQueryString("year");
			    		}
			    	}
			    });
            });

            //刪除工作項次1
            $(document).on("click", "a[name='delbtn1']", function () {
                var str = confirm('確定刪除嗎?');
                if (str) {
                    $.ajax({
			        	type: "POST",
			        	async: false, //在沒有返回值之前,不會執行下一步動作
			        	url: "../Handler/DelWeekReport_5_7.aspx",
			        	data: {
			        		type: "list1",
			        		rid: $(this).attr("aid"),
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
                                location.href = "WeekReport_5_7.aspx?rid=" + $("#RGuid").val() + "&rpid=" + $("#RPGuid").val() + "&year=" + $.getQueryString("year");
			        		}
			        	}
			        });
                }
            });

            //刪除工作項次2
            $(document).on("click", "a[name='delbtn2']", function () {
                var str = confirm('確定刪除嗎?');
                if (str) {
                    $.ajax({
			        	type: "POST",
			        	async: false, //在沒有返回值之前,不會執行下一步動作
			        	url: "../Handler/DelWeekReport_5_7.aspx",
                        data: {
                            type: "list2",
			        		rid: $(this).attr("aid"),
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
                                location.href = "WeekReport_5_7.aspx?rid=" + $("#RGuid").val() + "&rpid=" + $("#RPGuid").val() + "&year=" + $.getQueryString("year");
			        		}
			        	}
			        });
                }
            });

        }); // end js

        //轉換日期 ex: 20210505 => 05.05(二)
        function getDate(fulldate) {
            var year = (parseInt(fulldate.substring("0", "3")) + 1911).toString();
            var month = fulldate.substring("3", "5");
            var date = fulldate.substring("5", "7");
            var dob = year + "/" + month + "/" + date;
            var then = new Date(dob);
            var theday = then.getDay() + 1;
            var weekday = new Array(6);
            weekday[1]="日";
            weekday[2]="一";
            weekday[3]="二";
            weekday[4]="三";
            weekday[5]="四";
            weekday[6]="五";
            weekday[7] = "六";
            return month + "." + date + "(" + weekday[theday] + ")";
        }

        function getData1(p) {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetWeekReport_5_7.aspx",
                data: {
                    type: "list1",
                    rid: $.getQueryString("rid"),
                    year: $.getQueryString("year"),
                    PageNo: p,
					PageSize: Page.Option.PageSize,
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
                                $("#RGuid").val($(this).children("guid").text().trim());
                                $("#RPGuid").val($(this).children("父層guid").text().trim());
                                $("#reportno").html($(this).children("工作項次").text().trim());
                                $("#reportname").html($(this).children("名稱").text().trim());
							});
						}

						$("#tablist1 tbody").empty();
						var tabstr = '';
						if ($(data).find("data_item2").length > 0) {
                            $(data).find("data_item2").each(function (i) {                                
								tabstr += '<tr>';
                                tabstr += '<td align="center">';
                                tabstr += '<span name="sn_' + $(this).children("guid").text().trim() + '">' + $(this).children("場次").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="co_' + $(this).children("guid").text().trim() + '">' + $(this).children("公用天然氣事業").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td>';                                
                                tabstr += '<span name="tm1_' + $(this).children("guid").text().trim() + '">' + getDate($(this).children("能源局發文日期").text().trim()) + '</span>'
                                tabstr += '<span style="display:none" name="tmd1_' + $(this).children("guid").text().trim() + '">' + $(this).children("能源局發文日期").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td>';                                
                                tabstr += '<span name="tm2_' + $(this).children("guid").text().trim() + '">' + getDate($(this).children("業者繳交日期").text().trim()) + '</span>'
                                tabstr += '<span style="display:none" name="tmd2_' + $(this).children("guid").text().trim() + '">' + $(this).children("業者繳交日期").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td>';                                
                                tabstr += '<span name="tm3_' + $(this).children("guid").text().trim() + '">' + getDate($(this).children("工研院審閱日期").text().trim()) + '</span>'
                                tabstr += '<span style="display:none" name="tmd3_' + $(this).children("guid").text().trim() + '">' + $(this).children("工研院審閱日期").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td>';
                                var condition = $(this).children("補正情形").text().trim();
                                if (condition != '')
                                    condition = getDate($(this).children("補正情形").text().trim());
                                    
                                tabstr += '<span name="tm4_' + $(this).children("guid").text().trim() + '">' + condition + '</span>'
                                tabstr += '<span style="display:none" name="tmd4_' + $(this).children("guid").text().trim() + '">' + $(this).children("補正情形").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="ot1_' + $(this).children("guid").text().trim() + '">' + $(this).children("備註").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td align="center" class="font-normal">';
                                tabstr += '<a href="javascript:void(0);" name="delbtn1" aid="' + $(this).children("guid").text().trim() + '">刪除</a> ';
                                tabstr += '<a href="#workitem1" name="editbtn1" class="colorboxM" title="編輯災害防救業務計畫修正與審閱進度" aid="' + $(this).children("guid").text().trim() + '">編輯</a>';
                                tabstr += '</td>';
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="8">查詢無資料</td></tr>';
                        $("#tablist1 tbody").append(tabstr);
                        Page.Option.Selector = "#pageblock1";
                        Page.Option.FunctionName = "getData1";
						Page.CreatePage(p, $("total", data).text());
					}
				}
			});
        }

        function getData2(p) {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetWeekReport_5_7.aspx",
                data: {
                    type: "list2",
                    rid: $.getQueryString("rid"),
                    year: $.getQueryString("year"),
                    PageNo: p,
					PageSize: Page.Option.PageSize,
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
                                $("#RGuid").val($(this).children("guid").text().trim());
                                $("#RPGuid").val($(this).children("父層guid").text().trim());
                                $("#reportno").html($(this).children("工作項次").text().trim());
                                $("#reportname").html($(this).children("名稱").text().trim());
							});
						}

						$("#tablist2 tbody").empty();
						var tabstr = '';
						if ($(data).find("data_item2").length > 0) {
                            $(data).find("data_item2").each(function (i) {                                
								tabstr += '<tr>';
                                tabstr += '<td align="center">';
                                tabstr += '<span name="sn_' + $(this).children("guid").text().trim() + '">' + $(this).children("場次").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="cn_' + $(this).children("guid").text().trim() + '">' + $(this).children("執行內容").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td>';                                
                                tabstr += '<span name="tmb_' + $(this).children("guid").text().trim() + '">' + getDate($(this).children("預定日期起").text().trim()) + '</span>'
                                tabstr += '<span style="display:none" name="tmdb_' + $(this).children("guid").text().trim() + '">' + $(this).children("預定日期起").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td>';                                
                                tabstr += '<span name="tme_' + $(this).children("guid").text().trim() + '">' + getDate($(this).children("預定日期迄").text().trim()) + '</span>'
                                tabstr += '<span style="display:none" name="tmde_' + $(this).children("guid").text().trim() + '">' + $(this).children("預定日期迄").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="ot2_' + $(this).children("guid").text().trim() + '">' + $(this).children("備註").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td align="center" class="font-normal">';
                                tabstr += '<a href="javascript:void(0);" name="delbtn2" aid="' + $(this).children("guid").text().trim() + '">刪除</a> ';
                                tabstr += '<a href="#workitem2" name="editbtn2" class="colorboxM" title="編輯災防計畫審閱及其他災防事項" aid="' + $(this).children("guid").text().trim() + '">編輯</a>';
                                tabstr += '</td>';
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="6">查詢無資料</td></tr>';
                        $("#tablist2 tbody").append(tabstr);
                        Page.Option.Selector = "#pageblock2";
                        Page.Option.FunctionName = "getData2";
						Page.CreatePage(p, $("total", data).text());
					}
				}
			});
        }

        function doOpenDialog() {
            var WinHeight = $("html").height();
            var ColHeight = WinHeight * 0.6;
            $.colorbox({ title: "編輯災害防救業務計畫修正與審閱進度", inline: true, href: "#workitem1", width: "100%", maxWidth: "800", maxHeight: ColHeight, opacity: 0.5 });
        }

        function doOpenDialog2() {
            var WinHeight = $("html").height();
            var ColHeight = WinHeight * 0.6;
            $.colorbox({ title: "編輯災防計畫審閱及其他災防事項", inline: true, href: "#workitem2", width: "100%", maxWidth: "800", maxHeight: ColHeight, opacity: 0.5 });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="ContentWrapper">
        <input type="hidden" id="HGuid" />
        <input type="hidden" id="RGuid" />
        <input type="hidden" id="RPGuid" />
        <div class="container margin15T">
            <div class="padding10ALL">
                <div class="filetitlewrapper">
                    <span class="filetitle font-size7">計畫書填報</span>
                </div>

                <div class="BoxBorderDa padding5ALL font-size4 margin10T">
                    <table>
                        <tr>
                            <td width="60" align="center">
                                <span id="reportno"></span>
                            </td>
                            <td>
                                <span id="reportname"></span>
                            </td>
                        </tr>
                    </table>
                </div>


                <div class="twocol margin10T">
                    <div class="left font-size5 ">
                        <i class="fa fa-chevron-circle-right IconCa" aria-hidden="true"></i>
                        公用天然氣事業災害防救業務計畫修正與審閱進度
                    </div>
                    <div class="right font-normal font-size3">
                        <!--<a href="#datesetting" class="colorboxS" title="重設日期">重設日期</a>-->
                        <a id="newbtn1" href="#workitem1" class="genbtn colorboxM" title="新增災害防救業務計畫修正與審閱進度">新增</a>
                    </div>
                </div>

                <div class="stripeMeCS font-size3 margin10T">
                    <table id="tablist1" width="100%" border="0" cellspacing="0" cellpadding="0">
                        <thead>
                            <tr>
                                <th nowrap="nowrap" width="100">項次</th>
                                <th nowrap="nowrap">公用天然氣事業</th>
                                <th nowrap="nowrap" width="130">能源署發文日期</th>
                                <th nowrap="nowrap" width="120">業者繳交日期</th>
                                <th nowrap="nowrap" width="130">工研院審閱日期</th>
                                <th nowrap="nowrap" width="100">補正情形</th>
                                <th nowrap="nowrap">備註</th>
                                <th nowrap="nowrap" width="100">功能</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                    </table>
                    <div class="margin10B margin10T textcenter">
						<div id="pageblock1"></div>
					</div>
                </div>
                <!-- stripeMe -->

                <div class="twocol margin10T">
                    <div class="left font-size5 ">
                        <i class="fa fa-chevron-circle-right IconCa" aria-hidden="true"></i>
                        災防計畫審閱及其他災防事項
                    </div>
                    <div class="right font-normal font-size3">
                        <!--<a href="#datesetting" class="colorboxS" title="重設日期">重設日期</a>-->
                        <a id="newbtn2" href="#workitem2" class="genbtn colorboxM" title="新增災防計畫審閱及其他災防事項">新增</a>
                    </div>
                </div>

                <div class="stripeMeCS font-size3 margin10T">
                    <table id="tablist2" width="100%" border="0" cellspacing="0" cellpadding="0">
                        <thead>
                            <tr>
                                <th nowrap="nowrap" width="100">項次</th>
                                <th nowrap="nowrap">執行內容</th>
                                <th nowrap="nowrap" width="100">交辦日期</th>
                                <th nowrap="nowrap" width="100">完成日期</th>
                                <th nowrap="nowrap">備註</th>
                                <th nowrap="nowrap" width="100">功能</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                    </table>
                    <div class="margin10B margin10T textcenter">
						<div id="pageblock2"></div>
					</div>
                </div>
                <!-- stripeMe -->


            </div>
        </div>
        <!-- container -->
    </div>
    <!-- ContentWrapper -->

    <!-- colorbox -->
<div style="display:none;">
    <div id="workitem1">
        <div class="margin35T padding5RL">
            <div class="OchiTrasTable width100 TitleLength08 font-size3">
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">公用天然事業</div>
                    <div class="OchiCell width100">
                        <textarea id="txt_company" rows="5" cols="" class="inputex width100"></textarea>
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">能源署發文日期</div>
                    <div class="OchiCell width100">
                        <input id="txt_time1" type="text" class="inputex pickDate width30" disabled>
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">業者繳交日期</div>
                    <div class="OchiCell width100">
                        <input id="txt_time2" type="text" class="inputex pickDate width30" disabled>
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">工研院審閱日期</div>
                    <div class="OchiCell width100">
                        <input id="txt_time3" type="text" class="inputex pickDate width30" disabled>
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">補正情形</div>
                    <div class="OchiCell width100">
                        <input id="txt_time4" type="text" class="inputex pickDate width30" disabled>
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">備註</div>
                    <div class="OchiCell width100">
                        <input id="txt_other1" type="text" class="inputex width100">
                    </div>
                </div><!-- OchiRow -->
            </div><!-- OchiTrasTable -->
        </div>

        <div class="twocol margin10T">
            <div class="right">
                <a href="javascript:void(0);" id="canbtn1" class="genbtn closecolorbox">取消</a>
                <a href="javascript:void(0);" id="subbtn1" class="genbtn">儲存</a>
            </div>
        </div>
        <br /><br />
    </div>
</div>

<div style="display:none;">
    <div id="workitem2">
        <div class="margin35T padding5RL">
            <div class="OchiTrasTable width100 TitleLength08 font-size3">
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">執行內容</div>
                    <div class="OchiCell width100">
                        <textarea id="txt_content" rows="5" cols="" class="inputex width100"></textarea>
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">交辦日期</div>
                    <div class="OchiCell width100">
                        <input id="txt_begintime" type="text" class="inputex pickDate width30" disabled>
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">完成日期</div>
                    <div class="OchiCell width100">
                        <input id="txt_endtime" type="text" class="inputex pickDate width30" disabled>
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">備註</div>
                    <div class="OchiCell width100">
                        <input id="txt_other2" type="text" class="inputex width100">
                    </div>
                </div><!-- OchiRow -->
            </div><!-- OchiTrasTable -->
        </div>

        <div class="twocol margin10T">
            <div class="right">
                <a href="javascript:void(0);" id="canbtn2" class="genbtn closecolorbox">取消</a>
                <a href="javascript:void(0);" id="subbtn2" class="genbtn">儲存</a>
            </div>
        </div>
        <br /><br />
    </div>
</div>
</asp:Content>