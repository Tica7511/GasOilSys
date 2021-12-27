<%@ Page Title="" Language="C#" MasterPageFile="~/WebPage/MasterPageWeek.master" AutoEventWireup="true" CodeFile="WeekSearch.aspx.cs" Inherits="WebPage_WeekSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        #datepick-div{
            position:absolute;
            z-index:9999;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {

            $(".pickDate").datepick({
                dateFormat: 'yymmdd',
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: '../images/calendar.gif',
                yearRange: 'c-6:c+6'
            }).BootStrap(); //BootStrap() 產生符合 BootStrap 的樣式內容

            $("#tablist").hide();

            //查詢列表
            $(document).on("click", "#querybtn", function () {
                getData();
            });

            //新增開窗
            $(document).on("click", "#newbtn", function () {
                $("#HGuid").val('');
                $("#txt_no1").attr('disabled', false);
                $("#txt_no2").attr('disabled', false);
                $("#txt_no3").attr('disabled', false);
                var datenow = new Date();
                var year = datenow.toLocaleDateString().slice(0, 4)
                var month = (datenow.getMonth() + 1 < 10 ? '0' : '') + (datenow.getMonth() + 1);
                var date = (datenow.getDate() < 10 ? '0' : '') + datenow.getDate();
                var fulldate = (parseInt(year) - 1911).toString() + month + date;

                $("#txt_no1").val('1');
                $("#txt_no2").val('1');
                $("#txt_no3").val('0');
                $("#txt_content").val('');
                $("#txt_time").val(fulldate);
            });

            // 編輯開窗
            $(document).on("click", "a[name='editbtn']", function () {
                $("#HGuid").val($(this).attr("aid"));
                $("#txt_no1").attr('disabled', true);
                $("#txt_no2").attr('disabled', true);
                $("#txt_no3").attr('disabled', true);

                var nostr = new Array();
                nostr = ($("span[name='sn_" + $("#HGuid").val() + "']").html()).split('.');

                for (var j = 1; j <= nostr.length; j++)
                    $("#txt_no" + j).val(nostr[j - 1]);
                for (var i = nostr.length; i < 3; i++)
                    $("#txt_no" + (i + 1)).val('0');
                $("#txt_content").val($("span[name='cn_" + $("#HGuid").val() + "']").html());
                $("#txt_time").val($("span[name='tmd_" + $("#HGuid").val() + "']").html());

                doOpenDialog();
            });

            // 儲存開窗
            $(document).on("click", "#subbtn", function () {
				var msg = '';
				if ($("#txt_no1").val() == "")
                    msg += "請完整的輸入【工作項次】 ";
                if ($("#txt_no2").val() == "")
                    msg += "請完整的輸入【工作項次】 ";
				if ($("#txt_content").val() == "")
                    msg += "請輸入【預定完成執行內容】 ";
                if ($("#txt_time").val() == "")
                    msg += "請輸入【預定日期】 ";

				if (msg != "") {
					alert("錯誤訊息: " + msg);
					return false;
				}

                var mode = ($("#HGuid").val() == "") ? "new" : "mod";

                $.ajax({
			    	type: "POST",
			    	async: false, //在沒有返回值之前,不會執行下一步動作
			    	url: "../Handler/AddWeek.aspx",
			    	data: {
			    		mode: mode,
			    		guid: $("#HGuid").val(),
			    		nb1: $("#txt_no1").val(),
			    		nb2: $("#txt_no2").val(),
			    		nb3: $("#txt_no3").val(),
			    		content: $("#txt_content").val(),
			    		time: $("#txt_time").val(),
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
                            location.href = "WeekSearch.aspx";
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
			        	url: "../Handler/DelWeek.aspx",
			        	data: {
			        		guid: $(this).attr("aid"),
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
                                location.href = "WeekSearch.aspx";
			        		}
			        	}
			        });
                }
            });

        }); // end js

        //轉換日期 ex: 1100505 => 05.05(二)
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
            var msg = '';
			if (parseInt($("#txt_starttime").val()) > parseInt($("#txt_endtime").val()))
                msg += "【起始日期】不可大於【結束日期】 ";
            if ($("#txt_starttime").val() != '' && $("#txt_endtime").val() == '')
                msg += "請選擇【結束日期】 ";
            if ($("#txt_starttime").val() == '' && $("#txt_endtime").val() != '')
                msg += "請選擇【起始日期】 ";

			if (msg != "") {
				alert("錯誤訊息: " + msg);
				return false;
			}

			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetWeekSearch.aspx",
                data: {
                    starttime: $("#txt_starttime").val(),
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
                        $("#tablist tbody").empty();
                        var tabstr = '';
                        if ($(data).find("data_item").length > 0) {
							$(data).find("data_item").each(function (i) {
                                tabstr += '<tr>';
                                tabstr += '<td align="center">';
                                tabstr += '<span name="sn_' + $(this).children("guid").text().trim() + '">' + $(this).children("工作項次").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td align="center">';                                
                                tabstr += '<span name="tm_' + $(this).children("guid").text().trim() + '">' + getDate($(this).children("預定日期").text().trim()) + '</span>';
                                tabstr += '<span style="display:none" name="tmd_' + $(this).children("guid").text().trim() + '">' + $(this).children("預定日期").text().trim() + '</span>';
                                tabstr += '</td>';
                                tabstr += '<td>';
                                tabstr += '<span name="cn_' + $(this).children("guid").text().trim() + '">' + $(this).children("執行內容").text().trim() + '</span>'
                                tabstr += '</td>';
                                tabstr += '<td align="center" class="font-normal">';
                                tabstr += '<a href="javascript:void(0);" name="delbtn" aid="' + $(this).children("guid").text().trim() + '">刪除</a> ';
                                tabstr += '<a href="javascript:void(0);" name="editbtn" class="colorboxM" title="編輯工作項次" aid="' + $(this).children("guid").text().trim() + '">編輯</a>';
                                tabstr += '</td>';
								tabstr += '</tr>';
                            });
                        }
                        else
							tabstr += '<tr><td colspan="4">查詢無資料</td></tr>';
						$("#tablist tbody").append(tabstr);
                        $("#tablist").show();
					}
				}
			});
        }

        function doOpenDialog() {
            var WinHeight = $("html").height();
            var ColHeight = WinHeight * 0.6;
            $.colorbox({title: "編輯工作項次", inline: true, href: "#workitem", width: "100%", maxWidth: "800", maxHeight: ColHeight, opacity: 0.5 });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div id="ContentWrapper">
        <input type="hidden" id="HGuid" />
        <div class="container margin15T">
            <div class="padding10ALL">
                <div class="filetitlewrapper">
                    <span class="filetitle font-size7">週報查詢</span>
                </div>

                <div class="BoxBgWa BoxRadiusA BoxBorderSa padding10ALL margin10T">
                    <div class="OchiTrasTable width100 font-size3 TitleLength05">
                        <div class="OchiRow">
                            <div class="OchiHalf">
                                <div class="OchiCell OchiTitle TitleSetWidth">起始日期</div>
                                <div class="OchiCell width100">
                                    <input id="txt_starttime" type="text" class="inputex pickDate width80" disabled>
                                </div>
                            </div><!-- OchiHalf -->
                            <div class="OchiHalf">
                                <div class="OchiCell OchiTitle TitleSetWidth">結束日期</div>
                                <div class="OchiCell width100">
                                    <input id="txt_endtime" type="text" class="inputex pickDate width80" disabled>
                                </div>
                            </div><!-- OchiHalf -->
                        </div><!-- OchiRow -->
                    </div><!-- OchiTrasTable -->

                    <div class="textright margin10T"><input id="querybtn" type="button" class="genbtn" value="查詢"> </div>
                </div>


                <div class="twocol margin10T">
                    <div class="left font-size5 "><i class="fa fa-chevron-circle-right IconCa" aria-hidden="true"></i> 查詢結果</div>
                    <div class="right font-normal font-size3">
                    </div>
                </div>

                <div class="stripeMeCS font-size3 margin10T">
                    <table id="tablist" width="100%" border="0" cellspacing="0" cellpadding="0">
                        <thead>
                        <tr>
                            <th nowrap="nowrap" width="100">工作項次</th>
                            <th nowrap="nowrap" width="100">預定日期</th>
                            <th nowrap="nowrap">預定完成執行內容</th>
                            <th nowrap="nowrap" width="100">功能</th>
                        </tr>
                        </thead>
                        <tbody></tbody>
                    </table>
                </div><!-- stripeMe -->


            </div>
        </div><!-- container -->
    </div><!-- ContentWrapper -->

 <!-- colorbox -->
<div style="display:none;">
    <div id="workitem">
        <div class="margin35T padding5RL">
            <div class="OchiTrasTable width100 TitleLength08 font-size3">
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">工作項次</div>
                    <div class="OchiCell width100">
                        <input id="txt_no1" type="number" min="1" max="9" class="inputex width10">﹒
                        <input id="txt_no2" type="number" min="1" max="9" class="inputex width10">﹒
                        <input id="txt_no3" type="number" min="0" max="9" class="inputex width10">
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">預定日期</div>
                    <div class="OchiCell width100">
                        <input id="txt_time" type="text" class="inputex pickDate width30" disabled> 
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">預定完成執行內容</div>
                    <div class="OchiCell width100">
                        <textarea id="txt_content" rows="5" cols="" class="inputex width100"></textarea>
                    </div>
                </div><!-- OchiRow -->
            </div><!-- OchiTrasTable -->
        </div>

        <div class="twocol margin10T">
            <div class="right">
                <a href="#" class="genbtn closecolorbox">取消</a>
                <a id="subbtn" href="#" class="genbtn">儲存</a>
            </div>
        </div>
        <br /><br />
    </div>
</div>

</asp:Content>

