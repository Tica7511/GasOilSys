<%@ Page Title="" Language="C#" MasterPageFile="~/WebPage/MasterPageWeek.master" AutoEventWireup="true" CodeFile="WeekReport_2_2_2.aspx.cs" Inherits="WebPage_WeekReport_2_2_2" %>

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

            //新增開窗
            $(document).on("click", "a[name='newbtn']", function () {
                $("#HGuid").val('');
                var datenow = new Date();
                var year = datenow.toLocaleDateString().slice(0,4)
                var month = (datenow.getMonth()+1<10 ? '0' : '')+(datenow.getMonth()+1);
                var date = (datenow.getDate()<10 ? '0' : '')+datenow.getDate();
                var fulldate = (parseInt(year) - 1911).toString() + month + date;

                $("#txt_unit").val('');
                $("#txt_pipename").val('');
                $("#txt_pipeno").val('');
                $("#txt_begin").val('');
                $("#txt_end").val('');
                $("#txt_begintime").val(fulldate);
                $("#txt_endtime").val(fulldate);
                $("#txt_length").val('');
                $("#txt_other").val('');
            });

            // 編輯開窗
            $(document).on("click", "a[name='editbtn']", function () {
                $("#HGuid").val($(this).attr("aid"));
                
                $("#txt_unit").val($("span[name='un_" + $("#HGuid").val() + "']").html());
                $("#txt_pipename").val($("span[name='pna_" + $("#HGuid").val() + "']").html());
                $("#txt_pipeno").val($("span[name='pno_" + $("#HGuid").val() + "']").html());
                $("#txt_begin").val($("span[name='be_" + $("#HGuid").val() + "']").html());
                $("#txt_end").val($("span[name='en_" + $("#HGuid").val() + "']").html());
                $("#txt_begintime").val($("span[name='tmd1_" + $("#HGuid").val() + "']").html());
                $("#txt_endtime").val($("span[name='tmd2_" + $("#HGuid").val() + "']").html());
                $("#txt_length").val($("span[name='le_" + $("#HGuid").val() + "']").html());
                $("#txt_other").val($("span[name='ot_" + $("#HGuid").val() + "']").html());
            });

            // 儲存開窗
            $(document).on("click", "#subbtn", function () {
				var msg = '';
				if ($("#txt_unit").val() == "")
					msg += "請輸入【受查單位】 ";
				if ($("#txt_begintime").val() == "")
                    msg += "請輸入【檢測日期(起)】 ";
                if ($("#txt_endtime").val() == "")
                    msg += "請輸入【檢測日期(迄)】 ";
                if (parseInt($("#txt_begintime").val()) > parseInt($("#txt_endtime").val()))
                    msg += "【檢測日期(迄)】不可小於【檢測日期(起)】 ";
				if (msg != "") {
					alert("錯誤訊息: " + msg);
					return false;
				}

                var mode = ($("#HGuid").val() == "") ? "new" : "mod";

                $.ajax({
			    	type: "POST",
			    	async: false, //在沒有返回值之前,不會執行下一步動作
			    	url: "../Handler/AddWeekReport_2_2_2.aspx",
			    	data: {
			    		mode: mode,
			    		rid: $("#HGuid").val(),
                        rpid: $.getQueryString("rpid"),
                        no: $("#reportno").html(),
			    		unit: $("#txt_unit").val(),
			    		pipename: $("#txt_pipename").val(),
			    		pipeno: $("#txt_pipeno").val(),
			    		begin: $("#txt_begin").val(),
			    		end: $("#txt_end").val(),
			    		begintime: $("#txt_begintime").val(),
			    		endtime: $("#txt_endtime").val(),
			    		length: $("#txt_length").val(),
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
                            location.href = "WeekReport_2_2_2.aspx?rid=" + $("#RGuid").val() + "&rpid=" + $("#RPGuid").val() + "&year=" + $.getQueryString("year");
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
			        	url: "../Handler/DelWeekReport_2_2_2.aspx",
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
                                location.href = "WeekReport_2_2_2.aspx?rid=" + $("#RGuid").val() + "&rpid=" + $("#RPGuid").val() + "&year=" + $.getQueryString("year");
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
				url: "../Handler/GetWeekReport_2_2_2.aspx",
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
                                tabstr += '<span name="sn_' + $(this).children("guid").text().trim() + '">' + $(this).children("場次").text().trim() + '</span>';
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="un_' + $(this).children("guid").text().trim() + '">' + $(this).children("受查單位").text().trim() + '</span>';
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="pna_' + $(this).children("guid").text().trim() + '">' + $(this).children("管線名稱").text().trim() + '</span>';
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="pno_' + $(this).children("guid").text().trim() + '">' + $(this).children("管線識別碼").text().trim() + '</span>';
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="be_' + $(this).children("guid").text().trim() + '">' + $(this).children("起點").text().trim() + '</span>';
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="en_' + $(this).children("guid").text().trim() + '">' + $(this).children("迄點").text().trim() + '</span>';
                                tabstr += '</td>';
                                tabstr += '<td align="center">';                                
                                tabstr += '<span name="tm1_' + $(this).children("guid").text().trim() + '">' + getDate($(this).children("預定日期起").text().trim()) + '</span> ~ ';
                                tabstr += '<span name="tm2_' + $(this).children("guid").text().trim() + '">' + getDate($(this).children("預定日期迄").text().trim()) + '</span>';
                                tabstr += '<span style="display:none" name="tmd1_' + $(this).children("guid").text().trim() + '">' + $(this).children("預定日期起").text().trim() + '</span>';
                                tabstr += '<span style="display:none" name="tmd2_' + $(this).children("guid").text().trim() + '">' + $(this).children("預定日期迄").text().trim() + '</span>';
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="le_' + $(this).children("guid").text().trim() + '">' + $(this).children("檢測長度").text().trim() + '</span>';
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="ot_' + $(this).children("guid").text().trim() + '">' + $(this).children("備註").text().trim() + '</span>';
                                tabstr += '</td>';
                                tabstr += '<td align="center" class="font-normal">';
                                tabstr += '<a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("guid").text().trim() + '">刪除</a> ';
                                tabstr += '<a href="#workitem" name="editbtn" class="colorboxM" title="編輯工作項次" aid="' + $(this).children("guid").text().trim() + '">編輯</a>';
                                tabstr += '</td>';
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="10">查詢無資料</td></tr>';
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
                                <th nowrap="nowrap">受查單位</th>
                                <th nowrap="nowrap">管線名稱</th>
                                <th nowrap="nowrap">管線識別碼</th>
                                <th nowrap="nowrap">起點</th>
                                <th nowrap="nowrap">迄點</th>
                                <th nowrap="nowrap">檢測期程</th>
                                <th nowrap="nowrap">檢測長度</th>
                                <th nowrap="nowrap">備註</th>
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
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">管線名稱</div>
                    <div class="OchiCell width100">
                        <input id="txt_pipename" type="text" class="inputex width70" />
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">管線識別碼</div>
                    <div class="OchiCell width100">
                        <input id="txt_pipeno" type="text" class="inputex width70" />
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">起點</div>
                    <div class="OchiCell width100">
                        <input id="txt_begin" type="text" class="inputex width100" />
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">迄點</div>
                    <div class="OchiCell width100">
                        <input id="txt_end" type="text" class="inputex width100" />
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">檢測日期(起)</div>
                    <div class="OchiCell width100">
                        <input id="txt_begintime" type="text" class="inputex pickDate width30" disabled>
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">檢測日期(迄)</div>
                    <div class="OchiCell width100">
                        <input id="txt_endtime" type="text" class="inputex pickDate width30" disabled>
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">檢測長度</div>
                    <div class="OchiCell width100">
                        <input id="txt_length" type="text" class="inputex width70">
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
</asp:Content>

