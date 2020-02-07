<%@ Page Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="BayshoreSolutions.WebModules.Cms.FileManager.FileList.Edit" Title="FileList" Theme="WebModules" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">

    <h3 id="HeaderCtl" runat="server"></h3>
    
    <Bss:MessageBox ID="Msg" runat="server" />
    
    <div class="entity_edit">
    
        <div class="field">
            Start-from folder
            <div>
                <asp:TextBox ID="RootPath" Width="30em" runat="server" />
            </div>
            <span class="help" runat="server">
                Path to the folder whose contents will be listed, relative to the main file storage path. 
                To show all files, leave this blank.
            </span>
        </div>

        <div class="field">
            <div>
                <asp:CheckBox ID="ShowFolderList" runat="server" Text="Show folder list" />
            </div>
            <span id="Span1" class="help" runat="server">
                Shows a tree of folders which may be navigated by the user.
            </span>
        </div>

        <div class="toolbar">
           <asp:Button ID="SaveButton" CssClass="saveButton" runat="server" Text="Save" OnClick="SaveButton_Click" />
           <asp:Button ID="CancelButton" CssClass="cancelButton" runat="server" OnClick="CancelButton_Click" Text="Cancel" />
        </div>

    </div>
    
</asp:Content>
