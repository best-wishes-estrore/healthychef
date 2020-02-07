<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContentArea.ascx.cs" Inherits="BayshoreSolutions.WebModules.QuickContent.ContentArea" %>
<asp:PlaceHolder ID="placeholderEdit" runat="server">
    <div class="quick-content-contentbox" 
        id="divHolder" 
		ondblclick="showPopWin('<%=GetBaseUrl()%>','<%=GetCMSUrl()%>', 800, 540, null, true);return false;"
		title="Double-click to Edit">
		
		<asp:Literal ID="literalContentEdit" runat="server"></asp:Literal>
    </div>    
</asp:PlaceHolder>
    
<asp:PlaceHolder ID="placeholderView" runat="server">
    <asp:Literal ID="literalContentView" runat="server"></asp:Literal>
</asp:PlaceHolder>

