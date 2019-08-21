using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.DAL;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public partial class OrderFulfillment_Search : FormControlBase
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //lvwOrders.ItemDataBound += lvwOrders_ItemDataBound;

            //btnSearch.Click += btnSearch_Click;
            //btnClear.Click += btnClear_Click;
            btnCreateShippingFile.Click += btnCreateShippingFile_Click;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindddlDelDates();

                //Bind();
            }
        }

        protected override void LoadForm()
        {
            BindlvwOrders();
        }

        protected override void SaveForm()
        {
            throw new NotImplementedException();
        }

        protected override void ClearForm()
        {
            txtSearchLastName.Text = string.Empty;
            txtSearchEmail.Text = string.Empty;
            txtSearchPurchNum.Text = string.Empty;
            BindddlDelDates();
            ddlStatus.ClearSelection();
            ddlTypes.ClearSelection();
        }

        protected override void SetButtons()
        {
            throw new NotImplementedException();
        }

        void BindddlDelDates()
        {
            if (ddlDelDates.Items.Count == 0)
            {
                ddlDelDates.DataSource = hccProductionCalendar.GetAll();
                ddlDelDates.DataTextField = "DeliveryDate";
                ddlDelDates.DataTextFormatString = "{0:MM-dd-yyyy}";
                ddlDelDates.DataValueField = "CalendarId";
                ddlDelDates.DataBind();

                ddlDelDates.Items.Insert(0, new ListItem("All", "-1"));
            }

            if (Request.QueryString["cal"] != null)
            {
                ddlDelDates.SelectedIndex = ddlDelDates.Items.IndexOf(ddlDelDates.Items.FindByValue(Request.QueryString["cal"]));
            }
            else
                ddlDelDates.SelectedIndex = ddlDelDates.Items.IndexOf(ddlDelDates.Items.FindByValue(hccProductionCalendar.GetNext4Last2Calendars()[1].CalendarID.ToString()));

        }

        void BindlvwOrders()
        {
            string lastName = null;
            if (!string.IsNullOrWhiteSpace(txtSearchLastName.Text.Trim())) { lastName = txtSearchLastName.Text.Trim(); }

            string email = null;
            if (!string.IsNullOrWhiteSpace(txtSearchEmail.Text.Trim())) { email = txtSearchEmail.Text.Trim(); }

            int? purchNum = null;
            if (!string.IsNullOrWhiteSpace(txtSearchPurchNum.Text.Trim())) { purchNum = int.Parse(txtSearchPurchNum.Text.Trim()); }

            DateTime? delDate = null;
            if (ddlDelDates.SelectedIndex > 0) { delDate = DateTime.Parse(ddlDelDates.SelectedItem.Text.Trim()); }

            List<AggrCartItem> agItems = hccCartItem.Search(lastName, email, purchNum, delDate, false, true);

            if (ddlStatus.SelectedIndex != 0)
            {
                agItems = agItems.Where(a => a.SimpleStatus == ddlStatus.SelectedItem.Text).ToList();
            }

            if (ddlTypes.SelectedIndex != 0)
            {
                agItems = agItems.Where(a => a.CartItem.ItemType == ((Enums.CartItemType)(int.Parse(ddlTypes.SelectedValue)))).ToList();
            }

            //lvwOrders.DataSource = agItems.OrderBy(a => a.CartItem.OrderNumber).ThenBy(a => a.DeliveryDate);
            //lvwOrders.DataBind();
        }

        void lvwOrders_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                try
                {
                    AggrCartItem aggrItem = (AggrCartItem)e.Item.DataItem;

                    if (aggrItem != null)
                    {
                        string qs = "?ci=" + aggrItem.CartItem.CartItemID + "&dd=" + aggrItem.DeliveryDate.ToShortDateString();

                        if (aggrItem.CartItem.ItemType == Enums.CartItemType.AlaCarte)
                        {
                            if (aggrItem.ALC_Count > 1)
                                ((Label)e.Item.FindControl("lblSimpleName")).Text += " (" + aggrItem.ALC_Count + ")";

                            qs += "&it=" + ((int)Enums.CartItemType.AlaCarte).ToString();
                        }
                        else if (aggrItem.CartItem.ItemType == Enums.CartItemType.DefinedPlan)
                        {
                            PlaceHolder plcAllergies = (PlaceHolder)e.Item.FindControl("plcAllergies");
                            if (plcAllergies != null)
                            {
                                Label lblAllg = new Label();

                                if (aggrItem.CartItem.ItemType == Enums.CartItemType.DefinedPlan)
                                {
                                    hccCartItemCalendar cartCal = hccCartItemCalendar.GetBy(aggrItem.CartItem.CartItemID, aggrItem.DeliveryDate);
                                    if (aggrItem.CartItem.GetDaysWithAllergens(cartCal.CartCalendarID).Count > 0)
                                    {
                                        lblAllg.Text = "Alert";
                                    }
                                    else
                                    {
                                        lblAllg.Text = "Ok";
                                    }
                                }
                                plcAllergies.Controls.Add(lblAllg);
                            }

                            LinkButton lkbPostpone = (LinkButton)e.Item.FindControl("lkbPostpone");
                            if (lkbPostpone != null)
                            {
                                lkbPostpone.Attributes.Add("onclick", "if (confirm('Are you sure that you want to postpone this delivery? This cannot be undone.')) {return confirm('Are you really sure that you want to postpone this delivery? Seriously, postponements cannot be undone.');} else {return false;}");
                                if (aggrItem.CartItem.ItemType == Enums.CartItemType.DefinedPlan)
                                    lkbPostpone.Visible = true;
                            }

                            qs += "&it=" + ((int)Enums.CartItemType.DefinedPlan).ToString();
                        }
                        else if (aggrItem.CartItem.ItemType == Enums.CartItemType.GiftCard)
                        {
                            qs += "&it=" + ((int)Enums.CartItemType.GiftCard).ToString();
                        }

                        PlaceHolder plcDetails = (PlaceHolder)e.Item.FindControl("plcDetails");
                        if (plcDetails != null)
                        {
                            Label lblDetails = new Label();
                            lblDetails.Text = "<a href='OrderFulfillmentEditor.aspx" + qs + "'>Details</a>";
                            plcDetails.Controls.Add(lblDetails);
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        protected void lkbPostpone_Click(object sender, EventArgs e)
        {
            ListViewDataItem lvwItem = (ListViewDataItem)((LinkButton)sender).Parent;

            //int cartItemId = int.Parse(lvwOrders.DataKeys[lvwOrders.Items.IndexOf(lvwItem)].Values[0].ToString());
            //DateTime delDate = DateTime.Parse(lvwOrders.DataKeys[lvwOrders.Items.IndexOf(lvwItem)].Values[1].ToString());

            //hccCartItemCalendar oldCartCal = hccCartItemCalendar.GetBy(cartItemId, delDate);

            //if (oldCartCal != null)
            //{
            //    // remove any pre-defined program exceptions, as they are tied to a specific defaultmenu, re: delivery date
            //    List<hccCartDefaultMenuException> defExs = hccCartDefaultMenuException.GetBy(oldCartCal.CartCalendarID);
            //    defExs.ForEach(delegate(hccCartDefaultMenuException defEx)
            //    {
            //        List<hccCartDefaultMenuExPref> prefs = hccCartDefaultMenuExPref.GetBy(defEx.DefaultMenuExceptID);
            //        prefs.ForEach(a => a.Delete());
            //        defEx.Delete();
            //    });

            //  var otherCals = hccCartItemCalendar.GetByCartItemID(cartItemId);
            //var lastCal = otherCals.Last();

            //hccCartItemCalendar newCartCal = lastCal.GetNextCartCalendar(DayOfWeek.Friday);

            //if (newCartCal != null)
            //oldCartCal.Delete();

            BindlvwOrders();
        }


        //void btnSearch_Click(object sender, EventArgs e)
        //{
        //    BindlvwOrders();
        //}

        //void btnClear_Click(object sender, EventArgs e)
        //{
        //    Clear();
        //    Bind();
        //}
        void btnCreateShippingFile_Click(object sender, EventArgs e)
        {
            string lastName = null;
            if (!string.IsNullOrWhiteSpace(txtSearchLastName.Text.Trim())) { lastName = txtSearchLastName.Text.Trim(); }

            string email = null;
            if (!string.IsNullOrWhiteSpace(txtSearchEmail.Text.Trim())) { email = txtSearchEmail.Text.Trim(); }

            int? purchNum = null;
            if (!string.IsNullOrWhiteSpace(txtSearchPurchNum.Text.Trim())) { purchNum = int.Parse(txtSearchPurchNum.Text.Trim()); }

            DateTime? delDate = null;
            if (ddlDelDates.SelectedIndex > 0) { delDate = DateTime.Parse(ddlDelDates.SelectedItem.Text.Trim()); }

            List<AggrCartItem> agItems = hccCartItem.Search(lastName, email, purchNum, delDate, false, false);

            if (ddlStatus.SelectedIndex != 0)
            {
                agItems = agItems.Where(a => a.SimpleStatus == ddlStatus.SelectedItem.Text).ToList();
            }

            if (ddlTypes.SelectedIndex != 0)
            {
                agItems = agItems.Where(a => a.CartItem.ItemType == ((Enums.CartItemType)(int.Parse(ddlTypes.SelectedValue)))).ToList();
            }


            List<int> cartItemIds = (from item in agItems select item.CartItemId).ToList();
            using (healthychefEntities hce = new healthychefEntities())
            {
                var items = (from he in hce.hccCartItems where cartItemIds.Contains(he.CartItemID) && !he.IsCancelled && !he.IsFulfilled select he);

                var orders = (from ag in
                              (from agi in items
                               orderby agi.OrderNumber, agi.DeliveryDate, agi.SnapShipAddrId, agi.UserProfileID
                               select agi)
                              join up in hce.hccUserProfiles on ag.UserProfileID equals up.UserProfileID
                              join mp in hce.aspnet_Membership on up.MembershipID equals mp.UserId
                              group ag by new
                              {
                                  OrderNumber = ag.OrderNumber,
                                  PurchaseNumber = (ag.hccCart == null ? 0 : ag.hccCart.PurchaseNumber),
                                  SnapShipAddrId = ag.SnapShipAddrId,
                                  Email = (mp == null ? "" : mp.Email),
                                  UserProfileId = ag.UserProfileID
                              }
                              into agag
                              select new { agag.Key.OrderNumber, agag.Key.SnapShipAddrId, agag.Key.PurchaseNumber, agag.Key.Email, agag.Key.UserProfileId });
                var orderDetails = (from o in orders
                                    join a in hce.hccAddresses on o.SnapShipAddrId equals a.AddressID
                                    where a != null
                                    select new ShippingLabelDetails
                                    {
                                        OrderNumber = o.OrderNumber,
                                        UserProfileId = o.UserProfileId,
                                        PurchaseNum = o.PurchaseNumber,
                                        Company = "",
                                        Contact = a.FirstName + " " + a.LastName,
                                        Address1 = (a.Address1 == null ? "" : a.Address1),
                                        Address2 = (a.Address2 == null ? "" : a.Address2),
                                        City = (a.City == null ? "" : a.City),
                                        StateProvince = (a.State == null ? "" : a.State),
                                        Country = (a.Country == null ? "" : a.Country),
                                        Zip = (a.PostalCode == null ? "" : a.PostalCode),
                                        Phone = (a.Phone == null ? "" : a.Phone),
                                        Email = (o.Email == null ? "" : o.Email)
                                    }).ToList();

                List<ShippingLabelDetails> ordersRefined = orderDetails.Distinct(new ShippingLabelDetailsComparer()).ToList();

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine("Purchase#,Company,Contact,Address1,Address2,City,State/Province,Country,Zip,Phone,Email,Delivery_Instructions");
                foreach (var detail in ordersRefined)
                {
                    string shippingNote = string.Join(";", (from n in hce.hccUserProfileNotes where n.UserProfileID == detail.UserProfileId && n.NoteTypeID == 2 select n.Note));
                    sb.AppendLine(detail.OrderNumber + ",\"" +
                        detail.Company.Replace("\"", "\"\"") + "\",\"" + detail.Contact.Replace("\"", "\"\"") + "\",\"" +
                        detail.Address1.Replace("\"", "\"\"") + "\",\"" + detail.Address2.Replace("\"", "\"\"") + "\",\"" +
                        detail.City.Replace("\"", "\"\"") + "\",\"" + detail.StateProvince.Replace("\"", "\"\"") + "\",\"" +
                        detail.Country.Replace("\"", "\"\"") + "\",\"" + detail.Zip.Replace("\"", "\"\"") + "\",\"" +
                        detail.Phone.Replace("\"", "\"\"") + "\",\"" + detail.Email.Replace("\"", "\"\"") + "\",\"" +
                        shippingNote.Replace("\"", "\"\"") + "\"");
                }
                HttpContext.Current.Response.Write(sb.ToString());
                HttpContext.Current.Response.ContentType = "text/csv";
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + "Shipping_export.csv");
                HttpContext.Current.Response.End();



            }

        }

    }
}
