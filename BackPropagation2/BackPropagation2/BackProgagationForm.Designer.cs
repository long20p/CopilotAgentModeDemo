namespace BackPropagation2
{
    partial class BackProgagationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackProgagationForm));
            this.tabPages = new System.Windows.Forms.TabControl();
            this.testingTab = new System.Windows.Forms.TabPage();
            this.testResetBtn = new System.Windows.Forms.Button();
            this.testingCanvasGroup = new System.Windows.Forms.GroupBox();
            this.smallImagePanel = new System.Windows.Forms.Panel();
            this.testingPanel = new BackPropagation2.CanvasPanel();
            this.resultLbl = new System.Windows.Forms.Label();
            this.recognizeBtn = new System.Windows.Forms.Button();
            this.trainingTab = new System.Windows.Forms.TabPage();
            this.trainSetPathLbl = new System.Windows.Forms.Label();
            this.patternBrowseBtn = new System.Windows.Forms.Button();
            this.patternPathTxt = new System.Windows.Forms.TextBox();
            this.processLbl = new System.Windows.Forms.Label();
            this.trainBtn = new System.Windows.Forms.Button();
            this.settingGroup = new System.Windows.Forms.GroupBox();
            this.allowedErrorTxt = new System.Windows.Forms.TextBox();
            this.allowedErrorLbl = new System.Windows.Forms.Label();
            this.maxIterationTxt = new System.Windows.Forms.TextBox();
            this.maxIterationLbl = new System.Windows.Forms.Label();
            this.hidLayerElemCountTxt = new System.Windows.Forms.TextBox();
            this.hidLayerElemCountLbl = new System.Windows.Forms.Label();
            this.hidLayerCountTxt = new System.Windows.Forms.TextBox();
            this.hidLayerCountLbl = new System.Windows.Forms.Label();
            this.tabPages.SuspendLayout();
            this.testingTab.SuspendLayout();
            this.testingCanvasGroup.SuspendLayout();
            this.trainingTab.SuspendLayout();
            this.settingGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPages
            // 
            this.tabPages.Controls.Add(this.testingTab);
            this.tabPages.Controls.Add(this.trainingTab);
            this.tabPages.Location = new System.Drawing.Point(13, 13);
            this.tabPages.Name = "tabPages";
            this.tabPages.SelectedIndex = 0;
            this.tabPages.Size = new System.Drawing.Size(588, 325);
            this.tabPages.TabIndex = 0;
            // 
            // testingTab
            // 
            this.testingTab.Controls.Add(this.testResetBtn);
            this.testingTab.Controls.Add(this.testingCanvasGroup);
            this.testingTab.Controls.Add(this.resultLbl);
            this.testingTab.Controls.Add(this.recognizeBtn);
            this.testingTab.Location = new System.Drawing.Point(4, 22);
            this.testingTab.Name = "testingTab";
            this.testingTab.Padding = new System.Windows.Forms.Padding(3);
            this.testingTab.Size = new System.Drawing.Size(580, 299);
            this.testingTab.TabIndex = 0;
            this.testingTab.Text = "Testing";
            this.testingTab.UseVisualStyleBackColor = true;
            // 
            // testResetBtn
            // 
            this.testResetBtn.Location = new System.Drawing.Point(377, 26);
            this.testResetBtn.Name = "testResetBtn";
            this.testResetBtn.Size = new System.Drawing.Size(81, 23);
            this.testResetBtn.TabIndex = 4;
            this.testResetBtn.Text = "Reset";
            this.testResetBtn.UseVisualStyleBackColor = true;
            this.testResetBtn.Click += new System.EventHandler(this.testResetBtn_Click);
            // 
            // testingCanvasGroup
            // 
            this.testingCanvasGroup.Controls.Add(this.smallImagePanel);
            this.testingCanvasGroup.Controls.Add(this.testingPanel);
            this.testingCanvasGroup.Location = new System.Drawing.Point(18, 17);
            this.testingCanvasGroup.Name = "testingCanvasGroup";
            this.testingCanvasGroup.Size = new System.Drawing.Size(241, 265);
            this.testingCanvasGroup.TabIndex = 3;
            this.testingCanvasGroup.TabStop = false;
            this.testingCanvasGroup.Text = "Testing canvas";
            // 
            // smallImagePanel
            // 
            this.smallImagePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.smallImagePanel.Location = new System.Drawing.Point(21, 225);
            this.smallImagePanel.Name = "smallImagePanel";
            this.smallImagePanel.Size = new System.Drawing.Size(30, 30);
            this.smallImagePanel.TabIndex = 5;
            this.smallImagePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.smallImagePanel_Paint);
            // 
            // testingPanel
            // 
            this.testingPanel.BackColor = System.Drawing.Color.White;
            this.testingPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.testingPanel.BufferMask = ((System.Drawing.Bitmap)(resources.GetObject("testingPanel.BufferMask")));
            this.testingPanel.Location = new System.Drawing.Point(21, 19);
            this.testingPanel.Name = "testingPanel";
            this.testingPanel.Size = new System.Drawing.Size(200, 200);
            this.testingPanel.TabIndex = 0;
            // 
            // resultLbl
            // 
            this.resultLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.resultLbl.Location = new System.Drawing.Point(279, 67);
            this.resultLbl.Name = "resultLbl";
            this.resultLbl.Padding = new System.Windows.Forms.Padding(1);
            this.resultLbl.Size = new System.Drawing.Size(280, 215);
            this.resultLbl.TabIndex = 2;
            this.resultLbl.Text = "Result displays here...";
            // 
            // recognizeBtn
            // 
            this.recognizeBtn.Location = new System.Drawing.Point(279, 26);
            this.recognizeBtn.Name = "recognizeBtn";
            this.recognizeBtn.Size = new System.Drawing.Size(81, 23);
            this.recognizeBtn.TabIndex = 1;
            this.recognizeBtn.Text = "Recognize";
            this.recognizeBtn.UseVisualStyleBackColor = true;
            this.recognizeBtn.Click += new System.EventHandler(this.recognizeBtn_Click);
            // 
            // trainingTab
            // 
            this.trainingTab.Controls.Add(this.trainSetPathLbl);
            this.trainingTab.Controls.Add(this.patternBrowseBtn);
            this.trainingTab.Controls.Add(this.patternPathTxt);
            this.trainingTab.Controls.Add(this.processLbl);
            this.trainingTab.Controls.Add(this.trainBtn);
            this.trainingTab.Controls.Add(this.settingGroup);
            this.trainingTab.Location = new System.Drawing.Point(4, 22);
            this.trainingTab.Name = "trainingTab";
            this.trainingTab.Padding = new System.Windows.Forms.Padding(1);
            this.trainingTab.Size = new System.Drawing.Size(580, 299);
            this.trainingTab.TabIndex = 1;
            this.trainingTab.Text = "Training";
            this.trainingTab.UseVisualStyleBackColor = true;
            // 
            // trainSetPathLbl
            // 
            this.trainSetPathLbl.AutoSize = true;
            this.trainSetPathLbl.Location = new System.Drawing.Point(8, 189);
            this.trainSetPathLbl.Name = "trainSetPathLbl";
            this.trainSetPathLbl.Size = new System.Drawing.Size(86, 13);
            this.trainSetPathLbl.TabIndex = 11;
            this.trainSetPathLbl.Text = "Training set path";
            // 
            // patternBrowseBtn
            // 
            this.patternBrowseBtn.Location = new System.Drawing.Point(489, 184);
            this.patternBrowseBtn.Name = "patternBrowseBtn";
            this.patternBrowseBtn.Size = new System.Drawing.Size(75, 23);
            this.patternBrowseBtn.TabIndex = 10;
            this.patternBrowseBtn.Text = "Browse";
            this.patternBrowseBtn.UseVisualStyleBackColor = true;
            this.patternBrowseBtn.Click += new System.EventHandler(this.patternBrowseBtn_Click);
            // 
            // patternPathTxt
            // 
            this.patternPathTxt.Location = new System.Drawing.Point(100, 186);
            this.patternPathTxt.Name = "patternPathTxt";
            this.patternPathTxt.Size = new System.Drawing.Size(370, 20);
            this.patternPathTxt.TabIndex = 9;
            // 
            // processLbl
            // 
            this.processLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.processLbl.Location = new System.Drawing.Point(294, 27);
            this.processLbl.Name = "processLbl";
            this.processLbl.Padding = new System.Windows.Forms.Padding(1);
            this.processLbl.Size = new System.Drawing.Size(270, 139);
            this.processLbl.TabIndex = 4;
            this.processLbl.Text = "Select the path to training set and click button \"Train\"";
            // 
            // trainBtn
            // 
            this.trainBtn.Location = new System.Drawing.Point(429, 225);
            this.trainBtn.Name = "trainBtn";
            this.trainBtn.Size = new System.Drawing.Size(135, 58);
            this.trainBtn.TabIndex = 3;
            this.trainBtn.Text = "Train";
            this.trainBtn.UseVisualStyleBackColor = true;
            this.trainBtn.Click += new System.EventHandler(this.trainBtn_Click);
            // 
            // settingGroup
            // 
            this.settingGroup.Controls.Add(this.allowedErrorTxt);
            this.settingGroup.Controls.Add(this.allowedErrorLbl);
            this.settingGroup.Controls.Add(this.maxIterationTxt);
            this.settingGroup.Controls.Add(this.maxIterationLbl);
            this.settingGroup.Controls.Add(this.hidLayerElemCountTxt);
            this.settingGroup.Controls.Add(this.hidLayerElemCountLbl);
            this.settingGroup.Controls.Add(this.hidLayerCountTxt);
            this.settingGroup.Controls.Add(this.hidLayerCountLbl);
            this.settingGroup.Location = new System.Drawing.Point(11, 20);
            this.settingGroup.Name = "settingGroup";
            this.settingGroup.Size = new System.Drawing.Size(270, 146);
            this.settingGroup.TabIndex = 1;
            this.settingGroup.TabStop = false;
            this.settingGroup.Text = "Settings";
            // 
            // allowedErrorTxt
            // 
            this.allowedErrorTxt.Location = new System.Drawing.Point(170, 112);
            this.allowedErrorTxt.Name = "allowedErrorTxt";
            this.allowedErrorTxt.Size = new System.Drawing.Size(41, 20);
            this.allowedErrorTxt.TabIndex = 7;
            this.allowedErrorTxt.Text = "0.1";
            // 
            // allowedErrorLbl
            // 
            this.allowedErrorLbl.AutoSize = true;
            this.allowedErrorLbl.Location = new System.Drawing.Point(17, 115);
            this.allowedErrorLbl.Name = "allowedErrorLbl";
            this.allowedErrorLbl.Size = new System.Drawing.Size(68, 13);
            this.allowedErrorLbl.TabIndex = 6;
            this.allowedErrorLbl.Text = "Allowed error";
            // 
            // maxIterationTxt
            // 
            this.maxIterationTxt.Location = new System.Drawing.Point(170, 83);
            this.maxIterationTxt.Name = "maxIterationTxt";
            this.maxIterationTxt.Size = new System.Drawing.Size(82, 20);
            this.maxIterationTxt.TabIndex = 5;
            this.maxIterationTxt.Text = "10000";
            // 
            // maxIterationLbl
            // 
            this.maxIterationLbl.AutoSize = true;
            this.maxIterationLbl.Location = new System.Drawing.Point(17, 86);
            this.maxIterationLbl.Name = "maxIterationLbl";
            this.maxIterationLbl.Size = new System.Drawing.Size(97, 13);
            this.maxIterationLbl.TabIndex = 4;
            this.maxIterationLbl.Text = "Max iteration count";
            // 
            // hidLayerElemCountTxt
            // 
            this.hidLayerElemCountTxt.Location = new System.Drawing.Point(170, 53);
            this.hidLayerElemCountTxt.Name = "hidLayerElemCountTxt";
            this.hidLayerElemCountTxt.Size = new System.Drawing.Size(82, 20);
            this.hidLayerElemCountTxt.TabIndex = 3;
            this.hidLayerElemCountTxt.Text = "25";
            // 
            // hidLayerElemCountLbl
            // 
            this.hidLayerElemCountLbl.AutoSize = true;
            this.hidLayerElemCountLbl.Location = new System.Drawing.Point(17, 56);
            this.hidLayerElemCountLbl.Name = "hidLayerElemCountLbl";
            this.hidLayerElemCountLbl.Size = new System.Drawing.Size(136, 13);
            this.hidLayerElemCountLbl.TabIndex = 2;
            this.hidLayerElemCountLbl.Text = "Hidden layer element count";
            // 
            // hidLayerCountTxt
            // 
            this.hidLayerCountTxt.Location = new System.Drawing.Point(170, 23);
            this.hidLayerCountTxt.Name = "hidLayerCountTxt";
            this.hidLayerCountTxt.Size = new System.Drawing.Size(41, 20);
            this.hidLayerCountTxt.TabIndex = 1;
            this.hidLayerCountTxt.Text = "1";
            // 
            // hidLayerCountLbl
            // 
            this.hidLayerCountLbl.AutoSize = true;
            this.hidLayerCountLbl.Location = new System.Drawing.Point(17, 26);
            this.hidLayerCountLbl.Name = "hidLayerCountLbl";
            this.hidLayerCountLbl.Size = new System.Drawing.Size(96, 13);
            this.hidLayerCountLbl.TabIndex = 0;
            this.hidLayerCountLbl.Text = "Hidden layer count";
            // 
            // BackProgagationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 350);
            this.Controls.Add(this.tabPages);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "BackProgagationForm";
            this.Text = "BackProgagation - Letter recognition";
            this.tabPages.ResumeLayout(false);
            this.testingTab.ResumeLayout(false);
            this.testingCanvasGroup.ResumeLayout(false);
            this.trainingTab.ResumeLayout(false);
            this.trainingTab.PerformLayout();
            this.settingGroup.ResumeLayout(false);
            this.settingGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabPages;
        private System.Windows.Forms.TabPage testingTab;
        private System.Windows.Forms.TabPage trainingTab;
        private System.Windows.Forms.Label processLbl;
        private System.Windows.Forms.Button trainBtn;
        private System.Windows.Forms.GroupBox settingGroup;
        private System.Windows.Forms.TextBox hidLayerElemCountTxt;
        private System.Windows.Forms.Label hidLayerElemCountLbl;
        private System.Windows.Forms.TextBox hidLayerCountTxt;
        private CanvasPanel testingPanel;
        private System.Windows.Forms.Label resultLbl;
        private System.Windows.Forms.Button recognizeBtn;
        private System.Windows.Forms.GroupBox testingCanvasGroup;
        private System.Windows.Forms.Label maxIterationLbl;
        private System.Windows.Forms.TextBox allowedErrorTxt;
        private System.Windows.Forms.Label allowedErrorLbl;
        private System.Windows.Forms.TextBox maxIterationTxt;
        private System.Windows.Forms.Button testResetBtn;
        private System.Windows.Forms.Label hidLayerCountLbl;
        private System.Windows.Forms.Button patternBrowseBtn;
        private System.Windows.Forms.TextBox patternPathTxt;
        private System.Windows.Forms.Panel smallImagePanel;
        private System.Windows.Forms.Label trainSetPathLbl;
    }
}

