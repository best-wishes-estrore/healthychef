<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderFulfillment_Search.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.OrderFulfillment_Search" %>



<!-- Begin Search Fields -->
<div ng-app="UserApp" ng-controller="OrderFullfillment" ng-init="InitCall();">
    <div id="divSearchPanel" runat="server">
        <div class="col-sm-12">
            <div class="col-sm-3">
                <div class="form-group">
                    <label class="search">Delivery Date:</label>
                    <asp:DropDownList ID="ddlDelDates" CssClass="form-control" runat="server" />
                </div>
            </div>
            <div class="col-sm-2 m-2 p-5">
                <button class="btn btn-info" type="button" ng-click="SearchOrders()">Search</button>
            </div>
        </div>
        <div class="col-sm-12">
            <div class="row-fluid">
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="search">Purchase #: </label>
                        <asp:TextBox ID="txtSearchPurchNum" CssClass="form-control" runat="server" MaxLength="15" ng-model="PurchaseNumberFilter" />
                        <asp:CompareValidator ErrorMessage="Purchase Number must be numeric." ControlToValidate="txtSearchPurchNum" runat="server"
                            ValidationGroup="OrderFulfillSearchGroup" Operator="DataTypeCheck" Type="String" Display="Dynamic" SetFocusOnError="true" />
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="search">Customer Name:</label>
                        <asp:TextBox ID="txtSearchLastName" CssClass="form-control" runat="server" MaxLength="50" ng-model="CustomerNameFilter" />
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="search">Email: </label>
                        <asp:TextBox ID="txtSearchEmail" CssClass="form-control" runat="server" MaxLength="100" ng-model="EmailFilter" />
                    </div>
                </div>

                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="search">Status: </label>
                        <br />
                        <asp:DropDownList ID="ddlStatus" CssClass="form-control" runat="server" Visible="false">
                            <asp:ListItem Value="0" Text="Selected a Status..." />
                            <asp:ListItem Value="1" Text="Complete" />
                            <asp:ListItem Value="2" Text="Incomplete" />
                            <asp:ListItem Value="3" Text="Cancelled" />
                        </asp:DropDownList>
                        <select id="StatusID" ng-model="StatusFilter" style="width: 100%">
                            <option value="">Select Status</option>
                            <option value="1">Complete</option>
                            <option value="2">Incomplete</option>
                            <option value="3">Cancelled</option>
                        </select>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group">
                        <label class="search">Type: </label>
                        <asp:DropDownList ID="ddlTypes" CssClass="form-control" runat="server" Visible="false">
                            <asp:ListItem Value="0" Text="Selected a Type..." />
                            <asp:ListItem Value="1" Text="A La Carte" />
                            <asp:ListItem Value="3" Text="Gift Certificate" />
                            <asp:ListItem Value="2" Text="Program Plan" />
                        </asp:DropDownList>
                        <select id="ItemType" ng-model="TypeFilter" style="width: 100%">
                            <option value="">Selected a Type...</option>
                            <option value="1">A La Carte</option>
                            <option value="3">Gift Certificate</option>
                            <option value="2">Program Plan</option>
                        </select>
                    </div>
                </div>
                <%--<div class="clearfix"></div>--%>
                <div class="col-sm-3 m-2 p-5">
                    <%-- <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-info" Text="Search" ValidationGroup="OrderFulfillSearchGroup" />--%>
               
                &nbsp;
       <%-- <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-danger" CausesValidation="false" UseSubmitBehavior="false" />--%>
                    <button class="btn btn-danger" type="button" ng-click="ResetFilters()">Clear</button>
                    &nbsp;
       
                    <asp:ValidationSummary runat="server" ValidationGroup="OrderFulfillSearchGroup" />
                </div>
                 <div class="col-sm-2 pull-right m-2 p-5">
            <asp:Button ID="btnCreateShippingFile" runat="server" CssClass="btn btn-info" Text="Export Shipping File" ValidationGroup="OrderFulfillSearchGroup" />
        </div>
            </div>
        </div>
    </div>
    <div class="clearfix"></div>
    <!-- End Search Fields -->
    <%--<asp:ListView ID="lvwOrders" runat="server" DataKeyNames="CartItemID, DeliveryDate">
    <LayoutTemplate>
        <div class="row-fluid">
            <div class="m-2">
                <table class="table  table-bordered table-hover">
                    <tr>
                        <th>Order #</th>
                        <th>Customer Name</th>
                        <th>Type</th>
                        <th>Qty</th>
                        <th>Delivery Date</th>
                        <th>Status</th>
                        <th>Allergens</th>
                        <th>&nbsp;</th>
                        <th></th>
                    </tr>
                    <tr id="itemPlaceHolder" runat="server" />
                </table>
            </div>
        </div>
    </LayoutTemplate>
    <ItemTemplate>
        <tr>
            <td><%# Eval("CartItem.OrderNumber") %></td>
            <td><%# Eval("CartItem.UserProfile.ParentProfileName") %></td>
            <td>
                <asp:Label ID="lblSimpleName" runat="server" Text='<%# Eval("SimpleName") %>' />
            </td>
            <td><%# Eval("TotalQuantity") %></td>
            <td><%# ((DateTime)Eval("DeliveryDate")).ToShortDateString() %></td>
            <td><%# Eval("SimpleStatus") %></td>
            <td>
                <asp:PlaceHolder ID="plcAllergies" runat="server" />
            </td>
            <td>
                <asp:PlaceHolder ID="plcDetails" runat="server" />
            </td>
            <td>
                <asp:LinkButton ID="lkbPostpone" runat="server" Text="Postpone" OnClick="lkbPostpone_Click"
                    Visible="false" />
            </td>
        </tr>
    </ItemTemplate>
    <AlternatingItemTemplate>
        <tr style="background-color: #eee;">
            <td><%# Eval("CartItem.OrderNumber") %></td>
            <td><%# Eval("CartItem.UserProfile.ParentProfileName") %></td>
            <td>
                <asp:Label ID="lblSimpleName" runat="server" Text='<%# Eval("SimpleName") %>' />
            </td>
            <td><%# Eval("TotalQuantity") %></td>
            <td><%# ((DateTime)Eval("DeliveryDate")).ToShortDateString() %></td>
            <td><%# Eval("SimpleStatus") %></td>
            <td>
                <asp:PlaceHolder ID="plcAllergies" runat="server" />
            </td>
            <td>
                <asp:PlaceHolder ID="plcDetails" runat="server" />
            </td>
            <td>
                <asp:LinkButton ID="lkbPostpone" runat="server" Text="Postpone" OnClick="lkbPostpone_Click"
                    Visible="false" />
            </td>
        </tr>
    </AlternatingItemTemplate>
    <EmptyDataTemplate>There are no orders for this date.</EmptyDataTemplate>
</asp:ListView>--%>
    <div class="col-sm-12">
        <div class="p-5">
            <%--<label>Count :  {{((page - 1) * pageSize) + 1}} to  {{ (((page - 1) * pageSize) + pageSize) | number }} of {{ displayOrders.length }}</label>--%>
            <label ng-show="Orders.length!=0">Count :  {{((page - 1) * pageSize) + 1}} to  {{ displayOrders.length <= (((page - 1) * pageSize) + pageSize) ? displayOrders.length : (((page - 1) * pageSize) + pageSize) | number }} of {{ displayOrders.length }}</label>
        </div>
         <div ng-show="ShowLoader" class="loader">
                    <img src="/App_Themes/HealthyChef/Images/Blocks-1s-200px.gif" alt="Loading..." />
                </div>
        <table ng-show="totalRecords!=0" class="table  table-bordered table-hover">
            <thead>
                <tr>
                    <th scope="col" ng-click="orderfullFillmentTable('OrderNum')">Order #</th>
                    <th scope="col" ng-click="orderfullFillmentTable('CustomerName')">Customer Name</th>
                    <th scope="col" ng-click="orderfullFillmentTable('Type')">Type</th>
                    <th scope="col" ng-click="orderfullFillmentTable('Quantity')">Qty</th>
                    <th scope="col" ng-click="orderfullFillmentTable('DeliveryDate')">Delivery Date</th>
                    <th scope="col" ng-click="orderfullFillmentTable('Status')">Status</th>
                    <th scope="col" ng-click="orderfullFillmentTable('Allergens')">Allergens</th>
                    <th colspan="2"></th>
                </tr>
            </thead>
            <tbody>
                <tr ng-repeat="order in displayOrders = (filteredOrders = (Orders | orderBy:OrderByField : reverse )) | limitTo:pageSize:pageSize*(page-1) ">
                <%--<tr ng-repeat="order in displayOrders =Orders| limitTo:pageSize:pageSize*(page-1) ">--%>
                    <td>{{order.OrderNum}}</td>
                    <td>{{order.CustomerName}}</td>
                    <td>{{order.Type}}</td>
                    <td>{{order.Quantity}}</td>
                    <td>{{order.DeliveryDate}}</td>
                    <td>{{order.Status}}</td>
                    <td>{{order.Allergens}}</td>
                    <td><a href="" ng-click="Postpone(order)" ng-show="{{order.Postpone}}">Postpone</a> </td>
                    <td><a href="/WebModules/ShoppingCart/Admin/OrderFulfillmentEditor.aspx?ci={{order.CartItemID}}&dd={{order.DeliveryDate}}&it={{order.ItemType}}">Details</a> </td>
                </tr>
            </tbody>
        </table>
      <%--  <div ng-show="(Orders|filter:{StatusID : StatusFilter}).length==0 && Orders.length!=0" class="aling_center">There are no orders for this date.</div>--%>
        <div ng-show="totalRecords==0" class="aling_center">
            <p>There are no orders for this date.</p>
        </div>
           <div ng-show=" displayOrders.length==0 && Orders.length!=0"  class="aling_center">There are no orders for this date.</div>
       
        <ul ng-show="Orders.length!=0" uib-pagination class="pagination-sm pagination" total-items="filteredOrders.length" ng-model="page"
            ng-click="pageChanged()" previous-text="&lsaquo;" next-text="&rsaquo;" items-per-page="pageSize" max-size="10" boundary-links="true">
        </ul>
    </div>
</div>
<script>
    var app = angular.module('UserApp', ["ui.bootstrap"]);
    app.controller('OrderFullfillment', function ($scope, $http) {
        
        $scope.page = 1;
        $scope.pageSize = 50;
        $scope.ShowLoader = true;

        $scope.InitCall = function () {
            var deliverydate = $("#ctl00_Body_OrderFulfillment_Search1_ddlDelDates option:selected").text();
            var lastName = $scope.CustomerNameFilter;
            var email = $scope.EmailFilter;
            var purchNum = $scope.PurchaseNumberFilter;
            var ddlStatusId = $("#StatusID").val();
            var ddlTypesId = $("#ItemType").val();
            var ddlStatusValue = $("#StatusID option:selected").text();
            var ddlTypesValue = $("#ItemType option:selected").text();
            var searchparameters = { lastName: lastName === undefined ? '' : lastName, email: email === undefined ? '' : email, purchaseNumber: purchNum === undefined ? '' : purchNum, deliveryDate: deliverydate === undefined ? '' : deliverydate, ddlStatusId: ddlStatusId, ddlTypesId: ddlTypesId, ddlStatusValue: ddlStatusValue, ddlTypesValue: ddlTypesValue};
            $scope.GetData(searchparameters);
        };

        $scope.GetData = function (searchparameters) {

            $scope.Orders = [];
            $scope.ShowLoader = true;
            $http({
                method: "POST",
                url: GetApiBaseURL() + "GetOrderFullfillment",
                data: searchparameters
            }).then(function mySuccess(response) {

                $scope.Orders = response.data;
                if ($scope.Orders.length > 0)
                {
                    $("#ddlStatus").val(response.data[0].Status);
                    $("#ddlTypes").val(response.data[0].Type);
                }
                //append status ids
                //$scope.AppendStatusIds();

                $scope.displayOrders = $scope.Orders.slice(0, $scope.pageSize);
                $scope.totalRecords = $scope.Orders.length;
                console.log($scope.totalRecords);
                $scope.ShowLoader = false;
            }, function myError(response) {
                $scope.ErrorMessage = response.statusText;
                $scope.ShowLoader = false;
            });
        };

        $scope.pageChanged = function () {

            var startPos = ($scope.page - 1) * $scope.pageSize;
            $scope.displayOrders = $scope.Orders.slice(startPos, startPos + $scope.pageSize);
        };
        $scope.orderfullFillmentTable = function (x) {

            $scope.reverse = ($scope.OrderByField === x) ? !$scope.reverse : false;
            $scope.OrderByField = x;
        };
        $scope.SearchOrders = function () {
            var deliverydate = $("#ctl00_Body_OrderFulfillment_Search1_ddlDelDates option:selected").text();
            var lastName = $scope.CustomerNameFilter;
            var email = $scope.EmailFilter;
            var purchNum = $scope.PurchaseNumberFilter;
            var ddlStatusId = $("#StatusID").val();
            var ddlTypesId = $("#ItemType").val();
            var ddlStatusValue = $("#StatusID option:selected").text();
            var ddlTypesValue = $("#ItemType option:selected").text();
            var searchparameters = { lastName: lastName === undefined ? '' : lastName, email: email === undefined ? '' : email, purchaseNumber: purchNum === undefined ? '' : purchNum, deliveryDate: deliverydate === undefined ? '' : deliverydate, ddlStatusId: ddlStatusId, ddlTypesId: ddlTypesId, ddlStatusValue: ddlStatusValue, ddlTypesValue: ddlTypesValue };
            $scope.GetData(searchparameters);
        };
        $scope.ResetFilters = function () {

            $scope.CustomerNameFilter = '';
            $scope.PurchaseNumberFilter = '';
            $scope.StatusFilter = '';
            $scope.TypeFilter = '';
            $scope.EmailFilter = '';
            //$scope.date = '';
        };

        $scope.AppendStatusIds = function () {

            ////for status id
            angular.forEach($scope.Orders, function (order, index) {
                order.StatusID = 0;
                if (order.Status === 'Complete') {
                    order.StatusID = 1;
                }
                else if (order.Status === 'Incomplete') {
                    order.StatusID = 2;
                }
                else if (order.Status === 'Cancelled') {
                    order.StatusID = 3;
                }
            });

        };

        $scope.Postpone = function (order) {
            var id = order.CartItemID;
            var date = order.DeliveryDateObj.split('T')[0];

            if (confirm("Are you sure that you want to postpone this delivery? This cannot be undone.")) {
                $http({
                    method: "GET",
                    url: GetApiBaseURL() + "PostponeOrderFullfilment/" + id + "/" + date
                    //data: certificate
                }).then(function mySuccess(response) {
                    if (response.data.IsSuccess) {
                        //$scope.GetData(deliverydate);
                        $scope.InitCall();
                        alert(response.data.Message);
                    }
                }, function myError(response) {

                    if (response.data.IsSuccess == false) {
                        alert(response.data.Message)
                    }
                });
            }
        };

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
             /*table { table-layout: fixed; }
 table th, table td { overflow: hidden; }*/
        /*.pagination {
                margin-top: 16%;
        }*/
    </style>