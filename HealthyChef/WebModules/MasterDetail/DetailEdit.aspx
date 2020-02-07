<%@ Page Title="Edit Content Item" Language="C#" AutoEventWireup="true" 
    MasterPageFile="~/Templates/WebModules/Module.Master" 
    CodeBehind="DetailEdit.aspx.cs" 
    Inherits="BayshoreSolutions.WebModules.MasterDetail.DetailEdit" 
    Theme="WebModules" 
    ValidateRequest="false"
%>
<%@ Register Src="Controls/DetailEditControl.ascx" TagName="DetailEditControl" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">

<bss:MessageBox ID="Msg" runat="server" />

<div class="entity_edit">

    <uc1:DetailEditControl id="DetailEditControl1" runat="server" />
    
    <div class="toolbar">
        <asp:Button ID="MasterDetail_SaveButton" runat="server"
                    CssClass="saveButton"
                    CausesValidation="true"
                    Text="Save"
                    ValidationGroup=""
                    OnClick="MasterDetail_SaveButton_Click" />
        <asp:Button ID="MasterDetail_CancelButton" runat="server"
                    CssClass="cancelButton"
                    CausesValidation="false"
                    Text="Cancel"
                    OnClick="MasterDetail_CancelButton_Click" />
    </div>

</div>

</asp:Content>
