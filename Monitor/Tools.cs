using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Monitor;

internal class Tools
{
	public int moveLarge = 0;
	
	public int moveSmall = 0;
	public struct revArrayOriginal
	{
		public byte[] RevData1;

		public byte[] RevData2;

		public byte[] RevData3;

		public byte[] RevData4;
	}

	public string GetConfigured(string appKey)
	{
		NameValueCollection appSettings = ConfigurationManager.AppSettings;
		string res = appSettings[appKey] ?? "Not Found";
		return res.Trim();
	}

	public void SaveDataToTxt(string output)
	{
		string path = "predict//tmp//data.txt";
		try
		{
			if (!File.Exists(path))
			{
				File.Create(path).Close();
			}
			UTF8Encoding utf8WithoutBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
			StreamWriter streamWriter = new StreamWriter(path, append: true, utf8WithoutBom);
			streamWriter.Write(output.Trim() + "\r\n");
			streamWriter.Flush();
			streamWriter.Close();
		}
		catch (Exception)
		{
		}
	}

	public List<byte[]> FourByteSplicing(revArrayOriginal revarrayoriginal, bool saveButton, string movePath)
	{
		byte[] coundat = new byte[4096];
		revarrayoriginal.RevData1.CopyTo(coundat, 0);
		revarrayoriginal.RevData2.CopyTo(coundat, 1024);
		revarrayoriginal.RevData3.CopyTo(coundat, 2048);
		revarrayoriginal.RevData4.CopyTo(coundat, 3072);
		List<byte[]> ts = splitAry(coundat, 64);
		string path = "predict//tmp//data.txt";
		// 保存上一刻的10进制矩阵
		int[,] pre = { };
		try
		{
			if (!Directory.Exists(movePath)) {
				Directory.CreateDirectory(movePath);
			}

			string time = DateTime.Now.ToLongDateString().ToString();

			if (File.Exists(path))
			{
				pre = ReadFileToArray(path);
				if (saveButton && !string.Equals(movePath, "")) {
					CopyToFile(path, movePath, GetTimeStamp());
				}
				File.Delete(path);
			}
		}
		catch (Exception)
		{
		}
		string lsarray = "";
		for (int i = 0; i < ts.Count; i++)
		{
			List<byte[]> ts2 = splitAry(ts[i], 2);
			for (int j = 0; j < ts2.Count; j++)
			{
				lsarray = lsarray + Byte2Int(ts2[j]) + " ";
			}
			SaveDataToTxt(lsarray + "\r\n");
			lsarray = "";
		}
		int[,] cur = ReadFileToArray(path);

		int changeCnt = CalcChangeCount(pre, cur);
		// 暂存上一次变化的个数
		SaveChangeCnt(DateTime.Now.ToString(), GetLoginUserId(), changeCnt);

		// 关闭睡姿预测
		// SendingTxtDataToServer("127.0.0.1", pre, cur);
		return ts;
	}

	/**
	public string saveOnBedPhoto(string userId, PictureBox PicMainProgress, TextBox tb_FrameCount)
	{
		int flagOnBed = DisplayLastNOnBedData(int.Parse(userId), 3);
		if (flagOnBed != 0)
		{
			Bitmap bit = new Bitmap(PicMainProgress.Width, PicMainProgress.Height);
			PicMainProgress.DrawToBitmap(bit, PicMainProgress.ClientRectangle);
			bit.Save("PictureTmp\\Picture_" + tb_FrameCount.Text + "_" + flagOnBed + "_" + userId + ".bmp", ImageFormat.Bmp);
			bit.Dispose();
			return "PictureTmp\\Picture_" + tb_FrameCount.Text + "_" + flagOnBed + "_" + userId + ".bmp";
		}
		return "";
	}


	public string ToHexStrFromByte(byte[] byteDatas)
	{
		StringBuilder builder = new StringBuilder();
		for (int i = 0; i < byteDatas.Length; i++)
		{
			builder.Append($"{byteDatas[i]:X2} ");
		}
		return builder.ToString().Trim();
	}

	public string ToDecimalStrFromByte(byte[] byteDatas)
	{
		StringBuilder builder = new StringBuilder();
		for (int i = 0; i < byteDatas.Length; i++)
		{
			builder.Append(Convert.ToInt32($"{byteDatas[i]:X2} ".Trim(), 16) + " ");
		}
		return builder.ToString().Trim();
	}

	public string ToDecimalComputeFromByte(byte[] byteDatas)
	{
		double num_k = double.Parse(GetConfigured("Num_k"));
		double num_b = double.Parse(GetConfigured("Num_b"));
		StringBuilder builder = new StringBuilder();
		for (int i = 0; i < byteDatas.Length; i++)
		{
			int num_x = Convert.ToInt32($"{byteDatas[i]:X2} ".Trim(), 16);
			builder.Append(num_k * (double)num_x + num_b + " ");
		}
		return builder.ToString().Trim();
	}

	public int bytesToInt(byte[] src, int offset)
	{
		return (src[offset] & 0xFF) | ((src[offset + 1] & 0xFF) << 8) | ((src[offset + 2] & 0xFF) << 16) | ((src[offset + 3] & 0xFF) << 24);
	}

	public byte[] intToBytes(int value)
	{
		byte[] src = new byte[4];
		src[3] = (byte)((uint)(value >> 24) & 0xFFu);
		src[2] = (byte)((uint)(value >> 16) & 0xFFu);
		src[1] = (byte)((uint)(value >> 8) & 0xFFu);
		src[0] = (byte)((uint)value & 0xFFu);
		return src;
	}
	*/

	public List<byte[]> splitAry(byte[] ary, int subSize)
	{
		int count = ((ary.Length % subSize == 0) ? (ary.Length / subSize) : (ary.Length / subSize + 1));
		List<byte[]> subAryList = new List<byte[]>();
		for (int i = 0; i < count; i++)
		{
			int index = i * subSize;
			byte[] subary = ary.Skip(index).Take(subSize).ToArray();
			subAryList.Add(subary);
		}
		return subAryList;
	}

	public int Byte2Int(byte[] b)
	{
		return ((b[0] & 0xFF) << 8) | (b[1] & 0xFF);
	}

	public byte JudgmentData(byte color_data, byte record_data)
	{
		int now_data = color_data - record_data;
		if (now_data < 0)
		{
			now_data = 0;
		}
		return (byte)now_data;
	}

	/**
	// 从数据库查出上一次的姿势
	public int QueryLastPostureStatus(int user_id)
	{
		string sql = "SELECT result_data.posture FROM result_data WHERE result_data.time >= " + SleepTimeTotal() + " AND result_data.user_id = " + user_id + " ORDER BY result_data.id DESC LIMIT 1";
		DataRow dr = SQLiteHelper.ExecuteDataRow(sql);
		if (dr == null)
		{
			return 6;
		}
		return int.Parse(dr.ItemArray[0].ToString());
	}


	public void SendingTxtDataToServer(string handleServerAddr, int[,] pre, int[,] cur)
	{
		try
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();
			string str3 = Directory.GetCurrentDirectory();
			str3 = Path.Combine(str3, "predict") + "\\";
			Process p = new Process();
			p.StartInfo.WorkingDirectory = str3;
			p.StartInfo.FileName = str3 + "predict.exe";
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.RedirectStandardInput = true;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.RedirectStandardError = true;
			p.StartInfo.CreateNoWindow = true;
			p.Start();
			p.StandardInput.AutoFlush = true;
			string strOuput = p.StandardOutput.ReadToEnd();
			p.WaitForExit();
			p.Close();
			if (strOuput.Trim() == "")
			{
				Console.WriteLine("本次处理失败");
			}
			else
			{
				strOuput = strOuput.Replace("\n", "").Replace(" ", "").Replace("\t", "")
					.Replace("\r", "")
					.TrimStart('(')
					.TrimEnd(')');
				string[] arrTemp = strOuput.Split(',');
				
				int changeCnt = CalcChangeCount(pre, cur);
				if (changeCnt >= 100 && changeCnt < 300) {
					moveSmall++;
				} else if (changeCnt >= 300) {
					moveLarge++;
				}	
				SaveSQLite(arrTemp[1].Replace("'", ""), DateTime.Now.ToString(), GetLoginUserId(), changeCnt, moveLarge, moveSmall);
			}
			sw.Stop();
			Console.WriteLine((decimal)sw.ElapsedTicks / (decimal)Stopwatch.Frequency * 1000m);
			sw.Reset();
		}
		catch (Exception)
		{
			Console.WriteLine("连接数据处理服务器错误!");
		}
	}
	*/

	public void SaveChangeCnt(string time, int userId, int changeCnt) 
	{
		string sqlstr;
		// 将处理的结果插入数据库
		sqlstr = "INSERT INTO 'main'.'change' ('id','time','user_id','change_count', 'time_stamp') VALUES (NULL, '" + time + "','" + userId + "','" + changeCnt + "','" + GetTimeStamp() + "');";

		string connectionstr = $"Data Source=sleep.db;Version=3";
		SQLiteConnection sqlc = new SQLiteConnection(connectionstr);
		if (sqlc.State == ConnectionState.Closed)
		{
			sqlc.Open();
		}
		SQLiteCommand cmd = new SQLiteCommand(sqlstr, sqlc);
		cmd.CommandType = CommandType.Text;
		int len = cmd.ExecuteNonQuery();
		sqlc.Close();
		sqlc.Dispose();
	}

	/**
	public void SaveSQLite(string posture, string time, int userId, int changeCnt, int moveLarge, int moveSmall)
	{
		// int lastStatus = QueryLastPostureStatus(userId);
		string sqlstr;
		// 将处理的结果插入数据库
		sqlstr = "INSERT INTO 'main'.'result_data' ('id', 'posture', 'time','user_id','change_count', 'move_large', 'move_small') VALUES (NULL, '" + posture + "', '" + time + "','" + userId + "','" + changeCnt + "','" + moveLarge + "','" + moveSmall + "');";

		string connectionstr = $"Data Source=sleep.db;Version=3";
		SQLiteConnection sqlc = new SQLiteConnection(connectionstr);
		if (sqlc.State == ConnectionState.Closed)
		{
			sqlc.Open();
		}
		SQLiteCommand cmd = new SQLiteCommand(sqlstr, sqlc);
		cmd.CommandType = CommandType.Text;
		int len = cmd.ExecuteNonQuery();
		sqlc.Close();
		sqlc.Dispose();
	}
	*/

	public int GetLoginUserId()
	{
		return int.Parse(GetConfigured("LoginUserId"));
	}

	public double GetDisplayCoefficient()
	{
		return double.Parse(GetConfigured("DisplayCoefficient"));
	}

	public int SleepTimeTotal()
	{
		return (int)DateTimeToUnixTime(DateTime.Now) - int.Parse(GetConfigured("SleepTimeTotal"));
	}

	/**
	public DataSet DisplayOldData()
	{
		string sql = "SELECT result_data.id,result_data.time, result_data.posture FROM  result_data  ORDER BY  result_data.id DESC LIMIT 30";
		DataSet ds = SQLiteHelper.ExecuteDataset(sql);
		foreach (DataRow row in ds.Tables[0].Rows)
		{
			row["time"] = DateTime.Parse(row["time"].ToString()).ToString("dd日HH:mm:ss");
			if (int.Parse((string)row["posture"]) == 0)
			{
				row["posture"] = 0.05;
			}
		}
		return ds;
	}


	public int DisplayLastOneOnBedData(int UserId)
	{
		string sql = "SELECT posture FROM result_data WHERE user_id = " + UserId + " ORDER BY result_data.id DESC  LIMIT 1";
		DataRow dr = SQLiteHelper.ExecuteDataRow(sql);
		if (dr == null)
		{
			return 0;
		}
		return int.Parse(dr.ItemArray[0].ToString());
	}


	public int DisplayLastTwoOnBedData(int UserId)
	{
		string sql = "SELECT posture FROM result_data WHERE user_id = " + UserId + " ORDER BY result_data.id DESC  LIMIT 2";
		DataSet ds = SQLiteHelper.ExecuteDataset(sql);
		if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows.Count != 2)
		{
			return 0;
		}
		return int.Parse(ds.Tables[0].Rows[1]["posture"].ToString());
	}

	public int DisplayLastNOnBedData(int UserId, int n)
	{
		string sql = "SELECT posture FROM result_data WHERE user_id = " + UserId + " ORDER BY result_data.id DESC  LIMIT " + n;
		DataSet ds = SQLiteHelper.ExecuteDataset(sql);
		if (ds.Tables[0].Rows.Count == 0)
		{
			return 0;
		}
		int totalCount = 0;
		foreach (DataRow row in ds.Tables[0].Rows)
		{
			totalCount += int.Parse(row["posture"].ToString());
		}
		totalCount /= n;
		if (DisplayLastOneOnBedData(UserId) == totalCount)
		{
			return DisplayLastOneOnBedData(UserId);
		}
		return 0;
	}

	public decimal DisplayLastOneHoursOnBedData(string userId, long nowTime)
	{
		string sql = "SELECT COUNT(time) FROM result_data WHERE user_id = " + userId + " AND result_data.time >= " + (nowTime + 3600) + " ORDER BY result_data.id DESC";
		DataRow dr = SQLiteHelper.ExecuteDataRow(sql);
		if (dr == null)
		{
			return 0m;
		}
		return (decimal)int.Parse(dr.ItemArray[0].ToString()) * decimal.Parse(GetConfigured("DeviceUploadInterval"));
	}

	public decimal DisplayLastTotalHoursOnBedData(string userId, long nowTime)
	{
		string sql = "SELECT COUNT(time) FROM result_data WHERE user_id = " + userId + " AND result_data.time >= " + (nowTime + int.Parse(GetConfigured("SleepTimeTotal"))) + " AND result_data.posture != 0  ORDER BY result_data.id DESC";
		DataRow dr = SQLiteHelper.ExecuteDataRow(sql);
		if (dr == null)
		{
			return 0m;
		}
		return (decimal)int.Parse(dr.ItemArray[0].ToString()) * decimal.Parse(GetConfigured("DeviceUploadInterval"));
	}

	public decimal GetSpecifyPostureColumnCount(string userId, int postureNum)
	{
		string sql = "SELECT COUNT(result_data.posture) FROM result_data WHERE result_data.posture = " + postureNum + " AND result_data.user_id = " + userId;
		DataRow dr = SQLiteHelper.ExecuteDataRow(sql);
		if (dr == null)
		{
			return 0m;
		}
		return (decimal)int.Parse(dr.ItemArray[0].ToString()) * decimal.Parse(GetConfigured("DeviceUploadInterval"));
	}

	public decimal GetColumnCount(int postureNum)
	{
		return (decimal)postureNum * decimal.Parse(GetConfigured("DeviceUploadInterval"));
	}

	public PostureState GetUserPosturePhoto(string userId)
	{
		string sql = "SELECT user_posture.* FROM user_posture WHERE user_posture.user_id =  " + userId;
		DataRow dr = SQLiteHelper.ExecuteDataRow(sql);
		if (dr == null)
		{
			PostureState postureState2 = new PostureState();
			postureState2.posture_1 = "";
			postureState2.posture_2 = "";
			postureState2.posture_3 = "";
			postureState2.posture_4 = "";
			postureState2.posture_5 = "";
			postureState2.posture_6 = "";
			postureState2.posture_7 = "";
			try
			{
				SQLiteParameter[] parameters = new SQLiteParameter[0];
				string SQLString = "INSERT INTO \"main\".\"user_posture\" (\"id\", \"create_time\", \"user_id\") VALUES (NULL, '" + DateTime.Now.ToString() + "', " + userId + ")";
				object[] commandParameters = parameters;
				int rows = SQLiteHelper.ExecuteNonQuery(SQLString, commandParameters);
				Console.WriteLine(rows);
				return postureState2;
			}
			catch (Exception)
			{
				return postureState2;
			}
		}
		PostureState postureState = new PostureState();
		postureState.create_time = dr.ItemArray[1].ToString();
		postureState.posture_1 = dr.ItemArray[3].ToString();
		postureState.posture_2 = dr.ItemArray[4].ToString();
		postureState.posture_3 = dr.ItemArray[5].ToString();
		postureState.posture_4 = dr.ItemArray[6].ToString();
		postureState.posture_5 = dr.ItemArray[7].ToString();
		postureState.posture_6 = dr.ItemArray[8].ToString();
		postureState.posture_7 = dr.ItemArray[9].ToString();
		return postureState;
	}

	public int SetUserPosturePhoto(string userId, PostureState postureState, bool changeTime)
	{
		SQLiteParameter[] parameters = new SQLiteParameter[0];
		string SQLString = ((!changeTime) ? ("UPDATE \"main\".\"user_posture\" SET \"posture_1\" = '" + postureState.posture_1 + "', \"posture_2\" = '" + postureState.posture_2 + "', \"posture_3\" = '" + postureState.posture_3 + "', \"posture_4\" = '" + postureState.posture_4 + "', \"posture_5\" = '" + postureState.posture_5 + "', \"posture_6\" = '" + postureState.posture_6 + "', \"posture_7\" = '" + postureState.posture_7 + "' WHERE \"user_id\" =" + userId) : ("UPDATE \"main\".\"user_posture\" SET \"create_time\" = '" + DateTime.Now.ToString() + "', \"posture_1\" = '" + postureState.posture_1 + "', \"posture_2\" = '" + postureState.posture_2 + "', \"posture_3\" = '" + postureState.posture_3 + "', \"posture_4\" = '" + postureState.posture_4 + "', \"posture_5\" = '" + postureState.posture_5 + "', \"posture_6\" = '" + postureState.posture_6 + "', \"posture_7\" = '" + postureState.posture_7 + "' WHERE \"user_id\" =" + userId));
		object[] commandParameters = parameters;
		return SQLiteHelper.ExecuteNonQuery(SQLString, commandParameters);
	}

	public PostureStateCount GetUserPostureCount(string userId)
	{
		string sql = "SELECT * FROM user_posture_count WHERE user_posture_count.user_id =  " + userId;
		DataRow dr = SQLiteHelper.ExecuteDataRow(sql);
		if (dr == null)
		{
			PostureStateCount postureStateCount2 = new PostureStateCount();
			postureStateCount2.posture_1_count = 0;
			postureStateCount2.posture_2_count = 0;
			postureStateCount2.posture_3_count = 0;
			postureStateCount2.posture_4_count = 0;
			postureStateCount2.posture_5_count = 0;
			postureStateCount2.posture_6_count = 0;
			postureStateCount2.posture_7_count = 0;
			try
			{
				SQLiteParameter[] parameters = new SQLiteParameter[0];
				string SQLString = "INSERT INTO \"main\".\"user_posture_count\" (\"id\", \"create_time\", \"user_id\") VALUES (NULL, '" + DateTime.Now.ToString() + "', " + userId + ")";
				object[] commandParameters = parameters;
				int rows = SQLiteHelper.ExecuteNonQuery(SQLString, commandParameters);
				Console.WriteLine(rows);
				return postureStateCount2;
			}
			catch (Exception)
			{
				return postureStateCount2;
			}
		}
		PostureStateCount postureStateCount = new PostureStateCount();
		postureStateCount.create_time = dr.ItemArray[1].ToString();
		postureStateCount.posture_1_count = int.Parse(dr.ItemArray[3].ToString());
		postureStateCount.posture_2_count = int.Parse(dr.ItemArray[4].ToString());
		postureStateCount.posture_3_count = int.Parse(dr.ItemArray[5].ToString());
		postureStateCount.posture_4_count = int.Parse(dr.ItemArray[6].ToString());
		postureStateCount.posture_5_count = int.Parse(dr.ItemArray[7].ToString());
		postureStateCount.posture_6_count = int.Parse(dr.ItemArray[8].ToString());
		postureStateCount.posture_7_count = int.Parse(dr.ItemArray[9].ToString());
		return postureStateCount;
	}

	public int SetUserPostureCount(string userId, PostureStateCount postureStateCount, bool changeTime)
	{
		SQLiteParameter[] parameters = new SQLiteParameter[0];
		string SQLString = ((!changeTime) ? ("UPDATE \"main\".\"user_posture_count\" SET \"posture_1_count\" = '" + postureStateCount.posture_1_count + "', \"posture_2_count\" = '" + postureStateCount.posture_2_count + "', \"posture_3_count\" = '" + postureStateCount.posture_3_count + "', \"posture_4_count\" = '" + postureStateCount.posture_4_count + "', \"posture_5_count\" = '" + postureStateCount.posture_5_count + "', \"posture_6_count\" = '" + postureStateCount.posture_6_count + "', \"posture_7_count\" = '" + postureStateCount.posture_7_count + "' WHERE \"user_id\" =" + userId) : ("UPDATE \"main\".\"user_posture_count\" SET \"create_time\" = '" + DateTime.Now.ToString() + "', \"posture_1_count\" = '" + postureStateCount.posture_1_count + "', \"posture_2_count\" = '" + postureStateCount.posture_2_count + "', \"posture_3_count\" = '" + postureStateCount.posture_3_count + "', \"posture_4_count\" = '" + postureStateCount.posture_4_count + "', \"posture_5_count\" = '" + postureStateCount.posture_5_count + "', \"posture_6_count\" = '" + postureStateCount.posture_6_count + "', \"posture_7_count\" = '" + postureStateCount.posture_7_count + "' WHERE \"user_id\" =" + userId));
		object[] commandParameters = parameters;
		return SQLiteHelper.ExecuteNonQuery(SQLString, commandParameters);
	}
*/
	public long DateTimeToUnixTime(DateTime dateTime)
	{
		DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
		return (long)(dateTime - startTime).TotalSeconds;
	}

	
	public string formatIntToTimeStr(int l)
	{
		TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(l));
		string str = "";
		if (ts.Hours > 0)
		{
			str = $"{ts.Hours:00}" + ":" + $"{ts.Minutes:00}" + ":" + $"{ts.Seconds:00}";
		}
		if (ts.Hours == 0 && ts.Minutes > 0)
		{
			str = "00:" + $"{ts.Minutes:00}" + ":" + $"{ts.Seconds:00}";
		}
		if (ts.Hours == 0 && ts.Minutes == 0)
		{
			str = "00:00:" + $"{ts.Seconds:00}";
		}
		return str;
	}

	public string formatIntToTimeStrText(int l, bool displayMinutes)
	{
		TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(l));
		string str = "";
		if (displayMinutes)
		{
			if (ts.Hours > 0)
			{
				str = int.Parse($"{ts.Hours:00}") + "时" + int.Parse($"{ts.Minutes:00}") + "分";
			}
			if (ts.Hours == 0 && ts.Minutes > 0)
			{
				str = int.Parse($"{ts.Minutes:00}") + "分";
			}
			if (ts.Hours == 0 && ts.Minutes == 0)
			{
				str = ((ts.Seconds != 0) ? (int.Parse($"{ts.Seconds:00}") + "秒") : "无");
			}
		}
		else
		{
			if (ts.Hours > 0)
			{
				str = int.Parse($"{ts.Hours:00}") + "时" + int.Parse($"{ts.Minutes:00}") + "分" + int.Parse($"{ts.Seconds:00}") + "秒";
			}
			if (ts.Hours == 0 && ts.Minutes > 0)
			{
				str = int.Parse($"{ts.Minutes:00}") + "分" + int.Parse($"{ts.Seconds:00}") + "秒";
			}
			if (ts.Hours == 0 && ts.Minutes == 0)
			{
				str = ((ts.Seconds != 0) ? (int.Parse($"{ts.Seconds:00}") + "秒") : "无");
			}
		}
		return str;
	}
/**

	public int GetTurnTotalCount(int user_id)
	{
		string sql = "SELECT SUM(result_data.last_state) FROM result_data WHERE result_data.time >= " + SleepTimeTotal() + " AND result_data.user_id = " + user_id + " ORDER BY result_data.id DESC ";
		DataRow dr = SQLiteHelper.ExecuteDataRow(sql);
		if (dr.ItemArray[0].ToString() == null || dr.ItemArray[0].ToString() == "")
		{
			return 0;
		}
		return int.Parse(dr.ItemArray[0].ToString());
	}
*/
	public string GetNowUserName()
	{
		string sql = "SELECT user.name FROM user WHERE user.id =  " + GetLoginUserId();
		DataRow dr = SQLiteHelper.ExecuteDataRow(sql);
		if (dr == null)
		{
			return "未命名";
		}
		return dr.ItemArray[0].ToString();
	}

	public Dictionary<string, string> GetUserProfile(string userName)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		string sql = "SELECT user.* FROM user WHERE user.name = '" + userName + "'";
		DataRow dr = SQLiteHelper.ExecuteDataRow(sql);
		if (dr == null)
		{
			dictionary.Add("height", "无");
			dictionary.Add("weight", "无");
			dictionary.Add("age", "无");
			dictionary.Add("sex", "无");
			return dictionary;
		}
		string agetStr;
		try
		{
			agetStr = (DateTime.Now.Year - Convert.ToDateTime(dr.ItemArray[5].ToString()).Year).ToString();
		}
		catch (Exception)
		{
			agetStr = "0";
		}
		dictionary.Add("height", dr.ItemArray[3].ToString());
		dictionary.Add("weight", dr.ItemArray[4].ToString());
		dictionary.Add("age", agetStr);
		dictionary.Add("sex", dr.ItemArray[6].ToString());
		return dictionary;
	}

	public int[,] ReadFileToArray(string path)
	{

		int[,] iret = null;

		ArrayList alNumLine = getFileContent(path);
		string[] strLineArr = null;
		if (alNumLine.Count > 0)
		{
			strLineArr = Convert.ToString(alNumLine[0]).Split(' ');
			iret = new int[alNumLine.Count, strLineArr.Length];
			for (int i = 0; i < alNumLine.Count; i++)
			{
				strLineArr = Convert.ToString(alNumLine[i]).Split(' ');
				for (int j = 0; j < strLineArr.Length; j++)
				{
					iret[i, j] = Convert.ToInt32(strLineArr[j]);
				}
			}
		}

		return iret;

	}

	public ArrayList getFileContent(string path)
	{

		ArrayList alRet = new ArrayList();

		if (!File.Exists(path))
		{
			return alRet;
		}

		try
		{
			//读出一行文本，并临时存放在ArrayList中
			StreamReader sr = new StreamReader(path, Encoding.GetEncoding("utf-8"));
			string l;
			while ((l = sr.ReadLine()) != null)
			{
				if (!string.IsNullOrEmpty(l.Trim()))
					alRet.Add(l.Trim());
			}
			sr.Close();
		}
		catch (IOException ex)
		{
		}

		return alRet;
	}

	/// <summary>
	/// 计算压力点变化的个数
	/// </summary>
	/// <param name="pre">上一次保存的数据文件</param>
	/// <param name="cur">当前保存的数据文件</param>
	/// <returns></returns>
	private int CalcChangeCount(int[,] pre, int[,] cur) {
		int cnt = 0;
		for (int i = 0; i < 64; i++) {
			for (int j = 0; j < 32; j++) {
				try {
					if (Math.Abs(cur[i, j] - pre[i, j]) >= 500) {
						cnt++;
					}
				} catch (Exception e) { 
				}
				
			}
		}

		return cnt;
	}

	/// <summary>
	/// 拷贝文件到另一个文件夹下
	/// </summary>
	/// <param name="sourceName">源文件路径</param>
	/// <param name="folderPath">目标路径（目标文件夹）</param>
	/// <param name="timeStamp">时间戳</param>
	private void CopyToFile(string sourceName, string folderPath, string timeStamp)
	{
		//例子：
		//源文件路径
		//string sourceName = @"D:\Source\Test.txt";
		//目标路径:项目下的NewTest文件夹,(如果没有就创建该文件夹)
		//string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NewTest");

		if (!Directory.Exists(folderPath))
		{
			Directory.CreateDirectory(folderPath);
		}

		//这里可以给文件换个新名字，如下：
		string fileName = "data_" + timeStamp + ".txt";

		//目标整体路径
		string targetPath = Path.Combine(folderPath, fileName);

		//Copy到新文件下
		FileInfo file = new FileInfo(sourceName);
		if (file.Exists)
		{
			//true 为覆盖已存在的同名文件，false 为不覆盖
			file.CopyTo(targetPath, true);
		}
	}

	/// <summary>
	/// 获取时间戳
	/// </summary>
	/// <returns></returns>
	public string GetTimeStamp()
	{
		//DateTime.Now获取的是电脑上的当前时间
		TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
		return Convert.ToInt64(ts.TotalSeconds).ToString();//精确到秒
	}

	public int[] GetChangeCnts() {
		string sql = "SELECT change_count FROM 'change' WHERE user_id ='" + GetLoginUserId() + "' ORDER BY time DESC LIMIT 3";
		DataSet ds = SQLiteHelper.ExecuteDataset(sql);

        int[] arr = new int[3];
		int i = 0;
		foreach (DataRow dr in ds.Tables[0].Rows)
		{
			arr[i++] = int.Parse(dr.ItemArray[0].ToString());
		}
		return arr;
	}


	/// <summary>
	/// 大体动次数
	/// <param name="lastChangeCnt">上一次改变的点数</param>
	/// </summary>
	/// <returns></returns>
	public int GetLargeMove()
	{
		int[] arr = GetChangeCnts();
		if (arr[0] >= 200 && arr[1] < 100 && arr[2] < 200) {
			moveLarge++;
		}

		if (moveSmall > 0 && arr[0] >= 200 && (arr[1] >= 50 && arr[1] < 200)) { 
			moveSmall--;
		}

		return moveLarge;
	}
	/// <summary>
	/// 小体动次数
	/// <param name="lastChangeCnt">上一次改变的点数</param>
	/// </summary>
	/// <returns></returns>
	public int GetSmallMove()
	{
		int[] arr = GetChangeCnts();
		if ((arr[0] >= 50 && arr[0] < 200) && arr[1] < 50 && arr[2] < 200) {
			moveSmall++;
		}
		
		return moveSmall;
	}
	/// <summary>
	/// 保存体动结果
	/// </summary>
	public void SaveMoveResult(int userId, string startTime, string endTime, string startStamp, string endStamp) {
		int large = GetLargeMove();
		int small = GetSmallMove();
		string sqlstr = "INSERT INTO 'move_result' ('id', 'start_time', 'end_time', 'move_large', 'move_small', 'user_id', 'start_stamp', 'end_stamp') VALUES (NULL, '" + startTime +"','" + endTime + "','" + large + "','" + small + "','" + userId + "','" + startStamp + "','" + endStamp + "');";
		string connectionstr = $"Data Source=sleep.db;Version=3";
		SQLiteConnection sqlc = new SQLiteConnection(connectionstr);
		if (sqlc.State == ConnectionState.Closed)
		{
			sqlc.Open();
		}
		SQLiteCommand cmd = new SQLiteCommand(sqlstr, sqlc);
		cmd.CommandType = CommandType.Text;
		int len = cmd.ExecuteNonQuery();
		sqlc.Close();
		sqlc.Dispose();
	}

	/// <summary>
	/// 查出压力变化个数最近三帧
	/// </summary>
	/// <param name="userId"></param>
	/// <returns></returns>
	public DataSet DisplayChangeCntDatas()
	{
		// 查询此次登录 10000条数据 — 每4s插入一次数据 10h约9000条
		string sql = "SELECT change_count, time FROM change Where user_id ='" + GetLoginUserId() + "' ORDER BY time DESC LIMIT 30";
		DataSet ds = SQLiteHelper.ExecuteDataset(sql);
		foreach (DataRow row in ds.Tables[0].Rows)
		{
			row["time"] = DateTime.Parse(row["time"].ToString()).ToString("HH:mm:ss");
		}
		return ds;
	}
	/// <summary>
	/// 查出最新的压力个数变化
	/// </summary>
	/// <returns></returns>
	public int GetChangeCnt() {
		string sql = "SELECT change_count FROM change WHERE user_id = " + GetLoginUserId() + " ORDER BY time DESC LIMIT 1";
		DataRow dr = SQLiteHelper.ExecuteDataRow(sql);
		if (dr == null)
		{
			return 0;
		}
		return int.Parse(dr.ItemArray[0].ToString());
	}

	/// <summary>
	/// 体动总数
	/// </summary>
	/// <returns></returns>
	public int GetTotalMoveCnt() {
		return moveLarge + moveSmall;
	}

	/// <summary>
	/// 下拉框的数据
	/// </summary>
	/// <returns></returns>
	public DataSet GetLoginTimes() {
		string sql = "SELECT start_time FROM 'move_result' Where user_id ='" + GetLoginUserId() + "'order by id LIMIT 5;";
		DataSet ds = SQLiteHelper.ExecuteDataset(sql);
		return ds;
	}
}
