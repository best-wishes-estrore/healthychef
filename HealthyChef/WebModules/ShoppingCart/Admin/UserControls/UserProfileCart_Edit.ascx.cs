using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.Common.Events;
using HealthyChef.DAL;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public partial class UserProfileCart_Edit : FormControlBase
    {   // this.PrimaryKeyIndex as hccCart.CartId
        hccCart CurrentCart { get; set; }

        public event CartSavedEventHandler CartSaved;
        public void OnCartSaved(CartEventArgs e)
        {
            if (CartSaved != null)
                CartSaved(this, e);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
                        
            btnAddNewItem.Click += btnAddNewItem_Click;
            rblItemType.SelectedIndexChanged += rblItemType_SelectedIndexChanged;

            GiftCertEdit1.ControlSaved += GiftCertEdit1_ControlSaved;
            GiftCertEdit1.ControlCancelled += GiftCertEdit1_ControlCancelled;
            MenuItemAddToCart1.ControlCancelled += MenuItemAddToCart1_ControlCancelled;
            MenuItemAddToCart1.ControlSaved += MenuItemAddToCart1_ControlSaved;
            ProgramPlanAddToCart1.ControlCancelled += ProgramPlanAddToCart1_ControlCancelled;
            ProgramPlanAddToCart1.ControlSaved += ProgramPlanAddToCart1_ControlSaved;

            CartDisplay1.CartSaved += CartDisplay1_CartSaved;          
        }
               
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        protected override void LoadForm()
        {
            try
            {
                if (this.PrimaryKeyIndex > 0 && CurrentCart == null)
                {
                    hccUserProfile profile = hccUserProfile.GetById(this.PrimaryKeyIndex);
                    CurrentCart = hccCart.GetCurrentCart(profile.ASPUser);
                }

                if (CurrentCart != null)
                {
                    CartDisplay1.CurrentCartId = CurrentCart.CartID;
                    CartDisplay1.Bind();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override void SaveForm()
        {
            throw new NotImplementedException();
        }

        protected override void ClearForm()
        {
            rblItemType.ClearSelection();           
        }

        protected override void SetButtons()
        {
            throw new NotImplementedException();
        }

        void btnAddNewItem_Click(object sender, EventArgs e)
        {
            btnAddNewItem.Visible = false;
            pnlAddCartItem.Visible = true;
        }
        
        void rblItemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            MenuItemAddToCart1.Clear();
            ProgramPlanAddToCart1.Clear();
            GiftCertEdit1.Clear();

            if (this.PrimaryKeyIndex > 0 && CurrentCart == null)
            {
                hccUserProfile profile = hccUserProfile.GetById(this.PrimaryKeyIndex);
                CurrentCart = hccCart.GetCurrentCart(profile.ASPUser);
            }

            if (CurrentCart != null)
            {
                switch (rblItemType.SelectedValue)
                {
                    case "1":
                        pnlAlaCarte.Visible = true;
                        pnlProgramPlan.Visible = false;
                        pnlGiftCard.Visible = false;
                        MenuItemAddToCart1.PrimaryKeyIndex = CurrentCart.CartID;
                        MenuItemAddToCart1.Bind();
                        break;
                    case "2":
                        pnlAlaCarte.Visible = false;
                        pnlProgramPlan.Visible = true;
                        pnlGiftCard.Visible = false;
                        ProgramPlanAddToCart1.PrimaryKeyIndex = CurrentCart.CartID;
                        ProgramPlanAddToCart1.Bind();
                        break;
                    case "3":
                        pnlAlaCarte.Visible = false;
                        pnlProgramPlan.Visible = false;
                        pnlGiftCard.Visible = true;
                        GiftCertEdit1.PrimaryKeyIndex = CurrentCart.CartID;
                        GiftCertEdit1.Bind();
                        break;
                    default: break;
                }
            }
        }

        void GiftCertEdit1_ControlSaved(object sender, ControlSavedEventArgs e)
        {
            GiftCertEdit1.Clear();
            pnlAlaCarte.Visible = false;
            pnlProgramPlan.Visible = false;
            pnlGiftCard.Visible = false;

            btnAddNewItem.Visible = true;
            pnlAddCartItem.Visible = false;

            this.Clear();
            this.Bind();
            lblFeedback.Text = "Gift Certificate added to cart: " + DateTime.Now;
        }

        void GiftCertEdit1_ControlCancelled(object sender)
        {
            this.Clear();

            GiftCertEdit1.Clear();
            pnlAlaCarte.Visible = false;
            pnlProgramPlan.Visible = false;
            pnlGiftCard.Visible = false;

            btnAddNewItem.Visible = true;
            pnlAddCartItem.Visible = false;
        }

        void MenuItemAddToCart1_ControlSaved(object sender, ControlSavedEventArgs e)
        {
            MenuItemAddToCart1.Clear();
            pnlAlaCarte.Visible = false;
            pnlProgramPlan.Visible = false;
            pnlGiftCard.Visible = false;

            btnAddNewItem.Visible = true;
            pnlAddCartItem.Visible = false;

            this.Clear();
            this.Bind();
            lblFeedback.Text = "Menu Item added to cart: " + DateTime.Now;
        }

        void MenuItemAddToCart1_ControlCancelled(object sender)
        {
            this.Clear();

            MenuItemAddToCart1.Clear();
            pnlAlaCarte.Visible = false;
            pnlProgramPlan.Visible = false;
            pnlGiftCard.Visible = false;

            btnAddNewItem.Visible = true;
            pnlAddCartItem.Visible = false;
        }

        void ProgramPlanAddToCart1_ControlSaved(object sender, ControlSavedEventArgs e)
        {
            ProgramPlanAddToCart1.Clear();
            pnlAlaCarte.Visible = false;
            pnlProgramPlan.Visible = false;
            pnlGiftCard.Visible = false;

            btnAddNewItem.Visible = true;
            pnlAddCartItem.Visible = false;

            this.Clear();
            this.Bind();
            lblFeedback.Text = "Program Plan added to cart: " + DateTime.Now;
        }

        void ProgramPlanAddToCart1_ControlCancelled(object sender)
        {
            this.Clear();

            ProgramPlanAddToCart1.Clear();
            pnlAlaCarte.Visible = false;
            pnlProgramPlan.Visible = false;
            pnlGiftCard.Visible = false;

            btnAddNewItem.Visible = true;
            pnlAddCartItem.Visible = false;
        }

        void CartDisplay1_CartSaved(object sender, CartEventArgs e)
        {
            this.OnCartSaved(e);
        }
    }
}