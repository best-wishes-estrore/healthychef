<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SummaryDisplay.ascx.cs" Inherits="BayshoreSolutions.WebModules.MasterDetail.SummaryDisplay" %>

<table width=100%>
	<tr>
		<td align="right">
			<table>
				<tr>
					<td>
						<asp:Label runat="server" ID="lblFilter" Text="Show:" Visible="false" />
						<asp:DropDownList runat="server" ID="ddlFilter" Visible="false" AutoPostBack="true"
								OnSelectedIndexChanged="Filter_Changed" />				
					</td>
					<td>
						<div class="MasterDetail_RSS">
							<asp:HyperLink ID="RssHyperLink1" runat="server" ToolTip="View RSS feed">
								<img id="Img1" style="border: 0px none white; vertical-align: middle;" src="~/WebModules/MasterDetail/public/images/feed-icon-14x14.png" runat="server" alt="" /> RSS
							</asp:HyperLink>
						</div>
					</td>
				</tr>
			</table>

		</td>
	</tr>
	<tr>
		<td>
			<asp:Repeater ID="SummaryListRepeater" runat="server" OnItemDatabound="SummaryListRepeater_ItemDataBound">
				<ItemTemplate>
				
					<div runat="server" id="divSummary" class="MasterDetail_ListItem">
						<table width="100%">
							<tr runat="server" id="trFeaturedHeader" class="MasterDetail_FeaturedLabel" visible="false">
								<td colspan="2" align="center" >
									<asp:Label runat="server" ID="lblFeatured" Visible="true" Text="********* Featured **********" />
								</td>
							</tr>
							<tr valign=top>
								<td id="tdImageCol" runat="server">
									<asp:Image runat="server" ID="imgMain" CssClass="MasterDetail_Image" />
								</td>
								<td>
									<table width="100%">
										<tr>
											<td>
												<asp:HyperLink runat="server" ID="hypTitle" />
											</td>
										</tr>
										<tr>
											<td align="right">
												<asp:Label id="lblPostDate" runat="server" Font-Size="Smaller" />
											</td>
										</tr>
										<tr>
											<td>
												<asp:Literal ID="litSummary" runat="server" />
											</td>
										</tr>
										<tr>
											<td>
												<asp:HyperLink runat="server" ID="hypReadMore" Text="Read more" />
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</div>
					
				</ItemTemplate>
				<SeparatorTemplate><hr /></SeparatorTemplate>
			</asp:Repeater>

			<asp:Repeater ID="PagerRepeater" Runat="server" OnItemDataBound="PagerRepeater_ItemDataBound">
				<HeaderTemplate>
					<div class="MasterDetail_PageLabel">Page</div>
					<div class="MasterDetail_PageNumbers">
				</HeaderTemplate>
			    
				<ItemTemplate>
					<div id="divActivePageNumber" runat="server" visible="false" class="MasterDetail_Pagination"><%# Container.DataItem %>&nbsp;</div>
					<div id="divPageNumber" runat="server" visible="true" class="MasterDetail_Pagination">
						<asp:LinkButton ID="btnPage" runat="server"	CommandName="Page" CommandArgument="<%#Container.DataItem %>" >
							<%# Container.DataItem %>
						</asp:LinkButton>&nbsp;
					</div>
				</ItemTemplate>
			    
				<FooterTemplate>
					</div>
				</FooterTemplate>
			</asp:Repeater>

		</td>
	</tr>
</table>


