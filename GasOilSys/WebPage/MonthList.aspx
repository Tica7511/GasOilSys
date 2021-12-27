<%@ Page Title="" Language="C#" MasterPageFile="~/WebPage/MasterPageWeek.master" AutoEventWireup="true" CodeFile="MonthList.aspx.cs" Inherits="WebPage_MonthList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            getYear();
            getData($("#sellist option:selected").val());            

            //選擇年份
            $(document).on("change", "#sellist", function () {
                getData($("#sellist option:selected").val());
            });
        });

        function getYear() {
            $.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetWeekYears.aspx",
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
                        $("#sellist").empty();
                        var ddlstr = '';
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                ddlstr += '<option value="' + $(this).children("year").text().trim() + '">' + $(this).children("year").text().trim() + '</option>'
							});
                        }
                        $("#sellist").append(ddlstr);
					}
				}
			});
        }

        function getData(year) {
            $("#tablist tbody").empty();
            var tabstr = '';

            tabstr += '<tr>';
            tabstr += '<td align="center">一月</td><td align="center" class="font-normal">';
            tabstr += '<a href="MonthView.aspx?month=01&year=' + year + '">編輯</a>'
            tabstr += '</td>';
            tabstr += '</tr>';
            tabstr += '<tr>';
            tabstr += '<td align="center">二月</td><td align="center" class="font-normal">';
            tabstr += '<a href="MonthView.aspx?month=02&year=' + year + '">編輯</a>'
            tabstr += '</td>';
            tabstr += '</tr>';
            tabstr += '<tr>';
            tabstr += '<td align="center">三月</td><td align="center" class="font-normal">';
            tabstr += '<a href="MonthView.aspx?month=03&year=' + year + '">編輯</a>'
            tabstr += '</td>';
            tabstr += '</tr>';
            tabstr += '<tr>';
            tabstr += '<td align="center">四月</td><td align="center" class="font-normal">';
            tabstr += '<a href="MonthView.aspx?month=04&year=' + year + '">編輯</a>'
            tabstr += '</td>';
            tabstr += '</tr>';
            tabstr += '<tr>';
            tabstr += '<td align="center">五月</td><td align="center" class="font-normal">';
            tabstr += '<a href="MonthView.aspx?month=05&year=' + year + '">編輯</a>'
            tabstr += '</td>';
            tabstr += '</tr>';
            tabstr += '<tr>';
            tabstr += '<td align="center">六月</td><td align="center" class="font-normal">';
            tabstr += '<a href="MonthView.aspx?month=06&year=' + year + '">編輯</a>'
            tabstr += '</td>';
            tabstr += '</tr>';
            tabstr += '<tr>';
            tabstr += '<td align="center">七月</td><td align="center" class="font-normal">';
            tabstr += '<a href="MonthView.aspx?month=07&year=' + year + '">編輯</a>'
            tabstr += '</td>';
            tabstr += '</tr>';
            tabstr += '<tr>';
            tabstr += '<td align="center">八月</td><td align="center" class="font-normal">';
            tabstr += '<a href="MonthView.aspx?month=08&year=' + year + '">編輯</a>'
            tabstr += '</td>';
            tabstr += '</tr>';
            tabstr += '<tr>';
            tabstr += '<td align="center">九月</td><td align="center" class="font-normal">';
            tabstr += '<a href="MonthView.aspx?month=09&year=' + year + '">編輯</a>'
            tabstr += '</td>';
            tabstr += '</tr>';
            tabstr += '<tr>';
            tabstr += '<td align="center">十月</td><td align="center" class="font-normal">';
            tabstr += '<a href="MonthView.aspx?month=10&year=' + year + '">編輯</a>'
            tabstr += '</td>';
            tabstr += '</tr>';
            tabstr += '<tr>';
            tabstr += '<td align="center">十一月</td><td align="center" class="font-normal">';
            tabstr += '<a href="MonthView.aspx?month=11&year=' + year + '">編輯</a>'
            tabstr += '</td>';
            tabstr += '</tr>';
            tabstr += '<tr>';
            tabstr += '<td align="center">十二月</td><td align="center" class="font-normal">';
            tabstr += '<a href="MonthView.aspx?month=12&year=' + year + '">編輯</a>'
            tabstr += '</td>';
            tabstr += '</tr>';

            $("#tablist tbody").append(tabstr);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div id="ContentWrapper">

        <div class="container margin15T">
            <div class="padding10ALL">
                <div class="filetitlewrapper">
                    <span class="filetitle font-size7">月報系統</span>
                    <span class="btnright"></span>
                </div>

                <div class="twocol margin10T">
                    <div class="left font-size5 "><i class="fa fa-chevron-circle-right IconCa" aria-hidden="true"></i> 
                        <select id="sellist" class="inputex">
                        </select> 年</div>
                    <div class="right font-normal font-size3">
                    </div>
                </div>

                <div class="stripeMeCS font-size3 margin10T">
                    <table id="tablist" width="100%" border="0" cellspacing="0" cellpadding="0">
                        <thead>
                        <tr>
                            <th nowrap="nowrap">月份</th>
                            <th nowrap="nowrap" width="100">功能</th>
                        </tr>
                        </thead>
                        <tbody></tbody>
                    </table>
                </div><!-- stripeMe -->

            </div>
        </div><!-- container -->
     </div><!-- ContentWrapper -->

</asp:Content>

