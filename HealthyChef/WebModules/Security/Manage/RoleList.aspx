<%@ Page Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" StylesheetTheme="WebModules"
    Theme="WebModules" AutoEventWireup="true" Inherits="BayshoreSolutions.WebModules.Admin.Security.RoleList"
    Title="Security" Codebehind="RoleList.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="Server">

    <script type="text/C#" runat="server">
        int nrUsersInRole = 0;
    </script>

    <div style="float: right; margin-bottom: 1em;">
        <asp:FormView ID="FormView1" runat="server" DataKeyNames="RoleName" DataSourceID="RolesDataSource"
            DefaultMode="Insert" OnItemInserted="FormView1_ItemInserted">
            <InsertItemTemplate>
                New role 
                <asp:TextBox ID="RoleNameTextBox" runat="server" Text='<%# Bind("RoleName") %>' />
                <asp:Button ID="AddButton" runat="server" CommandName="Insert" Text="Add" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="RoleNameTextBox"
                    ErrorMessage="Required" Display="dynamic" />
            </InsertItemTemplate>
        </asp:FormView>
    </div>
    <h3>Roles</h3>
    
    <asp:GridView ID="GridView1" runat="server" DataSourceID="RolesDataSource"
        AutoGenerateColumns="False" SkinID="DetailSkin" Width="100%" OnRowCommand="GridView1_RowCommand">
        <Columns>
            <asp:TemplateField HeaderText="Role" SortExpression="RoleName">
                <EditItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Container.DataItem %>'></asp:Label>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink1" runat="server" 
                        NavigateUrl='<%# string.Format("RoleDetails.aspx?Role={0}",Container.DataItem) %>'
                        Text='<%# Container.DataItem %>'></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Users">
                <ItemTemplate>
                    <%# nrUsersInRole = System.Web.Security.Roles.GetUsersInRole(Container.DataItem.ToString()).Length%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemStyle HorizontalAlign="right" />
                <ItemTemplate>
                    <%-- Allow delete only if there are no users in the role. --%>
                    <asp:LinkButton ID="deleteButton" 
                        CommandArgument='<%# Container.DataItem %>' 
                        CommandName="Delete_" 
                        Text="Delete"
                        CausesValidation="false"
                        Visible='<%# nrUsersInRole == 0 %>'
                        runat="server"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="RolesDataSource" runat="server" OldValuesParameterFormatString="{0}" 
        TypeName="System.Web.Security.Roles"
        SelectMethod="GetAllRoles" 
        InsertMethod="CreateRole">
        <InsertParameters>
            <asp:Parameter Name="roleName" Type="String" />
        </InsertParameters>
    </asp:ObjectDataSource>
</asp:Content>
