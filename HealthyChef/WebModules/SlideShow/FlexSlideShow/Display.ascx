<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Display.ascx.cs" Inherits="BayshoreSolutions.WebModules.SlideShow.FlexSlideShow.Display" %>

<script type="text/javascript">
    $(document).ready(function () {
        function fadeNext() {
            $('.slide').show();
            $('#slider > .slide:last').fadeOut(2000, function () {
                $(this).prependTo('#slider');  
            });
        }
        window.setInterval(fadeNext, 5000);
    });
</script>

<div id="slider">
    <asp:ListView runat="server" OnItemDataBound="Item_DataBound" ID="ImagesList">
        <ItemTemplate>
            <div class="slide"><%-- style="display: none;" --%>
                <asp:HyperLink CssClass="fss_link" runat="server" ID="hyperlink" Visible="false" />
                <asp:Image CssClass="fullscreen" runat="server" ID="image" />
            </div>
        </ItemTemplate>
    </asp:ListView>
</div>
