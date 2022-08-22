using Microsoft.Web.WebView2.WinForms;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ilyfairy.Tools.WebTranslate;

public static class WebViewEx
{
    public static async Task GoogleTranslateBeautify(this WebView2 web)
    {
        //顶栏空白
        await web.ExecuteScriptAsync("document.querySelector('body > div > header')?.parentElement?.remove();");
        //顶栏
        await web.ExecuteScriptAsync("var tmp = document.querySelector('body > c-wiz > div > div');if(tmp.innerHTML == '') tmp.remove();");
        //去除 文字,文档
        await web.ExecuteScriptAsync("document.querySelector('body > c-wiz > div > div > c-wiz > div > nav')?.parentElement?.remove();");
        //去除帮助
        await web.ExecuteScriptAsync("var tmp = document.querySelector('body > c-wiz > div > div > c-wiz > div:nth-child(3)');if(tmp.children.length == 0) tmp.remove();");
        //去除语音
        await web.ExecuteScriptAsync("document.querySelector('body > c-wiz > div > div > c-wiz > div > c-wiz > div > div > div > c-wiz > div > div > c-wiz > span > div > div > span > button > div')?.parentElement?.parentElement?.parentElement?.remove();");
        await web.ExecuteScriptAsync("document.body.style.overflowY = 'auto'"); //自动垂直滚动条

    }
    public static async Task BaiduTranslateBeautify(this WebView2 web)
    {
        await web.ExecuteScriptAsync("document.querySelector('#header').remove();"); //顶栏
        await web.ExecuteScriptAsync("document.querySelector('.hot-link-out-container').remove();");
        await web.ExecuteScriptAsync("document.querySelector('.footer').remove();"); //最底栏
        await web.ExecuteScriptAsync("document.querySelector('#main-outer > div > div > div.translate-wrap').style.margin = '10px';");
        await web.ExecuteScriptAsync("document.querySelector('#main-outer > div > div').classList.remove('inner');");
        await web.ExecuteScriptAsync("document.querySelector('.domain-trans-wrapper').remove();"); //通用按钮
        await web.ExecuteScriptAsync("document.querySelector('.translate-setting').remove();");
        await web.ExecuteScriptAsync("document.querySelector('body > div.container').style.width = 'auto';"); //去除小大小,自适应大小
        await web.ExecuteScriptAsync("document.querySelector('.trans-operation-wrapper').style.margin = '0 0 10px 0';");
        //await web.ExecuteScriptAsync("document.querySelector('.trans-other-wrap').remove();"); //子翻译
        await web.ExecuteScriptAsync("document.querySelector('.side-nav-wrapper').remove();"); //子翻译列表
        await web.ExecuteScriptAsync(@"setInterval(()=>{
try{
    document.querySelector('#app-read')?.remove();
    document.querySelector('.hot-link-middle')?.remove();
    document.querySelector('.op-correct')?.remove(); //报错
    document.querySelector('.note-expand-btn')?.remove(); //笔记
    let ses = document.querySelectorAll('#left-result-container > section'); //session
    if(ses.length != 0){
        ses.forEach(v=>v.remove())
    }
}catch(e){}
},200);"); //子翻译小广告
        await web.ExecuteScriptAsync("document.querySelector('.app-guide')?.remove();");
        await web.ExecuteScriptAsync("document.querySelector('.op-favor-container')?.remove();"); // 收藏夹
    }
    public static async Task GoogleTranslateFocusInputBox(this WebView2 web)
    {
        await web.ExecuteScriptAsync("document.querySelector('body > c-wiz > div > div > c-wiz > div > c-wiz > div > div > div > c-wiz > span > span > div > textarea')?.focus();");
    }
    public static async Task GoogleTranslateInput(this WebView2 web, string text)
    {
        char[] cs = text.ToCharArray();
        StringBuilder s = new();
        for (int i = 0; i < cs.Length; i++)
        {
            s.AppendFormat($"\\u{(int)cs[i]:x4}");
        }
        await web.ExecuteScriptAsync($@"
var input = document.querySelector('body > c-wiz > div > div > c-wiz > div > c-wiz > div > div > div > c-wiz > span > span > div > textarea');
var inputEvent = document.createEvent('HTMLEvents');
inputEvent.initEvent('input', true, true);
input.value = '{s}';
input.dispatchEvent(inputEvent)
");
    }
}
