// JavaScript Document
$(window).load(PreloadFunction);//預載畫面移除
$(document).ready(MainFunction);//主程式:所有JS程式放在此
//主程式內容
	function MainFunction(){
//RWD選單處理:桌機使用superfish，手機平板使用simplerSidebar，而simplerSidebar內的選單為複製主選單內容。

        var menucode = "\
<ul>\
<li><a href='WeekIndex.aspx' target='_self'>週報</a></li>\
<li><a href='MonthList.aspx' target='_self'>月報</a></li>\
<li><a href='SeasonList.aspx' target='_self'>季報</a></li>\
<li><a href='WeekReportList.aspx' target='_self'>計畫書填報</a></li>\
<li><a href='WeekSearch.aspx' target='_self'>週報查詢</a></li>\
</ul>\
";

        //將選單程式引入HTML
        $(".superfishmenu").html(menucode);
        //複製選單到sidebar
        var clonemenu = $(".superfishmenu > ul").clone(false);
        //移除superfish的id與class並給予新id
        clonemenu.appendTo($("#sidebar-wrapper")).attr("id", "sidemenu");
        //啟動下拉選單superfish
        $(".superfishmenu > ul").superfish({delay:300,}).supposition();
        //啟動mmenu
        $("#sidebar-wrapper").mmenu({
            //設定下拉選單為直接向下展開,而非滑動
            slidingSubmenus:false,
        });
        var mmenuswitch = $("#sidebar-wrapper").data( "mmenu" );

        //由於開關是動態加入(利用JS加入),故要使用動態binding的方式加入函式動作
        $(document).on("click", "#toggle-sidebar", function(){
            mmenuswitch.open();
        });

        //利用 設定滑動關閉選單
        $("#sidebar-wrapper").swipe({
            swipe:function(event, direction, distance, duration, fingerCount) {//事件，方向，距離（像素為單位），時間，手指數量
                if(direction == "left")//當向上滑動手指時令當前頁面記數器加1
                {mmenuswitch.close();}
            }
        });
	
	}//MainFunction
	
