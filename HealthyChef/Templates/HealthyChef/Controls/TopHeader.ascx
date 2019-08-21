<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopHeader.ascx.cs" Inherits="HealthyChef.Templates.HealthyChef.Controls.TopHeader" %>
<!-- header -->
<div id="header">
    <div class="shell">
        <div class="top-navigation">
            <span>
                <img alt="" src="/App_Themes/HealthyChef/images/icon-1.png" />1-866-575-2433</span>
            <a href="/contact_us.aspx">
                <img alt="" src="/App_Themes/HealthyChef/images/icon-2.png" />Email Us</a>
            <a id="aLogin" runat="server" href="/login.aspx">
                <img alt="" src="/App_Themes/HealthyChef/images/gold-star.gif" />Login</a>
            <a id="aProfile" runat="server" visible="false" href="/my-profile.aspx">
                <img alt="" src="/App_Themes/HealthyChef/images/icon-3.png" />Profile</a>
            <a id="aSignOut" runat="server" visible="false" causesvalidation="false">
                <img alt="" src="/App_Themes/HealthyChef/images/gold-star.gif" />Sign Out</a>
            <a id="aCart" runat="server" visible="false" href="/cart.aspx">
                <img alt="" src="/App_Themes/HealthyChef/Images/icon-cart.png" />My Cart (<asp:Literal ID="lblCartCount" runat="server" Text="" />)</a>
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
<script>
    //$('#ctl00_TopHeader1_aSignOut').removeClass("slideDown");
    //if ($("#ctl00_TopHeader1_aCart").hasClass(".cart-has-items")) {

    //    $("#slider").removeClass("slideDown");
    //}
    //else {
    //    $("#slider").addClass("slideDown");
    //}

</script>

