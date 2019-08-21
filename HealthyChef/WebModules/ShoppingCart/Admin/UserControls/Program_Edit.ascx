<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Program_Edit.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.Program_Edit" %>

<%@ Register TagPrefix="hcc" TagName="PagePicker" Src="~/WebModules/Components/PagePicker/PagePicker.ascx" %>
<%@ Register TagPrefix="hcc" TagName="ImagePicker" Src="~/WebModules/Components/ImagePicker/ImagePicker.ascx" %>

<div class="fieldRow col-sm-12">
    <div class="fieldCol">
        <asp:Button ID="btnSave" CssClass="btn btn-info" runat="server" Text="Save" />
        <%--OnClick="SubmitButtonClick"--%>
        <%--<asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger" Text="Cancel" CausesValidation="false" />--%>
        <input type="button" id="btnCancel" onclick="CancelProgramDetails()" value="Cancel"   class="btn btn-danger" />
        <asp:Button ID="btnDeactivate" runat="server" CssClass="btn btn-info" Text="Deactivate" CausesValidation="false" />
        <div>
            <asp:ValidationSummary ID="ValSumPrograms1" runat="server" />
            <asp:CustomValidator ID="cstValPlanPriceDefault" runat="server" EnableClientScript="false"
                Display="None" ErrorMessage="A single plan price adjustment range value must to be selected as the Default."
                SetFocusOnError="true" OnServerValidate="cstValPlanPriceDefault_ServerValidate" />
        </div>
    </div>
</div>
<div class="p-5">
    <div class="fieldRow col-sm-4">
        <div class="fieldCol">
            Program Name:<br />
        <asp:TextBox ID="txtProgramName" runat="server"  /><%--MaxLength="20"--%>
            <asp:RequiredFieldValidator ID="rfvProgramName" runat="server" ControlToValidate="txtProgramName"
                Text="*" Display="Dynamic" ErrorMessage="A Program name is required." SetFocusOnError="true" />
            <asp:CustomValidator ID="cstProgramName" runat="server" ControlToValidate="txtProgramName"
                EnableClientScript="false" Text="*" Display="Dynamic" ErrorMessage="There is already a program that uses the name entered."
                SetFocusOnError="true" OnServerValidate="cstName_ServerValidate" />
        </div>
    </div>
    <div class="fieldRow col-sm-4">
        <div class="fieldCol" style="vertical-align: top;">
            Description:<br />
        <asp:TextBox ID="txtProgramDesc" runat="server" TextMode="MultiLine" Columns="20" Rows="5" />
            <asp:RequiredFieldValidator ID="rfvProgramDesc" runat="server" ControlToValidate="txtProgramDesc"
                Text="*" Display="Dynamic" ErrorMessage="A Program description is required."
                SetFocusOnError="true" />
        </div>
    </div>
    <div class="fieldRow col-sm-4">
        <div class="fieldCol">
            More Info Page:<br />
        <hcc:PagePicker ID="PagePicker1" class="col-sm-12" runat="server" />
        </div>
    </div>
    
</div>
<div class="fieldRow col-sm-12">
        <div class="fieldCol">
            Program Image:<br />
        <hcc:ImagePicker ID="ImagePicker1" runat="server" />
        </div>
    </div>
<div class="fieldRow col-sm-3">
    <div class="fieldCol">
        Display on Website:
        <asp:CheckBox ID="chkDisplayOnWebsite" runat="server" />
    </div>
</div>
<div class="clearfix"></div>
<div class="fieldRow desc DESC1">
    <div class="fieldCol">
        <fieldset>
            <legend>Required Meal Types Per Day</legend>
            <asp:GridView ID="gvwMealTypes" CssClass="table table-hover table-bordered" runat="server" AutoGenerateColumns="false" DataKeyNames="MealTypeID">
                <Columns>
                    <asp:BoundField DataField="MealTypeName" HeaderText="Meal Type" ItemStyle-Width="150px" />
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <asp:TextBox ID="txtReqQuantity" runat="server" Text='<%# Eval("RequiredQuantity") %>' Width="50px" />
                            <asp:CompareValidator ID="cprReqQuan" runat="server" ControlToValidate="txtReqQuantity"
                                Text="*" Display="Dynamic" ErrorMessage="Required Quantity must be a whole number."
                                SetFocusOnError="true" Operator="DataTypeCheck" Type="Integer" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle HorizontalAlign="Left" />
            </asp:GridView>
        </fieldset>
    </div>
    <br />
    <div class="fieldCol desc">
        <fieldset>
            <legend>Plan Price Adjustments (Per Day)</legend>
            <table class="table table-bordered table-hover">
                <tr>
                    <th>Name
                    </th>
                    <th>Amount
                    </th>
                    <th>Default</th>
                </tr>
                <tr>
                    <td>Range 1:
                        <asp:TextBox ID="txtCalRange1Text" runat="server" AutoPostBack="false" Width="250px" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtCalRange1Value" runat="server" Width="60px" />
                        <asp:RequiredFieldValidator ID="rfvCalRange1Value" runat="server" ControlToValidate="txtCalRange1Value"
                            Text="*" Display="Dynamic" ErrorMessage="Range 1 Per Day Price Adjustment must be entered if Range 1 Adj. Name has been entered."
                            SetFocusOnError="true" Enabled="false" />
                        <asp:CompareValidator ID="cprCalRange1Value" runat="server" ControlToValidate="txtCalRange1Value"
                            Text="*" Display="Dynamic" ErrorMessage="Range 1 Per Day Price Adjustment must be numeric"
                            SetFocusOnError="true" Operator="DataTypeCheck" Type="Double" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsDefault1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Range 2:
                        <asp:TextBox ID="txtCalRange2Text" runat="server" AutoPostBack="false" Width="250px" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtCalRange2Value" runat="server" Width="60px" />
                        <asp:RequiredFieldValidator ID="rfvCalRange2Value" runat="server" ControlToValidate="txtCalRange2Value"
                            Text="*" Display="Dynamic" ErrorMessage="Range 2 Per Day Price Adjustment must be entered if Range 2 Adj. Name has been entered."
                            SetFocusOnError="true" Enabled="false" />
                        <asp:CompareValidator ID="cprCalRange2Value" runat="server" ControlToValidate="txtCalRange2Value"
                            Text="*" Display="Dynamic" ErrorMessage="Range 2 Per Day Price Adjustment must be numeric"
                            SetFocusOnError="true" Operator="DataTypeCheck" Type="Double" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsDefault2" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Range 3:
                        <asp:TextBox ID="txtCalRange3Text" runat="server" AutoPostBack="false" Width="250px" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtCalRange3Value" runat="server" Width="60px" />
                        <asp:RequiredFieldValidator ID="rfvCalRange3Value" runat="server" ControlToValidate="txtCalRange3Value"
                            Text="*" Display="Dynamic" ErrorMessage="Range 3 Per Day Price Adjustment must be entered if Range 3 Adj. Name has been entered."
                            SetFocusOnError="true" Enabled="false" />
                        <asp:CompareValidator ID="cprCalRange3Value" runat="server" ControlToValidate="txtCalRange3Value"
                            Text="*" Display="Dynamic" ErrorMessage="Range 3 Per Day Price Adjustment must be numeric"
                            SetFocusOnError="true" Operator="DataTypeCheck" Type="Double" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsDefault3" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Range 4:
                        <asp:TextBox ID="txtCalRange4Text" runat="server" AutoPostBack="false" Width="250px" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtCalRange4Value" runat="server" Width="60px" />
                        <asp:RequiredFieldValidator ID="rfvCalRange4Value" runat="server" ControlToValidate="txtCalRange4Value"
                            Text="*" Display="Dynamic" ErrorMessage="Range 4 Per Day Price Adjustment must be entered if Range 4 Adj. Name has been entered."
                            SetFocusOnError="true" Enabled="false" />
                        <asp:CompareValidator ID="cprCalRange4Value" runat="server" ControlToValidate="txtCalRange4Value"
                            Text="*" Display="Dynamic" ErrorMessage="Range 4 Per Day Price Adjustment must be numeric"
                            SetFocusOnError="true" Operator="DataTypeCheck" Type="Double" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsDefault4" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Range 5:
                        <asp:TextBox ID="txtCalRange5Text" runat="server" AutoPostBack="false" Width="250px" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtCalRange5Value" runat="server" Width="60px" />
                        <asp:RequiredFieldValidator ID="rfvCalRange5Value" runat="server" ControlToValidate="txtCalRange5Value"
                            Text="*" Display="Dynamic" ErrorMessage="Range 5 Per Day Price Adjustment must be entered if Range 5 Adj. Name has been entered."
                            SetFocusOnError="true" Enabled="false" />
                        <asp:CompareValidator ID="cprCalRange5Value" runat="server" ControlToValidate="txtCalRange5Value"
                            Text="*" Display="Dynamic" ErrorMessage="Range 5 Per Day Price Adjustment must be numeric"
                            SetFocusOnError="true" Operator="DataTypeCheck" Type="Double" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsDefault5" runat="server" />
                    </td>
                </tr>
            </table>
        </fieldset>
        <div class="clearfix"><br /></div>
        <div class="row-fluid desc">
            <fieldset>
                <legend>Member Plans</legend>
                <asp:GridView ID="gvwPlans" runat="server" CssClass="table table-bordered table-hover" AutoGenerateColumns="false" Width="100%">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="280px" />
                        <asp:BoundField DataField="NumWeeks" HeaderText="Weeks" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="NumDaysPerWeek" HeaderText="Days" ItemStyle-Width="60px"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="IsDefault" HeaderText="Default" />
                        <asp:HyperLinkField Text="Edit" DataNavigateUrlFormatString="~/WebModules/ShoppingCart/Admin/PlanManager.aspx?pid={0}"
                            DataNavigateUrlFields="PlanID" ItemStyle-HorizontalAlign="Right" />
                    </Columns>
                    <EmptyDataTemplate>
                       <%-- There are currently no plans assigned to this program.--%>
                    </EmptyDataTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:GridView>
            </fieldset>
        </div>
    </div>
</div>
<style>
     .desc legend {
        margin:0px;
        text-align:left;
        font-size:16px;
    }
    .desc fieldset {
        border-radius:0px;
        margin-bottom:0px;
    }
    .desc .fieldCol {
        width: auto !important;
    }
    .desc .fieldRow {
        padding:0px 15px;
    }
    .DESC1 {
         padding:0px 15px;
    }
</style>
<script>
    function CancelProgramDetails()
    {
        window.location.href = "/WebModules/ShoppingCart/Admin/ProgramManager.aspx";
    }
</script>
