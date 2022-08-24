using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Ilyfairy.Tools.WebTranslate.Forms;

public partial class SettingForm : Form
{
    public readonly string Version;
    public readonly WindowConfig Config;
    private WindowConfig NewConfig;
    public event Func<object,WindowConfig,bool>? ConfigUpdated;
    public event Action? ConfigWindowClosed;

    public SettingForm(WindowConfig config)
    {
        InitializeComponent();
        hotkeyTextBox.KeyDown += HotKey_TextBox_Changed;
        hotkeyTextBox.KeyUp += HotkeyTextBox_KeyUp;

        Version = (Assembly.GetEntryAssembly()?.GetCustomAttributes()?.FirstOrDefault(v => v is AssemblyFileVersionAttribute) as AssemblyFileVersionAttribute)?.Version ?? "Unknow";
        if (Version.LastIndexOf('.') is int verLastDot && verLastDot!= -1)
        {
            Version = Version[0..verLastDot];
        }
        versionLabel.Text = $"{Version}";
        Config = config;
        NewConfig = Config.Clone();
    }



    private void SettingForm_Load(object sender, EventArgs e)
    {
        
    }

    private void ShowHotKey()
    {
        hotkeyTextBox.Text = NewConfig.GlobalHotKey.ToString();
    }
    private void HotKey_TextBox_Changed(object? sender, KeyEventArgs e)
    {
        NewConfig.GlobalHotKey.Key = Keys.None;
        NewConfig.GlobalHotKey.Modifier = KeyModifiers.None;

        if (WinApi.IsKeyDown(Keys.LWin) || WinApi.IsKeyDown(Keys.RWin))
            NewConfig.GlobalHotKey.Modifier |= KeyModifiers.Win;
        if (WinApi.IsKeyDown(Keys.ControlKey))
            NewConfig.GlobalHotKey.Modifier |= KeyModifiers.Ctrl;
        if (WinApi.IsKeyDown(Keys.ShiftKey))
            NewConfig.GlobalHotKey.Modifier |= KeyModifiers.Shift;
        if (WinApi.IsKeyDown(Keys.Menu))
            NewConfig.GlobalHotKey.Modifier |= KeyModifiers.Alt;
        if(NewConfig.GlobalHotKey.Modifier == KeyModifiers.None) NewConfig.GlobalHotKey.Modifier = KeyModifiers.Ctrl;
        if (e.KeyCode is Keys.Control or Keys.ControlKey or Keys.LControlKey or Keys.RControlKey
    or Keys.Shift or Keys.ShiftKey or Keys.LShiftKey or Keys.RShiftKey
    or Keys.LWin or Keys.RWin or Keys.Menu)
        {
        }
        else
        {
            NewConfig.GlobalHotKey.Key = e.KeyCode;
        }
        ShowHotKey();
    }

    private void HotkeyTextBox_KeyUp(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode is Keys.Control or Keys.ControlKey or Keys.LControlKey or Keys.RControlKey
            or Keys.Shift or Keys.ShiftKey or Keys.LShiftKey or Keys.RShiftKey
            or Keys.LWin or Keys.RWin or Keys.Menu)
        {
            if (NewConfig.GlobalHotKey.Key == Keys.None)
            {
                NewConfig.GlobalHotKey.Modifier = KeyModifiers.None;
            }
        }
        ShowHotKey();
    }

    public void ShowWindow()
    {
        Text = "设置";
        NewConfig = Config.Clone();
        autoHideCheckBox.Checked = NewConfig.AutoHide;
        topMostCheckBox.Checked = NewConfig.TopMost;
        Visible = true;
        ShowHotKey();
        Show();
        Activate();
        saveButton.Focus();
    }
    protected override void OnClosing(CancelEventArgs e)
    {
        e.Cancel = true;
        Visible = false;
        Hide();
        ConfigWindowClosed?.Invoke();
    }

    private void Save_Click(object sender, EventArgs e)
    {
        NewConfig.TopMost = topMostCheckBox.Checked;
        NewConfig.AutoHide = autoHideCheckBox.Checked;
        bool ok = ConfigUpdated?.Invoke(this, NewConfig) ?? false;
        if (ok)
        {
            this.Text = "保存成功";
        }
        else
        {
            this.Text = "保存失败";
        }
    }

}

