<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreditCardDisplay.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.CreditCardDisplay" %>

<div class="fieldRow">
    <div class="fieldCol">
        Name on Card:<asp:Label ID="lblNameOnCard" runat="server" />
    </div>
</div>
<div class="fieldRow">
    <div class="fieldCol">
        Card Number:
        <asp:Label ID="lblLast4" runat="server" />
    </div>
</div>
<div class="fieldRow">
    <div class="fieldCol">
        Card Type:
        <asp:Label ID="lblType" runat="server" />
    </div>
</div>
<div class="fieldRow">
    <div class="fieldCol">
        Expires:
        <asp:Label ID="lblExpires" runat="server" />
    </div>
</div>
