<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BillingInfo_Edit.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.BillingInfo_Edit" %>
<%@ Register TagPrefix="hcc" TagName="AddressEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/Address_Edit.ascx" %>
<%@ Register TagPrefix="hcc" TagName="CreditCardEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/CreditCard_Edit.ascx" %>
<div class="plan_edit">
    <asp:Panel ID="pnlAddressEdit" runat="server" DefaultButton="btnSave">
        <div class="fieldRow">
            <div class="fieldCol">
                <hcc:AddressEdit ID="AddressEdit_Billing1" runat="server" ShowValidationSummary="false" AddressType="Billing" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <h3>Credit Card Information</h3>
                <asp:CheckBox ID="chkUpdateCard" runat="server" AutoPostBack="true" Enabled="false" />&nbsp;Update Card Information
            <hcc:CreditCardEdit ID="CreditCardEdit1" runat="server" ShowValidationSummary="false" EnableFields="false" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="pixels-button-user pixels-button-solid-green-user btn btn-info" OnClientClick="if(Page_ClientValidate()){ MakeUpdateProg(true); }" Visible="false" />
                <button type="button" ng-click="UpdateCreditCardInfo();" class="btn btn-info">Save</button>

                <div>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
                </div>
            </div>
        </div>
    </asp:Panel>
</div>
