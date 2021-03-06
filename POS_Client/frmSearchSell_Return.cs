using POS_Client.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;

namespace POS_Client
{
	public class frmSearchSell_Return : MasterThinForm
	{
		private ucSellInfo_Return[] ucsells;

		public DataTable dt;

		private IContainer components;

		private Button btn_cancel;

		private Button btn_enter;

		private Button btn_reset;

		private Panel panel5;

		private Label label10;

		private Panel panel3;

		private Label label6;

		private TextBox sellNo;

		private TextBox tb_MemberName;

		private TableLayoutPanel tableLayoutPanel1;

		private Panel panel1;

		private ucSellInfo_Return ucSellInfo_Return2;

		private ucSellInfo_Return ucSellInfo_Return1;

		private ucSellInfo_Return ucSellInfo_Return3;

		private Button btn_view;

		private Panel panel6;

		private Label label12;

		private Panel panel2;

		private DateTimePicker eDate;

		private DateTimePicker sDate;

		private Label label2;

		private Panel panel4;

		private Label label3;

		private TextBox tb_phone;

		private TableLayoutPanel tableLayoutPanel2;

		private Panel panel7;

		private Label label4;

		private Panel panel8;

		private Label label5;

		private Panel panel9;

		private Label label7;

		private Panel panel10;

		private Label label8;

		private Panel panel11;

		private DateTimePicker dateTimePicker1;

		private DateTimePicker dateTimePicker2;

		private Label label9;

		private Label label11;

		private Panel panel12;

		private Button button1;

		private Panel panel13;

		private Panel panel14;

		private Label label1;

		private TextBox tb_barcode;

		public frmSearchSell_Return()
			: base("銷售單|退貨|補印收據")
		{
			InitializeComponent();
			sDate.Value = DateTime.Today.AddDays(-30.0);
			eDate.Value = DateTime.Today;
			sellNo.Focus();
			ucsells = new ucSellInfo_Return[3]
			{
				ucSellInfo_Return1,
				ucSellInfo_Return2,
				ucSellInfo_Return3
			};
			ucSellInfo_Return[] array = ucsells;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnClickMember += new EventHandler(viewMemberInfo);
			}
			dt = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "hms.sellNo,hms.sellTime,hms.memberId,hms.items,hcr.Name,hcr.Mobile,hcr.IdNo,hcr.CompanyIdNo,hms.cash,hms.Credit,hms.sum", "hypos_main_sell as hms left outer join hypos_CUST_RTL as hcr on hms.memberId= hcr.VipNo", "", "hms.editDate desc", null, null, CommandOperationType.ExecuteReaderReturnDataTable);
			int num = 0;
			for (int j = 0; j < 3; j++)
			{
				if (j < dt.Rows.Count)
				{
					string text = dt.Rows[j]["IdNo"].ToString();
					if (text.Length == 10)
					{
						text = text.Substring(0, 2) + "***" + text.Substring(5, 5);
					}
					if (!string.IsNullOrEmpty(dt.Rows[j]["Name"].ToString()))
					{
						ucsells[num].setMemberName(dt.Rows[j]["Name"].ToString());
					}
					else
					{
						ucsells[num].setMemberName("非會員");
					}
					ucsells[num].setsellNo(dt.Rows[j]["sellNo"].ToString());
					ucsells[num].setsellDate("銷售日期: " + dt.Rows[j]["sellTime"].ToString());
					ucsells[num].setcellphone(dt.Rows[j]["Mobile"].ToString());
					ucsells[num].setmemberNo("會員號: " + dt.Rows[j]["memberId"].ToString());
					ucsells[num].setIdNo("身分證字號: " + text);
					ucsells[num].setCompanyIdno("統一編號: " + dt.Rows[j]["CompanyIdNo"].ToString());
					ucsells[num].setPayType("付款模式: 現金(" + dt.Rows[j]["cash"].ToString() + ") / 賒帳(" + dt.Rows[j]["Credit"].ToString() + ")");
					ucsells[num].setItem("購買品項: " + dt.Rows[j]["items"].ToString());
					ucsells[num].setSum("消費總額: " + dt.Rows[j]["sum"].ToString());
					ucsells[num].Visible = true;
				}
				else
				{
					ucsells[num].Visible = false;
				}
				ucsells[num].BackColor = Color.White;
				num++;
			}
		}

		public void viewMemberInfo(object sellNumber, EventArgs s)
		{
			switchForm(new frmMainShopSimpleReturnWithMoney(sellNumber.ToString(), "frmSearchSell_Return", ""));
		}

		private void btn_cancel_Click(object sender, EventArgs e)
		{
			switchForm(new frmMain());
		}

		private void tb_sellNo_Enter(object sender, EventArgs e)
		{
			if (sellNo.Text == "請輸入銷售單號")
			{
				sellNo.Text = "";
			}
		}

		private void tb_sellNo_Leave(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(sellNo.Text))
			{
				sellNo.Text = "請輸入銷售單號";
			}
		}

		private void tb_MemberName_Enter(object sender, EventArgs e)
		{
			if (tb_MemberName.Text == "請輸入會員姓名")
			{
				tb_MemberName.Text = "";
			}
		}

		private void tb_MemberName_Leave(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(tb_MemberName.Text))
			{
				tb_MemberName.Text = "請輸入會員姓名";
			}
		}

		private void btn_reset_Click(object sender, EventArgs e)
		{
			sellNo.Text = "請輸入銷售單號";
			tb_MemberName.Text = "請輸入會員姓名";
			sDate.Checked = false;
			eDate.Checked = false;
			sDate.Text = "";
			eDate.Text = "";
			tb_barcode.Text = "";
			sellNo.Focus();
		}

		private void tb_SellNo_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				btn_enter_Click(sender, e);
			}
		}

		private void tb_MemberName_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				btn_enter_Click(sender, e);
			}
		}

		private void btn_enter_Click(object sender, EventArgs e)
		{
			string text = "";
			if (sDate.Checked && eDate.Checked)
			{
				DateTime t = Convert.ToDateTime(sDate.Value);
				DateTime t2 = Convert.ToDateTime(eDate.Value);
				if (DateTime.Compare(t, t2) > 0)
				{
					text += "起日不可大於迄日，請重新設定\n";
				}
			}
			if (sellNo.Text == "請輸入銷售單號" && tb_MemberName.Text == "請輸入會員姓名" && tb_phone.Text == "請輸入會員電話" && !sDate.Checked && !eDate.Checked)
			{
				text += "必須輸入查詢條件\n";
			}
			if (!"".Equals(text))
			{
				AutoClosingMessageBox.Show(text);
				return;
			}
			if (sellNo.Text != "請輸入銷售單號")
			{
				dt = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "*", "hypos_main_sell", "sellNo={0}", "", null, new string[1]
				{
					sellNo.Text
				}, CommandOperationType.ExecuteReaderReturnDataTable);
				if (dt.Rows.Count > 0)
				{
					switchForm(new frmMainShopSimpleReturnWithMoney(dt.Rows[0]["sellNo"].ToString(), "frmSearchSell_Return", ""));
				}
				else
				{
					AutoClosingMessageBox.Show("銷售單不存在，請正確輸入銷售單編號");
				}
				return;
			}
			int num = 0;
			List<string> list = new List<string>();
			string text2 = "SELECT distinct hms.sellNo,hms.sellTime,hms.memberId,hms.items,hcr.Name,hcr.Mobile  ,hcr.IdNo,hcr.CompanyIdNo,hms.cash,hms.Credit,hms.sum FROM hypos_main_sell as hms left outer join hypos_CUST_RTL as hcr on hms.memberId= hcr.VipNo join hypos_detail_sell on hms.sellNo = hypos_detail_sell.sellNo WHERE 1=1 ";
			if (tb_MemberName.Text != "請輸入會員姓名")
			{
				if ("非會員".Equals(tb_MemberName.Text))
				{
					text2 += " AND hcr.Name is null";
				}
				else
				{
					text2 = text2 + " AND hcr.Name like {" + num + "}";
					list.Add("%" + tb_MemberName.Text.ToString().Trim() + "%");
					num++;
				}
			}
			if (tb_phone.Text != "請輸入會員電話")
			{
				text2 = text2 + " AND (hcr.Telphone like {" + num + "} or hcr.Mobile like {" + num + "})";
				list.Add("%" + tb_phone.Text.ToString().Trim() + "%");
				num++;
			}
			if (tb_barcode.Text != "請輸入商品編號")
			{
				text2 = text2 + " and  hypos_detail_sell.barcode like {" + num + "}";
				list.Add("%" + tb_barcode.Text.ToString().Trim() + "%");
				num++;
			}
			if (sDate.Checked)
			{
				string str = sDate.Text.ToString();
				str += " 00:00:00";
				text2 = text2 + " AND hms.sellTime >= {" + num + "}";
				list.Add(str);
				num++;
			}
			if (eDate.Checked)
			{
				string str2 = eDate.Text.ToString();
				str2 += " 23:59:59";
				text2 = text2 + " AND hms.sellTime <= {" + num + "}";
				list.Add(str2);
				num++;
			}
			DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, text2, list.ToArray(), CommandOperationType.ExecuteReaderReturnDataTable);
			switchForm(new frmSearchSellResult(dataTable, "frmSearchSell_Return"));
		}

		private void btn_view_Click(object sender, EventArgs e)
		{
			DateTime dateTime = DateTime.Now.AddMonths(-2);
			DateTime now = DateTime.Now;
			string text = new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
			string text2 = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month), 23, 59, 59).ToString("yyyy-MM-dd HH:mm:ss");
			string sql = "SELECT hms.sellNo,hms.sellTime,hms.memberId,hms.items,hcr.Name,hcr.Mobile ,hcr.IdNo,hcr.CompanyIdNo,hms.cash,hms.Credit,hms.sum FROM hypos_main_sell as hms left outer join hypos_CUST_RTL as hcr on hms.memberId= hcr.VipNo WHERE sellTime >='" + text + "' and sellTime <='" + text2 + "'";
			DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, sql, null, CommandOperationType.ExecuteReaderReturnDataTable);
			if (dataTable.Rows.Count > 0)
			{
				switchForm(new frmSearchSellResult_View(dataTable));
			}
			else
			{
				AutoClosingMessageBox.Show("無近期銷售單");
			}
		}

		private void tb_phone_Enter(object sender, EventArgs e)
		{
			if (tb_phone.Text == "請輸入會員電話")
			{
				tb_phone.Text = "";
			}
		}

		private void tb_phone_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				btn_enter_Click(sender, e);
			}
		}

		private void tb_phone_Leave(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(tb_phone.Text))
			{
				tb_phone.Text = "請輸入會員電話";
			}
		}

		private void tb_barcode_Enter(object sender, EventArgs e)
		{
			if (tb_barcode.Text == "請輸入商品編號")
			{
				tb_barcode.Text = "";
			}
		}

		private void tb_barcode_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				btn_enter_Click(sender, e);
			}
		}

		private void tb_barcode_Leave_1(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(tb_barcode.Text))
			{
				tb_barcode.Text = "請輸入商品編號";
			}
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POS_Client.frmSearchSell_Return));
			btn_cancel = new System.Windows.Forms.Button();
			btn_enter = new System.Windows.Forms.Button();
			btn_reset = new System.Windows.Forms.Button();
			panel5 = new System.Windows.Forms.Panel();
			label10 = new System.Windows.Forms.Label();
			panel3 = new System.Windows.Forms.Panel();
			label6 = new System.Windows.Forms.Label();
			sellNo = new System.Windows.Forms.TextBox();
			tb_MemberName = new System.Windows.Forms.TextBox();
			tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			tb_barcode = new System.Windows.Forms.TextBox();
			panel14 = new System.Windows.Forms.Panel();
			label1 = new System.Windows.Forms.Label();
			panel4 = new System.Windows.Forms.Panel();
			label3 = new System.Windows.Forms.Label();
			panel2 = new System.Windows.Forms.Panel();
			eDate = new System.Windows.Forms.DateTimePicker();
			sDate = new System.Windows.Forms.DateTimePicker();
			label2 = new System.Windows.Forms.Label();
			panel13 = new System.Windows.Forms.Panel();
			tb_phone = new System.Windows.Forms.TextBox();
			panel6 = new System.Windows.Forms.Panel();
			label12 = new System.Windows.Forms.Label();
			panel1 = new System.Windows.Forms.Panel();
			ucSellInfo_Return3 = new POS_Client.ucSellInfo_Return();
			ucSellInfo_Return2 = new POS_Client.ucSellInfo_Return();
			ucSellInfo_Return1 = new POS_Client.ucSellInfo_Return();
			btn_view = new System.Windows.Forms.Button();
			tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			panel7 = new System.Windows.Forms.Panel();
			label4 = new System.Windows.Forms.Label();
			panel8 = new System.Windows.Forms.Panel();
			label5 = new System.Windows.Forms.Label();
			panel9 = new System.Windows.Forms.Panel();
			label7 = new System.Windows.Forms.Label();
			panel10 = new System.Windows.Forms.Panel();
			label8 = new System.Windows.Forms.Label();
			panel11 = new System.Windows.Forms.Panel();
			dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
			label9 = new System.Windows.Forms.Label();
			label11 = new System.Windows.Forms.Label();
			panel12 = new System.Windows.Forms.Panel();
			button1 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)pb_virtualKeyBoard).BeginInit();
			panel5.SuspendLayout();
			panel3.SuspendLayout();
			tableLayoutPanel1.SuspendLayout();
			panel14.SuspendLayout();
			panel4.SuspendLayout();
			panel2.SuspendLayout();
			panel13.SuspendLayout();
			panel6.SuspendLayout();
			panel1.SuspendLayout();
			tableLayoutPanel2.SuspendLayout();
			panel7.SuspendLayout();
			panel8.SuspendLayout();
			panel9.SuspendLayout();
			panel10.SuspendLayout();
			panel11.SuspendLayout();
			SuspendLayout();
			btn_cancel.BackColor = System.Drawing.Color.FromArgb(175, 175, 175);
			btn_cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			btn_cancel.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
			btn_cancel.ForeColor = System.Drawing.Color.White;
			btn_cancel.Location = new System.Drawing.Point(568, 294);
			btn_cancel.Name = "btn_cancel";
			btn_cancel.Size = new System.Drawing.Size(75, 35);
			btn_cancel.TabIndex = 43;
			btn_cancel.TabStop = false;
			btn_cancel.Text = "取消";
			btn_cancel.UseVisualStyleBackColor = false;
			btn_cancel.Click += new System.EventHandler(btn_cancel_Click);
			btn_enter.BackColor = System.Drawing.Color.FromArgb(157, 189, 59);
			btn_enter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			btn_enter.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
			btn_enter.ForeColor = System.Drawing.Color.White;
			btn_enter.Location = new System.Drawing.Point(366, 294);
			btn_enter.Name = "btn_enter";
			btn_enter.Size = new System.Drawing.Size(75, 35);
			btn_enter.TabIndex = 1;
			btn_enter.TabStop = false;
			btn_enter.Text = "查詢";
			btn_enter.UseVisualStyleBackColor = false;
			btn_enter.Click += new System.EventHandler(btn_enter_Click);
			btn_reset.BackColor = System.Drawing.Color.FromArgb(175, 175, 175);
			btn_reset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			btn_reset.Font = new System.Drawing.Font("微軟正黑體", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
			btn_reset.ForeColor = System.Drawing.Color.White;
			btn_reset.Location = new System.Drawing.Point(467, 294);
			btn_reset.Name = "btn_reset";
			btn_reset.Size = new System.Drawing.Size(75, 35);
			btn_reset.TabIndex = 42;
			btn_reset.TabStop = false;
			btn_reset.Text = "重設";
			btn_reset.UseVisualStyleBackColor = false;
			btn_reset.Click += new System.EventHandler(btn_reset_Click);
			panel5.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
			panel5.Controls.Add(label10);
			panel5.Location = new System.Drawing.Point(1, 119);
			panel5.Margin = new System.Windows.Forms.Padding(0);
			panel5.Name = "panel5";
			panel5.Size = new System.Drawing.Size(194, 58);
			panel5.TabIndex = 23;
			label10.AutoSize = true;
			label10.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
			label10.ForeColor = System.Drawing.Color.White;
			label10.Location = new System.Drawing.Point(65, 15);
			label10.Name = "label10";
			label10.Size = new System.Drawing.Size(74, 21);
			label10.TabIndex = 0;
			label10.Text = "會員姓名";
			panel3.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
			panel3.Controls.Add(label6);
			panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			panel3.Location = new System.Drawing.Point(1, 1);
			panel3.Margin = new System.Windows.Forms.Padding(0);
			panel3.Name = "panel3";
			panel3.Size = new System.Drawing.Size(194, 58);
			panel3.TabIndex = 21;
			label6.AutoSize = true;
			label6.BackColor = System.Drawing.Color.Transparent;
			label6.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
			label6.ForeColor = System.Drawing.Color.White;
			label6.Location = new System.Drawing.Point(65, 15);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(74, 21);
			label6.TabIndex = 0;
			label6.Text = "銷售編號";
			sellNo.BackColor = System.Drawing.Color.White;
			sellNo.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
			sellNo.ForeColor = System.Drawing.Color.DarkGray;
			sellNo.ImeMode = System.Windows.Forms.ImeMode.Disable;
			sellNo.Location = new System.Drawing.Point(211, 16);
			sellNo.Margin = new System.Windows.Forms.Padding(15);
			sellNo.MaxLength = 0;
			sellNo.Name = "sellNo";
			sellNo.Size = new System.Drawing.Size(674, 29);
			sellNo.TabIndex = 1;
			sellNo.Text = "請輸入銷售單號";
			sellNo.Enter += new System.EventHandler(tb_sellNo_Enter);
			sellNo.KeyDown += new System.Windows.Forms.KeyEventHandler(tb_SellNo_KeyDown);
			sellNo.Leave += new System.EventHandler(tb_sellNo_Leave);
			tb_MemberName.BackColor = System.Drawing.Color.White;
			tb_MemberName.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
			tb_MemberName.ForeColor = System.Drawing.Color.DarkGray;
			tb_MemberName.Location = new System.Drawing.Point(9, 13);
			tb_MemberName.Margin = new System.Windows.Forms.Padding(15);
			tb_MemberName.Name = "tb_MemberName";
			tb_MemberName.Size = new System.Drawing.Size(255, 29);
			tb_MemberName.TabIndex = 2;
			tb_MemberName.Text = "請輸入會員姓名";
			tb_MemberName.Enter += new System.EventHandler(tb_MemberName_Enter);
			tb_MemberName.KeyDown += new System.Windows.Forms.KeyEventHandler(tb_MemberName_KeyDown);
			tb_MemberName.Leave += new System.EventHandler(tb_MemberName_Leave);
			tableLayoutPanel1.BackColor = System.Drawing.Color.White;
			tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
			tableLayoutPanel1.ColumnCount = 2;
			tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.63662f));
			tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 78.36338f));
			tableLayoutPanel1.Controls.Add(tb_barcode, 1, 1);
			tableLayoutPanel1.Controls.Add(panel14, 0, 1);
			tableLayoutPanel1.Controls.Add(panel4, 0, 3);
			tableLayoutPanel1.Controls.Add(panel3, 0, 0);
			tableLayoutPanel1.Controls.Add(sellNo, 1, 0);
			tableLayoutPanel1.Controls.Add(panel2, 1, 3);
			tableLayoutPanel1.Controls.Add(panel13, 1, 2);
			tableLayoutPanel1.Controls.Add(panel5, 0, 2);
			tableLayoutPanel1.Location = new System.Drawing.Point(44, 43);
			tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 4;
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25f));
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25f));
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25f));
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25f));
			tableLayoutPanel1.Size = new System.Drawing.Size(901, 240);
			tableLayoutPanel1.TabIndex = 40;
			tb_barcode.BackColor = System.Drawing.Color.White;
			tb_barcode.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
			tb_barcode.ForeColor = System.Drawing.Color.DarkGray;
			tb_barcode.ImeMode = System.Windows.Forms.ImeMode.Disable;
			tb_barcode.Location = new System.Drawing.Point(211, 75);
			tb_barcode.Margin = new System.Windows.Forms.Padding(15);
			tb_barcode.MaxLength = 0;
			tb_barcode.Name = "tb_barcode";
			tb_barcode.Size = new System.Drawing.Size(674, 29);
			tb_barcode.TabIndex = 55;
			tb_barcode.Text = "請輸入商品編號";
			tb_barcode.Enter += new System.EventHandler(tb_barcode_Enter);
			tb_barcode.KeyDown += new System.Windows.Forms.KeyEventHandler(tb_barcode_KeyDown);
			tb_barcode.Leave += new System.EventHandler(tb_barcode_Leave_1);
			panel14.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
			panel14.Controls.Add(label1);
			panel14.Location = new System.Drawing.Point(1, 60);
			panel14.Margin = new System.Windows.Forms.Padding(0);
			panel14.Name = "panel14";
			panel14.Size = new System.Drawing.Size(194, 58);
			panel14.TabIndex = 27;
			label1.AutoSize = true;
			label1.BackColor = System.Drawing.Color.Transparent;
			label1.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
			label1.ForeColor = System.Drawing.Color.White;
			label1.Location = new System.Drawing.Point(62, 18);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(74, 21);
			label1.TabIndex = 0;
			label1.Text = "商品編號";
			panel4.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
			panel4.Controls.Add(label3);
			panel4.Dock = System.Windows.Forms.DockStyle.Fill;
			panel4.Location = new System.Drawing.Point(1, 178);
			panel4.Margin = new System.Windows.Forms.Padding(0);
			panel4.Name = "panel4";
			panel4.Size = new System.Drawing.Size(194, 61);
			panel4.TabIndex = 25;
			label3.AutoSize = true;
			label3.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
			label3.ForeColor = System.Drawing.Color.White;
			label3.Location = new System.Drawing.Point(61, 16);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(74, 21);
			label3.TabIndex = 0;
			label3.Text = "銷售日期";
			panel2.Controls.Add(eDate);
			panel2.Controls.Add(sDate);
			panel2.Controls.Add(label2);
			panel2.Location = new System.Drawing.Point(199, 181);
			panel2.Name = "panel2";
			panel2.Size = new System.Drawing.Size(539, 47);
			panel2.TabIndex = 24;
			eDate.Anchor = System.Windows.Forms.AnchorStyles.Left;
			eDate.CustomFormat = "yyyy-MM-dd";
			eDate.Font = new System.Drawing.Font("微軟正黑體", 15f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
			eDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			eDate.Location = new System.Drawing.Point(289, 6);
			eDate.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
			eDate.Name = "eDate";
			eDate.ShowCheckBox = true;
			eDate.Size = new System.Drawing.Size(221, 34);
			eDate.TabIndex = 33;
			sDate.Anchor = System.Windows.Forms.AnchorStyles.Left;
			sDate.CustomFormat = "yyyy-MM-dd";
			sDate.Font = new System.Drawing.Font("微軟正黑體", 15f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
			sDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			sDate.Location = new System.Drawing.Point(14, 7);
			sDate.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
			sDate.Name = "sDate";
			sDate.ShowCheckBox = true;
			sDate.Size = new System.Drawing.Size(226, 34);
			sDate.TabIndex = 32;
			label2.AutoSize = true;
			label2.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
			label2.Location = new System.Drawing.Point(255, 14);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(21, 20);
			label2.TabIndex = 1;
			label2.Text = "~";
			panel13.Controls.Add(tb_phone);
			panel13.Controls.Add(tb_MemberName);
			panel13.Controls.Add(panel6);
			panel13.Location = new System.Drawing.Point(199, 122);
			panel13.Name = "panel13";
			panel13.Size = new System.Drawing.Size(698, 52);
			panel13.TabIndex = 26;
			tb_phone.BackColor = System.Drawing.Color.White;
			tb_phone.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
			tb_phone.ForeColor = System.Drawing.Color.DarkGray;
			tb_phone.ImeMode = System.Windows.Forms.ImeMode.Disable;
			tb_phone.Location = new System.Drawing.Point(427, 15);
			tb_phone.Margin = new System.Windows.Forms.Padding(15);
			tb_phone.MaxLength = 0;
			tb_phone.Name = "tb_phone";
			tb_phone.Size = new System.Drawing.Size(259, 29);
			tb_phone.TabIndex = 53;
			tb_phone.Text = "請輸入會員電話";
			tb_phone.Enter += new System.EventHandler(tb_phone_Enter);
			tb_phone.KeyDown += new System.Windows.Forms.KeyEventHandler(tb_phone_KeyDown);
			tb_phone.Leave += new System.EventHandler(tb_phone_Leave);
			panel6.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
			panel6.Controls.Add(label12);
			panel6.Location = new System.Drawing.Point(270, 0);
			panel6.Margin = new System.Windows.Forms.Padding(0);
			panel6.Name = "panel6";
			panel6.Size = new System.Drawing.Size(150, 58);
			panel6.TabIndex = 20;
			label12.AutoSize = true;
			label12.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
			label12.ForeColor = System.Drawing.Color.White;
			label12.Location = new System.Drawing.Point(73, 17);
			label12.Name = "label12";
			label12.Size = new System.Drawing.Size(74, 21);
			label12.TabIndex = 0;
			label12.Text = "會員電話";
			panel1.Controls.Add(ucSellInfo_Return3);
			panel1.Controls.Add(ucSellInfo_Return2);
			panel1.Controls.Add(ucSellInfo_Return1);
			panel1.Location = new System.Drawing.Point(90, 347);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(801, 312);
			panel1.TabIndex = 45;
			ucSellInfo_Return3.BackColor = System.Drawing.Color.White;
			ucSellInfo_Return3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			ucSellInfo_Return3.Cursor = System.Windows.Forms.Cursors.Hand;
			ucSellInfo_Return3.Location = new System.Drawing.Point(0, 204);
			ucSellInfo_Return3.Margin = new System.Windows.Forms.Padding(0);
			ucSellInfo_Return3.MaximumSize = new System.Drawing.Size(801, 102);
			ucSellInfo_Return3.MinimumSize = new System.Drawing.Size(398, 102);
			ucSellInfo_Return3.Name = "ucSellInfo_Return3";
			ucSellInfo_Return3.Size = new System.Drawing.Size(801, 102);
			ucSellInfo_Return3.TabIndex = 3;
			ucSellInfo_Return2.BackColor = System.Drawing.Color.White;
			ucSellInfo_Return2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			ucSellInfo_Return2.Cursor = System.Windows.Forms.Cursors.Hand;
			ucSellInfo_Return2.Location = new System.Drawing.Point(0, 102);
			ucSellInfo_Return2.Margin = new System.Windows.Forms.Padding(0);
			ucSellInfo_Return2.MaximumSize = new System.Drawing.Size(801, 102);
			ucSellInfo_Return2.MinimumSize = new System.Drawing.Size(398, 102);
			ucSellInfo_Return2.Name = "ucSellInfo_Return2";
			ucSellInfo_Return2.Size = new System.Drawing.Size(801, 102);
			ucSellInfo_Return2.TabIndex = 2;
			ucSellInfo_Return1.BackColor = System.Drawing.Color.White;
			ucSellInfo_Return1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			ucSellInfo_Return1.Cursor = System.Windows.Forms.Cursors.Hand;
			ucSellInfo_Return1.Location = new System.Drawing.Point(0, 0);
			ucSellInfo_Return1.Margin = new System.Windows.Forms.Padding(0);
			ucSellInfo_Return1.MaximumSize = new System.Drawing.Size(801, 102);
			ucSellInfo_Return1.MinimumSize = new System.Drawing.Size(398, 102);
			ucSellInfo_Return1.Name = "ucSellInfo_Return1";
			ucSellInfo_Return1.Size = new System.Drawing.Size(801, 102);
			ucSellInfo_Return1.TabIndex = 1;
			btn_view.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
			btn_view.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			btn_view.Font = new System.Drawing.Font("微軟正黑體", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
			btn_view.ForeColor = System.Drawing.Color.White;
			btn_view.Location = new System.Drawing.Point(782, 312);
			btn_view.Name = "btn_view";
			btn_view.Size = new System.Drawing.Size(109, 29);
			btn_view.TabIndex = 52;
			btn_view.TabStop = false;
			btn_view.Text = "檢視近期銷售單";
			btn_view.UseVisualStyleBackColor = false;
			btn_view.Click += new System.EventHandler(btn_view_Click);
			tableLayoutPanel2.BackColor = System.Drawing.Color.White;
			tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
			tableLayoutPanel2.ColumnCount = 2;
			tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.63662f));
			tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 78.36338f));
			tableLayoutPanel2.Controls.Add(panel7, 0, 3);
			tableLayoutPanel2.Controls.Add(panel8, 0, 2);
			tableLayoutPanel2.Controls.Add(panel9, 0, 0);
			tableLayoutPanel2.Controls.Add(panel10, 0, 1);
			tableLayoutPanel2.Controls.Add(panel11, 1, 3);
			tableLayoutPanel2.Location = new System.Drawing.Point(141, 43);
			tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
			tableLayoutPanel2.Name = "tableLayoutPanel2";
			tableLayoutPanel2.RowCount = 4;
			tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25f));
			tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25f));
			tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25f));
			tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25f));
			tableLayoutPanel2.Size = new System.Drawing.Size(699, 240);
			tableLayoutPanel2.TabIndex = 40;
			panel7.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
			panel7.Controls.Add(label4);
			panel7.Dock = System.Windows.Forms.DockStyle.Fill;
			panel7.Location = new System.Drawing.Point(1, 178);
			panel7.Margin = new System.Windows.Forms.Padding(0);
			panel7.Name = "panel7";
			panel7.Size = new System.Drawing.Size(150, 61);
			panel7.TabIndex = 25;
			label4.AutoSize = true;
			label4.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
			label4.ForeColor = System.Drawing.Color.White;
			label4.Location = new System.Drawing.Point(33, 16);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(106, 21);
			label4.TabIndex = 0;
			label4.Text = "銷售日期區間";
			panel8.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
			panel8.Controls.Add(label5);
			panel8.Dock = System.Windows.Forms.DockStyle.Fill;
			panel8.Location = new System.Drawing.Point(1, 119);
			panel8.Margin = new System.Windows.Forms.Padding(0);
			panel8.Name = "panel8";
			panel8.Size = new System.Drawing.Size(150, 58);
			panel8.TabIndex = 20;
			label5.AutoSize = true;
			label5.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
			label5.ForeColor = System.Drawing.Color.White;
			label5.Location = new System.Drawing.Point(97, 23);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(42, 21);
			label5.TabIndex = 0;
			label5.Text = "電話";
			panel9.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
			panel9.Controls.Add(label7);
			panel9.Dock = System.Windows.Forms.DockStyle.Fill;
			panel9.Location = new System.Drawing.Point(1, 1);
			panel9.Margin = new System.Windows.Forms.Padding(0);
			panel9.Name = "panel9";
			panel9.Size = new System.Drawing.Size(150, 58);
			panel9.TabIndex = 21;
			label7.AutoSize = true;
			label7.BackColor = System.Drawing.Color.Transparent;
			label7.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
			label7.ForeColor = System.Drawing.Color.White;
			label7.Location = new System.Drawing.Point(65, 15);
			label7.Name = "label7";
			label7.Size = new System.Drawing.Size(74, 21);
			label7.TabIndex = 0;
			label7.Text = "銷售編號";
			panel10.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
			panel10.Controls.Add(label8);
			panel10.Dock = System.Windows.Forms.DockStyle.Fill;
			panel10.Location = new System.Drawing.Point(1, 60);
			panel10.Margin = new System.Windows.Forms.Padding(0);
			panel10.Name = "panel10";
			panel10.Size = new System.Drawing.Size(150, 58);
			panel10.TabIndex = 23;
			label8.AutoSize = true;
			label8.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
			label8.ForeColor = System.Drawing.Color.White;
			label8.Location = new System.Drawing.Point(65, 15);
			label8.Name = "label8";
			label8.Size = new System.Drawing.Size(74, 21);
			label8.TabIndex = 0;
			label8.Text = "會員姓名";
			panel11.Controls.Add(dateTimePicker1);
			panel11.Controls.Add(dateTimePicker2);
			panel11.Controls.Add(label9);
			panel11.Location = new System.Drawing.Point(155, 181);
			panel11.Name = "panel11";
			panel11.Size = new System.Drawing.Size(539, 47);
			panel11.TabIndex = 24;
			dateTimePicker1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			dateTimePicker1.CustomFormat = "yyyy-MM-dd";
			dateTimePicker1.Font = new System.Drawing.Font("微軟正黑體", 15f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
			dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			dateTimePicker1.Location = new System.Drawing.Point(236, 6);
			dateTimePicker1.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
			dateTimePicker1.Name = "dateTimePicker1";
			dateTimePicker1.ShowCheckBox = true;
			dateTimePicker1.Size = new System.Drawing.Size(186, 34);
			dateTimePicker1.TabIndex = 33;
			dateTimePicker2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			dateTimePicker2.CustomFormat = "yyyy-MM-dd";
			dateTimePicker2.Font = new System.Drawing.Font("微軟正黑體", 15f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
			dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			dateTimePicker2.Location = new System.Drawing.Point(14, 7);
			dateTimePicker2.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
			dateTimePicker2.Name = "dateTimePicker2";
			dateTimePicker2.ShowCheckBox = true;
			dateTimePicker2.Size = new System.Drawing.Size(181, 34);
			dateTimePicker2.TabIndex = 32;
			label9.AutoSize = true;
			label9.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
			label9.Location = new System.Drawing.Point(202, 14);
			label9.Name = "label9";
			label9.Size = new System.Drawing.Size(21, 20);
			label9.TabIndex = 1;
			label9.Text = "~";
			label11.Font = new System.Drawing.Font("微軟正黑體", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 136);
			label11.Image = POS_Client.Properties.Resources.oblique;
			label11.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			label11.Location = new System.Drawing.Point(88, 314);
			label11.Name = "label11";
			label11.Size = new System.Drawing.Size(127, 23);
			label11.TabIndex = 46;
			label11.Text = "最近銷售單";
			label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			panel12.Location = new System.Drawing.Point(90, 347);
			panel12.Name = "panel12";
			panel12.Size = new System.Drawing.Size(801, 312);
			panel12.TabIndex = 45;
			button1.BackColor = System.Drawing.Color.FromArgb(102, 102, 102);
			button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			button1.Font = new System.Drawing.Font("微軟正黑體", 12f, System.Drawing.FontStyle.Bold);
			button1.ForeColor = System.Drawing.Color.White;
			button1.Location = new System.Drawing.Point(747, 308);
			button1.Name = "button1";
			button1.Size = new System.Drawing.Size(144, 35);
			button1.TabIndex = 52;
			button1.TabStop = false;
			button1.Text = "檢視近期銷售單";
			button1.UseVisualStyleBackColor = false;
			button1.Click += new System.EventHandler(btn_view_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.SystemColors.Control;
			base.ClientSize = new System.Drawing.Size(981, 661);
			base.Controls.Add(button1);
			base.Controls.Add(btn_view);
			base.Controls.Add(panel1);
			base.Controls.Add(label11);
			base.Controls.Add(btn_cancel);
			base.Controls.Add(btn_reset);
			base.Controls.Add(btn_enter);
			base.Controls.Add(tableLayoutPanel1);
			base.Controls.Add(tableLayoutPanel2);
			base.Controls.Add(panel12);
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Name = "frmSearchSell_Return";
			base.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			Text = "會員選擇";
			base.Controls.SetChildIndex(panel12, 0);
			base.Controls.SetChildIndex(tableLayoutPanel2, 0);
			base.Controls.SetChildIndex(tableLayoutPanel1, 0);
			base.Controls.SetChildIndex(btn_enter, 0);
			base.Controls.SetChildIndex(btn_reset, 0);
			base.Controls.SetChildIndex(btn_cancel, 0);
			base.Controls.SetChildIndex(label11, 0);
			base.Controls.SetChildIndex(panel1, 0);
			base.Controls.SetChildIndex(btn_view, 0);
			base.Controls.SetChildIndex(button1, 0);
			base.Controls.SetChildIndex(pb_virtualKeyBoard, 0);
			((System.ComponentModel.ISupportInitialize)pb_virtualKeyBoard).EndInit();
			panel5.ResumeLayout(false);
			panel5.PerformLayout();
			panel3.ResumeLayout(false);
			panel3.PerformLayout();
			tableLayoutPanel1.ResumeLayout(false);
			tableLayoutPanel1.PerformLayout();
			panel14.ResumeLayout(false);
			panel14.PerformLayout();
			panel4.ResumeLayout(false);
			panel4.PerformLayout();
			panel2.ResumeLayout(false);
			panel2.PerformLayout();
			panel13.ResumeLayout(false);
			panel13.PerformLayout();
			panel6.ResumeLayout(false);
			panel6.PerformLayout();
			panel1.ResumeLayout(false);
			tableLayoutPanel2.ResumeLayout(false);
			panel7.ResumeLayout(false);
			panel7.PerformLayout();
			panel8.ResumeLayout(false);
			panel8.PerformLayout();
			panel9.ResumeLayout(false);
			panel9.PerformLayout();
			panel10.ResumeLayout(false);
			panel10.PerformLayout();
			panel11.ResumeLayout(false);
			panel11.PerformLayout();
			ResumeLayout(false);
		}
	}
}
