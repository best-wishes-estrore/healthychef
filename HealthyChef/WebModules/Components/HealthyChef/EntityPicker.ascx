<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EntityPicker.ascx.cs"
    Inherits="HealthyChef.WebModules.Components.HealthyChef.EntityPicker" %>
<style type="text/css">
    .retired
    {
        background-color: #ff6a00;
    }

    .retired_child
    {
        background-color: #ffd800;
    }

    .active
    {
        background-color: #fff;
    }
</style>

<fieldset>
    <legend>
        <asp:Label ID="lblTitle" runat="server" />
    </legend>
    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Always">
        <ContentTemplate>
            <table>
                <tr>
                    <td>Available Items
                    </td>
                    <td>&nbsp;
                    </td>
                    <td>Selected Items
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ListBox runat="server" ID="lstAvailableItems" Font-Size="10px" Width="300px"
                            SelectionMode="Multiple" Rows="12" />
                    </td>
                    <td style="height: 100%; vertical-align: middle; width: 50px;">
                        <span style="display: inline-block; margin: 0 10px 0 10px">
                            <asp:Button CausesValidation="false" runat="server" Font-Bold="true" Font-Size="12px"
                                Width="90%" ID="btnDeselectAll" Text="&lt;&lt;" ToolTip="Deselect All Items"
                                CommandName="DeselectAll" Style="margin-top: 7px;" />
                            <asp:Button CausesValidation="false" runat="server" Font-Bold="true" Font-Size="12px"
                                Width="90%" ID="btnDeselctSelected" Text="&lt;" ToolTip="Deselect Selected Items"
                                CommandName="DeselectSelected" Style="margin-top: 7px;" />
                            <asp:Button CausesValidation="false" runat="server" Font-Bold="true" Font-Size="12px"
                                Width="90%" ID="btnSelectSelected" Text="&gt;" ToolTip="Selected Selected Items"
                                CommandName="SelectSelected" Style="margin-top: 7px;" />
                            <asp:Button CausesValidation="false" runat="server" Font-Bold="true" Font-Size="12px"
                                Width="90%" ID="btnSelectAll" Text="&gt;&gt;" ToolTip="Select All Items" CommandName="SelectAll"
                                Style="margin-top: 7px;" />
                        </span>
                    </td>
                    <td>
                        <asp:ListBox runat="server" ID="lstSelectedItems" Font-Size="10px" Width="300px"
                            SelectionMode="Multiple" Rows="12" />
                    </td>
                    <td width="200px">
                        <div style="display: inline-block; height: 15px; width: 15px; background-color: #ff6a00;margin:0px 10px" />
                        <div style="display: inline-block; width: 185px;margin-left:18px;">- Items highlighted in orange are retired.</div>
                        <div style="display: inline-block; height: 15px; width: 15px; background-color: #ffd800;margin:0px" />
                        <div style="display: inline-block; width: 185px;margin-left:18px;">- Items highlighted in yellow contain components that are retired.</div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</fieldset>
