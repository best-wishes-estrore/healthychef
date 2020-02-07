<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Display.ascx.cs" Inherits="BayshoreSolutions.WebModules.SlideShow.JsSlideShow_Display" %>

<div id="slideshow_div" class="wm_slideshow_module" runat="server">
    <asp:Repeater 
        ID="ImagesLinksList" 
        runat="server"
        OnItemDataBound="ImagesLinksList_ItemDataBound"
        >
        <ItemTemplate
            ><a id="anchorLinkUrl" runat="server"> <img src="<%# ResolveUrl(((BayshoreSolutions.WebModules.SlideShow.SlideShowImage)Container.DataItem).GetFullPath()) %>" alt="" /></a>
        </ItemTemplate>
    </asp:Repeater
></div>
