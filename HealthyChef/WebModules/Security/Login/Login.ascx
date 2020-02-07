<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="BayshoreSolutions.WebModules.Security.Login.Login" %>

<%@ Register Src="~/WebModules/Security/UserRegistration/Display.ascx" TagName="Register" TagPrefix="UsrCtrl" %>
<%@ Register Src="~/WebModules/Security/PasswordRecovery/PasswordRecoveryDisplay.ascx" TagName="Password" TagPrefix="UsrCtrl" %>

<style type="text/css">
    .cms-login fieldset, .cms-login fieldset ul {
        position: relative;
        margin: 0;
        padding: 0;
        border: none;
        list-style: none;
    }

        .cms-login fieldset ul li {
            margin: 0.5em 0;
        }

            .cms-login fieldset ul li table {
            }
</style>

<p>&nbsp;</p>
<div style="width: 100%; text-align: center;">
    <asp:Label ID="litMessage" runat="server" ForeColor="Red" EnableViewState="false" />
</div>
<asp:MultiView ID="multiViews" ActiveViewIndex="0" runat="server">
    <asp:View ID="multiLogin" runat="server">
        <script type="text/javascript">
            $(document).ready(function () {
                $('input:first', '.cms-login').focus();

                //Bind event handlers to radial buttons
                $('input:radio', '.cms-login').bind('click', checkLoginRadios);
                $('input:radio:last', '.cms-login').trigger('click');

                function checkLoginRadios() {
                    if ($('input:radio:checked', '.cms-login').val() == '1') {
                        $('input[type=password]').val("");
                        $('.cms-password').removeAttr('disabled');
                    } else {
                        $('input[type=password]').val("");
                        $('.cms-password').attr('disabled', true);
                    }
                }
            });
        </script>
        <asp:Panel ID="pnlLogin" DefaultButton="btnLogin" CssClass="cms-login login-page" runat="server">

            <h2 class="header">Account Log In</h2>

            <div style="width: 100%; text-align: center;">
                <p><span style="color: rgb(0, 0, 0);"><span style="font-size: small;">New Customers - Please enter your email address only, and click Continue.</span></span></p>
                <p><span style="color: rgb(0, 0, 0);"><span style="font-size: small;">Existing Customers - Please enter your email address and password, and click Continue.</span></span></p>
            </div>
            <div class="form">
            <fieldset>
                <ul>
                    <li>
                        <asp:Label ID="lblEmail" AssociatedControlID="txtEmail" Text=" Enter your e-mail address: " Font-Bold="true" runat="server" />
                        <asp:TextBox ID="txtEmail" MaxLength="200" Width="160" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" Text="*" Display="Dynamic" ErrorMessage="Email is required." SetFocusOnError="true" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail" Text="*" Display="Dynamic" ErrorMessage="Must be a valid email address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                    </li>
                    <li>
                        <asp:RadioButtonList ID="rdoCustomerType" class="table" runat="server">
                            <asp:ListItem Text=" I am a new customer " Value="0" />
                            <asp:ListItem Text=" I am a returning customer " Value="1" Selected="True" />
                        </asp:RadioButtonList>
                        <div style="clear: both;"></div>
                    </li>
                    <li>
                        <asp:Label ID="lblPassword" AssociatedControlID="txtPassword" Text=" Password: " Font-Bold="true" runat="server" />
                        <asp:TextBox ID="txtPassword" MaxLength="30" Width="160" TextMode="Password" CssClass="cms-password" runat="server" />
                    </li>
                    <li>
                        <asp:Button ID="btnLogin" OnClick="btnLogin_Click" Text="Continue" CssClass="pixels-button pixels-button-solid-green pixels-button-checkout" Style="margin: 1em 2em;" runat="server" />
                    </li>
                    <li>
                        <p style="margin: 2em 0; text-align: right;">
                            <asp:LinkButton ID="linkForgot" OnClick="linkForgot_Click" Text="Forgot your password? Click Here" CausesValidation="false" runat="server" />
                        </p>
                    </li>
                </ul>
            </fieldset>
                </div>
        </asp:Panel>
    </asp:View>
    <asp:View ID="viewRegister" runat="server">
        <UsrCtrl:Register ID="usrctrlRegister" runat="server" />
    </asp:View>
    <asp:View ID="viewPassword" runat="server">
        <UsrCtrl:Password ID="usrctrlPassword" runat="server" />
    </asp:View>
</asp:MultiView>