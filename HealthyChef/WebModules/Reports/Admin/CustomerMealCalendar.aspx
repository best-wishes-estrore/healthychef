<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerMealCalendar.aspx.cs" Inherits="HealthyChef.WebModules.Reports.Admin.CustomerMealCalendar" 
    MasterPageFile="~/Templates/WebModules/Default.master" Theme="WebModules"  %>

<asp:Content ContentPlaceHolderID="header" runat="server" ID="Header1">
    <style type="text/css">
        @media print{@page {size: landscape}}

        .tdOutput
        {
            text-align: center; vertical-align:middle; padding-bottom: 20px; padding-top: 20px; font-family: Lucida Sans; font-size: 10pt;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <center>
        <h1>
            Customer Meal Calendar
        </h1>
    </center>
    
    <table width="100%" border="0">
        <tr>
            <td>
                <asp:Label ID="lblFeedback" runat="server" />
                <asp:ValidationSummary ID="ValSum1" runat="server" ValidationGroup="CustomerMealCalendarGroup" />
            </td>
        </tr>
        <tr align="center">
            <td>Delivery Date:&nbsp;
                <asp:TextBox runat="server" ID="txtStartDate" ClientIDMode="Static" Style="width: 80px; text-align: center;"
                    ValidationGroup="CustomerMealCalendar" />
                <asp:Button ID="btnLoadCustomers" runat="server" Text="Load Customers"  OnClick="btnLoadCustomers_Click"/>
                <asp:DropDownList ID="ddlCustomers" runat="server"></asp:DropDownList>
                <asp:Button runat="server" Text="Refresh" OnClick="ButtonRefresh_Click" ID="ButtonRefresh" />
                <ajax:CalendarExtender ID="CalExt1" runat="server" TargetControlID="txtStartDate" PopupPosition="TopRight" />
                <asp:RequiredFieldValidator ID="rfvVal1" runat="server" ControlToValidate="txtStartDate"
                    Display="None" ErrorMessage="Start Date is required."
                    SetFocusOnError="true" ValidationGroup="CustomerMealCalendar" />
                <asp:CompareValidator ID="cprVal1" runat="server" ControlToValidate="txtStartDate"
                    Display="None" ErrorMessage="Start Date must be a date." Operator="DataTypeCheck" Type="Date"
                    SetFocusOnError="true" ValidationGroup="CustomerMealCalendar" />
                <input id="btnPrint" type="button" value="Print" onclick="javascript: PrintContent();" />
            </td>
        </tr>
    </table>
    <div id="divInvoice">
        <table style="width: 100%">
            <tr align="center">
                <td><asp:Label ID="lblOrderData" runat="server" Text="" EnableViewState="false" Font-Bold="true"></asp:Label></td>
            </tr>
            <tr align="center">
                <td><asp:Label ID="lblCustData" runat="server" Text="" EnableViewState="false" Font-Bold="true"></asp:Label></td>
            </tr>
        </table>
        <asp:ListView ID="lvCustomerMealReport" runat="server" EnableViewState="false">
            <LayoutTemplate> 
                <table width="990px" runat="server" cellpadding="0" cellspacing="0" id="tblCustomerMealReport">
                    <tr runat="server">
                        <td runat="server">
                            <table id="itemPlaceholderContainer" runat="server" border="1" style="width: 990px; " cellpadding="2" cellspacing="0">
                                <tr runat="server">
                                    <th runat="server" style="text-align: center; height: 30px; width: 40px">Day</th>
                                    <th runat="server" style="text-align: center; height: 30px; width: 160px;">Breakfast</th>
                                    <th runat="server" style="text-align: center; height: 30px; width: 160px;">Snack</th>
                                    <th runat="server" style="text-align: center; height: 30px; width: 160px;">Lunch</th>
                                    <th runat="server" style="text-align: center; height: 30px; width: 160px;">Snack</th>
                                    <th runat="server" style="text-align: center; height: 30px; width: 160px;">Dinner</th>
                                    <th runat="server" style="text-align: center; height: 30px; width: 160px;">Dessert</th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr runat="server">
                        <td runat="server" style=""></td>
                    </tr>
                </table>                                   
            </LayoutTemplate>
            <ItemTemplate>  
                <tr>
                    <td class="tdOutput" align="center" valign="middle">
                        <asp:Label ID="DayNumberLabel" runat="server" Text="&nbsp;" Font-Names="Lucida Sans" Font-Size="10pt" />
                    </td>
                    <td class="tdOutput" align="center" valign="middle">
                        <asp:Label ID="BreakfastLabel" runat="server" Text="&nbsp;" Font-Names="Lucida Sans" Font-Size="10pt" />
                    </td>
                    <td class="tdOutput" align="center" valign="middle">
                        <asp:Label ID="SnackLabel" runat="server" Text="&nbsp;" Font-Names="Lucida Sans" Font-Size="10pt" />
                    </td>
                    <td class="tdOutput" align="center" valign="middle">
                        <asp:Label ID="LunchLabel" runat="server" Text="&nbsp;" Font-Names="Lucida Sans" Font-Size="10pt" />
                    </td>
                    <td class="tdOutput" align="center" valign="middle">
                        <asp:Label ID="SnackLabel1" runat="server" Text="&nbsp;" Font-Names="Lucida Sans" Font-Size="10pt" />
                    </td>
                    <td class="tdOutput" align="center" valign="middle">
                        <asp:Label ID="DinnerLabel" runat="server" Text="&nbsp;" Font-Names="Lucida Sans" Font-Size="10pt" />
                    </td>
                    <td class="tdOutput" align="center" valign="middle">
                        <asp:Label ID="DessertLabel" runat="server" Text="&nbsp;" Font-Names="Lucida Sans" Font-Size="10pt" />
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr style="background-color: lightgray;">
                    <td class="tdOutput" align="center" valign="middle">
                        <asp:Label ID="DayNumberLabel" runat="server" Text="&nbsp;" Font-Names="Lucida Sans" Font-Size="10pt" />
                    </td>
                    <td class="tdOutput" align="center" valign="middle">
                        <asp:Label ID="BreakfastLabel" runat="server" Text="&nbsp;" Font-Names="Lucida Sans" Font-Size="10pt" />
                    </td>
                    <td class="tdOutput" align="center" valign="middle">
                        <asp:Label ID="SnackLabel" runat="server" Text="&nbsp;" Font-Names="Lucida Sans" Font-Size="10pt" />
                    </td>
                    <td class="tdOutput" align="center" valign="middle">
                        <asp:Label ID="LunchLabel" runat="server" Text="&nbsp;" Font-Names="Lucida Sans" Font-Size="10pt" />
                    </td>
                    <td class="tdOutput" align="center" valign="middle">
                        <asp:Label ID="SnackLabel1" runat="server" Text="&nbsp;" Font-Names="Lucida Sans" Font-Size="10pt" />
                    </td>
                    <td class="tdOutput" align="center" valign="middle">
                        <asp:Label ID="DinnerLabel" runat="server" Text="&nbsp;" Font-Names="Lucida Sans" Font-Size="10pt" />
                    </td>
                    <td class="tdOutput" align="center" valign="middle">
                        <asp:Label ID="DessertLabel" runat="server" Text="&nbsp;" Font-Names="Lucida Sans" Font-Size="10pt" />
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <EmptyDataTemplate>
                    <table runat="server" style="">
                        <tr>
                            <td>No data was returned.</td>
                        </tr>
                    </table>
            </EmptyDataTemplate>
        </asp:ListView>
    </div>
    <script type="text/javascript">
        function PrintContent() {
            var DocumentContainer = document.getElementById('divInvoice');
            var WindowObject = window.open('', "_blank", "toolbars=no,scrollbars=no,status=no,resizable=no,landscape=no");
            WindowObject.document.writeln(DocumentContainer.innerHTML);
            WindowObject.document.close();
            WindowObject.focus();
            WindowObject.print();
            WindowObject.close();
        }
    </script>
       
</asp:Content>
