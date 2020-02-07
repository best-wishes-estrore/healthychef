<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Display.ascx.cs" Inherits="BayshoreSolutions.WebModules.SiteMapModule.Display" %>
<asp:TreeView ID="siteMapTree" runat="server" DataSourceID="siteMapDataSource" NodeWrap="true">
</asp:TreeView>
<asp:SiteMapDataSource ID="siteMapDataSource" runat="server" />
