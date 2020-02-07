<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/WebModules/Module.master" AutoEventWireup="true" CodeBehind="MiniSummaryEdit.aspx.cs" Inherits="BayshoreSolutions.WebModules.MasterDetail.MiniSummaryEdit" Theme="WebModules" %>
<%@ Register Src="~/WebModules/Components/PagePicker/PagePicker.ascx" TagName="PagePickerControl" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
	<p class="MasterDetail_AdminHelp">
		<span style="color: #6CC2DD;">Module Description:<br />
		</span>This module displays a brief, one line summary for each detail item found on the Starting page specified, and all subpages below it.
		<br />
	</p>

	<table>
		<tr>
			<td>Starting page</td>
			<td>
				<uc1:PagePickerControl ID="PagePicker1" runat="server" />
			</td>
		</tr>
		<tr>
			<td>Num rows to display</td>
			<td>
				<asp:TextBox runat="server" ID="tbNumRows" Text="10" Width="20" />
			</td>
		</tr>
		<tr>
			<td></td>
			<td>
				<asp:CheckBox runat="server" ID="cbShowElapsedTime" Text="Show Elapsed Time" Checked="true" />
			</td>
		</tr>
		<tr>
			<td></td>
			<td>
				<asp:CheckBox runat="server" ID="cbShowFeaturedOnly" Text="Show Featured Only" Checked="false" />
			</td>
		</tr>
		<tr>
			<td></td>
			<td>
				<br />
				<asp:Button runat="server" ID="btnSubmit" Text="Save Settings" onclick="btnSubmit_Click"/>
				&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				<asp:Button runat="server" ID="btnCancel" Text="Cancel" onclick="btnCancel_Click"/>
			</td>	
		</tr>
	</table>

</asp:Content>
