<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DeliverySettings_Edit.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.DeliverySettings_Edit" %>
<style>
    .HiddenCol {
        display: none;
    }
</style>
<script type="text/javascript">
    $(function () {
        $("#divPlanTabs").tabs({
            cookie: {
                // store cookie for a day, without, it would be a session cookie
                expires: 1
            }
        });
    });

    function forRentClicked(sender) {

        if ($(sender).is(":checked")) {
            $("#ctl00_Body_DeliverySettingsEdit1_txtPickupFee").css({ 'display': 'block' });
        } else {
            $("#ctl00_Body_DeliverySettingsEdit1_txtPickupFee").css({ 'display': 'none' });
        }
    };

    function BoxSavevalidation() {

        var _txtBoxName = document.getElementById('ctl00_Body_DeliverySettingsEdit1_txtBoxName');
        var _txtDIM_W = document.getElementById('ctl00_Body_DeliverySettingsEdit1_txtDIM_W');
        var _txtDIM_L = document.getElementById('ctl00_Body_DeliverySettingsEdit1_txtDIM_L');
        var _txtDIM_H = document.getElementById('ctl00_Body_DeliverySettingsEdit1_txtDIM_H');
        var _txtMaxNoMeals = document.getElementById('ctl00_Body_DeliverySettingsEdit1_txtMaxNoMeals');

        if ($("#ctl00_Body_DeliverySettingsEdit1_txtBoxName").val() == "") {

            $('#ctl00_Body_DeliverySettingsEdit1_txtBoxName').css('border-color', '#FF0000');
            $("#ctl00_Body_DeliverySettingsEdit1_txtBoxName").focus();
        }
        else {
            $('#ctl00_Body_DeliverySettingsEdit1_txtBoxName').css('border-color', '#d5d5d5');
        }

        if ($("#ctl00_Body_DeliverySettingsEdit1_txtDIM_W").val() == "") {

            $('#ctl00_Body_DeliverySettingsEdit1_txtDIM_W').css('border-color', '#FF0000');
            $("#ctl00_Body_DeliverySettingsEdit1_txtDIM_W").focus();
        }
        else {
            $('#ctl00_Body_DeliverySettingsEdit1_txtDIM_W').css('border-color', '#d5d5d5');
        }
        if ($("#ctl00_Body_DeliverySettingsEdit1_txtDIM_L").val() == "") {

            $('#ctl00_Body_DeliverySettingsEdit1_txtDIM_L').css('border-color', '#FF0000');
            $("#ctl00_Body_DeliverySettingsEdit1_txtDIM_L").focus();
        }
        else {
            $('#ctl00_Body_DeliverySettingsEdit1_txtDIM_L').css('border-color', '#d5d5d5');
        }
        if ($("#ctl00_Body_DeliverySettingsEdit1_txtDIM_H").val() == "") {

            $('#ctl00_Body_DeliverySettingsEdit1_txtDIM_H').css('border-color', '#FF0000');
            $("#ctl00_Body_DeliverySettingsEdit1_txtDIM_H").focus();
        }
        else {
            $('#ctl00_Body_DeliverySettingsEdit1_txtDIM_H').css('border-color', '#d5d5d5');
        }

        if ($("#ctl00_Body_DeliverySettingsEdit1_txtMaxNoMeals").val() == "") {

            $('#ctl00_Body_DeliverySettingsEdit1_txtMaxNoMeals').css('border-color', '#FF0000');
            $("#ctl00_Body_DeliverySettingsEdit1_txtMaxNoMeals").focus();
        }
        else {
            $('#ctl00_Body_DeliverySettingsEdit1_txtMaxNoMeals').css('border-color', '#d5d5d5');
        }

        if (_txtBoxName.value == '' || _txtDIM_W.value == '' || _txtDIM_L.value == '' || _txtDIM_H.value == '' || _txtMaxNoMeals.value == '') {
            alert('please enter value');
            return false;
        }
        else {
            return confirm('Confirming: Do you want to update the database?')
        }
    };

    function shippingZoneValidation() {

        var _txtZoneName = document.getElementById('ctl00_Body_DeliverySettingsEdit1_txtZoneName');
        var _txtMultiplier = document.getElementById('ctl00_Body_DeliverySettingsEdit1_txtMultiplier');
        var _txtShippingDesc = document.getElementById('ctl00_Body_DeliverySettingsEdit1_txtShippingDesc');

        if ($("#ctl00_Body_DeliverySettingsEdit1_txtZoneName").val() == "") {

            $('#ctl00_Body_DeliverySettingsEdit1_txtZoneName').css('border-color', '#FF0000');
            $("#ctl00_Body_DeliverySettingsEdit1_txtZoneName").focus();
        }
        else {
            $('#ctl00_Body_DeliverySettingsEdit1_txtZoneName').css('border-color', '#d5d5d5');
        }

        if ($("#ctl00_Body_DeliverySettingsEdit1_txtMultiplier").val() == "") {

            $('#ctl00_Body_DeliverySettingsEdit1_txtMultiplier').css('border-color', '#FF0000');
            $("#ctl00_Body_DeliverySettingsEdit1_txtMultiplier").focus();
        }
        else {
            $('#ctl00_Body_DeliverySettingsEdit1_txtMultiplier').css('border-color', '#d5d5d5');
        }

        if ($("#ctl00_Body_DeliverySettingsEdit1_txtShippingDesc").val() == "") {

            $('#ctl00_Body_DeliverySettingsEdit1_txtShippingDesc').css('border-color', '#FF0000');
            $("#ctl00_Body_DeliverySettingsEdit1_txtShippingDesc").focus();
        }
        else {
            $('#ctl00_Body_DeliverySettingsEdit1_txtShippingDesc').css('border-color', '#d5d5d5');
        }

        if (_txtZoneName.value == '' || _txtMultiplier.value == '' || _txtShippingDesc.value == '') {
            alert('please enter value');
            return false;
        }
        else {
            return confirm('Confirming: Do you want to update the database?')
        }
    };

    function LookUpZipCodeValidation() {

        var _txtZipCode = document.getElementById('ctl00_Body_DeliverySettingsEdit1_txtZipCode');

        if ($("#ctl00_Body_DeliverySettingsEdit1_txtZipCode").val() == "") {

            $('#ctl00_Body_DeliverySettingsEdit1_txtZipCode').css('border-color', '#FF0000');
            $("#ctl00_Body_DeliverySettingsEdit1_txtZipCode").focus();
        }
        else {
            $('#ctl00_Body_DeliverySettingsEdit1_txtZipCode').css('border-color', '#d5d5d5');
        }
        if (_txtZipCode.value == '') {
            alert('please enter value');
            return false;
        }
        else {
            return confirm('Confirming: Do you want to Lookup the database?')
        }
    };

    function SaveZipCodeValidation() {

        var _txtZipCode = document.getElementById('ctl00_Body_DeliverySettingsEdit1_txtZipCode');

        if ($("#ctl00_Body_DeliverySettingsEdit1_txtZipCode").val() == "") {

            $('#ctl00_Body_DeliverySettingsEdit1_txtZipCode').css('border-color', '#FF0000');
            $("#ctl00_Body_DeliverySettingsEdit1_txtZipCode").focus();
        }
        else {
            $('#ctl00_Body_DeliverySettingsEdit1_txtZipCode').css('border-color', '#d5d5d5');
        }
        if (_txtZipCode.value == '') {
            alert('please enter value');
            return false;
        }
        else {
            return confirm('Confirming: Do you want to update the database?')
        }
    };

    function GetPrice(evt) {

        var Cost = $(evt).val();
        var Multiplier = parseFloat($('#ctl00_Body_DeliverySettingsEdit1_lblShipMultiplier').text());
        var Price = parseFloat(Cost) * parseFloat(Multiplier);
        var FindlableId = $(evt).parent().parent().find('.clsPrice')[0];
        //var FindlableId1 =$(FindlableId).attr('id');
        $(FindlableId).text(Price);
    }

    function ValidateZipCode(event) {

        var ZipCode = $('#ctl00_Body_DeliverySettingsEdit1_txtShippingZipCode').val();
        if (ZipCode == "") {
            alert("Please Enter Value")
            return false;
        }
        else {
            if (!ZipCode.match(/^\d+/)) {
                alert("Please only enter numeric value only (Allowed input:0-9)")
                return false;
            }
        }
    }

</script>
<div class="page-header">
    <h1>Shipping Fees</h1>
</div>
<asp:Label ID="lblFeedback" runat="server" EnableViewState="false" ForeColor="Green" />
<asp:Panel ID="pnlPlanLists" runat="server" ng-app="UserApp" ng-controller="GetMessageBox" ng-init="GetAllShippingZone();">
    <div class="col-sm-12">
        <div class="fieldRow">
            <asp:Button ID="btnSave" CssClass="btn btn-info" runat="server" Text="Save" />
            <div>
                <asp:ValidationSummary ID="ValSumCoupon1" runat="server" />
            </div>
        </div>
    </div>
    <div class="clearfix"></div>
    <div id="divPlanTabs" style="margin: 10px;">
        <ul>
            <li><a href="#shipping">Zip Codes</a></li>
            <li><a href="#shippingZones">Shipping Zones</a></li>
            <li><a href="#zipCodes">Map Zip Codes</a></li>
            <li><a href="#boxes">Manage Box Sizes</a></li>
            <li style="display: none"><a href="#shippingClass">Shipping Class</a></li>
        </ul>

        <div id="shipping">
            <asp:UpdatePanel ID="Upanelshipping" runat="server">
                <ContentTemplate>
                    <table class="aling_table1" style="float: left; width: 60%; margin-top: 15px;">
                        <tr>
                            <td>
                                <label class="col-sm-12">Zip Code :</label></td>
                            <td>
                                <asp:TextBox ID="txtShippingZipCode" runat="server" />
                                <asp:Button ID="btnShippingSearch" runat="server" CssClass="btn btn-info" Text="Search" OnClick="btnShippingSearch_Click" OnClientClick="return ValidateZipCode(this);" />
                                <asp:Label ID="lblZipCodeErrorMsg" runat="server" ForeColor="Red"></asp:Label>
                                <asp:DropDownList runat="server" ID="ddlZipCode" AutoPostBack="true" CausesValidation="false" OnSelectedIndexChanged="ddlZipCode_SelectedIndexChanged" Style="display: none;"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="col-sm-12">Is Pickup :</label></td>
                            <td>
                                <asp:CheckBox ID="chkIsPickup" CssClass="pull-left checkbox-straight" runat="server" onclick="forRentClicked(this)" />

                                <p class="pull-left checkbox-straight">Pickup Fee </p>
                                <asp:TextBox ID="txtPickupFee" runat="server" CssClass="pull-left checkbox-straight"></asp:TextBox>
                                <asp:Button ID="btnSave1" runat="server" CssClass="btn btn-info checkbox-straight" Text="Save" OnClick="btnSave1_Click" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label class="col-sm-12">Zone Name :</label></td>
                            <td>
                                <asp:Label ID="lblShippingZone" runat="server" Style="display: none;"></asp:Label>
                                <asp:DropDownList runat="server" ID="ddlShippingZone" AutoPostBack="true" CausesValidation="false" OnSelectedIndexChanged="ddlShipingZone_SelectedIndexChanged"></asp:DropDownList>
                                <asp:Button ID="btnUpdate" Text="Update" CssClass="btn btn-info" runat="server" OnClick="btnUpdate_Click" />&nbsp;&nbsp;<asp:Label ID="lblUpdateSuccess" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <div class="alert alert-success" style="float: right; width: 40%;">
                        <strong>Use this area</strong>
                        <p>* To search for Zip Code information</p>
                        <p>* To enable pickups from any Zip Code</p>
                        <p>* To set the fee for Pickups</p>
                    </div>
                    <table>
                        <tr>
                            <td>
                                <label class="col-sm-12">Min Fee Per Shipment</label></td>
                            <td>:</td>
                            <td>
                                <asp:Label ID="lblMinShippingFeeOrder" runat="server"></asp:Label>
                                <asp:TextBox ID="txtMinShippingFeeOrder" runat="server" ReadOnly="true" Style="display: none;" />
                                <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="txtMinShippingFeeOrder"
                                    Text="*" Display="Dynamic" ErrorMessage="Global Min. Shipping Cost must be numeric."
                                    SetFocusOnError="true" Operator="DataTypeCheck" Type="Double" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="col-sm-12">Max Fee Per Shipment</label></td>
                            <td>:</td>
                            <td>
                                <asp:Label ID="lblMaxShippingFeeOrder" runat="server"></asp:Label>
                                <asp:TextBox ID="txtMaxShippingFeeOrder" runat="server" ReadOnly="true" Style="display: none;" />
                                <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="txtMaxShippingFeeOrder"
                                    Text="*" Display="Dynamic" ErrorMessage="Global Max. Shipping Cost must be numeric."
                                    SetFocusOnError="true" Operator="DataTypeCheck" Type="Double" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="col-sm-12">Multiplier</label></td>
                            <td>:</td>
                            <td>
                                <asp:Label ID="lblShipMultiplier" runat="server"></asp:Label></td>
                        </tr>

                    </table>
                    <asp:Label ID="lblUpdateMessage" runat="server" Style="color: red;"></asp:Label>
                    <br />

                    <asp:GridView ID="shippingGrd" CssClass="table table-bordered table-hover" runat="server" AutoGenerateColumns="false" EmptyDataText="No records has been added."
                        DataKeyNames="ID"
                        OnRowCancelingEdit="OnBoxZoneFee_RowCancelingEdit" OnRowEditing="OnBoxZoneFee_RowEditing" OnRowDeleting="shippingGrd_RowDeleting"
                        OnRowUpdating="OnBoxZoneFee_RowUpdating">
                        <Columns>
                            <asp:TemplateField HeaderText="BoxID" ItemStyle-Width="100" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblBoxID" runat="server" Text='<%# Eval("BoxID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="BoxName" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblBoxName" runat="server" Text='<%# Eval("BoxName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DIM_W" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblDIM_W" runat="server" Text='<%# Eval("DIM_W") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DIM_L" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblDIM_L" runat="server" Text='<%# Eval("DIM_L") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DIM_H" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblDIM_H" runat="server" Text='<%# Eval("DIM_H") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID" ItemStyle-Width="100" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="MaxNoMeals" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblMaxNoMeals" runat="server" Text='<%# Eval("MaxNoMeals") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cost" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblCost" runat="server" Text='<%# Eval("Cost") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtCost" class="clsCost" onblur="return GetPrice(this);" runat="server" Text='<%# Eval("Cost") %>'></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvCost" runat="server" ControlToValidate="txtCost"
                                        ErrorMessage="Please Enter Cost" ValidationGroup="EditValidate"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revCost" runat="server" ControlToValidate="txtCost"
                                        ErrorMessage="Please Enter Numaric & Decimal Upto two" ValidationExpression="((\d+)((\.\d{1,2})?))$"
                                        ValidationGroup="EditValidate"></asp:RegularExpressionValidator>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fee" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblPrice" class="clsPrice" runat="server" Text='<%# Eval("Price") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtPrice" runat="server" Text='<%# Eval("Price") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Pickup Fee" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblpickupFee" runat="server" Text='<%# Eval("PickupFee") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtPickupFee" runat="server" Text='<%# Eval("PickupFee") %>'></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvPickupFee" runat="server" ControlToValidate="txtPickupFee"
                                        ErrorMessage="Please Enter Pickup Fee" ValidationGroup="EditValidate"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ButtonType="Link" CausesValidation="True" ValidationGroup="EditValidate" ShowEditButton="false" ShowDeleteButton="false" ItemStyle-Width="150" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div id="shippingZones">
            <asp:UpdatePanel ID="UpanelshippingZones" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="shippingZonesGrid" CssClass="table table-bordered table-hover" runat="server" AutoGenerateColumns="false" EmptyDataText="No records has been added."
                        DataKeyNames="ZoneID"
                        OnRowDataBound="OnRowDataBound" OnRowEditing="OnRowEditing" OnRowCancelingEdit="OnRowCancelingEdit"
                        OnRowUpdating="OnRowUpdating" OnRowDeleting="OnRowDeleting">
                        <Columns>
                            <asp:TemplateField HeaderText="ZoneName" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblZoneName" runat="server" Text='<%# Eval("ZoneName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtZoneName" runat="server" Text='<%# Eval("ZoneName") %>'></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvZoneName" runat="server" ControlToValidate="txtZoneName"
                                        ErrorMessage="Please Enter Shipping Zone Name" ValidationGroup="ShippingZoneEditValidate"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Multiplier" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblMultiplier" runat="server" Text='<%# Eval("Multiplier") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtMultiplier" runat="server" Text='<%# Eval("Multiplier") %>'></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvMultiplier" runat="server" ControlToValidate="txtMultiplier"
                                        ErrorMessage="Please Enter Multiplier" ValidationGroup="ShippingZoneEditValidate"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revMultiplier" runat="server" ControlToValidate="txtMultiplier"
                                        ErrorMessage="Please add a 0 for decimals, like “0.50”" ValidationExpression="((\d+)((\.\d{1,2})?))$"
                                        ValidationGroup="ShippingZoneEditValidate"></asp:RegularExpressionValidator>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="MinFee" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblMinFee" runat="server" Text='<%# Eval("MinFee") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtMinFee" runat="server" Text='<%# Eval("MinFee") %>'></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="revMinFee" runat="server" ControlToValidate="txtMinFee"
                                        ErrorMessage="Please Enter Numaric & Decimal Upto two" ValidationExpression="((\d+)((\.\d{1,2})?))$"
                                        ValidationGroup="ShippingZoneEditValidate"></asp:RegularExpressionValidator>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="MaxFee" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblMaxFee" runat="server" Text='<%# Eval("MaxFee") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtMaxFee" runat="server" Text='<%# Eval("MaxFee") %>'></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="revMaxFee" runat="server" ControlToValidate="txtMaxFee"
                                        ErrorMessage="Please Enter Numaric & Decimal Upto two" ValidationExpression="((\d+)((\.\d{1,2})?))$"
                                        ValidationGroup="ShippingZoneEditValidate"></asp:RegularExpressionValidator>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Default" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblDefaultShippingZone" runat="server" Text='<%# Eval("IsDefaultShippingZone") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkDefaultShippingZone" runat="server" Checked='<%# Eval("IsDefaultShippingZone").ToString() == "True" ? true :false %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="OrderMinimum" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderMinimum" runat="server" Text='<%# Eval("OrderMinimum") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtOrderMinimum" runat="server" Text='<%# Eval("OrderMinimum") %>' MaxLength="4"></asp:TextBox>
                                  <%--  <asp:RegularExpressionValidator ID="revOrderMinimum" runat="server"
                                        ControlToValidate="txtOrderMinimum" ErrorMessage="Please Enter Only Numbers & upto Maxvalue 9999"
                                        ValidationExpression="^\d+$" ValidationGroup="ShippingZoneEditValidate"></asp:RegularExpressionValidator>--%>

                                    <asp:RangeValidator runat="server" Type="Integer"
                                        MinimumValue="1" MaximumValue="9999" ControlToValidate="txtOrderMinimum"
                                        ErrorMessage="Value must be a whole number between 1 and 9999" ValidationExpression="^\d+$" ValidationGroup="ShippingZoneEditValidate" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PickupShippingZone" ItemStyle-Width="150">
                                <HeaderStyle CssClass="HiddenCol" />
                                <ItemTemplate>
                                    <asp:Label ID="lblPickupShippingZone" runat="server" Text='<%# Eval("IsPickupShippingZone") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" CssClass="HiddenCol" />
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkPickupShippingZone" runat="server" Checked='<%# Eval("IsPickupShippingZone").ToString() == "True" ? true :false %>' />
                                </EditItemTemplate>
                                <FooterStyle CssClass="HiddenCol" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Shipping Description" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblShippingDesc" runat="server" Text='<%# Eval("TypeName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtShippingDesc" runat="server" Text='<%# Eval("TypeName") %>'></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvShippingDesc" runat="server" ControlToValidate="txtShippingDesc"
                                        ErrorMessage="Please Enter Shipping Description" ValidationGroup="ShippingZoneEditValidate"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ButtonType="Link" CausesValidation="True" ValidationGroup="ShippingZoneEditValidate" ShowEditButton="true" ShowDeleteButton="true" ItemStyle-Width="150" />
                        </Columns>
                    </asp:GridView>
                    <%--<div>
                     <table ng-show="ShippingZones.length!=0" class="table table-bordered table-hover">
                        <thead>
                            <tr>
                                <th scope="col">ZoneName</th>
                                <th scope="col">Multiplier</th>
                                <th scope="col">MinFee</th>
                                <th scope="col">MaxFee</th>
                                <th scope="col">Default</th>
                                <th scope="col">Shipping Description</th>
                                <th scope="col">&nbsp;</th>
                                <th scope="col">&nbsp;</th>
                            </tr>
                        </thead>

                        <tbody>
                            <tr ng-repeat="u in ShippingZones">
                                <td><span>{{ u.ZoneName }}</span></td>
                                <td>{{ u.Multiplier }}</td>
                                <td>{{ u.MinFee }}</td>
                                <td>{{ u.MaxFee }}</td>
                                <td>{{ u.IsDefaultShippingZone }}</td>
                                <td>{{ u.TypeName }}</td>
                                <td><a ng-click="GetDetails($index)">Edit</a></td>
                                <td><a ng-click="GetDetails($index)">Delete</a></td>
                            </tr>
                        </tbody>
                        <div ng-show="ShippingZones.length==0" style="color: red; text-align:center;font-weight: bold">
                            <p>No Records Found</p>
                        </div>
                    </table>
                        </div>--%>
                    <div class="col-sm-10">
                        <br />
                        <p>
                            <asp:Label ID="lblShippingError" runat="server" Style="color: red;"></asp:Label>
                            <asp:Label ID="lblShippingSuccess" runat="server" Style="color: blue;"></asp:Label>
                        </p>
                        <table class="aling_table table_format">
                            <tr>
                                <td>
                                    <label>Shipping Zone</label></td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="txtZoneName" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Shipping Description</label></td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="txtShippingDesc" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Multiplier</label></td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="txtMultiplier" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>MinFee</label></td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="txtMinFee" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>MaxFee</label></td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="txtMaxFee" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Is Default</label></td>
                                <td>:</td>
                                <td>
                                    <asp:CheckBox ID="chkIsDefaultShippingZone" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>OrderMinium</label></td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="txtOrderMinium" runat="server" MaxLength="4" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkIsPickupShippingZone" runat="server" Style="display: none;" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnSaveShippingZone" runat="server" OnClick="btnSaveShippingZone_Click" OnClientClick="return shippingZoneValidation();" Text="Add Shipping Zone" Visible="false" />
                                    <button type="button" ng-click="AddOrUpdateShippingZone();" class="btn btn-info">Add Shipping Zone</button>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblShipingMessage" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>

                        <table class="alert alert-success pull-right">
                            <tr>
                                <td>Use this area</td>
                            </tr>
                            <tr>
                                <td>* To edit Shipping zone rules</td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div id="zipCodes" style="margin-top: 15px">
            <asp:UpdatePanel ID="UpanelzipCodes" runat="server">
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnNewCSVUpload" />
                </Triggers>
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>
                                <table style="display: none">
                                    <tr>
                                        <td>Shipping Class</td>
                                        <td>:</td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="drpZoneID"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Zip Code</td>
                                        <td>:</td>
                                        <td>
                                            <asp:TextBox ID="txtZipCode" runat="server" />
                                            <asp:Button ID="btnSaveZipCode" runat="server" Text="Add Zipcode" OnClick="btnSaveZipCode_Click" OnClientClick="return SaveZipCodeValidation();" />
                                            <asp:Label ID="lblSaveZipCodeError" runat="server" Style="color: red;"></asp:Label>
                                            <asp:Button ID="btnLookupZipCode" runat="server" Text="Lookup ZipCode" OnClick="btnLookupZipCode_Click" OnClientClick="return LookUpZipCodeValidation();" Style="display: none;" />
                                            <asp:Label ID="lblLookupError" runat="server" Style="color: red;"></asp:Label>
                                            <asp:Label ID="lblLookupShow" runat="server" Style="color: blue; display: none;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnShowZipCodes" runat="server" Text="Show zip Codes" OnClick="btnShowZipCodes_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="m-2" style="width: 71%;">
                                <table class="aling_table">
                                    <tr>
                                        <td>
                                            <asp:FileUpload ID="FileUpload1" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button Text="CSV Upload" runat="server" ID="btnCSVUpload" OnClick="btnCSVUpload_Click" OnClientClick="return CsvUploadfun();" Style="display: none;" />
                                            <asp:Button Text="Csv Upload" runat="server" ID="btnNewCSVUpload" OnClick="btnNewCSVUpload_Click" />
                                            <asp:Label ID="lblCsvErrorMsg" runat="server" Style="color: red;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px">
                                            <asp:LinkButton ID="lnkbtnCsvDownload" runat="server" OnClick="lnkbtnCsvDownload_Click">Download_Csv Template</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table class="alert alert-success" style="float: right; width: 100%; padding: 20px; display: block">
                                    <tr>
                                        <td>Use this area</td>
                                    </tr>
                                    <tr>
                                        <td>* To upload Shipping zone data</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>

                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:WebModules %>"
                        SelectCommand="select ZoneID from hccShippingZone"></asp:SqlDataSource>
                    <asp:GridView ID="grdZipCode" runat="server" AutoGenerateColumns="false" EmptyDataText="No records has been added."
                        DataKeyNames="ZipZoneID" OnRowCancelingEdit="grdZipCode_RowCancelingEdit" OnRowDeleting="grdZipCode_RowDeleting" OnRowEditing="grdZipCode_RowEditing" OnRowUpdating="grdZipCode_RowUpdating">
                        <Columns>
                            <asp:TemplateField HeaderText="ZoneID" ItemStyle-Width="150" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblZipCodeZoneID" runat="server" Text='<%# Eval("ZoneID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ZoneName" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblZipZoneName" runat="server" Text='<%# Eval("ZoneName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ZipCode" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblZipCode" runat="server" Text='<%# Eval("ZipCode") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtZipCode" runat="server" Text='<%# Eval("ZipCode") %>'></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvZipCode" runat="server" ControlToValidate="txtZipCode"
                                        ErrorMessage="Please Enter Zip Code " ValidationGroup="ZipCodeEditValidate"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator runat="server" Operator="DataTypeCheck" Type="Integer"
                                        ControlToValidate="txtZipCode" ValidationGroup="ZipCodeEditValidate" ErrorMessage="Value must be a whole number"></asp:CompareValidator>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ZoneID" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblZoneID" runat="server" Text='<%# Eval("ZoneID") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtZoneID" runat="server" Text='<%# Eval("ZoneID") %>'></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvZoneID" runat="server" ControlToValidate="txtZoneID"
                                        ErrorMessage="Please Enter Zone ID" ValidationGroup="ZipCodeEditValidate"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator runat="server" Operator="DataTypeCheck" Type="Integer"
                                        ControlToValidate="txtZoneID" ValidationGroup="ZipCodeEditValidate" ErrorMessage="Value must be a whole number"></asp:CompareValidator>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Shipping Class" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblShimentTypeId" runat="server" Text='<%# Eval("ShippingClass") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ButtonType="Link" CausesValidation="True" ValidationGroup="ZipCodeEditValidate" ShowEditButton="true" ShowDeleteButton="true" ItemStyle-Width="150" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div id="boxes" style="margin-top: 15px">
            <asp:UpdatePanel ID="Upanelboxes" runat="server">
                <ContentTemplate>
                    <table class="aling_table">
                        <tr>
                            <td>Box Name</td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="txtBoxName" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>DIM_W</td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="txtDIM_W" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>DIM_L</td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="txtDIM_L" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>DIM_H</td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="txtDIM_H" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>Max No Meals</td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="txtMaxNoMeals" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnBoxSave" runat="server" CssClass="btn btn-info" Text="Save Box Size" OnClick="btnBoxSave_Click" OnClientClick="return BoxSavevalidation();" />
                                <asp:Label ID="lblErrorRecord" runat="server" Style="color: red;"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table class="alert alert-success" style="float: right; width: 40%; padding: 20px; display: block">
                        <tr>
                            <td>Use this area</td>
                        </tr>
                        <tr>
                            <td>* To Set Box Sizes data</td>
                        </tr>
                        <tr>
                            <td>* To Edit Box Sizes rules</td>
                        </tr>
                    </table>
                    <br />
                    <asp:GridView ID="grdBoxSizes" runat="server" CssClass="table table-bordered table-hover" AutoGenerateColumns="false" EmptyDataText="No records has been added."
                        DataKeyNames="BoxID" OnRowCancelingEdit="OnBoxSizes_RowCancelingEdit" OnRowDataBound="OnBoxSizes_RowDataBound" OnRowDeleting="OnBoxSizes_RowDeleting" OnRowEditing="OnBoxSizes_RowEditing" OnRowUpdating="OnBoxSizes_RowUpdating">
                        <Columns>
                            <asp:TemplateField HeaderText="BoxName" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblBoxName" runat="server" Text='<%# Eval("BoxName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtBoxName" runat="server" Text='<%# Eval("BoxName") %>'></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvBoxName" runat="server" ControlToValidate="txtBoxName"
                                        ErrorMessage="Please Enter Box Name" ValidationGroup="BoxSizeEditValidate"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DIM_W" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblDIM_W" runat="server" Text='<%# Eval("DIM_W") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDIM_W" runat="server" Text='<%# Eval("DIM_W") %>'></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvDIM_W" runat="server" ControlToValidate="txtDIM_W"
                                        ErrorMessage="Please Enter DIM_W" ValidationGroup="BoxSizeEditValidate"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revDIM_W" runat="server" ControlToValidate="txtDIM_W"
                                        ErrorMessage="Please Enter Numaric & Decimal Upto two" ValidationExpression="((\d+)((\.\d{1,2})?))$"
                                        ValidationGroup="BoxSizeEditValidate"></asp:RegularExpressionValidator>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DIM_L" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblDIM_L" runat="server" Text='<%# Eval("DIM_L") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDIM_L" runat="server" Text='<%# Eval("DIM_L") %>'></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvDIM_L" runat="server" ControlToValidate="txtDIM_L"
                                        ErrorMessage="Please Enter DIM_L" ValidationGroup="BoxSizeEditValidate"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revDIM_L" runat="server" ControlToValidate="txtDIM_L"
                                        ErrorMessage="Please Enter Numaric & Decimal Upto two" ValidationExpression="((\d+)((\.\d{1,2})?))$"
                                        ValidationGroup="BoxSizeEditValidate"></asp:RegularExpressionValidator>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DIM_H" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblDIM_H" runat="server" Text='<%# Eval("DIM_H") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDIM_H" runat="server" Text='<%# Eval("DIM_H") %>'></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvDIM_H" runat="server" ControlToValidate="txtDIM_H"
                                        ErrorMessage="Please Enter DIM_H" ValidationGroup="BoxSizeEditValidate"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revDIM_H" runat="server" ControlToValidate="txtDIM_H"
                                        ErrorMessage="Please Enter Numaric & Decimal Upto two" ValidationExpression="((\d+)((\.\d{1,2})?))$"
                                        ValidationGroup="BoxSizeEditValidate"></asp:RegularExpressionValidator>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="MaxNoMeals" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblMaxNoMeals" runat="server" Text='<%# Eval("MaxNoMeals") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtMaxNoMeals" runat="server" Text='<%# Eval("MaxNoMeals") %>'></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvMaxNoMeals" runat="server" ControlToValidate="txtMaxNoMeals"
                                        ErrorMessage="Please Enter MaxNoMeals" ValidationGroup="BoxSizeEditValidate"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator runat="server" Operator="DataTypeCheck" Type="Integer"
                                        ControlToValidate="txtMaxNoMeals" ValidationGroup="BoxSizeEditValidate" ErrorMessage="Value must be a whole number">
                                    </asp:CompareValidator>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:CommandField ButtonType="Link" CausesValidation="True" ValidationGroup="BoxSizeEditValidate" ShowEditButton="true" ShowDeleteButton="true" ItemStyle-Width="150" />
                        </Columns>
                    </asp:GridView>
                    <%-- <div>
                    <table ng-show="MessageBoxSizes.length!=0" class="table table-bordered table-hover">
                        <thead>
                            <tr align="left">
                                <th scope="col">BoxName</th>
                                <th scope="col">DIM_W</th>
                                <th scope="col">DIM_L</th>
                                <th scope="col">DIM_H</th>
                                <th scope="col">MaxNoMeals</th>
                                <th scope="col">&nbsp;</th>
                                <th scope="col">&nbsp;</th>
                            </tr>
                        </thead>

                        <tbody>
                            <tr style="background-color: #FEFEFE;" ng-repeat="u in MessageBoxSizes">
                                <td><span>{{ u.BoxName }}</span></td>
                                <td>{{ u.DIM_W }}</td>
                                <td>{{ u.DIM_L }}</td>
                                <td>{{ u.DIM_H }}</td>
                                <td>{{ u.MaxNoMeals }}</td>
                                <td align="left"><a ng-click="GetDetails($index)">Edit</a></td>
                                <td align="left"><a ng-click="GetDetails($index)">Delete</a></td>
                            </tr>
                        </tbody>
                    </table>
                        </div>
                    <div ng-show="MessageBoxSizes.length==0" style="color: red; text-align:center;font-weight: bold">
                        <p>No Records Found</p>
                    </div>--%>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div id="shippingClass">
            <asp:UpdatePanel ID="UpdatePanelShippingClass" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="grdAddShippingClass" runat="server" AutoGenerateColumns="false" EmptyDataText="No records has been added."
                        DataKeyNames="ID" OnRowCancelingEdit="grdAddShippingClass_RowCancelingEdit" OnRowDataBound="grdAddShippingClass_RowDataBound" OnRowEditing="grdAddShippingClass_RowEditing" OnRowDeleting="grdAddShippingClass_RowDeleting" OnRowUpdating="grdAddShippingClass_RowUpdating">
                        <Columns>
                            <asp:TemplateField HeaderText="ID" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtID" runat="server" Text='<%# Eval("Id") %>'></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvID" runat="server" ControlToValidate="txtID"
                                        ErrorMessage="Please Enter ID" ValidationGroup="BoxSizeEditValidate"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("TypeName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDescription" runat="server" Text='<%# Eval("TypeName") %>'></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription"
                                        ErrorMessage="Please Enter Description" ValidationGroup="BoxSizeEditValidate"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ButtonType="Link" CausesValidation="True" ValidationGroup="BoxSizeEditValidate" ShowEditButton="true" ShowDeleteButton="true" ItemStyle-Width="150" />
                        </Columns>
                    </asp:GridView>
                    <br />
                    <table>
                        <tr>
                            <td>ID</td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="txtID" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>Description</td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="txtDescription" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnAddShippingClass" runat="server" Text="Add Shipping Class" OnClick="btnAddShippingClass_Click" />
                                <asp:Label ID="lblAddshippingError" runat="server" Style="color: red;"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <script type="text/javascript">
        var app = angular.module('UserApp', ["ui.bootstrap"]);

        //Getting data from API
        app.controller('GetMessageBox', function ($scope, $http) {

            $scope.GetAllShippingZone = function () {
                $http({
                    method: "GET",
                    url: GetApiBaseURL() + "GetAllShippingZone"
                }).then(function mySuccess(response) {
                    $scope.ShippingZones = response.data;
                    console.log(response.data.length);
                }, function myError(response) {
                    $scope.ErrorMessage = response.statusText;
                });
            }

            $http({
                method: "GET",
                url: GetApiBaseURL() + "GetAllMessageboxSizes"
            }).then(function mySuccess(response) {
                $scope.MessageBoxSizes = response.data;
                console.log(response.data.length);
            }, function myError(response) {
                $scope.ErrorMessage = response.statusText;
            });

            $scope.GetDetails = function (index) {

            };

            $scope.AddOrUpdateShippingZone = function () {
                debugger;
                if (shippingZoneValidation()) {
                    var ZoneName = $('#ctl00_Body_DeliverySettingsEdit1_txtZoneName').val();
                    var ShippingDesc = $('#ctl00_Body_DeliverySettingsEdit1_txtShippingDesc').val();
                    var Multiplier = $('#ctl00_Body_DeliverySettingsEdit1_txtMultiplier').val();
                    var MinFee = $('#ctl00_Body_DeliverySettingsEdit1_txtMinFee').val();
                    var MaxFee = $('#ctl00_Body_DeliverySettingsEdit1_txtMaxFee').val();
                    var isDefault = $('#ctl00_Body_DeliverySettingsEdit1_chkIsDefaultShippingZone').prop('checked');
                    var OrderMinimum = $('#ctl00_Body_DeliverySettingsEdit1_txtOrderMinium').val();
                    var ShippingZonedata = { ZoneName: ZoneName, Multiplier: Multiplier, MinFee: MinFee, MaxFee: MaxFee, IsDefaultShippingZone: isDefault, Description: ShippingDesc, OrderMinimum: OrderMinimum };

                    $http({
                        method: "POST",
                        url: GetApiBaseURL() + "AddOrUpdateShippingZone",
                        data: ShippingZonedata
                    }).then(function mySuccess(response) {

                        if (response.data.IsSuccess) {
                            debugger;
                            alert(response.data.Message);

                            $('#ctl00_Body_DeliverySettingsEdit1_txtZoneName').val('');
                            $('#ctl00_Body_DeliverySettingsEdit1_txtShippingDesc').val('');
                            $('#ctl00_Body_DeliverySettingsEdit1_txtMultiplier').val('');
                            $('#ctl00_Body_DeliverySettingsEdit1_txtMinFee').val('');
                            $('#ctl00_Body_DeliverySettingsEdit1_txtMaxFee').val('');
                            $('#ctl00_Body_DeliverySettingsEdit1_txtOrderMinium').val('');
                            $('#ctl00_Body_DeliverySettingsEdit1_chkIsDefaultShippingZone').prop('checked', false);

                            $scope.GetAllShippingZone();
                        }
                        else {
                            alert(response.data.Message);
                        }
                    }, function myError(response) {
                        alert(response.data.Message);
                    });
                }
                else {
                    return false;
                }

            }

        });
    </script>
</asp:Panel>

<%# Eval("MealTypeName") %>
<script>
    $(document).ready(function () {
        $(document).on('keypress', '#ctl00_Body_DeliverySettingsEdit1_txtOrderMinium', function (event) {

            $(this).val($(this).val().replace(/[^\d].+/, ""));
            if ((event.which < 48 || event.which > 57)) {
                event.preventDefault();
            }
        })
        $(document).on('change', '#ctl00_Body_DeliverySettingsEdit1_shippingZonesGrid tr td input', function () {
            var td_index = $(this).closest('td').index();
            var tr_value = $(this).closest('tr');
            if (td_index == 2) {
                var txt_2 = $(this).val();
                var txt_3 = $(tr_value).find('td').eq(3).find('input').val();
                if (txt_2 >= txt_3) {
                    $(this).next().html('MinFee should be less than MaxFee').css("visibility", 'visible');
                    $(tr_value).find('td:last').children().eq(0).off('click');

                }
            }
            if (td_index == 3) {
                var txt_3 = $(this).val();
                var txt_2 = $(tr_value).find('td').eq(2).find('input').val();
                if (txt_2 >= txt_3) {
                    $(this).next().html('MaxFee should be greater than MinFee').css("visibility", 'visible');
                    $(tr_value).find('td:last').children().eq(0).off('click');
                }
            }
        });
    });
</script>

