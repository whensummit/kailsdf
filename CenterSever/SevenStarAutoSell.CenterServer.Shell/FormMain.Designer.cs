﻿namespace SevenStarAutoSell.CenterServer.Shell
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnPushMessage = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnPushMessage
            // 
            this.btnPushMessage.Location = new System.Drawing.Point(166, 99);
            this.btnPushMessage.Name = "btnPushMessage";
            this.btnPushMessage.Size = new System.Drawing.Size(105, 48);
            this.btnPushMessage.TabIndex = 0;
            this.btnPushMessage.Text = "推送消息";
            this.btnPushMessage.UseVisualStyleBackColor = true;
            this.btnPushMessage.Click += new System.EventHandler(this.btnPushMessage_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 325);
            this.Controls.Add(this.btnPushMessage);
            this.Name = "FormMain";
            this.Text = "中心服务器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPushMessage;
    }
}

