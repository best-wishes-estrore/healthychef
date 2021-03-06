﻿//
//  this script keeps the asp:Menu static menu item hover-image
//  highlighted while the user is hovering sub-menus. requires jquery.
//
//  the client id of the asp:Menu must be provided as a variable above
//  this script on the calling page. example:
//      <script type="text/javascript">
//      var _mainNavMenuId = "<%= MainNavMenu.ClientID %>";
//      </script>
//      <script src="/client/MainNavMenu_preserveHighlightWhileHoveringSubmenus.js"></script>
//
//  jkeyes 20090427
//

//the menu item ids generated by ASP.NET are like 
//"ctl00_header_MainNav1_MainNavMenun15". the n15 part tells us the index.
function getMenuItemIndex(menuItemId){
    var nIndex = -1; //index of the last 'n' character.
    var menuItemIndex = -1;
    if (menuItemId 
        && 0 <= (nIndex = menuItemId.lastIndexOf('n')) 
        && (nIndex+1) < menuItemId.length) {
        //chop the id so "ctl00_header_MainNav1_MainNavMenun0123abc" becomes "0123abc".
        var choppedId = menuItemId.substring(nIndex + 1, menuItemId.length);
        //get the number part via regex.
        var r = new RegExp("\\d+");
        var m = r.exec(choppedId);
        if (m && m.length>0){
            menuItemIndex = parseInt(m[0]);
        }
    }
    
    return menuItemIndex;
}

var _currentHoverImg = null; //jquery object.
var _currentHoverContainer = null; //jquery object.
var _timeoutId = null;

//get the element which contains the image rollover associated with the 
//currently-hovered menu 
function getMatchingHoverContainer(menuItemDivIndex){
    var imgContainerIndex = -1;
    var hoverContainer = null;
    //_mainNavMenuId is a variable provided by MainNavControl.ascx.
    $("#" + _mainNavMenuId + " > tbody > tr > td").each(function() {
        imgContainerIndex = getMenuItemIndex($(this).get(0).id);
        if (menuItemDivIndex == imgContainerIndex) {
            hoverContainer = $(this);
        }
    });
    return hoverContainer;
}

$(document).ready(function() {
    //set up mouseover handler.
    $(".mainNavDropDown > table > tbody > tr").mouseover(function() {    
        //pass the DOM object (instead of the jQuery object) to it.
        var domObject = $(this).get(0);
        var hoverMenuItemDiv = null;
        var hoverMenuItemDiv_jqObj = null; //jquery object.
        var tmpDomObj = null;

        //crawl up parents until we find the enclosing <div>. fragile but works ok.
        var hoverMenuItemDiv_jqObj = $(this).parent();
        while (hoverMenuItemDiv_jqObj) {
            tmpDomObj = $(hoverMenuItemDiv_jqObj).get(0);
            if (tmpDomObj.tagName && tmpDomObj.tagName.toLowerCase() == "div") {
                hoverMenuItemDiv = tmpDomObj;
                break;
            }
            hoverMenuItemDiv_jqObj = $(hoverMenuItemDiv_jqObj).parent();
        }

        //sanity check.
        if (!hoverMenuItemDiv_jqObj) return; // alert("ancestor div not found."); //we failed.
        if (!hoverMenuItemDiv.id) return; // alert("parent div does not have an id."); //we failed.

        var menuItemDivIndex = getMenuItemIndex(hoverMenuItemDiv.id);
        var hoverContainer = getMatchingHoverContainer(menuItemDivIndex);

        //if hover container not found, the user is hovering a submenu.
        //submenus are placed as *siblings* to their parents. yes, really.
        //(the ASP.NET javascript is even uglier than this...)
        var previousSibling = $(hoverMenuItemDiv_jqObj).prev();
        var previousSiblingIndex = -1;
        while (!hoverContainer) {
            hoverMenuItemDiv = $(previousSibling).get(0);
            if (!hoverMenuItemDiv) break; //fail
            menuItemDivIndex = getMenuItemIndex(hoverMenuItemDiv.id);
            hoverContainer = getMatchingHoverContainer(menuItemDivIndex);
            previousSibling = $(previousSibling).prev();
        }

        //sanity check.
        if (!hoverContainer) return; // alert("hoverContainer not found."); //we failed.

        //get the image element.
        var imgEl = null;
        var imgQuery = "#" + $(hoverContainer).get(0).id + " .mainNavStaticMenuItem_img";
        var imgJq = $(imgQuery);
        var tmpDomObj_src = "";
        if ($(imgJq).length > 0) {
            //set to the new item.
            imgEl = $(imgJq).get(0);

            if (imgEl) {
                //if we are still in the same menu, we don't need to swap.
                if (_currentHoverImg && imgEl.id == _currentHoverImg.get(0).id) {
                    if (_timeoutId) //cancel the pending image change 
                        clearTimeout(_timeoutId); 
                }
                else {
                    _currentHoverImg = imgJq;
                    _currentHoverContainer = hoverContainer; 

                    //swap the img src to its "on" image.
                    tmpDomObj_src = imgEl.src;
                    imgEl.src = imgEl.lang;
                    imgEl.lang = tmpDomObj_src;
                    $(hoverContainer).get(0).style.background = "#5393d9";
                }
            }
        }
    });

    //set up mouseout handler.
    $(".mainNavDropDown > table > tbody > tr").mouseout(function() {
        //wait a little bit, to prevent flicker in IE.
        //if we are still in the same sub menu (but hovering a different item)
        //during the next 500 ms, then this timeout callback will be cancelled.
        _timeoutId = setTimeout('resetNonHoverImage();', 500);
    });
});

function resetNonHoverImage() {
    var tmpOldDomObj = null;
    var tmpOldDomObj_src = "";
    //reset old item.
    if (_currentHoverImg) {
        tmpOldDomObj = $(_currentHoverImg).get(0);
        //swap the img src to its "off" image.
        tmpOldDomObj_src = tmpOldDomObj.src;
        tmpOldDomObj.src = tmpOldDomObj.lang;
        tmpOldDomObj.lang = tmpOldDomObj_src;
        tmpOldDomObj.style.background = "";
        $(_currentHoverContainer).get(0).style.background = "";
    }
}
