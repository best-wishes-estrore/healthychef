using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace HealthyChef.Common.Events
{
    public class CartEventArgs : EventArgs
    {
        public int CartId { get; set; }

        public CartEventArgs(int cartId)
        {
            CartId = cartId;
        }
    }

    public class DateChangedEventArgs : EventArgs
    {
        public DateTime? PreviousDate { get; set; }
        public DateTime? NewDate { get; set; }
    }

    public class ItemsSelectionChangedEventArgs : EventArgs
    {
        public List<ListItem> SelectedItems
        {
            get;
            private set;
        }

        public ItemsSelectionChangedEventArgs(ListControl list)
        {
            SelectedItems = list.Items.OfType<ListItem>().ToList();
        }
    }

    public class CardInfoSaveFailedEventArgs : EventArgs
    {
        public Exception FailException { get; set; }

        public CardInfoSaveFailedEventArgs(Exception ex)
        {
            FailException = ex;
        }
    }

    public class PasswordResetEventArgs : EventArgs
    {
        public bool IsSuccessful { get; set; }

        public PasswordResetEventArgs(bool isSuccess)
        {
            IsSuccessful = isSuccess;
        }
    }

    public class ControlSavedEventArgs : EventArgs
    {
        public object PrimaryKeyIndex { get; set; }

        public ControlSavedEventArgs(object primaryKeyIndex)
        {
            PrimaryKeyIndex = primaryKeyIndex;
        }
    }
}