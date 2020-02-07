<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuItemsByDate.ascx.cs" Inherits="HealthyChef.WebModules.ShoppingCart.MenuItemsByDate" %>
<%@ Register TagPrefix="hcc" TagName="MealTabRepeaterControl" Src="~/WebModules/ShoppingCart/Controls/MealTabRepeater.ascx" %>



<style type="text/css">
    .sticky
    {
        position: fixed;
        top: 0;
        margin-left: 8px;
    }

    #cart-meals
    {
        width: 200px;
        height: 600px;
        overflow-y: scroll;
    }

        #cart-meals ul
        {
            margin: 0;
            padding: 0;
        }

        #cart-meals ul li
        {
            margin: 0;
            padding: 0.6em;
            border-bottom: dashed 1px #999;
            display: block;
        }
</style>

<asp:HiddenField ID="hdnLastMealTab" runat="server" ClientIDMode="Static" Value="Breakfast" />
<!-- Contents Container -->
<div style="position: relative; padding: 1em;">

    <!-- Date Picker -->
    <fieldset class="css-order">
        <ul>
            <li>
                <div class="font-myriad-bold-cond" style="font-size: 2em; float: left;">Select a Delivery Date:&nbsp;</div>
                <div style="float: left; margin-top: 4px;">
                    <asp:DropDownList ID="ddlDeliveryDate" DataTextFormatString="{0:MMMM d, yyyy (ddd)}" AutoPostBack="true"
                        CssClass="date-picker" runat="server" />
                </div>
            </li>
        </ul>
    </fieldset>
    <!-- Meal Items By Selected Date -->
    <div class="left" style="width: 100%;">
        <div class="meals">
            <h2>&nbsp;</h2>
            <div class="m-nav" style="text-align: center;">
                <ul>
                    <li>
                        <%--<asp:LinkButton ID="lbBreakfast" runat="server" ClientIDMode="Static" OnClick="lbBreakfastClick">Breakfast<span>&nbsp;</span></asp:LinkButton>--%>
                        <asp:Button ID="btnBreakfast" runat="server" Text="Breakfast" UseSubmitBehavior="True" OnClick="lbBreakfastClick" ClientIDMode="Static" CssClass="alc-button-submit" />
                        <%--<asp:HyperLink ID="hypBreakfast" runat="server" ClientIDMode="Static"></asp:HyperLink>--%>
                    </li>
                    <li>
                        <%--<asp:LinkButton ID="lbLunch" runat="server" ClientIDMode="Static" OnClick="lbLunchClick" OnClientClick="return true;" CausesValidation="False">Lunch<span>&nbsp;</span></asp:LinkButton>--%>
                        <asp:Button ID="btnLunch" runat="server" Text="Lunch" UseSubmitBehavior="True" OnClick="lbLunchClick" ClientIDMode="Static" CssClass="alc-button-submit" />
                        <%--<asp:HyperLink ID="hypLunch" runat="server" ClientIDMode="Static" NavigateUrl="~/browse-menu.aspx">Lunch<span>&nbsp;</span></asp:HyperLink>--%>
                    </li>
                    <li>
                        <%--<asp:LinkButton ID="lbDinner" runat="server" ClientIDMode="Static" OnClick="lbDinnerClick">Dinner<span>&nbsp;</span></asp:LinkButton>--%>
                        <asp:Button ID="btnDinner" runat="server" Text="Dinner" UseSubmitBehavior="True" OnClick="lbDinnerClick" ClientIDMode="Static" CssClass="alc-button-submit" />
                        <%--<asp:HyperLink ID="hypDinner" runat="server" ClientIDMode="Static">Dinner<span>&nbsp;</span></asp:HyperLink>--%>
                    </li>
                    <li>
                        <%--<asp:LinkButton ID="lbChild" runat="server" ClientIDMode="Static" OnClick="lbChildClick">Child<span>&nbsp;</span></asp:LinkButton>--%>
                        <asp:Button ID="btnChild" runat="server" Text="Child" UseSubmitBehavior="True" OnClick="lbChildClick" ClientIDMode="Static" CssClass="alc-button-submit" />
                        <%--<asp:HyperLink ID="hypChild" runat="server" ClientIDMode="Static">Child<span>&nbsp;</span></asp:HyperLink>--%>
                    </li>
                    <li>
                        <%--<asp:LinkButton ID="lbDessert" runat="server" ClientIDMode="Static" OnClick="lbDessertClick">Dessert<span>&nbsp;</span></asp:LinkButton>--%>
                        <asp:Button ID="btnDessert" runat="server" Text="Dessert" UseSubmitBehavior="True" OnClick="lbDessertClick" ClientIDMode="Static" CssClass="alc-button-submit" />
                        <%--<asp:HyperLink ID="hypDessert" runat="server" ClientIDMode="Static">Dessert<span>&nbsp;</span></asp:HyperLink>--%>
                    </li>
                    <li>
                        <%--<asp:LinkButton ID="lbOther" runat="server" ClientIDMode="Static" OnClick="lbOtherClick">Other<span>&nbsp;</span></asp:LinkButton>--%>
                        <asp:Button ID="btnOther" runat="server" Text="Other" UseSubmitBehavior="True" OnClick="lbOtherClick" ClientIDMode="Static" CssClass="alc-button-submit" />
                        <%--<asp:HyperLink ID="hypOther" runat="server" ClientIDMode="Static">Other<span>&nbsp;</span></asp:HyperLink>--%>
                    </li>

<%--                    <li><a href="#">Breakfast<span>&nbsp;</span></a></li>
                    <li><a href="#">Lunch<span>&nbsp;</span></a></li>
                    <li><a href="#">Dinner<span>&nbsp;</span></a></li>
                    <li><a href="#">Child<span>&nbsp;</span></a></li>
                    <li><a href="#">Dessert<span>&nbsp;</span></a></li>
                    <li><a href="#">Other<span>&nbsp;</span></a></li>--%>
                </ul>
            </div>
            <hcc:MealTabRepeaterControl runat="server" ID="MealTabRepeater" />
            <!-- Breakfast -->
            <%--<hcc:MealTabRepeaterControl runat="server" ID="MealTabRepeaterBreakfast" />
            <!-- Lunch --->
            <hcc:MealTabRepeaterControl runat="server" ID="MealTabRepeaterLunch" />
            <!-- Dinner -->
            <hcc:MealTabRepeaterControl runat="server" ID="MealTabRepeaterDinner" />
            <!-- Child -->
            <hcc:MealTabRepeaterControl runat="server" ID="MealTabRepeaterChild" />
            <!-- Dessert -->
            <hcc:MealTabRepeaterControl runat="server" ID="MealTabRepeaterDessert" />
            <!-- Other -->
            <hcc:MealTabRepeaterControl runat="server" ID="MealTabRepeaterOther" />--%>
        </div>
    </div>
    <div class="cl">&nbsp;</div>
</div>

<script type="text/javascript">
    $(document).ready(function() {
        $('.m-nav input:not(.active)').hover(function () {
            //if ($(this).hasClass('active'))
            //    return;
            $(this).toggleClass('active');           
        });

        //Prepare each row iteration
        $('.mealItem').each(function (index, element) {
            //Select the first radio button in each set
            $(this).find('[type="radio"]').prop('checked', false);
            $(this).find('[type="radio"][value="3"]').prop('checked', true);
            //Alternate row colors and remove border from last List Item
            $(this).find('.form-select-menu').each(function () {
                $('tr:odd', $(this)).addClass('bg-grey-light');
                $('tr:last td', $(this)).css('border', 'none');
            });
            //Set default prices
            calcPrice(element, false);
        });

        //set all <DIV> elements equal in height w/o max-height property
        if ($('.m-nav')[0]) {

            //Show only 1st meal program article container
            $('.article-cnt').hide();
            $(".article-cnt:first").show();

            $('.m-nav li a:first').addClass('active-meal');

            //Set references
            $('.m-nav li a').each(function (i, e) {
                $(e).click(function (event) {
                    event.preventDefault();
                    $("#hdnLastMealTab").val($(this).text());
                    //Clear active menu item
                    $('.m-nav li a').removeClass('active-meal');

                    //Display the correct article
                    $('.article-cnt').hide();
                    $('.article-cnt:eq(' + i + ')').show();

                    //Set this menu item as active
                    $(this).addClass('active-meal');
                });
            });
        }

        var lastMealTab = $("#hdnLastMealTab").val();

        if (lastMealTab.length > 0) {
            var t = $('.m-nav li a:contains(' + lastMealTab + ')');
            var idx = $('.m-nav li a').index($('.m-nav li a:contains(' + lastMealTab + ')'));
            //Clear active menu item
            $('.m-nav li a').removeClass('active-meal');

            //Display the correct article
            $('.article-cnt').hide();
            $('.article-cnt:eq(' + idx + ')').show();

            //Set this menu item as active
            t.addClass('active-meal');
        }

        //Select Button Click
        $('.aSelectALC').click(function (event) {
            event.preventDefault();

            //Hide any open elements
            $('.fly-out').hide();
            $('.panel-add-to-cart').hide();

            //Scroll to element
            $('.fly-out', $(this).closest('.mealItem')).toggle();
            var position = $(this).closest('.mealItem').offset();
            if (($(this).closest('.mealItem').outerHeight() + position.top) < $(document).height()) {
                window.scrollTo(0, position.top - 10);
            }

            //Apply method   
            $(this).closest('.mealItem').find('.panel-add-to-cart').show();
        });

        //unselect sibling size options on select
        $('.meal-size').click(function () {
            $('tr', $(this).closest('.form-select-menu')).removeClass('bg-lime-light');
            $(this).closest('tr').addClass('bg-lime-light');

            $(this).closest('.mealItem').find('[type="radio"]').prop('checked', false);
            $(this).find(':radio').prop('checked', true);

            calcPrice($(this).closest('.mealItem'), true);
        });

        $(':checkbox', '.form-select-menu').click(function () {
            if ($(this).is(':checked')) {
                $(this).closest('tr').addClass('bg-lime-light');
            } else {
                $(this).closest('tr').removeClass('bg-lime-light');
            }
            calcPrice($(this).closest('.mealItem'), true);
        });

        //Do green background highlighting on load
        $('input[type=radio]:checked', '.form-select-menu').each(function () {
            $('tr', $(this).closest('.form-select-menu')).removeClass('bg-lime-light');
            $(this).closest('tr').addClass('bg-lime-light');
        });

        $(".meal-side").change(function () {
            var mealItem = $(this).closest('.mealItem');

            if (mealItem.length == 0)
                return;

            if (!checkHighlightMealSideControl(mealItem, 1) || !checkHighlightMealSideControl(mealItem, 2))
                return;
            
            calcPrice(mealItem, true);
        });

        function checkHighlightMealSideControl(mealItem, ordinal)
        {
            if (!mealItem)
                return false;

            var mealSideControl = mealItem.find("select[meal-side='" + ordinal + "']");

            if (mealSideControl.length == 0)
                return false;

            var mealSideMenuItemId = mealSideControl.val();

            if (mealSideMenuItemId > -1) {
                mealSideControl.closest('tr').addClass('bg-lime-light');
            } else {
                mealSideControl.closest('tr').removeClass('bg-lime-light');
            }
            return true;
        }

        function getMealSideCostBySize(mealSide, size) {
            var result;

            switch(size) {
                case "1":
                    result = parseFloat(mealSide.attr("cost-child"));
                    break;
                case "2":
                    result = parseFloat(mealSide.attr("cost-small"));
                    break;
                case "3":
                    result = parseFloat(mealSide.attr("cost-regular"));
                    break;
                case "4":
                    result = parseFloat(mealSide.attr("cost-large"));
                    break;
                default:
                    result = 0;
                    break;
            }
            return isNaN(result) ? 0 : result;
        }

        function getMealSideCost(mealItem, ordinal) {
            try
            {
                var selectedMealSide = mealItem.find("select[meal-side='" + ordinal + "']").find(":selected");
                if (selectedMealSide.length == 0)
                    return 0;

                var selectedSize = mealItem.find(".meal-size").find("input:checked");
                if (selectedSize.length == 0)
                    return 0;

                return getMealSideCostBySize(selectedMealSide, selectedSize.val());
            }
            catch (ex)
            {
                return 0;
            }
        }

        //Functions
        function calcPrice(mealItem, allowOverridePrice) {
            var price = 0;
            //Grab Size price
            var sizePrice = parseFloat($('input[type=radio]:checked', mealItem).closest("span").attr('data-price'));
            price += ((isNaN(sizePrice)) ? 0 : sizePrice);
            //Grab Option prices
            $('input[type=checkbox]:checked', mealItem).each(function (index, element) {
                var optionPrice = parseFloat($(element).attr('data-price'));
                price += ((isNaN(optionPrice)) ? 0 : optionPrice);
            });

            if (allowOverridePrice) {
                price += getMealSideCost(mealItem, 1);
                price += getMealSideCost(mealItem, 2);
            }

            //Output to Html
            var priceLabel = $('.plan-price-text', mealItem);

            if (priceLabel.text().trim() == '' || allowOverridePrice) {
                priceLabel.text('$' + price);
                priceLabel.formatCurrency();
            }
        }
    });


</script>
