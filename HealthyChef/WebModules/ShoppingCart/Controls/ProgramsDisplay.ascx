<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProgramsDisplay.ascx.cs" Inherits="HealthyChef.WebModules.ShoppingCart.ProgramsDisplay" %>

<link href="/App_Themes/jquery/popover.css" rel="stylesheet" />
<script src="../../../client/jquery-1.9.1.min.js"></script>
<%--<script src="/client/jquery.popover-1.1.2.js"></script>--%>
<script src="../../../client/jquery.popover-1.1.2.js"></script>

<style type="text/css">
    fieldset {
        margin: 0;
        padding: 1em;
        border: none;
    }

    .popoverClass {
        height: 35px;
        width: 450px;
    }
</style>

<div>&nbsp;</div>

<asp:MultiView ID="multi_programs" runat="server" ActiveViewIndex="0">
    <asp:View ID="view_list" runat="server">
        <%-- Loop thru the available Healthy Chef Programs --%>
        <asp:Repeater ID="rptMenuPrograms" runat="server">
            <ItemTemplate>
                <div class="section">
                    <div class="section-top">&nbsp</div>

                    <div class="section-middle">
                        <%--<%# GetProgramImage(Eval("Name").ToString()) %>--%>
                        <img width='150' height='126' alt='<%# Eval("Name")%> image' src='<%# Eval("ImagePath") %>' class='left' />
                        <div class="description">
                            <h3><%# Eval("Name")%></h3>
                            <div style="margin: 0.5em 0; color: #333; font-size: 13px; line-height: 18px;"><%# Eval("Description")%></div>
                        </div>
                        <div class="cl">&nbsp;</div>
                        <div class="price">
                            <p>
                                As low as<br />
                                <span>
                                    <asp:Label ID="lblPricePerDay" runat="server" /></span> per day.
                            </p>

                            <asp:HyperLink ID="lnkSelect"
                                NavigateUrl='<%# Regex.Replace((DataBinder.Eval(Container.DataItem, "Name", "/details/{0}")), @"[^\w\d/]", "-") %>'
                                Text="Select" CssClass="page-button page-button-arrow"
                                Style="margin-bottom: 4px;" runat="server" />

                            <%--<asp:LinkButton ID="lnkSelect" runat="server" Text="Select"  
                            CssClass="page-button page-button-arrow" Style="margin-bottom: 4px;" />
                         OnClick="lnkSelectClick" CommandArgument='<%# Eval("ProgramId")%>'--%>

                            <asp:HyperLink ID="lnkMoreInfo" runat="server"
                                Text="More Info" CssClass="page-button page-button-arrow" />

                        </div>
                    </div>
                    <div class="section-bottom">&nbsp</div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </asp:View>

    <asp:View ID="view_details" runat="server">
        <div class="full-page-panel">
            <div class="top">
                &nbsp;
            </div>
            <div class="middle health-chief-diet">
                <h2><%= _ProgramName %></h2>
                <fieldset style="float: left; width: 40%;">
                    <asp:Repeater ID="rpet_plan_types" runat="server">
                        <HeaderTemplate>
                            <legend>Plan Type</legend>
                            <table id="plan-types" width="100%" cellpadding="0" cellspacing="0" class="form-select-menu" border="0">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="left-box" valign="top">
                                    <input type="radio" name="plan_type"
                                        id="plan-type-<%# DataBinder.Eval(Container.DataItem, "PlanID") %>"
                                        data-id='<%# DataBinder.Eval(Container.DataItem, "PlanID") %>'
                                        data-price='<%# DataBinder.Eval(Container.DataItem, "PricePerDay") %>'
                                        data-weeks='<%# DataBinder.Eval(Container.DataItem, "NumWeeks") %>'
                                        data-days-per-week='<%# DataBinder.Eval(Container.DataItem, "NumDaysPerWeek") %>'
                                        data-meals-per-day='<%# DataBinder.Eval(Container.DataItem, "MealsPerDay") %>'
                                        data-default='<%# bool.Parse(DataBinder.Eval(Container.DataItem, "IsDefault").ToString()) ? "true" : "false" %>'
                                        value='<%# DataBinder.Eval(Container.DataItem, "PlanID") %>' />
                                </td>
                                <td class="middle-box" valign="top">
                                    <label for="plan-type-<%# DataBinder.Eval(Container.DataItem, "PlanID") %>">
                                        <%# DataBinder.Eval(Container.DataItem, "Name") %>
                                    </label>
                                </td>
                                <td class="right-box" align="right" valign="top"><span><%# string.Format("{0:c}", DataBinder.Eval(Container.DataItem, "PricePerDay")) %></span> / day</td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                    <asp:RadioButtonList ID="rdo_plan_type" CssClass="radio-button-table plan-types" Visible="false" runat="server" />
                </fieldset>

                <fieldset style="float: left; width: 25%;">
                    <asp:Repeater ID="rpet_plan_options" runat="server">
                        <HeaderTemplate>
                            <legend id="legend-options">Plan Option</legend>
                            <table id="plan-options" width="100%" cellpadding="0" cellspacing="0" class="form-select-menu" border="0">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="left-box" valign="top">
                                    <input type="radio" name="plan_option"
                                        id="plan-option-<%# DataBinder.Eval(Container.DataItem, "ProgramOptionID") %>"
                                        data-id='<%# DataBinder.Eval(Container.DataItem, "ProgramOptionID") %>'
                                        data-price='<%# DataBinder.Eval(Container.DataItem, "OptionValue") %>'
                                        data-default='<%# bool.Parse(DataBinder.Eval(Container.DataItem, "IsDefault").ToString()) ? "true" : "false" %>'
                                        value='<%# DataBinder.Eval(Container.DataItem, "ProgramOptionID") %>' />
                                </td>
                                <td class="middle-box" valign="top">
                                    <label for="plan-option-<%# DataBinder.Eval(Container.DataItem, "ProgramOptionID") %>">
                                        <%# DataBinder.Eval(Container.DataItem, "OptionText") %>
                                    </label>
                                </td>
                                <td class="right-box" align="right" valign="top"><span>
                                    <asp:Label ID="lblOptionPrice" runat="server" />
                                </span>/ day</td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                    <asp:RadioButtonList ID="rdo_plan_option" CssClass="radio-button-table plan-options" runat="server" />
                    <input type="hidden" id="program-id" value="<%= _ProgramId %>" />
                </fieldset>
                <fieldset style="float: right; width: 20%;">

                    <div class="form-control">
                        <legend>Start Date</legend>
                        <asp:DropDownList ID="ddl_delivery_date" DataTextFormatString="{0:MMMM d, yyyy (ddd)}" CssClass="date-picker" runat="server" />
                    </div>
                    <div id="divProfiles" runat="server" visible="false">
                        <legend>Profile</legend>
                        <asp:DropDownList ID="ddlProfiles" runat="server" />
                    </div>
                    <%--<legend>Renew Billing?</legend>
                    <asp:CheckBox ID="chx_renew" Text="Yes, automatically renew my plan when it expires" CssClass="auto-renew" runat="server" />--%>
                    <div class="form-control">
                        <legend>SubTotal</legend>
                        <div align="center" style="font-size: 1.2em; font-weight: bold;">
                            Your selected plan price is
                           <div id="program-subtotal">0.00</div>
                        </div>

                        <p id="days-in-week-label" align="center">for a total of <b>X</b> meals, plus extras.</p>
                    </div>
                    <div class="form-control">

                        <legend>Quantity</legend>
                        <asp:TextBox ID="txt_quantity" MaxLength="2" Text="1" Style="margin-right: 1em; text-align: center; width: 20px; float: left;" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvQty" runat="server" ControlToValidate="txt_quantity" Display="Dynamic"
                            ErrorMessage="Quantity is required" Text="*" SetFocusOnError="true" ValidationGroup="AddToCartGroup" />
                        <asp:CompareValidator ID="cprQty" runat="server" ControlToValidate="txt_quantity" Display="Dynamic"
                            ErrorMessage="Quantity must be a number" Text="*" SetFocusOnError="true" ValidationGroup="AddToCartGroup"
                            Type="Integer" Operator="DataTypeCheck" />
                        <asp:Button ID="btn_add_to_cart" OnClick="btn_add_to_cart_Click" Text="Add to Cart" CssClass="add-cart page-button page-button-plus" Style="float: left;" runat="server" />

                        <asp:HyperLink ID="hlAutoRenewLogin" CssClass="autoRenewLogin" Visible="False" runat="server" Text="Login or create an account to sign up for Auto-Renew"></asp:HyperLink>
                        <div id="divRecurring" runat="server">
                            Check here to Sign up for Auto-Renew
                            <asp:CheckBox runat="server" ID="cbxRecurring" ClientIDMode="Static" Text="" TextAlign="Left" Visible="True" /><br />
                            <a id="aRecurringDescription" style="font-weight: bold;">(Click to Learn More)</a>
                        </div>

                        <asp:ValidationSummary ID="ValSumProg" runat="server" ValidationGroup="AddToCartGroup" />
                        <p style="clear: both; text-align: center;">
                            <br />
                            <%--<asp:LinkButton ID="lkbCancel" runat="server" Text="View More Programs" OnClick="lkbCancel_Click" CssClass="add-cart-see-more" />--%>
                        </p>
                    </div>
                </fieldset>
                <div class="cl">&nbsp;</div>
            </div>
            <div class="bottom">&nbsp;</div>
        </div>

        <script type="text/javascript">
            $(document).ready(function () {
                //Select first radio button for Plan Types
                //$(':radio:first', '.form-select-menu').attr('checked', 'checked');

                //Alternate row colors and remove border from last List Item
                $('.form-select-menu').each(function () {
                    $('tr:odd', $(this)).addClass('bg-grey-light');
                    $('tr:last td', $(this)).css('border', 'none');
                });

                //Do green background highlighting on radio click
                $(':radio', '.form-select-menu').click(function () {
                    $('tr', $(this).closest('.form-select-menu')).removeClass('bg-lime-light');
                    $(this).closest('tr').addClass('bg-lime-light');
                    calcPrice();
                });

                $('input[type=radio][data-default$="true"]').each(function () {
                    $(this).attr('checked', 'checked').closest('tr').addClass('bg-lime-light');
                });
                //    $('tr', $(this).closest('.form-select-menu')).removeClass('bg-lime-light');
                //    $(this).closest('tr').addClass('bg-lime-light');      

                //Do green background highlighting on load
                //$('input[type=radio]:checked', '.form-select-menu').each(function () {
                //    $('tr', $(this).closest('.form-select-menu')).removeClass('bg-lime-light');
                //    $(this).closest('tr').addClass('bg-lime-light');                    

                //});

                calcPrice();

                function calcPrice() {
                     
                    var variance = parseFloat($('input[type=radio]:checked', 'table#plan-options').attr('data-price'));
                    if (isNaN(variance)) { variance = 0; }

                    //Determine checked plan-type element
                    var planId = parseFloat($('input[type=radio]:checked', 'table#plan-types').val());
                    var planRadio = $('#plan-type-' + planId);

                    var pricePerDay = parseFloat($(planRadio).attr('data-price'));
                    var weeks = parseFloat($(planRadio).attr('data-weeks'));
                    var weeksDays = parseFloat($(planRadio).attr('data-days-per-week'));
                    var days = parseFloat(weeks * weeksDays);
                    var mealsPerDay = parseInt($(planRadio).attr('data-meals-per-day'));
                    var totalMeals = days * mealsPerDay;
                    //alert('variance: ' + variance + '\nplanId: ' + planId + '\npricePerDay: ' + pricePerDay + '\nweeks: ' + weeks + '\nweeksDays: ' + weeksDays + '\ndays: ' + days);
                    var subtotal = (pricePerDay + variance) * days;
                    var subtotalmealprice = '$' + subtotal.toFixed(2);
                    $('#program-subtotal').text(subtotalmealprice);
                    //$('#program-subtotal').formatCurrency();
                    $('b', '#days-in-week-label').text(totalMeals);
                }

            });

            $('#aRecurringDescription').popover({
                url: '/recurring-description.aspx',
                classes: 'popoverClass',
                trigger: 'none'
            });

            $('#aRecurringDescription').click(function (event) {
                 
                event.preventDefault();
                event.stopPropagation();
                $('#aRecurringDescription').popover('show');
            });

        </script>
    </asp:View>
</asp:MultiView>
