using System;
using System.Collections.Generic;
using System.Text;
using WebTranslate;

namespace Ilyfairy.Tools.WebTranslate.TranslateTab;

public class GoogleTranslateTab : WebTranslateTabBase
{
    public override Uri Uri => new("https://translate.google.cn/");

    public async override void Trim()
    {
        //顶栏空白
        await WebView.ExecuteScriptAsync("document.querySelector('body > div > header')?.parentElement?.remove();");
        //顶栏  当返回上一级时,会重新出现空白
        await WebView.ExecuteScriptAsync("var tmp = document.querySelector('body > c-wiz > div > div');if(tmp.innerHTML == '') tmp.remove();delete tmp;");
        //去除 文字,文档
        await WebView.ExecuteScriptAsync("document.querySelector('body > c-wiz > div > div > c-wiz > div > nav')?.parentElement?.remove();");
        //去除帮助
        await WebView.ExecuteScriptAsync("var tmp2 = document.querySelector('body > c-wiz > div > div > c-wiz > div:nth-child(3)');if(tmp2.children.length == 0) tmp2.remove();delete tmp2;");
        //去除语音
        await WebView.ExecuteScriptAsync("document.querySelector('body > c-wiz > div > div > c-wiz > div > c-wiz > div > div > div > c-wiz > div > div > c-wiz > span > div > div > span > button > div')?.parentElement?.parentElement?.parentElement?.remove();");
        await WebView.ExecuteScriptAsync("document.body.style.overflowY = 'auto'"); //自动垂直滚动条
    }

    public async override void FocusInput()
    {
        await WebView.ExecuteScriptAsync("document.querySelector('body > c-wiz > div > div > c-wiz > div > c-wiz > div > div > div > c-wiz > span > span > div > textarea')?.focus();");
    }

    public async override void InputText(string text)
    {
        await WebView.ExecuteScriptAsync($@"
var input = document.querySelector('body > c-wiz > div > div > c-wiz > div > c-wiz > div > div > div > c-wiz > span > span > div > textarea');
var inputEvent = document.createEvent('HTMLEvents');
inputEvent.initEvent('input', true, true);
input.value = '{Utils.UnicodeEncode(text)}';
input.dispatchEvent(inputEvent)
");
    }

    public async override void SwitchLanguage()
    {
        await WebView.ExecuteScriptAsync("document.querySelector('html > body > c-wiz > div > div > c-wiz > div > c-wiz > div > div > c-wiz > div > c-wiz > div > div > span > button > div').click();");
    }

    public override async Task<string> GetInputText()
    {
        string r = await WebView.ExecuteScriptAsync("document.querySelector('body > c-wiz > div > div > c-wiz > div > c-wiz > div > div > div > c-wiz > span > span > div > textarea').value");
        if (string.IsNullOrWhiteSpace(r) || r.Length <= 2) return "";
        return r[1..^1];
    }
}
