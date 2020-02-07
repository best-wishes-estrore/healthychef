<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PathNameEdit.ascx.cs" 
    Inherits="BayshoreSolutions.WebModules.Cms.Controls.PathNameEdit" %>

<div class="field">
    <span>Path Name &nbsp; <a id="editPathNameCtl" href="javascript:editPathName('editPathNameCtl', '<%= PathNameText.ClientID %>');">Edit</a></span>
    <%--<div class="help"></div>--%>
    <div>
        <asp:TextBox ID="PathNameText" runat="server" 
            MaxLength="255"
            Width="300px" 
            Enabled="false"
            Style="margin-right: 0;"
        />.aspx
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Required"
            ControlToValidate="PathNameText" Display="Dynamic" 
        />
        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
            ControlToValidate="PathNameText"
            ErrorMessage="Invalid path name" ValidationExpression="^[-A-Za-z0-9_]*$" Display="Dynamic" 
        />
    </div>
</div>

