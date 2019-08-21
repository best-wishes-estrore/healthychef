<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true" CodeBehind="CancellationsManager.aspx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.CancellationsManager" Theme="WebModules" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="main-content" ng-app="UserApp" ng-controller="GetCancellation">
        <div class="main-content-inner">
            <a href="../" onclick="MakeUpdateProg(true);" class="btn btn-info b-10"><< Back to Dashboard</a>
            <div class="page-header">
                <h1>Cancellations Manager</h1>
            </div>
            <div class="col-sm-12">
                <div class="hcc_admin_ctrl left">
                    <asp:Panel ID="pnlSearchPurchase" runat="server" DefaultButton="btnFindPurchase">
                        Purchase Number:
            <asp:TextBox ID="txtPurchaseNumber" runat="server" ValidationGroup="CancelGroup" />
                        <asp:Button ID="btnFindPurchase" CssClass="btn btn-info" runat="server" Text="Find Purchase" ValidationGroup="CancelGroup" />
                        <div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPurchaseNumber"
                                Display="Dynamic" ErrorMessage="A Purchase Number is required." ValidationGroup="CancelGroup" EnableClientScript="false" />
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtPurchaseNumber"
                                Display="Dynamic" ErrorMessage="Please enter a valid Purchase Number" Operator="DataTypeCheck" Type="Integer" ValidationGroup="CancelGroup" />
                            <asp:Label ID="lblFeedback" runat="server" EnableViewState="false" ForeColor="Red" />
                        </div>
                    </asp:Panel>
                    <br />
                    <asp:Panel ID="pnlPurchase" runat="server" Visible="false">
                        <asp:Label runat="server" ID="lblPurchaseStatus" />
                        &nbsp;&nbsp;
            <asp:Button ID="btnCancelPurchase" runat="server" CssClass="btn btn-info" Text="Void Transaction" Visible="false"
                OnClientClick="javascript: return confirm('Are you sure that you want to cancel this entire purchase?')" />
                        <%--<asp:Button ID="btnCancelPurchase" runat="server" Text="Cancel Entire Purchase" Visible="false"
                OnClientClick="javascript: return confirm('Are you sure that you want to cancel this entire purchase?')" />--%>
                        <br />
                        <fieldset>
                            <legend class="cartleft">
                                <asp:Label ID="lblOrdNumsLegend" runat="server" /></legend>
                            <%--<asp:GridView ID="gvwOrderNumbers" runat="server" DataKeyNames="Item1" AutoGenerateColumns="false" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="Item1" HeaderText="Order Number" ItemStyle-Width="150px" />

                                    <asp:BoundField DataField="Item2" HeaderText="Item Count" ItemStyle-Width="150px" />

                                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="150px">
                                        <ItemTemplate><%# (Boolean.Parse(Eval("Item3").ToString())) ? "Cancelled" : "Active" %></ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:CommandField ShowSelectButton="true" SelectText="Show Items" />
                                    <asp:CommandField ShowDeleteButton="true" DeleteText="Cancel All Items" />
                                </Columns>
                                <EmptyDataTemplate>No order numbers found for this purchase.</EmptyDataTemplate>
                            </asp:GridView>--%>
                                           
                            <table ng-show="GetPurchaseDetails.length!=0" class="table  table-bordered table-hover">
                                <thead>
                                    <tr align="left">
                                        <th scope="col">Order Number</th>
                                        <th scope="col">Item Count</th>
                                        <th scope="col">Status</th>
                                        <th scope="col">&nbsp;</th>
                                        <th scope="col">&nbsp;</th>
                                    </tr>
                                </thead>

                                <tbody>
                                      <tr ng-show="ShowLoader" class="loader">
                                                <td colspan="6" align="center">
                                                    <img src="/App_Themes/HealthyChef/Images/Blocks-1s-200px.gif" alt="Loading..." />
                                                </td>
                                            </tr>
                                    <tr style="background-color: #FEFEFE;" ng-repeat="u in GetPurchaseDetails">
                                        <td><span>{{ u.OrderNumber }}</span></td>
                                        <td>{{ u.ItemCount }}</td>
                                        <td>{{ u.Status == true ? "Cancelled":"Active" }}</td>
                                        <td align="left"><a href="" ng-click="ShowItems(u.OrderNumber)" style="cursor:pointer">Show Items</a></td>
                                        <td align="left"><a ng-click="CancelItems(u)"  style="cursor:pointer">{{ u.Status == true ?(u.StatusID==50)?"": "Un-Cancel All Items":(u.StatusID==50)?"":"Cancel All Items" }}</a></td>
                                     
                                    </tr>
                                </tbody>
                            </table>
                            <div ng-show="GetPurchaseDetails.length==0" class="aling_center">
                                <p>No order numbers found for this purchase.</p>
                            </div>
                        </fieldset>
                        <asp:Panel ID="pnlCartItems" runat="server" ng-show="GetItemDetails.length!=0" >
                            <br />
                            <br />
                            <fieldset>
                                <%--<legend></legend>--%>
                                    <asp:Label ID="lblCartItemsLegend" runat="server" />
                                <legend class="cartleft"><asp:Label ID="Label1" runat="server">Cart Items for Order #:{{GetItemDetails[0].OrderNumber}}</asp:Label></legend>
                                <%--<asp:GridView ID="gvwCartItems" runat="server" DataKeyNames="CartItemID" AutoGenerateColumns="false" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Profile Name" ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <%# Eval("UserProfile.ProfileName") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ItemName" HeaderText="Item Name" ItemStyle-Width="350px" />
                                        <asp:BoundField DataField="Quantity" HeaderText="Quantity" ItemStyle-Width="100px" />
                                        <asp:BoundField DataField="DeliveryDate" HeaderText="Delivery Date" ItemStyle-Width="150px" DataFormatString="{0:MM/dd/yyyy}" />
                                        <asp:BoundField DataField="IsCancelled" HeaderText="Is Cancelled" ItemStyle-Width="100px" />
                                        <asp:CommandField ShowDeleteButton="true" DeleteText="Cancel Item" />
                                    </Columns>
                                </asp:GridView>--%>
                                <div class="table-responsive">
                                <table class="table table-bordered table-hover">
                                    <thead>
                                        <tr align="left">
                                            <th scope="col">Profile Name</th>
                                            <th scope="col" style="width:45%">Item Name</th>
                                            <th scope="col">Quantity</th>
                                            <th scope="col">Delivery Date</th>
                                            <th scope="col">Is Cancelled</th>
                                            <th scope="col" style="width:13%">&nbsp;</th>
                                        </tr>
                                    </thead>

                                    <tbody>
                                          <tr ng-show="ShowLoader1" class="loader">
                                                <td colspan="6" align="center">
                                                    <img src="/App_Themes/HealthyChef/Images/Blocks-1s-200px.gif" alt="Loading..." />
                                                </td>
                                            </tr>
                                        <tr style="background-color: #FEFEFE;" ng-repeat="u in GetItemDetails">
                                            <td><span>{{ u.ProfileName }}</span></td>
                                            <td>{{ u.ItemName }}</td>
                                            <td>{{ u.Quantity }}</td>
                                            <td>{{ u.DeliveryDate}}</td>
                                            <td>{{ u.IsCancelled==true?"True":"False"}}</td>
                                            <td align="left"><a href="" ng-click="CancelItem(u)">{{u.IsCancelled == true?(GetPurchaseDetails[0].StatusID==50)?"":  "Un-Cancel Item": (GetPurchaseDetails[0].StatusID==50)?"" : "Cancel Item" }}</a></td>
                                        </tr>
                                    </tbody>
                                </table>
                                    </div>
                            </fieldset>
                        </asp:Panel>
                    </asp:Panel>
                </div>
            </div>
            <div class="col-sm-12">
                * Note: Only purchases that have been paid for, and then cancelled, may be "Un-cancelled".
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var app = angular.module('UserApp', ["ui.bootstrap"]);

        //Getting data from API
        app.controller('GetCancellation', function ($scope, $http) {
             
            $scope.GetItemDetails=[];
            var PurchaseNumber = $("#ctl00_Body_txtPurchaseNumber").val();
            GetDetails(PurchaseNumber);
            function GetDetails(PurchaseNumber) {
                $scope.ShowLoader = true;
                $http({
                    method: "GET",
                    url: GetApiBaseURL() + "GetCancellation/" + PurchaseNumber,
                }).then(function mySuccess(response) {
                    $scope.GetPurchaseDetails = response.data;
                    $scope.ShowLoader = false;
                    //$("#PurchaseStatus").text("Purchase #" + PurchaseNumber + "- Customer: " + response.data[0].ProfileName + " - Status: " + response.data[0].Status + "");
                    }, function myError(response) {
                        $scope.ShowLoader = false;
                    $scope.ErrorMessage = response.statusText;
                });
            };

            $scope.ShowItems = function (orderNumber) {
                $scope.ShowLoader1 = true;
                $http({
                    method: "GET",
                    url: GetApiBaseURL() + "CartItemsDetails/" + orderNumber,
                }).then(function mySuccess(response) {
                     
                    $scope.GetItemDetails = response.data;
                    $scope.ShowLoader1 = false;
                    }, function myError(response) {
                        $scope.ShowLoader1 = false;
                    $scope.ErrorMessage = response.statusText;
                });
            }
            $scope.CancelItems = function (u) {
                $scope.ShowLoader = true;
                var iscancel = false;
                if (u.Status==true)
                {
                    iscancel = false;
                }
                else if (u.Status == false) {
                    iscancel = true;
                }
                var PurchaseNumber = $("#ctl00_Body_txtPurchaseNumber").val();
                $http({
                    method: "POST",
                    url: GetApiBaseURL() + "CancelCartItems/" + PurchaseNumber + "/" + u.OrderNumber+ "/" + iscancel,
                }).then(function mySuccess(response) {
                     
                    //$scope.GetItemDetails = response.data;
                    if (response.data.IsSuccess) {
                        //alert(response.data.Message)
                        //window.location.href = "/WebModules/ShoppingCart/Admin/CancellationsManager.aspx";
                        GetDetails(PurchaseNumber);
                        $scope.ShowItems(u.OrderNumber);
                        $scope.ShowLoader = false;
                    }

                    }, function myError(response) {
                        $scope.ShowLoader = false;
                    $scope.ErrorMessage = response.statusText;
                    alert(response.data.Message)

                });
            }
            $scope.CancelItem = function (u) {
                $scope.ShowLoader1 = true;
                var iscancel = false;
                if (u.IsCancelled == true) {
                    iscancel = false;
                }
                else if (u.IsCancelled == false) {
                    iscancel = true;
                }
                $http({
                    method: "POST",
                    url: GetApiBaseURL() + "CancelItemDetails/" + u.CartItemID + "/" + iscancel,
                }).then(function mySuccess(response) {
                     
                    //$scope.GetItemDetails = response.data;
                    if (response.data.IsSuccess) {
                        //alert(response.data.Message)
                        //window.location.href = "/WebModules/ShoppingCart/Admin/CancellationsManager.aspx";
                        GetDetails(PurchaseNumber);
                        $scope.ShowItems(u.OrderNumber);
                        $scope.ShowLoader1 = false;
                    }

                    }, function myError(response) {
                        $scope.ShowLoader1 = false;
                    $scope.ErrorMessage = response.statusText;
                    alert(response.data.Message)

                });
            }
        });
    </script>
    <script type="text/javascript">
        $(function () {
            ToggleMenus('cancellation', 'ordermanagement', 'storemanagement');
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
    top: 300px;
    left: 0;
            }

    </style>
</asp:Content>
