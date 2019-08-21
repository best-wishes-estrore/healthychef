<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormBuilderFieldInput_display.ascx.cs" Inherits="BayshoreSolutions.WebModules.FormBuilder.Controls.FormBuilderFieldInput_display" %>

<div id="fieldDiv" runat="server">
    <asp:Literal ID="litPlaceHolder" runat="server" />
    <asp:Label ID="FieldLabel" runat="server" Font-Bold="true" />
	<asp:PlaceHolder ID="FieldInputPlaceHolder" runat="server" />
</div>
