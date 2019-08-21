<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminForgotPassword.aspx.cs" Inherits="HealthyChef.AdminForgotPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link href="Scripts/bootstrap.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css?family=Fjalla+One" rel="stylesheet" />
    <link href="Scripts/Login.css" rel="stylesheet" />
    <script src="Scripts/jquery.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>

</head>
<body>
    <div class="wrapper">
        <div style="width: 100%; text-align: center;">
            <asp:Label ID="litMessage" runat="server" ForeColor="Red" EnableViewState="false" />
        </div>
        <form action="/login.aspx" method="post" class="form-signin" novalidate="novalidate" runat="server">
            <div class="login-page">
                <div class="text-center">
                    <img class="img-responsive" src="App_Themes/HealthyChef/Images/Healthy-Chef-Logo.png" />
                    <h2 class="heading">Admin</h2>
                </div>
                <hr />
                <div class="form_submit">
                    <div class="p-10">
                        <asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label><br />
                        <asp:TextBox ID="txtForgotEmail" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="ForgotEmailRequired" runat="server" ErrorMessage="Please Enter email address." ControlToValidate="txtForgotEmail" CssClass="field-validation-valid"></asp:RequiredFieldValidator>
                        <br /><br />
                        <asp:RegularExpressionValidator ID="ValidForgotEmailRequired" runat="server" ErrorMessage="Please Enter valid email address." ControlToValidate="txtForgotEmail" CssClass="field-validation-valid" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        <asp:Label ID="lblerror" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblsuccess" runat="server" Text=""></asp:Label>
                        <asp:Button ID="btnContinue" runat="server" Text="Continue" class="btn btn-success continue" OnClick="btnContinue_Click" />
                        <input type="button" value="<< Login" class="btn btn-success login_data" onclick="Loginredirection()" />

                    </div>
                </div>
            </div>
        </form>

    </div>
    <script>
        function Loginredirection() {
            window.location = "/AdminLogin.aspx";
        };
    </script>
    <style>
        .login_data {
            margin-top: 30px;
        }

        .continue {
            margin-top: 30px;
            margin-right: 20px;
        }

        #ForgotEmailRequired, #ValidForgotEmailRequired {
            padding-top: 8px;
        }

        #lblsuccess {
            font-family: inherit;
            font-size: 16px;
            color: #39aa35;
            display: block;
        }

        #lblerror {
            font-family: inherit;
            font-size: 16px;
            color: #ff0000;

        }
        #ForgotEmailRequired {
            font-family: inherit;
            font-size: 16px;
        }
    </style>
</body>
</html>
