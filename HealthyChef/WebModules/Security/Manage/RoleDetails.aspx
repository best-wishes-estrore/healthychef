<%@ Page Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true" CodeBehind="RoleDetails.aspx.cs" Inherits="BayshoreSolutions.WebModules.Security.Manage.RoleDetails" Title="Role Details" Theme="WebModules" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <asp:ObjectDataSource ID="RoleDataSource" runat="server" 
        TypeName="System.Web.Security.Roles" SelectMethod="GetUsersInRole">
        <SelectParameters>
            <asp:QueryStringParameter Name="roleName" QueryStringField="role" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    
    <asp:Label ID="ErrorMessage" style="display: block;" runat="server" ForeColor="red"></asp:Label>
    <asp:Label ID="Label1" runat="server" Font-Bold="true"><%=Request.QueryString["Role"] %></asp:Label>
    
    <asp:GridView ID="GridView1" runat="server" DataSourceID="RoleDataSource" SkinID="DetailSkin"
        Width="100%" AutoGenerateColumns="False" OnRowCommand="GridView1_RowCommand"
        ShowHeader="false"
    >
        <EmptyDataTemplate>
            There are no users in this role.
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <a href='UserEdit.aspx?UserName=<%# Container.DataItem %>'><%# Container.DataItem %></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="5em">
                <ItemTemplate>
                    <asp:LinkButton ID="DeleteButton" runat="server"
                        CommandName="Delete_"
                        CommandArgument="<%# Container.DataItem %>"
                        Text="Delete"
                    />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
