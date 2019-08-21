<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PagePicker.ascx.cs" Inherits="BayshoreSolutions.WebModules.Components.PagePicker.PagePicker" %>

<style type="text/css">
    .TreeView
    {
        width:300px;
        height:300px;
        overflow:scroll;
        background-color:white;
        border:1px #7F9DB9 solid;
        position:absolute;
    }
    .CoverFrame
    {
        position:absolute;
        height:303px;
        width:303px;
    }
    .NodeStyle
    {
        border:1px white solid;
        padding-left:1px;
        padding-right:1px;
        
        font-family: verdana;
        font-size:8pt;
        color:black;
    }
    .HoverNodeStyle
    {
        background-color:skyblue;
        border:1px blue solid;
        color:white;
    }
</style>

<asp:TextBox ID="WebpageNameTextBox" runat="server" ReadOnly="True" Width="163px" />

<asp:HyperLink ID="ChooseLink" runat="server" 
    Text="Browse..." 
    style="white-space: nowrap;" 
/>&nbsp;|&nbsp;<asp:LinkButton ID="ClearButton" runat="server" 
    CausesValidation="false" 
    Text="Clear"
    OnClick="ClearButton_Click" 
    style="white-space: nowrap;" 
/>

<asp:RequiredFieldValidator ID="uxRequiredFieldValidator" runat="server" Visible="false" 
    ControlToValidate="WebpageNameTextBox" Text="Required" ErrorMessage="Required" Display="dynamic" 
/>

<div runat="server" id="treeDiv" style="display: none;">
    <iframe id="CoverFrame" class="CoverFrame" frameborder="0"></iframe>
    <asp:TreeView ID="TreeView1" runat="server" DataSourceID="SiteMapDataSource1" 
        OnSelectedNodeChanged="TreeView1_SelectedNodeChanged"
        OnTreeNodeDataBound="TreeView1_TreeNodeDataBound" ExpandDepth="1" ShowLines="True"
        PopulateNodesFromClient="False" CssClass="TreeView">
        <HoverNodeStyle CssClass="HoverNodeStyle" />
        <NodeStyle CssClass="NodeStyle" ImageUrl="~/WebModules/Components/PagePicker/Images/Icon_Webpage_16x16.gif" />
        <SelectedNodeStyle CssClass="HoverNodeStyle" />
        <RootNodeStyle ImageUrl="~/WebModules/Components/PagePicker/Images/Icon_Home_16x16.gif" />
    </asp:TreeView>
</div>

<asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server" 
    SiteMapProvider="WebModulesSiteMapProviderHack" 
/>
