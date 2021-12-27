<%@ Page Title="" Language="C#" MasterPageFile="~/WebPage/MasterPageWeek.master" AutoEventWireup="true" CodeFile="WeekReportList.aspx.cs" Inherits="WebPage_WeekReportList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!-- 本頁面使用的JS -->    
    <script type="text/javascript">
        $(document).ready(function () {
            getYear();

            GetList($("#sellist").val());

            // 全部展開
            $(document).on("click", "#btnallopen", function () {
                $("#tablist").treetable('expandAll');
                return false;
            });

            // 全部收合
            $(document).on("click", "#btnallclose", function () {
                $("#tablist").treetable('collapseAll');
                return false;
            });

            //選擇年份
            $(document).on("change", "#sellist", function () {
                GetList($("#sellist option:selected").val());
            });

            //編輯開窗
            $(document).on("click", "a[name='editbtn']", function () {
                $("#HGuid").val($(this).attr("aid"));
                $("#content").val($("span[name='n_" + $("#HGuid").val() + "']").html());
                doOpenDialog();
            });

            //儲存內容
            $(document).on("click", "#subbtn", function () {
                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "../Handler/AddWeekReport.aspx",
                    data: {
                        rid: $("#HGuid").val(),
                        year: $("#sellist").val(),
                        content: $("#content").val(),
                    },
                    error: function (xhr) {
                        $("#errMsg").html("Error: " + xhr.status);
                        console.log(xhr.responseText);
                    },
                    success: function (data) {
                        if ($(data).find("Error").length > 0)
                            $("#errMsg").html($(data).find("Error").attr("Message"));
                        else {
                            if ($(data).find("data_item").length > 0) {
                                $(data).find("data_item").each(function (i) {
                                    alert($("Response", data).text());
                                    $("span[name='n_" + $(this).children("guid").text().trim() + "']").html($(this).children("名稱").text().trim());  
                                    parent.$.colorbox.close();
                                });
                            }                             
                        }
                    }
                });
            });

            //詳細頁面
            $(document).on("click", "a[name='detailbtn']", function () {
                var rno = ($(this).attr("rno")).replace(/\./g, '_');
                var rid = $(this).attr("rid");
                var rpid = $(this).attr("rpid");
                var ryear = $(this).attr("ryear");
                location.href = "WeekReport_" + rno + ".aspx?rid=" + rid + "&rpid=" + rpid + "&year=" + ryear;
            });
        });

        function GetList(year) {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetWeekReportList.aspx",
                data: {
                    year: year,
                },
                error: function (xhr) {
                    $("#errMsg").html("Error: " + xhr.status);
                    console.log(xhr.responseText);
                },
                success: function (data) {
                    if ($(data).find("Error").length > 0)
                        $("#errMsg").html($(data).find("Error").attr("Message"));
                    else {
                        $("#tablist tbody").empty();
                        var tabstr = '';
                        if ($(data).find("lv1").length > 0) {
                            // get max level
                            var tCont = 1;
                            var xLv = 0;
                            do {
                                xLv++;
                                tCont++;
                            }
                            while ($(data).find("lv" + tCont).length > 0)

                            // parent list
                            for (var i = 1; i <= xLv; i++) {
                                if (i == 1) {
                                    var dataStr = '';
                                    $(data).find("lv" + i).each(function (index) {
                                        dataStr = '<tr guid="' + $(this).attr("lvGuid") + '" data-tt-id="' + $(this).attr("lvGuid") + '">';
                                        dataStr += '<td>' + $(this).attr("lvNo") + '</td>';
                                        dataStr += '<td><span name="n_' + $(this).attr("lvGuid") + '">' + $(this).attr("lvName") + '</span></td>';
                                        dataStr += '<td style="text-align:center;">';
                                        dataStr += '<a href="#checklistedit" class="colorboxM" name="editbtn" title="內容修改" aid="' + $(this).attr("lvGuid") +
                                            '"><img src="../images/icon-edit.png" /></a>';
                                        dataStr += '</td>';
                                        dataStr += '<td></td>';
                                        dataStr += '</tr>';
                                        $("#tablist tbody").append(dataStr);
                                    });
                                }
                                else
                                    ParentLevel(data, i);
                            }
                            $("#tablist").treetable({
                                expandable: true, // 展開or收合
                                column: 0
                            });
                        }
                        else {
                            tabstr += '<tr><td colspan="4">查詢無資料</td></tr>';
                            $("#tablist tbody").append(tabstr);
                            $("#tablist").treetable({
                                expandable: true, // 展開or收合
                                column: 0
                            });
                        }                        
                    }
                }
            });
        }

        // 題目父階層
        function ParentLevel(xml, lv) {
            var dataStr = '';
            var trrCss = (lv == 2) ? "" : (lv - 1);
            $(xml).find("lv" + lv).each(function (i) {
                if ($(this).attr("lvGuid") != undefined) {
                    dataStr += '<tr guid="' + $(this).attr("lvGuid") + '" data-tt-id="' + $(this).attr("lvGuid") + '" data-tt-parent-id="' + $(this).attr("pGuid") + '" class="son' + trrCss + '">';
                    dataStr += '<td>' + $(this).attr("lvNo") + '</td>';
                    dataStr += '<td><span name="n_' + $(this).attr("lvGuid") + '">' + $(this).attr("lvName") + '</span></td>';
                    dataStr += '<td style="text-align:center;">';
                    dataStr += '<a href="#checklistedit" class="colorboxM" name="editbtn" title="內容修改" aid="' + $(this).attr("lvGuid") +
                        '"><img src="../images/icon-edit.png" /></a>';                  
                    dataStr += '</td>';
                    dataStr += '<td style="text-align:center;">';
                    if ($(this).attr("Dis") == 'Y')
                        dataStr += '<a href="javascript:void(0);" target="_self" name="detailbtn" rid="' + $(this).attr("lvGuid") + '" rpid="' + $(this).attr("pGuid") +
                            '" rno="' + $(this).attr("lvNo") + '" ryear="' + $("#sellist").val() +'" ><img src="../images/icon-lookdetail.png" /></a>';                    
                    dataStr += '</td>';
                    dataStr += '</tr>';

                    //check change item
                    if ($(this).next().attr("lvGuid") == undefined) {
                        $("#tablist tbody tr[guid='" + $(this).attr("pGuid") + "']").after(dataStr);
                        dataStr = '';
                    }
                }
            });
        }

        function getYear() {
            $.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetWeekReportYears.aspx",
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

        function doOpenDialog() {
            var WinHeight = $("html").height();
            var ColHeight = WinHeight * 0.6;
            $.colorbox({ inline: true, href: "#checklistedit", width: "100%", maxWidth: "800", maxHeight: ColHeight, opacity: 0.5 });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <input type="hidden" id="HGuid" />
    <div id="ContentWrapper">
        <div class="container margin15T">
            <div class="padding10ALL">
                <div class="filetitlewrapper">
                    <span class="filetitle font-size7">計畫書填報</span>
                </div>

                <div class="twocol margin10T">
                    <div class="left font-size5 "><i class="fa fa-chevron-circle-right IconCa" aria-hidden="true"></i> 
                        <select id="sellist" class="inputex">
                        </select> 年
                    </div>
                    <div class="right font-normal font-size3">
                    </div>
                </div>

                <div class="twocol margin15T">
                    <div class="right font-normal">
                        <a href="#" id="btnallopen"><i class="fa fa-plus-square-o" aria-hidden="true"></i>&nbsp;全部展開</a>&nbsp;&nbsp;
                            <a href="#" id="btnallclose"><i class="fa fa-minus-square-o" aria-hidden="true"></i>&nbsp;全部收合</a>
                    </div>
                </div>
                <div class="stripetree font-size3 margin10T">
                    <table id="tablist" width="100%" border="0" cellspacing="0" cellpadding="0">
                        <thead>
                            <tr>
                                <th nowrap="nowrap" width="120">工作項次</th>
                                <th nowrap="nowrap">內容</th>
                                <th nowrap="nowrap" width="80">編輯</th>
                                <th nowrap="nowrap" width="80">詳細內容</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                    </table>
                </div>
                <!-- stripeMeCS -->

            </div>
        </div>
        <!-- container -->
    </div>
    <!-- ContentWrapper -->

    <!-- edit dialog -->
    <div style="display: none;">
        <div id="checklistedit">
            <div class="margin35T padding5RL">
                <div class="OchiTrasTable width100 TitleLength03 font-size3">
                    <div class="OchiRow">
                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">內容</div>
                        <div class="OchiCell width100">
                            <textarea id="content" rows="8" cols="" class="inputex width100"></textarea>
                        </div>
                    </div><!-- OchiRow -->
                </div> 
                <!-- OchiTrasTable -->
            </div>

            <div class="twocol margin10T">
                <div class="right">
                    <a href="javascript:void(0);" id="canbtn" class="genbtn closecolorbox">取消</a>
                    <a href="javascript:void(0);" id="subbtn" class="genbtn">儲存</a>
                </div>
            </div>
            <br />
        </div>
    </div>
</asp:Content>

