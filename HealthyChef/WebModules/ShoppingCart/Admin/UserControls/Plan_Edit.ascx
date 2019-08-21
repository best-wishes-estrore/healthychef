<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Plan_Edit.ascx.cs" Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.Plan_Edit" %>

<div class="fieldRow col-sm-12">
    <div class="fieldCol">
        <asp:HiddenField ID="planid" runat="server" />
        <asp:Button ID="btnSave" CssClass="btn btn-info" runat="server" Text="Save" Visible="false" />
        <button type="button" ng-click="AddOrUpdatePlan();" class="btn btn-info">Save</button>
        <asp:Button ID="btnCancel" CssClass="btn btn-danger" runat="server" Text="Cancel" CausesValidation="false" Visible="false" />
        <a href="/WebModules/ShoppingCart/Admin/PlanManager.aspx" class="btn btn-danger">Cancel</a>
        <asp:Button ID="btnDeactivate" runat="server" Text="Deactivate" CausesValidation="false" CssClass="btn btn-info" />
        <div>
            <asp:ValidationSummary ID="ValSumPlans1" runat="server" />
        </div>
    </div>
</div>
<div class="col-sm-12">
    <div class="p-5" style="display: inline-block">
        <div class="fieldRow col-sm-4">
            <div class="fieldCol">
                <label>Plan Name:</label><br />
                <asp:TextBox runat="server" ID="txtPlanName" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvPlanName" runat="server" ControlToValidate="txtPlanName"
                    Text="*" Display="Dynamic" ErrorMessage="A Plan name is required." SetFocusOnError="true" />
                <asp:CustomValidator ID="cstPlanName" runat="server" ControlToValidate="txtPlanName"
                    EnableClientScript="false" Text="*" Display="Dynamic" ErrorMessage="There is already a plan that uses the name entered."
                    SetFocusOnError="true" OnServerValidate="cstName_ServerValidate" />
            </div>
        </div>
        <div class="fieldRow col-sm-4">
            <div class="fieldCol">
                <label>Program:</label><br />
                <asp:DropDownList ID="ddlPrograms" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlPrograms"
                    Text="*" Display="Dynamic" ErrorMessage="A Program is required." SetFocusOnError="true"
                    InitialValue="0" />
            </div>
        </div>
        <div class="fieldRow col-sm-4">
            <div class="fieldCol">
                <label>Is Default Plan for this Program?:</label><br />
                <asp:CheckBox ID="chkIsDefault" runat="server" />
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="fieldRow col-sm-4">
            <div class="fieldCol">
                <label>Sales Tax Eligible</label><br />
                <asp:CheckBox ID="chkIsTaxEligible" runat="server" />
            </div>
        </div>

        <div class="fieldRow col-sm-4">
            <div class="fieldCol">
                <label>Price Per Day ($):</label><br />
                <asp:TextBox ID="txtPricePerDay" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtPricePerDay"
                    Text="*" Display="Dynamic" ErrorMessage="A Price Per Day is required." SetFocusOnError="true" />
                <asp:CompareValidator ID="cprPricePerDay" runat="server" ControlToValidate="txtPricePerDay"
                    Text="*" Display="Dynamic" ErrorMessage="Price Per Day must be numeric." SetFocusOnError="true"
                    Operator="DataTypeCheck" Type="Double" />
            </div>
        </div>
        <div class="fieldRow col-sm-4">
            <div class="fieldCol">
                <label>Weeks of Meals:</label><br />
                <asp:DropDownList ID="ddlNumWeeks" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPlanDesc"
                    Text="*" Display="Dynamic" ErrorMessage="Weeks of Meals is required." SetFocusOnError="true"
                    InitialValue="0" />
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="fieldRow col-sm-4">
            <div class="fieldCol">
                <label>Days of Meals Per Week:</label><br />
                <asp:DropDownList ID="ddlNumDays" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlNumDays"
                    Text="*" Display="Dynamic" ErrorMessage="Days of Meals Per Week is required."
                    SetFocusOnError="true" InitialValue="0" />
            </div>
        </div>
        <div class="fieldRow col-sm-4">
            <div class="fieldCol">
                <label>Description:</label><br />
                <asp:TextBox runat="server" ID="txtPlanDesc" TextMode="MultiLine" Width="100%"  Columns="20" Rows="8" />
                <asp:RequiredFieldValidator ID="rfvPlanDesc" runat="server" ControlToValidate="txtPlanDesc"
                    Text="*" Display="Dynamic" ErrorMessage="A Plan description is required." SetFocusOnError="true" />
            </div>
        </div>
    </div>
</div>
