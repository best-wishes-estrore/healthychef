<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserProfileNote_Edit.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.UserProfileNote_Edit" %>
<asp:Panel ID="pnlNotesAddEdit" runat="server" DefaultButton="btnAddNote" Visible="false">
    <asp:GridView ID="gvwNotes" runat="server" AutoGenerateColumns="false" DataKeyNames="NoteId">
        <Columns>
            <asp:BoundField DataField="Note" HeaderText="Note" ItemStyle-Width="50%" />
            <asp:BoundField DataField="DisplayToUser" HeaderText="Display To User" />
            <asp:BoundField DataField="DateCreated" HeaderText="Date Created" />
            <asp:CommandField ShowDeleteButton="true" ShowEditButton="false" ShowSelectButton="true" />
        </Columns>
        <EmptyDataTemplate>
            There are no notes for this section.
        </EmptyDataTemplate>
    </asp:GridView>
    <p><hr /></p>
    <div class="fieldRow">
        <div class="fieldCol">
            Add / Edit Note:
        </div>
    </div>
    <div class="fieldRow">
        <div class="fieldCol">
            <asp:TextBox ID="txtNote" runat="server" TextMode="MultiLine" Columns="50" Rows="3"
                MaxLength="1000" />
           <%-- <asp:RequiredFieldValidator ID="rfvNote" runat="server" ControlToValidate="txtNote"
                Text="*" Display="None" ErrorMessage="A note is required." SetFocusOnError="true" />--%>
        </div>
    </div>
    <div class="fieldRow">
        <div class="fieldCol">
            <asp:CheckBox ID="chkNoteDisplayToUser" runat="server" Text="Display To User?" Visible="false" />
        </div>
    </div>
    <div class="fieldRow">
        <div class="fieldCol">
            <asp:Button ID="btnAddNote" CssClass="btn btn-info" runat="server" Text="Save Note" />
            &nbsp;&nbsp;
            <asp:Button ID="btnCancel" CssClass="btn btn-danger" runat="server" Text="Cancel" CausesValidation="false" />
        </div>
    </div>
    <div class="fieldRow">
        <div class="fieldCol">
            <asp:ValidationSummary ID="ValSum_Note" runat="server" DisplayMode="BulletList" />
        </div>
    </div>
</asp:Panel>
<asp:Panel ID="pnlNotesDisplay" runat="server" Visible="false">    
    <p class="label"><asp:Label ID="lblNotesTitle" runat="server" /></p>
    <asp:Label ID="lblDisplayNotes" runat="server" />
</asp:Panel>
