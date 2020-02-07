<%@ Page Title="Edit Content Item" Language="C#" AutoEventWireup="true" MasterPageFile="~/Templates/WebModules/Module.Master"
	CodeBehind="SummaryEdit.aspx.cs" Inherits="BayshoreSolutions.WebModules.MasterDetail.SummaryEdit" Theme="WebModules"
	ValidateRequest="false" %>
<%@ Register Src="Controls/DetailEditControl.ascx" TagName="DetailEditControl" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
	<p class="MasterDetail_AdminHelp">
		<span style="color: #6CC2DD;">Module Description:<br />
		</span>This module can be used to create master/detail pages that are ideal for purposes like Press Releases, News Articles,
		Corporate Biographies, and Case Studies.
		<br />
		<br />
		A summary page is presented to the user containing a short description of each detail item, along with a thumbnail photo.
		The user clicks a link to "Read more", and can then view the full content managed detail page.
	</p>
	<bss:MessageBox ID="Msg" runat="server" />
	<asp:MultiView ID="MultiViewCtl" runat="server">
		<asp:View ID="ListView" runat="server">
			<fieldset>
				<legend>Settings</legend>
				<table width="100%">
					<tr>
						<td nowrap>
							<asp:CheckBox ID="cbAllowComments" runat="server" Checked="true" Text="Display User Comments" />
						</td>
						<td>
							Show
							<asp:TextBox ID="txtItemsPerPage" runat="server" Width="24" />items per Summary page
							<asp:RequiredFieldValidator ID="rfvItemsPerPage" runat="server" Text="Required" ErrorMessage="Required" ControlToValidate="txtItemsPerPage"
								Display="Dynamic" SetFocusOnError="true">
							</asp:RequiredFieldValidator>
							<asp:RangeValidator ID="rvItemsPerPage" runat="server" ErrorMessage="<br>Please choose a non-negative number, 0 for no paging"
								ControlToValidate="txtItemsPerPage" Display="Dynamic" SetFocusOnError="true" MinimumValue="0" MaximumValue="999999" Type="Integer" />
						</td>
					</tr>
					<tr>
						<td nowrap>
							<asp:CheckBox ID="cbRequireAuthentication" runat="server" Checked="true" Text="Require Login for Comments" />
						</td>
						<td>
							<asp:CheckBox ID="IsPostDateVisible" runat="server" Checked="true" Text="Show Post Date" />
						</td>
					</tr>
					<tr>
						<td nowrap>
							<asp:CheckBox ID="cbTagFilter" runat="server" Checked="false" Text="Display Tag Filter" />
						</td>
						<td>
						    <asp:CheckBox ID="cbShowImageIfBlank" runat="server" Checked="false" Text="Show Image Space If Blank" />
						</td>
					</tr>
                    <tr>
                        <td nowrap>
                            Detail Item Template to use <asp:DropDownList ID="dlTemplateList" runat="server" />
						</td>
                        <td>&nbsp;</td>
					</tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>
					<tr>
			<td colspan="2" align="center">
							<asp:Button ID="MasterDetail_List_SaveButton" runat="server" CausesValidation="true" Text="Save Settings" ValidationGroup=""
								OnClick="MasterDetail_List_SaveButton_Click" />
						</td>
					</tr>
				</table>
			</fieldset>
			<div style="float: right;">
				<a href="FlaggedCommentsAdmin.aspx?<%=Request.QueryString %>" >View Flagged Comments</a>
			</div>
			<br />
			
			<table width=100%>
				<tr>
					<td>
						<h4>
							Detail Items</h4>
					</td>
					<td align="right">
						<asp:Button ID="CreateNewButton" runat="server" Text="Add New Detail Item" OnClick="CreateNewButton_Click" />
					</td>
				</tr>
				<tr>
					<td colspan="2">
						<asp:GridView ID="MasterDetail_Item_List" DataKeyNames="Id" runat="server" AutoGenerateColumns="False" CellPadding="1" AllowPaging="True"
							AllowSorting="True" Width="100%" PageSize="50" OnPageIndexChanging="MasterDetail_Item_List_PageIndexChanging" OnRowDeleting="MasterDetail_Item_List_OnRowDeleting"
							OnRowCommand="MasterDetail_Item_List_OnRowCommand" OnRowDataBound="MasterDetail_Item_List_RowDataBound" AlternatingRowStyle-BackColor="#ebebeb"
							SkinID="DetailSkin">
							<Columns>
								<asp:TemplateField>
									<ItemTemplate>
										<asp:ImageButton ID="MoveUpButton" runat="server" ImageUrl="~/WebModules/Admin/Images/Icons/Small/UpArrow.gif" ToolTip="Move up"
											CommandArgument='<%# Eval("Id") %>' CommandName="MoveUp" /><asp:ImageButton ID="MoveDownButton" runat="server" ImageUrl="~/WebModules/Admin/Images/Icons/Small/DownArrow.gif"
												ToolTip="Move down" CommandArgument='<%# Eval("Id") %>' CommandName="MoveDown" />
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Title">
									<ItemTemplate>
										<a id="SelectLink" runat="server" />
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Visible" ItemStyle-HorizontalAlign="Center">
									<ItemTemplate>
										<asp:CheckBox ID="VisibleCtl" runat="server" Enabled="false" />
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Featured" ItemStyle-HorizontalAlign="Center">
									<ItemTemplate>
										<asp:CheckBox ID="FeaturedCtl" runat="server" Enabled="false" />
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Post Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
									<ItemTemplate>
										<asp:Literal ID="PostDateCtl" runat="server" />
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField ItemStyle-HorizontalAlign="Center">
									<ItemTemplate>
										<asp:ImageButton ID="DeleteButton" ImageUrl="~/WebModules/MasterDetail/public/images/Delete.gif" 
											CommandName="Delete" CommandArgument='<%# Eval("Id") %>' 
											OnClientClick="return confirm('Delete the item?')"
											ToolTip="Delete" runat="server" CausesValidation="false" />
									</ItemTemplate>
								</asp:TemplateField>
							</Columns>
							<PagerStyle HorizontalAlign="Right" />
							<PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" NextPageText="Next &gt;" PreviousPageText="&lt; Prev" LastPageText="Last &gt;&gt;"
								FirstPageText="&lt;&lt; First" PageButtonCount="20" />
						</asp:GridView>
					</td>
				</tr>
			</table>
			<%-- Database-side GridView-paging requires an ObjectDataSource. --%>
			<%--<asp:ObjectDataSource ID="MasterDetail_ItemResource_PagingDataSource" runat="server" 
            TypeName="BayshoreSolutions.WebModules.Cms.MasterDetail.MasterDetail_ItemResource_edit"
            EnablePaging="true"
            SelectMethod="GetAllUsers" 
            SelectCountMethod="GetAllUsersCount" >
        </asp:ObjectDataSource>--%>
		</asp:View>
		<asp:View ID="EditView" runat="server">
			<div class="entity_edit">
				<uc1:DetailEditControl ID="DetailEditControl1" runat="server" />
				<div class="toolbar">
					<asp:Button ID="MasterDetail_SaveButton" runat="server" CssClass="saveButton" CausesValidation="true" Text="Save" ValidationGroup=""
						OnClick="MasterDetail_SaveButton_Click" />
					<asp:Button ID="MasterDetail_CancelButton" runat="server" CssClass="cancelButton" CausesValidation="false" Text="Cancel"
						OnClick="MasterDetail_CancelButton_Click" />
				</div>
			</div>
		</asp:View>
	</asp:MultiView>
</asp:Content>
