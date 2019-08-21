<%@ Page Title="" Language="C#" Theme="WebModules" MasterPageFile="~/Templates/WebModules/Default.master"
    AutoEventWireup="true" CodeBehind="AccountManager.aspx.cs" MaintainScrollPositionOnPostback="true"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.AccountManager" %>



<%@ Register TagPrefix="hcc" TagName="UserProfileEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/UserProfile_Edit.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
    <style type="text/css">
        .search {
            font-size: 11px;
        }

            .search input[type=text] {
                font-size: 11px;
            }

        .cart-list li {
            list-style: none;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.22/pdfmake.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/0.4.1/html2canvas.min.js"></script>
    <div class="main-content" ng-app="UserApp" ng-controller="GetUsers">
        <asp:HiddenField ID="CurrentUserID" runat="server" />
        <asp:HiddenField ID="CurrentLoggedUserID" runat="server" />
        <%--<input type="hidden" ng-model="CurrentUserID" value="" id="CurrentUserID" />--%>
        <div class="main-content-inner">
            <a href="../" onclick="MakeUpdateProg(true);" class="btn btn-info b-10"><< Back to Dashboard</a>
            <div class="page-header">
                <h1>User Account Manager</h1>
            </div>

            <div class="hcc_admin_ctrl">
                <!-- Begin Search Fields -->
                <div id="divSearchPanel" runat="server">
                    <div class="row-fluid">
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label class="search">Customer First/Last Name:</label>
                                <asp:TextBox ID="txtSearchLastName" CssClass="form-control" runat="server" MaxLength="50" ng-model="NameFilter" />
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label class="search">Email:</label>
                                <asp:TextBox ID="txtSearchEmail" CssClass="form-control" runat="server" MaxLength="100" ng-model="EmailFilter" />
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label class="search">Phone:</label>
                                <asp:TextBox ID="txtSearchPhone" CssClass="form-control" runat="server" MaxLength="20" ng-model="PhoneFilter" onkeypress="return isNumeric(event)" />
                                <%--<asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="txtSearchPhone"
                                        Display="Dynamic" ErrorMessage="Phone Number must be numeric." Operator="GreaterThanEqual" Type="Double"
                                        SetFocusOnError="true" ValidationGroup="UserSearchGroup" />--%>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label class="search">Purchase #: </label>
                                <asp:TextBox ID="txtSearchPurchNum" CssClass="form-control" runat="server" MaxLength="15" ng-model="PurchaseNumberFilter" onkeypress="return isNumeric1(event)" />
                                <asp:CompareValidator ID="cpvPurchaseNum" runat="server" ControlToValidate="txtSearchPurchNum"
                                    Display="Dynamic" ErrorMessage="Purchase Number must be numeric." Operator="DataTypeCheck" Type="Integer"
                                    SetFocusOnError="true" ValidationGroup="UserSearchGroup" />
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label class="search">Delivery Date: </label>
                                <asp:DropDownList ID="ddlDeliveryDates" CssClass="form-control" runat="server" Visible="false" />
                                <asp:TextBox ID="DeliveryDateFilter" CssClass="form-control datepicker-fridays" runat="server" MaxLength="20" ng-model="DeliveryDateFilter" placeholder="MM/DD/YYYY" />
                                <%--<ajax:CalendarExtender runat="server" TargetControlID="DeliveryDateFilter" PopupPosition="TopRight" Format="MM/dd/yyyy" />--%>
                                <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="DeliveryDateFilter"
                                    Display="Dynamic" ErrorMessage="Delivery Date must be date." Operator="DataTypeCheck" Type="Date"
                                    SetFocusOnError="true" ValidationGroup="UserSearchGroup" />
                            </div>
                        </div>
                    </div>
                    <div class="clearfix"></div>
                    <div class="rowfluid">
                        <div class="page-header">
                            <h1 class="search">Roles:</h1>
                        </div>
                        <asp:RadioButtonList ID="cblRoles" runat="server" RepeatDirection="Horizontal" Visible="false" />
                        <div id="RolesButtons">

                            <div class="col-sm-3" id="divany" runat="server">
                                <input type="radio" id="" name="RoleFilter" value="any" ng-model="RoleFilter" />
                                <label for="any">Any</label>
                            </div>
                            <div class="col-sm-3" id="divAdministrator" runat="server">
                                <input type="radio" id="59E993C9-2944-4081-9C5B-33FE7D407481" name="RoleFilter" value="Administrators" ng-model="RoleFilter" />
                                <label for="Administrators">Administrators</label>
                            </div>
                            <div class="col-sm-3" id="divCustomer" runat="server">
                                <input type="radio" id="ADD94163-C76B-4C20-81B1-46E9AFA080A8" ng-checked="true" name="RoleFilter" value="Customer" ng-model="RoleFilter" />
                                <label for="Customer">Customer</label>
                            </div>
                            <div class="col-sm-3" id="divEmployeeManager" runat="server">
                                <input type="radio" id="58CB9220-D966-49F2-AE10-6E9F8EA34EE2" name="RoleFilter" value="EmployeeManager" ng-model="RoleFilter" />
                                <label for="EmployeeManager">Employee Manager</label>
                            </div>
                            <div class="col-sm-3" id="divEmployeeProduction" runat="server">
                                <input type="radio" id="0835D12E-07C6-40C3-A8E5-729A0F0C5566" name="RoleFilter" value="EmployeeProduction" ng-model="RoleFilter" />
                                <label for="EmployeeProduction">Employee Production</label>
                            </div>
                            <div class="col-sm-3" id="divEmployeeService" runat="server">
                                <input type="radio" id="E278A78A-9D64-4B63-ACCD-C345C1A346D4" name="RoleFilter" value="EmployeeService" ng-model="RoleFilter" />
                                <label for="EmployeeService">Employee Service</label>
                            </div>
                            <%--<div class="col-sm-3">
                                <input type="radio" name="RoleFilter" value="Others" checked="checked" />
                                <label for="Others">Others</label>
                            </div>--%>
                        </div>
                    </div>
                </div>
            </div>
            <div class="clearfix"></div>
            <div class="searchuser" style="margin: 10px 20px; float: left">
                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-info" Text="Search Users" ValidationGroup="UserSearchGroup" OnClientClick="MakeUpdateProg(true);" Visible="false" />
                <button ng-click="SearchByUsers()" class="btn btn-info" id="Searchbyusers" type="button">Search Users</button>
            </div>
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-danger" CausesValidation="false" OnClientClick="MakeUpdateProg(true);" Visible="false" />
            <%--<button ng-click="SearchByUsers()" class="btn btn-info" id="Searchbyusers" type="button">Search Users</button>--%>
            <div class="align-right" style="margin: 0px 20px; float: right">
                <%--<div class="export">
                    <i class="fa fa-file-pdf-o" aria-hidden="true" ng-click="Export()"></i>
                </div>--%>
                <button ng-click="ResetFilters()" class="btn btn-danger" id="UserFiltersReset" type="button">Clear</button>
                <asp:Button ID="btnAddNewUser" runat="server" CssClass="btn btn-info" Text="Add New User" CausesValidation="false" OnClientClick="MakeUpdateProg(true);" />

                <asp:CompareValidator ID="CompareValidator1" ErrorMessage="Purchase # must be a number." ControlToValidate="txtSearchPurchNum"
                    runat="server" ValidationGroup="UserSearchGroup" EnableClientScript="false" Operator="DataTypeCheck" Type="Integer" />

                <%-- Password:--%>
            </div>
        </div>
        <!-- End Search Fields -->
        <div>
            <asp:Label ID="lblFeedback" runat="server" ForeColor="Green" Font-Bold="true" EnableViewState="false" />
        </div>

        <div id="divEdit" runat="server" visible="false">
            <hcc:UserProfileEdit ID="UserProfileEdit1" runat="server" ValidationGroup="UserProfileEditGroup" />
        </div>
        <asp:Panel ID="pnlGrids" runat="server">
            <asp:Label ID="lblAccountCount" runat="server" />
            <asp:GridView ID="gvwAccounts" runat="server" AutoGenerateColumns="false" Width="100%"
                DataKeyNames="ProviderUserKey" AllowPaging="true" PageSize="50">
                <Columns>
                    <asp:CommandField ShowSelectButton="true" ShowEditButton="false" ShowDeleteButton="false"
                        ItemStyle-HorizontalAlign="Left" />
                    <asp:TemplateField HeaderText="Customer Name">
                        <ItemTemplate>
                            <asp:Label ID="lblFullName" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="IsApproved" HeaderText="Activated" />
                    <asp:BoundField DataField="IsLockedOut" HeaderText="Is Locked Out" />
                    <asp:BoundField DataField="IsOnline" HeaderText="Is Online" />
                    <asp:TemplateField HeaderText="User Role">
                        <ItemTemplate>
                            <asp:Label ID="lblUserRole" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle HorizontalAlign="Left" />
                <EmptyDataTemplate>
                    There are currently no accounts on record for this website.
                </EmptyDataTemplate>
                <RowStyle BackColor="#fefefe" />
                <AlternatingRowStyle BackColor="#eeeeee" />
                <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" />
            </asp:GridView>


            <div class="col-sm-12">
                <div ng-show="Users.length!=0">
                    <%-- <label ng-show="Users[0].Totalrecords!=0">Count :  {{((page - 1) * pageSize) + 1}} to  {{(displayItems.length < 50)? (pageSize>2 ? Users[0].Totalrecords : displayItems.length): (((page - 1) * pageSize)+50)}} of {{Users[0].Totalrecords }}</label>--%>
                    <label ng-show="Users[0].Totalrecords!=0">Count :  {{((page - 1) * pageSize) + 1}} to  {{ displayItems.length< 50 ? (page>=2 ? Users[0].Totalrecords :displayItems[0].Totalrecords) : (((page - 1) * pageSize) + pageSize) }} of {{ Users[0].Totalrecords }}</label>
                </div>

                <div ng-show="ShowLoader" class="loader">
                    <img src="/App_Themes/HealthyChef/Images/Blocks-1s-200px.gif" alt="Loading..." />
                </div>
                <div class="table-responsive">
                    <table ng-show="Users.length!=0" class="table  table-bordered table-hover align_table" id="sample">
                        <thead>
                            <tr>
                                <th scope="col">&nbsp;</th>
                                <th scope="col" ng-click="orderUserTable('Name')" style="width: 150px">Customer Name</th>
                                <th scope="col" ng-click="orderUserTable('Email')">Email</th>
                                <th scope="col">Activated</th>
                                <th scope="col">Is Locked Out</th>
                                <th scope="col">Is Online</th>
                                <th scope="col">User Role</th>
                            </tr>
                        </thead>
                        <tbody>

                            <%--<tr ng-show="ShowLoader">
                            <td colspan="7" align="center">
                                <img src="/App_Themes/HealthyChef/Images/Blocks-1s-200px.gif" alt="Loading..." />
                            </td>
                        </tr>--%>

                            <tr style="background-color: #FEFEFE;" ng-repeat="u in displayItems ">


                                <td><a href="/WebModules/ShoppingCart/Admin/AccountManager.aspx?UserID={{u.UserID}}">Select</a></td>
                                <td><span>{{ u.Name }}</span></td>
                                <td>{{ u.Email }}</td>
                                <td ng-if="u.IsApproved">
                                    <i class="glyphicon glyphicon-ok"></i>
                                </td>
                                <td ng-if="!u.IsApproved">
                                    <i class="glyphicon glyphicon-remove"></i>
                                </td>
                                <td ng-if="u.IsLockedOut">
                                    <i class="glyphicon glyphicon-ok"></i>
                                </td>
                                <td ng-if="!u.IsLockedOut">
                                    <i class="glyphicon glyphicon-remove"></i>
                                </td>
                                <td ng-if="u.IsOnline">
                                    <i class="glyphicon glyphicon-ok"></i>
                                </td>
                                <td ng-if="!u.IsOnline">
                                    <i class="glyphicon glyphicon-remove"></i>
                                </td>
                                <td><span>{{ u.Role }}</span></td>
                            </tr>
                        </tbody>

                    </table>
                </div>
                <div ng-show="displayItems.length==0" class="aling_center">
                    <p>There are currently no accounts on record for this website.</p>
                </div>
                <ul ng-show="Users.length!=0" uib-pagination class="pagination-sm pagination" ng-model="page" total-items="Users[0].Totalrecords"
                    ng-click="pageChanged()" previous-text="&lsaquo;" next-text="&rsaquo;" items-per-page="pageSize" max-size="10" boundary-links="true">
                </ul>
                <div style="display: none;" class="aling_center">There are currently no accounts on record for this website.</div>

            </div>
        </asp:Panel>
    </div>
    <script type="text/javascript">
        var app = angular.module('UserApp', ["ui.bootstrap"]);

        //Getting data from API
        app.controller('GetUsers', function ($scope, $http) {

            $scope.Users = [];
            $scope.page = 1;
            $scope.pageSize = 50;
            $scope.GetDetails = function (index) {

                var name = $scope.Users[index].Name;
                var country = $scope.Users[index].Email;
                alert("Name: " + name + "\nEmail: " + country);
            };

            $scope.orderUserTable = function (x) {

                $scope.reverse = ($scope.OrderByField === x) ? !$scope.reverse : false;
                $scope.OrderByField = x;
            }

            $scope.ResetFilters = function () {

                $scope.NameFilter = '';
                $scope.EmailFilter = '';
                $scope.PhoneFilter = '';
                $scope.PurchaseNumberFilter = '';
                $scope.RoleFilter = $scope.RoleFilter;//'Customer';
                $scope.DeliveryDateFilter = '';
                $('#ctl00_Body_CompareValidator2').hide();
            }

            $scope.UpdateCustomerStatus = function () {
                var userID = $('#ctl00_Body_CurrentUserID').val();
                var isActive = $('#ctl00_Body_UserProfileEdit1_chkIsActive').prop('checked');
                var isLockedOut = $('#ctl00_Body_UserProfileEdit1_chkIsLockedOut').prop('checked');

                var data = { UserID: userID, IsActive: isActive, IsLockedOut: isLockedOut };

                $http({
                    method: "POST",
                    url: GetApiBaseURL() + "UpdateStatusOfCustomer",
                    data: data
                }).then(function mySuccess(response) {

                    if (response.data.IsSuccess) {
                        alert(response.data.Message);
                        location.reload();
                    }
                    else {
                        alert(response.data.Message);
                    }
                }, function myError(response) {
                    alert(response.data.Message);
                });
            }

            $scope.UpdateBasicInfo = function () {

                var userID = $('#ctl00_Body_CurrentUserID').val();
                var isActive = $('#ctl00_Body_UserProfileEdit1_chkIsActive').prop('checked');
                var isLockedOut = $('#ctl00_Body_UserProfileEdit1_chkIsLockedOut').prop('checked');

                var Email = $('#ctl00_Body_UserProfileEdit1_txtEmail').val();
                var FirstName = $('#ctl00_Body_UserProfileEdit1_txtFirstName').val();
                var LastName = $('#ctl00_Body_UserProfileEdit1_txtLastName').val();
                var DefaultCouponId = $('#ctl00_Body_UserProfileEdit1_ddlCoupons').val();
                var ProfileName = $('#ctl00_Body_UserProfileEdit1_txtProfileName').val();
                var CanyonRanchCustomer = $('#ctl00_Body_UserProfileEdit1_cbCanyonRanchCustomer').prop('checked');

                var Roles = [];

                var BasicInfodata = {
                    UserID: userID,
                    IsActive: isActive,
                    IsLockedOut: isLockedOut,
                    Email: Email,
                    FirstName: FirstName,
                    LastName: LastName,
                    ProfileName: ProfileName,
                    DefaultCouponId: DefaultCouponId,
                    CanyonRanchCustomer: CanyonRanchCustomer,
                    Roles: Roles
                };

                $http({
                    method: "POST",
                    url: GetApiBaseURL() + "UpdateBasicInfo",
                    data: BasicInfodata
                }).then(function mySuccess(response) {

                    if (response.data.IsSuccess) {
                        alert(response.data.Message);
                    }
                    else {
                        alert(response.data.Message);
                    }
                }, function myError(response) {
                    alert(response.data.Message);
                });
            }

            $scope.UpdateShippingAddress = function () {
                var userID = $('#ctl00_Body_CurrentUserID').val();
                var ShippingAddressID = $('#ctl00_Body_UserProfileEdit1_AddressEdit_Shipping1_hdnAddId').val();
                var FirstName = $('#ctl00_Body_UserProfileEdit1_AddressEdit_Shipping1_txtFirstName').val();
                var LastName = $('#ctl00_Body_UserProfileEdit1_AddressEdit_Shipping1_txtLastName').val();
                var Address1 = $('#ctl00_Body_UserProfileEdit1_AddressEdit_Shipping1_txtAddress1').val();
                var Address2 = $('#ctl00_Body_UserProfileEdit1_AddressEdit_Shipping1_txtAddress2').val();
                var City = $('#ctl00_Body_UserProfileEdit1_AddressEdit_Shipping1_txtCity').val();
                var State = $('#ctl00_Body_UserProfileEdit1_AddressEdit_Shipping1_ddlUSStates').val();
                var Phone = $('#ctl00_Body_UserProfileEdit1_AddressEdit_Shipping1_txtPhone').val();
                var PostalCode = $('#ctl00_Body_UserProfileEdit1_AddressEdit_Shipping1_txtZipCode').val();
                var DefaultShippingTypeID = $('#ctl00_Body_UserProfileEdit1_AddressEdit_Shipping1_ddlDeliveryTypes').val();
                var IsBusiness = $('#ctl00_Body_UserProfileEdit1_AddressEdit_Shipping1_chkIsBusiness').prop('checked');

                var ShippingAddress = {
                    UserID: userID,
                    ShippingAddressID: ShippingAddressID,
                    FirstName: FirstName,
                    LastName: LastName,
                    Address1: Address1,
                    Address2: Address2,
                    City: City,
                    State: State,
                    Phone: Phone,
                    PostalCode: PostalCode,
                    DefaultShippingTypeID: DefaultShippingTypeID,
                    IsBusiness: IsBusiness
                };

                $http({
                    method: "POST",
                    url: GetApiBaseURL() + "UpdateShippingAddress",
                    data: ShippingAddress
                }).then(function mySuccess(response) {
                    if (response.data.IsSuccess) {
                        setTimeout(function () {// wait for 5 secs(2)
                            location.reload(); // then reload the page.(3)
                        }, 1000);
                        // alert(response.data.Message);
                        var d = new Date();
                        //location.reload();
                        $("#ctl00_Body_UserProfileEdit1_AddressEdit_Shipping1_lblFeedback0").text("Shipping Address - information saved:  " + d.toLocaleString());

                    }
                    else {
                        alert(response.data.Message);
                    }
                }, function myError(response) {
                    alert(response.data.Message);
                });
            }

            $scope.UpdateBillingInfo = function () {

                var userID = $('#ctl00_Body_CurrentUserID').val();
                var BillingAddressID = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_AddressEdit_Billing1_hdnAddId').val();
                var FirstName = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_AddressEdit_Billing1_txtFirstName').val();
                var LastName = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_AddressEdit_Billing1_txtLastName').val();
                var Address1 = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_AddressEdit_Billing1_txtAddress1').val();
                var Address2 = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_AddressEdit_Billing1_txtAddress2').val();
                var City = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_AddressEdit_Billing1_txtCity').val();
                var State = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_AddressEdit_Billing1_ddlUSStates').val();
                var Phone = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_AddressEdit_Billing1_txtPhone').val();
                var PostalCode = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_AddressEdit_Billing1_txtZipCode').val();

                var UpdateCreditCardInfo = false;

                var NameOnCard = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_CreditCardEdit1_txtNameOnCard').val();
                var CardNumber = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_CreditCardEdit1_txtCCNumber').val();
                var ExipiresOnMonth = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_CreditCardEdit1_ddlExpMonth').val();
                var ExipiresOnYear = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_CreditCardEdit1_ddlExpYear').val();
                var CardIdCode = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_CreditCardEdit1_txtCCAuthCode').val();

                var BillingInfo = {
                    UserID: userID,
                    BillingAddressID: BillingAddressID,
                    FirstName: FirstName,
                    LastName: LastName,
                    Address1: Address1,
                    Address2: Address2,
                    City: City,
                    State: State,
                    Phone: Phone,
                    PostalCode: PostalCode,
                    UpdateCreditCardInfo: UpdateCreditCardInfo,
                    NameOnCard: NameOnCard,
                    CardNumber: CardNumber,
                    ExipiresOnMonth: ExipiresOnMonth,
                    ExipiresOnYear: ExipiresOnYear,
                    CardIdCode: CardIdCode
                };

                $http({
                    method: "POST",
                    url: GetApiBaseURL() + "UpdateBillingInfo",
                    data: BillingInfo
                }).then(function mySuccess(response) {

                    if (response.data.IsSuccess) {
                        //alert(response.data.Message);
                        setTimeout(function () {// wait for 5 secs(2)
                            location.reload(); // then reload the page.(3)
                        }, 1000);
                        var d = new Date();
                        $("#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_AddressEdit_Billing1_lblFeedback0").text("Billing - information saved:  " + d.toLocaleString());

                    }
                    else {
                        alert(response.data.Message);
                    }
                }, function myError(response) {
                    alert(response.data.Message);
                });
            }
            $scope.UpdateCreditCardInfo = function () {

                var userID = $('#ctl00_Body_CurrentUserID').val();
                var BillingAddressID = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_AddressEdit_Billing1_hdnAddId').val();
                var FirstName = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_AddressEdit_Billing1_txtFirstName').val();
                var LastName = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_AddressEdit_Billing1_txtLastName').val();
                var Address1 = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_AddressEdit_Billing1_txtAddress1').val();
                var Address2 = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_AddressEdit_Billing1_txtAddress2').val();
                var City = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_AddressEdit_Billing1_txtCity').val();
                var State = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_AddressEdit_Billing1_ddlUSStates').val();
                var Phone = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_AddressEdit_Billing1_txtPhone').val();
                var PostalCode = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_AddressEdit_Billing1_txtZipCode').val();

                var UpdateCreditCardInfo = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_chkUpdateCard').prop('checked');


                var NameOnCard = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_CreditCardEdit1_txtNameOnCard').val();
                var CardNumber = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_CreditCardEdit1_txtCCNumber').val();
                var ExipiresOnMonth = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_CreditCardEdit1_ddlExpMonth').val();
                var ExipiresOnYear = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_CreditCardEdit1_ddlExpYear').val();
                var CardIdCode = $('#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_CreditCardEdit1_txtCCAuthCode').val();

                var BillingInfo = {
                    UserID: userID,
                    BillingAddressID: BillingAddressID,
                    FirstName: FirstName,
                    LastName: LastName,
                    Address1: Address1,
                    Address2: Address2,
                    City: City,
                    State: State,
                    Phone: Phone,
                    PostalCode: PostalCode,
                    UpdateCreditCardInfo: UpdateCreditCardInfo,
                    NameOnCard: NameOnCard,
                    CardNumber: CardNumber,
                    ExipiresOnMonth: ExipiresOnMonth,
                    ExipiresOnYear: ExipiresOnYear,
                    CardIdCode: CardIdCode
                };

                $http({
                    method: "POST",
                    url: GetApiBaseURL() + "UpdateBillingInfo",
                    data: BillingInfo
                }).then(function mySuccess(response) {
                    if (response.data.IsSuccess) {
                        //alert(response.data.Message);
                        setTimeout(function () {// wait for 5 secs(2)
                            location.reload(); // then reload the page.(3)
                        }, 1000);
                        $("#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_chkUpdateCard").prop('checked', true);
                        var d = new Date();
                        $("#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_CreditCardEdit1_lblcreditcardfeedback").text("CreditCard - information saved:  " + d.toLocaleString());
                    }
                    else {
                        alert(response.data.Message);
                    }
                }, function myError(response) {

                    alert(response.data.Message);
                    setTimeout(function () {// wait for 5 secs(2)
                        location.reload(); // then reload the page.(3)
                    }, 1000);
                    var d = new Date();
                    $("#ctl00_Body_UserProfileEdit1_BillingInfoEdit1_CreditCardEdit1_lblcreditcardfeedback").text("CreditCard - information saved:  " + d.toLocaleString());
                });
            }

            $scope.AddOrUpdateNotesForUser = function () {
                var userID = $('#ctl00_Body_CurrentUserID').val();

                var UserNotesData = { UserID: userID, NoteId: NoteId, Note: Note, DisplayToUser: DisplayToUser, NotetypeId: NotetypeId };

                $http({
                    method: "POST",
                    url: GetApiBaseURL() + "AddOrUpdateNotesForUser",
                    data: UserNotesData
                }).then(function mySuccess(response) {

                    if (response.data.IsSuccess) {
                        alert(response.data.Message);
                    }
                    else {
                        alert(response.data.Message);
                    }
                }, function myError(response) {
                    alert(response.data.Message);
                });
            }


            $scope.SearchByUsers = function () {
                $scope.ShowLoader = true;
                if ($('#ctl00_Body_cpvPurchaseNum').css('display') == 'none') {
                    var currentloggeduserid = $("#ctl00_Body_CurrentLoggedUserID").val();
                    var rolename = '';
                    var lastName = $scope.NameFilter;
                    var email = $scope.EmailFilter;
                    var phone = $scope.PhoneFilter;
                    var purchNum = $scope.PurchaseNumberFilter;
                    var delivDate = $scope.DeliveryDateFilter;
                    $scope.page = 1;
                    $scope.pageSize = 50;
                    var pagenumber = $scope.page;
                    var pagesize = $scope.pageSize;
                    var totalrecord = 0;
                    var selectedRadioValue = $("input[type='radio'][name='RoleFilter']:checked");
                    if (selectedRadioValue.length > 0) {
                        roleid = selectedRadioValue[0].id;
                        rolename = selectedRadioValue[0].defaultValue;
                    }
                    if (rolename == 'any') {
                        rolename = '';
                    }
                    var searchparameters = { lastName: lastName === undefined ? '' : lastName, email: email === undefined ? '' : email, phone: phone === undefined ? '' : phone, purchaseNumber: purchNum === undefined ? '' : purchNum, deliveryDate: delivDate === undefined ? '' : delivDate, roles: rolename === undefined ? '' : rolename, pagenumber: pagenumber, pagesize: pagesize, totalrecords: totalrecord, CurrentLoggedUserId: currentloggeduserid };

                    $http({
                        method: "POST",
                        url: GetApiBaseURL() + "SearchGetUserAccounts",
                        data: searchparameters
                    }).then(function mySuccess(response) {
                        $scope.Users = response.data;
                        $scope.displayItems = $scope.Users.slice(0, $scope.pageSize);
                        $scope.ShowLoader = false;
                        console.log($scope.totalRecords);
                    }, function myError(response) {
                        alert(response.data.Message);
                        $scope.ShowLoader = false;
                    });
                }
                else {

                    $scope.ShowLoader = false;
                    $scope.Users = [];
                    $scope.displayItems = [];
                }

            }

            $scope.pageChanged = function () {

                $scope.ShowLoader = true;
                if ($('#ctl00_Body_cpvPurchaseNum').css('display') == 'none') {
                    var rolename = '';
                    var currentloggeduserid = $("#ctl00_Body_CurrentLoggedUserID").val();
                    var lastName = $scope.NameFilter;
                    var email = $scope.EmailFilter;
                    var phone = $scope.PhoneFilter;
                    var purchNum = $scope.PurchaseNumberFilter;
                    var delivDate = $scope.DeliveryDateFilter;
                    var pagenumber = $scope.page;
                    var pagesize = 50;
                    var totalrecord = 0;
                    var selectedRadioValue = $("input[type='radio'][name='RoleFilter']:checked");
                    if (selectedRadioValue.length > 0) {
                        roleid = selectedRadioValue[0].id;
                        rolename = selectedRadioValue[0].defaultValue;
                    }
                    if (rolename == 'any') {
                        rolename = '';
                    }
                    var searchparameters = { lastName: lastName === undefined ? '' : lastName, email: email === undefined ? '' : email, phone: phone === undefined ? '' : phone, purchaseNumber: purchNum === undefined ? '' : purchNum, deliveryDate: delivDate === undefined ? '' : delivDate, roles: rolename === undefined ? '' : rolename, pagenumber: pagenumber, pagesize: pagesize, totalrecords: totalrecord, CurrentLoggedUserId: currentloggeduserid };

                    $http({
                        method: "POST",
                        url: GetApiBaseURL() + "SearchGetUserAccounts",
                        data: searchparameters
                    }).then(function mySuccess(response) {
                        $scope.Users = response.data;
                        if ($scope.Users.length > 51) {
                            $scope.displayItems = $scope.Users.slice(($scope.page - 1) * ($scope.pageSize), ($scope.pageSize) * ($scope.page));
                        }
                        else {
                            $scope.displayItems = $scope.Users.slice(0, $scope.pageSize);
                        }
                        $scope.ShowLoader = false;
                        console.log($scope.totalRecords);
                    }, function myError(response) {
                        alert(response.data.Message);
                        $scope.ShowLoader = false;
                    });
                }
                else {
                    $scope.ShowLoader = false;
                    $scope.Users = [];
                    $scope.displayItems = [];
                }
            };



        });


    </script>
    <script type="text/javascript">
        $(function () {
            $('input:radio[name="RoleFilter"]').filter('[value="Customer"]').attr('checked', true);
            ToggleMenus('useraccounts', undefined, undefined);
            preventResfreshOnEnter();
        });


        function isNumeric(evt) {
            var isValidNumeric = true;
            var isValidSpecialChar = true;
            var isAlpha = true;
            var specialKeys = new Array();
            //specialKeys.push(96);  // `
            //specialKeys.push(126); // ~
            //specialKeys.push(33);  // !
            //specialKeys.push(64);  // @
            //specialKeys.push(35);  // #
            //specialKeys.push(36);  // $
            //specialKeys.push(37);  // %
            //specialKeys.push(94);  // ^
            //specialKeys.push(42);  // *
            specialKeys.push(40);  // (
            specialKeys.push(41);  // )
            specialKeys.push(95);  // _
            //specialKeys.push(61);  // =
            specialKeys.push(43);  // +
            //specialKeys.push(91);  // [
            //specialKeys.push(123); // {
            //specialKeys.push(93);  // ]
            //specialKeys.push(125); // }
            //specialKeys.push(59);  // ;
            //specialKeys.push(58);  // :
            //specialKeys.push(39);  // '
            //specialKeys.push(34);  // "
            //specialKeys.push(44);  // ,
            //specialKeys.push(60);  // <
            specialKeys.push(46);  // .
            //specialKeys.push(62);  // >
            //specialKeys.push(47);  // /
            //specialKeys.push(63);  // ?
            //specialKeys.push(92);  // \
            //specialKeys.push(124); // |
            specialKeys.push(32); // (space)
            specialKeys.push(45);//-


            var charCode = (evt.which) ? evt.which : event.keyCode;

            if ((charCode < 65 || charCode > 90) && (charCode < 97 || charCode > 123))// && charCode != 32)
            {
                isAlpha = false;
            }
            if ((charCode == 46) || (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))) {
                isValidNumeric = false;
            }
            var ret = (specialKeys.indexOf(evt.keyCode) == -1)//&& evt.charCode == evt.keyCode);
            if (ret == true) {
                isValidSpecialChar = false;
            }
            if (isAlpha == false) {
                if (isValidNumeric == true || isValidSpecialChar == true) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }
        }

        function isNumeric1(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if ((charCode == 46) || (charCode != 46 && charCode > 31
                && (charCode < 48 || charCode > 57)))
                return false;

            return true;
        }

        function Validate(event) {
            var regex = new RegExp("^[0-9-!@#$%*?]");
            var key = String.fromCharCode(event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        }




    </script>
    <style>
        .pagination > li > a:hover, .pagination > li > a:active, .pagination > li > a {
            background-color: #F9F9F9;
            border-color: #D9D9D9;
            z-index: auto;
            color: #777;
        }

        .loader {
            /*position: absolute;
            top: 20%;
            left: 40%;
            opacity: 0.8;
            z-index: 99;*/
            position: fixed;
            top: 0;
            left: 50%;
            opacity: 0.8;
            z-index: 99;
        }

            .loader img {
                position: absolute;
                top: 150px;
                left: 0;
            }
        /*table { table-layout: fixed; }
 table th, table td { overflow: hidden; }*/
        /*.pagination {
                margin-top: 16%;
        }*/
    </style>
</asp:Content>
