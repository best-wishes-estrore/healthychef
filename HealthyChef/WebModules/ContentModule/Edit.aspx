<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs"
    Title="Content Module"
    Inherits="BayshoreSolutions.WebModules.ContentModule.Edit"
    Theme="WebModules"
    MasterPageFile="~/Templates/WebModules/Default.master"
    ValidateRequest="false" %>

<%@ Register Src="~/WebModules/Components/TextEditor/TextEditor/TextEditorControl.ascx" TagName="TextEditor" TagPrefix="uc1" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Body">

    <div style="margin-top: 1em;">
        <uc1:TextEditor ID="ContentTextEditor" runat="server"
            BeforeUnloadWarning="true"
            Height="500px" />
    </div>

    <div style="margin-top: 1em;">
        <div id="ModifiedDatePanel" runat="server"
            style="float: left; font-size: smaller;"
            class="help">
            Last modified on
            <asp:Label runat="server" ID="ContentModifiedDate" />
        </div>

        <div style="text-align: right;">
            <asp:LinkButton ID="PreviewPendingButton" runat="server"
                OnClientClick="wm_textEditor_disableBeforeUnloadWarning();"
                OnClick="PreviewPendingButton_Click"
                Text="Preview"
                ToolTip="View current changes without publishing"
                Style="margin: 0 1em 0 0;" />
        </div>
    </div>

    <div style="clear: both; margin: 1em 0;">
        <div id="HistoryPanel" runat="server"
            style="float: left;">
            Version 
            <asp:DropDownList runat="server" ID="ContentVersions"
                OnSelectedIndexChanged="ContentVersions_Change"
                AutoPostBack="true" />
            <asp:Button runat="server" ID="RestoreArchived" Text="Restore Content"
                OnClientClick="wm_textEditor_disableBeforeUnloadWarning();"
                OnClick="Restore_Click"
                ToolTip="Restore the content from the selected date" />
        </div>

        <div style="text-align: right;">
            <asp:Button runat="server" ID="SaveAndPublish" Text="Save & Publish" Font-Bold="true"
                OnClientClick="wm_textEditor_disableBeforeUnloadWarning();"
                OnClick="SaveAndPublish_Click"
                ToolTip="Save your changes and display to public users" />
            <asp:Button runat="server" ID="Propose" Text="Propose Content"
                OnClientClick="wm_textEditor_disableBeforeUnloadWarning();"
                OnClick="Propose_Click"
                ToolTip="Submit your changes for review" />
            <asp:Button runat="server" ID="Cancel" Text="Cancel" OnClick="Cancel_Click"
                ToolTip="Discard your changes" />
        </div>
    </div>
    <script type="text/javascript">
    $(function () {
        ToggleMenus('websites', undefined, undefined);
    });
    </script>
</asp:Content>
