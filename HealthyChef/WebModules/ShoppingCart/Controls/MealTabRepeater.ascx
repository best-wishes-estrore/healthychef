<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MealTabRepeater.ascx.cs" Inherits="HealthyChef.WebModules.ShoppingCart.MealTabRepeater" %>



<style>
    .healthychef-favorite-logo {
        height: 15px;

        
    }
    .options-logo {
        width: 20px;

    }
    .main-description img[src=''] {
        width:0px;
        height:0px;
    }
</style>
<div class="article-cnt">
    <asp:ListView ID="lvwMealItems" runat="server" DataKeyNames="MenuItemID">
        <LayoutTemplate>
            <div id="itemPlaceHolder" runat="server" />
        </LayoutTemplate>
        <ItemTemplate>
            <div>
                <asp:Label ID="lblHeaderMealType" runat="server" />
                <div class="mealItem">
                    <div class="main-description">
                        <span class="food-title"><%# Eval("Name") %></span>                                          
                        <span><asp:Image ID="imgCaynonRanchLogo" runat="server" ToolTip="Healthy Chef Favorite" CssClass="healthychef-favorite-logo" AlternateText="" ImageUrl="~/App_Themes/HealthyChef/images/Healthy-Chef-Logo-leaves.png" Visible='<%# ((bool?)Eval("CanyonRanchRecipe") ?? false) %>' /></span>
                        <span><asp:Image ID="imgSoyFree" runat="server" ToolTip="Soy Free Option Available" CssClass="options-logo" AlternateText="" ImageUrl="~/App_Themes/HealthyChef/Images/sf-button.png" Visible='<%# ((bool?)Eval("CanyonRanchApproved") ?? false)  %>' /></span>
                        <span><asp:Image ID="imgVegetarianOption" runat="server" ToolTip="Vegetarian Option Available" AlternateText="Vegetarian Option Available" CssClass="options-logo" ImageUrl="~/App_Themes/HealthyChef/Images/vg-button.png" Visible='<%# ((bool?)Eval("VegetarianOptionAvailable")?? false) %>' /></span>
                        <span><asp:Image ID="imgVeganOption" runat="server" ToolTip="Vegan Option Available" AlternateText="Vegan Option Available" CssClass="options-logo" ImageUrl="~/App_Themes/HealthyChef/Images/vn-button.png" Visible='<%# ((bool?)Eval("VeganOptionAvailable") ?? false) %>' /></span>
                        <span><asp:Image ID="imgGlutenFree" runat="server" ToolTip="Gluten Free Option Available" AlternateText="Gluten Free Option Available" CssClass="options-logo" ImageUrl="~/App_Themes/HealthyChef/Images/gf-button.png" Visible='<%#((bool?)Eval("GlutenFreeOptionAvailable")?? false) %>' /></span>
                        <div><%# Eval("Description") %></div>
                        <p><em>Allergens: <%# Eval("AllergensList").ToString().Trim().Substring(Eval("AllergensList").ToString().Trim().Length - 1, 1).Contains(",") ?  Eval("AllergensList").ToString().Trim().Substring(0, Eval("AllergensList").ToString().Trim().Length -1) : Eval("AllergensList") %></em></p>
                        <div class="fly-out" style="display: none; width: 100%;">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="vertical-align: top;width: 30%;padding-right: 1em;">
                                        <div class="fly-column" style="width: 100%;">
                                            <div id="divProfiles" runat="server" visible="false">
                                                <h3 class="font-myriad-cond color-olive">Select a Profile</h3>
                                                <asp:DropDownList ID="ddlProfiles" runat="server" Width="100%" CssClass="select-profile" />
                                            </div>
                                            <div id="divSizes" runat="server" visible="false">
                                                <h3 class="font-myriad-cond color-olive">Serving Size</h3>
                                                <table class="form-select-menu radio-sizes">
                                                    <asp:Repeater ID="rptMealSizes" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td class="left-box">
                                                                    <asp:RadioButton ID="rdoMealSize" runat="server" CssClass="meal-size" value='<%# Eval("SizeId") %>' data-price='<%# Eval("Price") %>' />
                                                                </td>
                                                                <td class="middle-box">
                                                                    <%# Eval("Description") %>
                                                                </td>
                                                                <td class="right-box">
                                                                    <%# string.Format("{0:c}", Eval("Price")) %>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                            </div>
                                        </div>
                                    </td>
                                    <td style="vertical-align: top;padding-right: 1em;">
                                        <div class="fly-column" style="width: 100%;">
                                            <div id="divPrefs" runat="server" visible="false">
                                                <h3 class="font-myriad-cond color-olive">Preferences</h3>
                                                <table class="form-select-menu radio-prefs" border="0">
                                                    <asp:Repeater ID="rptMealPrefs" runat="server" Visible="false">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td class="left-box">
                                                                    <asp:CheckBox ID="chkPref" runat="server" CssClass="meal-prefs" value='<%# Eval("Preference.PreferenceID") %>' data-price='<%# Eval("Preference.Cost") %>' />
                                                                </td>
                                                                <td class="middle-box">
                                                                    <%# Eval("Preference.Name") %>
                                                                </td>
                                                                <td class="right-box">
                                                                    <%# string.Format("{0:c}", Eval("Preference.Cost")) %>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                            </div>
                                        </div>
                                    </td>
                                    <td id="sidesDishesColumn" style="vertical-align: top;width: 40%;">
                                        <div class="fly-column" style="width: 100%;">
                                            <div id="divSidesDishes" runat="server" visible="true">
                                                <h3 class="font-myriad-cond color-olive">Select Side Dishes</h3>
                                                <table class="form-select-menu select-sides" border="0">
                                                    <tr>
                                                        <td>
                                                            <asp:DropDownList ID="ddlSide1" runat="server" Width="100%" Visible="false" CssClass="meal-side" style="margin-bottom: 2px;" meal-side='1' />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:DropDownList ID="ddlSide2" runat="server" Width="100%" Visible="false" CssClass="meal-side" style="" meal-side='2' />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="cl">&nbsp;</div>
                        </div>
                    </div>
                    <div class="side-column">
                        <a href="#" class="aSelectALC page-button page-button-arrow" title="Select">Select</a>
                        <div class="panel-add-to-cart" style="display: none; position: relative;">
                            Price: <span class="plan-price-text right-align">
                                <asp:Literal ID="ltlPrice" runat="server" />
                            </span>
                            <div style="height: 10px;"></div>
                            Quantity:
                            <asp:TextBox ID="txtQuantity" Text="1" Width="20" MaxLength="2" CssClass="right-align txt-quantity" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvQty" runat="server" ControlToValidate="txtQuantity" Display="Dynamic"
                                ErrorMessage="Quantity is required" Text="*" SetFocusOnError="true" ValidationGroup="AddToCartGroup" />
                            <asp:CompareValidator ID="cprQty" runat="server" ControlToValidate="txtQuantity" Display="Dynamic"
                                ErrorMessage="Quantity must be a number" Text="*" SetFocusOnError="true" ValidationGroup="AddToCartGroup" 
                                Type="Integer" Operator="DataTypeCheck" />
                            <div style="height: 10px;"></div>
                            <asp:Button ID="btnAddToCart" runat="server" Text="Add to Cart" OnClick="btnAddToCartClick" CssClass="addToCart"
                                 ValidationGroup="AddToCartGroup" />
                            <asp:Label ID="lblAddItemFeedback" runat="server" />
                            <asp:ValidationSummary ID="ValSumAtC" runat="server"  ValidationGroup="AddToCartGroup" Font-Size="XX-Small" />
                        </div>
                    </div>
                    <div class="cl">&nbsp;</div>
                </div>
            </div>
        </ItemTemplate>
    </asp:ListView>
    <div class="article-list">
     <span ><asp:Image ID="imgCaynonRanchLogo" runat="server" ToolTip="Healthy Chef Favorite" CssClass="healthychef-favorite-logo" AlternateText="" ImageUrl="~/App_Themes/HealthyChef/images/Healthy-Chef-Logo-leaves.png"/> Healthy Chef Favorite</span>
    <span><asp:Image ID="imgSoyFree" runat="server" ToolTip="Soy Free Option Available" CssClass="options-logo" AlternateText="" ImageUrl="~/App_Themes/HealthyChef/Images/sf-button.png" /> Soy Free Option Available</span>
<span><asp:Image ID="imgVegetarianOption" runat="server" ToolTip="Vegetarian Option Available" AlternateText="Vegetarian Option Available" CssClass="options-logo" ImageUrl="~/App_Themes/HealthyChef/Images/vg-button.png"/> Vegetarian Option Available</span>
<span><asp:Image ID="imgVeganOption" runat="server" ToolTip="Vegan Option Available" AlternateText="Vegan Option Available" CssClass="options-logo" ImageUrl="~/App_Themes/HealthyChef/Images/vn-button.png"/> Vegan Option Available</span>
<span><asp:Image ID="imgGlutenFree" runat="server" ToolTip="Gluten Free Option Available" AlternateText="Gluten Free Option Available" CssClass="options-logo" ImageUrl="~/App_Themes/HealthyChef/Images/gf-button.png"/> Gluten Free Option Available</span>
        </div>

</div>

