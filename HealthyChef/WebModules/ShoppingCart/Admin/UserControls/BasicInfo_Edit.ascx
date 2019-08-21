<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BasicInfo_Edit.ascx.cs" Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.BasicInfo_Edit" %>
<div class="plan_edit">
    <asp:Panel ID="pnlBasicEdit" runat="server" DefaultButton="btnSave">
        <div class="fieldRow">
            <div class="fieldCol" style="text-align: right;">
                <h3>Account Balance:
                <asp:Label ID="lblAccountBalance" runat="server" /></h3>
                <hr style="width: 100%;" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:Label ID="lblProfileName" AssociatedControlID="txtProfileName" Text="Profile Name:" runat="server" />
                <asp:TextBox ID="txtProfileName" runat="server"  />
                <asp:RequiredFieldValidator ID="rfvProfileName" runat="server" ControlToValidate="txtProfileName" Text="*"
                    Display="Dynamic" ErrorMessage="Profile Name is required." SetFocusOnError="true" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:Label ID="lblFirstName" AssociatedControlID="txtFirstName" Text="First Name:" runat="server" />
                <asp:TextBox ID="txtFirstName" runat="server" />
                <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName" Text="*"
                    Display="Dynamic" ErrorMessage="First Name is required." SetFocusOnError="true" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:Label ID="lblLastName" AssociatedControlID="txtLastName" Text="Last Name:" runat="server" />
                <asp:TextBox ID="txtLastName" runat="server" />
                <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName" Text="*"
                    Display="Dynamic" ErrorMessage="Last Name is required." SetFocusOnError="true" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:Label ID="lblEmail" AssociatedControlID="txtEmail" Text="E-mail:" runat="server" />
                <asp:TextBox ID="txtEmail" runat="server" Width="200px" />
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" Text="*" Display="Dynamic"
                    ErrorMessage="Email is required." SetFocusOnError="true" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                    Text="*" Display="Dynamic" ErrorMessage="Must be a valid email address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <!--<asp:Label ID="Label1" AssociatedControlID="txtEmail" Text="Yes, I would like to receive the Healthy Chef Creations newsletter and promotional emails from Healthy Chef Creations" runat="server" />
            <asp:CheckBox ID="cbMarketingOptIn" runat="server" />-->
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="pixels-button-user pixels-button-solid-green-user btn btn-info" />
                <div>
                    <asp:ValidationSummary ID="ValSum1" runat="server" />
                </div>
                <div>
                    <asp:Label ID="lblFeedback0" runat="server" ForeColor="Green" EnableViewState="false" data-ctrl="basic" />
                </div>
            </div>
        </div>
</asp:Panel>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        if (document.URL.indexOf('login.aspx') == -1) {
            $("[id$=pnlBasicEdit]").find(":input").change(function () {
                $("[id$=lblFeedback0]").filter('[data-ctrl="basic"]')
                    .text('There may be unsaved values in this section. If you have changed information in this section, please be sure to save it before moving on.')
                    .css("color", "#F60");
            });
        }
    });


    $('.AlphabetsOnly').keypress(function (e) {
        
        var regex = new RegExp("^[a-zA-Z ]+$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    });

</script>
