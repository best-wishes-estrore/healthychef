<%@ Page Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true" Inherits="BayshoreSolutions.WebModules.Admin.MyWebsite.PagesSharingModule" Title="Pages Sharing Module" StylesheetTheme="WebModules" Theme="WebModules" Codebehind="PagesSharingModule.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">

<h3>
    <asp:Literal ID="literalModuleType" runat="server"></asp:Literal> 
    Module '<asp:Literal ID="literalModuleName" runat="server" ></asp:Literal>' is shared by these pages:
</h3>

<asp:ListView
    id="lvPages"
    runat="server">
    <EmptyDataTemplate>
        <em>There is no data to display</em>
    </EmptyDataTemplate>
    <LayoutTemplate>
        <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
    </LayoutTemplate>
    <ItemTemplate>
        <a href='<%# "Default.aspx?InstanceId=" + Eval("InstanceId").ToString() %>' > <%# Eval("Path") %> </a><br />
    </ItemTemplate>
</asp:ListView>
<br />
<asp:LinkButton ID="btnReturn" runat="server" Text="<< Back" OnClick="btnReturn_Click"></asp:LinkButton>

</asp:Content>

