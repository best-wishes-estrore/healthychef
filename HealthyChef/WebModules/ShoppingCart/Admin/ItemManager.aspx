<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/WebModules/Default.master"
    Theme="WebModules" AutoEventWireup="true" CodeBehind="ItemManager.aspx.cs" Inherits="HealthyChef.WebModules.ShoppingCart.Admin.ItemManager" %>

<%@ Register TagPrefix="hcc" TagName="MenuItem_Edit" Src="~/WebModules/ShoppingCart/Admin/UserControls/MenuItem_Edit.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#divTabs").tabs({
                cookie: {
                    // store cookie for a day, without, it would be a session cookie
                    expires: 1
                }
            });
        });
    </script>
    <div class="main-content" ng-app="UserApp" ng-controller="ItemManager" ng-init="GetData()">
        <asp:HiddenField runat="server" ID="CurrentMenuId" />
        <div class="main-content-inner">
            <a href="../" onclick="MakeUpdateProg(true);" class="btn btn-info b-10"><< Back to Dashboard</a>
            <div class="page-header">
                <h1>Item Manager</h1>
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
            <asp:Button ID="btnAddNewMenuItem" CssClass="btn btn-info" runat="server" Text="Add New Menu Item" />
                </div>
                <asp:Panel ID="pnlEditMenuItem" runat="server" Visible="false">
                    <hcc:MenuItem_Edit ID="MenuItem_Edit1" runat="server" ValidationGroup="MenuItem_EditGroup" />
                </asp:Panel>
                <asp:Panel ID="pnlMenuLists" runat="server" CssClass="col-sm-12">
                    <div id="divTabs">
                        <ul>
                            <li><a href="#active">Active Menu Items</a></li>
                            <li><a href="#retired">Retired Menu Items</a></li>
                        </ul>
                        <div id="active">
                            <%-- <asp:GridView ID="gvwActiveMenuItems"  CssClass="table table-bordered table-hover" runat="server" AutoGenerateColumns="false"
                                DataKeyNames="MenuItemID" AllowPaging="true" PageSize="50" Width="100%" AllowSorting="true">
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="300px" SortExpression="Name" />
                                    <asp:BoundField DataField="MealType" HeaderText="Meal Type" ItemStyle-Width="150px"
                                        SortExpression="MealType" />
                                    <asp:BoundField DataField="CostChild" HeaderText="Price (Ch)" DataFormatString="{0:c}"
                                        ItemStyle-Width="70px" SortExpression="CostChild" />
                                    <asp:BoundField DataField="CostSmall" HeaderText="Price (Sm)" DataFormatString="{0:c}"
                                        ItemStyle-Width="70px" SortExpression="CostSmall" />
                                    <asp:BoundField DataField="CostRegular" HeaderText="Price (Rg)" DataFormatString="{0:c}"
                                        ItemStyle-Width="70px" SortExpression="CostRegular" />
                                    <asp:BoundField DataField="CostLarge" HeaderText="Price (Lg)" DataFormatString="{0:c}"
                                        ItemStyle-Width="70px" SortExpression="CostLarge" />
                                    <asp:BoundField DataField="AllergensList" HeaderText="Allergens" />
                                    <asp:BoundField DataField="IsTaxEligible" HeaderText="Sales Tax Eligible" ItemStyle-Width="50px" />
                                    <asp:CommandField ShowSelectButton="true" SelectText="Edit" ItemStyle-HorizontalAlign="Right" />
                                </Columns>
                                <EmptyDataTemplate>
                                    There are currently no active menu items on record.
                                </EmptyDataTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <AlternatingRowStyle BackColor="#eeeeee" />
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" />
                                <PagerStyle HorizontalAlign="Right" />
                            </asp:GridView>--%>
                            <div class="m-2">
                                <%--<label>Count :  {{((page - 1) * pageSize) + 1}} to  {{ (((page - 1) * pageSize) + pageSize) | number }} of {{ ActiveMenus.length }}</label>--%>
                                <label ng-show="ActiveMenus.length!=0">Count :  {{((page - 1) * pageSize) + 1}} to  {{ ActiveMenus.length <= (((page - 1) * pageSize) + pageSize) ? ActiveMenus.length : (((page - 1) * pageSize) + pageSize) | number }} of {{ ActiveMenus.length }}</label>
                            </div>
                            <table ng-show="ActiveMenus.length!=0" class="table table-bordered table-hover">
                                <thead>
                                    <tr>
                                        <th scope="col" ng-click="orderActiveTable('ITEMNAME')">Name</th>
                                        <th scope="col" ng-click="orderActiveTable('MEALTYPE')">Meal Type</th>
                                        <th scope="col" ng-click="orderActiveTable('COSTCHILD')">Price (Ch)</th>
                                        <th scope="col" ng-click="orderActiveTable('COSTSMALL')">Price (Sm)</th>
                                        <th scope="col" ng-click="orderActiveTable('COSTREGULAR')">Price (Rg)</th>
                                        <th scope="col" ng-click="orderActiveTable('COSTLARGE')">Price (Lg)</th>
                                        <th scope="col">Allergens</th>
                                        <th scope="col">Sales Tax Eligible</th>
                                        <th></th>
                                    </tr>
                                </thead>
                                <tbody>

                                    <tr ng-show="ShowLoader">
                                        <td colspan="9" align="center">
                                            <img src="/App_Themes/HealthyChef/Images/Blocks-1s-200px.gif" alt="Loading..." />
                                        </td>
                                    </tr>

                                    <tr ng-repeat="activemenu in displayActiveMenus =( ActiveMenus | orderBy:OrderByActiveField : Activereverse) |  limitTo:pageSize:pageSize*(page-1) ">
                                        <td>{{activemenu.ITEMNAME}}</td>
                                        <td>{{activemenu.MEALTYPE}}</td>
                                        <td>${{activemenu.COSTCHILD==0?"0.00":activemenu.COSTCHILD}}</td>
                                        <td>${{activemenu.COSTSMALL==0?"0.00":activemenu.COSTSMALL}}</td>
                                        <td>${{activemenu.COSTREGULAR==0?"0.00":activemenu.COSTREGULAR}}</td>
                                        <td>${{activemenu.COSTLARGE==0?"0.00":activemenu.COSTLARGE}}</td>
                                        <td>{{activemenu.ALLERGENS}}</td>
                                        <td>{{activemenu.ISTAXELIGIBLE==true?"True":"False"}}</td>
                                        <td><a href="/WebModules/ShoppingCart/Admin/ItemManager.aspx?MenuId={{activemenu.MENUITEMID}}">Edit</a></td>
                                    </tr>
                                </tbody>
                            </table>
                            <div ng-show="ActiveMenus.length==0" class="aling_center">
                                <p>There are currently no active menu items on record.</p>
                            </div>
                            <ul ng-show="ActiveMenus.length!=0" uib-pagination class="pagination-sm pagination" total-items="ActiveMenus.length" ng-model="page"
                                ng-click="ActivepageChanged()" previous-text="&lsaquo;" next-text="&rsaquo;" items-per-page="pageSize" max-size="10" boundary-links="true">
                            </ul>
                        </div>
                        <div id="retired">
                            <%--             <asp:GridView ID="gvwRetiredMenuItems" CssClass="table table-bordered table-hover" runat="server" AutoGenerateColumns="false"
                                DataKeyNames="MenuItemID" AllowPaging="true" PageSize="50" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="300px" SortExpression="Name" />
                                    <asp:BoundField DataField="MealType" HeaderText="Meal Type" ItemStyle-Width="150px"
                                        SortExpression="MealType" />
                                    <asp:BoundField DataField="CostChild" HeaderText="Price (Ch)" DataFormatString="{0:c}"
                                        ItemStyle-Width="70px" SortExpression="CostChild" />
                                    <asp:BoundField DataField="CostSmall" HeaderText="Price (Sm)" DataFormatString="{0:c}"
                                        ItemStyle-Width="70px" SortExpression="CostSmall" />
                                    <asp:BoundField DataField="CostRegular" HeaderText="Price (Rg)" DataFormatString="{0:c}"
                                        ItemStyle-Width="70px" SortExpression="CostRegular" />
                                    <asp:BoundField DataField="CostLarge" HeaderText="Price (Lg)" DataFormatString="{0:c}"
                                        ItemStyle-Width="70px" SortExpression="CostLarge" />
                                    <asp:BoundField DataField="AllergensList" HeaderText="Allergens" />
                                    <asp:CommandField ShowSelectButton="true" SelectText="Edit" ItemStyle-HorizontalAlign="Right" />
                                </Columns>
                                <EmptyDataTemplate>
                                    There are currently no active menu items on record.
                                </EmptyDataTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <AlternatingRowStyle BackColor="#eeeeee" />
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" />
                                <PagerStyle HorizontalAlign="Right" />
                            </asp:GridView>--%>
                            <div class="m-2">
                                <%--<label>Count :  {{((page1 - 1) * pageSize) + 1}} to  {{ (((page1 - 1) * pageSize) + pageSize) | number }} of {{ RetiredMenus.length }}</label>--%>
                                <label ng-show="RetiredMenus.length !=0">Count :  {{((page1 - 1) * pageSize) + 1}} to  {{ RetiredMenus.length <= (((page1 - 1) * pageSize) + pageSize) ? RetiredMenus.length : (((page1 - 1) * pageSize) + pageSize) | number }} of {{ RetiredMenus.length }}</label>
                            </div>
                            <table ng-show="RetiredMenus.length !=0" class="table table-bordered table-hover">
                                <thead>
                                    <tr>
                                        <th scope="col" ng-click="orderRetiredTable('ITEMNAME')">Name</th>
                                        <th scope="col" ng-click="orderRetiredTable('MEALTYPE')">Meal Type</th>
                                        <th scope="col" ng-click="orderRetiredTable('COSTCHILD')">Price (Ch)</th>
                                        <th scope="col" ng-click="orderRetiredTable('COSTSMALL')">Price (Sm)</th>
                                        <th scope="col" ng-click="orderRetiredTable('COSTREGULAR')">Price (Rg)</th>
                                        <th scope="col" ng-click="orderRetiredTable('COSTLARGE')">Price (Lg)</th>
                                        <th scope="col">Allergens</th>
                                        <th></th>
                                    </tr>
                                </thead>
                                <tbody>

                                    <tr ng-show="ShowLoader">
                                        <td colspan="8" align="center">
                                            <img src="/App_Themes/HealthyChef/Images/Blocks-1s-200px.gif" alt="Loading..." />
                                        </td>
                                    </tr>
                                    <tr ng-repeat="retiremenu in displayRetiredMenus = ( RetiredMenus | orderBy:OrderByRetiredField : Retiredreverse) |  limitTo:pageSize:pageSize*(page1-1)" style="background-color: #FEFEFE;">
                                        <td>{{retiremenu.ITEMNAME}}</td>
                                        <td>{{retiremenu.MEALTYPE}}</td>
                                        <td>${{retiremenu.COSTCHILD==0?"0.00":retiremenu.COSTCHILD}}</td>
                                        <td>${{retiremenu.COSTSMALL==0?"0.00":retiremenu.COSTSMALL}}</td>
                                        <td>${{retiremenu.COSTREGULAR==0?"0.00":retiremenu.COSTREGULAR}}</td>
                                        <td>${{retiremenu.COSTLARGE==0?"0.00":retiremenu.COSTLARGE}}</td>
                                        <td>{{retiremenu.ALLERGENS}}</td>
                                        <td><a href="/WebModules/ShoppingCart/Admin/ItemManager.aspx?MenuId={{retiremenu.MENUITEMID}}">Edit</a></td>
                                    </tr>
                                </tbody>
                            </table>
                            <div ng-show="RetiredMenus.length==0" class="aling_center">
                                <p>There are currently no active menu items on record.</p>
                            </div>
                            <ul ng-show="RetiredMenus.length !=0" uib-pagination class="pagination-sm pagination" total-items="RetiredMenus.length" ng-model="page1"
                                ng-click="RetiredpageChanged()" previous-text="&lsaquo;" next-text="&rsaquo;" items-per-page="pageSize" max-size="10" boundary-links="true">
                            </ul>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    <script>
        var app = angular.module('UserApp', ["ui.bootstrap"]);
        //Getting data from API
        app.controller('ItemManager', function ($scope, $http) {

            //$scope.Users = null;
            //$scope.displayItems = null;
            //$scope.totalRecords = 0;
            $scope.page = 1;
            $scope.page1 = 1;
            $scope.pageSize = 50;
            $scope.ShowLoader = true;

            $scope.ActiveMenus2 = [];
            $scope.RetiredMenus2 = [];
            $scope.GetData = function () {
                $http({
                    method: "GET",
                    url: GetApiBaseURL() + "GetItems"
                }).then(function mySuccess(response) {

                    $scope.Items = response.data;
                    $scope.ActiveMenus = [];
                    $scope.RetiredMenus = [];

                    for (var i = 0; i < $scope.Items.length; i++) {
                        if ($scope.Items[i].ISRETIRED == false) {
                            $scope.ActiveMenus.push($scope.Items[i]);
                            $scope.displayActiveMenus = $scope.ActiveMenus.slice(0, $scope.pageSize);
                            $scope.ActiveMenus2 = $scope.ActiveMenus;
                        }
                        else {
                            $scope.RetiredMenus.push($scope.Items[i]);
                            $scope.displayRetiredMenus = $scope.RetiredMenus.slice(0, $scope.pageSize);
                            $scope.RetiredMenus2 = $scope.RetiredMenus;
                        }
                    }
                    $scope.totalRecords = $scope.ActiveMenus.length;
                    console.log($scope.totalRecords);
                    $scope.ShowLoader = false;
                }, function myError(response) {

                    $scope.ErrorMessage = response.statusText;
                    $scope.ShowLoader = false;
                });
            }
            $scope.ActivepageChanged = function () {

                var startPos = ($scope.page - 1) * $scope.pageSize;
                $scope.displayActiveMenus = $scope.ActiveMenus.slice(startPos, startPos + $scope.pageSize);
            };
            $scope.RetiredpageChanged = function () {

                var startPos = ($scope.page1 - 1) * $scope.pageSize;
                $scope.displayRetiredMenus = $scope.RetiredMenus.slice(startPos, startPos + $scope.pageSize);
            };

            $scope.orderActiveTable = function (x) {
                $scope.Activereverse = ($scope.OrderByActiveField === x) ? !$scope.Activereverse : false;
                $scope.OrderByActiveField = x;
            }

            $scope.orderRetiredTable = function (x) {
                $scope.Retiredreverse = ($scope.OrderByRetiredField === x) ? !$scope.Retiredreverse : false;
                $scope.OrderByRetiredField = x;
            }

            $scope.SaveMenu = function () {

                var menuid = 0;
                var selectedIngredients = [];
                var selectedPrefs = [];
                var usedInMenus = [];
                $('#ctl00_Body_MenuItem_Edit1_lstIngredients_lstSelectedItems option').each(function () {
                    selectedIngredients.push($(this).val());
                });
                $('#ctl00_Body_MenuItem_Edit1_lstPreferences_lstSelectedItems option').each(function () {
                    selectedPrefs.push($(this).val());
                });
                $('#ctl00_Body_MenuItem_Edit1_lstPreferences_lstSelectedItems option').each(function () {
                    usedInMenus.push($(this).val());
                });
                if ($("#ctl00_Body_CurrentMenuId").val() == "") {
                    menuid = 0;
                }
                else {
                    menuid = $("#ctl00_Body_CurrentMenuId").val();
                }
                var item = {
                    MenuItemId: menuid,
                    ItemName: $("#ctl00_Body_MenuItem_Edit1_txtMenuItemName").val(),
                    Description: $("#ctl00_Body_MenuItem_Edit1_txtDescription").val(),
                    MealTypeId: $("#ctl00_Body_MenuItem_Edit1_ddlMealType option:selected").val(),
                    CostChild: $("#ctl00_Body_MenuItem_Edit1_txtCostChild").val(),
                    CostRegular: $("#ctl00_Body_MenuItem_Edit1_txtCostRegular").val(),
                    CostLarge: $("#ctl00_Body_MenuItem_Edit1_txtCostLarge").val(),
                    CostSmall: $("#ctl00_Body_MenuItem_Edit1_txtCostSmall").val(),
                    IsTaxEligible: $("#ctl00_Body_MenuItem_Edit1_chkIsTaxEligible").prop("checked"),
                    IsRetired: $("#ctl00_Body_MenuItem_Edit1_btnDeactivate").val(),
                    Caleries: $("#ctl00_Body_MenuItem_Edit1_txtCalories").val(),
                    DietaryFiber: $("#ctl00_Body_MenuItem_Edit1_txtDietaryFiber").val(),
                    Protein: $("#ctl00_Body_MenuItem_Edit1_txtProtein").val(),
                    TotalCarbohydrates: $("#ctl00_Body_MenuItem_Edit1_txtTotalCarbohydrates").val(),
                    TotalFat: $("#ctl00_Body_MenuItem_Edit1_txtTotalFat").val(),
                    selectedIngredients: selectedIngredients,
                    selectedPrefs: selectedPrefs,
                    usedInMenus: "",
                    UseCostChild: $("#ctl00_Body_MenuItem_Edit1_chkUsePriceChild").prop("checked"),
                    UseCostSmall: $("#ctl00_Body_MenuItem_Edit1_chkUsePriceSmall").prop("checked"),
                    UseCostRegular: $("#ctl00_Body_MenuItem_Edit1_chkUsePriceRegular").prop("checked"),
                    UseCostLarge: $("#ctl00_Body_MenuItem_Edit1_chkUsePriceLarge").prop("checked"),
                    CanyonRanchRecipe: $("#ctl00_Body_MenuItem_Edit1_cbCanyonRanchRecipe").prop("checked"),
                    CanyonRanchApproved: $("#ctl00_Body_MenuItem_Edit1_cbCanyonRanchApproved").prop("checked"),
                    VegetarianOptionAvailable: $("#ctl00_Body_MenuItem_Edit1_cbVegetarianOption").prop("checked"),
                    VeganOptionAvailable: $("#ctl00_Body_MenuItem_Edit1_cbVeganOption").prop("checked"),
                    GlutenFreeOptionAvailable: $("#ctl00_Body_MenuItem_Edit1_cbGlutenFreeOption").prop("checked")
                };

                $http({
                    method: "POST",
                    url: GetApiBaseURL() + "AddOrUpdateItem",
                    data: item
                }).then(function mySuccess(response) {

                    if (response.data.IsSuccess) {
                        alert(response.data.Message)//ctl00_Body_lblFeedback
                        //$scope.GetData();
                        window.location.href = "/WebModules/ShoppingCart/Admin/ItemManager.aspx";
                        var d = new Date();
                        $("#lblFeedback").text("Item saved:  " + d.toLocaleString());
                       
                    }
                }, function myError(response) {

                    if (response.data.IsSuccess == false) {
                        alert(response.data.Message)
                    }
                });
            }

            $scope.Alphas = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "ALL"];

            $scope.SearchByAlpha = function (_alpha, $event) {

                var _anchorTag = $event.currentTarget;
                var activeClass = 'activealphabet';
                $('.search-alphabet').removeClass(activeClass);
                $(_anchorTag).addClass(activeClass);
                if (_alpha !== 'ALL') {
                    $scope.ActiveMenus = $scope.ActiveMenus2.filter((menu) => menu.ITEMNAME.startsWith(_alpha));
                    $scope.RetiredMenus = $scope.RetiredMenus2.filter((menu) => menu.ITEMNAME.startsWith(_alpha));
                }
                else {
                    $scope.ActiveMenus = $scope.ActiveMenus2;
                    $scope.RetiredMenus = $scope.RetiredMenus2;
                }
                $scope.displayActiveMenus = $scope.ActiveMenus.slice(0, $scope.pageSize);
                $scope.displayRetiredMenus = $scope.RetiredMenus.slice(0, $scope.pageSize);
            }

        });


    </script>
    <script type="text/javascript">
        $(function () {
            ToggleMenus('items', 'listmanagement', 'storemanagement');

        });
    </script>

</asp:Content>
