<%@ Page Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" Theme="WebModules"
    AutoEventWireup="true" Inherits="BayshoreSolutions.WebModules.Admin.MyWebsite._Default"
    Title="Website" StylesheetTheme="WebModules" CodeBehind="Default.aspx.cs" %>

<%@ Register Src="~/WebModules/Admin/Controls/ModuleList.ascx" TagName="ModuleList" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="Server">
    <div class="main-content">
        <div class="main-content-inner">
            <div class="page-header">
                <h1>
                    <asp:HyperLink ID="WebpageNameLink" runat="server" CssClass="PageTitle" Style="display: block;" />
                    <asp:Label ID="WebpageUrlLabel" runat="server" CssClass="help" />
                </h1>
            </div>
            <div class="row-fluid">
                <div class="col-sm-12">
                    <div class="row-fluid">
                        <div id="PageSettings_div" runat="server" style="padding: 0 0 8px 0; border-bottom: 1px dotted #C0C2C4;">
                            <asp:Repeater ID="PageSettingsList1" runat="server">
                                <ItemTemplate>
                                    <div style="padding: 4px 0px; font-size: 12px;">
                                        <asp:HyperLink ID="Link1"
                                            runat="server"
                                            NavigateUrl='<%# string.Format("{0}.aspx?InstanceId={1}", ((PageSetting)Container.DataItem).Url, this._instanceId) %>'
                                            ToolTip='<%# ((PageSetting)Container.DataItem).ToolTip %>'
                                            Enabled='<%# ((PageSetting)Container.DataItem).Enabled %>'>

                                            <asp:Image ID="Image1" runat="server" CssClass="icon" Style="padding-left: 0px; margin-left: 0px;" ImageUrl='<%# string.Format("~/WebModules/Admin/Images/Icons/Small/{0}.gif", ((PageSetting)Container.DataItem).Type) %>' /><asp:Label
                                                ID="Label1" runat="server" Text='<%# ((PageSetting)Container.DataItem).Text %>' />
                                        </asp:HyperLink></div></ItemTemplate></asp:Repeater></div><div id="PageDetails_div" runat="server" class="m-2">
                            <p>
                                <strong>Title</strong> <br />
                                <asp:Label ID="TitleLabel" runat="server">Test</asp:Label></p><p id="Keywords_p" runat="server">
                                <strong>Keywords</strong> <br />
                                <asp:Label ID="KeywordsLabel" runat="server">Test</asp:Label></p><p id="Description_p" runat="server">
                                <strong>Description</strong> <br />
                                <asp:Label ID="DescriptionLabel" runat="server">Test</asp:Label></p></div></div><div class="row-fluid">
                        <div class="pull-right m-2">
                            <asp:HyperLink ID="AddPageLink" runat="server" Text="New Page"
                                ToolTip="Create a new subpage under the current page." />
                            <asp:HyperLink ID="uxAddLink" runat="server" Text="New Link"
                                ToolTip="Create a new link under the current page."
                                Style="border-left: 1px solid #C0C2C4; padding-left: .5em;" />
                            <asp:HyperLink ID="AddModuleLink" runat="server" Text="New Module"
                                ToolTip="Add a new module to the current page."
                                Style="border-left: 1px solid #C0C2C4; padding-left: .5em;" />
                        </div>

                        <%-- Modules --%>
                        <div id="Modules_div" runat="server">
                            <uc1:ModuleList ID="ModuleList1" runat="server"></uc1:ModuleList>
                        </div>

                        <%-- Subpages --%>
                        <table id="simple-table" class="table  table-bordered table-hover">
                            <tr>
                                <td colspan="3">
                                    <asp:GridView ID="SubpagesGridView" runat="server" AllowPaging="True" PageSize="50" AutoGenerateColumns="False" DataKeyNames="InstanceId" DataSourceID="SubpagesDataSource" SkinID="DetailSkin" Width="100%" OnRowCommand="SubpagesGridView_RowCommand" OnRowDataBound="SubpagesGridView_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <%= _isSystemRoot ? "Websites" : "Subpages" %>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="Link1" runat="server" NavigateUrl='<%# string.Format("?InstanceId={0}", Eval("InstanceId")) %>'>
                                                        <asp:Image ID="IconImage" runat="server" CssClass="icon" /><%# _isSystemRoot ? string.Format("{0} ({1})", ((BayshoreSolutions.WebModules.WebpageInfo)Container.DataItem).Website.Resource.Name, Eval("Text")) : Eval("Text")%>
                                                    </asp:HyperLink></ItemTemplate></asp:TemplateField><asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" Visible="False" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument='<%# Eval("InstanceId") %>' CommandName="SortUp"
                                                        ImageUrl="~/WebModules/Admin/Images/Icons/Small/UpArrow.gif" Visible='<%# Eval("Visible") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="20px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImageButton2" runat="server" CommandArgument='<%# Eval("InstanceId") %>' CommandName="SortDown"
                                                        ImageUrl="~/WebModules/Admin/Images/Icons/Small/DownArrow.gif" Visible='<%# Eval("Visible") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="20px" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <div style="padding: 5px; color: #7D7D7D; border: 1px dashed #7D7D7D; font-style: italic; font-size: 12px;">
                                                No Subpages <%--<%# SubpagesGridView.Visible = false %>--%></div></EmptyDataTemplate><EmptyDataRowStyle BackColor="White" />
                                        <PagerStyle HorizontalAlign="Right" CssClass="color_code" />
                                        <PagerSettings Position="Bottom" Mode="NextPreviousFirstLast"
                                            NextPageText="Next &gt;"
                                            PreviousPageText="&lt; Previous"
                                            LastPageText="Last &gt;&gt;"
                                            FirstPageText="&lt;&lt; First" />
                                    </asp:GridView>
                                    <asp:ObjectDataSource ID="SubpagesDataSource" runat="server" SelectMethod="GetWebpages"
                                        TypeName="HealthyChef.WebPagesCustomModels.WebPagesController">
                                        <SelectParameters>
                                            <asp:QueryStringParameter DefaultValue="1" Name="parentNavigationId" QueryStringField="InstanceId"
                                                Type="Int32" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            ToggleMenus('websites', undefined, undefined);
        });
    </script>
    <style>
        .color_code a {
            color: white;
        }

        .color_code td:first-child {
            border-right: 1px solid #fff;
        }
    </style>
</asp:Content>
