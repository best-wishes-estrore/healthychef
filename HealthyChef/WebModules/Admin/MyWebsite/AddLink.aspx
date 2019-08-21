<%@ Page Language="C#" Theme="WebModules" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true" Inherits="BayshoreSolutions.WebModules.Admin.MyWebsite.AddLink" Title="Add Link" CodeBehind="AddLink.aspx.cs" %>

<%@ Register Src="~/WebModules/Components/PagePicker/PagePicker.ascx" TagName="PagePicker" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="Server">
    <div class="page-header">
        <h1>New Link</h1>
    </div>
    <bss:MessageBox ID="Msg" runat="server" />
    <div class="m-2">
        <h2>Name </h2>
        <asp:TextBox ID="uxTitle" runat="server" /><br />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="uxTitle"
            ErrorMessage="Required" /><br />
        <asp:CheckBox ID="uxShowInNavigation" runat="server" Text="Visible in navigation" /><br />
        <asp:CheckBox ID="uxIsAlias" AutoPostBack="true" CausesValidation="false" runat="server" Text="Internal link (alias)" OnCheckedChanged="uxIsAlias_CheckedChanged" />

        <div class="row-fluid">
            <div class="m-1">
                <asp:MultiView ID="uxView" runat="server">
                    <asp:View runat="server" ID="uxInternalAliasView">
                        <fieldset class="row">
                            <legend>Link (alias) to an internal page</legend>
                            <div>Target Page</div>
                            <div>
                                <uc1:PagePicker ID="PagePicker1" IsRequired="true" runat="server" />
                            </div>
                            <div class="row">
                                <asp:CheckBox ID="uxRedirect" runat="server" Text="Redirect" />
                            </div>
                        </fieldset>
                    </asp:View>
                    <asp:View runat="server" ID="uxExternalLinkView">
                        <fieldset class="row">
                            <legend>Link to a web address</legend>
                            <div>Target URL</div>
                            <div>
                                <asp:TextBox ID="uxAbsoluteUrl" runat="server" Text="http://" Width="35em" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="uxAbsoluteUrl" Display="dynamic"
                                    ErrorMessage="Required" />
                                <%-- regex validator allows absolute URLs or ASP.NET relative URLs (e.g., ~/foo). --%>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="uxAbsoluteUrl" ErrorMessage="Invalid URL" runat="server" Display="Dynamic"
                                    ValidationExpression="((\w+://([\w-]+\.)+[\w-]+)|~)(/[\w- ./?%&=]*)?"></asp:RegularExpressionValidator>
                            </div>
                        </fieldset>
                    </asp:View>
                </asp:MultiView>

                <div class="entity_edit">
                    <div class="toolbar p-5">
                        <asp:Button ID="uxCreateButton" runat="server"
                            OnClick="uxCreateButton_Click"
                            Text="Save"
                            CssClass="saveButton btn btn-info" />
                        <asp:Button ID="uxCancelButton" runat="server"
                            CommandName="Cancel"
                            Text="Cancel"
                            CausesValidation="False"
                            OnClick="uxCancelButton_Click"
                            CssClass="cancelButton btn btn-danger" />
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
</asp:Content>

