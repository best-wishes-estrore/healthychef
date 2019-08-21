<%@ Page Language="C#" MasterPageFile="~/Templates/None/None.master" Theme="WebModules" AutoEventWireup="true" Title="Login" Codebehind="Login.aspx.cs" Inherits="BayshoreSolutions.WebModules.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <asp:MultiView ID="LoginMultiView" runat="server" ActiveViewIndex="0" EnableTheming="False">
        <asp:View ID="LoginView" runat="server">
            <div style="margin-top: 7em;">
                <center>
                    <asp:Login ID="Login1" runat="server" DestinationPageUrl="~/WebModules/Default.aspx"
                        RememberMeSet="True" OnLoggedIn="Login1_LoggedIn" DisplayRememberMe="false" 
                        BackColor="white" BorderWidth="1" BorderColor="gray" BorderStyle="dotted" BorderPadding="10"
                        UserNameLabelText="Username&nbsp;" PasswordLabelText="Password&nbsp;" TitleText="WebModules Login" 
                        LoginButtonText="Log in"
                        FailureText="Invalid username or password.">
                        <TextBoxStyle Width="150px" />
                    </asp:Login>
                </center>
            </div>
        </asp:View>
        <asp:View ID="CreateUserView" runat="server">
            <center>
            <div style="margin-top: 7em;">
            <asp:CreateUserWizard ID="CreateUserWizard" BackColor="white" BorderColor="gray" BorderWidth="1" BorderStyle="solid" CellPadding="10" runat="server" OnCreatedUser="CreateUserWizard_CreatedUser" FinishDestinationPageUrl="~/WebModules/Default.aspx" ActiveStepIndex="0" OnContinueButtonClick="CreateUserWizard_ContinueButtonClick">
                <WizardSteps>
                    <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
                        <ContentTemplate>
                            <table border="0">
                                <tr>
                                    <td align="center" colspan="2">
                                        Create an administrator account</td>
                                </tr>
                                <tr> 
                                    <td align="left">
                                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label></td>
                                    <td style="width: 202px">
                                        <asp:TextBox ID="UserName" runat="server" Width="150px" />
                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                            ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label></td>
                                    <td style="width: 202px">
                                        <asp:TextBox ID="Password" runat="server" TextMode="Password" Width="150px" />
                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                            ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword">Confirm Password:</asp:Label></td>
                                    <td style="width: 202px">
                                        <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password" Width="150px" />
                                        <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPassword"
                                            ErrorMessage="Confirm Password is required." ToolTip="Confirm Password is required."
                                            ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email">E-mail:</asp:Label></td>
                                    <td style="width: 202px">
                                        <asp:TextBox ID="Email" runat="server" Width="150px"
                                            Text="mfeliciano@bayshoresolutions.com"
                                             />
                                        <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email"
                                            ErrorMessage="E-mail is required." ToolTip="E-mail is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password"
                                            ControlToValidate="ConfirmPassword" Display="Dynamic" ErrorMessage="The Password and Confirmation Password must match."
                                            ValidationGroup="CreateUserWizard1"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2" style="color: red">
                                        <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:CreateUserWizardStep>
                    <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server" OnActivate="CompleteWizardStep1_Activate">
                    </asp:CompleteWizardStep>
                </WizardSteps>
                <TextBoxStyle Width="150px" />
                <LabelStyle HorizontalAlign="Left" />
            </asp:CreateUserWizard>
            </div>
            </center>
        </asp:View>
    </asp:MultiView>
</asp:Content>