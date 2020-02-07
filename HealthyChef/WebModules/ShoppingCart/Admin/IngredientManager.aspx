<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/WebModules/Default.master"
    Theme="WebModules" AutoEventWireup="true" CodeBehind="IngredientManager.aspx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.IngredientManager" %>

<%@ Register TagPrefix="hcc" TagName="IngredientEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/Ingredient_Edit.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#divIngTabs").tabs({
                cookie: {
                    // store cookie for a day, without, it would be a session cookie
                    expires: 1
                }
            });
        });
    </script>
    <div class="main-content" ng-app="UserApp" ng-controller="GetIngredient">
        <div class="main-content-inner">
            <a href="../" onclick="MakeUpdateProg(true);" class="btn btn-info b-10"><< Back to Dashboard</a>
            <div class="page-header">
                <h1>Ingredients Manager</h1>
            </div>
            <div class="hcc_admin_ctrl">
                <div>
                    <asp:Label ID="lblFeedback" runat="server" ForeColor="Green" Font-Bold="true" EnableViewState="false" />
                </div>
                <div style="width: 100%; display: inline-block; text-align: right; margin-bottom: 2px;">
                    <asp:Repeater ID="rptAlphas" runat="server" Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="lkbAlpha" runat="server" />
                        </ItemTemplate>
                        <SeparatorTemplate>
                            <span style="margin: 0px 2px;">|</span>
                        </SeparatorTemplate>
                    </asp:Repeater>

                    <div runat="server" id="AplhabetsRow" style="display: inline-block; cursor: pointer;">
                        <div ng-repeat="alpha in Alphas" style="display: inline-block; cursor: pointer;">
                            <a ng-click="SearchByAlpha(alpha,$event);" class="search-alphabet {{$index==Alphas.length-1 ? 'activealphabet' : ''}}">{{alpha}}</a>
                            <span style="margin: 0px 2px;" ng-show="$index < Alphas.length-1">|</span>
                        </div>
                    </div>

                    &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnAddNewIngredient" CssClass="btn btn-info" runat="server" Text="Add New Ingredient" />
                </div>
                <div id="divEdit" runat="server" visible="false">
                    <hcc:IngredientEdit ID="IngredientEdit1" runat="server" ValidationGroup="IngredientEditGroup" ShowDeactivate="true" />
                </div>
                <asp:Panel ID="pnlGrids" runat="server">
                    <div id="divIngTabs">
                        <ul>
                            <li><a href="#active">Active Ingredients</a></li>
                            <li><a href="#retired">Retired Ingredients</a></li>
                        </ul>
                        <div id="active" class="col-sm-12">
                            <%--         <asp:GridView ID="gvwActiveIngreds" CssClass="table table-bordered table-hover" runat="server" AutoGenerateColumns="false" DataKeyNames="IngredientId"
                                AllowPaging="true" PageSize="50" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="350px" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                    <asp:TemplateField HeaderText="Allergens">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAllergens" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField ShowSelectButton="true" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px" />
                                </Columns>
                                <HeaderStyle HorizontalAlign="Left" />
                                <AlternatingRowStyle BackColor="#eeeeee" />
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" />
                                <PagerStyle HorizontalAlign="Right" />
                                <EmptyDataTemplate>
                                    There are currently no active ingredients on record for this website.
                                </EmptyDataTemplate>
                            </asp:GridView>--%>
                            <div class="p-5">
                                <%--<label>Count :  {{((activepage - 1) * activepageSize) + 1}} to  {{ (((activepage - 1) * activepageSize) + activepageSize) | number }} of {{ ActiveIngredient.length }}</label>--%>
                                <label ng-show="ActiveIngredient.length!=0">Count :  {{((activepage - 1) * activepageSize) + 1}} to  {{ ActiveIngredient.length <= (((activepage - 1) * activepageSize) + activepageSize) ? ActiveIngredient.length : (((activepage - 1) * activepageSize) + activepageSize) | number }} of {{ ActiveIngredient.length }}</label>

                            </div>
                            <table ng-show="ActiveIngredient.length!=0" class="table  table-bordered table-hover">
                                <thead>
                                    <tr>
                                        <th scope="col" ng-click="orderActiveTable('Name')">Name</th>
                                        <th scope="col" ng-click="orderActiveTable('Description')">Description</th>
                                        <th scope="col">Allergens</th>
                                        <th scope="col">&nbsp;</th>
                                    </tr>
                                </thead>

                                <tbody>

                                    <tr ng-show="ShowLoader">
                                        <td colspan="6" align="center">
                                            <img src="/App_Themes/HealthyChef/Images/Blocks-1s-200px.gif" alt="Loading..." />
                                        </td>
                                    </tr>

                                    <tr ng-repeat="u in ActivedisplayItems = (ActiveIngredient | orderBy:OrderByActiveField : Activereverse) |  limitTo:activepageSize:activepageSize*(activepage-1) ">
                                        <td><span>{{ u.Name }}</span></td>
                                        <td>{{ u.Description }}</td>
                                        <td>{{ u.Allergens}}</td>
                                        <td><a href="/WebModules/ShoppingCart/Admin/IngredientManager.aspx?i={{u.IngredientID}}">Select</a></td>
                                    </tr>
                                </tbody>
                            </table>
                            <div ng-show="ActiveIngredient.length==0" class="aling_center">
                                <p> There are currently no active ingredients on record for this website.</p>
                            </div>
                            <ul uib-pagination class="pagination-sm pagination" total-items="ActiveIngredient.length" ng-model="activepage"
                                ng-change="ActivepageChanged()" previous-text="&lsaquo;" next-text="&rsaquo;" items-per-page="activepageSize" max-size="10" boundary-links="true">
                            </ul>
                        </div>
                        <div id="retired" class="col-sm-12">
                            <%--         <asp:GridView ID="gvwRetiredIngreds" runat="server" CssClass="table table-bordered table-hover" AutoGenerateColumns="false" DataKeyNames="IngredientId"
                                AllowPaging="true" PageSize="50" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="350px" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                    <asp:TemplateField HeaderText="Allergens">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAllergens" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField ShowSelectButton="true" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px" />
                                </Columns>
                                <HeaderStyle HorizontalAlign="Left" />
                                <AlternatingRowStyle BackColor="#eeeeee" />
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" />
                                <PagerStyle HorizontalAlign="Right" />
                                <EmptyDataTemplate>
                                    There are currently no retired ingredients on record for this website.
                                </EmptyDataTemplate>
                            </asp:GridView>--%>
                            <div class="p-5">
                                <%--<label>Count :  {{((retiredpage - 1) * retiredpageSize) + 1}} to  {{ (((retiredpage - 1) * retiredpageSize) + retiredpageSize) | number }} of {{ RetiredIngredient.length }}</label>--%>
                                <label ng-show="RetiredIngredient.length!=0">Count :  {{((retiredpage - 1) * retiredpageSize) + 1}} to  {{ RetiredIngredient.length <= (((retiredpage - 1) * retiredpageSize) + retiredpageSize) ? RetiredIngredient.length : (((retiredpage - 1) * retiredpageSize) + retiredpageSize) | number }} of {{ RetiredIngredient.length }}</label>

                            </div>
                            <table ng-show="RetiredIngredient.length!=0" class="table  table-bordered table-hover">
                                <thead>
                                    <tr>
                                        <th scope="col" ng-click="orderRetiredTable('Name')">Name</th>
                                        <th scope="col" ng-click="orderRetiredTable('Description')">Description</th>
                                        <th scope="col">Allergens</th>
                                        <th scope="col">&nbsp;</th>
                                    </tr>
                                </thead>

                                <tbody>

                                    <tr ng-show="ShowLoader">
                                        <td colspan="6" align="center">
                                            <img src="/App_Themes/HealthyChef/Images/Blocks-1s-200px.gif" alt="Loading..." />
                                        </td>
                                    </tr>

                                    <tr ng-repeat="u in RetireddisplayItems = (RetiredIngredient | orderBy:OrderByRetiredField : Retiredreverse) |  limitTo:retiredpageSize:retiredpageSize*(retiredpage-1)">
                                        <td><span>{{ u.Name }}</span></td>
                                        <td>{{ u.Description }}</td>
                                        <td>{{ u.Allergens}}</td>
                                        <td><a href="/WebModules/ShoppingCart/Admin/IngredientManager.aspx?i={{u.IngredientID}}">Select</a></td>
                                    </tr>
                                </tbody>
                            </table>
                            <div ng-show="RetiredIngredient.length==0" class="aling_center">
                                <p>There are currently no retired ingredients on record for this website.</p>
                            </div>
                            <ul uib-pagination class="pagination-sm pagination" total-items="RetiredIngredient.length" ng-model="retiredpage"
                                ng-change="RetiredpageChanged()" previous-text="&lsaquo;" next-text="&rsaquo;" items-per-page="retiredpageSize" max-size="10" boundary-links="true">
                            </ul>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var app = angular.module('UserApp', ["ui.bootstrap"]);

        //Getting data from API
        app.controller('GetIngredient', function ($scope, $http) {
            $scope.active = true;
            $scope.activepage = 1;
            $scope.activepageSize = 50;
            $scope.retiredpage = 1;
            $scope.retiredpageSize = 50;
            $scope.ShowLoader = true;
            var activeingredient = new Array();
            var inactiveingredient = new Array();

            $scope.ActiveIngredient2 = new Array();
            $scope.RetiredIngredient2 = new Array();

            $http({
                method: "GET",
                url: GetApiBaseURL() + "GetAllIngredient"
            }).then(function mySuccess(response) {

                angular.forEach(response.data, function (value, key) {
                    if (response.data[key].IsRetired == false) {
                        activeingredient.push(response.data[key]);
                    }
                    else {
                        inactiveingredient.push(response.data[key]);
                    }
                });

                $scope.ActiveIngredient = activeingredient;
                $scope.ActiveIngredient2 = $scope.ActiveIngredient;
                $scope.RetiredIngredient = inactiveingredient;
                $scope.RetiredIngredient2 = $scope.RetiredIngredient;

                $scope.ActivedisplayItems = $scope.ActiveIngredient.slice(0, $scope.activepageSize);
                $scope.RetireddisplayItems = $scope.RetiredIngredient.slice(0, $scope.retiredpageSize);
                console.log(activeingredient.length);
                console.log(inactiveingredient.length);
                $scope.ShowLoader = false;
            }, function myError(response) {
                //$scope.ErrorMessage = response.statusText;
                $scope.ShowLoader = false;
            });


            $scope.ActivepageChanged = function () {
                var startPos = ($scope.activepage - 1) * $scope.activepageSize;
                $scope.ActivedisplayItems = $scope.ActiveIngredient.slice(startPos, startPos + $scope.activepageSize);
            };

            $scope.RetiredpageChanged = function () {
                var startPos = ($scope.retiredpage - 1) * $scope.retiredpageSize;
                $scope.RetireddisplayItems = $scope.RetiredIngredient.slice(startPos, startPos + $scope.retiredpageSize);
            };

            $scope.orderActiveTable = function (x) {
                $scope.Activereverse = ($scope.OrderByActiveField === x) ? !$scope.Activereverse : false;
                $scope.OrderByActiveField = x;
            }

            $scope.orderRetiredTable = function (x) {
                $scope.Retiredreverse = ($scope.OrderByRetiredField === x) ? !$scope.Retiredreverse : false;
                $scope.OrderByRetiredField = x;
            }

            $scope.GetDetails = function (index) {

            };

            $scope.AddOrUpdateIngredient = function () {

                var AvailableItems = [];
                var ingredientid = $('#ctl00_Body_IngredientEdit1_ingredientid').val();
                var name = $('#ctl00_Body_IngredientEdit1_txtIngredientName').val();
                var desc = $('#ctl00_Body_IngredientEdit1_txtIngredientDesc').val();
                $('#ctl00_Body_IngredientEdit1_epAllergens_lstSelectedItems option').each(function () {
                    AvailableItems.push($(this).val());
                });



                var data = { IngredientID: ingredientid, Name: name, Description: desc, AllergensIds: AvailableItems };

                $http({
                    method: "POST",
                    url: GetApiBaseURL() + "AddOrUpdateIngredient",
                    data: data
                }).then(function mySuccess(response) {

                    if (response.data.IsSuccess) {
                        alert(response.data.Message);
                        window.location.href = "/WebModules/ShoppingCart/Admin/IngredientManager.aspx";

                    }
                    else {
                        alert(response.data.Message);
                    }
                }, function myError(response) {
                    alert(response.data.Message);
                });
            }

            $scope.UpdateRetireIngredient = function (isRetire) {

                var AvailableItems = [];
                var ingredientid = $('#ctl00_Body_IngredientEdit1_ingredientid').val();
                var name = $('#ctl00_Body_IngredientEdit1_txtIngredientName').val();
                var desc = $('#ctl00_Body_IngredientEdit1_txtIngredientDesc').val();
                $('#ctl00_Body_IngredientEdit1_epAllergens_lstSelectedItems option').each(function () {
                    AvailableItems.push($(this).val());
                });

                var data = { IngredientID: ingredientid, Name: name, Description: desc, AllergensIds: AvailableItems, IsRetired: isRetire };

                $http({
                    method: "POST",
                    url: GetApiBaseURL() + "AddOrUpdateIngredient",
                    data: data
                }).then(function mySuccess(response) {

                    if (response.data.IsSuccess) {
                        alert(response.data.Message);
                        window.location.href = "/WebModules/ShoppingCart/Admin/IngredientManager.aspx";

                    }
                    else {
                        alert(response.data.Message);
                    }
                }, function myError(response) {
                    alert(response.data.Message);
                });
            }

            $scope.Alphas = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "ALL"];

            $scope.SearchByAlpha = function (_alpha, $event) {

                var _anchorTag = $event.currentTarget;
                var activeClass = 'activealphabet';
                $('.search-alphabet').removeClass(activeClass);
                $(_anchorTag).addClass(activeClass);

                if (_alpha !== 'ALL') {
                    $scope.ActiveIngredient = $scope.ActiveIngredient2.filter((menu) => menu.Name.startsWith(_alpha));
                    $scope.RetiredIngredient = $scope.RetiredIngredient2.filter((menu) => menu.Name.startsWith(_alpha));
                }
                else {
                    $scope.ActiveIngredient = $scope.ActiveIngredient2;
                    $scope.RetiredIngredient = $scope.RetiredIngredient2;
                }
                $scope.ActivedisplayItems = $scope.ActiveIngredient.slice(0, $scope.pageSize);
                $scope.RetireddisplayItems = $scope.RetiredIngredient.slice(0, $scope.pageSize);
            }


        });


    </script>
    <script type="text/javascript">
        $(function () {
            ToggleMenus('ingredients', 'listmanagement', 'storemanagement');

        });
    </script>
</asp:Content>
