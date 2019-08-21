<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CartDisplay.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Controls.Cart.CartDisplay" %>
<%@ Register Src="~/WebModules/ShoppingCart/Admin/UserControls/CreditCardDisplay.ascx" TagPrefix="hcc" TagName="CreditCardDisplay" %>
<%@ Register Src="~/WebModules/ShoppingCart/Admin/UserControls/CartItemList.ascx" TagPrefix="hcc" TagName="CartItemList" %>


<script type="text/javascript">
    function preventBack() {
        window.history.forward();
    }
    setTimeout("preventBack()", 0);
    window.onunload = function () {
        null;
    };
    $(function () {
        $('#<%= CheckBoxRequired.ClientID %>').hide();
    });
    function CheckBoxRequired_ClientValidate(sender, e) {
        e.IsValid = jQuery("#<%= cbTermsAndConditions.ClientID %>").is(':checked');
        if (e.IsValid == false) {
            $('#<%= CheckBoxRequired.ClientID %>').show();
            MakeUpdateProg(false);
        }
        else {
            $('#<%= CheckBoxRequired.ClientID %>').hide();
        }
    }
</script>
<style>
    #ctl00_Body_UserProfileEdit1_ProfileCartEdit1_CartDisplay1_divShippingmethod123 {
        text-align:left !important;
        margin: 20px;
    }
    .label {
    margin: 0px 0px;
    font-weight: bold;
    width: 60px;
    float: right;
    text-align: right;
    padding: 0px;
}
    .mycart-page .m-2 .fieldRow {
     padding: 0px; 
}
    .right .cart-list li  {
        padding:0px;
    }
    .right .cart-list {
        margin:0px;
    }
</style>

<asp:Panel ID="pnlCartDisplay" runat="server" CssClass="mycart-page table_row">
    <h2 class="header color-orange">My Cart</h2>
    <asp:Label ID="lblFeedbackCart" runat="server" EnableViewState="false" ForeColor="Red" />
    <div class="mycart-page">
        <!-- Group by user profile -->
        <div>
            <asp:ListView ID="lvwCart" runat="server" DataKeyNames="ShippingAddressID">
                <LayoutTemplate>

                    <div id="itemPlaceHolder" runat="server">
                    </div>

                </LayoutTemplate>
                <ItemTemplate>
                    <div>
                        <hcc:CartItemList ID="CartItemList1" runat="server" OnCartItemListItemUpdated="CartItemList1_CartItemListItemUpdated" />
                    </div>
                </ItemTemplate>
                <EmptyDataTemplate>
                    There are currently no items in your cart.
                </EmptyDataTemplate>
            </asp:ListView>

        </div>
        <!-- 2nd table-->
        <%--<div style="min-width: 400px;" runat="server" id="tdCartDiscountButtons" ClientIDMode="Static">--%>
        <div class="col-1 divAddCouponCode">
            <!-- Coupon Information-->
            <div id="divAddCouponCode" runat="server" class="m-2">
                <asp:TextBox ID="txtCouponCode" runat="server" ValidationGroup="CouponGroup" EnableViewState="false" MaxLength="20" Enabled="True" />
                <asp:Button ID="btnAddCouponCode" runat="server" CssClass="btn btn-info" Text="Apply Coupon Code" ValidationGroup="CouponGroup" Enabled="True" />
                <asp:Label ID="lblFeedbackCoupon" runat="server" Font-Size="10px" ForeColor="Red" EnableViewState="false" />
            </div>
            <div id="divActiveCoupon" runat="server" visible="false" class="m-2">
                <asp:Label ID="lblActiveCoupon" runat="server" />
                <asp:LinkButton ID="btnRemoveCoupon" CssClass="btn btn-danger" runat="server" Text="Delete" OnClick="btnRemoveCoupon_Click" />
            </div>
            <div id="divRedeemGift" runat="server" class="m-2">
                <%--visible="false"--%>
                <asp:TextBox ID="txtRedeemCode" runat="server" ValidationGroup="RedeemGiftGroup" EnableViewState="false" MaxLength="10" Enabled="True" />
                <asp:Button ID="btnRedeemGift" runat="server" Text="Redeem Gift Certificate" CssClass="btn btn-info" ValidationGroup="RedeemGiftGroup" Enabled="True" />
                <asp:Label ID="lblFeedbackRedeemGift" runat="server" Font-Size="10px" ForeColor="Red" EnableViewState="false" />
            </div>
        </div>
        <div class="col-1">
            <!-- Gift Certificate Information-->
            <div id="divCalculateShipping" runat="server">
                <asp:Label ID="lblCalcShipping" runat="server">Calculate Shipping :</asp:Label>
                <asp:TextBox ID="txtCalZipCode" runat="server" Style="width: 120px;"></asp:TextBox>
                <asp:Button ID="btnSearchZipCode" runat="server" OnClick="btnSearchZipCode_Click" Text="Calculate" />
                <asp:DropDownList ID="ddlCalczipcode" runat="server" OnSelectedIndexChanged="OncalcZipcode_SelectedIndexChanged" style="display:none;"></asp:DropDownList>
               
                <asp:CheckBox ID="chkIsPickup" runat="server" style="display:none;"/>
                <asp:Label ID="lblIsPickup" runat="server" style="display:none;">IsPickup</asp:Label>
            </div>
            <div id="divShippingmethod123" style="text-align:center" runat="server">
                 <asp:Label ID="lblShipMenthod" runat="server" style="display:none;">Shipping Method :</asp:Label>
                 <asp:Label ID="lblShipDelType" runat="server"></asp:Label>
            </div>
            <div id="divShippingMethod" runat="server" class="shippingMethod">
                <asp:Label ID="lblShippingMethod" runat="server" style="display:none;">Shipping Method :</asp:Label>
                <asp:CheckBoxList ID="chkShippingMethod" runat="server" style="display:none;">
                    <asp:ListItem Text="Fresh Delivery :Fed Ex Overnight" Value="ckhFreshDel"></asp:ListItem>
                    <asp:ListItem Text="Pickup" Value="ckhPickup"></asp:ListItem>
                </asp:CheckBoxList>
            </div>
        </div>
        <div class="col-1 right">
            <ul class="cart-list">
                <li>
                    <span class="spanlabel">Original Price :</span>
                    <asp:Label ID="lblSubTotal" CssClass="label" runat="server" />
                </li>
                <li>
                    <div id="divDiscounts" runat="server" >
                        <span class="spanlabel">Discount :</span>
                        <asp:Label ID="lblDiscount" runat="server" CssClass="label1" />
                    </div>
                </li>
                <li>
                        <span class="spanlabel">Sub-Total :</span>
                        <asp:Label ID="lblmocksubtotal" runat="server" CssClass="label1" />
                </li>
                <li>
                    <div id="divSubTotalAdj" runat="server" hidden="hidden">
                        <span class="spanlabel">Discounted Sub-Total :</span>
                        <asp:Label ID="lblSubTotalAdj" runat="server" CssClass="label" />
                    </div>
                </li>
                <li>
                    <div>
                        <span class="spanlabel">Tax :</span>
                        <asp:Label ID="lblTax" runat="server" CssClass="label" />
                    </div>
                </li>
                <li>
                    <div id="divShipping" runat="server">
                        <span class="spanlabel">Shipping :</span>
                        <span class="label">
                            <asp:Label ID="lblShipping" runat="server" /></span>
                    </div>
                </li>
            </ul>
            <div class="col">
                <span class="spanlabel">Total :</span>
                <span class="label">
                    <asp:Label ID="lblGrandTotal" runat="server" /></span>
            </div>
            <ul class="cart-list">
                <li>
                    <div id="divAcctBalance" runat="server">
                        <span class="spanlabel">Account Balance :</span>
                        <span class="label">
                            <asp:Label ID="lblAcctBalance" runat="server" /></span>
                    </div>
                </li>
                <li>
                    <div id="divPaymentDue" runat="server">
                        <span class="spanlabel">Payment Amount :</span>
                        <span class="label">
                            <asp:Label ID="lblPaymentDue" runat="server" /></span>
                    </div>
                </li>
            </ul>
            <div id="divRemainAcctBalance" runat="server" class="col">
                <span class="spanlabel">Remaining Acct. Balance :</span>
                <span class="label">
                    <asp:Label ID="lblRemainAcctBalance" runat="server" /></span>
            </div>
        </div>
        <div class="clear"></div>
        <div class="cotinue-shopping-button">
            <asp:Button ID="btnContinue" runat="server" Text="Continue Shopping" CausesValidation="false" CssClass="pixels-button-double pixels-button-double-orange" />
            &nbsp;
                <asp:Button ID="btnClearCart" runat="server" Text="Empty Cart" CausesValidation="false" CssClass="btn btn-info" Visible="false" />
            &nbsp;
                <asp:Button ID="btnCheckOut" runat="server" Text="Check Out" ValidationGroup="CheckoutGroup" CssClass="btn btn-info" Visible="false" />
            <asp:CustomValidator ID="cstCheckOut" runat="server" OnServerValidate="cstCheckOut_ServerValidate"
                Display="None" SetFocusOnError="true" ValidationGroup="CheckoutGroup" EnableClientScript="false" />
            <div id="divValSum" runat="server" visible="true" style="text-align: right;">
                <asp:ValidationSummary ID="ValSumCheckout" runat="server" DisplayMode="List" ValidationGroup="CheckoutGroup" />
            </div>
            <br />
            <asp:Label ID="lblFeedBack" runat="server" EnableViewState="false" />
        </div>
        <div id="trBillingInfo" class="m-2" runat="server" visible="false">
            <span style="padding-right: 15px;">Billing Address:&nbsp;
            <a id="aBillEdit1" runat="server" class="form-show" href="~/my-profile.aspx#billAddr" visible="false">[&nbsp;Edit&nbsp;]</a>
                <br />
                <asp:Label ID="lblBillingAddress" runat="server" />
            </span>
            <span>Billing Information:&nbsp;
                <a id="aBillEdit2" runat="server" class="form-show" href="~/my-profile.aspx#billCC" visible="false">[&nbsp;Edit&nbsp;]</a>
                <hcc:CreditCardDisplay ID="CreditCardDisplay1" runat="server" />
            </span>
        </div>
        </div>
</asp:Panel>
<asp:Panel ID="pnlConfirm" runat="server" Visible="false">
    <h2 style="text-align: center; font-size: 30px" class="color-orange">Order Summary</h2>
    <asp:Label ID="lblOrderDetails" runat="server" />
    <div style="margin: auto 0px auto auto; text-align: right; max-width: 400px;">
        <fieldset>
            <legend style="display: none"></legend>
            <span style="font-size: .9em;">
                <asp:CustomValidator runat="server" ID="CheckBoxRequired" EnableClientScript="true"
                    OnServerValidate="CheckBoxRequired_ServerValidate"
                    ClientValidationFunction="CheckBoxRequired_ClientValidate" ValidationGroup="ConfirmCheckoutGroup">You must select this box to proceed.<br /></asp:CustomValidator>
                <asp:CheckBox runat="server" ValidationGroup="ConfirmCheckoutGroup" ID="cbTermsAndConditions" />
                <asp:Label runat="server" AssociatedControlID="cbTermsAndConditions">(Required) I have read, understand and agree to the terms and conditions, including the cancellation and change policies.  Terms and conditions may be viewed by clicking the link at the bottom of any page on this website.</asp:Label></span>
            <br />
            <br />
            <asp:CheckBox runat="server" Checked="true" ID="cbMarketingOptIn" />
            <span style="font-size: .9em;">Yes, I would like to receive the Healthy Chef Creations newsletter and promotional emails from Healthy Chef Creations.</span>
            <br />
            <br />
        </fieldset>  
        <%--<asp:CheckBox runat="server" ID="cbxRecurring" ClientIDMode="Static" Text="Check here to Sign up for Auto-Renew" TextAlign="Left" Visible="False"/>--%>
        <br />
        <asp:Button ID="btnConfirmCancel" runat="server" Text="Cancel and Restart" CausesValidation="false"
            CssClass="pixels-button-double pixels-button-double-orange" />
        &nbsp;
        <asp:Button ID="btnConfirmComplete" runat="server" Text="Complete Purchase" ValidationGroup="ConfirmCheckoutGroup"
            CssClass="pixels-button-double pixels-button-double-green" OnClientClick="MakeUpdateProg(true);" />
        <asp:CustomValidator ID="CustomValidator1" runat="server" OnServerValidate="cstCheckOut_ServerValidate"
            SetFocusOnError="true" ValidationGroup="ConfirmCheckoutGroup" EnableClientScript="false" /><br />
        (Your payment card will be charged)
        <div id="div1" runat="server" style="text-align: right;">
            <asp:ValidationSummary ID="ValSumConfirmCheckoutGroup" runat="server" DisplayMode="List" ValidationGroup="ConfirmCheckoutGroup" />
        </div>
        <br />
        <asp:Label ID="lblConfirmFeedback" runat="server" EnableViewState="false" />

    </div>
</asp:Panel>
<%--<asp:Panel ID="pnlCartError" runat="server" Visible="false">
    <h2 style="text-align: center; font-size: 30px" class="color-orange" runat="server" ClientIDMode="Static" id="h2OrderSummary">Order Error</h2>  
    <asp:Label ID="lblOrderErrorDetails" runat="server" />  
    <br />
    <br />
    <div style="margin: auto 0px auto auto; text-align: right; max-width: 400px;">
    <asp:Button ID="btnConfirmErrorResolved" runat="server" Text="Cancel and Restart" CausesValidation="false"
        CssClass="pixels-button-double pixels-button-double-orange" OnClick="btnConfirmCancel_Click" />
    <asp:Label ID="lblOrderErrorMessage" runat="server" EnableViewState="false" />
    </div>
</asp:Panel>--%>
<style>
     .pnlprgmpln select, .pnlprgmpln input {
         margin:5px 0px;
     }
       .label , .label1 {
    width: 80px;
    margin: 0px 5px;
    text-align:left;
    font-weight: bold;
    float: right;
    padding: 0px;
}
</style>
