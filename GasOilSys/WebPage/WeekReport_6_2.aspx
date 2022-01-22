<%@ Page Title="" Language="C#" MasterPageFile="~/WebPage/MasterPageWeek.master" AutoEventWireup="true" CodeFile="WeekReport_6_2.aspx.cs" Inherits="WebPage_WeekReport_6_2" %>

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
                var year = datenow.toLocaleDateString().slice(0,4)
                var month = (datenow.getMonth()+1<10 ? '0' : '')+(datenow.getMonth()+1);
                var date = (datenow.getDate()<10 ? '0' : '')+datenow.getDate();
                var fulldate = (parseInt(year) - 1911).toString() + month + date;

                $("#txt_content").val('');                
                $("#txt_time").val(fulldate);
                $("#txt_place").val('');
                $("#txt_mancount").val('0');
                $("#txt_other").val('');
            });

            // 編輯開窗1
            $(document).on("click", "a[name='editbtn']", function () {
                $("#HGuid").val($(this).attr("aid"));
                
                $("#txt_content").val($("span[name='cn_" + $("#HGuid").val() + "']").html());
                $("#txt_time").val($("span[name='tmd_" + $("#HGuid").val() + "']").html());
                $("#txt_place").val($("span[name='pl_" + $("#HGuid").val() + "']").html());
                $("#txt_mancount").val($("span[name='mc_" + $("#HGuid").val() + "']").html());
                $("#txt_other").val($("span[name='ot_" + $("#HGuid").val() + "']").html());
            });

            // 儲存開窗1
            $(document).on("click", "#subbtn", function () {
				var msg = '';
				if ($("#txt_content").val() == "")
					msg += "請輸入【工作內容】 ";
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
			    	url: "../Handler/AddWeekReport_6_2.aspx",
                    data: {
                        type: "1",
			    		mode: mode,
			    		rid: $("#HGuid").val(),
			    		rpid: $.getQueryString("rpid"),
			    		no: $("#reportno").html(),
			    		content: $("#txt_content").val(),
			    		time: $("#txt_time").val(),
			    		place: $("#txt_place").val(),
			    		mancount: $("#txt_mancount").val(),
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
                            location.href = "WeekReport_6_2.aspx?rid=" + $("#RGuid").val() + "&rpid=" + $("#RPGuid").val() + "&year=" + $.getQueryString("year");
			    		}
			    	}
			    });
            });

            //新增開窗2
            $(document).on("click", "a[name='newbtn2']", function () {
                $("#HGuid").val('');
                var datenow = new Date();
                var year = datenow.toLocaleDateString().slice(0,4)
                var month = (datenow.getMonth()+1<10 ? '0' : '')+(datenow.getMonth()+1);
                var date = (datenow.getDate()<10 ? '0' : '')+datenow.getDate();
                var fulldate = (parseInt(year) - 1911).toString() + month + date;

                $("#txt_content2").val('');                
                $("#txt_time2").val(fulldate);
                $("#txt_other2").val('');
            });

            // 編輯開窗2
            $(document).on("click", "a[name='editbtn']", function () {
                $("#HGuid").val($(this).attr("aid"));
                
                $("#txt_content2").val($("span[name='cn_" + $("#HGuid").val() + "']").html());
                $("#txt_time2").val($("span[name='tmd_" + $("#HGuid").val() + "']").html());
                $("#txt_other2").val($("span[name='ot_" + $("#HGuid").val() + "']").html());
            });

            // 儲存開窗2
            $(document).on("click", "#subbtn2", function () {
				var msg = '';
				if ($("#txt_content2").val() == "")
					msg += "請輸入【工作內容】 ";
				if ($("#txt_time2").val() == "")
					msg += "請輸入【時間】 ";

				if (msg != "") {
					alert("錯誤訊息: " + msg);
					return false;
				}

                var mode = ($("#HGuid").val() == "") ? "new" : "mod";

                $.ajax({
			    	type: "POST",
			    	async: false, //在沒有返回值之前,不會執行下一步動作
			    	url: "../Handler/AddWeekReport_6_2.aspx",
                    data: {
                        type: "2",
			    		mode: mode,
			    		rid: $("#HGuid").val(),
			    		rpid: $.getQueryString("rpid"),
			    		no: $("#reportno").html(),
			    		content: $("#txt_content2").val(),
			    		time: $("#txt_time2").val(),
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
                            location.href = "WeekReport_6_2.aspx?rid=" + $("#RGuid").val() + "&rpid=" + $("#RPGuid").val() + "&year=" + $.getQueryString("year");
			    		}
			    	}
			    });
            });

            //新增開窗3
            $(document).on("click", "a[name='newbtn3']", function () {
                $("#HGuid").val('');
                var datenow = new Date();
                var year = datenow.toLocaleDateString().slice(0,4)
                var month = (datenow.getMonth()+1<10 ? '0' : '')+(datenow.getMonth()+1);
                var date = (datenow.getDate()<10 ? '0' : '')+datenow.getDate();
                var fulldate = (parseInt(year) - 1911).toString() + month + date;

                $("#txt_content3").val('');                
                $("#txt_time3").val(fulldate);
                $("#txt_mancount2").val('0');
                $("#txt_other3").val('');
            });

            // 編輯開窗3
            $(document).on("click", "a[name='editbtn']", function () {
                $("#HGuid").val($(this).attr("aid"));
                
                $("#txt_content3").val($("span[name='cn_" + $("#HGuid").val() + "']").html());
                $("#txt_time3").val($("span[name='tmd_" + $("#HGuid").val() + "']").html());
                $("#txt_mancount2").val($("span[name='mc_" + $("#HGuid").val() + "']").html());
                $("#txt_other3").val($("span[name='ot_" + $("#HGuid").val() + "']").html());
            });

            // 儲存開窗3
            $(document).on("click", "#subbtn3", function () {
				var msg = '';
				if ($("#txt_content3").val() == "")
					msg += "請輸入【工作內容】 ";
				if ($("#txt_time3").val() == "")
					msg += "請輸入【時間】 ";

				if (msg != "") {
					alert("錯誤訊息: " + msg);
					return false;
				}

                var mode = ($("#HGuid").val() == "") ? "new" : "mod";

                $.ajax({
			    	type: "POST",
			    	async: false, //在沒有返回值之前,不會執行下一步動作
			    	url: "../Handler/AddWeekReport_6_2.aspx",
                    data: {
                        type: "3",
			    		mode: mode,
			    		rid: $("#HGuid").val(),
			    		rpid: $.getQueryString("rpid"),
			    		no: $("#reportno").html(),
			    		content: $("#txt_content3").val(),
			    		time: $("#txt_time3").val(),
			    		mancount: $("#txt_mancount2").val(),
			    		other: $("#txt_other3").val(),
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
                            location.href = "WeekReport_6_2.aspx?rid=" + $("#RGuid").val() + "&rpid=" + $("#RPGuid").val() + "&year=" + $.getQueryString("year");
			    		}
			    	}
			    });
            });

            //新增開窗4
            $(document).on("click", "a[name='newbtn4']", function () {
                $("#HGuid").val('');
                var datenow = new Date();
                var year = datenow.toLocaleDateString().slice(0,4)
                var month = (datenow.getMonth()+1<10 ? '0' : '')+(datenow.getMonth()+1);
                var date = (datenow.getDate()<10 ? '0' : '')+datenow.getDate();
                var fulldate = (parseInt(year) - 1911).toString() + month + date;

                $("#txt_content4").val('');                
                $("#txt_time4").val(fulldate);
                $("#txt_filename").val('');
            });

            // 編輯開窗4
            $(document).on("click", "a[name='editbtn']", function () {
                $("#HGuid").val($(this).attr("aid"));
                
                $("#txt_content4").val($("span[name='cn_" + $("#HGuid").val() + "']").html());
                $("#txt_time4").val($("span[name='tmd_" + $("#HGuid").val() + "']").html());
                $("#txt_filename").val($("span[name='ot_" + $("#HGuid").val() + "']").html());
            });

            // 儲存開窗4
            $(document).on("click", "#subbtn4", function () {
				var msg = '';
				if ($("#txt_content4").val() == "")
					msg += "請輸入【工作內容】 ";
				if ($("#txt_time4").val() == "")
					msg += "請輸入【時間】 ";

				if (msg != "") {
					alert("錯誤訊息: " + msg);
					return false;
				}

                var mode = ($("#HGuid").val() == "") ? "new" : "mod";

                $.ajax({
			    	type: "POST",
			    	async: false, //在沒有返回值之前,不會執行下一步動作
			    	url: "../Handler/AddWeekReport_6_2.aspx",
                    data: {
                        type: "4",
			    		mode: mode,
			    		rid: $("#HGuid").val(),
			    		rpid: $.getQueryString("rpid"),
			    		no: $("#reportno").html(),
			    		content: $("#txt_content4").val(),
			    		time: $("#txt_time4").val(),
			    		filename: $("#txt_filename").val(),
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
                            location.href = "WeekReport_6_2.aspx?rid=" + $("#RGuid").val() + "&rpid=" + $("#RPGuid").val() + "&year=" + $.getQueryString("year");
			    		}
			    	}
			    });
            });

            //新增開窗5
            $(document).on("click", "a[name='newbtn5']", function () {
                $("#HGuid").val('');
                var datenow = new Date();
                var year = datenow.toLocaleDateString().slice(0,4)
                var month = (datenow.getMonth()+1<10 ? '0' : '')+(datenow.getMonth()+1);
                var date = (datenow.getDate()<10 ? '0' : '')+datenow.getDate();
                var fulldate = (parseInt(year) - 1911).toString() + month + date;

                $("#txt_content5").val('');                
                $("#txt_time5").val(fulldate);
                $("#txt_place2").val('');
                $("#txt_mancount3").val('0');
                $("#txt_other4").val('');
            });

            // 編輯開窗5
            $(document).on("click", "a[name='editbtn']", function () {
                $("#HGuid").val($(this).attr("aid"));
                var fulldate = $("span[name='tmd_" + $("#HGuid").val() + "']").html();
                var year = fulldate.substring("0", "4");
                var month = fulldate.substring("4", "6");
                var date = fulldate.substring("6", "8");
                
                $("#txt_content5").val($("span[name='cn_" + $("#HGuid").val() + "']").html());
                $("#txt_time5").val($("span[name='tmd_" + $("#HGuid").val() + "']").html());
                $("#txt_place2").val($("span[name='pl_" + $("#HGuid").val() + "']").html());
                $("#txt_mancount3").val($("span[name='mc_" + $("#HGuid").val() + "']").html());
                $("#txt_other4").val($("span[name='ot_" + $("#HGuid").val() + "']").html());
            });

            // 儲存開窗5
            $(document).on("click", "#subbtn5", function () {
				var msg = '';
				if ($("#txt_content5").val() == "")
					msg += "請輸入【執行內容】 ";
				if ($("#txt_time5").val() == "")
					msg += "請輸入【時間】 ";

				if (msg != "") {
					alert("錯誤訊息: " + msg);
					return false;
				}

                var mode = ($("#HGuid").val() == "") ? "new" : "mod";

                $.ajax({
			    	type: "POST",
			    	async: false, //在沒有返回值之前,不會執行下一步動作
			    	url: "../Handler/AddWeekReport_6_2.aspx",
                    data: {
                        type: "5",
			    		mode: mode,
			    		rid: $("#HGuid").val(),
			    		rpid: $.getQueryString("rpid"),
			    		no: $("#reportno").html(),
			    		content: $("#txt_content5").val(),
			    		time: $("#txt_time5").val(),
			    		place: $("#txt_place2").val(),
			    		mancount: $("#txt_mancount3").val(),
			    		other: $("#txt_other4").val(),
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
                            location.href = "WeekReport_6_2.aspx?rid=" + $("#RGuid").val() + "&rpid=" + $("#RPGuid").val() + "&year=" + $.getQueryString("year");
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
			        	url: "../Handler/DelWeekReport_6_2.aspx",
                        data: {
                            type: $(this).attr("bid"),
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
                                location.href = "WeekReport_6_2.aspx?rid=" + $("#RGuid").val() + "&rpid=" + $("#RPGuid").val() + "&year=" + $.getQueryString("year");
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
				url: "../Handler/GetWeekReport_6_2.aspx",
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
                                tabstr += '<span name="mc_' + $(this).children("guid").text().trim() + '">' + $(this).children("人數").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="ot_' + $(this).children("guid").text().trim() + '">' + $(this).children("備註").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td align="center" class="font-normal">';
                                tabstr += '<a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("guid").text().trim() + '" bid="1">刪除</a> ';
                                tabstr += '<a href="#workitem" name="editbtn" class="colorboxM" title="編輯工作項次" aid="' + $(this).children("guid").text().trim() + '">編輯</a>';
                                tabstr += '</td>';
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="7">查詢無資料</td></tr>';
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
                                tabstr += '<span name="cn_' + $(this).children("guid").text().trim() + '">' + $(this).children("執行內容").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td align="center">';                                
                                tabstr += '<span name="tm_' + $(this).children("guid").text().trim() + '">' + getDate($(this).children("預定日期").text().trim()) + '</span>'
                                tabstr += '<span style="display:none" name="tmd_' + $(this).children("guid").text().trim() + '">' + $(this).children("預定日期").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="ot_' + $(this).children("guid").text().trim() + '">' + $(this).children("備註").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td align="center" class="font-normal">';
                                tabstr += '<a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("guid").text().trim() + '" bid="2">刪除</a> ';
                                tabstr += '<a href="#workitem2" name="editbtn" class="colorboxM" title="編輯工作項次" aid="' + $(this).children("guid").text().trim() + '">編輯</a>';
                                tabstr += '</td>';
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="5">查詢無資料</td></tr>';
                        $("#tablist2 tbody").append(tabstr);

                        $("#tablist3 tbody").empty();
						var tabstr = '';
						if ($(data).find("data_item4").length > 0) {
                            $(data).find("data_item4").each(function (i) {                                
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
                                tabstr += '<span name="mc_' + $(this).children("guid").text().trim() + '">' + $(this).children("人數").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="ot_' + $(this).children("guid").text().trim() + '">' + $(this).children("備註").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td align="center" class="font-normal">';
                                tabstr += '<a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("guid").text().trim() + '" bid="3">刪除</a> ';
                                tabstr += '<a href="#workitem3" name="editbtn" class="colorboxM" title="編輯工作項次" aid="' + $(this).children("guid").text().trim() + '">編輯</a>';
                                tabstr += '</td>';
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="6">查詢無資料</td></tr>';
                        $("#tablist3 tbody").append(tabstr);

                        $("#tablist4 tbody").empty();
						var tabstr = '';
						if ($(data).find("data_item5").length > 0) {
                            $(data).find("data_item5").each(function (i) {                                
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
                                tabstr += '<span name="fn_' + $(this).children("guid").text().trim() + '">' + $(this).children("文件名稱").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td align="center" class="font-normal">';
                                tabstr += '<a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("guid").text().trim() + '" bid="4">刪除</a> ';
                                tabstr += '<a href="#workitem4" name="editbtn" class="colorboxM" title="編輯工作項次" aid="' + $(this).children("guid").text().trim() + '">編輯</a>';
                                tabstr += '</td>';
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="5">查詢無資料</td></tr>';
                        $("#tablist4 tbody").append(tabstr);

                        $("#tablist5 tbody").empty();
						var tabstr = '';
						if ($(data).find("data_item6").length > 0) {
                            $(data).find("data_item6").each(function (i) {                                
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
                                tabstr += '<span name="mc_' + $(this).children("guid").text().trim() + '">' + $(this).children("人數").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="ot_' + $(this).children("guid").text().trim() + '">' + $(this).children("備註").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td align="center" class="font-normal">';
                                tabstr += '<a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("guid").text().trim() + '" bid="5">刪除</a> ';
                                tabstr += '<a href="#workitem5" name="editbtn" class="colorboxM" title="編輯工作項次" aid="' + $(this).children("guid").text().trim() + '">編輯</a>';
                                tabstr += '</td>';
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="7">查詢無資料</td></tr>';
                        $("#tablist5 tbody").append(tabstr);
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
                                <th colspan="7">總工作報告會議</th>
                            </tr>
                            <tr>
                                <th nowrap="nowrap" width="100">項次</th>
                                <th nowrap="nowrap" width="500">工作內容</th>
                                <th nowrap="nowrap" width="100">時間</th>
                                <th nowrap="nowrap">地點</th>
                                <th nowrap="nowrap" width="100">人數</th>
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
                                <th colspan="5">繳交報告及配合事項</th>
                            </tr>
                            <tr>
                                <th nowrap="nowrap" width="100">項次</th>
                                <th nowrap="nowrap" width="500">工作內容</th>
                                <th nowrap="nowrap" width="100">時間</th>
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
                        <a name="newbtn3" href="#workitem3" class="genbtn colorboxM" title="新增工作項次">新增</a>
                    </div>
                </div>

                <div class="stripeMeCS font-size3 margin10T">
                    <table id="tablist3" width="100%" border="0" cellspacing="0" cellpadding="0">
                        <thead>
                            <tr>
                                <th colspan="6">臨時交辦議題配合事項(專家及業者會議)</th>
                            </tr>
                            <tr>
                                <th nowrap="nowrap" width="100">項次</th>
                                <th nowrap="nowrap" width="500">工作內容</th>
                                <th nowrap="nowrap" width="100">時間</th>
                                <th nowrap="nowrap" width="100">人數</th>
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
                        <a name="newbtn4" href="#workitem4" class="genbtn colorboxM" title="新增工作項次">新增</a>
                    </div>
                </div>

                <div class="stripeMeCS font-size3 margin10T">
                    <table id="tablist4" width="100%" border="0" cellspacing="0" cellpadding="0">
                        <thead>
                            <tr>
                                <th colspan="5">臨時交辦議題配合事項</th>
                            </tr>
                            <tr>
                                <th nowrap="nowrap" width="100">項次</th>
                                <th nowrap="nowrap" width="500">工作內容</th>
                                <th nowrap="nowrap" width="100">時間</th>
                                <th nowrap="nowrap">文件名稱</th>
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
                        <a name="newbtn5" href="#workitem5" class="genbtn colorboxM" title="新增工作項次">新增</a>
                    </div>
                </div>

                <div class="stripeMeCS font-size3 margin10T">
                    <table id="tablist5" width="100%" border="0" cellspacing="0" cellpadding="0">
                        <thead>
                            <tr>
                                <th colspan="7">工作討論會議統計</th>
                            </tr>
                            <tr>
                                <th nowrap="nowrap" width="100">項次</th>
                                <th nowrap="nowrap" width="500">執行內容</th>
                                <th nowrap="nowrap" width="100">時間</th>
                                <th nowrap="nowrap">地點</th>
                                <th nowrap="nowrap" width="100">人數</th>
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
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">工作內容</div>
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
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">人數</div>
                    <div class="OchiCell width100">
                        <input id="txt_mancount" type="number" min="0" class="inputex width10">
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
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">工作內容</div>
                    <div class="OchiCell width100">
                        <textarea id="txt_content2" rows="5" cols="" class="inputex width100"></textarea>
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">時間</div>
                    <div class="OchiCell width100">
                        <input id="txt_time2" type="text" class="inputex pickDate width30" disabled>
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

<div style="display:none;">
    <div id="workitem3">
        <div class="margin35T padding5RL">
            <div class="OchiTrasTable width100 TitleLength08 font-size3">
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">工作內容</div>
                    <div class="OchiCell width100">
                        <textarea id="txt_content3" rows="5" cols="" class="inputex width100"></textarea>
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">時間</div>
                    <div class="OchiCell width100">
                        <input id="txt_time3" type="text" class="inputex pickDate width30" disabled>
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">人數</div>
                    <div class="OchiCell width100">
                        <input id="txt_mancount2" type="number" min="0" class="inputex width10">
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">備註</div>
                    <div class="OchiCell width100">
                        <input id="txt_other3" type="text" class="inputex width100">
                    </div>
                </div><!-- OchiRow -->
            </div><!-- OchiTrasTable -->
        </div>

        <div class="twocol margin10T">
            <div class="right">
                <a href="javascript:void(0);" id="canbtn3" class="genbtn closecolorbox">取消</a>
                <a href="javascript:void(0);" id="subbtn3" class="genbtn">儲存</a>
            </div>
        </div>
        <br /><br />
    </div>
</div>

<div style="display:none;">
    <div id="workitem4">
        <div class="margin35T padding5RL">
            <div class="OchiTrasTable width100 TitleLength08 font-size3">
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">工作內容</div>
                    <div class="OchiCell width100">
                        <textarea id="txt_content4" rows="5" cols="" class="inputex width100"></textarea>
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">時間</div>
                    <div class="OchiCell width100">
                        <input id="txt_time4" type="text" class="inputex pickDate width30" disabled>
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">文件名稱</div>
                    <div class="OchiCell width100">
                        <input id="txt_filename" type="text" class="inputex width100">
                    </div>
                </div><!-- OchiRow -->
            </div><!-- OchiTrasTable -->
        </div>

        <div class="twocol margin10T">
            <div class="right">
                <a href="javascript:void(0);" id="canbtn4" class="genbtn closecolorbox">取消</a>
                <a href="javascript:void(0);" id="subbtn4" class="genbtn">儲存</a>
            </div>
        </div>
        <br /><br />
    </div>
</div>

<div style="display:none;">
    <div id="workitem5">
        <div class="margin35T padding5RL">
            <div class="OchiTrasTable width100 TitleLength08 font-size3">
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">執行內容</div>
                    <div class="OchiCell width100">
                        <textarea id="txt_content5" rows="5" cols="" class="inputex width100"></textarea>
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">時間</div>
                    <div class="OchiCell width100">
                        <input id="txt_time5" type="text" class="inputex pickDate width30" disabled>
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">地點</div>
                    <div class="OchiCell width100">
                        <input id="txt_place2" type="text" class="inputex width100">
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">人數</div>
                    <div class="OchiCell width100">
                        <input id="txt_mancount3" type="number" min="0" class="inputex width10">
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">備註</div>
                    <div class="OchiCell width100">
                        <input id="txt_other4" type="text" class="inputex width100">
                    </div>
                </div><!-- OchiRow -->
            </div><!-- OchiTrasTable -->
        </div>

        <div class="twocol margin10T">
            <div class="right">
                <a href="javascript:void(0);" id="canbtn5" class="genbtn closecolorbox">取消</a>
                <a href="javascript:void(0);" id="subbtn5" class="genbtn">儲存</a>
            </div>
        </div>
        <br /><br />
    </div>
</div>
</asp:Content>



