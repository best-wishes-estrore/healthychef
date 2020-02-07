<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserProfileCart_Edit.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.UserProfileCart_Edit" %>
<%@ Register TagPrefix="hcc" TagName="CartDisplay" Src="~/WebModules/ShoppingCart/Controls/Cart/CartDisplay.ascx" %>
<%@ Register TagPrefix="hcc" TagName="CreditCardDisplay" Src="~/WebModules/ShoppingCart/Admin/UserControls/CreditCardDisplay.ascx" %>
<%@ Register TagPrefix="hcc" TagName="GiftCertEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/GiftCertificate_Edit.ascx" %>
<%@ Register TagPrefix="hcc" TagName="MenuItemAddToCart" Src="~/WebModules/ShoppingCart/Admin/UserControls/MenuItemAddToCart.ascx" %>
<%@ Register TagPrefix="hcc" TagName="ProgramPlanAddToCart" Src="~/WebModules/ShoppingCart/Admin/UserControls/ProgramPlanAddToCart.ascx" %>
<table style="width: 100%;">
    <tr>
        <td>
            <asp:Label ID="lblFeedback" runat="server" EnableViewState="false" ForeColor="Green" />
        </td>
    </tr>
    <tr>
        <td>
            <hcc:CartDisplay ID="CartDisplay1" runat="server" DisplayCouponDetails="true" ShowContinueButton="false" />
        </td>
    </tr>    
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td>
            <asp:Button ID="btnAddNewItem" CssClass="btn btn-info" runat="server" Text="Add Cart Item" />
            <asp:Panel ID="pnlAddCartItem" runat="server" Visible="false">
                Choose an item type:<br />
                <asp:RadioButtonList ID="rblItemType" runat="server" AutoPostBack="true">
                    <asp:ListItem Value="1" Text="A la Carte" />
                    <asp:ListItem Value="2" Text="Program Plan" />
                    <asp:ListItem Value="3" Text="Gift Certificate" />
                </asp:RadioButtonList>
                <asp:Panel ID="pnlAlaCarte" runat="server" Visible="false">
                    <fieldset class="pnlprgmpln">
                        <legend>A la Carte</legend>
                        <hcc:MenuItemAddToCart ID="MenuItemAddToCart1" runat="server" ValidationGroup="UserProfileCartMenuItemEditGroup" />
                    </fieldset>
                </asp:Panel>
                <asp:Panel ID="pnlProgramPlan" runat="server" Visible="false">
                    <fieldset class="pnlprgmpln">
                        <legend>Program Plans</legend>
                        <hcc:ProgramPlanAddToCart ID="ProgramPlanAddToCart1" runat="server" ValidationGroup="UserProfileCartPlanEditGroup" />
                    </fieldset>
                </asp:Panel>
                <asp:Panel ID="pnlGiftCard" runat="server" Visible="false">
                    <fieldset class="pnlprgmpln">
                        <legend>Gift Certificate</legend>
                        <hcc:GiftCertEdit ID="GiftCertEdit1" runat="server" ValidationGroup="UserProfileCartGiftCertEditGroup" />
                    </fieldset>
                </asp:Panel>
            </asp:Panel>
        </td>
    </tr>
</table>
