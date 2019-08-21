using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HealthyChef.Common;

namespace HealthyChef.DAL
{
    public class AggrCartItem
    {
        private List<hccCartItem> _cartItems;

        public hccCartItem CartItem
        {
            get
            {
                return _cartItems.FirstOrDefault();
            }
            set
            {
                if (value == null)
                    return;

                if (!_cartItems.Any(ci => ci.CartItemID == value.CartItemID))
                    _cartItems.Add(value);
            }
        }
        public hccCartSnapshot CartSnap { get; set; }
        public int CartItemId { get; set; }
        public string SimpleName { get; set; }
        public DateTime DeliveryDate { get; set; }
        /// <summary>
        /// Total quantity of Al A Carte items for this order number
        /// </summary>
        public int ALC_Count { get; set; }
        /// <summary>
        /// Total quantity of All items for this order number
        /// </summary>
        public int TotalQuantity { get; set; }
        public string SimpleStatus
        {
            get
            {
                string retVal = string.Empty;

                if (CartItem.ItemType == Enums.CartItemType.DefinedPlan)
                {
                    hccCartItemCalendar cartCal = hccCartItemCalendar.GetBy(CartItem.CartItemID, this.DeliveryDate);

                    if (cartCal != null)
                    {
                        if (cartCal.IsFulfilled)
                            retVal = hccCartItem.StatusComplete;
                        else
                            retVal = hccCartItem.StatusIncomplete;
                    }
                }
                else
                {
                    if (CartItem.ItemType == Enums.CartItemType.AlaCarte)
                    {
                        if (_cartItems.Any(ci => ci.SimpleStatus != hccCartItem.StatusComplete))
                            retVal = hccCartItem.StatusIncomplete;
                        else
                            retVal = hccCartItem.StatusComplete;
                    }
                    else
                        retVal = CartItem.SimpleStatus;
                }

                if (CartItem.IsCancelled)
                    retVal = hccCartItem.StatusCancelled;

                return retVal;
            }
        }

        public AggrCartItem()
        {
            _cartItems = new List<hccCartItem>();
        }

        public AggrCartItem(hccCartItem cartItem, bool includeSnapshot)
            : this()
        {
            try
            {
                CartItem = cartItem;

                if (includeSnapshot)
                    CartSnap = hccCartSnapshot.GetBy(cartItem.CartID);

                CartItemId = cartItem.CartItemID;
                SimpleName = cartItem.SimpleName;
                DeliveryDate = cartItem.DeliveryDate;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AggrCartItem(hccCartItemCalendar cartCalendar, bool includeSnapshot)
            : this()
        {
            try
            {
                CartItem = hccCartItem.GetById(cartCalendar.CartItemID);

                if (includeSnapshot)
                    CartSnap = hccCartSnapshot.GetBy(CartItem.CartID);

                CartItemId = CartItem.CartItemID;
                TotalQuantity = CartItem.Quantity;

                var t = GetNameAndDateAndMenuItems(CartItem, cartCalendar);

                if (t != null)
                {
                    SimpleName = t.Item1;
                    DeliveryDate = t.Item2;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void AddCartItem(hccCartItem cartItem)
        {
            if (cartItem != null && !_cartItems.Any(ci => ci.CartItemID == cartItem.CartItemID))
                _cartItems.Add(cartItem);
        }

        public Tuple<string, DateTime> GetNameAndDateAndMenuItems(hccCartItem cartItem, hccCartItemCalendar cartCalendar) //, out List<hccMenuItem> menuItems)
        {
            string newName = cartItem.SimpleName;
            DateTime? delDate = null;
            Tuple<string, DateTime> retVal = null;
            try
            {
                hccProductionCalendar prodCal = hccProductionCalendar.GetById(cartCalendar.CalendarID);

                if (prodCal != null)
                {
                    hccProgramPlan plan = hccProgramPlan.GetById(cartItem.Plan_PlanID.Value);
                    List<hccCartItemCalendar> cartCals = hccCartItemCalendar.GetByCartItemID(cartItem.CartItemID);
                    hccCartItemCalendar cartCal = cartCals.Where(a => a.CalendarID == prodCal.CalendarID).SingleOrDefault();

                    if (cartCal != null)
                    {
                        int cartCalIndex = cartCals.IndexOf(cartCal);

                        newName += " - Week: " + (cartCalIndex + 1).ToString();
                        delDate = prodCal.DeliveryDate;
                    }
                }
            }
            catch { throw; }

            if (delDate.HasValue)
                retVal = new Tuple<string, DateTime>(newName, delDate.Value);

            return retVal;
        }

        public static List<AggrCartItem> GetFromRange(List<hccCartItem> cartItems, bool includeSnapshots)
        {
            List<AggrCartItem> aggrCartItems = new List<AggrCartItem>();

            foreach (hccCartItem cartItem in cartItems)
            {
               
                    if (cartItem.ItemType == Common.Enums.CartItemType.GiftCard)
                    {
                        AggrCartItem ag = aggrCartItems.SingleOrDefault(a => a.CartItem.OrderNumber == cartItem.OrderNumber);

                        if (ag == null)
                        {
                            ag = new AggrCartItem(cartItem, includeSnapshots);
                            aggrCartItems.Add(ag);
                        }

                        ag.TotalQuantity += cartItem.Quantity;
                    }

                    if (cartItem.ItemType == Common.Enums.CartItemType.AlaCarte)
                    {
                        AggrCartItem ag;

                        if (!aggrCartItems.Any(a => a.CartItem.OrderNumber == cartItem.OrderNumber))
                            ag = aggrCartItems.SingleOrDefault(a => a.CartItem.OrderNumber == cartItem.OrderNumber);
                        else
                            ag = aggrCartItems.SingleOrDefault(a => a.CartItem.OrderNumber == cartItem.OrderNumber);

                        if (ag == null)
                        {
                            ag = new AggrCartItem(cartItem, includeSnapshots);
                            aggrCartItems.Add(ag);
                        }
                        else
                        {
                            ag.AddCartItem(cartItem);
                        }

                        ag.ALC_Count++;
                        ag.TotalQuantity += cartItem.Quantity;
                    }
               
            }

            return aggrCartItems;
        }

        public static List<AggrCartItem> GetFromRange(List<hccCartItemCalendar> cartCalendars, bool includeSnapshots)
        {
            List<AggrCartItem> aggrCartItems = new List<AggrCartItem>();

            foreach (hccCartItemCalendar cartCal in cartCalendars)
            {
                aggrCartItems.Add(new AggrCartItem(cartCal, includeSnapshots));
            }

            return aggrCartItems;
        }
    }
}
