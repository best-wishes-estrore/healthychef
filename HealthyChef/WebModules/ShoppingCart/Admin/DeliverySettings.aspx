<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/WebModules/Default.master"
    Theme="WebModules" AutoEventWireup="true" CodeBehind="DeliverySettings.aspx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.DeliverySettings" %>

<%@ Register TagPrefix="hcc" TagName="DeliverySettingsEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/DeliverySettings_Edit.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <div class="main-content">
        <div class="main-content-inner">
            <a href="../" onclick="MakeUpdateProg(true);"  class="btn btn-info b-10"><< Back to Dashboard</a>
            <hcc:DeliverySettingsEdit ID="DeliverySettingsEdit1" runat="server" ValidationGroup="DeliverySettingsEditGroup" />
        </div>
    </div>
     <script type="text/javascript">
        $(function () {
            ToggleMenus('shippingfees', 'listmanagement', 'storemanagement');
        });
    </script>
    <style>
        .aling_table1 td {
        margin: 5px 0px;
    }
    </style>
</asp:Content>
