// JavaScript Document
$(window).load(PreloadFunction);//預載畫面移除
$(document).ready(MainFunction);//主程式:所有JS程式放在此
//主程式內容
	function MainFunction(){

    $('.equalheightblock').matchHeight({
        byRow: true,//若為false,則所有區塊等高
        property: 'height',//使用min-height會出問題
        target: null,//設定等高對象:$('.sidebar')
        remove: false
    });

	$('.itemhint').powerTip({placement:'e',smartPlacement:true,});//powertip:tooltip
	$('.itemhinthold').powerTip({placement:'e',smartPlacement:true,mouseOnToPopup:'true'});//tooltip可點選
	
	//datepicker
	$(".Jdatepicker").datetimepicker({
		format:'Y/m/d',
		timepicker:false,
		withoutBottomPanel: true,
		scrollInput:false,//取消input滾動調整日期功能
        dayOfWeekStart:1,
		//yearEnd:2022,//下拉選單最末年份
		});
    $(".JdatepickerS").datetimepicker({
        format:'Y/m/d',
        timepicker:false,
        withoutBottomPanel: true,
        scrollInput:false,//取消input滾動調整日期功能
        dayOfWeekStart:1,
        //minDate:0,//最小值為今天
        onShow:function( ct ){
            this.setOptions({
                maxDate:jQuery('.JdatepickerE').val()?jQuery('.JdatepickerE').val():false
            })
        },
    });
    $(".JdatepickerE").datetimepicker({
        format:'Y/m/d',
        timepicker:false,
        withoutBottomPanel: true,
        scrollInput:false,//取消input滾動調整日期功能
        dayOfWeekStart:1,
        onShow:function( ct ){
            this.setOptions({
                minDate:jQuery('.JdatepickerS').val()?jQuery('.JdatepickerS').val():false
            })
        },
        //yearEnd:2022,//下拉選單最末年份
    });

        //colorbox
        var WinHeight = $("html").height();
        var ColHeight = WinHeight * 0.6;
        var ColHeightL = WinHeight * 0.85;
        $(".colorboxS").colorbox({inline:true, width:"100%", maxWidth:"500", maxHeight:"400", opacity:0.5});
        $(".colorboxM").colorbox({inline:true, width:"100%", maxWidth:"800", maxHeight:ColHeight, opacity:0.5});
        $(".colorboxL").colorbox({inline:true, width:"100%", maxWidth:"900", opacity:0.5});
        //關閉colorbox
        $(".closecolorbox").click(function(){
            $.colorbox.close()
        })
        $(".colorboxS,.colorboxM,.colorboxL,.colorboxGen,.colorboxiframe").colorbox({
            title: function(){
                return $(this).attr('data-colorboxtitle');
            }});
        //修改外框
        $("#cboxTopLeft").hide();
        $("#cboxTopRight").hide();
        $("#cboxBottomLeft").hide();
        $("#cboxBottomRight").hide();
        $("#cboxMiddleLeft").hide();
        $("#cboxMiddleRight").hide();
        $("#cboxTopCenter").hide();
        $("#cboxBottomCenter").hide();
        $("#cboxContent").addClass("colorboxnewborder");

        //Magnific Popup
        $('.open-popup-link').magnificPopup({
            type:'inline',
            midClick: false, // 是否使用滑鼠中鍵
            closeOnBgClick:true,//點擊背景關閉視窗
            showCloseBtn:true,//隱藏關閉按鈕
            fixedContentPos:true,//彈出視窗是否固定在畫面上
            mainClass: 'mfp-fade',//加入CSS淡入淡出效果
            tClose: '關閉',//翻譯字串
        });

        /*  Magnific Popup自動播放
        $.magnificPopup.open({
            items: {
                src: '#test-popup'//要自動播放的ID
            },
            type: 'inline'
         });
        */
        //Magnific Popup關閉動作
        $(".closemagnificPopup").click(function(){
            $.magnificPopup.close();
        });
	
	//滾動偵測:scrollmagic
	var controller = new ScrollMagic();
	//滾動偵測之GSAP動畫
	//宣告物件
	var mainmenuItem = $(".menuwrapper");
	var backTotopBtn = $(".backTop");
	
	
	//動畫內容

	var backtopbtnMV = TweenLite.to(backTotopBtn,0.2,{x:-100});

	//backtop按鈕
	var sceneBacktop = new ScrollScene({triggerElement: "#ContentWrapper"})
								.setTween(backtopbtnMV).triggerHook(0)
								.addTo(controller);
	//scrollmagic debug模式
	//sceneB2.addIndicators({zindex:100});
	
	
	//其他程式內容
	//錨點移動
	$(".backTotop").click(function(){backTotop();return false});



	//網站內容預設為桌機版(no-js)下狀態,RWD平板、手機內容以動態方式取代加入,故平板、手機內容需在JS內以變數方式設定
	var Htmlsidemenu = '<i class="fa fa-bars font-white" aria-hidden="true" id="toggle-sidebar"></i>';
	//設定斷點:判斷螢幕尺寸決定內容
	//桌機狀態:瀏覽器大於 980
	$.breakpoint({
        condition: function () {
            return window.matchMedia('all and (min-width:981px)').matches;
        },
        enter: function () {
            $(".superfishmenu").show();
			$("#opensidemenu").html("");
			
        },
    });
	
	//平板手機狀態:瀏覽器介於 980~480
	$.breakpoint({
        condition: function () {
            return window.matchMedia('all and (max-width:980px) and (min-width:480px)').matches;
        },
        enter: function () {
            $(".superfishmenu").hide();
			$("#opensidemenu").html(Htmlsidemenu);

        },

    });
	
	//手機狀態:瀏覽器小於 480 且為橫式
	$.breakpoint({
	condition: function () {
            return window.matchMedia('all and (max-width:479px) and (orientation:landscape)').matches;
        },
        enter: function () {
            $(".superfishmenu").hide();
			$("#opensidemenu").html(Htmlsidemenu);
		
        },
	 });
	 
	 //手機狀態:尺寸小於 480 且為直立 orientation:portrait
	$.breakpoint({
        condition: function () {
            return window.matchMedia('all and (max-width:479px) and (orientation:portrait)').matches;
        },
        enter: function () {
            $(".superfishmenu").hide();
			$("#opensidemenu").html(Htmlsidemenu);

        },

    });
	
	}//MainFunction
	
	/* ======== 自訂程式詳細內容 ======== */
	//預載動畫
	function PreloadFunction(){
		$('#status').fadeOut(); // will first fade out the loading animation
		$('#preloader').delay(350).fadeOut('slow'); // will fade out the white DIV that covers the website.
		$('body').delay(350);
		animateFunction();//畫面讀取完畢後直接執行的動畫
	}//預載動畫END
	
	//畫面讀取完畢後直接執行的動畫
	function animateFunction(){
		
		
	}

	
//錨點移動:use Jquery Plugin animatescroll
	function backTotop(){
		$('#WrapperBody').animatescroll({scrollSpeed:1000,padding:0})
	}

	
