<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleList.ascx.cs" Inherits="BayshoreSolutions.WebModules.Cms.Controls.ModuleList" %>


<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <%--<tr>
        <td class="TitlebarLeft" style="height: 19px">
            <img id="Img10" runat="server" height="19" src="~/WebModules/Admin/Images/spacer.gif"
                width="7" alt="" /></td>
        <td class="TitlebarMiddle" style="height: 19px; width: 100%;">
            <%# Eval("WebpageNavigationText") %>
            Modules</td>
        <td class="TitlebarRight" style="height: 19px">
            <img id="Img11" runat="server" height="19" src="~/WebModules/Admin/Images/spacer.gif"
                width="7" alt="" /></td>
    </tr>--%>
    <tr>
        <td class="TitlebarBody" colspan="3">
            <asp:GridView ID="ModulesGridView" runat="server" AutoGenerateColumns="False"  CssClass="table  table-bordered table-hover"
                DataKeyNames="ID"
                SkinID="DetailSkin" 
                Width="100%" 
                OnRowCommand="ModulesGridView_RowCommand"
                OnRowDataBound="ModulesGridView_RowDataBound"
                >
                <Columns>
                    <asp:TemplateField HeaderText="Modules">
                        <ItemTemplate>
                            <a id="EditModuleSettingsLink" runat="server"
                                style="background-repeat: no-repeat; background-position: top left; padding-left: 21px;"  
                                href='<%# ((BayshoreSolutions.WebModules.WebModuleInfo)Container.DataItem).GetEditUrl() %>'
                            ><%# Eval("InstanceName") %></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ModuleTypeName" HeaderText="Type" />
                    <asp:BoundField DataField="PlaceHolder" HeaderText="Location" />
                    <asp:TemplateField ItemStyle-Width="20px">
                        <ItemTemplate>
                            <asp:ImageButton ToolTip="Move Up" ID="ImageButton1" runat="server" CommandArgument='<%# Eval("ID") %>'
                                CommandName="SortUp" ImageUrl="~/WebModules/Admin/Images/Icons/Small/UpArrow.gif" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="20px">
                        <ItemTemplate>
                            <asp:ImageButton ToolTip="Move Down" ID="ImageButton2" runat="server" CommandArgument='<%# Eval("ID") %>'
                                CommandName="SortDown" ImageUrl="~/WebModules/Admin/Images/Icons/Small/DownArrow.gif" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="20px">
                        <ItemTemplate>
                            <a href='<%# "ModuleSettings.aspx?moduleID=" + Eval("ID") + "&instanceID=" + _instanceId %>'>
                                <img alt="" runat="server" 
                                    title="Settings"
                                    src="~/WebModules/Admin/Images/Icons/Small/Settings.gif" 
                                    style="border-style:none;"
                            /></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="20px">
                        <ItemTemplate>
                            <asp:ImageButton 
                                ToolTip="Delete Module" 
                                ID="btnDelete" 
                                runat="server" 
                                CommandArgument='<%# Eval("ID") %>'
                                CommandName="DeleteModule" 
                                ImageUrl="~/WebModules/Admin/Images/Icons/Small/Delete.gif" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <div style="padding: 5px; color: #7D7D7D; border: 1px dashed #7D7D7D; font-style: italic; font-size: 12px;">
                        <%--<%# ModulesGridView.Visible = false %>--%>
                        No Modules
                    </div>
                </EmptyDataTemplate>
                <EmptyDataRowStyle BackColor="White" />
            </asp:GridView>
        </td>
    </tr>
</table>
<%--<div style="text-align:right;">
    <asp:HyperLink ID="AddModuleLink" runat="server" Text="Add Module" />
</div>--%>