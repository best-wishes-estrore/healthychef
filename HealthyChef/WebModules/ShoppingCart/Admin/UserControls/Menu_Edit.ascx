<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Menu_Edit.ascx.cs" Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.Menu_Edit" %>
<%@ Register TagPrefix="hcc" TagName="EntityPicker" Src="~/WebModules/Components/HealthyChef/EntityPicker.ascx" %>
<script type="text/javascript">
    $(function () {
        $("#divMenuEditTabs").tabs({
            cookie: {
                // store cookie for a day, without, it would be a session cookie
                expires: 1
            }
        });
    });
</script>

<div class="fieldRow">
    <div class="fieldCol">
        <asp:Button ID="btnSave" CssClass="btn btn-info" runat="server" Text="Save" />
        <%--<button type="button" class="btn btn-info" ng-click="SaveMenu()">save</button>--%>
        <asp:Button ID="btnCancel" CssClass="btn btn-danger" runat="server" Text="Cancel" CausesValidation="false" Visible="false" />
        <a href="/WebModules/ShoppingCart/Admin/MenuManager.aspx" class="btn btn-danger">Cancel</a>
        <div>
            <asp:ValidationSummary ID="ValidationSummary" runat="server" />
        </div>
    </div>
</div>
<div class="m-2">
<div class="fieldRow">
    <div class="fieldCol">
        Name:
        <asp:TextBox runat="server" ID="txtMenuName" MaxLength="255" Width="400px" />
        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtMenuName"
            Text="*" Display="Dynamic" ErrorMessage="Name is required." />
        <asp:CustomValidator ID="cstIngredName" runat="server" ControlToValidate="txtMenuName"
            EnableClientScript="false" Text="*" Display="Dynamic" ErrorMessage="There is already a menu that uses the name entered."
            SetFocusOnError="true" OnServerValidate="cstName_ServerValidate" />
    </div>
</div>
    </div>
<div class="fieldRow">
    <div class="fieldCol">
        <div id="divMenuEditTabs">
            <ul>
                <li><a href="#breakfasts">Breakfast</a></li>
                <li><a href="#lunches">Lunch</a></li>
                <li><a href="#dinners">Dinner</a></li>
                <li><a href="#kidsmeals">Child Meal</a></li>
                <li><a href="#dessert">Dessert</a></li>
                <li><a href="#othergoods">Other</a></li>
            </ul>
            <div id="breakfasts">
                <hcc:EntityPicker runat="server" ID="epBreakfast_BreakfastEntrees" DataTextField="Name" DataValueField="MenuItemId" Title="Breakfast Entrees" />
                <hcc:EntityPicker runat="server" ID="epBreakfast_BreakfastSides" DataTextField="Name" DataValueField="MenuItemId" Title="Breakfast Sides" />
            </div>
            <div id="lunches">
                <hcc:EntityPicker runat="server" ID="epLunch_LunchEntrees" DataTextField="Name" DataValueField="MenuItemId" Title="Lunch Entrees" />
                <hcc:EntityPicker runat="server" ID="epLunch_LunchSides" DataTextField="Name" DataValueField="MenuItemId" Title="Lunch Sides" />
            </div>
            <div id="dinners">
                <hcc:EntityPicker runat="server" ID="epDinner_DinnerEntrees" DataTextField="Name" DataValueField="MenuItemId" Title="Dinner Entrees" />
                <hcc:EntityPicker runat="server" ID="epDinner_DinnerSides" DataTextField="Name" DataValueField="MenuItemId" Title="Dinner Sides" />
            </div>
            <div id="kidsmeals">
                <hcc:EntityPicker runat="server" ID="epChild_ChildEntrees" DataTextField="Name" DataValueField="MenuItemId" Title="Child Entrees" />
                <hcc:EntityPicker runat="server" ID="epChild_ChildSides" DataTextField="Name" DataValueField="MenuItemId" Title="Child Sides" />
            </div>
            <div id="dessert">
                <hcc:EntityPicker runat="server" ID="epDeserts" DataTextField="Name" DataValueField="MenuItemId" Title="Desserts" />
            </div>
            <div id="othergoods">
                <hcc:EntityPicker runat="server" ID="epOther_OtherEntrees" DataTextField="Name" DataValueField="MenuItemId" Title="Other Entrees" />
                <hcc:EntityPicker runat="server" ID="epOther_OtherSides" DataTextField="Name" DataValueField="MenuItemId" Title="Other Sides" />
                <hcc:EntityPicker runat="server" ID="epOther_Soups" DataTextField="Name" DataValueField="MenuItemId" Title="Soups" />
                <hcc:EntityPicker runat="server" ID="epOther_Salads" DataTextField="Name" DataValueField="MenuItemId" Title="Salads" />
                <hcc:EntityPicker runat="server" ID="epOther_Beverages" DataTextField="Name" DataValueField="MenuItemId" Title="Beverages" />
                <hcc:EntityPicker runat="server" ID="epOther_Snacks" DataTextField="Name" DataValueField="MenuItemId" Title="Snacks" />
                <hcc:EntityPicker runat="server" ID="epOther_Supplements" DataTextField="Name" DataValueField="MenuItemId" Title="Supplements" />
                <hcc:EntityPicker runat="server" ID="epOther_Goods" DataTextField="Name" DataValueField="MenuItemId" Title="Goods" />
                <hcc:EntityPicker runat="server" ID="epOther_Misc" DataTextField="Name" DataValueField="MenuItemId" Title="Miscellaneous" />
                <hcc:EntityPicker runat="server" ID="epCustomer_Pickup_Weekly_Fee" DataTextField="Name" DataValueField="MenuItemId" Title="Customer Pickup Weekly Fee" />
            </div>
        </div>
    </div>
</div>
