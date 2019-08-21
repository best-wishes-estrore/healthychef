//
//    Adapted from:
//    http://slayeroffice.com/code/imageCrossFade/index.html
//    http://sonspring.com/journal/slideshow-alternative
//
//    Modifications by Justin M. Keyes:
//      - parameterized to allow multiple instances with different settings.
//      - optional "random" feature.
//      - configurable display time and fade time.
//      - added more comments.
//

function wm_slideshow_init(container_id, displayTime/*ms*/, fadeTime/*ms*/, displayOrder) {
    var d = document, imgs = [], container = null, css = null;
	if(!d.getElementById || !d.createElement)return;

    css = d.createElement('link');
    css.setAttribute('href', wm_slideshow_resolve_url('~/WebModules/SlideShow/public/js/slideshow2.css'));
    css.setAttribute('rel','stylesheet');
    css.setAttribute('type','text/css');
	d.getElementsByTagName('head')[0].appendChild(css);

	container = d.getElementById(container_id);
   if (!container) return;
	imgs = container.getElementsByTagName('img');
	if (imgs.length > 0) {
	    for (i = 1; i < imgs.length; i++) imgs[i].xOpacity = 0;
	    imgs[0].style.display = 'block';
	    imgs[0].xOpacity = .99;
	    setTimeout(function() { wm_slideshow_xfade(imgs, 0, 1, displayTime, fadeTime, displayOrder) }, displayTime);
	}
}

function wm_slideshow_xfade(imgs, currentIndex, nextIndex, displayTime/*ms*/, fadeTime/*ms*/, displayOrder) {
	//opacity is incremented by 5% on each pass, so fadeIncrementTime is 1/20 of total fadeTime.
	var fadeIncrementTime = fadeTime/20;
	var cOpacity = imgs[currentIndex].xOpacity;
	//if nextIndex is invalid, restart the loop.
	if (!imgs[nextIndex]) nextIndex = 0;
    var nOpacity = imgs[nextIndex].xOpacity;

	//increment by 5% on each pass.
	cOpacity-=.05;
	nOpacity+=.05;

	imgs[nextIndex].style.display = 'block';
	imgs[currentIndex].xOpacity = cOpacity;
	imgs[nextIndex].xOpacity = nOpacity;

	setOpacity(imgs[currentIndex]);
	setOpacity(imgs[nextIndex]);

	if(cOpacity<=0) {
		imgs[currentIndex].style.display = 'none';
		currentIndex = nextIndex;
		
		if (displayOrder == "random") {
		    do { nextIndex = Math.floor(imgs.length * Math.random()); }
		    while (nextIndex == currentIndex);
		}
		else {
		    nextIndex++;
		}

		setTimeout(wm_slideshow_xfade_again, displayTime);
	}
	else {
	    setTimeout(wm_slideshow_xfade_again, fadeIncrementTime);
	}

	function wm_slideshow_xfade_again() {
	    wm_slideshow_xfade(imgs, currentIndex, nextIndex, displayTime, fadeTime, displayOrder); 
	}

	function setOpacity(obj) {
		if(obj.xOpacity>.99) {
			obj.xOpacity = .99;
			return;
		}

		obj.style.opacity = obj.xOpacity;
		obj.style.MozOpacity = obj.xOpacity;
		obj.style.filter = 'alpha(opacity=' + (obj.xOpacity*100) + ')';
	}
}
