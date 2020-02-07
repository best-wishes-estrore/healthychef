using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.Common.Events;
using HealthyChef.DAL;
using HealthyChef.WebModules.ShoppingCart.Controls.Cart;
using System.Web.Security;
using HealthyChef.DAL.Extensions;
using System.Web.UI.HtmlControls;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public partial class CartItemList : System.Web.UI.UserControl
    {
        public event CartItemListItemUpdatedEventHandler CartItemListItemUpdated;

        protected void OnCartItemListItemUpdated()
        {
            if (CartItemListItemUpdated != null)
                CartItemListItemUpdated();
        }

        public ProfileCart CurrentProfileCart
        {
            get
            {
                return (ProfileCart)ViewState["CurrentProfileCart"];
            }
            set
            {
                ViewState["CurrentProfileCart"] = value;
            }
        }

        public bool DisplayHeader
        {
            get
            {
                if (ViewState["DisplayHeader"] == null)
                    ViewState["DisplayHeader"] = false;

                return bool.Parse(ViewState["DisplayHeader"].ToString());
            }
            set
            {
                ViewState["DisplayHeader"] = value;
            }
        }

        //Binding Cart Items
        public void Bind()
        {
            trHeader.Visible = DisplayHeader;
            ShowAllTotals();

            if (CurrentProfileCart.ShippingAddressId > 0 && lblShippingMethod != null)
            {
                Enums.DeliveryTypes delType = (Enums.DeliveryTypes)CurrentProfileCart.ShippingAddress.DefaultShippingTypeID;

                if (delType == Enums.DeliveryTypes.Delivery || delType == Enums.DeliveryTypes.LocalDelivery)
                    lblShippingMethod.Text = "Delivery";
                else
                    lblShippingMethod.Text = "Pick-Up";
            }

            List<string> profileNameLists = new List<string>();

            CurrentProfileCart.CartItems.ForEach(delegate (hccCartItem item)
            {
                if (item.UserProfile != null && !profileNameLists.Contains(item.UserProfile.ProfileName))
                    profileNameLists.Add(item.UserProfile.ProfileName);
            });


            string profileNames = string.Empty;
            profileNameLists.ForEach(a => profileNames += a + ", ");
            profileNames = profileNames.Trim().TrimEnd(new char[] { ',' });

            lblUserProfile.Text = profileNames + " : " + CurrentProfileCart.CartItems[0].DeliveryDate.ToShortDateString();

            lblProfileSubTotal.Text = this.CurrentProfileCart.MockProfileSubTotal.ToString("c");
            lblProfileDiscount.Text = this.CurrentProfileCart.SubDiscountAmount.ToString("c");
           // lblProfileSubTotalAdj.Text = (this.CurrentProfileCart.SubTotalNA - this.CurrentProfileCart.SubDiscountAmount).ToString("c");
            lblProfileTaxSubTotal.Text = CurrentProfileCart.SubTax.ToString("c");
            lblProfileShipActTotal.Text = CurrentProfileCart.SubShippingActual.ToString("c");
            lblProfileShipSubTotal.Text = CurrentProfileCart.SubShipping.ToString("c");
            lblProfileGrandTotal.Text = (CurrentProfileCart.SubTotalAdj + CurrentProfileCart.SubTax + CurrentProfileCart.SubShipping).ToString("c");

            lvwCartItems.DataSource = CurrentProfileCart.CartItems.OrderBy(a => a.OrderNumber);
            lvwCartItems.DataBind();

        }

        protected void lkbRemove_Click(object sender, EventArgs e)
        {
            LinkButton lkbRemove = (LinkButton)sender;
            int cartItemId = int.Parse(lkbRemove.CommandArgument);
            hccCartItem cartItem = hccCartItem.GetById(cartItemId);

            if (cartItem != null)
            {
                hccCartItem vsItem = CurrentProfileCart.CartItems.Single(a => a.CartItemID == cartItemId);
                if (vsItem != null)
                    CurrentProfileCart.CartItems.Remove(vsItem);

                try
                {  // remove any potential recurring status to prevent orphaning records without a valid cartId and cartitemId 
                    using (var hcE = new healthychefEntities())
                    {
                        var rOrder = hcE.hccRecurringOrders.FirstOrDefault(i => i.CartID == cartItem.CartID && i.CartItemID == cartItemId);

                        if (rOrder != null)
                        {
                            hcE.hccRecurringOrders.DeleteObject(rOrder);
                            hcE.SaveChanges();
                        }
                    }
                }
                catch (Exception) { }


                cartItem.Delete(((List<hccRecurringOrder>)Session["autorenew"]));
                OnCartItemListItemUpdated();
            }
            else
            {
                // couldnt find cart item
            }
        }

        protected void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            Page.Validate("QuantityGroup");

            if (Page.IsValid)
            {
                TextBox txtQuantity = (TextBox)sender;
                //CheckBox chk = (CheckBox)sender;
                if (txtQuantity != null)
                {
                    int newQty = int.Parse(txtQuantity.Text);
                    ListViewDataItem item = (ListViewDataItem)txtQuantity.Parent;

                    int cartItemId = int.Parse(lvwCartItems.DataKeys[item.DataItemIndex].Value.ToString());
                    hccCartItem cartItem = hccCartItem.GetById(cartItemId);

                    if (cartItem != null)
                    {
                        if (cartItem.ItemTypeID == 2)
                        {
                            if (newQty > 0)
                            {
                                List<hccCartItem> cartItems = hccCartItem.GetWithoutSideItemsBy(cartItem.CartID);

                                if (cartItem.AdjustQuantity(newQty))
                                {
                                    #region recurringitems adjust
                                    // If the adjusted cart item is a recurring item(s), 
                                    // check the quantity of the cartitemId in the Session 
                                    // Recurring order object, if greater than new quantity, 
                                    // adjust the recurring values accordingly.
                                    //if (Session["autorenew"] != null)
                                    //{
                                    //    var lstRo = ((List<hccRecurringOrder>)Session["autorenew"]);
                                    //    while (lstRo.Count(x => x.CartItemID == cartItemId) > newQty)
                                    //    {
                                    //        lstRo.Remove(lstRo.Find(x => x.CartItemID == cartItemId));
                                    //    }
                                    //}
                                    #endregion

                                    OnCartItemListItemUpdated();
                                }

                            }
                            else
                            {
                                cartItem.Delete(((List<hccRecurringOrder>)Session["autorenew"]));
                                OnCartItemListItemUpdated();
                            }
                        }
                        else
                        {
                            if (newQty == 1)
                            {
                                cartItem.AdjustQuantity(newQty);
                                hccCartItem hcccartItem = new hccCartItem();
                                hcccartItem.RemoveFamilyStyle(cartItemId);
                                OnCartItemListItemUpdated();
                            }
                            else
                            {
                                cartItem.AdjustQuantity(newQty);
                                OnCartItemListItemUpdated();
                            }
                        }
                    }
                }
            }
        }

        void ShowAllTotals()
        {
            AuthNet.AuthNetConfig config = new AuthNet.AuthNetConfig();

            if (config.Settings.TestMode)
            {
                trProfDiscount.Visible = true;
                //trProfDiscountSubTotal.Visible = true;
                trProfTaxTotal.Visible = true;
                trProfActShipTotal.Visible = true;
                trProfShipTotal.Visible = true;
                trProfTotal.Visible = true;
            }
        }

        protected void ChkAutoRenew_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chbActive = (CheckBox)sender;
                ListViewDataItem item = (ListViewDataItem)chbActive.Parent;
                TextBox txtQuantity = lvwCartItems.Items[item.DataItemIndex].FindControl("txtQuantity") as TextBox;
                
                if (chbActive.Text != null)
                {
                    int cartitemid = Convert.ToInt32(chbActive.Text);
                    hccCartItem hcccartItem = hccCartItem.GetById(cartitemid);
                    hccRecurringOrder IsExistingrecurringorder = null;
                    if (hcccartItem.ItemTypeID == 2)
                    {
                        if (hcccartItem != null)
                        {
                            IsExistingrecurringorder = hccRecurringOrder.GetByCartItemId(hcccartItem.CartItemID);
                        }
                        else
                        {
                            IsExistingrecurringorder = null;
                        }

                        if (IsExistingrecurringorder != null)
                        {
                            IsExistingrecurringorder.Delete();
                            if (hcccartItem != null)
                            {
                                hcccartItem.Plan_IsAutoRenew = false;
                                hcccartItem.DiscountAdjPrice = Convert.ToDecimal("0.00");
                                hcccartItem.DiscountPerEach = Convert.ToDecimal("0.00");
                                hcccartItem.Save();
                            }
                            lblfeedback.Visible = true;
                            lblfeedback.Text = "Auto renew Item Deleted Successfully";
                        }
                        else
                        {
                            hccCart hccCart = hccCart.GetById(hcccartItem.CartID);
                            if (hccCart != null)
                            {

                                hccRecurringOrder hccrecurringOrder = new hccRecurringOrder
                                {
                                    CartID = hcccartItem.CartID,
                                    CartItemID = hcccartItem.CartItemID,
                                    UserProfileID = hcccartItem.UserProfileID,
                                    AspNetUserID = hccCart.AspNetUserID,
                                    PurchaseNumber = hccCart.PurchaseNumber,
                                    TotalAmount = Math.Round(Convert.ToDecimal(Convert.ToDouble(hcccartItem.ItemPrice) - Convert.ToDouble(hcccartItem.ItemPrice) * 0.05), 2)
                                };
                                hccrecurringOrder.Save();

                                if (hcccartItem != null)
                                {
                                    hcccartItem.Plan_IsAutoRenew = true;
                                    hcccartItem.DiscountAdjPrice = Convert.ToDecimal(hcccartItem.ItemPrice);
                                    hcccartItem.DiscountPerEach = Convert.ToDecimal(Convert.ToDouble(hcccartItem.ItemPrice) * 0.05);
                                    // lblProfileSubTotalAdj.Text = (this.CurrentProfileCart.SubTotalNA - (this.CurrentProfileCart.SubDiscountAmount+ (hcccartItem.DiscountPerEach))).ToString("c");
                                    hcccartItem.Save();
                                }
                                lblfeedback.Visible = true;
                                lblfeedback.Text = "Auto renew Item Created Successfully";
                            }
                        }
                    }
                    else
                    {
                        if (chbActive.Checked == true && Convert.ToInt32(txtQuantity.Text) <= 1)
                        {
                            chbActive.Checked = false;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Family style requires atleast 2 servings. Please increase the quantity to apply Family Style')", true);
                        }
                        else
                        {
                            if (hcccartItem != null)
                            {
                                if (chbActive.Checked == true)
                                {
                                    hcccartItem.Plan_IsAutoRenew = true;
                                    hcccartItem.DiscountAdjPrice = Convert.ToDecimal(hcccartItem.ItemPrice);
                                    hcccartItem.DiscountPerEach = Math.Round(Convert.ToDecimal(Convert.ToDouble(hcccartItem.ItemPrice) * 0.1), 2);
                                }
                                else
                                {
                                    hcccartItem.Plan_IsAutoRenew = false;
                                    hcccartItem.DiscountAdjPrice = Convert.ToDecimal("0.00");
                                    hcccartItem.DiscountPerEach = Convert.ToDecimal("0.00");
                                }
                                hcccartItem.Save();
                            }
                        }
                    }
                }
                else
                {
                    lblfeedback.Visible = true;
                    lblfeedback.Text = "There is no records found";

                }
                OnCartItemListItemUpdated();
                //Page.Response.Redirect(Page.Request.Url.ToString() + "#tabs9", true);

            }
            catch (Exception E)
            {
                lblfeedback.Visible = true;
                lblfeedback.Text = "Error in deleting cart item " + E.Message;
            }
        }
    }
}