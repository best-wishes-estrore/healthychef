<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Breadcrumbs.ascx.cs" Inherits="BayshoreSolutions.WebModules.Templates.WebModules.Controls.Breadcrumbs" %>
<asp:DataList ID="BreadcrumbsList" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
    <ItemTemplate>
        <a id="BreadcrumbLink" 
            href='<%# string.Format("{0}?InstanceId={1}", Page.ResolveUrl("~/WebModules/Admin/MyWebsite/Default.aspx"), Eval("InstanceId")) %>'
            class="BreadcrumbLink btn btn-info" 
            ><%# Eval("Text") %></a>
    </ItemTemplate>
    <FooterTemplate><br /><br /></FooterTemplate>
</asp:DataList>
