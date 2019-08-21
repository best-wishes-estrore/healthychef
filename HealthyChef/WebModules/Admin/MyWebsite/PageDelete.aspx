<%@ Page Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true" Inherits="BayshoreSolutions.WebModules.Admin.MyWebsite.PageDelete" Title="Delete" StylesheetTheme="WebModules" Theme="WebModules" Codebehind="PageDelete.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">

<h3>Deleting '<asp:Literal id="PageName" runat="server"></asp:Literal>'...</h3>
Are you sure you want to delete this <%= PageType %>? 
<strong>This will also delete any sub-pages and links under this <%= PageType %>.</strong>

<div class="entity_edit">

    <div class="toolbar">
        <asp:Button ID="EditDeleteButton" runat="server" 
            CommandName="Delete" 
            Text="Delete" 
            OnClick="EditDeleteButton_Click" 
            CssClass="saveButton"
        />
        <asp:Button ID="DeleteCancelButton" runat="server" 
            CausesValidation="False" 
            CommandName="Cancel"
            Text="Cancel" 
            OnClick="DeleteCancelButton_Click" 
            CssClass="cancelButton"
        />
    </div>

</div>

</asp:Content>

