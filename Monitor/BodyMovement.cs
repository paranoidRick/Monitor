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

    private MetroButton metroButton1;

    private Label label1;

	// 单例莫上
	private static BodyMovement instance;
    private ComboBox comboBox1;
    private Chart chart1;
    private Label startTime;
    private Label label3;
    private Label endTime;
    private Label label5;
    private Label bigResult;
    private Label label7;
    private Label smallResult;
    private static object _lock = new object();

	public BodyMovement()
	{
		InitializeComponent();
	}

	public static BodyMovement GetInstance()
	{
		if (instance == null || instance.IsDisposed)
		{
			lock (_lock)
			{
				if (instance == null || instance.IsDisposed)
				{
					instance = new BodyMovement();
				}
			}

		}
		return instance;
	}

	private void BodyMovement_Load(object sender, EventArgs e)
	{
		InitChart();
	}

	private void InitChart()
	{
		DataSet ds = tools.GetLoginTimes();
		foreach (DataRow row in ds.Tables[0].Rows) {
            comboBox1.Items.Add(row.ItemArray[0].ToString());
		}
		comboBox1.DisplayMember = "start_time";
	}

	private void InitializeComponent()
	{
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.label1 = new System.Windows.Forms.Label();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.startTime = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.endTime = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.bigResult = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.smallResult = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            // 
            // metroButton1
            // 
            this.metroButton1.Location = new System.Drawing.Point(0, 0);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(75, 23);
            this.metroButton1.TabIndex = 0;
            this.metroButton1.UseSelectable = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(1088, 139);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(215, 23);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.Text = "== 请选择 ==";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // chart1
            // 
            chartArea2.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea2);
            legend2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(31, 100);
            this.chart1.Margin = new System.Windows.Forms.Padding(4);
            this.chart1.Name = "chart1";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.IsValueShownAsLabel = true;
            series2.IsXValueIndexed = true;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(1435, 703);
            this.chart1.TabIndex = 0;
            this.chart1.TabStop = false;
            this.chart1.Text = "chart1";
            // 
            // startTime
            // 
            this.startTime.AutoSize = true;
            this.startTime.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.startTime.Location = new System.Drawing.Point(383, 57);
            this.startTime.Name = "startTime";
            this.startTime.Size = new System.Drawing.Size(226, 23);
            this.startTime.TabIndex = 2;
            this.startTime.Text = "2022/6/21 00:00:00";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(629, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "——";
            // 
            // endTime
            // 
            this.endTime.AutoSize = true;
            this.endTime.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.endTime.Location = new System.Drawing.Point(689, 57);
            this.endTime.Name = "endTime";
            this.endTime.Size = new System.Drawing.Size(226, 23);
            this.endTime.TabIndex = 4;
            this.endTime.Text = "2022/6/21 00:00:00";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(433, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(148, 23);
            this.label5.TabIndex = 5;
            this.label5.Text = "大体动次数：";
            // 
            // bigResult
            // 
            this.bigResult.AutoSize = true;
            this.bigResult.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bigResult.Location = new System.Drawing.Point(587, 100);
            this.bigResult.Name = "bigResult";
            this.bigResult.Size = new System.Drawing.Size(22, 23);
            this.bigResult.TabIndex = 6;
            this.bigResult.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(689, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(148, 23);
            this.label7.TabIndex = 7;
            this.label7.Text = "小体动次数：";
            // 
            // smallResult
            // 
            this.smallResult.AutoSize = true;
            this.smallResult.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.smallResult.Location = new System.Drawing.Point(843, 100);
            this.smallResult.Name = "smallResult";
            this.smallResult.Size = new System.Drawing.Size(22, 23);
            this.smallResult.TabIndex = 8;
            this.smallResult.Text = "0";
            // 
            // BodyMovement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1479, 809);
            this.Controls.Add(this.smallResult);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.bigResult);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.endTime);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.startTime);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.metroButton1);
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

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
		chart1.ChartAreas.Clear();
		ChartArea chartArea1 = new ChartArea("C1");
		chart1.ChartAreas.Add(chartArea1);
		chart1.Series.Clear();
		Series series2 = new Series("S1");
		series2.ChartArea = "C1";
		chart1.Series.Add(series2);
		chart1.ChartAreas[0].AxisY.Minimum = 0.0;
		chart1.ChartAreas[0].AxisX.Interval = 5.0;
		chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Silver;
		chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Silver;
		chart1.Series[0].Color = Color.Red;
		chart1.Series[0].ChartType = SeriesChartType.Spline;
		chart1.Series[0].Points.Clear();

		chart1.Series.Clear();
		chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Silver;
		chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Silver;

		chart1.ChartAreas[0].Area3DStyle.Enable3D = false;
		chart1.ChartAreas[0].Area3DStyle.LightStyle = LightStyle.Realistic;
		chart1.ChartAreas[0].BackColor = Color.FromArgb(64, 165, 191, 228);
		chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
		chart1.ChartAreas[0].AxisX.ScaleView.Position = 1.0;
		chart1.ChartAreas[0].AxisX.ScaleView.Size = 10.0;
		chart1.ChartAreas[0].AxisY.IsStartedFromZero = false;
		chart1.ChartAreas[0].AxisX.ScrollBar.ButtonColor = Color.Silver;
		chart1.ChartAreas[0].AxisX.ScrollBar.LineColor = Color.Black;

		// 下拉列表选中的值
		string selected = comboBox1.SelectedItem.ToString();
        string[] res = GetDurationTimeStamp(selected);
        // 查出数据库数据
        DataSet data = DisplayDatas(res);
		// chart显示数据
		chart1.DataSource = data;
		Series series = chart1.Series.Add("压力变化个数");
		series.XValueMember = "time";
		series.YValueMembers = "change_count";
		series.IsValueShownAsLabel = true;
		series.MarkerBorderColor = Color.Black;
		chart1.Series[0].ChartType = SeriesChartType.Spline;
		chart1.Series[0].Color = Color.Blue;
		chart1.ChartAreas[0].AxisX.Interval = 1.0;
        // 显示结果
        string[] moveTimes = GetDurationTimes(selected);
        startTime.Text = moveTimes[0];
        endTime.Text = moveTimes[1];

        int[] moveRes = GetMoveResult(selected);
        bigResult.Text = moveRes[0].ToString();
        smallResult.Text = moveRes[1].ToString();
        
    }

	/// <summary>
	/// 按时间段查出change_count
	/// </summary>
	/// <param name="userId"></param>
	/// <returns></returns>
	public DataSet DisplayDatas(string[] res)
	{
		string sql = "SELECT time, change_count FROM change WHERE time_stamp >= '" + res[0] + "' AND time_stamp <= '" + res[1] + "'AND user_id = '" + tools.GetLoginUserId() + "' ORDER BY id";
		DataSet ds = SQLiteHelper.ExecuteDataset(sql);
		foreach (DataRow row in ds.Tables[0].Rows)
		{
			row["time"] = DateTime.Parse(row["time"].ToString()).ToString("HH:mm:ss");
		}
		return ds;
	}

	/// <summary>
	/// 连接开始和断开连接的timestamp
	/// </summary>
	/// <returns></returns>
	private string[] GetDurationTimeStamp(string time) { 
		string[] res = new string[2];
		string sql = "SELECT start_stamp, end_stamp FROM move_result WHERE start_time = '" + time + "' AND user_id ='" + tools.GetLoginUserId() + "' ORDER BY id;";
		DataSet ds = SQLiteHelper.ExecuteDataset(sql);

		if (ds == null) {
			return res;
		}

		foreach (DataRow dr in ds.Tables[0].Rows)
		{
			res[0] = dr.ItemArray[0].ToString();
            res[1] = dr.ItemArray[1].ToString();
        }
		return res;
	}

    /// <summary>
    /// 查出大小体动的次数
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private string[] GetDurationTimes(string time)
    {
        string[] res = new string[2];
        string sql = "SELECT start_time, end_time FROM move_result WHERE start_time = '" + time + "' AND user_id ='" + tools.GetLoginUserId() + "' ORDER BY id;";
        DataSet ds = SQLiteHelper.ExecuteDataset(sql);

        if (ds == null)
        {
            return res;
        }
        
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            res[0] = dr.ItemArray[0].ToString();
            res[1] = dr.ItemArray[1].ToString();
        }
        return res;
    }

    /// <summary>
    /// 查出大小体动的次数
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private int[] GetMoveResult(string time) {
        int[] res = new int[2];
        string sql = "SELECT move_large, move_small FROM move_result WHERE start_time = '" + time + "' AND user_id ='" + tools.GetLoginUserId() + "' ORDER BY id;";
        DataSet ds = SQLiteHelper.ExecuteDataset(sql);

        if (ds == null)
        {
            return res;
        }

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            res[0] = int.Parse(dr.ItemArray[0].ToString());
            res[1] = int.Parse(dr.ItemArray[1].ToString());
        }
        return res;
    }

}