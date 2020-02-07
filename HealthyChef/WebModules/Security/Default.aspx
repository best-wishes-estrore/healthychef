<%@ Page Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BayshoreSolutions.WebModules.Security.Default" Title="Security Settings" Theme="WebModules" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <div class="main-content">
        <div class="main-content-inner">
            <div class="page-header">
                <h1>Security Modules Settings</h1>
            </div>
            <div class="entity_edit">

                <fieldset>
                    <legend><strong>Email notification</strong></legend>

                    <div class="help">These settings apply to <strong>all</strong> security-related emails sent from this site.</div>

                    <div class="field">
                        Email 'From' address
            <div>
                <asp:TextBox ID="EmailFromAddress" runat="server" Width="300px" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="EmailFromAddress"
                    Display="Dynamic" ErrorMessage="Required" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="EmailFromAddress"
                    ErrorMessage="Must be a vaild email address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="dynamic"></asp:RegularExpressionValidator>
            </div>
                    </div>

                    <div class="field">
                        Email body header
            <div>
                <span class="help">Text will be inserted at the beginning of the email.</span>
            </div>
                        <asp:TextBox ID="EmailBodyHeader" runat="server" Height="4em" Width="100%" TextMode="multiline" />
                    </div>

                    <div class="field">
                        Email body footer
            <div>
                <span class="help">Text will be inserted at the end of the email.</span>
            </div>
                        <asp:TextBox ID="EmailBodyFooter" runat="server" Height="4em" Width="100%" TextMode="multiline" />
                    </div>
                </fieldset>

                <fieldset>
                    <legend><strong>User registration</strong></legend>

                    <div class="field">
                        Email subject 
            <div>
                <asp:TextBox ID="UserRegEmailSubject" runat="server" Width="300px" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                    ControlToValidate="UserRegEmailSubject"
                    ErrorMessage="Required" />
            </div>
                    </div>

                    <div class="field">
                        Email body 
            <asp:TextBox ID="UserRegEmailBody" runat="server" TextMode="MultiLine" Width="100%" Height="4em" />
                    </div>
                </fieldset>

                <fieldset>
                    <legend><strong>Password recovery</strong></legend>

                    <div class="field">
                        Email subject 
            <div>
                <asp:TextBox ID="PasswordRecoveryEmailSubject" runat="server" Width="300px" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                    ControlToValidate="PasswordRecoveryEmailSubject"
                    ErrorMessage="Required" />
            </div>
                    </div>

                    <div class="field">
                        Email body 
            <asp:TextBox ID="PasswordRecoveryEmailBody" runat="server" TextMode="MultiLine" Width="100%" Height="4em" />
                    </div>
                </fieldset>

                <div class="toolbar m-2">
                    <asp:Button ID="SaveButton" CssClass="saveButton btn btn-info" runat="server" Text="Save" Font-Bold="true" OnClick="SaveButton_Click" />
                    <asp:Button ID="CancelButton" CssClass="cancelButton btn btn-danger" runat="server"
                        CausesValidation="false"
                        OnClick="CancelButton_Click"
                        Text="Cancel" />
                </div>

            </div>
        </div>
    </div>
    <script type="text/javascript">
    $(function () {
        ToggleMenus('system', undefined, undefined);
    });
    </script>
</asp:Content>
