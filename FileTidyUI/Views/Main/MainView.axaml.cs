using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using Xilium.CefGlue;
using Xilium.CefGlue.Avalonia;
using Xilium.CefGlue.Common.Handlers;
using FileTidyBase.Controller;
using FileTidyBase.Models;
using System.Xml.Linq;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Layout;
using FileTidyUI.Controls;
using System.ComponentModel;
using ReactiveUI;
using FileTidyUI.Views.Dialogs;
using Avalonia.ReactiveUI;
using System.Windows.Input;
using FileTidyUI.ResultSets;

namespace FileTidyUI.Views.Main;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }


}
