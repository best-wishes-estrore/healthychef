<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Details.aspx.cs" 
    Theme="HealthyChef" MasterPageFile="~/Templates/HealthyChef/SubPageFull.Master"
    Inherits="HealthyChef.WebModules.ShoppingCart.Details" %>

<%@ Register Src="~/WebModules/ShoppingCart/Controls/ProgramsDisplay.ascx" TagPrefix="bss" TagName="ProgramsDisplay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <bss:ProgramsDisplay runat="server" ID="ProgramsDisplay" />
</asp:Content>