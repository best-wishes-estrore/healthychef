<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DetailDisplay.ascx.cs" Inherits="BayshoreSolutions.WebModules.MasterDetail.DetailDisplay" %>
<%@ Register src="Controls/EmailPopup.ascx" tagname="EmailPopup" tagprefix="uc1" %>

   
<div class="MasterDetail_ItemDetail">
	<table width="100%">
		<tr>
			<td>

    			<div align="right"><asp:Label ID="lblPostDate" runat="server" /><asp:Literal ID="litCommentsLink" runat="server" />&nbsp;&nbsp;|&nbsp;&nbsp;<uc1:EmailPopup ID="EmailPopup1" runat="server" />&nbsp;&nbsp;<a href="javascript:window.print();">Print</a></div>
						
    				
			</td>
		</tr>
		<tr>
			<td>
				<asp:Image ID="img" runat="server" />
			</td>
		</tr>
		<tr>
			<td>
				<h1 id="MasterDetail_ItemDetail_Title">
					<asp:Literal ID="litTitle" runat="server" />
				</h1>
			</td>
		</tr>
		<tr>
			<td>
				<p>
					<asp:Literal ID="litBody" runat="server" />
				</p>
			</td>
		</tr>
		<tr>
			<td>
				<asp:HyperLink runat="server" ID="hypBack" Text="<< Back" />
			</td>
		</tr>
	</table>
	<asp:Panel runat="server" ID="CommentsPanel" DefaultButton="btnCommentSubmit" >
		<a id="Comments" ></a>
		<h3>Reader Comments</h3>
	
		<asp:Repeater runat="server" ID="CommentsListRepeater" onitemdatabound="CommentsListRepeater_ItemDataBound" 
			onitemcommand="CommentsListRepeater_ItemCommand">
			<ItemTemplate>
			
				<div class="MasterDetail_Comments">
					<table width=100%>
						<tr>
							<td>
								<asp:Label runat="server" ID="lblName" CssClass="MasterDetail_CommentsName" /> <asp:Label runat="server" ID="lblTimestamp" CssClass="MasterDetail_CommentsDate" />
							</td>
							<td align="right">
								<asp:LinkButton runat="server" ID="btnFlag" Text="Flag Inappropriate" CssClass="MasterDetail_CommentsFlag" CommandName="Flag" />
							</td>
						</tr>
						<tr>
							<td colspan="2">
								<asp:Label runat="server" ID="lblText" CssClass="MasterDetail_CommentsText" />
							</td>
						</tr>
					</table>
				</div>
				<br />
			</ItemTemplate>
		</asp:Repeater>

		<div class="MasterDetail_Comments">

<%-- Anti-spam form fields to throw off bots.  This is a substitution for CAPTCHA --%>
<%-- If a bot fills in any of the fields hidden using css, we will reject the entire form --%>
<%-- The included EmailPopup.ascx control uses these instances for spam protection also. --%>
<style>
	.sc_style1 { display: none; }
</style>
<p class="sc_style1">
<input name="url" type="text" value=""/>
<input name="email" type="text" value=""/>
<textarea name="comment" id="comment" ></textarea>
</p>
<%-- End Anti-spam fields --%>

<script language="javascript" type="text/javascript">
function ValidateMaxChars(text,val) 
{
	var maxChars = new Number(val);
	var currCharCount = text.value.length;
	
	// Update character counter display
	document.getElementById("cmtCounter").innerHTML = maxChars - currCharCount;

	// Alert user if count exceeds max
	if (currCharCount >= maxChars) {
		text.value = text.value.substring(0, maxChars);
		document.getElementById("cmtCounter").innerHTML = 0;
		alert("Comment is limited to " + val + " characters");
	}
}
</script>
			<asp:Label ID="lblLoginMsg" runat="server" Visible="false" Text="If you would like to submit a comment, please login to your account." />
			<div id="submitCommentsDiv" runat="server">
			<h3>Submit your comment</h3>
			<table>
				<tr>
					<td>
						<asp:Label runat="server" ID="lblCommentName" Text="* Name" />
						<asp:RequiredFieldValidator runat="server" ID="aReq" ControlToValidate="tbCommentName" Display="Dynamic" ErrorMessage="&nbsp;&nbsp;Required"
							ValidationGroup="cGroup" />
					</td>
				</tr>
				<tr>
					<td>
						<asp:TextBox ID="tbCommentName" Width="250px" MaxLength="128" runat="server" Text='<%# Bind("ReplyAuthor") %>'>
						</asp:TextBox>
					</td>
				</tr>
				<tr>
					<td>
						<asp:Label runat="server" ID="lblCommentEmail" Text="* Email" />
						<asp:RegularExpressionValidator ID="aEmReq" runat="server" ControlToValidate="tbCommentEmail" Display="Dynamic"
							ErrorMessage="&nbsp;&nbsp;Invalid Email Address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="cGroup" />
						<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="tbCommentEmail" Display="Dynamic" ErrorMessage="&nbsp;&nbsp;Required"
							ValidationGroup="cGroup" />
					</td>
				</tr>
				<tr>
					<td>
						<asp:TextBox ID="tbCommentEmail" Width="250px" MaxLength="128" runat="server" Text='<%# Bind("ReplyAuthorEmail") %>'>
						</asp:TextBox>
					</td>
				</tr>
				<tr>
					<td>
						<asp:Label runat="server" ID="lblCommentText" Text="* Your Comment" />
						<asp:RequiredFieldValidator runat="server" ID="pReq" ControlToValidate="tbCommentText" Display="Dynamic" ErrorMessage="&nbsp;&nbsp;Required"
							ValidationGroup="cGroup" />
					</td>
				</tr>
				<tr>
					<td>
						<table cellpadding="0" cellspacing="0">
							<tr>
								<td>
									<%--Arbitrarily chose 300 as max character limit on comments.  Database accepts unlimited.--%>
									<asp:TextBox ID="tbCommentText" TextMode="MultiLine" Rows="5" Width="500px" runat="server" Text='<%# Bind("ReplyText") %>' 
										onKeyUp="ValidateMaxChars(this,300)" onChange="ValidateMaxChars(this,300)" />
								</td>
							</tr>
							<tr>
								<td align="right">
									You have <b><span id=cmtCounter>300</span></b> characters remaining for your comment.
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td>
						<asp:Button ID="btnCommentSubmit" runat="server" CausesValidation="True" CommandName="Insert" Text="Post Comment" 
							ValidationGroup="cGroup" onclick="btnCommentSubmit_Click" />
					</td>
				</tr>
			</table>
			</div>

		</div>

	</asp:Panel>  <%--End of Comments Panel--%>
	

</div>

