<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailPopup.ascx.cs" Inherits="BayshoreSolutions.WebModules.MasterDetail.Controls.EmailPopup" %>

<%-- ScriptManager required here for Update Panel & ModalPopupExtender inside EmailPopup control --%>
<%-- DEVELOPER NOTE: This can be removed if master pages declare a ScriptManager --%>
<%--<asp:ScriptManager ID="EmailPopupScriptManager" runat="server" EnablePartialRendering="true" />--%>


<%-- Email Popup --%>
<script>
	function HideEmailModalPopup() {
		document.getElementById('ModalPopupBehaviorEmail').style.display = 'none';
		return false;
	}
	function ValidateMaxCharsEmail(text, val) {
		var maxChars = new Number(val);
		var currCharCount = text.value.length;

		// Update character counter display
		document.getElementById("msgCounter").innerHTML = maxChars - currCharCount;

		// Alert user if count exceeds max
		if (currCharCount >= maxChars) {
			text.value = text.value.substring(0, maxChars);
			document.getElementById("msgCounter").innerHTML = 0;
			alert("Message is limited to " + val + " characters");
		}
	}
</script>
<asp:UpdatePanel ID="EmailUpdatePanel" runat="server" RenderMode="Inline">
	<ContentTemplate>
<a href="javascript:$find('ModalPopupBehaviorEmail').show();">Email</a>

<asp:Button id="EmailButton1" runat="server" style="display:none" CausesValidation="false"/>
<ajax:ModalPopupExtender ID="EmailModalPopupExtender" runat="server"
	BehaviorID="ModalPopupBehaviorEmail"
    TargetControlID="EmailButton1"
    PopupControlID="EmailPopupPanel"
    PopupDragHandleControlID="EmailDragPanel"
    BackgroundCssClass="modalBackground" 
    CancelControlID="btnClose" />
	<asp:Panel ID="EmailPopupPanel" runat="server" Width="460px" Height="305px" Style="display: none" CssClass="modalPanel">
		<asp:Panel ID="EmailDragPanel" runat="server" CssClass="modalHeader" HorizontalAlign="Left">
			<table width="100%">
				<tr>
					<td>
						<asp:Label runat="server" ID="lblPreviewHeader" Text="Send this article to a friend" Font-Bold="true" Font-Size="Larger" />
					</td>
					<td align="right">
						<asp:ImageButton ID="btnClose" runat="server" ImageUrl="~/WebModules/MasterDetail/public/images/close.gif" AlternateText="Close" onmouseover="this.style.cursor='default';" />
					</td>
				</tr>
			</table>
		</asp:Panel>    
		<asp:Panel ID="EmailPanel1" runat="server" Width="99%" Height="260px" CssClass="modalBody" HorizontalAlign="Left" >
			<table cellpadding="0" border="0" cellspacing="4" width="100%">
				<tr>
					<td nowrap>Your Name</td>
					<td width="90%">
						<asp:TextBox ID="tbSenderName" runat="server" MaxLength="80" Width="250" />
						<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbSenderName"
							ErrorMessage="Required" ValidationGroup="emailtoform" />
					</td>
				</tr>
				<tr>
					<td nowrap>Your Email</td>
					<td>
						<asp:TextBox ID="tbSenderEmail" runat="server" MaxLength="80" Width="250" />
						<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbSenderEmail"
							Display="Dynamic" ErrorMessage="Required" ValidationGroup="emailtoform" />
						<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tbSenderEmail"
							Display="Dynamic" ErrorMessage="Invalid e-mail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
							ValidationGroup="emailtoform" />
					</td>
				</tr>
				<tr>
					<td nowrap>Friend's Email</td>
					<td>
						<asp:TextBox ID="tbRecipientEmail" runat="server" MaxLength="80" Width="250" />
						<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbRecipientEmail"
							Display="Dynamic" ErrorMessage="Required" ValidationGroup="emailtoform" />
						<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="tbRecipientEmail"
							Display="Dynamic" ErrorMessage="Invalid e-mail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
							ValidationGroup="emailtoform" />
					</td>
				</tr>
				<tr>
					<td valign="top" nowrap>Message</td>
					<td>
						<asp:TextBox runat="server" ID="tbMessage" Rows="6" TextMode="MultiLine" Width="300"
							onKeyUp="ValidateMaxCharsEmail(this,250)" onChange="ValidateMaxCharsEmail(this,250)" />
					</td>
				</tr>
				<tr>
					<td></td>
					<td align="right">
						You have <b><span id="msgCounter">250</span></b> characters remaining for your message.
					</td>
				</tr>
				<tr>
					<td></td>
					<td>
						<asp:Button ID="btnSendMail" CausesValidation="true" runat="server" OnClick="EmailArticleLink"
							Text="Send Email Message" ValidationGroup="emailtoform" />
						&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:Button ID="btnCancelMail" runat="server" Text="Cancel" OnClientClick="javascript:$find('ModalPopupBehaviorEmail').hide(); return false;" />
					</td>
				</tr>
			</table>
		</asp:Panel>
	</asp:Panel>    				
	
	</ContentTemplate>
</asp:UpdatePanel>
