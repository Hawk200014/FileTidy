using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xilium.CefGlue;
using Xilium.CefGlue.Avalonia;
using Xilium.CefGlue.Common.Handlers;

namespace FileTidyUI.Helper;

public class BrowserLifeSpanHandler : LifeSpanHandler
{
    protected override bool OnBeforePopup(
        CefBrowser browser,
        CefFrame frame,
        string targetUrl,
        string targetFrameName,
        CefWindowOpenDisposition targetDisposition,
        bool userGesture,
        CefPopupFeatures popupFeatures,
        CefWindowInfo windowInfo,
        ref CefClient client,
        CefBrowserSettings settings,
        ref CefDictionaryValue extraInfo,
        ref bool noJavascriptAccess)
    {
        var bounds = windowInfo.Bounds;
        Dispatcher.UIThread.Post(() =>
        {
            var window = new Window();
            var popupBrowser = new AvaloniaCefBrowser();
            popupBrowser.Address = targetUrl;
            window.Content = popupBrowser;
            window.Position = new PixelPoint(bounds.X, bounds.Y);
            window.Height = bounds.Height;
            window.Width = bounds.Width;
            window.Title = targetUrl;
            window.Show();
        });
        return true;
    }
}
