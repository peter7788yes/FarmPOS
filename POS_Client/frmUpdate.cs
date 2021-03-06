using System;
using System.ComponentModel;
using System.Deployment.Application;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace POS_Client
{
	public class frmUpdate : Form
	{
		private IContainer components;

		private ProgressBar pbStatus;

		public frmUpdate()
		{
			InitializeComponent();
		}

		private void frmUpdate_Load(object sender, EventArgs e)
		{
			if (Program.IsDeployClickOnce)
			{
				if (NetworkInterface.GetIsNetworkAvailable())
				{
					GoCheckUpdate();
				}
				else
				{
					Close();
				}
			}
			else
			{
				Close();
			}
		}

		private void GoCheckUpdate()
		{
			UpdateProgram();
		}

		private void UpdateProgram()
		{
			try
			{
				Program.Logger.Info("[ClickOnce] -- CheckForUpdate");
				ApplicationDeployment currentDeployment = ApplicationDeployment.CurrentDeployment;
				if (ApplicationDeployment.CurrentDeployment.CheckForUpdate())
				{
					if (MessageBox.Show("發現新的版本,是否確定更新", "更新通知", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
					{
						currentDeployment.UpdateProgressChanged += new DeploymentProgressChangedEventHandler(obj_UpdateProgressChanged);
						currentDeployment.UpdateCompleted += new AsyncCompletedEventHandler(obj_UpdateCompleted);
						currentDeployment.UpdateAsync();
					}
					else
					{
						Program.Logger.Info("[ClickOnce] -- 使用者取消更新");
						Close();
					}
				}
				else
				{
					Program.Logger.Info("[ClickOnce] -- 程式已為最新版本:" + Program.Version);
					Close();
				}
			}
			catch (Exception ex)
			{
				Program.Logger.Fatal("[ClickOnce] --" + ex.ToString());
				MessageBox.Show("更新程式失敗:目前為離線狀態。");
				Close();
			}
		}

		private void obj_UpdateProgressChanged(object sender, DeploymentProgressChangedEventArgs e)
		{
			pbStatus.Value = e.ProgressPercentage;
			Application.DoEvents();
		}

		private void obj_UpdateCompleted(object sender, AsyncCompletedEventArgs e)
		{
			Program.Logger.Info("[ClickOnce] -- 更新完成");
			MessageBox.Show("更新完成，重新啟動程式");
			Program.Upgraded = true;
			Close();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POS_Client.frmUpdate));
			pbStatus = new System.Windows.Forms.ProgressBar();
			SuspendLayout();
			pbStatus.Location = new System.Drawing.Point(12, 12);
			pbStatus.Name = "pbStatus";
			pbStatus.Size = new System.Drawing.Size(295, 23);
			pbStatus.TabIndex = 0;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(319, 46);
			base.ControlBox = false;
			base.Controls.Add(pbStatus);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "frmUpdate";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "程式版本更新中，請稍候";
			base.Load += new System.EventHandler(frmUpdate_Load);
			ResumeLayout(false);
		}
	}
}
