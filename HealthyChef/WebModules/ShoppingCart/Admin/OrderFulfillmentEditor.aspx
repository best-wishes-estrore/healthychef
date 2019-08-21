<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/WebModules/Default.master"
    Theme="WebModules" AutoEventWireup="true" CodeBehind="OrderFulfillmentEditor.aspx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.OrderFulfillmentEditor" %>

<%@ Register TagPrefix="hcc" TagName="OrderFulfillmentAlaCarte_Edit" Src="~/WebModules/ShoppingCart/Admin/UserControls/OrderFulfillmentAlaCarte_Edit.ascx" %>
<%@ Register TagPrefix="hcc" TagName="OrderFulfillmentProgram_Edit" Src="~/WebModules/ShoppingCart/Admin/UserControls/OrderFulfillmentProgram_Edit.ascx" %>
<%@ Register TagPrefix="hcc" TagName="OrderFulfillmentGiftCert_Edit" Src="~/WebModules/ShoppingCart/Admin/UserControls/OrderFulfillmentGiftCert_Edit.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <hcc:OrderFulfillmentAlaCarte_Edit ID="OrderFulfillmentAlaCarte_Edit1" runat="server" Visible="false" ValidationGroup="OF_AlaCarte_EditGroup"  />
    <hcc:OrderFulfillmentProgram_Edit ID="OrderFulfillmentProgram_Edit1" runat="server" Visible="false" ValidationGroup="OF_Program_EditGroup"/>
    <hcc:OrderFulfillmentGiftCert_Edit ID="OrderFulfillmentGiftCert_Edit1" runat="server" Visible="false" ValidationGroup="OF_GC_EditGroup" />
 <script type="text/javascript">
        $(function () {
            ToggleMenus('orderfulfillment', 'ordermanagement', 'storemanagement');
        });
    </script>
</asp:Content>
