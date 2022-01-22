<%@ Page Title="" Language="C#" MasterPageFile="~/WebPage/MasterPageWeek.master" AutoEventWireup="true" CodeFile="WeekReport_1_4_4.aspx.cs" Inherits="WebPage_WeekReport_1_4_4" %>

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
                yearRange: 'c-60:c+10'
            }).BootStrap(); //BootStrap() 產生符合 BootStrap 的樣式內容

            //新增開窗1
            $(document).on("click", "a[name='newbtn']", function () {
                $("#HGuid").val('');
                var datenow = new Date();
                var year = datenow.toLocaleDateString().slice(0, 4);
                var month = (datenow.getMonth()+1<10 ? '0' : '')+(datenow.getMonth()+1);
                var date = (datenow.getDate() < 10 ? '0' : '') + datenow.getDate();
                var fulldate = (parseInt(year) - 1911).toString() + month + date;

                $("#txt_unit").val('');                
                $("#txt_time").val(fulldate);
                $("#txt_place").val('');
                $("#txt_other").val('');
            });

            //新增開窗2
            $(document).on("click", "a[name='newbtn2']", function () {
                $("#HGuid").val('');
                var datenow = new Date();
                var year = datenow.toLocaleDateString().slice(0, 4);
                var month = (datenow.getMonth()+1<10 ? '0' : '')+(datenow.getMonth()+1);
                var date = (datenow.getDate() < 10 ? '0' : '') + datenow.getDate();
                var fulldate = (parseInt(year) - 1911).toString() + month + date;

                $("#txt_unit2").val('');                
                $("#txt_unitname").val('');                
                $("#txt_time2").val(fulldate);
                $("#txt_location").val('');
                $("#txt_concentration").val('');
                $("#txt_situation").val('');
            });

            // 編輯開窗1
            $(document).on("click", "a[name='editbtn']", function () {
                $("#HGuid").val($(this).attr("aid"));
                
                $("#txt_unit").val($("span[name='un_" + $("#HGuid").val() + "']").html());
                $("#txt_time").val($("span[name='tmd_" + $("#HGuid").val() + "']").html());
                $("#txt_place").val($("span[name='pl_" + $("#HGuid").val() + "']").html());
                $("#txt_other").val($("span[name='ot_" + $("#HGuid").val() + "']").html());
            });

            // 編輯開窗2
            $(document).on("click", "a[name='editbtn2']", function () {
                $("#HGuid").val($(this).attr("aid"));
                
                $("#txt_unit2").val($("span[name='un2_" + $("#HGuid").val() + "']").html());
                $("#txt_unitname").val($("span[name='unn_" + $("#HGuid").val() + "']").html());
                $("#txt_time2").val($("span[name='tmd_" + $("#HGuid").val() + "']").html());
                $("#txt_location").val($("span[name='lo_" + $("#HGuid").val() + "']").html());
                $("#txt_concentration").val($("span[name='co_" + $("#HGuid").val() + "']").html());
                $("#txt_situation").val($("span[name='si_" + $("#HGuid").val() + "']").html());
            });

            // 儲存開窗1
            $(document).on("click", "#subbtn", function () {
				var msg = '';
				if ($("#txt_unit").val() == "")
					msg += "請輸入【受查單位】 ";
				if ($("#txt_time").val() == "")
					msg += "請輸入【時間】 ";

				if (msg != "") {
					alert("錯誤訊息: " + msg);
					return false;
				}

                var mode = ($("#HGuid").val() == "") ? "new" : "mod";

                $.ajax({
			    	type: "POST",
			    	async: false, //在沒有返回值之前,不會執行下一步動作
			    	url: "../Handler/AddWeekReport_1_4_4.aspx",
			    	data: {
                        mode: mode,
                        type: "list1",
			    		rid: $("#HGuid").val(),
                        rpid: $.getQueryString("rpid"),
                        no: $("#reportno").html(),
			    		unit: $("#txt_unit").val(),
			    		time: $("#txt_time").val(),
			    		place: $("#txt_place").val(),
			    		other: $("#txt_other").val(),
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
                            location.href = "WeekReport_1_4_4.aspx?rid=" + $("#RGuid").val() + "&rpid=" + $("#RPGuid").val() + "&year=" + $.getQueryString("year");
			    		}
			    	}
			    });
            });

            // 儲存開窗2
            $(document).on("click", "#subbtn2", function () {
				var msg = '';
				if ($("#txt_unit2").val() == "")
                    msg += "請輸入【檢測單位】 ";
                if ($("#txt_unitname").val() == "")
					msg += "請輸入【站場名稱】 ";
				if ($("#txt_time2").val() == "")
					msg += "請輸入【檢測日期】 ";

				if (msg != "") {
					alert("錯誤訊息: " + msg);
					return false;
				}

                var mode = ($("#HGuid").val() == "") ? "new" : "mod";

                $.ajax({
			    	type: "POST",
			    	async: false, //在沒有返回值之前,不會執行下一步動作
			    	url: "../Handler/AddWeekReport_1_4_4.aspx",
			    	data: {
                        mode: mode,
                        type: "list2",
			    		rid: $("#HGuid").val(),
                        rpid: $.getQueryString("rpid"),
                        no: $("#reportno").html(),
			    		unit: $("#txt_unit2").val(),
			    		unitname: $("#txt_unitname").val(),
			    		time: $("#txt_time2").val(),
			    		location: $("#txt_location").val(),
			    		concentration: $("#txt_concentration").val(),
			    		situation: $("#txt_situation").val(),
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
                            location.href = "WeekReport_1_4_4.aspx?rid=" + $("#RGuid").val() + "&rpid=" + $("#RPGuid").val() + "&year=" + $.getQueryString("year");
			    		}
			    	}
			    });
            });

            //刪除工作項次1
            $(document).on("click", "a[name='delbtn']", function () {
                var str = confirm('確定刪除嗎?');
                if (str) {
                    $.ajax({
			        	type: "POST",
			        	async: false, //在沒有返回值之前,不會執行下一步動作
			        	url: "../Handler/DelWeekReport_1_4_4.aspx",
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
                                location.href = "WeekReport_1_4_4.aspx?rid=" + $("#RGuid").val() + "&rpid=" + $("#RPGuid").val() + "&year=" + $.getQueryString("year");
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
			        	url: "../Handler/DelWeekReport_1_4_4.aspx",
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
                                location.href = "WeekReport_1_4_4.aspx?rid=" + $("#RGuid").val() + "&rpid=" + $("#RPGuid").val() + "&year=" + $.getQueryString("year");
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
				url: "../Handler/GetWeekReport_1_4_4.aspx",
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
								tabstr += '<tr>';
                                tabstr += '<td align="center">';
                                tabstr += '<span name="sn_' + $(this).children("guid").text().trim() + '">' + $(this).children("場次").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="un_' + $(this).children("guid").text().trim() + '">' + $(this).children("受查單位").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td align="center">';                                
                                tabstr += '<span name="tm_' + $(this).children("guid").text().trim() + '">' + getDate($(this).children("預定日期").text().trim()) + '</span>'
                                tabstr += '<span style="display:none" name="tmd_' + $(this).children("guid").text().trim() + '">' + $(this).children("預定日期").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="pl_' + $(this).children("guid").text().trim() + '">' + $(this).children("地點").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="ot_' + $(this).children("guid").text().trim() + '">' + $(this).children("備註").text().trim() + '</span>'
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

                        $("#tablist2 tbody").empty();
						var tabstr = '';
						if ($(data).find("data_item3").length > 0) {
                            $(data).find("data_item3").each(function (i) {                                
								tabstr += '<tr>';
                                tabstr += '<td align="center">';
                                tabstr += '<span name="sn_' + $(this).children("guid").text().trim() + '">' + $(this).children("場次").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="un2_' + $(this).children("guid").text().trim() + '">' + $(this).children("檢測單位").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="unn_' + $(this).children("guid").text().trim() + '">' + $(this).children("站場名稱").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td align="center">';                                
                                tabstr += '<span name="tm_' + $(this).children("guid").text().trim() + '">' + getDate($(this).children("預定日期").text().trim()) + '</span>'
                                tabstr += '<span style="display:none" name="tmd_' + $(this).children("guid").text().trim() + '">' + $(this).children("預定日期").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="lo_' + $(this).children("guid").text().trim() + '">' + $(this).children("洩漏位置").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="co_' + $(this).children("guid").text().trim() + '">' + $(this).children("洩漏源甲烷濃度").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="si_' + $(this).children("guid").text().trim() + '">' + $(this).children("改善情形").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td align="center" class="font-normal">';
                                tabstr += '<a href="javascript:void(0);" name="delbtn2" aid="' + $(this).children("guid").text().trim() + '">刪除</a> ';
                                tabstr += '<a href="#workitem2" name="editbtn2" class="colorboxM" title="編輯工作項次" aid="' + $(this).children("guid").text().trim() + '">編輯</a>';
                                tabstr += '</td>';
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="8">查詢無資料</td></tr>';
						$("#tablist2 tbody").append(tabstr);
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
                                <th nowrap="nowrap" width="100">場次</th>
                                <th nowrap="nowrap">受查內容</th>
                                <th nowrap="nowrap" width="100">時間</th>
                                <th nowrap="nowrap">地點</th>
                                <th nowrap="nowrap">備註</th>
                                <th nowrap="nowrap" width="100">功能</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                    </table>
                </div>
                <!-- stripeMe -->
                <br />

                <div class="twocol margin10T">
                    <div class="left"></div>
                    <div class="right font-normal font-size3">
                        <a name="newbtn2" href="#workitem2" class="genbtn colorboxM" title="新增工作項次">新增</a>
                    </div>
                </div>

                <div class="stripeMeCS font-size3 margin10T">
                    <table id="tablist2" width="100%" border="0" cellspacing="0" cellpadding="0">
                        <thead>
                            <tr>
                                <th nowrap="nowrap" width="100">場次</th>
                                <th nowrap="nowrap">檢測單位</th>
                                <th nowrap="nowrap">站場名稱</th>
                                <th nowrap="nowrap" width="100">檢測日期</th>
                                <th nowrap="nowrap">洩漏位置</th>
                                <th nowrap="nowrap">洩漏源<br />甲烷濃度</th>
                                <th nowrap="nowrap">改善情形</th>
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
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">受查單位</div>
                    <div class="OchiCell width100">
                        <textarea id="txt_unit" rows="5" cols="" class="inputex width100"></textarea>
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">時間</div>
                    <div class="OchiCell width100">
                        <input id="txt_time" type="text" class="inputex pickDate width30" disabled>
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">地點</div>
                    <div class="OchiCell width100">
                        <input id="txt_place" type="text" class="inputex width100">
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">備註</div>
                    <div class="OchiCell width100">
                        <input id="txt_other" type="text" class="inputex width100">
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

<div style="display:none;">
    <div id="workitem2">
        <div class="margin35T padding5RL">
            <div class="OchiTrasTable width100 TitleLength08 font-size3">
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">檢測單位</div>
                    <div class="OchiCell width100">
                        <input id="txt_unit2" type="text" class="inputex width100" />
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">站場名稱</div>
                    <div class="OchiCell width100">
                        <input id="txt_unitname" type="text" class="inputex width100" />
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">檢測日期</div>
                    <div class="OchiCell width100">
                        <input id="txt_time2" type="text" class="inputex pickDate width30" disabled>
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">洩漏位置</div>
                    <div class="OchiCell width100">
                        <input id="txt_location" type="text" class="inputex width100" />
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">洩漏源甲烷濃度</div>
                    <div class="OchiCell width100">
                        <input id="txt_concentration" type="text" class="inputex width100" />
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">改善情形</div>
                    <div class="OchiCell width100">
                        <input id="txt_situation" type="text" class="inputex width100">
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





