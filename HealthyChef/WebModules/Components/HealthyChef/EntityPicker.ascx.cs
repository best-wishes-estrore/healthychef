using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Objects.DataClasses;
using System.Reflection;
using HealthyChef.Common;
using HealthyChef.Common.Events;
using HealthyChef.DAL;

namespace HealthyChef.WebModules.Components.HealthyChef
{
    public partial class EntityPicker : System.Web.UI.UserControl
    {
        public event ItemsSelectionChangedHandler ItemsSelectionChanged;

        protected void OnItemsSelectedChanged()
        {
            if (ItemsSelectionChanged != null)
                ItemsSelectionChanged(this, new ItemsSelectionChangedEventArgs(lstSelectedItems));
        }

        protected void OnDataTextFieldChanged(string dataTextField)
        {
            lstAvailableItems.DataTextField = dataTextField;
            lstSelectedItems.DataTextField = dataTextField;
        }

        protected void OnDataValueFieldChanged(string dataValueField)
        {
            lstAvailableItems.DataValueField = dataValueField;
            lstSelectedItems.DataValueField = dataValueField;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            btnDeselctSelected.Command += new CommandEventHandler(ShiftItems_Command);
            btnDeselectAll.Command += new CommandEventHandler(ShiftItems_Command);
            btnSelectAll.Command += new CommandEventHandler(ShiftItems_Command);
            btnSelectSelected.Command += new CommandEventHandler(ShiftItems_Command);

            lstSelectedItems.DataBound += lstSelectedItems_DataBound;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private ListSelectionMode _SelectionMode = ListSelectionMode.Multiple;

        [PersistenceMode(PersistenceMode.Attribute)]
        public ListSelectionMode SelectionMode
        {
            get { return _SelectionMode; }
            set
            {
                _SelectionMode = value;
                lstAvailableItems.SelectionMode = value;
                lstSelectedItems.SelectionMode = value;

                if (value == ListSelectionMode.Single)
                {
                    btnSelectAll.Visible = false;
                    btnDeselectAll.Visible = false;
                }
            }
        }

        private string _DataTextField;
        private string _DataValueField;

        [PersistenceMode(PersistenceMode.Attribute)]
        public string DataTextField
        {
            get { return _DataTextField; }
            set
            {
                _DataTextField = value;
                OnDataTextFieldChanged(value);
            }
        }

        [PersistenceMode(PersistenceMode.Attribute)]
        public string DataValueField
        {
            get { return _DataValueField; }
            set
            {
                _DataValueField = value;
                OnDataValueFieldChanged(value);
            }
        }

        [PersistenceMode(PersistenceMode.Attribute)]
        public string Title
        {
            get;
            set;
        }

        public Type EntityType
        {
            get
            {
                if (ViewState["EntityType"] == null)
                    ViewState["EntityType"] = null;

                return (Type)ViewState["EntityType"];
            }
            set
            {
                ViewState["EntityType"] = value;
            }
        }

        public void Bind<T>(List<T> allItemsSet, List<T> selectedItemsSet)
        {
            EntityType = typeof(T);
            lblTitle.Text = Title;
            lstAvailableItems.DataSource = allItemsSet;
            lstSelectedItems.DataSource = selectedItemsSet;

            lstAvailableItems.DataBind();
            lstSelectedItems.DataBind();

            List<ListItem> allItems = lstAvailableItems.Items.OfType<ListItem>().ToList();
            List<string> selectedItems = lstSelectedItems.Items.OfType<ListItem>().Select(a => a.Value).ToList();

            allItems.ForEach(a => a.Enabled = !selectedItems.Contains(a.Value));
        }

        void lstSelectedItems_DataBound(object sender, EventArgs e)
        {
            ListBox lstSelectedItems = (ListBox)sender;
            List<ListItem> items = lstSelectedItems.Items.OfType<ListItem>().ToList();

            items.ForEach(delegate(ListItem item)
            {
                if (EntityType == typeof(hccMenuItem))
                {
                    hccMenuItem t1 = hccMenuItem.GetById(int.Parse(item.Value));
                    if (t1.IsRetired) { item.Attributes.Add("class", "retired"); }
                    else item.Attributes.Add("class", "active");

                    List<hccIngredient> ings = t1.GetIngredients();

                    ings.ForEach(delegate(hccIngredient ing) { if (ing.IsRetired) { item.Attributes.Add("class", "retired_child"); } });
                }
                else if (EntityType == typeof(hccPreference))
                {
                    hccPreference t1 = hccPreference.GetById(int.Parse(item.Value));
                    if (t1.IsRetired) { item.Text += " - Retired"; item.Attributes.Add("class", "retired"); }
                    else item.Attributes.Add("class", "active");
                }
                else if (EntityType == typeof(hccAllergen))
                {
                    hccAllergen t1 = hccAllergen.GetById(int.Parse(item.Value));
                    if (!t1.IsActive) { item.Text += " - Retired"; item.Attributes.Add("class", "retired"); }
                    else item.Attributes.Add("class", "active");
                }
                else if (EntityType == typeof(hccIngredient))
                {
                    hccIngredient t1 = hccIngredient.GetById(int.Parse(item.Value));
                    if (t1.IsRetired) { item.Text += " - Retired"; item.Attributes.Add("class", "retired"); }
                    else item.Attributes.Add("class", "active");
                }
                else
                {//do nothing
                }
            });
        }

        protected void ShiftItems_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "DeselectAll")
            {
                OnDeselectAll();
            }
            else if (e.CommandName == "DeselectSelected")
            {
                OnDeselectSelected();
            }
            else if (e.CommandName == "SelectAll")
            {
                OnSelectAll();
            }
            else if (e.CommandName == "SelectSelected")
            {
                OnSelectSelected();
            }
        }

        protected void OnDeselectAll()
        {
            lstSelectedItems.Items.Clear();
            lstAvailableItems.Items.OfType<ListItem>().ToList().ForEach(a => a.Enabled = true);
        }

        protected void OnDeselectSelected()
        {
            List<ListItem> selectedItems = GetSelectedItems(lstSelectedItems);
            var selectedItemValues = selectedItems.Select(a => a.Value);

            lstAvailableItems.Items.OfType<ListItem>().Where(a => selectedItemValues.Contains(a.Value)).ToList().ForEach(a => a.Enabled = true);
            selectedItems.ForEach(a => lstSelectedItems.Items.Remove(a));
        }

        protected void OnSelectAll()
        {
            List<ListItem> items = lstAvailableItems.Items.OfType<ListItem>().Where(a => a.Enabled).ToList();

            foreach (var item in items)
            {
                lstSelectedItems.Items.Add(new ListItem(item.Text, item.Value));
                item.Enabled = false;
            }
        }

        protected void OnSelectSelected()
        {
            List<ListItem> items = GetSelectedItems(lstAvailableItems);

            foreach (var item in items)
            {
                lstSelectedItems.Items.Add(new ListItem(item.Text, item.Value));
                item.Enabled = false;
            }
        }

        protected List<ListItem> GetSelectedItems(ListControl list)
        {
            return list.Items.OfType<ListItem>().Where(a => a.Selected).ToList();
        }

        public List<int> GetSelectedKeys()
        {
            return lstSelectedItems.Items.OfType<ListItem>().Select(a => int.Parse(a.Value)).ToList();
        }

        public void Reset()
        {
            if (lstAvailableItems.Items.Count > 0)
            {
                lstAvailableItems.Items.OfType<ListItem>().ToList().ForEach(a => a.Enabled = true);
            }

            lstSelectedItems.Items.Clear();
        }
    }
}