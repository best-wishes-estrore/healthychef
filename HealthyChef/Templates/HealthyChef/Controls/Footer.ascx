<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Footer.ascx.cs" Inherits="HealthyChef.Templates.HealthyChef.Controls.Footer" %>
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
    <div id="thawteseal" style="text-align: center;" title="Click to Verify - This site chose Thawte SSL for secure e-commerce and confidential communications.">
        <%--<div><a href="http://www.thawte.com/ssl-certificates/" target="_blank" style="color: #0000  00; text-decoration: none; font: bold 10px arial,sans-serif; margin: 0px; padding: 0px;">ABOUT SSL CERTIFICATES</a></div>--%>
    </div>
    <div class="shell">
        <%--<div style="float: left; margin: 10px;"><%= (Page.Request.IsSecureConnection) ? @"<script type=""text/javascript"" src=""https://sealserver.trustwave.com/seal.js?style=normal""></script>" : "" %></div>--%>
        <div style="float: left; margin: 10px;"><%= (Page.Request.IsSecureConnection) ? @"<script type=""text/javascript"" src=""https://cdn.ywxi.net/js/1.js""></script>" : "" %></div>
        <div style="float: left; margin: 10px;"><%= (Page.Request.IsSecureConnection) ? @"<script type=""text/javascript"" src=""https://seal.thawte.com/getthawteseal?host_name=www.healthychefcreations.com&amp;size=S&amp;lang=en""></script>" : "" %></div>
        <div style="float: left; margin: 10px;"><%= (Page.Request.IsSecureConnection) ? @"<!-- (c) 2005, 2015. Authorize.Net is a registered trademark of CyberSource Corporation --> <div class=""AuthorizeNetSeal""> <script type=""text/javascript"" language=""javascript"">var ANS_customer_id=""dd0dd120-d5f4-41cf-b967-0814c7a6315b"";</script> <script type=""text/javascript"" language=""javascript"" src=""//verify.authorize.net/anetseal/seal.js"" ></script> <a href=""http://www.authorize.net/"" id=""AuthorizeNetText"" target=""_blank"">Accept Credit Cards</a> </div>" : "" %></div>
    </div>
    <script type="text/javascript">
        !function () { var a = document.createElement("script"); a.type = "text/javascript", a.async = !0, a.src = "//configusa.veinteractive.com/tags/A08F77AF/E33F/46CC/A80B/924B9E1C4E4D/tag.js"; var b = document.getElementsByTagName("head")[0]; if (b) b.appendChild(a, b); else { var b = document.getElementsByTagName("script")[0]; b.parentNode.insertBefore(a, b) } }();
    </script>
</div>
<!--ignore end-->
