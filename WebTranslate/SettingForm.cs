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
    public event EventHandler<WindowConfig>? ConfigUpdated;

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
        saveButton.Focus();
    }

    private void ShowHotKey()
    {
        hotkeyTextBox.Text = TempHotKey.ToString();
    }

    private void HotKey_TextBox_Changed(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode is Keys.Control or Keys.ControlKey or Keys.LControlKey or Keys.RControlKey) return;
        if (e.KeyCode is Keys.Shift or Keys.ShiftKey or Keys.LShiftKey or Keys.RShiftKey) return;
        if (e.KeyCode is Keys.LWin or Keys.RWin) return;
        if (e.KeyCode is Keys.Alt) return;

        TempHotKey.Key = Keys.None;
        TempHotKey.Modifier = KeyModifiers.None;

        if (WinApi.IsKeyDown(Keys.LWin) || WinApi.IsKeyDown(Keys.RWin))
            TempHotKey.Modifier |= KeyModifiers.Windows;
        if (WinApi.IsKeyDown(Keys.ControlKey))
            TempHotKey.Modifier |= KeyModifiers.Control;
        if (WinApi.IsKeyDown(Keys.ShiftKey))
            TempHotKey.Modifier |= KeyModifiers.Shift;
        if (WinApi.IsKeyDown(Keys.Menu))
            TempHotKey.Modifier |= KeyModifiers.Alt;
        TempHotKey.Key = e.KeyCode;
        ShowHotKey();
    }

    protected override void OnShown(EventArgs e)
    {
        isFristInput = true;
        OldConfig = Config.Clone();
        hotkeyTextBox.Text = OldConfig.GlobalHotKey.ToString();
        TempHotKey.Modifier = Config.GlobalHotKey.Modifier;
        TempHotKey.Key = Config.GlobalHotKey.Key;
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
        ConfigUpdated?.Invoke(this, OldConfig);
    }

}

