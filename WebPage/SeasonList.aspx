<%@ Page Title="" Language="C#" MasterPageFile="~/WebPage/MasterPageWeek.master" AutoEventWireup="true" CodeFile="SeasonList.aspx.cs" Inherits="WebPage_SeasonList" %>

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
                        if ($(data).find("data_item").length > 0) {
							$(data).find("data_item").each(function (i) {
                                $("#sellist").append($("<option></option>").attr("value", $(this).children("year").text().trim()).text($(this).children("year").text().trim()));
							});
                        }
					}
				}
			});
        }

        function getData(year) {
            $("#tablist tbody").empty();
            var tabstr = '';

            tabstr += '<tr>';
            tabstr += '<td align="center">第一季</td><td align="center" class="font-normal">';
            tabstr += '<a href="SeasonView.aspx?season=01&year=' + year + '">編輯</a>'
            tabstr += '</td>';
            tabstr += '</tr>';
            tabstr += '<tr>';
            tabstr += '<td align="center">第二季</td><td align="center" class="font-normal">';
            tabstr += '<a href="SeasonView.aspx?season=02&year=' + year + '">編輯</a>'
            tabstr += '</td>';
            tabstr += '</tr>';
            tabstr += '<tr>';
            tabstr += '<td align="center">第三季</td><td align="center" class="font-normal">';
            tabstr += '<a href="SeasonView.aspx?season=03&year=' + year + '">編輯</a>'
            tabstr += '</td>';
            tabstr += '</tr>';
            tabstr += '<tr>';
            tabstr += '<td align="center">第四季</td><td align="center" class="font-normal">';
            tabstr += '<a href="SeasonView.aspx?season=04&year=' + year + '">編輯</a>'
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
                    <span class="filetitle font-size7">季報系統</span>
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
                            <th nowrap="nowrap">季別</th>
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



