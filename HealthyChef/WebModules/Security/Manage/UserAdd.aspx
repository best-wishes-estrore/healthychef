<%@ Page Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true"
    Inherits="BayshoreSolutions.WebModules.Admin.Security.UserAdd" Title="New User"
    StylesheetTheme="WebModules" CodeBehind="UserAdd.aspx.cs" %>

<%@ Register TagPrefix="hcc" TagName="UserProfileEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/UserProfile_Edit.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="Server">
    <h3>
        Add / Edit User</h3>
    <hcc:UserProfileEdit ID="UserProfileEdit1" runat="server" ValidationGroup="UserProfileEditGroup" />
</asp:Content>
