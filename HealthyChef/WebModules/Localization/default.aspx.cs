using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WM = BayshoreSolutions.WebModules;

namespace BayshoreSolutions.WebModules.Cms.Localization
{
    public partial class _Default : System.Web.UI.Page
    {
        protected string _selectedFirstLetter = null;
        protected string _selectedToken = null;
        protected string _cultureCode = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            _selectedFirstLetter = Request.QueryString["tokenFirstLetter"];
            _selectedToken = Request.QueryString["token"];
            _cultureCode = System.Threading.Thread.CurrentThread.CurrentCulture.Name;

            if (null != _selectedFirstLetter && _selectedFirstLetter.Length > 1) throw new ArgumentException("tokenFirstLetter length must be exactly 1.");

            System.Collections.Generic.List<string> letters =
                WM.KeywordTokens.KeywordToken.GetTokenNameFirstLetters();

            if (string.IsNullOrEmpty(_selectedFirstLetter)
                && null != letters
                && letters.Count > 0)
            {
                _selectedFirstLetter = letters[0];
            }

            KeywordTokenNameFirstLettersList.DataSource = letters;
            KeywordTokenNameFirstLettersList.DataBind();

            if (!Page.IsPostBack)
            {
                MultiView1.SetActiveView(listView);

                if (string.IsNullOrEmpty(_selectedToken)
                    && !string.IsNullOrEmpty(_selectedFirstLetter))
                { //show list
                    KeywordTokensList.DataSource =
                        WM.KeywordTokens.KeywordToken.GetByTokenNameFirstChar(
                            _cultureCode,
                            _selectedFirstLetter);

                    KeywordTokensList.DataBind();
                }
                else if (!string.IsNullOrEmpty(_selectedToken))
                { //edit existing token
                    MultiView1.SetActiveView(editView);

                    WM.KeywordTokens.KeywordToken item =
                        WM.KeywordTokens.KeywordToken.GetItem(_selectedToken);

                    if (item != null)
                    { //load values from existing token.
                        EditTokenTextBox.ReadOnly = true;
                        EditTokenTextBox.Enabled = false;
                        EditTokenTextBox.Text = item.Token;
                        EditTokenValueTextBox.Text = item.Value;
                    }
                }
            }
        }

        protected void addButton_Click(object sender, EventArgs e)
        {
            this.MultiView1.SetActiveView(this.editView);
        }

        protected void EditSubmitButton_Click(object sender, EventArgs e)
        {
            WM.KeywordTokens.KeywordToken token =
                WM.KeywordTokens.KeywordToken.GetItem(EditTokenTextBox.Text);

            if (null == token) //create new token.
            {
                token = new WM.KeywordTokens.KeywordToken();
                token.SiteId = 1; //dummy value; not used.
                token.Token = EditTokenTextBox.Text;
                token.Value = EditTokenValueTextBox.Text;

                WM.KeywordTokens.KeywordToken.AddItem(token);
            }
            else //update existing token.
            {
                token.SiteId = 1; //dummy value; not used.
                token.Value = EditTokenValueTextBox.Text;

                WM.KeywordTokens.KeywordToken.UpdateItem(token);
            }

            ShowList();
        }

        protected void KeywordTokensList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Delete_")
            {
                string token = e.CommandArgument.ToString();

                WM.KeywordTokens.KeywordToken.DeleteItem(token);
            }

            ShowList();
        }

        protected void CancelEditButton_Click(object sender, EventArgs e)
        {
            ShowList();
        }

        protected void ShowList()
        {
            Response.Redirect("Default.aspx?tokenFirstLetter="
                + _selectedFirstLetter);
        }

        protected void EditToken()
        {
            Response.Redirect("Default.aspx?tokenFirstLetter="
                + _selectedFirstLetter
                + "&token="
                + _selectedToken);
        }

        protected void ExportButton_Click(object sender, EventArgs e)
        {
            ExportButton.DataSource = WM.KeywordTokens.KeywordToken.GetItems();
        }
    }
}