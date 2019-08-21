<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/WebModules/Default.master"
    AutoEventWireup="true" CodeBehind="PlanManager.aspx.cs" Inherits="HealthyChef.WebModules.ShoppingCart.Admin.PlanManager"
    Theme="WebModules" %>

<%@ Register TagPrefix="hcc" TagName="PlanEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/Plan_Edit.ascx" %>
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
    <div class="main-content" ng-app="UserApp" ng-controller="PlanManager">
        <div class="main-content-inner">
            <a href="../" onclick="MakeUpdateProg(true);" class="btn btn-info b-10"><< Back to Dashboard</a>
            <div class="page-header">
                <h1>Plan Manager</h1>
            </div>
            <div class="hcc_admin_ctrl">
                <div>
                    <asp:Label ID="lblFeedback" runat="server" ForeColor="Green" Font-Bold="true" EnableViewState="false" />
                </div>
                <div style="width: 100%; display: inline-block; text-align: right; margin-bottom: 2px;">
                    <asp:Button ID="btnAddNewPlan" CssClass="btn btn-info" runat="server" Text="Add New Plan" />
                </div>
                <asp:Panel ID="pnlEditPlan" runat="server" Visible="false">
                    <hcc:PlanEdit ID="PlanEdit1" runat="server" ValidationGroup="PlanEditGroup" />
                </asp:Panel>
                <asp:Panel ID="pnlPlanLists" runat="server">
                    <div id="divPlanTabs">
                        <ul>
                            <li><a href="#active">Active Plans</a></li>
                            <li><a href="#retired">Retired Plans</a></li>
                        </ul>
                        <div id="active" class="col-sm-12">
                            <%--            <asp:GridView ID="gvwPlansActive" CssClass="table table-bordered table-hover" runat="server" AutoGenerateColumns="false" DataKeyNames="PlanID"
                                AllowPaging="true" PageSize="50" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="275px" />
                                    <asp:BoundField DataField="NumWeeks" HeaderText="# Weeks" ItemStyle-Width="80px" />
                                    <asp:BoundField DataField="NumDaysPerWeek" HeaderText="Days/Week" ItemStyle-Width="80px" />
                                    <asp:BoundField DataField="PricePerDay" HeaderText="Price/Day" DataFormatString="{0:c}"
                                        ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="ProgramID" HeaderText="Program" ItemStyle-Width="275px" />
                                    <asp:BoundField DataField="IsTaxEligible" HeaderText="Tax Eligible" ItemStyle-Width="80px" />
                                    <asp:BoundField DataField="IsDefault" HeaderText="Default" />
                                    <asp:CommandField ShowSelectButton="true" SelectText="Edit" ItemStyle-HorizontalAlign="Right" />
                                </Columns>
                                <EmptyDataTemplate>
                                    There are currently no active plans
                                </EmptyDataTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <AlternatingRowStyle BackColor="#eeeeee" />
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" />
                            </asp:GridView>--%>
                            <div class="m-2">
                                <%--<label>Count :  {{((page - 1) * pageSize) + 1}} to  {{ (((page - 1) * pageSize) + pageSize) | number }} of {{ Activeplans.length }}</label>--%>
                                <label ng-show="Activeplans.length !=0">Count :  {{((page - 1) * pageSize) + 1}} to  {{ Activeplans.length <= (((page - 1) * pageSize) + pageSize) ? Activeplans.length : (((page - 1) * pageSize) + pageSize) | number }} of {{ Activeplans.length }}</label>
                            </div>
                            <table ng-show="Activeplans.length !=0" class="table  table-bordered table-hover">
                                <thead>
                                    <tr>
                                        <th scope="col" ng-click="orderActiveTable('Name')">Name</th>
                                        <th scope="col" ng-click="orderActiveTable('NumWeeks')"># Weeks</th>
                                        <th scope="col" ng-click="orderActiveTable('NumDaysPerWeek')">Days/Week</th>
                                        <th scope="col" ng-click="orderActiveTable('PricePerDay')">Price/Day</th>
                                        <th scope="col" ng-click="orderActiveTable('ProgramName')">Program</th>
                                        <th scope="col">Tax Eligible</th>
                                        <th scope="col">Default</th>
                                        <th></th>
                                    </tr>
                                </thead>
                                <tbody>

                                    <tr ng-show="ShowLoader">
                                        <td colspan="6" align="center">
                                            <img src="/App_Themes/HealthyChef/Images/Blocks-1s-200px.gif" alt="Loading..." />
                                        </td>
                                    </tr>

                                    <tr ng-repeat="activeplan in displayActiveplans | orderBy:OrderByActiveField : Activereverse">
                                        <td>{{activeplan.Name}}</td>
                                        <td>{{activeplan.NumWeeks}}</td>
                                        <td>{{activeplan.NumDaysPerWeek}}</td>
                                        <td>${{activeplan.PricePerDay==0?"0.00":activeplan.PricePerDay}}</td>
                                        <td>{{activeplan.ProgramName}}</td>
                                        <td>{{activeplan.IsTaxEligible==true?"True":"False"}}</td>
                                        <td>{{activeplan.IsDefault==true?"True":"False"}}</td>
                                        <td><a href="/WebModules/ShoppingCart/Admin/PlanManager.aspx?pid={{activeplan.PlanID}}">Edit</a></td>
                                    </tr>
                                </tbody>
                            </table>
                            <div ng-show="Activeplans.length ==0" class="aling_center">
                                <p> There are currently no active plans</p>
                            </div>
                            <ul uib-pagination class="pagination-sm pagination" total-items="Activeplans.length" ng-model="page"
                                ng-click="ActiveplanspageChanged()" previous-text="&lsaquo;" next-text="&rsaquo;" items-per-page="pageSize" max-size="10" boundary-links="true">
                            </ul>
                        </div>
                        <div id="retired" class="col-sm-12">
                            <%--<asp:GridView ID="gvwPlansRetired" CssClass="table table-bordered table-hover" runat="server" AutoGenerateColumns="false" DataKeyNames="PlanID"
                                AllowPaging="true" PageSize="50" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="Name" />
                                    <asp:BoundField DataField="NumWeeks" HeaderText="# Weeks" />
                                    <asp:BoundField DataField="NumDaysPerWeek" HeaderText="Days Per Week" />
                                    <asp:BoundField DataField="PricePerDay" HeaderText="Price Per Day" DataFormatString="{0:c}" />
                                    <asp:BoundField DataField="ProgramID" HeaderText="Program" />
                                    <asp:CommandField ShowSelectButton="true" SelectText="Select" ItemStyle-HorizontalAlign="Right" />
                                </Columns>
                                <EmptyDataTemplate>
                                    There are currently no retired Plans
                                </EmptyDataTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <AlternatingRowStyle BackColor="#eeeeee" />
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" />
                            </asp:GridView>--%>
                            <div class="m-2">
                                <%--<label>Count :  {{((page1 - 1) * pageSize) + 1}} to  {{ (((page1 - 1) * pageSize) + pageSize) | number }} of {{ Retiredplans.length }}</label>--%>
                                <label ng-show="Retiredplans.length !=0">Count :  {{((page1 - 1) * pageSize) + 1}} to  {{ Retiredplans.length <= (((page1 - 1) * pageSize) + pageSize) ? Retiredplans.length : (((page1 - 1) * pageSize) + pageSize) | number }} of {{ Retiredplans.length }}</label>
                            </div>
                            <table ng-show="Retiredplans.length !=0" class="table  table-bordered table-hover">
                                <thead>
                                    <tr align="left">
                                        <th scope="col" ng-click="orderRetiredTable('Name')">Name</th>
                                        <th scope="col" ng-click="orderRetiredTable('NumWeeks')"># Weeks</th>
                                        <th scope="col" ng-click="orderRetiredTable('NumDaysPerWeek')">Days/Week</th>
                                        <th scope="col" ng-click="orderRetiredTable('PricePerDay')">Price/Day</th>
                                        <th scope="col" ng-click="orderRetiredTable('ProgramName')">Program</th>
                                        <th scope="col">Tax Eligible</th>
                                        <th scope="col">Default</th>
                                        <th></th>
                                    </tr>
                                </thead>
                                <tbody>

                                    <tr ng-show="ShowLoader">
                                        <td colspan="6" align="center">
                                            <img src="/App_Themes/HealthyChef/Images/Blocks-1s-200px.gif" alt="Loading..." />
                                        </td>
                                    </tr>

                                    <tr ng-repeat="retiredplan in displayRetiredplans | orderBy:OrderByRetiredField : Retiredreverse">
                                        <td>{{retiredplan.Name}}</td>
                                        <td>{{retiredplan.NumWeeks}}</td>
                                        <td>{{retiredplan.NumDaysPerWeek}}</td>
                                        <td>${{retiredplan.PricePerDay==0?"0.00":retiredplan.PricePerDay}}</td>
                                        <td>{{retiredplan.ProgramName}}</td>
                                        <td>{{retiredplan.IsTaxEligible==true?"True":"False"}}</td>
                                        <td>{{retiredplan.IsDefault==true?"True":"False"}}</td>
                                        <td><a href="/WebModules/ShoppingCart/Admin/PlanManager.aspx?pid={{retiredplan.PlanID}}">Edit</a></td>
                                    </tr>
                                </tbody>
                            </table>
                            <div ng-show="Retiredplans.length ==0" class="aling_center">
                                <p> There are currently no retired Plans</p>
                            </div>
                            <ul uib-pagination class="pagination-sm pagination" total-items="Retiredplans.length" ng-model="page1"
                                ng-click="RetiredplanspageChanged()" previous-text="&lsaquo;" next-text="&rsaquo;" items-per-page="pageSize" max-size="10" boundary-links="true">
                            </ul>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    <script>
        var app = angular.module('UserApp', ["ui.bootstrap"]);
        app.controller('PlanManager', function ($scope, $http) {

            $scope.page = 1;
            $scope.page1 = 1;
            $scope.pageSize = 50;
            
            $scope.ShowLoader = true;
            $http({
                method: "GET",
                url: GetApiBaseURL() + "GetPlans"
            }).then(function mySuccess(response) {
                 
                $scope.plans = response.data;
                $scope.Activeplans = [];
                $scope.Retiredplans = [];

                for (var i = 0; i < $scope.plans.length; i++) {
                    if ($scope.plans[i].IsActive == true) {
                        $scope.Activeplans.push($scope.plans[i]);
                        $scope.displayActiveplans = $scope.Activeplans.slice(0, $scope.pageSize);
                    }
                    else {
                        $scope.Retiredplans.push($scope.plans[i]);
                        $scope.displayRetiredplans = $scope.Retiredplans.slice(0, $scope.pageSize);
                    }
                }
                $scope.totalRecords = $scope.plans.length;
                console.log($scope.totalRecords);
                $scope.ShowLoader = false;
            }, function myError(response) {
                 
                $scope.ErrorMessage = response.statusText;
                $scope.ShowLoader = false;
            });

            $scope.ActiveplanspageChanged = function () {
                 
                var startPos = ($scope.page - 1) * $scope.pageSize;
                $scope.displayActiveplans = $scope.Activeplans.slice(startPos, startPos + $scope.pageSize);
            };

            $scope.RetiredplanspageChanged = function () {
                 
                var startPos = ($scope.page1 - 1) * $scope.pageSize;
                $scope.displayRetiredplans = $scope.Retiredplans.slice(startPos, startPos + $scope.pageSize);
            };

            $scope.orderActiveTable = function (x) {
                $scope.Activereverse = ($scope.OrderByActiveField === x) ? !$scope.Activereverse : false;
                $scope.OrderByActiveField = x;
            }

            $scope.orderRetiredTable = function (x) {
                $scope.Retiredreverse = ($scope.OrderByRetiredField === x) ? !$scope.Retiredreverse : false;
                $scope.OrderByRetiredField = x;
            }

            $scope.AddOrUpdatePlan = function () {
                 
                var planid = $('#ctl00_Body_PlanEdit1_planid').val();
                var planname = $('#ctl00_Body_PlanEdit1_txtPlanName').val();
                var programs = $('#ctl00_Body_PlanEdit1_ddlPrograms').val();
                var chkIsDefault = $('#ctl00_Body_PlanEdit1_chkIsDefault').prop('checked');
                var chkIsTaxEligible = $('#ctl00_Body_PlanEdit1_chkIsTaxEligible').prop('checked');
                var ddlNumWeeks = $('#ctl00_Body_PlanEdit1_ddlNumWeeks').val();
                var pricePerDay = $('#ctl00_Body_PlanEdit1_txtPricePerDay').val();
                var numdays = $('#ctl00_Body_PlanEdit1_ddlNumDays').val();
                var plandesc = $('#ctl00_Body_PlanEdit1_txtPlanDesc').val();




                var data = { PlanID: planid, ProgramID: programs, Name: planname, IsDefault: chkIsDefault, IsTaxEligible: chkIsTaxEligible, NumWeeks: ddlNumWeeks, NumDaysPerWeek: numdays, PricePerDay: pricePerDay, Description: plandesc };

                $http({
                    method: "POST",
                    url: GetApiBaseURL() + "AddOrUpdatePlan",
                    data: data
                }).then(function mySuccess(response) {
                     
                    if (response.data.IsSuccess) {
                        alert(response.data.Message);
                        window.location.href = "/WebModules/ShoppingCart/Admin/PlanManager.aspx";

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
             ToggleMenus('plans', 'listmanagement', 'storemanagement');
         });
    </script>
</asp:Content>
