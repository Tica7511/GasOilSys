<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OilSelfEvaluation.aspx.cs" Inherits="WebPage_OilSelfEvaluation" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=11; IE=10; IE=9; IE=8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="keywords" content="關鍵字內容" />
    <meta name="description" content="描述" />
    <!--告訴搜尋引擎這篇網頁的內容或摘要。-->
    <meta name="generator" content="Notepad" />
    <!--告訴搜尋引擎這篇網頁是用什麼軟體製作的。-->
    <meta name="author" content="工研院 資訊處" />
    <!--告訴搜尋引擎這篇網頁是由誰製作的。-->
    <meta name="copyright" content="本網頁著作權所有" />
    <!--告訴搜尋引擎這篇網頁是...... -->
    <meta name="revisit-after" content="3 days" />
    <!--告訴搜尋引擎3天之後再來一次這篇網頁，也許要重新登錄。-->
    <title>石油業輸儲設備查核及檢測資訊系統</title>
    <!--#include file="Head_Include.html"-->
    <style>
        .warp{width:100%;white-space:normal;word-wrap:break-word;word-break:break-all;}
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#exportBtn").attr("href", "../Handler/OilEvaluationReport.aspx?cp=" + $.getQueryString("cp"));
            $("#taiwanYear").html(getTaiwanDate());

            GetList();

            $("#lbl_CompanyName").html($("#CompanyName").val());

            RemoveQuestion();

            // disabled
            switch ($("#Competence").val()) {
                case "01":
                    $(".cRadio").prop("disabled", true);
                    $("#exportBtn").hide();
                    break;
                case "02":
                    $(".mRadio").prop("disabled", true);
                    $(".mRef").prop("disabled", true);
                    $("#exportBtn").hide();
                    break;
                case "03":
                    $("#exportBtn").show();
                    break;
                default:
                    $("#subbtn").hide();
                    $("#subbtnTop").hide();                    
                    $("#alertText").hide();                    
                    $(".cRadio").prop("disabled", true);
                    $(".mRadio").prop("disabled", true);
                    $(".mRef").prop("disabled", true);
                    $("#exportBtn").hide();
                    break;
            }

            // Get Answer
            GetAns();

            //$(".mRadio[value='01']").prop("checked", true);

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

            // 送出自評表
            $(document).on("click", "#subbtn", function () {
                // Get form
                var form = $('#form1')[0];

                // Create an FormData object
                var data = new FormData(form);

                // If you want to add an extra field for the FormData
                data.append("cpid", $.getQueryString("cp"));

                $.ajax({
                    type: "POST",
                    async: true, //在沒有返回值之前,不會執行下一步動作
                    url: "../Handler/OilSaveSelfEvaluation.aspx",
                    data: data,
                    processData: false,
                    contentType: false,
                    cache: false,
                    error: function (xhr) {
                        $("#errMsg").html("Error: " + xhr.status);
                        console.log(xhr.responseText);
                    },
                    beforeSend: function () {
                        $("#subbtn").val("資料儲存中...");
                        $("#subbtn").prop("disabled", true);
                    },
                    complete: function () {
                        $("#subbtn").val("儲存");
                        $("#subbtn").prop("disabled", false);
                    },
                    success: function (data) {
                        if ($(data).find("Error").length > 0) {
                            $("#errMsg").html($(data).find("Error").attr("Message"));
                        }
                        else {
                            alert($("Response", data).text());
                            //location.href = "LessonManage.aspx";
                        }
                    }
                });
            });

            $(document).on("click", "#subbtnTop", function () {
                // Get form
                var form = $('#form1')[0];

                // Create an FormData object
                var data = new FormData(form);

                // If you want to add an extra field for the FormData
                data.append("cpid", $.getQueryString("cp"));

                $.ajax({
                    type: "POST",
                    async: true, //在沒有返回值之前,不會執行下一步動作
                    url: "../Handler/OilSaveSelfEvaluation.aspx",
                    data: data,
                    processData: false,
                    contentType: false,
                    cache: false,
                    error: function (xhr) {
                        $("#errMsg").html("Error: " + xhr.status);
                        console.log(xhr.responseText);
                    },
                    beforeSend: function () {
                        $("#subbtnTop").val("資料儲存中...");
                        $("#subbtnTop").prop("disabled", true);
                    },
                    complete: function () {
                        $("#subbtnTop").val("儲存");
                        $("#subbtnTop").prop("disabled", false);
                    },
                    success: function (data) {
                        if ($(data).find("Error").length > 0) {
                            $("#errMsg").html($(data).find("Error").attr("Message"));
                        }
                        else {
                            alert($("Response", data).text());
                            //location.href = "LessonManage.aspx";
                        }
                    }
                });
            });

            // 答案為符合時 隱藏委員意見列表按鈕
            //$(document).on("change", ".psCtrl", function () {
            //    if (this.value != "01") {
            //        $(this).closest("tr").find("span").show();
            //        $(this).closest("tr").find("input[name='psbtn']").show();
            //    }
            //    else {
            //        $(this).closest("tr").find("td:last-child span").hide();
            //        $(this).closest("tr").find("a[name='psbtn']").hide();
            //    }
            //});

            $(document).on("change", ".psCtrl", function () {
                $(this).closest("tr").find("span").show();
                //$(this).closest("tr").find("input[name='psbtn']").show();
            });

            // 說明列表開窗
            $(document).on("click", "input[name='psbtn']", function () {
                $("#qGuid").val($(this).attr("qid"));
                GetLogList();                
                //GetLogListAns();                
                doOpenDialog();
            });

            // 刪除說明
            $(document).on("click", "input[name='delbtn']", function () {
                var isDel = confirm("確定刪除意見嗎?");
                if (isDel) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../Handler/OilDelLogSefEvaluation.aspx",
                        data: {
                            cpid: $.getQueryString("cp"),
                            qguid: $("#qGuid").val(),
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
                                alert("刪除完成");
                                if ($(data).find("data_item").length > 0) {
                                    $(data).find("data_item").each(function (i) {
                                        $($("#vf_" + $("#qGuid").val())).html($(this).children("檢視文件").text().trim());
                                        $($("#vf_" + $("#qGuid").val())).data('powertip', $(this).children("檢視文件").text().trim());
                                        $($("#fv_" + $("#qGuid").val())).val($(this).children("檢視文件").text().trim());
                                        $($("#sp_" + $("#qGuid").val())).html($(this).children("委員意見").text().trim());
                                        $($("#sp_" + $("#qGuid").val())).data('powertip', $(this).children("委員意見").text().trim());
                                        $($("#ps_" + $("#qGuid").val())).val($(this).children("委員意見").text().trim());
                                    });
                                }
                                else {
                                    $($("#vf_" + $("#qGuid").val())).html('');
                                    $($("#vf_" + $("#qGuid").val())).data('powertip', '');
                                    $($("#fv_" + $("#qGuid").val())).val('');
                                    $($("#sp_" + $("#qGuid").val())).html('');
                                    $($("#sp_" + $("#qGuid").val())).data('powertip', '');
                                    $($("#ps_" + $("#qGuid").val())).val('');
                                }
                                GetLogList();
                                //GetLogListAns();
                            }
                        }
                    });
                }
            });

            // 刪除查核建議
            $(document).on("click", "input[name='delallbtn']", function () {
                var isDel = confirm("確定刪除意見嗎?");
                if (isDel) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../Handler/OilDelAllSefEvaluation.aspx",
                        data: {
                            cpid: $.getQueryString("cp"),
                            qguid: $("#qGuid").val(),
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
                                alert("刪除完成");

                                if ($(data).find("data_item").length < 1) {
                                    $("span[name='spcom_" + $("#qGuid").val() + "']").html('');
                                }

                                GetAllList();
                            }
                        }
                    });
                }
            });

            // radio button 再次點擊後取消
            $(document).on("click", "input:radio", function () {
                var domName = $(this).attr('name');

                var $radio = $(this);
                // if this was previously checked

                if ($radio.data('waschecked') == true) {
                    console.log($radio.data('waschecked') == true);
                    $radio.prop('checked', false);
                    //$("input:radio[name='radio" + domName + "']").data('waschecked',false);
                    $radio.data('waschecked', false);
                } else {
                    console.log($radio.data('waschecked') == true);
                    $radio.prop('checked', true);
                    //$("input:radio[name='radio" + domName + "']").data('waschecked',true);
                    $radio.data('waschecked', true);
                }
            });            

            // 開啟編輯說明開窗
            $(document).on("click", "input[name='editbtn']", function () {
                $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetOilLogSefEvaluationDetail.aspx",
                data: {
                      guid: $(this).attr("aid")
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
                                $("#psStr2Edit").val($(this).children("委員意見").text().trim());
                                $("#psViewFileEdit").val($(this).children("檢視文件").text().trim());
                                //$("#psIsopEdit").val($(this).children("是否列入查核意見").text().trim());
                                //if ($("#psIsopEdit").val() == "Y") {
                                //    $("#psIsopEdit").find("option[text='是']").attr("selected", true);
                                //}
                                //else {
                                //    $("#psIsopEdit").find("option[text='否']").attr("selected", true);
                                //}
                                $("#logGuid").val($(this).children("guid").text().trim());
                            });
                            doOpenDialog3();
                        }                        
                    }
                }
                });                
            });

            //取消編輯說明開窗
            $(document).on("click", "#ps_cancel3", function () {
                GetLogList();
                //GetLogListAns();
                doOpenDialog();
            });

            //儲存編輯完說明log
            $(document).on("click", "#ps_savebtn3", function () {
                var str = '確定儲存';
                var isSave = '';

                isSave = confirm(str);

                if (isSave) {                    
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../Handler/OilSaveLogSefEvaluation.aspx",
                        data: {
                            type: "edit",
                            cpid: $.getQueryString("cp"),
                            guid: $("#logGuid").val(),
                            qid: $("#qGuid").val(),
                            qOpinions: $("#psStr2Edit").val(),
                            qViewFile: $("#psViewFileEdit").val(),
                            //qIsop: $("#psIsopEdit").val(),
                            qYear: getTaiwanDate(),
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
                                var simpleStr = $("#psStr2Edit").val();
                                simpleStr = (simpleStr.length > 15) ? simpleStr.substr(0, 15) + "..." : simpleStr;
                                $($("#sp_" + $("#qGuid").val())).html(simpleStr);
                                $($("#sp_" + $("#qGuid").val())).data('powertip', $("#psStr2Edit").val());
                                var simpleView = $("#psViewFileEdit").val();
                                simpleView = (simpleView.length > 15) ? simpleView.substr(0, 15) + "..." : simpleView;
                                $($("#vf_" + $("#qGuid").val())).html(simpleView);
                                $($("#vf_" + $("#qGuid").val())).data('powertip', $("#psViewFileEdit").val());
                                $($("#ps_" + $("#qGuid").val())).val($("#psStr2Edit").val());
                                $($("#fv_" + $("#qGuid").val())).val($("#psViewFileEdit").val());

                                GetLogList();
                                //GetLogListAns();
                                doOpenDialog();
                            }
                        }
                    });                    
                }
            });

            // 開啟新增說明開窗
            $(document).on("click", "input[name='newbtn']", function () {
                $("#qAnswer").val($("input:radio[name=mg_" + $("#qGuid").val() + "]:checked").val());
                $("#psStr2").val("");
                $("#psViewFile").val("");
                doOpenDialog2();
            });

            //取消新增說明開窗
            $(document).on("click", "#ps_cancel2", function () {
                GetLogList();
                //GetLogListAns();
                doOpenDialog();
            });

            //儲存新增完說明log
            $(document).on("click", "#ps_savebtn2", function () {
                //var isNull = '';
                //var str = '';
                //var isSave = '';

                //if ($("#qAnswer").val() == '') {
                //    isNull = 'false';
                //    str = '請填寫答案再儲存!';
                //}
                //else {
                //    isNull = 'true';
                //    str = '確定儲存?'
                //}

                //isSave = confirm(str);

                //if (isSave) {
                //    if (isNull == 'true') {
                //        $.ajax({
                //            type: "POST",
                //            async: false, //在沒有返回值之前,不會執行下一步動作
                //            url: "../Handler/OilSaveLogSefEvaluation.aspx",
                //            data: {
                //                type: "add",
                //                cpid: $.getQueryString("cp"),
                //                guid: "",
                //                qid: $("#qGuid").val(),
                //                qOpinions: $("#psStr2").val(),
                //                qAnswer: $("#qAnswer").val(),
                //                qViewFile: $("#psViewFile").val(),
                //                //qIsop: $("#psIsop").val(),
                //                qYear: "110",
                //            },
                //            error: function (xhr) {
                //                alert("Error: " + xhr.status);
                //                console.log(xhr.responseText);
                //            },
                //            success: function (data) {
                //                if ($(data).find("Error").length > 0) {
                //                    alert($(data).find("Error").attr("Message"));
                //                }
                //                else {
                //                    alert($("Response", data).text());
                //                    var simpleStr = $("#psStr2").val();
                //                    simpleStr = (simpleStr.length > 15) ? simpleStr.substr(0, 15) + "..." : simpleStr;
                //                    $($("#sp_" + $("#qGuid").val())).html(simpleStr);
                //                    $($("#sp_" + $("#qGuid").val())).data('powertip', $("#psStr2").val());
                //                    var simpleView = $("#psViewFile").val();
                //                    simpleView = (simpleView.length > 15) ? simpleView.substr(0, 15) + "..." : simpleView;
                //                    $($("#vf_" + $("#qGuid").val())).html(simpleView);
                //                    $($("#vf_" + $("#qGuid").val())).data('powertip', $("#psViewFile").val());
                //                    $($("#ps_" + $("#qGuid").val())).val($("#psStr2").val());
                //                    $($("#fv_" + $("#qGuid").val())).val($("#psViewFile").val());

                //                    GetLogList();
                //                    //GetLogListAns();
                //                    doOpenDialog();
                //                }
                //            }
                //        });
                //    }
                //}

                $.ajax({
                    type: "POST",
                    async: false, //在沒有返回值之前,不會執行下一步動作
                    url: "../Handler/OilSaveLogSefEvaluation.aspx",
                    data: {
                        type: "add",
                        cpid: $.getQueryString("cp"),
                        guid: "",
                        qid: $("#qGuid").val(),
                        qOpinions: $("#psStr2").val(),
                        qAnswer: $("#qAnswer").val(),
                        qViewFile: $("#psViewFile").val(),
                        //qIsop: $("#psIsop").val(),
                        qYear: getTaiwanDate(),
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
                            var simpleStr = $("#psStr2").val();
                            simpleStr = (simpleStr.length > 15) ? simpleStr.substr(0, 15) + "..." : simpleStr;
                            $($("#sp_" + $("#qGuid").val())).html(simpleStr);
                            $($("#sp_" + $("#qGuid").val())).data('powertip', $("#psStr2").val());
                            var simpleView = $("#psViewFile").val();
                            simpleView = (simpleView.length > 15) ? simpleView.substr(0, 15) + "..." : simpleView;
                            $($("#vf_" + $("#qGuid").val())).html(simpleView);
                            $($("#vf_" + $("#qGuid").val())).data('powertip', $("#psViewFile").val());
                            $($("#ps_" + $("#qGuid").val())).val($("#psStr2").val());
                            $($("#fv_" + $("#qGuid").val())).val($("#psViewFile").val());

                            GetLogList();
                            //GetLogListAns();
                            doOpenDialog();
                        }
                    }
                });
            });

            //查核建議開窗
            $(document).on("click", "input[name='psallbtn']", function () {
                $("#qGuid").val($(this).attr("guid"));
                GetAllList();
                doOpenDialog4();
            });

            // 開啟編輯查核建議開窗
            $(document).on("click", "input[name='editallbtn']", function () {
                $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetOilAllSuggestion.aspx",
                data: {
                      guid: $(this).attr("aid")
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
                                $("#psStr2All2").val($(this).children("委員意見").text().trim());
                                //$("#psViewFileAll2").val($(this).children("檢視文件").text().trim());
                                //$("#psIsopAll2").val($(this).children("是否列入查核意見").text().trim());
                                //if ($("#psIsopAll2").val() == "Y") {
                                //    $("#psIsopAll2").find("option[text='是']").attr("selected", true);
                                //}
                                //else {
                                //    $("#psIsopAll2").find("option[text='否']").attr("selected", true);
                                //}
                                $("#logGuid").val($(this).children("guid").text().trim());
                            });
                            $("#fileUpload2").val("");
                            $("#filelist2").empty();
                            GetAllFileList();
                            getExtension();

                            doOpenDialog6();
                        }                        
                    }
                }
                });
            });

            //取消編輯查核建議開窗
            $(document).on("click", "#ps_cancelAll2", function () {
                GetAllList();
                doOpenDialog4();
            });

            //儲存編輯完查核建議
            $(document).on("click", "#ps_savebtnAll2", function () {
                var str = '確定儲存?';
                var isSave = '';

                isSave = confirm(str);

                if (isSave) {      

                    // Get form
                    var form = $('#form2')[0];

                    // Create an FormData object
                    var data = new FormData(form);

                    // If you want to add an extra field for the FormData
                    data.append("type", "edit");
                    data.append("cpid", $.getQueryString("cp"));
                    data.append("guid", $("#logGuid").val());
                    data.append("qid", $("#qGuid").val());
                    data.append("qOpinions", $("#psStr2All2").val());
                    data.append("qViewFile", "");
                    data.append("qIsop", "");
                    $.each($("#fileUpload2")[0].files, function(i, file) {
                        data.append('file2', file);
                    });

                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../Handler/OilSaveAllSefEvaluation.aspx",
                        data: data,
                        processData: false,
                        contentType: false,
                        cache: false,
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
                                //var simpleStr = $("#psStr2Edit").val();
                                //simpleStr = (simpleStr.length > 15) ? simpleStr.substr(0, 15) + "..." : simpleStr;
                                //$($("#sp_" + $("#qGuid").val())).html(simpleStr);
                                //$($("#sp_" + $("#qGuid").val())).data('powertip', $("#psStr2Edit").val());
                                //var simpleView = $("#psViewFileEdit").val();
                                //simpleView = (simpleView.length > 15) ? simpleView.substr(0, 15) + "..." : simpleView;
                                //$($("#vf_" + $("#qGuid").val())).html(simpleView);
                                //$($("#vf_" + $("#qGuid").val())).data('powertip', $("#psViewFileEdit").val());
                                //$($("#ps_" + $("#qGuid").val())).val($("#psStr2Edit").val());
                                //$($("#fv_" + $("#qGuid").val())).val($("#psViewFileEdit").val());

                                GetAllList();
                                doOpenDialog4();
                            }
                        }
                    });                    
                }
            });

            // 開啟新增查核建議開窗
            $(document).on("click", "input[name='newallbtn']", function () {
                $("#psStr2All").val("");
                $("#psViewFileAll").val("");
                $("#logGuid").val("");
                $("#fileUpload").val("");
                $("#filelist").empty();
                doOpenDialog5();
            });

            //取消新增查核建議開窗
            $(document).on("click", "#ps_cancelAll", function () {
                GetAllList();
                doOpenDialog4();
            });

            //儲存新增完查核建議
            $(document).on("click", "#ps_savebtnAll", function () {
                var str = '確定儲存?';
                var isSave = '';

                isSave = confirm(str);

                if (isSave) {

                    // Get form
                    var form = $('#form2')[0];

                    // Create an FormData object
                    var data = new FormData(form);

                    // If you want to add an extra field for the FormData
                    data.append("type", "add");
                    data.append("cpid", $.getQueryString("cp"));
                    data.append("guid", $("#logGuid").val());
                    data.append("qid", $("#qGuid").val());
                    data.append("qOpinions", $("#psStr2All").val());
                    data.append("qViewFile", "");
                    data.append("qIsop", "");
                    $.each($("#fileUpload")[0].files, function(i, file) {
                        data.append('file', file);
                    });

                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../Handler/OilSaveAllSefEvaluation.aspx",
                        data: data,
                        processData: false,
                        contentType: false,
                        cache: false,
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

                                if ($("span[name='spcom_" + $("#qGuid").val() + "']").text() == '')
                                    $("span[name='spcom_" + $("#qGuid").val() + "']").html('查核建議...');
                                //var simpleStr = $("#psStr2").val();
                                //simpleStr = (simpleStr.length > 15) ? simpleStr.substr(0, 15) + "..." : simpleStr;
                                //$($("#sp_" + $("#qGuid").val())).html(simpleStr);
                                //$($("#sp_" + $("#qGuid").val())).data('powertip', $("#psStr2").val());
                                //var simpleView = $("#psViewFile").val();
                                //simpleView = (simpleView.length > 15) ? simpleView.substr(0, 15) + "..." : simpleView;
                                //$($("#vf_" + $("#qGuid").val())).html(simpleView);
                                //$($("#vf_" + $("#qGuid").val())).data('powertip', $("#psViewFile").val());
                                //$($("#ps_" + $("#qGuid").val())).val($("#psStr2").val());
                                //$($("#fv_" + $("#qGuid").val())).val($("#psViewFile").val());

                                GetAllList();
                                doOpenDialog4();
                            }
                        }
                    });
                }
            });

            //上傳前的附件列表
            $(document).on("change", "#fileUpload", function () {
                $("#filelist").empty();
                var fp = $("#fileUpload");
                var lg = fp[0].files.length; // get length
                var items = fp[0].files;
                var fragment = "";

                if (lg > 0) {
                    for (var i = 0; i < lg; i++) {
                        var fileName = items[i].name; // get file name

                        // append li to UL tag to display File info
                        fragment += "<label>" + (i + 1) + ". " + fileName + "</label></br>";
                    }

                    $("#filelist").append(fragment);
                }
            });

            //上傳前的附件列表
            $(document).on("change", "#fileUpload2", function () {
                $("#filelist2").empty();
                var fp = $("#fileUpload2");
                var lg = fp[0].files.length; // get length
                var items = fp[0].files;
                var fragment = "";

                if (lg > 0) {
                    for (var i = 0; i < lg; i++) {
                        var fileName = items[i].name; // get file name

                        // append li to UL tag to display File info
                        fragment += "<label>" + (i + 1) + ". " + fileName + "</label></br>";
                    }

                    $("#filelist2").append(fragment);
                }
            });

            //刪除查核意見檔案
            $(document).on("click", "a[name='delbtnFile']", function () {
            var isDel = confirm("確定刪除檔案嗎?");
                if (isDel) {
                    $.ajax({
                        type: "POST",
                        async: false, //在沒有返回值之前,不會執行下一步動作
                        url: "../Handler/DelOilAllSefEvaluationFile.aspx",
                        data: {
                            cpid: $.getQueryString("cp"),
                            sn: $(this).attr("sn"),
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
                                alert("刪除完成");

                                GetAllFileList();
                                getExtension();
                            }
                        }
                    });
                }
            });

            // 儲存備註
            //$(document).on("click", "#ps_savebtn", function () {
            //	var simpleStr = $("#psStr").val();
            //	simpleStr = (simpleStr.length > 15) ? simpleStr.substr(0, 15) + "..." : simpleStr;
            //	$($("#sp_" + $("#qGuid").val())).html(simpleStr);
            //	$($("#sp_" + $("#qGuid").val())).data('powertip', $("#psStr").val());
            //	$($("#ps_" + $("#qGuid").val())).val($("#psStr").val());
            //	$.colorbox.close();
            //});

        }); // end js

        function GetLogListAns() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetOilCommitteeSuggestion.aspx",
                data: {
                    cpid: $.getQueryString("cp"),
                    qid: $("#qGuid").val(),
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
                                $("input[name='mglog_" + $(this).children("guid").text().trim() +"'][value='" + $(this).children("答案").text().trim() + "']").prop("checked", true);
                            });
                        }
                    }
                }
            });
        }

        //取得委員意見列表
        function GetLogList() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetOilCommitteeSuggestion.aspx",
                data: {
                    cpid: $.getQueryString("cp"),
                    qid: $("#qGuid").val(),
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
                        $("#tablistOpen tbody").empty();
                        var tabstr = '';
                        var isans = '';
                        var isansStr = '';
                        var isop = '';
                        var isopStr = '';
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                var ans = $(this).children("答案").text().trim();
                                //isop = $(this).children("是否列入查核意見").text().trim();
                                //if (isop != 'Y')
                                //    isopStr = '否';
                                //else
                                //    isopStr = '是';
                                tabstr += '<tr>';
                                tabstr += '<td nowrap="nowrap" style="display:none">';
                                tabstr += '<input type="hidden" aid="' + $(this).children("委員guid").text().trim() + '" />';
                                tabstr += '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("委員").text().trim() + '</td>';
                                //tabstr += '<td align="center" nowrap>';
                                //tabstr += '<div class="inlineitem textcenter"><input type="radio" name="mglog_' + $(this).children("guid").text().trim() +'" value="00" disabled /><br>0</div>&emsp;';
                                //tabstr += '<div class="inlineitem textcenter"><input type="radio" name="mglog_' + $(this).children("guid").text().trim() +'" value="01" disabled /><br>1</div>&emsp;';
                                //tabstr += '<div class="inlineitem textcenter"><input type="radio" name="mglog_' + $(this).children("guid").text().trim() +'" value="02" disabled /><br>2</div>&emsp;';
                                //tabstr += '<div class="inlineitem textcenter"><input type="radio" name="mglog_' + $(this).children("guid").text().trim() +'" value="03" disabled /><br>3</div>&emsp;';
                                //tabstr += '<div class="inlineitem textcenter"><input type="radio" name="mglog_' + $(this).children("guid").text().trim() +'" value="04" disabled /><br>4</div>&emsp;';
                                //tabstr += '<div class="inlineitem textcenter"><input type="radio" name="mglog_' + $(this).children("guid").text().trim() +'" value="05" disabled /><br>5</div>&emsp;';
                                //tabstr += '<div class="inlineitem textcenter"><input type="radio" name="mglog_' + $(this).children("guid").text().trim() +'" value="06" disabled /><br>&emsp;</div>';
                                //tabstr += '</td>';
                                tabstr += '<td width="200"><div class="warp">' + $(this).children("檢視文件").text().trim() + '</div></td>';
                                tabstr += '<td width="300"><div class="warp">' + $(this).children("委員意見").text().trim() + '</div></td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("修改日期").text().trim() + '</td>';
                                //tabstr += '<td name="ftd" nowrap="nowrap">' + isopStr + '</td>';
                                tabstr += '<td name="ftd" nowrap="nowrap">';
                                tabstr += '<input type=button value="編輯" class="genbtn" name="editbtn" aid="' + $(this).children("guid").text().trim() + '" />';
                                tabstr += '</td>'
                                tabstr += '<td name="ftd" nowrap="nowrap">';
                                tabstr += '<input type=button value="刪除" class="genbtn" name="delbtn" aid="' + $(this).children("guid").text().trim() + '" />';
                                tabstr += '</td>'
                                tabstr += '</tr>';
                            });
                            $("#fth").show();
                            $("#fth2").show();
                        }
                        else {
                            tabstr += '<tr><td colspan="6">查詢無資料</td></tr>';
                            $("#fth").hide();
                            $("#fth2").hide();
                        }                            
                        $("#tablistOpen tbody").append(tabstr);
                        if ($("#Competence").val() == '04' || $("#Competence").val() == '02' || $("#Competence").val() == '05' || $("#Competence").val() == '06') {
                            $("input[name='newbtn']").hide();
                            $("#opn").hide();
                            $("#opnall").hide();
                            $("#fth").hide();
                            $("#fth2").hide();
                            $("td[name='ftd']").hide();
                        }
                    }
                }
            });
        }

        //取得查核建議列表
        function GetAllList() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetOilAllSuggestion.aspx",
                data: {
                    cpid: $.getQueryString("cp"),
                    qid: $("#qGuid").val(),
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
                        $("#tablistAll tbody").empty();
                        var tabstr = '';
                        var isop = '';
                        var isopStr = '';
                        if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                //isop = $(this).children("是否列入查核意見").text().trim();
                                //if (isop != 'Y')
                                //    isopStr = '否';
                                //else
                                //    isopStr = '是';
                                tabstr += '<tr>';
                                tabstr += '<td nowrap="nowrap" style="display:none">';
                                tabstr += '<input type="hidden" aid="' + $(this).children("委員guid").text().trim() + '" />';
                                tabstr += '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("委員").text().trim() + '</td>';
                                //tabstr += '<td nowrap="nowrap">' + $(this).children("檢視文件").text().trim() + '</td>';
                                tabstr += '<td width="300"><div class="warp">' + $(this).children("委員意見").text().trim() + '</div></td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("修改日期").text().trim() + '</td>';
                                //tabstr += '<td name="ftd" nowrap="nowrap">' + isopStr + '</td>';
                                tabstr += '<td name="ftd" nowrap="nowrap">';
                                tabstr += '<input type=button value="編輯" class="genbtn" name="editallbtn" aid="' + $(this).children("guid").text().trim() + '" />';
                                tabstr += '</td>'
                                tabstr += '<td name="ftd" nowrap="nowrap">';
                                tabstr += '<input type=button value="刪除" class="genbtn" name="delallbtn" aid="' + $(this).children("guid").text().trim() + '" />';
                                tabstr += '</td>'
                                tabstr += '</tr>';
                            });
                            $("#fthall").show();
                            $("#fthall2").show();
                        }
                        else {
                            tabstr += '<tr><td colspan="6">查詢無資料</td></tr>';
                            $("#fthall").hide();
                            $("#fthall2").hide();
                        }
                            
                        $("#tablistAll tbody").append(tabstr);
                        if ($("#Competence").val() == '04' || $("#Competence").val() == '02' || $("#Competence").val() == '05' || $("#Competence").val() == '06') {
                            $("input[name='newallbtn']").hide();
                            $("#opn").hide();
                            $("#opnall").hide();
                            $("#fthall").hide();
                            $("#fthall2").hide();
                            $("td[name='ftd']").hide();
                        }
                    }
                }
            });
        }

        //查核建議內的附件列表
        function GetAllFileList() {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../handler/GetFile.aspx",
                data: {
                    cpid: $.getQueryString("cp"),
                    guid: $("#logGuid").val(),
                    year: getTaiwanDate(),
                    type: "06",
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
						$("#tablistFile tbody").empty();
						var tabstr = '';
						if ($(data).find("data_item").length > 0) {
                            $(data).find("data_item").each(function (i) {
                                var filename = $(this).children("新檔名").text().trim();
                                var fileextension = $(this).children("附檔名").text().trim();
								tabstr += '<tr>';
                                tabstr += '<td nowrap="nowrap">';                                
                                tabstr += '<img width="200px" height="200px" name="img_' + $(this).children("guid").text().trim() + $(this).children("排序").text().trim() + '" src="../DOWNLOAD.aspx?category=Oil&type=selfEvaluation&sn=' + $(this).children("排序").text().trim() +
                                    '&v=' + $(this).children("guid").text().trim() + '" alt="' + filename + fileextension + '" style="display:none" >';                                
                                tabstr += '<a name="a_' + $(this).children("guid").text().trim() + $(this).children("排序").text().trim() + '" href="../DOWNLOAD.aspx?category=Oil&type=selfEvaluation&sn=' + $(this).children("排序").text().trim() +
                                    '&v=' + $(this).children("guid").text().trim() + '" style="display:none" >' + filename + fileextension + '</a>';
                                tabstr += '</td>';
                                tabstr += '<td nowrap="nowrap">' + $(this).children("上傳日期").text().trim() + '</td>';
                                tabstr += '<td name="td_editFile" nowrap="" align="center"><a href="javascript:void(0);" name="delbtnFile" aid="' + $(this).children("guid").text().trim() +
                                    '" sn="' + $(this).children("排序").text().trim() + '">刪除</a></td>';
								tabstr += '</tr>';
							});
						}
						else
							tabstr += '<tr><td colspan="3">查詢無資料</td></tr>';
                        $("#tablistFile tbody").append(tabstr);

                        //確認權限&按鈕顯示或隱藏
                        if (($("#Competence").val() == '01') || ($("#Competence").val() == '03')) {
                                $("#uploadfile2").show();
                                $("#thFunc").show();
                                $("td[name='td_editFile']").show();
                        }
                        else {
                            $("#uploadfile2").hide();
                            $("#thFunc").hide();
                            $("td[name='td_editFile']").hide();
                        }
					}
				}
			});
        }

        function getExtension() {
			$.ajax({
				type: "POST",
				async: false, //在沒有返回值之前,不會執行下一步動作
				url: "../handler/GetFile.aspx",
                data: {
                    cpid: $.getQueryString("cp"),
                    guid: $("#logGuid").val(),
                    year: getTaiwanDate(),
                    type: "06",
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
                                var fileextension = $(this).children("附檔名").text().trim();
								if (fileextension == ".jpg" || fileextension == ".jpeg" || fileextension == ".png") {
                                    $("img[name='img_" + $(this).children("guid").text().trim() + $(this).children("排序").text().trim() + "']").show();
                                }
                                else {
                                    $("a[name='a_" + $(this).children("guid").text().trim() + $(this).children("排序").text().trim() + "']").show();
                                }
							});
						}
					}
				}
			});
        }

        function GetList() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetSelfEvaluation_QuestionList.aspx",
                data: {
                    category: "oil",
                    year: getTaiwanDate(),
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
                                        var sGuid = $(this).attr("lvGuid");
                                        dataStr = '<tr guid="' + $(this).attr("lvGuid") + '" data-tt-id="' + $(this).attr("lvGuid") + '">';
                                        if ($(this).attr("psred") == 'Y')
                                            dataStr += '<td><span style="color: red">' + $(this).attr("lvName") + '</span></td>';
                                        else
                                            dataStr += '<td>' + $(this).attr("lvName") + '</td>';
                                        if ($(this).attr("psanswer") == 'Y') {
                                            dataStr += '<td align="center" nowrap>';
                                            dataStr += '<div class="inlineitem textcenter"><input type="radio" name="cg_' + $(this).attr("lvGuid") + '" value="00" class="cRadio" /><br>0</div>&emsp;';
                                            dataStr += '<div class="inlineitem textcenter"><input type="radio" name="cg_' + $(this).attr("lvGuid") + '" value="01" class="cRadio" /><br>1</div>&emsp;';
                                            dataStr += '<div class="inlineitem textcenter"><input type="radio" name="cg_' + $(this).attr("lvGuid") + '" value="02" class="cRadio" /><br>2</div>&emsp;';
                                            dataStr += '<div class="inlineitem textcenter"><input type="radio" name="cg_' + $(this).attr("lvGuid") + '" value="03" class="cRadio" /><br>3</div>&emsp;';
                                            dataStr += '<div class="inlineitem textcenter"><input type="radio" name="cg_' + $(this).attr("lvGuid") + '" value="04" class="cRadio" /><br>4</div>&emsp;';
                                            dataStr += '<div class="inlineitem textcenter"><input type="radio" name="cg_' + $(this).attr("lvGuid") + '" value="05" class="cRadio" /><br>5</div>&emsp;';
                                            dataStr += '<div class="inlineitem textcenter"><input type="radio" name="cg_' + $(this).attr("lvGuid") + '" value="06" class="cRadio" /><br>&emsp;</div>';
                                            dataStr += '</td>';
                                            dataStr += '<td align="center" nowrap>';
                                            dataStr += '<div class="inlineitem textcenter"><input type="radio" name="mg_' + $(this).attr("lvGuid") + '" value="00" class="psCtrl mRadio" /><br>0</div>&emsp;';
                                            dataStr += '<div class="inlineitem textcenter"><input type="radio" name="mg_' + $(this).attr("lvGuid") + '" value="01" class="psCtrl mRadio" /><br>1</div>&emsp;';
                                            dataStr += '<div class="inlineitem textcenter"><input type="radio" name="mg_' + $(this).attr("lvGuid") + '" value="02" class="psCtrl mRadio" /><br>2</div>&emsp;';
                                            dataStr += '<div class="inlineitem textcenter"><input type="radio" name="mg_' + $(this).attr("lvGuid") + '" value="03" class="psCtrl mRadio" /><br>3</div>&emsp;';
                                            dataStr += '<div class="inlineitem textcenter"><input type="radio" name="mg_' + $(this).attr("lvGuid") + '" value="04" class="psCtrl mRadio" /><br>4</div>&emsp;';
                                            dataStr += '<div class="inlineitem textcenter"><input type="radio" name="mg_' + $(this).attr("lvGuid") + '" value="05" class="psCtrl mRadio" /><br>5</div>&emsp;';
                                            dataStr += '<div class="inlineitem textcenter"><input type="radio" name="mg_' + $(this).attr("lvGuid") + '" value="06" class="psCtrl mRadio" /><br>&emsp;</div>';
                                            dataStr += '</td>';
                                        }
                                        else {
                                            dataStr += '<td></td><td></td>';
                                        }
                                        dataStr += '<td style="text-align:center;">' + $(this).attr("ref") + '</td>';
                                        if ($(this).attr("psall") != '') {
                                            $.ajax({
                                                type: "POST",
                                                async: false, //在沒有返回值之前,不會執行下一步動作
                                                url: "../Handler/GetOilAllSuggestion.aspx",
                                                data: {
                                                    cpid: $.getQueryString("cp"),
                                                    qid: $(this).attr("lvGuid"),
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
                                                        var dataStr2 = '';
                                                        if ($(data).find("data_item").length > 0) {
                                                            dataStr2 += '<td><span name="spcom_' + sGuid + '">查核建議...</span></td>';
                                                            dataStr += dataStr2;
                                                        }
                                                        else {
                                                            dataStr += '<td><span name="spcom_' + sGuid + '"></span></td>';
                                                        }
                                                    }
                                                }
                                            });
                                        }
                                        else {
                                            dataStr += '<td><span name="spcom_' + sGuid + '"></span></td>';
                                        }
                                        dataStr += '<td style="text-align:center;">';
                                        if ($(this).attr("psall") != '') {
                                            dataStr += '<input type="button" class="grebtn font-size3" value="查核建議" guid="' + $(this).attr("lvGuid") + '" name="psallbtn" title="查核建議" />';
                                        }
                                        dataStr += '</td>';
                                        dataStr += '</tr>'
                                        $("#tablist tbody").append(dataStr);
                                    });
                                }
                                else
                                    ParentLevel(data, i);
                            }

                            // question list
                            var qStr = '';
                            $(data).find("q").each(function () {
                                qStr += '<tr guid="' + $(this).attr("qGuid") + '"  data-tt-id="' + $(this).attr("lvGuid") + '" data-tt-parent-id="' + $(this).attr("pGuid") + '" class="son' + xLv + '">';
                                qStr += '<td>' + $(this).attr("qTitle") + '</td>';
                                qStr += '<td align="center" nowrap>';
                                qStr += '<div class="inlineitem textcenter"><input type="radio" name="cg_' + $(this).attr("qGuid") + '" value="00" class="cRadio" /><br>0</div>&emsp;';
                                qStr += '<div class="inlineitem textcenter"><input type="radio" name="cg_' + $(this).attr("qGuid") + '" value="01" class="cRadio" /><br>1</div>&emsp;';
                                qStr += '<div class="inlineitem textcenter"><input type="radio" name="cg_' + $(this).attr("qGuid") + '" value="02" class="cRadio" /><br>2</div>&emsp;';
                                qStr += '<div class="inlineitem textcenter"><input type="radio" name="cg_' + $(this).attr("qGuid") + '" value="03" class="cRadio" /><br>3</div>&emsp;';
                                qStr += '<div class="inlineitem textcenter"><input type="radio" name="cg_' + $(this).attr("qGuid") + '" value="04" class="cRadio" /><br>4</div>&emsp;';
                                qStr += '<div class="inlineitem textcenter"><input type="radio" name="cg_' + $(this).attr("qGuid") + '" value="05" class="cRadio" /><br>5</div>&emsp;';
                                qStr += '<div class="inlineitem textcenter"><input type="radio" name="cg_' + $(this).attr("qGuid") + '" value="06" class="cRadio" /><br>&emsp;</div>';
                                qStr += '</td>';
                                qStr += '<td align="center" nowrap>';
                                qStr += '<div class="inlineitem textcenter"><input type="radio" name="mg_' + $(this).attr("qGuid") + '" value="00" class="psCtrl mRadio" /><br>0</div>&emsp;';
                                qStr += '<div class="inlineitem textcenter"><input type="radio" name="mg_' + $(this).attr("qGuid") + '" value="01" class="psCtrl mRadio" /><br>1</div>&emsp;';
                                qStr += '<div class="inlineitem textcenter"><input type="radio" name="mg_' + $(this).attr("qGuid") + '" value="02" class="psCtrl mRadio" /><br>2</div>&emsp;';
                                qStr += '<div class="inlineitem textcenter"><input type="radio" name="mg_' + $(this).attr("qGuid") + '" value="03" class="psCtrl mRadio" /><br>3</div>&emsp;';
                                qStr += '<div class="inlineitem textcenter"><input type="radio" name="mg_' + $(this).attr("qGuid") + '" value="04" class="psCtrl mRadio" /><br>4</div>&emsp;';
                                qStr += '<div class="inlineitem textcenter"><input type="radio" name="mg_' + $(this).attr("qGuid") + '" value="05" class="psCtrl mRadio" /><br>5</div>&emsp;';
                                qStr += '<div class="inlineitem textcenter"><input type="radio" name="mg_' + $(this).attr("qGuid") + '" value="06" class="psCtrl mRadio" /><br>&emsp;</div>';
                                qStr += '</td>';
                                qStr += '<td style="text-align:center;">';
                                qStr += '<span id="vf_' + $(this).attr("qGuid") + '" class="itemhint mRef" title="" style="display:none;"></span>';
                                qStr += '<input type="hidden" id="fv_' + $(this).attr("qGuid") + '" name="fv_' + $(this).attr("qGuid") + '" />';
                                qStr += '</td>';
                                qStr += '<td class="font-normal">';
                                qStr += '<span id="sp_' + $(this).attr("qGuid") + '" class="itemhint" title="" style="display:none;"></span>';
                                qStr += '<input type="hidden" id="ps_' + $(this).attr("qGuid") + '" name="ps_' + $(this).attr("qGuid") + '" />';
                                qStr += '</td>';
                                qStr += '<td align="center" nowrap>';
                                qStr += '<input type="button" value="說明列表" class="genbtn font-size3" qid="' + $(this).attr("qGuid") + '" name="psbtn" title="說明列表" />';
                                qStr += '</td>';
                                qStr += '</tr>';

                                //check change item
                                if ($(this).next().attr("pGuid") == undefined) {
                                    $("#tablist tbody tr[guid='" + $(this).attr("pGuid") + "']").after(qStr);
                                    qStr = '';
                                }
                            });

                            $("#tablist").treetable({
                                expandable: true, // 展開or收合
                                column: 0
                            });
                            //$("#tablist").treetable('expandAll');
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
                    var sGuid = $(this).attr("lvGuid");
                    dataStr += '<tr guid="' + $(this).attr("lvGuid") + '" data-tt-id="' + $(this).attr("lvGuid") + '" data-tt-parent-id="' + $(this).attr("pGuid") + '" class="son' + trrCss + '">';
                    if ($(this).attr("psred") == 'Y')
                        dataStr += '<td><span style="color: red">' + $(this).attr("lvName") + '</span></td>';
                    else
                        dataStr += '<td>' + $(this).attr("lvName") + '</td>';                    
                    if ($(this).attr("psanswer") == 'Y') {
                        dataStr += '<td align="center" nowrap>';
                        dataStr += '<div class="inlineitem textcenter"><input type="radio" name="cg_' + $(this).attr("lvGuid") + '" value="00" class="cRadio" /><br>0</div>&emsp;';
                        dataStr += '<div class="inlineitem textcenter"><input type="radio" name="cg_' + $(this).attr("lvGuid") + '" value="01" class="cRadio" /><br>1</div>&emsp;';
                        dataStr += '<div class="inlineitem textcenter"><input type="radio" name="cg_' + $(this).attr("lvGuid") + '" value="02" class="cRadio" /><br>2</div>&emsp;';
                        dataStr += '<div class="inlineitem textcenter"><input type="radio" name="cg_' + $(this).attr("lvGuid") + '" value="03" class="cRadio" /><br>3</div>&emsp;';
                        dataStr += '<div class="inlineitem textcenter"><input type="radio" name="cg_' + $(this).attr("lvGuid") + '" value="04" class="cRadio" /><br>4</div>&emsp;';
                        dataStr += '<div class="inlineitem textcenter"><input type="radio" name="cg_' + $(this).attr("lvGuid") + '" value="05" class="cRadio" /><br>5</div>&emsp;';
                        dataStr += '<div class="inlineitem textcenter"><input type="radio" name="cg_' + $(this).attr("lvGuid") + '" value="06" class="cRadio" /><br>&emsp;</div>';
                        dataStr += '</td>';
                        dataStr += '<td align="center" nowrap>';
                        dataStr += '<div class="inlineitem textcenter"><input type="radio" name="mg_' + $(this).attr("lvGuid") + '" value="00" class="psCtrl mRadio" /><br>0</div>&emsp;';
                        dataStr += '<div class="inlineitem textcenter"><input type="radio" name="mg_' + $(this).attr("lvGuid") + '" value="01" class="psCtrl mRadio" /><br>1</div>&emsp;';
                        dataStr += '<div class="inlineitem textcenter"><input type="radio" name="mg_' + $(this).attr("lvGuid") + '" value="02" class="psCtrl mRadio" /><br>2</div>&emsp;';
                        dataStr += '<div class="inlineitem textcenter"><input type="radio" name="mg_' + $(this).attr("lvGuid") + '" value="03" class="psCtrl mRadio" /><br>3</div>&emsp;';
                        dataStr += '<div class="inlineitem textcenter"><input type="radio" name="mg_' + $(this).attr("lvGuid") + '" value="04" class="psCtrl mRadio" /><br>4</div>&emsp;';
                        dataStr += '<div class="inlineitem textcenter"><input type="radio" name="mg_' + $(this).attr("lvGuid") + '" value="05" class="psCtrl mRadio" /><br>5</div>&emsp;';
                        dataStr += '<div class="inlineitem textcenter"><input type="radio" name="mg_' + $(this).attr("lvGuid") + '" value="06" class="psCtrl mRadio" /><br>&emsp;</div>';
                        dataStr += '</td>';
                    }
                    else {
                        dataStr += '<td></td><td></td>';
                    }
                    dataStr += '<td style="text-align:center;">' + $(this).attr("ref") + '</td>';
                    if ($(this).attr("psall") != '') {
                        $.ajax({
                            type: "POST",
                            async: false, //在沒有返回值之前,不會執行下一步動作
                            url: "../Handler/GetOilAllSuggestion.aspx",
                            data: {
                                cpid: $.getQueryString("cp"),
                                qid: $(this).attr("lvGuid"),
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
                                    var dataStr2 = '';
                                    if ($(data).find("data_item").length > 0) {
                                        dataStr2 += '<td><span name="spcom_' + sGuid + '">查核建議...</span></td>';
                                        dataStr += dataStr2;
                                    }
                                    else {
                                        dataStr += '<td><span name="spcom_' + sGuid + '"></span></td>';
                                    }
                                }
                            }
                        });
                    }
                    else {
                        dataStr += '<td><span name="spcom_' + sGuid + '"></span></td>';
                    }
                    dataStr += '<td style="text-align:center;">';
                    if ($(this).attr("psall") != '') {
                        dataStr += '<input type="button" class="grebtn font-size3" value="查核建議" guid="' + $(this).attr("lvGuid") + '" name="psallbtn" title="查核建議" />';
                    }                        
                    dataStr += '</td>';
                    dataStr += '</tr>';

                    //check change item
                    if ($(this).next().attr("pGuid") == undefined) {
                        $("#tablist tbody tr[guid='" + $(this).attr("pGuid") + "']").after(dataStr);
                        dataStr = '';
                    }
                }
            });
        }

        function RemoveQuestion() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetOilExclude.aspx",
                data: {
                    cpid: $.getQueryString("cp"),
                    year: getTaiwanDate()
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
                            $(data).find("data_item").each(function () {
                                if ($(this).children("排除題目guid").text().trim() != "")
                                    $("#tablist tbody").find("tr[guid='" + $(this).children("排除題目guid").text().trim() + "']").remove();
                                else {
                                    $("#tablist tbody").find("tr[guid='" + $(this).children("排除分類guid").text().trim() + "']").remove();
                                    $("#tablist tbody").find("tr[data-tt-parent-id='" + $(this).children("排除分類guid").text().trim() + "']").remove();
                                }
                            });
                        }
                    }
                }
            });
        }

        function GetAns() {
            $.ajax({
                type: "POST",
                async: false, //在沒有返回值之前,不會執行下一步動作
                url: "../Handler/GetOilAnswer.aspx",
                data: {
                    cpid: $.getQueryString("cp"),
                    year: getTaiwanDate()
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
                            $(data).find("data_item[填寫人員類別='01']").each(function () {
                                $("input[name='mg_" + $(this).attr("題目guid") + "'][value='" + $(this).attr("答案") + "']").prop("checked", true);
                                $("input[name='ps_" + $(this).attr("題目guid") + "']").val($(this).attr("委員意見"));
                                $("input[name='fv_" + $(this).attr("題目guid") + "']").val($(this).attr("檢視文件"));
                                var simpleStr = ($(this).attr("委員意見").length > 15) ? $(this).attr("委員意見").substr(0, 15) + "..." : $(this).attr("委員意見");
                                var simpleView = ($(this).attr("檢視文件").length > 15) ? $(this).attr("檢視文件").substr(0, 15) + "..." : $(this).attr("檢視文件");
                                $("#sp_" + $(this).attr("題目guid")).html(simpleStr);
                                $("#vf_" + $(this).attr("題目guid")).html(simpleView);
                                $("#sp_" + $(this).attr("題目guid")).attr("title", $(this).attr("委員意見"));
                                $("#vf_" + $(this).attr("題目guid")).attr("title", $(this).attr("檢視文件"));
                                //if ($("#Competence").val() != "02" && $(this).attr("答案") != "") {
                                //    $("input[name='psbtn'][qid='" + $(this).attr("題目guid") + "']").show();
                                //}
                                $("#sp_" + $(this).attr("題目guid")).show();
                                $("#vf_" + $(this).attr("題目guid")).show();
                            });
                            $(data).find("data_item[填寫人員類別='02']").each(function () {
                                $("input[name='cg_" + $(this).attr("題目guid") + "'][value='" + $(this).attr("答案") + "']").prop("checked", true);
                            });
                        }
                    }
                }
            });
        }

        function doOpenDialog() {
            var WinHeight = $("html").height();
            var ColHeight = WinHeight * 0.6;
            $.colorbox({ title:"說明列表", inline: true, href: "#checklistedit", width: "100%", maxWidth: "800", maxHeight: ColHeight, opacity: 0.5 });
        }
        function doOpenDialog2() {
            var WinHeight = $("html").height();
            var ColHeight = WinHeight * 0.6;
            $.colorbox({ title:"新增", inline: true, href: "#checklistedit2", width: "100%", maxWidth: "800", maxHeight: ColHeight, opacity: 0.5 });
        }
        function doOpenDialog3() {
            var WinHeight = $("html").height();
            var ColHeight = WinHeight * 0.6;
            $.colorbox({ title:"編輯", inline: true, href: "#checklistedit3", width: "100%", maxWidth: "800", maxHeight: ColHeight, opacity: 0.5 });
        }
        function doOpenDialog4() {
            var WinHeight = $("html").height();
            var ColHeight = WinHeight * 0.6;
            $.colorbox({ title:"查核建議", inline: true, href: "#alllist", width: "100%", maxWidth: "800", maxHeight: ColHeight, opacity: 0.5 });
        }
        function doOpenDialog5() {
            var WinHeight = $("html").height();
            var ColHeight = WinHeight * 0.6;
            $.colorbox({ title:"新增", inline: true, href: "#checklistedit4", width: "100%", maxWidth: "800", maxHeight: ColHeight, opacity: 0.5 });
        }
        function doOpenDialog6() {
            var WinHeight = $("html").height();
            var ColHeight = WinHeight * 0.6;
            $.colorbox({ title:"編輯", inline: true, href: "#checklistedit5", width: "100%", maxWidth: "1000", maxHeight: ColHeight, opacity: 0.5 });
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
        <input type="hidden" id="Competence" value="<%= identity %>" />
        <input type="hidden" id="CompanyName" value="<%= companyName %>" />
        <input type="hidden" id="qGuid" />
        <input type="hidden" id="qAnswer" />
        <input type="hidden" id="logGuid" />
        <form id="form1">
            <!-- Preloader -->
            <div id="preloader">
                <div id="status">
                    <div id="CSS3loading">
                        <!-- css3 loading -->
                        <div class="sk-three-bounce">
                            <div class="sk-child sk-bounce1"></div>
                            <div class="sk-child sk-bounce2"></div>
                            <div class="sk-child sk-bounce3"></div>
                        </div>
                        <!-- css3 loading -->
                        <span id="loadingword">資料讀取中，請稍待...</span>
                    </div>
                    <!-- CSS3loading -->
                </div>
                <!-- status -->
            </div>
            <!-- preloader -->

            <div class="container BoxBgWa BoxShadowD">
                <div class="WrapperBody" id="WrapperBody">
                    <!--#include file="OilHeader.html"-->

                    <div id="ContentWrapper">
                        <div class="container margin15T">
                            <div class="padding10ALL">
                                <div class="filetitlewrapper">
                                    <span class="filetitle font-size7">
                                        <label id="lbl_CompanyName"></label>
                                    </span>
                                    <span class="btnright">
                                        <div class="font-size4 font-normal">
                                            <a id="exportBtn" href="#" class="genbtn">產生報告</a>
                                            <i class="fa fa-file-word-o IconCc" aria-hidden="true"></i><a href="../doc/111年度查核填寫內容下載-石油.docx" target="_blank">查核填寫內容下載</a> 
                                            <i class="fa fa-file-pdf-o IconCc" aria-hidden="true"></i><a href="../doc/111年度查核配事項下載-石油.pdf" target="_blank">查核配合事項下載</a> 
                                            <i class="fa fa-file-powerpoint-o IconCc" aria-hidden="true"></i><a href="../doc/111年度簡報大綱下載-石油.pptx" target="_blank">簡報大綱下載</a>
                                            <i class="fa fa-file-pdf-o IconCc" aria-hidden="true"></i><a href="../doc/111年度應準備資料下載-石油.pdf" target="_blank">應準備資料下載</a>
                                        </div>
                                    </span>
                                </div>
                                <div class="twocol">
                                    <div class="right font-normal font-size4">
                                        <span id="alertText" style="color: red">* 請先點選儲存再離開表單</span>
                                        <input type="button" id="subbtnTop" value="儲存" class="genbtn" />&nbsp;&nbsp;
                                        <a href="#" id="btnallopen"><i class="fa fa-plus-square-o" aria-hidden="true"></i>&nbsp;全部展開</a>&nbsp;&nbsp;
									<a href="#" id="btnallclose"><i class="fa fa-minus-square-o" aria-hidden="true"></i>&nbsp;全部收合</a>
                                    </div>
                                </div>

                                <div class="stripetreeB margin10T">
                                    <table id="tablist" width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <thead>
                                            <tr>
                                                <th nowrap="nowrap" rowspan="2" class="font-size3"><span id="taiwanYear"></span>年石油管線及儲油設施查核項目</th>
                                                <th nowrap="nowrap" width="200" class="font-size3">業者</th>
                                                <th nowrap="nowrap" width="200" class="font-size3">委員</th>
                                                <th nowrap="nowrap" rowspan="2" class="font-size3">檢視文件</th>
                                                <th nowrap="nowrap" rowspan="2" width="300" class="font-size3">審查說明</th>
                                                <th nowrap="nowrap" rowspan="2" width="50" class="font-size3">功能</th>
                                            </tr>
                                            <tr>
                                                <th nowrap="nowrap"><div class="width100 font-size3 textleft">不符合<div style="float: right;">不適用</div><div style="float: right;">符合&nbsp;</div></div></th>
                                                <th nowrap="nowrap"><div class="width100 font-size3 textleft">不符合<div style="float: right;">不適用</div><div style="float: right;">符合&nbsp;</div></div></th>
                                            </tr>
                                        </thead>
                                        <tbody></tbody>
                                    </table>
                                </div>
                                <!-- stripetree -->
                                <div id="errMsg" style="color: red;"></div>
                                <div style="margin-top: 20px;">
                                    <input type="button" id="subbtn" value="儲存" class="genbtn" /></div>
                            </div>
                        </div>
                        <!-- container -->
                    </div>
                    <!-- ContentWrapper -->

                    <div class="container-fluid">
                        <div class="backTop"><a href="#" class="backTotop">TOP</a></div>
                    </div>
                </div>
                <!-- WrapperBody -->

                <!--#include file="Footer.html"-->
            </div>
            <!-- BoxBgWa -->
            <!-- 側邊選單內容:動態複製主選單內容 -->
            <div id="sidebar-wrapper"></div>
            <!-- sidebar-wrapper -->
        </form>
    </div>
    <!-- 結尾用div:修正mmenu form bug -->

    <form id="form2">
    <!-- logList -->
    <div style="display: none;">
        <div id="checklistedit">
            <div class="margin35T padding5RL">
                <div align="right">
                    <input type="button" name="newbtn" value="新增" class="genbtn" />
                </div>
                <div class="stripetreeG margin10T">
                    <table id="tablistOpen" width="100%" border="0" cellspacing="0" cellpadding="0">
                        <thead>
                            <tr>
                                <th nowrap="nowrap" width="8%">委員</th>
                                <%--<th nowrap="nowrap" width="">答案</th>--%>
                                <th nowrap="nowrap" width="200">檢視文件</th>
                                <th nowrap="nowrap" width="300">審查說明</th>
                                <th nowrap="nowrap" width="23%">修改日期</th>
                                <%--<th id="opn" nowrap="nowrap" width="">是否列入查核意見</th>--%>
                                <th id="fth2" nowrap="nowrap" width="11%"></th>
                                <th id="fth" nowrap="nowrap" width="11%"></th>
                            </tr>
                            <%--<tr>
                                <th nowrap="nowrap"><div class="textleft">不符合<div style="float: right;">不適用</div><div style="float: right;">符合&nbsp;</div></div></th>
                            </tr>--%>
                        </thead>
                        <tbody></tbody>
                    </table>
                </div>
                <!-- stripetree -->
                <div id="errMsgOpen" style="color: red;"></div>
            </div>
        </div>
    </div>    

    <!-- all opinnions list -->
    <div style="display: none;">
        <div id="alllist">
            <div class="margin35T padding5RL">
                <div align="right">
                    <input type="button" name="newallbtn" value="新增" class="genbtn" />
                </div>
                <div class="stripetreeG margin10T">
                    <table id="tablistAll" width="100%" border="0" cellspacing="0" cellpadding="0">
                        <thead>
                            <tr>
                                <th nowrap="nowrap" width="8%">委員</th>
                                <%--<th nowrap="nowrap" width="">檢視文件</th>--%>
                                <th nowrap="nowrap" width="300">查核建議</th>
                                <th nowrap="nowrap" width="23%">修改日期</th>
                                <%--<th id="opnall" nowrap="nowrap" width="">是否列入查核意見</th>--%>
                                <th id="fthall" nowrap="nowrap" width="11%"></th>
                                <th id="fthall2" nowrap="nowrap" width="11%"></th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                    </table>
                </div>
                <!-- stripetree -->
                <div id="errMsgOpen2" style="color: red;"></div>
            </div>
        </div>
    </div>

    <!-- new opinnions -->
    <div style="display: none;">
        <div id="checklistedit2">
            <div class="margin35T padding5RL">
                <div class="OchiTrasTable width100 TitleLength03 font-size3">
                    <div class="OchiRow">
                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">檢視文件</div>
                        <div class="OchiCell width100">
                            <input type="text" id="psViewFile"class="inputex width80" />
                        </div>
                    </div><!-- OchiRow -->
                    <div class="OchiRow">
                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">審查說明</div>
                        <div class="OchiCell width100">
                            <textarea id="psStr2" rows="8" cols="" class="inputex width100"></textarea>
                        </div>
                    </div><!-- OchiRow -->
                    <%--<div class="OchiRow">
                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">是否列入查核意見</div>
                        <div class="OchiCell width100">
                            <select id="psIsop" class="inputex width30">
                                <option value="Y">是</option>
                                <option value="N" selected>否</option>
                            </select>
                        </div>
                    </div><!-- OchiRow -->--%>
                </div> 
                <!-- OchiTrasTable -->
            </div>

            <div class="twocol margin10T">
                <div class="right">
                    <a href="javascript:void(0);" id="ps_cancel2" class="genbtn">取消</a>
                    <a href="javascript:void(0);" id="ps_savebtn2" class="genbtn">儲存</a>
                </div>
            </div>
            <br />
            <br />
        </div>
    </div>

    <!-- edit opinnions -->
    <div style="display: none;">
        <div id="checklistedit3">
            <div class="margin35T padding5RL">
                <div class="OchiTrasTable width100 TitleLength03 font-size3">
                    <div class="OchiRow">
                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">檢視文件</div>
                        <div class="OchiCell width100">
                            <input type="text" id="psViewFileEdit"class="inputex width80" />
                        </div>
                    </div><!-- OchiRow -->
                    <div class="OchiRow">
                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">審查說明</div>
                        <div class="OchiCell width100">
                            <textarea id="psStr2Edit" rows="8" cols="" class="inputex width100"></textarea>
                        </div>
                    </div><!-- OchiRow -->
                    <%--<div class="OchiRow">
                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">是否列入查核意見</div>
                        <div class="OchiCell width100">
                            <select id="psIsopEdit" class="inputex width30">
                                <option value="Y">是</option>
                                <option value="N">否</option>
                            </select>
                        </div>
                    </div><!-- OchiRow -->--%>
                </div> 
                <!-- OchiTrasTable -->
            </div>

            <div class="twocol margin10T">
                <div class="right">
                    <a href="javascript:void(0);" id="ps_cancel3" class="genbtn">取消</a>
                    <a href="javascript:void(0);" id="ps_savebtn3" class="genbtn">儲存</a>
                </div>
            </div>
            <br />
            <br />
        </div>
    </div>

    <!-- all opnnions add -->
    <div style="display: none;">
        <div id="checklistedit4">
            <div class="margin35T padding5RL">
                <div class="OchiTrasTable width100 TitleLength03 font-size3">
                    <%--<div class="OchiRow">
                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">檢視文件</div>
                        <div class="OchiCell width100">
                            <input type="text" id="psViewFileAll"class="inputex width80" />
                        </div>
                    </div><!-- OchiRow -->--%>
                    <div class="OchiRow">
                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">查核建議</div>
                        <div class="OchiCell width100">
                            <textarea id="psStr2All" rows="8" cols="" class="inputex width100"></textarea>
                        </div>
                    </div><!-- OchiRow -->
                    <div class="OchiRow" id="uploadfile">
                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">上傳檔案</div>
                        <div class="OchiCell width100">
                            <input type="file" id="fileUpload" multiple="multiple" />
                            <div id="filelist"></div>
                        </div>
                    </div><!-- OchiRow -->
                    <%--<div class="OchiRow">
                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">是否列入查核意見</div>
                        <div class="OchiCell width100">
                            <select id="psIsopAll" class="inputex width30">
                                <option selected="selected" value="Y">是</option>
                                <option value="N">否</option>
                            </select>
                        </div>
                    </div><!-- OchiRow -->--%>
                </div> 
                <!-- OchiTrasTable -->
            </div>

            <div class="twocol margin10T">
                <div class="right">
                    <a href="javascript:void(0);" id="ps_cancelAll" class="genbtn">取消</a>
                    <a href="javascript:void(0);" id="ps_savebtnAll" class="genbtn">儲存</a>
                </div>
            </div>
            <br />
            <br />
        </div>
    </div>

    <!-- all opnnions edit -->
    <div style="display: none;">
        <div id="checklistedit5">
            <div class="margin35T padding5RL">
                <div class="OchiTrasTable width100 TitleLength03 font-size3">
                    <%--<div class="OchiRow">
                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">檢視文件</div>
                        <div class="OchiCell width100">
                            <input type="text" id="psViewFileAll2"class="inputex width80" />
                        </div>
                    </div><!-- OchiRow -->--%>
                    <div class="OchiRow">
                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">查核建議</div>
                        <div class="OchiCell width100">
                            <textarea id="psStr2All2" rows="8" cols="" class="inputex width100"></textarea>
                        </div>
                    </div><!-- OchiRow -->
                    <div class="OchiRow" id="uploadfile2">
                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">上傳檔案</div>
                        <div class="OchiCell width100">
                            <input type="file" id="fileUpload2" multiple="multiple" />
                            <div id="filelist2"></div>
                        </div>
                    </div><!-- OchiRow -->
                    <div class="twocol margin10T">
                        <div class="right">
                            <a href="javascript:void(0);" id="ps_cancelAll2" class="genbtn">取消</a>
                            <a href="javascript:void(0);" id="ps_savebtnAll2" class="genbtn">儲存</a>
                        </div>
                    </div>
                    <br />
                    <div class="OchiRow">
                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">附件列表</div>
                        <div class="OchiCell width100">
                            <div class="stripeMeB tbover">
                                <table id="tablistFile" width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr>
                                            <th nowrap width="50%">檔案名稱 </th>
                                            <th nowrap width="30%">上傳日期 </th>
                                            <th id="thFunc" nowrap width="10%">功能 </th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div><!-- stripeMe -->
                        </div>
                    </div><!-- OchiRow -->
                    <%--<div class="OchiRow">
                        <div class="OchiCell OchiTitle IconCe TitleSetWidth">是否列入查核意見</div>
                        <div class="OchiCell width100">
                            <select id="psIsopAll2" class="inputex width30">
                                <option value="Y">是</option>
                                <option value="N">否</option>
                            </select>
                        </div>
                    </div><!-- OchiRow -->--%>
                </div> 
                <!-- OchiTrasTable -->
            </div>
            <br />
            <br />
        </div>
    </div>
    </form>

    <!-- 本頁面使用的JS -->
    <script type="text/javascript">
        $(document).ready(function () {
            $(".container").css("max-width", "1800px");

            //編輯按鈕控制
            $(".editnotebtn,.sugnote").hide();
            $(".editnotebtnopen").on("click", function () {
                $(this).parent().next().next().children(".editnotebtn,span").fadeIn();
            });
            $(".editnotebtnclose").on("click", function () {
                $(this).parent().next().next().children(".editnotebtn,span").fadeOut();
            });
        });
    </script>
    <script type="text/javascript" src="../js/GenCommon.js"></script>
    <!-- UIcolor JS -->
    <script type="text/javascript" src="../js/PageCommon.js"></script>
    <!-- 系統共用 JS -->
    <script type="text/javascript" src="../js/MenuGas.js"></script>
    <!-- 系統共用 JS -->
    <script type="text/javascript" src="../js/autoHeight.js"></script>
    <!-- 高度不足頁面的絕對置底footer -->
</body>
</html>