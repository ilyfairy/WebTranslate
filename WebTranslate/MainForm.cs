using Ilyfairy.Tools.WinFormTranslate;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Win32;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Mail;
using System.Reflection;
using System.Security.Policy;
using YamlDotNet.Serialization;

namespace Ilyfairy.Tools.WinFormTranslate;

public partial class MainForm : Form
{
    private readonly List<WebView2> webs = new();
    private WebView2 Web => webs[index]; //标签栏
    private int index = 0; //标签栏索引

    private readonly HotKeyManager HotKey; //热键管理
    private string LastInputText = ""; //最后输入文本

    private readonly string configFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"config.yaml"); //配置文件名
    private WindowConfig Config; //窗口配置

    private bool isExit = false; //是否开始退出
    private bool isHide; //启动时是否隐藏

    private RegistryKey? registryRun = null; //注册表Run
    private readonly string registryName = "WinFormTranslate"; //注册表启动项名
    private readonly string startupCmd = $"\"{Application.ExecutablePath}\" -nogui"; //注册表启动项命令


    public MainForm(bool isHide)
    {
        InitializeComponent();
        
        if (isHide)
        {
            this.Opacity = 0;
            this.ShowInTaskbar = false;
            this.isHide = true;
        }

        HotKey = new HotKeyManager(Handle);
        panel.HorizontalScroll.Enabled = false;
        panel.VerticalScroll.Enabled = false;
        notifyExit.Click += NotifyExit_Click;
        this.Deactivate += MainForm_Deactivate;

        if (File.Exists(configFileName))
        {
            try
            {
                Deserializer deser = new();
                Config = deser.Deserialize<WindowConfig>(File.ReadAllText(configFileName));
                Config.GlobalHotKey.Modifier = Config.GlobalHotKey.Modifier.Distinct().ToArray(); //去除重复的快捷键
                if (Config.Width <= 0) Config.Width = Width;
                if (Config.Height <= 0) Config.Height = Height;
            }
            catch (Exception)
            {
                NewConfig();
            }
        }
        else
        {
            NewConfig();
        }
        Width = Config!.Width;
        Height = Config.Height;
        SaveConfig();
    }




    #region Event
    private async void Form_Load(object sender, EventArgs e)
    {
        await NewWeb("https://translate.google.cn/");
        await CreateWebView("https://fanyi.baidu.com/");
        try
        {
            registryRun = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            if ((registryRun.GetValue(registryName) as string) == startupCmd)
            {
                notifyStartup.Checked = true;
            }
        }
        catch { }
        this.TopMost = Config.TopMost;
        await Task.Delay(500);
        bool ok = HotKey.Add("hotkey", Config.GlobalHotKey.Modifier.Aggregate((v1, v2) => v1 | v2), Config.GlobalHotKey.Key, HotKeyCallback);
        if (!ok)
        {
            MessageBox.Show("热键注册失败");
        }
    }

    private void Web_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.W:
                if(ModifierKeys == Keys.Control) Hide();
                break;
            case Keys.Tab:
                if((ModifierKeys & Keys.Control) == Keys.Control)
                {
                    if((ModifierKeys & Keys.Shift) == Keys.Shift)
                    {
                        Last();
                    }
                    else
                    {
                        Next();
                    }
                }
                break;
            case Keys.F12:
                if (ModifierKeys == Keys.Control)
                {
                    MessageBox.Show("设置");
                    e.Handled = true;
                }
                break;
            default:
                return;
        }
        e.Handled = true;
    }

    private void MainForm_Deactivate(object? sender, EventArgs e)
    {
        if (Config.AutoHide)
        {
            Hide();
            
        }
    }
    private void Web_NavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
    {
        if (Web.Tag is Uri uri)
        {
            if (Web.Source.Host != uri.Host)
            {
                Web.Source = uri;
            }
        }
        _ = Beautify();
    }

    private void Web_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        _ = Beautify();
    }
    #endregion



    #region Methods
    private void ShowWindow()
    {
        isHide = false;
        Opacity = 1;
        if (WindowState != FormWindowState.Maximized)
        {
            WindowState = FormWindowState.Normal;
        }
        Show();
        Activate();
    }
    private void NewConfig()
    {
        Serializer ser = new();
        Config = new();
        Config.GlobalHotKey = new KeyCombination()
        {
            Modifier = new ControlKeys[] { ControlKeys.Control, ControlKeys.Shift },
            Key = Keys.T
        };
        Config.Width = Width;
        Config.Height = Height;
        try
        {
            File.WriteAllText(configFileName, ser.Serialize(Config));
        }
        catch { }
    }
    private void SaveConfig()
    {
        Serializer ser = new();
        try
        {
            File.WriteAllText(configFileName, ser.Serialize(Config));
        }
        catch { }
    }
    #endregion



    #region Override
    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        if (isHide)
        {
            Hide();
        }
    }
    protected override void OnClosing(CancelEventArgs e)
    {
        if (isExit || ModifierKeys == Keys.Control)
        {
            //退出
            Config.Width = Width;
            Config.Height = Height;
            SaveConfig();
            return;
        }
        else
        {
            //隐藏
            Hide();
            e.Cancel = true;
        }
    }
    #endregion



    #region Notify
    private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        ShowWindow();
    }
    private void NotifyExit_Click(object? sender, EventArgs e)
    {
        isExit = true;
        Close();
    }
    private void notifyStartup_Click(object sender, EventArgs e)
    {
        notifyStartup.Checked = !notifyStartup.Checked;
        if(registryRun == null)
        {
            try
            {
                registryRun = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            }
            catch (Exception)
            {
                MessageBox.Show("设置失败");
                return;
            }
        }
        if (notifyStartup.Checked)
        {
            registryRun.SetValue(registryName, startupCmd, RegistryValueKind.String);
        }
        else
        {
            registryRun.DeleteValue(registryName);
        }
    }
    #endregion



    #region GlobalHotKey
    protected override void WndProc(ref Message m)
    {
        if (m.Msg == 0x312)
        {
            HotKey.Loop((int)m.WParam);
        }
        base.WndProc(ref m);

    }
    private async void HotKeyCallback()
    {
        ShowWindow();
        await Web.GoogleTranslateFocusInputBox();
        string text = Clipboard.GetText();
        if (text != LastInputText && !string.IsNullOrWhiteSpace(text))
        {
            LastInputText = text;
            await Web.GoogleTranslateInput(text);
        }
    }
    #endregion



    #region WebUtil
    private async Task NewWeb(string url)
    {
        var web = await CreateWebView(url);
        panel.Controls.Add(web);
        web.Focus();
    }
    private void Last()
    {
        if (index > 0)
        {
            index--;
            SwitchWeb();
        }
        else
        {
            index = webs.Count - 1;
            SwitchWeb();
        }
    }
    private void Next()
    {
        _ = Beautify();
        if (index < webs.Count - 1)
        {
            index++;
            SwitchWeb();
        }
        else
        {
            index = 0;
            SwitchWeb();
        }
    }
    private void SwitchWeb()
    {
        panel.Controls.Clear();
        panel.Controls.Add(Web);
        Web.Focus();
    }
    private async Task Beautify()
    {
        await Task.Delay(100);
        switch (Web.Source.Host)
        {
            case "translate.google.cn":
                await Web.GoogleTranslateBeautify();
                break;
            case "fanyi.baidu.com":
                await Web.BaiduTranslateBeautify();
                break;
        }
    }
    private async Task<WebView2> CreateWebView(string url)
    {
        WebView2 web = new();
        web.Dock = DockStyle.Fill;
        await web.EnsureCoreWebView2Async();
        web.KeyDown += Web_KeyDown; ;
        web.NavigationStarting += Web_NavigationStarting;
        web.NavigationCompleted += Web_NavigationCompleted;
        web.NavigateToString("<span>loading...<span>");
        var uri = new Uri(url);
        web.Source = new Uri(url);
        web.Tag = uri;
        webs.Add(web);
        return web;
    }
    #endregion

}