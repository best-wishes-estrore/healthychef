FlashSlideShow module
===============================================================================

Only JsSlideShow supports hyperlinks for each slide; FlashSlideShow does not.


MODULE INSTALLATION AND CONFIGURATION
===============================================================================
Application database:
    Run db/Install.sql on the application database.

Web.config:
    <httpHandlers>
        <!-- flash slideshow handler outputs images XML. -->
        <add verb="*" path="slideshowimagehandler.ashx" type="BayshoreSolutions.WebModules.SlideShow.ImagesXml" />
    
    <system.webServices>
		<validation />
		<modules>...</modules
		<handlers>
			<!-- add this line to support IIS7 -->
		    <add name="slideshow" verb="*" path="slideshowimagehandler.ashx" type="BayshoreSolutions.WebModules.SlideShow.ImagesXml" preCondition="integratedMode"/>



TO DO
===============================================================================
- add linking feature to javascript slideshow
- user currently cannot disable the "loop" feature in javascript slideshow.

Version