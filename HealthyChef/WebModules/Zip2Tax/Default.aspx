<%@ Page Title="ZipToTax Settings" Language="C#" AutoEventWireup="true"
    MasterPageFile="~/Templates/WebModules/Default.master"
    CodeBehind="Default.aspx.cs"
    Inherits="HealthyChef.WebModules.Zip2Tax._default"
    Theme="WebModules"
    ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <div class="main-content">
        <div class="main-content-inner">
            <div class="p-5">
                <div class="pull-right">
                    <a id="MainMenuLink" runat="server" class="btn btn-info">Return to Main Menu</a>
                </div>
                <div class="clearfix"></div>
            </div>
            <div class="page-header">
                <h1>ZipToTax Settings</h1>
            </div>
            <div class="col-sm-12">
                Server:<asp:Literal runat="server" ID="ltlServer" /><br />
                DB Name:<asp:Literal runat="server" ID="ltlDBName" /><br />
                DB Username:<asp:Literal runat="server" ID="ltlDBUsername" /><br />
                DB Password:<asp:Literal runat="server" ID="ltlDBPassword" /><br />
            </div>
            <div class="clearfix m-2"></div>
            <div class="row-fluid">
                <div class="m-2">
                    <div class="col-sm-3">
                        <label>Zip Code: </label>
                        <asp:TextBox ID="txtZipTest" runat="server" CssClass="form-control" ValidationGroup="ZipTestGroup" />
                    </div>
                    <asp:RequiredFieldValidator ID="rfvZipTest" runat="server" ControlToValidate="txtZipTest" SetFocusOnError="true"
                        ErrorMessage="A zip is required" ValidationGroup="ZipTestGroup" /><br />
                    <div class="p-5">
                        <asp:Button ID="btnZipTest" runat="server" CssClass="btn btn-info" Text="Test Service" ValidationGroup="ZipTestGroup" OnClick="btnZipTest_Click" />
                        <asp:Literal ID="ltlZipTest" runat="server" />
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
