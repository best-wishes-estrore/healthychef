<%@ Page Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true" CodeBehind="edit.aspx.cs" Inherits="BayshoreSolutions.WebModules.SiteMapModule.edit" Title="SiteMapModule" Theme="WebModules" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <div style="margin-bottom: .25em;">Site map provider</div> 
    <asp:DropDownList ID="siteMapProviderName" runat="server" />
    <br />
    <br />
    <br />
    <asp:Button ID="SaveButton" runat="server" OnClick="SaveButton_Click" Text="Save" />
    <asp:Button ID="CancelButton" runat="server" OnClick="CancelButton_Click" Text="Cancel" />    
</asp:Content>
