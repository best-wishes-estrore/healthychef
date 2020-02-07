<%@ Page Language="C#" Theme="WebModules" AutoEventWireup="true"
    MasterPageFile="~/Templates/WebModules/Default.master"
    CodeBehind="CultureCode_edit.aspx.cs"
    Inherits="BayshoreSolutions.WebModules.Cms.Localization.CultureCode_edit"
    Title="Localization: Culture Codes" %>

<%@ Register Src="~/WebModules/Localization/Controls/Header.ascx" TagPrefix="uc1" TagName="Header" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">

    <uc1:Header ID="Header1" runat="server" />

    <bss:MessageBox ID="Msg" runat="server" />

    <asp:MultiView ID="MultiViewCtl" runat="server">
        <asp:View ID="View_Edit_CultureCode" runat="server">

            <div class="entity_edit m-2 p-5">

                <h4 id="CultureCodeNameHeaderCtl" runat="server"></h4>

                <div id="NewCultureCodeNamePanel" class="field" runat="server">
                    <asp:Label Text="Culture Code" ID="Label1" runat="server" AssociatedControlID="CultureCodeCtl" />
                    <div class="help">Example: en-us</div>
                    <div>
                        <asp:TextBox ID="CultureCodeCtl" runat="server" MaxLength="10" />
                        <asp:RequiredFieldValidator ID="CultureCodeCtl_RequiredFieldValidator" runat="server"
                            Text="Required"
                            ControlToValidate="CultureCodeCtl" />
                    </div>
                </div>

                <div class="field">
                    <div>
                        <span id="IsDefaultMsg" runat="server">This is the <strong>system default</strong> culture.
                        </span>
                        <span id="IsNotDefaultMsg" runat="server">This culture code is not the system default. 
                        <asp:LinkButton ID="SetSystemDefaultButton" runat="server"
                            Style="white-space: nowrap;"
                            Text="Set System Default"
                            OnClick="SetSystemDefaultButton_Click" />
                        </span>
                    </div>
                </div>

                <div class="field">
                    <div>
                        <asp:CheckBox ID="IsActiveCtl" runat="server"
                            Text="Active"
                            Checked="true" />
                    </div>
                    <div class="help">Check 'Active' to enable this culture. If the culture is not active, it will be ignored by the system.</div>
                </div>

                <div class="field">
                    <asp:Label Text="Alias To" ID="AliasToCultureCodeLabel" runat="server" AssociatedControlID="AliasToCultureCodeCtl" />
                    <%--<div class="help">Help text</div>--%>

                    <asp:DropDownList ID="AliasToCultureCodeCtl" runat="server" />
                </div>

                <div class="toolbar">
                    <asp:Button ID="CultureCode_SaveButton" runat="server"
                        CssClass="saveButton btn btn-info"
                        CausesValidation="true"
                        Text="Save"
                        ValidationGroup=""
                        OnClick="CultureCode_SaveButton_Click" />
                    <asp:Button ID="CultureCode_CancelButton" runat="server"
                        CssClass="cancelButton btn btn-danger"
                        CausesValidation="false"
                        Text="Cancel"
                        OnClick="CultureCode_CancelButton_Click" />
                </div>

            </div>

        </asp:View>

        <asp:View ID="View_List_CultureCode" runat="server">

            <div class="m-2">
                <a href='<%= Request.FilePath + "?New_CultureCode=true" %>' title="Create a new CultureCode">New Culture</a>
            </div>
            <div class="m-2 p-5">
                <asp:GridView ID="CultureCode_List"
                    DataKeyNames="CultureCode"
                    runat="server"
                    AutoGenerateColumns="False"
                    CellPadding="1"
                    AllowSorting="False"
                    AllowPaging="false"
                    OnRowDeleting="CultureCode_List_OnRowDeleting"
                    OnRowCommand="CultureCode_List_OnRowCommand"
                    OnRowDataBound="CultureCode_List_RowDataBound"
                    AlternatingRowStyle-BackColor="#ebebeb"
                    CssClass="table table-bordered table-hover">
                    <Columns>
                        <asp:TemplateField HeaderText="Culture Code" ItemStyle-Width="8em">
                            <ItemTemplate>
                                <a href='?CultureCode=<%# Eval("CultureCode") %>'><%# Eval("CultureCode")%></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="1.5em" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="DeleteButton" CommandName="Delete" CommandArgument='<%# Eval("CultureCode") %>' OnClientClick="return confirm('Delete the item?')" ToolTip="Delete" runat="server" CausesValidation="false">X</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Native Name" ItemStyle-Width="18em">
                            <ItemTemplate>
                                <%# new System.Globalization.CultureInfo((string)Eval("CultureCode")).NativeName %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="AliasToCultureCode" ItemStyle-Width="8em" HeaderText="Alias To" SortExpression="AliasToCultureCode" />
                        <asp:CheckBoxField DataField="IsDefault" ItemStyle-Width="5em" ItemStyle-HorizontalAlign="center" HeaderText="Default" SortExpression="IsDefault" />
                        <asp:CheckBoxField DataField="IsActive" ItemStyle-Width="5em" ItemStyle-HorizontalAlign="center" HeaderText="Active" SortExpression="IsActive" />
                    </Columns>
                </asp:GridView>
            </div>
        </asp:View>
    </asp:MultiView>
    <script type="text/javascript">
        $(function () {
            ToggleMenus('system', undefined, undefined);
        });
    </script>
</asp:Content>
