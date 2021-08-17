
namespace CSGOTraderBot
{
    partial class Settings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("General");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("CSGO500");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Sites", new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSettingsOk = new System.Windows.Forms.Button();
            this.btnSettingsCancel = new System.Windows.Forms.Button();
            this.ucSettingsSitesCSGO5001 = new CSGOTraderBot.UserControls.ucSettingsSitesCSGO500();
            this.ucSettingsGeneral1 = new CSGOTraderBot.UserControls.ucSettingsGeneral();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.treeView1.FullRowSelect = true;
            this.treeView1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.treeView1.Location = new System.Drawing.Point(6, 13);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "General";
            treeNode1.Text = "General";
            treeNode2.Name = "CSGO500";
            treeNode2.Text = "CSGO500";
            treeNode3.Name = "Sites";
            treeNode3.Text = "Sites";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode3});
            this.treeView1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.treeView1.ShowLines = false;
            this.treeView1.Size = new System.Drawing.Size(188, 382);
            this.treeView1.TabIndex = 3;
            this.treeView1.DoubleClick += new System.EventHandler(this.TreeView1_DoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.BackColor = System.Drawing.SystemColors.Window;
            this.groupBox1.Controls.Add(this.treeView1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 401);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // btnSettingsOk
            // 
            this.btnSettingsOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSettingsOk.BackColor = System.Drawing.Color.SlateGray;
            this.btnSettingsOk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSettingsOk.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnSettingsOk.FlatAppearance.BorderSize = 0;
            this.btnSettingsOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettingsOk.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSettingsOk.ForeColor = System.Drawing.SystemColors.Control;
            this.btnSettingsOk.Location = new System.Drawing.Point(698, 382);
            this.btnSettingsOk.Name = "btnSettingsOk";
            this.btnSettingsOk.Size = new System.Drawing.Size(99, 31);
            this.btnSettingsOk.TabIndex = 7;
            this.btnSettingsOk.Text = "OK";
            this.btnSettingsOk.UseVisualStyleBackColor = false;
            this.btnSettingsOk.Click += new System.EventHandler(this.BtnSettingsOk_Click);
            // 
            // btnSettingsCancel
            // 
            this.btnSettingsCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSettingsCancel.BackColor = System.Drawing.Color.SlateGray;
            this.btnSettingsCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSettingsCancel.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnSettingsCancel.FlatAppearance.BorderSize = 0;
            this.btnSettingsCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettingsCancel.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSettingsCancel.ForeColor = System.Drawing.SystemColors.Control;
            this.btnSettingsCancel.Location = new System.Drawing.Point(593, 382);
            this.btnSettingsCancel.Name = "btnSettingsCancel";
            this.btnSettingsCancel.Size = new System.Drawing.Size(99, 31);
            this.btnSettingsCancel.TabIndex = 8;
            this.btnSettingsCancel.Text = "Cancel";
            this.btnSettingsCancel.UseVisualStyleBackColor = false;
            this.btnSettingsCancel.Click += new System.EventHandler(this.BtnSettingsCancel_Click);
            // 
            // ucSettingsSitesCSGO5001
            // 
            this.ucSettingsSitesCSGO5001.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucSettingsSitesCSGO5001.Location = new System.Drawing.Point(218, 12);
            this.ucSettingsSitesCSGO5001.Name = "ucSettingsSitesCSGO5001";
            this.ucSettingsSitesCSGO5001.Size = new System.Drawing.Size(579, 344);
            this.ucSettingsSitesCSGO5001.TabIndex = 1;
            // 
            // ucSettingsGeneral1
            // 
            this.ucSettingsGeneral1.Location = new System.Drawing.Point(218, 12);
            this.ucSettingsGeneral1.Name = "ucSettingsGeneral1";
            this.ucSettingsGeneral1.Size = new System.Drawing.Size(579, 344);
            this.ucSettingsGeneral1.TabIndex = 9;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(809, 425);
            this.Controls.Add(this.ucSettingsGeneral1);
            this.Controls.Add(this.btnSettingsCancel);
            this.Controls.Add(this.btnSettingsOk);
            this.Controls.Add(this.ucSettingsSitesCSGO5001);
            this.Controls.Add(this.groupBox1);
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(825, 464);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(825, 464);
            this.Name = "Settings";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private UserControls.ucSettingsSitesCSGO500 ucSettingsSitesCSGO5001;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSettingsOk;
        private System.Windows.Forms.Button btnSettingsCancel;
        private UserControls.ucSettingsGeneral ucSettingsGeneral1;
    }
}