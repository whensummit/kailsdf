namespace SevenStarAutoSell.Client.UserControls
{
    partial class UcMainContentQuickInput
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvBetNoList = new System.Windows.Forms.DataGridView();
            this.orderNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.company = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sellTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.betNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.money = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.state = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.operate = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvOddsList = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buyCompany = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.odds = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.canBetMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.btnDirectSell = new System.Windows.Forms.Button();
            this.lblAverageOdds = new System.Windows.Forms.Label();
            this.lblCanBetMoney = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBetMoney = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBetNo = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBetNoList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOddsList)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(729, 462);
            this.panel1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvBetNoList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvOddsList);
            this.splitContainer1.Size = new System.Drawing.Size(719, 307);
            this.splitContainer1.SplitterDistance = 483;
            this.splitContainer1.TabIndex = 3;
            // 
            // dgvBetNoList
            // 
            this.dgvBetNoList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBetNoList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.orderNumber,
            this.company,
            this.sellTime,
            this.betNo,
            this.money,
            this.state,
            this.operate});
            this.dgvBetNoList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBetNoList.Location = new System.Drawing.Point(0, 0);
            this.dgvBetNoList.Name = "dgvBetNoList";
            this.dgvBetNoList.RowHeadersVisible = false;
            this.dgvBetNoList.RowTemplate.Height = 23;
            this.dgvBetNoList.Size = new System.Drawing.Size(483, 307);
            this.dgvBetNoList.TabIndex = 3;
            // 
            // orderNumber
            // 
            this.orderNumber.HeaderText = "注单编号";
            this.orderNumber.Name = "orderNumber";
            this.orderNumber.Width = 80;
            // 
            // company
            // 
            this.company.HeaderText = "出货公司";
            this.company.Name = "company";
            this.company.Width = 80;
            // 
            // sellTime
            // 
            this.sellTime.HeaderText = "时间";
            this.sellTime.Name = "sellTime";
            // 
            // betNo
            // 
            this.betNo.HeaderText = "号码";
            this.betNo.Name = "betNo";
            this.betNo.Width = 40;
            // 
            // money
            // 
            this.money.HeaderText = "金额";
            this.money.Name = "money";
            this.money.Width = 60;
            // 
            // state
            // 
            this.state.HeaderText = "状态";
            this.state.Name = "state";
            this.state.Width = 80;
            // 
            // operate
            // 
            this.operate.HeaderText = "撤单";
            this.operate.Name = "operate";
            this.operate.Width = 80;
            // 
            // dgvOddsList
            // 
            this.dgvOddsList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOddsList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.buyCompany,
            this.odds,
            this.canBetMoney});
            this.dgvOddsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOddsList.Location = new System.Drawing.Point(0, 0);
            this.dgvOddsList.Name = "dgvOddsList";
            this.dgvOddsList.RowHeadersVisible = false;
            this.dgvOddsList.RowTemplate.Height = 23;
            this.dgvOddsList.Size = new System.Drawing.Size(232, 307);
            this.dgvOddsList.TabIndex = 1;
            this.dgvOddsList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOddsList_CellContentClick);
            // 
            // ID
            // 
            this.ID.DataPropertyName = "Id";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.Visible = false;
            // 
            // buyCompany
            // 
            this.buyCompany.DataPropertyName = "CompanyName";
            this.buyCompany.HeaderText = "公司";
            this.buyCompany.Name = "buyCompany";
            // 
            // odds
            // 
            this.odds.DataPropertyName = "Odds";
            this.odds.HeaderText = "赔率";
            this.odds.Name = "odds";
            this.odds.Width = 60;
            // 
            // canBetMoney
            // 
            this.canBetMoney.DataPropertyName = "CanBetMoney";
            this.canBetMoney.HeaderText = "可下金额";
            this.canBetMoney.Name = "canBetMoney";
            this.canBetMoney.Width = 80;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(719, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(10, 307);
            this.panel3.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.btnDirectSell);
            this.panel2.Controls.Add(this.lblAverageOdds);
            this.panel2.Controls.Add(this.lblCanBetMoney);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.txtBetMoney);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.txtBetNo);
            this.panel2.Controls.Add(this.checkBox1);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.radioButton4);
            this.panel2.Controls.Add(this.radioButton3);
            this.panel2.Controls.Add(this.radioButton2);
            this.panel2.Controls.Add(this.radioButton1);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 307);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(729, 155);
            this.panel2.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(555, 38);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 29);
            this.button2.TabIndex = 16;
            this.button2.Text = "比例出货";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            // 
            // btnDirectSell
            // 
            this.btnDirectSell.Location = new System.Drawing.Point(454, 38);
            this.btnDirectSell.Name = "btnDirectSell";
            this.btnDirectSell.Size = new System.Drawing.Size(75, 30);
            this.btnDirectSell.TabIndex = 15;
            this.btnDirectSell.Text = "直接出货";
            this.btnDirectSell.UseVisualStyleBackColor = true;
            this.btnDirectSell.Click += new System.EventHandler(this.btnDirectSell_Click);
            // 
            // lblAverageOdds
            // 
            this.lblAverageOdds.AutoSize = true;
            this.lblAverageOdds.ForeColor = System.Drawing.Color.Red;
            this.lblAverageOdds.Location = new System.Drawing.Point(404, 55);
            this.lblAverageOdds.Name = "lblAverageOdds";
            this.lblAverageOdds.Size = new System.Drawing.Size(0, 12);
            this.lblAverageOdds.TabIndex = 14;
            // 
            // lblCanBetMoney
            // 
            this.lblCanBetMoney.AutoSize = true;
            this.lblCanBetMoney.ForeColor = System.Drawing.Color.Red;
            this.lblCanBetMoney.Location = new System.Drawing.Point(404, 38);
            this.lblCanBetMoney.Name = "lblCanBetMoney";
            this.lblCanBetMoney.Size = new System.Drawing.Size(0, 12);
            this.lblCanBetMoney.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(345, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "平均赔率：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(345, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "可下金额：";
            // 
            // txtBetMoney
            // 
            this.txtBetMoney.Location = new System.Drawing.Point(255, 44);
            this.txtBetMoney.Name = "txtBetMoney";
            this.txtBetMoney.Size = new System.Drawing.Size(84, 21);
            this.txtBetMoney.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(201, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "金额：";
            // 
            // txtBetNo
            // 
            this.txtBetNo.Location = new System.Drawing.Point(132, 45);
            this.txtBetNo.MaxLength = 4;
            this.txtBetNo.Name = "txtBetNo";
            this.txtBetNo.Size = new System.Drawing.Size(64, 21);
            this.txtBetNo.TabIndex = 8;
            this.txtBetNo.TextChanged += new System.EventHandler(this.txtBetNo_TextChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(39, 51);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(76, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "号码：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "全转";
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(282, 11);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(53, 16);
            this.radioButton4.TabIndex = 4;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "公司4";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(223, 11);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(53, 16);
            this.radioButton3.TabIndex = 3;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "公司3";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(164, 11);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(53, 16);
            this.radioButton2.TabIndex = 2;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "公司2";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(105, 11);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(53, 16);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "公司1";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(4, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "出货公司：";
            // 
            // UcMainContentQuickInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "UcMainContentQuickInput";
            this.Size = new System.Drawing.Size(729, 462);
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBetNoList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOddsList)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgvOddsList;
        private System.Windows.Forms.DataGridView dgvBetNoList;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox txtBetNo;
        private System.Windows.Forms.TextBox txtBetMoney;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblCanBetMoney;
        private System.Windows.Forms.Label lblAverageOdds;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnDirectSell;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn company;
        private System.Windows.Forms.DataGridViewTextBoxColumn sellTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn betNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn money;
        private System.Windows.Forms.DataGridViewTextBoxColumn state;
        private System.Windows.Forms.DataGridViewButtonColumn operate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn buyCompany;
        private System.Windows.Forms.DataGridViewTextBoxColumn odds;
        private System.Windows.Forms.DataGridViewTextBoxColumn canBetMoney;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}
