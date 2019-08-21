using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Data;


using BayshoreSolutions.WebModules.ContentModule;
using BayshoreSolutions.WebModules.ContentModule.DataAccessLayerTableAdapters;
using System.Collections.ObjectModel;

namespace BayshoreSolutions.WebModules.ContentModule
{
    class Content
    {

        public static Content GetActiveContentForDisplay(int moduleId, string culture)
        {
            ContentTableAdapter contentAdapter = new ContentTableAdapter();
            DataAccessLayer.ContentDataTable contentTable;

            contentTable = contentAdapter.GetActiveContent(moduleId, culture);
            if (contentTable.Rows.Count > 0)
            {
                DataAccessLayer.ContentRow activeContentRow = (DataAccessLayer.ContentRow)contentTable.Rows[0];
                Content content = new Content();
                content.ContentVersionId = activeContentRow.ContentVersionId;
                content.ModuleId = activeContentRow.ModuleId;
                content.Culture = activeContentRow.Culture;
                content.StatusId = activeContentRow.StatusId;
                content.Modified = activeContentRow.Modified;
                content.Text = activeContentRow.Text;
                return content;
            }
            else
            {
                return null;
            }

        }
        public static Content GetActiveContent(int moduleId, string culture)
        {
            ContentTableAdapter contentAdapter = new ContentTableAdapter();
            DataAccessLayer.ContentDataTable contentTable;

            contentTable = contentAdapter.GetContentByStatusId(2, moduleId, culture);
            if (contentTable.Rows.Count > 0)
            {
                DataAccessLayer.ContentRow activeContentRow = (DataAccessLayer.ContentRow)contentTable.Rows[0];
                Content content = new Content();
                content.ContentVersionId = activeContentRow.ContentVersionId;
                content.ModuleId = activeContentRow.ModuleId;
                content.Culture = activeContentRow.Culture;
                content.StatusId = activeContentRow.StatusId;
                content.Modified = activeContentRow.Modified;
                content.Text = activeContentRow.Text;
                return content;
            }
            else
            {
                return null;
            }

        }
        public static Content GetPendingContent(int moduleId, string culture)
        {
            ContentTableAdapter contentAdapter = new ContentTableAdapter();
            DataAccessLayer.ContentDataTable contentTable;

            contentTable = contentAdapter.GetContentByStatusId(1, moduleId, culture);
            if (contentTable.Rows.Count > 0)
            {
                DataAccessLayer.ContentRow activeContentRow = (DataAccessLayer.ContentRow)contentTable.Rows[0];
                Content content = new Content();
                content.ContentVersionId = activeContentRow.ContentVersionId;
                content.ModuleId = activeContentRow.ModuleId;
                content.Culture = activeContentRow.Culture;
                content.StatusId = activeContentRow.StatusId;
                content.Modified = activeContentRow.Modified;
                content.Text = activeContentRow.Text;
                return content;
            }
            else
            {
                return null;
            }

        }

        #region Private Fields and Public Properties
        private int _contentVersionId;
        private int _moduleId;
        private string _culture;
        private int _statusId;
        private DateTime _modified;
        private string _text;

        public int ContentVersionId
        {
            get { return _contentVersionId; }
            set { _contentVersionId = value; }
        }
        public int ModuleId
        {
            get { return _moduleId; }
            set { _moduleId = value; }
        }
        public string Culture
        {
            get { return _culture; }
            set { _culture = value; }
        }
        public int StatusId
        {
            get { return _statusId; }
            set { _statusId = value; }
        }
        public DateTime Modified
        {
            get { return _modified; }
            set { _modified = value; }
        }
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        #endregion

        public static void UpdateContent(Content contentObject)
        {
            UpdateContent(contentObject.ContentVersionId, contentObject.ModuleId, contentObject.Culture, contentObject.StatusId, contentObject.Modified, contentObject.Text);
        }
        public static void UpdateContent(int contentVersionId, int moduleId, string culture, int statusId, DateTime modified, string text)
        {
            ContentTableAdapter contentAdapter = new ContentTableAdapter();

            DataAccessLayer.ContentDataTable contentTable = contentAdapter.GetContentByContentVersionId(contentVersionId);
            if (contentTable.Rows.Count != 1)
            {
                throw new Exception("GetContentByContentVersionId did not return 1 row.");
            }

            DataAccessLayer.ContentRow contentRow = (DataAccessLayer.ContentRow)contentTable.Rows[0];
            contentRow.ModuleId = moduleId;
            contentRow.Modified = modified;
            contentRow.StatusId = statusId;
            contentRow.Culture = culture;
            contentRow.Text = text;

            contentAdapter.Update(contentRow);
        }
        public void UpdateContent()
        {
            UpdateContent(this);
        }

        public static void CreateContent(Content contentObject)
        {
            CreateContent(contentObject.ModuleId, contentObject.Culture, contentObject.StatusId, contentObject.Modified, contentObject.Text);
        }
        public static void CreateContent(int moduleId, string culture, int statusId, DateTime modified, string text)
        {
            ContentTableAdapter contentAdapter = new ContentTableAdapter();

            DataAccessLayer.ContentDataTable contentTable = new DataAccessLayer.ContentDataTable();
            DataAccessLayer.ContentRow contentRow = contentTable.NewContentRow();
            contentRow.ModuleId = moduleId;
            contentRow.Modified = modified;
            contentRow.StatusId = statusId;
            contentRow.Culture = culture;
            contentRow.Text = text;
            //contentRow.RowState = DataRowState.Added;

            contentTable.AddContentRow(contentRow);

            contentAdapter.Update(contentTable);
        }
        public void CreateContent()
        {
            CreateContent(this);
        }

        public static Collection<Content> GetArchivedContent(int moduleId, string culture)
        {
            ContentTableAdapter contentAdapter = new ContentTableAdapter();
            DataAccessLayer.ContentDataTable contentTable = contentAdapter.GetContentByStatusId(3, moduleId, culture);
            Collection<Content> archivedContent = new Collection<Content>();

            foreach (DataRow dataRow in contentTable.Rows)
            {
                DataAccessLayer.ContentRow contentRow = (DataAccessLayer.ContentRow)dataRow;
                Content contentObject = new Content();
                contentObject.ContentVersionId = contentRow.ContentVersionId;
                contentObject.ModuleId = contentRow.ModuleId;
                contentObject.StatusId = contentRow.StatusId;
                contentObject.Culture = contentRow.Culture;
                contentObject.Text = contentRow.Text;
                contentObject.Modified = contentRow.Modified;

                archivedContent.Add(contentObject);
            }

            return archivedContent;
        }
        public static Content GetContentByContentVersionId(int contentVersionId)
        {
            ContentTableAdapter contentAdapter = new ContentTableAdapter();
            DataAccessLayer.ContentDataTable contentTable = contentAdapter.GetContentByContentVersionId(contentVersionId);
            Content archivedContent = new Content();

            if (contentTable.Rows.Count == 0)
                return null;

            DataAccessLayer.ContentRow contentRow = (DataAccessLayer.ContentRow)contentTable.Rows[0];

            archivedContent.ContentVersionId = contentRow.ContentVersionId;
            archivedContent.ModuleId = contentRow.ModuleId;
            archivedContent.StatusId = contentRow.StatusId;
            archivedContent.Culture = contentRow.Culture;
            archivedContent.Text = contentRow.Text;
            archivedContent.Modified = contentRow.Modified;

            return archivedContent;
        }

    }
}
