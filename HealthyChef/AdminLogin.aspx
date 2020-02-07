<%@ Page Language="C#" AutoEventWireup="true" Title="Login" CodeBehind="AdminLogin.aspx.cs" Inherits="HealthyChef.AdminLogin" %>

<%--<%@ Control Language="C#" AutoEventWireup="true" Title="Login" CodeBehind="AdminLogin.aspx.cs" Inherits="BayshoreSolutions.WebModules.Security.Login.Login" %>--%>


<!DOCTYPE html>
<html>
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
        <div style="text-align: justify; max-width: 380px; margin: 0px auto;">
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
                        <!--<label class="col-md-3" for="Email">Enter your E-mail address:</label>-->
                        <asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ErrorMessage="Please Enter email address." ControlToValidate="txtEmail" CssClass="field-validation-valid"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="ValidEmailRequired" runat="server" ErrorMessage="Please Enter valid email address." ControlToValidate="txtEmail" CssClass="field-validation-valid" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>

                        <!--<input class="col-md-4" data-val="true" data-val-required="Please Enter Email Address" id="Email" name="Email" type="text" value="" />
                        <span class="field-validation-valid" data-valmsg-for="Email" data-valmsg-replace="true"></span>-->
                    </div>

                    <div class="p-10">

                        <asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ErrorMessage="Please Enter password" ControlToValidate="txtPassword" CssClass="field-validation-valid"></asp:RequiredFieldValidator>

                        <!--<label class="col-md-3" for="Password">Password:</label>
                        <input class="col-md-4" data-val="true" data-val-required="Please Enter Password" id="Password" name="Password" type="password" />
                        <span class="field-validation-valid" data-valmsg-for="Password" data-valmsg-replace="true"></span>-->
                    </div>

                    <div class="p-10 text-center">
                        <%--<input type="submit" class="btn btn-success" value="Continue" onclick="Continueclick" />--%>
                        <asp:Button ID="btnLogin" runat="server" Text="Continue" class="btn btn-success" OnClick="btnLogin_Click" />
                    </div>
                </div>
            </div>
        </form>
        <asp:HyperLink ID="ForgotPassword" runat="server" CssClass="forgot_password">Forgot your password? Click Here</asp:HyperLink>
    </div>
    <script>
        $("#ForgotPassword").click(function () {
            window.location = "/AdminForgotPassword.aspx";
        });
    </script>
    <style>
        .forgot_password {
            text-align: center;
            position: absolute;
            left: 40%;
            margin-top: 16px;
            font-size: 16px;
            cursor: pointer;
        }
    </style>
</body>
</html>
