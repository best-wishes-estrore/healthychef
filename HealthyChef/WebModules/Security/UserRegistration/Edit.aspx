<%@ Page Language="C#" Title="UserRegistration Module Settings" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="BayshoreSolutions.WebModules.Cms.Security.UserRegistration.Edit" Theme="WebModules" %>
<%@ Register Src="~/WebModules/Components/PagePicker/PagePicker.ascx" TagPrefix="uc1" TagName="PagePicker" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">

<h3><asp:literal id="HeaderCtl" runat="server"></asp:literal> Module Settings</h3>

<div class="entity_edit">

    <div class="field">
        Confirmation Page 
        <div>
            <uc1:PagePicker ID="ConfirmationPage" runat="server" />
        </div>
    </div>
    <div class="field">
        Notification Email
        <span class="help">Each time a user signs up, an email will be sent to this address.</span>
        <asp:TextBox ID="NotifyEmail" runat="server" />
    </div>
    <div class="toolbar">
        <asp:Button ID="SaveButton" CssClass="saveButton" Text="Save" runat="server" OnClick="SaveButton_Click" />
        <asp:Button ID="CancelButton" CssClass="cancelButton" Text="Cancel" runat="server" OnClick="CancelButton_Click" />
    </div>

</div>

</asp:Content>
