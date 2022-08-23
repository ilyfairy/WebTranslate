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

        if (e.KeyCode is Keys.Control or Keys.ControlKey or Keys.LControlKey or Keys.RControlKey)
        {
            NewConfig.GlobalHotKey.Modifier = KeyModifiers.Control;
            ShowHotKey();
            return;
        }
        if (e.KeyCode is Keys.Shift or Keys.ShiftKey or Keys.LShiftKey or Keys.RShiftKey)
        {
            NewConfig.GlobalHotKey.Modifier = KeyModifiers.Shift;
            ShowHotKey();
            return;
        }
        if (e.KeyCode is Keys.LWin or Keys.LWin)
        {
            NewConfig.GlobalHotKey.Modifier = KeyModifiers.Windows;
            ShowHotKey();
            return;
        }
        if (e.KeyCode is Keys.Menu)
        {
            NewConfig.GlobalHotKey.Modifier = KeyModifiers.Alt;
            ShowHotKey();
            return;
        }

        if (WinApi.IsKeyDown(Keys.LWin) || WinApi.IsKeyDown(Keys.RWin))
            NewConfig.GlobalHotKey.Modifier |= KeyModifiers.Windows;
        if (WinApi.IsKeyDown(Keys.ControlKey))
            NewConfig.GlobalHotKey.Modifier |= KeyModifiers.Control;
        if (WinApi.IsKeyDown(Keys.ShiftKey))
            NewConfig.GlobalHotKey.Modifier |= KeyModifiers.Shift;
        if (WinApi.IsKeyDown(Keys.Menu))
            NewConfig.GlobalHotKey.Modifier |= KeyModifiers.Alt;
        if(NewConfig.GlobalHotKey.Modifier == KeyModifiers.None) NewConfig.GlobalHotKey.Modifier = KeyModifiers.Control;
        NewConfig.GlobalHotKey.Key = e.KeyCode;
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

