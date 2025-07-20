using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.System;
using Windows.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Input;
using System.Threading.Tasks;
using System.Reflection;

namespace P42.Uno.HardwareKeys;

static class FrameworkExtensions
{
    public static bool IsTextEditable(this Control control)
    {
        if (control == null)
            return false;

        if (!control.IsEnabled)
            return false;

        if (control.Visibility == Visibility.Collapsed) 
            return false;

        if (control is TextBox textBox)
            return !textBox.IsReadOnly;

        if (control is RichEditBox rBox)
            return !rBox.IsReadOnly;

        if (control is ComboBox cBox)
            return cBox.IsEditable || cBox.IsTextSearchEnabled;

        if (control is AutoSuggestBox aBox)
            return true;

        return false;
    }
}
