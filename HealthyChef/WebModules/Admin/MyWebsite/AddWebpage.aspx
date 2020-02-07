<%@ Page Language="C#" Theme="WebModules" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true" Inherits="BayshoreSolutions.WebModules.Admin.MyWebsite.AddWebpage" Title="Add Webpage" CodeBehind="AddWebpage.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="Server">

    <div class="main-content">
        <div class="main-content-inner">
            <div class="page-header">
                <h1 id="HeaderCtl" runat="server">New Page</h1>
                <bss:MessageBox ID="Msg" runat="server" />
            </div>
            <div class="row-fluid">
                <div class="col-sm-12">
                    <div class="col-sm-3">
                        <div class="form-group">
                            <label class="label_td">Title</label>
                            <asp:TextBox ID="TitleTextBox" runat="server" Text="" CssClass="form-control" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                ControlToValidate="TitleTextBox"
                                ErrorMessage="Required" />
                        </div>
                    </div>
                    <div class="col-sm-3">
                        <div class="form-group">
                            <label class="label_td">Keywords</label>
                            <asp:TextBox ID="KeywordsTextBox" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                    <div class="col-sm-3">
                        <div class="form-group">
                            <label class="label_td">Description</label>
                            <asp:TextBox ID="DescriptionTextBox" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                    <div class="col-sm-3">
                        <div class="form-group">
                            <label class="label_td">Template</label>
                            <asp:DropDownList ID="TemplateDropDownList" runat="server"
                                AppendDataBoundItems="true">
                                <%-- <asp:ListItem>None</asp:ListItem> --%>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row-fluid">
                <div class="col-sm-12">
                    <div class="col-sm-3">
                        <div class="form-group">
                            <label class="label_td">Theme</label>
                            <asp:DropDownList ID="ThemeDropDownList" CssClass="form-control" runat="server"
                                AppendDataBoundItems="True">
                                <%-- "Default" uses the theme defined by the website root page. --%>
                                <asp:ListItem Value="">Default</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-sm-12">
                    <div class="col-sm-3">
                        <div class="form-group">
                            <label class="label_td"></label>
                            <asp:CheckBox ID="DisplayCheckBox" CssClass="form-control" runat="server" Checked="true"
                                Text="Visible in navigation" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row-fluid">
                <div class="col-sm-12">
                    <div class="entity_edit col-sm-5">
                        <div class="toolbar p-5">
                            <asp:Button ID="CreateButton" runat="server" Text="Save" CommandName="Insert" OnClick="CreateButton_Click" CausesValidation="True" CssClass="saveButton btn btn-info" />
                            <asp:Button ID="InsertCancelButton" runat="server" CommandName="Cancel" Text="Cancel" CausesValidation="False" OnClick="InsertCancelButton_Click" CssClass="cancelButton btn btn-danger" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            ToggleMenus('websites', undefined, undefined);
        });
    </script>
</asp:Content>

