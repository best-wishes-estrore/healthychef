<%@ Page Language="C#" Title="System Settings" MasterPageFile="~/Templates/WebModules/Default.master" StylesheetTheme="WebModules" Theme="WebModules" AutoEventWireup="true" Inherits="BayshoreSolutions.WebModules.Admin.WebsiteSettings._Default" CodeBehind="Default.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="Server">

    <bss:MessageBox ID="Msg" runat="server" />
    <div class="main-content">
        <div class="main-content-inner">
            <div class="page-header">
                <h1>System Settings</h1>
            </div>
            <div class="row-col">
                <asp:DataList ID="ApplicationsList" runat="server" RepeatColumns="2" RepeatLayout="Table" RepeatDirection="vertical" ItemStyle-VerticalAlign="Top" OnItemDataBound="ApplicationsList_ItemDataBound">
                    <ItemTemplate>
                        <div style="float: left; padding-bottom: .5em; width: 350px;">
                            <div style="vertical-align: top;">
                                <asp:Image ID="Icon" runat="server"
                                    Style="padding: 0 2px 0 0; position: relative; top: 4px;"
                                    ImageUrl='<%# Eval("Icon") %>' />

                                <a href='<%# ResolveUrl((string)Eval("Path")) %>'><%# Eval("Name") %></a>
                            </div>
                            <div style="padding-left: 22px; font-size: 85%;">
                                <asp:Label ID="DescriptionLabel" runat="server" Text='<%# Eval("Description") %>'></asp:Label>

                                <asp:Repeater ID="ModulesList" runat="server">
                                    <HeaderTemplate>
                                        <div>
                                        Modules:
                                    </HeaderTemplate>
                                    <ItemTemplate><span style="color: #5A5A5A;"><%# Eval("Name") %></span></ItemTemplate>
                                    <SeparatorTemplate>, </SeparatorTemplate>
                                    <FooterTemplate></div></FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
            </div>
            <hr />

            <div class="col-sm-12">
                <p>
                    <asp:LinkButton ID="ClearCacheButton" runat="server"
                        Style="font-size: 85%;"
                        Text="Reset Cache"
                        ToolTip="Clear the pages, modules, and navigation caches."
                        OnClick="ClearCacheButton_Click" />
                </p>
                <p>
                    <asp:LinkButton ID="ToggleAdminButtons" runat="server"
                        Style="font-size: 85%;"
                        Text="Enable/Disable Admin Buttons"
                        OnClick="ToggleAdminButtons_Click" />
                </p>
            </div>
        </div>
    </div>
    <script type="text/javascript">
    $(function () {
        ToggleMenus('system', undefined, undefined);
    });
    </script>
</asp:Content>

