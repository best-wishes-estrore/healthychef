<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CalendarMenuItemSelectionControl.ascx.cs" Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.CalendarMenuItemSelectionControl" %>
<%--<asp:ScriptManagerProxy runat="server" ID="scriptManager1">
        <Scripts>
            <asp:ScriptReference Path="/client/jquery-ui/jquery-ui-1.8.21.custom.min.js" />
            <asp:ScriptReference Path="~/WebModules/ShoppingCart/Admin/client/StandardFunctions.js" />
            <asp:ScriptReference Path="~/WebModules/ShoppingCart/Admin/client/UI.js" />
        </Scripts>
        <Services>
            <asp:ServiceReference Path="~/WebModules/ShoppingCart/Admin/AjaxHandlers/CalendarServices.asmx" />
        </Services>
    </asp:ScriptManagerProxy>--%>
<div class="hcc_admin_ctrl">
    <span style="display:block; margin:10px 0 10px 0">Select Delivery Date</span>
    <div class="calendar"></div>
    <asp:HiddenField ID="SelectedCalendarDate" runat="server" />
    <asp:PlaceHolder runat="server" ID="placeholder1">
        <script type="text/javascript">
            var calendarservices = (function () {
                var selectedDate = null;

                function init() {
                    
                }

                function setSelectedDate(date) {
                    $("#<%=SelectedCalendarDate.ClientID%>").val(date);
                    selectedDate = date;
                }

                function postback() {
                    <%=PagePostBackScript %>
                }

                return {
                    setSelectedDate: setSelectedDate,
                    init: init,
                    postback: postback,
                    selectedDate: selectedDate
                };
            })();

            function setInlineDatePickers() {
                $(".calendar").datepicker({
                    onSelect: function (date, instance) {
                        calendarservices.setSelectedDate(date);
                        calendarservices.postback();
                    },
                    defaultDate:$("#<%=SelectedCalendarDate.ClientID%>").val()
                });

                calendarservices.init();
            }
        </script>
    </asp:PlaceHolder>
</div>