<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserProfileRecurringOrders_Edit.ascx.cs" Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.UserProfileRecurringOrders_Edit" %>

    <script>
        $(document).ready(function () {           
            $('a[id*=btnDelete]').each(function () {
                var hrefJs = $(this).attr('href');
                hrefJs = hrefJs.substring(11, hrefJs.length); // remvoves the leading "javascript:"
                $(this).attr('href', 'javascript: if(confirmDelete()){' + hrefJs + '; }');
            });
        }); 

        function confirmDelete() {
            if (!confirm('Are you sure you want to cancel this recurring order?')) {
                return false;
            }

            return true;
        }
    </script>

    
    <div class="m-2">
        <table>
            <tr>
                <th>Recurring Orders</th><th>Last Scheduled Delivery</th><th></th>
            </tr>
            <asp:ListView ID="lvRecurringOrders" runat="server">
                <ItemTemplate> 
                    <tr>  
                        <td>
                            <asp:Literal ID="litCartItem" runat="server" Text='<%# GetCartItemMeal(int.Parse( Eval("CartItemID").ToString())) %>'></asp:Literal>
                        </td>
                        <td style="text-align: center;"><asp:Literal ID="litNextStartDate" runat="server" Text='<%# GetNextRecurringDate(int.Parse(Eval("CartID").ToString()), int.Parse(Eval("CartItemID").ToString())) %>'></asp:Literal></td>
                        <td><asp:LinkButton ID="btnDelete" OnCommand="btnDeleteOnCommand" runat="server" Text="Cancel Auto-Renew" ClientIDMode="Static" CommandArgument='<%# Eval("CartID") + "_" + Eval("CartItemID") %>' /></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate> 
                    <tr>  
                        <td>
                            <asp:Literal ID="litCartItem" runat="server" Text='<%# GetCartItemMeal(int.Parse(Eval("CartItemID").ToString()))%>'></asp:Literal>
                        </td>
                        <td style="text-align: center;"><asp:Literal ID="litNextStartDate" runat="server" Text='<%# GetNextRecurringDate(int.Parse(Eval("CartID").ToString()), int.Parse(Eval("CartItemID").ToString())) %>'></asp:Literal></td>
                        <td><asp:LinkButton ID="btnDelete" OnCommand="btnDeleteOnCommand" runat="server" Text="Cancel Auto-Renew" ClientIDMode="Static" CommandArgument='<%# Eval("CartID") + "_" + Eval("CartItemID") %>' /></td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:ListView>
        </table>
    </div>
