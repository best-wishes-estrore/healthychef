<%@ Page Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="BayshoreSolutions.WebModules.Security.Login.Edit" Title="Login Module" Theme="webModules" %>

<%@ Register Src="../../Components/PagePicker/PagePicker.ascx" TagName="PagePicker"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
<table cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td>Page to go to once the user is logged in:</td>
        <td><uc1:PagePicker ID="loginPage" runat="server" /></td>
    </tr>
    <tr>
        <td>Password recovery page:</td>
        <td><uc1:PagePicker ID="PasswordRecoveryPage" runat="server" /></td>
    </tr>
</table>
    <asp:Button ID="SaveButton" runat="server" OnClick="SaveButton_Click" Text="Save" />
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Cancel" />
</asp:Content>
