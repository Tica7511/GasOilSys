var serarrA = ["一、", "二、", "三、", "四、", "五、", "六、", "七、", "八、", "九、", "十、", "十一、", "十二、"];
var serarrB = ["(一)、", "(二)、", "(三)、", "(四)、", "(五)、", "(六)、", "(七)、", "(八)、", "(九)、", "(十)、", "(十一)、", "(十二)、"];
var serarrAnum = $("#navmenuV > ul >li").length;
var serarrSanum = $(".navSa >li").length;
var serarrSbnum = $(".navSb >li").length;
for (i = 0; i < serarrAnum; i++) {
    var j = i + 1;
    $("<span class='navsernum'>" + serarrA[i] + "</span>").insertBefore("#navmenuV > ul >li:nth-child(" + j + ") > a");
}
for (i = 0; i < serarrSanum; i++) {
    var j = i + 1;
    $("<span class='navsernum'>" + serarrB[i] + "</span>").insertBefore(".navSa >li:nth-child(" + j + ") > a");
}
for (i = 0; i < serarrSbnum; i++) {
    var j = i + 1;
    $("<span class='navsernum'>" + serarrB[i] + "</span>").insertBefore(".navSb >li:nth-child(" + j + ") > a");
}


var filecode = "\
<div class=\"font-size4 font-normal\"><i class=\"fa fa-file-word-o IconCc\" aria-hidden=\"true\"></i><a href=\"doc/附件3、110年天然氣生產進口事業查核填寫內容.docx\" target=\"_blank\">查核填寫內容下載</a> <i class=\"fa fa-file-powerpoint-o IconCc\" aria-hidden=\"true\"></i><a href=\"doc/查核配合事項(天然氣).pptx\" target=\"_blank\">查核配合事項下載</a> <i class=\"fa fa-file-powerpoint-o IconCc\" aria-hidden=\"true\"></i><a href=\"doc/附件4、110年天然氣生產、進口事業輸儲設備查核簡報大綱.pptx\" target=\"_blank\">簡報大綱下載</a></div>\
";

//將選單程式引入HTML
$("#filedown").html(filecode);