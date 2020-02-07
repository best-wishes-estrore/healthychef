<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormBuilderResponse_display.ascx.cs" Inherits="BayshoreSolutions.WebModules.FormBuilder.Controls.FormBuilderResponse_display" %>

<asp:Literal ID="literalStyle" runat="server"></asp:Literal>
<strong class="formPageName"><asp:Literal ID="formtitle" runat="server" /></strong>
<asp:Panel ID="FormPanel" runat="server" CssClass="entity_edit wm_FormBuilder">
    <asp:ValidationSummary ID="FormBuilderResponseValidationSummary" runat="server" 
        EnableClientScript="true"
        ShowMessageBox="true"
        ShowSummary="false"
    />
    
    <asp:Literal id="literalFields" runat="server"></asp:Literal>
    <asp:PlaceHolder ID="FieldInputList" runat="server" />
    
	<div id="buttonDiv" runat="server" visible="false">
		<label>&nbsp;</label>
        <asp:Button ID="FormBuilderResponse_SaveButton" runat="server"
                    CausesValidation="true"
                    Text="Submit"
                    OnClick="FormBuilderResponse_SaveButton_Click" />
    </div>

</asp:Panel>


