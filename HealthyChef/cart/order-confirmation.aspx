<%@ Page Language="C#" Theme="HealthyChef" AutoEventWireup="true" CodeBehind="order-confirmation.aspx.cs" Inherits="HealthyChef.Cart.OrderConfirmation" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>Healthy Chef Creations - Order Confirmation</title>
    <link rel="shortcut icon" href="/App_Themes/HealthyChef/images/Healthy-Chef-Logo-leaves.png" />
    <%-- <link rel="stylesheet" href="/client/jquery-ui-1.10.2.custom/css/south-street/jquery-ui-1.10.2.custom.min.css" type="text/css" />
    <script src="/client/jquery-1.9.1.min.js" type="text/javascript"></script>--%>

    <meta name="msvalidate.01" content="36EB1A194C116E88B600C9EB1A332622" />
    <meta name="google-site-verification" content="pEdaIoVNdrxoDXJowruYbab9hy2qMQxLxb2gJJKLaJE" />

    <script type="text/javascript">
        function getParameterByName(name) {
            name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regexS = "[\\?&]" + name + "=([^&#]*)";
            var regex = new RegExp(regexS);
            var results = regex.exec(window.location.search);
            if (results == null)
                return "";
            else
                return decodeURIComponent(results[1].replace(/\+/g, " "));
        }

        var hdn1 = getParameterByName("pn");
        var hdn2 = getParameterByName("tl");
        var hdn3 = getParameterByName("tx");
        var hdn4 = getParameterByName("ts");
        var hdn5 = getParameterByName("ct");
        var hdn6 = getParameterByName("st");
        var hdn7 = getParameterByName("cy");

        var _gaq = _gaq || [];
        _gaq.push(['_setAccount', 'UA-32947650-1']);
        _gaq.push(['_trackPageview']);
        _gaq.push(['_addTrans',
            hdn1, // transaction ID - required
            'Healthy Chef Creations',  // affiliation or store name
            hdn2, // total - required
            hdn3, // tax
            hdn4, // shipping
            hdn5, // city
            hdn6, // state or province
            hdn7, // country
        ]);
        _gaq.push(['_trackTrans']); //submits transaction to the Analytics servers

        (function () {
            var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
            ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
        })();
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"/>
        <%-- <asp:HiddenField ID="hdnPurchaseNum" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnTotal" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnTax" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnShipping" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnCity" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnState" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnCountry" runat="server" ClientIDMode="Static" />--%>
        <img src="//cdsusa.veinteractive.com/DataReceiverService.asmx/Pixel?journeycode=A08F77AF-E33F-46CC-A80B-924B9E1C4E4D" width="1" height="1"/>
        <%--<img id="track_id" src="https://shareasale.com/sale.cfm?amount=AMOUNTOFSALE&tracking=TRACKINGNUMBER&transtype=sale&merchantID=65905" width="1" height="1">--%>
        <iframe id="stage_id_iframe" src="https://stage39.go2cloud.org/aff_l?offer_id=84&amount=AMOUNT" scrolling="no" frameborder="0" width="1" height="1"></iframe>
        <div id="wrapper">
            <%--<uc1:TopHeader runat="server" ID="TopHeader1" />--%>
            <!-- header -->
            <div id="header">
                <div class="shell">
                    <div class="top-navigation">
                        <span>
                            <img alt="" src="/App_Themes/HealthyChef/images/icon-1.png" />1-866-575-2433</span>
                        <a href="/contact_us.aspx">
                            <img alt="" src="/App_Themes/HealthyChef/images/icon-2.png" />Email Us</a>
                        <a id="aCart" runat="server" visible="false" href="/cart.aspx">
                            <img alt="" src="/App_Themes/HealthyChef/Images/icon-cart.png" />My Cart (<asp:Literal ID="lblCartCount" runat="server" Text="" />)</a>
                        <a id="aLogin" runat="server" href="/login.aspx">
                            <img alt="" src="/App_Themes/HealthyChef/images/icon-3.png" />Member Login</a>
                        <a id="aProfile" runat="server" visible="false" href="/my-profile.aspx">
                            <img alt="" src="/App_Themes/HealthyChef/images/icon-3.png" />My Profile</a>
                        <a id="aSignOut" runat="server" visible="false" causesvalidation="false">
                            <img alt="" src="/App_Themes/HealthyChef/images/icon-3.png" />Sign Out</a>
                        <asp:LoginView runat="server" ID="AdminLink">
                            <RoleGroups>
                                <asp:RoleGroup Roles="Administrators, EmployeeProduction, EmployeeService, EmployeeManager">
                                    <ContentTemplate>
                                        <a href="/admin"><span>Admin</span></a>
                                    </ContentTemplate>
                                </asp:RoleGroup>
                            </RoleGroups>
                        </asp:LoginView>
                    </div>
                </div>
            </div>
            <!-- END header-->

            <div class="shell">
                <%--<uc1:Header runat="server" ID="Header1" />--%>
                <!--ignore begin-->
                <div id="navigation">
                    <h1 id="logo">
                        <a href="/" title="Healthy Chef">Healthy Chef
                            <img src="../App_Themes/HealthyChef/Images/Healthy-Chef-Logo.png" />
                        </a>
                    </h1>
                    <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1">
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>

                    <ul>
                        <li class="last"><a href="/order-now.aspx">Order Now!<em>&nbsp;</em></a></li>
                        <li><a href="/meal-programs.aspx">Meal Programs</a></li>
                        <li><a href="/our-food.aspx">Our Food</a></li>
                        <li><a href="/how-it-works.aspx">How It Works</a></li>
                        <li><a href="/about-us.aspx">About</a></li>
                    </ul>
                </div>
                <!--ignore end-->
                <!-- main -->
                <div id="main">
                    <div class="container">
                        <div class="container-bottom">
                            <div class="container-cnt">
                                <!-- Header -->
                                <div class="full-page-header">
                                    <div class="alignleft">
                                        <%--<asp:ContentPlaceHolder ID="HeaderImage" runat="server" />--%>
                                    </div>
                                    <div class="info">
                                        <%--<asp:ContentPlaceHolder ID="HeaderText" runat="server" />--%>
                                    </div>
                                    <div class="cl">&nbsp;</div>
                                </div>
                                <!-- Body Content -->
                                <%--<asp:ContentPlaceHolder ID="body" runat="server" />--%>
                                <asp:Label ID="lblFeedback" runat="server" />
                                <asp:Panel ID="pnlComplete" runat="server">
                                    <div id="printContainer">
                                        <h2 style="text-align: center; font-size: 30px" class="color-orange">Order Confirmation</h2>
                                        <h3>Your purchase is complete.  Thank you for your order!</h3>
                                        <br />
                                        <asp:Label ID="lblCompleteDetail" runat="server" />
                                        <br />
                                        You may view your order at any time at <a href="www.healthychefcreations.com">www.healthychefcreations.com</a>  by logging in and clicking My Profile, My Orders.  
                                        <br />
                                        <br />
                                        An electronic receipt has been emailed to
                                        <asp:Label ID="lblCompleteEmail" runat="server" />.
                                    </div>
                                    <br />
                                    <asp:Button ID="imbPrint" runat="server" ImageUrl="/App_Themes/pullmanholt/Images/btn_print.gif"
                                        OnClientClick="javascript:PrintContent()" Text="Print a copy of your receipt" CssClass="pixels-button-double pixels-button-double-orange" />
                                    &nbsp;
                                     <asp:Button ID="btnContinue" runat="server" Text="Continue Shopping" CausesValidation="false"
                                         CssClass="pixels-button-double pixels-button-double-orange" />
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>
                <%--<uc1:Footer runat="server" ID="Footer1" />--%>
                <asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server" ShowStartingNode="False"
                    SiteMapProvider="WebModulesSiteMapLimitedProvider" />
                <!--ignore begin-->
                <div id="footer">
                    <p class="copy">
                        Copyright <%=DateTime.Today.Year%> Healthy Chef Creations<br />
                        <a href="/terms-and-conditions.aspx">Terms and Conditions</a>
                        &nbsp;&nbsp;&nbsp;&nbsp;
        <a href="/privacy-statement.aspx">Privacy Policy</a>
                    </p>
                    <div class="bottom-navigation">
                        <bss:QuickContent runat="server" ID="FooterContent" ContentName="FooterContent" />
                        <div class="cl">
                            &nbsp;
                        </div>
                    </div>
                </div>
                <!--ignore end-->

            </div>
        </div>
    </form>
    <!-- Google Code for Sale Conversion Page -->
    <script type="text/javascript">
        /* <![CDATA[ */
        var google_conversion_id = 1068905029;
        var google_conversion_language = "en";
        var google_conversion_format = "2";
        var google_conversion_color = "ffffff";
        var google_conversion_label = "H_dkCJ2t1QYQxeTY_QM";
        var google_conversion_value = 0;
        /* ]]> */
    </script>
    <script type="text/javascript" src="//www.googleadservices.com/pagead/conversion.js">
    </script>
    <noscript>
        <div style="display: inline;">
            <img height="1" width="1" style="border-style: none;" alt="" src="//www.googleadservices.com/pagead/conversion/1068905029/?value=0&amp;label=H_dkCJ2t1QYQxeTY_QM&amp;guid=ON&amp;script=0" />
        </div>
    </noscript>
    <script type="text/javascript"> if (!window.mstag) mstag = { loadTag: function () { }, time: (new Date()).getTime() };</script>
    <script id="mstag_tops" type="text/javascript" src="//flex.atdmt.com/mstag/site/9a37952d-1bd2-49a4-a86c-ef3e9367624e/mstag.js"></script>
    <script type="text/javascript">    mstag.loadTag("analytics", { dedup: "1", domainId: "2337722", type: "1", actionid: "125196" })</script>
    <noscript>
        <iframe src="//flex.atdmt.com/mstag/tag/9a37952d-1bd2-49a4-a86c-ef3e9367624e/analytics.html?dedup=1&domainId=2337722&type=1&actionid=125196" frameborder="0" scrolling="no" width="1" height="1" style="visibility: hidden; display: none"></iframe>
        
    </noscript>
    <script type="text/javascript">
        var gaJsHost = (("https:" == document.location.protocol) ? "https://ssl." : "http://www.");
        document.write(unescape("%3Cscript src='" + gaJsHost + "google-analytics.com/ga.js' type='text/javascript'%3E%3C/script%3E"));
    </script>
    <script type="text/javascript">
        function PrintContent() {
            var DocumentContainer = document.getElementById('printContainer');
            var WindowObject = window.open('', "PrintOrderInvoice", "width=740,height=325,top=200,left=250,toolbars=no,scrollbars=yes,status=no,resizable=no");
            WindowObject.document.writeln(DocumentContainer.innerHTML);
            WindowObject.document.close();
            WindowObject.focus();
            WindowObject.print();
            WindowObject.close();
        }
    </script>
    <script src="../../client/jquery-ui-1.10.2.custom/development-bundle/jquery-1.9.1.js"></script>
    <script>
        var jq = $.noConflict();
        jq(document).ready(function () {
            jq(".navbar-toggle").click(function () {
                jq("#navigation > ul").slideToggle();
                
            });
        });
    </script>
    <script>
        function getParameterByNameVal(name) {
            name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regexS = "[\\?&]" + name + "=([^&#]*)";
            var regex = new RegExp(regexS);
            //alert("location path" + window.location.search);
            var results = regex.exec(window.location.search);//location.search: https://healthychefcreations.com/cart/order-confirmation.aspx?pn=1989195&tl=109.9000&tx=0.0000&ts=24.9500&ct=asdf&st=MD&cy= 
            if (results == null)
                return "";
            else
                return decodeURIComponent(results[1].replace(/\+/g, " "));
        }
    </script>
</body> 
</html>


<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
<script>
    $(document).ready(function () {
        var purchaseNum = getParameterByNameVal("pn");//Purchase Number
        var totalAmount = getParameterByNameVal("tl");//Total Amount
        //document.getElementById("track_id").src = "https://shareasale.com/sale.cfm?amount=" + totalAmount + "&tracking=" + purchaseNum + "&transtype=sale&merchantID=65905";
        document.getElementById("stage_id_iframe").src = "https://stage39.go2cloud.org/aff_l?offer_id=84&amount=" + totalAmount;
    });
</script>




