<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WhiteBox.ascx.cs" Inherits="HealthyChef.Templates.HealthyChef.Controls.WhiteBox" %>

<div class="box-white">
    <div class="center"><bss:QuickContent runat="server" ID="quick_panel01" ContentName="Sidebar Panel 01" /></div>
    <div class="center"><bss:QuickContent runat="server" ID="quick_panel02" ContentName="Sidebar Panel 02" /></div>
    <div class="center"><bss:QuickContent runat="server" ID="quick_panel03" ContentName="Sidebar Panel 03" /></div>
    <div class="center"><bss:QuickContent runat="server" ID="quick_panel04" ContentName="Sidebar Panel 04" /></div>
    <div class="center"><bss:QuickContent runat="server" ID="quick_panel05" ContentName="Sidebar Panel 05" /></div>
    <script type="text/javascript">
        $(document).ready(function () {
            //Remove any containers with empty content
            $('.box-white > .center').each(function (index, element) {
                if ($(element).html() == '') {
                    $(element).remove();
                }
            });            

            //Show one at random
            var randomNumber = Math.floor(Math.random() * $('.box-white > .center').size());
            $('.box-white > .center').hide();
            $('.box-white > .center:eq(' + randomNumber + ')').show();
        });
    </script>
    <div class="top">&nbsp;</div>
    <div class="bottom">&nbsp;</div>
</div>