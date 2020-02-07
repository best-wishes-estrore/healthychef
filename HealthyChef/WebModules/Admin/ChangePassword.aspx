<%@ Page Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="BayshoreSolutions.WebModules.Admin.Security.ChangePassword" Title="Change Password" Theme="WebModules" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <asp:ChangePassword ID="ChangePassword1" runat="server" CancelDestinationPageUrl="default.aspx"
        ContinueDestinationPageUrl="default.aspx">
    </asp:ChangePassword>
</asp:Content>
