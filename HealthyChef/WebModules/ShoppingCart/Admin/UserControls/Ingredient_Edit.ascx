<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Ingredient_Edit.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.Ingredient_Edit" %>
<%@ Register Src="~/WebModules/Components/HealthyChef/EntityPicker.ascx" TagName="EntityPickerControl"
    TagPrefix="hcc" %>
<asp:Panel ID="pnlIngredientEdit" runat="server" DefaultButton="btnSave">
    <div class="col-sm-12">
        <div class="fieldRow col-sm-12">
            <div class="fieldCol">
                <asp:HiddenField ID="ingredientid" runat="server" />
                <asp:Button ID="btnSave" CssClass="btn btn-info" runat="server" Text="Save" Visible="false" />
                <button type="button" ng-click="AddOrUpdateIngredient();" class="btn btn-info" >Save</button>
                <asp:Button ID="btnCancel" CssClass="btn btn-danger" runat="server" Text="Cancel" CausesValidation="false" Visible="false" />
                <a href="/WebModules/ShoppingCart/Admin/IngredientManager.aspx" class="btn btn-danger">Cancel</a>
                <asp:Button ID="btnDeactivate" runat="server" Text="Retire" CausesValidation="false" CssClass="btn btn-info" />
               <%-- <button type="button" ng-click="UpdateRetireIngredient(true);" class="btn btn-primary" >Retire</button>--%>
                <div>
                    <asp:ValidationSummary ID="ValSumIngredients1" runat="server" />
                </div>
            </div>
        </div>
        <div class="fieldRow col-sm-3">
            <div class="fieldCol">
                Name:
            <asp:TextBox ID="txtIngredientName" runat="server" MaxLength="50" />
                <asp:RequiredFieldValidator ID="rfvIngredientName" runat="server" ControlToValidate="txtIngredientName"
                    Text="*" Display="Dynamic" ErrorMessage="A Ingredient name is required." SetFocusOnError="true" />
                <asp:CustomValidator ID="cstIngredName" runat="server" ControlToValidate="txtIngredientName"
                    EnableClientScript="false" Text="*" Display="Dynamic" ErrorMessage="There is already a ingredient that uses the name entered."
                    SetFocusOnError="true" OnServerValidate="cstName_ServerValidate" />
            </div>
        </div>
        <div class="fieldRow col-sm-3">
            <div class="fieldCol">
                Description:
            <asp:TextBox ID="txtIngredientDesc" runat="server" MaxLength="255" />
                <asp:RequiredFieldValidator ID="rfvIngredientDesc" runat="server" ControlToValidate="txtIngredientDesc"
                    Text="*" Display="Dynamic" ErrorMessage="A Ingredient description is required."
                    SetFocusOnError="true" />
            </div>
        </div>
        <div class="fieldRow col-sm-12">
            <div class="fieldCol">
                Allergens:
            <hcc:EntityPickerControl runat="server" ID="epAllergens" DataTextField="Name" DataValueField="AllergenID"
                Title="Selected Allergens"></hcc:EntityPickerControl>
            </div>
        </div>
    </div>
</asp:Panel>
<div class="clearfix"></div>
<div class="col-sm-12">
    <fieldset>
        <legend>Used In Items</legend>
        <asp:GridView ID="gvwIngUsage" runat="server" AutoGenerateColumns="false" ShowHeader="false">
            <Columns>
                <asp:HyperLinkField DataTextField="Name" ShowHeader="false" DataNavigateUrlFields="MenuItemID"
                    DataNavigateUrlFormatString="~/WebModules/ShoppingCart/Admin/ItemManager.aspx?i={0}" />
            </Columns>
            <EmptyDataTemplate>
                This ingredient is not currently being used in any items.
            </EmptyDataTemplate>
        </asp:GridView>
    </fieldset>
</div>
