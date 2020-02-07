<%@ Page Title="FormBuilder Module" Language="C#" AutoEventWireup="true" MasterPageFile="~/Templates/WebModules/Default.master"
    CodeBehind="Edit.aspx.cs" Inherits="BayshoreSolutions.WebModules.FormBuilder.Edit"
    Theme="WebModules" ValidateRequest="false" EnableEventValidation="false" %>

<%@ Register Src="~/WebModules/FormBuilder/Controls/FormBuilderField_edit.ascx" TagName="FormBuilderField_edit"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebModules/Components/PagePicker/PagePicker.ascx" TagName="PagePicker"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebModules/Components/TextEditor/TextEditor/TextEditorControl.ascx" TagName="TextEditorControl" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <style>
        .smallfont, .smallfont tr, .smallfont td, .smallfont input, .smallfont select {
            font-size: 10px;
        }
    </style>
    <div class="p-5">
        <div class="pull-right">
            <a id="MainMenuLink" runat="server">Return to Main Menu</a>
        </div>
         <div class="clearfix"></div>
    </div>
   
	<h3>
        <em>
            <asp:Literal ID="ModuleName" runat="server" /></em>
        <asp:Literal ID="ModulTypeName" runat="server" />
        Module</h3>
    <bss:MessageBox ID="Msg" runat="server" />
    <asp:LinkButton ID="btnViewSubmissions" runat="server" OnClick="btnViewSubmissions_Click" Text="View Submissions" />
    <br />
    <br />
    <fieldset style="background-color: #FFF8AF;">
        <legend>Settings</legend>
        <table class="smallfont" cellpadding="5" width="100%">
            <tr>
                <td>Notify Email for New Submissions<br />
                    <div>
                        <asp:TextBox ID="NotifyEmailCtl" runat="server" Width="200" MaxLength="250" />
                    </div>
                </td>
                <td>Confirmation Page<br />
                    <uc1:PagePicker ID="ConfirmationPageIdCtl" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Submit Button Text<br />
                    <div>
                        <asp:TextBox ID="tbSubmitButtonText" runat="server" Width="200" MaxLength="250" />
                    </div>
                </td>
                <td nowrap>
                    <span>Display style:</span><br />
                    <asp:DropDownList ID="StyleDropDown" runat="server">
                        <asp:ListItem Text="Block" Value="block" />
                        <asp:ListItem Text="Inline" Value="inline" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="chkAcknowledgementEnabled" runat="server"
                        OnCheckedChanged="chkAcknowledgementEnabled_CheckedChanged"
                        Text="Send Acknowledgement Email" AutoPostBack="True" />
                </td>
                <td nowrap>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Panel ID="pnlEmailAcknowledgement" runat="server">
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Label ID="labelAckFromEmailAddress" runat="server">From Email Address:</asp:Label><br />
                                    <asp:TextBox ID="txtAcknowledgementFromEmail" runat="server" Width="200" MaxLength="256" />
                                </td>
                                <td>
                                    <asp:Label ID="labelAckEmailAddressField" runat="server">Email Address Field:</asp:Label><br />
                                    <asp:TextBox ID="txtAcknowledgementEmailField" runat="server" Width="200" MaxLength="256" /><br />
                                    <asp:Label ID="labelAckEmailInstructions" runat="server">Choose the exact field label below for the submitter's email address.</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="labelAckSubject" runat="server">Subject: </asp:Label><br />
                                    <asp:TextBox ID="txtAcknowledgementSubject" runat="server" Width="100%" MaxLength="256" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="labelAckBody" runat="server">Body: </asp:Label><br />
                                    <uc2:TextEditorControl ID="txtAcknowledgementBody" runat="server"
                                        ToolbarSet="Default" />
                                    <br />
                                    <asp:CustomValidator
                                        ID="cvAckowledgementBody"
                                        runat="server"
                                        Display="Dynamic"
                                        ErrorMessage="Maximum Body Length is 4000 characters<br />"
                                        OnServerValidate="cvAcknowledgementBody_ServerValidate"
                                        ValidationGroup="FormBuilderModule"></asp:CustomValidator>
                                    <asp:Label ID="labelAckTemplateInstructions" runat="server">To insert a field value in the subject or body, use this token:  ##FIELD NAME## .  Use the exact label text for the field below, plus the double # symbols on each side, to replace the token with the value entered by the submitter.</asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="FormBuilderModule_SaveButton" runat="server" CssClass="saveButton"
                        CausesValidation="true" Text="Save Settings" ValidationGroup="FormBuilderModule" OnClick="FormBuilderModule_SaveButton_Click" />
                </td>
                <td nowrap>&nbsp;</td>
            </tr>
        </table>
    </fieldset>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <h3 style="display: inline">Load A Starter Template</h3>

            <asp:DropDownList
                ID="ddlTemplates"
                runat="server"
                OnDataBound="TemplateList_DataBound"
                DataTextField="Name"
                DataValueField="Id">
            </asp:DropDownList>

            <asp:Button ID="btnLoadTemplate" runat="server" OnClick="LoadTemplate_Click" Text="Load Template" />
            <asp:Button ID="btnDeleteTemplate" runat="server" Text="Delete Template"
                OnClientClick="return confirm('Delete template?');" OnClick="DeleteTemplate_Click" />
            <h3>Build A Custom Form</h3>
            <fieldset style="background-color: #E8F4FD;">
                <legend>Add a form field</legend>
                <uc1:FormBuilderField_edit ID="FormBuilderField_edit1" runat="server" />
            </fieldset>
            <h3>My Form</h3>
            <asp:Repeater ID="FieldsList" runat="server" OnItemDataBound="FieldsList_ItemDataBound">
                <ItemTemplate>
                    <fieldset style="background-color: #EFEFEF; padding: 0px;">
                        <uc1:FormBuilderField_edit ID="FormBuilderField_edit2" runat="server" />
                    </fieldset>
                    <br />
                </ItemTemplate>
            </asp:Repeater>
            <table width="100%">
                <tr>
                    <td align="center">Save as starter template
						<asp:TextBox runat="server" ID="tbTemplateName" MaxLength="50" />
                        <asp:Button ID="btnSaveTemplate" runat="server" OnClick="SaveTemplate_Click" Text="Save Template" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
