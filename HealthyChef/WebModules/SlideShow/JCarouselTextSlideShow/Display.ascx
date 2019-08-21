<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Display.ascx.cs" Inherits="BayshoreSolutions.WebModules.SlideShow.JCarouselTextSlideShow_Display" %>

<asp:PlaceHolder ID="phSlideShow" runat="server">
	<div id="divSlideShow" class="text-slider" runat="server">
		<ul>
			<asp:ListView ID="lvSlideShow" runat="server">
				<ItemTemplate>
					<li id="liSlide" runat="server">
						<div class="slide-content"><%# Eval("SlideTextContent") %></div>
					</li>
				</ItemTemplate>
			</asp:ListView>
		</ul>
		<asp:PlaceHolder ID="phPerSlideNav" runat="server">
			<div class="c-holder">
				<div class="cent">
					<asp:ListView ID="lvSlideShowNav" runat="server">
						<ItemTemplate>
							<a id="aSlideNav" runat="server" />
						</ItemTemplate>
					</asp:ListView>
					<div class="cl">&nbsp;</div>
				</div>
			</div>
		</asp:PlaceHolder>
		<asp:PlaceHolder ID="phPrevNextNav" runat="server">
			<div class="slider-navigation">
				<a href="#" class="prev">&nbsp;&laquo;&nbsp;</a>
				<a href="#" class="next">&nbsp;&raquo;&nbsp;</a>
			</div>
		</asp:PlaceHolder>
	</div>
</asp:PlaceHolder>
