<%@ Page Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" StylesheetTheme="WebModules"
    Theme="WebModules" AutoEventWireup="true" Inherits="BayshoreSolutions.WebModules.Admin.Security.UserList"
    Title="Security" Codebehind="UserList.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="Server">
    <div style="float: right; margin-bottom: 1em;">
        <asp:Button ID="AddButton" runat="server" OnClick="AddButton_Click" Text="New User" />
    </div>
    <h3>Users</h3>
    <asp:Label ID="ErrorMessage" runat="server" ForeColor="Red"></asp:Label>
    <asp:GridView ID="GridView1" runat="server" 
        DataSourceID="UsersDataSource"
        AutoGenerateColumns="False" 
        DataKeyNames="UserName" 
        SkinID="DetailSkin" 
        Width="100%"
        OnRowCommand="GridView1_RowCommand" 

        AllowPaging="true"
        PageSize="50"
        >
        <Columns>
            <asp:HyperLinkField HeaderText="User" DataNavigateUrlFields="UserName" DataTextField="UserName" DataNavigateUrlFormatString="UserEdit.aspx?UserName={0}" Text="[blank]" />
            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="email" />
            <asp:TemplateField>
                <HeaderTemplate>Online</HeaderTemplate>
                <ItemTemplate><%# ((bool)Eval("IsOnline")) ? "Yes" : string.Empty %></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>Approved/Active</HeaderTemplate>
                <ItemTemplate><%# ((bool)Eval("IsApproved")) ? "Yes" : string.Empty %></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="DeleteButton" runat="server" 
                        CommandName="Delete_"
                        CommandArgument='<%# Eval("UserName") %>'
                        OnClientClick='<%# string.Format("return confirm(\"Delete user \\\"{0}\\\"?\");", Eval("UserName")) %>'
                        Text="Delete"
                    />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerStyle HorizontalAlign="Right" />
        <PagerSettings Position="Bottom" 
            Mode="NextPreviousFirstLast" 
            NextPageText="Next &gt;" 
            PreviousPageText="&lt; Previous" 
            LastPageText="Last &gt;&gt;"
            FirstPageText="&lt;&lt; First" />
    </asp:GridView>


    <asp:ObjectDataSource ID="UsersDataSource" runat="server" 
        TypeName="BayshoreSolutions.WebModules.Admin.Security.UserList"
        EnablePaging="true"
        SelectMethod="GetAllUsers" 
        SelectCountMethod="GetAllUsersCount"
        >
    </asp:ObjectDataSource>
    
</asp:Content>
