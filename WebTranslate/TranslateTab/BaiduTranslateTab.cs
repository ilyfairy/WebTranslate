using System;
using System.Collections.Generic;
using System.Text;
using WebTranslate;

namespace Ilyfairy.Tools.WebTranslate.TranslateTab;

public class BaiduTranslateTab : WebTranslateTabBase
{
    public override Uri Uri => new("https://fanyi.baidu.com/");

    public async override void Trim()
    {
        await WebView.ExecuteScriptAsync("document.querySelector('#header').remove();"); //顶栏
        await WebView.ExecuteScriptAsync("document.querySelector('.hot-link-out-container').remove();");
        await WebView.ExecuteScriptAsync("document.querySelector('.footer').remove();"); //最底栏
        await WebView.ExecuteScriptAsync("document.querySelector('#main-outer > div > div > div.translate-wrap').style.margin = '10px';");
        await WebView.ExecuteScriptAsync("document.querySelector('#main-outer > div > div').classList.remove('inner');");
        await WebView.ExecuteScriptAsync("document.querySelector('.domain-trans-wrapper').remove();"); //通用按钮
        await WebView.ExecuteScriptAsync("document.querySelector('.translate-setting').remove();");
        //await WebView.ExecuteScriptAsync("document.querySelector('.trans-right trans-other-right').remove();"); //右边广告
        await WebView.ExecuteScriptAsync("document.querySelector('body > div.container').style.width = 'auto';"); //去除小大小,自适应大小
        await WebView.ExecuteScriptAsync("document.querySelector('.trans-operation-wrapper').style.margin = '0 0 10px 0';");
        //await WebView.ExecuteScriptAsync("document.querySelector('.trans-other-wrap').remove();"); //子翻译
        await WebView.ExecuteScriptAsync("document.querySelector('.side-nav-wrapper').remove();"); //子翻译列表
        await WebView.ExecuteScriptAsync(@"
if((typeof window.isWhile) == 'undefined'){
    window.isWhile = true;
    setInterval(()=>{
    try{
        document.querySelector('#app-read')?.remove();
        document.querySelector('.hot-link-middle')?.remove();
        document.querySelector('.op-correct')?.remove(); //报错
        document.querySelector('.note-expand-btn')?.remove(); //笔记
        document.querySelector('#sideBannerContainer').remove(); //右边广告
        let ses = document.querySelectorAll('#left-result-container > section'); //session
        if(ses.length != 0){
            ses.forEach(v=>v.remove())
        }
    }catch(e){}
    },200);
}
"); //子翻译小广告
        await WebView.ExecuteScriptAsync("document.querySelector('.app-guide')?.remove();");
        await WebView.ExecuteScriptAsync("document.querySelector('.op-favor-container')?.remove();"); // 收藏夹
        await WebView.ExecuteScriptAsync("document.querySelector('.app-side-link')?.remove();"); // 广告
    }

    public async override void FocusInput()
    {
        await WebView.ExecuteScriptAsync("document.querySelector('#baidu_translate_input').focus();");
    }

    public async override void InputText(string text)
    {
        await WebView.ExecuteScriptAsync($@"
var input = document.querySelector('#baidu_translate_input');
var inputEvent = document.createEvent('HTMLEvents');
inputEvent.initEvent('input', true, true);
input.value = '{Utils.UnicodeEncode(text)}';
input.dispatchEvent(inputEvent)
");
    }

    public async override void SwitchLanguage()
    {
        await WebView.ExecuteScriptAsync("document.querySelector('.exchange-mask').click();");
    }

    public override async Task<string> GetInputText()
    {
        string r = await WebView.ExecuteScriptAsync("document.querySelector('#baidu_translate_input').value");
        if (string.IsNullOrWhiteSpace(r) || r.Length <= 2) return "";
        return r[1..^1];
    }
}