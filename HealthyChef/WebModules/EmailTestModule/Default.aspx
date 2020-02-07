<%@ Page Title="Email Test Page" Language="C#" AutoEventWireup="true"
    MasterPageFile="~/Templates/WebModules/Default.master"
    CodeBehind="Default.aspx.cs"
    Inherits="HealthyChef.WebModules.EmailTestModule._default"
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
           
            <div class="row-fluid">
                <div class="page-header">
                    <h1>Email Test Page</h1>
                </div>
                <div class="col-sm-12 p-5">
                    <asp:Button ID="btnEmailTest" runat="server" CssClass="btn btn-info" Text="Test Email" OnClick="btnEmailTest_Click" />
                    <asp:Label ID="lblEmailTest" runat="server" EnableViewState="false" />
                </div>
            </div>
            <div class="clearfix"></div>
            <div class="row-fluid">
                <div class="page-header">
                    <h1>Delete Default Menu Duplicates</h1>
                </div>
                <div class="col-sm-12 p-5">
                    <asp:Button ID="btnDelDupes" runat="server" CssClass="btn btn-danger" Text="Delete Duplicates" OnClick="btnDelDupes_Click" />
                    <asp:Label ID="lblFeedbackDelDupes" runat="server" EnableViewState="false" />
                </div>
            </div>
        </div>
    </div>
    <%-- <h3>User Import</h3>

    <div>
        <div class="row">
            <asp:Button ID="btnUserImprot" runat="server" Text="Run Customer Import" OnClick="btnUserImprot_Click" />
            <asp:Button ID="imbPrint" runat="server" ImageUrl="/App_Themes/pullmanholt/Images/btn_print.gif"
                OnClientClick="javascript:PrintContent()" Text="Print" />
            <br />
            <div id="divReport">
                <asp:Label ID="lblImportResults" runat="server" EnableViewState="false" Font-Size="8px" />
            </div>
        </div>
    </div>--%>
    <script type="text/javascript">
        function PrintContent() {
            var DocumentContainer = document.getElementById('divReport');
            var WindowObject = window.open('', "PrintOrderInvoice", "width=740,height=325,top=200,left=250,toolbars=no,scrollbars=yes,status=no,resizable=no");
            WindowObject.document.writeln(DocumentContainer.innerHTML);
            WindowObject.document.close();
            WindowObject.focus();
            WindowObject.print();
            WindowObject.close();
        }
    </script>
    <script type="text/javascript">
        $(function () {
            ToggleMenus('system', undefined, undefined);

        });

    </script>
</asp:Content>
