<%@ Page Language="C#" Theme="WebModules" MasterPageFile="~/Templates/WebModules/Default.master"
    AutoEventWireup="true"
    Inherits="BayshoreSolutions.WebModules.Cms.Localization._Default"
    CodeBehind="Default.aspx.cs"
    ValidateRequest="false"
    Title="Localization: Keyword Tokens" %>

<%@ Register Src="~/WebModules/Localization/Controls/Header.ascx" TagPrefix="uc1" TagName="Header" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <div class="main-content">
        <div class="main-content-inner">
            <div class="col-sm-12">
            <uc1:Header ID="Header1" runat="server" />

            <asp:MultiView ID="MultiView1" runat="server">

                <asp:View ID="listView" runat="server">

                    <asp:Button ID="addButton" runat="server" CssClass="btn btn-info" OnClick="addButton_Click" Text="New Token" />

                    <bss:ExportButton ID="ExportButton" runat="server" CssClass="btn btn-info"
                        Text="Export"
                        OnClick="ExportButton_Click"
                        OutputFileName="keyword_tokens">
                        <bss:BoundField DataField="Token" HeaderText="Token Name" />
                        <bss:BoundField DataField="Value" HeaderText="Token Value" />
                    </bss:ExportButton>

                    <br />
                    <br />

                    <div>
                        <asp:Repeater ID="KeywordTokenNameFirstLettersList" runat="server">
                            <ItemTemplate>
                                <a style='<%# (_selectedFirstLetter == (string)Container.DataItem) ? "font-weight: bold;": string.Empty %>'
                                    href='<%# "Default.aspx?tokenFirstLetter=" + Container.DataItem %>'><%# Container.DataItem %></a>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>

                    <asp:GridView ID="KeywordTokensList" runat="server" AutoGenerateColumns="False"
                        Width="100%"
                        OnRowCommand="KeywordTokensList_RowCommand"
                        AllowPaging="False"
                        SkinID="DetailSkin">
                        <Columns>
                            <asp:TemplateField HeaderText="Token">
                                <ItemTemplate>
                                    <a href='<%# string.Format("Default.aspx?tokenFirstLetter={0}&token={1}", _selectedFirstLetter, Eval("Token")) %>'><%# Eval("Token") %></a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Value" HeaderText="Value" SortExpression="Value" />
                            <asp:TemplateField ShowHeader="False" ItemStyle-Width="5em">
                                <ItemTemplate>
                                    <asp:LinkButton ID="DeleteButton" runat="server"
                                        CausesValidation="False"
                                        CommandName="Delete_"
                                        CommandArgument='<%# Eval("Token") %>'
                                        OnClientClick='<%# string.Format("return confirm(\"Delete the \\\"{0}\\\" token?\");", Eval("Token")) %>'
                                        Text="Delete" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            No tokens found.
                        </EmptyDataTemplate>
                    </asp:GridView>

                </asp:View>

                <asp:View ID="editView" runat="server">

                    <h4><%= string.IsNullOrEmpty(_selectedToken) ? "New Token" : string.Format("Edit Token <em>{0}</em>", _selectedToken)%></h4>

                    <div class="entity_edit">

                        <h4 id="CultureCodeNameHeaderCtl" runat="server"></h4>

                        <div class="field">
                            <asp:Label Text="Token Name" ID="Label4" runat="server"
                                AssociatedControlID="EditTokenValueTextBox" />
                            <%--<div class="help"></div>--%>
                            <div>
                                <asp:TextBox ID="EditTokenTextBox" runat="server" MaxLength="50"
                                    Width="25em" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                    ControlToValidate="EditTokenTextBox"
                                    ErrorMessage="Required"
                                    ValidationGroup="NewEditToken" />
                            </div>
                        </div>

                        <div class="field">
                            <asp:Label Text="Token Value" ID="Label3" runat="server"
                                AssociatedControlID="EditTokenValueTextBox" />
                            <div class="help">This text will appear in place of the token. HTML is allowed.</div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                ControlToValidate="EditTokenValueTextBox"
                                ErrorMessage="Required"
                                ValidationGroup="NewEditToken"
                                Display="Dynamic" />
                            <div>
                                <asp:TextBox ID="EditTokenValueTextBox" runat="server" MaxLength="500"
                                    Width="100%"
                                    Height="10em"
                                    TextMode="MultiLine" />
                            </div>
                        </div>

                        <div class="toolbar p-5">
                            <asp:Button ID="EditSubmitButton" runat="server"
                                CssClass="saveButton btn btn-info"
                                CausesValidation="true"
                                Text="Save"
                                ValidationGroup="NewEditToken"
                                OnClick="EditSubmitButton_Click" />
                            <asp:Button ID="CancelEditButton" runat="server"
                                CssClass="cancelButton btn btn-danger"
                                CausesValidation="false"
                                Text="Cancel"
                                OnClick="CancelEditButton_Click" />
                        </div>

                    </div>

                </asp:View>

            </asp:MultiView>
                </div>
        </div>
    </div>
    <script type="text/javascript">
    $(function () {
        ToggleMenus('system', undefined, undefined);
    });
    </script>

</asp:Content>
