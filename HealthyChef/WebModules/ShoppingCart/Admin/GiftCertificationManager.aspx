<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/WebModules/Default.master"
    Theme="WebModules" AutoEventWireup="true" CodeBehind="GiftCertificationManager.aspx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.GiftCertificationManager" %>

<%@ Register TagPrefix="hcc" TagName="GiftCertificateEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/GiftCertificate_Edit.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#divCertTabs").tabs({
                cookie: {
                    // store cookie for a day, without, it would be a session cookie
                    expires: 1
                }
            });

            //Set references
            $('#divCertTabs li a').each(function (i, e) {
                $(e).click(function (event) {
                    event.preventDefault();
                    $("#hdnLastTab").val($(this).text());
                });
            });

            var hdnLastTab = $("#hdnLastTab").val();

            if (hdnLastTab.length > 0) {
                var t = $('#divCertTabs li a:contains(' + hdnLastTab + ')');
                var idx = $('#divCertTabs li a').index($('#divCertTabs li a:contains(' + hdnLastTab + ')'));
                $('#divCertTabs').tabs('option', "active", idx);
            }
        });
    </script>

    <asp:HiddenField ID="hdnLastTab" runat="server" ClientIDMode="Static" Value="Issued" />
    <div class="main-content" ng-app="UserApp" ng-controller="GiftCertificates" ng-init="GetData()">
        <asp:HiddenField runat="server" ID="CurrentCartItemId" />
        <div class="main-content-inner">
            <a href="../" onclick="MakeUpdateProg(true);" class="btn btn-info b-10"><< Back to Dashboard</a>
            <div class="page-header">
                <h1>Gift Certificate Manager</h1>
            </div>
            <div class="hcc_admin_ctrl">
                <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnFindGC">
                    <asp:Label ID="lblFeedback" runat="server" ForeColor="Green" Font-Bold="true" EnableViewState="false" />
                    <asp:ValidationSummary ID="VAlSum1" runat="server" ValidationGroup="FinGCGroup" />
                    <div class="col-sm-3 filter-control">
                        <div class="form-group">
                            <label>Start Date:</label>
                            <asp:TextBox ID="txtStartDate" CssClass="form-control datepicker" runat="server" ValidationGroup="FinGCGroup" ng-model="startDate" />
                            <%--<ajax:CalendarExtender ID="cal1" runat="server" TargetControlID="txtStartDate" PopupPosition="TopRight" Format="MM-dd-yyyy" />--%>
                            <asp:CompareValidator ID="cmp1" runat="server" ControlToValidate="txtStartDate"
                                Display="Dynamic" ErrorMessage="Start Date must be a date value." SetFocusOnError="true"
                                Operator="DataTypeCheck" Type="Date" ValidationGroup="FinGCGroup" />
                        </div>
                    </div>
                    <div class="col-sm-3 filter-control">
                        <div class="form-group">
                            <label>End Date:</label>
                            <asp:TextBox ID="txtEndDate" CssClass="form-control datepicker" runat="server" ValidationGroup="FinGCGroup" ng-model="endDate" />
                            <%--<ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtEndDate" PopupPosition="TopRight" Format="MM-dd-yyyy" />--%>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtEndDate"
                                Display="Dynamic" ErrorMessage="End Date must be a date value." SetFocusOnError="true"
                                Operator="DataTypeCheck" Type="Date" ValidationGroup="FinGCGroup" />

                        </div>
                    </div>

                    <div class="col-sm-3 m-2 p-5 filter-control">
                        <asp:Button ID="btnFindGC" CssClass="btn btn-info" runat="server" Text="Find" OnClick="btnFindGC_Click" ValidationGroup="FinGCGroup" Visible="false" />
                        <button type="button" ng-click="GetData();" class="btn btn-info" id="FindCertsBydate">Find</button>
                        <button ng-click="ResetFilters()" class="btn btn-danger" id="UserFiltersReset" type="button">Clear</button>
                    </div>

                    <div class="col-sm-12">
                        <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="txtEndDate" ControlToCompare="txtStartDate"
                            Display="Dynamic" ErrorMessage="Start Date must occur before End Date." SetFocusOnError="true"
                            Operator="GreaterThan" Type="Date" ValidationGroup="FinGCGroup" />
                    </div>

                </asp:Panel>
                <div class="clearfix"></div>
                <asp:Panel ID="pnlGrids" runat="server" CssClass="col-sm-12">
                    <div id="divCertTabs">
                        <ul>
                            <li><a href="#active" onclick="SetCurrentTab(0);">Issued</a></li>
                            <li><a href="#redeemed" onclick="SetCurrentTab(1);">Redeemed</a></li>
                            <li><a href="#imported" onclick="SetCurrentTab(2);">Imported</a></li>
                        </ul>
                        <div id="active">
                            <%--<asp:GridView ID="gvwIssuedCerts" runat="server" AutoGenerateColumns="false" DataKeyNames="CartItemID"
                                Width="100%" CssClass="table table-bordered table-hover" ShowFooter="true">
                                <Columns>
                                    <asp:BoundField DataField="Gift_RedeemCode" HeaderText="Redeem Code" />
                                    <asp:BoundField DataField="ItemPrice" HeaderText="Amount" DataFormatString="{0:c}" />
                                    <asp:BoundField DataField="Gift_IssuedTo" HeaderText="Issued To" />
                                    <asp:BoundField DataField="Gift_IssuedDate" HeaderText="Issued Date" />
                                    <asp:BoundField DataField="IsCompleted" HeaderText="Sent To Recipient" />
                                    <asp:CommandField ShowSelectButton="true" SelectText="Edit" />
                                </Columns>
                                <HeaderStyle HorizontalAlign="Left" />
                                <AlternatingRowStyle BackColor="#eeeeee" />
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" />
                                <PagerStyle HorizontalAlign="Right" />
                                <EmptyDataTemplate>
                                    There are currently no issued gift certificates.
                                </EmptyDataTemplate>
                            </asp:GridView>--%>
                            <div>
                                <div class="m-2">
                                    <%--<label>Count :  {{((page - 1) * pageSize) + 1}} to  {{ (((page - 1) * pageSize) + pageSize) | number }} of {{ Issuedcards.length }}</label>--%>
                                    <label ng-show="Issuedcards.length != 0">Count :  {{((page - 1) * pageSize) + 1}} to  {{ Issuedcards.length <= (((page - 1) * pageSize) + pageSize) ? Issuedcards.length : (((page - 1) * pageSize) + pageSize) | number }} of {{ Issuedcards.length }}</label>
                                </div>
                                <table ng-show="Issuedcards.length != 0" class="table  table-bordered table-hover">
                                    <thead>
                                        <tr align="left">
                                            <th scope="col" ng-click="orderIssuedCertsTable('RedeemCode')">Redeem Code</th>
                                            <th scope="col" ng-click="orderIssuedCertsTable('Amount')">Amount</th>
                                            <th scope="col" ng-click="orderIssuedCertsTable('IssuedTo')">Issued To</th>
                                            <th scope="col" ng-click="orderIssuedCertsTable('IssuedDateObj')">Issued Date</th>
                                            <th scope="col">Sent To Recipient</th>
                                            <th></th>
                                        </tr>
                                    </thead>
                                    <tbody>

                                        <tr ng-show="ShowLoaderIssued">
                                            <td colspan="6" align="center">
                                                <img src="/App_Themes/HealthyChef/Images/Blocks-1s-200px.gif" alt="Loading..." />
                                            </td>
                                        </tr>
                                         <%--| orderBy:OrderByIssuedField : Issuedreverse--%>
                                        <tr ng-repeat="issuecard in displayIssuedcards = ( Issuedcards) | limitTo:pageSize:pageSize*(page-1) " style="background-color: #FEFEFE;">
                                            <td>{{issuecard.RedeemCode}}</td>
                                            <td>{{issuecard.RedeemCode==null? issuecard.IssuedTotal:issuecard.Amount}}</td>
                                            <td>{{issuecard.RedeemCode==null? "":issuecard.IssuedTo}}</td>
                                            <td>{{issuecard.RedeemCode==null? "":issuecard.IssuedDate}}</td>
                                            <td>{{issuecard.RedeemCode==null? "":(issuecard.SendToRecipient==true?"True":"False")}}</td>
                                            <td><a href="/WebModules/ShoppingCart/Admin/GiftCertificationManager.aspx?CartId={{issuecard.CartItemId}}">{{issuecard.RedeemCode==null? "":"Edit"}}</a></td>
                                        </tr>
                                        
                                    </tbody>
                                </table>

                                <div ng-show="Issuedcards.length == 0" class="aling_center">
                                    <p>There are currently no issued gift certificates.</p>
                                </div>
                                <ul ng-show="Issuedcards.length != 0" uib-pagination class="pagination-sm pagination" total-items="Issuedcards.length" ng-model="page"
                                    ng-click="IssuedcardspageChanged()" previous-text="&lsaquo;" next-text="&rsaquo;" items-per-page="pageSize" max-size="10" boundary-links="true">
                                </ul>
                            </div>
                        </div>
                        <div id="redeemed">

                            <%--<asp:GridView ID="gvwRedeemedCerts" CssClass="table table-bordered table-hover" runat="server" AutoGenerateColumns="false" DataKeyNames="CartItemID" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="Gift_RedeemCode" HeaderText="Redeem Code" />
                                    <asp:BoundField DataField="ItemPrice" HeaderText="Amount" DataFormatString="{0:c}" />
                                    <asp:BoundField DataField="Gift_IssuedTo" HeaderText="Issued To" />
                                    <asp:BoundField DataField="Gift_IssuedDate" HeaderText="Issued Date" />
                                    <asp:BoundField DataField="Gift_RedeemedBy" HeaderText="Redeemed By" />
                                    <asp:BoundField DataField="Gift_RedeemedDate" HeaderText="Redeemed Date" />
                                    <asp:CommandField ShowSelectButton="true" SelectText="View" />
                                </Columns>
                                <HeaderStyle HorizontalAlign="Left" />
                                <AlternatingRowStyle BackColor="#eeeeee" />
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" />
                                <PagerStyle HorizontalAlign="Right" />
                                <EmptyDataTemplate>
                                    There are currently no redeemed gift certificates
                                </EmptyDataTemplate>
                            </asp:GridView>--%>
                            <div>
                                <div class="m-2">
                                    <%--<label>Count :  {{((page1 - 1) * pageSize) + 1}} to  {{ (((page1 - 1) * pageSize) + pageSize) | number }} of {{ Redeemedcards.length }}</label>--%>
                                    <label ng-show="Redeemedcards.length != 0">Count :  {{((page1 - 1) * pageSize) + 1}} to  {{ Redeemedcards.length <= (((page1 - 1) * pageSize) + pageSize) ? Redeemedcards.length : (((page1 - 1) * pageSize) + pageSize) | number }} of {{ Redeemedcards.length }}</label>
                                </div>
                                <table ng-show="Redeemedcards.length != 0" class="table  table-bordered table-hover">
                                    <thead>
                                        <tr align="left">
                                            <th scope="col" ng-click="orderRedeemedCertsTable('RedeemCode')">Redeem Code</th>
                                            <th scope="col" ng-click="orderRedeemedCertsTable('Amount')">Amount</th>
                                            <th scope="col" ng-click="orderRedeemedCertsTable('IssuedTo')">Issued To</th>
                                            <th scope="col" ng-click="orderRedeemedCertsTable('IssuedDateObj')">Issued Date</th>
                                            <th scope="col" ng-click="orderRedeemedCertsTable('RedeemedBy')">Redeemed By</th>
                                            <th scope="col" ng-click="orderRedeemedCertsTable('RedeemedDate')">Redeemed Date</th>
                                            <th></th>
                                        </tr>
                                    </thead>
                                    <tbody>

                                        <tr ng-show="ShowLoaderRedeemed">
                                            <td colspan="6" align="center">
                                                <img src="/App_Themes/HealthyChef/Images/Blocks-1s-200px.gif" alt="Loading..." />
                                            </td>
                                        </tr>

                                        <tr ng-repeat="redeem in displayRedeemedcards = ( Redeemedcards | orderBy:OrderByRedeemedField : Redeemedreverse) | limitTo:pageSize:pageSize*(page1-1) " style="background-color: #FEFEFE;">
                                            <td>{{redeem.RedeemCode}}</td>
                                            <td>{{redeem.Amount}}</td>
                                            <td>{{redeem.IssuedTo}}</td>
                                            <td>{{redeem.IssuedDate}}</td>
                                            <td>{{redeem.RedeemedBy}}</td>
                                            <td>{{redeem.RedeemedDate}}</td>
                                            <td><a href="/WebModules/ShoppingCart/Admin/GiftCertificationManager.aspx?CartId={{redeem.CartItemId}}">View</a></td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div ng-show="Redeemedcards.length == 0" class="aling_center">
                                    <p>  There are currently no redeemed gift certificates</p>
                                </div>
                                <ul ng-show="Redeemedcards.length != 0" uib-pagination class="pagination-sm pagination" total-items="Redeemedcards.length" ng-model="page1"
                                    ng-click="RedeemedcardspageChanged()" previous-text="&lsaquo;" next-text="&rsaquo;" items-per-page="pageSize" max-size="10" boundary-links="true">
                                </ul>
                            </div>
                        </div>
                        <div id="imported">
                            <%-- <asp:GridView ID="gvwImportedGiftCerts" CssClass="table table-bordered table-hover" runat="server" AutoGenerateColumns="false" Width="100%"
                                DataKeyNames="ImportsGCID" AllowPaging="true" AllowSorting="true" PageSize="25" Font-Size="10px">
                                <Columns>
                                    <asp:BoundField DataField="code" HeaderText="Code" />
                                    <asp:BoundField DataField="amount" HeaderText="Amount" DataFormatString="{0:c}" />
                                    <asp:BoundField DataField="date_added" HeaderText="Date Added" />
                                    <asp:BoundField DataField="date_expiration" HeaderText="Date Expires" />
                                    <asp:CommandField ShowSelectButton="true" />
                                </Columns>
                                <HeaderStyle HorizontalAlign="Left" />
                                <AlternatingRowStyle BackColor="#eeeeee" />
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" />
                                <PagerStyle HorizontalAlign="Right" />
                                <EmptyDataTemplate>
                                    There are currently no imported gift certificates
                                </EmptyDataTemplate>
                            </asp:GridView>--%>
                            <div>
                                <div class="m-2">
                                    <%--<label>Count :  {{((page2 - 1) * pageSize) + 1}} to  {{ (((page2 - 1) * pageSize) + pageSize) | number }} of {{ Importedcards.length }}</label>--%>
                                    <label ng-show="Importedcards.length!=0">Count :  {{((page2 - 1) * pageSize) + 1}} to  {{ Importedcards.length <= (((page2 - 1) * pageSize) + pageSize) ? Importedcards.length : (((page2 - 1) * pageSize) + pageSize) | number }} of {{ Importedcards.length }}</label>
                                </div>
                                <table ng-show="Importedcards.length!=0" class="table  table-bordered table-hover">
                                    <thead>
                                        <tr align="left">
                                            <th scope="col" ng-click="orderImportedCertsTable('Code')">Code</th>
                                            <th scope="col" ng-click="orderImportedCertsTable('Amount')">Amount</th>
                                            <th scope="col" ng-click="orderImportedCertsTable('DateAdded')">Date Added</th>
                                            <th scope="col" ng-click="orderImportedCertsTable('DateExpires')">Date Expires</th>
                                            <th></th>
                                        </tr>
                                    </thead>
                                    <tbody>

                                        <tr ng-show="ShowLoaderImported">
                                            <td colspan="6" align="center">
                                                <img src="/App_Themes/HealthyChef/Images/Blocks-1s-200px.gif" alt="Loading..." />
                                            </td>
                                        </tr>

                                        <tr ng-repeat="import in displayImportedcards = ( Importedcards | orderBy:OrderByImportedField : Importedreverse) | limitTo:pageSize:pageSize*(page2-1) " style="background-color: #FEFEFE;">
                                            <td>{{import.Code}}</td>
                                            <td>{{import.Amount}}</td>
                                            <td>{{import.DateAdded}}</td>
                                            <td>{{import.DateExpires}}</td>
                                            <td><a href="/WebModules/ShoppingCart/Admin/GiftCertificationManager.aspx?ImportCartId={{import.ImportId}}">Select</a></td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div ng-show="Importedcards.length == 0" class="aling_center">
                                    <p> There are currently no imported gift certificates</p>
                                </div>
                                <ul ng-show="Importedcards.length!=0" uib-pagination class="pagination-sm pagination" total-items="Importedcards.length" ng-model="page2"
                                    ng-click="ImportedcardspageChanged()" previous-text="&lsaquo;" next-text="&rsaquo;" items-per-page="pageSize" max-size="10" boundary-links="true">
                                </ul>
                            </div>
                            <asp:Panel ID="pnlImpCertEdit" runat="server" Visible="false">
                                <p class="col-sm-2">Code:</p>
                                <asp:Label ID="lblCode" runat="server" /><br /><br />
                                <p class="col-sm-2">Amount:</p>
                                <asp:TextBox ID="txtAmount" runat="server" /><br /><br />
                                <p class="col-sm-2">Redeemed:</p>
                                <asp:CheckBox ID="chkRedeemed" runat="server" /><br /><br />
                                <p class="col-sm-2">Date Used:</p>
                                <asp:TextBox ID="txtDateUsed" runat="server" /><br /><br />
                                <asp:Button ID="btnSave" runat="server" Text="Save" Visible="false" />
                                <button type="button" class="btn btn-info m-2" ng-click="SaveImportGift()">Save</button>
                            </asp:Panel>
                            <asp:Label ID="lblFBImpCert" runat="server" EnableViewState="false" />
                        </div>
                    </div>

                </asp:Panel>
                <div id="divEdit" runat="server" visible="false">
                    <hcc:GiftCertificateEdit ID="GiftCertificateEdit1" runat="server" ValidationGroup="GiftCertificateEditGroup" ShowSentToRecipCheckbox="true" />
                </div>
            </div>
        </div>
    </div>
    <script>
        var app = angular.module('UserApp', ["ui.bootstrap"]);
        //Getting data from API
        app.controller('GiftCertificates', function ($scope, $http, $location) {

            //$scope.Users = null;
            //$scope.displayItems = null;
            //$scope.totalRecords = 0;

            $scope.page = 1;
            $scope.page1 = 1;
            $scope.page2 = 1;
            $scope.pageSize = 50;
            $scope.ShowLoaderIssued = true;
            $scope.ShowLoaderRedeemed = true;
            $scope.ShowLoaderImported = true;

            $scope.GetData = function () {

                $http({
                    method: "GET",
                    url: GetApiBaseURL() + "GetIssuedCerts/" + $scope.startDate + "/" + $scope.endDate,
                }).then(function mySuccess(response) {

                    $scope.Issuedcards = response.data;
                    $scope.displayIssuedcards = $scope.Issuedcards.slice(0, $scope.pageSize);
                    $scope.totalRecords = $scope.Issuedcards.length;
                    console.log($scope.totalRecords);
                    $scope.ShowLoaderIssued = false;
                }, function myError(response) {

                    $scope.ErrorMessage = response.statusText;
                    $scope.ShowLoaderIssued = false;
                });
                $http({
                    method: "GET",
                    url: GetApiBaseURL() + "GetRedeemdedCerts/" + $scope.startDate + "/" + $scope.endDate,
                }).then(function mySuccess(response) {

                    $scope.Redeemedcards = response.data;
                    $scope.displayRedeemedcards = $scope.Redeemedcards.slice(0, $scope.pageSize);
                    $scope.totalRecords = $scope.Redeemedcards.length;
                    console.log($scope.totalRecords);
                    $scope.ShowLoaderRedeemed = false;
                }, function myError(response) {

                    $scope.ErrorMessage = response.statusText;
                    $scope.ShowLoaderRedeemed = false;
                });
                $http({
                    method: "GET",
                    url: GetApiBaseURL() + "GetImported"
                }).then(function mySuccess(response) {

                    $scope.Importedcards = response.data;
                    $scope.displayImportedcards = $scope.Importedcards.slice(0, $scope.pageSize);
                    $scope.totalRecords = $scope.Importedcards.length;
                    console.log($scope.totalRecords);
                    $scope.ShowLoaderImported = false;
                }, function myError(response) {

                    $scope.ErrorMessage = response.statusText;
                    $scope.ShowLoaderImported = false;
                });
            }
            $scope.IssuedcardspageChanged = function () {

                var startPos = ($scope.page - 1) * $scope.pageSize;
                $scope.displayIssuedcards = $scope.Issuedcards.slice(startPos, startPos + $scope.pageSize);
            };

            $scope.RedeemedcardspageChanged = function () {

                var startPos = ($scope.page1 - 1) * $scope.pageSize;
                $scope.displayRedeemedcards = $scope.Redeemedcards.slice(startPos, startPos + $scope.pageSize);
            };

            $scope.ImportedcardspageChanged = function () {

                var startPos = ($scope.page2 - 1) * $scope.pageSize;
                $scope.displayImportedcards = $scope.Importedcards.slice(startPos, startPos + $scope.pageSize);
            };

            $scope.orderIssuedCertsTable = function (x) {
                $scope.Issuedreverse = ($scope.OrderByIssuedField === x) ? !$scope.Issuedreverse : false;
                $scope.OrderByIssuedField = x;
            }

            $scope.orderRedeemedCertsTable = function (x) {
                $scope.Redeemedreverse = ($scope.OrderByRedeemedField === x) ? !$scope.Redeemedreverse : false;
                $scope.OrderByRedeemedField = x;
            }

            $scope.orderImportedCertsTable = function (x) {
                $scope.Importedreverse = ($scope.OrderByImportedField === x) ? !$scope.Importedreverse : false;
                $scope.OrderByImportedField = x;
            }

            $scope.FindCertsByDates = function () {
                console.log('FindCertsByDates');
            };

            $scope.SaveGiftCertificate = function () {
                var certificate = {
                    CartId: $('#ctl00_Body_GiftCertificateEdit1_CartId').val(),
                    CartItemId: $("#ctl00_Body_CurrentCartItemId").val(),
                    Amount: $("#ctl00_Body_GiftCertificateEdit1_txtAmount").val(),
                    FirstName: $("#ctl00_Body_GiftCertificateEdit1_AddressRecipient_txtFirstName").val(),
                    LastName: $("#ctl00_Body_GiftCertificateEdit1_AddressRecipient_txtLastName").val(),
                    Address1: $("#ctl00_Body_GiftCertificateEdit1_AddressRecipient_txtAddress1").val(),
                    Address2: $("#ctl00_Body_GiftCertificateEdit1_AddressRecipient_txtAddress2").val(),
                    City: $("#ctl00_Body_GiftCertificateEdit1_AddressRecipient_txtCity").val(),
                    State: $("#ctl00_Body_GiftCertificateEdit1_AddressRecipient_ddlUSStates option:selected").val(),
                    Zipcode: $("#ctl00_Body_GiftCertificateEdit1_AddressRecipient_txtZipCode").val(),
                    Phone: $("#ctl00_Body_GiftCertificateEdit1_AddressRecipient_txtPhone").val(),
                    ReceipientEmail: $("#ctl00_Body_GiftCertificateEdit1_txtRecipEmail").val(),
                    ReceipientMessage: $("#ctl00_Body_GiftCertificateEdit1_txtRecipMessage").val(),
                    SendtoReceipient: $("#chkSentToRecip").prop("checked"),
                    AddressTypeID: $("#ctl00_Body_GiftCertificateEdit1_AddressRecipient_Addresstype").val(),
                    AddressID: 0
                };
                $http({
                    method: "POST",
                    url: GetApiBaseURL() + "UpdateGiftCertificates",
                    data: certificate
                }).then(function mySuccess(response) {
                    if (response.data.IsSuccess) {
                        alert(response.data.Message);
                        window.location.href = "/WebModules/ShoppingCart/Admin/GiftCertificationManager.aspx";
                    }
                }, function myError(response) {
                    if (response.data.IsSuccess == false) {
                        alert(response.data.Message);

                    }
                });
            };
            $scope.SaveImportGift = function () {

                var certificate = {
                    ImportedGiftcertId: $("#ctl00_Body_CurrentCartItemId").val(),
                    Code: $("#ctl00_Body_lblCode").text(),
                    Amount: $("#ctl00_Body_txtAmount").val(),
                    Redeemed: $("#ctl00_Body_chkRedeemed").prop("checked"),
                    DateUsed: $("#ctl00_Body_txtDateUsed").val()
                }
                $http({
                    method: "POST",
                    url: GetApiBaseURL() + "UpdateImportedGiftCertificate",
                    data: certificate
                }).then(function mySuccess(response) {

                    if (response.data.IsSuccess) {
                        alert(response.data.Message);
                        window.location.href = "/WebModules/ShoppingCart/Admin/GiftCertificationManager.aspx";
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
                $scope.GetData();
            }

        });
    </script>
    <script type="text/javascript">
        function SetCurrentTab(curTab) {
            $('#divCertTabs').tabs('option', "active", curTab);
            localStorage.setItem('currenttab', curTab);
            if (curTab == '2') {
                $('.filter-control').hide('fade');
            }
            else {
                $('.filter-control').show('fade');
            }
        }

        $(function () {
            ToggleMenus('giftcertificates', 'productionmanagement', 'storemanagement');

            var _activeTab = localStorage.getItem('currenttab');
            if (_activeTab !== '' && _activeTab !== undefined && _activeTab !== null) {
                SetCurrentTab(_activeTab);
            }
            else {
                SetCurrentTab(0);
            }

        });
    </script>
</asp:Content>
