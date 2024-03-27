<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OilOnlineEvaluation.aspx.cs" Inherits="WebPage_OilOnlineEvaluation" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=11; IE=10; IE=9; IE=8" />
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="keywords" content="關鍵字內容" />
    <meta name="description" content="描述" /><!--告訴搜尋引擎這篇網頁的內容或摘要。--> 
    <meta name="generator" content="Notepad" /><!--告訴搜尋引擎這篇網頁是用什麼軟體製作的。--> 
    <meta name="author" content="工研院 資訊處" /><!--告訴搜尋引擎這篇網頁是由誰製作的。-->
    <meta name="copyright" content="本網頁著作權所有" /><!--告訴搜尋引擎這篇網頁是...... -->
    <meta name="revisit-after" content="3 days" /><!--告訴搜尋引擎3天之後再來一次這篇網頁，也許要重新登錄。-->
    <title>石油業輸儲設備查核及檢測資訊系統</title>
    <!--#include file="Head_Include.html"-->
    <script type="text/javascript">
        $(document).ready(function () {
            getYearList();
            $("#sellist").val(getTaiwanDate());
            getData(getTaiwanDate());
            getZipbtn();
            $("#alertText").hide();
            $("#alertText2").hide();
            $("#alertText3").hide();
            $("#alertText4").hide();
            $("#alertText5").hide();
            $("#filediv").hide();
            $("#filediv2").hide();
            $("#filediv3").hide();
            $("#filediv4").hide();
            $("#filediv5").hide();

            //選擇年份
            $(document).on("change", "#sellist", function () {
                getData($("#sellist option:selected").val());
                $("a[name='zipbtn']").attr('href', '../DOWNLOAD.aspx?isZip=Y&category=Oil&type=online&details=1&year=' + $("#sellist option:selected").val() + '&cid=' + $.getQueryString("cp"));
                $("a[name='zipbtn2']").attr('href', '../DOWNLOAD.aspx?isZip=Y&category=Oil&type=online&details=2&year=' + $("#sellist option:selected").val() + '&cid=' + $.getQueryString("cp"));
                $("a[name='zipbtn3']").attr('href', '../DOWNLOAD.aspx?isZip=Y&category=Oil&type=online&details=3&year=' + $("#sellist option:selected").val() + '&cid=' + $.getQueryString("cp"));
                $("a[name='zipbtn4']").attr('href', '../DOWNLOAD.aspx?isZip=Y&category=Oil&type=online&details=4&year=' + $("#sellist option:selected").val() + '&cid=' + $.getQueryString("cp"));
                $("a[name='zipbtn5']").attr('href', '../DOWNLOAD.aspx?isZip=Y&category=Oil&type=online&details=5&year=' + $("#sellist option:selected").val() + '&cid=' + $.getQueryString("cp"));
            });

            $(document).on("click", "#filebtn", function () {
                $("#filebtn").hide();
                $("#filediv").show();
            });

            $(document).on("click", "#cancelbtn", function () {
                getData(getTaiwanDate());
                $("#filediv").hide();
                $("#filebtn").show();
            });

            $(document).on("click", "#filebtn2", function () {
                $("#filebtn2").hide();
                $("#filediv2").show();
            });

            $(document).on("click", "#cancelbtn2", function () {
                getData(getTaiwanDate());
                $("#filediv2").hide();
                $("#filebtn2").show();
            });

            $(document).on("click", "#filebtn3", function () {
                $("#filebtn3").hide();
                $("#filediv3").show();
            });

            $(document).on("click", "#cancelbtn3", function () {
                getData(getTaiwanDate());
                $("#filediv3").hide();
                $("#filebtn3").show();
            });

            $(document).on("click", "#filebtn4", function () {
                $("#filebtn4").hide();
                $("#filediv4").show();
            });

            $(document).on("click", "#cancelbtn4", function () {
                getData(getTaiwanDate());
                $("#filediv4").hide();
                $("#filebtn4").show();
            });

            $(document).on("click", "#filebtn5", function () {
                $("#filebtn5").hide();
                $("#filediv5").show();
            });

            $(document).on("click", "#cancelbtn5", function () {
                getData(getTaiwanDate());
                $("#filediv5").hide();
                $("#filebtn5").show();
            });

            $(document).on("click", "input[name='delbtn']", function () {
                var isDel = confirm("確定刪除檔案嗎?");
                if (isDel) {
                    $.ajax({
				        type: "POST",
				        async: false, //在沒有返回值之前,不會執行下一步動作
				        url: "../Handler/DelOilOnlineEvaluation.aspx",
                        data: {
                            rid : $(this).attr("aid"),
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
                                getData(getTaiwanDate());
				        	}
				        }
			        });
                }                
            });

            $(document).on("click", "#subbtn", function () {
                var msg = '';

				if ($("#fileUpload").val() == "")
                    msg += "請先選擇檔案再上傳";
                if (msg != "") {
                    alert(msg);
					return false;
				}

                // Get form
				var form = $('#form1')[0];

				// Create an FormData object 
				var data = new FormData(form);

                var curFiles = [];
                var files = this.files;

                Array.prototype.push.apply(curFiles, files);

				// If you want to add an extra field for the FormData
                data.append("cpid", $.getQueryString("cp"));
                data.append("category", "oil");
                data.append("year", getTaiwanDate());
                data.append("type", "online");
                data.append("details", "1");
                for (var i = 0, j = curFiles.length; i < j; i) {
                    data.append("myFile[]", curFiles[i]);
                }

                //$.each($("#fileUpload")[0].files, function (i, file) {
                //    data.append('file', file);
                //});

                $.ajax({
				    type: "POST",
				    async: true, //在沒有返回值之前,不會執行下一步動作
				    url: "../Handler/AddDownload.aspx",
				    data: data,
					processData: false,
					contentType: false,
					cache: false,
				    error: function (xhr) {
				    	alert("Error: " + xhr.status);
				    	console.log(xhr.responseText);
                    },
                    beforeSend: function () {
                        $("#alertText").show();
                        $("#subbtn").prop("disabled", true);
                    },
                    complete: function () {
                        $("#alertText").hide();
                        $("#subbtn").prop("disabled", false);
                    },
				    success: function (data) {
				    	if ($(data).find("Error").length > 0) {
				    		alert($(data).find("Error").attr("Message"));
				    	}
                        else {
                            alert($("Response", data).text());
                            getData(getTaiwanDate());
                            $("#filediv").hide();
                            $("#filebtn").show();
                            $("#fileUpload").val("");
				    	}
				    }
			    });
            });

            $(document).on("click", "#subbtn2", function () {
                var msg = '';

				if ($("#fileUpload2").val() == "")
                    msg += "請先選擇檔案再上傳";
                if (msg != "") {
                    alert(msg);
					return false;
				}

                // Get form
				var form = $('#form1')[0];

				// Create an FormData object 
                var data = new FormData(form);

                var curFiles = [];
                var files = this.files;

                Array.prototype.push.apply(curFiles, files);

				// If you want to add an extra field for the FormData
                data.append("cpid", $.getQueryString("cp"));
                data.append("category", "oil");
                data.append("year", getTaiwanDate());
                data.append("type", "online");
                data.append("details", "2");
                for (var i = 0, j = curFiles.length; i < j; i) {
                    data.append("myFile2[]", curFiles[i]);
                }

                $.ajax({
				    type: "POST",
				    async: true, //在沒有返回值之前,不會執行下一步動作
				    url: "../Handler/AddDownload.aspx",
				    data: data,
					processData: false,
					contentType: false,
					cache: false,
				    error: function (xhr) {
				    	alert("Error: " + xhr.status);
				    	console.log(xhr.responseText);
                    },
                    beforeSend: function () {
                        $("#alertText2").show();
                        $("#subbtn2").prop("disabled", true);
                    },
                    complete: function () {
                        $("#alertText2").hide();
                        $("#subbtn2").prop("disabled", false);
                    },
				    success: function (data) {
				    	if ($(data).find("Error").length > 0) {
				    		alert($(data).find("Error").attr("Message"));
				    	}
                        else {
                            alert($("Response", data).text());
                            getData(getTaiwanDate());
                            $("#filediv2").hide();
                            $("#filebtn2").show();
                            $("#fileUpload2").val("");
				    	}
				    }
			    });
            });

            $(document).on("click", "#subbtn3", function () {
                var msg = '';

				if ($("#fileUpload3").val() == "")
                    msg += "請先選擇檔案再上傳";
                if (msg != "") {
                    alert(msg);
					return false;
				}

                // Get form
				var form = $('#form1')[0];

				// Create an FormData object 
                var data = new FormData(form);

                var curFiles = [];
                var files = this.files;

                Array.prototype.push.apply(curFiles, files);

				// If you want to add an extra field for the FormData
                data.append("cpid", $.getQueryString("cp"));
                data.append("category", "oil");
                data.append("year", getTaiwanDate());
                data.append("type", "online");
                data.append("details", "3");
                for (var i = 0, j = curFiles.length; i < j; i) {
                    data.append("myFile3[]", curFiles[i]);
                }

                $.ajax({
                    type: "POST",
                    async: true, //在沒有返回值之前,不會執行下一步動作
				    url: "../Handler/AddDownload.aspx",
				    data: data,
					processData: false,
					contentType: false,
					cache: false,
				    error: function (xhr) {
				    	alert("Error: " + xhr.status);
				    	console.log(xhr.responseText);
                    },
                    beforeSend: function () {
                        $("#alertText3").show();
                        $("#subbtn3").prop("disabled", true);
                    },
                    complete: function () {
                        $("#alertText3").hide();
                        $("#subbtn3").prop("disabled", false);
                    },
				    success: function (data) {
				    	if ($(data).find("Error").length > 0) {
				    		alert($(data).find("Error").attr("Message"));
				    	}
                        else {
                            alert($("Response", data).text());
                            getData(getTaiwanDate());
                            $("#filediv3").hide();
                            $("#filebtn3").show();
                            $("#fileUpload3").val("");
				    	}
				    }
			    });
            });

            $(document).on("click", "#subbtn4", function () {
                var msg = '';

				if ($("#fileUpload4").val() == "")
                    msg += "請先選擇檔案再上傳";
                if (msg != "") {
                    alert(msg);
					return false;
				}

                // Get form
				var form = $('#form1')[0];

				// Create an FormData object 
                var data = new FormData(form);

                var curFiles = [];
                var files = this.files;

                Array.prototype.push.apply(curFiles, files);

				// If you want to add an extra field for the FormData
                data.append("cpid", $.getQueryString("cp"));
                data.append("category", "oil");
                data.append("year", getTaiwanDate());
                data.append("type", "online");
                data.append("details", "4");
                for (var i = 0, j = curFiles.length; i < j; i) {
                    data.append("myFile4[]", curFiles[i]);
                }

                $.ajax({
				    type: "POST",
				    async: true, //在沒有返回值之前,不會執行下一步動作
				    url: "../Handler/AddDownload.aspx",
				    data: data,
					processData: false,
					contentType: false,
					cache: false,
				    error: function (xhr) {
				    	alert("Error: " + xhr.status);
				    	console.log(xhr.responseText);
                    },
                    beforeSend: function () {
                        $("#alertText4").show();
                        $("#subbtn4").prop("disabled", true);
                    },
                    complete: function () {
                        $("#alertText4").hide();
                        $("#subbtn4").prop("disabled", false);
                    },
				    success: function (data) {
				    	if ($(data).find("Error").length > 0) {
				    		alert($(data).find("Error").attr("Message"));
				    	}
                        else {
                            alert($("Response", data).text());
                            getData(getTaiwanDate());
                            $("#filediv4").hide();
                            $("#filebtn4").show();
                            $("#fileUpload4").val("");
				    	}
				    }
			    });
            });

            $(document).on("click", "#subbtn5", function () {
                var msg = '';

				if ($("#fileUpload5").val() == "")
                    msg += "請先選擇檔案再上傳";
                if (msg != "") {
                    alert(msg);
					return false;
				}

                // Get form
				var form = $('#form1')[0];

				// Create an FormData object 
                var data = new FormData(form);

                var curFiles = [];
                var files = this.files;

                Array.prototype.push.apply(curFiles, files);

				// If you want to add an extra field for the FormData
                data.append("cpid", $.getQueryString("cp"));
                data.append("category", "oil");
                data.append("year", getTaiwanDate());
                data.append("type", "online");
                data.append("details", "5");
                for (var i = 0, j = curFiles.length; i < j; i) {
                    data.append("myFile[]", curFiles[i]);
                }

                $.ajax({
				    type: "POST",
				    async: true, //在沒有返回值之前,不會執行下一步動作
				    url: "../Handler/AddDownload.aspx",
				    data: data,
					processData: false,
					contentType: false,
					cache: false,
				    error: function (xhr) {
				    	alert("Error: " + xhr.status);
				    	console.log(xhr.responseText);
                    },
                    beforeSend: function () {
                        $("#alertText5").show();
                        $("#subbtn5").prop("disabled", true);
                    },
                    complete: function () {
                        $("#alertText5").hide();
                        $("#subbtn5").prop("disabled", false);
                    },
				    success: function (data) {
				    	if ($(data).find("Error").length > 0) {
				    		alert($(data).find("Error").attr("Message"));
				    	}
                        else {
                            alert($("Response", data).text());
                            getData(getTaiwanDate());
                            $("#filediv5").hide();
                            $("#filebtn5").show();
                            $("#fileUpload5").val("");
				    	}
				    }
			    });
            });
        }); // end js

        //新增壓縮檔案按鈕
        function getZipbtn() {
            $("#zip1").empty();
            $("#zip2").empty();
            $("#zip3").empty();
            $("#zip4").empty();
            $("#zip5").empty();

            $("#zip1").append('&ensp;<a name="zipbtn" class="genbtn" title="管線管理壓縮檔" style="padding:3px 15px;*padding:5px;_padding:5px;" href="../DOWNLOAD.aspx?isZip=Y&category=Oil&type=online&details=1&year=' + $("#sellist option:selected").val() + '&cid=' + $.getQueryString("cp") + '">全部壓縮下載</a>');
            $("#zip2").append('&ensp;<a name="zipbtn" class="genbtn" title="儲槽管理壓縮檔" style="padding:3px 15px;*padding:5px;_padding:5px;" href="../DOWNLOAD.aspx?isZip=Y&category=Oil&type=online&details=2&year=' + $("#sellist option:selected").val() + '&cid=' + $.getQueryString("cp") + '">全部壓縮下載</a>');
            $("#zip3").append('&ensp;<a name="zipbtn" class="genbtn" title="災害防救壓縮檔" style="padding:3px 15px;*padding:5px;_padding:5px;" href="../DOWNLOAD.aspx?isZip=Y&category=Oil&type=online&details=3&year=' + $("#sellist option:selected").val() + '&cid=' + $.getQueryString("cp") + '">全部壓縮下載</a>');
            $("#zip4").append('&ensp;<a name="zipbtn" class="genbtn" title="關鍵基礎設施壓縮檔" style="padding:3px 15px;*padding:5px;_padding:5px;" href="../DOWNLOAD.aspx?isZip=Y&category=Oil&type=online&details=4&year=' + $("#sellist option:selected").val() + '&cid=' + $.getQueryString("cp") + '">全部壓縮下載</a>');
            $("#zip5").append('&ensp;<a name="zipbtn" class="genbtn" title="法規壓縮檔" style="padding:3px 15px;*padding:5px;_padding:5px;" href="../DOWNLOAD.aspx?isZip=Y&category=Oil&type=online&details=5&year=' + $("#sellist option:selected").val() + '&cid=' + $.getQueryString("cp") + '">全部壓縮下載</a>');
        }

		function getData(year) {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../Handler/GetOilOnlineEvaluation.aspx",
                data: {
                    year: year,
					cpid: $.getQueryString("cp")
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
                                tabstr += '<td nowrap="nowrap" align="center">';
                                tabstr += '<a href="../DOWNLOAD.aspx?category=Oil&type=online&details=1&rid=' + $(this).children("guid").text().trim() + '"';
                                tabstr += '>'+ $(this).children("檔案名稱").text().trim() + '</a>';
                                tabstr += '</td>';
                                tabstr += '<td nowrap="nowrap" align="center">' + $(this).children("上傳日期").text().trim() + '</td>';
                                tabstr += '<td name="ftd" nowrap="nowrap" align="center">';
                                tabstr += '<input type="button" value="刪除" class="genbtn" name="delbtn" aid="' + $(this).children("guid").text().trim() + '" />';
                                tabstr += '</td>';
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="3">查詢無資料</td></tr>';
                        $("#tablist tbody").append(tabstr); 

                        $("#tablist2 tbody").empty();
						var tabstr2 = '';
						if ($(data).find("data_item2").length > 0) {
							$(data).find("data_item2").each(function (i) {
								tabstr2 += '<tr>';
                                tabstr2 += '<td nowrap="nowrap" align="center">';
                                tabstr2 += '<a href="../DOWNLOAD.aspx?category=Oil&type=online&details=2&rid=' + $(this).children("guid").text().trim() + '"';
                                tabstr2 += '>'+ $(this).children("檔案名稱").text().trim() + '</a>';
                                tabstr2 += '</td>';
                                tabstr2 += '<td nowrap="nowrap" align="center">' + $(this).children("上傳日期").text().trim() + '</td>';
                                tabstr2 += '<td name="ftd" nowrap="nowrap" align="center">';
                                tabstr2 += '<input type="button" value="刪除" class="genbtn" name="delbtn" aid="' + $(this).children("guid").text().trim() + '" />';
                                tabstr2 += '</td>';
								tabstr2 += '</tr>';
							});
						}
						else
							tabstr2 += '<tr><td colspan="3">查詢無資料</td></tr>';
                        $("#tablist2 tbody").append(tabstr2);

                        $("#tablist3 tbody").empty();
						var tabstr3 = '';
						if ($(data).find("data_item3").length > 0) {
							$(data).find("data_item3").each(function (i) {
								tabstr3 += '<tr>';
                                tabstr3 += '<td nowrap="nowrap" align="center">';
                                tabstr3 += '<a href="../DOWNLOAD.aspx?category=Oil&type=online&details=3&rid=' + $(this).children("guid").text().trim() + '"';
                                tabstr3 += '>'+ $(this).children("檔案名稱").text().trim() + '</a>';
                                tabstr3 += '</td>';
                                tabstr3 += '<td nowrap="nowrap" align="center">' + $(this).children("上傳日期").text().trim() + '</td>';
                                tabstr3 += '<td name="ftd" nowrap="nowrap" align="center">';
                                tabstr3 += '<input type="button" value="刪除" class="genbtn" name="delbtn" aid="' + $(this).children("guid").text().trim() + '" />';
                                tabstr3 += '</td>';
								tabstr3 += '</tr>';
							});
						}
						else
							tabstr3 += '<tr><td colspan="3">查詢無資料</td></tr>';
                        $("#tablist3 tbody").append(tabstr3);

                        $("#tablist5 tbody").empty();
						var tabstr5 = '';
						if ($(data).find("data_item5").length > 0) {
							$(data).find("data_item5").each(function (i) {
								tabstr5 += '<tr>';
                                tabstr5 += '<td nowrap="nowrap" align="center">';
                                tabstr5 += '<a href="../DOWNLOAD.aspx?category=Oil&type=online&details=5&rid=' + $(this).children("guid").text().trim() + '"';
                                tabstr5 += '>'+ $(this).children("檔案名稱").text().trim() + '</a>';
                                tabstr5 += '</td>';
                                tabstr5 += '<td nowrap="nowrap" align="center">' + $(this).children("上傳日期").text().trim() + '</td>';
                                tabstr5 += '<td name="ftd" nowrap="nowrap" align="center">';
                                tabstr5 += '<input type="button" value="刪除" class="genbtn" name="delbtn" aid="' + $(this).children("guid").text().trim() + '" />';
                                tabstr5 += '</td>';
								tabstr5 += '</tr>';
							});
						}
						else
							tabstr5 += '<tr><td colspan="3">查詢無資料</td></tr>';
                        $("#tablist5 tbody").append(tabstr5);

                        $("#tablist4 tbody").empty();
						var tabstr4 = '';
						if ($(data).find("data_item4").length > 0) {
							$(data).find("data_item4").each(function (i) {
								tabstr4 += '<tr>';
                                tabstr4 += '<td nowrap="nowrap" align="center">';
                                tabstr4 += '<a href="../DOWNLOAD.aspx?category=Oil&type=online&details=4&rid=' + $(this).children("guid").text().trim() + '"';
                                tabstr4 += '>'+ $(this).children("檔案名稱").text().trim() + '</a>';
                                tabstr4 += '</td>';
                                tabstr4 += '<td nowrap="nowrap" align="center">' + $(this).children("上傳日期").text().trim() + '</td>';
                                tabstr4 += '<td name="ftd" nowrap="nowrap" align="center">';
                                tabstr4 += '<input type="button" value="刪除" class="genbtn" name="delbtn" aid="' + $(this).children("guid").text().trim() + '" />';
                                tabstr4 += '</td>';
								tabstr4 += '</tr>';
							});
						}
						else
							tabstr4 += '<tr><td colspan="3">查詢無資料</td></tr>';
                        $("#tablist4 tbody").append(tabstr4);

                        //確認權限&按鈕顯示或隱藏
                        if ($("#sellist").val() != getTaiwanDate()) {
                            $("#fileall").hide();
                            $("#thFunc").hide();
                            $("#fileall2").hide();
                            $("#thFunc2").hide();
                            $("#fileall3").hide();
                            $("#thFunc3").hide();
                            $("#fileall4").hide();
                            $("#thFunc4").hide();
                            $("#fileall5").hide();
                            $("#thFunc5").hide();
                            $("td[name='ftd']").hide();
                        }
                        else {
                            if ($("#Competence").val() == "01" || $("#Competence").val() == "04" || $("#Competence").val() == "05") {
                                $("#fileall").hide();
                                $("#thFunc").hide();
                                $("#fileall2").hide();
                                $("#thFunc2").hide();
                                $("#fileall3").hide();
                                $("#thFunc3").hide();
                                $("#fileall4").hide();
                                $("#thFunc4").hide();
                                $("#fileall5").hide();
                                $("#thFunc5").hide();
                                $("td[name='ftd']").hide();
                            }
                            else {
                                $("#fileall").show();
                                $("#thFunc").show();
                                $("#fileall2").show();
                                $("#thFunc2").show();
                                $("#fileall3").show();
                                $("#thFunc3").show();
                                $("#fileall4").show();
                                $("#thFunc4").show();
                                $("#fileall5").show();
                                $("#thFunc5").show();
                                $("td[name='ftd']").show();
                            }
                        }

                        getConfirmedStatus();
					}
				}
			});
        }

        //取得民國年份之下拉選單
        function getYearList() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetOilOnlineEvaluation.aspx",
                data: {
                    cpid: $.getQueryString("cp"),
                    year: getTaiwanDate(),
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
                        if ($(data).find("data_item6").length > 0) {
                            $(data).find("data_item6").each(function (i) {
                                ddlstr += '<option value="' + $(this).children("年度").text().trim() + '">' + $(this).children("年度").text().trim() + '</option>'
                            });
                        }
                        else {
                            ddlstr += '<option>請選擇</option>'
                        }
                        $("#sellist").append(ddlstr);
                    }
                }
            });
        }

        //確認資料是否完成
        function getConfirmedStatus() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetCompanyName.aspx",
                data: {
                    type: "Oil",
                    cpid: $.getQueryString("cp"),
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
                                var dataConfirm = $(this).children("資料是否確認").text().trim();
                                if ($("#Competence").val() != '03') {
                                    if (dataConfirm == "是") {
                                        $("#fileall").hide();
                                        $("#thFunc").hide();
                                        $("td[name='ftd']").hide();
                                        $("#fileall2").hide();
                                        $("#thFunc2").hide();
                                        $("td[name='ftd']").hide();
                                        $("#fileall3").hide();
                                        $("#thFunc3").hide();
                                        $("td[name='ftd']").hide();
                                        $("#fileall5").hide();
                                        $("#thFunc5").hide();
                                        $("td[name='ftd']").hide();
                                        $("#fileall4").hide();
                                        $("#thFunc4").hide();
                                        $("td[name='ftd']").hide();
                                    }
                                }
                            });
                        }
                    }
                }
            });
        }

        function getTaiwanDate() {
            var nowDate = new Date();

            var nowYear = nowDate.getFullYear();
            var nowTwYear = (nowYear - 1911);

            return nowTwYear;
        }
    </script>
</head>
<body class="bgB">
<!-- 開頭用div:修正mmenu form bug -->
<div>
    <input type="hidden" id="Competence" value="<%= usercompetence %>" />
<form id="form1">
<!-- Preloader -->
<div id="preloader" >
	<div id="status" >
<div id="CSS3loading">
<!-- css3 loading -->
<div class="sk-three-bounce">
      <div class="sk-child sk-bounce1"></div>
      <div class="sk-child sk-bounce2"></div>
      <div class="sk-child sk-bounce3"></div>
</div> 
<!-- css3 loading -->
<span id="loadingword">資料讀取中，請稍待...</span> 
</div><!-- CSS3loading -->  
    </div><!-- status -->
</div><!-- preloader -->
<div class="container BoxBgWa BoxShadowD">
<div class="WrapperBody" id="WrapperBody">
        <!--#include file="OilHeader.html"-->

        <div id="ContentWrapper">
            <div class="container margin15T"><div class="padding10ALL">
                    <div class="filetitlewrapper"><!--#include file="OilBreadTitle.html"--></div>

                    <div class="row margin20T">
                        <div class="col-lg-3 col-md-4 col-sm-5">
                            <div id="navmenuV"><!--#include file="OilLeftMenu.html"--></div>
                        </div>
                        <div class="col-lg-9 col-md-8 col-sm-7">
                            <div class="twocol">
                                <div class="left font-size5 "><i class="fa fa-chevron-circle-right IconCa" aria-hidden="true"></i> 
                                    <select id="sellist" class="inputex">
                                    </select> 年
                                </div>
                                <div id="zip1" class="right"></div>
                                <div id="fileall" class="right">
                                <input type="button" title="管線管理檔案上傳" id="filebtn" name="filebtn" value="上傳檔案" class="genbtn" />
                                    <div id="filediv">
                                        <span id="alertText" class="font-size4 font-bold" style="color: red">*檔案上傳中，請稍後... </span>
                                        <input type="file" id="fileUpload" multiple="multiple" name="fileUpload" />
                                        <input type="button" id="subbtn" value="上傳" class="genbtn" />
                                        <input type="button" id="cancelbtn" value="取消" class="genbtn" />
                                    </div>                                
                                </div>
                            </div>
                            <br />
                            <div class="stripeMeB tbover">
                                <table id="tablist" width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th colspan="4">管線管理</th>
                                        </tr>
                                        <tr>
                                            <th nowrap width="50%">檔案名稱 </th>
                                            <th nowrap width="30%">上傳日期 </th>
                                            <th id="thFunc" nowrap width="10%">功能 </th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div><!-- stripeMe -->
                            <br />
                            <br />

                            <div class="twocol">
                                <div id="zip2" class="right"></div>
                                <div id="fileall2" class="right">
                                <input type="button" title="儲槽管理檔案上傳" id="filebtn2" name="filebtn" value="上傳檔案" class="genbtn" />
                                    <div id="filediv2">
                                        <span id="alertText2" class="font-size4 font-bold" style="color: red">*檔案上傳中，請稍後... </span>
                                        <input type="file" id="fileUpload2" multiple="multiple" name="fileUpload" />
                                        <input type="button" id="subbtn2" value="上傳" class="genbtn" />
                                        <input type="button" id="cancelbtn2" value="取消" class="genbtn" />
                                    </div>                                
                                </div>
                            </div>
                            <br />
                            <div class="stripeMeB tbover">
                                <table id="tablist2" width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th colspan="4">儲槽管理</th>
                                        </tr>
                                        <tr>
                                            <th nowrap width="50%">檔案名稱 </th>
                                            <th nowrap width="30%">上傳日期 </th>
                                            <th id="thFunc2" nowrap width="10%">功能 </th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div><!-- stripeMe -->
                            <br />
                            <br />

                            <div class="twocol">
                                <div id="zip3" class="right"></div>
                                <div id="fileall3" class="right">
                                <input type="button" title="災害防救檔案上傳" id="filebtn3" name="filebtn" value="上傳檔案" class="genbtn" />
                                    <div id="filediv3">
                                        <span id="alertText3" class="font-size4 font-bold" style="color: red">*檔案上傳中，請稍後... </span>
                                        <input type="file" id="fileUpload3" multiple="multiple" name="fileUpload" />
                                        <input type="button" id="subbtn3" value="上傳" class="genbtn" />
                                        <input type="button" id="cancelbtn3" value="取消" class="genbtn" />
                                    </div>                                
                                </div>
                            </div>
                            <br />
                            <div class="stripeMeB tbover">
                                <table id="tablist3" width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th colspan="4">災害防救</th>
                                        </tr>
                                        <tr>
                                            <th nowrap width="50%">檔案名稱 </th>
                                            <th nowrap width="30%">上傳日期 </th>
                                            <th id="thFunc3" nowrap width="10%">功能 </th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div><!-- stripeMe -->
                            <br />
                            <br />

                            <div class="twocol">
                                <div id="zip5" class="right"></div>
                                <div id="fileall5" class="right">
                                    <input type="button" title="法規檔案上傳" id="filebtn5" name="filebtn" value="上傳檔案" class="genbtn" />
                                    <div id="filediv5">
                                        <span id="alertText5" class="font-size4 font-bold" style="color: red">*檔案上傳中，請稍後... </span>
                                        <input type="file" id="fileUpload5" multiple="multiple" name="fileUpload" />
                                        <input type="button" id="subbtn5" value="上傳" class="genbtn" />
                                        <input type="button" id="cancelbtn5" value="取消" class="genbtn" />
                                    </div>                                
                                </div>
                            </div>
                            <br />
                            <div class="stripeMeB tbover">
                                <table id="tablist5" width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th colspan="4">法規</th>
                                        </tr>
                                        <tr>
                                            <th nowrap width="50%">檔案名稱 </th>
                                            <th nowrap width="30%">上傳日期 </th>
                                            <th id="thFunc5" nowrap width="10%">功能 </th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div><!-- stripeMe -->
                            <br />
                            <br />

                            <div class="twocol">
                                <div id="zip4" class="right"></div>
                                <div id="fileall4" class="right"">
                                    <input type="button" title="關鍵基礎設施檔案上傳" id="filebtn4" name="filebtn" value="上傳檔案" class="genbtn" />
                                    <div id="filediv4">
                                        <span id="alertText4" class="font-size4 font-bold" style="color: red">*檔案上傳中，請稍後... </span>
                                        <input type="file" id="fileUpload4" multiple="multiple" name="fileUpload" />
                                        <input type="button" id="subbtn4" value="上傳" class="genbtn" />
                                        <input type="button" id="cancelbtn4" value="取消" class="genbtn" />
                                    </div>                                
                                </div>
                            </div>
                            <br />
                            <div class="stripeMeB tbover">
                                <table id="tablist4" width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th colspan="4">關鍵基礎設施</th>
                                        </tr>
                                        <tr>
                                            <th nowrap width="50%">檔案名稱 </th>
                                            <th nowrap width="30%">上傳日期 </th>
                                            <th id="thFunc4" nowrap width="10%">功能 </th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div><!-- stripeMe -->
                            <br />
                            <br />

                        </div><!-- col -->
                    </div><!-- row -->


                </div>
            </div><!-- container -->
        </div><!-- ContentWrapper -->



<div class="container-fluid">
<div class="backTop"><a href="#" class="backTotop">TOP</a></div>
</div>        
</div><!-- WrapperBody -->

        <!--#include file="Footer.html"-->

</div><!-- BoxBgWa -->
<!-- 側邊選單內容:動態複製主選單內容 -->
<div id="sidebar-wrapper">
   
</div><!-- sidebar-wrapper -->

</form>
</div>
<!-- 結尾用div:修正mmenu form bug -->

<!-- colorbox -->
<div style="display:none;">
    <div id="workitem">
        <div class="margin35T padding5RL">
            <div class="OchiTrasTable width100 TitleLength08 font-size3">
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">工作項次</div>
                    <div class="OchiCell width100">
                        <input type="number" class="inputex width10">﹒<input type="number" class="inputex width10">﹒<input type="number" class="inputex width10">
                    </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">預定日期</div>
                    <div class="OchiCell width100"><input type="text" class="inputex Jdatepicker width30"> </div>
                </div><!-- OchiRow -->
                <div class="OchiRow">
                    <div class="OchiCell OchiTitle IconCe TitleSetWidth">預定完成執行內容</div>
                    <div class="OchiCell width100"><textarea rows="5" cols="" class="inputex width100"></textarea></div>
                </div><!-- OchiRow -->
            </div><!-- OchiTrasTable -->
        </div>

        <div class="twocol margin10T">
            <div class="right">
                <a href="#" class="genbtn closecolorbox">取消</a>
                <a href="#" class="genbtn">儲存</a>
            </div>
        </div>
        <br /><br />
    </div>
</div>

<!-- 本頁面使用的JS -->
    <script type="text/javascript">
        $(document).ready(function(){
        
        });
    </script>
    <script type="text/javascript" src="../js/GenCommon.js"></script><!-- UIcolor JS -->
    <script type="text/javascript" src="../js/PageCommon.js"></script><!-- 系統共用 JS -->
    <script type="text/javascript" src="../js/MenuOil.js"></script><!-- 系統共用 JS -->
    <script type="text/javascript" src="../js/SubMenuOilA.js"></script><!-- 內頁選單 -->
    <script type="text/javascript" src="../js/autoHeight.js"></script><!-- 高度不足頁面的絕對置底footer -->
</body>
</html>


