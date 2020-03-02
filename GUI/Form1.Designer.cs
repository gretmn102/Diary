using System.Windows.Forms;
namespace GUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.listView1 = new System.Windows.Forms.ListView();
            this.chHour = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chMinute = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAddNote = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnDelNote = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Location = new System.Drawing.Point(18, 9);
            this.monthCalendar1.MaxDate = new System.DateTime(2036, 12, 31, 0, 0, 0, 0);
            this.monthCalendar1.MaxSelectionCount = 1;
            this.monthCalendar1.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 0;
            this.monthCalendar1.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar1_DateChanged);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "HH:mm";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(18, 184);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.ShowUpDown = true;
            this.dateTimePicker1.Size = new System.Drawing.Size(49, 20);
            this.dateTimePicker1.TabIndex = 1;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chHour,
            this.chMinute,
            this.chValue});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(199, 22);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(275, 182);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // chHour
            // 
            this.chHour.Text = "Hour";
            this.chHour.Width = 56;
            // 
            // chMinute
            // 
            this.chMinute.Text = "Minute";
            // 
            // chValue
            // 
            this.chValue.Text = "Value";
            this.chValue.Width = 67;
            // 
            // btnAddNote
            // 
            this.btnAddNote.Location = new System.Drawing.Point(73, 181);
            this.btnAddNote.Name = "btnAddNote";
            this.btnAddNote.Size = new System.Drawing.Size(53, 23);
            this.btnAddNote.TabIndex = 4;
            this.btnAddNote.Text = "set";
            this.btnAddNote.UseVisualStyleBackColor = true;
            this.btnAddNote.Click += new System.EventHandler(this.btnAddNote_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(18, 221);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(456, 120);
            this.textBox1.TabIndex = 5;
            // 
            // btnDelNote
            // 
            this.btnDelNote.Location = new System.Drawing.Point(132, 181);
            this.btnDelNote.Name = "btnDelNote";
            this.btnDelNote.Size = new System.Drawing.Size(61, 23);
            this.btnDelNote.TabIndex = 6;
            this.btnDelNote.Text = "Del";
            this.btnDelNote.UseVisualStyleBackColor = true;
            this.btnDelNote.Click += new System.EventHandler(this.btnDelNote_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 373);
            this.Controls.Add(this.btnDelNote);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnAddNote);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.monthCalendar1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MonthCalendar monthCalendar1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.ListView listView1;
        private ColumnHeader chHour;
        private ColumnHeader chValue;
        private Button btnAddNote;
        private TextBox textBox1;
        private ColumnHeader chMinute;
        private Button btnDelNote;
    }
}

