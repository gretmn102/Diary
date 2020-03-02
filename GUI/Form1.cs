using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GUI
{

    public partial class Form1 : Form
    {
        string pathDB = "dataBase.db";
        CoreDiary.Date db;
        public Form1()
        {
            InitializeComponent();
            
            if (!System.IO.File.Exists(pathDB)) CoreDiary.createDB(pathDB);
            db = new CoreDiary.Date(pathDB);
            ViewDayNotes();
            this.listView1.ListViewItemSorter = new ListViewItemComparer();
        }

        //string GetFormatTime()
        //{

        //}

        private void btnAddNote_Click(object sender, EventArgs e)
        {
            
            DateTime time = dateTimePicker1.Value;
            var hour = time.Hour.ToString();
            var min = time.Minute.ToString();
            string name = GetTimeFormat(hour, min);
            string note = this.textBox1.Text;

            db.AddToday(dateTimePicker1.Value, note);
            var res = this.listView1.Items.Find(name, false);
            if (res.Length == 0)
            {
                var item = new System.Windows.Forms.ListViewItem(new[] { hour, min, note }, -1);
                item.Name = name;
                this.listView1.Items.Add(item);
            }
            else
            {
                var sub = res[0].SubItems;
                sub[2].Text = note;
            }
        }

        private string GetTimeFormat(string hour, string min)
        {
            return String.Format("{0,2:D2}:{1,2:D2}", hour, min);
        }

        void ViewDayNotes()
        {
            db.SaveHor();

            var items = this.listView1.Items;

            items.Clear();

            var xs = db.GetDayNotes(this.monthCalendar1.SelectionStart);
            ListViewItem lst = new ListViewItem();
            var xs2 = xs.Select(x => { lst = new ListViewItem(x.Item2, -1);
                                       lst.Name = GetTimeFormat(x.Item2[0], x.Item2[1]);
                                       return lst;
            });

            items.AddRange(xs2.ToArray());
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            ViewDayNotes();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            db.SaveHor();
            db.SaveDB();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = listView1.SelectedItems;
            if (s.Count == 0) { return; }
            //System.Windows.Forms.ListViewItem.ListViewSubItemCollection sub;
            var sub = s[0].SubItems;

            var hour = int.Parse(sub[0].Text);
            var min = int.Parse(sub[1].Text);
            var note = sub[2].Text;
            var date = this.monthCalendar1.SelectionStart;

            this.dateTimePicker1.Value = new DateTime(date.Year, date.Month, date.Day, hour, min, 0);
            this.textBox1.Text = note;
        }

        private void btnDelNote_Click(object sender, EventArgs e)
        {
            DateTime time = dateTimePicker1.Value;
            db.RemoveNote(time);
            string hour = time.Hour.ToString();
            string min = time.Minute.ToString();
            string name = GetTimeFormat(hour, min);
            var res = this.listView1.Items.Find(name, false);
            if (res.Length == 0)
            {
                
                //return;
            }
            else
            {
                res[0].Remove();
                //var sub = res[0].SubItems;
                //sub[2].Text = note;
            }
        }
    }
    class ListViewItemComparer : System.Collections.IComparer
    {
        private int col;
        public ListViewItemComparer()
        {
            col = 0;
        }
        public ListViewItemComparer(int column)
        {
            col = column;
        }
        public int Compare(object x, object y)
        {
            //return String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
            return String.Compare(((ListViewItem)x).Name, ((ListViewItem)y).Name);
        }
    }
}
