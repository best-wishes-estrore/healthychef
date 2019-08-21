<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/WebModules/Default.master"
    Theme="WebModules" AutoEventWireup="true" CodeBehind="MenuManager.aspx.cs" Inherits="HealthyChef.WebModules.ShoppingCart.Admin.MenuManager" %>

<%@ Register Src="~/WebModules/ShoppingCart/Admin/UserControls/Menu_Edit.ascx" TagName="MenuEdit"
    TagPrefix="hcc" %>
<asp:Content runat="server" ContentPlaceHolderID="header">
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
    <div class="main-content" ng-app="UserApp" ng-controller="Menus" ng-init="Getdata()">
        <asp:HiddenField runat="server" ID="CurrentMenuId" />
        <div class="main-content-inner">
            <a href="../" onclick="MakeUpdateProg(true);" class="btn btn-info b-10"><< Back to Dashboard</a>
            <div class="page-header">
                <h1>Menu Manager</h1>
            </div>
            <div class="hcc_admin_ctrl">
                <div>
                    <asp:Label ID="lblFeedback" runat="server" ForeColor="Green" Font-Bold="true" EnableViewState="false" />
                </div>
                <div class="col-sm-12">
                    <div class="pull-right m-2">
                        <asp:Button ID="btnAddNewMenu" CssClass="btn btn-info" runat="server" Text="Add New Menu" />
                    </div>
                </div>
                <asp:Panel ID="pnlEditMenu" runat="server" Visible="false">
                    <hcc:MenuEdit runat="server" ID="MenuEdit1" UseUnRetireBehavior="true" ValidationGroup="MenuEditGroup" />
                </asp:Panel>
                <div class="clearfix"></div>
                <asp:Panel ID="pnlMenus" runat="server" CssClass="col-sm-12">
                    <div id="divPlanTabs">
                        <ul>
                            <li><a href="#menus">Menus</a></li>
                        </ul>
                        <div id="menus">
                            <%--<asp:GridView ID="gvwActiveMenus" runat="server" CssClass="table table-bordered table-hover" AutoGenerateColumns="false" DataKeyNames="MenuID"
                                AllowPaging="true" PageSize="50" Width="100%" ShowHeader="false">
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="300px" SortExpression="Name" />
                                    <asp:CommandField ShowSelectButton="true" SelectText="Edit" ItemStyle-HorizontalAlign="Right" />
                                </Columns>
                                <EmptyDataTemplate>
                                    There are currently no active menus on record.
                                </EmptyDataTemplate>
                                <AlternatingRowStyle BackColor="#eeeeee" />
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" />
                                <PagerStyle HorizontalAlign="Right" />
                            </asp:GridView>--%>
                            <div class="m-2">
                                <%--<label>Count :  {{((page - 1) * pageSize) + 1}} to  {{ (((page - 1) * pageSize) + pageSize) | number }} of {{ Menus.length }}</label>--%>
                                <label ng-show="Menus.length !=0">Count :  {{((page - 1) * pageSize) + 1}} to  {{ Menus.length <= (((page - 1) * pageSize) + pageSize) ? Menus.length : (((page - 1) * pageSize) + pageSize) | number }} of {{ Menus.length }}</label>
                                
                            </div>
                            <table ng-show="Menus.length !=0" class="table  table-bordered table-hover">
                                <%--<thead>
                                        <tr align="left">
                                            <th></th>
                                        </tr>
                                    </thead>--%>
                                <tbody>
                                    <tr ng-repeat="menu in displayMenus" style="background-color: #FEFEFE;">
                                        <td>{{menu.Name}}</td>
                                        <td><a href="/WebModules/ShoppingCart/Admin/MenuManager.aspx?MenuId={{menu.MenuID}}">Edit</a></td>
                                    </tr>
                                </tbody>
                            </table>
                            <div ng-show="Menus.length ==0" class="aling_center">
                                <p> There are currently no active menus on record.</p>
                            </div>
                            <ul ng-show="Menus.length !=0" uib-pagination class="pagination-sm pagination" total-items="Menus.length" ng-model="page"
                                ng-click="pageChanged()" previous-text="&lsaquo;" next-text="&rsaquo;" items-per-page="pageSize" max-size="10" boundary-links="true">
                            </ul>
                        </div>
                    </div>
            </div>
            </asp:Panel>
        </div>
    </div>
    <script>
        var app = angular.module('UserApp', ["ui.bootstrap"]);
        app.controller('Menus', function ($scope, $http) {

            //$scope.Users = null;
            //$scope.displayItems = null;
            //$scope.totalRecords = 0;
            $scope.page = 1;
            $scope.pageSize = 50;
            $scope.Getdata = function () {
                $http({
                    method: "GET",
                    url: GetApiBaseURL() + "GetMenus"
                }).then(function mySuccess(response) {
                     
                    $scope.Menus = response.data;
                    $scope.displayMenus = $scope.Menus.slice(0, $scope.pageSize);
                    $scope.totalRecords = $scope.Menus.length;
                    console.log($scope.totalRecords);
                }, function myError(response) {
                     
                    $scope.ErrorMessage = response.statusText;
                });
            }
            $scope.pageChanged = function () {
                 
                var startPos = ($scope.page - 1) * $scope.pageSize;
                $scope.displayMenus = $scope.Menus.slice(startPos, startPos + $scope.pageSize);
            };
            $scope.SaveMenu = function () {
                 
                var menuid = 0;
                var BreakfastEntrees = [];
                var BreakfastSides = [];
                var LunchEntrees = [];
                var LunchSides = [];
                var DinnerEntrees = [];
                var DinnerSides = [];
                var ChildEntrees = [];
                var ChildSides = [];
                var Desserts = [];
                var OtherEntrees = [];
                var OtherSides = [];
                var Soups = [];
                var Salads = [];
                var Beverages = [];
                var Snacks = [];
                var Supplements = [];
                var Goods = [];
                var Miscellaneous = [];
                if ($("#ctl00_Body_CurrentMenuId").val() == "") {
                    menuid = 0;
                }
                else {
                    menuid = $("#ctl00_Body_CurrentMenuId").val();
                }
                $('#ctl00_Body_MenuEdit1_epBreakfast_BreakfastEntrees_lstSelectedItems option').each(function () {
                    BreakfastEntrees.push($(this).val());
                });
                $('#ctl00_Body_MenuEdit1_epBreakfast_BreakfastSides_lstSelectedItems option').each(function () {
                    BreakfastSides.push($(this).val());
                });
                $('#ctl00_Body_MenuEdit1_epLunch_LunchEntrees_lstSelectedItems option').each(function () {
                    LunchEntrees.push($(this).val());
                });
                $('#ctl00_Body_MenuEdit1_epLunch_LunchSides_lstSelectedItems option').each(function () {
                    LunchSides.push($(this).val());
                });
                $('#ctl00_Body_MenuEdit1_epDinner_DinnerEntrees_lstSelectedItems option').each(function () {
                    DinnerEntrees.push($(this).val());
                });
                $('#ctl00_Body_MenuEdit1_epDinner_DinnerSides_lstSelectedItems option').each(function () {
                    DinnerSides.push($(this).val());
                });
                $('#ctl00_Body_MenuEdit1_epChild_ChildEntrees_lstSelectedItems option').each(function () {
                    ChildEntrees.push($(this).val());
                });
                $('#ctl00_Body_MenuEdit1_epChild_ChildSides_lstSelectedItems option').each(function () {
                    ChildSides.push($(this).val());
                });
                $('#ctl00_Body_MenuEdit1_epChild_ChildSides_lstSelectedItems option').each(function () {
                    ChildSides.push($(this).val());
                });
                $('#ctl00_Body_MenuEdit1_epDeserts_lstSelectedItems option').each(function () {
                    Desserts.push($(this).val());
                });
                $('#ctl00_Body_MenuEdit1_epOther_OtherEntrees_lstSelectedItems option').each(function () {
                    OtherEntrees.push($(this).val());
                });
                $('#ctl00_Body_MenuEdit1_epOther_OtherSides_lstSelectedItems option').each(function () {
                    OtherSides.push($(this).val());
                });
                $('#ctl00_Body_MenuEdit1_epOther_Soups_lstSelectedItems option').each(function () {
                    Soups.push($(this).val());
                });
                $('#ctl00_Body_MenuEdit1_epOther_Salads_lstSelectedItems option').each(function () {
                    Salads.push($(this).val());
                });
                $('#ctl00_Body_MenuEdit1_epOther_Beverages_lstSelectedItems option').each(function () {
                    Beverages.push($(this).val());
                });
                $('#ctl00_Body_MenuEdit1_epOther_Snacks_lstSelectedItems option').each(function () {
                    Snacks.push($(this).val());
                });
                $('#ctl00_Body_MenuEdit1_epOther_Supplements_lstSelectedItems option').each(function () {
                    Supplements.push($(this).val());
                });
                $('#ctl00_Body_MenuEdit1_epOther_Goods_lstSelectedItems option').each(function () {
                    Goods.push($(this).val());
                });
                $('#ctl00_Body_MenuEdit1_epOther_Misc_lstSelectedItems option').each(function () {
                    Miscellaneous.push($(this).val());
                });
                var _menu = {
                    MenuID: menuid,
                    Name: $("#ctl00_Body_MenuEdit1_txtMenuName").val(),
                    BreakfastEntrees: BreakfastEntrees,
                    BreakfastSides: BreakfastSides,
                    LunchEntrees: LunchEntrees,
                    LunchSides: LunchSides,
                    DinnerEntrees: DinnerEntrees,
                    DinnerSides: DinnerSides,
                    ChildEntrees: ChildEntrees,
                    ChildSides: ChildSides,
                    Desserts: Desserts,
                    OtherEntrees: OtherEntrees,
                    OtherSides: OtherSides,
                    Soups: Soups,
                    Salads: Salads,
                    Beverages: Beverages,
                    Snacks: Snacks,
                    Supplements: Supplements,
                    Goods: Goods,
                    Miscellaneous: Miscellaneous
                }
                $http({
                    method: "POST",
                    url: GetApiBaseURL() + "AddorUpdateMenu",
                    data: _menu
                }).then(function mySuccess(response) {
                     
                    if (response.data.IsSuccess) {
                        alert(response.data.Message)
                    }
                }, function myError(response) {
                     
                    if (response.data.IsSuccess == false) {
                        alert(response.data.Message)
                    }
                });
            }
        });
    </script>
    <script type="text/javascript">
        $(function () {
            ToggleMenus('menus', 'productionmanagement', 'storemanagement');

        });
    </script>
    <style>
    #divMenuEditTabs select option {
        padding:0.5px;
    }
</style>
</asp:Content>
