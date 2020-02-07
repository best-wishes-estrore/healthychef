using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace BayshoreSolutions.WebModules.SlideShow
{
    public enum ImageDisplayOrder
    {
        sequential = 0,
        random = 1
    }
	public enum NavType
	{
		none = 0,
		per_slide = 1,
		prev_next = 2,
		per_slide_with_prev_next = 3
	}
	public enum WrapType
	{
		none = 0,
		first = 1,
		last = 2,
		both = 3,
		circular = 4
	}

    public partial class SlideShowModule
    {
        public static SlideShowModule GetByModuleId(int ModuleId)
        {
            SlideShowModuleCollection col =  new SlideShowModuleController().FetchByID(ModuleId);
            if (col.Count > 0)
                return col[0];
            else
                return null;
        }
        public string GetCss(string slideshow_div_clientId)
        {
            return @"
            <style type=""text/css"">
                #" + slideshow_div_clientId + @"
                { /* these properties must not conflict with '.wm_slideshow_module img' defined in slideshow2.css. */
	            /*border: 1px solid #000;*/
	        /*cursor: pointer;*/
	        overflow: hidden;
	        /*margin: 50px auto 10px;*/
	        position: relative;
	        width: " + this.Width + @"px;
	        height: " + this.Height + @"px;
            }

        #" + slideshow_div_clientId + @" img
        {
	        border: 0;
	        /*cursor: pointer;*/
	        width: " + this.Width + @"px;
	        height: " + this.Height + @"px;
        }
        </style>
        ";
        }

//#slider-img .jcarousel-clip,
//#slider-img ul,
//#slider-img ul li { list-style: none; position: relative; overflow: hidden; }

		public string GetJCarouselCss(string slideshow_div_clientId)
		{
			return @"
            <style type=""text/css"">
            #" + slideshow_div_clientId + @" div.jcarousel-container
            { /* these properties must not conflict with '.wm_slideshow_module img' defined in slideshow2.css. */
	        /*border: 1px solid #000;*/
	        /*cursor: pointer;*/
	        overflow: hidden;
	        /*margin: 50px auto 10px;*/
	        position: relative;
	        width: " + this.Width + @"px;
	        height: " + (this.Height) + @"px;
			float: left;
            }

			#" + slideshow_div_clientId + @" ul,
			#" + slideshow_div_clientId + @" ul li
			{
				list-style: none;
				position: relative;
				display: block;
				overflow: hidden;
				padding: 0;
			}

			#" + slideshow_div_clientId + @" img
			{
				border: 0;
				/*cursor: pointer;*/
				width: " + this.Width + @"px;
				height: " + this.Height + @"px
				padding: 0;
			}

			#" + slideshow_div_clientId + @" div.c-holder
			{
				clear: both;
				border: 0;
				/*cursor: pointer;*/
				width: " + this.Width + @"px;
			}
			</style>
        ";
		}

		public string GetJCarouselTextOnlySliderCss(string slideshow_div_clientId)
		{
			return @"
            <style type=""text/css"">
            #" + slideshow_div_clientId + @" div.jcarousel-container
            { /* these properties must not conflict with '.wm_slideshow_module img' defined in slideshow2.css. */
	        /*border: 1px solid #000;*/
	        /*cursor: pointer;*/
	        overflow: hidden;
	        /*margin: 50px auto 10px;*/
	        position: relative;
	        width: " + this.Width + @"px;
	        height: " + (this.Height) + @"px;
			float: left;
            }

            #" + slideshow_div_clientId + @" .jcarousel-clip
            { /* these properties must not conflict with '.wm_slideshow_module img' defined in slideshow2.css. */
	        width: " + (this.Width) + @"px;
	        height: " + (this.Height) + @"px;
            }

			#" + slideshow_div_clientId + @" ul,
			#" + slideshow_div_clientId + @" ul li
			{
				list-style: none;
				position: relative;
				overflow: hidden;
				/*padding: 18px 18px 0 18px;*/ 
				width: " + (this.Width) + @"px;
				height: " + (this.Height) + @"px;
			}

			#" + slideshow_div_clientId + @" span.slide-content
			{
				border: 0;
				/*cursor: pointer;*/
				width: " + this.Width + @"px;
				height: " + this.Height + @"px
				padding: 0;
			}

			#" + slideshow_div_clientId + @" div.c-holder
			{
				clear: both;
				border: 0;
				/*cursor: pointer;*/
				width: " + this.Width + @"px;
			}
			</style>
        ";
		}

        public string GetInitScript(string slideshow_div_clientId)
        {
            //display time in milliseconds.
            int displayTime_ms = this.ImageDisplayTime * 1000;
            //fade time in milliseconds.
            int fadeTime_ms = this.ImageFadeTime * 1000;
            string displayOrder = ((ImageDisplayOrder)this.ImageDisplayOrder).ToString().ToLower();
            string initClosure = string.Format("function() {{ wm_slideshow_init('{0}', {1}, {2}, '{3}'); }}",
                slideshow_div_clientId,
                displayTime_ms,
                fadeTime_ms,
                displayOrder);
            return "window.addEventListener?window.addEventListener('load'," + initClosure + ",false):window.attachEvent('onload'," + initClosure + ");";
        }

		public string GetJCarouselInitScript(string slideshow_div_clientId)
		{
			string displayOrder = ((ImageDisplayOrder)this.ImageDisplayOrder).ToString().ToLower();
			string initClosure = @"$(function() {    
    if ($('#" + slideshow_div_clientId + @".slider').length) {
		$('#" + slideshow_div_clientId + @".slider ul').jcarousel({
			scroll: 1,
			auto: " + this.ImageDisplayTime + @",
			wrap: '" + ((WrapType)this.WrapType).ToString() + @"',
			itemFirstInCallback: mycarousel_" + slideshow_div_clientId + @"_itemFirstInCallback,
			itemVisibleInCallback: mycarousel_" + slideshow_div_clientId + @"_itemVisibleInCallback,
		    initCallback: mycarousel_" + slideshow_div_clientId + @"_initCallback,
			buttonNextHTML: null,
			buttonPrevHTML: null,
			start: 1
	    });
	};
    
    var leng = $('#" + slideshow_div_clientId + @" .c-holder a').length;
    var wid = $('#" + slideshow_div_clientId + @" .c-holder a').width();
    var cnt = (leng * (wid +4));
    $('#" + slideshow_div_clientId + @" .cent').width(cnt);
});

function mycarousel_" + slideshow_div_clientId + @"_initCallback(carousel) {
	$('#" + slideshow_div_clientId + @".slider .c-holder a').bind('click', function() {
		carousel.scroll($.jcarousel.intval($(this).text()));
		return false;
	});

	$('#" + slideshow_div_clientId + @".slider .slider-navigation a.next').click(function() {
		carousel.next();
		return false;
	});
	
	$('#" + slideshow_div_clientId + @".slider .slider-navigation a.prev').click(function() {
		carousel.prev();
		return false;
	});	
};

function mycarousel_" + slideshow_div_clientId + @"_itemFirstInCallback(carousel, item, idx, state) {
	var slideIdx = idx%carousel.size();
	
	if (slideIdx == 0)
		slideIdx = carousel.size();
		
	$('#" + slideshow_div_clientId + @".slider .c-holder a').removeClass('active');
	$('#" + slideshow_div_clientId + @".slider .c-holder a').eq(slideIdx-1).addClass('active');
};

function mycarousel_" + slideshow_div_clientId + @"_itemVisibleInCallback(carousel, item, idx, state) {" + ((((SlideShow.WrapType)this.WrapType) == SlideShow.WrapType.none) || (((SlideShow.WrapType)this.WrapType) == SlideShow.WrapType.last) ? @"
	if (idx == 1) {
		$('#" + slideshow_div_clientId + @".slider .slider-navigation a.prev').addClass('off');
	}
	else {
		$('#" + slideshow_div_clientId + @".slider .slider-navigation a.prev').removeClass('off');
	}" : String.Empty) + ((((SlideShow.WrapType)this.WrapType) == SlideShow.WrapType.none) || (((SlideShow.WrapType)this.WrapType) == SlideShow.WrapType.first) ? @"

	if (idx == carousel.size()) {
		$('#" + slideshow_div_clientId + @".slider .slider-navigation a.next').addClass('off');
	}
	else {
		$('#" + slideshow_div_clientId + @".slider .slider-navigation a.next').removeClass('off');
	}" : String.Empty) + @"
}";
			return initClosure;
		}

		public string GetJCarouselTextSliderInitScript(string slideshow_div_clientId)
		{
			string displayOrder = ((ImageDisplayOrder)this.ImageDisplayOrder).ToString().ToLower();
			string initClosure = @"$(function() {    
    
	 if ($('#" + slideshow_div_clientId + @".text-slider').length) {
		$('#" + slideshow_div_clientId + @".text-slider ul').jcarousel({
			scroll: 1,
			auto: " + this.ImageDisplayTime + @",
			wrap: '" + ((WrapType)this.WrapType).ToString() + @"',
			visible: 1,
			itemFirstInCallback: mycarousel_" + slideshow_div_clientId + @"_itemFirstInCallback,
			itemVisibleInCallback: mycarousel_" + slideshow_div_clientId + @"_itemVisibleInCallback,
		    initCallback: mycarousel_" + slideshow_div_clientId + @"_initCallback2,
			buttonNextHTML: null,
			buttonPrevHTML: null,
			start: 1
	    });
	};
    
    var leng = $('#" + slideshow_div_clientId + @" .c-holder a').length;
    var wid = $('#" + slideshow_div_clientId + @" .c-holder a').width();
    var cnt = (leng * (wid +4));
    $('#" + slideshow_div_clientId + @" .cent').width(cnt);
});

function mycarousel_" + slideshow_div_clientId + @"_initCallback2(carousel) {
	$('#" + slideshow_div_clientId + @".text-slider .c-holder a').bind('click', function() {
		carousel.scroll($.jcarousel.intval($(this).text()));
		return false;
	});

	$('#" + slideshow_div_clientId + @".text-slider .slider-navigation a.next').click(function() {
		carousel.next();
		return false;
	});
	
	$('#" + slideshow_div_clientId + @".text-slider .slider-navigation a.prev').click(function() {
		carousel.prev();
		return false;
	});	
};

function mycarousel_" + slideshow_div_clientId + @"_itemFirstInCallback(carousel, item, idx, state) {
	var slideIdx = idx%carousel.size();
	
	if (slideIdx == 0)
		slideIdx = carousel.size();
		
	$('#" + slideshow_div_clientId + @".text-slider .c-holder a').removeClass('active');
	$('#" + slideshow_div_clientId + @".text-slider .c-holder a').eq(slideIdx-1).addClass('active');
};

function mycarousel_" + slideshow_div_clientId + @"_itemVisibleInCallback(carousel, item, idx, state) {" + ((((SlideShow.WrapType)this.WrapType) == SlideShow.WrapType.none) || (((SlideShow.WrapType)this.WrapType) == SlideShow.WrapType.last) ? @"
	if (idx == 1) {
		$('#" + slideshow_div_clientId + @".text-slider .slider-navigation a.prev').addClass('off');
	}
	else {
		$('#" + slideshow_div_clientId + @".text-slider .slider-navigation a.prev').removeClass('off');
	}" : String.Empty) + ((((SlideShow.WrapType)this.WrapType) == SlideShow.WrapType.none) || (((SlideShow.WrapType)this.WrapType) == SlideShow.WrapType.first) ? @"

	if (idx == carousel.size()) {
		$('#" + slideshow_div_clientId + @".text-slider .slider-navigation a.next').addClass('off');
	}
	else {
		$('#" + slideshow_div_clientId + @".text-slider .slider-navigation a.next').removeClass('off');
	}" : String.Empty) + @"
};";
			return initClosure;
		}
    }
    public partial class SlideShowImage
    {
        public static SlideShowImageCollection GetByModuleId(int moduleId)
        {
            return new SlideShowImageCollection()
                .Where("ModuleId", moduleId)
                .OrderByAsc(SlideShowImage.Columns.SortOrder)
                .Load();
        }
        public static SlideShowImage GetById(int imageId)
        {
            return new SlideShowImageController().FetchByID(imageId)[0];
        }

        public static SlideShowImageCollection GetByModuleId(int moduleId, string sortBy, string sortDirection)
        {
            string sortdir = String.IsNullOrEmpty(sortDirection) ? "asc" : sortDirection;

            SlideShowImageCollection col = new SlideShowImageCollection().Where("ModuleId", moduleId);
            if (String.IsNullOrEmpty(sortBy))
                return col.Load();
            if (sortdir == "desc")
                col= col.OrderByDesc(sortBy);
            else
                col= col.OrderByAsc(sortBy);
            return col.Load();

        }
        public static void Delete(int imageId)
        {
            new SlideShowImageController().Delete(imageId);
        }
        public static void MoveUp(int imageId)
        {
            MovePosition(imageId, false);
        }

        /// <summary>
        /// Increments the image sort order down, and resorts sibling images.
        /// </summary>
        public static void MoveDown(int imageId)
        {
            MovePosition(imageId, true);
        }

        private static void MovePosition(int imageId, bool direction)
        {
            SPs.SlideShowImageMovePosition(imageId, direction).Execute();
            
        }
        public string GetFullPath()
        {
            string strFullPath = string.Format("~/userfiles/webmodules_files/module_{0}/{1}",
                this.ModuleId,
                this.ImageFileName);
            return VirtualPathUtility.ToAppRelative(strFullPath);
        }
        public void Delete()
        {
             new SlideShowImageController().Delete(this.Id);
        }
    }
}
