<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductionCalendar_Edit.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.ProductionCalendar_Edit" %>
<%@ Register TagPrefix="hcc" TagName="EntityPicker" Src="~/WebModules/Components/HealthyChef/EntityPicker.ascx" %>
<div class="col-sm-12">
  
    <div class="fieldRow">
        <div class="fieldCol">
            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-info" Text="Save" Visible="false" />
            <button type="button" class="btn btn-info" ng-click="SaveCalendar();">Save</button>
            <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger" Text="Cancel" CausesValidation="false" Visible="false" />
            <a href="/WebModules/ShoppingCart/Admin/ProductionCalendar.aspx" class="btn btn-danger">Cancel</a>
            <%--<asp:Button ID="btnRetire" runat="server" Text="Retire" CausesValidation="false" />--%>
            <div>
                <asp:ValidationSummary ID="ValSumPrefs1" runat="server" />
            </div>
        </div>
    </div>
    <div class="fieldRow col-sm-3">
        <div class="fieldCol">
            Name:
        <asp:TextBox runat="server" ID="txtCalendarName" style="width:100%;"/>
            <asp:RequiredFieldValidator runat="server" ID="rfvCalendarName" ControlToValidate="txtCalendarName"
                Text="*" Display="Dynamic" ErrorMessage="Calendar Name is Required" />
            <asp:CustomValidator ID="cstCalendarName" runat="server" ControlToValidate="txtCalendarName"
                EnableClientScript="false" Text="*" Display="Dynamic" ErrorMessage="There is already a calendar that uses the name entered."
                SetFocusOnError="true" OnServerValidate="cstName_ServerValidate" />
        </div>
    </div>
    <div class="fieldRow col-sm-3">
        <div class="fieldCol">
            Menu:<br />
        <asp:DropDownList ID="ddlMenus" CssClass="form-control" runat="server" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="ddlMenus"
                Text="*" Display="Dynamic" ErrorMessage="A menu is Required" InitialValue="-1" />
        </div>
    </div>
    <div class="fieldRow col-sm-3">
        <div class="fieldCol">
            Order Delivery Date:
        <asp:TextBox runat="server" CssClass="form-control datepicker-fridays" ID="txtOrderDeliveryDate" AutoPostBack="true" />
           <%-- <ajax:CalendarExtender ID="calDelDate" runat="server" TargetControlID="txtOrderDeliveryDate"
                PopupPosition="Right" />--%>
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtOrderDeliveryDate"
                Text="*" Display="Dynamic" ErrorMessage="Select a Delivery Date" />
            <asp:CompareValidator ID="cprDelDate" runat="server" ControlToValidate="txtOrderDeliveryDate"
                Text="*" Display="Dynamic" ErrorMessage="Delivery Date must be a date value."
                Operator="DataTypeCheck" Type="Date" />
        </div>
    </div>
    <div class="fieldRow col-sm-3">
        <div class="fieldCol">
            Order Cut-Off Date
        <asp:TextBox runat="server" CssClass="form-control datepicker-thursdays" ID="txtOrderCutOffDate" Enabled="true" />
            <%--<ajax:CalendarExtender ID="calCutOff" runat="server" TargetControlID="txtOrderCutOffDate"
                PopupPosition="Right" />--%>
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtOrderCutOffDate"
                Text="*" Display="Dynamic" ErrorMessage="Select a Cut-Off Date" />
            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtOrderCutOffDate"
                Text="*" Display="Dynamic" ErrorMessage="Cut-Off Date must be a date value."
                Operator="DataTypeCheck" Type="Date" />
        </div>
    </div>
    <div class="fieldRow col-sm-3">
        <div class="fieldCol p-5">
            Description:
            <asp:TextBox runat="server" ID="txtDescription" placeholder="(Optional)" TextMode="MultiLine" Rows="5" Columns="40" />
        </div>
    </div>
</div>
