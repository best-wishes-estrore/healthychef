<%@ Page Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true" CodeBehind="PageSecurity.aspx.cs" Inherits="BayshoreSolutions.WebModules.Admin.MyWebsite.PageSecurity" Title="Page Security" Theme="WebModules" %>
<%@ Register Src="../../Security/PageSecurity/PageSecurity.ascx" TagName="PageSecurity"
    TagPrefix="uc1" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <h3>Page Security</h3>
    <uc1:PageSecurity id="PageSecurity1" runat="server">
    </uc1:PageSecurity> 
</asp:Content>
