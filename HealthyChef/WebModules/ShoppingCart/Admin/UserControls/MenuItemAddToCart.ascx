<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuItemAddToCart.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.MenuItemAddToCart" %>

<div id="divProfiles" runat="server" visible="false">
    <asp:DropDownList ID="ddlProfiles" runat="server" />
    <asp:RequiredFieldValidator ID="rfvProfiles" runat="server" ControlToValidate="ddlProfiles"
        Text="*" ErrorMessage="A profile is required." SetFocusOnError="true" InitialValue="-1" />
    <br />
</div>
<asp:DropDownList ID="ddlDeliveryDates" runat="server" AutoPostBack="true" />
<asp:RequiredFieldValidator ID="rfvDeliveryDates" runat="server" ControlToValidate="ddlDeliveryDates"
    Text="*" ErrorMessage="A delivery date is required." SetFocusOnError="true" InitialValue="-1" />
<br />
<asp:DropDownList ID="ddlMenuItems" runat="server" AutoPostBack="true" Enabled="false" />
<asp:RequiredFieldValidator ID="rfvMenuItems" runat="server" ControlToValidate="ddlMenuItems"
    Text="*" ErrorMessage="A menu item is required." SetFocusOnError="true" InitialValue="-1" />
<br />
<asp:Panel ID="pnlMealSides" runat="server" Visible="false">
    <asp:DropDownList ID="ddlMealSide1MenuItems" runat="server" AutoPostBack="true"/>
    <br />
    <asp:DropDownList ID="ddlMealSide2MenuItems" runat="server" AutoPostBack="true"/>
</asp:Panel>
<asp:DropDownList ID="ddlOptions" runat="server" AutoPostBack="true" Enabled="false" />
<asp:RequiredFieldValidator ID="rfvOptions" runat="server" ControlToValidate="ddlOptions"
    Text="*" ErrorMessage="An option is required." SetFocusOnError="true" InitialValue="-1" />
<br />
<div id="divPreferences" runat="server" visible="false">
    <asp:CheckBoxList ID="cblPreferences" runat="server" />
    <br />
</div>
<asp:TextBox ID="txtQuantity" runat="server" onKeyPress="javascript:validateFamilyStyle(this);" Text="1" MaxLength="3" />
<asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="txtQuantity"
    Text="*" ErrorMessage="A quantity is required." SetFocusOnError="true" />
<asp:RangeValidator ID="cpvQuantity" runat="server" ControlToValidate="txtQuantity"
    Text="*" ErrorMessage="Quantity must be greater than zero." SetFocusOnError="true"
    Type="Integer" MinimumValue="1" MaximumValue="999" />
<br />
<asp:Label ID="lblFamilyStyle" runat="server" Text="Family Style"></asp:Label> <asp:CheckBox runat="server" onclick="javascript:onChkFamilyStyleChanged(this)" ID="chkFamilyStyle" ClientIDMode="Static" Text="" TextAlign="Left" Visible="True"/>
<asp:Button ID="btnSave" runat="server" OnClientClick="return validateQuantity();" Text="Add to Cart" />
<asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" />
<div id="divFamilyStyle" style="display:none">
<asp:Label ID="lblerror" CssClass="lblerror" runat="server" Text="Family Style requires 2 or more servings."></asp:Label>
<%--<asp:Label ID="lblincrease" CssClass="lblerror" runat="server" Text="Increase by 2?"></asp:Label>
<asp:Label ID="lblYes" runat="server" Text="Yes" CssClass="yesorno"></asp:Label>
<asp:Label ID="lblNo" runat="server" Text="No" CssClass="yesorno"></asp:Label>--%>
</div>
<br />
<asp:ValidationSummary ID="ValSum1" runat="server" />

<script>
    function validateQuantity() {
        var quantity = document.getElementById("ctl00_Body_UserProfileEdit1_ProfileCartEdit1_MenuItemAddToCart1_txtQuantity").value;
        var ischecked = document.getElementById("chkFamilyStyle");
        if (quantity <= 1 && ischecked.checked) {
            alert('Family Style will deliver 2 or more portions in one serving dish.This makes heating meals for the whole family more convenient, reduces packaging, and allows you to save 10% on the price.');
            return false;
        }
        else {
            return true;
        }
        
    }
    function onChkFamilyStyleChanged(obj)
    {
        var quantity = document.getElementById("ctl00_Body_UserProfileEdit1_ProfileCartEdit1_MenuItemAddToCart1_txtQuantity").value;
        if (obj.checked && quantity <=1)
        {
            $('#divFamilyStyle').css('display', 'block');
            document.getElementById("ctl00_Body_UserProfileEdit1_ProfileCartEdit1_MenuItemAddToCart1_txtQuantity").value = 2;
            $('#ctl00_Body_UserProfileEdit1_ProfileCartEdit1_MenuItemAddToCart1_txtQuantity').focus();
        }
        else if (!(obj.checked) && quantity==2)
        {
            document.getElementById("ctl00_Body_UserProfileEdit1_ProfileCartEdit1_MenuItemAddToCart1_txtQuantity").value = 1;
            $('#divFamilyStyle').css('display', 'none');
        }
    }

    $('#ctl00_Body_UserProfileEdit1_ProfileCartEdit1_MenuItemAddToCart1_lblYes').click(function () {
        document.getElementById("ctl00_Body_UserProfileEdit1_ProfileCartEdit1_MenuItemAddToCart1_txtQuantity").value = 2;
        //document.getElementById('cbxFamilyStyle').checked = true;
        $('#divFamilyStyle').css('display', 'none');
    });

    $('#ctl00_Body_UserProfileEdit1_ProfileCartEdit1_MenuItemAddToCart1_lblNo').click(function () {
        $('#divFamilyStyle').css('display', 'none');
        document.getElementById('cbxFamilyStyle').checked = false;
    });

    function validateFamilyStyle(obj) {
        var isFamilyStyleChecked = document.getElementById('cbxFamilyStyle').checked;
        if (obj.value <= 1 && isFamilyStyleChecked)
        {
            document.getElementById('cbxFamilyStyle').checked = false;
        }
    }
</script>

<style>
    .yesorno{
        color:red;
        cursor:pointer;
        text-decoration:underline;
    }
    .lblerror{
        color:red;
    }
</style>