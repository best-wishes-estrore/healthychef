<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Menu_ListControl.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.Menu_ListControl" %>
<asp:RadioButtonList runat="server" ID="radiobuttonlist1" RepeatLayout="Flow" RepeatDirection="Vertical">
</asp:RadioButtonList>
<asp:PlaceHolder runat="server" ID="norecordsplaceholder" Visible="false"><span>No menus
    found for this date</span> </asp:PlaceHolder>
