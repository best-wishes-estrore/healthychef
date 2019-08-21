<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DetailEditControl.ascx.cs" Inherits="BayshoreSolutions.WebModules.MasterDetail.Controls.DetailEditControl" %>
<%@ Register Src="~/WebModules/Components/TextEditor/TextEditor/TextEditorControl.ascx" TagName="TextEditor" TagPrefix="uc1" %>
<%@ Register Src="~/WebModules/Components/ImagePicker/ImagePicker.ascx" TagName="ImagePicker" TagPrefix="uc1" %>

<script language="javascript" type="text/javascript">
	function ValidateMaxCharsEditControl(text, val) {
		var maxChars = new Number(val);
		var currCharCount = text.value.length;

		// Alert user if count exceeds max
		if (currCharCount >= maxChars) {
			text.value = text.value.substring(0, maxChars);
			document.getElementById("cmtCounter").innerHTML = 0;
			alert("Cannot exceed " + val + " characters");
		}
	}
</script>

<fieldset>
	<legend>Settings</legend>
<table width="100%">
	<tr>
		<td>
			<asp:CheckBox ID="cbIsVisible" runat="server" Text="Visible in navigation" Checked="true" />
		</td>
		<td>
			<asp:CheckBox ID="cbIsFeatured" runat="server" Text="Featured" />
		</td>
		<td>
			Post Date
			<asp:RequiredFieldValidator ID="PostDateRequiredFieldValidator" runat="server" ControlToValidate="calPostDate" Display="Dynamic"
				ErrorMessage="Required" />
			<br />
			<bss:PopupCalendar ID="calPostDate" runat="server" />
		</td>
		<td>
			Post Time
			<asp:RegularExpressionValidator ID="regexValidatorPostTime" runat="server" ControlToValidate="tbPostTime" ErrorMessage="<br>Please leave blank or enter a time as 'HH:MM AM' OR 'HH:MM PM', eg. 02:30 PM"
				ValidationExpression="^\s*[0-9]{1,2}:[0-9]{2}\s?(AM|PM|am|pm)$" Display="Dynamic">
			</asp:RegularExpressionValidator>
			<br />
			<asp:TextBox ID="tbPostTime" runat="server" Width="80" />
		</td>
		<td>
			Remove Date<br />
			<bss:PopupCalendar ID="calRemoveDate" runat="server" />
		</td>
	</tr>
</table>
</fieldset>

<div class="field">
	<strong>Headline</strong>
	<div class="help">
		Used for header link text in summary display as well as the content title in detail display.
	</div>
	<div>
		<asp:TextBox ID="tbPageNavigationText" runat="server" Width="100%" MaxLength="256" />
	</div>
</div>
<div class="field">
	<strong>Tags</strong>
	<div class="help">
		Comma-separated words or phrases used to filter articles when the "Filter" option is enabled on the Summary page. At least on tag required.
	</div>
	<div>
        <asp:RequiredFieldValidator ID="reqTags" runat="server" ErrorMessage="At least one tag is required. Default is 'General'." ControlToValidate="tbTags" />
		<asp:TextBox ID="tbTags" runat="server" Width="100%" MaxLength="256" Text="General" />
	</div>
</div>
<div class="field">
	<strong>Short Description</strong>
	<div class="help">
		Optional. If blank, a short description will be generated automatically from the Long Description. Max 256 chars.
	</div>
	<div>
		<uc1:TextEditor ID="tbShortDesc" runat="server" Height="200px" IsRequired="false" />
		<asp:CustomValidator ID="cvShortDesc" runat="server" ControlToValidate="tbShortDesc" ErrorMessage="Please enter no more than 256 characters"
			Display="Dynamic" SetFocusOnError="true" ValidateEmptyText="false" OnServerValidate="cvShortDesc_ServerValidate" />
	</div>
</div>
<div class="field">
	<strong>Long Description</strong>
	<div>
        <asp:CustomValidator ID="cvLongDesc" runat="server" 
            ErrorMessage="Please enter a description" ControlToValidate="tbLongDesc" 
            SetFocusOnError="True" ValidateEmptyText="True" Display="Dynamic" 
            onservervalidate="cvLongDesc_ServerValidate"></asp:CustomValidator>
		<uc1:TextEditor ID="tbLongDesc" runat="server" Height="350px" IsRequired="true" />
	</div>
</div>
<div class="field">
	<strong>Image</strong>
	<div>
		<uc1:ImagePicker ID="ImagePathCtl" runat="server" />
	</div>
</div>
<fieldset>
	<legend>SEO Optimizations (optional)</legend>
	<div style="background-color: #E7F3F8;">
		<div class="field">
			<span>Title Tag</span>
			<div class="help">
				If blank, Headline description will be used.
			</div>
			<div>
				<asp:TextBox ID="tbTitle" runat="server" Width="100%" MaxLength="256" />
			</div>
		</div>
		<div class="field">
			<span>META Keywords</span>
			<div>
				<asp:TextBox ID="tbMetaKeywords" runat="server" Width="100%" MaxLength="256" />
			</div>
		</div>
		<div class="field">
			<span>META Description</span>
			<div>
				<asp:TextBox ID="tbMetaDescription" runat="server" TextMode="MultiLine" Width="100%" Rows="3" onKeyUp="ValidateMaxCharsEditControl(this,256)"
					onChange="ValidateMaxCharsEditControl(this,256)" />
			</div>
		</div>
	</div>
</fieldset>
