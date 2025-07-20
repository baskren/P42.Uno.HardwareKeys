/*******************************************************************
 *
 * Common source for .net bait, mac, tizen, windows, wasm, and skia
 *
 * IF YOU MAKE A CHANGE HERE, BE SURE TO UPDATE ALL OF THE ABOVE!
 *
 **********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System.Threading.Tasks;

namespace P42.Uno.HardwareKeys;

partial class CoreListener : Button
{
    /*
    * 
    * NOTES:
    * 
    * numpad 5 : clear
    * 
    * Keys that don't OnKeyDown
    *      WASM: HOME, END, LEFT, RIGHT 
    *      UWP: INSERT, DELETE 
    *      WPF: HOME, END, LEFT, RIGHT, F10 (up[`]) 
    *      GtK: HOME, END, LEFT, RIGHT
    *      
    * Keys that are not right
    *      WASM:
    *      UWP:
    *      WPF: CONTROL+, WINDOWS+, MENU+
    *      
    * 
    */

    TextBox _textBox;

    void PlatformBuild()
    {
        Content = _platformCoreElement = _textBox = new TextBox();
        _textBox.Name = "HardwareKeys.CoreListener";
        //_textBox.TextChanged += _textBox_TextChanged;
        _textBox.BeforeTextChanging += _textBox_BeforeTextChanging;
        _textBox.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Black);
        _textBox.TextAlignment = TextAlignment.Justify;
        HorizontalAlignment = HorizontalAlignment.Stretch;
        Margin = new Thickness(5);
        Padding = new Thickness(5);
        //_textBox.Foreground = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));

        Task.Run(async () =>
        {
            try
            {
                while (_textBox != null)
                {
                    PlatformKeyQuery();
                    DispatcherQueue.TryEnqueue(() =>
                    {
                        //System.Diagnostics.Debug.WriteLine($"XXXXXX:[{InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Shift)}]");
                    });
                    await Task.Delay(1000);

                }
            }
            catch (Exception e) 
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        });

    }

    void PlatformKeyQuery()
    {
        PlatformNumLockQuery();
        PlatformCapsLockQuery();
        //PlatformShiftPressedQuery();
        PlatformControlPressedQuery();
        PlatformMenuPressedQuery();
        PlatformWindowsPressedQuery();
    }

    partial void PlatformNumLockQuery()
        => IsNumLockEngaged = WinUIKeyEngaged(VirtualKey.NumberKeyLock, CoreVirtualKeyStates.Locked);

    partial void PlatformCapsLockQuery()
        => IsCapsLockEngaged = WinUIKeyEngaged(VirtualKey.CapitalLock, CoreVirtualKeyStates.Locked);

    partial void PlatformShiftPressedQuery()
        => IsShiftPressed = WinUIKeyEngaged(VirtualKey.Shift, CoreVirtualKeyStates.Down);

    partial void PlatformControlPressedQuery()
        => IsControlPressed = WinUIKeyEngaged(VirtualKey.Control, CoreVirtualKeyStates.Down);

    partial void PlatformMenuPressedQuery()
        => IsMenuPressed = WinUIKeyEngaged(VirtualKey.Menu, CoreVirtualKeyStates.Down);

    partial void PlatformWindowsPressedQuery()
        => IsWindowsPressed = WinUIKeyEngaged(VirtualKey.LeftWindows, VirtualKey.RightWindows, CoreVirtualKeyStates.Down);


    protected override void OnGotFocus(RoutedEventArgs e)
    {
        base.OnGotFocus(e);
        //FocusManager.TryFocusAsync(_textBox, FocusState.Programmatic);
        TryFocusAsync(_textBox, FocusState.Programmatic).Forget();

        //PlatformKeyQuery();
    }

    string _lastTextEntered = string.Empty;

    readonly Dictionary<VirtualKey, string> FatFingerBuffer = new Dictionary<VirtualKey, string>();
    private void _textBox_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
    {
        System.Diagnostics.Debug.WriteLine($"TEXT: [{args.NewText}]");

#if WINDOWS
        if (_waitingForText && !string.IsNullOrEmpty(args.NewText))
        {
            //var key = _textBox.Text.Substring(_textBox.Text.Length - 1);
            FatFingerBuffer[_lastKeyDown] = args.NewText;
            OnSimpleKeyDown(args.NewText, _lastKeyDown);
            _lastKeyDown = VirtualKey.None;
        }
#else
        FatFingerBuffer[_lastKeyDown] = args.NewText;
        _lastTextEntered = args.NewText;
#endif
        args.Cancel = true;
    }

    VirtualKey _lastKeyDown;
    bool _waitingForText;
    protected override void OnKeyDown(KeyRoutedEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine($"DOWN: [{e.Key}]");
        System.Diagnostics.Debug.WriteLine($"   e.Key[{e.Key}]  modifiers[{string.Join(',', CurrentModifiers)}]");

        _lastKeyDown = e.Key;
        base.OnKeyDown(e);

        if (IsCharacterKey(e.Key))
        {
#if WINDOWS
            _waitingForText = true;
            //System.Diagnostics.Debug.Write(" ... ");
#else
            OnSimpleKeyDown(_lastTextEntered, _lastKeyDown);
            _lastKeyDown = VirtualKey.None;
#endif
        }
        else
        {
            e.Handled = true;

            if (ProcessModifier(e.Key, true))
            {
                if (!MuteModifiers)
                    OnSimpleKeyDown(string.Empty, e.Key);
            }
            else
                OnSimpleKeyDown(string.Empty, e.Key);
        }

    }

    protected override void OnKeyUp(KeyRoutedEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine($"UP: [{e.Key}]");

        var modifiers = CurrentModifiers;
        base.OnKeyUp(e);

        /*
        var key = _textBox.Text;
        if (string.IsNullOrEmpty(key))
        {
            System.Diagnostics.Debug.WriteLine("A");
            if (FatFingerBuffer.TryGetValue(e.Key, out var txt))
                key = txt;
        }
        else
            key = key.Substring(key.Length - 1);
        */
#if WINDOWS
        FatFingerBuffer.TryGetValue(e.Key, out var key);
#else
        var key = _lastTextEntered;
#endif
        System.Diagnostics.Debug.WriteLine($"   e.Key[{e.Key}] key[{key}] modifiers[{string.Join(',', CurrentModifiers)}]");

        if (ProcessModifier(e.Key, false))
        {
            if (!MuteModifiers)
                OnSimpleKeyUp(key, e.Key, modifiers);
        }
        else
            OnSimpleKeyUp(key, e.Key, modifiers);

        _lastTextEntered = string.Empty;
        //_textBox.Text = String.Empty;
    }

}
