<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormBuilderFieldOption_edit.ascx.cs" Inherits="BayshoreSolutions.WebModules.FormBuilder.Controls.FormBuilderFieldOption_edit" %>

<div class="field">
    Label
    <asp:TextBox ID="OptionName" runat="server" Width="200" />
    <asp:Button ID="OptionAdd_Button" runat="server" 
        Text="Add" 
        onclick="OptionAdd_Button_Click" 
        CausesValidation="true"
    />
    <asp:RequiredFieldValidator ID="OptionsListRfv1" runat="server"
        ControlToValidate="OptionName"
        Text="Required"
        ErrorMessage="Option name is required."
        Display="Static"
    />
</div>

<table>
    <asp:Repeater ID="OptionsList" runat="server" 
        onitemcommand="OptionsList_ItemCommand">
        <ItemTemplate>
            <tr>
                <td><%# Container.DataItem %></td>
                <td>
                    <asp:Button ID="DeleteButton" runat="server" 
                        CommandName="Delete"
                        CommandArgument="<%# Container.DataItem %>"
                        Text="Remove"
                        ToolTip="Remove this option"
                        CausesValidation="false"
                    />
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>

