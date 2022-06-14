using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Reflection;

namespace Monitor;

public class SQLiteHelper
{
	private static readonly string m_DataSource;

	private static readonly string m_ConnectionString;

	static SQLiteHelper()
	{
		m_DataSource = ConfigurationManager.AppSettings["DataSource"];
		try
		{
			SQLiteConnectionStringBuilder connectionStringBuilder = new SQLiteConnectionStringBuilder
			{
				Version = 3,
				Pooling = true,
				FailIfMissing = false,
				DataSource = m_DataSource
			};
			m_ConnectionString = connectionStringBuilder.ConnectionString;
			using SQLiteConnection conn = new SQLiteConnection(m_ConnectionString);
			conn.Open();
		}
		catch
		{
		}
	}

	private static SQLiteConnection GetSQLiteConnection()
	{
		return new SQLiteConnection(m_ConnectionString);
	}

	private static void PrepareCommand(SQLiteCommand cmd, SQLiteConnection conn, string cmdText, params object[] commandParameters)
	{
		if (conn.State != ConnectionState.Open)
		{
			conn.Open();
		}
		cmd.Parameters.Clear();
		cmd.Connection = conn;
		cmd.CommandText = cmdText;
		cmd.CommandType = CommandType.Text;
		cmd.CommandTimeout = 30;
		if (commandParameters != null)
		{
			foreach (object parm in commandParameters)
			{
				cmd.Parameters.AddWithValue(string.Empty, parm);
			}
		}
	}

	public static int ExecuteNonQuery(string cmdText, params object[] commandParameters)
	{
		SQLiteCommand command = new SQLiteCommand();
		using SQLiteConnection connection = GetSQLiteConnection();
		PrepareCommand(command, connection, cmdText, commandParameters);
		return command.ExecuteNonQuery();
	}

	public static DataSet ExecuteDataset(string cmdText, params object[] commandParameters)
	{
		DataSet ds = new DataSet();
		SQLiteCommand command = new SQLiteCommand();
		using SQLiteConnection connection = GetSQLiteConnection();
		PrepareCommand(command, connection, cmdText, commandParameters);
		SQLiteDataAdapter da = new SQLiteDataAdapter(command);
		da.Fill(ds);
		return ds;
	}

	public static SQLiteDataReader ExecuteReader(string cmdText, params object[] commandParameters)
	{
		SQLiteCommand command = new SQLiteCommand();
		SQLiteConnection connection = GetSQLiteConnection();
		try
		{
			PrepareCommand(command, connection, cmdText, commandParameters);
			return command.ExecuteReader(CommandBehavior.CloseConnection);
		}
		catch
		{
			connection.Close();
			throw;
		}
	}

	public static DataRow ExecuteDataRow(string cmdText, params object[] commandParameters)
	{
		DataSet ds = ExecuteDataset(cmdText, commandParameters);
		if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
		{
			return ds.Tables[0].Rows[0];
		}
		return null;
	}

	public static object ExecuteScalar(string cmdText, params object[] commandParameters)
	{
		SQLiteCommand cmd = new SQLiteCommand();
		using SQLiteConnection connection = GetSQLiteConnection();
		PrepareCommand(cmd, connection, cmdText, commandParameters);
		return cmd.ExecuteScalar();
	}

	public static bool BatchInsert<T>(string tableName, List<T> dataList)
	{
		try
		{
			if (dataList != null && dataList.Count > 0)
			{
				PropertyInfo[] propertyInfos = dataList[0].GetType().GetProperties();
				List<string> propertyStrs = new List<string>();
				string propertyStr = "";
				PropertyInfo[] array = propertyInfos;
				foreach (PropertyInfo propertyInfo in array)
				{
					propertyStrs.Add(propertyInfo.Name);
					propertyStr = propertyStr + "@" + propertyInfo.Name + ",";
				}
				propertyStr = propertyStr.Remove(propertyStr.Length - 1);
				using SQLiteConnection conn = new SQLiteConnection(m_ConnectionString);
				using SQLiteCommand command = new SQLiteCommand(conn);
				command.Connection.Open();
				using SQLiteTransaction transaction = conn.BeginTransaction();
				command.Transaction = transaction;
				command.CommandText = "insert into " + tableName + " values(" + propertyStr + ")";
				foreach (T needInsertData in dataList)
				{
					command.Parameters.Clear();
					for (int i = 0; i < propertyStrs.Count; i++)
					{
						command.Parameters.AddWithValue("@" + propertyStrs[i], propertyInfos[i].GetValue(needInsertData, null));
					}
					command.ExecuteNonQuery();
				}
				transaction.Commit();
			}
		}
		catch (Exception)
		{
			return false;
		}
		return true;
	}

	public static bool DeleteTableData(string tableName)
	{
		try
		{
			using SQLiteConnection conn = new SQLiteConnection(m_ConnectionString);
			using SQLiteCommand command = new SQLiteCommand(conn);
			command.Connection.Open();
			command.CommandText = "delete from " + tableName;
			command.ExecuteNonQuery();
		}
		catch (Exception)
		{
			return false;
		}
		return true;
	}

	public static SQLiteParameter CreateSqliteParameter(string name, DbType type, int size, object value)
	{
		SQLiteParameter parm = new SQLiteParameter(name, type, size);
		parm.Value = value;
		return parm;
	}
}
