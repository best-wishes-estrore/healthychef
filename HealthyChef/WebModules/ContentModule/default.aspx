<%@ Page Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="BayshoreSolutions.WebModules.ContentModule._default" Title="Content Module Settings" Theme="webModules" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <div class="main-content">
        <div class="main-content-inner">
            <div class="page-header">
                <h1>Content Module Settings</h1>
            </div>
            <div class="row-fluid">
                <div class="col-sm-12">
                    <div class="entity_edit">
                        <div class="field" style="width: 100%">
                            Content Reviewer Email 
                            <div class="help">Email address will be notified when content is submitted for review/approval.</div>
                            <div>
                                <asp:TextBox ID="emailAddress" runat="server"
                                    Width="25em" />
                                <bss:CannedRegExValidator ID="RegularExpressionValidator1" runat="server"
                                    Type="Email"
                                    ControlToValidate="emailAddress"
                                    Text="Invalid email" />
                            </div>
                        </div>

                        <div class="toolbar p-5">
                            <asp:Button ID="SaveSettings" runat="server"
                                Text="Save"
                                CssClass="saveButton btn btn-info"
                                OnClick="SaveSettings_Click" />
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
    $(function () {
        ToggleMenus('system', undefined, undefined);
    });
    </script>
</asp:Content>
