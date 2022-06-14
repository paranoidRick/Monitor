using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MetroFramework.Controls;
using MetroFramework.Forms;

namespace Monitor;

public class BodyMovement : MetroForm
{
	private Tools tools = new Tools();

	private Chart chart1;
    private TextBox textBox1;
    private MetroButton metroButton1;
    private Label label1;

	public BodyMovement()
	{
		InitializeComponent();
	}

	private void BodyMovement_Load(object sender, EventArgs e)
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
		chart1.Titles[0].Text = "压力变化统计图";
		chart1.Titles[0].ForeColor = Color.RoyalBlue;
		chart1.Titles[0].Font = new Font("Microsoft Sans Serif", 12f);
		chart1.Series[0].Color = Color.Red;
		chart1.Series[0].ChartType = SeriesChartType.Line;
		chart1.Series[0].Points.Clear();
		// 查出数据库数据
		DataSet data = DisplayData(tools.GetLoginUserId());
		chart1.Series.Clear();
		chart1.ChartAreas[0].AxisY.IsStartedFromZero = false;
		chart1.ChartAreas[0].AxisY.Maximum = 1000.0;
		chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Silver;
		chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Silver;

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
		chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
		chart1.ChartAreas[0].AxisY.ScaleView.Position = -1.0;
		chart1.ChartAreas[0].AxisY.ScaleView.Size = 100.0;
		chart1.ChartAreas[0].AxisX.ScrollBar.ButtonColor = Color.Silver;
		chart1.ChartAreas[0].AxisX.ScrollBar.LineColor = Color.Black;
		// chart显示数据
		chart1.DataSource = data;
		Series series = chart1.Series.Add("压力变化个数");
		series.XValueMember = "time";
		series.YValueMembers = "change_count";
		chart1.Series[0].ChartType = SeriesChartType.FastLine;
		chart1.Series[0].Color = Color.Blue;
		chart1.ChartAreas[0].AxisX.Interval = 1.0;
	}

	// 查出change_count
	public DataSet DisplayData(int userId)
	{
		string time = tools.GetLastLoginTime(userId);
		if (string.IsNullOrEmpty(time)) {
			return null;
		}
		// 查询此次登录 10000条数据 — 每4s插入一次数据 10h约9000条
		string sql = "SELECT result_data.id, result_data.time,  result_data.change_count FROM result_data WHERE  result_data.time >=  '" + time + "'AND result_data.user_id ='" + tools.GetLoginUserId() + "' ORDER BY result_data.id DESC LIMIT 10000";
		DataSet ds = SQLiteHelper.ExecuteDataset(sql);
		foreach (DataRow row in ds.Tables[0].Rows)
		{
			row["time"] = DateTime.Parse(row["time"].ToString()).ToString("HH:mm:ss");
		}
		return ds;
	}

	private void InitializeComponent()
	{
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
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
            series1.IsXValueIndexed = true;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(1435, 699);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            this.chart1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseClick);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(1217, 200);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(192, 55);
            this.textBox1.TabIndex = 1;
            // 
            // metroButton1
            // 
            this.metroButton1.Location = new System.Drawing.Point(1309, 135);
            this.metroButton1.Margin = new System.Windows.Forms.Padding(4);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(100, 39);
            this.metroButton1.TabIndex = 2;
            this.metroButton1.Text = "刷新";
            this.metroButton1.UseSelectable = true;
            this.metroButton1.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // BodyMovement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1479, 809);
            this.Controls.Add(this.metroButton1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chart1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "BodyMovement";
            this.Padding = new System.Windows.Forms.Padding(27, 75, 27, 25);
            this.Load += new System.EventHandler(this.BodyMovement_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

	}

	// 鼠标移动事件
	private void chart1_MouseClick(object sender, MouseEventArgs e)  //chart1是你建的chart控件，实际名字根据你自己代码里的命名
	{
		HitTestResult hit = chart1.HitTest(e.X, e.Y);
		if (hit.Series != null)
		{
			var xValue = hit.Series.Points[hit.PointIndex].XValue;
			var yValue = hit.Series.Points[hit.PointIndex].YValues.First();
			textBox1.Font = new Font("Microsoft Sans Serif", 12f);
			textBox1.Text = "当前值为:" + yValue.ToString();//textbox1也是自己建的一个专门用来显示的内容框，也可以用messagebox直接弹出内容
		}
		else
		{
			textBox1.Text = "未放置到波形曲线";
		}
	}

	private void metroButton1_Click(object sender, EventArgs e)
	{
		InitChart();
	}
}