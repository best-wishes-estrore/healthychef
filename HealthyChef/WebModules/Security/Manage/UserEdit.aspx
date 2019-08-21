<%@ Page Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true" Inherits="BayshoreSolutions.WebModules.Admin.Security.UserEdit" Title="Edit User"
    StylesheetTheme="WebModules" Codebehind="UserEdit.aspx.cs" %>

<%@ Register Src="~/WebModules/Components/PagePicker/PagePicker.ascx" TagName="PagePicker"
    TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="Server">

<div class="entity_edit">

<bss:MessageBox id="Msg" runat="server" />

<table border="0" cellpadding="0" cellspacing="6" style="width: 100%">
<tr>
    <td style="width: 50%; vertical-align: top; padding-right: 10px;">

        User: &nbsp; <span style="font-size: 150%;"><%# _user.UserName %></span>

        <br />
        <br />

        <div class="field">
            First Name
            <%--<div class="help">Help text</div>--%>
            <div>
                <asp:TextBox ID="FirstNameCtl" runat="server" Width="20em" />
            </div>
        </div>
     
        <div class="field">
            Last Name
            <%--<div class="help">Help text</div>--%>
            <div>
                <asp:TextBox ID="LastNameCtl" runat="server" Width="20em" />
            </div>
        </div>

        <div class="field">
            Email
            <%--<div class="help">Help text</div>--%>
            <div>
                <asp:TextBox ID="EmailCtl" runat="server" Width="20em" 
                    Text='<%# _user.Email %>'
                />
                <bss:CannedRegExValidator ID="CannedRegExValidator1" runat="server"
                    ControlToValidate="EmailCtl"
                    Display="Dynamic"
                    Text="Invalid email address" ErrorMessage="Invalid email address"
                    Type="email" />
            </div>
        </div>

        Roles
        <br />
        <asp:CheckBoxList ID="RolesList" runat="server" 
            DataTextFormatString=" {0} <a href='roledetails.aspx?role={0}'>Edit Role</a>" />

        <div class="field">
            Root Page
            <%--<div class="help">Help text</div>--%>
            <div>
                <uc1:PagePicker ID="PagePicker" runat="server" />
            </div>
        </div>        
        
        <div class="field">
            <%--<div class="help">Help text</div>--%>
            <div>
                <asp:CheckBox ID="IsApprovedCheckBox" runat="server" Checked='<%# _user.IsApproved %>' Text="Approved/Active" />
            </div>
        </div>

        <div class="field">
            Comments
            <%--<div class="help">Help text</div>--%>
            <div>
                <asp:TextBox ID="CommentsCtl" runat="server" Height="76px" Text='<%# _user.Comment %>'
                    TextMode="MultiLine" Width="280px" />
            </div>
        </div>


        <div class="toolbar">
        
            <asp:Button ID="SaveButton" runat="server" 
                CausesValidation="True" 
                CommandName="Update"
                CssClass="saveButton"
                Text="Save" 
                OnClick="SaveButton_Click" 
            />

            <asp:Button ID="EditDeleteButton" runat="server" CommandName="Delete" Text="Delete"
                OnClick="EditDeleteButton_Click" 
                OnClientClick="return confirm('Delete this user? (This action is permanent.)');"
                />

            <asp:Button ID="SaveCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                Text="Cancel" 
                CssClass="cancelButton"
                OnClick="SaveCancelButton_Click" 
            />
                
        </div>

    </td>
    <td rowspan="2" 
        style="padding-left: 10px; width: 25em; border-left: gray 1px solid; vertical-align: top;"
    >
        
        <p>
            Created:
            <asp:Label ID="CreationDateLabel" runat="server" Text='<%# formatDate(_user.CreationDate) %>' ForeColor="gray"></asp:Label>
        </p>
        <p>
            Last login:
            <asp:Label ID="LastLoginDateLabel" runat="server" Text='<%# formatDate(_user.LastLoginDate) %>' ForeColor="gray"></asp:Label>
        </p>
        <p>
            Last action:
            <asp:Label ID="LastActivityDateLabel" runat="server" Text='<%# formatDate(_user.LastActivityDate) %>' ForeColor="gray"></asp:Label>
        </p>
        <p>
            Last password change:
            <asp:Label ID="LastPasswordChangeDateLabel" runat="server" Text='<%# formatDate(_user.LastPasswordChangedDate) %>' ForeColor="gray"></asp:Label>
        </p>
        <p>
            Last password attempt lockout:
            <asp:Label ID="LastLockoutDateLabel" runat="server" Text='<%# formatDate(_user.LastLockoutDate) %>' ForeColor="gray"></asp:Label>
        </p>
        
        <p>
            <asp:CheckBox ID="CheckBox1" Checked="<%# _user.IsOnline %>" Text="Online" Enabled="false" runat="server" />
        </p>

        <p>
            <asp:CheckBox ID="CheckBox2" Checked="<%# _user.IsLockedOut %>" Text="Locked out" Enabled="false" runat="server" />
            <asp:LinkButton ID="Unlock" runat="server" Visible='<%# _user.IsLockedOut %>' OnClick="Unlock_Click">Unlock</asp:LinkButton>
        </p>
        
        <div>
            <asp:LinkButton ID="ResetPassword1" runat="server" CssClass="btn btn-info"
                Text="Reset Password"
                OnClick="ResetPassword_Click" />
            
            <p id="NewPassword" runat="server"></p>
        </div>

    </td>
</tr>
</table> 
</div>

</asp:Content>
