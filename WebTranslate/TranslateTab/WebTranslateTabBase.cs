using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ilyfairy.Tools.WebTranslate.TranslateTab;

public abstract class WebTranslateTabBase
{
    public WebTranslateTabBase()
    {
        WebView = new();
        Init();
    }

    private async void Init()
    {
        WebView.Dock = DockStyle.Fill;
        await WebView.EnsureCoreWebView2Async();
        WebView.KeyDown += WebView_KeyDown; ;
        WebView.NavigationStarting += WebView_NavigationStarting;
        WebView.NavigationCompleted += WebView_NavigationCompleted;
        WebView.NavigateToString("<span>loading...<span>");
        WebView.Source = Uri;
    }

    private void WebView_KeyDown(object? sender, KeyEventArgs e)
    {
        KeyDown?.Invoke(this, e);
    }

    private void WebView_NavigationStarting(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs e)
    {
        if (WebView.Source.Host != Uri.Host)
        {
            WebView.Source = Uri;
        }
    }

    private void WebView_NavigationCompleted(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
    {
        Trim();
    }

    public event EventHandler<KeyEventArgs>? KeyDown;
    public WebView2 WebView { get; }
    public abstract Uri Uri { get; }
    public abstract void Trim();
    public abstract void FocusInput();
    public abstract void Input(string text);
    public abstract void SwitchLanguage();
}
