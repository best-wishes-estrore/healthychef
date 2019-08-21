<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserSubProfile_Edit.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.UserSubProfile_Edit" %>

<%@ Register TagPrefix="hcc" TagName="AddressEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/Address_Edit.ascx" %>
<%@ Register TagPrefix="hcc" TagName="ProfilePrefsEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/UserProfilePrefs_Edit.ascx" %>
<%@ Register TagPrefix="hcc" TagName="ProfileAllgsEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/UserProfileAllg_Edit.ascx" %>
<%@ Register TagPrefix="hcc" TagName="ProfileNotesEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/UserProfileNote_Edit.ascx" %>

<script type="text/javascript">
    $(document).ready(function () {
        if ($("#hdnShowSubTabs").val() == 'true') {
            var idx = 0;
            var lastSubTab = $("#hdnSubLastTab").val();

            if (lastSubTab.length > 0) {
                var t = $('#divSubTabs li a.profSub:contains(' + lastSubTab + ')');
                idx = $('#divSubTabs li a.profSub').index($('#divSubTabs li a.profSub:contains(' + lastSubTab + ')'));
                //$('#divSubTabs').tabs('option', "active", idx);
            }

            $("#divSubTabs").tabs({
                active: idx,
                cookie: {
                    // store cookie for a day, without, it would be a session cookie
                    expires: 1
                }
            });
        }
        else {
            $("#divSubTabs").hide();
        }

        //$('#divSubTabs li a.profSub:first').addClass('active-meal');

        ////Set references
        //$('#divSubTabs li a.profSub').each(function (i, e) {
        //    $(e).click(function (event) {
        //        event.preventDefault();
        //        $("#hdnSubLastTab").val($(this).text());
        //        //Clear active menu item
        //        $('#divSubTabs li a.profSub').removeClass('active-meal');

        //        //Set this menu item as active
        //        $(this).addClass('active-meal');
        //    });
        //});



    });
</script>
<asp:HiddenField ID="hdnShowSubTabs" runat="server" ClientIDMode="Static" Value="false" />
<asp:HiddenField ID="hdnSubLastTab" runat="server" ClientIDMode="Static" Value="Basic" />
<div class="fieldRow">
    <div class="fieldCol p-5">
        <asp:Button ID="btnAddSubProfile" runat="server" Text="Add Sub-Profile" CausesValidation="false" CssClass="pixels-button-user pixels-button-solid-green-user btn btn-info" />
        <asp:Label ID="lblSubProfileFeedback" runat="server" EnableViewState="false" />
    </div>
</div>
<div class="fieldRow">
    <div class="fieldCol">
        <asp:Button ID="btnSave0" runat="server" Text="Save Sub-Profile" Visible="false" CssClass="pixels-button-user pixels-button-solid-green-user btn btn-info" />
        <asp:Button ID="btnCancel0" runat="server" Text="Cancel / Close" CausesValidation="false" Visible="false" CssClass="pixels-button-user pixels-button-solid-green-userbtn btn-danger" />
        <div>
            <asp:ValidationSummary ID="ValSumProfile0" runat="server" />
            <asp:Label ID="lblFeedback0" runat="server" ForeColor="Green" EnableViewState="false" />
        </div>
    </div>
</div>
<div id="divSubTabs">
    <ul>
        <li runat="server" id="liAddresses"><a href="#tabsSub2" class="profSub">Basic Info</a></li>
        <li runat="server" id="liShipping"><a href="#tabsSub3" class="profSub">Shipping</a></li>
        <li runat="server" id="liPrefs"><a href="#tabsSub4" class="profSub">Preferences</a></li>
        <li runat="server" id="liAllergens"><a href="#tabsSub5" class="profSub">Allergens</a></li>
    </ul>
    <div id="tabsSub2">
        <div class="fieldRow">
            <div class="fieldCol">
                <label class="col-md-2">Is Active:</label>
                <asp:CheckBox ID="chkSubIsActive" runat="server" Checked="true" />
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="fieldRow">
            <div class="fieldCol">
                <label class="col-md-2">Profile Name:</label>
                <asp:TextBox ID="txtSubProfileName" runat="server" />
                <asp:RequiredFieldValidator ID="rfvProfileName" runat="server" ControlToValidate="txtSubProfileName"
                    Text="*" Display="Dynamic" ErrorMessage="Name is required." SetFocusOnError="true" />
                <asp:CustomValidator ID="cstProfileName" runat="server" ControlToValidate="txtSubProfileName"
                    Text="*" Display="Dynamic" ErrorMessage="The Sub-Profile Name entered already exists for this account."
                    SetFocusOnError="true" OnServerValidate="cstProfileName_ServerValidate" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <label class="col-md-2">First Name:</label>
                <asp:TextBox ID="txtSubFirstName" runat="server" />
                <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtSubFirstName"
                    Text="*" Display="Dynamic" ErrorMessage="First Name is required." SetFocusOnError="true" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <label class="col-md-2">Last Name:</label>
                <asp:TextBox ID="txtSubLastName" runat="server" />
                <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtSubLastName"
                    Text="*" Display="Dynamic" ErrorMessage="Last Name is required." SetFocusOnError="true" />
            </div>
        </div>
    </div>
    <div id="tabsSub3">
        <div class="fieldRow">
            <div class="fieldCol">
                NOTE: Changes saved to shipping addresses in profiles will be applied to future purchases only, and will not be automatically applied to future delivery weeks for existing orders from previous purchases. To change the shipping address for a future delivery week for an existing order from a previous purchase, please contact Customer Service. Shipping addresses for existing orders from previous purchases cannot be changed online.<br /><br />	            
                <asp:CheckBox ID="chkUseParentShippingAddress" runat="server" Text="Same as Main Account Shipping Address" AutoPostBack="true" />
                <hcc:AddressEdit ID="AddressEdit_SubShipping" runat="server" ShowValidationSummary="false" ShowIsBusiness="true"
                    ValidationMessagePrefix="Sub-Profile - Shipping Address - " ShowSave="false" AddressType="Shipping" ShowDeliveryTypes="true" />
                <br />
                <hcc:ProfileNotesEdit ID="SubProfileNotesEdit_Shipping" runat="server" CurrentNoteType="ShippingNote" />
            </div>
        </div>
    </div>
    <div id="tabsSub4">
        <div class="fieldRow">
            <div class="fieldCol">
                <hcc:ProfilePrefsEdit ID="SubProfilePrefsEdit1" runat="server" />
            </div>
        </div>
    </div>
    <div id="tabsSub5">
        <div class="fieldRow">
            <div class="fieldCol">
                <hcc:ProfileAllgsEdit ID="SubProfileAllgsEdit1" runat="server" />
            </div>
        </div>
    </div>
</div>
<style>
    #tabsSub2 .fieldRow {
        padding:5px 0px;
    }
</style>
<script>
    $(document).ready(function () {
        $("#ctl00_Body_UserProfileEdit1_SubProfileEdit1_AddressEdit_SubShipping_SaveAddressButton").css("display", "none");
    });

</script>