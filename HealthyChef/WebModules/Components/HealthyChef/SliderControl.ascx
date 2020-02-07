<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SliderControl.ascx.cs" Inherits="HealthyChef.WebModules.Components.HealthyChef.SliderControl" %>

<div style="padding:15px;">
    <asp:Literal runat="server" ID="sliderValueLabel" />&nbsp;<asp:Label runat="server" ID="slider_value" />
    <div id="slider" runat="server"></div>
    <asp:HiddenField runat="server" ID="currentValue" />
</div>

<asp:PlaceHolder runat="server" ID="placeholder1">
<script type="text/javascript">
    $(document).ready(function() {
        $("#<%=slider.ClientID %>").slider({
            range: "min",
            value: <%=CurrentValue%>,
            min: <%=MinValue%>,
            max: <%=MaxValue%>,
            slide: function (event, ui) { $("#<%=slider_value.ClientID%>").html(ui.value); },
            stop: function (event, ui) { $("#<%=currentValue.ClientID %>").val(ui.value); }
        });

        $("#<%=slider_value.ClientID%>").html(<%=CurrentValue%>);
    });
</script>
</asp:PlaceHolder>