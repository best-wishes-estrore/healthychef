<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Preference_Edit.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.Preference_Edit" %>
<asp:Panel ID="pnlPreferenceEdit" runat="server" DefaultButton="btnSave">
    <div class="col-sm-12">
    <div class="fieldRow col-sm-12">
        <div class="fieldCol">
            <asp:HiddenField ID="preferenceid" runat="server" />
            <asp:Button ID="btnSave" CssClass="btn btn-info" runat="server" Text="Save" Visible="false"/>
            <button type="button" ng-click="AddOrUpdatePreference();" class="btn btn-info" >Save</button>
            <asp:Button ID="btnCancel" CssClass="btn btn-danger" runat="server" Text="Cancel" CausesValidation="false" Visible="false" />
            <a href="/WebModules/ShoppingCart/Admin/PreferencesManager.aspx" class="btn btn-danger">Cancel</a>
            <asp:Button ID="btnDeactivate" runat="server" Text="Retire" CausesValidation="false" CssClass="btn btn-info" />
            <div>
                <asp:ValidationSummary ID="ValSumPrefs1" runat="server" />
            </div>
        </div>
    </div>
    <div class="fieldRow col-sm-3">
        <div class="fieldCol p-5">
            Name:
            <asp:TextBox ID="txtPrefName" runat="server" MaxLength="50" />
            <ajax:TextBoxWatermarkExtender ID="tbwme1" runat="server" TargetControlID="txtPrefName"
                WatermarkText="Name" WatermarkCssClass="watermark" />
            <asp:RequiredFieldValidator ID="rfvPrefName" runat="server" ControlToValidate="txtPrefName"
                Text="*" Display="Dynamic" ErrorMessage="A preference name is required." SetFocusOnError="true" />
            <asp:CustomValidator ID="cstPrefName" runat="server" ControlToValidate="txtPrefName"
                EnableClientScript="false" Text="*" Display="Dynamic" ErrorMessage="There is already a preference that uses the name and meal type entered."
                SetFocusOnError="true" OnServerValidate="cstName_ServerValidate" />
        </div>
    </div>
    <div class="fieldRow col-sm-3">
        <div class="fieldCol p-5">
            Description:
            <asp:TextBox ID="txtPrefDesc" runat="server" MaxLength="255" />
            <ajax:TextBoxWatermarkExtender ID="tbwme2" runat="server" TargetControlID="txtPrefDesc"
                WatermarkText="Description" WatermarkCssClass="watermark" />
            <asp:RequiredFieldValidator ID="rfvPrefDesc" runat="server" ControlToValidate="txtPrefDesc"
                Text="*" Display="Dynamic" ErrorMessage="A preference description is required."
                SetFocusOnError="true" />
        </div>
    </div>
    <div class="fieldRow col-sm-3">
        <div class="fieldCol p-5">
            Type:<br />
            <asp:DropDownList ID="ddlPrefTypes" runat="server" />
            <asp:RequiredFieldValidator ID="rfvPrefTypes" runat="server" ControlToValidate="ddlPrefTypes"
                Text="*" Display="Dynamic" ErrorMessage="A preference type is required." SetFocusOnError="true"
                InitialValue="-1" />
        </div>
    </div>
        </div>
</asp:Panel>