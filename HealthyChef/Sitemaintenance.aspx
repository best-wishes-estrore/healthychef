﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sitemaintenance.aspx.cs" Inherits="HealthyChef.Sitemaintenance" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 280px;
            height: 60px;
        }

        .imageContainer {
            width: auto;
            height: 500px;
            background-image: url(App_Themes/HealthyChef/Images/body.jpg);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <div>
            <img class="auto-style1" src="App_Themes/HealthyChef/Images/Healthy-Chef-Logo.png" />
            <div style="font-family: sans-serif;">
                <%-- class="imageContainer"<img src="App_Themes/HealthyChef/Images/bg-full-page-top.png" />--%>
                <%--<img src="App_Themes/HealthyChef/Images/body.jpg" />--%>
                <h4 style="text-align: center;">&nbsp;</h4>
                <h4 style="text-align: center;">&nbsp;</h4>
                <h2 style="text-align: center;">Thank you for visiting HealthyChefCreations.com </h2>

                <h4 style="text-align: center;">We are currently upgrading our website as part of our routine site maintenance and will be offline for approximately 24-36 hours.</h4>

                <h4 style="text-align: center;">If you would like to place an order while our website is offline, please call us at 866-575-2433 during our normal business hours (8am - 6pm ET, M-F) 
                </h4>
                <h4 style="text-align: center;">and we would be happy to take your order over the phone.</h4>

                <h4 style="text-align: center;">&nbsp;</h4>
                <h4 style="text-align: center;">We appreciate your business and apologize for any inconvenience.</h4>
                <%--<img src="App_Themes/HealthyChef/Images/bg-full-page-bottom.png" />--%>
            </div>
        </div>
    </form>
</body>
</html>
