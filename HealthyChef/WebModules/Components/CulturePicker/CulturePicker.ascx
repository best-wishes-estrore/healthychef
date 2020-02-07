<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CulturePicker.ascx.cs" 
    Inherits="BayshoreSolutions.WebModules.Components.CulturePicker.CulturePicker" %>

<asp:Label ID="BadLanguage" runat="server" ForeColor="Red" 
    Text="<strong>Invalid culture code;</strong> please select a language before editing content"/>

<asp:DropDownList ID="CultureSelect" runat="server" 
    AutoPostBack="true" 
    OnSelectedIndexChanged="CultureSelect_SelectedIndexChanged" 
    style="margin-right: 0px;"
    />

<%--
<asp:Repeater ID="CultureLinks" runat="server"
    OnItemCommand="CultureLinks_ItemCommand">
    <ItemTemplate>
        <asp:LinkButton ID="LinkButton1" runat="server" 
            Font-Bold='<%# Eval("Name").ToString().ToLower() == System.Threading.Thread.CurrentThread.CurrentCulture.ToString().ToLower() %>' 
            CommandArgument='<%# Eval("Name") %>' 
            CommandName="Select" 
            CausesValidation="false"
        ><%# System.Globalization.CultureInfo.GetCultureInfo((string)Eval("Name")).NativeName %></asp:LinkButton>
    </ItemTemplate>
    <SeparatorTemplate> | </SeparatorTemplate>
</asp:Repeater>
--%>