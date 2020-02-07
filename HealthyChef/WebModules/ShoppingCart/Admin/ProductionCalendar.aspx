<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/WebModules/Default.master"
    Theme="WebModules" AutoEventWireup="true" CodeBehind="ProductionCalendar.aspx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.ProductionCalendar" %>

<%@ Register TagPrefix="hcc" TagName="ProdCalendar" Src="~/WebModules/ShoppingCart/Admin/UserControls/ProductionCalendar_Edit.ascx" %>
<asp:Content ID="header" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#divPlanTabs").tabs({
                cookie: {
                    // store cookie for a day, without, it would be a session cookie
                    expires: 1
                }
            });
        });
    </script>
    <div class="main-content" ng-app="UserApp" ng-controller="ProductionCalendar" ng-init="Getdata()">
        <asp:HiddenField runat="server" ID="CurrentCalendarId" />
        <div class="main-content-inner">
            <a href="../" onclick="MakeUpdateProg(true);" class="btn btn-info b-10"><< Back to Dashboard</a>
            <div class="page-header">
                <h1>Production Calendar</h1>
            </div>
            <div class="hcc_admin_ctrl">
                <div>
                    <asp:Label ID="lblFeedback" runat="server" ForeColor="Green" Font-Bold="true" EnableViewState="false" />
                </div>
                <div class="pull-right m-2">
                    <div class="col-sm-12">
                        <div class="col-sm-12">
                            <asp:Button ID="btnAddNewProdCal" CssClass="btn btn-info" runat="server" Text="Add New Delivery Date" />

                            <asp:Button ID="btnBatchNewProdCals" CssClass="btn btn-info" runat="server" Text="Create 1 Year of Delivery Dates From Last Known Delivery Date (Fridays Only)" />
                        </div>
                    </div>
                </div>
                <asp:Panel ID="pnlEditProdCal" runat="server" Visible="false">
                    <hcc:ProdCalendar runat="server" ID="ProdCalendar1" ValidationGroup="ProdCalendarEditGroup"
                        ShowDeactivate="false" />
                </asp:Panel>
                <div class="clearfix"></div>
                <asp:Panel ID="pnlProdCalList" runat="server" CssClass="col-sm-12">
                    <div id="divPlanTabs">
                        <ul>
                            <li><a href="#active">Future Calendars</a></li>
                            <li><a href="#past">Past Calendars</a></li>
                        </ul>
                        <div id="active">
                            <fieldset class="desc">
                                <asp:ValidationSummary ID="VAlSum1" runat="server" ValidationGroup="FinGCGroup" />
                                <legend>Find a Delivery Date</legend>
                                <div class="col-sm-3">
                                    <div class="form-group">
                                        Start Date:&nbsp;
                                        <asp:TextBox runat="server" CssClass="datepicker" ID="txtStartDate" ng-model="startDate" ValidationGroup="FinGCGroup" />
                                        <%--<ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate" PopupPosition="TopRight" Format="MM-dd-yyyy" />--%>
                                        <asp:CompareValidator ID="cmp1" runat="server" ControlToValidate="txtStartDate"
                                            Display="Dynamic" ErrorMessage="Start Date must be a date value." SetFocusOnError="true"
                                            Operator="DataTypeCheck" Type="Date" ValidationGroup="FinGCGroup" CssClass="validation-error" />
                                        &nbsp;&nbsp; 
                                    </div>
                                </div>
                                <div class="col-sm-3">
                                    <div class="form-group">
                                        End Date:&nbsp;                                                                                                                                        
                                        <asp:TextBox runat="server" CssClass="datepicker" ID="txtEndDate" ng-model="endDate" ValidationGroup="FinGCGroup" />
                                        <%--<ajax:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate" PopupPosition="TopRight" Format="MM-dd-yyyy" />--%>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtEndDate"
                                            Display="Dynamic" ErrorMessage="End Date must be a date value." SetFocusOnError="true"
                                            Operator="DataTypeCheck" Type="Date" ValidationGroup="FinGCGroup" CssClass="validation-error" />
                                        &nbsp;&nbsp; 
                                    </div>
                                </div>

                                <div class="col-sm-3 m-2 filter-control">
                                    <asp:Button runat="server" CssClass="btn btn-info" ID="btnFind" Text="Find" Visible="false" />
                                    <button type="button" ng-click="Getdata();" class="btn btn-info" id="FindCalendarsBydate">Find</button>
                                    <button ng-click="ResetFilters()" class="btn btn-danger" id="UserFiltersReset" type="button">Clear</button>
                                </div>

                                <div class="col-sm-12">
                                    <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="txtEndDate" ControlToCompare="txtStartDate"
                                        Display="Dynamic" ErrorMessage="Start Date must occur before End Date." SetFocusOnError="true"
                                        Operator="GreaterThan" Type="Date" ValidationGroup="FinGCGroup" CssClass="validation-error" />
                                </div>
                            </fieldset>
                            <br />
                            <!--<asp:GridView ID="gvwProdCalendars" runat="server" CssClass="table table-bordered table-hover" DataKeyNames="CalendarID" AutoGenerateColumns="false" Visible="false"
                                Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="200px" />
                                    <asp:BoundField DataField="DeliveryDate" HeaderText="Delivery Date" ItemStyle-Width="200px" />
                                    <asp:BoundField DataField="OrderCutOffDate" HeaderText="Order Cut-Off Date" ItemStyle-Width="200px" />
                                    <asp:BoundField DataField="MenuName" HeaderText="Menu" />
                                    <asp:CommandField ShowSelectButton="true" SelectText="Edit" ItemStyle-HorizontalAlign="Right" />
                                </Columns>
                                <EmptyDataTemplate>
                                    There are currently no active calendars
                                </EmptyDataTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <AlternatingRowStyle BackColor="#eeeeee" />
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" />
                            </asp:GridView>-->
                            <div>
                                <div class="m-2">
                                    <%--<label>Count :  {{((page - 1) * pageSize) + 1}} to  {{ (((page - 1) * pageSize) + pageSize) | number }} of {{ futureCalendar.length }}</label>--%>
                                    <label ng-show="futureCalendar.length!=0">Count :  {{((page - 1) * pageSize) + 1}} to  {{ futureCalendar.length <= (((page - 1) * pageSize) + pageSize) ? futureCalendar.length : (((page - 1) * pageSize) + pageSize) | number }} of {{ futureCalendar.length }}</label>
                                </div>
                                <table ng-show="futureCalendar.length!=0" class="table  table-bordered table-hover">
                                    <thead>
                                        <tr align="left">
                                            <th scope="col" ng-click="orderFutureTable('Name')">Name</th>
                                            <th scope="col" ng-click="orderFutureTable('DeliveryDateObj')">Delivery Date</th>
                                            <th scope="col" ng-click="orderFutureTable('DeliveryDateObj')">Order Cut-Off Date</th>
                                            <th scope="col" ng-click="orderFutureTable('Menu')">Menu</th>
                                            <th></th>
                                        </tr>
                                    </thead>
                                    <tbody>

                                        <tr ng-show="ShowLoaderFuture">
                                            <td colspan="6" align="center">
                                                <img src="/App_Themes/HealthyChef/Images/Blocks-1s-200px.gif" alt="Loading..." />
                                            </td>
                                        </tr>

                                        <tr ng-repeat="calendar in displayfutureCalendar = ( futureCalendar | orderBy:OrderByFutureField : Futurereverse) | limitTo:pageSize:pageSize*(page-1) " style="background-color: #FEFEFE;">

                                            <td>{{calendar.Name}}</td>
                                            <td>{{calendar.DeliveryDate}}</td>
                                            <td>{{calendar.OrderCutOffDate}}</td>
                                            <td>{{calendar.Menu}}</td>
                                            <td><a href="/WebModules/ShoppingCart/Admin/ProductionCalendar.aspx?CalendarId={{calendar.CalendarId}}">Edit</a></td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div ng-show="futureCalendar.length==0" class="aling_center">
                                    <p> There are currently no active calendars</p>
                                </div>
                                <ul ng-show="futureCalendar.length!=0" uib-pagination class="pagination-sm pagination" total-items="futureCalendar.length" ng-model="page"
                                    ng-click="pageChanged()" previous-text="&lsaquo;" next-text="&rsaquo;" items-per-page="pageSize" max-size="10" boundary-links="true">
                                </ul>
                            </div>
                        </div>
                        <div id="past">
                            <%--<asp:GridView ID="gvwPastCalendars" runat="server" CssClass="table table-bordered table-hover" DataKeyNames="CalendarID" AutoGenerateColumns="false"
                                Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="200px" />
                                    <asp:BoundField DataField="DeliveryDate" HeaderText="Delivery Date" ItemStyle-Width="200px" />
                                    <asp:BoundField DataField="OrderCutOffDate" HeaderText="Order Cut-Off Date" ItemStyle-Width="200px" />
                                    <asp:BoundField DataField="MenuName" HeaderText="Menu" />
                                    <asp:CommandField ShowSelectButton="true" SelectText="Edit" ItemStyle-HorizontalAlign="Right" />
                                </Columns>
                                <EmptyDataTemplate>
                                    There are currently no active calendars
                                </EmptyDataTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <AlternatingRowStyle BackColor="#eeeeee" />
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" />
                            </asp:GridView>--%>
                            <div>
                                <%--<label>Count :  {{((page1 - 1) * pageSize) + 1}} to  {{ (((page1 - 1) * pageSize) + pageSize) | number }} of {{ pastCalendar.length }}</label>--%>
                                <label ng-show="pastCalendar.length!=0">Count :  {{((page1 - 1) * pageSize) + 1}} to  {{ pastCalendar.length <= (((page1 - 1) * pageSize) + pageSize) ? pastCalendar.length : (((page1 - 1) * pageSize) + pageSize) | number }} of {{ pastCalendar.length }}</label>

                                <table ng-show="pastCalendar.length!=0" class="table  table-bordered table-hover">
                                    <thead>
                                        <tr align="left">
                                            <th scope="col" ng-click="orderPastTable('Name')">Name</th>
                                            <th scope="col" ng-click="orderPastTable('DeliveryDateObj')">Delivery Date</th>
                                            <th scope="col" ng-click="orderPastTable('DeliveryDateObj')">Order Cut-Off Date</th>
                                            <th scope="col" ng-click="orderPastTable('Menu')">Menu</th>
                                            <th></th>
                                        </tr>
                                    </thead>
                                    <tbody>

                                        <tr ng-show="ShowLoaderPast">
                                            <td colspan="6" align="center">
                                                <img src="/App_Themes/HealthyChef/Images/Blocks-1s-200px.gif" alt="Loading..." />
                                            </td>
                                        </tr>

                                        <tr ng-repeat="calendar in displaypastCalendar = ( pastCalendar | orderBy:OrderByPastField : Pastreverse) | limitTo:pageSize:pageSize*(page1-1) " style="background-color: #FEFEFE;">

                                            <td>{{calendar.Name}}</td>
                                            <td>{{calendar.DeliveryDate}}</td>
                                            <td>{{calendar.OrderCutOffDate}}</td>
                                            <td>{{calendar.Menu}}</td>
                                            <td><a href="/WebModules/ShoppingCart/Admin/ProductionCalendar.aspx?CalendarId={{calendar.CalendarId}}">Edit</a></td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div ng-show="pastCalendar.length==0" class="aling_center">
                                    <p>  There are currently no active calendars</p>
                                </div>
                                <ul ng-show="pastCalendar.length!=0" uib-pagination class="pagination-sm pagination" total-items="pastCalendar.length" ng-model="page1"
                                    ng-click="pastCalendarpageChanged()" previous-text="&lsaquo;" next-text="&rsaquo;" items-per-page="pageSize" max-size="10" boundary-links="true">
                                </ul>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    <script>
        var app = angular.module('UserApp', ["ui.bootstrap"]);
        //Getting data from API
        app.controller('ProductionCalendar', function ($scope, $http) {

            //$scope.Users = null;
            //$scope.displayItems = null;
            //$scope.totalRecords = 0;
            $scope.page = 1;
            $scope.page1 = 1;
            $scope.pageSize = 50;
            $scope.ShowLoaderFuture = true;
            $scope.ShowLoaderPast = true;
            $scope.Getdata = function () {
                $http({
                    method: "GET",
                    url: GetApiBaseURL() + "GetFutureCalender/" + $scope.startDate + "/" + $scope.endDate,
                }).then(function mySuccess(response) {

                    $scope.futureCalendar = response.data;
                    $scope.displayfutureCalendar = $scope.futureCalendar.slice(0, $scope.pageSize);
                    $scope.totalRecords = $scope.futureCalendar.length;
                    console.log($scope.totalRecords);
                    $scope.ShowLoaderFuture = false;
                }, function myError(response) {

                    $scope.ErrorMessage = response.statusText;
                    $scope.ShowLoaderFuture = false;
                });
                $http({
                    method: "GET",
                    url: GetApiBaseURL() + "GetPastCalender/" + $scope.startDate + "/" + $scope.endDate,
                }).then(function mySuccess(response) {

                    $scope.pastCalendar = response.data;
                    $scope.displaypastCalendar = $scope.pastCalendar.slice(0, $scope.pageSize);
                    $scope.totalRecords = $scope.pastCalendar.length;
                    console.log($scope.totalRecords);
                    $scope.ShowLoaderPast = false;
                }, function myError(response) {

                    $scope.ErrorMessage = response.statusText;
                    $scope.ShowLoaderPast = false;
                });
            }
            $scope.pageChanged = function () {

                var startPos = ($scope.page - 1) * $scope.pageSize;
                $scope.displayfutureCalendar = $scope.futureCalendar.slice(startPos, startPos + $scope.pageSize);

            };
            $scope.pastCalendarpageChanged = function () {

                var startPos = ($scope.page1 - 1) * $scope.pageSize;
                $scope.displaypastCalendar = $scope.pastCalendar.slice(startPos, startPos + $scope.pageSize);

            };

            $scope.orderFutureTable = function (x) {
                $scope.Futurereverse = ($scope.OrderByFutureField === x) ? !$scope.Futurereverse : false;
                $scope.OrderByFutureField = x;
            }

            $scope.orderPastTable = function (x) {
                $scope.Pastreverse = ($scope.OrderByPastField === x) ? !$scope.Pastreverse : false;
                $scope.OrderByPastField = x;
            }

            $scope.SaveCalendar = function () {
                
                var calendarid = 0;
                if ($("#ctl00_Body_CurrentCalendarId").val() == "") {
                    calendarid = 0;
                }
                else {
                    calendarid = $("#ctl00_Body_CurrentCalendarId").val();
                }
                var calendar = {
                    CalendarId: calendarid,
                    Name: $("#ctl00_Body_ProdCalendar1_txtCalendarName").val(),
                    DeliveryDatestring: $("#ctl00_Body_ProdCalendar1_txtOrderDeliveryDate").val(),
                    OrderCutOffDatestring: $("#ctl00_Body_ProdCalendar1_txtOrderCutOffDate").val(),
                    MenuId: $("#ctl00_Body_ProdCalendar1_ddlMenus option:selected").val(),
                    Description: $("#ctl00_Body_ProdCalendar1_txtDescription").val()
                }
                $http({
                    method: "POST",
                    url: GetApiBaseURL() + "AddorUpdateProductionCalendar",
                    data: calendar
                }).then(function mySuccess(response) {

                    if (response.data.IsSuccess) {
                        alert(response.data.Message)
                        window.location.href = "/WebModules/ShoppingCart/Admin/ProductionCalendar.aspx";
                    }
                }, function myError(response) {

                    if (response.data.IsSuccess == false) {
                        alert(response.data.Message)
                    }
                });
            }

            $scope.ResetFilters = function () {

                $scope.startDate = '';
                $scope.endDate = '';
                $scope.Getdata();
            }

        });
    </script>
    <script type="text/javascript">
        $(function () {
            ToggleMenus('productioncalendars', 'productionmanagement', 'storemanagement');

        });
    </script>
    <style>
     .desc legend {
        margin:0px;
        text-align:left;
        font-size:16px;
    }
       .desc {
        border-radius:0px;
        margin-bottom:0px;
    }
        </style>
</asp:Content>
