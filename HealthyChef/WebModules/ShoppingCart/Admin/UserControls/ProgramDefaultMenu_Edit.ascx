<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProgramDefaultMenu_Edit.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.ProgramDefaultMenu_Edit" %>
<style>
    .dayPanel {
        /*width: 130px;
        height: 100%;
        display: inline-block;
        padding: 0px;
        margin-right: 8px;
        border: 0;*/
        width: 13.2%;
        float: left;
    }

    center {
        margin-top: 12px;
    }

    .mealItemchk tbody {
        max-height: 170px;
        display: block;
        overflow-y: auto;
        width: 100%;
        min-height: 170px;
    }

    .dayPanel select {
        width: 100%;
        font-size: 10px;
    }
</style>
<div class="col-sm-12 lengend">
    <div class="fieldRow">
        <div class="fieldCol">
            <p></p>
            Required Daily Menu Items:
       <br />
            <ul id="ulReqTypes" runat="server" clientidmode="Static" />
        </div>
    </div>
    <div class="fieldRow">
        <div class="fieldCol">
            <input id="btnSave" type="button" class="btn btn-info" value="Save" title="Save" onclick="SaveDDLs();" />
            <p id="pFeedback"></p>
            <asp:Label ID="lblFeedback" runat="server" ClientIDMode="Static" EnableViewState="false" ForeColor="Red" />
        </div>
    </div>
    <div>&nbsp;</div>
    <div class="fieldRow">
        <div class="fieldCol">
            <asp:Panel ID="pnlDefaultMenu" runat="server" ClientIDMode="Static">
            </asp:Panel>
        </div>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        var d = $("#pnlDefaultMenu").hasClass("disabled");

        if (d) {
            $("#btnSave").attr("enabled", "enabled"); //[BWE]
        }
        else {
            $("#btnSave").removeAttr("enabled");//[BWE]
        }

    });
    function SaveDDLs() {
        MakeUpdateProg(true);

        var strJson = '{"hccProgramDefaultMenus": [';
        var isValid = true;
        var cals, fat, prtn, carb, fbr;

        $('.mealItemDdl').each(function (index) {
            var ddl = $(this);
            var ddlVals = ddl.val().toString().split("-");
            var menuItemId = ddlVals[0];
            var size = ddlVals[1];

            //if (menuItemId == "0")
            //{
            //    isValid = false;
            //    ddl.css("border","1px solid red");
            //}
            //else {
            //ddl.css("border", "1px solid red");
            var progId = ddl.attr("progId");
            var calId = ddl.attr("calId");
            var day = ddl.attr("day");
            var mealtype = ddl.attr("type");
            var ord = ddl.attr("ord");

            if (!size)
                size = 0;

            var defMenuId = ddl.attr("defMenuId");

            strJson = strJson + '{ "ProgramID": "' + progId + '", "CalendarID": "' + calId + '", "MenuItemSizeID": "' + size + '", "DayNumber": "' + day
                + '", "MealTypeId": "' + mealtype + '", "Ordinal": "' + ord + '", "MenuItemID": "' + menuItemId + '"';

            if (defMenuId)
                strJson = strJson + ', "DefaultMenuID": "' + defMenuId + '"';


            var defMenuAvailablePrefs = $("[type='" + mealtype + "'][calid='" + calId + "'][progid='" + progId + "'][day='" + day + "']table").find("span");
            var preferencelist = [];
            defMenuAvailablePrefs.each(function () {
                var defMenuPrefsChk = $(this);
                var defMenuPrefsChkValue = defMenuPrefsChk.find("input[type=checkbox]").is(":checked");
                var preferenceId = defMenuPrefsChk[0].attributes[0].value;

                if (defMenuPrefsChkValue) {
                     
                    preferencelist.push(preferenceId);
                    //strJson = strJson + ', "preferenceID": "' + preferenceId + '"';                   
                }
            });
            var preferencearrstring = "";
            $.each(preferencelist, function (key, value) {
                preferencearrstring = preferencearrstring + value + ","
            })
            if (preferencearrstring != "") {
                preferencearrstring = preferencearrstring.substring(0, preferencearrstring.length - 1);

            }
            strJson = strJson + ', "preferenceID": [' + preferencearrstring + ']';
            strJson = strJson + '},';
            //}
        });

        // if (isValid) {
        strJson = strJson.slice(0, -1); // remove trailing comma
        strJson = strJson + "]}";

        $.ajax({
            type: "POST",
            url: '/WebModules/ShoppingCart/Admin/WS_UpdateDefaultMenu.asmx/UpdateMenus',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: strJson,
            success: function (msg) {
                MakeUpdateProg(false);
                $("#lblFeedback").text('');
                $("#pFeedback").text('Default Menu Saved: ' + new Date().toLocaleString()).css("color", "green");

                var nutri = msg.d.split("|");

                $("#cals1").text(nutri[0]);
                $("#fat1").text(nutri[1]);
                $("#prtn1").text(nutri[2]);
                $("#carb1").text(nutri[3]);
                $("#fbr1").text(nutri[4]);
                $("#cals2").text(nutri[5]);
                $("#fat2").text(nutri[6]);
                $("#prtn2").text(nutri[7]);
                $("#carb2").text(nutri[8]);
                $("#fbr2").text(nutri[9]);
                $("#cals3").text(nutri[10]);
                $("#fat3").text(nutri[11]);
                $("#prtn3").text(nutri[12]);
                $("#carb3").text(nutri[13]);
                $("#fbr3").text(nutri[14]);
                $("#cals4").text(nutri[15]);
                $("#fat4").text(nutri[16]);
                $("#prtn4").text(nutri[17]);
                $("#carb4").text(nutri[18]);
                $("#fbr4").text(nutri[19]);
                $("#cals5").text(nutri[20]);
                $("#fat5").text(nutri[21]);
                $("#prtn5").text(nutri[22]);
                $("#carb5").text(nutri[23]);
                $("#fbr5").text(nutri[24]);
                $("#cals6").text(nutri[25]);
                $("#fat6").text(nutri[26]);
                $("#prtn6").text(nutri[27]);
                $("#carb6").text(nutri[28]);
                $("#fbr6").text(nutri[29]);
                $("#cals7").text(nutri[30]);
                $("#fat7").text(nutri[31]);
                $("#prtn7").text(nutri[32]);
                $("#carb7").text(nutri[33]);
                $("#fbr7").text(nutri[34]);
            },
            error: function (xhr, msg) {
                $("#pFeedback").text('Save Failed.').css("color", "red");
                MakeUpdateProg(false);
            }
        });
        //  }
    }

    $(".mealItemDdl").change(function () {
         
        var ddl = $(this);
        var progId = ddl.attr("progId");
        var calId = ddl.attr("calId");
        var day = ddl.attr("day");
        var mealtype = ddl.attr("type");
        var ord = ddl.attr("ord");
        var defMenuId = ddl.attr("defMenuId");
        //var menutitemid = $('option:selected', this).val();
        var ddlVals = $('option:selected', this).val().toString().split("-");
        var menuItemId = ddlVals[0];

        var defaultmenutitemdetails = {
            "DefaultMenuId": defMenuId,
            "ProgramID": progId,
            "CalendarID": calId,
            "MenuItemID": menuItemId,
            "MenuItemSizeID": "0",
            "MealTypeID": mealtype,
            "DayNumber": day,
            "Ordinal": ord,
            "preferenceID": []
        };
        $.ajax({
            type: "POST",
            data: JSON.stringify({ 'Defaultmenudetails': defaultmenutitemdetails }),
            url: '/WebModules/ShoppingCart/Admin/WS_UpdateDefaultMenu.asmx/ShowingPreferencesByMenuitemid',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (res) {
                 
                if (res.d != '') {
                    var pref = res.d.split("*");
                    for (var i = 0; i < pref.length - 1; i++) {
                        var tbody = ''
                        var menuitem = pref[i].split('&');
                        //for (var j = 0; j <= menuitem.length; j++)
                        //{
                        var detailsofgettingchek = menuitem[1].split('|');
                        //$("[type='" + detailsofgettingchek[0] + "'][calid='" + detailsofgettingchek[1] + "'][progid='" + detailsofgettingchek[2] + "'][day='" + detailsofgettingchek[3] + "']").find('tbody').children().remove();
                        if (menuitem[0].split('|')[1] == 1)
                            tbody += '<tr><td><span prefID=' + menuitem[0].split('|')[0] + '><input type="checkbox" checked/><label >' + menuitem[0].split('|')[2] + '</label></span></td></tr>';
                        else
                            if (menuitem[0].split('|')[2] = " ") {
                                tbody += '<tr><td></td></tr>';
                            }
                        if (menuitem[0].split('|')[1] == 0) {
                            if (menuitem[0].split('|')[2] != " ") {
                                tbody += '<tr><td><span prefID=' + menuitem[0].split('|')[0] + '><input type="checkbox" /><label >' + menuitem[0].split('|')[2] + '</label></span></td></tr>';
                            }
                        }
                        if (i == 0) {
                            $("[type='" + detailsofgettingchek[0] + "'][calid='" + detailsofgettingchek[1] + "'][progid='" + detailsofgettingchek[2] + "'][day='" + detailsofgettingchek[3] + "']").find('tbody').children().remove();
                        }
                        $("[type='" + detailsofgettingchek[0] + "'][calid='" + detailsofgettingchek[1] + "'][progid='" + detailsofgettingchek[2] + "'][day='" + detailsofgettingchek[3] + "']").find('tbody').append(tbody);

                        // }
                    }
                }
                else {
                    tbody += '<tr><td><span prefID=0><input type="checkbox" /><label >No preferences</label></span></td></tr>';
                }
            },
            error: function (xhr, msg) {
                 
                alert('error');
            }
        });
    });
</script>
