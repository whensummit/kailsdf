using NetMQ.Sockets;
using SevenStarAutoSell.Client.Common;
using SevenStarAutoSell.Common.Defs;
using SevenStarAutoSell.Common.Extensions;
using SevenStarAutoSell.Models;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading;

namespace SevenStarAutoSell.Client
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            txtAccount.Text = "admin";
            txtPwd.Text = "111111";
        }


        /// <summary>
        /// 发送登录请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            // 校验
            if (string.IsNullOrWhiteSpace(txtAccount.Text))
            {
                MessageBox.Show("请输入用户名！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtPwd.Text))
            {
                MessageBox.Show("请输入密码！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var loginParam = new SysMemberLoginParam()
            {
                UserName = txtAccount.Text.Trim(),
                Password = txtPwd.Text.Trim()
            };


            //启动与中心服务器的连接
            PublicData.RequestSocket = new RequestSocket();
            PublicData.RequestSocket.Connect(MQConfig.ResponseServer);

            var result = PublicData.RequestSocket.RequestSendReceive<MQActionResult<string>>(
                    IdKeyData.Create(PublicData.SessionId, "SysMember/Login", loginParam));
            if (result == null)
            {
                MessageBoxEx.Alert("连接服务器失败，请检查网络或联系管理员！");
                return;
            }
            else if (result.IsOK)
            {
                PublicData.SessionId = result.Data;
                PublicData.SysmemberAccount = loginParam.UserName;

                Visible = false;
                using (var formLogin = new MainForm() { StartPosition = FormStartPosition.CenterScreen, TopMost = true })
                {
                    //暂时采用模式调试
                    formLogin.Show();

                    //var diaResult = formLogin.ShowDialog();
                    //if (diaResult == DialogResult.Cancel)
                    //{
                    //    Close();
                    //    return;
                    //}
                    //else if (formLogin.ShowDialog() == DialogResult.Ignore)//重新登录
                    //{
                    //    Visible = true;
                    //    return;
                    //}
                }
            }
            else
            {
                MessageBoxEx.Alert(result.ErrorMsg);
                txtPwd.Focus();
                return;
            }
        }

        private void btnCancel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Close();
        }


        private void txtAccount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                txtPwd.Focus();
        }

        private void txtPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnLogin.PerformClick();
        }
    }
}
