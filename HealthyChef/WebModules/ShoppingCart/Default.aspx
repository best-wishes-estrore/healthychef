<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true" Theme="WebModules" CodeBehind="Default.aspx.cs" Inherits="HealthyChef.WebModules.ShoppingCart.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <h3>Store Management</h3>
    <div>
        <asp:Menu ID="Menu1" runat="server" Orientation="Vertical" DataSourceID="HealthyChefAdminSiteMap"
            DynamicEnableDefaultPopOutImage="False" StaticEnableDefaultPopOutImage="False" StaticDisplayLevels="2">          
            <DynamicMenuItemStyle  HorizontalPadding="10px" VerticalPadding="5px" />
            <DynamicSelectedStyle HorizontalPadding="10px" VerticalPadding="5px" />
       </asp:Menu>
        <asp:SiteMapDataSource ID="HealthyChefAdminSiteMap" runat="server" SiteMapProvider="HealthyChefAdmin_XmlSiteMapProvider"
            ShowStartingNode="false" StartingNodeUrl ="~/WebModules/ShoppingCart/" />
    </div>
</asp:Content>
