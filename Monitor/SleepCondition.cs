using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MetroFramework.Controls;
using MetroFramework.Forms;

namespace Monitor;

public class SleepCondition : MetroForm
{
	private Tools tools = new Tools();

	private IContainer components;

	private Chart chart1;

	private MetroButton metroButton1;

	private MetroButton metroButton2;

    private Label emancipationCnt;

    private Label emancipationTxt;

    private Label label1;

	public SleepCondition()
	{
		InitializeComponent();
	}

	private void SleepCondition_Load(object sender, EventArgs e)
	{
		InitChart();
	}

	private void InitChart()
	{
		chart1.ChartAreas.Clear();
		ChartArea chartArea1 = new ChartArea("C1");
		chart1.ChartAreas.Add(chartArea1);
		chart1.Series.Clear();
		Series series2 = new Series("S1");
		series2.ChartArea = "C1";
		chart1.Series.Add(series2);
		chart1.ChartAreas[0].AxisY.Minimum = 0.0;
		chart1.ChartAreas[0].AxisY.Maximum = 100.0;
		chart1.ChartAreas[0].AxisX.Interval = 5.0;
		chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Silver;
		chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Silver;
		chart1.Titles.Clear();
		chart1.Titles.Add("S01");
		chart1.Titles[0].Text = tools.GetNowUserName() + "睡姿分布情况统计图";
		chart1.Titles[0].ForeColor = Color.RoyalBlue;
		chart1.Titles[0].Font = new Font("Microsoft Sans Serif", 12f);
		chart1.Series[0].Color = Color.Red;
		chart1.Series[0].ChartType = SeriesChartType.Line;
		chart1.Series[0].Points.Clear();
		DataSet data = DisplayData(tools.GetLoginUserId());
		emancipationCnt.Text = CaclEmancipation(data).ToString();
		chart1.Series.Clear();
		chart1.ChartAreas[0].AxisY.Minimum = 0.0;
		chart1.ChartAreas[0].AxisY.Maximum = 7.0;
		chart1.ChartAreas[0].AxisX.Interval = 15.0;
		chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Silver;
		chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Silver;
		CustomLabel labe9 = new()
        {
			FromPosition = -1.0,
			ToPosition = 1.0,
			Text = "不在床"
		};
		CustomLabel label0 = new CustomLabel
		{
			FromPosition = 0.0,
			ToPosition = 2.0,
			Text = "坐立"
		};
		CustomLabel label1 = new CustomLabel
		{
			FromPosition = 1.0,
			ToPosition = 3.0,
			Text = "右侧树干型"
		};
		CustomLabel label2 = new CustomLabel
		{
			FromPosition = 2.0,
			ToPosition = 4.0,
			Text = "右侧胎儿型"
		};
		CustomLabel label3 = new CustomLabel
		{
			FromPosition = 3.0,
			ToPosition = 5.0,
			Text = "仰卧"
		};
		CustomLabel label4 = new CustomLabel
		{
			FromPosition = 4.0,
			ToPosition = 6.0,
			Text = "左侧树干型"
		};
		CustomLabel label5 = new CustomLabel
		{
			FromPosition = 5.0,
			ToPosition = 7.0,
			Text = "左侧胎儿型"
		};
		CustomLabel label6 = new CustomLabel
		{
			FromPosition = 6.0,
			ToPosition = 8.0,
			Text = "俯卧"
		};
		chart1.ChartAreas[0].AxisY.CustomLabels.Add(labe9);
		chart1.ChartAreas[0].AxisY.CustomLabels.Add(label0);
		chart1.ChartAreas[0].AxisY.CustomLabels.Add(label1);
		chart1.ChartAreas[0].AxisY.CustomLabels.Add(label2);
		chart1.ChartAreas[0].AxisY.CustomLabels.Add(label3);
		chart1.ChartAreas[0].AxisY.CustomLabels.Add(label4);
		chart1.ChartAreas[0].AxisY.CustomLabels.Add(label5);
		chart1.ChartAreas[0].AxisY.CustomLabels.Add(label6);
		chart1.ChartAreas[0].Area3DStyle.Enable3D = false;
		chart1.ChartAreas[0].Area3DStyle.LightStyle = LightStyle.Realistic;
		chart1.ChartAreas[0].BackColor = Color.FromArgb(64, 165, 191, 228);
		chart1.ChartAreas[0].CursorX.AutoScroll = true;
		chart1.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
		chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
		chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
		chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
		chart1.ChartAreas[0].AxisX.ScaleView.Position = 1.0;
		chart1.ChartAreas[0].AxisX.ScaleView.Size = 10.0;
		chart1.ChartAreas[0].AxisX.ScrollBar.ButtonColor = Color.Silver;
		chart1.ChartAreas[0].AxisX.ScrollBar.LineColor = Color.Black;
		chart1.DataSource = data;
		Series series = chart1.Series.Add("当前状态");
		series.XValueMember = "time";
		series.YValueMembers = "posture";
		chart1.Series[0].ChartType = SeriesChartType.FastLine;
		chart1.Series[0].Color = Color.Blue;
		chart1.ChartAreas[0].AxisX.Interval = 1.0;
	}

	public DataSet DisplayData(int userId)
	{
		// 根据上一次查询
		// string sql = "SELECT result_data.id, result_data.time,  result_data.posture FROM result_data WHERE  result_data.time >=  '" + tools.GetLastLoginTime(userId) + "'AND result_data.user_id ='" + tools.GetLoginUserId() + "' ORDER BY result_data.id DESC";
		string sql = "SELECT result_data.id, result_data.time,  result_data.posture FROM result_data WHERE  result_data.time >=  '" + tools.SleepTimeTotal() + "'AND result_data.user_id ='" + tools.GetLoginUserId() + "' ORDER BY result_data.id DESC";
		DataSet ds = SQLiteHelper.ExecuteDataset(sql);
		foreach (DataRow row in ds.Tables[0].Rows)
		{
			row["time"] = DateTime.Parse(row["time"].ToString()).ToString("HH:mm:ss");
		}
		return ds;
	}

	private void metroButton1_Click(object sender, EventArgs e)
	{
		InitChart();
	}

	private void metroButton2_Click(object sender, EventArgs e)
	{
		MessageBox.Show("当前用户数据将被删除!");

		// 删除该用户结果表（睡姿及压力变化个数）中该用户的数据
		string sql_result = "DELETE FROM `result_data` WHERE user_id = "+ tools.GetLoginUserId();
		SQLiteHelper.ExecuteDataset(sql_result);
		// 删除该用户登录时间
		string sql_time = "DELETE FROM `time` WHERE user_id = " + tools.GetLoginUserId();
		SQLiteHelper.ExecuteDataset(sql_time);
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.metroButton2 = new MetroFramework.Controls.MetroButton();
            this.label1 = new System.Windows.Forms.Label();
            this.emancipationCnt = new System.Windows.Forms.Label();
            this.emancipationTxt = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea1.AxisX.ScaleView.Zoomable = false;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(31, 104);
            this.chart1.Margin = new System.Windows.Forms.Padding(4);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(1145, 606);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // metroButton1
            // 
            this.metroButton1.Location = new System.Drawing.Point(1011, 32);
            this.metroButton1.Margin = new System.Windows.Forms.Padding(4);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(100, 39);
            this.metroButton1.TabIndex = 1;
            this.metroButton1.Text = "刷新";
            this.metroButton1.UseSelectable = true;
            this.metroButton1.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // metroButton2
            // 
            this.metroButton2.Location = new System.Drawing.Point(423, 32);
            this.metroButton2.Margin = new System.Windows.Forms.Padding(4);
            this.metroButton2.Name = "metroButton2";
            this.metroButton2.Size = new System.Drawing.Size(359, 39);
            this.metroButton2.TabIndex = 2;
            this.metroButton2.Text = "清除当前用户储存数据（慎点）";
            this.metroButton2.UseSelectable = true;
            this.metroButton2.Click += new System.EventHandler(this.metroButton2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(649, 714);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(497, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "*仅显示符合条件的最近1000条数据，更多数据请通过数据库管理工具查看";
            // 
            // emancipationCnt
            // 
            this.emancipationCnt.AutoSize = true;
            this.emancipationCnt.Location = new System.Drawing.Point(1096, 136);
            this.emancipationCnt.Name = "emancipationCnt";
            this.emancipationCnt.Size = new System.Drawing.Size(15, 15);
            this.emancipationCnt.TabIndex = 4;
            this.emancipationCnt.Text = "0";
            // 
            // emancipationTxt
            // 
            this.emancipationTxt.AutoSize = true;
            this.emancipationTxt.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.emancipationTxt.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.emancipationTxt.Location = new System.Drawing.Point(1007, 131);
            this.emancipationTxt.Name = "emancipationTxt";
            this.emancipationTxt.Size = new System.Drawing.Size(88, 24);
            this.emancipationTxt.TabIndex = 13;
            this.emancipationTxt.Text = "翻身次数: ";
            // 
            // SleepCondition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1219, 739);
            this.Controls.Add(this.emancipationCnt);
            this.Controls.Add(this.emancipationTxt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.metroButton2);
            this.Controls.Add(this.metroButton1);
            this.Controls.Add(this.chart1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximumSize = new System.Drawing.Size(1219, 739);
            this.MinimumSize = new System.Drawing.Size(1219, 739);
            this.Name = "SleepCondition";
            this.Padding = new System.Windows.Forms.Padding(27, 75, 27, 25);
            this.Text = "睡姿分布情况";
            this.Load += new System.EventHandler(this.SleepCondition_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

	}

	/// <summary>
	/// 过滤掉频繁跳变的睡姿
	/// </summary>
	/// <param name="data">数据库查出的原始数据</param>
	/// <returns>处理后的数据</returns>
	private int CaclEmancipation(DataSet data) {
		// 翻身次数
		int emancipationCnt = 0;
		List<int> dataList = new List<int>();
		foreach (DataRow row in data.Tables[0].Rows)
		{

			int cur = int.Parse(row["posture"].ToString());
			// 0 代表不在床
			if (cur == 0) {
				continue;
			}
			dataList.Add(cur);
		}

		int[] nums = dataList.ToArray();

		for (int i = 0; i < nums.Length - 4; i++) {
			// 此时两个姿势相同 跳过
			if (nums[i] == nums[i + 1])
			{
				continue;
			}
			// 两个姿势不同时 看这个姿势保持是否保持12s以上
			else {
				if (nums[i] != nums[i+4] && nums[i+1] == nums[i+2] && nums[i+1] == nums[i+3] && nums[i+1] == nums[i+4]) {
					emancipationCnt++;
				}
			}
	
		}

		return emancipationCnt;
	}
}
