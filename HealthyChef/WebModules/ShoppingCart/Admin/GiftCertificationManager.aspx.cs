using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.Common.Events;
using HealthyChef.DAL;
using HealthyChef.WebModules.ShoppingCart.Admin.UserControls;

namespace HealthyChef.WebModules.ShoppingCart.Admin
{
    public partial class GiftCertificationManager : System.Web.UI.Page
    {
        decimal IssuedTotal { get; set; }
        DateTime? StartDate { get; set; }
        DateTime? EndDate { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //btnAddNewCert.Click += new EventHandler(btnAddNewCert_Click);

            //gvwIssuedCerts.SelectedIndexChanged += new EventHandler(gvwCert_SelectedIndexChanged);
            //gvwIssuedCerts.PageIndexChanging += new GridViewPageEventHandler(gvwIssuedCerts_PageIndexChanging);
            //gvwIssuedCerts.RowDataBound += new GridViewRowEventHandler(gvwIssuedCerts_RowDataBound);

            //gvwRedeemedCerts.SelectedIndexChanged += new EventHandler(gvwCert_SelectedIndexChanged);
            //gvwRedeemedCerts.PageIndexChanging += new GridViewPageEventHandler(gvwRedeemedCerts_PageIndexChanging);
            //gvwRedeemedCerts.RowDataBound += new GridViewRowEventHandler(gvwRedeemedCerts_RowDataBound);

            GiftCertificateEdit1.ControlSaved += new ControlSavedEventHandler(GiftCertificateEdit1_ControlSaved);
            GiftCertificateEdit1.ControlCancelled += new ControlCancelledEventHandler(GiftCertificateEdit1_ControlCancelled);

            //gvwImportedGiftCerts.SelectedIndexChanged += gvwImportedGiftCerts_SelectedIndexChanged;
            //gvwImportedGiftCerts.PageIndexChanging += gvwImportedGiftCerts_PageIndexChanging;
            btnSave.Click += btnSave_Click;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            string CartItemId = string.Empty;
            string ImportItemId = string.Empty;
            if (Request.QueryString.AllKeys.Contains("CartId"))
            {
                CartItemId = Request.QueryString["CartId"].ToString();
            }
            if (!string.IsNullOrEmpty(CartItemId))
            {
                CurrentCartItemId.Value = CartItemId;
                pnlSearch.Visible = false;
                divEdit.Visible = true;
                pnlGrids.Visible = false;
                GiftCertificateEdit1.CurrentCartItemID = Convert.ToInt32(CartItemId);
                GiftCertificateEdit1.Bind();
            }
            else if (Request.QueryString.AllKeys.Contains("ImportCartId"))
            {
                ImportItemId = Request.QueryString["ImportCartId"].ToString();
                if (!string.IsNullOrEmpty(ImportItemId))
                {
                    int certId = Convert.ToInt32(ImportItemId);
                    ImportedGiftCert cert = ImportedGiftCert.GetById(certId);

                    if (cert != null)
                    {
                        CurrentCartItemId.Value = Convert.ToString(certId);
                        lblCode.Text = cert.code.ToString();
                        txtAmount.Text = cert.amount.Value.ToString();
                        txtDateUsed.Text = cert.date_used;
                        chkRedeemed.Checked = cert.is_used == "Y" ? true : false;
                        //txtUsedBy.Text = cert.used_by.ToString();

                        pnlImpCertEdit.Visible = true;
                        
                    }
                    else
                    {
                        lblFBImpCert.Text = "Gift certificate not found.";
                    }

                }
            }

            else if (!IsPostBack)
            {
                //BindgvwIssuedCerts();
                //BindgvwRedeemedCerts();
                //BindgvwImportedGiftCerts();
            }
        }

        protected void btnAddNewCert_Click(object sender, EventArgs e)
        {
            //btnAddNewCert.Visible = false;
            divEdit.Visible = true;
            pnlGrids.Visible = false;

            GiftCertificateEdit1.Bind();
        }

        void BindgvwIssuedCerts()
        {
            var giftCerts = hccCartItem.GetGiftsByRedeemed(false);
            IssuedTotal = giftCerts.Sum(a => a.ItemPrice);

            if (StartDate.HasValue)
                giftCerts = giftCerts.Where(a => a.Gift_IssuedDate >= StartDate.Value.Date).ToList();

            if (EndDate.HasValue)
                giftCerts = giftCerts.Where(a => a.Gift_IssuedDate <= EndDate.Value.Date.AddDays(1)).ToList();

            //gvwIssuedCerts.DataSource = giftCerts;
            //gvwIssuedCerts.DataBind();
        }

        void gvwCert_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView gvw = (GridView)sender;

            int cartItemID = int.Parse(gvw.SelectedDataKey.Value.ToString());

            GiftCertificateEdit1.CurrentCartItemID = cartItemID;
            GiftCertificateEdit1.Bind();

            pnlSearch.Visible = false;
            divEdit.Visible = true;
            pnlGrids.Visible = false;

        }

        void gvwIssuedCerts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvwIssuedCerts.PageIndex = e.NewPageIndex;
            BindgvwIssuedCerts();
        }

        void gvwIssuedCerts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(e.Row.Cells[2].Text))
                    {
                        Guid uid = Guid.Parse(e.Row.Cells[2].Text);
                        hccUserProfile user = hccUserProfile.GetParentProfileBy(uid);

                        if (user != null)
                            e.Row.Cells[2].Text = user.FullName;
                    }
                }
                catch (Exception)
                {
                }

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Style.Add("text-align", "right");
                e.Row.Cells[0].Text = "Total:";
                e.Row.Cells[1].Text = IssuedTotal.ToString("c");
            }
        }

        void BindgvwRedeemedCerts()
        {
            var giftCerts = hccCartItem.GetGiftsByRedeemed(true);

            if (StartDate.HasValue)
                giftCerts = giftCerts.Where(a => a.Gift_IssuedDate >= StartDate.Value.Date).ToList();

            if (EndDate.HasValue)
                giftCerts = giftCerts.Where(a => a.Gift_IssuedDate <= EndDate.Value.Date.AddDays(1)).ToList();

            //gvwRedeemedCerts.DataSource = giftCerts;
            //gvwRedeemedCerts.DataBind();
        }

        void gvwRedeemedCerts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvwRedeemedCerts.PageIndex = e.NewPageIndex;
            BindgvwRedeemedCerts();
        }

        void gvwRedeemedCerts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(e.Row.Cells[2].Text.Trim()))
                    {
                        Guid iid = Guid.Parse(e.Row.Cells[2].Text);
                        hccUserProfile iuser = hccUserProfile.GetParentProfileBy(iid);

                        if (iuser != null)
                            e.Row.Cells[2].Text = iuser.FullName;
                    }
                }
                catch (Exception)
                {
                }

                try
                {
                    if (!string.IsNullOrWhiteSpace(e.Row.Cells[4].Text.Trim()))
                    {
                        Guid rid = Guid.Parse(e.Row.Cells[4].Text);
                        hccUserProfile ruser = hccUserProfile.GetParentProfileBy(rid);

                        if (ruser != null)
                            e.Row.Cells[4].Text = ruser.FullName;
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        protected void GiftCertificateEdit1_ControlSaved(object sender, ControlSavedEventArgs e)
        {
            GiftCertificateEdit1.Clear();

            BindgvwIssuedCerts();
            BindgvwRedeemedCerts();

            pnlSearch.Visible = true;
            divEdit.Visible = false;
            pnlGrids.Visible = true;

            lblFeedback.Text = string.Format("Gift Certificate saved: {0}", DateTime.Now);
        }

        protected void GiftCertificateEdit1_ControlCancelled(object sender)
        {
            GiftCertificateEdit1.Clear();

            pnlSearch.Visible = true;
            divEdit.Visible = false;
            pnlGrids.Visible = true;
        }

        protected void btnFindGC_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtStartDate.Text))
                StartDate = DateTime.Parse(txtStartDate.Text.Trim());

            if (!string.IsNullOrWhiteSpace(txtEndDate.Text))
                EndDate = DateTime.Parse(txtEndDate.Text.Trim());

            BindgvwIssuedCerts();
            BindgvwRedeemedCerts();
        }

        void BindgvwImportedGiftCerts()
        {
            //gvwImportedGiftCerts.DataSource = ImportedGiftCert.GetActive();
            //gvwImportedGiftCerts.DataBind();
        }

        void gvwImportedGiftCerts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvwImportedGiftCerts.PageIndex = e.NewPageIndex;
            BindgvwImportedGiftCerts();
        }

        void gvwImportedGiftCerts_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView gvw = (GridView)sender;

            int certId = int.Parse(gvw.SelectedDataKey.Value.ToString());
            ImportedGiftCert cert = ImportedGiftCert.GetById(certId);

            if (cert != null)
            {
                lblCode.Text = cert.code.ToString();
                txtAmount.Text = cert.amount.Value.ToString();
                txtDateUsed.Text = cert.date_used;
                chkRedeemed.Checked = cert.is_used == "Y" ? true : false;
                //txtUsedBy.Text = cert.used_by.ToString();

                pnlImpCertEdit.Visible = true;
            }
            else
            {
                lblFBImpCert.Text = "Gift certificate not found.";
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            ImportedGiftCert cert = ImportedGiftCert.GetBy(lblCode.Text);

            if (cert != null)
            {
                cert.is_used = chkRedeemed.Checked ? "Y" : "N";
                cert.code = long.Parse(lblCode.Text.Trim());
                cert.amount = decimal.Parse(txtAmount.Text.Trim());
                cert.date_used = txtDateUsed.Text;
                //cert.used_by = txtUsedBy.Text;
                cert.Save();

                lblFBImpCert.Text = "Gift certificate has been saved.";
            }
            else
            {
                lblFBImpCert.Text = "Gift certificate not found.";
            }

            pnlImpCertEdit.Visible = false;
        }

    }
}