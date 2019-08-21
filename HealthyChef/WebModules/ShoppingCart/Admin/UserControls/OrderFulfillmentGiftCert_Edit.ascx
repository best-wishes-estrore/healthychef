<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderFulfillmentGiftCert_Edit.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.OrderFulfillmentGiftCert_Edit" %>

<%@ Register TagPrefix="hcc" TagName="ProfileNotesEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/UserProfileNote_Edit.ascx" %>

<link rel="stylesheet" type="text/css" href="/App_Themes/WebModules/cartadmin.css" />
<div class="main-content">
    <div class="main-content-inner">
        <asp:LinkButton ID="lkbBack" runat="server" CssClass="btn btn-info  b-10" Text="<< Back to Listing" PostBackUrl="~/WebModules/ShoppingCart/Admin/OrderFulfillment.aspx"
            CausesValidation="false" OnClientClick="MakeUpdateProg(true);" />
    </div>
    <div class="col-sm-12">
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:Label ID="lblFeedback" runat="server" EnableViewState="false" />
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <p>
                                <asp:Button ID="btnSave" runat="server" Text="Save" />
                            </p>
                        </td>
                        <td>
                            <p class="label2">Customer  Name:&nbsp;</p>
                            <asp:Label ID="lblCustomerName" runat="server" /><br />
                            <p class="label2">Profile Name:&nbsp;</p>
                            <asp:Label ID="lblProfileName" runat="server" /><br />
                            <p class="label2">Order Number:&nbsp;</p>
                            <asp:Label ID="lblOrderNumber" runat="server" /><br />
                            <p class="label2">Delivery Date:&nbsp;</p>
                            <asp:Label ID="lblDeliveryDate" runat="server" /><br />
                            <p class="label2">Sent to Recipient:&nbsp;</p>
                            <asp:CheckBox ID="chkIsComplete" runat="server" ClientIDMode="Static" /><br />
                            <p class="label2">Is Cancelled:&nbsp;</p>
                            <asp:CheckBox ID="chkIsCancelledDisplay" runat="server" ClientIDMode="Static" Enabled="false" /><br />
                        </td>
                        <td>
                            <p class="label">Allergens:</p>
                            <asp:ListView ID="lvwAllrgs" runat="server" GroupItemCount="2">
                                <LayoutTemplate>
                                    <ul>
                                        <li id="groupPlaceHolder" runat="server"></li>
                                    </ul>
                                </LayoutTemplate>
                                <GroupTemplate>
                                    <li id="itemPlaceHolder" runat="server"></li>
                                </GroupTemplate>
                                <ItemTemplate>
                                    <li><%# Eval("Name") %></li>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    No Allergens.
                                </EmptyDataTemplate>
                            </asp:ListView>
                            <hcc:ProfileNotesEdit ID="ProfileNotesEdit_AllergenNote" runat="server" CurrentNoteType="AllergenNote" ShowAllNotes="true" AllowDisplayToUser="true" UserDisplayNotesTitle="Allergen Notes:" />
                        </td>
                        <td>
                            <p class="label">Preferences:</p>
                            <asp:ListView ID="lvwPrefs" runat="server">
                                <LayoutTemplate>
                                    <ul>
                                        <li id="itemPlaceHolder" runat="server"></li>
                                    </ul>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <li><%# Eval("Name") %></li>
                                </ItemTemplate>
                                <EmptyDataTemplate>No Preferences.</EmptyDataTemplate>
                            </asp:ListView>
                        </td>
                        <td>
                            <hcc:ProfileNotesEdit ID="ProfileNotesEdit_PreferenceNote" runat="server" CurrentNoteType="PreferenceNote" ShowAllNotes="true" AllowDisplayToUser="true" UserDisplayNotesTitle="Preference Notes:" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="p-5">
        <div class="fieldRow">
            <div class="fieldCol">
                <p class="label2 col-md-2">Item Name:&nbsp;</p>
                <asp:Label ID="lblItemName" runat="server" /><br />
                <p class="label2 col-md-2">Item Price:&nbsp;</p>
                <asp:Label ID="lblPrice" runat="server" /><br />
                <p class="label2 col-md-2">Quantity:&nbsp;</p>
                <asp:Label ID="lblQuantity" runat="server" /><br />
                <p class="label2 col-md-2">Issued To:&nbsp;</p>
                <asp:Label ID="lblIssuedTo" runat="server" /><br />
                <p class="label2 col-md-2">Issued Date:&nbsp;</p>
                <asp:Label ID="lblIssuedDate" runat="server" /><br />
                <p class="label2 col-md-2">Recipient Address:&nbsp;</p>
                <asp:Label ID="lblRecipientAddress" runat="server" /><br />
                <p class="label2 col-md-2">Recipient Email:&nbsp;</p>
                <asp:Label ID="lblRecipientEmail" runat="server" /><br />
                <p class="label2 col-md-2">Recipient Message:&nbsp;</p>
                <asp:Label ID="lblRecipientMessage" runat="server" /><br />
                <p class="label2 col-md-2">Redeemed By:&nbsp;</p>
                <asp:Label ID="lblRedeemedBy" runat="server" /><br />
                <p class="label2 col-md-2">Redeemed Date:&nbsp;</p>
                <asp:Label ID="lblRedeemedDate" runat="server" />

            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $("#chkIsCancelled").change(function () {
            var c = confirm("Are you sure that you want to change this order item's Cancellation status?");

            if (!c) {
                $("#chkIsCancelled").prop('checked', !($("#chkIsCancelled").is(":checked")));
            }
        });
    });
</script>
