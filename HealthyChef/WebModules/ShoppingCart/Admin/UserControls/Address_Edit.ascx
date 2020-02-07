<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Address_Edit.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.Address_Edit" %>
<asp:Panel ID="pnlAddressEdit" runat="server" DefaultButton="btnSave">
    <p>
        <asp:PlaceHolder runat="server" ID="ShippingPlaceHolder" Visible="false">NOTE: Changes saved to shipping addresses in profiles will be applied to future purchases only, 
         and will not be automatically applied to future delivery weeks for existing orders from previous purchases. 
         To change the shipping address for a future delivery week for an existing order from a previous purchase, 
         please contact Customer Service. Shipping addresses for existing orders from previous purchases cannot be changed online.</asp:PlaceHolder>
    </p>
    <div class="table_row plan_edit">
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:Label ID="lblFirstName" AssociatedControlID="txtFirstName" Text="First Name:" runat="server" />
                <asp:TextBox ID="txtFirstName" runat="server" MaxLength="50" />
                <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName"
                    Text="*" Display="Dynamic" ErrorMessage="First Name is required." />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:Label ID="lblLastName" AssociatedControlID="txtLastName" Text="Last Name:" runat="server" />
                <asp:TextBox ID="txtLastName" runat="server" MaxLength="50" />
                <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName"
                    Text="*" Display="Dynamic" ErrorMessage="Last Name is required." />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:Label ID="lblAddress1" AssociatedControlID="txtAddress1" Text="Address 1:" runat="server" />
                <asp:TextBox ID="txtAddress1" runat="server" MaxLength="100" />
                <asp:RequiredFieldValidator ID="rfvAddress1" runat="server" ControlToValidate="txtAddress1"
                    Text="*" Display="Dynamic" ErrorMessage="Address 1 is required." SetFocusOnError="true" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:Label ID="lblAddress2" AssociatedControlID="txtAddress2" Text="Address 2:" runat="server" />
                <asp:TextBox ID="txtAddress2" runat="server" MaxLength="100" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:Label ID="lblCity" AssociatedControlID="txtCity" Text="City:" runat="server" />
                <asp:TextBox ID="txtCity" runat="server" MaxLength="30" />
                <asp:RequiredFieldValidator ID="rfvCity" runat="server" ControlToValidate="txtCity"
                    Text="*" Display="Dynamic" ErrorMessage="City is required." SetFocusOnError="true" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:Label ID="lblUSStates" AssociatedControlID="ddlUSStates" Text="State:" runat="server" />
                <asp:DropDownList ID="ddlUSStates" runat="server" />
                <asp:RequiredFieldValidator ID="rfvUSStates" runat="server" ControlToValidate="ddlUSStates"
                    Text="*" Display="Dynamic" ErrorMessage="State is required." SetFocusOnError="true"
                    InitialValue="-1" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:Label ID="lblZipCode" AssociatedControlID="txtZipCode" Text="Zip Code:" runat="server"/>
                <asp:TextBox ID="txtZipCode" runat="server" MinLength="5" MaxLength="5" />
                <asp:RequiredFieldValidator ID="rfvZipCode" runat="server" ControlToValidate="txtZipCode"
                    Text="*" Display="Dynamic" ErrorMessage="Zip Code is required." SetFocusOnError="true" />
                <asp:RegularExpressionValidator  Runat="server" ID="valNumbersOnly" ControlToValidate="txtZipCode" Display="Dynamic" ErrorMessage="Please enter a numbers only." ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)"></asp:RegularExpressionValidator>
                <%--<asp:RangeValidator ID="rvZipCode" runat="server" ControlToValidate="txtZipCode" Text="*"
                MinimumValue="5" MaximumValue="5" ErrorMessage="Zip code must be 5 digits" SetFocusOnError="true" />--%>
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:Label ID="lblPhone" AssociatedControlID="txtPhone" Text="Phone:" runat="server"/>
                <asp:TextBox ID="txtPhone" runat="server" MaxLength="20" />
               <asp:RegularExpressionValidator  Runat="server" ID="RegularExpressionValidator1" ControlToValidate="txtPhone" Display="Dynamic" ErrorMessage="Please enter a numbers only." ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)"></asp:RegularExpressionValidator>
                </div>
        </div>
        <div id="divShowDeliveryTypes" runat="server" class="fieldRow">
            <div class="fieldCol">
                <asp:Label ID="lblDeliveryTypes" AssociatedControlID="ddlDeliveryTypes" Text=" Delivery Type: " runat="server" />
                <asp:DropDownList ID="ddlDeliveryTypes" runat="server" />
            </div>
        </div>
        <div id="divIsBusiness" runat="server" class="fieldRow">
            <div class="fieldCol">
                <label class="col-sm-2">Is a Business Address?</label>
                <asp:CheckBox ID="chkIsBusiness" runat="server"  TextAlign="Left" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="pixels-button-user pixels-button-solid-green-user btn btn-info" Visible="false" />
                <button type="button" ng-click="UpdateShippingAddress();" class="btn btn-info" id="SaveAddressButton" runat="server">Save</button>
                  <button type="button" ng-click="UpdateBillingInfo();" visible="false" class="btn btn-info" id="SaveAddressBillingInfoButton" runat="server">Save Billing Info</button>
                 <p id="billinginfonote" visible="false" runat="server">Billing Info must be saved before you can add a credit card</p>
                <div>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
                </div>
                <div>
                    <asp:Label ID="lblFeedback0" runat="server" ForeColor="Green" EnableViewState="false" />
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdnAddId" runat="server" />
    </div>
</asp:Panel>

<%--Removed at Duncan's request on 7/31/2013. Does not want to see this warning message when updating Billing Info. --%>
<%--<script type="text/javascript">
    $(document).ready(function () {
        if (document.URL.indexOf('login.aspx') == -1) {
            var dc = "addr" + "<%= this.AddressType.ToString() %>";
            $("[id$=pnlAddressEdit]").filter('[data-ctrl="' + dc + '"]').find(":input").change(function () {
                $("[id$=lblFeedback0]").filter('[data-ctrl="' + dc + '"]')
                  .text('There may be unsaved values in this section. If you have changed information in this section, please be sure to save your changes.')
                    .css("color", "#F60");
            });
        }
    });
</script>--%>