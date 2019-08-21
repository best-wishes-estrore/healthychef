<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MainNavControl.ascx.cs" Inherits="HealthyChef.Templates.HealthyChef.Controls.MainNavControl" %>

<asp:SiteMapDataSource ID="TopNavDataSource" runat="server" ShowStartingNode="false" StartingNodeUrl="~/Default.aspx" SiteMapProvider="WebModulesSiteMapLimitedProvider" />

<script type="text/javascript">
  $(document).ready(function () {
    $('', '#navigation').prependTo('');
  });
</script>
    <script src="../../client/jquery-ui-1.10.2.custom/development-bundle/jquery-1.9.1.js"></script> 
           <script>   
               var jq = $.noConflict();
               jq(document).ready(function () {

                   var menu = jq("#navigation > ul");

                   jq(".navbar-toggle").click(function () {
                       if ($(menu).css("display") == "none") {
                           $(menu).slideDown();
                           $("#main").addClass("mgn0");
                           
                       }
                       else {
                           $(menu).slideUp();
                           $("#main").removeClass("mgn0");
                       }
                   });
               });
    </script>

<div id="navigation">
    <h1 id="logo">
        <a href="/" title="Healthy Chef">Healthy Chef
            <img src="../../../App_Themes/HealthyChef/Images/Healthy-Chef-Logo.png" />
        </a></h1>
       <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1">
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                     </button>
    
<asp:Repeater ID="rptMainNav" OnItemDataBound="rptMainNavItemDataBound" runat="server" EnableViewState="false">
   <%--DataSourceId="TopNavDataSource"--%>
    <HeaderTemplate>
        <ul>
          <li class="last"><a href="/order-now.aspx">Order Now!<em>&nbsp;</em></a></li>
    </HeaderTemplate>
    <ItemTemplate>        
        <li id='<%# Eval("Title") %>'>
            <a id="A1" runat="server" href=<%# Eval("Url") %>><%# Eval("Title") %></a>
            <asp:Repeater ID="rpet_sub_nav" runat="server" DataSource=<%# Eval("ChildNodes") %>  Visible=<%# ((SiteMapNodeCollection)Eval("ChildNodes")).Count > 0 %>>
                <HeaderTemplate>
                    <ul class="drop-down-menu">
                </HeaderTemplate>
                <ItemTemplate> 
                    <li><a id='A1' runat="server" href='<%# Eval("Url") %>'><%# Eval("Title").ToString().Trim() %></a></li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
        </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>

<%--    <ul>
        <li class="last"><a href="/order-now.aspx">Order Now!<em>&nbsp;</em></a></li>
        <li><a href="/meal-programs.aspx">Meal Programs</a></li>
        <li><a href="/our-food.aspx">Our Food</a></li>
        <li><a href="/how-it-works.aspx">How It Works</a></li>
        <li><a href="/about-us.aspx">About</a></li>
    </ul>--%>
</div>


    
