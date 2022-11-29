using System;
using System.Collections.Generic;
using System.Text;
using WebTranslate;

namespace Ilyfairy.Tools.WebTranslate.TranslateTab;

public class BingTranslateTab : WebTranslateTabBase
{
    public override Uri Uri => new("https://www.bing.com/Translator");

    public async override void Trim()
    {
        await WebView.ExecuteScriptAsync("document.querySelector('.desktop_header')?.remove();");
        await WebView.ExecuteScriptAsync("document.querySelector('.desktop_header_menu')?.remove();");
        await WebView.ExecuteScriptAsync("document.querySelector('footer')?.remove();");
        await WebView.ExecuteScriptAsync("document.querySelector('#theader_z')?.remove();");
        await WebView.ExecuteScriptAsync("document.querySelector('body').style.minWidth = 'auto';");
        await WebView.ExecuteScriptAsync("document.querySelector('body').style.minWidth = 'auto';");
        await WebView.ExecuteScriptAsync("document.querySelector('#tt_translatorHome').style.width = '95vw';");
    
    }

    public async override void FocusInput()
    {
        await WebView.ExecuteScriptAsync("document.querySelector('#tta_input_ta')?.focus();");
    }

    public async override void InputText(string text)
    {
        await WebView.ExecuteScriptAsync($@"
var input = document.querySelector('#tta_input_ta');
var inputEvent = document.createEvent('HTMLEvents');
inputEvent.initEvent('input', true, true);
input.value = '{Utils.UnicodeEncode(text)}';
input.dispatchEvent(inputEvent)
");
    }

    public async override void SwitchLanguage()
    {
        await WebView.ExecuteScriptAsync("document.querySelector('tta_revIcon').click();");
    }

    public override async Task<string> GetInputText()
    {
        string r = await WebView.ExecuteScriptAsync("document.querySelector('#tta_input_ta').value");
        if (r is "null" && string.IsNullOrWhiteSpace(r) || r.Length <= 2) return "";
        return r[1..^1];
    }
}
