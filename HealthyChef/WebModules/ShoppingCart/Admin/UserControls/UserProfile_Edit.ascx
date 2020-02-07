<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserProfile_Edit.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.UserProfile_Edit" %>

<%@ Register TagPrefix="hcc" TagName="PasswordReset" Src="~/WebModules/ShoppingCart/Admin/UserControls/PasswordReset.ascx" %>
<%@ Register TagPrefix="hcc" TagName="AddressEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/Address_Edit.ascx" %>
<%@ Register TagPrefix="hcc" TagName="BillingInfoEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/BillingInfo_Edit.ascx" %>
<%@ Register TagPrefix="hcc" TagName="ProfilePrefsEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/UserProfilePrefs_Edit.ascx" %>
<%@ Register TagPrefix="hcc" TagName="ProfileAllgsEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/UserProfileAllg_Edit.ascx" %>
<%@ Register TagPrefix="hcc" TagName="SubProfileEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/UserSubProfile_Edit.ascx" %>
<%@ Register TagPrefix="hcc" TagName="ProfileNotesEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/UserProfileNote_Edit.ascx" %>
<%@ Register TagPrefix="hcc" TagName="ProfileCartEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/UserProfileCart_Edit.ascx" %>
<%@ Register TagPrefix="hcc" TagName="ProfileLedger" Src="~/WebModules/ShoppingCart/Admin/UserControls/UserProfileLedger_Edit.ascx" %>
<%@ Register TagPrefix="hcc" TagName="PurchaseHistory" Src="~/WebModules/ShoppingCart/Admin/UserControls/UserProfilePurchaseHistory_Edit.ascx" %>
<%@ Register TagPrefix="hcc" TagName="RecurringOrders" Src="~/WebModules/ShoppingCart/Admin/UserControls/UserProfileRecurringOrders_Edit.ascx" %>

<script type="text/javascript">
    $(document).ready(function () {
        $('#UserFiltersReset').hide();
        if ($("#hdnShowTabs").val() == 'true') {

            var idx = 0;
            var hdnProfileLastTab = $("#hdnProfileLastTab").val();

            if (hdnProfileLastTab.length > 0) {
                var t = $('#divProfileTabs li a.profTab:contains(' + hdnProfileLastTab + ')');
                idx = $('#divProfileTabs li a.profTab').index($('#divProfileTabs li a.profTab:contains(' + hdnProfileLastTab + ')'));
            }

            $("#divProfileTabs").tabs({
                active: idx,
                cookie: {
                    // store cookie for a day, without, it would be a session cookie
                    expires: 1
                }
            });
        }
        else {
            $("#divProfileTabs").hide();
        }

        $('#divProfileTabs li a.profTab:first').addClass('active-meal');

        //Set references
        $('#divProfileTabs li a.profTab').each(function (i, e) {
            $(e).click(function (event) {
                event.preventDefault();
                $("#hdnProfileLastTab").val($(this).text());
                //Clear active menu item
                $('#divProfileTabs li a.profTab').removeClass('active-meal');

                //Set this menu item as active
                $(this).addClass('active-meal');
            });
        });
    });
</script>

<asp:HiddenField ID="hdnShowTabs" runat="server" Value="false" ClientIDMode="Static" />
<asp:HiddenField ID="hdnProfileLastTab" runat="server" ClientIDMode="Static" Value="Basic" />
 <asp:HiddenField ID="CurrentUserID" runat="server" />
<div class="m-2">
    <div class="fieldRow">
        <div class="fieldCol">
            <asp:Button ID="btnSave0" runat="server" Text="Save" CausesValidation="false" OnClientClick="MakeUpdateProg(true);" CssClass="btn btn-info" />
            <%--<button type="button" ng-click="UpdateBasicInfo();" class="btn btn-primary" >Save</button>--%>
            <asp:Button ID="btnCancel0" runat="server" Text="Cancel / Close" CausesValidation="false" OnClientClick="MakeUpdateProg(true);" CssClass="btn btn-danger" Visible="false" />
            <a href="/WebModules/ShoppingCart/Admin/AccountManager.aspx" class="btn btn-danger">Cancel / Close</a>
            <div>
                <asp:ValidationSummary ID="ValSumProfile0" runat="server" />
                <asp:Label ID="lblFeedback" runat="server" ForeColor="Red" EnableViewState="false" />
                <asp:CustomValidator ID="cstValProfile0" runat="server" EnableClientScript="false"
                    Enabled="false" Display="None" />
                <asp:CustomValidator ID="cstValCardInfo0" runat="server" EnableClientScript="false"
                    Enabled="false" Display="None" />
            </div>
        </div>
    </div>
    <p></p>
    <asp:Panel ID="pnlGeneralInfo" runat="server">
        <div class="fieldRow">
            <div class="fieldCol align_check">
                <asp:CheckBox ID="chkIsLockedOut" runat="server" Text="Is Locked Out?" />
                &nbsp;&nbsp; 
            <asp:CheckBox ID="chkIsActive" runat="server" Text="Is Active?" />
                &nbsp;&nbsp; 
            <asp:Button ID="btnSaveActiveStatus" CssClass="btn btn-info m-2" runat="server" Text="Save Is Locked / Is Active" OnClientClick="MakeUpdateProg(true);" OnClick="btnSaveActivestatus_Click" CausesValidation="false" Visible="false" />
                <button type="button" ng-click="UpdateCustomerStatus();" class="btn btn-info">Save Is Locked / Is Active</button>
                <asp:Label ID="lblSaveActiveStatusFeedback" runat="server" ForeColor="Green" EnableViewState="false" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                Email:
        <asp:TextBox ID="txtEmail" runat="server" />
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                    Text="*" Display="Dynamic" ErrorMessage="Email is required." SetFocusOnError="true" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                    Text="*" Display="Dynamic" ErrorMessage="Must be a valid email address" ValidationExpression="(\w[-._\w]*\w@\w[-._\w]*\w\.\w{2,3})" />
            </div>
        </div>
        <div class="fieldRow" id="divRoles" runat="server">

            <div class="fieldCol">
                <fieldset>
                    <legend>Roles:</legend>
                    <div style="float: right; position: relative; right: 150px;">
                        <table style="padding: 5px;">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="cbCanyonRanchCustomer" runat="server" />
                                </td>
                                <td>
                                    <label>Opted-In for Marketing.</label></td>
                            </tr>
                        </table>
                    </div>
                    <asp:CheckBoxList ID="cblRoles" runat="server" AutoPostBack="true" Enabled="false" />
                </fieldset>
            </div>
        </div>
        <div class="fieldRow" id="divPassword" runat="server" visible="false">
            <div class="fieldCol">
                <hcc:PasswordReset ID="PasswordReset1" runat="server" ValidationGroup="PasswordResetGroup" />
            </div>
        </div>
    </asp:Panel>
    <div>&nbsp;</div>
    <div id="divProfileTabs">
        <ul>
            <li id="liBasic" runat="server" clientidmode="Static"><a href="#tabs1" class="profTab">Basic Info</a></li>
            <li id="liBilling" runat="server" clientidmode="Static"><a href="#tabs2" class="profTab">Billing</a></li>
            <li id="liShipping" runat="server" clientidmode="Static"><a href="#tabs3" class="profTab">Shipping</a></li>
            <li id="liPrefs" runat="server" clientidmode="Static"><a href="#tabs4" class="profTab">Preferences</a></li>
            <li id="liAllergens" runat="server" clientidmode="Static"><a href="#tabs5" class="profTab">Allergens</a></li>
            <li id="liSubProfiles" runat="server" clientidmode="Static"><a href="#tabs6" class="profTab">Sub-Profiles</a></li>
            <li id="liNotes" runat="server" clientidmode="Static"><a href="#tabs7" class="profTab">Notes</a></li>
            <li id="liTransactions" runat="server" clientidmode="Static"><a href="#tabs8" class="profTab">Transactions</a></li>
            <li id="liPurchases" runat="server" clientidmode="Static"><a href="#tabs10" class="profTab">Purchase History</a></li>
            <li id="liRecurring" runat="server" clientidmode="Static"><a href="#tabs11" class="profTab">Auto-Renewals</a></li>
            <li id="liCart" runat="server" clientidmode="Static"><a href="#tabs9" class="profTab">Current Cart</a></li>
        </ul>
        <div id="tabs1" runat="server" clientidmode="Static">
            <div class="m-2">
                <div class="plan_edit">
                    <div class="fieldRow">
                        <div class="fieldCol">
                            <label>Profile Name:</label>
                            <asp:TextBox ID="txtProfileName" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvProfileName" runat="server" ControlToValidate="txtProfileName"
                                Text="*" Display="Dynamic" ErrorMessage="Profile Name is required." SetFocusOnError="true" ValidationGroup="UserProfileEditGroup"
                                Enabled="false" />
                            <asp:CustomValidator ID="cstProfileName" runat="server" ControlToValidate="txtProfileName"
                                Text="*" Display="Dynamic" ErrorMessage="The Profile Name entered already exists for this account."
                                SetFocusOnError="true" />
                        </div>
                    </div>
                    <div class="fieldRow">
                        <div class="fieldCol">
                            <label>First Name:</label>
                            <asp:TextBox ID="txtFirstName" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName"
                                Text="*" Display="Dynamic" ErrorMessage="First Name is required." SetFocusOnError="true" ValidationGroup="UserProfileEditGroup"
                                Enabled="false" />
                        </div>
                    </div>
                    <div class="fieldRow">
                        <div class="fieldCol">
                            <label>Last Name:</label>
                            <asp:TextBox ID="txtLastName" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName"
                                Text="*" Display="Dynamic" ErrorMessage="Last Name is required." SetFocusOnError="true" ValidationGroup="UserProfileEditGroup"
                                Enabled="false" />
                        </div>
                    </div>
                    <div class="fieldRow">
                        <div class="fieldCol">
                            <label>Default Coupon:</label>
                            <asp:DropDownList ID="ddlCoupons" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="tabs2" runat="server" clientidmode="Static">
            <div class="">
                <div class="fieldRow">
                    <div class="fieldCol">
                        <hcc:BillingInfoEdit ID="BillingInfoEdit1" runat="server" ValidationGroup="UserProfileEdit_BillingAddressGroup"
                            ValidationMessagePrefix="Billing - " ShowValidationSummary="true" ShowAddressSave="true" />
                        <br />
                        <hcc:ProfileNotesEdit ID="ProfileNotesEdit_Billing" runat="server" ValidationGroup="ProfileBillNotesEditGroup"
                            CurrentNoteType="BillingNote" AllowAddEdit="true" AllowDisplayToUser="true" ShowAllNotes="true" />
                    </div>
                </div>
            </div>
        </div>
        <div id="tabs3" runat="server" clientidmode="Static">
            <div class="m-2">
                <div class="fieldRow">
                    <div class="fieldCol">
                        <hcc:AddressEdit ID="AddressEdit_Shipping1" runat="server" ValidationGroup="UserProfileEdit_ShippingAddressGroup"
                            ShowValidationSummary="true" ShowSave="true" ValidationMessagePrefix="Shipping Address - " AddressType="Shipping"
                            EnableFields="false" ShowIsBusiness="true" ShowDeliveryTypes="true" />
                        <br />
                        <hcc:ProfileNotesEdit ID="ProfileNotesEdit_Shipping" runat="server" CurrentNoteType="ShippingNote" AllowAddEdit="true" AllowDisplayToUser="true" ShowAllNotes="true" />
                    </div>
                </div>
            </div>
        </div>
        <div id="tabs4" runat="server" clientidmode="Static">
            <div class="m-2">
                <div class="fieldRow">
                    <div class="fieldCol">
                        <hcc:ProfilePrefsEdit ID="ProfilePrefsEdit1" runat="server" NotesAllowAddEdit="true" NotesAllowDisplayToUser="true" />

                    </div>
                </div>
            </div>
        </div>
        <div id="tabs5" runat="server" clientidmode="Static">
            <div class="m-2">
                <div class="fieldRow">
                    <div class="fieldCol">
                        <hcc:ProfileAllgsEdit ID="ProfileAllgsEdit1" runat="server" NotesAllowAddEdit="true" NotesAllowDisplayToUser="true" />
                    </div>
                </div>
            </div>
        </div>
        <div id="tabs6" runat="server" clientidmode="Static">
            <div class="m-2">
                <div class="fieldRow">
                    <div class="fieldCol p-5">
                        <asp:GridView ID="gvwSubProfiles" runat="server" AutoGenerateColumns="false" DataKeyNames="UserProfileId"
                            Width="900px">
                            <Columns>
                                <asp:BoundField DataField="ProfileName" HeaderText="Profile Name" />
                                <asp:BoundField DataField="IsActive" HeaderText="Is Active" />
                                <asp:CommandField HeaderText="Actions" ShowDeleteButton="true" DeleteText="Deactivate"
                                    ShowSelectButton="true" />
                            </Columns>
                            <EmptyDataTemplate>
                                This profile has no sub-profiles.
                            </EmptyDataTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <SelectedRowStyle CssClass="selectedRow" />
                        </asp:GridView>
                        <hcc:SubProfileEdit ID="SubProfileEdit1" runat="server" ValidationGroup="SubProfile_EditGroup"
                            ValidationMessagePrefix="Sub-Profile - " NotesAllowAddEdit="true" NotesAllowDisplayToUser="true" />
                    </div>
                </div>
            </div>
        </div>
        <div id="tabs8" runat="server" clientidmode="Static">
            <div class="m-2">
                <div class="fieldRow">
                    <div class="fieldCol">
                        <hcc:ProfileLedger ID="ProfileLedger1" runat="server" ValidationGroup="AddTransactionGroup" />
                    </div>
                </div>
            </div>
        </div>
        <div id="tabs10" runat="server" clientidmode="Static">
            <div class="m-2">
                <div class="fieldRow">
                    <div class="fieldCol">
                        <asp:Label ID="lblPurchHistoryFeedback" runat="server" EnableViewState="false" ForeColor="Green" Font-Bold="true" />
                        <hcc:PurchaseHistory ID="PurchaseHistory1" runat="server" ValidationGroup="UserPurchaseHistoryEditGroup" />
                    </div>
                </div>
            </div>
        </div>
        <div id="tabs11" runat="server" clientidmode="Static">
            <div class="m-2">
                <div class="fieldRow">
                    <div class="fieldCol">
                        <asp:Label ID="lblRecurringOrders" runat="server" EnableViewState="false" ForeColor="Green" Font-Bold="true" />
                        <hcc:RecurringOrders ID="RecurringOrders1" runat="server" />
                    </div>
                </div>
            </div>
        </div>
        <div id="tabs7" runat="server" clientidmode="Static">
            <div class="m-2">
                <div class="fieldRow">
                    <div class="fieldCol">
                        These notes are NOT VISIBLE to the Consumer
                <br />
                        <hcc:ProfileNotesEdit ID="ProfileNotesEdit_General" runat="server" ValidationGroup="ProfileGenNotesEditGroup"
                            CurrentNoteType="GeneralNote" AllowAddEdit="true" ShowAllNotes="true" />
                    </div>
                </div>
            </div>
        </div>
        <div id="tabs9" runat="server" clientidmode="Static">
            <div class="m-2">
                <div class="fieldRow">
                    <div class="fieldCol">
                        <hcc:ProfileCartEdit ID="ProfileCartEdit1" runat="server" ValidationGroup="ProfileCartEditGroup" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<style>
    #tabs11 table {
        width: 100%;
    }
    #tabs11 table tr td:nth-child(2) {
      text-align:left !important;
    }
     #tabs11 table tr td:nth-child(3) {
     width:13%;
    }
    #tabs2 th, #tabs3 th, #tabs7 th, #tabs4 th {
        width:20%;
    }
    .searchuser {
        display:none;
    }
    /*#tabs3 #SaveAddressButton {
        display:none;
    }*/
</style>
