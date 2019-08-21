<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PagesModulesList.aspx.cs"
    Title="Modules"
    MasterPageFile="~/Templates/WebModules/Default.master"
    StylesheetTheme="WebModules" Theme="WebModules"  
    Inherits="BayshoreSolutions.WebModules.Admin.MyWebsite.PagesModulesList" 
    %>
<%@ Register Src="~/WebModules/Components/WebsitePicker/WebsitePicker.ascx" TagPrefix="uc1" TagName="WebsitePicker" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="Server">
    
    <h3>Modules in <em><asp:Literal ID="WebsiteNameCtl" runat="server" /></em> Website</h3>
    
    <p class="help">
        Following is a list of all modules in the website, with links to the pages
        that contain the modules.
    </p>
    
    <p>
        Show up to 
        <asp:DropDownList ID="MaxModuleInstancesSelect" runat="server"
            RepeatDirection="Horizontal"
            RepeatLayout="Flow"
            AutoPostBack="true" 
            OnSelectedIndexChanged="MaxModuleInstancesSelect_SelectedIndexChanged"
        >
            <asp:ListItem Selected="True" Text="5" Value="5" />
            <asp:ListItem Selected="False" Text="25" Value="25" />
            <asp:ListItem Selected="False" Text="100" Value="100" />
        </asp:DropDownList>
        results per module type.
    </p>
     
    <%--<div style="margin-bottom: 1em;">
        Website: 
        <uc1:WebsitePicker id="WebsitePicker1" runat="server" OnWebsiteSelected="WebsitePicker1_OnWebsiteSelected" />
    </div>--%>

    <%-- a list of all applications in the site. --%>
    <asp:Repeater ID="WebAppsList" runat="server" OnItemDataBound="WebAppsList_ItemDataBound">
        <ItemTemplate>
            <div>
                <strong><%# Eval("Name") %></strong>
                <div>
                    <%-- a list of modules in this application. --%>
                    <asp:Repeater ID="ModulesList" runat="server">
                        <ItemTemplate>
                            <div style="margin-left: 2em;">
                                <strong><%# Eval("Name") %></strong> 
                                <%-- a list of pages that contain this module. --%>
                                <asp:Repeater ID="PagesList" runat="server">
                                    <ItemTemplate>
                                        <div style="margin-left: 2em;">
                                            <a href='<%# ResolveUrl(BayshoreSolutions.WebModules.Cms.Admin.GetMainMenuUrl((int)Eval("InstanceId"))) %>'
                                                ><%# ResolveUrl(Eval("Path").ToString()) %></a> 
                                            <%--<div style="color: Gray;"><%# ResolveUrl(Eval("Path").ToString()) %></div>--%>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <div id="NoPagesMsgCtl" runat="server" class="help">
                                    <em>This module is not used by any pages.</em>
                                </div>
                                <div id="NotShownMsgCtl" runat="server" style="margin-left: 2em;" />
                            </div>
                        </ItemTemplate>
                        <SeparatorTemplate><div>&nbsp;</div></SeparatorTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <hr />
        </ItemTemplate>
    </asp:Repeater>

</asp:Content>
