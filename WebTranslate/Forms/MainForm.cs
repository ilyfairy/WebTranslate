using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Win32;
using System.ComponentModel;
using YamlDotNet.Serialization;
using Ilyfairy.Tools.WebTranslate;
using Ilyfairy.Tools.WebTranslate.TranslateTab;

namespace Ilyfairy.Tools.WebTranslate.Forms;

public partial class MainForm : Form
{
    private readonly List<WebTranslateTabBase> webs = new();
    private WebTranslateTabBase Web => webs[index]; //标签栏
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
        webs.Add(new GoogleTranslateTab());
        webs.Add(new BaiduTranslateTab());
        foreach (var item in webs)
        {
            item.KeyDown += Web_KeyDown;
        }
        panel.Controls.Add(Web.WebView);

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
    private void Web_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (ModifierKeys, e.KeyCode)
        {
            case (Keys.Control, Keys.W): //关闭
                Hide();
                break;
            case (Keys.Control, Keys.Tab): //下一个标签页
                Next();
                break;
            case (Keys.Control | Keys.Shift, Keys.Tab): //上一个标签页
                Last();
                break;
            case (Keys.Control | Keys.Shift, Keys.S): //交换语言
                Web.SwitchLanguage();
                break;
            case (Keys.Control, Keys.F12): //设置
                TopMost = false; //取消置顶 防止设置窗口被覆盖
                SettingForm.ShowWindow();
                break;
            default:
                return;
        }
        e.Handled = true;
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
    private void HotKeyCallback()
    {
        ShowWindow();
        string text = Clipboard.GetText();
        Web.FocusInput();
        if (text == LastInputText || string.IsNullOrWhiteSpace(text)) return;
        LastInputText = text;
        Web.Input(text);
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
        Web.Trim();
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
        panel.Controls.Add(Web.WebView);
        Web.WebView.Focus();
        Web.Trim();
    }
    #endregion

}