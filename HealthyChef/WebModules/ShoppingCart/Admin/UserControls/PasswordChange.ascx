<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PasswordChange.ascx.cs" Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.ChangePassword" %>
<fieldset>
    <%--    <legend>Change Password:</legend>--%>
    <asp:Panel ID="pnlChangePassword" runat="server" DefaultButton="btnSave">
        <table>
            <tr>
                <td style="text-align: right; font-weight: bold; vertical-align: top;">Instructions:
                </td>
                <td>
                    <ol style="margin: 0;">
                        <li>Enter your current password into the field labeled "Current Password".</li>
                        <li>Enter your new password into the field labeled "New Password".</li>
                        <li>Confirm your new password by entering it a second time into the field labeled "Confirm New Password".</li>
                    </ol>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; font-weight: bold; vertical-align: top;">Password Requirements:
                </td>
                <td>
                    <ul style="margin: 0;">
                        <li>Passwords must be at least 5 characters in length.</li>
                    </ul>
                    <br /><br />
                </td>
            </tr>
            <tr>
                <td style="text-align: right; font-weight: bold;">Current Password:
                </td>
                <td>
                    <asp:TextBox ID="txtCurrentPassword" runat="server" TextMode="Password" autocomplete="off" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator0" runat="server" ControlToValidate="txtCurrentPassword"
                        Text="*" Display="Dynamic" ErrorMessage="Current password is required" SetFocusOnError="true" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right; font-weight: bold;">New Password:
                </td>
                <td>
                    <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" autocomplete="off" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNewPassword"
                        Text="*" Display="Dynamic" ErrorMessage="New password is required" SetFocusOnError="true" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right; font-weight: bold;">Confirm New Password:
                </td>
                <td>
                    <asp:TextBox ID="txtNewPasswordConfirm" runat="server" TextMode="Password" autocomplete="off" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNewPasswordConfirm"
                        Text="*" Display="Dynamic" ErrorMessage="Confirm new password is required" SetFocusOnError="true" />
                    <asp:CompareValidator ID="cpv1" runat="server" ControlToValidate="txtNewPasswordConfirm"
                        ControlToCompare="txtNewPassword" Text="*" Display="Dynamic" ErrorMessage="New password must match new password confirm"
                        SetFocusOnError="true" Type="String" Operator="Equal" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnSave" runat="server" Text="Save Password" CssClass="pixels-button-user pixels-button-solid-green-user btn btn-info" />
                    <br />
                    <asp:ValidationSummary ID="ValSum1" runat="server" CssClass="errorText" />
                    <asp:Label ID="lblFeedback" runat="server" CssClass="errorText" EnableViewState="false" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</fieldset>
