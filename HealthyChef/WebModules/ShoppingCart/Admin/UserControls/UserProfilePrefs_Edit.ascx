<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserProfilePrefs_Edit.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.UserProfilePrefs_Edit" %>

<%@ Register TagPrefix="hcc" TagName="ProfileNotesEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/UserProfileNote_Edit.ascx" %>
<div class="m-2">
    <asp:Panel ID="pnlPrefs" runat="server" DefaultButton="btnSave">
        <div class="fieldRow">
            <div class="fieldCol">
                <p>Account Preferences shall be applied to Custom Meal Programs only.</p>
                <p>
                  <span style="color:red">NOTE: Changes to your Custom Meal Program preferences can be made by calling our customer service center.</span>
                    If changes are not made by the ordering deadline, they may not be reflected in your next delivery. 
                    Please review with your customer service rep for details. 
                </p>
                <asp:CheckBoxList ID="cblPreferences" runat="server" RepeatColumns="5" RepeatLayout="Table" CssClass="checkbox_1" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="pixels-button-user pixels-button-solid-green-user btn btn-info" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:ValidationSummary ID="ValSum1" runat="server" DisplayMode="BulletList" />
            </div>
            <asp:Label ID="lblFeedback" runat="server" ForeColor="Green" EnableViewState="false" data-ctrl="pref" />
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <hcc:ProfileNotesEdit ID="ProfileNotesEdit_Prefs" runat="server" CurrentNoteType="PreferenceNote"
                    AllowAddEdit="true" AllowDisplayToUser="true" />
            </div>
        </div>
    </asp:Panel>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        if (document.URL.indexOf('login.aspx') == -1) {
            $("[id$=pnlPrefs]").find(":input").change(function () {
                $("[id$=lblFeedback]").filter('[data-ctrl="pref"]')
                    .text('There may be unsaved values in this section. If you have changed information in this section, please be sure to save it before moving on.')
                    .css("color", "#F60");
            });
        }
    });
</script>

