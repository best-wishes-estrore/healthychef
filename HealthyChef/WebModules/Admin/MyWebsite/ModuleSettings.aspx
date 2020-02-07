<%@ Page Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true"
    Inherits="BayshoreSolutions.WebModules.Admin.MyWebsite.ModuleSettings" Title="Module Settings"
    StylesheetTheme="WebModules" Theme="WebModules" CodeBehind="ModuleSettings.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <div class="page-header">
        <h1>Module Settings</h1>
    </div>
    <div class="entity_edit m-2">
        <p>
            <strong id="ModuleTypeName" class="help" runat="server" />:
        <span id="ModuleTypeDescription" class="help" runat="server" />
        </p>

        <div class="field">
            Placeholder
        <div>
            <asp:DropDownList ID="PlaceholderDropDownList" runat="server" />
        </div>
        </div>

        <div class="field">
            Name
        <div>
            <asp:TextBox ID="ModuleNameTextBox" runat="server" MaxLength="150" Width="25em" />
        </div>
        </div>

        <div class="toolbar p-5">
            <asp:Button ID="UpdateButton" runat="server"
                CausesValidation="True"
                Text="Save"
                OnClick="UpdateButton_Click"
                CssClass="saveButton btn btn-info" />
            <asp:Button ID="UpdateCancelButton" runat="server"
                CausesValidation="False"
                Text="Cancel"
                OnClick="UpdateCancelButton_Click"
                CssClass="cancelButton btn btn-danger" />
        </div>

    </div>

    
    <script type="text/javascript">
        $(function () {
            ToggleMenus('websites', undefined, undefined);
        });
    </script>
</asp:Content>
