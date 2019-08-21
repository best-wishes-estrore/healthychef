<%@ Page Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true" Inherits="BayshoreSolutions.WebModules.Admin.MyWebsite.ModuleDelete" Title="Delete Module" StylesheetTheme="WebModules" Theme="WebModules" CodeBehind="ModuleDelete.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="Server">
    <div class=" m-2">
        <h3>Deleting '<asp:Literal ID="ModuleName" runat="server"></asp:Literal>'...</h3>
        Are you sure you want to delete this module?

        <div class="entity_edit p-5">

            <bss:MessageBox ID="messageBox" runat="server" />

            <div class="toolbar">
                <asp:Button ID="EditDeleteButton" runat="server"
                    CommandName="Delete"
                    Text="Delete"
                    OnClick="EditDeleteButton_Click"
                    CssClass="saveButton btn btn-danger" />
                <asp:Button ID="DeleteCancelButton" runat="server"
                    CausesValidation="False"
                    CommandName="Cancel"
                    Text="Cancel"
                    OnClick="DeleteCancelButton_Click"
                    CssClass="cancelButton btn btn-danger" />
            </div>
        </div>
    </div>

</asp:Content>
