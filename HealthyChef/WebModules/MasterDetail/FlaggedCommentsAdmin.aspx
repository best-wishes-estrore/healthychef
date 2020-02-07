<%@ Page Title="Flagged Comments Admin" Language="C#" AutoEventWireup="true" MasterPageFile="~/Templates/WebModules/Module.Master"
	CodeBehind="FlaggedCommentsAdmin.aspx.cs" Inherits="BayshoreSolutions.WebModules.MasterDetail.FlaggedCommentsAdmin"
	Theme="WebModules" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
	<h3 style="display:inline;">Comments Flagged As Inappropriate</h3>
	<fieldset>
		<legend>Options</legend>
		<table >
			<tr>
				<td>
					Sort by: <asp:RadioButtonList ID="rblSortOrder" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
						<asp:ListItem Value="0" Selected="True">Date&nbsp;&nbsp;&nbsp;</asp:ListItem>
						<asp:ListItem Value="1">Number of flags</asp:ListItem>
					</asp:RadioButtonList>
				</td>
				<td></td>
			</tr>
			<tr>
				<td>
					Show Comments that have been flagged more than <asp:TextBox runat="server" ID="tbMinFlags" Text="5" Width="20" MaxLength="3" />time(s) 
				</td>
				<td>
					<asp:Button runat="server" ID="btnUpdateMinFlags" Text="Update" onclick="btnUpdateMinFlags_Click" />
				</td>
			</tr>
		</table>
	</fieldset>
	<br />
	<div align="right">
		<asp:Button runat="server" ID="btnDeleteAll" Text="Delete All Flagged Comments" onclick="btnDeleteAll_Click" OnClientClick="return confirm('Are you sure you want to delete ALL flagged comments shown on this page?');" />
	</div>
	<br />
	<asp:Repeater runat="server" ID="CommentsListRepeater" onitemdatabound="CommentsListRepeater_ItemDataBound" 
		onitemcommand="CommentsListRepeater_ItemCommand">
		<ItemTemplate>
		
			<div class="MasterDetail_Comments">
				<table width=100%>
					<tr>
						<td>
							<asp:HiddenField runat="server" ID="hidCommentRecId" />
							<asp:Label runat="server" ID="lblName" CssClass="MasterDetail_CommentsName" /> <asp:Label runat="server" ID="lblTimestamp" CssClass="MasterDetail_CommentsDate" />
						</td>
						<td align="right" NOWRAP>
							<asp:Label runat="server" ID="lblFlags" Font-Bold="true" ForeColor="Red" />
						</td>
					</tr>
					<tr>
						<td>
							<asp:Label runat="server" ID="lblText" CssClass="MasterDetail_CommentsText" />
						</td>
						<td align="right" NOWRAP>
							<asp:Button runat="server" ID="btnDelete" Text="Delete"  CausesValidation="false"
									CommandName="Delete" CommandArgument='<%# Eval("Id") %>' />
							&nbsp;&nbsp;
							<asp:Button runat="server" ID="btnAccept" Text="Allow" CausesValidation="false" 
									CommandName="Accept" CommandArgument='<%# Eval("Id") %>'/>
						</td>
					</tr>
					<tr>
					</tr>
				</table>
			</div>
			<br />
		</ItemTemplate>
	</asp:Repeater>
</asp:Content>
