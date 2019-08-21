<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubNavControl.ascx.cs" Inherits="HealthyChef.Templates.HealthyChef.Controls.SubNavControl" %>

<asp:SiteMapDataSource ID="SubNavDataSource"
    runat="server" ShowStartingNode="false"
    SiteMapProvider="WebModulesSiteMapProvider"
    StartFromCurrentNode="true" />
    
<asp:Repeater ID="SubNavRepeater" runat="server"
    DataSourceID="SubNavDataSource">
    <%-- IsInBranch() determines whether the requested page is at or under the 
         current node; if it is, then the class is set to "on". 
         You can modify this as needed for your project. --%>
    <ItemTemplate> 
        <div class='<%# BayshoreSolutions.Common.Web.Url.IsInBranch(Request.Url.ToString(), Eval("Url").ToString()) ? "on" : string.Empty %>'
            ><a id="A1" runat="server" href='<%# Eval("Url") %>'
                ><span><%# Eval("Title") %></span></a></div>
    </ItemTemplate>
</asp:Repeater>
