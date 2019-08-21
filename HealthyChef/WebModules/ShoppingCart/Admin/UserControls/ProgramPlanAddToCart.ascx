<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProgramPlanAddToCart.ascx.cs" Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.ProgramPlanAddToCart" %>

<div id="divProfiles" runat="server" visible="false">
    <asp:DropDownList ID="ddlProfiles" runat="server" />
    <asp:RequiredFieldValidator ID="rfvProfiles" runat="server" ControlToValidate="ddlProfiles"
        Text="*" ErrorMessage="A profile is required." SetFocusOnError="true" InitialValue="-1" />
    <br />
</div>
<asp:DropDownList ID="ddlStartDates" runat="server" />
<asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="ddlStartDates"
    Text="*" ErrorMessage="A start date is required." SetFocusOnError="true" InitialValue="-1" />
<br />
<asp:DropDownList ID="ddlPrograms" runat="server" AutoPostBack="true" />
<asp:RequiredFieldValidator ID="rfvPrograms" runat="server" ControlToValidate="ddlPrograms"
    Text="*" ErrorMessage="A program is required." SetFocusOnError="true" InitialValue="-1" />
<br />
<asp:DropDownList ID="ddlPlans" runat="server" AutoPostBack="true" Enabled="false" />
<asp:Label ID="lblPlanPrice" runat="server" EnableViewState="false" />
<asp:RequiredFieldValidator ID="rfvPlans" runat="server" ControlToValidate="ddlPlans"
    Text="*" ErrorMessage="A plan is required." SetFocusOnError="true" InitialValue="-1" />
<br />
<asp:DropDownList ID="ddlOptions" runat="server" AutoPostBack="true"  Enabled="false" />
<asp:RequiredFieldValidator ID="rfvOptions" runat="server" ControlToValidate="ddlOptions"
    Text="*" ErrorMessage="An option is required." SetFocusOnError="true" InitialValue="-1" />
<br />
<%--<asp:CheckBox ID="chkAutoRenew" runat="server" Text="Auto-Renew on Completion?" />
<br />--%>
<asp:TextBox ID="txtQuantity" runat="server" Text="1" MaxLength="3" />
<asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="txtQuantity"
    Text="*" ErrorMessage="A quantity is required." SetFocusOnError="true" />
<asp:RangeValidator ID="cpvQuantity" runat="server" ControlToValidate="txtQuantity"
    Text="*" ErrorMessage="Quantity must be greater than zero." SetFocusOnError="true"
     Type="Integer" MinimumValue="1" MaximumValue="999" />
<br />
<asp:Label ID="lblAutoRenew" runat="server" Text="Auto-Renew "></asp:Label> <asp:CheckBox runat="server" ID="cbxRecurring" ClientIDMode="Static" Text="" TextAlign="Left" Visible="True"/>
<asp:Button ID="btnSave" runat="server" Text="Add to Cart" />
<asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" />
<br />
<asp:ValidationSummary ID="ValSum1" runat="server" />

