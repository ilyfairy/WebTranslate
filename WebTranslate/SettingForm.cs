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
using static Ilyfairy.Tools.WebTranslate.Constants;

namespace Ilyfairy.Tools.WebTranslate;

public partial class SettingForm : Form
{
    public readonly string Version;
    public readonly WindowConfig Config;
    private WindowConfig OldConfig;
    public event Func<object,WindowConfig,bool>? ConfigUpdated;

    public KeyCombination TempHotKey = new();
    public bool isFristInput = false;

    public SettingForm(WindowConfig config)
    {
        InitializeComponent();
        hotkeyTextBox.KeyDown += HotKey_TextBox_Changed;
        

        Version = (Assembly.GetEntryAssembly()?.GetCustomAttributes()?.FirstOrDefault(v => v is AssemblyFileVersionAttribute) as AssemblyFileVersionAttribute)?.Version ?? "Unknow";
        versionLabel.Text = $"{Version}";
        Config = config;
        OldConfig = Config.Clone();
    }

    private void SettingForm_Load(object sender, EventArgs e)
    {
        
    }

    private void ShowHotKey()
    {
        hotkeyTextBox.Text = TempHotKey.ToString();
    }

    private void HotKey_TextBox_Changed(object? sender, KeyEventArgs e)
    {
        TempHotKey.Key = Keys.None;
        TempHotKey.Modifier = KeyModifiers.None;

        if (e.KeyCode is Keys.Control or Keys.ControlKey or Keys.LControlKey or Keys.RControlKey)
        {
            TempHotKey.Modifier = KeyModifiers.Control;
            ShowHotKey();
            return;
        }
        if (e.KeyCode is Keys.Shift or Keys.ShiftKey or Keys.LShiftKey or Keys.RShiftKey)
        {
            TempHotKey.Modifier = KeyModifiers.Shift;
            ShowHotKey();
            return;
        }
        if (e.KeyCode is Keys.LWin or Keys.LWin)
        {
            TempHotKey.Modifier = KeyModifiers.Windows;
            ShowHotKey();
            return;
        }
        if (e.KeyCode is Keys.Menu)
        {
            TempHotKey.Modifier = KeyModifiers.Alt;
            ShowHotKey();
            return;
        }

        if (WinApi.IsKeyDown(Keys.LWin) || WinApi.IsKeyDown(Keys.RWin))
            TempHotKey.Modifier |= KeyModifiers.Windows;
        if (WinApi.IsKeyDown(Keys.ControlKey))
            TempHotKey.Modifier |= KeyModifiers.Control;
        if (WinApi.IsKeyDown(Keys.ShiftKey))
            TempHotKey.Modifier |= KeyModifiers.Shift;
        if (WinApi.IsKeyDown(Keys.Menu))
            TempHotKey.Modifier |= KeyModifiers.Alt;
        if(TempHotKey.Modifier == KeyModifiers.None) TempHotKey.Modifier = KeyModifiers.Control;
        TempHotKey.Key = e.KeyCode;
        ShowHotKey();
    }


    public void ShowWindow()
    {
        Text = "设置";
        isFristInput = true;
        OldConfig = Config.Clone();
        hotkeyTextBox.Text = OldConfig.GlobalHotKey.ToString();
        TempHotKey.Modifier = Config.GlobalHotKey.Modifier;
        TempHotKey.Key = Config.GlobalHotKey.Key;
        Show();
        Activate();
        saveButton.Focus();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        e.Cancel = true;
        Hide();
    }


    private void Save_Click(object sender, EventArgs e)
    {
        Config.GlobalHotKey.Modifier = TempHotKey.Modifier;
        Config.GlobalHotKey.Key = TempHotKey.Key;
        bool ok = ConfigUpdated?.Invoke(this, OldConfig) ?? false;
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

