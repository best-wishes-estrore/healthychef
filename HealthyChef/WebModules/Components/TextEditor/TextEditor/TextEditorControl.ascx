<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TextEditorControl.ascx.cs" Inherits="BayshoreSolutions.Components.TextEditor.TextEditorControl" %>
<%@ Register TagPrefix="FCKeditorV2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>

<div id="fckPanel" runat="server" visible="true">
    <asp:CustomValidator ID="fckRequiredFieldValidator" runat="server" 
        Visible="false" SetFocusOnError="true" ControlToValidate="fckEditor" Text="Required" ErrorMessage="Required" Display="dynamic" 
    	EnableClientScript="true"
        ValidateEmptyText="true"
        OnServerValidate="fckRequiredFieldValidator_ServerValidate" 
        />
    <div>
        <FCKeditorV2:FCKeditor id="fckEditor" runat="server"
            ToolbarCanCollapse="false"
            BasePath="~/WebModules/Components/TextEditor/fckeditor-2.6.6/" 
            SkinPath="skins/office2003/" 
            Height="350"
						ToolbarSet="WebModules"
            />
    </div>
</div>
