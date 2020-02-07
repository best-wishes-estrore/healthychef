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
using System.Collections.Generic;
using System.IO;

using Bss = BayshoreSolutions.Common;
using BayshoreSolutions.WebModules.Cms.FileManager.Model;

namespace BayshoreSolutions.WebModules.Cms.FileManager.FileList
{
    public partial class Display : BayshoreSolutions.WebModules.WebModuleBase
    {
        protected string _physicalRootPath;
        protected string _virtualRootPath = "file/demos";
        protected FileList_Module _fileListModule;

        protected string GetFullPhysicalPath(string relativePath)
        {
            return Path.Combine(_physicalRootPath, relativePath);
        }

        /// <summary>
        /// Converts an absolute physical path into an absolute virtual path.
        /// </summary>
        protected string GetFullVirtualPath(string fullPhysicalPath)
        {
            string path =
                //remove the root path
                fullPhysicalPath.Replace(_physicalRootPath, string.Empty)
                //convert Windows filesystem path separators into URL path separators.
                .Replace("\\", "/");

            path = Bss.Web.Url.Combine(_virtualRootPath, path);

            return path;
        }

        protected void ShowFiles(string fullPhysicalPath)
        {
            FolderName.Visible = _fileListModule.ShowFolderList;
            if (_fileListModule.ShowFolderList
                && FolderTree.Nodes.Count > 0)
            {
                FolderName.Text = (null == FolderTree.SelectedNode)
                    ? FolderTree.Nodes[0].Text
                    : FolderTree.SelectedNode.Text;
            }

            FileList.DataSource =
                //GetFiles() returns full physical paths, which are then snipped in the markup logic.
                Directory.GetFiles(fullPhysicalPath);
            FileList.DataBind();
            FileList.Visible = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _fileListModule = FileList_Module.Get(this.ModuleId);
            _virtualRootPath = _fileListModule.GetFullVirtualRootPath();
            _physicalRootPath = Server.MapPath(_virtualRootPath).TrimEnd('\\');

            if (!IsPostBack)
            {
                populateNodes(_physicalRootPath);
                ShowFiles(_physicalRootPath);
                FolderTreePanel.Visible = _fileListModule.ShowFolderList;
            }
        }

        protected void CategoryTreeView_SelectedNodeChanged(object sender, EventArgs e)
        {
            //each tree node value stores the relative path.
            ShowFiles(GetFullPhysicalPath(this.FolderTree.SelectedValue));
        }

        private void populateNodes(string physicalPath)
        {
            string nodeText = Path.GetFileName(physicalPath);
            if (physicalPath == Server.MapPath(Settings.FileStorageRootPath).TrimEnd('\\'))
                nodeText = "Files";

            FolderTree.Nodes.Clear();
            TreeNode root = new TreeNode(nodeText, string.Empty);
            FolderTree.Nodes.Add(root);
            PopulateNode(root);
        }

        /// <summary>
        /// Recursively populates the tree.
        /// </summary>
        private void PopulateNode(TreeNode parentNode)
        {
            string path = GetFullPhysicalPath(parentNode.Value);

            foreach (string dir in Directory.GetDirectories(path))
            {
                //friendly name to display.
                string name = Path.GetFileName(dir);

                //physical path relative to the physical root path.
                string value =
                    //remove the root path
                    dir.Replace(_physicalRootPath, string.Empty)
                    //remove the beginning slash so the path isn't interpreted as an absolute path.
                    .TrimStart('\\');

                TreeNode node = new TreeNode(name, value);
                parentNode.ChildNodes.Add(node);

                //recurse
                PopulateNode(node);
            }
        }
    }
}
