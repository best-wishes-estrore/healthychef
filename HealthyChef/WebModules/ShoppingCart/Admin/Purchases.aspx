<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/Templates/WebModules/Default.master"
    CodeBehind="Purchases.aspx.cs" Inherits="HealthyChef.WebModules.ShoppingCart.Admin.Purchases" Theme="WebModules" %>



<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <%-- <script type="text/javascript">
        $(function () {
            if ($("#hdnShowTabs").val() == 'true') {
                $("#divTabs").tabs({
                    cookie: {
                        // store cookie for a day, without, it would be a session cookie
                        expires: 1
                    }
                });
            }
            else {
                $("#divTabs").hide();
            }
        });
    </script>
    <asp:HiddenField ID="hdnShowTabs" runat="server" ClientIDMode="Static" Value="true" />--%>




    <div class="main-content">
        <div class="main-content-inner">
            <a href="../" onclick="MakeUpdateProg(true);" class="btn btn-info b-10"><< Back to Dashboard</a>
            <div class="page-header">
                <h1>Purchases</h1>
            </div>

            <div class="hcc_admin_ctrl" ng-app="UserApp" ng-controller="Purchases">
                <!-- Begin Search Fields -->
                <div id="divSearchPanel" runat="server">
                    <div class="row-fluid">
                        <div class="col-sm-12">
                            <div class="col-sm-3 align_ment">
                                <div class="form-group">
                                    <label>Purchase Status:</label>
                                    <asp:DropDownList ID="ddlPurchaseType" CssClass="form-control" runat="server" Visible="false">
                                        <asp:ListItem Text="Active(Paid)" Value="1" Selected="True" />
                                        <asp:ListItem Text="Completed" Value="2" />
                                        <asp:ListItem Text="Open" Value="3" />
                                        <asp:ListItem Text="Cancelled" Value="4" />
                                    </asp:DropDownList>
                                    <select id="StatusID" ng-model="StatusFilter">
                                       <%-- <option value="">Select Status</option>--%>
                                        <option value="20">Active(Paid)</option>
                                        <option value="40">Completed</option>
                                        <option value="10">Open</option>
                                        <option value="50">Cancelled</option>
                                    </select>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label class="search">Email:</label>
                                    <asp:TextBox ID="txtSearchEmail" CssClass="form-control" runat="server" MaxLength="100" ng-model="EmailFilter" />
                                </div>
                            </div>
                            <div class="col-sm-3" style="margin-bottom:-10px">
                                <div class="form-group">
                                    <label class="search">Purchase Date:</label>
                                    <asp:TextBox ID="txtSearchPurchaseDate" CssClass="form-control datepicker1" runat="server" MaxLength="20" ng-model="PurchaseDateFilter" placeholder="MM/DD/YYYY" />
                                 <%--   <ajax:CalendarExtender runat="server" TargetControlID="txtSearchPurchaseDate" PopupPosition="TopRight" />--%>
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtSearchPurchaseDate"
                                        Display="Dynamic" ErrorMessage="Purchase Date must be date." Operator="DataTypeCheck" Type="Date"
                                        SetFocusOnError="true" ValidationGroup="PurchaseSearchGroup" />
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label class="search">Purchase #:</label>
                                    <asp:TextBox ID="txtSearchPurchaseNum" CssClass="form-control" runat="server" MaxLength="15" ng-model="PurchaseNumberFilter" />
                                    <asp:CompareValidator ID="cpvPurchaseNum" runat="server" ControlToValidate="txtSearchPurchaseNum"
                                        Display="Dynamic" ErrorMessage="Purchase Number must be numeric." Operator="DataTypeCheck" Type="Integer"
                                        SetFocusOnError="true" ValidationGroup="PurchaseSearchGroup" />
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label class="search">Customer First/Last Name:</label>
                                    <asp:TextBox ID="txtSearchLastName" CssClass="form-control" runat="server" MaxLength="20" ng-model="CustomerNameFilter" />
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label class="search">Delivery Date:</label>
                                    <asp:DropDownList ID="ddlSearchDeliveryDate" CssClass="form-control" runat="server" Visible="false" />
                                    <asp:TextBox ID="DeliveryDateFilter" CssClass="form-control datepicker-fridays" runat="server" MaxLength="20" ng-model="DeliveryDateFilter"  placeholder="MM/DD/YYYY" />
                                    <%--<ajax:CalendarExtender runat="server" TargetControlID="DeliveryDateFilter" PopupPosition="TopRight" Format="MM/dd/yyyy" />--%>
                                    <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="DeliveryDateFilter"
                                        Display="Dynamic" ErrorMessage="Delivery Date must be date." Operator="DataTypeCheck" Type="Date"
                                        SetFocusOnError="true" ValidationGroup="PurchaseSearchGroup" />

                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clearfix"></div>
                    <div class="row-fluid">
                        <div class="col-sm-12">
                            <div class="col-md-5">
                                <asp:Button ID="btnSearch" CssClass="btn btn-info" runat="server" Text="Search Purchases" ValidationGroup="PurchaseSearchGroup" OnClientClick="MakeUpdateProg(true);" Visible="false" />
                                <%--<asp:Button ID="btnClear" Cssclass="btn btn-danger" runat="server" Text="Clear" CausesValidation="false" OnClientClick="MakeUpdateProg(true);" />--%>
                                  <button ng-click="SearchByPurchases()" class="btn btn-info" id="SearchbyPurchases" type="button">Search Purchases</button>
                                <button ng-click="ResetFilters()" class="btn btn-danger" type="button">Clear</button>
                            
                            </div>
                        </div>
                    </div>

                    <div id="divDetails" runat="server" visible="false" clientidmode="Static" style="border: 1px solid #333;">
                        <asp:LinkButton ID="lkbPrint" runat="server" Text="Print" OnClientClick="javascript:PrintContent(this)" />
                        <a onclick="javascript:HideContent()" style="text-decoration: underline; cursor: default;">Hide</a>
                        <div id="divOrderHtml" runat="server" clientidmode="Static"></div>
                    </div>
                </div>
                <!-- End Search Fields -->
                <div id="activeorders">

                    <%--<asp:GridView ID="gvwPaid" runat="server" DataKeyNames="CartID" Visible="false"
                        AllowPaging="true" PageSize="50" AutoGenerateColumns="false" Width="100%"
                        OnPageIndexChanging="gvwPaid_PageIndexChanging" OnSelectedIndexChanging="gvwPaid_SelectedIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="Purchase #">
                                <ItemTemplate>
                                    <%# Eval("PurchaseNumber") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Last Name">
                                <ItemTemplate>
                                    <%# Eval("OwnerProfile.LastName") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email">
                                <ItemTemplate>
                                    <%# Eval("OwnerProfile.ASPUser.Email") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total">
                                <ItemTemplate>
                                    <%# decimal.Parse(Eval("TotalAmount").ToString()).ToString("c") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="PurchaseDate" DataFormatString="{0:MM/dd/yyy}" />
                            <asp:CommandField ShowSelectButton="true" />
                        </Columns>
                        <EmptyDataTemplate>
                            There are no paid purchases on record for this account.
                        </EmptyDataTemplate>
                        <PagerSettings Position="TopAndBottom" />
                    </asp:GridView>--%>
                    <div class="col-sm-12" >
                        <div ng-show="Purchases.length!=0" class="m-2">
                            <%--<label>Count :  {{((page - 1) * pageSize) + 1}} to  {{ (((page - 1) * pageSize) + pageSize) | number }} of {{ displayPurchases.length }}</label>--%>
                             <%--<label ng-show="Purchases.length!=0">Count :  {{((page - 1) * pageSize) + 1}} to  {{ displayPurchases.length <= (((page - 1) * pageSize) + pageSize) ? displayPurchases.length : (((page - 1) * pageSize) + pageSize) | number }} of {{ displayPurchases.length }}</label>--%>
                            <%--<label ng-show="Purchases[0].TotalRecords!=0">Count :  {{((page - 1) * pageSize) + 1}} to  {{(displayPurchases.length < 50 && page>2 )? displayPurchases.length  :(((page - 1) * pageSize)+50)}} of {{Purchases[0].TotalRecords }}</label>--%>
                           <label ng-show="Purchases[0].TotalRecords!=0">Count :  {{((page - 1) * pageSize) + 1}} to  {{ displayPurchases.length< 50 ? Purchases[0].TotalRecords  : (((page - 1) * pageSize) + pageSize) }} of {{ Purchases[0].TotalRecords }}</label>
                            
                            </div>
                      <div ng-show="displayPurchases.length==0" class="aling_center"> <p>There are no {{StatusName}} purchases on record for this account.</p></div>
                       <div ng-show="ShowLoader" class="loader">
                                        <img src="/App_Themes/HealthyChef/Images/Blocks-1s-200px.gif" alt="Loading..." />
                                </div>
                        <table ng-show="Purchases.length!=0" class="table  table-bordered table-hover">
                            <thead>
                                <tr align="left">
                                    <th scope="col" ng-click="orderPurchasesTable('PurchaseNumber')">Purchase #</th>
                                    <th scope="col" ng-click="orderPurchasesTable('CustomerName')">Customer Name</th>
                                    <th scope="col" ng-click="orderPurchasesTable('Email')">Email</th>
                                    <th scope="col" ng-click="orderPurchasesTable('TotalAmount')">Total</th>
                                    <th scope="col" ng-click="orderPurchasesTable('PurchaseDate')">Purchase Date</th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody>

                              <%--  <tr ng-show="ShowLoader">
                                    <td colspan="9" align="center">
                                        <img src="/App_Themes/HealthyChef/Images/Blocks-1s-200px.gif" alt="Loading..." />
                                    </td>
                                </tr>--%>

                                <tr  ng-repeat="purchase in displayPurchases " style="background-color: #FEFEFE;">
                                    <td>{{purchase.PurchaseNumber}}</td>
                                    <td>{{purchase.CustomerName}}</td>
                                    <td>{{purchase.Email}}</td>
                                    <td>${{purchase.TotalAmount}}</td>
                                    <td>{{purchase.PurchaseDate}}</td>
                                    <td style="cursor:pointer"><a ng-click="ShowPurchaseModal(purchase.CartID);">Select</a></td>
                                </tr>
                            </tbody>
                        </table>
                   
                       <%--<div ng-show="(Purchases|filter:{StatusID : StatusFilter}).length==0 && displayPurchases.length!=0" class="aling_center">No Records Found</div>--%>
                       <%-- <ul ng-show="Purchases.length!=0" uib-pagination class="pagination-sm pagination" total-items="filteredPurchases.length" ng-model="page"
                            ng-click="pageChanged()" previous-text="&lsaquo;" next-text="&rsaquo;" items-per-page="pageSize" max-size="10" boundary-links="true">
                        </ul>--%>
                     <ul ng-show="Purchases.length!=0" uib-pagination class="pagination-sm pagination" ng-model="page" total-items="Purchases[0].TotalRecords"
                    ng-click="pageChanged()" previous-text="&lsaquo;" next-text="&rsaquo;" items-per-page="pageSize" max-size="10" boundary-links="true">
                </ul>
                   
                   
                </div>
                <div id="completedorders">
                    <%--<asp:GridView ID="gvwComplete" runat="server" DataKeyNames="CartID" Visible="false"
                        AllowPaging="true" PageSize="50" AutoGenerateColumns="false" Width="100%"
                        OnPageIndexChanging="gvwComplete_PageIndexChanging" OnSelectedIndexChanging="gvwComplete_SelectedIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="Purchase #">
                                <ItemTemplate>
                                    <%# Eval("PurchaseNumber") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Last Name">
                                <ItemTemplate>
                                    <%# Eval("OwnerProfile.LastName") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email">
                                <ItemTemplate>
                                    <%# Eval("OwnerProfile.ASPUser.Email") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total">
                                <ItemTemplate>
                                    <%# decimal.Parse(Eval("TotalAmount").ToString()).ToString("c") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Purchase Date">
                                <ItemTemplate>
                                    <%# DateTime.Parse(Eval("PurchaseDate").ToString()).ToShortDateString() %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowSelectButton="true" />
                        </Columns>
                        <EmptyDataTemplate>
                            There are no completed purchases on record for this account.
                        </EmptyDataTemplate>
                        <PagerSettings Position="TopAndBottom" />
                    </asp:GridView>--%>
                </div>
                <div id="openorders">
                    <%--<asp:GridView ID="gvwOpen" runat="server" DataKeyNames="CartID" Visible="false"
                        AllowPaging="true" PageSize="50" AutoGenerateColumns="false" Width="100%"
                        OnPageIndexChanging="gvwOpen_PageIndexChanging" OnSelectedIndexChanging="gvwOpen_SelectedIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="Purchase #">
                                <ItemTemplate>
                                    <%# Eval("PurchaseNumber") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Last Name">
                                <ItemTemplate>
                                    <%# Eval("OwnerProfile.LastName") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email">
                                <ItemTemplate>
                                    <%# Eval("OwnerProfile.ASPUser.Email") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total">
                                <ItemTemplate>
                                    <%# decimal.Parse(Eval("TotalAmount").ToString()).ToString("c") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Create Date">
                                <ItemTemplate>
                                    <%# DateTime.Parse(Eval("CreatedDate").ToString()).ToShortDateString() %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowSelectButton="true" />
                        </Columns>
                        <EmptyDataTemplate>
                            There are no open purchases on record for this account.
                        </EmptyDataTemplate>
                        <PagerSettings Position="TopAndBottom" />
                    </asp:GridView>--%>
                </div>
                <div id="cancelorders">
                    <%--<asp:GridView ID="gvwCancel" runat="server" DataKeyNames="CartID" Visible="false"
                        AllowPaging="true" PageSize="50" AutoGenerateColumns="false" Width="100%"
                        OnPageIndexChanging="gvwCancel_PageIndexChanging" OnSelectedIndexChanging="gvwCancel_SelectedIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="Purchase #">
                                <ItemTemplate>
                                    <%# Eval("PurchaseNumber") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Last Name">
                                <ItemTemplate>
                                    <%# Eval("OwnerProfile.LastName") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email">
                                <ItemTemplate>
                                    <%# Eval("OwnerProfile.ASPUser.Email") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total">
                                <ItemTemplate>
                                    <%# decimal.Parse(Eval("TotalAmount").ToString()).ToString("c") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Created Date">
                                <ItemTemplate>
                                    <%# DateTime.Parse(Eval("CreatedDate").ToString()).ToShortDateString() %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowSelectButton="true" />
                        </Columns>
                        <EmptyDataTemplate>
                            There are no cancelled purchases on record for this account.
                        </EmptyDataTemplate>
                        <PagerSettings Position="TopAndBottom" />
                    </asp:GridView>--%>
                </div>
                <%-- </div>--%>

                <div class="modal fade" id="PurchaseDetails" role="dialog">
                    <div class="modal-dialog modal-lg">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" ng-click="HidePurchaseModal();">&times;</button>
                                <h4 class="modal-title">{{PurchaseHeader}}</h4>
                            </div>
                            <div class="modal-body" ng-bind-html="PurchaseDetailsHTML">
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-danger" data-dismiss="modal" ng-click="HidePurchaseModal();">Close</button>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>


    <script type="text/javascript">
        var app = angular.module('UserApp', ["ui.bootstrap"]);
        function PrintContent(element) {
            var DocumentContainer = $("#divOrderHtml");
            var WindowObject = window.open('', "PrintOrderInvoice", "width=740,height=325,top=200,left=250,toolbars=no,scrollbars=yes,status=no,resizable=no");
            WindowObject.document.writeln(DocumentContainer[0].innerHTML);
            WindowObject.document.close();
            WindowObject.focus();
            WindowObject.print();
            WindowObject.close();
        }

        function HideContent() {
            $('#divDetails').hide();
        }
        //Getting data from API
        app.controller('Purchases', function ($scope, $http, $sce) {
            $scope.Purchases=[];
            $scope.page = 1;
            $scope.pageSize = 50;
            $scope.TempPurchases = [];
            $scope.StatusFilter = "20";
           
            $scope.SearchByPurchases = function ()
            {
                $scope.ShowLoader = true;
                if (($('#ctl00_Body_CompareValidator2').css('display') == 'none') && ($('#ctl00_Body_CompareValidator1').css('display') == 'none' ) ){
                    $scope.page = 1;
                    $scope.pageSize = 50;
                    var lastName = $scope.CustomerNameFilter;
                    var email = $scope.EmailFilter;
                    var Purchasedate = $scope.PurchaseDateFilter;
                    var purchNum = $scope.PurchaseNumberFilter;
                    var delivDate = $scope.DeliveryDateFilter;
                    var StatusID = $scope.StatusFilter;
                    var pagenumber = $scope.page;
                    var pagesize = $scope.pageSize;
                    var totalrecord = 0;
                    var searchparameters = { StatusId: StatusID === undefined ? '0' : StatusID, lastName: lastName === undefined ? '' : lastName, email: email === undefined ? '' : email, Purchasedate: Purchasedate === undefined ? '' : Purchasedate, purchaseNumber: purchNum === undefined ? '' : purchNum, deliveryDate: delivDate === undefined ? '' : delivDate, pagenumber: pagenumber, pagesize: pagesize, totalrecords: totalrecord };
                    $http({
                        method: "POST",
                        url: GetApiBaseURL() + "GetAllPurchases",
                        data: searchparameters
                    }).then(function mySuccess(response) {
                        $scope.Purchases = response.data;
                        $scope.displayPurchases = $scope.Purchases.slice(0, $scope.pageSize);
                        $scope.totalRecords = $scope.Purchases.length;
                        console.log($scope.totalRecords);
                        $scope.ShowLoader = false;
                        $scope.StatusName = $("#StatusID option:selected").text();
                    }, function myError(response) {
                        $scope.ErrorMessage = response.statusText;
                        $scope.ShowLoader = false;
                    });
                }
                else {
                    $scope.ShowLoader = false;
                    $scope.StatusName = $("#StatusID option:selected").text();
                    $scope.Purchases = [];
                    $scope.displayPurchases = [];
                }
            }
            $scope.pageChanged = function () {
                
                $scope.ShowLoader = true;
                if (($('#ctl00_Body_CompareValidator2').css('display') == 'none') && ($('#ctl00_Body_CompareValidator1').css('display') == 'none')){
                    //var startPos = ($scope.page - 1) * $scope.pageSize;
                    //$scope.displayPurchases = $scope.Purchases.slice(startPos, startPos + $scope.pageSize);
                    //if ($scope.TempPurchases.length != 0) {
                    //    $scope.Purchases = $scope.TempPurchases;
                    //    $scope.displayPurchases = $scope.Purchases.slice(($scope.page - 1) * ($scope.pageSize), ($scope.pageSize) * ($scope.page));
                    //    $scope.ShowLoader = false;
                    //}
                    //else {
                        var lastName = $scope.CustomerNameFilter;
                        var email = $scope.EmailFilter;
                        var Purchasedate = $scope.PurchaseDateFilter;
                        var purchNum = $scope.PurchaseNumberFilter;
                        var delivDate = $scope.DeliveryDateFilter;
                        var StatusID = $scope.StatusFilter;
                        var pagenumber = $scope.page;
                        var pagesize = 50;
                        var totalrecord = 0;
                        var searchparameters = { StatusId: StatusID === undefined ? '20' : StatusID, lastName: lastName === undefined ? '' : lastName, email: email === undefined ? '' : email, Purchasedate: Purchasedate === undefined ? '' : Purchasedate, purchaseNumber: purchNum === undefined ? '' : purchNum, deliveryDate: delivDate === undefined ? '' : delivDate, pagenumber: pagenumber, pagesize: pagesize, totalrecords: totalrecord };

                        $http({
                            method: "POST",
                            url: GetApiBaseURL() + "GetAllPurchases",
                            data: searchparameters
                        }).then(function mySuccess(response) {
                            $scope.Purchases = response.data;
                            if ($scope.Purchases.length > 51) {
                                $scope.TempPurchases = $scope.Purchases;
                                $scope.displayPurchases = $scope.Purchases.slice(($scope.page - 1) * ($scope.pageSize), ($scope.pageSize) * ($scope.page));
                            }
                            else {
                                $scope.displayPurchases = $scope.Purchases.slice(0, $scope.pageSize);
                            }
                            console.log($scope.totalRecords);
                            $scope.ShowLoader = false;
                            $scope.StatusName = $("#StatusID option:selected").text();
                        }, function myError(response) {
                            $scope.ErrorMessage = response.statusText;
                            $scope.ShowLoader = false;
                        });
                    //}
                }
                else {
                    $scope.ShowLoader = false;
                    $scope.StatusName = $("#StatusID option:selected").text();
                    $scope.Purchases = [];
                    $scope.displayPurchases = [];
                }
            };

            $scope.orderPurchasesTable = function (x) {
                 
                $scope.reverse = ($scope.OrderByField === x) ? !$scope.reverse : false;
                $scope.OrderByField = x;
            };

            $scope.ResetFilters = function () {
                 
                $scope.CustomerNameFilter = '';
                $scope.PurchaseDateFilter = '';
                $scope.EmailFilter = '';
                $scope.PurchaseNumberFilter = '';
                $scope.StatusFilter =$scope.StatusFilter;//"20";
                $scope.DeliveryDateFilter = '';
                $('#ctl00_Body_CompareValidator1').hide();
                $('#ctl00_Body_CompareValidator2').hide();
            };

            $scope.ShowPurchaseModal = function (CartID) {
                $http({
                    method: "GET",
                    url: GetApiBaseURL() + "PurchaseDetails/" + CartID,
                }).then(function mySuccess(response) {
                     
                    if (response.data.IsSuccess) {
                        $scope.PurchaseHeader = 'Cart #' + CartID;
                        $scope.PurchaseDetailsHTML = $sce.trustAsHtml(response.data.Message);
                        $('#PurchaseDetails').modal('show');
                    }
                    else {
                        alert(response.data.Message);
                    }
                }, function myError(response) {
                    alert(response.data.Message);
                });
            };

            $scope.HidePurchaseModal = function () {
                $('#PurchaseDetails').modal('hide');
                $scope.PurchaseHeader = '';
                $scope.PurchaseDetailsHTML = '';
            };
        });
    </script>
    <script type="text/javascript">
        $(function () {
            ToggleMenus('purchases', 'ordermanagement', 'storemanagement');
            preventResfreshOnEnter();
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
    </style>
</asp:Content>

