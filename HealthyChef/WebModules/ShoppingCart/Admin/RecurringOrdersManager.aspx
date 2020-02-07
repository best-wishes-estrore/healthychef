<%@ Page Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true" CodeBehind="RecurringOrdersManager.aspx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.RecurringOrdersManager" Theme="WebModules" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
    <script>
        $(document).ready(function () {
            $('a[id*=btnDelete]').each(function () {
                var hrefJs = $(this).attr('href');
                hrefJs = hrefJs.substring(11, hrefJs.length); // remvoves the leading "javascript:"
                $(this).attr('href', 'javascript: if(confirmDelete()){' + hrefJs + '; }');
            });
        });

        function confirmDelete() {
            if (!confirm('Are you sure you want to cancel this recurring order?')) {
                return false;
            }

            return true;
        }

        function confirmTest() {
            if (!confirm('Are you sure you want to process test orders? Do not run this in production mode.')) {
                return false;
            }
            return true;
        }
    </script>
    <style type="text/css">
        .RecurringOrderReady {
            color: darkgreen;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="main-content" ng-app="UserApp" ng-controller="GetRecurring">
        <div class="main-content-inner">
            <a href="../" onclick="MakeUpdateProg(true);" class="btn btn-info b-10"><< Back to Dashboard</a>
            <div class="page-header">
                <h1>Recurring Orders Manager</h1>
            </div>
            <div class="hcc_admin_ctrl left">
                <asp:Panel ID="pnlSearchPurchase" runat="server" DefaultButton="btnGenerate">
                    <div class="row-fluid">
                        <div class="col-sm-12">
                            <div class="col-sm-9">
                                <ul>
                                    <li>The orders indicated in bold-green are expiring orders that are ready for auto-renewal.</li>
                                    <li>To process these auto-renewals, the "Submit Auto-Renew Orders" button must be clicked prior to the cut-off date (1 week before the Last Scheduled Delivery Date) for the next delivery date. </li>
                                    <li>Auto-renew orders that are not processed prior to the cut-off date will be added to the next available delivery date, and a week may be skipped.</li>
                                    <li>If a week is skipped, a manual purchase on the the account for the missed weeks will be required.</li>
                                    <li style="font-weight: bold">Before submitting orders for auto-renewal you must make sure that all they do not exceed the length of the Production Calendar, if so the Production Calendar will need to be extended before submission.</li>
                                    <li>Clicking "Submit Auto-Renew Orders" more then once will not create multiple orders for the same weeks.</li>
                                    <%--<li>The items in bold-green indicate auto-renewal orders ready to process.</li>
                                        <li>These orders must be renewed before the Last Scheduled Delivery date so they can be shipped the following Friday.</li> 
                                        <li>To process these auto-renewals you must click the "Submit Auto-Renew Orders" button.</li>--%>
                                </ul>
                            </div>
                            <div class="col-sm-3">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnGenerate" runat="server" CssClass="btn btn-info" Text="Submit Auto-Renew Orders" OnClick="btnGenerate_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="m-1" style="display: inline-block"></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnTestGenerate" runat="server" CssClass="btn btn-info" Text="Test Auto-Renew Orders" OnClientClick="return confirmTest();" OnClick="btnTestGenerate_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="color: red;">
                                            <asp:Literal ID="litNumRecurringLabel" runat="server" Text="Recurring Orders Processed:" Visible="False"></asp:Literal>
                                            <asp:Literal ID="litNumRecurringOrders" runat="server" Visible="False"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="color: red;">
                                            <asp:Literal ID="litNoOrders" runat="server" Text="No Orders Found to Process." Visible="False" /></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <%--            <div>
                <table style="width: 100%">
                    <tr style="font-weight: bold; text-align: center;">
                        <td>Customer</td><td>Profile</td><td>Recurring Order</td><td>Last Scheduled Delivery</td>
                    </tr>
                    <asp:ListView ID="lvRecurringOrders" runat="server" >
                        <ItemTemplate> 
                            <tr>
                                <td style="text-align: center;">
                                    <asp:Literal ID="litUserProfileID" runat="server" Text='<%# GetCustomer(int.Parse(Eval("CartID").ToString())) %>'></asp:Literal>
                                </td>  
                                <td>
                                    <asp:Literal ID="litCartItem" runat="server" Text='<%# GetCartItemMeal(int.Parse(Eval("CartItemID").ToString())) %>'></asp:Literal>
                                </td>

                                <td style="text-align: center;">
                                    <asp:Literal ID="litRecurringDate" runat="server" Text='<%# DateTime.Parse(Eval("MaxDeliveryDate").ToString()).ToShortDateString() %>'></asp:Literal>
                                </td>
                                
                             </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate> 
                            <tr style="background-color: lightgray;">
                                <td style="text-align: center;">
                                    <asp:Literal ID="litUserProfileID" runat="server" Text='<%# GetCustomer(int.Parse(Eval("CartID").ToString())) %>'></asp:Literal>
                                </td>                                  
                                <td>
                                    <asp:Literal ID="litCartItem" runat="server" Text='<%# GetCartItemMeal(int.Parse(Eval("CartItemID").ToString()))%>'></asp:Literal>
                                </td>

                                <td style="text-align: center;">
                                    <asp:Literal ID="litRecurringDate" runat="server" Text='<%# DateTime.Parse(Eval("MaxDeliveryDate").ToString()).ToShortDateString() %>'></asp:Literal>
                                </td>
                                
                             </tr>
                        </AlternatingItemTemplate>
                    </asp:ListView>
                </table>
            </div>--%>
                    <div class="clearfix"></div>
                    
                    <div class="col-sm-12">
                        <asp:Label runat="server" ID="lblConfirmFeedback"></asp:Label>
                        <table ng-show="RecurringOrders.length!=0" class="table  table-bordered table-hover">
                            <thead>
                                <tr>
                                    <td ng-click="orderRecurringTable('CustomerName')">Customer Name</td>
                                    <td ng-click="orderRecurringTable('ProfileName')">Profile</td>
                                    <td ng-click="orderRecurringTable('ItemName')">Recurring Order</td>
                                    <td ng-click="orderRecurringTable('maxdeliverydate')">Last Scheduled Delivery</td>
                                    <td></td>
                                </tr>
                            </thead>
                            <tbody>
                                  <tr ng-show="ShowLoader" class="loader">
                                                <td colspan="6" align="center">
                                                    <img src="/App_Themes/HealthyChef/Images/Blocks-1s-200px.gif" alt="Loading..." />
                                                </td>
                                            </tr>
                                <tr ng-class-odd="'odd'" ng-class-even="'even'"  ng-repeat="u in RecurringOrders | orderBy:OrderByField : reverse" >
                                    <td ng-class="u.Recurringready==true? 'RecurringOrderReady' : ''  "><span>{{ u.CustomerName }}</span></td>
                                    <td ng-class="u.Recurringready==true? 'RecurringOrderReady' : '' ">{{ u.ProfileName }}</td>
                                    <td ng-class="u.Recurringready==true? 'RecurringOrderReady' : '' ">{{  u.ItemName == "" ? "	Item not available":u.ItemName }}(Quantity:{{u.quantity}})</td>
                                    <td ng-class="u.Recurringready==true? 'RecurringOrderReady' : '' ">{{ u.maxdeliverydate }}</td>
                                    <td ng-class="u.Recurringready==true? 'RecurringOrderReady' : '' " align="left"><a style="color:#0079be ;cursor:pointer;text-decoration:none" ng-click="CancelAutoRenew(u)">Cancel Auto-Renew</a></td>
                                </tr>
                            </tbody>
                            <%--<asp:ListView ID="lvAllRecurringOrders" runat="server">
                                ng-class-odd="'odd'" ng-class-even="'even'" 
                                <ItemTemplate>
                                    <tr class="<%# Eval("MaxCutOffDate") != null ? (((DateTime)Eval("MaxCutOffDate") <  DateTime.Now &&  DateTime.Now > MinCutOffDate) || DateTime.Now > ((DateTime)Eval("MaxCutOffDate")).AddDays(1) ? "RecurringOrderReady" : "") : ""%>">
                                        <td style="text-align: center;">
                                            <asp:Literal ID="litCustomer" runat="server" Text='<%# GetCustomer(int.Parse(Eval("CartID").ToString())) %>'></asp:Literal></td>
                                        <td style="text-align: center;">
                                            <asp:Literal ID="litUserProfile" runat="server" Text='<%# GetProfileName(int.Parse(Eval("UserProfileID").ToString())) %>'></asp:Literal></td>
                                        <td>
                                            <asp:Literal ID="litCartItem" runat="server" Text='<%# GetCartItemMeal(int.Parse(Eval("CartItemID").ToString())) + "<br />(Quantity: " + Eval("Quantity") + ")" %>'></asp:Literal>
                                        </td>
                                        <td style="text-align: center;"><asp:Literal ID="litNextStartDate" runat="server" Text='<%# GetNextRecurringDate(int.Parse(Eval("CartID").ToString()), int.Parse(Eval("CartItemID").ToString())) %>'></asp:Literal></td>
                                        <td style="text-align: center;">
                                            <asp:Literal ID="litNextStartDate" runat="server" Text='<%# Eval("MaxCutOffDate") != null ? ((DateTime)Eval("maxdeliverydate")).ToShortDateString() : "" %>'></asp:Literal></td>
                                        <td>
                                            <asp:LinkButton ID="btnDelete" OnCommand="btnDeleteOnCommand" runat="server" Text="Cancel Auto-Renew" ClientIDMode="Static" CommandArgument='<%# Eval("CartID") + "_" + Eval("CartItemID") %>' /></td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr style="background-color: lightgray;" class="<%#  Eval("MaxCutOffDate") != null ? (((DateTime)Eval("MaxCutOffDate") <  DateTime.Now &&  DateTime.Now > MinCutOffDate) || DateTime.Now > ((DateTime)Eval("MaxCutOffDate")).AddDays(1) ? "RecurringOrderReady" : "") : ""%>">
                                        <td style="text-align: center;">
                                            <asp:Literal ID="litCustomer" runat="server" Text='<%# GetCustomer(int.Parse(Eval("CartID").ToString())) %>'></asp:Literal></td>
                                        <td style="text-align: center;">
                                            <asp:Literal ID="litUserProfile" runat="server" Text='<%# GetProfileName(int.Parse(Eval("UserProfileID").ToString())) %>'></asp:Literal></td>
                                        <td>
                                            <asp:Literal ID="litCartItem" runat="server" Text='<%# GetCartItemMeal(int.Parse(Eval("CartItemID").ToString()))  + "<br />(Quantity: " + Eval("Quantity") + ")"%>'></asp:Literal>
                                        </td>
                                        <td style="text-align: center;"><asp:Literal ID="litNextStartDate" runat="server" Text='<%# GetNextRecurringDate(int.Parse(Eval("CartID").ToString()), int.Parse(Eval("CartItemID").ToString())) %>'></asp:Literal></td>
                                        <td style="text-align: center;">
                                            <asp:Literal ID="litNextStartDate" runat="server" Text='<%# Eval("MaxCutOffDate") != null ? ((DateTime)Eval("maxdeliverydate")).ToShortDateString() : ""%>'></asp:Literal></td>
                                        <td>
                                            <asp:LinkButton ID="btnDelete" OnCommand="btnDeleteOnCommand" runat="server" Text="Cancel Auto-Renew" ClientIDMode="Static" CommandArgument='<%# Eval("CartID") + "_" + Eval("CartItemID") %>' /></td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:ListView>--%>
                        </table>
                        <div ng-show="RecurringOrders.length==0" style="color: red; font-weight: bold">
                            <p>No Records Found</p>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var app = angular.module('UserApp', ["ui.bootstrap"]);

        //Getting data from API
        app.controller('GetRecurring', function ($scope, $http)
        {
            $scope.ShowLoader = true;
            $http({
                method: "GET",
                url: GetApiBaseURL() + "GetRecurringOrder"
            }).then(function mySuccess(response) {
                $scope.RecurringOrders = response.data;
                console.log(response.data.length);
                $scope.ShowLoader = false;
                }, function myError(response) {
                    $scope.ShowLoader = false;
                $scope.ErrorMessage = response.statusText;
            });

            $scope.orderRecurringTable = function (x) {

                $scope.reverse = ($scope.OrderByField === x) ? !$scope.reverse : false;
                $scope.OrderByField = x;
            }

            $scope.CancelAutoRenew = function (order) {
                

                var cartitemid = order.cartitemid;
                var cartid = order.cartid;

                if (confirmDelete()) {
                    $http({
                        method: "POST",
                        url: GetApiBaseURL() + "CancelAutorenew/" + cartitemid + "/" + cartid
                    }).then(function mySuccess(response) {

                        if (response.data.IsSuccess) {
                            
                            alert(response.data.Message);
                            window.location.href = "/WebModules/ShoppingCart/Admin/RecurringOrdersManager.aspx";
                        }
                    }, function myError(response) {

                        if (response.data.IsSuccess == false) {
                            alert(response.data.Message)
                        }
                    });
                }
                else {

                }

            };

        });
    </script>
    <script type="text/javascript">
        $(function () {
            ToggleMenus('recurringorders', 'ordermanagement', 'storemanagement');
        });
    </script>
    <style>
         .loader {
            /*position: absolute;
            top: 20%;
            left: 40%;
            opacity: 0.8;
            z-index: 99;*/
            position: fixed;
            top: 0;
            left: 50%;
            opacity: 0.8;
            z-index: 99;
        }
            .loader img {
                    position: absolute;
    top: 150px;
    left: 0;
            }


            .odd {
  background-color: #ffffff;
  }
  .even {
    background-color: lightgray;
  }
    </style>
</asp:Content>
