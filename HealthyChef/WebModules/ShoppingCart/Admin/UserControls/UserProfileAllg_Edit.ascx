﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserProfileAllg_Edit.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.UserProfileAllg_Edit" %>

<%@ Register TagPrefix="hcc" TagName="ProfileNotesEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/UserProfileNote_Edit.ascx" %>
<div class="m-2">
    <asp:Panel ID="pnlAllg" runat="server" DefaultButton="btnSave">
        <div class="fieldRow">
            <div class="fieldCol">
                <p>Account Allergens shall be applied to all Meal Program orders only. A La Carte meals may be individually customized at checkout.</p>
                <p>
                    NOTE: Changes made to food preferences and allergen information in profiles will be applied to future purchases and also to future
                 delivery weeks for existing orders, but may not be reflected in your upcoming current delivery week.
                 Changes made after a weekly order deadline (i.e., Thursday night) will not be reflected in that week's deliveries.
                </p>
                <asp:CheckBoxList ID="cblAllergens" RepeatColumns="5" RepeatLayout="Table" CssClass="checkbox_1" runat="server" />
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
            <asp:Label ID="lblFeedback" runat="server" ForeColor="Green" EnableViewState="false" data-ctrl="allg" />
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <hcc:ProfileNotesEdit ID="ProfileNotesEdit_Allgs" runat="server" CurrentNoteType="AllergenNote" />
            </div>
        </div>
    </asp:Panel>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        if (document.URL.indexOf('login.aspx') == -1) {
            $("[id$=pnlAllg]").find(":input").change(function () {
                $("[id$=lblFeedback]").filter('[data-ctrl="allg"]')
                    .text('There may be unsaved values in this section. If you have changed information in this section, please be sure to save it before moving on.')
                    .css("color", "#F60");
            });
        }
    });
</script>
