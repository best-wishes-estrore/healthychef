<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CartModule.ascx.cs" Inherits="HealthyChef.WebModules.ShoppingCart.Controls.Cart.CartModule" %>

<%@ Register TagPrefix="hcc" TagName="CartDisplay" Src="~/WebModules/ShoppingCart/Controls/Cart/CartDisplay.ascx" %>

<asp:MultiView ID="multi_cart" ActiveViewIndex="0" runat="server">

    <asp:View ID="view_cart" runat="server">
        <hcc:CartDisplay ID="CartDisplay1" runat="server" ShowContinueButton="true" IsForPublic="true" />
    </asp:View>
</asp:MultiView>