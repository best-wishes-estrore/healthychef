<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Display.ascx.cs" Inherits="BayshoreSolutions.WebModules.Cms.FileManager.FileList.Display" %>

<div id="FolderTreePanel" style="float: left; padding-right: 1em;" runat="server">

    <div><strong>Folders</strong></div>

    <asp:TreeView ID="FolderTree" runat="server" 
        ShowExpandCollapse="true" 
        SelectedNodeStyle-BorderColor="#cbcbcb"
        SelectedNodeStyle-BorderStyle="Dotted"
        SelectedNodeStyle-BorderWidth="1"
        SelectedNodeStyle-HorizontalPadding="2px"
        SelectedNodeStyle-VerticalPadding="0px"
        SelectedNodeStyle-BackColor="#FFFFA5"
        SelectedNodeStyle-Font-Bold="true"
        OnTreeNodeCheckChanged="CategoryTreeView_SelectedNodeChanged"  
        OnSelectedNodeChanged="CategoryTreeView_SelectedNodeChanged" />

</div>

<div>

    <asp:Label ID="FolderName" Font-Bold="true" runat="server"></asp:Label>
    <asp:GridView ID="FileList" runat="server" AutoGenerateColumns="False" ShowHeader="false"
        BorderStyle="None" BorderWidth="0" GridLines="none" >
        <Columns>
            <asp:TemplateField 
                HeaderStyle-Font-Bold="true"
                HeaderStyle-HorizontalAlign="left">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink2" runat="server" 
                        NavigateUrl='<%# GetFullVirtualPath((string)Container.DataItem) %>' 
                        Text='<%# System.IO.Path.GetFileName((string)Container.DataItem) %>'>
                    </asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

</div>

