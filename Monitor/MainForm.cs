using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Resources;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MetroFramework;
using MetroFramework.Controls;
using Monitor.Properties;
using Properties;
using MetroFramework.Forms;
using System.Timers;

namespace Monitor;

public class MainForm : Form
{
	public struct CheckProgress
	{
		public RectangleF[] DrawRect;

		public Brush[] DrawBush;
	}

	private struct revArray
	{
		public byte[] RevData1;

		public byte[] RevData2;

		public byte[] RevData3;

		public byte[] RevData4;
	}

	private struct revFlag
	{
		public int revFlag1;

		public int revFlag2;

		public int revFlag3;

		public int revFlag4;
	}

	private bool initialarray;

	private CheckProgress checkProgress;

	private int COLUMN_NUM = 32;

	private int ROW_NUM = 64;

	private bool mClosePending;

	private int ForeColorIndex;

	private new Color[,] ForeColor;

	private Color[,] BackgroundColorList;

	private Socket client;

	private int Last_Row_Num;

	private const double Magnification = 0.00076923076923076923;

	private int blockCount = 8;

	private int multipleCount = 2;

	private int loopflag;

	private IContainer components;

	private PictureBox PicMainProgress;

	private BackgroundWorker bgwResult;

	private PictureBox ColorBarPictureBox;

	private Label label5;

	private Label label7;

	private PictureBox HorizontalNumberPictureBox;

	private PictureBox VerticalNumberPictureBox;

	private Label label9;

	private TextBox tb_FrameCount;

	private revArray revarray;

	private Tools.revArrayOriginal revarrayoriginal;

	private revFlag revflag;

	private Label label12;

	private Tools tools = new Tools();

	private System.Windows.Forms.Timer timer1;

	// private Label label1;

	// private Label sleepDuration;

	private Label label14;

	private Label label13;

	private Label TurnLarge;

	private Chart chart1;

	private Label metroLabel1;

	private MetroTextBox metroTextBox1;

	private MetroButton ConnectButton;

	// private Label tb_Test;

	private MetroButton metroButton2;

	// private Label label18;

	private MetroButton metroButton3;

	private MetroButton save_picture;

	private MetroCheckBox metroCheckBox1;

	private System.Windows.Forms.Timer timer2;

	private MetroButton reportButton;

	private PictureBox pictureBox1;

	private Label label20;

	private Label label21;

	private Label label22;

	private Label label23;

	private Label sexLabel;

	private Label ageLabel;

	private Label heightLabel;

	private Label weightLabel;

	// private PictureBox pictureBox2;

	// private PictureBox pictureBox3;

	private PictureBox pictureBox4;

	private Label label15;

	private Label label24;

	private Label label25;

	private MetroButton closeButton;

	private Label versionLabel;

	private Label TurnSmall;
	// 定时器每秒+1
	// private System.Windows.Forms.Timer add_perSec;

	// private PostureStateCount globalStateCount = new PostureStateCount();

	public bool globalLongTime;

    // 每秒+1的变量 time
    // private int t;
	// 关闭保存按钮
    private MetroButton metroButton7;

    // 保存按钮是否被点击
    private bool isChecked = false;

    private MetroButton BodyMove;

    private Label bodyMoveTime;

    private Label bodyMoveDuration;

    // 需要保存的路径
    private string path = "";

	// 保存登录时间
	public string StartTime = "";

	public string startStamp = "";

	public MainForm()
	{
		InitializeComponent();
		Control.CheckForIllegalCrossThreadCalls = false;
		metroLabel1.Text = "你好," + tools.GetNowUserName().ToString();
		Dictionary<string, string> userProfile = tools.GetUserProfile(tools.GetNowUserName().ToString());
		sexLabel.Text = userProfile["sex"];
		ageLabel.Text = userProfile["age"] + " 岁";
		heightLabel.Text = userProfile["height"] + " cm";
		weightLabel.Text = userProfile["weight"] + " kg";
		string userId = tools.GetLoginUserId().ToString();

		/**
		PostureStateCount postureStateCount = tools.GetUserPostureCount(userId);
		if (postureStateCount == null || postureStateCount.create_time == null || postureStateCount.create_time == "")
		{
			globalStateCount.posture_1_count = 0;
			globalStateCount.posture_2_count = 0;
			globalStateCount.posture_3_count = 0;
			globalStateCount.posture_4_count = 0;
			globalStateCount.posture_5_count = 0;
			globalStateCount.posture_6_count = 0;
			globalStateCount.posture_7_count = 0;
			tools.SetUserPostureCount(userId, globalStateCount, changeTime: true);
		}
		
		if (postureStateCount != null && postureStateCount.create_time != null)
		{
			int postureStateCountCreate = int.Parse(tools.DateTimeToUnixTime(Convert.ToDateTime(postureStateCount.create_time.ToString())).ToString());
			int nowTime = int.Parse(tools.DateTimeToUnixTime(DateTime.Now).ToString());
			int sleepTimeTotal = int.Parse(tools.GetConfigured("SleepTimeTotal"));
			if (nowTime - postureStateCountCreate < sleepTimeTotal)
			{
				globalStateCount.posture_1_count = postureStateCount.posture_1_count;
				globalStateCount.posture_2_count = postureStateCount.posture_2_count;
				globalStateCount.posture_3_count = postureStateCount.posture_3_count;
				globalStateCount.posture_4_count = postureStateCount.posture_4_count;
				globalStateCount.posture_5_count = postureStateCount.posture_5_count;
				globalStateCount.posture_6_count = postureStateCount.posture_6_count;
				globalStateCount.posture_7_count = postureStateCount.posture_7_count;
			}
			else
			{
				globalStateCount.posture_1_count = 0;
				globalStateCount.posture_2_count = 0;
				globalStateCount.posture_3_count = 0;
				globalStateCount.posture_4_count = 0;
				globalStateCount.posture_5_count = 0;
				globalStateCount.posture_6_count = 0;
				globalStateCount.posture_7_count = 0;
				tools.SetUserPostureCount(userId, globalStateCount, changeTime: true);
			}
			
	}
	*/
}

	private void MainForm_Load(object sender, EventArgs e)
	{
		try
		{
			string multiplecount = tools.GetConfigured("MultipleCount");
			if (multiplecount == null)
			{
				multiplecount = "1";
			}
			COLUMN_NUM *= Convert.ToInt32(multiplecount);
			ROW_NUM *= Convert.ToInt32(multiplecount);
			multipleCount = Convert.ToInt32(multiplecount);
		}
		catch (ConfigurationErrorsException)
		{
			MessageBox.Show("Error reading app settings");
		}
		initProgress();
		blockCount = int.Parse(tools.GetConfigured("ColorBlockNum"));
		InitChart();
	}

	private void initProgress()
	{
		checkProgress = default(CheckProgress);
		checkProgress.DrawRect = new RectangleF[COLUMN_NUM * ROW_NUM];
		checkProgress.DrawBush = new Brush[COLUMN_NUM * ROW_NUM];
		for (int j = 0; j < ROW_NUM; j++)
		{
			float rectHeight = (float)PicMainProgress.Height * 1f / (float)ROW_NUM;
			float rectWidth = (float)PicMainProgress.Width * 1f / (float)COLUMN_NUM;
			for (int i = 0; i < COLUMN_NUM; i++)
			{
				checkProgress.DrawRect[i + j * COLUMN_NUM] = new RectangleF((float)i * rectWidth, (float)j * rectHeight, rectWidth, rectHeight);
				checkProgress.DrawBush[i + j * COLUMN_NUM] = new SolidBrush(PicMainProgress.BackColor);
			}
		}
		BackgroundColorList = new Color[COLUMN_NUM, ROW_NUM];
		for (int x = 0; x < COLUMN_NUM; x++)
		{
			for (int y = 0; y < ROW_NUM; y++)
			{
				BackgroundColorList[x, y] = Color.FromArgb(127, 127, 127);
			}
		}
		ForeColor = (Color[,])BackgroundColorList.Clone();
		Init_Color_Bar();
		Init_Horizontal_Number_bar();
		Init_Vertical_Number_bar();
		Last_Row_Num = ROW_NUM;
	}

	public void Init_Color_Bar()
	{
		int heigh = ColorBarPictureBox.Width;
		int width = ColorBarPictureBox.Height;
		Bitmap bitmap = new Bitmap(width, heigh);
		for (int j = 0; j < heigh; j++)
		{
			for (int i = 0; i < width; i++)
			{
				int[] color = Calculate_Color((int)(4095.0 / (double)width * (double)i));
				bitmap.SetPixel(i, j, Color.FromArgb(color[0], color[1], color[2]));
			}
		}
		bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
		using MemoryStream ms = new MemoryStream(262144);
		bitmap.Save(ms, ImageFormat.Bmp);
		ms.Position = 0L;
		Image image = Image.FromStream(ms);
		ColorBarPictureBox.Image = image;
	}

	public void Init_Horizontal_Number_bar()
	{
		int num = HorizontalNumberPictureBox.Width;
		int heigh = HorizontalNumberPictureBox.Height;
		Bitmap bitmap = new Bitmap(num, heigh, PixelFormat.Format24bppRgb);
		Graphics graphics = Graphics.FromImage(bitmap);
		Font font = new Font(Font.FontFamily, 6f);
		float interval = (float)num / (float)(COLUMN_NUM * 2);
		graphics.Clear(Color.White);
		for (int i = 0; i < COLUMN_NUM; i++)
		{
			graphics.DrawString((i + 1).ToString() ?? "", font, new SolidBrush(Color.Black), interval * (float)i * 2f + 2f, 0f);
		}
		using MemoryStream ms = new MemoryStream(131072);
		bitmap.Save(ms, ImageFormat.Bmp);
		ms.Position = 0L;
		Image image = Image.FromStream(ms);
		HorizontalNumberPictureBox.Image = image;
	}

	public void Init_Vertical_Number_bar()
	{
		int num = VerticalNumberPictureBox.Width;
		int heigh = VerticalNumberPictureBox.Height;
		Bitmap bitmap = new Bitmap(num, heigh, PixelFormat.Format24bppRgb);
		Graphics graphics = Graphics.FromImage(bitmap);
		Font font = new Font(Font.FontFamily, 8f);
		float interval = (float)heigh / (float)(ROW_NUM * 2);
		graphics.Clear(Color.White);
		for (int i = 0; i < ROW_NUM; i++)
		{
			if (ROW_NUM < 16)
			{
				graphics.DrawString((i + 1).ToString() ?? "", font, new SolidBrush(Color.Black), 4f, interval * ((float)i * 2f + 1f));
			}
			else if (ROW_NUM < 32)
			{
				graphics.DrawString((i + 1).ToString() ?? "", font, new SolidBrush(Color.Black), 4f, interval * ((float)i * 2f + 0.5f));
			}
			else if (ROW_NUM < 64)
			{
				graphics.DrawString((i + 1).ToString() ?? "", font, new SolidBrush(Color.Black), 4f, interval * (float)i * 2f);
			}
			else
			{
				graphics.DrawString((i + 1).ToString() ?? "", font, new SolidBrush(Color.Black), 4f, interval * ((float)i * 2f - 0.5f));
			}
		}
		using MemoryStream ms = new MemoryStream(262144);
		bitmap.Save(ms, ImageFormat.Bmp);
		ms.Position = 0L;
		Image image = Image.FromStream(ms);
		VerticalNumberPictureBox.Image = image;
	}

	private int[] ParseNumIntArr(int colorNum)
	{
		int r = 0xFF & colorNum;
		int g = 0xFF00 & colorNum;
		g >>= 8;
		int b = 0xFF0000 & colorNum;
		b >>= 16;
		return new int[3] { r, g, b };
	}

	private int ParseRGB(Color color)
	{
		return (color.B << 16) | (ushort)((color.G << 8) | color.R);
	}

	public int[] Calculate_Color(int color_data)
	{
		int countSize = 4096 / blockCount;
		int blockIndex = color_data / countSize;
		int color = blockIndex * countSize + countSize / 2;
		int r;
		int g;
		int b;
		if (color == 4095)
		{
			r = (g = (b = 255));
		}
		else
		{
			b = (int)(255.0 * Math.Cos(0.00076923076923076923 * (double)color));
			g = (int)(255.0 * Math.Sin(0.00076923076923076923 * (double)color));
			r = (int)(-255.0 * Math.Cos(0.00076923076923076923 * (double)color));
			if (r < 0)
			{
				r = 0;
			}
			if (g < 0)
			{
				g = 0;
			}
			if (b < 0)
			{
				b = 0;
			}
		}
		return new int[3] { r, g, b };
	}

	private void bgwResult_DoWork(object sender, DoWorkEventArgs e)
	{
		while (!bgwResult.CancellationPending)
		{
			Thread.Sleep(10);
			if (client == null || !client.Connected)
			{
				Thread.Sleep(3000);
			}
			Color[,] randomColors = GetColorsFromSocket();
			ThreadPool.QueueUserWorkItem(delegate
			{
				if (randomColors != null)
				{
					for (int i = 0; i < ROW_NUM; i++)
					{
						for (int j = 0; j < COLUMN_NUM; j++)
						{
							checkProgress.DrawBush[j + i * COLUMN_NUM] = new SolidBrush(randomColors[j, i]);
						}
					}
					try
					{
						bgwResult.ReportProgress(1);
					}
					catch (Exception)
					{
					}
				}
				Thread.Sleep(1);
			});
		}
		e.Cancel = true;
	}

	private void bgwResult_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		PicMainProgress.Invalidate();
	}

	private void PicMainProgress_Paint(object sender, PaintEventArgs e)
	{
		e.Graphics.Clear(SystemColors.Control);
		for (int i = 0; i < COLUMN_NUM * ROW_NUM; i++)
		{
			e.Graphics.FillRectangle(checkProgress.DrawBush[i], checkProgress.DrawRect[i]);
		}
		e.Dispose();
	}

	public DataTable ToDataTable<T>(IList<T> data)
	{
		PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
		DataTable dt = new DataTable();
		for (int j = 0; j < properties.Count; j++)
		{
			PropertyDescriptor property = properties[j];
			dt.Columns.Add(property.Name, property.PropertyType);
		}
		object[] values = new object[properties.Count];
		foreach (T item in data)
		{
			for (int i = 0; i < values.Length; i++)
			{
				values[i] = properties[i].GetValue(item);
			}
			dt.Rows.Add(values);
		}
		return dt;
	}

	public Color[,] GetColorsFromSocket()
	{
		if (client == null || !client.Connected)
		{
			Console.WriteLine("当前设备处于断线状态");
			// tb_Test.Text = "设备为断线状态!";
			Console.WriteLine("--------------------程序停止--------------------");
			// pictureBox2.BackgroundImage = Image.FromFile("./Resources/black.png");
			// label1.Text = "离床";
			try
			{
				client.Shutdown(SocketShutdown.Both);
				client.Close();
			}
			catch (Exception)
			{
			}
			bgwResult.CancelAsync();
			bgwResult.Dispose();

			closeButton.Visible = false;
			ConnectButton.Visible = true;
			timer1.Stop();

			ForeColorIndex = 0;
			loopflag = 0;
			return BackgroundColorList;
		}
		else { 
			try
			{
				byte[] response = new byte[65536];
				int length = client.Receive(response);
				Console.WriteLine(length);
				byte[] data = response.Take(length).ToArray();
				Console.WriteLine("本次数据的长度为:" + data.Length);
				if (data.Length == 1024)
				{
					try
					{
						//if (tb_Test.Text == "设备为断线状态!")
						//{
						//	tb_Test.Text = "连接成功!";
						//}
						if (!initialarray)
						{
							if (loopflag == 0)
							{
								revflag.revFlag1++;
								if (revflag.revFlag1 < 8)
								{
									Console.WriteLine("本次数据将会被丢弃，本次索引为:" + revflag.revFlag1);
								}
								else
								{
									revflag.revFlag1 = 0;
									metroTextBox1.Text = "";
									revarray.RevData1 = data;
									revarrayoriginal.RevData1 = data;
								}
							}
							else if (loopflag == 1)
							{
								revflag.revFlag2++;
								if (revflag.revFlag2 >= 8)
								{
									revflag.revFlag2 = 0;
									revarray.RevData2 = data;
									revarrayoriginal.RevData2 = data;
								}
							}
							else if (loopflag == 2)
							{
								revflag.revFlag3++;
								if (revflag.revFlag3 >= 8)
								{
									revflag.revFlag3 = 0;
									revarray.RevData3 = data;
									revarrayoriginal.RevData3 = data;
								}
							}
							else if (loopflag == 3)
							{
								revflag.revFlag4++;
								if (revflag.revFlag4 >= 8)
								{
									revflag.revFlag4 = 0;
									revarray.RevData4 = data;
									revarrayoriginal.RevData4 = data;
									ThreadPool.QueueUserWorkItem(delegate
									{
										List<byte[]> list3 = tools.FourByteSplicing(revarrayoriginal, isChecked, path);
										double num5 = double.Parse(tools.GetConfigured("Num_k"));
										double num6 = double.Parse(tools.GetConfigured("Num_b"));
										string text2 = "";
										text2 += "      ";
										for (int num7 = 0; num7 < 32; num7++)
										{
											text2 = text2 + "[" + (num7 + 1).ToString("00") + "]    ";
										}
										text2 += "\r\n";
										for (int num8 = 0; num8 < list3.Count; num8++)
										{
											text2 = text2 + "[" + (num8 + 1).ToString("00") + "]";
											List<byte[]> list4 = tools.splitAry(list3[num8], 2);
											for (int num9 = 0; num9 < list4.Count; num9++)
											{
												int num10 = tools.Byte2Int(list4[num9]);
												text2 = text2 + ((decimal)((double)num10 * num5 + num6)).ToString("#0.00").PadLeft(7) + " ";
											}
											metroTextBox1.Text = metroTextBox1.Text + "\r\n" + text2;
											text2 = "";
										}
									});
								}
							}
						}
						if (initialarray)
						{
							if (loopflag == 0)
							{
								revflag.revFlag1++;
								if (revflag.revFlag1 < 8)
								{
									Console.WriteLine("本次数据将会被丢弃，本次索引为:" + revflag.revFlag1);
								}
								else if (revarray.RevData1 != null)
								{
									for (int l = 0; l < data.Length; l++)
									{
										data[l] = tools.JudgmentData(data[l], revarray.RevData1[l]);
									}
									revarrayoriginal.RevData1 = data;
								}
							}
							else if (loopflag == 1)
							{
								revflag.revFlag2++;
								if (revflag.revFlag2 >= 8 && revarray.RevData2 != null)
								{
									for (int k = 0; k < data.Length; k++)
									{
										data[k] = tools.JudgmentData(data[k], revarray.RevData2[k]);
									}
									revarrayoriginal.RevData2 = data;
								}
							}
							else if (loopflag == 2)
							{
								revflag.revFlag3++;
								if (revflag.revFlag3 >= 8 && revarray.RevData3 != null)
								{
									for (int j = 0; j < data.Length; j++)
									{
										data[j] = tools.JudgmentData(data[j], revarray.RevData3[j]);
									}
									revarrayoriginal.RevData3 = data;
								}
							}
							else if (loopflag == 3)
							{
								revflag.revFlag4++;
								if (revflag.revFlag4 >= 8 && revarray.RevData4 != null)
								{
									for (int i = 0; i < data.Length; i++)
									{
										data[i] = tools.JudgmentData(data[i], revarray.RevData4[i]);
									}
									revarrayoriginal.RevData4 = data;
									ThreadPool.QueueUserWorkItem(delegate
									{
										List<byte[]> list = tools.FourByteSplicing(revarrayoriginal, isChecked, path);
										double num = double.Parse(tools.GetConfigured("Num_k"));
										double num2 = double.Parse(tools.GetConfigured("Num_b"));
										string text = "";
										text += "      ";
										for (int m = 0; m < 32; m++)
										{
											text = text + "[" + (m + 1).ToString("00") + "]    ";
										}
										text += "\r\n";
										for (int n = 0; n < list.Count; n++)
										{
											text = text + "[" + (n + 1).ToString("00") + "]";
											List<byte[]> list2 = tools.splitAry(list[n], 2);
											for (int num3 = 0; num3 < list2.Count; num3++)
											{
												int num4 = tools.Byte2Int(list2[num3]);
												text = text + ((decimal)((double)num4 * num + num2)).ToString("#0.00").PadLeft(7) + " ";
											}
											metroTextBox1.Text = metroTextBox1.Text + "\r\n" + text;
											text = "";
										}
									});
								}
							}
						}
						loopflag++;
			
						int[] color_data = AnalysisDataNew(data);
						GetRealColorsNew(color_data);
					}
					catch (Exception)
					{
						ForeColorIndex = 0;
						loopflag = 0;
					}
				}
				else
				{
					try
					{
						string head = Encoding.ASCII.GetString(data);
						int.Parse(head);
						ForeColorIndex = 0;
						loopflag = 0;
						tb_FrameCount.Text = head;
					}
					catch (Exception ex2)
					{
						Console.WriteLine("循环错误:" + ex2.Message);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		return ToColors(ForeColor);
	}

	public Color[,] ToColors(Color[,] data)
	{
		Color[,] result = new Color[COLUMN_NUM, ROW_NUM];
		for (int y = 0; y < ROW_NUM; y++)
		{
			for (int x = 0; x < COLUMN_NUM; x++)
			{
				int[] color = Calculate_Color((int)((double)ParseRGB(data[x, y]) * tools.GetDisplayCoefficient()));
				result[x, y] = Color.FromArgb(color[0], color[1], color[2]);
			}
		}
		return result;
	}

	public int[] AnalysisDataNew(byte[] data)
	{
		int[] result = new int[512];
		for (int i = 0; i < data.Length; i += 2)
		{
			int j = data[i + 1] & 0xFF;
			int h = (data[i] << 8) & 0xFF00;
			result[i / 2] = j | h;
		}
		return result;
	}

	public void GetRealColorsNew(int[] color_data)
	{
		int ForeColorStartRow = ForeColorIndex * 16 * multipleCount;
		ForeColorIndex++;
		for (int y5 = 0; y5 < 16; y5++)
		{
			for (int x = 0; x < 32; x++)
			{
				int ForeX = x * multipleCount;
				int ForeY = ForeColorStartRow + y5 * multipleCount;
				int curr = color_data[x + y5 * 32];
				int[] color = ParseNumIntArr(curr);
				ForeColor[ForeX, ForeY] = Color.FromArgb(color[0], color[1], color[2]);
			}
		}
		for (int y4 = 0; y4 < ROW_NUM; y4 += multipleCount)
		{
			for (int x2 = 0; x2 < COLUMN_NUM - multipleCount; x2 += multipleCount)
			{
				int curr2 = ParseRGB(ForeColor[x2, y4]);
				int interval_c = ParseRGB(ForeColor[x2 + multipleCount, y4]);
				int interval = (interval_c - curr2) / multipleCount;
				for (int i = 1; i < multipleCount; i++)
				{
					int[] color2 = ParseNumIntArr(curr2 + interval * i);
					ForeColor[x2 + i, y4] = Color.FromArgb(color2[0], color2[1], color2[2]);
				}
			}
		}
		for (int y3 = 0; y3 < ROW_NUM; y3 += multipleCount)
		{
			int x3 = COLUMN_NUM - multipleCount;
			int curr3 = ParseRGB(ForeColor[x3, y3]);
			int interval2 = -curr3 / multipleCount;
			for (int j = 1; j < multipleCount; j++)
			{
				int[] color3 = ParseNumIntArr(curr3 + interval2 * j);
				ForeColor[x3 + j, y3] = Color.FromArgb(color3[0], color3[1], color3[2]);
			}
		}
		for (int y2 = 0; y2 < ROW_NUM - multipleCount; y2 += multipleCount)
		{
			for (int x4 = 0; x4 < COLUMN_NUM; x4++)
			{
				int curr4 = ParseRGB(ForeColor[x4, y2]);
				int interval3 = (ParseRGB(ForeColor[x4, y2 + multipleCount]) - curr4) / multipleCount;
				for (int k = 1; k < multipleCount; k++)
				{
					int[] color4 = ParseNumIntArr(curr4 + interval3 * k);
					ForeColor[x4, y2 + k] = Color.FromArgb(color4[0], color4[1], color4[2]);
				}
			}
		}
		for (int x5 = 0; x5 < COLUMN_NUM; x5++)
		{
			int y = ROW_NUM - multipleCount;
			int curr5 = ParseRGB(ForeColor[x5, y]);
			int interval4 = -curr5 / multipleCount;
			for (int l = 1; l < multipleCount; l++)
			{
				int[] color5 = ParseNumIntArr(curr5 + interval4 * l);
				ForeColor[x5, y + l] = Color.FromArgb(color5[0], color5[1], color5[2]);
			}
		}
	}

	public Color[,] GetRealColors(int[] color_data)
	{
		Color[,] result = new Color[COLUMN_NUM, ROW_NUM];
		for (int y = 0; y < ROW_NUM; y++)
		{
			for (int x = 0; x < COLUMN_NUM; x++)
			{
				int[] color = Calculate_Color(color_data[x + y * 32]);
				result[x, y] = Color.FromArgb(color[0], color[1], color[2]);
			}
		}
		return result;
	}

	public void StartSocketConnect()
	{
		try
		{
			client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			string addr = tools.GetConfigured("LocalAddr") + ":" + tools.GetConfigured("LocalAddrPort");
			client.Bind(new IPEndPoint(IPAddress.Parse(tools.GetConfigured("LocalAddr")), short.Parse(tools.GetConfigured("LocalAddrPort"))));
			IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(tools.GetConfigured("DeviceAddr")), short.Parse(tools.GetConfigured("DeviceAddrPort")));
			client.Connect(endPoint);
		}
		catch (Exception exp)
		{
			MessageBox.Show("连接过程中出现问题。" + exp.Message);
		}
		if (client.Connected)
		{
			MessageBox.Show("已开始测试！");
			//tb_Test.Text = "已开始测试！";
			client.Send(Encoding.UTF8.GetBytes("1"));
			//tb_Test.Text += "\n已发送数据";
		}
		else
		{
			MessageBox.Show("连接失败。");
		}
	}

	public void CloseSocketConnect()
	{
		try
		{
			client.Disconnect(reuseSocket: false);
		}
		catch (Exception)
		{
		}
		if (!client.Connected)
		{
			MessageBox.Show("断开成功！");
			// tb_Test.Text = "断开成功！";
			ConnectButton.Enabled = true;
		}
		else
		{
			MessageBox.Show("断开失败。");
		}
	}

	private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		bgwResult.CancelAsync();
		base.Enabled = false;
		e.Cancel = true;
		mClosePending = true;
		Environment.Exit(0);
	}

	private void bgwResult_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		if (mClosePending)
		{
			Close();
		}
	}

	private void ConnectButton_Click(object sender, EventArgs e)
	{
		Console.WriteLine("--------------------程序启动--------------------");
		save_picture.Enabled = true;
		if (bgwResult.IsBusy)
		{
			MessageBox.Show("请等待上一次队列处理完毕，再次点击启动");
			return;
		}
		ForeColorIndex = 0;
		loopflag = 0;
		StartSocketConnect();

		if (!bgwResult.IsBusy)
		{
			bgwResult.RunWorkerAsync();
		}

		timer1.Start();
		ConnectButton.Visible = false;
		closeButton.Visible = true;
		globalLongTime = false;
		// 记录连接时刻
		StartTime = DateTime.Now.ToLocalTime().ToString();
		startStamp = tools.GetTimeStamp();
		// 保存文件的路径
		path = "datas//" + DateTime.Now.ToLongDateString().ToString() + DateTime.Now.TimeOfDay.ToString("hhmmss");
		save_picture.Enabled = true;

		// 查看体动的按钮 必须断开连接后才能查看
		BodyMove.Enabled = false;
	}

	private void InitChart()
	{
		chart1.Series.Clear();
		chart1.ChartAreas[0].AxisY.Minimum = 0.0;
		chart1.ChartAreas[0].AxisX.Interval = 15.0;
		chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Silver;
		chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Silver;
        	
		Series series = chart1.Series.Add("变化个数");
		series.XValueMember = "time";
		series.YValueMembers = "change_count";
		series.IsValueShownAsLabel = true;

		chart1.Series[0].ChartType = SeriesChartType.Spline;
		chart1.Series[0].Color = Color.Blue;
	}

	private void UpdateQueueValue()
	{
		
		ResourceManager rm = Monitor.Properties.Resources.ResourceManager;
		/**
		string userId = tools.GetLoginUserId().ToString();
		int userBedStatus = tools.DisplayLastOneOnBedData(int.Parse(userId));
		if (userBedStatus != 0)
		{
			// add_perSec.Start();
			string kk = tools.formatIntToTimeStr(t);
			// 当前睡眠时长 
			sleepDuration.Text = kk.ToString();
			pictureBox2.BackgroundImage = Image.FromFile("./Resources/red.png");
			label1.Text = "在床";
		}
		else
		{
			// 离床 初始化t
			// add_perSec.Stop();
			
			pictureBox2.BackgroundImage = Image.FromFile("./Resources/black.png");
			label1.Text = "离床";
		}
		*/

		bodyMoveDuration.Text = tools.formatIntToTimeStr(tools.GetTotalMoveCnt()
			).ToString();

		DataSet data = tools.DisplayChangeCntDatas();
		chart1.DataSource = data;
		chart1.Series[0].ChartType = SeriesChartType.Spline;
		chart1.Series[0].Color = Color.Blue;
		chart1.ChartAreas[0].AxisX.Interval = 1.0;
		// int onBedTime = (int)tools.DisplayLastTotalHoursOnBedData(userId, tools.DateTimeToUnixTime(DateTime.Now));

		//int sleepTimeTotal = int.Parse(tools.GetConfigured("SleepTimeTotal"));
		//if (onBedTime > sleepTimeTotal)
		//{
			//label18.Text = "饱和睡眠";
		//}
		// int turnTotalCount = 0;

		// 大体动次数
		TurnLarge.Text = tools.GetLargeMove().ToString();
		TurnSmall.Text = tools.GetSmallMove().ToString();


		/**
		PostureState postureState = tools.GetUserPosturePhoto(userId);
		if (postureState == null || postureState.create_time == null || postureState.create_time == "")
		{
			PostureState postureStateNew2 = new PostureState();
			postureStateNew2.posture_1 = "";
			postureStateNew2.posture_2 = "";
			postureStateNew2.posture_3 = "";
			postureStateNew2.posture_4 = "";
			postureStateNew2.posture_5 = "";
			postureStateNew2.posture_6 = "";
			postureStateNew2.posture_7 = "";
			tools.SetUserPosturePhoto(userId, postureStateNew2, changeTime: true);
		}
		if (userBedStatus != 0 && postureState != null && postureState.create_time != null)
		{
			int postureStateCreate = int.Parse(tools.DateTimeToUnixTime(Convert.ToDateTime(postureState.create_time.ToString())).ToString());
			int nowTime2 = int.Parse(tools.DateTimeToUnixTime(DateTime.Now).ToString());
			if (postureStateCreate - nowTime2 < sleepTimeTotal)
			{
			
				if (userBedStatus == 1 && tools.DisplayLastNOnBedData(int.Parse(userId), 3) != 0 && (postureState.posture_1 == "" || postureState.posture_1 == null))
				{
					string urlStr7 = (postureState.posture_1 = tools.saveOnBedPhoto(userId, PicMainProgress, tb_FrameCount));
					tools.SetUserPosturePhoto(userId, postureState, changeTime: false);
				
					tb_Test.Text = "坐立的图片已保存";
				}
				else if (userBedStatus == 2 && tools.DisplayLastNOnBedData(int.Parse(userId), 3) != 0 && (postureState.posture_2 == "" || postureState.posture_2 == null))
				{
					string urlStr6 = (postureState.posture_2 = tools.saveOnBedPhoto(userId, PicMainProgress, tb_FrameCount));
					tools.SetUserPosturePhoto(userId, postureState, changeTime: false);
					
					tb_Test.Text = "右侧的图片已保存";
				}
				else if (userBedStatus == 4 && tools.DisplayLastNOnBedData(int.Parse(userId), 3) != 0 && (postureState.posture_4 == "" || postureState.posture_4 == null))
				{
					string urlStr4 = (postureState.posture_4 = tools.saveOnBedPhoto(userId, PicMainProgress, tb_FrameCount));
					tools.SetUserPosturePhoto(userId, postureState, changeTime: false);
				
					tb_Test.Text = "仰卧的图片已保存";
				}
				else if (userBedStatus == 6 && tools.DisplayLastNOnBedData(int.Parse(userId), 3) != 0 && (postureState.posture_6 == "" || postureState.posture_6 == null))
				{
					string urlStr2 = (postureState.posture_6 = tools.saveOnBedPhoto(userId, PicMainProgress, tb_FrameCount));
					tools.SetUserPosturePhoto(userId, postureState, changeTime: false);
				
					tb_Test.Text = "左侧的图片已保存";
				}
				else if (userBedStatus == 7 && tools.DisplayLastNOnBedData(int.Parse(userId), 3) != 0 && (postureState.posture_7 == "" || postureState.posture_7 == null))
				{
					string urlStr = (postureState.posture_7 = tools.saveOnBedPhoto(userId, PicMainProgress, tb_FrameCount));
					tools.SetUserPosturePhoto(userId, postureState, changeTime: false);
		
					tb_Test.Text = "俯卧的图片已保存";
				}
			}
			else
			{

				tb_Test.Text = "原有检测报告时间已过期";
				PostureState postureStateNew = new PostureState();
				postureStateNew.posture_1 = "";
				postureStateNew.posture_2 = "";
				postureStateNew.posture_3 = "";
				postureStateNew.posture_4 = "";
				postureStateNew.posture_5 = "";
				postureStateNew.posture_6 = "";
				postureStateNew.posture_7 = "";
				tools.SetUserPosturePhoto(userId, postureStateNew, changeTime: true);
			}
		}
		int last_one_data = userBedStatus;
		int last_two_data = tools.DisplayLastTwoOnBedData(int.Parse(userId));
		
		if (last_one_data == last_two_data)
		{
			switch (last_one_data)
			{
			case 1:
				globalStateCount.posture_1_count++;
				break;
			case 2:
				globalStateCount.posture_2_count++;
				break;
			case 3:
				globalStateCount.posture_3_count++;
				break;
			case 4:
				globalStateCount.posture_4_count++;
				break;
			case 5:
				globalStateCount.posture_5_count++;
				break;
			case 6:
				globalStateCount.posture_6_count++;
				break;
			case 7:
				globalStateCount.posture_7_count++;
				break;
			}
		}
		else
		{
			switch (last_two_data)
			{
			case 1:
				globalStateCount.posture_1_count = 0;
				break;
			case 2:
				globalStateCount.posture_2_count = 0;
				break;
			case 3:
				globalStateCount.posture_3_count = 0;
				break;
			case 4:
				globalStateCount.posture_4_count = 0;
				break;
			case 5:
				globalStateCount.posture_5_count = 0;
				break;
			case 6:
				globalStateCount.posture_6_count = 0;
				break;
			case 7:
				globalStateCount.posture_7_count = 0;
				break;
			}
		}
		PostureStateCount postureStateCountTemporary = tools.GetUserPostureCount(userId);
		if (last_one_data == last_two_data && postureStateCountTemporary != null && postureStateCountTemporary.create_time != null)
		{
			int postureStateCountCreate = int.Parse(tools.DateTimeToUnixTime(Convert.ToDateTime(postureStateCountTemporary.create_time.ToString())).ToString());
			int nowTime = int.Parse(tools.DateTimeToUnixTime(DateTime.Now).ToString());
			if (postureStateCountCreate - nowTime < sleepTimeTotal)
			{
				
				if (postureStateCountTemporary.posture_1_count < globalStateCount.posture_1_count)
				{
					postureStateCountTemporary.posture_1_count = globalStateCount.posture_1_count;
					tools.SetUserPostureCount(userId, postureStateCountTemporary, changeTime: false);
					
					tb_Test.Text = "坐立的睡姿已保存";
				}
				else if (postureStateCountTemporary.posture_3_count < globalStateCount.posture_3_count)
				{
					postureStateCountTemporary.posture_3_count = globalStateCount.posture_3_count;
					tools.SetUserPostureCount(userId, postureStateCountTemporary, changeTime: false);
				
					tb_Test.Text = "右侧胎儿型的睡姿已保存";
				}
				else if (postureStateCountTemporary.posture_4_count < globalStateCount.posture_4_count)
				{
					postureStateCountTemporary.posture_4_count = globalStateCount.posture_4_count;
					tools.SetUserPostureCount(userId, postureStateCountTemporary, changeTime: false);
				
					tb_Test.Text = "仰卧的睡姿已保存";
				}
				else if (postureStateCountTemporary.posture_6_count < globalStateCount.posture_6_count)
				{
					postureStateCountTemporary.posture_6_count = globalStateCount.posture_6_count;
					tools.SetUserPostureCount(userId, postureStateCountTemporary, changeTime: false);
				
					tb_Test.Text = "左侧胎儿型的睡姿已保存";
				}
				else if (postureStateCountTemporary.posture_7_count < globalStateCount.posture_7_count)
				{
					postureStateCountTemporary.posture_7_count = globalStateCount.posture_7_count;
					tools.SetUserPostureCount(userId, postureStateCountTemporary, changeTime: false);
			
					tb_Test.Text = "俯卧的睡姿已保存";
				}
			}
			else
			{
				tb_Test.Text = "原有睡姿计数时间已过期";
				globalStateCount.posture_1_count = 0;
				globalStateCount.posture_2_count = 0;
				globalStateCount.posture_3_count = 0;
				globalStateCount.posture_4_count = 0;
				globalStateCount.posture_5_count = 0;
				globalStateCount.posture_6_count = 0;
				globalStateCount.posture_7_count = 0;
				tools.SetUserPostureCount(userId, globalStateCount, changeTime: true);
			}
		}
		*/

	}
	private void PollingEvent(object sender, ElapsedEventArgs e)
	{
		//do something

	}


	private void CloseButton_Click(object sender, EventArgs e)
	{
		CloseSocketConnect();
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.bgwResult = new System.ComponentModel.BackgroundWorker();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tb_FrameCount = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.metroTextBox1 = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel1 = new System.Windows.Forms.Label();
            this.metroButton3 = new MetroFramework.Controls.MetroButton();
            this.ConnectButton = new MetroFramework.Controls.MetroButton();
            this.metroButton2 = new MetroFramework.Controls.MetroButton();
            this.save_picture = new MetroFramework.Controls.MetroButton();
            this.metroCheckBox1 = new MetroFramework.Controls.MetroCheckBox();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.reportButton = new MetroFramework.Controls.MetroButton();
            this.TurnLarge = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.VerticalNumberPictureBox = new System.Windows.Forms.PictureBox();
            this.HorizontalNumberPictureBox = new System.Windows.Forms.PictureBox();
            this.ColorBarPictureBox = new System.Windows.Forms.PictureBox();
            this.PicMainProgress = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.sexLabel = new System.Windows.Forms.Label();
            this.ageLabel = new System.Windows.Forms.Label();
            this.heightLabel = new System.Windows.Forms.Label();
            this.weightLabel = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.closeButton = new MetroFramework.Controls.MetroButton();
            this.versionLabel = new System.Windows.Forms.Label();
            this.TurnSmall = new System.Windows.Forms.Label();
            this.metroButton7 = new MetroFramework.Controls.MetroButton();
            this.BodyMove = new MetroFramework.Controls.MetroButton();
            this.bodyMoveTime = new System.Windows.Forms.Label();
            this.bodyMoveDuration = new System.Windows.Forms.Label();
			// this.add_perSec = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VerticalNumberPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HorizontalNumberPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorBarPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicMainProgress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // bgwResult
            // 
            this.bgwResult.WorkerReportsProgress = true;
            this.bgwResult.WorkerSupportsCancellation = true;
            this.bgwResult.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwResult_DoWork);
            this.bgwResult.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwResult_ProgressChanged);
            this.bgwResult.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwResult_RunWorkerCompleted);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(43, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 27);
            this.label5.TabIndex = 13;
            this.label5.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(33, 767);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 24);
            this.label7.TabIndex = 14;
            this.label7.Text = "4096";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1013, 866);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(112, 27);
            this.label9.TabIndex = 18;
            this.label9.Text = "帧计数值：";
            // 
            // tb_FrameCount
            // 
            this.tb_FrameCount.Location = new System.Drawing.Point(1131, 863);
            this.tb_FrameCount.Name = "tb_FrameCount";
            this.tb_FrameCount.ReadOnly = true;
            this.tb_FrameCount.Size = new System.Drawing.Size(134, 34);
            this.tb_FrameCount.TabIndex = 19;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(1193, 1089);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(135, 27);
            this.label12.TabIndex = 36;
            this.label12.Text = "Version:0.0.8";
            // 
            // timer1
            // 
            this.timer1.Interval = 4000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // chart1
            // 
            chartArea1.AxisX.Interval = 5D;
            chartArea1.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Alignment = System.Drawing.StringAlignment.Far;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(618, 53);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.IsValueShownAsLabel = true;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(643, 404);
            this.chart1.TabIndex = 0;
            this.chart1.TabStop = false;
            this.chart1.Text = "chart1";
            // 
            // metroTextBox1
            // 
            this.metroTextBox1.CustomButton.Image = null;
            this.metroTextBox1.CustomButton.Location = new System.Drawing.Point(14, 2);
            this.metroTextBox1.CustomButton.Name = "";
            this.metroTextBox1.CustomButton.Size = new System.Drawing.Size(27, 27);
            this.metroTextBox1.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox1.CustomButton.TabIndex = 1;
            this.metroTextBox1.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox1.CustomButton.UseSelectable = true;
            this.metroTextBox1.CustomButton.Visible = false;
            this.metroTextBox1.Lines = new string[] {
        "Text"};
            this.metroTextBox1.Location = new System.Drawing.Point(580, 812);
            this.metroTextBox1.MaxLength = 32767;
            this.metroTextBox1.Multiline = true;
            this.metroTextBox1.Name = "metroTextBox1";
            this.metroTextBox1.PasswordChar = '\0';
            this.metroTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox1.SelectedText = "";
            this.metroTextBox1.SelectionLength = 0;
            this.metroTextBox1.SelectionStart = 0;
            this.metroTextBox1.ShortcutsEnabled = true;
            this.metroTextBox1.Size = new System.Drawing.Size(44, 32);
            this.metroTextBox1.TabIndex = 0;
            this.metroTextBox1.Text = "Text";
            this.metroTextBox1.UseSelectable = true;
            this.metroTextBox1.Visible = false;
            this.metroTextBox1.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox1.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.metroLabel1.Font = new System.Drawing.Font("微软雅黑 Light", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.metroLabel1.Location = new System.Drawing.Point(675, 618);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(228, 52);
            this.metroLabel1.TabIndex = 53;
            this.metroLabel1.Text = "label18       ";
            // 
            // metroButton3
            // 
            this.metroButton3.Location = new System.Drawing.Point(493, 11);
            this.metroButton3.Name = "metroButton3";
            this.metroButton3.Size = new System.Drawing.Size(143, 29);
            this.metroButton3.TabIndex = 20;
            this.metroButton3.TabStop = false;
            this.metroButton3.Text = "查看实时压力值";
            this.metroButton3.UseSelectable = true;
            this.metroButton3.Click += new System.EventHandler(this.metroButton3_Click);
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(963, 738);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(123, 63);
            this.ConnectButton.TabIndex = 54;
            this.ConnectButton.TabStop = false;
            this.ConnectButton.Text = "连接";
            this.ConnectButton.UseSelectable = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // metroButton2
            // 
            this.metroButton2.Location = new System.Drawing.Point(1139, 738);
            this.metroButton2.Name = "metroButton2";
            this.metroButton2.Size = new System.Drawing.Size(122, 63);
            this.metroButton2.TabIndex = 57;
            this.metroButton2.TabStop = false;
            this.metroButton2.Text = "退出";
            this.metroButton2.UseSelectable = true;
            this.metroButton2.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // save_picture
            // 
            this.save_picture.Enabled = false;
            this.save_picture.Location = new System.Drawing.Point(1139, 625);
            this.save_picture.Name = "save_picture";
            this.save_picture.Size = new System.Drawing.Size(122, 63);
            this.save_picture.TabIndex = 58;
            this.save_picture.TabStop = false;
            this.save_picture.Text = "保存数据";
            this.save_picture.UseSelectable = true;
            this.save_picture.Click += new System.EventHandler(this.save_picture_Click);
            // 
            // metroCheckBox1
            // 
            this.metroCheckBox1.AutoSize = true;
            this.metroCheckBox1.Location = new System.Drawing.Point(165, 818);
            this.metroCheckBox1.Name = "metroCheckBox1";
            this.metroCheckBox1.Size = new System.Drawing.Size(72, 17);
            this.metroCheckBox1.TabIndex = 59;
            this.metroCheckBox1.Text = "未过滤";
            this.metroCheckBox1.UseSelectable = true;
            this.metroCheckBox1.Click += new System.EventHandler(this.InitialValue_Click);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 30000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // reportButton
            // 
            this.reportButton.Enabled = false;
            this.reportButton.Location = new System.Drawing.Point(964, 625);
            this.reportButton.Name = "reportButton";
            this.reportButton.Size = new System.Drawing.Size(122, 63);
            this.reportButton.TabIndex = 60;
            this.reportButton.TabStop = false;
            this.reportButton.Text = "导出报告";
            this.reportButton.UseSelectable = true;
            // 
            // TurnLarge
            // 
            this.TurnLarge.AutoSize = true;
            this.TurnLarge.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.TurnLarge.Location = new System.Drawing.Point(1203, 537);
            this.TurnLarge.Name = "TurnLarge";
            this.TurnLarge.Size = new System.Drawing.Size(24, 27);
            this.TurnLarge.TabIndex = 13;
            this.TurnLarge.Text = "0";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label14.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(1092, 537);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(124, 27);
            this.label14.TabIndex = 12;
            this.label14.Text = "大体动次数: ";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(658, 469);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(227, 31);
            this.label13.TabIndex = 11;
            this.label13.Text = "▇ 数据统计与分析：";
            // 
            // VerticalNumberPictureBox
            // 
            this.VerticalNumberPictureBox.Location = new System.Drawing.Point(96, 47);
            this.VerticalNumberPictureBox.Name = "VerticalNumberPictureBox";
            this.VerticalNumberPictureBox.Size = new System.Drawing.Size(26, 713);
            this.VerticalNumberPictureBox.TabIndex = 17;
            this.VerticalNumberPictureBox.TabStop = false;
            // 
            // HorizontalNumberPictureBox
            // 
            this.HorizontalNumberPictureBox.Location = new System.Drawing.Point(129, 766);
            this.HorizontalNumberPictureBox.Name = "HorizontalNumberPictureBox";
            this.HorizontalNumberPictureBox.Size = new System.Drawing.Size(482, 21);
            this.HorizontalNumberPictureBox.TabIndex = 16;
            this.HorizontalNumberPictureBox.TabStop = false;
            // 
            // ColorBarPictureBox
            // 
            this.ColorBarPictureBox.Location = new System.Drawing.Point(37, 51);
            this.ColorBarPictureBox.Name = "ColorBarPictureBox";
            this.ColorBarPictureBox.Size = new System.Drawing.Size(33, 713);
            this.ColorBarPictureBox.TabIndex = 12;
            this.ColorBarPictureBox.TabStop = false;
            // 
            // PicMainProgress
            // 
            this.PicMainProgress.Location = new System.Drawing.Point(129, 47);
            this.PicMainProgress.Margin = new System.Windows.Forms.Padding(4);
            this.PicMainProgress.Name = "PicMainProgress";
            this.PicMainProgress.Size = new System.Drawing.Size(482, 713);
            this.PicMainProgress.TabIndex = 0;
            this.PicMainProgress.TabStop = false;
            this.PicMainProgress.Paint += new System.Windows.Forms.PaintEventHandler(this.PicMainProgress_Paint);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.pictureBox1.Location = new System.Drawing.Point(664, 597);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(222, 235);
            this.pictureBox1.TabIndex = 61;
            this.pictureBox1.TabStop = false;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label20.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label20.Location = new System.Drawing.Point(677, 675);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(72, 33);
            this.label20.TabIndex = 62;
            this.label20.Text = "性别:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label21.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label21.Location = new System.Drawing.Point(677, 714);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(72, 33);
            this.label21.TabIndex = 63;
            this.label21.Text = "年龄:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label22.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label22.Location = new System.Drawing.Point(677, 752);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(72, 33);
            this.label22.TabIndex = 64;
            this.label22.Text = "身高:";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label23.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label23.Location = new System.Drawing.Point(677, 791);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(72, 33);
            this.label23.TabIndex = 65;
            this.label23.Text = "体重:";
            // 
            // sexLabel
            // 
            this.sexLabel.AutoSize = true;
            this.sexLabel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.sexLabel.Location = new System.Drawing.Point(772, 679);
            this.sexLabel.Name = "sexLabel";
            this.sexLabel.Size = new System.Drawing.Size(61, 27);
            this.sexLabel.TabIndex = 66;
            this.sexLabel.Text = "男/女";
            // 
            // ageLabel
            // 
            this.ageLabel.AutoSize = true;
            this.ageLabel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ageLabel.Location = new System.Drawing.Point(772, 718);
            this.ageLabel.Name = "ageLabel";
            this.ageLabel.Size = new System.Drawing.Size(24, 27);
            this.ageLabel.TabIndex = 67;
            this.ageLabel.Text = "0";
            // 
            // heightLabel
            // 
            this.heightLabel.AutoSize = true;
            this.heightLabel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.heightLabel.Location = new System.Drawing.Point(772, 755);
            this.heightLabel.Name = "heightLabel";
            this.heightLabel.Size = new System.Drawing.Size(24, 27);
            this.heightLabel.TabIndex = 68;
            this.heightLabel.Text = "0";
            // 
            // weightLabel
            // 
            this.weightLabel.AutoSize = true;
            this.weightLabel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.weightLabel.Location = new System.Drawing.Point(772, 795);
            this.weightLabel.Name = "weightLabel";
            this.weightLabel.Size = new System.Drawing.Size(24, 27);
            this.weightLabel.TabIndex = 69;
            this.weightLabel.Text = "0";
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.pictureBox4.Location = new System.Drawing.Point(664, 514);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(597, 61);
            this.pictureBox4.TabIndex = 72;
            this.pictureBox4.TabStop = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(23, 6);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(203, 31);
            this.label15.TabIndex = 74;
            this.label15.Text = "▇ 睡姿分布统计：";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.BackColor = System.Drawing.Color.Transparent;
            this.label24.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label24.Location = new System.Drawing.Point(677, 14);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(251, 31);
            this.label24.TabIndex = 75;
            this.label24.Text = "▇ 实时压力个数变化曲线：";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label25.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label25.Location = new System.Drawing.Point(679, 537);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(118, 27);
            this.label25.TabIndex = 77;
            this.label25.Text = "小体动次数:";
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(963, 738);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(123, 63);
            this.closeButton.TabIndex = 78;
            this.closeButton.Text = "断开连接";
            this.closeButton.UseSelectable = true;
            this.closeButton.Visible = false;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click_1);
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(1203, 831);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(138, 27);
            this.versionLabel.TabIndex = 79;
            this.versionLabel.Text = "版本号：1.0.1";
            // 
            // TurnSmall
            // 
            this.TurnSmall.AutoSize = true;
            this.TurnSmall.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.TurnSmall.Location = new System.Drawing.Point(794, 537);
            this.TurnSmall.Name = "TurnSmall";
            this.TurnSmall.Size = new System.Drawing.Size(24, 27);
            this.TurnSmall.TabIndex = 80;
            this.TurnSmall.Text = "0";
            // 
            // metroButton7
            // 
            this.metroButton7.Enabled = false;
            this.metroButton7.Location = new System.Drawing.Point(1139, 625);
            this.metroButton7.Name = "metroButton7";
            this.metroButton7.Size = new System.Drawing.Size(122, 63);
            this.metroButton7.TabIndex = 90;
            this.metroButton7.Text = "关闭保存";
            this.metroButton7.UseSelectable = true;
            this.metroButton7.Click += new System.EventHandler(this.CloseSave_Click);
            // 
            // BodyMove
            // 
            this.BodyMove.Location = new System.Drawing.Point(1118, 18);
            this.BodyMove.Name = "BodyMove";
            this.BodyMove.Size = new System.Drawing.Size(143, 29);
            this.BodyMove.TabIndex = 91;
            this.BodyMove.TabStop = false;
            this.BodyMove.Text = "查看个数变化结果";
            this.BodyMove.UseSelectable = true;
            this.BodyMove.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // BodyMoveTime
            // 
            this.bodyMoveTime.AutoSize = true;
            this.bodyMoveTime.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.bodyMoveTime.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bodyMoveTime.Location = new System.Drawing.Point(864, 537);
            this.bodyMoveTime.Name = "bodyMoveTime";
            this.bodyMoveTime.Size = new System.Drawing.Size(104, 27);
            this.bodyMoveTime.TabIndex = 92;
            this.bodyMoveTime.Text = "体动时长: ";
            // 
            // label2
            // 
            this.bodyMoveDuration.AutoSize = true;
            this.bodyMoveDuration.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.bodyMoveDuration.Location = new System.Drawing.Point(965, 537);
            this.bodyMoveDuration.Name = "label2";
            this.bodyMoveDuration.Size = new System.Drawing.Size(94, 27);
            this.bodyMoveDuration.TabIndex = 93;
            this.bodyMoveDuration.Text = "00:00:00";
			// 
			// add_perSec
			// 
			// this.add_perSec.Interval = 1000;
			// this.add_perSec.Tick += new System.EventHandler(this.Add_perSec_Tick);
			// this.add_perSec = new System.Windows.Forms.Timer(this.components);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1332, 853);
            this.Controls.Add(this.bodyMoveDuration);
            this.Controls.Add(this.bodyMoveTime);
            this.Controls.Add(this.BodyMove);
            this.Controls.Add(this.TurnSmall);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.weightLabel);
            this.Controls.Add(this.heightLabel);
            this.Controls.Add(this.TurnLarge);
            this.Controls.Add(this.ageLabel);
            this.Controls.Add(this.sexLabel);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.reportButton);
            this.Controls.Add(this.metroCheckBox1);
            this.Controls.Add(this.save_picture);
            this.Controls.Add(this.metroButton3);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.metroTextBox1);
            this.Controls.Add(this.metroButton2);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.tb_FrameCount);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.VerticalNumberPictureBox);
            this.Controls.Add(this.HorizontalNumberPictureBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ColorBarPictureBox);
            this.Controls.Add(this.PicMainProgress);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.metroButton7);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1370, 900);
            this.MinimumSize = new System.Drawing.Size(1350, 900);
            this.Name = "MainForm";
            this.Text = "睡姿分析";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VerticalNumberPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HorizontalNumberPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorBarPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicMainProgress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

	}

	private void InitialValue_Click(object sender, EventArgs e)
	{
		if (!initialarray)
		{
			initialarray = true;
			metroCheckBox1.Text = "已过滤";
		}
		else
		{
			initialarray = false;
			metroCheckBox1.Text = "未过滤";
		}
	}

	private void timer1_Tick(object sender, EventArgs e)
	{
		lock (this)
		{
			UpdateQueueValue();
		}
	}

	private void ExitButton_Click(object sender, EventArgs e)
	{
		if (client != null && client.Connected)
		{
			MessageBox.Show("请先断开链接!!!");
		}
		else { 
			Environment.Exit(0);
		}
			
	}

	private void metroButton3_Click(object sender, EventArgs e)
	{
		DetailedPressure detailedPressure = new DetailedPressure();
		detailedPressure.ShowDialog(this);
	}

	private void save_picture_Click(object sender, EventArgs e)
	{
		isChecked = true;
		save_picture.Enabled = false;
		save_picture.Visible = false;
		metroButton7.Visible = true;
		metroButton7.Enabled = true;
	}

	private void timer2_Tick(object sender, EventArgs e)
	{
		Refresh();
	}

	/**
	private void reportButton_Click(object sender, EventArgs e)
	{
		if (closeButton.Visible)
		{
			closeButton_Click_1(sender, e);
		}
		Report report = new Report();
		report.Show();
	}
	*/
	
	private void closeButton_Click_1(object sender, EventArgs e)
	{
		Console.WriteLine("--------------------程序停止--------------------");
		// pictureBox2.BackgroundImage = Image.FromFile("./Resources/black.png");
		// label1.Text = "离床";
		try
		{
			client.Shutdown(SocketShutdown.Both);
			client.Close();
		}
		catch (Exception)
		{
		}
		bgwResult.CancelAsync();
		bgwResult.Dispose();
		closeButton.Visible = false;
		ConnectButton.Visible = true;
		timer1.Stop();
		ForeColorIndex = 0;
		loopflag = 0;

		// 连接断开后 重置定时器和time t
		//bodyMoveDuration.Text = "00:00:00";
		//add_perSec.Stop();
		//t = 0;

		// 断开连接可以查看压力个数变化结果
		BodyMove.Enabled = true;

		string EndTime = DateTime.Now.ToLocalTime().ToString();
		string endStamp = tools.GetTimeStamp();
		// 保存此次变化个数到数据库
		tools.SaveMoveResult(tools.GetLoginUserId(), StartTime, EndTime, startStamp, endStamp);

		// 每次断开连接成功重新初始化 大/小体动
		tools.moveLarge = 0;
		tools.moveSmall = 0;
	}

	/**
	private void Add_perSec_Tick(object sender, EventArgs e)
	{
		t++;
	}*/

	private void CloseSave_Click(object sender, EventArgs e) { 
		isChecked = false;
		metroButton7.Visible = false;
		save_picture.Enabled = true;
		save_picture.Visible = true;
	}

    private void metroButton1_Click(object sender, EventArgs e)
    {
		BodyMovement bodyMovement = BodyMovement.GetInstance();
		bodyMovement.Show();
    }
}
