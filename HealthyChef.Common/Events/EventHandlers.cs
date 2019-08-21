using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthyChef.Common.Events
{
    public delegate void CartSavedEventHandler(object sender, CartEventArgs e);
    public delegate void DateChangedEventHandler(object sender, DateChangedEventArgs e);
    public delegate void ItemsSelectionChangedHandler(object sender, ItemsSelectionChangedEventArgs e);
    public delegate void CartItemListItemUpdatedEventHandler();
    public delegate void CardInfoSaveFailedEventHandler(object sender, CardInfoSaveFailedEventArgs e);
    public delegate void PasswordResetEventHandler(object sender, PasswordResetEventArgs e);
    public delegate void ControlSavedEventHandler(object sender, ControlSavedEventArgs e);
    public delegate void ControlCancelledEventHandler(object sender);
}
