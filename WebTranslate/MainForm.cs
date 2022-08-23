using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Win32;
using System.ComponentModel;
using YamlDotNet.Serialization;

namespace Ilyfairy.Tools.WebTranslate;

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
    private readonly string startupCmd = $"\"{Application.ExecutablePath}\" -nogui"; //注册表启动项命令

    private readonly SettingForm SettingForm; //设置窗口

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
        SettingForm = new SettingForm(Config);
        SettingForm.ConfigUpdated += SettingForm_ConfigUpdated;
        SettingForm.ConfigWindowClosed += SettingForm_ConfigWindowClosed;
    }



    #region Event
    private async void MainForm_Load(object sender, EventArgs e)
    {
        await NewWeb(Constants.GoogleTranslate.ToString());
        await CreateWebView(Constants.BaiduTranslate.ToString());
        try
        {
            registryRun = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            if ((registryRun.GetValue(Constants.RegistryName) as string) == startupCmd)
            {
                notifyStartup.Checked = true;
            }
        }
        catch { }
        this.TopMost = Config.TopMost;
        await Task.Delay(500);
        RegistryGlobalHotKey();
    }
    private async void Web_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.W:
                if (ModifierKeys == Keys.Control)
                {
                    Hide();
                    e.Handled = true;
                }
                break;
            case Keys.Tab:
                if((ModifierKeys & Keys.Control) == Keys.Control)
                {
                    if((ModifierKeys & Keys.Shift) == Keys.Shift)
                    {
                        Last();
                        e.Handled = true;
                    }
                    else
                    {
                        Next();
                        e.Handled = true;
                    }
                }
                break;
            case Keys.S:
                if(ModifierKeys.HasFlag(Keys.Control) && ModifierKeys.HasFlag(Keys.Shift))
                {
                    if (Web.Source.Host == Constants.GoogleTranslate.Host)
                    {
                        await Web.GoogleTranslateSwitchLanguage();
                        e.Handled = true;
                    }
                    else if (Web.Source.Host == Constants.BaiduTranslate.Host)
                    {
                        await Web.BaiduTranslateSwitchLanguage();
                        e.Handled = true;
                    }
                }
                break;
            case Keys.F12:
                if (ModifierKeys == Keys.Control)
                {
                    TopMost = false; //防止设置窗口被覆盖
                    SettingForm.ShowWindow();
                    e.Handled = true;
                }
                break;
            default:
                return;
        }
    }
    private bool SettingForm_ConfigUpdated(object sender, WindowConfig newConfig)
    {
        bool ok = true;
        if(newConfig.GlobalHotKey.Key != Config.GlobalHotKey.Key || newConfig.GlobalHotKey.Modifier != Config.GlobalHotKey.Modifier)
        {
            ok |= RegistryGlobalHotKey(newConfig.GlobalHotKey);
        }
        if (ok)
        {
            Config.TopMost = newConfig.TopMost;
            Config.AutoHide = newConfig.AutoHide;
            Config.GlobalHotKey.Modifier = newConfig.GlobalHotKey.Modifier;
            Config.GlobalHotKey.Key = newConfig.GlobalHotKey.Key;
            SaveConfig();
        }
        return ok;
    }

    //当设置关闭时,置顶才会生效
    private void SettingForm_ConfigWindowClosed()
    {
        if (Config.TopMost)
        {
            TopMost = Config.TopMost;
        }
    }
    private void MainForm_Deactivate(object? sender, EventArgs e)
    {
        //当设置窗口没有显示时,自动隐藏才会生效
        if (Config.AutoHide && !SettingForm.Visible)
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
            Modifier = KeyModifiers.Control | KeyModifiers.Shift,
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
    private bool RegistryGlobalHotKey(KeyCombination? hotkey = null)
    {
        hotkey ??= Config.GlobalHotKey;
        HotKey.Remove(Constants.GlobalHotKey);
        bool ok = HotKey.Add(Constants.GlobalHotKey, hotkey.Modifier, hotkey.Key, HotKeyCallback);
        if (!ok)
        {
            MessageBox.Show("热键注册失败");
        }
        return ok;
    }
    private async void HotKeyCallback()
    {
        ShowWindow();
        string text = Clipboard.GetText();
        await Web.GoogleTranslateFocusInputBox();
        await Web.BaiduTranslateFocusInputBox();
        if (text == LastInputText || string.IsNullOrWhiteSpace(text)) return;
        LastInputText = text;
        if(Web.Source.Host == Constants.GoogleTranslate.Host)
        {
            await Web.GoogleTranslateInput(text);
        }
        else if (Web.Source.Host == Constants.BaiduTranslate.Host)
        {
            await Web.BaiduTranslateInput(text);
        }

        Action f = F;
    }
    void F()
    {

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
    protected override void WndProc(ref Message m)
    {
        if (m.Msg == Constants.WM_HOTKEY)
        {
            HotKey.LoopAction((int)m.WParam);
        }
        base.WndProc(ref m);

    }
    #endregion



    #region Notify
    private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        ShowWindow();
    }
    private void NotifyExit_Click(object? sender, EventArgs e)
    {
        isExit = true;
        Close();
    }
    private void NotifyStartup_Click(object sender, EventArgs e)
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
            registryRun.SetValue(Constants.RegistryName, startupCmd, RegistryValueKind.String);
        }
        else
        {
            registryRun.DeleteValue(Constants.RegistryName);
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
        _ = Beautify();
    }
    private async Task Beautify()
    {
        await Task.Delay(100);
        if (Web.Source.Host == Constants.GoogleTranslate.Host)
        {
            await Web.GoogleTranslateBeautify();
        }
        else if (Web.Source.Host == Constants.BaiduTranslate.Host)
        {
            await Web.BaiduTranslateBeautify();
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