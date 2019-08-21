<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhoneNumber_Edit.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.PhoneNumber_Edit" %>
<asp:Panel ID="pnlPhone" runat="server" DefaultButton="btnAddPhone">
    <div class="fieldRow">
        <div class="fieldCol">
            Phone Number:
            <asp:TextBox ID="txtPhoneNumber" runat="server" ValidationGroup="PhoneGroup" MaxLength="20" />
            <asp:RequiredFieldValidator ID="rfvPhoneNumber" runat="server" ControlToValidate="txtPhoneNumber"
                Text="*" Display="Dynamic" ErrorMessage="Phone Number is required." SetFocusOnError="true" />
        </div>
        <div class="fieldCol">
            <asp:DropDownList ID="ddlPhoneTypes" runat="server" />
            <asp:RequiredFieldValidator ID="rfvPhoneType" runat="server" ControlToValidate="ddlPhoneTypes"
                Text="*" Display="Dynamic" ErrorMessage="Phone Type is required." SetFocusOnError="true"
                InitialValue="-1" />
        </div>
        <div class="fieldCol">
            <asp:CheckBox ID="chkIsPrimary" runat="server" Text="Is Primary Contact Number?" />
        </div>
        <div class="fieldCol">
            &nbsp;<asp:Button ID="btnAddPhone" runat="server" Text="Save Phone Number" />
        </div>
    </div>
    <div class="fieldRow">
        <div class="fieldCol">
            <asp:ValidationSummary ID="ValSum_Phone" runat="server" DisplayMode="BulletList" />
        </div>
    </div>
</asp:Panel>
