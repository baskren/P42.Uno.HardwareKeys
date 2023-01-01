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

namespace P42.Uno.HardwareKeys
{
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

            var type = control.GetType();

            if (type.GetProperty("QueryText") is PropertyInfo queryTextProperty &&
                queryTextProperty.CanWrite)
                return true;

            if (type.GetProperty("Text") is PropertyInfo textProperty &&
                textProperty.CanWrite)
            {
                if (type.GetProperty("IsReadOnly") is PropertyInfo readOnlyProperty)
                {
                    if (readOnlyProperty.CanRead &&
                        readOnlyProperty.PropertyType == typeof(bool))
                    {
                        return !(bool)readOnlyProperty.GetValue(control);
                    }
                }

                if (type.GetProperty("IsEditable") is PropertyInfo editableProperty &&
                    editableProperty.CanRead &&
                    editableProperty.PropertyType == typeof(bool))
                {
                    return (bool)editableProperty.GetValue(control);
                }

                if (type.GetProperty("IsTextSearchEnabled ") is PropertyInfo textSearchProperty &&
                    textSearchProperty.CanRead &&
                    textSearchProperty.PropertyType == typeof(bool))
                {
                    return (bool)textSearchProperty.GetValue(control);
                }
            }


            return false;
        }
    }
}