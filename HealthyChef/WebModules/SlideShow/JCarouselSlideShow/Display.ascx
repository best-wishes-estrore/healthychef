<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Display.ascx.cs" Inherits="BayshoreSolutions.WebModules.SlideShow.JCarouselSlideShow_Display" %>

<asp:PlaceHolder ID="phSlideShow" runat="server">
	<div id="divSlideShow" class="slider" runat="server">
		<ul class="slides">
			<asp:ListView ID="lvSlideShow" runat="server">
				<ItemTemplate>
					<li>
						<a id="anchorLinkUrl" runat="server">
							<img src="<%# ResolveUrl(((BayshoreSolutions.WebModules.SlideShow.SlideShowImage)Container.DataItem).GetFullPath()) %>" alt="" />
						</a>
					</li>
				</ItemTemplate>
			</asp:ListView>
		</ul>
		<asp:PlaceHolder ID="phPerSlideNav" runat="server">
        <ol class="flex-control-nav">
		    <asp:ListView ID="lvSlideShowNav" runat="server">
			    <ItemTemplate>
                    <li>
				        <a id="aSlideNav" runat="server" />
                    </li>
			    </ItemTemplate>
		    </asp:ListView>
        </ol>
		</asp:PlaceHolder>
		<asp:PlaceHolder ID="phPrevNextNav" runat="server">
			<div class="slider-navigation">
				<a href="#" class="prev">&nbsp;&laquo;&nbsp;</a>
				<a href="#" class="next">&nbsp;&raquo;&nbsp;</a>
			</div>
		</asp:PlaceHolder>
	</div>
</asp:PlaceHolder>
