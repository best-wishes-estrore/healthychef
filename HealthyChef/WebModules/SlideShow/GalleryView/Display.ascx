<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Display.ascx.cs" Inherits="BayshoreSolutions.WebModules.SlideShow.GalleryView.Display" %>
<%@ Import Namespace="BayshoreSolutions.WebModules.SlideShow" %>
<asp:Repeater id="rptImages" runat="server" >
    <HeaderTemplate>
        <ul id='<%= GalleryId %>'>
    </HeaderTemplate>
    <ItemTemplate>
        <li><img frame='<%# ResolveUrl(((SlideShowImage)Container.DataItem).GetFullPath() + ".ashx?width=70&height=40&crop=auto") %>' src='<%# ResolveUrl(((SlideShowImage)Container.DataItem).GetFullPath()) %>' alt='<%# Eval("SlideTextContentName") %>' title='<%# Eval("SlideTextContentName") %>' longdesc='<%# Eval("SlideTextContent") %>' /></li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>
