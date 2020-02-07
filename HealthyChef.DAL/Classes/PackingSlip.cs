using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HealthyChef.DAL;
using HealthyChef.Common;

namespace HealthyChef.DAL
{
    public class PackingSlip
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Customer { get; set; }
        public string OrderProfile { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryDate { get; set; }
        public string DeliveryMethod { get; set; }
        public string DeliveryDay { get; set; }
        public int ItemsCount { get; set; }
        public string SpecialInstructions { get; set; }
        public string OrderNumber { get; set; }
        public string IsFamily { get; set; }
        public PackingSlip() { }

        public static List<PackingSlip> GeneratePackingSlips(DateTime deliveryDate)
        {
            List<PackingSlip> outSlips = new List<PackingSlip>();
            List<AggrCartItem> agItems = hccCartItem.Search(null, null, null, deliveryDate, true, false);

            foreach (AggrCartItem agItem in agItems)
            {
                hccCart cart = hccCart.GetBy(agItem.CartItem.OrderNumber);

                if (cart != null && (cart.Status == Enums.CartStatus.Paid || cart.Status == Enums.CartStatus.Fulfilled))
                {
                    PackingSlip existItem = outSlips.SingleOrDefault(a => a.OrderNumber == agItem.CartItem.OrderNumber);

                    if (existItem == null)
                    {
                        PackingSlip ps = new PackingSlip
                        {
                            OrderNumber = agItem.CartItem.OrderNumber,
                            DeliveryDay = agItem.DeliveryDate.DayOfWeek.ToString(),
                            DeliveryDate = agItem.DeliveryDate.ToShortDateString()
                        };

                        //if (agItem.CartItem.UserProfile.ParentProfileID.HasValue)
                        //    ps.SpecialInstructions = hccUserProfileNote.GetBy(agItem.CartItem.UserProfile.ParentProfileID.Value, Enums.UserProfileNoteTypes.ShippingNote, null)
                        //        .Select(a => a.Note).DefaultIfEmpty(string.Empty).Aggregate((b, c) => b + ", " + c);

                        string n1 = hccUserProfileNote.GetBy(agItem.CartItem.UserProfile.UserProfileID, Enums.UserProfileNoteTypes.ShippingNote, null)
                               .Select(a => a.Note).DefaultIfEmpty(string.Empty).Aggregate((b, c) => b + ", " + c);

                        if (!string.IsNullOrWhiteSpace(n1))
                            if (!string.IsNullOrWhiteSpace(ps.SpecialInstructions))
                                ps.SpecialInstructions += ", " + n1;
                            else
                                ps.SpecialInstructions += n1;

                        if (agItem.CartSnap != null)
                        {
                            ps.LastName = agItem.CartSnap.LastName;
                            ps.FirstName = agItem.CartSnap.FirstName;
                            ps.OrderProfile = agItem.CartSnap.ProfileName;
                            ps.Customer = ps.LastName + ", " + ps.FirstName;
                        }
                        else
                        {
                            ps.LastName = agItem.CartItem.UserProfile.ParentProfileName;
                            ps.Customer = ps.LastName;
                            ps.OrderProfile = agItem.CartItem.UserProfile.ProfileName;
                        }
                        if (agItem.CartItem != null)
                        {
                            if (agItem.CartItem.Plan_IsAutoRenew == true && agItem.CartItem.ItemTypeID == 1)
                            {
                                ps.IsFamily = "Yes";
                            }
                            else if (agItem.CartItem.Plan_IsAutoRenew == false && agItem.CartItem.ItemTypeID == 1)
                            {
                                ps.IsFamily = "No";
                            }
                            else
                            {
                                ps.IsFamily = "N/A";
                            }
                        }
                        if (agItem.CartItem.SnapShipAddrId.HasValue)
                        {
                            hccAddress shipAddr = hccAddress.GetById(agItem.CartItem.SnapShipAddrId.Value);
                            ps.DeliveryAddress = shipAddr.ToString();
                            ps.DeliveryAddress += shipAddr.IsBusiness ? "<b>Business Address</b>" : "<b>Residential Address</b>";
                            ps.DeliveryMethod = Enums.GetEnumDescription(((Enums.DeliveryTypes)shipAddr.DefaultShippingTypeID));
                        }

                        if (agItem.CartItem.ItemType == Enums.CartItemType.DefinedPlan)
                        {
                            hccProductionCalendar pc = hccProductionCalendar.GetBy(agItem.DeliveryDate);
                            hccProgramPlan pg = hccProgramPlan.GetById(agItem.CartItem.Plan_PlanID.Value);
                            int defMenuCount = hccProgramDefaultMenu.GetBy(pc.CalendarID, pg.ProgramID)
                                .Where(a => a.MenuItemID > 0 && a.DayNumber <= pg.NumDaysPerWeek).Count();

                            ps.ItemsCount += agItem.TotalQuantity * defMenuCount;
                        }
                        else
                        {
                            ps.ItemsCount += agItem.TotalQuantity;
                        }

                        // NEW ASSUMPTION:  No packing sheet should be printed if no cart items exist.
                        if (ps.ItemsCount > 0)
                        {
                            outSlips.Add(ps);
                        }
                    }
                    else
                    {
                        if (agItem.CartItem.ItemType == Enums.CartItemType.DefinedPlan)
                        {
                            hccProductionCalendar pc = hccProductionCalendar.GetBy(agItem.DeliveryDate);
                            hccProgramPlan pg = hccProgramPlan.GetById(agItem.CartItem.Plan_PlanID.Value);
                            int defMenuCount = hccProgramDefaultMenu.GetBy(pc.CalendarID, pg.ProgramID)
                                .Where(a => a.MenuItemID > 0 && a.DayNumber <= pg.NumDaysPerWeek).Count();

                            existItem.ItemsCount += agItem.TotalQuantity * defMenuCount;
                        }
                        else
                        {
                            existItem.ItemsCount += agItem.TotalQuantity;
                        }
                    }
                }
            }

            return outSlips.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ThenBy(a => a.OrderNumber).ToList();
        }

    }

}