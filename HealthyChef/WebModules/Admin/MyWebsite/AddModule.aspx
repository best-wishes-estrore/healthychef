<%@ Page Language="C#" Theme="WebModules" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true" Inherits="BayshoreSolutions.WebModules.Admin.MyWebsite.AddModule" Title="Add Module" CodeBehind="AddModule.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="Server">

    <div class="entity_edit table_align">
        <div class="page-header">
            <h1>Add Module to Page</h1>
        </div>
        <table class="m-2">
            <tr>
                <td class="label_td">Module
                <div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ModuleList"
                        ErrorMessage="Required" />
                </div>
                </td>
                <td class="input_td">
                    <asp:Label ID="NoModules" runat="server" Text="There are no modules loaded.  Please click Skip " ForeColor="Red" Visible="false" />
                    <asp:RadioButtonList ID="ModuleList" runat="server"
                        AutoPostBack="true" OnSelectedIndexChanged="ModuleList_SelectedIndexChanged" />
                </td>
            </tr>
            <tr>
                <td class="label_td"></td>
                <td class="input_td">
                    <asp:CheckBox ID="UseExistingModule" runat="server"
                        AutoPostBack="true"
                        OnCheckedChanged="UseExistingModule_CheckedChanged"
                        Text="Share an existing module of the selected type" />
                </td>
            </tr>
            <asp:PlaceHolder ID="ExistingModulePanel" runat="server"
                Visible="false">
                <tr>
                    <td class="label_td">Shared Module</td>
                    <td class="input_td">
                        <asp:DropDownList ID="ExistingModulesPagesSelect" runat="server" />
                        <asp:Label ID="NoExistingModulesFoundCtl" runat="server"
                            CssClass="help"
                            Visible="false" />
                    </td>
                </tr>
            </asp:PlaceHolder>
            <tr>
                <td class="label_td">Placeholder</td>
                <td class="input_td">
                    <asp:DropDownList ID="PlaceholderDropDownList" runat="server" />
                </td>
            </tr>
            <tr></tr>
            <tr>
                <td class="label_td">Name</td>
                <td class="input_td">
                    <asp:TextBox ID="ModuleNameTextBox" runat="server" />
                    <div class="help">If blank, a name will be assigned automatically.</div>
                </td>
            </tr>
        </table>

        <div class="toolbar p-5 m-2">
            <asp:Button ID="SaveButton" runat="server"
                OnClick="SaveButton_Click"
                CssClass="saveButton btn btn-info"
                Text="Save" />
            <asp:Button ID="CancelButton" runat="server"
                OnClick="CancelButton_Click"
                CausesValidation="False"
                CssClass="cancelButton btn btn-danger"
                Text="Cancel" />
        </div>

    </div>
    <script type="text/javascript">
    $(function () {
        ToggleMenus('websites', undefined, undefined);
    });
    </script>
</asp:Content>

