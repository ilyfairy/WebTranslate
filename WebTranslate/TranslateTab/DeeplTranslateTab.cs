using System;
using System.Collections.Generic;
using System.Text;
using WebTranslate;

namespace Ilyfairy.Tools.WebTranslate.TranslateTab;

public class DeeplTranslateTab : WebTranslateTabBase
{
    public override Uri Uri => new("https://www.deepl.com/translator");

    public async override void Trim()
    {
        //Header
        await WebView.ExecuteScriptAsync("document.querySelector('header')?.remove();");
        await WebView.ExecuteScriptAsync("document.querySelector('.lmt__docTrans-tab-container')?.remove();");
        await WebView.ExecuteScriptAsync("document.querySelector('#lmt_pro_ad_container')?.remove();");
        await WebView.ExecuteScriptAsync("document.querySelector('#dl_quotes_container')?.remove();");
        await WebView.ExecuteScriptAsync("document.querySelector('.dl_footerV2_container')?.remove();");
        await WebView.ExecuteScriptAsync("document.querySelector('#dl_cookieBanner')?.remove();"); //cookies
        await WebView.ExecuteScriptAsync("document.querySelector('.eSEOtericText')?.remove();");
        await WebView.ExecuteScriptAsync("document.querySelector('.lmt__textarea_placeholder_text > div:nth-child(2)').remove();");
        await WebView.ExecuteScriptAsync("document.querySelector('#dl_translator').style.padding = '10px';");
    }

    public async override void FocusInput()
    {
        await WebView.ExecuteScriptAsync("document.querySelector('.lmt__textarea')?.focus();");
    }

    public async override void InputText(string text)
    {
        await WebView.ExecuteScriptAsync($@"
var input = document.querySelector('.lmt__textarea');
var inputEvent = document.createEvent('HTMLEvents');
inputEvent.initEvent('input', true, true);
input.value = '{Utils.UnicodeEncode(text)}';
input.dispatchEvent(inputEvent)
");
    }

    public async override void SwitchLanguage()
    {
        await WebView.ExecuteScriptAsync("document.querySelector('.lmt__language_container_switch').click();");
    }

    public override async Task<string> GetInputText()
    {
        string r = await WebView.ExecuteScriptAsync("document.querySelector('.lmt__textarea').value");
        if (string.IsNullOrWhiteSpace(r) || r.Length <= 2) return "";
        return r[1..^1];
    }
}
