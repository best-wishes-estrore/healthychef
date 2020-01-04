<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderFulfillmentProgram_Edit.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.OrderFulfillmentProgram_Edit" %>

<%@ Register TagPrefix="hcc" TagName="ProfileNotesEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/UserProfileNote_Edit.ascx" %>

<link rel="stylesheet" type="text/css" href="/App_Themes/WebModules/cartadmin.css" />

<asp:LinkButton ID="lkbBack" runat="server" CssClass="btn btn-info" Text="<< Back to Listing" PostBackUrl="~/WebModules/ShoppingCart/Admin/OrderFulfillment.aspx"
    CausesValidation="false" OnClientClick="MakeUpdateProg(true);" />
<table>
    <tbody>
        <tr>
            <td>
                <div class="fieldRow" style="padding: 0px 10px">
                    <div class="fieldCol">
                        <div class="table">
                            <div style="width: 14%; float: left">
                                <p class="label2">Meal Days:&nbsp;</p>
                                <asp:RadioButtonList ID="rdoDays" runat="server" AutoPostBack="true" onchange="MakeUpdateProg(true);" />
                                <p>
                                    <input id="btnSave" type="button" class="btn btn-info" value="Save" title="Save" onclick="SaveDDLs();" style="display: none;" />
                                    <input id="btnreset" type="button" class="btn btn-info" value="Reset" title="Reset" onclick="ResettoProgramDefaultMenu();" style="display: none;" />
                                </p>
                                <p id="pFeedback"></p>
                                <asp:Label ID="lblFeedback" runat="server" ClientIDMode="Static" EnableViewState="false" ForeColor="Red" />
                            </div>
                            <div style="width: 25%; float: left">
                                <p class="label2" style="float: left">Customer  Name:&nbsp;</p>
                                <asp:Label ID="lblCustomerName" runat="server" /><br />
                                <p class="label2" style="float: left">Profile Name:&nbsp;</p>
                                <asp:Label ID="lblProfileName" runat="server" /><br />
                                <p class="label2" style="float: left">Order Number:&nbsp;</p>
                                <asp:Label ID="lblOrderNumber" runat="server" /><br />
                                <p class="label2" style="float: left">Quantity:&nbsp;</p>
                                <asp:Label ID="lblQuantity" runat="server" /><br />
                                <p class="label2" style="float: left">Delivery Date:&nbsp;</p>
                                <asp:Label ID="lblDeliveryDate" runat="server" /><br />
                                <p class="label2" style="float: left">Is Complete:&nbsp;</p>
                                <asp:CheckBox ID="chkIsComplete" runat="server" ClientIDMode="Static" /><br />
                                <p class="label2" style="float: left">Is Cancelled:&nbsp;</p>
                                <asp:CheckBox ID="chkIsCancelledDisplay" runat="server" ClientIDMode="Static" Enabled="false" /><br />

                                <span style="font-size: 14px;">
                                    <p class="label2" style="float: left">Program:&nbsp;</p>
                                    <asp:Label ID="lblProgram" runat="server" /><br />
                                    <p class="label2" style="float: left">Plan:&nbsp;</p>
                                    <asp:Label ID="lblPlan" runat="server" /><br />
                                    <p class="label2" style="float: left">Option:&nbsp;</p>
                                    <asp:Label ID="lblPlanOption" runat="server" />
                                </span>
                            </div>
                            <div style="width: 10%; float: left;">
                                <p class="label">Allergens:</p><br />
                                <asp:ListView ID="lvwAllrgs" runat="server" GroupItemCount="2">
                                    <LayoutTemplate>
                                        <ul>
                                            <li id="groupPlaceHolder" runat="server"></li>
                                        </ul>
                                    </LayoutTemplate>
                                    <GroupTemplate>
                                        <li id="itemPlaceHolder" runat="server"></li>
                                    </GroupTemplate>
                                    <ItemTemplate>
                                        <li><%# Eval("Name") %></li>
                                    </ItemTemplate>
                                    <EmptyDataTemplate>
                                        No Allergens.
                                    </EmptyDataTemplate>
                                </asp:ListView>
                                <hcc:ProfileNotesEdit ID="ProfileNotesEdit_AllergenNote" runat="server" CurrentNoteType="AllergenNote"
                                    AllowDisplayToUser="true" ShowAllNotes="true" UserDisplayNotesTitle="Allergen Notes:" />
                            </div>
                            <div style="width: 17%; float: left">
                                <p class="label">Preferences:</p>
                                <asp:ListView ID="lvwPrefs" runat="server">
                                    <LayoutTemplate>
                                        <ul>
                                            <li id="itemPlaceHolder" runat="server"></li>
                                        </ul>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <li><%# Eval("Name") %></li>
                                    </ItemTemplate>
                                    <EmptyDataTemplate>No Preferences.</EmptyDataTemplate>
                                </asp:ListView>
                            </div>
                            <div class="pref-note" style="float: left; width: 32%">
                                <hcc:ProfileNotesEdit ID="ProfileNotesEdit_PreferenceNote" runat="server" CurrentNoteType="PreferenceNote"
                                    AllowDisplayToUser="true" ShowAllNotes="true" UserDisplayNotesTitle="Preference Notes:" />
                            </div>
                        </div>

                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div class="fieldRow" style="padding: 0px 10px">
                    <div class="fieldCol">
                        <table>
                            <tr>
                                <td style="width: 40%; float: left;">
                                    <div id="divTotalNutrition" runat="server" enableviewstate="false" visible="false" clientidmode="Static">
                                        <fieldset class="cals nutrData">
                                            <legend>Calories</legend>
                                            ##Cals##
                                        </fieldset>
                                        <fieldset class="fat nutrData">
                                            <legend>Fat</legend>
                                            ##Fats## 
                                        </fieldset>
                                        <fieldset class="prtn nutrData">
                                            <legend>Protein</legend>
                                            ##Ptrns##
                                        </fieldset>
                                        <fieldset class="carb nutrData">
                                            <legend>Carbs</legend>
                                            ##Carbs##
                                        </fieldset>
                                        <fieldset class="fbr nutrData">
                                            <legend>Fiber</legend>
                                            ##Fbrs##
                                        </fieldset>
                                        <fieldset class="sod nutrData">
                                            <legend>Sodium</legend>
                                            ##Sod##
                                        </fieldset>
                                    </div>
                                    <asp:Panel ID="pnlDefaultMenu" runat="server" CssClass="menuPanels" ClientIDMode="Static">
                                    </asp:Panel>
                                </td>
                                <td style="width: 60%; float: left;">
                                    <span class="label">Week at a Glance</span>
                                    <asp:LinkButton ID="lkbRefresh" runat="server" Text="Refresh" OnClick="lkbRefresh_Click" />
                                    <asp:ListView ID="lvwWeekGlance" runat="server" GroupItemCount="1">
                                        <LayoutTemplate>
                                            <table>
                                                <tr id="groupPlaceholder" runat="server" />
                                            </table>
                                        </LayoutTemplate>
                                        <GroupTemplate>
                                            <tr style="width: 50%; float: left; padding: 0px 10px;">
                                                <td id="itemPlaceholder" runat="server" />
                                            </tr>
                                        </GroupTemplate>
                                        <ItemTemplate>
                                            <td>
                                                <fieldset>
                                                    <legend class="label"><%# Eval("DayTitle") %></legend>
                                                    <%# Eval("DayInfo") %>
                                                </fieldset>
                                            </td>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td style="padding: 0px 10px">
                <fieldset>
                    <legend>Legend</legend>
                    <ul>
                        <li><span class="greenBorder">&nbsp;&nbsp;&nbsp;</span> - No Allergen Alert (default item). 
                The current default item does not have allergens conflicting with the customer's profile. Confirm customer preferences.</li>
                        <li><span class="redBorder">&nbsp;&nbsp;&nbsp;</span> - Allergen Alert. The current item has allergens conflicting with the customer's profile. Modify item preferences or select another item, then confirm customer preferences.</li>
                        <li><span class="blueBorder">&nbsp;&nbsp;&nbsp;</span> - No Allergen Alert (changed item). The current item has been changed from the default, 
                and does not have allergens conflicting with the customer's profile. Confirm customer preferences.</li>
                        <li><span class="italic">Italics - item is the default menu item.</span></li>
                        <li><span class="bold">Bold - item is the currently saved active menu item.</span></li>
                        <li><span class="redFont">Red - item has potential allergens that conflict with customer's profile. Review available item preferences.</span></li>
                    </ul>
                </fieldset>
            </td>
        </tr>

    </tbody>
</table>
<%--<div class="updateProgressContainer">
</div>
<div class="updateProgressDisplay">
    <img src="/App_Themes/HealthyChef/Images/Spinning_wheel_throbber.gif" alt="Loading..." />Loading....
</div>--%>
<script type="text/javascript">


    function EnableSideddl(type, NoOfside) {
        var isfirst = true;
        $('.mealItemDdl').each(function (index) {
            var ddltype =  $(this).attr("ddltype");
            if (ddltype == type) {
                debugger;
                if (NoOfside == 0) {
                    $(this).attr('disabled', 'disabled');
                }
                if (NoOfside == 2) {
                    $(this).removeAttr('disabled');
                 
                }
                if (NoOfside == 1) {
                    if (!isfirst) {
                       $(this).attr('disabled', 'disabled');
                    }
                    if (isfirst) {
                        $(this).removeAttr('disabled');
                        isfirst = false
                    }
                  

                   
                }
            }

        });
    }
    function BindNoOfSides() {
        debugger;
        $('.mealItemDdl').each(function (index) {
            debugger;
            var ddl = $(this);
            var ddltype = ddl.attr("ddltype");
            for (var day = 1; day < 8; day++) {
                var attrval = "BreakfastEntree";

                if (attrval == ddltype) {
                    debugger;
                    var val = $(this).val();
                    var lbl = "#lbl" + attrval;
                    var NoOfside = 0;
                   
                    if ($(this).val().split('-').length > 2) {
                        NoOfside = $(this).val().split('-')[2];
                      
                    }
                    $(lbl + '-' + day).text(NoOfside);
                    EnableSideddl('BreakfastSide', NoOfside);
                  
                }
                attrval = "LunchEntree";

                if (attrval == ddltype) {
                    debugger;
                    var val = $(this).val();
                    var lbl = "#lbl" + attrval;
                     var NoOfside = 0;
                   
                    if ($(this).val().split('-').length > 2) {
                        NoOfside = $(this).val().split('-')[2];
                      
                    }
                    $(lbl + '-' + day).text(NoOfside);
                     EnableSideddl('LunchSide', NoOfside);
                    
                }

                attrval = "DinnerEntree";

                if (attrval == ddltype) {
                    debugger;
                    var val = $(this).val();
                    var lbl = "#lbl" + attrval;
                   
                    var NoOfside = 0;
                   
                    if ($(this).val().split('-').length > 2) {
                        NoOfside = $(this).val().split('-')[2];
                      
                    }
                     $(lbl + '-' + day).text(NoOfside);
                    EnableSideddl('DinnerSide', NoOfside);
                   
                }
            }

        });
    }
    $(document).ready(function () {
        debugger;
        if ($("#pnlDefaultMenu").is(":visible") && !($("#chkIsComplete").is(":disabled"))) {
            $("#btnSave").show();
            $("#btnreset").show();

            $("#divTotalNutrition").insertAfter(".dayName");
            $("#pFeedback").prependTo("#divTotalNutrition");
        }
        else {
            $("#btnSave").hide();
            $("#btnreset").hide();
     
        }

        //set meal day redFont
        var day = $(".dayName").text().split(' ')[1];
        var mealDay = $(".mealDay" + day);
        var redBorders = $("select.redBorder");
        if (redBorders.length > 0) {
            mealDay.addClass("redFont");
        }
        else { mealDay.removeClass("redFont"); }

        $("#chkIsComplete").change(function () {
            var c = confirm("Are you sure that you have checked the allergens and preferences, and want to change this order item's Completion status?");

            if (!c) {
                $("#chkIsComplete").prop('checked', !($("#chkIsComplete").is(":checked")));
            }
        });

        $("#chkIsCancelled").change(function () {
            var c = confirm("Are you sure that you want to change this order item's Cancellation status?");

            if (!c) {
                $("#chkIsCancelled").prop('checked', !($("#chkIsCancelled").is(":checked")));
            }
        });

        $("select").change(function () {
            var selOption = $(this).find(":selected");
            var divNuts = $(this).parent().siblings('.divNuts');
            var divAllrgs = $(this).parent().siblings('.divAllrgs');
            var divPrefs = $(this).parent().siblings('.divPrefs');
            divNuts.empty();
            divAllrgs.empty();
            divPrefs.empty();

            if (selOption.text().trim() == "None") {
                $(this).removeClass("blueBorder").removeClass("redBorder").removeClass("greenBorder");

                divNuts.hide();
                divAllrgs.hide();
                divPrefs.hide();
            }
            else {
                if (selOption.attr("class")) {
                    if (selOption.attr("class").toString().indexOf("redFont") < 0) {
                        var selOptClass = selOption.attr("class").toString();

                        if (selOptClass.indexOf("italic") < 0)
                            $(this).addClass("blueBorder").removeClass("redBorder").removeClass("greenBorder");
                        else
                            $(this).addClass("greenBorder").removeClass("redBorder").removeClass("blueBorder");
                    }
                    else {
                        $(this).addClass("redBorder").removeClass("blueBorder").removeClass("greenBorder");
                    }
                }
                else {
                    var dflt = $(this).find("option.italic");

                    if (dflt && dflt.attr("class")) {
                        if (dflt.attr("class").toString().indexOf("redFont") < 0)
                            $(this).addClass("greenBorder").removeClass("redBorder");
                        else
                            $(this).addClass("blueBorder").removeClass("redBorder");
                    }
                    else {
                        $(this).removeClass("blueBorder").removeClass("redBorder").removeClass("greenBorder");
                    }
                }
            }

            // get prefs
            var menuItemId = selOption.val().toString().split('-')[0];

            if (menuItemId != 0) {
                var path = '/WebModules/ShoppingCart/Admin/WS_GetMenuItemPrefs.asmx/GetPreferences?mid=' + menuItemId;

                $.ajax({
                    type: "POST",
                    url: path,
                    contentType: "text/plain;",
                    dataType: "text",
                    success: function (msg) {
                        //$("#pFeedback").text('Update menuitem success.').removeClass("redFont").addClass("greenFont");

                        try {
                            var xmlDoc = $.parseXML(msg);
                            var nuts = $(xmlDoc).find('menuData').find("nutrition").text().trim();
                            var alrgs = $(xmlDoc).find('menuData').find("allergens").text().trim();
                            var prefs = $(xmlDoc).find('menuData').find("prefs").text().trim();

                            if (nuts.length > 0) {
                                divNuts.text(nuts);
                                divNuts.show();
                            }

                            if (alrgs.length > 0) {
                                divAllrgs.text(alrgs);
                                divAllrgs.show();
                            }

                            if (prefs != 'None') {
                                var prefData = prefs.split("|");
                                for (var i = 0; i < prefData.length; i++) {
                                    var prefInfo = prefData[i].split(":");
                                    divPrefs.append("<input type='checkbox' value='" + prefInfo[0] + "'><span>" + prefInfo[1] + "</span>")
                                }
                                divPrefs.show();
                            }
                        }
                        catch (err) {
                            $("#pFeedback").text('Update menu item failed.').removeClass("redFont").removeClass("greenFont").removeClass("blueFont");
                        }

                    },
                    error: function (xhr, msg) {
                        //$("#pFeedback").text('Update menu item Failed.').removeClass("greenFont").addClass("redFont");                   
                    }
                });
            }
            //set meal day redFont
            var day = $(".dayName").text().split(' ')[1];
            var mealDay = $(".mealDay" + day);
            var redBorders = $("select.redBorder");
            if (redBorders.length > 0) {
                mealDay.addClass("redFont");
            }
            else { mealDay.removeClass("redFont"); }

             BindNoOfSides();
        });

        BindNoOfSides();
    });

    function SaveDDLs() {
        MakeUpdateProg(true);
        $("#pFeedback").text("");

        var strJson = '{"defaultMenus": [';
        var isValid = true;
        var cals, fat, prtn, carb, fbr;

        //var inValidFields = $(".menuPanels").find(".redBorder");

        //if (inValidFields.length == 0) {
        $('.mealItemDdl').each(function (index) {
            var ddl = $(this);
            var ddlVals = ddl.val().toString().split("-");
            var menuItemId = ddlVals[0];
            var size = ddlVals[1];

            var progId = ddl.attr("progId");
            var calId = ddl.attr("calId");
            var day = ddl.attr("day");
            var mealtype = ddl.attr("type");
            var ord = ddl.attr("ord");

            if (!size)
                size = 0;

            var defMenuId = ddl.attr("defMenuId");

            strJson = strJson + '{ "ProgramID": "' + progId + '", "CalendarID": "' + calId + '", "MenuItemSizeID": "' + size + '", "DayNumber": "' + day
                + '", "MealTypeID": "' + mealtype + '", "Ordinal": "' + ord + '", "MenuItemID": "' + menuItemId + '"';

            if (defMenuId)
                strJson = strJson + ', "DefaultMenuID": "' + defMenuId + '"';

            // get prefs
            var divPrefs = $(this).parent().siblings('.divPrefs');
            var chks = divPrefs.find("input:checkbox:checked");
            var strPrefIds = '';

            if (chks.length > 0) {
                for (var i = 0; i < chks.length; i++) {
                    strPrefIds += chks[i].value + ',';
                }
                strPrefIds = strPrefIds.slice(0, -1); // remove trailing comma
            }

            if (strPrefIds.length > 0)
                strJson = strJson + ', "Prefs": "' + strPrefIds + '"';

            strJson = strJson + '},';
        });

        if (strJson.substring(strJson.length - 1, strJson.length) == ',')
            strJson = strJson.slice(0, -1); // remove trailing comma

        strJson = strJson + "]}";

        var path = '/WebModules/ShoppingCart/Admin/WS_ReplaceProgramMeals.asmx/ReplaceProgramMeals?cid=<%= this.PrimaryKeyIndex.ToString() %>';

        var complete = $("#chkIsComplete").is(":checked")
        path += "&cmplt=" + complete;

        var cancel = $("#chkIsCancelled").is(":checked")
        path += "&can=" + cancel;

        $.ajax({
            type: "POST",
            url: path,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: strJson,
            success: function (msg) {
                $("#pFeedback").text('Menu Saved: ' + new Date().toLocaleString()).removeClass("redFont").addClass("greenFont");

                var nutri = msg.d.split("|");

                $(".cals").html('<legend>Calories</legend>' + nutri[0]);
                $(".fat").html('<legend>Fat</legend>' + nutri[1]);
                $(".prtn").html('<legend>Protien</legend>' + nutri[2]);
                $(".carb").html('<legend>Carbs</legend>' + nutri[3]);
                $(".fbr").html('<legend>Fiber</legend>' + nutri[4]);
                $(".sod").html('<legend>Sodium</legend>' + nutri[5]);
                MakeUpdateProg(false);
            },
            error: function (xhr, msg) {
                $("#pFeedback").text('Save Failed.').removeClass("greenFont").addClass("redFont");
                MakeUpdateProg(false);
            }
        });
    }
    function ResettoProgramDefaultMenu()
    {
         MakeUpdateProg(true);
        $("#pFeedback").text("");

        var strJson = [];
        $('.mealItemDdl').each(function (index) {
            var resetmenu = {};
             var ddl = $(this);
            var cartcalId = ddl.attr("defMenuCartCalendarId");
            var defmenuexceptid = ddl.attr("defMenuExceptionId");
            var defmenuid = ddl.attr("defmenuid");
             var day = ddl.attr("day");
             resetmenu = {DefaultMenuExceptionId:defmenuexceptid,DefaultMenuID:defmenuid,CartCalendarID:cartcalId,DayNumber:day};
            strJson.push(resetmenu);
        });
         $.ajax({
            type: "POST",
            data: JSON.stringify({ 'resetMenuList': strJson }),
            url: '/WebModules/ShoppingCart/Admin/WS_ReplaceProgramMeals.asmx/ResetTheProgramDefaultmenu?cid=<%= this.PrimaryKeyIndex.ToString() %>',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
             success: function (msg) {
                 window.location.reload();
                 $("#pFeedback").text('Reset Menu Successfully: ' + new Date().toLocaleString()).removeClass("redFont").addClass("greenFont");
                 //alert('Reset Menu Successfully: ' + new Date().toLocaleString());
                 MakeUpdateProg(false);
            },
            error: function (xhr, msg) {
                $("#pFeedback").text('Reset Menu Failed.').removeClass("greenFont").addClass("redFont");
                MakeUpdateProg(false);
            }
        });
        //alert('hai');
    }
</script>
<style type="text/css">
    .main-content, td, legend, span, li, label {
        font-size: 10px !important;
    }

    .table {
        box-shadow: 0px 0px 0px #ddd;
    }

    .dayPanel select {
        width: 200px;
    }

    .nutrData {
        display: inline;
        width: 45px;
    }

    hr {
        display: none;
    }

    fieldset {
        margin-top: 5px;
        font-size: 8px;
    }

    select {
        height: 20px;
    }

    .wm_loginName {
        font-size: 16px !important;
    }

    label {
        font-weight: 500 !important;
        background-color: #fff !important;
        color: #444 !important;
        font-size: 10px !important;
    }

    fieldset {
        display: block;
        -webkit-margin-start: 0 !important;
        -webkit-margin-end: 0px !important;
        -webkit-padding-before: 0 !important;
        -webkit-padding-start: 0.75em !important;
        -webkit-padding-end: 0.75em !important;
        -webkit-padding-after: 0 !important;
        min-width: -webkit-min-content;
        border-width: 1px;
        border-style: groove;
        border-color: threedface;
        border-image: initial;
        margin-top: 0px !important;
        border-radius: 5px;
        /*float: left !important;*/
    }

    .pref-note .label {
        float: left;
        width: 100%;
        text-align: left;
        margin-left: -7px;
    }

    fieldset p {
        margin: 0 0 6px !important;
    }
</style>
