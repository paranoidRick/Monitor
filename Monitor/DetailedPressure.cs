using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Monitor;

public class DetailedPressure : Form
{
	private IContainer components;

	private TextBox textBox1;

	private Timer timer1;

	public DetailedPressure()
	{
		InitializeComponent();
	}

	private void DetailedPressure_Load(object sender, EventArgs e)
	{
	}

	private void timer1_Tick(object sender, EventArgs e)
	{
		try
		{
			MainForm mainForm = (MainForm)base.Owner;
			textBox1.Text = mainForm.Controls["metroTextBox1"].Text;
		}
		catch
		{
			Console.WriteLine("读取错误");
		}
	}

	private void DetailedPressure_FormClosing(object sender, FormClosingEventArgs e)
	{
		timer1.Stop();
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
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		base.SuspendLayout();
		this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.textBox1.Location = new System.Drawing.Point(0, 0);
		this.textBox1.Multiline = true;
		this.textBox1.Name = "textBox1";
		this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
		this.textBox1.Size = new System.Drawing.Size(1345, 626);
		this.textBox1.TabIndex = 0;
		this.timer1.Enabled = true;
		this.timer1.Tick += new System.EventHandler(timer1_Tick);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1345, 626);
		base.Controls.Add(this.textBox1);
		base.Name = "DetailedPressure";
		this.Text = "实时压力数据";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(DetailedPressure_FormClosing);
		base.Load += new System.EventHandler(DetailedPressure_Load);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
