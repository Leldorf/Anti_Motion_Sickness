using GameOverlay.Drawing;
using System;
using System.Windows.Forms;


namespace AntiMotionSickness
{
    public partial class Form1 : Form
    {
        private ColorDialog _colorDialog = new ColorDialog();
        private AntiMotionSickness _overlay;

        private LineConfig[] _lineConfigs;
        private bool[] _lineUpdates = { true, true, true };

        private const float SIZE_MULTIPLIER = 0.05f;
        ConfigFileManager configManager;


        public Form1(AntiMotionSickness overlay)
        {
            InitializeComponent();
            this.Resize += Form1_Resize;
            this.FormClosing += Form1_FormClosing;
            _overlay = overlay;
            _lineConfigs = new LineConfig[3];
            configManager = new ConfigFileManager();
            InitConfigs();
            updateConfig();
            updateConfigUI();
        }

        ~Form1()
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            configManager.SetConfigData("LastUsedConfig",
                new LineConfigPreset()
                {
                    center = _lineConfigs[0],
                    corner = _lineConfigs[1],
                    cross = _lineConfigs[2]
                });
            configManager.SaveData();
        }

        private void InitConfigs()
        {
            if (loadConfig("LastUsedConfig"))
            {
                removeConfig("LastUsedConfig");
                return;
            }
            if (loadConfig("Default")) return;
            SetDefaultConfig();
        }
        private bool loadConfig(string name)
        {
            LineConfigPreset preset;
            if (!configManager.GetConfigData(name, out preset)) return false;

            _lineConfigs[0] = preset.center;
            _lineConfigs[1] = preset.corner;
            _lineConfigs[2] = preset.cross;
            updateConfig(true);
            updateConfigUI();
            return true;
        }

        private void saveConfig(string name)
        {
            LineConfigPreset preset = new LineConfigPreset
            {
                center = _lineConfigs[0],
                corner = _lineConfigs[1],
                cross = _lineConfigs[2]
            };
            configManager.SetConfigData<LineConfigPreset>(name, preset);
        }

        private void removeConfig(string name)
        {
            configManager.RemoveConfig(name);
        }

        private void SetDefaultConfig()
        {
            _lineConfigs[0] = new LineConfig
            {
                isVisible = true, hasBorder = true,
                thickness = 3.0f, borderThickness = 6.0f,
                size = 0.2f, distance = 0f,
                color = new Color(0, 0, 0, 1f),
                borderColor = new Color(255, 255, 255, 0.33f)
            };
            _lineConfigs[1] = new LineConfig
            {
                isVisible = true, hasBorder = true,
                thickness = 3.0f, borderThickness = 4.0f,
                size = 1f, distance = 1f,
                color = new Color(0, 0, 0, 1f),
                borderColor = new Color(255, 255, 255, 0.33f)
            };
            _lineConfigs[2] = new LineConfig
            {
                isVisible = true, hasBorder = true,
                thickness = 3.0f, borderThickness = 4.0f,
                size = 1f, distance = 1f,
                color = new Color(0, 0, 0, 1f),
                borderColor = new Color(255, 255, 255, 0.33f)
            };
            saveConfig("Default");
        }

        private void updateConfig(bool updateAll = false)
        {
            for (int i = 0; i < _lineUpdates.Length; i++)
            {
                if (updateAll || _lineUpdates[i])
                {
                    _overlay.ConfigureLine(i, _lineConfigs[i]);
                    _lineUpdates[i] = false;
                }
            }
        }

        private void updateConfigUI()
        {
            //center
            centerReticleActivate.Checked = _lineConfigs[0].isVisible;
            centerReticleBorderActivate.Checked = _lineConfigs[0].hasBorder;

            centerReticleColorBtn.BackColor = System.Drawing.Color.FromArgb(
                (int)(_lineConfigs[0].color.R * 255),
                (int)(_lineConfigs[0].color.G * 255),
                (int)(_lineConfigs[0].color.B * 255));
            centerReticleBorderColorBtn.BackColor = System.Drawing.Color.FromArgb(
                (int)(_lineConfigs[0].borderColor.R * 255),
                (int)(_lineConfigs[0].borderColor.G * 255),
                (int)(_lineConfigs[0].borderColor.B * 255));

            centerReticleTransparency.Value = (int)(_lineConfigs[0].color.A * 255f);
            centerReticleTransparencyText.Text = _lineConfigs[0].color.A.ToString();
            centerReticleBorderTransparency.Value = (int)(_lineConfigs[0].borderColor.A * 255f);
            centerReticleBorderTransparencyText.Text = _lineConfigs[0].borderColor.A.ToString();

            centerReticleThickness.Value = (int)(_lineConfigs[0].thickness);
            centerReticleThicknessText.Text = _lineConfigs[0].thickness.ToString();
            centerReticleBorderThickness.Value = (int)(_lineConfigs[0].borderThickness);
            centerReticleBorderThicknessText.Text = _lineConfigs[0].borderThickness.ToString();

            centerReticleSize.Value = (int)(_lineConfigs[0].size * 20);
            centerReticleSizeText.Text = _lineConfigs[0].size.ToString();
            centerReticleDistance.Value = (int)(_lineConfigs[0].distance * 20);
            centerReticleDistanceText.Text = _lineConfigs[0].distance.ToString();


            //corner
            cornerReticleActivate.Checked = _lineConfigs[1].isVisible;
            cornerReticleBorderActivate.Checked = _lineConfigs[1].hasBorder;

            cornerReticleColorBtn.BackColor = System.Drawing.Color.FromArgb(
                (int)(_lineConfigs[1].color.R * 255),
                (int)(_lineConfigs[1].color.G * 255),
                (int)(_lineConfigs[1].color.B * 255));
            cornerReticleBorderColorBtn.BackColor = System.Drawing.Color.FromArgb(
                (int)(_lineConfigs[1].borderColor.R * 255),
                (int)(_lineConfigs[1].borderColor.G * 255),
                (int)(_lineConfigs[1].borderColor.B * 255));

            cornerReticleTransparency.Value = (int)(_lineConfigs[1].color.A * 255f);
            cornerReticleTransparencyText.Text = _lineConfigs[1].color.A.ToString();
            cornerReticleBorderTransparency.Value = (int)(_lineConfigs[1].borderColor.A * 255f);
            cornerReticleBorderTransparencyText.Text = _lineConfigs[1].borderColor.A.ToString();

            cornerReticleThickness.Value = (int)(_lineConfigs[1].thickness);
            cornerReticleThicknessText.Text = _lineConfigs[1].thickness.ToString();
            cornerReticleBorderThickness.Value = (int)(_lineConfigs[1].borderThickness);
            cornerReticleBorderThicknessText.Text = _lineConfigs[1].borderThickness.ToString();

            cornerReticleSize.Value = (int)(_lineConfigs[1].size / SIZE_MULTIPLIER);
            cornerReticleSizeText.Text = _lineConfigs[1].size.ToString();
            cornerReticleDistance.Value = (int)(_lineConfigs[1].distance / SIZE_MULTIPLIER);
            cornerReticleDistanceText.Text = _lineConfigs[1].distance.ToString();


            //cross
            crossReticleActivate.Checked = _lineConfigs[2].isVisible;
            crossReticleBorderActivate.Checked = _lineConfigs[2].hasBorder;

            crossReticleColorBtn.BackColor = System.Drawing.Color.FromArgb(
                (int)(_lineConfigs[2].color.R * 255),
                (int)(_lineConfigs[2].color.G * 255),
                (int)(_lineConfigs[2].color.B * 255));
            crossReticleBorderColorBtn.BackColor = System.Drawing.Color.FromArgb(
                (int)(_lineConfigs[2].borderColor.R * 255),
                (int)(_lineConfigs[2].borderColor.G * 255),
                (int)(_lineConfigs[2].borderColor.B * 255));

            crossReticleTransparency.Value = (int)(_lineConfigs[2].color.A * 255f);
            crossReticleTransparencyText.Text = _lineConfigs[2].color.A.ToString();
            crossReticleBorderTransparency.Value = (int)(_lineConfigs[2].borderColor.A * 255f);
            crossReticleBorderTransparencyText.Text = _lineConfigs[2].borderColor.A.ToString();

            crossReticleThickness.Value = (int)(_lineConfigs[2].thickness);
            crossReticleThicknessText.Text = _lineConfigs[2].thickness.ToString();
            crossReticleBorderThickness.Value = (int)(_lineConfigs[2].borderThickness);
            crossReticleBorderThicknessText.Text = _lineConfigs[2].borderThickness.ToString();

            crossReticleSize.Value = (int)(_lineConfigs[2].size / SIZE_MULTIPLIER);
            crossReticleSizeText.Text = _lineConfigs[2].size.ToString();
            crossReticleDistance.Value = (int)(_lineConfigs[2].distance / SIZE_MULTIPLIER);
            crossReticleDistanceText.Text = _lineConfigs[2].distance.ToString();


            presetListBox.Items.Clear();
            foreach (var name in configManager.GetConfigIndex())
            {
                presetListBox.Items.Add(name);
            }
        }

        private void VisibleChange(bool bVisible, bool bIconVisible)
        {
            this.Visible = bVisible;
            this.notifyIcon1.Visible = bIconVisible;
        }
        private void SetHideMode()
        {
            VisibleChange(false, true);
            this.WindowState = FormWindowState.Minimized;
        }
        private void SetShowMode()
        {
            VisibleChange(true, false);
            this.WindowState = FormWindowState.Normal;
        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SetShowMode();
        }

        private void openWindowToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            SetShowMode();
        }

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.notifyIcon1.Visible = false;
            Application.ExitThread();
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                SetHideMode();
            }
        }

        private void UpdatePreview()
        {

        }


        //preset
        private void presetLoadBtn_Click(object sender, EventArgs e)
        {
            loadConfig(presetListBox.SelectedItem.ToString());
        }

        private void presetSaveBtn_Click(object sender, EventArgs e)
        {
            if (presetNameTextBox.Text == string.Empty)
            {
                presetNameTextBox.Focus();
                return;
            }

            saveConfig(presetNameTextBox.Text);
            if (presetListBox.FindString(presetNameTextBox.Text) == ListBox.NoMatches)
            {
                presetListBox.Items.Add(presetNameTextBox.Text);
            }
            presetNameTextBox.Text = string.Empty;
        }

        private void presetRemoveBtn_Click(object sender, EventArgs e)
        {
            if (presetListBox.SelectedItem == null) return;
            removeConfig(presetListBox.SelectedItem.ToString());
            presetListBox.Items.Remove(presetListBox.SelectedItem);
        }
        private void presetNameTextBox_Click(object sender, EventArgs e)
        {
            presetNameTextBox.Text = string.Empty;
        }

        private void presetListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            presetNameTextBox.Text = presetListBox.SelectedItem?.ToString();
        }



        //center reticle
        private void centerReticle_CheckedChanged(object sender, EventArgs e)
        {
            _lineConfigs[0].isVisible = this.centerReticleActivate.Checked;
            _lineUpdates[0] = true;
            updateConfig();
        }

        private void centerReticleBorderActivate_CheckedChanged(object sender, EventArgs e)
        {
            _lineConfigs[0].hasBorder = this.centerReticleBorderActivate.Checked;
            _lineUpdates[0] = true;
            updateConfig();
        }

        private void centerReticleColorChange(object sender, EventArgs e)
        {
            if (_colorDialog.ShowDialog() == DialogResult.OK)
            {
                this.centerReticleColorBtn.BackColor = _colorDialog.Color;
                _lineConfigs[0].color = new Color(_colorDialog.Color, centerReticleTransparency.Value);
                _lineUpdates[0] = true;
                updateConfig();
            }
        }
        private void centerReticleBorderColorBtn_Click(object sender, EventArgs e)
        {
            if (_colorDialog.ShowDialog() == DialogResult.OK)
            {
                this.centerReticleBorderColorBtn.BackColor = _colorDialog.Color;
                _lineConfigs[0].borderColor = new Color(_colorDialog.Color, centerReticleBorderTransparency.Value);
                _lineUpdates[0] = true;
                updateConfig();
            }
        }
        private void centerReticleTransparency_Scroll(object sender, EventArgs e)
        {
            centerReticleTransparencyText.Text = (centerReticleTransparency.Value / 255f).ToString("F2");
            _lineConfigs[0].color.A = centerReticleTransparency.Value / 255f;
            _lineUpdates[0] = true;
            updateConfig();
        }

        private void centerReticleBorderTransparency_Scroll(object sender, EventArgs e)
        {
            centerReticleBorderTransparencyText.Text = (centerReticleBorderTransparency.Value / 255f).ToString("F2");
            _lineConfigs[0].borderColor.A = centerReticleBorderTransparency.Value / 255f;
            _lineUpdates[0] = true;
            updateConfig();
        }

        private void centerReticleThickness_Scroll(object sender, EventArgs e)
        {
            centerReticleThicknessText.Text = centerReticleThickness.Value.ToString();
            _lineConfigs[0].thickness = centerReticleThickness.Value;
            _lineUpdates[0] = true;
            updateConfig();
        }

        private void centerReticleBorderThickness_Scroll(object sender, EventArgs e)
        {
            centerReticleBorderThicknessText.Text = centerReticleBorderThickness.Value.ToString();
            _lineConfigs[0].borderThickness = centerReticleBorderThickness.Value;
            _lineUpdates[0] = true;
            updateConfig();
        }


        private void centerReticleSize_Scroll(object sender, EventArgs e)
        {
            centerReticleSizeText.Text = (centerReticleSize.Value * SIZE_MULTIPLIER).ToString();
            _lineConfigs[0].size = centerReticleSize.Value * SIZE_MULTIPLIER;
            _lineUpdates[0] = true;
            updateConfig();
        }

        private void centerReticleDistance_Scroll(object sender, EventArgs e)
        {
            centerReticleDistanceText.Text = (centerReticleDistance.Value * SIZE_MULTIPLIER).ToString();
            _lineConfigs[0].distance = centerReticleDistance.Value * SIZE_MULTIPLIER;
            _lineUpdates[0] = true;
            updateConfig();
        }

        //corner reticle
        private void cornerReticleActivate_CheckedChanged(object sender, EventArgs e)
        {
            _lineConfigs[1].isVisible = this.cornerReticleActivate.Checked;
            _lineUpdates[1] = true;
            updateConfig();
        }

        private void cornerReticleBorderActivate_CheckedChanged(object sender, EventArgs e)
        {
            _lineConfigs[1].hasBorder = this.cornerReticleBorderActivate.Checked;
            _lineUpdates[1] = true;
            updateConfig();
        }

        private void cornerReticleColorBtn_Click(object sender, EventArgs e)
        {
            if (_colorDialog.ShowDialog() == DialogResult.OK)
            {
                this.cornerReticleColorBtn.BackColor = _colorDialog.Color;
                _lineConfigs[1].color = new Color(_colorDialog.Color, cornerReticleTransparency.Value);
                _lineUpdates[1] = true;
                updateConfig();
            }
        }

        private void cornerReticleBorderColorBtn_Click(object sender, EventArgs e)
        {
            if (_colorDialog.ShowDialog() == DialogResult.OK)
            {
                this.cornerReticleBorderColorBtn.BackColor = _colorDialog.Color;
                _lineConfigs[1].borderColor = new Color(_colorDialog.Color, cornerReticleBorderTransparency.Value);
                _lineUpdates[1] = true;
                updateConfig();
            }
        }

        private void cornerReticleTransparency_Scroll(object sender, EventArgs e)
        {
            cornerReticleTransparencyText.Text = (cornerReticleTransparency.Value / 255f).ToString("F2");
            _lineConfigs[1].color.A = cornerReticleTransparency.Value / 255f;
            _lineUpdates[1] = true;
            updateConfig();
        }

        private void cornerReticleBorderTransparency_Scroll(object sender, EventArgs e)
        {
            cornerReticleBorderTransparencyText.Text = (cornerReticleBorderTransparency.Value / 255f).ToString("F2");
            _lineConfigs[1].borderColor.A = cornerReticleBorderTransparency.Value / 255f;
            _lineUpdates[1] = true;
            updateConfig();
        }
        private void cornerReticleThickness_Scroll(object sender, EventArgs e)
        {
            cornerReticleThicknessText.Text = cornerReticleThickness.Value.ToString();
            _lineConfigs[1].thickness = cornerReticleThickness.Value;
            _lineUpdates[1] = true;
            updateConfig();
        }

        private void cornerReticleBorderThickness_Scroll(object sender, EventArgs e)
        {
            cornerReticleBorderThicknessText.Text = cornerReticleBorderThickness.Value.ToString();
            _lineConfigs[1].borderThickness = cornerReticleBorderThickness.Value;
            _lineUpdates[1] = true;
            updateConfig();
        }

        private void cornerReticleSize_Scroll(object sender, EventArgs e)
        {
            cornerReticleSizeText.Text = (cornerReticleSize.Value * SIZE_MULTIPLIER).ToString();
            _lineConfigs[1].size = cornerReticleSize.Value * SIZE_MULTIPLIER;
            _lineUpdates[1] = true;
            updateConfig();
        }

        private void cornerReticleDistance_Scroll(object sender, EventArgs e)
        {
            cornerReticleDistanceText.Text = (cornerReticleDistance.Value * SIZE_MULTIPLIER).ToString();
            _lineConfigs[1].distance = cornerReticleDistance.Value * SIZE_MULTIPLIER;
            _lineUpdates[1] = true;
            updateConfig();
        }

        //cross reticle
        private void crossReticleActivate_CheckedChanged(object sender, EventArgs e)
        {
            _lineConfigs[2].isVisible = this.crossReticleActivate.Checked;
            _lineUpdates[2] = true;
            updateConfig();
        }

        private void crossReticleBorder_CheckedChanged(object sender, EventArgs e)
        {
            _lineConfigs[2].hasBorder = this.crossReticleBorderActivate.Checked;
            _lineUpdates[2] = true;
            updateConfig();
        }

        private void crossReticleColor_Click(object sender, EventArgs e)
        {
            if (_colorDialog.ShowDialog() == DialogResult.OK)
            {
                this.crossReticleColorBtn.BackColor = _colorDialog.Color;
                _lineConfigs[2].color = new Color(_colorDialog.Color, crossReticleTransparency.Value);
                _lineUpdates[2] = true;
                updateConfig();
            }
        }

        private void crossReticleBorderColor_Click(object sender, EventArgs e)
        {
            if (_colorDialog.ShowDialog() == DialogResult.OK)
            {
                this.crossReticleBorderColorBtn.BackColor = _colorDialog.Color;
                _lineConfigs[2].borderColor = new Color(_colorDialog.Color, crossReticleBorderTransparency.Value);
                _lineUpdates[2] = true;
                updateConfig();
            }
        }

        private void crossReticleTransparency_Scroll(object sender, EventArgs e)
        {
            crossReticleTransparencyText.Text = (crossReticleTransparency.Value / 255f).ToString("F2");
            _lineConfigs[2].color.A = crossReticleTransparency.Value / 255f;
            _lineUpdates[2] = true;
            updateConfig();
        }

        private void crossReticleBorderTransparency_Scroll(object sender, EventArgs e)
        {
            crossReticleBorderTransparencyText.Text = (crossReticleBorderTransparency.Value / 255f).ToString("F2");
            _lineConfigs[2].borderColor.A = crossReticleBorderTransparency.Value / 255f;
            _lineUpdates[2] = true;
            updateConfig();
        }
        private void crossReticleThickness_Scroll(object sender, EventArgs e)
        {
            crossReticleThicknessText.Text = crossReticleThickness.Value.ToString();
            _lineConfigs[2].thickness = crossReticleThickness.Value;
            _lineUpdates[2] = true;
            updateConfig();
        }

        private void crossReticleBorderThickness_Scroll(object sender, EventArgs e)
        {
            crossReticleBorderThicknessText.Text = crossReticleBorderThickness.Value.ToString();
            _lineConfigs[2].borderThickness = crossReticleBorderThickness.Value;
            _lineUpdates[2] = true;
            updateConfig();
        }

        private void crossReticleSize_Scroll(object sender, EventArgs e)
        {
            crossReticleSizeText.Text = (crossReticleSize.Value * SIZE_MULTIPLIER).ToString();
            _lineConfigs[2].size = crossReticleSize.Value * SIZE_MULTIPLIER;
            _lineUpdates[2] = true;
            updateConfig();
        }

        private void crossReticleDistance_Scroll(object sender, EventArgs e)
        {
            crossReticleDistanceText.Text = (crossReticleDistance.Value * SIZE_MULTIPLIER).ToString();
            _lineConfigs[2].distance = crossReticleDistance.Value * SIZE_MULTIPLIER;
            _lineUpdates[2] = true;
            updateConfig();
        }
    }
}
