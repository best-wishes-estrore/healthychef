<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/WebModules/Default.master"
    Theme="WebModules" AutoEventWireup="true" CodeBehind="PreferencesManager.aspx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.PreferencesManager" %>

<%@ Register TagPrefix="hcc" TagName="PreferenceEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/Preference_Edit.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#divPrefTabs").tabs({
                cookie: {
                    // store cookie for a day, without, it would be a session cookie
                    expires: 1
                }
            });
            $("#divActiveTabs").tabs({
                cookie: {
                    // store cookie for a day, without, it would be a session cookie
                    expires: 1
                }
            });
            $("#divRetiredTabs").tabs({
                cookie: {
                    // store cookie for a day, without, it would be a session cookie
                    expires: 1
                }
            });
        });
    </script>
    <div class="main-content" ng-app="UserApp" ng-controller="PreferenceManager">
        <div class="main-content-inner">
            <a href="../" onclick="MakeUpdateProg(true);" class="btn btn-info b-10"><< Back to Dashboard</a>
            <div class="page-header">
                <h1>Preferences Manager</h1>
            </div>
            <div class="hcc_admin_ctrl">
                <div>
                    <asp:Label ID="lblFeedback" runat="server" ForeColor="Green" Font-Bold="true" EnableViewState="false" />
                </div>
                <div style="width: 100%; display: inline-block; text-align: right; margin-bottom: 2px;">
                    <asp:Button ID="btnAddNewPreference" runat="server" CssClass="btn btn-info" Text="Add New Preference" />
                </div>
                <div id="divEdit" runat="server" visible="false">
                    <hcc:PreferenceEdit ID="PreferenceEdit1" runat="server" ShowRetire="true" ValidationGroup="PreferenceEdit_Group" />
                </div>
                <asp:Panel ID="pnlGrids" runat="server">
                    <div id="divPrefTabs">
                        <ul>
                            <li><a href="#mealpreferences">Meal Preferences</a></li>
                            <li><a href="#customerpreferences">Customer Preferences</a></li>
                        </ul>
                        <div id="mealpreferences">
                            <div id="divActiveTabs">
                                <ul>
                                    <li><a href="#activemp">Active</a></li>
                                    <li><a href="#retiredmp">Retired</a></li>
                                </ul>
                                <div id="activemp" class="col-sm-12">
                                    <%--<asp:GridView ID="gvwActiveMealPrefs" CssClass="table table-bordered table-hover" runat="server" AutoGenerateColumns="false"
                                        DataKeyNames="PreferenceId" AllowPaging="true" PageSize="50" Width="100%">
                                        <Columns>
                                            <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="350px" />
                                            <asp:BoundField DataField="Description" HeaderText="Description" />
                                            <asp:CommandField ShowSelectButton="true" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px" />
                                        </Columns>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <AlternatingRowStyle BackColor="#eeeeee" />
                                        <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" />
                                        <PagerStyle HorizontalAlign="Right" />
                                        <EmptyDataTemplate>
                                            There are currently no active meal preferences on record for this website.
                                        </EmptyDataTemplate>
                                    </asp:GridView>--%>
                                    <div class="m-2">
                                        <%--<label>Count :  {{((page - 1) * pageSize) + 1}} to  {{ (((page - 1) * pageSize) + pageSize) | number }} of {{ Activepreference.length }}</label>--%>
                                        <label ng-show="Activepreference.length !=0">Count :  {{((page - 1) * pageSize) + 1}} to  {{ Activepreference.length <= (((page - 1) * pageSize) + pageSize) ? Activepreference.length : (((page - 1) * pageSize) + pageSize) | number }} of {{ Activepreference.length }}</label>
                                    </div>
                                    <table ng-show="Activepreference.length !=0" class="table  table-bordered table-hover">
                                        <thead>
                                            <tr>
                                                <th scope="col" ng-click="orderMealActiveTable('Name');">Name</th>
                                                <th scope="col" ng-click="orderMealActiveTable('Description');">Description</th>
                                                <th></th>
                                            </tr>
                                        </thead>
                                        <tbody>

                                            <tr ng-show="ShowLoaderMealpref">
                                                <td colspan="6" align="center">
                                                    <img src="/App_Themes/HealthyChef/Images/Blocks-1s-200px.gif" alt="Loading..." />
                                                </td>
                                            </tr>

                                            <tr ng-repeat="activeprefer in displayActivepreference | orderBy:OrderByMealActiveField : MealActivereverse">
                                                <td>{{activeprefer.Name}}</td>
                                                <td>{{activeprefer.Description}}</td>
                                                <td><a href="/WebModules/ShoppingCart/Admin/PreferencesManager.aspx?PreferenceID={{activeprefer.PreferenceID}}">Select</a></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div ng-show="Activepreference.length ==0">
                                        <p>There are currently no active meal preferences on record for this website.</p>
                                    </div>
                                    <ul uib-pagination class="pagination-sm pagination" total-items="Activepreference.length" ng-model="page"
                                        ng-click="MealActivepreferencepageChanged()" previous-text="&lsaquo;" next-text="&rsaquo;" items-per-page="pageSize" max-size="10" boundary-links="true">
                                    </ul>
                                </div>
                                <div id="retiredmp">
                                    <%--      <asp:GridView ID="gvwRetiredMealPrefs" CssClass="table table-bordered table-hover" runat="server" AutoGenerateColumns="false"
                                        DataKeyNames="PreferenceId" AllowPaging="true" PageSize="50" Width="100%">
                                        <Columns>
                                            <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="350px" />
                                            <asp:BoundField DataField="Description" HeaderText="Description" />
                                            <asp:CommandField ShowSelectButton="true" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px" />
                                        </Columns>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <AlternatingRowStyle BackColor="#eeeeee" />
                                        <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" />
                                        <PagerStyle HorizontalAlign="Right" />
                                        <EmptyDataTemplate>
                                            There are currently no retired meal preferences on record for this website.
                                        </EmptyDataTemplate>
                                    </asp:GridView>--%>
                                    <div class="m-2">
                                        <%--<label>Count :  {{((page1 - 1) * pageSize) + 1}} to  {{ (((page1 - 1) * pageSize) + pageSize) | number }} of {{ Retiredpreference.length }}</label>--%>
                                        <label ng-show="Retiredpreference.length !=0">Count :  {{((page1 - 1) * pageSize) + 1}} to  {{ Retiredpreference.length <= (((page1 - 1) * pageSize) + pageSize) ? Retiredpreference.length : (((page1 - 1) * pageSize) + pageSize) | number }} of {{ Retiredpreference.length }}</label>
                                    </div>
                                    <table ng-show="Retiredpreference.length !=0" class="table table-bordered table-hover">
                                        <thead>
                                            <tr>
                                                <th scope="col" ng-click="orderMealRetiredTable('Name');">Name</th>
                                                <th scope="col" ng-click="orderMealRetiredTable('Description');">Description</th>
                                                <th></th>
                                            </tr>
                                        </thead>
                                        <tbody>

                                            <tr ng-show="ShowLoaderMealpref">
                                                <td colspan="6" align="center">
                                                    <img src="/App_Themes/HealthyChef/Images/Blocks-1s-200px.gif" alt="Loading..." />
                                                </td>
                                            </tr>

                                            <tr ng-repeat="retiredprefer in displayRetiredpreference | orderBy:OrderByMealRetiredField : MealRetiredreverse">
                                                <td>{{retiredprefer.Name}}</td>
                                                <td>{{retiredprefer.Description}}</td>
                                                <td><a href="/WebModules/ShoppingCart/Admin/PreferencesManager.aspx?PreferenceID={{retiredprefer.PreferenceID}}">Select</a></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div ng-show="Retiredpreference.length ==0" class="aling_center">
                                        <p> There are currently no retired meal preferences on record for this website.</p>
                                    </div>
                                    <ul uib-pagination class="pagination-sm pagination" total-items="Retiredpreference.length" ng-model="page1"
                                        ng-click="MealRetirepreferencepageChanged()" previous-text="&lsaquo;" next-text="&rsaquo;" items-per-page="pageSize" max-size="10" boundary-links="true">
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <div id="customerpreferences" class="col-sm-12">
                            <div id="divRetiredTabs">
                                <ul>
                                    <li><a href="#activecp">Active</a></li>
                                    <li><a href="#retiredcp">Retired</a></li>
                                </ul>
                                <div id="activecp">
                                    <%--     <asp:GridView ID="gvwActiveCustPrefs" CssClass="table table-bordered table-hover" runat="server" AutoGenerateColumns="false"
                                        DataKeyNames="PreferenceId" AllowPaging="true" PageSize="50" Width="100%">
                                        <Columns>
                                            <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="350px" />
                                            <asp:BoundField DataField="Description" HeaderText="Description" />
                                            <asp:CommandField ShowSelectButton="true" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px" />
                                        </Columns>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <AlternatingRowStyle BackColor="#eeeeee" />
                                        <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" />
                                        <PagerStyle HorizontalAlign="Right" />
                                        <EmptyDataTemplate>
                                            There are currently no active customer preferences on record for this website.
                                        </EmptyDataTemplate>
                                    </asp:GridView>--%>
                                    <div class="m-2">
                                        <%--<label>Count :  {{((page2 - 1) * pageSize) + 1}} to  {{ (((page2 - 1) * pageSize) + pageSize) | number }} of {{ displayCustActivepreference.length }}</label>--%>
                                        <label ng-show="CustActivepreference.length !=0">Count :  {{((page2 - 1) * pageSize) + 1}} to  {{ displayCustActivepreference.length <= (((page2 - 1) * pageSize) + pageSize) ? displayCustActivepreference.length : (((page2 - 1) * pageSize) + pageSize) | number }} of {{ displayCustActivepreference.length }}</label>
                                    </div>
                                    <table ng-show="CustActivepreference.length !=0" class="table table-bordered table-hover">
                                        <thead>
                                            <tr>
                                                <th scope="col" ng-click="orderCustActiveTable('Name');">Name</th>
                                                <th scope="col" ng-click="orderCustActiveTable('Description');">Description</th>
                                                <th></th>
                                            </tr>
                                        </thead>
                                        <tbody>


                                            <tr ng-show="ShowLoaderCustpref">
                                                <td colspan="6" align="center">
                                                    <img src="/App_Themes/HealthyChef/Images/Blocks-1s-200px.gif" alt="Loading..." />
                                                </td>
                                            </tr>

                                            <tr ng-repeat="activeprefer in displayCustActivepreference | orderBy:OrderByCustActiveField : CustActivereverse" style="background-color: #FEFEFE;">
                                                <td>{{activeprefer.Name}}</td>
                                                <td>{{activeprefer.Description}}</td>
                                                <td><a href="/WebModules/ShoppingCart/Admin/PreferencesManager.aspx?PreferenceID={{activeprefer.PreferenceID}}">Select</a></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div ng-show="CustActivepreference.length ==0" class="aling_center">
                                        <p>  There are currently no active customer preferences on record for this website.</p>
                                    </div>
                                    <ul uib-pagination class="pagination-sm pagination" total-items="CustActivepreference.length" ng-model="page2"
                                        ng-click="CustActivepreferencepageChanged()" previous-text="&lsaquo;" next-text="&rsaquo;" items-per-page="pageSize" max-size="10" boundary-links="true">
                                    </ul>
                                </div>
                                <div id="retiredcp">
                                    <%--     <asp:GridView ID="gvwRetiredCustPrefs" CssClass="table table-bordered table-hover" runat="server" AutoGenerateColumns="false"
                                        DataKeyNames="PreferenceId" AllowPaging="true" PageSize="50" Width="100%">
                                        <Columns>
                                            <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="350px" />
                                            <asp:BoundField DataField="Description" HeaderText="Description" />
                                            <asp:CommandField ShowSelectButton="true" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px" />
                                        </Columns>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <AlternatingRowStyle BackColor="#eeeeee" />
                                        <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" />
                                        <PagerStyle HorizontalAlign="Right" />
                                        <EmptyDataTemplate>
                                            There are currently no retired customer preferences on record for this website.
                                        </EmptyDataTemplate>
                                    </asp:GridView>--%>
                                    <div class="m-2">
                                        <%--<label>Count :  {{((page3 - 1) * pageSize) + 1}} to  {{ (((page3 - 1) * pageSize) + pageSize) | number }} of {{ displayCustRetiredpreference.length }}</label>--%>
                                        <label ng-show="CustRetiredpreference.length !=0">Count :  {{((page3 - 1) * pageSize) + 1}} to  {{ displayCustRetiredpreference.length <= (((page3 - 1) * pageSize) + pageSize) ? displayCustRetiredpreference.length : (((page3 - 1) * pageSize) + pageSize) | number }} of {{ displayCustRetiredpreference.length }}</label>
                                    </div>
                                    <table ng-show="CustRetiredpreference.length !=0" class="table table-bordered table-hover">
                                        <thead>
                                            <tr>
                                                <th scope="col" ng-click="orderCustRetiredTable('Name');">Name</th>
                                                <th scope="col" ng-click="orderCustRetiredTable('Description');">Description</th>
                                                <th></th>
                                            </tr>
                                        </thead>
                                        <tbody>

                                            <tr ng-show="ShowLoaderCustpref">
                                                <td colspan="6" align="center">
                                                    <img src="/App_Themes/HealthyChef/Images/Blocks-1s-200px.gif" alt="Loading..." />
                                                </td>
                                            </tr>

                                            <tr ng-repeat="retiredprefer in displayCustRetiredpreference | orderBy:OrderByCustRetiredField : CustRetiredreverse" style="background-color: #FEFEFE;">
                                                <td>{{retiredprefer.Name}}</td>
                                                <td>{{retiredprefer.Description}}</td>
                                                <td><a href="/WebModules/ShoppingCart/Admin/PreferencesManager.aspx?PreferenceID={{retiredprefer.PreferenceID}}">Select</a></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div ng-show="CustRetiredpreference.length ==0">
                                        <p>  There are currently no retired customer preferences on record for this website.</p>
                                    </div>
                                    <ul uib-pagination class="pagination-sm pagination" total-items="CustRetiredpreference.length" ng-model="page3"
                                        ng-click="CustRetirepreferencepageChanged()" previous-text="&lsaquo;" next-text="&rsaquo;" items-per-page="pageSize" max-size="10" boundary-links="true">
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    <script>
        var app = angular.module('UserApp', ["ui.bootstrap"]);
        app.controller('PreferenceManager', function ($scope, $http) {

            //$scope.Users = null;
            //$scope.displayItems = null;
            //$scope.totalRecords = 0;
            $scope.page = 1;
            $scope.page1 = 1;
            $scope.page2 = 1;
            $scope.page3 = 1;
            $scope.pageSize = 50;
            $scope.Activepreference = [];
            $scope.Retiredpreference = [];
            $scope.CustActivepreference = [];
            $scope.CustRetiredpreference = [];

            $scope.ShowLoaderMealpref = true;
            $scope.ShowLoaderCustpref = true;

            $http({
                method: "GET",
                url: GetApiBaseURL() + "GetMealPrefs"
            }).then(function mySuccess(response) {
                 
                $scope.Mealpreference = response.data;
                for (var i = 0; i < $scope.Mealpreference.length; i++) {
                    if ($scope.Mealpreference[i].IsRetired == false) {
                        $scope.Activepreference.push($scope.Mealpreference[i]);
                        $scope.displayActivepreference = $scope.Activepreference.slice(0, $scope.pageSize);
                    }
                    else {
                        $scope.Retiredpreference.push($scope.Mealpreference[i]);
                        $scope.displayRetiredpreference = $scope.Retiredpreference.slice(0, $scope.pageSize);
                    }
                }
                $scope.ShowLoaderMealpref = false;
            }, function myError(response) {
                 
                $scope.ErrorMessage = response.statusText;
                $scope.ShowLoaderMealpref = false;
            });
            $http({
                method: "GET",
                url: GetApiBaseURL() + "GetCustomerPrefs"
            }).then(function mySuccess(response) {
                 
                $scope.Customerpreference = response.data;
                for (var i = 0; i < $scope.Customerpreference.length; i++) {
                    if ($scope.Customerpreference[i].IsRetired == false) {
                        $scope.CustActivepreference.push($scope.Customerpreference[i]);
                        $scope.displayCustActivepreference = $scope.CustActivepreference.slice(0, $scope.pageSize);
                    }
                    else {
                        $scope.CustRetiredpreference.push($scope.Customerpreference[i]);
                        $scope.displayCustRetiredpreference = $scope.CustRetiredpreference.slice(0, $scope.pageSize);
                    }
                }
                $scope.ShowLoaderCustpref = false;
            }, function myError(response) {
                 
                $scope.ErrorMessage = response.statusText;
                $scope.ShowLoaderCustpref = false;
            });
            $scope.MealActivepreferencepageChanged = function () {
                 
                var startPos = ($scope.page - 1) * $scope.pageSize;
                $scope.displayActivepreference = $scope.Activepreference.slice(startPos, startPos + $scope.pageSize);
            };
            $scope.MealRetirepreferencepageChanged = function () {
                 
                var startPos = ($scope.page1 - 1) * $scope.pageSize;
                $scope.displayRetiredpreference = $scope.Retiredpreference.slice(startPos, startPos + $scope.pageSize);
            };

            $scope.CustActivepreferencepageChanged = function () {
                 
                var startPos = ($scope.page2 - 1) * $scope.pageSize;
                $scope.displayCustActivepreference = $scope.CustActivepreference.slice(startPos, startPos + $scope.pageSize);
            };
            $scope.CustRetirepreferencepageChanged = function () {
                 
                var startPos = ($scope.page3 - 1) * $scope.pageSize;
                $scope.displayCustRetiredpreference = $scope.CustRetiredpreference.slice(startPos, startPos + $scope.pageSize);
            };

            $scope.orderMealActiveTable = function (x) {
                $scope.MealActivereverse = ($scope.OrderByMealActiveField === x) ? !$scope.MealActivereverse : false;
                $scope.OrderByMealActiveField = x;
            }

            $scope.orderMealRetiredTable = function (x) {
                $scope.MealRetiredreverse = ($scope.OrderByMealRetiredField === x) ? !$scope.MealRetiredreverse : false;
                $scope.OrderByMealRetiredField = x;
            }

            $scope.orderCustActiveTable = function (x) {
                $scope.CustActivereverse = ($scope.OrderByCustActiveField === x) ? !$scope.CustActivereverse : false;
                $scope.OrderByCustActiveField = x;
            }

            $scope.orderCustRetiredTable = function (x) {
                $scope.CustRetiredreverse = ($scope.OrderByCustRetiredField === x) ? !$scope.CustRetiredreverse : false;
                $scope.OrderByCustRetiredField = x;
            }


            $scope.AddOrUpdatePreference = function () {
                 
                var preferenceid = $('#ctl00_Body_PreferenceEdit1_preferenceid').val();
                var name = $('#ctl00_Body_PreferenceEdit1_txtPrefName').val();
                var desc = $('#ctl00_Body_PreferenceEdit1_txtPrefDesc').val();
                var PrefTypes = $('#ctl00_Body_PreferenceEdit1_ddlPrefTypes').val();

                var data = { PreferenceID: preferenceid, Name: name, Description: desc, PrefType: PrefTypes };

                $http({
                    method: "POST",
                    url: GetApiBaseURL() + "AddOrUpdatePreference",
                    data: data
                }).then(function mySuccess(response) {
                     
                    if (response.data.IsSuccess) {
                        alert(response.data.Message);
                        window.location.href = "/WebModules/ShoppingCart/Admin/PreferencesManager.aspx";

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
            ToggleMenus('preferences', 'listmanagement', 'storemanagement');

        });
    </script>
</asp:Content>
