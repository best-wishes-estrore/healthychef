<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageSecurity.ascx.cs" Inherits="BayshoreSolutions.WebModules.Cms.Security.PageSecurity.PageSecurity" %>

<%-- TODO: implement inheritance... --%>
<%--
<strong>
Inherited Security Settings<br />
</strong>
--%>


<div style="margin: 1em 0em;">
    <asp:CheckBox ID="IsPublic" Text="Public" AutoPostBack="true" runat="server" OnCheckedChanged="IsPublic_CheckedChanged" />
</div>

<div style="margin: 1em 0em;">
    <asp:Label ID="HelpText" runat="server" />
</div>

<asp:GridView ID="AclList" runat="server" AutoGenerateColumns="False" 
    DataKeyNames="NavigationId,RoleName" Style="margin: 1em 0em;"
    SkinID="DetailSkin" OnRowDeleting="AclList_RowDeleting" OnDataBound="AclList_DataBound">
    <Columns>
        <asp:BoundField DataField="RoleName" HeaderText="Role" />
        <asp:TemplateField ItemStyle-Width="10em" ItemStyle-HorizontalAlign="center">
            <ItemTemplate>
                <asp:LinkButton ID="AclDeleteButton" runat="server"
                    CommandName="Delete" 
                    Text='<%# (bool)Eval("IsInherited") ? "Inherited" : "Remove" %>'
                    Enabled='<%# !(bool)Eval("IsInherited") %>'
                    ForeColor='<%# (bool)Eval("IsInherited") ? System.Drawing.Color.Gray : System.Drawing.Color.Empty %>'
                ></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

<div style="margin: 1em 0em;">
    Grant access to role 
    <asp:DropDownList ID="RolesDropDown" runat="server" DataSourceID="RolesDataSource" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="RolesDropDown_SelectedIndexChanged">
        <asp:ListItem Selected="True"></asp:ListItem>
    </asp:DropDownList>
</div>

<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="RolesDropDown"
    ErrorMessage="You must select a role." Display="Dynamic" ValidationGroup="AddAccess" />

<div style="display: block; margin: 2em 0em;">
<%--
    <asp:Button ID="SaveButton" runat="server" OnClick="SaveButton_Click" Text="Save"
        ValidationGroup="AddAccess" />
--%>
    <a id="MainMenuLink" runat="server">Return to Main Menu</a>
</div>

<asp:ObjectDataSource ID="RolesDataSource" runat="server"
    SelectMethod="GetAllRoles" 
    TypeName="System.Web.Security.Roles">
</asp:ObjectDataSource>
