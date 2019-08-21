<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImagePicker.ascx.cs" Inherits="BayshoreSolutions.WebModules.Components.ImagePicker.ImagePicker" %>
<span style="white-space: nowrap;">
    <asp:TextBox ID="txtImageURL" Width="20em" runat="server" />
    <input id="btnSelect" runat="server" type="button" value="Browse..." title="Select" />
    <input id="btnClear" runat="server" type="button" value="Clear" title="Clear" />
</span>
<asp:RequiredFieldValidator ID="uxRequiredFieldValidator" runat="server" Visible="false" SetFocusOnError="true" ControlToValidate="txtImageURL" Text="Required" ErrorMessage="Required" Display="dynamic" />
