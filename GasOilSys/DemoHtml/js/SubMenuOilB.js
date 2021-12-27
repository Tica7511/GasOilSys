// JavaScript Document
var menucode = "\
<ul class='nav'>\
<li><a href='oil-firmC001.html' target='_self'>石油業者基本資料</a></li>\
<li><a href='#' target='_self'>儲油設施基本資料</a>\
<ul class='navSa'>\
<li><a href='oil-firmC101.html' target='_self'>庫區基本資料</a></li>\
<li><a href='oil-firmC102.html' target='_self'>儲槽基本資料</a></li>\
<li><a href='oil-firmC103.html' target='_self'>儲槽基礎、壁板、頂板</a></li>\
<li><a href='oil-firmC104.html' target='_self'>儲槽底板</a></li>\
<li><a href='oil-firmC105.html' target='_self'>底板更換紀錄</a></li>\
<li><a href='oil-firmC106.html' target='_self'>陰極防蝕系統</a></li>\
<li><a href='oil-firmC107.html' target='_self'>槽區管線</a></li>\
<li><a href='oil-firmC108.html' target='_self'>內部稽核</a></li>\
</ul>\
</li>\
<li><a href='oil-firmC301.html' target='_self'>控制室</a></li>\
<li><a href='oil-firmC401.html' target='_self'>事故學習</a></li>\
<li><a href='../WebPage/OilSelfEvaluation.aspx' target='_blank'>查核項目自評</a></li>\
<li><a href='doc/110 年度大林煉油廠第十蒸餾工場查核簡報.pptx' target='_blank'>查核簡報上傳</a></li>\
</ul>\
";

//將選單程式引入HTML
$("#navmenuV").html(menucode);
//插入中文編碼
var serarrA = ["一、","二、","三、","四、","五、","六、","七、","八、","九、","十、","十一、","十二、"];
var serarrB = ["(一)、","(二)、","(三)、","(四)、","(五)、","(六)、","(七)、","(八)、","(九)、","(十)、","(十一)、","(十二)、"];
var serarrAnum = $("#navmenuV > ul >li").length;
var serarrSanum = $(".navSa >li").length;
var serarrSbnum = $(".navSb >li").length;
for(i=0;i<serarrAnum; i++){
    var j = i + 1;
    $("<span class='navsernum'>"+ serarrA[i] +"</span>").insertBefore("#navmenuV > ul >li:nth-child(" + j + ") > a");
}
for(i=0;i<serarrSanum; i++){
    var j = i + 1;
    $("<span class='navsernum'>"+ serarrB[i] +"</span>").insertBefore(".navSa >li:nth-child(" + j + ") > a");
}
for(i=0;i<serarrSbnum; i++){
    var j = i + 1;
    $("<span class='navsernum'>"+ serarrB[i] +"</span>").insertBefore(".navSb >li:nth-child(" + j + ") > a");
}

var filecode = "\
<div class=\"font-size4 font-normal\"><i class=\"fa fa-file-word-o IconCc\" aria-hidden=\"true\"></i><a href=\"doc/附件3、110年度石油業者石油管線及儲油設施查核.docx\" target=\"_blank\">查核填寫內容下載</a> <i class=\"fa fa-file-powerpoint-o IconCc\" aria-hidden=\"true\"></i><a href=\"doc/查核配合事項_石油.pptx\" target=\"_blank\">查核配合事項下載</a> <i class=\"fa fa-file-powerpoint-o IconCc\" aria-hidden=\"true\"></i><a href=\"doc/附件4、110年度石油業者簡報內容格式_管線及儲槽.pptx\" target=\"_blank\">簡報大綱下載</a></div>\
";

//將選單程式引入HTML
$("#filedown").html(filecode);