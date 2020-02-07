<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MiniSummaryDisplay.ascx.cs" Inherits="BayshoreSolutions.WebModules.MasterDetail.MiniSummaryDisplay" %>

<div class="MasterDetail_MiniSummary">
	<ul class="MasterDetail_MiniSummaryList">
	<asp:Repeater runat="server" ID="MiniSummaryRepeater" onitemdatabound="MiniSummaryRepeater_ItemDataBound" >
		<ItemTemplate>
			<li id="liContainer" runat="server" >
				<asp:Label runat="server" ID="lblCategory" CssClass="MasterDetail_MiniSummaryCategory" />
				<asp:HyperLink runat="server" ID="hypTitle" CssClass="MasterDetail_MiniSummaryTitle" />
				<asp:Label runat="server" ID="lblCommentCount" CssClass="MasterDetail_MiniSummaryComment"/>
				<asp:Label runat="server" ID="lblTimestamp" CssClass="MasterDetail_MiniSummaryTimestamp"/>
			</li>
		</ItemTemplate>
	</asp:Repeater>
	</ul>
</div>
