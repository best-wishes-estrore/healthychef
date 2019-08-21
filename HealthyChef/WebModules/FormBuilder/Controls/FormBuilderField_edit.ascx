<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormBuilderField_edit.ascx.cs"
	Inherits="BayshoreSolutions.WebModules.FormBuilder.Controls.FormBuilderField_edit" %>
<%@ Register Src="FormBuilderFieldOption_edit.ascx" TagName="FormBuilderFieldOption_edit"
	TagPrefix="uc1" %>
<bss:MessageBox ID="Msg" runat="server" ForeColor="Green" />
<table class="smallfont" cellpadding="10">
	<tr>
		<td>
			Field Label<br />
			<asp:TextBox ID="FieldName" runat="server" Width="190" MaxLength="256" Font-Bold="true" />
			<asp:RequiredFieldValidator ID="FieldNameRequiredFieldValidator" runat="server" ErrorMessage="<br>Required"
				ControlToValidate="FieldName" Display="Dynamic" />
		</td>
		<td nowrap>
			<br />
			<asp:CheckBox ID="IsFieldRequired" runat="server" Text="Required" ToolTip="Field is required before the form may be submitted." />
		</td>
		<td>
			Display Type<br />
			<asp:DropDownList ID="FieldTypesList" runat="server" />
			<asp:RequiredFieldValidator ID="FieldTypesListRequiredFieldValidator" runat="server"
				InitialValue="" ErrorMessage="<br>Required" ControlToValidate="FieldTypesList"
				Display="Dynamic" />
		</td>
		<td>
			Width (pixels)<br />
			<asp:TextBox ID="FieldWidth" runat="server" Width="50px" /><asp:CompareValidator
				ID="CompareValidator1" runat="server" ErrorMessage="<br>Field width must be integer"
				ControlToValidate="FieldWidth" Operator="DataTypeCheck" Type="Integer" Display="Dynamic" ></asp:CompareValidator>
		</td>
		<td nowrap>
			<asp:ImageButton ID="MoveUpButton" runat="server" ImageUrl="~/WebModules/Admin/Images/Icons/Small/UpArrow.gif"
				ToolTip="Move the field up" OnClick="MoveUpButton_Click" Visible="false" Style="margin-right: 5px;"
				CausesValidation="false" />
			<asp:ImageButton ID="MoveDownButton" runat="server" ImageUrl="~/WebModules/Admin/Images/Icons/Small/DownArrow.gif"
				ToolTip="Move the field down" OnClick="MoveDownButton_Click" Visible="false"
				Style="margin-right: 5px;" CausesValidation="false" />
			<asp:Button ID="FormBuilderField_SaveButton" runat="server" CssClass="saveButton"
				CausesValidation="true" Text="Save" OnClick="FormBuilderField_SaveButton_Click"
				Style="margin-right: 5px;" />
			<asp:Button ID="FormBuilderField_DeleteButton" runat="server" CssClass="cancelButton"
				CausesValidation="false" ToolTip="Delete the field" Text="Delete" OnClientClick="return confirm('If you delete this field, all associated response data will be lost.\nDelete the field?');"
				OnClick="FormBuilderField_DeleteButton_Click" Visible="false" />
		</td>
	</tr>
	<tr>
		<td colspan="5" id="Options_td" runat="server">
			<fieldset style="width:300px;">
				<legend>Options (RadioButton, CheckBox, or DropDownList only)</legend>
				<uc1:FormBuilderFieldOption_edit ID="FormBuilderFieldOption_edit1" runat="server" />
			</fieldset>
		</td>
	</tr>
</table>
