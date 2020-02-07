<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/WebModules/Default.master"
    Theme="WebModules" AutoEventWireup="true" CodeBehind="ProgramManager.aspx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.ProgramManager" %>

<%@ Register TagPrefix="hcc" TagName="ProgramEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/Program_Edit.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#divProgramTabs").tabs({
                cookie: {
                    // store cookie for a day, without, it would be a session cookie
                    expires: 1
                }
            });
        });
    </script>
    <div class="main-content" ng-app="UserApp" ng-controller="Getprograms">
        <div class="main-content-inner">
            <a href="../" onclick="MakeUpdateProg(true);" class="btn btn-info b-10"><< Back to Dashboard</a>
            <div class="page-header">
                <h1>Program Manager</h1>
            </div>
            <div class="hcc_admin_ctrl">
                <div>
                    <asp:Label ID="lblFeedback" runat="server" ForeColor="Green" Font-Bold="true" EnableViewState="false" />
                </div>
                <div style="width: 100%; display: inline-block; text-align: right; margin-bottom: 2px;">
                    <asp:Button ID="btnAddNewProgram" CssClass="btn btn-info" runat="server" Text="Add New Program" />
                </div>
                <asp:Panel ID="pnlEditProgram" runat="server" Visible="false">
                    <hcc:ProgramEdit ID="ProgramEdit1" runat="server" ValidationGroup="ProgramEditGroup" />
                </asp:Panel>
                <asp:Panel ID="pnlProgramLists" runat="server" CssClass="col-sm-12">
                    <div id="divProgramTabs">
                        <ul>
                            <li><a href="#active">Active Programs</a></li>
                            <li><a href="#retired">Retired Programs</a></li>
                        </ul>
                        <div id="active">
                            <%-- <asp:GridView ID="gvwProgramsActive"  CssClass="table table-bordered table-hover" runat="server" AutoGenerateColumns="false" DataKeyNames="ProgramID" AllowPaging="true"
                                PageSize="50" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="Name" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                    <asp:CommandField ShowSelectButton="true" SelectText="Select" ItemStyle-HorizontalAlign="Right" />
                                </Columns>
                                <EmptyDataTemplate>
                                    There are currently no active Programs
                                </EmptyDataTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <AlternatingRowStyle BackColor="#eeeeee" />
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" />
                            </asp:GridView>--%>
                            <label ng-show="Activeprograms.length!=0">Count :  {{((page - 1) * pageSize) + 1}} to  {{ displayActiveprograms.length <= (((page - 1) * pageSize) + pageSize) ? displayActiveprograms.length : (((page - 1) * pageSize) + pageSize) | number }} of {{ displayActiveprograms.length }}</label>
                            <table ng-show="Activeprograms.length!=0" class="table table-bordered table-hover">
                                <thead>
                                    <tr align="left">
                                        <th scope="col" ng-click="orderActiveTable('Name')">Name</th>
                                        <th scope="col" ng-click="orderActiveTable('Description')">Description</th>
                                        <th scope="col">&nbsp;</th>
                                    </tr>
                                </thead>

                                <tbody>
                                    <tr style="background-color: #FEFEFE;" ng-repeat="u in displayActiveprograms = (Activeprograms | orderBy:OrderByActiveField : Activereverse)">
                                        <td><span>{{ u.Name }}</span></td>
                                        <td>{{ u.Description }}</td>
                                        <td align="left"><a href="/WebModules/ShoppingCart/Admin/ProgramManager.aspx?ProgramID={{u.ProgramID}}">Select</a></td>
                                    </tr>
                                </tbody>
                            </table>
                            <ul ng-show="Activeprograms.length!=0" uib-pagination class="pagination-sm pagination" total-items="Activeprograms.length" ng-model="page"
                                ng-change="pageChangedActive()" previous-text="&lsaquo;" next-text="&rsaquo;" items-per-page="pageSize" max-size="10" boundary-links="true">
                            </ul>
                            <div ng-show="Activeprograms.length==0" class="aling_center">
                                <p>There are currently no active Programs</p>
                            </div>
                        </div>
                        <div id="retired">
                            <%--<asp:GridView ID="gvwProgramsRetired" CssClass="table table-bordered table-hover" runat="server" AutoGenerateColumns="false" DataKeyNames="ProgramID" AllowPaging="true"
                                PageSize="50" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="Name" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                    <asp:CommandField ShowSelectButton="true" SelectText="Select" ItemStyle-HorizontalAlign="Right" />
                                </Columns>
                                <EmptyDataTemplate>
                                    There are currently no retired Programs
                                </EmptyDataTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <AlternatingRowStyle BackColor="#eeeeee" />
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" />
                            </asp:GridView>--%>
                            <label ng-show="Retiredprograms.length!=0">Count :  {{((page1 - 1) * pageSize) + 1}} to  {{ displayRetiredprograms.length <= (((page1 - 1) * pageSize) + pageSize) ? displayRetiredprograms.length : (((page1 - 1) * pageSize) + pageSize) | number }} of {{ displayRetiredprograms.length }}</label>
                            <table ng-show="Retiredprograms.length!=0" class="table table-bordered table-hover">
                                <thead>
                                    <tr align="left">
                                        <th scope="col" ng-click="orderRetiredTable('Name')">Name</th>
                                        <th scope="col" ng-click="orderRetiredTable('Description')">Description</th>
                                        <th scope="col">&nbsp;</th>
                                    </tr>
                                </thead>

                                <tbody>
                                    <tr style="background-color: #FEFEFE;" ng-repeat="u in displayRetiredprograms = (Retiredprograms  | orderBy:OrderByRetiredField : Retiredreverse)">
                                        <td><span>{{ u.Name }}</span></td>
                                        <td>{{ u.Description }}</td>
                                        <td align="left"><a href="/WebModules/ShoppingCart/Admin/ProgramManager.aspx?ProgramID={{u.ProgramID}}">Select</a></td>
                                    </tr>
                                </tbody>
                            </table>
                            <ul ng-show="Retiredprograms.length!=0" uib-pagination class="pagination-sm pagination" total-items="Retiredprograms.length" ng-model="page1"
                                ng-change="pageChangedRetired()" previous-text="&lsaquo;" next-text="&rsaquo;" items-per-page="pageSize" max-size="10" boundary-links="true">
                            </ul>
                            <div ng-show="Retiredprograms.length==0" class="aling_center">
                                <p>  There are currently no retired Programs</p>
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
        app.controller('Getprograms', function ($scope, $http) {

            $scope.page = 1;
            $scope.page1 = 1;
            $scope.pageSize = 50;
            $scope.active = true;
            var activeprograms = new Array();
            var retiredprograms = new Array();

            $http({
                method: "GET",
                url: GetApiBaseURL() + "GetPrograms"
            }).then(function mySuccess(response) {
                angular.forEach(response.data, function (value, key) {
                    if (response.data[key].IsActive == true) {
                        activeprograms.push(response.data[key]);
                    }
                    else {
                        retiredprograms.push(response.data[key]);
                    }
                });
                $scope.Activeprograms = activeprograms;
                $scope.Retiredprograms = retiredprograms;
                console.log(activeprograms.length);
                console.log(retiredprograms.length);
            }, function myError(response) {
                $scope.ErrorMessage = response.statusText;
            });

            $scope.GetDetails = function (index) {

            };

            $scope.pageChangedActive = function () {

                var startPos = ($scope.page - 1) * $scope.pageSize;
                $scope.displayActiveprograms = $scope.Activeprograms.slice(startPos, startPos + $scope.pageSize);
                //$scope.Pager = "Count :  " + (startPos + 1) + " to " + (startPos + $scope.pageSize) + " of " + $scope.totalRecords;
            };

            $scope.pageChangedRetired = function () {

                var startPos = ($scope.page1 - 1) * $scope.pageSize;                
                $scope.displayRetiredprograms = $scope.Retiredprograms.slice(startPos, startPos + $scope.pageSize);
                //$scope.Pager = "Count :  " + (startPos + 1) + " to " + (startPos + $scope.pageSize) + " of " + $scope.totalRecords;
            };

            $scope.orderActiveTable = function (x) {
                $scope.Activereverse = ($scope.OrderByActiveField === x) ? !$scope.Activereverse : false;
                $scope.OrderByActiveField = x;
            }

            $scope.orderRetiredTable = function (x) {
                $scope.Retiredreverse = ($scope.OrderByRetiredField === x) ? !$scope.Retiredreverse : false;
                $scope.OrderByRetiredField = x;
            }

        });
    </script>
    <script type="text/javascript">
        $(function () {
            ToggleMenus('programs', 'listmanagement', 'storemanagement');

        });
    </script>
</asp:Content>
