<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true" CodeBehind="DefaultMenuManager.aspx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.DefaultMenuManager" Theme="WebModules" %>

<%@ Register TagPrefix="hcc" TagName="DefaultMenuManager" Src="~/WebModules/ShoppingCart/Admin/UserControls/ProgramDefaultMenu_Edit.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="main-content">
        <div class="main-content-inner">
            <a href="../" onclick="MakeUpdateProg(true);" class="btn btn-info b-10"><< Back to Dashboard</a>
            <div class="fieldRow">
                <div class="fieldCol m-2">
                    <div class="col-sm-3">
                        <div class="form-group">
                            <label>Delivery Date:</label>
                            <asp:DropDownList ID="ddlDelDates" CssClass="form-control" runat="server" AutoPostBack="true" onchange="MakeUpdateProg(true);" />
                            <asp:RequiredFieldValidator ID="rfvDelDates" runat="server" ControlToValidate="ddlDelDates"
                                Display="None" ErrorMessage="A delivery date is required." SetFocusOnError="true" ValidationGroup="DefaultMenuGroup"
                                InitialValue="-1" />
                        </div>
                    </div>
                    <div class="col-sm-5">
                        <div class="form-group">
                            <label>Program:</label>
                            <asp:DropDownList ID="ddlPrograms" runat="server" CssClass="form-control" AutoPostBack="true" onchange="MakeUpdateProg(true);" />
                        </div>
                    </div>
                    <div class="col-sm-3 m-2 p-5">
                        <asp:Button ID="btnGetPrograms" CssClass="btn btn-info" runat="server" Text="Get Default Menu" ValidationGroup="DefaultMenuGroup" OnClientClick="MakeUpdateProg(true);" />
                        <asp:ValidationSummary ID="ValSum1" runat="server" ValidationGroup="DefaultMenuGroup" />
                    </div>
                </div>
            </div>
            <hcc:DefaultMenuManager ID="DefaultMenuManager1" runat="server" Visible="false" />
        </div>
    </div>
    <script type="text/javascript">
    $(function () {
        ToggleMenus('programdefaultmenus', 'productionmanagement', 'storemanagement');
    });
    </script>
</asp:Content>

