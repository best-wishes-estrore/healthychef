<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Allergen_Edit.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.Allergen_Edit" %>
<asp:Panel ID="pnlAllergenEdit" runat="server" DefaultButton="btnSave">
    <div class="fieldRow col-sm-12">
        <div class="fieldCol">
            <asp:HiddenField ID="allergenID" runat="server" />
            <asp:Button ID="btnSave" CssClass="btn btn-info" runat="server" Text="Save" Visible="false" />
            <button type="button" ng-click="AddOrUpdateAllergen();" class="btn btn-info" >Save</button>
            <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger" Text="Cancel" CausesValidation="false" Visible="false" />
            <a href="/WebModules/ShoppingCart/Admin/AllergenManager.aspx" class="btn btn-danger">Cancel</a>
            <asp:Button ID="btnDeactivate" runat="server" Text="Retire" CausesValidation="false" CssClass="btn btn-info" />            
           <%-- <button type="button" ng-click="UpdateRetireAllergen();" class="btn btn-danger" >Retire</button>--%>
            <div>
                <asp:ValidationSummary ID="ValSumAllergens1" runat="server" />
            </div>
        </div>
    </div>
    <div class="col-sm-12">
        <div class="p-5">
            <div class="fieldRow col-sm-3">
                <div class="fieldCol">
                    <label>Name:</label><br />
                    <asp:TextBox ID="txtAllergenName" runat="server" MaxLength="50" CssClass="form-control" placeholder="Name" required="required" />
                    <%--<ajax:TextBoxWatermarkExtender ID="tbwme1" runat="server" TargetControlID="txtAllergenName"
                        WatermarkText="Name" WatermarkCssClass="watermark" />--%>
                    <asp:RequiredFieldValidator ID="rfvAllergenName" runat="server" ControlToValidate="txtAllergenName"
                        Text="*" Display="Dynamic" ErrorMessage="An allergen name is required." SetFocusOnError="true" />
                    <asp:CustomValidator ID="cstAlrgnName" runat="server" ControlToValidate="txtAllergenName"
                        EnableClientScript="false" Text="*" Display="Dynamic" ErrorMessage="There is already a allergen that uses the name entered."
                        SetFocusOnError="true" OnServerValidate="cstName_ServerValidate" />
                </div>
            </div>
            <div class="fieldRow col-sm-3">
                <div class="fieldCol">
                    <label>Description:</label>
                    <asp:TextBox ID="txtAllergenDesc" runat="server" MaxLength="255" CssClass="form-control" placeholder="Description" required="required"/>
                    <%--<ajax:TextBoxWatermarkExtender ID="tbwme2" runat="server" TargetControlID="txtAllergenDesc"
                        WatermarkText="Description" WatermarkCssClass="watermark" />--%>
                    <asp:RequiredFieldValidator ID="rfvAllergenDesc" runat="server" ControlToValidate="txtAllergenDesc"
                        Text="*" Display="Dynamic" ErrorMessage="An allergen description is required."
                        SetFocusOnError="true" />
                </div>
            </div>
        </div>
    </div>
</asp:Panel>
<div class="clearfix"></div>
    <div class="m-2">
<fieldset>
        <legend>Used In Items</legend>
        <asp:GridView ID="gvwIngUsage" runat="server" AutoGenerateColumns="false" AllowPaging="true"
            PageSize="50" ShowHeader="false">
            <Columns>
                <asp:HyperLinkField DataTextField="Name" ShowHeader="false" DataNavigateUrlFields="IngredientID"
                    DataNavigateUrlFormatString="~/WebModules/ShoppingCart/Admin/IngredientManager.aspx?i={0}" />
            </Columns>
            <HeaderStyle Height="0px" />
            <EmptyDataTemplate>
                This ingredient is not currently being used in any items.
            </EmptyDataTemplate>
        </asp:GridView>
</fieldset>
    </div>
