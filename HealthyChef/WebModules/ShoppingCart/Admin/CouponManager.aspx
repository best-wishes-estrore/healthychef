<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/WebModules/Default.master"
    Theme="WebModules" AutoEventWireup="true" CodeBehind="CouponManager.aspx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.CouponManager" %>

<%@ Register TagPrefix="hcc" TagName="CouponEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/Coupon_Edit.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#divCouponTabs").tabs({
                cookie: {
                    // store cookie for a day, without, it would be a session cookie
                    expires: 1
                }
            });
        });
    </script>
    <div class="main-content" ng-app="UserApp" ng-controller="GetCoupons">
        <asp:HiddenField ID="CurrentUserID" runat="server" />
        <div class="main-content-inner">
            <div style="clear: both;" />
            <a href="../" onclick="MakeUpdateProg(true);" class="btn btn-info b-10"><< Back to Dashboard</a>
            <div class="page-header">
                <h1>Coupons Manager</h1>
            </div>
            <div class="hcc_admin_ctrl">
                <div>
                    <asp:Label ID="lblFeedback" runat="server" ForeColor="Green" Font-Bold="true" EnableViewState="false" />
                </div>
                <div class="col-sm-12">
                    <asp:Button ID="btnAddCoupon" CssClass="btn btn-info" runat="server" Text="Add New Coupon" />
                </div>
                <div id="divEdit" runat="server" visible="false">
                    <hcc:CouponEdit ID="CouponEdit1" runat="server" ValidationGroup="CouponEditGroup"
                        ShowDeactivate="false" />
                </div>
                <div class="clearfix"></div>
                <asp:Panel ID="pnlGrids" runat="server">
                    <div class="m-2">
                        <div id="divCouponTabs">
                            <ul>
                                <li><a href="#active">Active Coupons</a></li>
                                <li><a href="#inactive">Inactive Coupons</a></li>
                            </ul>
                            <div id="active" class="col-sm-12">
                                <%--  <asp:GridView ID="gvwActiveCoupons" CssClass="table table-bordered table-hover" runat="server" AutoGenerateColumns="false" DataKeyNames="CouponID"
                                    Width="100%">
                                    <Columns>
                                        <asp:BoundField DataField="RedeemCode" HeaderText="Redeem Code" />
                                        <asp:BoundField DataField="Title" HeaderText="Title" />
                                        <asp:TemplateField HeaderText="Details">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDetails" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Effective Dates">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEffectiveDates" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:CommandField ShowSelectButton="true" SelectText="Edit" ItemStyle-HorizontalAlign="Right"
                                            ItemStyle-Width="100px" />
                                    </Columns>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <AlternatingRowStyle BackColor="#eeeeee" />
                                    <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" />
                                    <PagerStyle HorizontalAlign="Right" />
                                    <EmptyDataTemplate>
                                        There are currently no active coupons.
                                    </EmptyDataTemplate>
                                </asp:GridView>--%>
                                <div class="m-2">
                                    <%--<label>Count :  {{((page - 1) * pageSize) + 1}} to  {{ (((page - 1) * pageSize) + pageSize) | number }} of {{ displayActivecoupons.length }}</label>--%>
                                    <label ng-show="Activecoupons.length!=0">Count :  {{((page - 1) * pageSize) + 1}} to  {{ displayActivecoupons.length <= (((page - 1) * pageSize) + pageSize) ? displayActivecoupons.length : (((page - 1) * pageSize) + pageSize) | number }} of {{ displayActivecoupons.length }}</label>

                                </div>
                                <table ng-show="Activecoupons.length!=0" class="table  table-bordered table-hover">
                                    <thead>
                                        <tr>
                                            <th style="width:13%" scope="col" ng-click="orderActiveTable('RedeemCode')">Redeem Code</th>
                                            <th scope="col" ng-click="orderActiveTable('Title')">Title</th>
                                            <th scope="col">Details</th>
                                            <th scope="col">Effective Dates</th>
                                            <th scope="col">&nbsp;</th>
                                        </tr>
                                    </thead>

                                    <tbody>

                                        <tr ng-show="ShowLoader">
                                            <td colspan="6" align="center">
                                                <img src="/App_Themes/HealthyChef/Images/Blocks-1s-200px.gif" alt="Loading..." />
                                            </td>
                                        </tr>

                                        <tr ng-repeat="u in displayActivecoupons = (Activecoupons | orderBy:OrderByActiveField : Activereverse) | limitTo:pageSize:pageSize*(page-1) ">
                                            <td><span>{{ u.RedeemCode }}</span></td>
                                            <td>{{ u.Title }}</td>
                                            <td>{{ u.Details }}</td>
                                            <td>{{ u.EffectiveDates}}</td>
                                            <td><a href="/WebModules/ShoppingCart/Admin/CouponManager.aspx?CouponID={{u.CouponID}}">Edit</a></td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div ng-show="Activecoupons.length==0" class="aling_center">
                                    <p>There are currently no active coupons.</p>
                                </div>
                                <ul uib-pagination class="pagination-sm pagination" total-items="Activecoupons.length" ng-model="page"
                                    ng-click="ActivecouponspageChanged()" previous-text="&lsaquo;" next-text="&rsaquo;" items-per-page="pageSize" max-size="10" boundary-links="true">
                                </ul>
                            </div>
                            <div id="inactive" class="col-sm-12">
                                <%--<asp:GridView ID="gvwInactiveCoupons" CssClass="table table-bordered table-hover" runat="server" AutoGenerateColumns="false"
                                    DataKeyNames="CouponID" Width="100%">
                                    <Columns>
                                        <asp:BoundField DataField="RedeemCode" HeaderText="Redeem Code" />
                                        <asp:BoundField DataField="Title" HeaderText="Title" />
                                        <asp:TemplateField HeaderText="Details">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDetails" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Effective Dates">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEffectiveDates" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:CommandField ShowSelectButton="true" SelectText="Edit" ItemStyle-HorizontalAlign="Right"
                                            ItemStyle-Width="100px" />
                                    </Columns>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <AlternatingRowStyle BackColor="#eeeeee" />
                                    <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" />
                                    <PagerStyle HorizontalAlign="Right" />
                                    <EmptyDataTemplate>
                                        There are currently no inactive coupons.
                                    </EmptyDataTemplate>
                                </asp:GridView>--%>
                                <div class="m-2">
                                    <%--<label>Count :  {{((page1 - 1) * pageSize) + 1}} to  {{ (((page1 - 1) * pageSize) + pageSize) | number }} of {{ InActivecoupons.length }}</label>--%>
                                    <label ng-show="InActivecoupons.length!=0">Count :  {{((page1 - 1) * pageSize) + 1}} to  {{ InActivecoupons.length <= (((page1 - 1) * pageSize) + pageSize) ? InActivecoupons.length : (((page1 - 1) * pageSize) + pageSize) | number }} of {{ InActivecoupons.length }}</label>

                                </div>
                                <table ng-show="InActivecoupons.length!=0" class="table  table-bordered table-hover">
                                    <thead>
                                        <tr>
                                            <th style="width:13%" scope="col" ng-click="orderRetiredTable('RedeemCode')">Redeem Code</th>
                                            <th scope="col" ng-click="orderRetiredTable('Title')">Title</th>
                                            <th scope="col">Details</th>
                                            <th scope="col">Effective Dates</th>
                                            <th scope="col">&nbsp;</th>
                                        </tr>
                                    </thead>

                                    <tbody>

                                        <tr ng-show="ShowLoader">
                                            <td colspan="6" align="center">
                                                <img src="/App_Themes/HealthyChef/Images/Blocks-1s-200px.gif" alt="Loading..." />
                                            </td>
                                        </tr>

                                        <tr ng-repeat="u in displayInActivecoupons = (InActivecoupons | orderBy:OrderByRetiredField : Retiredreverse)  | limitTo:pageSize:pageSize*(page1-1) ">
                                            <td><span>{{ u.RedeemCode }}</span></td>
                                            <td>{{ u.Title }}</td>
                                            <td>{{ u.Details }}</td>
                                            <td>{{ u.EffectiveDates}}</td>
                                            <td><a href="/WebModules/ShoppingCart/Admin/CouponManager.aspx?CouponID={{u.CouponID}}">Edit</a></td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div ng-show="InActivecoupons.length==0" class="aling_center">
                                    <p> There are currently no inactive coupons.</p>
                                </div>
                                <ul uib-pagination class="pagination-sm pagination" total-items="InActivecoupons.length" ng-model="page1"
                                    ng-click="InActivecouponspageChanged()" previous-text="&lsaquo;" next-text="&rsaquo;" items-per-page="pageSize" max-size="10" boundary-links="true">
                                </ul>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlUsedCoupons" runat="server">
                    <div class="page-header">
                        <h1>Search Used Coupons:</h1>
                    </div>
                    <div class="col-sm-3">
                        <div class="form-group">
                            <label>Start Date:</label>
                            <asp:TextBox ID="txtStartDate" CssClass="form-control datepicker1" runat="server" ValidationGroup="CouponSearchGroup" />
                            <%--<ajax:CalendarExtender ID="calExt1" runat="server" TargetControlID="txtStartDate" PopupPosition="TopRight" />--%>
                            <asp:RequiredFieldValidator ID="rfvStart" runat="server" ControlToValidate="txtStartDate"
                                Display="Dynamic" Text="Start Date required." SetFocusOnError="true" ValidationGroup="CouponSearchGroup" />
                            <asp:CompareValidator ID="cpvStart" runat="server" ControlToValidate="txtStartDate"
                                Display="Dynamic" Text="Start date must be a date." SetFocusOnError="true" ValidationGroup="CouponSearchGroup"
                                Operator="DataTypeCheck" Type="Date" />
                        </div>
                    </div>
                    <div class="col-sm-3">
                        <div class="form-group">
                            <label>End Date:</label>
                            <asp:TextBox ID="txtEndDate" CssClass="form-control datepicker1" runat="server" ValidationGroup="CouponSearchGroup" />
                            <asp:RequiredFieldValidator ID="rfvEnd" runat="server" ControlToValidate="txtEndDate"
                                Display="Dynamic" Text="End Date required." SetFocusOnError="true" ValidationGroup="CouponSearchGroup" />
                            <asp:CompareValidator ID="cpvEnd" runat="server" ControlToValidate="txtEndDate"
                                Display="Dynamic" Text="End date must be a date." SetFocusOnError="true" ValidationGroup="CouponSearchGroup"
                                Operator="DataTypeCheck" Type="Date" />
                            <%--<ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtEndDate" PopupPosition="TopRight" />--%>
                        </div>
                    </div>
                    <div class="col-sm-3">
                        <div class="m-2 p-5">
                            <asp:Button ID="btnSearchCoupons" CssClass="btn btn-info" runat="server" Text="Search Coupons" ValidationGroup="CouponSearchGroup" />
                        </div>
                    </div>

                    <div class="col-sm-12">
                        <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="txtEndDate" ControlToCompare="txtStartDate"
                            Display="Dynamic" ErrorMessage="Start Date must occur before End Date." SetFocusOnError="true"
                            Operator="GreaterThan" Type="Date" ValidationGroup="FinGCGroup" />
                    </div>

                    <br />
                    <asp:GridView ID="gvwUsedCoupons" runat="server" AutoGenerateColumns="false" Width="100%">
                        <Columns>
                            <asp:BoundField DataField="RedeemCode" HeaderText="Coupon Code" />
                            <asp:BoundField DataField="PaymentDue" HeaderText="Payment Amount" DataFormatString="{0:c}" />
                            <asp:BoundField DataField="PurchaseNumber" HeaderText="PurchaseNumber" />
                            <asp:BoundField DataField="FullName" HeaderText="CustomerName" />
                            <asp:BoundField DataField="PurchaseDate" HeaderText="PurchaseDate" DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" />
                        </Columns>
                        <EmptyDataTemplate>There are no used coupons during this time.</EmptyDataTemplate>
                    </asp:GridView>
                </asp:Panel>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var app = angular.module('UserApp', ["ui.bootstrap"]);

        //Getting data from API
        app.controller('GetCoupons', function ($scope, $http) {

            $scope.active = true;
            var activecoupons = new Array();
            var inactivecoupons = new Array();
            $scope.page = 1;
            $scope.page1 = 1;
            $scope.pageSize = 50;
            $scope.ShowLoader = true;
            $http({
                method: "GET",
                url: GetApiBaseURL() + "GetAllCoupons"
            }).then(function mySuccess(response) {
                angular.forEach(response.data, function (value, key) {
                    if (response.data[key].IsActive == true) {
                        activecoupons.push(response.data[key]);
                    }
                    else {
                        inactivecoupons.push(response.data[key]);
                    }
                });

                $scope.Activecoupons = activecoupons;
                $scope.InActivecoupons = inactivecoupons;
                $scope.displayActivecoupons = $scope.Activecoupons.slice(0, $scope.pageSize);
                $scope.displayInActivecoupons = $scope.InActivecoupons.slice(0, $scope.pageSize);

                console.log(activecoupons.length);
                console.log(inactivecoupons.length);
                $scope.ShowLoader = false;
            }, function myError(response) {
                $scope.ErrorMessage = response.statusText;
                $scope.ShowLoader = false;
            });


            $scope.GetDetails = function (index) {

            };
            $scope.ActivecouponspageChanged = function () {

                var startPos = ($scope.page - 1) * $scope.pageSize;
                $scope.displayActivecoupons = $scope.Activecoupons.slice(startPos, startPos + $scope.pageSize);
            };

            $scope.InActivecouponspageChanged = function () {

                var startPos = ($scope.page1 - 1) * $scope.pageSize;
                $scope.displayInActivecoupons = $scope.InActivecoupons.slice(startPos, startPos + $scope.pageSize);
            };

            $scope.orderActiveTable = function (x) {
                $scope.Activereverse = ($scope.OrderByActiveField === x) ? !$scope.Activereverse : false;
                $scope.OrderByActiveField = x;
            }

            $scope.orderRetiredTable = function (x) {
                $scope.Retiredreverse = ($scope.OrderByRetiredField === x) ? !$scope.Retiredreverse : false;
                $scope.OrderByRetiredField = x;
            }

            $scope.AddOrUpdateCoupon = function () {

                var couponid = $('#ctl00_Body_CouponEdit1_couponID').val();
                var reedemedcode = $('#ctl00_Body_CouponEdit1_txtRedeemCode').val();
                var title = $('#ctl00_Body_CouponEdit1_txtTitle').val();
                var description = $('#ctl00_Body_CouponEdit1_txtDescription').val();
                var amount = $('#ctl00_Body_CouponEdit1_txtAmount').val();
                var DiscountTypes = $('#ctl00_Body_CouponEdit1_ddlDiscountTypes').val();
                var UsageTypes = $('#ctl00_Body_CouponEdit1_ddlUsageTypes').val();
                var startdate = $('#ctl00_Body_CouponEdit1_txtStartDate').val();
                var enddate = $('#ctl00_Body_CouponEdit1_txtEndDate').val();
                var CreatedBy = $('#ctl00_Body_CurrentUserID').val();

                var data = { CouponID: couponid, DiscountTypeID: DiscountTypes, UsageTypeID: UsageTypes, RedeemCode: reedemedcode, Title: title, Description: description, Amount: amount, StartDateString: startdate, EndDateString: enddate, CreatedBy: CreatedBy};

                $http({
                    method: "POST",
                    url: GetApiBaseURL() + "AddOrUpdateCoupon",
                    data: data
                }).then(function mySuccess(response) {

                    if (response.data.IsSuccess) {

                        window.location.href = "/WebModules/ShoppingCart/Admin/CouponManager.aspx";
                    }
                    else {
                        alert(response.data.Message);
                    }
                }, function myError(response) {
                    alert(response.data.Message);
                });
            }
        });
    </script>
    <script type="text/javascript">
        $(function () {
            ToggleMenus('coupons', 'listmanagement', 'storemanagement');

        });
    </script>
</asp:Content>
