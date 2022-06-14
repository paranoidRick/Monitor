using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Controls;
using MetroFramework.Forms;

namespace Monitor;

public class Login : MetroForm
{
	private IContainer components;

	private MetroLabel metroLabel1;

	private MetroTextBox UserNameTextBox;

	private MetroTextBox PasswordTextBox;

	private MetroLabel metroLabel2;

	private MetroButton loginButton;

	private MetroButton registerButton;

	public Login()
	{
		InitializeComponent();
	}

	private void Login_Load(object sender, EventArgs e)
	{
	}

	private void loginButton_Click(object sender, EventArgs e)
	{
		string userName = UserNameTextBox.Text;
		string userPassWord = PasswordTextBox.Text;
		if (userName == "" || userPassWord == "")
		{
			MessageBox.Show("用户名或密码不能为空!");
			return;
		}
		string sql = "SELECT user.id FROM user WHERE user.name = '" + userName + "' AND user.password = '" + userPassWord + "' ";
		DataRow dr = SQLiteHelper.ExecuteDataRow(sql);
		if (dr == null)
		{
			MessageBox.Show("用户名或密码错误，请重试!");
			UserNameTextBox.Text = "";
			PasswordTextBox.Text = "";
			return;
		}
		MessageBox.Show("登陆成功!");
		int userId = int.Parse(dr.ItemArray[0].ToString());
		Console.WriteLine("当前登录用户的ID为:" + userId);
		Configuration config = ConfigurationManager.OpenExeConfiguration(Assembly.GetEntryAssembly().Location);
		AppSettingsSection sec = config.AppSettings;
		sec.Settings["LoginUserId"].Value = userId.ToString();
		config.Save();
		ConfigurationManager.RefreshSection("appSettings");
		MainForm mainForm = new MainForm();
		mainForm.Show();
		base.Visible = false;
	}

	private void registerButton_Click(object sender, EventArgs e)
	{
		Register register = new Register();
		register.Show();
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
		this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
		this.UserNameTextBox = new MetroFramework.Controls.MetroTextBox();
		this.PasswordTextBox = new MetroFramework.Controls.MetroTextBox();
		this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
		this.loginButton = new MetroFramework.Controls.MetroButton();
		this.registerButton = new MetroFramework.Controls.MetroButton();
		base.SuspendLayout();
		this.metroLabel1.AutoSize = true;
		this.metroLabel1.Location = new System.Drawing.Point(82, 112);
		this.metroLabel1.Name = "metroLabel1";
		this.metroLabel1.Size = new System.Drawing.Size(54, 19);
		this.metroLabel1.TabIndex = 0;
		this.metroLabel1.Text = "用户名:";
		this.UserNameTextBox.CustomButton.Image = null;
		this.UserNameTextBox.CustomButton.Location = new System.Drawing.Point(190, 1);
		this.UserNameTextBox.CustomButton.Name = "";
		this.UserNameTextBox.CustomButton.Size = new System.Drawing.Size(21, 21);
		this.UserNameTextBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
		this.UserNameTextBox.CustomButton.TabIndex = 1;
		this.UserNameTextBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
		this.UserNameTextBox.CustomButton.UseSelectable = true;
		this.UserNameTextBox.CustomButton.Visible = false;
		this.UserNameTextBox.Lines = new string[0];
		this.UserNameTextBox.Location = new System.Drawing.Point(159, 112);
		this.UserNameTextBox.MaxLength = 32767;
		this.UserNameTextBox.Name = "UserNameTextBox";
		this.UserNameTextBox.PasswordChar = '\0';
		this.UserNameTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.UserNameTextBox.SelectedText = "";
		this.UserNameTextBox.SelectionLength = 0;
		this.UserNameTextBox.SelectionStart = 0;
		this.UserNameTextBox.ShortcutsEnabled = true;
		this.UserNameTextBox.Size = new System.Drawing.Size(212, 23);
		this.UserNameTextBox.TabIndex = 1;
		this.UserNameTextBox.UseSelectable = true;
		this.UserNameTextBox.WaterMarkColor = System.Drawing.Color.FromArgb(109, 109, 109);
		this.UserNameTextBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
		this.PasswordTextBox.CustomButton.Image = null;
		this.PasswordTextBox.CustomButton.Location = new System.Drawing.Point(190, 1);
		this.PasswordTextBox.CustomButton.Name = "";
		this.PasswordTextBox.CustomButton.Size = new System.Drawing.Size(21, 21);
		this.PasswordTextBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
		this.PasswordTextBox.CustomButton.TabIndex = 1;
		this.PasswordTextBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
		this.PasswordTextBox.CustomButton.UseSelectable = true;
		this.PasswordTextBox.CustomButton.Visible = false;
		this.PasswordTextBox.Lines = new string[0];
		this.PasswordTextBox.Location = new System.Drawing.Point(159, 159);
		this.PasswordTextBox.MaxLength = 32767;
		this.PasswordTextBox.Name = "PasswordTextBox";
		this.PasswordTextBox.PasswordChar = '●';
		this.PasswordTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.PasswordTextBox.SelectedText = "";
		this.PasswordTextBox.SelectionLength = 0;
		this.PasswordTextBox.SelectionStart = 0;
		this.PasswordTextBox.ShortcutsEnabled = true;
		this.PasswordTextBox.Size = new System.Drawing.Size(212, 23);
		this.PasswordTextBox.TabIndex = 3;
		this.PasswordTextBox.UseSelectable = true;
		this.PasswordTextBox.UseSystemPasswordChar = true;
		this.PasswordTextBox.WaterMarkColor = System.Drawing.Color.FromArgb(109, 109, 109);
		this.PasswordTextBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
		this.metroLabel2.AutoSize = true;
		this.metroLabel2.Location = new System.Drawing.Point(82, 159);
		this.metroLabel2.Name = "metroLabel2";
		this.metroLabel2.Size = new System.Drawing.Size(40, 19);
		this.metroLabel2.TabIndex = 2;
		this.metroLabel2.Text = "密码:";
		this.loginButton.Location = new System.Drawing.Point(126, 230);
		this.loginButton.Name = "loginButton";
		this.loginButton.Size = new System.Drawing.Size(76, 42);
		this.loginButton.TabIndex = 4;
		this.loginButton.Text = "登录";
		this.loginButton.UseSelectable = true;
		this.loginButton.Click += new System.EventHandler(loginButton_Click);
		this.registerButton.Location = new System.Drawing.Point(265, 230);
		this.registerButton.Name = "registerButton";
		this.registerButton.Size = new System.Drawing.Size(76, 42);
		this.registerButton.TabIndex = 5;
		this.registerButton.Text = "注册";
		this.registerButton.UseSelectable = true;
		this.registerButton.Click += new System.EventHandler(registerButton_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(455, 319);
		base.Controls.Add(this.registerButton);
		base.Controls.Add(this.loginButton);
		base.Controls.Add(this.PasswordTextBox);
		base.Controls.Add(this.metroLabel2);
		base.Controls.Add(this.UserNameTextBox);
		base.Controls.Add(this.metroLabel1);
		this.MaximumSize = new System.Drawing.Size(455, 319);
		this.MinimumSize = new System.Drawing.Size(455, 319);
		base.Name = "Login";
		this.Text = "Login";
		base.Load += new System.EventHandler(Login_Load);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
