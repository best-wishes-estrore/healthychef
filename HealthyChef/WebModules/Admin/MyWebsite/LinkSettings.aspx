<%@ Page Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true" Inherits="BayshoreSolutions.WebModules.Admin.MyWebsite.LinkSettings" Title="Edit Link" StylesheetTheme="WebModules" Theme="WebModules" Codebehind="LinkSettings.aspx.cs" %>
<%@ Register Src="../Controls/PathNameEdit.ascx" TagName="PathNameEdit" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">

<h3>Link Settings</h3>

<bss:MessageBox ID="Msg" runat="server" />

<table>

    <tr>
        <td class="label_td">Navigation Text</td>
        <td class="input_td">
            <asp:TextBox ID="TextText" runat="server"  Width="300px" MaxLength="255" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Required" ControlToValidate="TextText" runat="server" />
            <div class="help">
                Culture-dependent.
            </div>
        </td>
    </tr>

    <tr>
        <td class="label_td">Target URL</td>
        <td class="input_td">
            <asp:TextBox ID="TargetUrlText" runat="server"  Width="300px" MaxLength="500" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Required" ControlToValidate="TargetUrlText" runat="server" />
        </td>
    </tr>

    <uc1:PathNameEdit ID="PathNameEditCtl" runat="server" />
    
    <tr>
        <td class="label_td"></td>
        <td class="input_td">
            <asp:CheckBox ID="DisplayLinkInNavCheckBox" runat="server" Text="Visible in navigation" />
        </td>
    </tr>
    
</table>

<div class="entity_edit">

    <div class="toolbar">
        <asp:Button ID="UpdateButton" runat="server" 
            CausesValidation="True" 
            CommandName="Update"
            Text="Save" 
            OnClick="UpdateButton_Click" 
            CssClass="saveButton"
        />
        <asp:Button ID="UpdateCancelButton" runat="server" 
            CausesValidation="False" 
            CommandName="Cancel"
            Text="Cancel" 
            OnClick="UpdateCancelButton_Click" 
            CssClass="cancelButton"
        />

    </div>
    
</div>

</asp:Content>

