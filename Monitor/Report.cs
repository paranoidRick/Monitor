/**
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MetroFramework.Controls;
using MetroFramework.Forms;
using NPOI.XWPF.UserModel;

namespace Monitor;

public class Report : MetroForm
{
	private Tools tools = new Tools();

	private IContainer components;

	private Label label1;

	private Label label2;

	private Label label3;

	private Label label4;

	private Label label5;

	private Label label6;

	private Label label7;

	private Label userNameLabel;

	private Label sexLabel;

	private Label ageLabel;

	private Label heightLabel;

	private Label weightLabel;

	private MetroButton printButton;

	private MetroButton regenerateButton;

	private Label label8;

	private Label label10;

	private Label TurnLarge;

	private Label TurnSmall;

	private Label TurnRate;

	private Label TurnTotalTime;

	private Label sleepDuration;

	private Label label15;

	private Label posture_0_label;

	private Label posture_1_label;

	private Label posture_2_label;

	private Label posture_3_label;

	private Label posture_4_label;

	private Label label22;

	private Label label24;

	private Label label26;

	private Label label28;

	private Label label30;

	private Label label32;

	private Label label34;

	private Label label36;

	private Label label38;

	private Label label40;

	private Label label9;

	private Label label21;

	private PictureBox pictureBox1;

	private Label label11;

	private PictureBox pictureBox2;

	private PictureBox pictureBox3;

	private PictureBox pictureBox4;

	private PictureBox pictureBox5;

	private Label label12;

	private Label label13;

	private Label label14;

	private Label label16;

	private Chart chart1;

	private Label label17;

	private Label posture_count_1_label;

	private Label posture_count_2_label;

	private Label posture_count_3_label;

	private Label posture_count_4_label;

	private Label posture_count_5_label;

	private Label DepartureCount;

	private Label label19;

	private Label label18;

	private Label label20;

	private Label posture_5_label;

	private Label posture_6_label;

	private Label posture_count_6_label;

	private Label posture_count_7_label;

	private PictureBox pictureBox6;

	private PictureBox pictureBox7;

	private Label label23;

	private Label label25;

	public Report()
	{
		InitializeComponent();
	}

	private void Report_Load(object sender, EventArgs e)
	{
		string userId = tools.GetLoginUserId().ToString();
		string userName = tools.GetNowUserName().ToString();
		userNameLabel.Text = userName;
		Dictionary<string, string> userProfile = tools.GetUserProfile(userName);
		sexLabel.Text = userProfile["sex"];
		ageLabel.Text = userProfile["age"] + " 岁";
		heightLabel.Text = userProfile["height"] + " cm";
		weightLabel.Text = userProfile["weight"] + " kg";
		UpdateQueueValue();
		PostureState postureState = tools.GetUserPosturePhoto(userId);
		try
		{
			pictureBox1.Load(postureState.posture_1);
		}
		catch (Exception)
		{
		}
		try
		{
			pictureBox2.Load(postureState.posture_2);
		}
		catch (Exception)
		{
		}
		try
		{
			pictureBox3.Load(postureState.posture_3);
		}
		catch (Exception)
		{
		}
		try
		{
			pictureBox4.Load(postureState.posture_4);
		}
		catch (Exception)
		{
		}
		try
		{
			pictureBox5.Load(postureState.posture_5);
		}
		catch (Exception)
		{
		}
		try
		{
			pictureBox6.Load(postureState.posture_6);
		}
		catch (Exception)
		{
		}
		try
		{
			pictureBox7.Load(postureState.posture_7);
		}
		catch (Exception)
		{
		}
		InitChart();
		try
		{
			PostureStateCount postureStateCount = tools.GetUserPostureCount(userId);
			if (postureStateCount != null && postureStateCount.create_time != null)
			{
				int postureStateCountCreate = int.Parse(tools.DateTimeToUnixTime(Convert.ToDateTime(postureStateCount.create_time.ToString())).ToString());
				int nowTime = int.Parse(tools.DateTimeToUnixTime(DateTime.Now).ToString());
				int sleepTimeTotal = int.Parse(tools.GetConfigured("SleepTimeTotal"));
				if (nowTime - postureStateCountCreate < sleepTimeTotal)
				{
					posture_count_1_label.Text = tools.formatIntToTimeStrText((int)tools.GetColumnCount(postureStateCount.posture_1_count), displayMinutes: true);
					posture_count_2_label.Text = tools.formatIntToTimeStrText((int)tools.GetColumnCount(postureStateCount.posture_2_count), displayMinutes: true);
					posture_count_3_label.Text = tools.formatIntToTimeStrText((int)tools.GetColumnCount(postureStateCount.posture_3_count), displayMinutes: true);
					posture_count_4_label.Text = tools.formatIntToTimeStrText((int)tools.GetColumnCount(postureStateCount.posture_4_count), displayMinutes: true);
					posture_count_5_label.Text = tools.formatIntToTimeStrText((int)tools.GetColumnCount(postureStateCount.posture_5_count), displayMinutes: true);
					posture_count_6_label.Text = tools.formatIntToTimeStrText((int)tools.GetColumnCount(postureStateCount.posture_6_count), displayMinutes: true);
					posture_count_7_label.Text = tools.formatIntToTimeStrText((int)tools.GetColumnCount(postureStateCount.posture_7_count), displayMinutes: true);
				}
				else
				{
					postureStateCount.posture_1_count = 0;
					postureStateCount.posture_2_count = 0;
					postureStateCount.posture_3_count = 0;
					postureStateCount.posture_4_count = 0;
					postureStateCount.posture_5_count = 0;
					postureStateCount.posture_6_count = 0;
					postureStateCount.posture_7_count = 0;
					tools.SetUserPostureCount(userId, postureStateCount, changeTime: true);
				}
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void regenerateButton_Click(object sender, EventArgs e)
	{
		MessageBox.Show("该用户的报告数据已被清空，请重启本页面。");
		string userId = tools.GetLoginUserId().ToString();
		PostureState postureStateNew = new PostureState();
		postureStateNew.posture_1 = "";
		postureStateNew.posture_2 = "";
		postureStateNew.posture_3 = "";
		postureStateNew.posture_4 = "";
		postureStateNew.posture_5 = "";
		postureStateNew.posture_6 = "";
		postureStateNew.posture_7 = "";
		tools.SetUserPosturePhoto(userId, postureStateNew, changeTime: true);
		PostureStateCount postureStateCount = tools.GetUserPostureCount(userId);
		postureStateCount.posture_1_count = 0;
		postureStateCount.posture_2_count = 0;
		postureStateCount.posture_3_count = 0;
		postureStateCount.posture_4_count = 0;
		postureStateCount.posture_5_count = 0;
		postureStateCount.posture_6_count = 0;
		postureStateCount.posture_7_count = 0;
		tools.SetUserPostureCount(userId, postureStateCount, changeTime: true);
		Close();
	}

	private void printButton_Click(object sender, EventArgs e)
	{
		string userId = tools.GetLoginUserId().ToString();
		string userFileName = "PictureTmp//user_chart1_" + userId + ".png";
		try
		{
			chart1.SaveImage(userFileName, ChartImageFormat.Png);
		}
		catch (Exception)
		{
			MessageBox.Show("保存失败!");
		}
		saveWord(userId, userFileName);
	}

	private void UpdateQueueValue()
	{
		string userId = tools.GetLoginUserId().ToString();
		int onBedTime = (int)tools.DisplayLastTotalHoursOnBedData(userId, tools.DateTimeToUnixTime(DateTime.Now));

		string kk = tools.formatIntToTimeStrText(onBedTime, displayMinutes: true);
		sleepDuration.Text = kk.ToString();
		int sleepTimeTotal = int.Parse(tools.GetConfigured("SleepTimeTotal"));
		if (onBedTime > sleepTimeTotal)
		{
			posture_2_label.Text = "饱和睡眠";
		}
		posture_0_label.Text = tools.formatIntToTimeStrText((int)tools.GetSpecifyPostureColumnCount(userId, 1), displayMinutes: true);
		posture_1_label.Text = tools.formatIntToTimeStrText((int)tools.GetSpecifyPostureColumnCount(userId, 2), displayMinutes: true);
		posture_2_label.Text = tools.formatIntToTimeStrText((int)tools.GetSpecifyPostureColumnCount(userId, 3), displayMinutes: true);
		posture_3_label.Text = tools.formatIntToTimeStrText((int)tools.GetSpecifyPostureColumnCount(userId, 4), displayMinutes: true);
		posture_4_label.Text = tools.formatIntToTimeStrText((int)tools.GetSpecifyPostureColumnCount(userId, 5), displayMinutes: true);
		posture_5_label.Text = tools.formatIntToTimeStrText((int)tools.GetSpecifyPostureColumnCount(userId, 6), displayMinutes: true);
		posture_6_label.Text = tools.formatIntToTimeStrText((int)tools.GetSpecifyPostureColumnCount(userId, 7), displayMinutes: true);
		int turnLargeCount = 0;
		int turnSmallCount = 0;
		try
		{
			turnLargeCount = tools.GetLargeMove();
			turnSmallCount = tools.GetSmallMove();

			TurnLarge.Text = turnLargeCount.ToString();
			TurnSmall.Text = turnSmallCount.ToString();
			decimal turnRate = Math.Round((decimal)turnLargeCount / (decimal)onBedTime, 5);
			TurnRate.Text = turnRate.ToString();
			// if (turnRate != 0m)
			// {
			// 	TurnCycle.Text = Math.Round(1m / turnRate, 4).ToString();
			// }

		}
		catch
		{
			TurnRate.Text = "暂无数据";
			TurnSmall.Text = "暂无数据";
		}
		if (turnLargeCount > 0)
		{
			decimal turnTotalTime = (decimal)turnLargeCount * decimal.Parse(tools.GetConfigured("DeviceUploadInterval"));
			TurnTotalTime.Text = tools.formatIntToTimeStrText(int.Parse(turnTotalTime.ToString()), displayMinutes: true);
		}
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
		chart1.Series[0].Color = Color.Red;
		chart1.Series[0].ChartType = SeriesChartType.Line;
		chart1.Series[0].Points.Clear();
		DataSet data = DisplayOldData();
		chart1.Series.Clear();
		chart1.ChartAreas[0].AxisY.Minimum = 0.0;
		chart1.ChartAreas[0].AxisY.Maximum = 7.0;
		chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Silver;
		chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Silver;
		CustomLabel labe9 = new CustomLabel
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
		chart1.ChartAreas[0].AxisX.ScaleView.ZoomReset();
		chart1.ChartAreas[0].AxisY.ScaleView.ZoomReset();

		int dataRow = data.Tables[0].Rows.Count;
		chart1.ChartAreas[0].AxisX.Interval = dataRow / 25;
		chart1.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
	}

	public DataSet DisplayOldData()
	{
		string sql = "SELECT result_data.id, result_data.time,  result_data.posture FROM result_data WHERE  result_data.time >=  '" + tools.SleepTimeTotal() + "'AND result_data.user_id ='" + tools.GetLoginUserId() + "' ORDER BY result_data.id DESC LIMIT 1000";
		DataSet ds = SQLiteHelper.ExecuteDataset(sql);
		foreach (DataRow row in ds.Tables[0].Rows)
		{
			row["time"] = DateTime.Parse(row["time"].ToString()).ToString("dd日HH:mm:ss");
		}
		return ds;
	}

	public bool saveWord(string userId, string userFileName)
	{
		PostureState postureState = tools.GetUserPosturePhoto(userId);
		string filepath = "predict\\tmp\\test1.docx";
		var tt = new
		{
			name = userNameLabel.Text,
			age = ageLabel.Text,
			sex = sexLabel.Text,
			height = heightLabel.Text,
			weight = weightLabel.Text,
			TurnTotal = TurnLarge.Text,
			TurnCycle = TurnSmall.Text,
			TurnRate = TurnRate.Text,
			TurnTime = TurnTotalTime.Text,
			SDuration = sleepDuration.Text,
			DeCount = DepartureCount.Text,
			posture0 = posture_0_label.Text,
			posture1 = posture_1_label.Text,
			posture2 = posture_2_label.Text,
			posture3 = posture_3_label.Text,
			posture4 = posture_4_label.Text,
			posture5 = posture_5_label.Text,
			posture6 = posture_6_label.Text,
			posturec0 = posture_count_1_label.Text,
			posturec1 = posture_count_2_label.Text,
			posturec2 = posture_count_3_label.Text,
			posturec3 = posture_count_4_label.Text,
			posturec4 = posture_count_5_label.Text,
			posturec5 = posture_count_6_label.Text,
			posturec6 = posture_count_7_label.Text
		};
		using (FileStream stream = File.OpenRead(filepath))
		{
			XWPFDocument doc = new XWPFDocument((Stream)stream);
			foreach (XWPFParagraph para2 in doc.Paragraphs)
			{
				NPOIHleper.ReplaceKey(para2, tt);
			}
			IList<XWPFTable> tables = doc.Tables;
			foreach (XWPFTable table in tables)
			{
				int index = tables.IndexOf(table);
				if (index == 3)
				{
					int cW3 = 6000;
					int cH3 = 3300;
					int widthPic3 = (int)((double)cW3 / 587.0 * 38.4 * 9525.0);
					int heightPic3 = (int)((double)cH3 / 587.0 * 38.4 * 9525.0);
					if (userFileName != "")
					{
						XWPFTableCell Cell2 = table.GetRow(0).GetCell(0);
						FileStream img = new FileStream(userFileName, FileMode.Open, FileAccess.Read);
						XWPFParagraph p = Cell2.AddParagraph();
						XWPFRun run = p.CreateRun();
						run.AddPicture(img, 6, "Picture", widthPic3, heightPic3);
					}
				}
				if (index == 4)
				{
					int cW2 = 1500;
					int cH2 = 2500;
					int widthPic2 = (int)((double)cW2 / 587.0 * 38.4 * 9525.0);
					int heightPic2 = (int)((double)cH2 / 587.0 * 38.4 * 9525.0);
					if (postureState.posture_1 != "")
					{
						XWPFTableCell Cell = table.GetRow(0).GetCell(0);
						FileStream img0 = new FileStream(postureState.posture_1, FileMode.Open, FileAccess.Read);
						XWPFParagraph p0 = Cell.AddParagraph();
						XWPFRun run0 = p0.CreateRun();
						run0.AddPicture(img0, 6, "Picture_1", widthPic2, heightPic2);
					}
					if (postureState.posture_2 != "")
					{
						XWPFTableCell Cell4 = table.GetRow(0).GetCell(1);
						FileStream img2 = new FileStream(postureState.posture_2, FileMode.Open, FileAccess.Read);
						XWPFParagraph p2 = Cell4.AddParagraph();
						XWPFRun run2 = p2.CreateRun();
						run2.AddPicture(img2, 6, "Picture_2", widthPic2, heightPic2);
					}
					if (postureState.posture_3 != "")
					{
						XWPFTableCell Cell5 = table.GetRow(0).GetCell(2);
						FileStream img3 = new FileStream(postureState.posture_3, FileMode.Open, FileAccess.Read);
						XWPFParagraph p3 = Cell5.AddParagraph();
						XWPFRun run3 = p3.CreateRun();
						run3.AddPicture(img3, 6, "Picture_3", widthPic2, heightPic2);
					}
					if (postureState.posture_4 != "")
					{
						XWPFTableCell Cell6 = table.GetRow(0).GetCell(3);
						FileStream img4 = new FileStream(postureState.posture_4, FileMode.Open, FileAccess.Read);
						XWPFParagraph p4 = Cell6.AddParagraph();
						XWPFRun run4 = p4.CreateRun();
						run4.AddPicture(img4, 6, "Picture_4", widthPic2, heightPic2);
					}
					if (postureState.posture_5 != "")
					{
						XWPFTableCell Cell7 = table.GetRow(0).GetCell(4);
						FileStream img5 = new FileStream(postureState.posture_5, FileMode.Open, FileAccess.Read);
						XWPFParagraph p5 = Cell7.AddParagraph();
						XWPFRun run5 = p5.CreateRun();
						run5.AddPicture(img5, 6, "Picture_5", widthPic2, heightPic2);
					}
				}
				if (index == 5)
				{
					int cW = 1500;
					int cH = 2500;
					int widthPic = (int)((double)cW / 587.0 * 38.4 * 9525.0);
					int heightPic = (int)((double)cH / 587.0 * 38.4 * 9525.0);
					if (postureState.posture_6 != "")
					{
						XWPFTableCell Cell0 = table.GetRow(0).GetCell(0);
						FileStream img6 = new FileStream(postureState.posture_6, FileMode.Open, FileAccess.Read);
						XWPFParagraph p6 = Cell0.AddParagraph();
						XWPFRun run6 = p6.CreateRun();
						run6.AddPicture(img6, 6, "Picture_6", widthPic, heightPic);
					}
					if (postureState.posture_7 != "")
					{
						XWPFTableCell Cell3 = table.GetRow(0).GetCell(1);
						FileStream img7 = new FileStream(postureState.posture_7, FileMode.Open, FileAccess.Read);
						XWPFParagraph p7 = Cell3.AddParagraph();
						XWPFRun run7 = p7.CreateRun();
						run7.AddPicture(img7, 6, "Picture_7", widthPic, heightPic);
					}
				}
				foreach (XWPFTableRow row in table.Rows)
				{
					foreach (XWPFTableCell cell in row.GetTableCells())
					{
						foreach (XWPFParagraph para in cell.Paragraphs)
						{
							NPOIHleper.ReplaceKey(para, tt);
						}
					}
				}
			}
			FileStream out1 = new FileStream("Picture\\reportTable_" + DateTime.Now.Ticks + ".docx", FileMode.Create);
			doc.Write(out1);
			out1.Close();
			DialogResult dr = MessageBox.Show("保存成功!");
			if (dr == DialogResult.OK)
			{
				Close();
			}
		}
		return true;
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
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.userNameLabel = new System.Windows.Forms.Label();
		this.sexLabel = new System.Windows.Forms.Label();
		this.ageLabel = new System.Windows.Forms.Label();
		this.heightLabel = new System.Windows.Forms.Label();
		this.weightLabel = new System.Windows.Forms.Label();
		this.printButton = new MetroFramework.Controls.MetroButton();
		this.regenerateButton = new MetroFramework.Controls.MetroButton();
		this.label8 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.TurnLarge = new System.Windows.Forms.Label();
		this.TurnSmall = new System.Windows.Forms.Label();
		this.TurnRate = new System.Windows.Forms.Label();
		this.TurnTotalTime = new System.Windows.Forms.Label();
		this.sleepDuration = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.posture_0_label = new System.Windows.Forms.Label();
		this.posture_1_label = new System.Windows.Forms.Label();
		this.posture_2_label = new System.Windows.Forms.Label();
		this.posture_3_label = new System.Windows.Forms.Label();
		this.posture_4_label = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.label11 = new System.Windows.Forms.Label();
		this.pictureBox2 = new System.Windows.Forms.PictureBox();
		this.pictureBox3 = new System.Windows.Forms.PictureBox();
		this.pictureBox4 = new System.Windows.Forms.PictureBox();
		this.pictureBox5 = new System.Windows.Forms.PictureBox();
		this.label12 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
		this.label17 = new System.Windows.Forms.Label();
		this.posture_count_1_label = new System.Windows.Forms.Label();
		this.posture_count_2_label = new System.Windows.Forms.Label();
		this.posture_count_3_label = new System.Windows.Forms.Label();
		this.posture_count_4_label = new System.Windows.Forms.Label();
		this.posture_count_5_label = new System.Windows.Forms.Label();
		this.DepartureCount = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.posture_5_label = new System.Windows.Forms.Label();
		this.posture_6_label = new System.Windows.Forms.Label();
		this.posture_count_6_label = new System.Windows.Forms.Label();
		this.posture_count_7_label = new System.Windows.Forms.Label();
		this.pictureBox6 = new System.Windows.Forms.PictureBox();
		this.pictureBox7 = new System.Windows.Forms.PictureBox();
		this.label23 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox3).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox4).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox5).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.chart1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox6).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox7).BeginInit();
		base.SuspendLayout();
		this.label1.Font = new System.Drawing.Font("微软雅黑", 27.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
		this.label1.Location = new System.Drawing.Point(447, 33);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(265, 58);
		this.label1.TabIndex = 4;
		this.label1.Text = "床垫检测报告";
		this.label2.Font = new System.Drawing.Font("微软雅黑", 15f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
		this.label2.Location = new System.Drawing.Point(38, 84);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(171, 40);
		this.label2.TabIndex = 5;
		this.label2.Text = "▇ 患者信息";
		this.label3.Font = new System.Drawing.Font("微软雅黑", 18f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
		this.label3.Location = new System.Drawing.Point(71, 143);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(75, 40);
		this.label3.TabIndex = 6;
		this.label3.Text = "姓名:";
		this.label4.Font = new System.Drawing.Font("微软雅黑", 18f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
		this.label4.Location = new System.Drawing.Point(280, 143);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(75, 40);
		this.label4.TabIndex = 7;
		this.label4.Text = "性别:";
		this.label5.Font = new System.Drawing.Font("微软雅黑", 18f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
		this.label5.Location = new System.Drawing.Point(501, 143);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(75, 40);
		this.label5.TabIndex = 8;
		this.label5.Text = "年龄:";
		this.label6.Font = new System.Drawing.Font("微软雅黑", 18f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
		this.label6.Location = new System.Drawing.Point(707, 143);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(75, 40);
		this.label6.TabIndex = 9;
		this.label6.Text = "身高:";
		this.label7.Font = new System.Drawing.Font("微软雅黑", 18f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
		this.label7.Location = new System.Drawing.Point(921, 143);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(75, 40);
		this.label7.TabIndex = 10;
		this.label7.Text = "体重:";
		this.userNameLabel.AutoSize = true;
		this.userNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18f);
		this.userNameLabel.Location = new System.Drawing.Point(161, 147);
		this.userNameLabel.Name = "userNameLabel";
		this.userNameLabel.Size = new System.Drawing.Size(37, 29);
		this.userNameLabel.TabIndex = 11;
		this.userNameLabel.Text = "无";
		this.sexLabel.AutoSize = true;
		this.sexLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18f);
		this.sexLabel.Location = new System.Drawing.Point(379, 147);
		this.sexLabel.Name = "sexLabel";
		this.sexLabel.Size = new System.Drawing.Size(37, 29);
		this.sexLabel.TabIndex = 12;
		this.sexLabel.Text = "无";
		this.ageLabel.AutoSize = true;
		this.ageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18f);
		this.ageLabel.Location = new System.Drawing.Point(582, 147);
		this.ageLabel.Name = "ageLabel";
		this.ageLabel.Size = new System.Drawing.Size(37, 29);
		this.ageLabel.TabIndex = 13;
		this.ageLabel.Text = "无";
		this.heightLabel.AutoSize = true;
		this.heightLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18f);
		this.heightLabel.Location = new System.Drawing.Point(805, 147);
		this.heightLabel.Name = "heightLabel";
		this.heightLabel.Size = new System.Drawing.Size(37, 29);
		this.heightLabel.TabIndex = 14;
		this.heightLabel.Text = "无";
		this.weightLabel.AutoSize = true;
		this.weightLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18f);
		this.weightLabel.Location = new System.Drawing.Point(1011, 147);
		this.weightLabel.Name = "weightLabel";
		this.weightLabel.Size = new System.Drawing.Size(37, 29);
		this.weightLabel.TabIndex = 15;
		this.weightLabel.Text = "无";
		this.printButton.Location = new System.Drawing.Point(913, 42);
		this.printButton.Name = "printButton";
		this.printButton.Size = new System.Drawing.Size(127, 31);
		this.printButton.TabIndex = 17;
		this.printButton.Text = "打印报告";
		this.printButton.UseSelectable = true;
		this.printButton.Click += new System.EventHandler(printButton_Click);
		this.regenerateButton.Location = new System.Drawing.Point(1084, 42);
		this.regenerateButton.Name = "regenerateButton";
		this.regenerateButton.Size = new System.Drawing.Size(127, 31);
		this.regenerateButton.TabIndex = 18;
		this.regenerateButton.Text = "重新生成报告";
		this.regenerateButton.UseSelectable = true;
		this.regenerateButton.Click += new System.EventHandler(regenerateButton_Click);
		this.label8.Font = new System.Drawing.Font("微软雅黑", 15f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
		this.label8.Location = new System.Drawing.Point(38, 205);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(171, 40);
		this.label8.TabIndex = 19;
		this.label8.Text = "▇ 检测结果";
		this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label10.Font = new System.Drawing.Font("微软雅黑", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
		this.label10.Location = new System.Drawing.Point(23, 256);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(105, 38);
		this.label10.TabIndex = 21;
		this.label10.Text = "大体动次数";
		this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.TurnLarge.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.TurnLarge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.TurnLarge.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.TurnLarge.Location = new System.Drawing.Point(23, 294);
		this.TurnLarge.Name = "TurnTotal";
		this.TurnLarge.Size = new System.Drawing.Size(105, 38);
		this.TurnLarge.TabIndex = 22;
		this.TurnLarge.Text = "0";
		this.TurnLarge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.TurnSmall.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.TurnSmall.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.TurnSmall.Font = new System.Drawing.Font("Calibri", 14.25f);
		this.TurnSmall.Location = new System.Drawing.Point(128, 294);
		this.TurnSmall.Name = "TurnCycle";
		this.TurnSmall.Size = new System.Drawing.Size(105, 38);
		this.TurnSmall.TabIndex = 23;
		this.TurnSmall.Text = "0";
		this.TurnSmall.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.TurnRate.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.TurnRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.TurnRate.Font = new System.Drawing.Font("Calibri", 14.25f);
		this.TurnRate.Location = new System.Drawing.Point(233, 294);
		this.TurnRate.Name = "TurnRate";
		this.TurnRate.Size = new System.Drawing.Size(105, 38);
		this.TurnRate.TabIndex = 24;
		this.TurnRate.Text = "0";
		this.TurnRate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.TurnTotalTime.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.TurnTotalTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.TurnTotalTime.Font = new System.Drawing.Font("Calibri", 14.25f);
		this.TurnTotalTime.Location = new System.Drawing.Point(338, 294);
		this.TurnTotalTime.Name = "TurnTotalTime";
		this.TurnTotalTime.Size = new System.Drawing.Size(105, 38);
		this.TurnTotalTime.TabIndex = 25;
		this.TurnTotalTime.Text = "0";
		this.TurnTotalTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.sleepDuration.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.sleepDuration.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.sleepDuration.Font = new System.Drawing.Font("Calibri", 14.25f);
		this.sleepDuration.Location = new System.Drawing.Point(443, 294);
		this.sleepDuration.Name = "sleepDuration";
		this.sleepDuration.Size = new System.Drawing.Size(105, 38);
		this.sleepDuration.TabIndex = 26;
		this.sleepDuration.Text = "0";
		this.sleepDuration.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label15.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label15.Font = new System.Drawing.Font("微软雅黑", 12f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(612, 294);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(117, 38);
		this.label15.TabIndex = 27;
		this.label15.Text = "时长统计";
		this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.posture_0_label.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.posture_0_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.posture_0_label.Font = new System.Drawing.Font("Calibri", 14.25f);
		this.posture_0_label.Location = new System.Drawing.Point(729, 294);
		this.posture_0_label.Name = "posture_0_label";
		this.posture_0_label.Size = new System.Drawing.Size(105, 38);
		this.posture_0_label.TabIndex = 28;
		this.posture_0_label.Text = "0";
		this.posture_0_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.posture_1_label.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.posture_1_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.posture_1_label.Font = new System.Drawing.Font("Calibri", 14.25f);
		this.posture_1_label.Location = new System.Drawing.Point(834, 294);
		this.posture_1_label.Name = "posture_1_label";
		this.posture_1_label.Size = new System.Drawing.Size(105, 38);
		this.posture_1_label.TabIndex = 29;
		this.posture_1_label.Text = "0";
		this.posture_1_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.posture_2_label.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.posture_2_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.posture_2_label.Font = new System.Drawing.Font("Calibri", 14.25f);
		this.posture_2_label.Location = new System.Drawing.Point(939, 294);
		this.posture_2_label.Name = "posture_2_label";
		this.posture_2_label.Size = new System.Drawing.Size(105, 38);
		this.posture_2_label.TabIndex = 30;
		this.posture_2_label.Text = "0";
		this.posture_2_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.posture_3_label.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.posture_3_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.posture_3_label.Font = new System.Drawing.Font("Calibri", 14.25f);
		this.posture_3_label.Location = new System.Drawing.Point(1044, 294);
		this.posture_3_label.Name = "posture_3_label";
		this.posture_3_label.Size = new System.Drawing.Size(105, 38);
		this.posture_3_label.TabIndex = 31;
		this.posture_3_label.Text = "0";
		this.posture_3_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.posture_4_label.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.posture_4_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.posture_4_label.Font = new System.Drawing.Font("Calibri", 14.25f);
		this.posture_4_label.Location = new System.Drawing.Point(1148, 294);
		this.posture_4_label.Name = "posture_4_label";
		this.posture_4_label.Size = new System.Drawing.Size(105, 38);
		this.posture_4_label.TabIndex = 32;
		this.posture_4_label.Text = "0";
		this.posture_4_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label22.Font = new System.Drawing.Font("微软雅黑", 12f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(128, 256);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(105, 38);
		this.label22.TabIndex = 34;
		this.label22.Text = "小体动次数";
		this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label24.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label24.Font = new System.Drawing.Font("微软雅黑", 12f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(233, 256);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(105, 38);
		this.label24.TabIndex = 36;
		this.label24.Text = "体动频率";
		this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label26.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label26.Font = new System.Drawing.Font("微软雅黑", 12f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(338, 256);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(105, 38);
		this.label26.TabIndex = 38;
		this.label26.Text = "体动时长";
		this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label28.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label28.Font = new System.Drawing.Font("微软雅黑", 12f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(443, 256);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(105, 38);
		this.label28.TabIndex = 40;
		this.label28.Text = "静止时长";
		this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label30.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label30.Font = new System.Drawing.Font("微软雅黑", 12f, System.Drawing.FontStyle.Bold);
		this.label30.Location = new System.Drawing.Point(612, 256);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(117, 38);
		this.label30.TabIndex = 42;
		this.label30.Text = "睡姿分布";
		this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label32.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label32.Font = new System.Drawing.Font("微软雅黑", 12f, System.Drawing.FontStyle.Bold);
		this.label32.Location = new System.Drawing.Point(729, 256);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(105, 38);
		this.label32.TabIndex = 44;
		this.label32.Text = "坐立";
		this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label34.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label34.Font = new System.Drawing.Font("微软雅黑", 12f, System.Drawing.FontStyle.Bold);
		this.label34.Location = new System.Drawing.Point(834, 256);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(105, 38);
		this.label34.TabIndex = 46;
		this.label34.Text = "右侧树干型";
		this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label36.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label36.Font = new System.Drawing.Font("微软雅黑", 12f, System.Drawing.FontStyle.Bold);
		this.label36.Location = new System.Drawing.Point(939, 256);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(105, 38);
		this.label36.TabIndex = 48;
		this.label36.Text = "右侧胎儿型";
		this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label38.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label38.Font = new System.Drawing.Font("微软雅黑", 12f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(1044, 256);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(105, 38);
		this.label38.TabIndex = 50;
		this.label38.Text = "仰卧";
		this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label40.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label40.Font = new System.Drawing.Font("微软雅黑", 12f, System.Drawing.FontStyle.Bold);
		this.label40.Location = new System.Drawing.Point(1148, 256);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(105, 38);
		this.label40.TabIndex = 52;
		this.label40.Text = "左侧树干型";
		this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label9.Font = new System.Drawing.Font("微软雅黑", 15f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
		this.label9.Location = new System.Drawing.Point(38, 435);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(171, 40);
		this.label9.TabIndex = 53;
		this.label9.Text = "▇ 睡眠曲线";
		this.label21.Font = new System.Drawing.Font("微软雅黑", 15f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
		this.label21.Location = new System.Drawing.Point(539, 476);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(276, 40);
		this.label21.TabIndex = 54;
		this.label21.Text = "▇ 不同姿势压力分布";
		this.pictureBox1.Location = new System.Drawing.Point(547, 530);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(131, 200);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox1.TabIndex = 55;
		this.pictureBox1.TabStop = false;
		this.label11.Font = new System.Drawing.Font("微软雅黑", 15.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		this.label11.Location = new System.Drawing.Point(581, 750);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(61, 31);
		this.label11.TabIndex = 56;
		this.label11.Text = "平躺";
		this.pictureBox2.Location = new System.Drawing.Point(684, 530);
		this.pictureBox2.Name = "pictureBox2";
		this.pictureBox2.Size = new System.Drawing.Size(131, 200);
		this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox2.TabIndex = 57;
		this.pictureBox2.TabStop = false;
		this.pictureBox3.Location = new System.Drawing.Point(821, 530);
		this.pictureBox3.Name = "pictureBox3";
		this.pictureBox3.Size = new System.Drawing.Size(131, 200);
		this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox3.TabIndex = 58;
		this.pictureBox3.TabStop = false;
		this.pictureBox4.Location = new System.Drawing.Point(958, 530);
		this.pictureBox4.Name = "pictureBox4";
		this.pictureBox4.Size = new System.Drawing.Size(131, 200);
		this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox4.TabIndex = 59;
		this.pictureBox4.TabStop = false;
		this.pictureBox5.Location = new System.Drawing.Point(1097, 530);
		this.pictureBox5.Name = "pictureBox5";
		this.pictureBox5.Size = new System.Drawing.Size(131, 200);
		this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox5.TabIndex = 60;
		this.pictureBox5.TabStop = false;
		this.label12.Font = new System.Drawing.Font("微软雅黑", 15.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		this.label12.Location = new System.Drawing.Point(692, 750);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(126, 31);
		this.label12.TabIndex = 61;
		this.label12.Text = "右侧树干型";
		this.label13.Font = new System.Drawing.Font("微软雅黑", 15.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		this.label13.Location = new System.Drawing.Point(832, 750);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(127, 31);
		this.label13.TabIndex = 62;
		this.label13.Text = "右侧胎儿型";
		this.label14.Font = new System.Drawing.Font("微软雅黑", 15.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		this.label14.Location = new System.Drawing.Point(995, 750);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(77, 31);
		this.label14.TabIndex = 63;
		this.label14.Text = "仰卧";
		this.label16.Font = new System.Drawing.Font("微软雅黑", 15.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		this.label16.Location = new System.Drawing.Point(1107, 750);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(131, 31);
		this.label16.TabIndex = 64;
		this.label16.Text = "左侧树干型";
		chartArea1.Name = "ChartArea1";
		this.chart1.ChartAreas.Add(chartArea1);
		legend1.Enabled = false;
		legend1.Name = "Legend1";
		this.chart1.Legends.Add(legend1);
		this.chart1.Location = new System.Drawing.Point(23, 478);
		this.chart1.Name = "chart1";
		series1.ChartArea = "ChartArea1";
		series1.Legend = "Legend1";
		series1.Name = "Series1";
		this.chart1.Series.Add(series1);
		this.chart1.Size = new System.Drawing.Size(487, 300);
		this.chart1.TabIndex = 65;
		this.chart1.Text = "chart1";
		this.label17.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.label17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label17.Font = new System.Drawing.Font("微软雅黑", 12f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(612, 332);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(117, 38);
		this.label17.TabIndex = 66;
		this.label17.Text = "睡姿保持时长";
		this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.posture_count_1_label.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.posture_count_1_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.posture_count_1_label.Font = new System.Drawing.Font("Calibri", 14.25f);
		this.posture_count_1_label.Location = new System.Drawing.Point(729, 332);
		this.posture_count_1_label.Name = "posture_count_1_label";
		this.posture_count_1_label.Size = new System.Drawing.Size(105, 38);
		this.posture_count_1_label.TabIndex = 67;
		this.posture_count_1_label.Text = "0";
		this.posture_count_1_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.posture_count_2_label.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.posture_count_2_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.posture_count_2_label.Font = new System.Drawing.Font("Calibri", 14.25f);
		this.posture_count_2_label.Location = new System.Drawing.Point(834, 332);
		this.posture_count_2_label.Name = "posture_count_2_label";
		this.posture_count_2_label.Size = new System.Drawing.Size(105, 38);
		this.posture_count_2_label.TabIndex = 68;
		this.posture_count_2_label.Text = "0";
		this.posture_count_2_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.posture_count_3_label.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.posture_count_3_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.posture_count_3_label.Font = new System.Drawing.Font("Calibri", 14.25f);
		this.posture_count_3_label.Location = new System.Drawing.Point(939, 332);
		this.posture_count_3_label.Name = "posture_count_3_label";
		this.posture_count_3_label.Size = new System.Drawing.Size(105, 38);
		this.posture_count_3_label.TabIndex = 69;
		this.posture_count_3_label.Text = "0";
		this.posture_count_3_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.posture_count_4_label.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.posture_count_4_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.posture_count_4_label.Font = new System.Drawing.Font("Calibri", 14.25f);
		this.posture_count_4_label.Location = new System.Drawing.Point(1044, 332);
		this.posture_count_4_label.Name = "posture_count_4_label";
		this.posture_count_4_label.Size = new System.Drawing.Size(105, 38);
		this.posture_count_4_label.TabIndex = 70;
		this.posture_count_4_label.Text = "0";
		this.posture_count_4_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.posture_count_5_label.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.posture_count_5_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.posture_count_5_label.Font = new System.Drawing.Font("Calibri", 14.25f);
		this.posture_count_5_label.Location = new System.Drawing.Point(1148, 332);
		this.posture_count_5_label.Name = "posture_count_5_label";
		this.posture_count_5_label.Size = new System.Drawing.Size(105, 38);
		this.posture_count_5_label.TabIndex = 71;
		this.posture_count_5_label.Text = "0";
		this.posture_count_5_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.DepartureCount.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.DepartureCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.DepartureCount.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DepartureCount.Location = new System.Drawing.Point(23, 370);
		this.DepartureCount.Name = "DepartureCount";
		this.DepartureCount.Size = new System.Drawing.Size(105, 38);
		this.DepartureCount.TabIndex = 73;
		this.DepartureCount.Text = "0";
		this.DepartureCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label19.Font = new System.Drawing.Font("微软雅黑", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
		this.label19.Location = new System.Drawing.Point(23, 332);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(105, 38);
		this.label19.TabIndex = 72;
		this.label19.Text = "觉醒次数";
		this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label18.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label18.Font = new System.Drawing.Font("微软雅黑", 12f, System.Drawing.FontStyle.Bold);
		this.label18.Location = new System.Drawing.Point(1252, 256);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(105, 38);
		this.label18.TabIndex = 74;
		this.label18.Text = "左侧胎儿型";
		this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label20.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label20.Font = new System.Drawing.Font("微软雅黑", 12f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(1356, 256);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(105, 38);
		this.label20.TabIndex = 75;
		this.label20.Text = "俯卧";
		this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.posture_5_label.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.posture_5_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.posture_5_label.Font = new System.Drawing.Font("Calibri", 14.25f);
		this.posture_5_label.Location = new System.Drawing.Point(1251, 294);
		this.posture_5_label.Name = "posture_5_label";
		this.posture_5_label.Size = new System.Drawing.Size(105, 38);
		this.posture_5_label.TabIndex = 76;
		this.posture_5_label.Text = "0";
		this.posture_5_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.posture_6_label.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.posture_6_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.posture_6_label.Font = new System.Drawing.Font("Calibri", 14.25f);
		this.posture_6_label.Location = new System.Drawing.Point(1356, 294);
		this.posture_6_label.Name = "posture_6_label";
		this.posture_6_label.Size = new System.Drawing.Size(105, 38);
		this.posture_6_label.TabIndex = 77;
		this.posture_6_label.Text = "0";
		this.posture_6_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.posture_count_6_label.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.posture_count_6_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.posture_count_6_label.Font = new System.Drawing.Font("Calibri", 14.25f);
		this.posture_count_6_label.Location = new System.Drawing.Point(1251, 332);
		this.posture_count_6_label.Name = "posture_count_6_label";
		this.posture_count_6_label.Size = new System.Drawing.Size(105, 38);
		this.posture_count_6_label.TabIndex = 78;
		this.posture_count_6_label.Text = "0";
		this.posture_count_6_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.posture_count_7_label.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.posture_count_7_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.posture_count_7_label.Font = new System.Drawing.Font("Calibri", 14.25f);
		this.posture_count_7_label.Location = new System.Drawing.Point(1356, 332);
		this.posture_count_7_label.Name = "posture_count_7_label";
		this.posture_count_7_label.Size = new System.Drawing.Size(105, 38);
		this.posture_count_7_label.TabIndex = 79;
		this.posture_count_7_label.Text = "0";
		this.posture_count_7_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.pictureBox6.Location = new System.Drawing.Point(1234, 530);
		this.pictureBox6.Name = "pictureBox6";
		this.pictureBox6.Size = new System.Drawing.Size(131, 200);
		this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox6.TabIndex = 80;
		this.pictureBox6.TabStop = false;
		this.pictureBox7.Location = new System.Drawing.Point(1371, 530);
		this.pictureBox7.Name = "pictureBox7";
		this.pictureBox7.Size = new System.Drawing.Size(131, 200);
		this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox7.TabIndex = 81;
		this.pictureBox7.TabStop = false;
		this.label23.Font = new System.Drawing.Font("微软雅黑", 15.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		this.label23.Location = new System.Drawing.Point(1244, 750);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(131, 31);
		this.label23.TabIndex = 82;
		this.label23.Text = "左侧胎儿型";
		this.label25.Font = new System.Drawing.Font("微软雅黑", 15.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		this.label25.Location = new System.Drawing.Point(1404, 750);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(57, 31);
		this.label25.TabIndex = 83;
		this.label25.Text = "俯卧";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1516, 790);
		base.Controls.Add(this.label25);
		base.Controls.Add(this.label23);
		base.Controls.Add(this.pictureBox7);
		base.Controls.Add(this.pictureBox6);
		base.Controls.Add(this.posture_count_7_label);
		base.Controls.Add(this.posture_count_6_label);
		base.Controls.Add(this.posture_6_label);
		base.Controls.Add(this.posture_5_label);
		base.Controls.Add(this.label20);
		base.Controls.Add(this.label18);
		base.Controls.Add(this.DepartureCount);
		base.Controls.Add(this.label19);
		base.Controls.Add(this.posture_count_5_label);
		base.Controls.Add(this.posture_count_4_label);
		base.Controls.Add(this.posture_count_3_label);
		base.Controls.Add(this.posture_count_2_label);
		base.Controls.Add(this.posture_count_1_label);
		base.Controls.Add(this.label17);
		base.Controls.Add(this.chart1);
		base.Controls.Add(this.label16);
		base.Controls.Add(this.label14);
		base.Controls.Add(this.label13);
		base.Controls.Add(this.label12);
		base.Controls.Add(this.pictureBox5);
		base.Controls.Add(this.pictureBox4);
		base.Controls.Add(this.pictureBox3);
		base.Controls.Add(this.pictureBox2);
		base.Controls.Add(this.label11);
		base.Controls.Add(this.pictureBox1);
		base.Controls.Add(this.label21);
		base.Controls.Add(this.label9);
		base.Controls.Add(this.label40);
		base.Controls.Add(this.label38);
		base.Controls.Add(this.label36);
		base.Controls.Add(this.label34);
		base.Controls.Add(this.label32);
		base.Controls.Add(this.label30);
		base.Controls.Add(this.label28);
		base.Controls.Add(this.label26);
		base.Controls.Add(this.label24);
		base.Controls.Add(this.label22);
		base.Controls.Add(this.posture_4_label);
		base.Controls.Add(this.posture_3_label);
		base.Controls.Add(this.posture_2_label);
		base.Controls.Add(this.posture_1_label);
		base.Controls.Add(this.posture_0_label);
		base.Controls.Add(this.label15);
		base.Controls.Add(this.sleepDuration);
		base.Controls.Add(this.TurnTotalTime);
		base.Controls.Add(this.TurnRate);
		base.Controls.Add(this.TurnSmall);
		base.Controls.Add(this.TurnLarge);
		base.Controls.Add(this.label8);
		base.Controls.Add(this.regenerateButton);
		base.Controls.Add(this.printButton);
		base.Controls.Add(this.weightLabel);
		base.Controls.Add(this.heightLabel);
		base.Controls.Add(this.ageLabel);
		base.Controls.Add(this.sexLabel);
		base.Controls.Add(this.userNameLabel);
		base.Controls.Add(this.label7);
		base.Controls.Add(this.label6);
		base.Controls.Add(this.label5);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.label10);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1605, 790);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(1265, 790);
		base.Name = "Report";
		base.Load += new System.EventHandler(Report_Load);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox3).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox4).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox5).EndInit();
		((System.ComponentModel.ISupportInitialize)this.chart1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox6).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox7).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
*/
