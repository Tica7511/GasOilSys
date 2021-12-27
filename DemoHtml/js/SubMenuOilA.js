// JavaScript Document
var menucode = "\
<ul class='nav'>\
<li><a href='oil-firmB001.html' target='_self'>石油業者基本資料</a></li>\
<li><a href='#' target='_self'>儲油設施基本資料</a>\
<ul class='navSa'>\
<li><a href='oil-firmB101.html' target='_self'>庫區基本資料</a></li>\
<li><a href='oil-firmB102.html' target='_self'>儲槽基本資料</a></li>\
<li><a href='oil-firmB103.html' target='_self'>儲槽基礎、壁板、頂板</a></li>\
<li><a href='oil-firmB104.html' target='_self'>儲槽底板</a></li>\
<li><a href='oil-firmB105.html' target='_self'>底板更換紀錄</a></li>\
<li><a href='oil-firmB106.html' target='_self'>陰極防蝕系統</a></li>\
<li><a href='oil-firmB107.html' target='_self'>槽區管線</a></li>\
<li><a href='oil-firmB108.html' target='_self'>內部稽核</a></li>\
</ul>\
</li>\
<li><a href='#' target='_self'>管線管理</a>\
<ul class='navSb'>\
<li><a href='oil-firmB201.html' target='_self'>轄區長途管線方塊圖</a></li>\
<li><a href='oil-firmB202.html' target='_self'>長途管線路徑圖</a></li>\
<li><a href='oil-firmB203.html' target='_self'>石油管線基本資料</a></li>\
<li><a href='oil-firmB204.html' target='_self'>管線路徑環境特質</a></li>\
<li><a href='oil-firmB205.html' target='_self'>管線完整性管理作為</a></li>\
<li><a href='oil-firmB206.html' target='_self'>智慧型通管器檢查(ILI)</a></li>\
<li><a href='oil-firmB207.html' target='_self'>緊密電位檢測(CIPS)</a></li>\
<li><a href='oil-firmB208.html' target='_self'>異常整流站</a></li>\
<li><a href='oil-firmB209.html' target='_self'>管線巡檢</a></li>\
<li><a href='oil-firmB210.html' target='_self'>管線維修或開挖</a></li>\
<li><a href='oil-firmB211.html' target='_self'>風險評估</a></li>\
<li><a href='oil-firmB212.html' target='_self'>內部稽核</a></li>\
</ul>\
</li>\
<li><a href='oil-firmB301.html' target='_self'>控制室</a></li>\
<li><a href='oil-firmB401.html' target='_self'>事故學習</a></li>\
<li><a href='../WebPage/OilSelfEvaluation.aspx' target='_blank'>查核項目自評</a></li>\
<li><a href='doc/查核簡報-豐德.pdf' target='_blank'>查核簡報上傳</a></li>\
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