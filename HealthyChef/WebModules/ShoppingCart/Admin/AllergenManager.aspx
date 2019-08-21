<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/WebModules/Default.master"
    Theme="WebModules" AutoEventWireup="true" CodeBehind="AllergenManager.aspx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.AllergenManager" %>

<%@ Register TagPrefix="hcc" TagName="AllergenEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/Allergen_Edit.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#divAlrgnTabs").tabs({
                cookie: {
                    // store cookie for a day, without, it would be a session cookie
                    expires: 1
                }
            });
        });
    </script>
    <div class="main-content" ng-app="UserApp" ng-controller="GetAllergens">
        <div class="main-content-inner">
            <a href="../" onclick="MakeUpdateProg(true);" class="btn btn-info b-10"><< Back to Dashboard</a>
            <div class="page-header">
                <h1>Allergen Manager</h1>
            </div>
            <div class="hcc_admin_ctrl">
                <div>
                    <asp:Label ID="lblFeedback" runat="server" ForeColor="Green" Font-Bold="true" EnableViewState="false" />
                </div>
                <div class="col-sm-12">
                    <asp:Button ID="btnAddNewAllergen" CssClass="btn btn-info" runat="server" Text="Add New Allergen" />
                </div>
                <div id="divEdit" runat="server" visible="false">
                    <hcc:AllergenEdit ID="AllergenEdit1" runat="server" ShowDeactivate="true" ValidationGroup="AllergenEditGroup" />
                </div>
                <div class="clearfix"></div>
                <asp:Panel ID="pnlGrids" runat="server" CssClass="col-sm-12">
                    <div class="m-2">
                        <div id="divAlrgnTabs">
                            <ul>
                                <li><a href="#active">Active Allergens</a></li>
                                <li><a href="#retired">Retired Allergens</a></li>
                            </ul>
                            <div id="active" class="col-sm-12">
                                <%--        <asp:GridView ID="gvwActiveAllergens" runat="server" CssClass="table table-bordered table-hover" AutoGenerateColumns="false"
                                    DataKeyNames="AllergenId" AllowPaging="true" PageSize="50" Width="100%">
                                    <Columns>
                                        <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="350px" />
                                        <asp:BoundField DataField="Description" HeaderText="Description" />
                                        <asp:CommandField ShowSelectButton="true" SelectText="Edit" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px" />
                                    </Columns>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <AlternatingRowStyle BackColor="#eeeeee" />
                                    <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" />
                                    <PagerStyle HorizontalAlign="Right" />
                                    <EmptyDataTemplate>
                                        There are currently no active allergens on record for this website.
                                    </EmptyDataTemplate>
                                </asp:GridView>--%>
                                <div>
                                    <div class="m-2">
                                        <%--<label>Count :  {{((page - 1) * pageSize) + 1}} to  {{ (((page - 1) * pageSize) + pageSize) | number }} of {{ displayActiveAllergens.length }}</label>--%>
                                        <label ng-show="ActiveAllergens.length!=0">Count :  {{((page - 1) * pageSize) + 1}} to  {{ displayActiveAllergens.length <= (((page - 1) * pageSize) + pageSize) ? displayActiveAllergens.length : (((page - 1) * pageSize) + pageSize) | number }} of {{ displayActiveAllergens.length }}</label>
                                    </div>

                                    <table ng-show="ActiveAllergens.length!=0" class="table table-bordered table-hover">
                                        <thead>
                                            <tr align="left">
                                                <th scope="col" ng-click="orderActiveTable('Name')">Name</th>
                                                <th scope="col" ng-click="orderActiveTable('Description')">Description</th>
                                                <th scope="col">&nbsp;</th>
                                            </tr>
                                        </thead>

                                        <tbody>

                                            <tr ng-show="ShowLoader">
                                                <td colspan="6" align="center">
                                                    <img src="/App_Themes/HealthyChef/Images/Blocks-1s-200px.gif" alt="Loading..." />
                                                </td>
                                            </tr>

                                            <tr style="background-color: #FEFEFE;" ng-repeat="u in displayActiveAllergens | orderBy:OrderByActiveField : Activereverse">
                                                <td ng-show="false">{{u.AllergenID}}</td>
                                                <td><span>{{ u.Name }}</span></td>
                                                <td>{{ u.Description }}</td>
                                                <td><a href="/WebModules/ShoppingCart/Admin/AllergenManager.aspx?AllergenID={{u.AllergenID}}">Edit</a></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div ng-show="ActiveAllergens.length==0" class="aling_center">
                                        <p>  There are currently no active allergens on record for this website.</p>
                                    </div>
                                    <ul ng-show="ActiveAllergens.length!=0" uib-pagination class="pagination-sm pagination" total-items="ActiveAllergens.length" ng-model="page"
                                        ng-click="ActiveallergenspageChanged()" previous-text="&lsaquo;" next-text="&rsaquo;" items-per-page="pageSize" max-size="10" boundary-links="true">
                                    </ul>
                                </div>
                            </div>
                            <div id="retired" class="col-sm-12">
                                <%--<asp:GridView ID="gvwRetiredAllergens" CssClass="table table-bordered table-hover" runat="server" AutoGenerateColumns="false"
                                    DataKeyNames="AllergenId" AllowPaging="true" PageSize="50" Width="100%">
                                    <Columns>
                                        <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="350px" />
                                        <asp:BoundField DataField="Description" HeaderText="Description" />
                                        <asp:CommandField ShowSelectButton="true" SelectText="Edit" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px" />
                                    </Columns>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <AlternatingRowStyle BackColor="#eeeeee" />
                                    <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" />
                                    <PagerStyle HorizontalAlign="Right" />
                                    <EmptyDataTemplate>
                                        There are currently no active allergens on record for this website.
                                    </EmptyDataTemplate>
                                </asp:GridView>--%>
                                <div class="m-2">
                                    <%--<label>Count :  {{((page1 - 1) * pageSize) + 1}} to  {{ (((page1 - 1) * pageSize) + pageSize) | number }} of {{ displayRetiredAllergens.length }}</label>--%>
                                    <label ng-show="RetiredAllergens.length!=0">Count :  {{((page1 - 1) * pageSize) + 1}} to  {{ displayRetiredAllergens.length <= (((page1 - 1) * pageSize) + pageSize) ? displayRetiredAllergens.length : (((page1 - 1) * pageSize) + pageSize) | number }} of {{ displayRetiredAllergens.length }}</label>
                                </div>
                                <table ng-show="RetiredAllergens.length!=0" class="table table-bordered table-hover">
                                    <thead>
                                        <tr align="left">
                                            <th scope="col" ng-click="orderRetiredTable('Name')">Name</th>
                                            <th scope="col" ng-click="orderRetiredTable('Description')">Description</th>
                                            <th scope="col">&nbsp;</th>
                                        </tr>
                                    </thead>

                                    <tbody>

                                        <tr ng-show="ShowLoader">
                                            <td colspan="6" align="center">
                                                <img src="/App_Themes/HealthyChef/Images/Blocks-1s-200px.gif" alt="Loading..." />
                                            </td>
                                        </tr>

                                        <tr style="background-color: #FEFEFE;" ng-repeat="u in displayRetiredAllergens | orderBy:OrderByRetiredField : Retiredreverse">
                                            <td ng-show="false">{{u.AllergenID}}</td>
                                            <td><span>{{u.Name}}</span></td>
                                            <td>{{u.Description}}</td>
                                            <td><a href="/WebModules/ShoppingCart/Admin/AllergenManager.aspx?AllergenID={{u.AllergenID}}">Edit</a></td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div ng-show="RetiredAllergens.length==0" class="aling_center">
                                    <p>There are currently no active allergens on record for this website.</p>
                                </div>
                                <ul ng-show="RetiredAllergens.length!=0" uib-pagination class="pagination-sm pagination" total-items="RetiredAllergens.length" ng-model="page1"
                                    ng-click="RetiredallergenspageChanged()" previous-text="&lsaquo;" next-text="&rsaquo;" items-per-page="pageSize" max-size="10" boundary-links="true">
                                </ul>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var app = angular.module('UserApp', ["ui.bootstrap"]);

        //Getting data from API
        app.controller('GetAllergens', function ($scope, $http) {

            $scope.active = true;
            $scope.page = 1;
            $scope.page1 = 1;
            $scope.pageSize = 50;
            var activeallergens = new Array();
            var retiredallergens = new Array();
            $scope.ShowLoader = true;

            $http({
                method: "GET",
                url: GetApiBaseURL() + "GetAllAllergens"
            }).then(function mySuccess(response) {
                angular.forEach(response.data, function (value, key) {
                     
                    if (response.data[key].IsActive == true) {
                        activeallergens.push(response.data[key]);
                    }
                    else {
                        retiredallergens.push(response.data[key]);
                    }
                });
                $scope.ActiveAllergens = activeallergens;
                $scope.displayActiveAllergens = $scope.ActiveAllergens.slice(0, $scope.pageSize);
                $scope.RetiredAllergens = retiredallergens;
                $scope.displayRetiredAllergens = $scope.RetiredAllergens.slice(0, $scope.pageSize);
                console.log(activeallergens.length);
                console.log(retiredallergens.length);
                $scope.ShowLoader = false;
            }, function myError(response) {
                $scope.ErrorMessage = response.statusText;
                $scope.ShowLoader = false;
            });


            $scope.GetActiveAllergensDetails = function (index) {
                 
                var allergens = {
                    "AllergenID": $scope.ActiveAllergens[index].AllergenID,
                    "Name": $scope.ActiveAllergens[index].Name,
                    "Description": $scope.ActiveAllergens[index].Description,
                    "IsActive": ''
                };
                $http({
                    method: "GET",
                    url: GetApiBaseURL() + "AddOrUpdateAllergen" + allergens,
                }).then(function mySuccess(response) {
                    console.log(response.Message);
                }, function myError(response) {
                    $scope.ErrorMessage = response.Message;
                });
            }


            $scope.GetRetiredAllergensDetails = function (index) {
                 
                var allergens = {
                    "AllergenID": $scope.RetiredAllergens[index].AllergenID,
                    "Name": $scope.RetiredAllergens[index].Name,
                    "Description": $scope.RetiredAllergens[index].Description,
                    "IsActive": ''
                };
                $http({
                    method: "GET",
                    url: GetApiBaseURL() + "AddOrUpdateAllergen" + allergens,
                }).then(function mySuccess(response) {
                    console.log(response.Message);
                }, function myError(response) {
                    $scope.ErrorMessage = response.Message;
                });
            }


            $scope.ActiveallergenspageChanged = function () {
                 
                var startPos = ($scope.page - 1) * $scope.pageSize;
                $scope.displayActiveAllergens = $scope.ActiveAllergens.slice(startPos, startPos + $scope.pageSize);
            };

            $scope.RetiredallergenspageChanged = function () {
                 
                var startPos = ($scope.page1 - 1) * $scope.pageSize;
                $scope.displayRetiredAllergens = $scope.RetiredAllergens.slice(startPos, startPos + $scope.pageSize);
            };

            $scope.orderActiveTable = function (x) {
                $scope.Activereverse = ($scope.OrderByActiveField === x) ? !$scope.Activereverse : false;
                $scope.OrderByActiveField = x;
            }

            $scope.orderRetiredTable = function (x) {
                $scope.Retiredreverse = ($scope.OrderByRetiredField === x) ? !$scope.Retiredreverse : false;
                $scope.OrderByRetiredField = x;
            }

            $scope.AddOrUpdateAllergen = function () {
                 
                var allergenID = $('#ctl00_Body_AllergenEdit1_allergenID').val();
                var name = $('#ctl00_Body_AllergenEdit1_txtAllergenName').val();
                var description = $('#ctl00_Body_AllergenEdit1_txtAllergenDesc').val();

                var data = { AllergenID: allergenID, Name: name, Description: description };

                $http({
                    method: "POST",
                    url: GetApiBaseURL() + "AddOrUpdateAllergen",
                    data: data
                }).then(function mySuccess(response) {
                     
                    if (response.data.IsSuccess) {
                        window.location.href = "/WebModules/ShoppingCart/Admin/AllergenManager.aspx";
                    }
                    else {
                        alert(response.data.Message);
                    }
                }, function myError(response) {
                    alert(response.data.Message);
                });
            }

            $scope.UpdateRetireAllergen = function (isRetire) {
                 
                var allergenID = $('#ctl00_Body_AllergenEdit1_allergenID').val();
                var name = $('#ctl00_Body_AllergenEdit1_txtAllergenName').val();
                var description = $('#ctl00_Body_AllergenEdit1_txtAllergenDesc').val();

                var data = { AllergenID: allergenID, Name: name, Description: description, IsActive: isRetire };

                $http({
                    method: "POST",
                    url: GetApiBaseURL() + "AddOrUpdateAllergen",
                    data: data
                }).then(function mySuccess(response) {
                     
                    if (response.data.IsSuccess) {
                        window.location.href = "/WebModules/ShoppingCart/Admin/AllergenManager.aspx";
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
            ToggleMenus('allergens', 'listmanagement', 'storemanagement');

        });
    </script>
</asp:Content>
