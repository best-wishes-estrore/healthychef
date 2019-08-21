<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Coupon_Edit.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.Coupon_Edit" %>
<asp:Panel ID="pnlPreferenceEdit" runat="server" DefaultButton="btnSave">
    <div class="col-sm-12">
        <div class="fieldRow col-sm-12">
            <asp:HiddenField ID="couponID" runat="server" />
            <asp:Button ID="btnSave" CssClass="btn btn-info" runat="server" Text="Save" Visible="false" />
            <button type="button" ng-click="AddOrUpdateCoupon();" class="btn btn-info">Save</button>
            <asp:Button ID="btnCancel" CssClass="btn btn-danger" runat="server" Text="Cancel" CausesValidation="false" Visible="false" />
            <a href="/WebModules/ShoppingCart/Admin/CouponManager.aspx" class="btn btn-danger">Cancel</a>
            <asp:Button ID="btnDeactivate" runat="server" Text="Deactivate" CausesValidation="false" CssClass="btn btn-info" />
            <div>
                <asp:ValidationSummary ID="ValSumCoupon1" runat="server" />
            </div>
        </div>
        <div class="fieldRow col-sm-3">
            <div class="fieldCol">
                Redeem Code:
            <asp:TextBox ID="txtRedeemCode" runat="server" MaxLength="20" AutoPostBack="true" />
                 <asp:Label ID="lblcouponvalidation" runat="server" ForeColor="Red" />
                <asp:RequiredFieldValidator ID="rfvRedeemCode" runat="server" ControlToValidate="txtRedeemCode"
                    Text="*" Display="Dynamic" ErrorMessage="A redeem code is required." SetFocusOnError="true" />
            </div>
        </div>
        <div class="fieldRow col-sm-3">
            <div class="fieldCol">
                Title:
            <asp:TextBox ID="txtTitle" runat="server" MaxLength="255" />
                <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtTitle"
                    Text="*" Display="Dynamic" ErrorMessage="A title is required." SetFocusOnError="true" />
            </div>
        </div>
        <div class="fieldRow col-sm-3">
            Description:
        <asp:TextBox ID="txtDescription" runat="server" />(optional)
        </div>
        <div class="fieldRow col-sm-3">
            <div class="fieldCol">
                Amount:
            <asp:TextBox ID="txtAmount" runat="server" MaxLength="5" TextMode="Number" /><br />
                <asp:RequiredFieldValidator ID="rfvAmount" runat="server" ControlToValidate="txtAmount"
                    Text="*" Display="Dynamic" ErrorMessage="An amount is required." SetFocusOnError="true" />
                <asp:CompareValidator ID="cpvAmount" runat="server" ControlToValidate="txtAmount"
                    Text="*" Display="Dynamic" ErrorMessage="An amount must be numeric." SetFocusOnError="true" Operator="DataTypeCheck" Type="Double" />
                <asp:DropDownList ID="ddlDiscountTypes" CssClass="m-5" runat="server" /><br />
                <asp:RequiredFieldValidator ID="rfvDiscountTypes" runat="server" ControlToValidate="ddlDiscountTypes"
                    Text="*" Display="Dynamic" ErrorMessage="A discount type is required." SetFocusOnError="true"
                    InitialValue="-1" />
            </div>
            <div class="fieldCol">
                <asp:DropDownList ID="ddlUsageTypes" runat="server" />
                <asp:RequiredFieldValidator ID="rfvUsageTypes" runat="server" ControlToValidate="ddlUsageTypes"
                    Text="*" Display="Dynamic" ErrorMessage="A usage type is required." SetFocusOnError="true"
                    InitialValue="-1" />
            </div>
        </div>
        <div class="fieldRow col-sm-3">
            <div class="fieldCol">
                Start Date:
            <asp:TextBox ID="txtStartDate" CssClass="form-control datepicker1" runat="server" />
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtStartDate"
                    Text="Start Date must be a date." Display="Dynamic" ErrorMessage="Start Date must be a date." SetFocusOnError="true" Operator="DataTypeCheck" Type="Date" />
                (optional)
           <%-- <ajax:CalendarExtender ID="calStartDate" runat="server" TargetControlID="txtStartDate"
                PopupPosition="TopRight" />--%>
            </div>
        </div>
        <div class="fieldRow col-sm-3">
            <div class="fieldCol">
                End Date:
            <asp:TextBox ID="txtEndDate" CssClass="form-control datepicker1" runat="server" />
                <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="txtEndDate"
                    Text="End Date must be a date." Display="Dynamic" ErrorMessage="End Date must be a date." SetFocusOnError="true" Operator="DataTypeCheck" Type="Date" />
                (optional)
          
                <%--<ajax:CalendarExtender ID="calExpDate" runat="server" TargetControlID="txtEndDate"
                    PopupPosition="TopRight" />--%>
            </div>
        </div>
        <div class="col-sm-12">
            <asp:CompareValidator ID="cpvEffectDates" runat="server" ControlToValidate="txtEndDate"
                ControlToCompare="txtStartDate" Text="Start Date must be before End Date" Display="Dynamic" ErrorMessage="Start Date must be before End Date"
                SetFocusOnError="true" Operator="GreaterThan" Type="Date" />
        </div>
    </div>
</asp:Panel>
