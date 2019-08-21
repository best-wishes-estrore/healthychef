<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserProfilePurchaseHistory_Edit.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.UserProfilePurchaseHistory_Edit" %>
<table style="width: 100%;">
    <tr>
        <td>
            <p id="pFeedback"></p>
            <asp:Label ID="lblFeedback" runat="server" ClientIDMode="Static" EnableViewState="false" ForeColor="Red" />
            <br />
            <asp:ListView ID="lvwPurchaseHistory" runat="server" DataKeyNames="CartID" OnItemDataBound="lvwPurchaseHistory_ItemDataBound">
                <LayoutTemplate>
                    <table class="table table-bordered table-hover">
                        <tr>
                            <th>Purchase Number:</th>
                            <th>Status</th>
                            <th>Purchase Date</th>
                            <th>Total</th>
                            <th>&nbsp;</th>
                        </tr>
                        <tr id="itemPlaceHolder" runat="server" />
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%# Eval("PurchaseNumber") %></td>
                        <td><%# (HealthyChef.Common.Enums.CartStatus)Eval("StatusID") %></td>
                        <td><%# Eval("PurchaseDate") %></td>
                        <td><%# decimal.Parse(Eval("TotalAmount").ToString()).ToString("c") %></td>
                        <td class="pull-right">
                            <asp:LinkButton ID="lkbSelect" runat="server" Text="Details" OnClick="lkbSelect_Click" CausesValidation="false" />
                            &nbsp;&nbsp;
                                <asp:LinkButton ID="lkbPrint" runat="server" Text="Print" OnClientClick="javascript:PrintContent(this)" Visible="false" />
                        </td>
                    </tr>
                    <tr id="trStatus" runat="server" class="cartStat" style="display: none;">
                        <td colspan="5" style="border: solid 1px #333; background-color: #eee;">Update status at own risk. This function will update a purchase's status. 
                            In addition, it will update the purchase's last "Modified Date" and "Modified By" user record. 
                            <br />
                            <br />
                            If the status "Paid" is selected, the cart's "Purchased Date" and "Purchased By" record will be updated. 
                            Once a purchase is marked as "Paid" and its "Purchased Date" and "Purchased By" record contains data, 
                            it will always there after be treated as a "Paid" purchase, even if the status is later reverted to Unfinalized or changed to Cancelled.
                            <br />
                            <br />
                            Updating the status here WILL NOT initiate any transactions with Authorize.Net.
                            <br />
                            <br />
                            <asp:HiddenField ID="hdnCartId" runat="server" Value="0" ClientIDMode="Static" />
                            <asp:HiddenField ID="hdnStatus" runat="server" Value="10" ClientIDMode="Static" />
                            <select id="ddlCartStatus">
                                <option value="0">Select new status...</option>
                                <option value="10">Unfinalized</option>
                                <option value="20">Paid</option>
                                <option value="40">Fulfilled</option>
                                <option value="50">Cancelled</option>
                            </select>
                            (Will update the status, only if this is changed)
                            <div id="divPaidOptions" style="display: none;">
                                <input id="chkNewSnapshot" type="checkbox" title="Create New Snapshot" />
                                Create New Snapshot (Will create a completely new snapshot of the purchase, includes address, basic cart info, and cart items info)
                                <br />
                                <input id="chkAddrs" type="checkbox" title="Reset Addresses" />
                                Re-Snaphot Addresses Only(Resets the snapshot addresses to the user's current address information)
                                <br />
                                <input id="chkAuthNet" type="checkbox" title="Rerun Auth.Net" />
                                Rerun Auth.Net (Will overwrite any existing PurchasedDate/PurchaseBy data, and attempt a new AuthCapture request)
                                <br />
                                <input id="chkLedger" type="checkbox" title="Create Ledger Entry" />
                                Create Ledger Entry (Will create a new ledger entry for the purchase, if one can not be found. Will not overwrite an existing ledger entry)
                                <br />
                                <input id="chkCustEmail" type="checkbox" title="Send Customer Email" />
                                Send Customer Email (Will send a purchase confirmation email to the customer)
                                <br />
                                <input id="chkMerchEmail" type="checkbox" title="Send Merchant Email" />
                                Send Merchant Email (Will send a purchase confirmation email to the merchant)
                                <br />
                                <input id="chkResetProgCals" type="checkbox" title="Repair Program Delivery Dates" />
                                Repair Program Delivery Dates (Will create any missing delivery dates for all program items in this purchase)
                                 <br />
                                <input id="chkReCalcItemTax" type="checkbox" title="Recalculate cart item tax values" />
                                Recalculate cart item tax values (Will re-calculate cart item tax values)
                            </div>
                            <br />
                            <input id="btnUpdate" type="button" value="Update Status" style="display: none;" />
                        </td>
                    </tr>
                    <tr id="trDetails" runat="server" visible="false">
                        <td colspan="5" style="border: 1px solid #333;" class="printContainer">&nbsp;Stuff</td>
                    </tr>
                </ItemTemplate>
                <EmptyDataTemplate>
                    There are no purchases on record for this account.
                </EmptyDataTemplate>
            </asp:ListView>
        </td>
    </tr>
</table>
<script type="text/javascript">
    $(document).ready(function () {
        $(".cartStat").each(function () {
            var hdnCartId = $(this).find("#hdnCartId");
            var hdnStatus = $(this).find("#hdnStatus");
            var ddlCartStatus = $(this).find("#ddlCartStatus");
            var divPaidOptions = $(this).find("#divPaidOptions");
            var btnUpdate = $(this).find("#btnUpdate");

            if (hdnStatus)
                ddlCartStatus.val(hdnStatus.val());

            if ($(this).val() == "0") {
                btnUpdate.hide();
            }
            else {
                btnUpdate.show();
            }

            if (ddlCartStatus.val() == "20") {
                divPaidOptions.show();
            }
            else {
                divPaidOptions.hide();
            }

            ddlCartStatus.change(function () {
                if ($(this).val() == "0") {
                    btnUpdate.hide();
                }
                else {
                    btnUpdate.show();
                }

                if ($(this).val() == "20") {
                    divPaidOptions.show();
                }
                else {
                    divPaidOptions.hide();
                }
            });

            btnUpdate.click(function () {
                SaveCart($(this))
            });
        });
    });



    function SaveCart(element) {
        var parent = $(element).closest(".cartStat");
        var hdnCartId = parent.find("#hdnCartId");
        var hdnStatus = parent.find("#hdnStatus");
        var ddlCartStatus = parent.find("#ddlCartStatus");
        var divPaidOptions = parent.find("#divPaidOptions");

        var updateStatus = false;
        var updateAddresses = false;
        var rerunAuthNet = false;
        var createLedgerEntry = false;
        var sendCustomerEmail = false;
        var sendMerchantEmail = false;
        var createNewSnapshot = false;
        var repairCartCals = false;
        var reCalcItemTax = false;

        if (ddlCartStatus.val() != '0') { updateStatus = true; }
        if (parent.find("#chkAddrs").prop("checked")) { updateAddresses = true; }
        if (parent.find("#chkAuthNet").prop("checked")) { rerunAuthNet = true; }
        if (parent.find("#chkLedger").prop("checked")) { createLedgerEntry = true; }
        if (parent.find("#chkCustEmail").prop("checked")) { sendCustomerEmail = true; }
        if (parent.find("#chkMerchEmail").prop("checked")) { sendMerchantEmail = true; }
        if (parent.find("#chkNewSnapshot").prop("checked")) { createNewSnapshot = true; }
        if (parent.find("#chkResetProgCals").prop("checked")) { repairCartCals = true; }
        if (parent.find("#chkReCalcItemTax").prop("checked")) { reCalcItemTax = true; }

        if (parseInt(hdnCartId.val()) > 0) {
            var strJson = '{"carts": [';

            strJson = strJson + '{ "cartId": "' + hdnCartId.val() + '", "updateStatus": "' + updateStatus + '", "statusId": "' + ddlCartStatus.val()
                + '", "updateAddresses": "' + updateAddresses + '", "rerunAuthNet": "' + rerunAuthNet + '", "createLedgerEntry": "' + createLedgerEntry
                 + '", "createNewSnapshot": "' + createNewSnapshot + '", "sendCustomerEmail": "' + sendCustomerEmail + '", "sendMerchantEmail": "'
                 + sendMerchantEmail + '", "repairCartCals": "' + repairCartCals + '", "reCalcItemTax": "' + reCalcItemTax + '"}';
            strJson = strJson + "]}";

            var path = '/WebModules/ShoppingCart/Admin/UserControls/WS_UpdateCartStatus.asmx/UpdateCarts'; //?mid=' + menuItemId;

            $.ajax({
                type: "POST",
                url: path,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: strJson,
                success: function (msg) {
                    MakeUpdateProg(false);
                    $("#lblFeedback").text('');
                    $("#pFeedback").text(msg.d).css("color", "green");

                    parent.find("#chkAddrs").prop("checked", false);
                    parent.find("#chkAuthNet").prop("checked", false);
                    parent.find("#chkLedger").prop("checked", false);
                    parent.find("#chkCustEmail").prop("checked", false);
                    parent.find("#chkMerchEmail").prop("checked", false);
                    parent.find("#chkNewSnapshot").prop("checked", false);
                    parent.find("#chkResetProgCals").prop("checked", false);
                    parent.find("#chkReCalcItemTax").prop("checked", false);
                },
                error: function (xhr, msg) {
                    $("#pFeedback").text('Save Failed.').css("color", "red");
                    MakeUpdateProg(false);
                }
            });
        }
    }

    function PrintContent(element) {
        var DocumentContainer = $(element).closest("tr").next().next().find("td.printContainer");
        var WindowObject = window.open('', "PrintOrderInvoice", "width=740,height=325,top=200,left=250,toolbars=no,scrollbars=yes,status=no,resizable=no");
        WindowObject.document.writeln(DocumentContainer[0].innerHTML);
        WindowObject.document.close();
        WindowObject.focus();
        WindowObject.print();
        WindowObject.close();
    }
</script>
