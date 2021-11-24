using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GeniePlugin.Interfaces;

namespace SpellTimerPlugin
{
    public partial class SpellTimerForm : Form
    {
        private Genie _host => Genie.Instance;

        public SpellTimerForm()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void EnableCastbar_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = EnableCastbar.Checked;
            _host.SendText("#var SpellTimer.EnableCastBar " + (enabled ? "1" : "0"));
            CastbarGroupBox.Enabled = enabled;
        }

        private void LoadSettings()
        {
            EnableCastbar.Checked = !(_host.get_Variable("SpellTimer.EnableCastBar") == "0");

            ShowCountdown.Checked = !(_host.get_Variable("SpellTimer.countdown") == "0");
            ShowBar.Checked = !(_host.get_Variable("SpellTimer.progress") == "0");
            ShowSpinners.Checked = !(_host.get_Variable("SpellTimer.spinners") == "0");
            ShowReady.Checked = !(_host.get_Variable("SpellTimer.prepared") == "0");

            Triquarter.Text = _host.get_Variable("SpellTimer.triquarter");
            Half.Text = _host.get_Variable("SpellTimer.half");
            Quarter.Text = _host.get_Variable("SpellTimer.quarter");
            Ready.Text = _host.get_Variable("SpellTimer.ready");

            CastbarGroupBox.Enabled = EnableCastbar.Checked;
        }

        private void Triquarter_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Triquarter.Text)) _host.SendText("#var SpellTimer.triquarter " + "\"\"");
            else _host.SendText("#var SpellTimer.triquarter " + Triquarter.Text);
        }

        private void Half_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Half.Text)) _host.SendText("#var SpellTimer.half " + "\"\"");
            else _host.SendText("#var SpellTimer.half " + Half.Text);
        }

        private void Quarter_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Quarter.Text)) _host.SendText("#var SpellTimer.quarter " + "\"\"");
            else _host.SendText("#var SpellTimer.quarter " + Quarter.Text);
        }

        private void Ready_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Ready.Text)) _host.SendText("#var SpellTimer.ready " + "\"\"");
            else _host.SendText("#var SpellTimer.ready " + Ready.Text);
        }

        private void ShowCountdown_CheckedChanged(object sender, EventArgs e)
        {
            _host.SendText("#var SpellTimer.countdown " + (ShowCountdown.Checked ? "1" : "0"));
        }

        private void ShowReady_CheckedChanged(object sender, EventArgs e)
        {
            _host.SendText("#var SpellTimer.prepared " + (ShowReady.Checked ? "1" : "0"));
        }

        private void ShowBar_CheckedChanged(object sender, EventArgs e)
        {
            _host.SendText("#var SpellTimer.progress " + (ShowBar.Checked ? "1" : "0"));
        }

        private void ShowSpinners_CheckedChanged(object sender, EventArgs e)
        {
            _host.SendText("#var SpellTimer.spinners " + (ShowSpinners.Checked ? "1" : "0"));
        }

        private void SpellTimerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _host.SendText("#save variable");
        }

        private void SpellTimerForm_Enter(object sender, EventArgs e)
        {
            LoadSettings();
        }
    }
}
