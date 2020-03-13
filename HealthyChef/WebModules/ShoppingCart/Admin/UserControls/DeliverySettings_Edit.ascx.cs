using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.DAL;
using HealthyChef.DAL.Extensions;
using System.Data;
using System.IO;
using OfficeOpenXml;
using BayshoreSolutions.Common.Web.UI.WebControls;
using System.Data.SqlClient;
using OfficeOpenXml.Style;
using System.Text;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public partial class DeliverySettings_Edit : FormControlBase
    {
        protected List<hccDeliverySetting> CurrentSettings { get; set; }
        protected hccGlobalSetting GlobalSettings { get; set; }
        //public object DialogResult { get; private set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            btnSave.Click += new EventHandler(base.SubmitButtonClick);
            //btnCancel.Click += new EventHandler(base.CancelButtonClick);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind();
                GetData();  //Get Shipping Zones List
                GetDataBoxSizes();  //GetBox Sizes List
                BindShippingZoneDDL();  //Get Shipping Zone Dropdown
                BindZipCodeDDL();       //Get Zip Code Dropdown
                BindShippingClass();
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.lnkbtnCsvDownload);
            }
        }

        protected override void LoadForm()
        {
            try
            {
                CurrentSettings = hccDeliverySetting.GetAll();
                GlobalSettings = hccGlobalSetting.GetSettings();

                if (GlobalSettings != null)
                {
                    //txtMinShipCost.Text = GlobalSettings.DeliveryMinCost.ToString("f2");
                    //txtMaxShipCost.Text = GlobalSettings.DeliveryMaxCost.ToString("f2");
                }

                BindlvwMealTypes();

                //foreach (ListViewItem row in lvwMealTypes.Items)
                //{
                //    int rowMealTypeId = int.Parse(lvwMealTypes.DataKeys[row.DataItemIndex].Value.ToString());

                //    hccDeliverySetting setting = CurrentSettings.Where(a => a.MealTypeID == rowMealTypeId).SingleOrDefault();

                //    if (setting == null)
                //    {
                //        setting = new hccDeliverySetting { MealTypeID = rowMealTypeId, ShipCost = 0.00m };
                //        setting.Save();
                //    }

                //    TextBox txtMinPrice = (TextBox)row.FindControl("txtMinPrice");
                //    if (txtMinPrice != null)
                //        txtMinPrice.Text = setting.ShipCost.ToString("f2");
                //}
            }
            catch (Exception)
            {
                throw;
            }

        }

        protected override void SaveForm()
        {
            try
            {
                if (CurrentSettings == null)
                    CurrentSettings = hccDeliverySetting.GetAll();

                if (GlobalSettings == null)
                    GlobalSettings = hccGlobalSetting.GetSettings();

                if (GlobalSettings != null)
                {
                    //if (!string.IsNullOrWhiteSpace(txtMinShipCost.Text))
                    //    GlobalSettings.DeliveryMinCost = decimal.Parse(txtMinShipCost.Text.Trim());

                    //if (!string.IsNullOrWhiteSpace(txtMaxShipCost.Text))
                    //    GlobalSettings.DeliveryMaxCost = decimal.Parse(txtMaxShipCost.Text.Trim());

                    GlobalSettings.Save();
                }

                //foreach (ListViewItem row in lvwMealTypes.Items)
                //{
                //    int rowMealTypeId = int.Parse(lvwMealTypes.DataKeys[row.DataItemIndex].Value.ToString());

                //    hccDeliverySetting setting = CurrentSettings.Where(a => a.MealTypeID == rowMealTypeId).SingleOrDefault();

                //    if (setting != null)
                //    {
                //        TextBox txtMinPrice = (TextBox)row.FindControl("txtMinPrice");
                //        if (txtMinPrice != null)
                //        {
                //            setting.ShipCost = decimal.Parse(txtMinPrice.Text.Trim());
                //            txtMinPrice.Text = setting.ShipCost.ToString("f2");
                //        }

                //        setting.Save();
                //    }
                //}
            }
            catch (Exception)
            {
                throw;
            }

        }

        protected override void ClearForm()
        {
            //txtMaxShipCost.Text = 0.ToString();
            //txtMinShipCost.Text = 0.ToString();
            ClearlvwMealTypes();
        }

        protected override void SetButtons()
        {
            //throw new NotImplementedException();
        }

        void BindlvwMealTypes()
        {
            List<RequiredMealType> reqTypes = new List<RequiredMealType>();
            List<Tuple<string, int>> mealTypes = Enums.GetEnumAsTupleList(typeof(Enums.MealTypes));
            Tuple<string, int> unkType = mealTypes.Single(a => a.Item1 == "Unknown");
            mealTypes.Remove(unkType);

            foreach (Tuple<string, int> mealType in mealTypes)
            {
                reqTypes.Add(new RequiredMealType
                {
                    MealTypeID = mealType.Item2,
                    MealTypeName = mealType.Item1
                });
            }

            //lvwMealTypes.DataSource = reqTypes;
            //lvwMealTypes.DataBind();
        }

        void ClearlvwMealTypes()
        {
            //foreach (ListViewItem row in lvwMealTypes.Items)
            //{
            //    TextBox txtMinPrice = (TextBox)row.FindControl("txtMinPrice");

            //    if (txtMinPrice != null)
            //        txtMinPrice.Text = 0.ToString();

            //}
        }
        /// <summary>
        /// Bind Shipping Classes 
        /// </summary>
        protected void BindShippingClass()
        {
            hccShippingZone hccshopin = new hccShippingZone();
            DataTable ds = hccshopin.BindGridShippingClass();
            grdAddShippingClass.DataSource = ds;
            grdAddShippingClass.DataBind();
        }
        /// <summary>
        /// Bind Shipping Zone in Dropdown List
        /// </summary>
        protected void BindZipCodeDDL()
        {
            hccShippingZone hccshopin = new hccShippingZone();
            DataSet ds = hccshopin.ZipCodeDDL();
            ddlZipCode.DataSource = ds;
            ddlZipCode.DataTextField = "ZipCode";
            ddlZipCode.DataValueField = "ZipZoneID";
            ddlZipCode.DataBind();
            ddlZipCode.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        /// <summary>
        /// Bind Zip Code in Dropdown List
        /// </summary>
        protected void BindShippingZoneDDL()
        {
            hccShippingZone hccshopin = new hccShippingZone();
            DataSet ds = hccshopin.ShippingZoneDDL();
            ddlShippingZone.DataSource = ds;
            ddlShippingZone.DataTextField = "ZoneName";
            ddlShippingZone.DataValueField = "ZoneID";
            ddlShippingZone.DataBind();
            ddlShippingZone.Items.Insert(0, new ListItem("--Select--", "0"));

            DataSet ds1 = hccshopin.ShippingZoneID();
            drpZoneID.DataSource = ds1;
            drpZoneID.DataValueField = "TypeName";
            drpZoneID.DataBind();
            drpZoneID.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        /// <summary>
        /// getdata in shipping zones
        /// </summary>
        protected void GetData()
        {

            hccShippingZone hccshopin = new hccShippingZone();
            DataTable ds = hccshopin.BindGrid();
            shippingZonesGrid.DataSource = ds;
            shippingZonesGrid.DataBind();
        }
        /// <summary>
        /// getdata in boxSizes
        /// </summary>
        protected void GetDataBoxSizes()
        {
            hccShippingZone hccshopin = new hccShippingZone();
            DataTable ds = hccshopin.BindGridBoxSizes();
            grdBoxSizes.DataSource = ds;
            grdBoxSizes.DataBind();
        }
        /// <summary>
        /// Getdata in ZipCodes
        /// </summary>
        protected void BindGridZipCodes()
        {
            hccShippingZone hccshopin = new hccShippingZone();
            DataTable ds = hccshopin.BindGridZipCodesNew();
            grdZipCode.DataSource = ds;
            grdZipCode.DataBind();
        }
        /// <summary>
        /// Save Fucntionality ShippingZones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveShippingZone_Click(object sender, EventArgs e)
        {
            try
            {
                var zipcode = 0;
                int pn = hccShippingZone.AddUpdateShippingZone(zipcode, txtZoneName.Text, txtShippingDesc.Text, txtMultiplier.Text, txtMinFee.Text, txtMaxFee.Text, chkIsDefaultShippingZone.Checked, chkIsPickupShippingZone.Checked,Convert.ToInt32(txtOrderMinium.Text));
                if (pn > 0)
                {
                    this.ClearShippingZone();
                    this.GetData();
                }
                else if (pn == -1)
                {
                    this.ClearShippingZone();
                    lblShippingError.Text = "Zone Name Saved Failed";
                }
                else if (pn == -2)
                {
                    this.ClearShippingZone();
                    lblShippingError.Text = "Zone Name Update Failed.";
                }
                else if (pn == -3)
                {
                    this.ClearShippingZone();
                    lblShippingError.Text = "Zone Name Already Exists";
                }
            }
            catch (Exception ex)
            {
                if (ex.Source == "CUSTOM")
                {
                    lblShippingError.Text = ex.Message.ToString();
                }
                else
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Clear all fields after save Fucntionality ShippingZones
        /// </summary>
        protected void ClearShippingZone()
        {
            txtZoneName.Text = "";
            txtShippingDesc.Text = "";
            txtMultiplier.Text = "";
            txtMinFee.Text = "";
            txtMaxFee.Text = "";
            chkIsDefaultShippingZone.Checked = false;
            chkIsPickupShippingZone.Checked = false;
            lblShippingError.Text = "";
            lblShippingSuccess.Text = "";
        }
        /// <summary>
        /// Inline Edit Fucntionality ShippingZones 
        /// </summary>
        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            lblShippingError.Text = "";
            lblShippingSuccess.Text = "";
            shippingZonesGrid.EditIndex = e.NewEditIndex;
            this.GetData();
        }
        /// <summary>
        /// Inline Cancel Edit Fucntionality ShippingZones
        /// </summary>
        protected void OnRowCancelingEdit(object sender, EventArgs e)
        {
            lblShippingError.Text = "";
            lblShippingSuccess.Text = "";
            shippingZonesGrid.EditIndex = -1;
            this.GetData();
        }
        /// <summary>
        /// Bind Grid Fucntionality ShippingZones
        /// </summary>
        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != shippingZonesGrid.EditIndex)
            {
                (e.Row.Cells[8].Controls[2] as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
        }
        /// <summary>
        /// Inline Delete Fucntionality ShippingZones
        /// </summary>
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            lblShippingError.Text = "";
            lblShippingSuccess.Text = "";
            int zoneID = Convert.ToInt32(shippingZonesGrid.DataKeys[e.RowIndex].Values[0]);
            int delete = hccShippingZone.Delete(zoneID);
            this.GetData();
        }
        /// <summary>
        /// Inline Update Fucntionality ShippingZones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            lblShippingError.Text = "";
            lblShippingSuccess.Text = "";
            GridViewRow row = shippingZonesGrid.Rows[e.RowIndex];
            int ZoneId = Convert.ToInt32(shippingZonesGrid.DataKeys[e.RowIndex].Values[0]);
            string Zonename = (row.FindControl("txtZoneName") as TextBox).Text;
            string Multiplier = (row.FindControl("txtMultiplier") as TextBox).Text;
            string MinFee = (row.FindControl("txtMinFee") as TextBox).Text;
            string MaxFee = (row.FindControl("txtMaxFee") as TextBox).Text;
            bool check = (row.FindControl("chkDefaultShippingZone") as CheckBox).Checked;
            string OrderMinimumtext = (row.FindControl("txtOrderMinimum") as TextBox).Text;

            
            int OrderMinimum = 0;
            if (string.IsNullOrEmpty(OrderMinimumtext))
            {
                OrderMinimum = 0;
            }
            else
            {
                OrderMinimum = Convert.ToInt32(OrderMinimumtext);

            }
            if (OrderMinimum==0)
            {

           

                return;
            }
           
            bool IsPickupcheck = (row.FindControl("chkPickupShippingZone") as CheckBox).Checked;
            string TypeName = (row.FindControl("txtShippingDesc") as TextBox).Text;

            float _minfee;
            float _maxfee;


            if (MinFee != null && MaxFee != null && Zonename != null && Multiplier != null && TypeName != null)
            {
                if (MinFee != "" && MaxFee != "" && Zonename != "" && Multiplier != "" && TypeName != "")
                {
                    _minfee = Convert.ToSingle(MinFee);
                    _maxfee = Convert.ToSingle(MaxFee);

                    int pn = 0;
                    if (_minfee < _maxfee)
                    {
                        pn = hccShippingZone.AddUpdateShippingZone(ZoneId, Zonename, TypeName, Multiplier, MinFee, MaxFee, check, IsPickupcheck, OrderMinimum);
                        if (pn > 0)
                        {
                            shippingZonesGrid.EditIndex = -1;
                            this.GetData();
                            lblShippingSuccess.Text = "Shipping Zone Updated Successfully.";
                        }
                        else if (pn == -2)
                        {
                            lblShippingError.Text = "Shipping Zone Updated Field.";
                        }
                    }
                    else
                    {
                        lblShippingError.Text = "UPDATE FAILED. The system could NOT update your new Zone information. Please make sure that the min fee is less than the max fee.";
                    }
                }
                else
                {
                    lblShippingError.Text = "UPDATE FAILED. The system could NOT update your new Zone information. Please make sure that the min fee is less than the max fee.";
                }
            }
            else
            {
                lblShippingError.Text = "UPDATE FAILED. The system could NOT update your new Zone information. Please make sure that the min fee is less than the max fee.";
                //Shipping Zone Updated Field. Invalid input values
            }
        }
        /// <summary>
        /// BoxSizes Save Fucntionality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBoxSave_Click(object sender, EventArgs e)
        {
            try
            {
                var boxid = 0;
                int pn = hccShippingZone.AddUpdateBox(boxid, txtBoxName.Text, txtDIM_W.Text, txtDIM_L.Text, txtDIM_H.Text, Convert.ToInt32(txtMaxNoMeals.Text));
                if (pn > 0)
                {
                    this.ClearBoxSize();
                    this.GetDataBoxSizes();
                }
                else if (pn == -1)
                {
                    this.ClearBoxSize();
                    lblErrorRecord.Text = "Box Name Saved Failed";
                }
                else if (pn == -2)
                {
                    this.ClearBoxSize();
                    lblErrorRecord.Text = "Box Name Update Failed.";
                }
                else if (pn == -3)
                {
                    this.ClearBoxSize();
                    lblErrorRecord.Text = "Box Name Is Already Exists";
                }
            }
            catch (Exception ex)
            {
                if (ex.Source == "CUSTOM")
                {
                    lblErrorRecord.Text = ex.Message.ToString();
                }
                else
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Clear All Field after Save Fucntionality BoxSizes
        /// </summary>
        protected void ClearBoxSize()
        {
            txtBoxName.Text = "";
            txtDIM_W.Text = "";
            txtDIM_L.Text = "";
            txtDIM_H.Text = "";
            txtMaxNoMeals.Text = "";
            lblErrorRecord.Text = "";
        }
        /// <summary>
        /// Inline Cancel Edit Fucntionality BoxSizes
        /// </summary>
        protected void OnBoxSizes_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdBoxSizes.EditIndex = -1;
            this.GetDataBoxSizes();
        }
        /// <summary>
        /// Inline Delete Fucntionality BoxSizes
        /// </summary>
        protected void OnBoxSizes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int BoxID = Convert.ToInt32(grdBoxSizes.DataKeys[e.RowIndex].Values[0]);
            int delete = hccShippingZone.DeleteBoxSize(BoxID);
            this.GetDataBoxSizes();
        }
        /// <summary>
        /// Inline Edit Fucntionality BoxSizes
        /// </summary>
        protected void OnBoxSizes_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdBoxSizes.EditIndex = e.NewEditIndex;
            this.GetDataBoxSizes();
        }
        /// <summary>
        /// Inline Update Fucntionality BoxSizes
        /// </summary>
        protected void OnBoxSizes_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = grdBoxSizes.Rows[e.RowIndex];
            int BoxID = Convert.ToInt32(grdBoxSizes.DataKeys[e.RowIndex].Values[0]);
            string BoxName = (row.FindControl("txtBoxName") as TextBox).Text;
            string DIM_W = (row.FindControl("txtDIM_W") as TextBox).Text;
            string DIM_L = (row.FindControl("txtDIM_L") as TextBox).Text;
            string DIM_H = (row.FindControl("txtDIM_H") as TextBox).Text;
            int MaxNoMeals = Convert.ToInt32((row.FindControl("txtMaxNoMeals") as TextBox).Text);
            int pn = hccShippingZone.AddUpdateBox(BoxID, BoxName, DIM_W, DIM_L, DIM_H, MaxNoMeals);
            if (pn > 0)
            {
                grdBoxSizes.EditIndex = -1;
                this.GetDataBoxSizes();
            }
            else if (pn == -2)
            {
                lblErrorRecord.Text = "Box Name Update Failed.";
            }
            lblErrorRecord.Text = "Box Data Updated Successfully.";
        }
        /// <summary>
        /// Bind Grid Fucntionality BoxSizes
        /// </summary>
        protected void OnBoxSizes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != grdBoxSizes.EditIndex)
            {
                (e.Row.Cells[5].Controls[2] as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
        }
        /// <summary>
        /// Addding Save Fucntionality ZipCodes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveZipCode_Click(object sender, EventArgs e)
        {
            try
            {
                var Zipcode = txtZipCode.Text;
                if (Zipcode.Length == 1)
                {
                    Zipcode = "0000" + Zipcode;
                }
                else if (Zipcode.Length == 2)
                {
                    Zipcode = "000" + Zipcode;
                }
                else if (Zipcode.Length == 3)
                {
                    Zipcode = "00" + Zipcode;
                }
                else if (Zipcode.Length == 4)
                {
                    Zipcode = "0" + Zipcode;
                }
                #region
                //var DestinationZip = txtZipCode.Text;
                //string ZipCodeFrom = "";
                //string ZipCodeTo = "";
                //if (DestinationZip.Contains('-'))
                //{
                //    string[] strArray = DestinationZip.Split('-');


                //    if (strArray[0].Length == 4)
                //    {
                //        ZipCodeFrom = strArray[0].Trim(new char[] { '\n', '\r' }) + "00";
                //        ZipCodeTo = strArray[1].Trim(new char[] { '\n', '\r' }) + "99";
                //    }
                //    else
                //    {
                //        ZipCodeFrom = strArray[0].Trim(new char[] { '\n', '\r' });
                //        ZipCodeTo = strArray[1].Trim(new char[] { '\n', '\r' });
                //    }


                //}
                //else
                //{
                //    //var zipcodeFormat = validataingZipcodes(DestinationZip);
                //    if (DestinationZip.Length == 1)
                //    {
                //        ZipCodeFrom = "00" + DestinationZip + "00";
                //        ZipCodeTo = "00" + DestinationZip + "99";
                //    }
                //    else if (DestinationZip.Length == 2)
                //    {
                //        ZipCodeFrom = "0" + DestinationZip + "00";
                //        ZipCodeTo = "0" + DestinationZip + "99";
                //    }
                //    else if (DestinationZip.Length == 3)
                //    {
                //        ZipCodeFrom = DestinationZip + "00";
                //        ZipCodeTo = DestinationZip + "99";
                //    }
                //    else
                //    {
                //        ZipCodeFrom = DestinationZip;
                //        ZipCodeTo = DestinationZip;
                //    }
                //}
                #endregion

                var zipcode = 0;
                var ZoneID = "5";
                int pn = hccShippingZone.AddUpdateZipCodelatest(zipcode, Zipcode, ZoneID, drpZoneID.SelectedValue);
                if (pn > 0)
                {
                    this.ClearZipCode();
                    this.BindGridZipCodes();
                }
                else if (pn == -1)
                {
                    this.ClearZipCode();
                    lblSaveZipCodeError.Text = "Zip Code Save Failed.";
                }
                else if (pn == -2)
                {
                    this.ClearZipCode();
                    lblSaveZipCodeError.Text = "Zip Code Update Failed.";
                }
                else if (pn == -3)
                {
                    this.ClearZipCode();
                    lblSaveZipCodeError.Text = "Zip Code Is Already Exists For This Zone.";
                }
                else if (pn == -4)
                {
                    this.ClearZipCode();
                    lblSaveZipCodeError.Text = "Zip Code Is Already Exists.";
                }
            }
            catch (Exception ex)
            {
                if (ex.Source == "CUSTOM")
                {
                    lblSaveZipCodeError.Text = ex.Message.ToString();
                }
                else
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Clear all fields after Addding Save Fucntionality ZipCodes
        /// </summary>
        protected void ClearZipCode()
        {
            txtZipCode.Text = "";
            lblSaveZipCodeError.Text = "";
        }
        /// <summary>
        /// Canceling inline Edit Fucntionality ZipCodes
        /// </summary>
        protected void grdZipCode_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdZipCode.EditIndex = -1;
            this.BindGridZipCodes();
        }
        /// <summary>
        /// Inline Delete Fucntionality ZipCodes
        /// </summary>
        protected void grdZipCode_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int ZipZoneID = Convert.ToInt32(grdZipCode.DataKeys[e.RowIndex].Values[0]);
            int delete = hccShippingZone.DeleteZipCode(ZipZoneID);
            this.BindGridZipCodes();
        }
        /// <summary>
        /// Inline Edit Fucntionality ZipCodes
        /// </summary>
        protected void grdZipCode_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdZipCode.EditIndex = e.NewEditIndex;
            this.BindGridZipCodes();
        }
        /// <summary>
        /// Inline Update Fucntionality ZipCodes
        /// </summary>
        protected void grdZipCode_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = grdZipCode.Rows[e.RowIndex];
            int ZipZoneID = Convert.ToInt32(grdZipCode.DataKeys[e.RowIndex].Values[0]);
            string ZipCode = (row.FindControl("txtZipCode") as TextBox).Text;
            string ZoneID = (row.FindControl("lblZipCodeZoneID") as Label).Text;

            int pn = hccShippingZone.AddUpdateZipCode(ZipZoneID, ZipCode, ZoneID);
            if (pn > 0)
            {
                grdZipCode.EditIndex = -1;
                this.BindGridZipCodes();
            }
            else if (pn == -2)
            {
                lblSaveZipCodeError.Text = "Zip Code Update Failed.";
            }
            lblSaveZipCodeError.Text = "Zip Code Updated Successfully.";
        }

        /// <summary>
        /// Upload CSV Zipcodes into Database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCSVUpload_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            hccShippingZone hccshoping = new hccShippingZone();

            if (FileUpload1.HasFile)
            {
                if (Path.GetExtension(FileUpload1.FileName) == ".xlsx" || Path.GetExtension(FileUpload1.FileName) == ".csv")
                {

                    string csvFilePath = Server.MapPath("~/CSVFiles") + FileUpload1.PostedFile.FileName;
                    FileUpload1.SaveAs(csvFilePath);
                    string filepath = Path.GetFullPath(csvFilePath);

                    string excelFilePath = Server.MapPath("~/CSVFiles/Sample.xlsx");

                    string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                    int len = 5;

                    Random rnd = new Random();
                    StringBuilder b = new StringBuilder(len);
                    for (int i = 0; i < len; i++)
                    {
                        b.Append(chars[rnd.Next(chars.Length)]);
                    }

                    string worksheetsName = b.ToString();
                    bool firstRowIsHeader = false;

                    var excelTextFormat = new ExcelTextFormat();
                    excelTextFormat.Delimiter = ',';
                    excelTextFormat.EOL = "\r";

                    var excelFileInfo = new FileInfo(excelFilePath);
                    var csvFileInfo = new FileInfo(filepath);

                    table.Columns.Add("ZipCodeFrom");
                    table.Columns.Add("ZipCodeTo");
                    table.Columns.Add("ZoneID");
                    table.Columns.Add("ShipmentTypeId");

                    //Data table values Variables
                    var ZipCodeFrom = "";
                    var ZipCodeTo = "";
                    var ZoneID = "";
                    var ShipmentTypeId = "";

                    //Excel File Variables

                    var DestinationZip = "";
                    var FedxZipcode = "";
                    var FedxGroundZone = "";


                    using (ExcelPackage package = new ExcelPackage(excelFileInfo))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(worksheetsName);
                        worksheet.Cells["A1"].LoadFromText(csvFileInfo, excelTextFormat, OfficeOpenXml.Table.TableStyles.Medium25, firstRowIsHeader);
                        package.Save();

                        for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                        {
                            var newRow = table.NewRow();

                            var row = worksheet.Cells[i, 1, i, worksheet.Dimension.End.Column];

                            if (worksheet.Cells[i, 1].Value.ToString() != "\n")
                            {
                                DestinationZip = worksheet.Cells[i, 1].Value.ToString();
                                FedxZipcode = worksheet.Cells[i, 2].Value.ToString();
                                FedxGroundZone = worksheet.Cells[i, 3].Value.ToString();

                                if (DestinationZip.Contains('-'))
                                {
                                    string[] strArray = DestinationZip.Split('-');


                                    if (strArray[0].Length == 4)
                                    {
                                        ZipCodeFrom = strArray[0].Trim(new char[] { '\n', '\r' }) + "00";
                                        ZipCodeTo = strArray[1].Trim(new char[] { '\n', '\r' }) + "99";
                                    }
                                    else
                                    {
                                        ZipCodeFrom = strArray[0].Trim(new char[] { '\n', '\r' });
                                        ZipCodeTo = strArray[1].Trim(new char[] { '\n', '\r' });
                                    }
                                    newRow[0] = ZipCodeFrom;
                                    newRow[1] = ZipCodeTo;
                                }
                                else
                                {
                                    if (DestinationZip.Length == 1)
                                    {
                                        ZipCodeFrom = "00" + DestinationZip + "00";
                                        ZipCodeTo = "00" + DestinationZip + "99";
                                    }
                                    else if (DestinationZip.Length == 2)
                                    {
                                        ZipCodeFrom = "0" + DestinationZip + "00";
                                        ZipCodeTo = "0" + DestinationZip + "99";
                                    }
                                    else if (DestinationZip.Length == 3)
                                    {
                                        ZipCodeFrom = DestinationZip + "00";
                                        ZipCodeTo = DestinationZip + "99";
                                    }
                                    else
                                    {
                                        ZipCodeFrom = DestinationZip;
                                        ZipCodeTo = DestinationZip;
                                    }

                                    newRow[0] = ZipCodeFrom;
                                    newRow[1] = ZipCodeTo;
                                }

                                if (FedxZipcode != "NA")
                                {
                                    ZoneID = FedxZipcode;
                                    ShipmentTypeId = "1";

                                    newRow[2] = Convert.ToInt32(ZoneID);
                                    newRow[3] = Convert.ToInt32(ShipmentTypeId);
                                }
                                else
                                {

                                    ZoneID = hccshoping.GetDefaultshippingZone();
                                    ShipmentTypeId = "1";
                                    newRow[2] = Convert.ToInt32(ZoneID);
                                    newRow[3] = Convert.ToInt32(ShipmentTypeId);
                                }
                                table.Rows.Add(newRow);
                                if (!string.IsNullOrEmpty(FedxGroundZone))
                                {
                                    var newRow1 = table.NewRow();
                                    newRow1[0] = ZipCodeFrom;
                                    newRow1[1] = ZipCodeTo;
                                    if (FedxZipcode != "NA")
                                    {
                                        ZoneID = FedxGroundZone;
                                        ShipmentTypeId = "2";
                                        newRow1[2] = Convert.ToInt32(ZoneID);
                                        newRow1[3] = Convert.ToInt32(ShipmentTypeId);
                                    }
                                    else
                                    {
                                        ZoneID = hccshoping.GetDefaultshippingZone();
                                        ShipmentTypeId = "2";
                                        newRow1[2] = Convert.ToInt32(ZoneID);
                                        newRow1[3] = Convert.ToInt32(ShipmentTypeId);
                                    }

                                    table.Rows.Add(newRow1);
                                }

                            }
                        }
                    }
                    #region
                    //ExcelPackage package = new ExcelPackage(FileUpload1.FileContent);
                    //ExcelWorksheet workSheet = package.Workbook.Worksheets[1];

                    //foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
                    //{
                    //    table.Columns.Add(firstRowCell.Text);
                    //}

                    //for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                    //{
                    //    var newRow = table.NewRow();

                    //    var row = workSheet.Cells[i, 1, i, workSheet.Dimension.End.Column];

                    //    foreach (var cell in row)
                    //    {
                    //        newRow[cell.Start.Column - 1] = cell.Text;
                    //    }

                    //    table.Rows.Add(newRow);
                    //}
                    #endregion
                }
                hccshoping.InsertExcelDataTemp(table);
            }
        }

        protected void ddlShipingZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblZipCodeErrorMsg.Text = "";
                shippingGrd.EditIndex = -1;
                int ZoneId = Convert.ToInt32(ddlShippingZone.SelectedValue);
                if (ZoneId > 0)
                {
                    hccShippingZone hccshopin = new hccShippingZone();
                    DataTable ds = hccshopin.BindGridShippingZoneByZoneID(ZoneId);
                    if (ds.Rows.Count > 0)
                    {
                        lblShippingZone.Text = ds.Rows[0]["ZoneName"].ToString();
                        lblMinShippingFeeOrder.Text = ds.Rows[0]["MinFee"].ToString();
                        lblMaxShippingFeeOrder.Text = ds.Rows[0]["MaxFee"].ToString();
                        txtMinShippingFeeOrder.Text = ds.Rows[0]["MinFee"].ToString();
                        txtMaxShippingFeeOrder.Text = ds.Rows[0]["MaxFee"].ToString();
                        lblShipMultiplier.Text = ds.Rows[0]["Multiplier"].ToString();
                        string IsPickup = ds.Rows[0]["IsPickupShippingZone"].ToString();
                        if (IsPickup == "True")
                        {
                            chkIsPickup.Checked = true;
                        }
                        else
                        {
                            chkIsPickup.Checked = false;
                        }
                    }
                    DataTable BoxZone = hccshopin.BindBoxToShippingZoneFee(ZoneId); //Get Box To Shipping Zone Fee List by ZoneId
                    decimal Cost = 0;
                    decimal Price = 0;
                    decimal Mutliplier = Convert.ToDecimal(ds.Rows[0]["Multiplier"].ToString());

                    foreach (DataRow row in BoxZone.Rows)
                    {
                        if (row["Cost"] == null)
                        {
                            Cost = 0;
                            Price = 0;
                            row["Price"] = Convert.ToDecimal(Price.ToString("G29")); 
                        }
                        else
                        {
                            if (row["Cost"].ToString() == "")
                            {
                                Cost = 0;
                                Price = 0;
                                row["Price"] = Convert.ToDecimal(Price.ToString("G29"));
                            }
                            else
                            {
                                Cost = Convert.ToDecimal(row["Cost"].ToString());
                                Price = Cost * Mutliplier;
                                row["Price"] = Convert.ToDecimal(Price.ToString("G29"));
                            }
                        }
                    }

                    shippingGrd.DataSource = BoxZone;
                    shippingGrd.DataBind();
                    shippingGrd.Visible = true;
                }
                else
                {
                    txtMinShippingFeeOrder.Text = "";
                    txtMaxShippingFeeOrder.Text = "";
                    lblShipMultiplier.Text = "";
                    chkIsPickup.Checked = false;
                    shippingGrd.Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Zone Is not Valid.");
            }
        }

        protected void ddlZipCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                shippingGrd.EditIndex = -1;
                string ZipCode = Convert.ToString(ddlZipCode.SelectedItem);
                hccShippingZone hccshopin = new hccShippingZone();
                DataSet ds = hccshopin.BindZoneByZipCode(ZipCode);
                ddlShippingZone.DataSource = ds;
                ddlShippingZone.DataTextField = ds.Tables[0].Columns[1].ColumnName;//"ZoneName"
                ddlShippingZone.DataValueField = ds.Tables[0].Columns[0].ColumnName; //"ZoneID"
                ddlShippingZone.DataBind();
                if (ZipCode == "--Select--")
                {
                    ddlShippingZone.Items.Insert(0, new ListItem("--Select--", "0"));
                    txtMinShippingFeeOrder.Text = "";
                    txtMaxShippingFeeOrder.Text = "";
                    lblShipMultiplier.Text = "";
                    chkIsPickup.Checked = false;
                    shippingGrd.Visible = false;
                }
                else
                {
                    int ZoneId = Convert.ToInt32(ds.Tables[0].Rows[0]["ZoneID"].ToString()); //ZoneId from On Select Zone Dropdown
                    DataTable ds1 = hccshopin.BindGridShippingZoneByZoneID(ZoneId);
                    if (ds1.Rows.Count > 0)
                    {
                        lblShippingZone.Text = ds1.Rows[0]["ZoneName"].ToString();
                        txtMinShippingFeeOrder.Text = ds1.Rows[0]["MinFee"].ToString();
                        txtMaxShippingFeeOrder.Text = ds1.Rows[0]["MaxFee"].ToString();
                        lblShipMultiplier.Text = ds1.Rows[0]["Multiplier"].ToString();
                        string IsPickup = ds1.Rows[0]["IsPickupShippingZone"].ToString();
                        if (IsPickup == "True")
                        {
                            chkIsPickup.Checked = true;
                        }
                        else
                        {
                            chkIsPickup.Checked = false;
                        }
                        DataTable BoxZone = hccshopin.BindBoxToShippingZoneFee(ZoneId); //Get Box To Shipping Zone Fee List by ZoneId
                        shippingGrd.DataSource = BoxZone;
                        shippingGrd.DataBind();
                        shippingGrd.Visible = true;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Zip Code Is not Valied.");
            }
        }

        protected void OnBoxZoneFee_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            shippingGrd.EditIndex = -1;
            hccShippingZone hccshopin = new hccShippingZone();
            int ZoneId = Convert.ToInt32(ddlShippingZone.SelectedValue);                //ZoneId from On Select Zone Dropdown
            DataTable BoxZone = hccshopin.BindBoxToShippingZoneFee(ZoneId); //Get Box To Shipping Zone Fee List by ZoneId
            shippingGrd.DataSource = BoxZone;
            shippingGrd.DataBind();
            shippingGrd.Visible = true;
        }

        protected void OnBoxZoneFee_RowEditing(object sender, GridViewEditEventArgs e)
        {
            shippingGrd.EditIndex = e.NewEditIndex;
            hccShippingZone hccshopin = new hccShippingZone();
            int ZoneId = Convert.ToInt32(ddlShippingZone.SelectedValue);                //ZoneId from On Select Zone Dropdown
            DataTable BoxZone = hccshopin.BindBoxToShippingZoneFee(ZoneId); //Get Box To Shipping Zone Fee List by ZoneId
            shippingGrd.DataSource = BoxZone;
            shippingGrd.DataBind();
            shippingGrd.Visible = true;
        }
        protected void OnBoxZoneFee_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int ID = 0;
            decimal Cost = 0;
            hccShippingZone hccshopin = new hccShippingZone();
            GridViewRow row = shippingGrd.Rows[e.RowIndex];
            if (shippingGrd.DataKeys[e.RowIndex].Values[0].ToString() == "" || shippingGrd.DataKeys[e.RowIndex].Values[0] == null)
            {
                ID = 0;       //ID Of BoxToZoneFee
            }
            else
            {
                ID = Convert.ToInt32(shippingGrd.DataKeys[e.RowIndex].Values[0]);       //ID Of BoxToZoneFee
            }

            int BoxID = Convert.ToInt32((row.FindControl("lblBoxID") as Label).Text);    //BoxID Of BoxToZoneFee
            int ZoneId = Convert.ToInt32(ddlShippingZone.SelectedValue);                //ZoneId from On Select Zone Dropdown
            decimal Multiplier = Convert.ToDecimal(lblShipMultiplier.Text);
            Cost = Convert.ToDecimal((row.FindControl("txtCost") as TextBox).Text);    //Cost Of BoxToZoneFee
            decimal Price = Convert.ToDecimal((row.FindControl("txtPrice") as TextBox).Text);   //Price Of BoxToZoneFee
            string Notes = (row.FindControl("txtNotes") as TextBox).Text;    //Notes Of BoxToZoneFee

            int pn = hccshopin.AddUpdateBoxtoZoneFee(ID, BoxID, ZoneId, Cost, Price, Notes);
            if (pn > 0)
            {
                shippingGrd.EditIndex = -1;
                DataTable BoxZone = hccshopin.BindBoxToShippingZoneFee(ZoneId); //Get Box To Shipping Zone Fee List by ZoneId
                shippingGrd.DataSource = BoxZone;
                shippingGrd.DataBind();
                shippingGrd.Visible = true;
            }
            else if (pn == -2)
            {
                lblUpdateMessage.Text = "Shipping Zone Update Field.";
            }
            lblUpdateMessage.Text = "Shipping Zone Updated Successfully.";
        }

        protected void shippingGrd_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            hccShippingZone hccshopin = new hccShippingZone();
            int ZoneId = Convert.ToInt32(ddlShippingZone.SelectedValue);                //ZoneId from On Select Zone Dropdown
            int ID = Convert.ToInt32(shippingGrd.DataKeys[e.RowIndex].Values[0]);

            int delete = hccshopin.DeleteBoxToZoneFee(ID);
            if (delete > 0)
            {
                DataTable BoxZone = hccshopin.BindBoxToShippingZoneFee(ZoneId); //Get Box To Shipping Zone Fee List by ZoneId
                shippingGrd.DataSource = BoxZone;
                shippingGrd.DataBind();
                shippingGrd.Visible = true;
            }
        }

        protected void btnShowZipCodes_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearZipCode();
                this.BindGridZipCodes();
            }
            catch (Exception ex)
            {
                if (ex.Source == "CUSTOM")
                {
                    lblSaveZipCodeError.Text = ex.Message.ToString();
                }
                else
                {
                    throw;
                }
            }
        }

        protected void btnLookupZipCode_Click(object sender, EventArgs e)
        {
            try
            {
                var zipCode = txtZipCode.Text;
                int islookup = hccShippingZone.IsLookUpNew(zipCode);
                if (islookup >= 1)
                {
                    this.ClearLookZipCode();
                    lblLookupShow.Text = "Zip Code found in database.";
                }
                else
                {
                    txtZipCode.Text = "";
                    lblLookupError.Text = "Zip Code not found in database.";
                }
            }
            catch (Exception ex)
            {
                if (ex.Source == "CUSTOM")
                {
                    lblLookupShow.Text = "";
                    lblLookupError.Text = ex.Message.ToString();
                }
                else
                {
                    throw;
                }
            }
        }
        protected void ClearLookZipCode()
        {
            lblLookupError.Text = "";
            lblLookupShow.Text = "";
        }

        protected void lnkbtnCsvDownload_Click(object sender, EventArgs e)
        {

            hccShippingZone hccshopin = new hccShippingZone();
            DataTable ds = hccshopin.BindGridZipCodesNew();
            grdZipCode.DataSource = ds;
            ExporttoExcelSheet(ds);
        }

        public void ExporttoExcelSheet(DataTable table)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode("CSVFileDAN.xlsx", System.Text.Encoding.UTF8));
            using (ExcelPackage pack = new ExcelPackage())
            {
                ExcelWorksheet ws = pack.Workbook.Worksheets.Add("CSVFileDAN");
                ws.Cells["A1"].LoadFromDataTable(table, true);
                var ms = new System.IO.MemoryStream();
                pack.SaveAs(ms);
                ms.WriteTo(HttpContext.Current.Response.OutputStream);
            }
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        private void ExporttoExcel(DataTable table)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Reports.xlsx");

            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            //sets font
            HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            HttpContext.Current.Response.Write("<BR><BR><BR>");
            //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
            HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
              "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
              "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
            //am getting my grid's column headers
            int columnscount = grdZipCode.Columns.Count;

            for (int j = 0; j < columnscount; j++)
            {      //write in new column
                HttpContext.Current.Response.Write("<Td>");
                //Get column headers  and make it as bold in excel columns
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(grdZipCode.Columns[j].HeaderText.ToString());
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
            }
            HttpContext.Current.Response.Write("</TR>");
            foreach (DataRow row in table.Rows)
            {//write in new row
                HttpContext.Current.Response.Write("<TR>");
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    HttpContext.Current.Response.Write("<Td>");
                    HttpContext.Current.Response.Write(row[i].ToString());
                    HttpContext.Current.Response.Write("</Td>");
                }
                HttpContext.Current.Response.Write("</TR>");
            }
            HttpContext.Current.Response.Write("</Table>");
            HttpContext.Current.Response.Write("</font>");
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        protected void btnShippingSearch_Click(object sender, EventArgs e)
        {
            shippingGrd.EditIndex = -1;
            txtPickupFee.Text = "";
            lblUpdateSuccess.Text = "";
            string ZipCode = Convert.ToString(txtShippingZipCode.Text);
            hccShippingZone hccshopin = new hccShippingZone();
            DataSet ds = hccshopin.BindZoneGetShippingZone();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlShippingZone.DataSource = ds;
                ddlShippingZone.DataTextField = ds.Tables[0].Columns[0].ColumnName;//"ZoneName"
                ddlShippingZone.DataValueField = ds.Tables[0].Columns[1].ColumnName; //"ZoneID"
                ddlShippingZone.DataBind();
                
                DataSet ds2 = hccshopin.BindZoneByZipCode(ZipCode);
                int ZoneId = Convert.ToInt32(ds2.Tables[0].Rows[0]["ZoneID"].ToString());  //ZoneId from On Select Zone Dropdown
                ddlShippingZone.SelectedValue = ZoneId.ToString();       //lblShippingZone.Text = ZoneId.ToString();
                lblZone.Text = Convert.ToString(ZoneId-1);
                DataTable ds1 = hccshopin.BindGridShippingZoneByZoneID(ZoneId);

                if (ds1.Rows.Count > 0)
                {
                    lblZipCodeErrorMsg.Text = "";
                    lblShippingZone.Text = ds1.Rows[0]["ZoneName"].ToString();
                    lblMinShippingFeeOrder.Text = ds1.Rows[0]["MinFee"].ToString();
                    lblMaxShippingFeeOrder.Text = ds1.Rows[0]["MaxFee"].ToString();
                    txtMinShippingFeeOrder.Text = ds1.Rows[0]["MinFee"].ToString();
                    txtMaxShippingFeeOrder.Text = ds1.Rows[0]["MaxFee"].ToString();
                    lblShipMultiplier.Text = ds1.Rows[0]["Multiplier"].ToString();
                    string IsPickup = ds1.Rows[0]["IsPickupShippingZone"].ToString();
                    if (IsPickup == "True")
                    {
                        chkIsPickup.Checked = true;
                        txtPickupFee.Style.Add("display", "block");
                    }
                    else
                    {
                        chkIsPickup.Checked = false;
                        txtPickupFee.Style.Add("display", "none");
                    }
                    if (chkIsPickup.Checked == true)
                    {
                        DataTable BoxZones = hccshopin.BindBoxToShippingZoneFee(ZoneId);
                    }
                    DataTable BoxZone = hccshopin.BindBoxToShippingZoneFee(ZoneId); //Get Box To Shipping Zone Fee List by ZoneId
                    decimal Cost = 0;
                    decimal Price = 0;
                    decimal Mutliplier = Convert.ToDecimal(ds1.Rows[0]["Multiplier"].ToString());
                    if (IsPickup == "True")
                    {
                        txtPickupFee.Text = Convert.ToDecimal(BoxZone.Rows[0]["PickupFee"]).ToString();
                    }
                    foreach (DataRow row in BoxZone.Rows)
                    {
                        if (row["Cost"] == null)
                        {
                            Cost = 0;
                            Price = 0;
                            row["Price"] = Convert.ToDecimal(Price.ToString("G29")); 
                        }
                        else
                        {
                            if (row["Cost"].ToString() == "")
                            {
                                Cost = 0;
                                Price = 0;
                                row["Price"] = Convert.ToDecimal(Price.ToString("G29")); 
                            }
                            else
                            {
                                Cost = Convert.ToDecimal(row["Cost"].ToString());
                                Price = Cost * Mutliplier;
                                Price = Convert.ToDecimal(Price.ToString("G29"));
                                row["Price"] = Price;
                            }
                        }
                    }
                    shippingGrd.DataSource = BoxZone;
                    shippingGrd.DataBind();
                    shippingGrd.Visible = true;
                }
            }
            else
            {
                lblZipCodeErrorMsg.Text = "This Zip code has been mapped to the default shipping zone";
                ddlShippingZone.Items.Insert(0, new ListItem("--Select--", "0"));
                DataTable ds1 = hccshopin.BindGridShippingZoneByZoneID(6);
                if (ds1.Rows.Count > 0)
                {
                    lblShippingZone.Text = ds1.Rows[0]["ZoneName"].ToString();
                    lblMinShippingFeeOrder.Text = ds1.Rows[0]["MinFee"].ToString();
                    lblMaxShippingFeeOrder.Text = ds1.Rows[0]["MaxFee"].ToString();
                    txtMinShippingFeeOrder.Text = ds1.Rows[0]["MinFee"].ToString();
                    txtMaxShippingFeeOrder.Text = ds1.Rows[0]["MaxFee"].ToString();
                    lblShipMultiplier.Text = ds1.Rows[0]["Multiplier"].ToString();
                    string IsPickup = ds1.Rows[0]["IsPickupShippingZone"].ToString();
                    if (IsPickup == "True")
                    {
                        chkIsPickup.Checked = true;
                    }
                    else
                    {
                        chkIsPickup.Checked = false;
                    }
                    DataTable BoxZone = hccshopin.BindBoxToShippingZoneFee(6); //Get Box To Shipping Zone Fee List by ZoneId
                    decimal Cost = 0;
                    decimal Price = 0;
                    decimal Mutliplier = Convert.ToDecimal(ds1.Rows[0]["Multiplier"].ToString());

                    foreach (DataRow row in BoxZone.Rows)
                    {
                        Cost = Convert.ToDecimal(row["Cost"].ToString());
                        Price = Cost * Mutliplier;
                        row["Price"] = Price;
                    }
                    shippingGrd.DataSource = BoxZone;
                    shippingGrd.DataBind();
                    shippingGrd.Visible = true;
                }
            }
        }

        protected void btnAddShippingClass_Click(object sender, EventArgs e)
        {
            try
            {
                var ID = 0;
                int pn = hccShippingZone.AddUpdateShippingClass(ID, txtDescription.Text);
                if (pn > 0)
                {
                    this.ClearShippingClass();
                    this.GetDataShippingClass();
                    lblAddshippingError.Text = "Shipping Class added successfully";
                }
                else if (pn == -1)
                {
                    this.ClearShippingClass();
                    lblAddshippingError.Text = "Shipping Class Name Saved Failed";
                }
                else if (pn == -2)
                {
                    this.ClearShippingClass();
                    lblAddshippingError.Text = "Shipping Class Update Failed.";
                }
                else if (pn == -3)
                {
                    this.ClearShippingClass();
                    lblAddshippingError.Text = "Shipping Class Is Already Exists";
                }
            }
            catch (Exception ex)
            {
                if (ex.Source == "CUSTOM")
                {
                    lblAddshippingError.Text = ex.Message.ToString();
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// getdata in boxSizes
        /// </summary>
        protected void GetDataShippingClass()
        {
            hccShippingZone hccshopin = new hccShippingZone();
            DataTable ds = hccshopin.BindGridShippingClass();
            grdAddShippingClass.DataSource = ds;
            grdAddShippingClass.DataBind();
        }

        /// <summary>
        /// Clear All Field after Save Fucntionality Shipping Classes
        /// </summary>
        protected void ClearShippingClass()
        {
            txtID.Text = "";
            txtDescription.Text = "";
            lblAddshippingError.Text = "";
        }

        protected void btnNewCSVUpload_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            hccShippingZone hccshoping = new hccShippingZone();

            if (FileUpload1.HasFile)
            {
                if (Path.GetExtension(FileUpload1.FileName) == ".xlsx" || Path.GetExtension(FileUpload1.FileName) == ".csv")
                {
                    string csvFilePath = Server.MapPath("~/CSVFiles") + FileUpload1.PostedFile.FileName;
                    FileUpload1.SaveAs(csvFilePath);
                    string filepath = Path.GetFullPath(csvFilePath);

                    string excelFilePath = Server.MapPath("~/CSVFiles/Sample.xlsx");

                    string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                    int len = 5;

                    Random rnd = new Random();
                    StringBuilder b = new StringBuilder(len);
                    for (int i = 0; i < len; i++)
                    {
                        b.Append(chars[rnd.Next(chars.Length)]);
                    }

                    string worksheetsName = b.ToString();
                    bool firstRowIsHeader = false;

                    var format = new ExcelTextFormat();
                    format.Delimiter = ',';
                    format.EOL = "\r";

                    var excelFileInfo = new FileInfo(excelFilePath);
                    var csvFileInfo = new FileInfo(filepath);

                    table.Columns.Add("ZipCode");
                    table.Columns.Add("ZoneID");
                    table.Columns.Add("Small");
                    table.Columns.Add("Medium");
                    table.Columns.Add("Large");
                    table.Columns.Add("ShippingClass");
                    table.Columns.Add("SmallID");
                    table.Columns.Add("MediumID");
                    table.Columns.Add("LargeID");

                    //Excel File Variables
                    var Zipcode = "";
                    var ZoneName = "";
                    var Small = "";
                    var Medium = "";
                    var Large = "";
                    var ShippingClass = "";

                    using (ExcelPackage package = new ExcelPackage(excelFileInfo))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(worksheetsName);
                        worksheet.Cells["A1"].LoadFromText(csvFileInfo, format, OfficeOpenXml.Table.TableStyles.Medium27, firstRowIsHeader);
                        package.Save();


                        for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                        {
                            var newRow = table.NewRow();

                            var row = worksheet.Cells[i, 1, i, worksheet.Dimension.End.Column];

                            if (worksheet.Cells[i, 1].Value.ToString() != "\n")
                            {
                                Zipcode = worksheet.Cells[i, 1].Value.ToString();
                                ZoneName = hccshoping.GetZoneIDByZoneName(worksheet.Cells[i, 2].Value.ToString()).ToString();
                                Small = worksheet.Cells[i, 3].Value.ToString();
                                Medium = worksheet.Cells[i, 4].Value.ToString();
                                Large = worksheet.Cells[i, 5].Value.ToString();
                                ShippingClass = worksheet.Cells[i, 6].Value.ToString();
                                if (Zipcode.Length == 1)
                                {
                                    Zipcode = "0000" + Zipcode;
                                }
                                else if (Zipcode.Length == 2)
                                {
                                    Zipcode = "000" + Zipcode;
                                }
                                else if (Zipcode.Length == 3)
                                {
                                    Zipcode = "00" + Zipcode;
                                }
                                else if (Zipcode.Length == 4)
                                {
                                    Zipcode = "0" + Zipcode;
                                }

                                newRow[0] = Zipcode;
                                newRow[1] = ZoneName;
                                newRow[2] = Small;
                                newRow[3] = Medium;
                                newRow[4] = Large;
                                newRow[5] = ShippingClass;
                                newRow[6] = 3;
                                newRow[7] = 4;
                                newRow[8] = 5;

                                table.Rows.Add(newRow);
                            }
                        }
                    }
                }

                hccshoping.InsertExcelDataNew(table);
            }
        }

        protected void grdAddShippingClass_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdAddShippingClass.EditIndex = -1;
            this.GetDataShippingClass();
        }

        protected void grdAddShippingClass_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != grdAddShippingClass.EditIndex)
            {
                (e.Row.Cells[2].Controls[2] as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
        }

        protected void grdAddShippingClass_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdAddShippingClass.EditIndex = e.NewEditIndex;
            this.GetDataShippingClass();
        }

        protected void grdAddShippingClass_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int ID = Convert.ToInt32(grdAddShippingClass.DataKeys[e.RowIndex].Values[0]);
            int delete = hccShippingZone.DeleteShippingClass(ID);
            this.GetDataShippingClass();
        }

        protected void grdAddShippingClass_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = grdAddShippingClass.Rows[e.RowIndex];
            int ID = Convert.ToInt32(grdAddShippingClass.DataKeys[e.RowIndex].Values[0]);
            string Description = (row.FindControl("txtDescription") as TextBox).Text;
            int pn = hccShippingZone.AddUpdateShippingClass(ID, Description);
            if (pn > 0)
            {
                grdAddShippingClass.EditIndex = -1;
                this.GetDataShippingClass();
            }
            else if (pn == -2)
            {
                lblErrorRecord.Text = "Box Name Update Failed.";
            }
            lblErrorRecord.Text = "Box Data Updated Successfully.";
        }

        protected void btnSave1_Click(object sender, EventArgs e)
        {
            shippingGrd.EditIndex = -1;
            string ZipCode = Convert.ToString(txtShippingZipCode.Text);
            hccShippingZone hccshopin = new hccShippingZone();
            DataSet ds = hccshopin.BindZoneGetShippingZone();
            int ZoneId = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlShippingZone.DataSource = ds;
                ddlShippingZone.DataTextField = ds.Tables[0].Columns[0].ColumnName;//"ZoneName"
                ddlShippingZone.DataValueField = ds.Tables[0].Columns[1].ColumnName; //"ZoneID"
                ddlShippingZone.DataBind();

                DataSet ds2 = hccshopin.BindZoneByZipCode(ZipCode);
                ZoneId = Convert.ToInt32(ds2.Tables[0].Rows[0]["ZoneID"].ToString());  //ZoneId from On Select Zone Dropdown
                ddlShippingZone.SelectedValue = ZoneId.ToString();
                DataTable ds1 = hccshopin.BindGridShippingZoneByZoneID(ZoneId);
                if (ds1.Rows.Count > 0)
                {
                    lblZipCodeErrorMsg.Text = "";
                    lblShippingZone.Text = ds1.Rows[0]["ZoneName"].ToString();
                    lblMinShippingFeeOrder.Text = ds1.Rows[0]["MinFee"].ToString();
                    lblMaxShippingFeeOrder.Text = ds1.Rows[0]["MaxFee"].ToString();
                    txtMinShippingFeeOrder.Text = ds1.Rows[0]["MinFee"].ToString();
                    txtMaxShippingFeeOrder.Text = ds1.Rows[0]["MaxFee"].ToString();
                    lblShipMultiplier.Text = ds1.Rows[0]["Multiplier"].ToString();
                    string IsPickup = ds1.Rows[0]["IsPickupShippingZone"].ToString();

                    if (chkIsPickup.Checked == true)
                    {
                        var PickupFee = txtPickupFee.Text;
                        if (PickupFee == string.Empty)
                        {
                            PickupFee = "0";
                            txtPickupFee.Text = PickupFee;
                        }
                        DataTable BoxZones = hccshopin.BindBoxToShippingZoneFee(ZoneId);
                        foreach (DataRow row in BoxZones.Rows)
                        {
                            row["PickupFee"] = PickupFee;
                            bool update = hccshopin.UpdatePickupfee(PickupFee, ZoneId);
                        }
                        int PickUp = 1;
                        bool IsPickup1 = hccshopin.UpdateIsPickupfee(PickUp, ZoneId);
                        txtPickupFee.Style.Add("display", "block");
                    }
                    else
                    {
                        txtPickupFee.Text = "0";
                        var PickupFee = txtPickupFee.Text;
                        DataTable BoxZones = hccshopin.BindBoxToShippingZoneFee(ZoneId);
                        foreach (DataRow row in BoxZones.Rows)
                        {
                            row["PickupFee"] = PickupFee;
                            bool update = hccshopin.UpdatePickupfee(PickupFee, ZoneId);
                        }
                        int PickUp=0;
                        bool IsPickup1 = hccshopin.UpdateIsPickupfee(PickUp, ZoneId);
                        txtPickupFee.Style.Add("display", "none");
                    }
                   
                    DataTable BoxZone = hccshopin.BindBoxToShippingZoneFee(ZoneId); //Get Box To Shipping Zone Fee List by ZoneId
                    decimal Cost = 0;
                    decimal Price = 0;
                    decimal Mutliplier = Convert.ToDecimal(ds1.Rows[0]["Multiplier"].ToString());
                    var PickupFee1 = txtPickupFee.Text;
                    foreach (DataRow row in BoxZone.Rows)
                    {
                        Cost = Convert.ToDecimal(row["Cost"].ToString());
                        Price = Cost * Mutliplier;
                        row["Price"] = Convert.ToDecimal(Price.ToString("G29")); 
                        row["PickupFee"] = PickupFee1;
                    }

                    shippingGrd.DataSource = BoxZone;
                    shippingGrd.DataBind();
                    shippingGrd.Visible = true;
                }
            }
            else
            {

                DataTable ds1 = hccshopin.BindGridShippingZoneByZoneID(6);
                if (ds1.Rows.Count > 0)
                {
                    lblZipCodeErrorMsg.Text = "";
                    lblShippingZone.Text = ds1.Rows[0]["ZoneName"].ToString();
                    lblMinShippingFeeOrder.Text = ds1.Rows[0]["MinFee"].ToString();
                    lblMaxShippingFeeOrder.Text = ds1.Rows[0]["MaxFee"].ToString();
                    txtMinShippingFeeOrder.Text = ds1.Rows[0]["MinFee"].ToString();
                    txtMaxShippingFeeOrder.Text = ds1.Rows[0]["MaxFee"].ToString();
                    lblShipMultiplier.Text = ds1.Rows[0]["Multiplier"].ToString();
                    string IsPickup = ds1.Rows[0]["IsPickupShippingZone"].ToString();

                    if (chkIsPickup.Checked == true)
                    {
                        var PickupFee = txtPickupFee.Text;
                        DataTable BoxZones = hccshopin.BindBoxToShippingZoneFee(6);
                        foreach (DataRow row in BoxZones.Rows)
                        {
                            row["PickupFee"] = PickupFee;
                            bool update = hccshopin.UpdatePickupfee(PickupFee, 6);
                        }

                    }
                    DataTable BoxZone = hccshopin.BindBoxToShippingZoneFee(6); //Get Box To Shipping Zone Fee List by ZoneId
                    decimal Cost = 0;
                    decimal Price = 0;
                    decimal Mutliplier = Convert.ToDecimal(ds1.Rows[0]["Multiplier"].ToString());
                    var PickupFee1 = txtPickupFee.Text;
                    foreach (DataRow row in BoxZone.Rows)
                    {
                        Cost = Convert.ToDecimal(row["Cost"].ToString());
                        Price = Cost * Mutliplier;
                        row["Price"] = Convert.ToDecimal(Price.ToString("G29"));
                        row["PickupFee"] = PickupFee1;
                    }
                    shippingGrd.DataSource = BoxZone;
                    shippingGrd.DataBind();
                    shippingGrd.Visible = true;
                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            hccShippingZone hccshopin = new hccShippingZone();
            string txtZipZone = txtShippingZipCode.Text;
            string ddlZone = ddlShippingZone.SelectedValue;
            lblZone.Text= Convert.ToString(Convert.ToInt32(ddlShippingZone.SelectedValue) - 1);
            if (txtZipZone == null || txtZipZone == "")
            {
                Response.Write("<script>alert('Please enter valid zip code')</script>");
            }
            else
            {
                if (ddlZone == null || ddlZone == "")
                {
                    Response.Write("<script>alert('Please enter Zipzone')</script>");
                }
                else
                {
                    int update = hccshopin.UpdateZoneName(txtZipZone, ddlZone);
                    lblUpdateSuccess.Text = "Zipcode updated successfully";
                }
            }
        }
    }
}