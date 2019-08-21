<%@ Page Language="C#" Theme="WebModules" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true" Inherits="BayshoreSolutions.WebModules.Admin.MyWebsite.AliasSettings" Title="Alias Settings" Codebehind="AliasSettings.aspx.cs" %>
<%@ Register Src="~/WebModules/Components/PagePicker/PagePicker.ascx" TagName="PagePicker" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">
<table border="0" cellpadding="0" cellspacing="4" style="width: 489px">
    <tr>
        <td valign="top">
            *Page URL</td>
        <td valign="top">
            <asp:TextBox ID="AliasPageTitleTextbox" runat="server" Width="250px" /></td>
        <td>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="AliasPageTitleTextbox"
                ErrorMessage="Required" /></td>
    </tr>
    <tr>
        <td valign="top">
            Display in Navigation?</td>
        <td valign="top">
            <asp:CheckBox ID="DisplayAliasInNavCheckBox" runat="server" Text="Yes" /></td>
        <td></td>
    </tr>
    <tr>
        <td valign="top">
            Redirect?</td>
        <td valign="top">
            <asp:CheckBox ID="RedirectAliasCheckbox" runat="server" Text="Yes" /></td>
        <td></td>
    </tr>
    <tr>
        <td style="width: 150px" valign="top"></td>
        <td align="right" valign="top">
            &nbsp;<asp:Button ID="CreateAliasButton" runat="server" OnClick="CreateAliasButton_Click"
                Text="Update Alias" />
            <asp:Button ID="InsertAliasCancelButton" runat="server" CommandName="Cancel" Text="Cancel"
                CausesValidation="False" OnClick="InsertAliasCancelButton_Click" /></td>
        <td></td>
    </tr>
</table>                           
</asp:Content>

