<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/WebModules/Default.master"
    Theme="WebModules" AutoEventWireup="true" CodeBehind="OrderFulfillment.aspx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.OrderFulfillment" %>

<%@ Register TagPrefix="hcc" TagName="OrderFulfillment_Search" Src="~/WebModules/ShoppingCart/Admin/UserControls/OrderFulfillment_Search.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="main-content">
        <div class="main-content-inner">
            <a href="../" onclick="MakeUpdateProg(true);" class="btn btn-info b-10"><< Back to Dashboard</a>
            <hcc:OrderFulfillment_Search ID="OrderFulfillment_Search1" runat="server" />
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            ToggleMenus('orderfulfillment', 'ordermanagement', 'storemanagement');
            preventResfreshOnEnter();
        });
    </script>
</asp:Content>
