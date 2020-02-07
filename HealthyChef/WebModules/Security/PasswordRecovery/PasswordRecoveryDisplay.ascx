<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PasswordRecoveryDisplay.ascx.cs"
    Inherits="BayshoreSolutions.WebModules.Security.PasswordRecovery.PasswordRecoveryDisplay" %>

<div class="login-page" style="color: #5A5A5A; margin: 1em 0em; text-align:center;">

    <h2 class="header">Account Log In</h2>

    Enter your email address to reset your password.
    <div style="color: red; margin: 1em 0em;">
        <asp:ValidationSummary ID="ValSum1" runat="server" ValidationGroup="PasswordRecovery1" />       
    </div>
     <asp:Literal ID="lblFeedback" runat="server" EnableViewState="False" />
</div>
<div id="divForm" runat="server" class="password-page">
    Email:
    <asp:TextBox ID="txtEmail" autocomplete="off" runat="server" />
    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" Display="None"
        ErrorMessage="Email is required" ToolTip="Email is required." ValidationGroup="PasswordRecovery1" />
    <p>
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" ValidationGroup="PasswordRecovery1" CssClass="pixels-button pixels-button-solid-green pixels-button-checkout" />
    </p>
</div>

