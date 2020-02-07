<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PasswordReset.ascx.cs" Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.PasswordReset" %>
<fieldset>
    <legend>Password:</legend>
    <asp:Button ID="btnSave" runat="server" Text="Reset Password" CssClass="btn btn-info"
        OnClientClick="javascript: return confirm('Are you sure that you want to reset this users password?')" />
</fieldset>
