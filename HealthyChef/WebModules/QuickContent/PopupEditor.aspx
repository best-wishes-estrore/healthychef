<%@ Page  validaterequest="false" Language="C#" AutoEventWireup="true" CodeBehind="PopupEditor.aspx.cs" Inherits="BayshoreSolutions.WebModules.QuickContent.PopupEditor" %>
<%@ OutputCache  NoStore="true" Location="None"%>
<%@ Register TagPrefix="FCKeditorV2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>

<html>
<head id="Head1" runat=server>
<title>Quick Content Editor</title>
<style>
body {
	background-color: #efefef;
}

body, html, input, select, td {
	font-family: Arial, Verdana, Helvetica, sans-serif;
	font-size:11px;
	color: #333333;
}
.button {
	font-size:12px;
}
</style>
</head>
<body>
<form id=elForm runat=server>
    <div style="text-align:center;margin:10px;">
		<table width=99%>
			<tr>
				<td colspan="3">
		            <FCKeditorV2:FCKeditor 
									id="txtContent" 
									runat="server" 
									BasePath="~/WebModules/Components/TextEditor/fckeditor-2.6.6/"
		              Height="450" 
		              Width="100%" 
		              SkinPath="skins/office2003/" 
									ToolbarSet="WebModules"
		              />
				</td>
			</tr>
			<tr>
				<td align="left">
			        Locale: <asp:DropDownList ID=ddlCulture runat=server OnSelectedIndexChanged="ddlCulture_SelectedIndexChanged" AutoPostBack="true"/>
				</td>
				<td align="center">

					<asp:Button id="btnSave" runat="server" Text=" Save " OnClick="btnSave_Click" CssClass="button" CausesValidation="false" ></asp:Button>
				    &nbsp;&nbsp;&nbsp;&nbsp;
					<asp:Button id="btnClose" runat="server" Text="Close" OnClientClick="parent.hidePopWin(false);parent.location.reload();" CssClass="button" CausesValidation="False" UseSubmitBehavior="false" ></asp:Button>
					<asp:Label runat="server" ID="lblMessage" ForeColor="Green" />
				</td>
				<td align="right">
					Version: <asp:DropDownList ID=ddlVersion runat=server OnSelectedIndexChanged="ddlVersion_SelectedIndexChanged" AutoPostBack="true"/>
				</td>
			</tr>
		</table>
    </div> 
</form>
</body>
</html>
