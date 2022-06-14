using System;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Controls;
using MetroFramework.Forms;

namespace Monitor;

public class Register : MetroForm
{
	private IContainer components;

	private MetroButton registerButton;

	private MetroButton resetButton;

	private MetroTextBox PasswordTextBox;

	private MetroLabel metroLabel2;

	private MetroTextBox UserNameTextBox;

	private MetroLabel metroLabel1;

	private MetroLabel metroLabel3;

	private MetroLabel metroLabel4;

	private MetroTextBox weightTextBox;

	private MetroLabel metroLabel5;

	private MetroTextBox heightTextBox;

	private MetroLabel metroLabel6;

	private MetroLabel metroLabel7;

	private ComboBox sexComboBox;

	private DateTimePicker ageTimePicker;

	public Register()
	{
		InitializeComponent();
	}

	private void Register_Load(object sender, EventArgs e)
	{
		ageTimePicker.Format = DateTimePickerFormat.Custom;
		ageTimePicker.CustomFormat = "yyyy/MM/dd";
	}

	private void registerButton_Click(object sender, EventArgs e)
	{
		string userName = UserNameTextBox.Text;
		string userPassWord = PasswordTextBox.Text;
		string sex = sexComboBox.Text;
		string height = heightTextBox.Text;
		string weight = weightTextBox.Text;
		string age = ageTimePicker.Text + " 00:00:00";
		if (userName == "" || userPassWord == "")
		{
			MessageBox.Show("用户名或密码不能为空!");
			return;
		}
		string sql = "SELECT user.id FROM user WHERE user.name = '" + userName + "'";
		DataRow dr = SQLiteHelper.ExecuteDataRow(sql);
		if (dr == null)
		{
			string sqlstr = "INSERT INTO 'main'.'user' ('id', 'name', 'password', 'height', 'weight', 'age', 'sex', 'create_time') VALUES (NULL, '" + userName + "', '" + userPassWord + "', '" + height + "', '" + weight + "', '" + age + "', '" + sex + "', '" + DateTime.Now.ToString() + "');";
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
			if (len > 1 || len == 1)
			{
				MessageBox.Show("注册成功!");
				resetButton_Click(sender, e);
			}
		}
		else
		{
			MessageBox.Show("系统中已有该用户名!");
			int.Parse(dr.ItemArray[0].ToString());
		}
	}

	private void resetButton_Click(object sender, EventArgs e)
	{
		UserNameTextBox.Text = "";
		PasswordTextBox.Text = "";
		sexComboBox.Text = "";
		heightTextBox.Text = "";
		weightTextBox.Text = "";
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
		this.registerButton = new MetroFramework.Controls.MetroButton();
		this.resetButton = new MetroFramework.Controls.MetroButton();
		this.PasswordTextBox = new MetroFramework.Controls.MetroTextBox();
		this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
		this.UserNameTextBox = new MetroFramework.Controls.MetroTextBox();
		this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
		this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
		this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
		this.weightTextBox = new MetroFramework.Controls.MetroTextBox();
		this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
		this.heightTextBox = new MetroFramework.Controls.MetroTextBox();
		this.metroLabel6 = new MetroFramework.Controls.MetroLabel();
		this.metroLabel7 = new MetroFramework.Controls.MetroLabel();
		this.sexComboBox = new System.Windows.Forms.ComboBox();
		this.ageTimePicker = new System.Windows.Forms.DateTimePicker();
		base.SuspendLayout();
		this.registerButton.Location = new System.Drawing.Point(126, 285);
		this.registerButton.Name = "registerButton";
		this.registerButton.Size = new System.Drawing.Size(76, 42);
		this.registerButton.TabIndex = 11;
		this.registerButton.Text = "注册";
		this.registerButton.UseSelectable = true;
		this.registerButton.Click += new System.EventHandler(registerButton_Click);
		this.resetButton.Location = new System.Drawing.Point(291, 285);
		this.resetButton.Name = "resetButton";
		this.resetButton.Size = new System.Drawing.Size(76, 42);
		this.resetButton.TabIndex = 10;
		this.resetButton.Text = "重置";
		this.resetButton.UseSelectable = true;
		this.resetButton.Click += new System.EventHandler(resetButton_Click);
		this.PasswordTextBox.CustomButton.Image = null;
		this.PasswordTextBox.CustomButton.Location = new System.Drawing.Point(123, 2);
		this.PasswordTextBox.CustomButton.Name = "";
		this.PasswordTextBox.CustomButton.Size = new System.Drawing.Size(15, 15);
		this.PasswordTextBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
		this.PasswordTextBox.CustomButton.TabIndex = 1;
		this.PasswordTextBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
		this.PasswordTextBox.CustomButton.UseSelectable = true;
		this.PasswordTextBox.CustomButton.Visible = false;
		this.PasswordTextBox.Lines = new string[0];
		this.PasswordTextBox.Location = new System.Drawing.Point(314, 91);
		this.PasswordTextBox.MaxLength = 32767;
		this.PasswordTextBox.Name = "PasswordTextBox";
		this.PasswordTextBox.PasswordChar = '●';
		this.PasswordTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.PasswordTextBox.SelectedText = "";
		this.PasswordTextBox.SelectionLength = 0;
		this.PasswordTextBox.SelectionStart = 0;
		this.PasswordTextBox.ShortcutsEnabled = true;
		this.PasswordTextBox.Size = new System.Drawing.Size(141, 20);
		this.PasswordTextBox.TabIndex = 9;
		this.PasswordTextBox.UseSelectable = true;
		this.PasswordTextBox.UseSystemPasswordChar = true;
		this.PasswordTextBox.WaterMarkColor = System.Drawing.Color.FromArgb(109, 109, 109);
		this.PasswordTextBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
		this.metroLabel2.AutoSize = true;
		this.metroLabel2.Location = new System.Drawing.Point(245, 91);
		this.metroLabel2.Name = "metroLabel2";
		this.metroLabel2.Size = new System.Drawing.Size(40, 19);
		this.metroLabel2.TabIndex = 8;
		this.metroLabel2.Text = "密码:";
		this.UserNameTextBox.CustomButton.Image = null;
		this.UserNameTextBox.CustomButton.Location = new System.Drawing.Point(123, 2);
		this.UserNameTextBox.CustomButton.Name = "";
		this.UserNameTextBox.CustomButton.Size = new System.Drawing.Size(15, 15);
		this.UserNameTextBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
		this.UserNameTextBox.CustomButton.TabIndex = 1;
		this.UserNameTextBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
		this.UserNameTextBox.CustomButton.UseSelectable = true;
		this.UserNameTextBox.CustomButton.Visible = false;
		this.UserNameTextBox.Lines = new string[0];
		this.UserNameTextBox.Location = new System.Drawing.Point(83, 91);
		this.UserNameTextBox.MaxLength = 32767;
		this.UserNameTextBox.Name = "UserNameTextBox";
		this.UserNameTextBox.PasswordChar = '\0';
		this.UserNameTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.UserNameTextBox.SelectedText = "";
		this.UserNameTextBox.SelectionLength = 0;
		this.UserNameTextBox.SelectionStart = 0;
		this.UserNameTextBox.ShortcutsEnabled = true;
		this.UserNameTextBox.Size = new System.Drawing.Size(141, 20);
		this.UserNameTextBox.TabIndex = 7;
		this.UserNameTextBox.UseSelectable = true;
		this.UserNameTextBox.WaterMarkColor = System.Drawing.Color.FromArgb(109, 109, 109);
		this.UserNameTextBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
		this.metroLabel1.AutoSize = true;
		this.metroLabel1.Location = new System.Drawing.Point(23, 91);
		this.metroLabel1.Name = "metroLabel1";
		this.metroLabel1.Size = new System.Drawing.Size(54, 19);
		this.metroLabel1.TabIndex = 6;
		this.metroLabel1.Text = "用户名:";
		this.metroLabel3.AutoSize = true;
		this.metroLabel3.Location = new System.Drawing.Point(83, 156);
		this.metroLabel3.Name = "metroLabel3";
		this.metroLabel3.Size = new System.Drawing.Size(0, 0);
		this.metroLabel3.TabIndex = 12;
		this.metroLabel4.AutoSize = true;
		this.metroLabel4.Location = new System.Drawing.Point(23, 219);
		this.metroLabel4.Name = "metroLabel4";
		this.metroLabel4.Size = new System.Drawing.Size(40, 19);
		this.metroLabel4.TabIndex = 13;
		this.metroLabel4.Text = "性别:";
		this.weightTextBox.CustomButton.Image = null;
		this.weightTextBox.CustomButton.Location = new System.Drawing.Point(123, 2);
		this.weightTextBox.CustomButton.Name = "";
		this.weightTextBox.CustomButton.Size = new System.Drawing.Size(15, 15);
		this.weightTextBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
		this.weightTextBox.CustomButton.TabIndex = 1;
		this.weightTextBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
		this.weightTextBox.CustomButton.UseSelectable = true;
		this.weightTextBox.CustomButton.Visible = false;
		this.weightTextBox.Lines = new string[0];
		this.weightTextBox.Location = new System.Drawing.Point(314, 223);
		this.weightTextBox.MaxLength = 32767;
		this.weightTextBox.Name = "weightTextBox";
		this.weightTextBox.PasswordChar = '\0';
		this.weightTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.weightTextBox.SelectedText = "";
		this.weightTextBox.SelectionLength = 0;
		this.weightTextBox.SelectionStart = 0;
		this.weightTextBox.ShortcutsEnabled = true;
		this.weightTextBox.Size = new System.Drawing.Size(141, 20);
		this.weightTextBox.TabIndex = 16;
		this.weightTextBox.UseSelectable = true;
		this.weightTextBox.WaterMarkColor = System.Drawing.Color.FromArgb(109, 109, 109);
		this.weightTextBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
		this.metroLabel5.AutoSize = true;
		this.metroLabel5.Location = new System.Drawing.Point(245, 223);
		this.metroLabel5.Name = "metroLabel5";
		this.metroLabel5.Size = new System.Drawing.Size(62, 19);
		this.metroLabel5.TabIndex = 15;
		this.metroLabel5.Text = "体重(kg):";
		this.heightTextBox.CustomButton.Image = null;
		this.heightTextBox.CustomButton.Location = new System.Drawing.Point(123, 2);
		this.heightTextBox.CustomButton.Name = "";
		this.heightTextBox.CustomButton.Size = new System.Drawing.Size(15, 15);
		this.heightTextBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
		this.heightTextBox.CustomButton.TabIndex = 1;
		this.heightTextBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
		this.heightTextBox.CustomButton.UseSelectable = true;
		this.heightTextBox.CustomButton.Visible = false;
		this.heightTextBox.Lines = new string[0];
		this.heightTextBox.Location = new System.Drawing.Point(314, 173);
		this.heightTextBox.MaxLength = 32767;
		this.heightTextBox.Name = "heightTextBox";
		this.heightTextBox.PasswordChar = '\0';
		this.heightTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.heightTextBox.SelectedText = "";
		this.heightTextBox.SelectionLength = 0;
		this.heightTextBox.SelectionStart = 0;
		this.heightTextBox.ShortcutsEnabled = true;
		this.heightTextBox.Size = new System.Drawing.Size(141, 20);
		this.heightTextBox.TabIndex = 18;
		this.heightTextBox.UseSelectable = true;
		this.heightTextBox.WaterMarkColor = System.Drawing.Color.FromArgb(109, 109, 109);
		this.heightTextBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
		this.metroLabel6.AutoSize = true;
		this.metroLabel6.Location = new System.Drawing.Point(245, 173);
		this.metroLabel6.Name = "metroLabel6";
		this.metroLabel6.Size = new System.Drawing.Size(66, 19);
		this.metroLabel6.TabIndex = 17;
		this.metroLabel6.Text = "身高(cm):";
		this.metroLabel7.AutoSize = true;
		this.metroLabel7.Location = new System.Drawing.Point(23, 173);
		this.metroLabel7.Name = "metroLabel7";
		this.metroLabel7.Size = new System.Drawing.Size(40, 19);
		this.metroLabel7.TabIndex = 19;
		this.metroLabel7.Text = "年龄:";
		this.sexComboBox.FormattingEnabled = true;
		this.sexComboBox.Items.AddRange(new object[2] { "男", "女" });
		this.sexComboBox.Location = new System.Drawing.Point(83, 222);
		this.sexComboBox.Name = "sexComboBox";
		this.sexComboBox.Size = new System.Drawing.Size(142, 20);
		this.sexComboBox.TabIndex = 21;
		this.ageTimePicker.Location = new System.Drawing.Point(83, 173);
		this.ageTimePicker.Name = "ageTimePicker";
		this.ageTimePicker.Size = new System.Drawing.Size(141, 21);
		this.ageTimePicker.TabIndex = 22;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(490, 350);
		base.Controls.Add(this.ageTimePicker);
		base.Controls.Add(this.sexComboBox);
		base.Controls.Add(this.metroLabel7);
		base.Controls.Add(this.heightTextBox);
		base.Controls.Add(this.metroLabel6);
		base.Controls.Add(this.weightTextBox);
		base.Controls.Add(this.metroLabel5);
		base.Controls.Add(this.metroLabel4);
		base.Controls.Add(this.metroLabel3);
		base.Controls.Add(this.registerButton);
		base.Controls.Add(this.resetButton);
		base.Controls.Add(this.PasswordTextBox);
		base.Controls.Add(this.metroLabel2);
		base.Controls.Add(this.UserNameTextBox);
		base.Controls.Add(this.metroLabel1);
		this.MaximumSize = new System.Drawing.Size(490, 350);
		this.MinimumSize = new System.Drawing.Size(490, 350);
		base.Name = "Register";
		this.Text = "Register";
		base.Load += new System.EventHandler(Register_Load);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
