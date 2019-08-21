<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CartItemList.ascx.cs" Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.CartItemList" %>
<asp:Label style="color:green;font-size:10px;" runat="server" Visible="false" id="lblfeedback"></asp:Label>
<table style="width: 100%; padding: 0px; margin: 0px;" class="txtQuantity_item" >
    <tr visible="true" style="height: 0px;">
        <th style="height: 0px;"></th>
        <th style="width: 80px; height: 0px;"></th>
        <th style="width: 80px; height: 0px;"></th>
        <th style="width: 100px; height: 0px;"></th>
    </tr>
    <tr id="trHeader" runat="server" visible="false">
        <th style="width:60%">Product</th>
        <th >Family Style</th>
        <th style="text-align: center;width:10%">Auto Renew</th>
        <th>Quantity</th>
        <th style="text-align: center;">Price</th>
        <th style="text-align: right;">Totals</th>
    </tr>
    <tr>
        <td colspan="4">
            <i class="color-olive">Profile Name(s):
            <asp:Label ID="lblUserProfile" runat="server" />
                &nbsp;:&nbsp;<asp:Label ID="lblShippingMethod" runat="server" />
            </i>
        </td>
    </tr>
    <asp:ListView ID="lvwCartItems" runat="server" DataKeyNames="CartItemID">
        <LayoutTemplate>
            <tr id="itemPlaceHolder" runat="server" />
            <tr>
                <td colspan="4">
                    <asp:ValidationSummary ID="ValSum1" runat="server" DisplayMode="BulletList" ValidationGroup="QuantityGroup" />
                </td>
            </tr>

        </LayoutTemplate>
        <ItemTemplate>
            <tr style="background-color: #ddd;">
                <td>
                    <%# Eval("OrderNumber") %>&nbsp;-&nbsp;
                    <span style="font-size: 11px;" id="ItemName"><%# Eval("ItemName") %></span></td>
                <td>
                    <asp:CheckBox ID="Chkfamilystyle" AutoPostBack="true" CausesValidation="false" OnCheckedChanged="ChkAutoRenew_CheckedChanged" Text='<%# Eval("CartItemID") %>'  runat="server" Cssclass="autorenew"  Checked='<%#Eval("Plan_IsAutoRenew")==null?false:((bool)Eval("Plan_IsAutoRenew"))%>'  Visible=  '<%# (Eval("Plan_IsAutoRenew")==null || Eval("ItemTypeID").ToString() =="2") ? false  : true  %>' />
                </td>
                <td style="width:10%">
                    <asp:CheckBox ID="chkautorenew" AutoPostBack="true" CausesValidation="false"  OnCheckedChanged="ChkAutoRenew_CheckedChanged" Text='<%# Eval("CartItemID") %>'  runat="server" Cssclass="autorenew"  Checked='<%#Eval("Plan_IsAutoRenew")==null?false:((bool)Eval("Plan_IsAutoRenew"))%>'  Visible=  '<%# (Eval("Plan_IsAutoRenew")==null || Eval("ItemTypeID").ToString() =="1") ? false  : true  %>' />
                </td>
                <td >
                    <asp:TextBox ID="txtQuantity"  runat="server" AutoPostBack="true" Width="35px" MaxLength="3"
                        OnTextChanged="txtQuantity_TextChanged" Text='<%# Eval("Quantity") %>' ToolTip="Update quantity" ValidationGroup="QuantityGroup" />
                    <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="txtQuantity"
                        Display="None" ErrorMessage="Quantity must have a value." SetFocusOnError="true" ValidationGroup="QuantityGroup" />
                    <asp:CompareValidator ID="cpvQuantity" runat="server" ControlToValidate="txtQuantity" Operator="DataTypeCheck" Type="Integer"
                        Display="None" ErrorMessage="Quantity must be a number" SetFocusOnError="true" ValidationGroup="QuantityGroup" />
                    <asp:LinkButton ID="lkbRemove" runat="server" CommandArgument='<%# Eval("CartItemID") %>' Text="X" ForeColor="Red" Font-Bold="true" Style="text-decoration: none;" ToolTip="Remove item from cart"
                        OnClick="lkbRemove_Click" CausesValidation="false" OnClientClick="javascript: return confirm('Are you sure that you want to remove this item from the cart?')" />
                </td>
                <td style="text-align: center; font-size: 11px;">
                    <%# Decimal.Parse(Eval("mockItemPrice").ToString()).ToString("c") %></td>
                <td style="text-align: right; font-size: 11px;">
                    <%# Decimal.Parse(Eval("mockTotalPrice").ToString()).ToString("c") %></td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr>
                <td>
                    <%# Eval("OrderNumber") %>&nbsp;-&nbsp;
                    <span style="font-size: 11px;"><%# Eval("ItemName") %></span></td>
                <td>
                   <asp:CheckBox ID="Chkfamilystyle" AutoPostBack="true" CausesValidation="false" Text='<%# Eval("CartItemID") %>' OnCheckedChanged="ChkAutoRenew_CheckedChanged"   runat="server" Cssclass="autorenew"  Checked='<%#Eval("Plan_IsAutoRenew")==null?false:((bool)Eval("Plan_IsAutoRenew"))%>'  Visible=  '<%# (Eval("Plan_IsAutoRenew")==null || Eval("ItemTypeID").ToString() =="2") ? false  : true %>' />
                    
                </td>
                <td  style="width:10%">
                   <asp:CheckBox ID="chkautorenew" AutoPostBack="true" CausesValidation="false" Text='<%# Eval("CartItemID") %>'  OnCheckedChanged="ChkAutoRenew_CheckedChanged"   runat="server" Cssclass="autorenew"  Checked='<%#Eval("Plan_IsAutoRenew")==null?false:((bool)Eval("Plan_IsAutoRenew"))%>'  Visible=  '<%# (Eval("Plan_IsAutoRenew")==null || Eval("ItemTypeID").ToString() =="1") ? false  : true %>' />
                    
                </td>
                <td>
                    <asp:TextBox ID="txtQuantity" runat="server" AutoPostBack="true" Width="35px" MaxLength="3"
                        OnTextChanged="txtQuantity_TextChanged" Text='<%# Eval("Quantity") %>' ToolTip="Update quantity" ValidationGroup="QuantityGroup" />
                    <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="txtQuantity"
                        Display="None" ErrorMessage="Quantity must have a value." SetFocusOnError="true" ValidationGroup="QuantityGroup" />
                    <asp:CompareValidator ID="cpvQuantity" runat="server" ControlToValidate="txtQuantity" Operator="DataTypeCheck" Type="Integer"
                        Display="None" ErrorMessage="Quantity must be a number" SetFocusOnError="true" ValidationGroup="QuantityGroup" />
                    <asp:LinkButton ID="lkbRemove" runat="server" CommandArgument='<%# Eval("CartItemID") %>' Text="X" ForeColor="Red" Font-Bold="true" Style="text-decoration: none;" ToolTip="Remove item from cart"
                        OnClick="lkbRemove_Click" CausesValidation="false" OnClientClick="javascript: return confirm('Are you sure that you want to remove this item from the cart?')" />
                </td>
                <td style="text-align: center; font-size: 11px;">
                    <%# Decimal.Parse(Eval("mockItemPrice").ToString()).ToString("c") %></td>
                <td style="text-align: right; font-size: 11px;">
                    <%# Decimal.Parse(Eval("mockTotalPrice").ToString()).ToString("c") %></td>
            </tr>
        </AlternatingItemTemplate>
    </asp:ListView>
</table>
<table style="width: 100%; border-top: 1px solid black; padding: 0px; margin: 0px;">
    <tr>
        <td colspan="2">&nbsp;</td>
        <td style="text-align: right; font-size: 11px;">Profile Sub-Total:
        </td>
        <td style="text-align: right; width: 80px; font-size: 11px;">
            <asp:Label ID="lblProfileSubTotal" runat="server" /></td>
    </tr>
    <tr id="trProfDiscount" runat="server" visible="false" style="display:none;">
        <td colspan="2">&nbsp;</td>
        <td style="text-align: right; font-size: 11px;">Discount:
        </td>
        <td style="text-align: right; width: 80px; font-size: 11px;">
            <asp:Label ID="lblProfileDiscount" runat="server" /></td>
    </tr>
    <tr id="trProfDiscountSubTotal" runat="server" visible="false" style="display:none;">
        <td colspan="2">&nbsp;</td>
        <td style="text-align: right; font-size: 11px;">Discounted Sub-Total:
        </td>
        <td style="text-align: right; width: 80px; font-size: 11px;">
            <asp:Label ID="lblProfileSubTotalAdj" runat="server" /></td>
    </tr>
    <tr id="trProfTaxTotal" runat="server" visible="false" style="display:none;">
        <td colspan="2">&nbsp;</td>
        <td style="text-align: right; font-size: 11px;">Profile Tax Sub-Total:
        </td>
        <td style="text-align: right; width: 80px; font-size: 11px;">
            <asp:Label ID="lblProfileTaxSubTotal" runat="server" /></td>
    </tr>
    <tr id="trProfActShipTotal" runat="server" visible="false" style="display:none;">
        <td colspan="2">&nbsp;</td>
        <td style="text-align: right; font-size: 11px;">*DEV - Actual Profile Shipping Sub-Total:
        </td>
        <td style="text-align: right; width: 80px; font-size: 11px;">
            <asp:Label ID="lblProfileShipActTotal" runat="server" /></td>
    </tr>
    <tr id="trProfShipTotal" runat="server" visible="false" style="display:none;">
        <td colspan="2">&nbsp;</td>
        <td style="text-align: right; font-size: 11px;">Profile Shipping Sub-Total:
        </td>
        <td style="text-align: right; width: 80px; font-size: 11px;">
            <asp:Label ID="lblProfileShipSubTotal" runat="server" /></td>
    </tr>
    <tr id="trProfTotal" runat="server" visible="false" style="display:none;">
        <td colspan="2">&nbsp;</td>
        <td style="text-align: right; font-size: 11px;">Profile Total:
        </td>
        <td style="text-align: right; width: 80px; font-size: 11px;">
            <asp:Label ID="lblProfileGrandTotal" runat="server" /></td>
    </tr>
</table>
<script>
    $(document).ready(function () {
        $('#ctl00_Body_UserProfileEdit1_ProfileCartEdit1_CartDisplay1_lvwCart_ctrl1_CartItemList1_lvwCartItems_ctrl1_chkautorenew').each(function () {
            var selectedID = $(this).attr('id');
            var value = $('label[for=' + selectedID + ']').css("display", "none");
        });
       
         $('#ctl00_Body_UserProfileEdit1_ProfileCartEdit1_CartDisplay1_lvwCart_ctrl1_CartItemList1_lvwCartItems_ctrl0_chkautorenew').each(function () {
            var selectedID = $(this).attr('id');
            var value = $('label[for=' + selectedID + ']').css("display", "none");
         });
        //    $('#ctl00_body_ctl00_CartDisplay1_lvwCart_ctrl0_CartItemList1_lvwCartItems_ctrl0_txtQuantity').on('change', function () {

        //        var Quantity = $('#ctl00_body_ctl00_CartDisplay1_lvwCart_ctrl0_CartItemList1_lvwCartItems_ctrl0_txtQuantity').val();
        //        var itemName = $('#ItemName').text();
        //        var ItemCount = itemName.split('-')[6].replace(" ", "");
        //        var MealCount = ItemCount * Quantity;
        //        var fruitsdd = itemName.split('-');
        //        fruitsdd[6] = MealCount;
        //        var dasdsa = fruitsdd.join("-");
        //        $('#ItemName').text(dasdsa);
        //    });
        //    $('.txtQuantity_item').find('td').find('input').each(function (index) {

        //        var Quantity = $(this).val();
        //        var itemName = $(this).parent().prev().find('span').text();
        //        var ItemCount = itemName.split('-')[6].replace(" ", "");
        //        var MealCount = ItemCount * Quantity;
        //        var fruitsdd = itemName.split('-');
        //        fruitsdd[6] = MealCount;
        //        var dasdsa = fruitsdd.join("-");
        //        $(this).parent().prev().find('span').text(dasdsa);
        //    });
    });
</script>
<style>
    .autorenew input {
    border-radius: 0 !important;
    color: #858585;
    background-color: #FFF;
    border: 1px solid #D5D5D5;
    padding: 5px 4px 6px;
    width:100%;
    vertical-align:middle;
    height: 15px;
    margin:0px;
    }
    .autorenew label {
        visibility:hidden;
        height:0px;
            margin-left: -17px;
    margin-top: -5px;
    }
    .txtQuantity_item tr td:first-child {
        width:60%;
    }
     .label , .label1 {
    width: 80px;
    margin: 0px 5px;
    text-align:left;
    font-weight: bold;
    float: right;
    padding: 0px;
}
     .txtQuantity_item td {
         vertical-align:middle;
     }
     .txtQuantity_item td:nth-child(2) {
         width:7%;
     }
     .txtQuantity_item td:nth-child(2) input {
         margin-top:13px;
     }
     .pnlprgmpln select, .pnlprgmpln input {
         margin:5px 0px;
     }
     #cbxRecurring {
         vertical-align:middle;
     }
</style>