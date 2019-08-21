<%@ Page Title="AuthorizeNet Module" Language="C#" AutoEventWireup="true" 
    MasterPageFile="~/Templates/WebModules/Module.Master" 
    CodeBehind="Settings.aspx.cs" 
    Inherits="HealthyChef.WebModules.AuthNetModule.Edit" 
    Theme="WebModules" 
    ValidateRequest="false"
    ViewStateMode="Disabled"
%>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">

<div>
    <div class="authnet row">
        <span class="authnet field">Name</span>
        <span class="authnet value"><asp:Literal runat="server" ID="AuthNetName"></asp:Literal></span>
    </div>

    <div class="authnet row">
        <span class="authnet field"></span>
        <span class="authnet value"><asp:Literal runat="server" ID="AuthNetApiKey"></asp:Literal></span>
    </div>

    <div class="authnet row">
        <span class="authnet field"></span>
        <span class="authnet value"><asp:Literal runat="server" ID="AuthNetTransactionKey"></asp:Literal></span>
    </div>

    <div class="authnet row">
        <span class="authnet field"></span>
        <span class="authnet value"><asp:Literal runat="server" ID="AuthNetMode"></asp:Literal></span>
    </div>

    <div class="authnet row">
        <span class="authnet field"></span>
        <span class="authnet value"><asp:Literal runat="server" ID="AuthNetTestCardNumber"></asp:Literal></span>
    </div>

    <div class="authnet row">
        <span class="authnet field"></span>
        <span class="authnet value"><asp:Literal runat="server" ID="AuthNetTestCardCVV"></asp:Literal></span>
    </div>

    <div class="authnet row">
        <span class="authnet field"></span>
        <span class="authnet value"><asp:Literal runat="server" ID="AuthNetTestCardExpirationMonthOffSet"></asp:Literal></span>
    </div>

    <div class="authnet row">
        <span class="authnet field"></span>
        <span class="authnet value"><asp:Literal runat="server" ID="AuthNetTestCardExpirationYearOffSet"></asp:Literal></span>
    </div>
</div>
    
</asp:Content>
