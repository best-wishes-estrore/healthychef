<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuItem_Edit.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.MenuItem_Edit" %>
<%@ Register Src="~/WebModules/Components/HealthyChef/EntityPicker.ascx" TagName="EntityPickerControl"
    TagPrefix="hcc" %>
<%--<%@ Register Src="~/WebModules/ShoppingCart/Admin/UserControls/ImageManager.ascx"
    TagName="ImageManagerControl" TagPrefix="hcc" %>--%>
<%@ Register Src="~/WebModules/Components/HealthyChef/SliderControl.ascx" TagPrefix="hcc"
    TagName="Slider" %>
<script type="text/javascript">
    $(function () {
        $("#divMenuItemEditTabs").tabs({
            cookie: {
                // store cookie for a day, without, it would be a session cookie
                expires: 1
            }
        });
    });
</script>

<div class="fieldRow">
    <div class="fieldCol">
        <asp:Button ID="btnSave" class="btn btn-info" runat="server" Text="Save" Visible="true" />
        <%--  <button type="button" class="btn btn-info" ng-click="SaveMenu()">Save</button>--%>
        <asp:Button ID="btnCancel" class="btn btn-danger" runat="server" Text="Cancel" CausesValidation="false" Visible="false" />
        <a href="/WebModules/ShoppingCart/Admin/ItemManager.aspx" class="btn btn-danger">Cancel</a>
        <asp:Button ID="btnDeactivate" class="btn btn-info" runat="server" Text="Deactivate" CausesValidation="false" />
        <div>
            <asp:ValidationSummary ID="ValSum1" runat="server" />
        </div>
    </div>
</div>
<div class="col-md-8">
    <div class="plan_edit5 m-2">
        <div class="fieldRow">
            <div class="fieldCol">
                <label>Meal Type<span class="required">*</span></label>
                <asp:DropDownList runat="server" ID="ddlMealType" />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="ddlMealType"
                    InitialValue="-1" Text="*" Display="Dynamic" ErrorMessage="A meal type is required." />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <label>Name<span class="required">*</span></label>
                <asp:TextBox runat="server" ID="txtMenuItemName" MaxLength="50" Width="400px" />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtMenuItemName"
                    Text="*" Display="Dynamic" ErrorMessage="Name is required." />
                <asp:CustomValidator ID="cstMenuItemName" runat="server" ControlToValidate="txtMenuItemName"
                    EnableClientScript="false" Text="*" Display="Dynamic" ErrorMessage="There is already an item that uses the name and meal-type entered."
                    SetFocusOnError="true" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <label style="float: left; width: 23%;">Upload Image</label>
                <input type="text" runat="server" id="MenuItemImageName" disabled="disabled" style="border: none; width: 64%; float: left;" />
                <div class="input-group">
                    <span class="input-group-btn">
                        <span class="btn btn-file" style="background: none !important;">
                            <input type="file" style="margin-left: 23%;" runat="server" id="MenuItemImage"></span>
                    </span>
                    <img src="" width="200px" id='upload-img-preview' />
                </div>
            </div>
        </div>

        <div class="fieldRow">
            <div class="fieldCol">
                <label style="width:180px">Number of Side Dishes </label>
                <asp:DropDownList ID="ddlsidedishes" runat="server">
                    <asp:ListItem Text="0" Value="0"></asp:ListItem>
                    <asp:ListItem Text="1" Value="1"></asp:ListItem>
                    <asp:ListItem Text="2" Value="2"></asp:ListItem>
                </asp:DropDownList>

            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:CheckBox ID="chkIsTaxEligible" runat="server" Text="Sales Tax Eligible" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:CheckBox ID="chkUsePriceChild" runat="server" />
                <label>Price (Child)($)<span class="required">*</span></label>
                <asp:TextBox runat="server" ID="txtCostChild" MaxLength="10" onkeypress="return isNumeric(event)" />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6" ControlToValidate="txtCostChild"
                    Text="*" Display="Dynamic" ErrorMessage="Price (Child) is required." />
                <asp:CompareValidator runat="server" ID="CompareValidator3" Type="Currency" ControlToValidate="txtCostChild"
                    Operator="DataTypeCheck" Text="*" Display="Dynamic" ErrorMessage="Price (Child) must be empty, or a currency value" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:CheckBox ID="chkUsePriceSmall" runat="server" />
                <label>Price (Small)($)<span class="required">*</span></label>
                <asp:TextBox runat="server" ID="txtCostSmall" MaxLength="10" onkeypress="return isNumeric(event)" />
                <asp:RequiredFieldValidator runat="server" ID="rfvCost" ControlToValidate="txtCostSmall"
                    Text="*" Display="Dynamic" ErrorMessage="Price (Small) is required." />
                <asp:CompareValidator runat="server" ID="ComapreValidator1" Type="Currency" ControlToValidate="txtCostSmall"
                    Operator="DataTypeCheck" Text="*" Display="Dynamic" ErrorMessage="Price (Small) must be empty, or a currency value" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:CheckBox ID="chkUsePriceRegular" runat="server" />
                <label>Price (Regular)($)<span class="required">*</span></label>
                <asp:TextBox runat="server" ID="txtCostRegular" MaxLength="10" onkeypress="return isNumeric(event)" />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="txtCostRegular"
                    Text="*" Display="Dynamic" ErrorMessage="Price (Regular) is required." />
                <asp:CompareValidator runat="server" ID="CompareValidator1" Type="Currency" ControlToValidate="txtCostRegular"
                    Operator="DataTypeCheck" Text="*" Display="Dynamic" ErrorMessage="Price (Regular) must be empty, or a currency value" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:CheckBox ID="chkUsePriceLarge" runat="server" />
                <label>Price (Large)($)<span class="required">*</span></label>
                <asp:TextBox runat="server" ID="txtCostLarge" MaxLength="10" onkeypress="return isNumeric(event)" />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="txtCostLarge"
                    Text="*" Display="Dynamic" ErrorMessage="Price (Large) is required." />
                <asp:CompareValidator runat="server" ID="CompareValidator2" Type="Currency" ControlToValidate="txtCostLarge"
                    Operator="DataTypeCheck" Text="*" Display="Dynamic" ErrorMessage="Price (Large) must be empty, or a currency value" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol desc">
                <fieldset>
                    <legend>Description<span class="required">*</span></legend>
                    <asp:TextBox runat="server" ID="txtDescription" Columns="60" Rows="5" TextMode="MultiLine" />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtDescription"
                        Text="*" Display="Dynamic" ErrorMessage="A description is required." />
                </fieldset>
            </div>
        </div>
    </div>
</div>
<div class="col-md-4">
    <div class="m-2 align-label pull-right" style="border: 1px solid #ddd; padding: 10px">
        <table>
            <tr>
                <td>
                    <asp:CheckBox ID="cbCanyonRanchRecipe" runat="server" />
                </td>
                <td>
                    <label>Healthy Chef Favorite</label></td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="cbCanyonRanchApproved" runat="server" />
                </td>
                <td>
                    <label>Soy Free</label></td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="cbVegetarianOption" runat="server" />
                </td>
                <td>
                    <label>Vegetarian</label></td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="cbVeganOption" runat="server" />
                </td>
                <td>
                    <label>Vegan</label></td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="cbGlutenFreeOption" runat="server" />
                </td>
                <td>
                    <label>Gluten-free</label></td>
            </tr>
        </table>
    </div>
</div>


<div class="fieldRow">
    <div class="fieldCol">
        <div id="divMenuItemEditTabs" class="m-2">
            <ul>
                <li><a href="#nutritionData">Nutrition Info</a></li>
                <li><a href="#ingredients">Ingredients</a></li>
                <li><a href="#preferences">Preferences</a></li>
                <li><a href="#usedinmenus">Used In Menus</a></li>
            </ul>
            <div id="nutritionData">
                <asp:Panel runat="server" ID="NutrionalDataPanel">
                    <table class="nutritionalData">
                        <tr>
                            <td>
                                <span class="fieldLabel">Calories<span class="required">*</span></span>
                                <asp:TextBox runat="server" ID="txtCalories" onkeypress="return isNumeric1(this,event)" />&nbsp;(kcal) 
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCalories" ID="txtValidator1"
                                    Text="" Display="Dynamic" ErrorMessage="" />
                                <asp:CompareValidator runat="server" ControlToValidate="txtCalories" ID="txtValidator2"
                                    Text="*" Display="Dynamic" ErrorMessage="" Operator="DataTypeCheck" Type="Double" />
                            </td>
                            <td>
                                <span class="fieldLabel">Total Fat<span class="required">*</span></span>
                                <asp:TextBox runat="server" ID="txtTotalFat" onkeypress="return isNumeric1(this,event)" />&nbsp;(g)
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTotalFat" ID="txtValidator12"
                                    Text="" Display="Dynamic" ErrorMessage="" />
                                <asp:CompareValidator runat="server" ControlToValidate="txtTotalFat" ID="txtValidator22"
                                    Text="*" Display="Dynamic" ErrorMessage="" Operator="DataTypeCheck" Type="Double" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="fieldLabel">Total Carbs<span class="required">*</span></span>
                                <asp:TextBox runat="server" ID="txtTotalCarbohydrates" onkeypress="return isNumeric1(this,event)" />&nbsp;(g)
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTotalCarbohydrates"
                                    ID="txtValidator17" Text="" Display="Dynamic" ErrorMessage="" />
                                <asp:CompareValidator runat="server" ControlToValidate="txtTotalCarbohydrates" ID="txtValidator27"
                                    Text="*" Display="Dynamic" ErrorMessage="" Operator="DataTypeCheck" Type="Double" />
                            </td>
                            <td>
                                <span class="fieldLabel">Dietary Fiber<span class="required">*</span></span>
                                <asp:TextBox runat="server" ID="txtDietaryFiber" onkeypress="return isNumeric1(this,event)" />&nbsp;(g)
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDietaryFiber" ID="txtValidator18"
                                    Text="" Display="Dynamic" ErrorMessage="" />
                                <asp:CompareValidator runat="server" ControlToValidate="txtDietaryFiber" ID="txtValidator28"
                                    Text="*" Display="Dynamic" ErrorMessage="" Operator="DataTypeCheck" Type="Double" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="fieldLabel">Protein<span class="required">*</span></span>
                                <asp:TextBox runat="server" ID="txtProtein" onkeypress="return isNumeric1(this,event)" />&nbsp;(g)
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtProtein" ID="txtValidator110"
                                    Text="" Display="Dynamic" ErrorMessage="" />
                                <asp:CompareValidator runat="server" ControlToValidate="txtProtein" ID="txtValidator210"
                                    Text="*" Display="Dynamic" ErrorMessage="" Operator="DataTypeCheck" Type="Double" />
                            </td>
                            <td>
                                <span class="fieldLabel">Sodium<span class="required">*</span></span>
                                <asp:TextBox runat="server" ID="txtSodium" onkeypress="return isNumeric1(this,event)" />&nbsp;(g)
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtSodium" ID="txtvalidator35"
                                    Text="" Display="Dynamic" ErrorMessage="" />
                                <asp:CompareValidator runat="server" ControlToValidate="txtSodium" ID="txtValidator30"
                                    Text="*" Display="Dynamic" ErrorMessage="" Operator="DataTypeCheck" Type="Double" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <div id="ingredients" class="desc">
                <hcc:EntityPickerControl runat="server" ID="lstIngredients" DataTextField="Name"
                    DataValueField="IngredientID" Title="Selected Ingredients" />
            </div>
            <div id="preferences" class="desc">
                <hcc:EntityPickerControl runat="server" ID="lstPreferences" DataTextField="Name"
                    DataValueField="PreferenceID" Title="Selected Preferences" />
            </div>
            <div id="usedinmenus">
                <asp:GridView ID="gvwItemUsage" runat="server" AutoGenerateColumns="false" ShowHeader="false">
                    <Columns>
                        <asp:HyperLinkField DataTextField="Name" ShowHeader="false" DataNavigateUrlFields="MenuID"
                            DataNavigateUrlFormatString="~/WebModules/ShoppingCart/Admin/MenuManager.aspx?m={0}" />
                    </Columns>
                    <HeaderStyle Height="0px" />
                    <EmptyDataTemplate>
                        This item is not currently being used in any menus.
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
    </div>
</div>
<style>
    .plan_edit5 label {
        width: 23%;
        float: initial;
    }

    .plan_edit5 select {
        float: initial;
    }

    .desc legend {
        margin: 0px;
        text-align: left;
        font-size: 16px;
    }

    .desc fieldset {
        border-radius: 0px;
        margin-bottom: 0px;
    }

    #divMenuItemEditTabs select option {
        padding: 0px 0px 1px;
    }

    .required {
        content: "*";
        color: red;
    }

    .btn-file {
        background-color: none !important;
        border-radius: 0px;
        border: none;
    }
</style>
<script>
    function readURL(input) {

        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#upload-img-preview').attr('src', e.target.result);
            }

            reader.readAsDataURL(input.files[0]);
        }
    }

    $("#MenuItemImage").change(function () {
        readURL(this);
    });

    function isNumeric(evt) {
        var isValidNumeric = true;
        var isValidSpecialChar = true;
        var isAlpha = true;
        var specialKeys = new Array();
        specialKeys.push(46);  // .


        var charCode = (evt.which) ? evt.which : event.keyCode;

        if ((charCode < 65 || charCode > 90) && (charCode < 97 || charCode > 123) && charCode != 32) {
            isAlpha = false;
        }
        if ((charCode == 46) || (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))) {
            isValidNumeric = false;
        }
        var ret = (specialKeys.indexOf(evt.keyCode) == -1)//&& evt.charCode == evt.keyCode);
        if (ret == true) {
            isValidSpecialChar = false;
        }
        if (isAlpha == false) {
            if (isValidNumeric == true || isValidSpecialChar == true) {
                return true;
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
    }
    //function isNumeric1(evt) {
    //    debugger;
    //    var charCode = (evt.which) ? evt.which : event.keyCode;
    //    if ((charCode == 46) || (charCode != 46 && charCode > 31
    //        && (charCode < 48 || charCode > 57))) {
    //        if (charCode === 46) {
    //            if (evt.value.indexOf('.') >= 0) {
    //                return true;
    //            }
    //            else {
    //                return false;
    //            }
    //        }
    //        else {
    //            return false;
    //        }
    //    }
    //    else {
    //            return true;
    //    }


    //}
    //function isNumeric1(evt) {
    //    var keyCodeEntered = (event.which) ? event.which : (window.event.keyCode) ? window.event.keyCode : -1;
    //    if ((keyCodeEntered >= 48) && (keyCodeEntered <= 57)) {

    //        return true;
    //    }

    //    // '.' decimal point...  
    //    else if (keyCodeEntered === 46) {
    //        // Allow only 1 decimal point ('.')...  
    //        if ((evt.value) && (evt.value.indexOf('.') >= 0))
    //            return false;
    //        else
    //            return true;
    //    }
    //    return false;
    //}
    function isNumeric1(el, evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        var number = el.value.split('.');
        if (charCode !== 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        //just one dot
        if (number.length > 1 && charCode === 46) {
            return false;
        }
        //get the carat position
        var caratPos = getSelectionStart(el);
        var dotPos = el.value.indexOf(".");
        if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
            return false;
        }
        return true;
    }

    //thanks: http://javascript.nwbox.com/cursor_position/
    function getSelectionStart(o) {
        if (o.createTextRange) {
            var r = document.selection.createRange().duplicate();
            r.moveEnd('character', o.value.length);
            if (r.text === '') return o.value.length;
            return o.value.lastIndexOf(r.text);
        }
        else return o.selectionStart;
    }


</script>
