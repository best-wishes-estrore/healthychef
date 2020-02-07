<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebsitePicker.ascx.cs" 
    Inherits="BayshoreSolutions.WebModules.Components.WebsitePicker.WebsitePicker" %>

<asp:DropDownList ID="WebsiteSelect" AutoPostBack="true" runat="server" 
    OnSelectedIndexChanged="WebsiteSelect_SelectedIndexChanged" />
