<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserProfileClientDisplay.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.UserProfileClientDisplay" %>

<%@ Register TagPrefix="hcc" TagName="PasswordChange" Src="~/WebModules/ShoppingCart/Admin/UserControls/PasswordChange.ascx" %>
<%@ Register TagPrefix="hcc" TagName="AddressEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/Address_Edit.ascx" %>
<%@ Register TagPrefix="hcc" TagName="BillingInfoEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/BillingInfo_Edit.ascx" %>
<%@ Register TagPrefix="hcc" TagName="ProfilePrefsEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/UserProfilePrefs_Edit.ascx" %>
<%@ Register TagPrefix="hcc" TagName="ProfileAllgsEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/UserProfileAllg_Edit.ascx" %>
<%@ Register TagPrefix="hcc" TagName="SubProfileEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/UserSubProfile_Edit.ascx" %>
<%@ Register TagPrefix="hcc" TagName="PurchaseHistory" Src="~/WebModules/ShoppingCart/Admin/UserControls/UserProfilePurchaseHistory_Edit.ascx" %>
<%@ Register TagPrefix="hcc" TagName="BasicEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/BasicInfo_Edit.ascx" %>
<%@ Register TagPrefix="hcc" TagName="ProfileNotesEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/UserProfileNote_Edit.ascx" %>
<%@ Register TagPrefix="hcc" TagName="UserProfileRecurringOrders" Src="~/WebModules/ShoppingCart/Admin/UserControls/UserProfileRecurringOrders_Edit.ascx"  %>


<style type="text/css">
    .block
    {
        display: inline-block;
    }

    .fieldRow
    {
        margin: 0.5em 0;
    }

    label.push
    {
        min-width: 100px;
        display: inline-block;
        margin: 0 1em 0 0;
        text-align: right;
    }

    fieldset
    {
        margin: 0;
        padding: 0;
        border: none;
    }

    legend
    {
        margin: 0;
        padding: 0;
        font-size: 25px;
        line-height: 25px;
        font-weight: normal;
        color: black;
        font-family: 'MyriadPro-BoldCond', arial, sans-serif;
    }

    .form-region .form-edit
    {
        display: none;
    }
</style>
<script type="text/javascript">
    $(document).ready(function () {
        $('label').each(function (index, element) {
            $(element).addClass('push');
            if ($('input:checkbox', $(element).parent())[0]) {
                $(element).css('text-align', 'left');
            }
        });

        $("#tabsClient").tabs({
            cookie: {
                // store cookie for a day, without, it would be a session cookie
                expires: 1
            }
        });

        ////set all <DIV> elements equal in height w/o max-height property
        //if ($('.m-nav')[0]) {

        $('#tabsClient li a.profTab:first').addClass('active-meal');

        //Set references
        $('#tabsClient li a.profTab').each(function (i, e) {
            $(e).click(function (event) {
                event.preventDefault();
                $("#hdnLastTab").val($(this).text());
                //Clear active menu item
                $('#tabsClient li a.profTab').removeClass('active-meal');

                //Set this menu item as active
                $(this).addClass('active-meal');
            });
        });
        //}

        var lastTab = $("#hdnLastTab").val();

        if (lastTab.length > 0) {
            var t = $('#tabsClient li a.profTab:contains(' + lastTab + ')');
            var idx = $('#tabsClient li a.profTab').index($('#tabsClient li a.profTab:contains(' + lastTab + ')'));
            $('#tabsClient').tabs('option', "active", idx);
        }
    });
</script>
<asp:HiddenField ID="hdnLastTab" runat="server" ClientIDMode="Static" Value="My Account" />

<div id="tabsClient" class="m-nav">
    <ul>
        <li id="li_link_01" runat="server"><a href="#panel1" class="profTab"><span>Account</span></a></li>
        <li id="li_link_02" runat="server"><a href="#panel2" class="profTab"><span>Shipping</span></a></li>
        <li id="li_link_03" runat="server"><a href="#panel3" class="profTab"><span>Billing</span></a></li>
        <li id="li_link_04" runat="server"><a href="#panel4" class="profTab"><span>Preferences</span></a></li>
        <li id="li_link_05" runat="server"><a href="#panel5" class="profTab"><span>Allergens</span></a></li>
        <li id="li_link_06" runat="server"><a href="#panel6" class="profTab"><span>Sub-Profiles</span></a></li>
        <li id="li_link_07" runat="server"><a href="#panel7" class="profTab"><span>Orders</span></a></li>
        <li id="li_link_09" runat="server"><a href="#panel9" class="profTab"><span>Auto-Renewals</span></a></li>
        <li id="li_link_08" runat="server"><a href="#panel8" class="profTab"><span>Password</span></a></li>
    </ul>
    <div id="panel1" runat="server" clientidmode="Static">
        <hcc:BasicEdit ID="BasicEdit1" runat="server" ValidationGroup="BasicInfoEditGroup" ShowValidationSummary="true" />
    </div>
    <div id="panel2" runat="server" clientidmode="Static">
        <hcc:AddressEdit ID="AddressEdit_Shipping1" runat="server" ShowShippingText="true" ShowValidationSummary="true"
            ValidationMessagePrefix="Shipping " ShowIsBusiness="true" ShowSave="true" AddressType="Shipping"
            ValidationGroup="ShippingInfoEditGroup" EnableFields="true" ShowDeliveryTypes="true" SaveText="Save" />
        <br />
        <hcc:ProfileNotesEdit ID="ProfileNotesEdit_Shipping" runat="server" CurrentNoteType="ShippingNote" AllowDisplayToUser="true" AllowAddEdit="false" ShowAllNotes="false" />
    </div>
    <div id="panel3" runat="server" clientidmode="Static">
        <hcc:BillingInfoEdit ID="BillingInfoEdit1" runat="server" ShowValidationSummary="true" SaveText="Save"
            ValidationGroup="BillingInfoEditGroup" ValidationMessagePrefix="Billing " ShowAddressSave="false" />
        <br />
        <hcc:ProfileNotesEdit ID="ProfileNotesEdit_Billing" runat="server" CurrentNoteType="BillingNote" AllowDisplayToUser="true" />
    </div>
    <div id="panel4" runat="server" clientidmode="Static">
        <hcc:ProfilePrefsEdit ID="ProfilePrefsEdit1" runat="server" ValidationGroup="PrefsEditGroup" ShowSave="true" SaveText="Save" ShowValidationSummary="true" NotesAllowDisplayToUser="true" />
    </div>
    <div id="panel5" runat="server" clientidmode="Static">
        <hcc:ProfileAllgsEdit ID="ProfileAllgsEdit1" runat="server" ValidationGroup="AllgGroupGroup" ShowSave="true" SaveText="Save" ShowValidationSummary="true" NotesAllowDisplayToUser="true" />
    </div>
    <div id="panel6" runat="server" clientidmode="Static">
        <asp:GridView ID="gvwSubProfiles" runat="server" AutoGenerateColumns="false" DataKeyNames="UserProfileId" Width="100%">
            <Columns>
                <asp:BoundField DataField="ProfileName" HeaderText="Profile Name" />
                <asp:BoundField DataField="IsActive" HeaderText="Is Active" />
                <asp:CommandField HeaderText="Actions" ShowDeleteButton="true" DeleteText="Deactivate" ShowSelectButton="true" />
            </Columns>
            <EmptyDataTemplate>
                This profile has no sub-profiles.                    
            </EmptyDataTemplate>
            <HeaderStyle HorizontalAlign="Left" />
            <SelectedRowStyle CssClass="selectedRow" />
        </asp:GridView>
        <hcc:SubProfileEdit ID="SubProfileEdit1" runat="server" ValidationGroup="SubProfileEditGroup" NotesAllowDisplayToUser="true" />
    </div>
    <div id="panel7" runat="server" clientidmode="Static">
        <hcc:PurchaseHistory ID="PurchaseHistory1" runat="server" ValidationGroup="PO" />
    </div>
    <div id="panel9" runat="server" ClientIDMode="Static">
        <hcc:UserProfileRecurringOrders runat="server" id="UserProfileRecurringOrders" />
    </div>
    <div id="panel8">
        <hcc:PasswordChange ID="PasswordChange1" runat="server" ValidationGroup="PasswordChangeGroup"
            ValidationMessagePrefix="Password - " ShowValidationSummary="true" />
    </div>
</div>

