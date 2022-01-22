<%@ Page Title="" Language="C#" MasterPageFile="~/WebPage/MasterPageWeek.master" AutoEventWireup="true" CodeFile="WeekReport_3_8.aspx.cs" Inherits="WebPage_WeekReport_3_8" %>

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
            getCommittee();

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

                $("#txt_content").val('');                
                $("#txt_time").val(fulldate);
                $("#txt_place").val('');
                $("#txt_committee").val('');
                $("#num_mancount").val('0');
                $("#txt_other").val('');
            });

            // 編輯開窗
            $(document).on("click", "a[name='editbtn']", function () {
                $("#HGuid").val($(this).attr("aid"));
                
                $("#txt_content").val($("span[name='cn_" + $("#HGuid").val() + "']").html());
                $("#txt_time").val($("span[name='tmd_" + $("#HGuid").val() + "']").html());
                $("#txt_place").val($("span[name='pl_" + $("#HGuid").val() + "']").html());
                $("#txt_committee").val($("span[name='co_" + $("#HGuid").val() + "']").html());
                $("#num_mancount").val($("span[name='mc_" + $("#HGuid").val() + "']").html());
                $("#txt_other").val($("span[name='ot_" + $("#HGuid").val() + "']").html());
            });

            // 儲存開窗
            $(document).on("click", "#subbtn", function () {
				var msg = '';
				if ($("#txt_content").val() == "")
					msg += "請輸入【執行內容】 ";
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
			    	url: "../Handler/AddWeekReport_3_8.aspx",
			    	data: {
			    		mode: mode,
			    		rid: $("#HGuid").val(),
                        rpid: $.getQueryString("rpid"),
                        no: $("#reportno").html(),
			    		content: $("#txt_content").val(),
			    		time: $("#txt_time").val(),
			    		place: $("#txt_place").val(),
			    		committee: $("#txt_committee").val(),
			    		mancount: $("#num_mancount").val(),
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
                            location.href = "WeekReport_3_8.aspx?rid=" + $("#RGuid").val() + "&rpid=" + $("#RPGuid").val() + "&year=" + $.getQueryString("year");
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
			        	url: "../Handler/DelWeekReport_3_8.aspx",
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
                                location.href = "WeekReport_3_8.aspx?rid=" + $("#RGuid").val() + "&rpid=" + $("#RPGuid").val() + "&year=" + $.getQueryString("year");
			        		}
			        	}
			        });
                }
            });

            $(document).on("click", "#checkall", function () {
                if ($("#checkall").prop("checked")) {
                    $("input[name='com_check']").prop("checked", true);
                }
                else {
                    $("input[name='com_check']").prop("checked", false);
                }
            });

            $(document).on("click", "#Csubbtn", function () {
                $("input[name='com_check']").each(function () {
                    if (this.checked) {
                        if ($("#txt_committee").val() == "") {
                            $("#txt_committee").val($("span[name='na_" + $(this).val().trim() + "']").html());
                        }
                        else {
                            var str = $("#txt_committee").val();
                            var str2 = "," + $("span[name='na_" + $(this).val().trim() + "']").html();
                            $("#txt_committee").val(str + str2);
                        }
                    }                    
                });
                doOpenDialog();
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
				url: "../Handler/GetWeekReport_3_8.aspx",
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
                                tabstr += '<span name="cn_' + $(this).children("guid").text().trim() + '">' + $(this).children("執行內容").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td align="center">';                                
                                tabstr += '<span name="tm_' + $(this).children("guid").text().trim() + '">' + getDate($(this).children("預定日期").text().trim()) + '</span>'
                                tabstr += '<span style="display:none" name="tmd_' + $(this).children("guid").text().trim() + '">' + $(this).children("預定日期").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="pl_' + $(this).children("guid").text().trim() + '">' + $(this).children("地點").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="co_' + $(this).children("guid").text().trim() + '">' + $(this).children("委員").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="mc_' + $(this).children("guid").text().trim() + '">' + $(this).children("人數").text().trim() + '</span>'
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
							tabstr += '<tr><td colspan="8">查詢無資料</td></tr>';
						$("#tablist tbody").append(tabstr);
					}
				}
			});
        }

        function getCommittee() {
            $.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetCommitteeList.aspx",
				data: {
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
						$("#committeelist tbody").empty();
						var tabstr = '';
						if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {                                
								tabstr += '<tr>';
                                tabstr += '<td align="center">';
                                tabstr += '<span>' + $(this).children("場次").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="na_' + $(this).children("guid").text().trim() + '">' + $(this).children("姓名").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td align="center" class="font-normal">';
                                tabstr += '<input type="checkbox" name="com_check" value="' + $(this).children("guid").text().trim() + '" />';
                                tabstr += '</td>';
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="3">查詢無資料</td></tr>';
						$("#committeelist tbody").append(tabstr);
					}
				}
			});
        }

         function doOpenDialog() {
            var WinHeight = $("html").height();
            var ColHeight = WinHeight * 0.6;
            $.colorbox({ title: "新增工作項次", inline: true, href: "#workitem", width: "100%", maxWidth: "800", maxHeight: ColHeight, opacity: 0.5 });
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
                                <th nowrap="nowrap">執行內容</th>
                                <th nowrap="nowrap" width="100">時間</th>
                                <th nowrap="nowrap">地點</th>
                                <th nowrap="nowrap">委員</th>
                                <th nowrap="nowrap">人數</th>
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
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">執行內容</div>
                    <div class="OchiCell width100">
                        <textarea id="txt_content" rows="5" cols="" class="inputex width100"></textarea>
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
                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">委員</div>
                        <div class="OchiCell width100">
                            <textarea id="txt_committee" rows="3" cols="" class="inputex width100"></textarea>
                            <a href="#committeeitem" id="btnadd" class="genbtn colorboxM" title="委員列表" >新增委員</a>
                        </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">人數</div>
                    <div class="OchiCell width100">
                        <input id="num_mancount" type="number" class="inputex width10" min="0">
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
    <div id="committeeitem">
        <div class="margin35T padding5RL">
            <div class="stripeMeCS font-size3 margin10T">
                <table id="committeelist" width="100%" border="0" cellspacing="0" cellpadding="0">
                    <thead>
                        <tr>
                            <th nowrap="nowrap" width="100">項次</th>
                            <th nowrap="nowrap">委員名稱</th>
                            <th nowrap="nowrap" width="100">全選 <input id="checkall" type="checkbox" /></th>
                        </tr>
                    </thead>
                    <tbody></tbody>
                </table>
            </div>
            <!-- stripeMe -->
        </div>

        <div class="twocol margin10T">
            <div class="right">
                <a href="javascript:void(0);" id="Ccanbtn" class="genbtn" onclick="doOpenDialog();">取消</a>
                <a href="javascript:void(0);" id="Csubbtn" class="genbtn">確定</a>
            </div>
        </div>
        <br /><br />
    </div>
</div>
</asp:Content>


