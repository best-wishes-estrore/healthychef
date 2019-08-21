<%@ Page Language="C#" Title="Edit Website" MasterPageFile="~/Templates/WebModules/Default.master"
    AutoEventWireup="true"
    CodeBehind="WebsiteEdit.aspx.cs"
    Inherits="BayshoreSolutions.WebModules.Admin.MyWebsite.WebsiteEdit"
    Theme="WebModules"
    StylesheetTheme="WebModules"
    ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="Server">
    <div class="page-header">
        <h1><em>
            <asp:Literal ID="WebsiteNameCtl" runat="server" /></em> Website</h1>
    </div>
    <asp:MultiView ID="uxMultiView" runat="server">
        <asp:View ID="uxView_Edit" runat="server">
            <%--<p>
                <a href="WebsiteEdit.aspx" title="View the website list">Website List</a>
            </p>--%>
            <p id="Msg" runat="server">
            </p>

            <div class="entity_edit m-2">

                <div class="field">
                    <asp:Label ID="NameLabel" CssClass="required" runat="server" AssociatedControlID="Name" Text="Name" />
                    <div>
                        <asp:TextBox ID="Name" runat="server" MaxLength="256" Width="15em" />
                        <asp:RequiredFieldValidator ID="NameRequiredFieldValidator" runat="server" Text="Required" ErrorMessage="Required" ControlToValidate="Name" Display="Dynamic" />
                    </div>
                    <span class="help">Culture-specific</span>
                </div>

                <div class="field">
                    <asp:Label ID="DescriptionLabel" runat="server" AssociatedControlID="Description" Text="Description" />
                    <div>
                        <asp:TextBox ID="Description" runat="server" MaxLength="256" />
                    </div>
                    <span class="help">Culture-specific</span>
                </div>

                <div class="field">
                    <asp:Label ID="DomainExpressionLabel" runat="server" AssociatedControlID="DomainExpression">Domain Expression</asp:Label>
                    <div>
                        <asp:TextBox ID="DomainExpression" runat="server" MaxLength="500" Width="95%" />
                        <%--<asp:RequiredFieldValidator ID="DomainExpressionRequiredFieldValidator" runat="server" Text="Required" ErrorMessage="Required" ControlToValidate="DomainExpression" Display="Dynamic" />--%>
                    </div>
                    <span class="help">Not culture-specific; this affects <strong>all</strong> cultures. Setting this improperly could adversely affect the behavior of your site.</span>
                </div>

                <div class="field">
                    <div class="align-label">
                        <asp:CheckBox ID="IsDefault" runat="server" />
                        <label>Default</label>
                        <%--<asp:RequiredFieldValidator ID="DomainExpressionRequiredFieldValidator" runat="server" Text="Required" ErrorMessage="Required" ControlToValidate="DomainExpression" Display="Dynamic" />--%>
                    </div>
                    <span class="help">The website marked as the default website will be chosen if a domain expression 
                        cannot be matched to any other active website. Only one (1) website may be marked as the default.
                    </span>
                </div>

                <div class="field">
                    <div class="align-label">
                        <asp:CheckBox ID="IsActive" runat="server" />
                        <label>Active</label>
                        <%--<asp:RequiredFieldValidator ID="DomainExpressionRequiredFieldValidator" runat="server" Text="Required" ErrorMessage="Required" ControlToValidate="DomainExpression" Display="Dynamic" />--%>
                    </div>
                    <span class="help">If the website is not marked as active, it will not be shown to public users.
                        At least one (1) website must be active. If no websites are active, the system will default to
                        the first website.
                    </span>
                </div>

                <%--<div class="field">
                    <asp:Label ID="RootNavIdLabel" CssClass="required" runat="server" AssociatedControlID="RootNavId">Root Page</asp:Label>
                    <div>
                        <asp:DropDownList ID="RootNavId" runat="server" />
                        <asp:RequiredFieldValidator ID="RootNavIdRequiredFieldValidator" InitialValue="0" runat="server" Text="Required" ErrorMessage="Required" ControlToValidate="RootNavId" Display="Dynamic" />
                    </div>
                    <span class="help">Not culture-specific; this affects <strong>all</strong> cultures. Setting this improperly could adversely affect the behavior of your site.</span>
                </div>--%>

                <div class="toolbar m-5 p-5">
                    <asp:Button ID="uxItemSaveButton"
                        runat="server"
                        CausesValidation="true"
                        Text="Save"
                        ValidationGroup=""
                        CssClass="saveButton  btn btn-info"
                        OnClick="uxItemSaveButton_Click" />
                    <asp:Button ID="uxItemCancelButton"
                        runat="server"
                        CausesValidation="false"
                        Text="Cancel"
                        CssClass="cancelButton btn btn-danger"
                        OnClick="uxItemCancelButton_Click" />
                </div>

            </div>

        </asp:View>

        <%--
        <asp:View ID="uxView_List" runat="server">
            <p>
                <a href="WebsiteEdit.aspx?SiteId=0" title="Create a new website">New Website</a>
            </p>

            <asp:GridView ID="uxList"
                DataKeyNames="SiteId"
                runat="server" 
                SkinID="DetailSkin"
                AutoGenerateColumns="False" 
                CellPadding="3" 
                AllowPaging="True" 
                AllowSorting="False" 
                Width="100%"
                PageSize="25"
                OnPageIndexChanging="uxList_PageIndexChanging"
                OnPageIndexChanged="uxList_PageIndexChanged"
                OnRowCommand="uxList_OnRowCommand"
                OnRowDataBound="uxList_RowDataBound"
                HeaderStyle-HorizontalAlign="left"
                >
                <Columns>
                    <asp:HyperLinkField DataTextField="Name" HeaderText="Name" DataNavigateUrlFields="SiteId" DataNavigateUrlFormatString="WebsiteEdit.aspx?SiteId={0}" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:TemplateField HeaderText="Default" ItemStyle-Width="5em">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="IsDefault" 
                                Enabled="false"
                                Checked='<%# (int)Eval("SiteId") == BayshoreSolutions.WebModules.Website.Default.SiteId %>'
                            />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="5em">
                        <!-- Delete button (hidden for the special Default website). -->
                        <ItemTemplate>
                            <asp:LinkButton ID="DeleteButton" runat="server" 
                                CommandName="Delete_"
                                CommandArgument='<%# Eval("SiteId") %>'
                                OnClientClick='<%# string.Format("return confirm(\"Delete website \\\"{0}\\\"?\");", Eval("Name")) %>'
                                Text="Delete"
                                Visible='<%# (int)Eval("SiteId") != BayshoreSolutions.WebModules.Website.Default.SiteId %>'
                            />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:View>
        --%>
    </asp:MultiView>
    <script type="text/javascript">
        $(function () {
            ToggleMenus('websites', undefined, undefined);
        });
    </script>
</asp:Content>
