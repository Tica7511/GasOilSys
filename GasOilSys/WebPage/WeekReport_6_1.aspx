﻿<%@ Page Title="" Language="C#" MasterPageFile="~/WebPage/MasterPageWeek.master" AutoEventWireup="true" CodeFile="WeekReport_6_1.aspx.cs" Inherits="WebPage_WeekReport_6_1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        #datepick-div{
            position:absolute;
            z-index:9999;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            getData();

            $(".pickDate").datepick({
                dateFormat: 'yymmdd',
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: '../images/calendar.gif',
                yearRange: 'c-6:c+6'
            }).BootStrap(); //BootStrap() 產生符合 BootStrap 的樣式內容

            //新增開窗
            $(document).on("click", "a[name='newbtn']", function () {
                $("#HGuid").val('');
                var datenow = new Date();
                var year = datenow.toLocaleDateString().slice(0, 4)
                var month = (datenow.getMonth() + 1 < 10 ? '0' : '') + (datenow.getMonth() + 1);
                var date = (datenow.getDate() < 10 ? '0' : '') + datenow.getDate();
                var fulldate = (parseInt(year) - 1911).toString() + month + date;

                $("#txt_no1").val('1');
                $("#txt_no2").val('1');
                $("#txt_no3").val('0');
                $("#txt_no4").val('0');
                $("#txt_content").val('');
                $("#txt_begintime").val(fulldate);
                $("#txt_endtime").val('');
            });

            // 編輯開窗
            $(document).on("click", "a[name='editbtn']", function () {
                $("#HGuid").val($(this).attr("aid"));

                var nostr = new Array();
                nostr = ($("span[name='no_" + $("#HGuid").val() + "']").html()).split('.');

                for (var j = 1; j <= nostr.length; j++)
                    $("#txt_no" + j).val(nostr[j - 1]);
                for (var i = nostr.length; i < 4; i++)
                    $("#txt_no" + (i + 1)).val('0');
                $("#txt_content").val($("span[name='cn_" + $("#HGuid").val() + "']").html());
                $("#txt_begintime").val($("span[name='tmd1_" + $("#HGuid").val() + "']").html());
                $("#txt_endtime").val($("span[name='tmd2_" + $("#HGuid").val() + "']").html());
            });

            // 儲存開窗
            $(document).on("click", "#subbtn", function () {
				var msg = '';
				if ($("#txt_no1").val() == "")
                    msg += "請完整的輸入【編號】 ";
                if ($("#txt_no2").val() == "")
                    msg += "請完整的輸入【編號】 ";
				if ($("#txt_content").val() == "")
                    msg += "請輸入【查核點內容說明】 ";
                if ($("#txt_begintime").val() == "")
                    msg += "請輸入【預定完成日期】 ";

				if (msg != "") {
					alert("錯誤訊息: " + msg);
					return false;
				}

                var mode = ($("#HGuid").val() == "") ? "new" : "mod";

                $.ajax({
			    	type: "POST",
			    	async: false, //在沒有返回值之前,不會執行下一步動作
			    	url: "../Handler/AddWeekReport_6_1.aspx",
			    	data: {
			    		mode: mode,
			    		rid: $("#HGuid").val(),
			    		rpid: $.getQueryString("rpid"),
			    		no: $("#reportno").html(),
			    		nb1: $("#txt_no1").val(),
			    		nb2: $("#txt_no2").val(),
			    		nb3: $("#txt_no3").val(),
			    		nb4: $("#txt_no4").val(),
			    		content: $("#txt_content").val(),
			    		begintime: $("#txt_begintime").val(),
			    		endtime: $("#txt_endtime").val(),
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
                            location.href = "WeekReport_6_1.aspx?rid=" + $("#RGuid").val() + "&rpid=" + $("#RPGuid").val() + "&year=" + $.getQueryString("year");
			    		}
			    	}
			    });
            });

            //刪除工作項次
            $(document).on("click", "a[name='delbtn']", function () {
                var str = confirm('確定刪除嗎?');
                if (str) {
                    $.ajax({
			        	type: "POST",
			        	async: false, //在沒有返回值之前,不會執行下一步動作
			        	url: "../Handler/DelWeekReport_6_1.aspx",
			        	data: {
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
                                location.href = "WeekReport_6_1.aspx?rid=" + $("#RGuid").val() + "&rpid=" + $("#RPGuid").val() + "&year=" + $.getQueryString("year");
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

        function getData() {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetWeekReport_6_1.aspx",
				data: {
                    rid: $.getQueryString("rid"),
                    year: $.getQueryString("year"),
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

						$("#tablist tbody").empty();
                        var tabstr = '';
						if ($(data).find("data_item2").length > 0) {
                            $(data).find("data_item2").each(function (i) { 
                                var nostr = '';
								tabstr += '<tr>';
                                tabstr += '<td align="center">';
                                tabstr += '<span name="sn_' + $(this).children("guid").text().trim() + '">' + $(this).children("場次").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td align="center">';
                                for (var j=1; j < 5; j++) {
                                    if ($(this).children("編號" + j).text().trim() != '' && $(this).children("編號" + j).text().trim()!= '0')
                                        nostr += $(this).children("編號" + j).text().trim() + ".";
                                }
                                nostr = nostr.substring(0, nostr.length - 1);
                                tabstr += '<span name="no_' + $(this).children("guid").text().trim() + '">' + nostr.trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="cn_' + $(this).children("guid").text().trim() + '">' + $(this).children("執行內容").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td align="center">';                                
                                tabstr += '<span name="tm1_' + $(this).children("guid").text().trim() + '">' + getDate($(this).children("預定日期起").text().trim()) + '</span>';
                                tabstr += '<span style="display:none" name="tmd1_' + $(this).children("guid").text().trim() + '">' + $(this).children("預定日期起").text().trim() + '</span>';
                                tabstr += '</td>';
                                tabstr += '<td align="center">';
                                var endtime = $(this).children("預定日期迄").text().trim();
                                if (endtime != '')
                                    endtime = getDate($(this).children("預定日期迄").text().trim());
                                tabstr += '<span name="tm2_' + $(this).children("guid").text().trim() + '">' + endtime + '</span>';
                                tabstr += '<span style="display:none" name="tmd2_' + $(this).children("guid").text().trim() + '">' + $(this).children("預定日期迄").text().trim() + '</span>';
                                tabstr += '</td>';
                                tabstr += '<td align="center" class="font-normal">';
                                tabstr += '<a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("guid").text().trim() + '">刪除</a> ';
                                tabstr += '<a href="#workitem" name="editbtn" class="colorboxM" title="編輯工作項次" aid="' + $(this).children("guid").text().trim() + '">編輯</a>';
                                tabstr += '</td>';
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="6">查詢無資料</td></tr>';
						$("#tablist tbody").append(tabstr);
					}
				}
			});
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
                    <div class="left"></div>
                    <div class="right font-normal font-size3">
                        <a name="newbtn" href="#workitem" class="genbtn colorboxM" title="新增工作項次">新增</a>
                    </div>
                </div>

                <div class="stripeMeCS font-size3 margin10T">
                    <table id="tablist" width="100%" border="0" cellspacing="0" cellpadding="0">
                        <thead>
                            <tr>
                                <th nowrap="nowrap" width="100">項次</th>
                                <th nowrap="nowrap" width="120">編號</th>
                                <th nowrap="nowrap">查核點內容說明</th>
                                <th nowrap="nowrap">預定完成日期</th>
                                <th nowrap="nowrap">完成日期</th>
                                <th nowrap="nowrap" width="100">功能</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                    </table>
                </div>
                <!-- stripeMe -->


            </div>
        </div>
        <!-- container -->
    </div>
    <!-- ContentWrapper -->

    <!-- colorbox -->
<div style="display:none;">
    <div id="workitem">
        <div class="margin35T padding5RL">
            <div class="OchiTrasTable width100 TitleLength08 font-size3">
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">編號</div>
                    <div class="OchiCell width100">
                        <input id="txt_no1" type="number" min="1" max="9" class="inputex width10">﹒<input id="txt_no2" type="number" min="1" max="9" class="inputex width10">﹒
                        <input id="txt_no3" type="number" min="0" max="9" value="0" class="inputex width10">﹒<input id="txt_no4" type="number" min="0" max="9" value="0" class="inputex width10">
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">查核點內容說明</div>
                    <div class="OchiCell width100">
                        <textarea id="txt_content" rows="5" cols="" class="inputex width100"></textarea>
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">預定完成日期</div>
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
            </div><!-- OchiTrasTable -->
        </div>

        <div class="twocol margin10T">
            <div class="right">
                <a href="javascript:void(0);" id="canbtn" class="genbtn closecolorbox">取消</a>
                <a href="javascript:void(0);" id="subbtn" class="genbtn">儲存</a>
            </div>
        </div>
        <br /><br />
    </div>
</div>
</asp:Content>


