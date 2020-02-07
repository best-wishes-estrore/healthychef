<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Display.ascx.cs" Inherits="BayshoreSolutions.WebModules.Cms.Security.UserRegistration.Display" %>

<%@ Register TagPrefix="hcc" TagName="AddressEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/Address_Edit.ascx" %>
<%@ Register TagPrefix="hcc" TagName="BillingInfoEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/BillingInfo_Edit.ascx" %>
<%@ Register TagPrefix="hcc" TagName="CreditCard" Src="~/WebModules/ShoppingCart/Admin/UserControls/CreditCard_Edit.ascx" %>

<bss:MessageBox ID="Msg" runat="server" />

<style type="text/css">
    .user-registation fieldset
    {
        margin: 20px 0;
        padding: 0;
        border: none;
    }

        .user-registation fieldset legend
        {
            margin: 0 0 0.3em 0;
            display: block;
            padding: 0;
            font-size: 2em;
            font-weight: normal;
            color: black;
            font-family: 'MyriadPro-BoldCond', arial, sans-serif;
        }

        .user-registation fieldset label
        {
            min-width: 120px;
            display: inline-block;
            margin: 0 1em 0 0;
            text-align: right;
        }

        .user-registation fieldset .fieldRow
        {
            margin-bottom: 0.4em;
        }
</style>

<div class="user-registation">
    <asp:Panel ID="pnl_registration" DefaultButton="SaveButton" runat="server">
        <fieldset>
            <legend>Shipping Information</legend>
            <hcc:AddressEdit ID="AddressEdit_Shipping1" runat="server" AddressType="Shipping" ShowIsBusiness="true" ShowDeliveryTypes="true"
                ValidationGroup="NewUserGroup" ShowValidationSummary="false" ValidationMessagePrefix="Shipping Address - " />
        </fieldset>
        <fieldset>
            <legend>Billing Information</legend>
            <p>
                <asp:CheckBox ID="chxSameBillingAddress" Text=" This is the same as my shipping address" OnCheckedChanged="chxSameBillingAddress_CheckedChanged" AutoPostBack="true" runat="server" />
            </p>
            <hcc:AddressEdit ID="AddressEdit_Billing1" runat="server" AddressType="Billing" EnableFields="true" ValidationGroup="NewUserGroup"
                ValidationMessagePrefix="Billing Address - " ShowValidationSummary="false" />
        </fieldset>
        <fieldset>
            <legend>Payment Information</legend>
            <hcc:CreditCard ID="CreditCard1" runat="server" ValidationGroup="NewUserGroup" ValidationMessagePrefix="Credit Card - " ShowValidationSummary="false" />
        </fieldset>
        <fieldset>
            <legend>Login Information</legend>
            <div class="fieldRow">
                <div class="fieldCol">
                    <asp:Label ID="lblEmail" AssociatedControlID="txtEmail" Text="E-mail: " runat="server" />
                    <asp:TextBox ID="txtEmail" MaxLength="200" Width="200" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" Text="*" Display="Dynamic"
                        ErrorMessage="Email is required." SetFocusOnError="true" />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail" Text="*" Display="Dynamic"
                        ErrorMessage="Must be a valid email address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                </div>
            </div>
            <div class="fieldRow">
                <div class="fieldCol">
                    <asp:Label ID="lblPassword" AssociatedControlID="txtPassword" Text="Password: " runat="server" />
                    <asp:TextBox ID="txtPassword" MaxLength="30" TextMode="Password" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                        Text="*" Display="Dynamic" ErrorMessage="Password is required." SetFocusOnError="true" ValidationGroup="NewUserGroup" />
                </div>
            </div>
            <div class="fieldRow">
                <div class="fieldCol">
                    <div style="display: inline;vertical-align:top;">
                        <asp:Label ID="lblPasswordRepeat" AssociatedControlID="txtPasswordRepeat" Text="Confirm Password: " runat="server" />
                        <asp:TextBox ID="txtPasswordRepeat" MaxLength="30" TextMode="Password" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvPasswordRepeat" runat="server" ControlToValidate="txtPasswordRepeat"
                            Text="*" Display="Dynamic" ErrorMessage="Password  is required." SetFocusOnError="true" ValidationGroup="NewUserGroup" />
                        <asp:CompareValidator ID="fcvPassword" Display="dynamic" ControlToValidate="txtPassword" ControlToCompare="txtPasswordRepeat"
                            Type="String" Text="*" ErrorMessage="Both passwords must match each other" runat="server" ValidationGroup="NewUserGroup" />
                    </div>
                    <div style="display: inline-block;">
                        <ul style="margin: 0;">
                            <li>Password must be at least 5 characters in length.</li>
                        </ul>
                    </div>
                </div>
            </div>
        </fieldset>
    </asp:Panel>
    <p style="padding-left: 20px;">
        <asp:Button ID="SaveButton" Text="Continue" OnClick="SaveButton_Click" CssClass="pixels-button pixels-button-solid-green" runat="server" ValidationGroup="NewUserGroup" OnClientClick="if(Page_ClientValidate()){ MakeUpdateProg(true); }"/>
    </p>
    <asp:ValidationSummary ID="val_summary" runat="server" ValidationGroup="NewUserGroup" />

</div>
