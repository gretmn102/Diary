module Gui

let monthCalendar1 = new System.Windows.Forms.MonthCalendar()
let dateTimePicker1 = new System.Windows.Forms.DateTimePicker()
let listView1 = new System.Windows.Forms.ListView()
let chHour  = new System.Windows.Forms.ColumnHeader()
let chMinute = new System.Windows.Forms.ColumnHeader()
let chValue = new System.Windows.Forms.ColumnHeader()
let btnAddNote = new System.Windows.Forms.Button()
let textBox1 = new System.Windows.Forms.TextBox()
let btnDelNote = new System.Windows.Forms.Button()

let pathDb = "Database.db"
if not <| System.IO.File.Exists pathDb then
    sprintf "Database not found at '%s' so it's been create" pathDb
    |> System.Windows.Forms.MessageBox.Show |> ignore
    
    CoreDiary.createDB pathDb

let db : CoreDiary.Date = new CoreDiary.Date(pathDb)
let GetTimeFormat(hour,min) =
    System.String.Format("{0,2:D2}:{1,2:D2}", hour, min)
let ViewDayNotes() = 
    db.SaveHor()
    let items = listView1.Items
    items.Clear()
    let xs = db.GetDayNotes(monthCalendar1.SelectionStart)
    // let lst = new System.Windows.Forms.ListViewItem()
    let xs2 =
        xs |> Array.map (fun (_, xs) ->
            let lst = new System.Windows.Forms.ListViewItem(xs, -1)
            lst.Name <- GetTimeFormat(xs.[0], xs.[1])
            lst
        )
    items.AddRange(xs2)

ViewDayNotes()

let monthCalendar1_DateChanged _ _ = ViewDayNotes()
let Form1_FormClosed _ _ = db.SaveHor(); db.SaveDB()

let listView1_SelectedIndexChanged _ _ =
    let s = listView1.SelectedItems
    if s.Count = 0 then printfn "возможно выделить ничто"
    else
        let sub = s.[0].SubItems

        let hour = System.Int32.Parse(sub.[0].Text)
        let min = System.Int32.Parse(sub.[1].Text)
        let note = sub.[2].Text
        let date = monthCalendar1.SelectionStart;

        dateTimePicker1.Value <- new System.DateTime(date.Year, date.Month, date.Day, hour, min, 0);
        textBox1.Text <- note
let btnDelNote_Click _ _ =
    let time = dateTimePicker1.Value
    db.RemoveNote(time)
    let hour = time.Hour.ToString()
    let min = time.Minute.ToString()
    let name = GetTimeFormat(hour, min)
    let res = listView1.Items.Find(name, false)
    if res.Length = 0 then ()
    else res.[0].Remove()
let btnAddNote_Click _ _ =
    let time = dateTimePicker1.Value
    let hour = time.Hour.ToString()
    let min = time.Minute.ToString()
    let name = GetTimeFormat(hour, min)
    let note = textBox1.Text

    db.AddToday dateTimePicker1.Value note
    let res = listView1.Items.Find(name, false)
    if res.Length = 0 then
        let item = new System.Windows.Forms.ListViewItem([| hour; min; note |], -1)
        item.Name <- name
        listView1.Items.Add(item) |> ignore
    else
        let sub = res.[0].SubItems
        sub.[2].Text <- note

type ListViewItemComparer() =
    interface System.Collections.IComparer with 
        member __.Compare(x, y) = 
            let x, y = x :?> System.Windows.Forms.ListViewItem, y :?> System.Windows.Forms.ListViewItem
            System.String.Compare(x.Name, y.Name)
listView1.ListViewItemSorter <- new ListViewItemComparer()    

// class ListViewItemComparer : System.Collections.IComparer
// {
//     private int col;
//     public ListViewItemComparer()
//     {
//         col = 0;
//     }
//     public ListViewItemComparer(int column)
//     {
//         col = column;
//     }
//     public int Compare(object x, object y)
//     {
//         //return String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
//         return String.Compare(((ListViewItem)x).Name, ((ListViewItem)y).Name);
//     }
// }

// SuspendLayout()
// 
// monthCalendar1
// 
monthCalendar1.Location <- new System.Drawing.Point(18, 9)
monthCalendar1.MaxDate <- new System.DateTime(2036, 12, 31, 0, 0, 0, 0)
monthCalendar1.MaxSelectionCount <- 1
monthCalendar1.MinDate <- new System.DateTime(2000, 1, 1, 0, 0, 0, 0)
monthCalendar1.Name <- "monthCalendar1"
monthCalendar1.TabIndex <- 0
monthCalendar1.DateChanged.AddHandler (new System.Windows.Forms.DateRangeEventHandler(monthCalendar1_DateChanged))
// 
// dateTimePicker1
// 
dateTimePicker1.CustomFormat <- "HH:mm"
dateTimePicker1.Format <- System.Windows.Forms.DateTimePickerFormat.Custom
dateTimePicker1.Location <- new System.Drawing.Point(18, 184)
dateTimePicker1.Name <- "dateTimePicker1"
dateTimePicker1.ShowUpDown <- true
dateTimePicker1.Size <- new System.Drawing.Size(49, 20)
dateTimePicker1.TabIndex <- 2
// 
// listView1
// 
listView1.Columns.AddRange([| chHour; chMinute; chValue |])
listView1.FullRowSelect <- true
listView1.GridLines <- true
listView1.Location <- new System.Drawing.Point(199, 22)
listView1.Name <- "listView1"
listView1.Size <- new System.Drawing.Size(275, 182)
listView1.TabIndex <- 1
listView1.UseCompatibleStateImageBehavior <- false
listView1.View <- System.Windows.Forms.View.Details
listView1.SelectedIndexChanged.AddHandler(new System.EventHandler(listView1_SelectedIndexChanged))
// 
// chHour
// 
chHour.Text <- "Hour"
chHour.Width <- 56
// 
// chMinute
// 
chMinute.Text <- "Minute"
// 
// chValue
// 
chValue.Text <- "Value"
chValue.Width <- 67
// 
// btnAddNote
// 
btnAddNote.Location <- new System.Drawing.Point(73, 181)
btnAddNote.Name <- "btnAddNote"
btnAddNote.Size <- new System.Drawing.Size(53, 23)
btnAddNote.TabIndex <- 3
btnAddNote.Text <- "set"
btnAddNote.UseVisualStyleBackColor <- true
btnAddNote.Click.AddHandler(new System.EventHandler(btnAddNote_Click))
// 
// textBox1
// 
textBox1.Location <- new System.Drawing.Point(18, 221)
textBox1.Multiline <- true
textBox1.Name <- "textBox1"
textBox1.ScrollBars <- System.Windows.Forms.ScrollBars.Both
textBox1.Size <- new System.Drawing.Size(456, 120)
textBox1.TabIndex <- 5
// 
// btnDelNote
// 
btnDelNote.Location <- new System.Drawing.Point(132, 181)
btnDelNote.Name <- "btnDelNote"
btnDelNote.Size <- new System.Drawing.Size(61, 23)
btnDelNote.TabIndex <- 4
btnDelNote.Text <- "Del"
btnDelNote.UseVisualStyleBackColor <- true
btnDelNote.Click.AddHandler( new System.EventHandler(btnDelNote_Click))
// 
// Form1
// 
let form = new System.Windows.Forms.Form()
form.AutoScaleDimensions <- new System.Drawing.SizeF(6.f, 13.f)
form.AutoScaleMode <- System.Windows.Forms.AutoScaleMode.Font
form.ClientSize <- new System.Drawing.Size(517, 373)
form.Controls.Add(btnDelNote)
form.Controls.Add(textBox1)
form.Controls.Add(btnAddNote)
form.Controls.Add(listView1)
form.Controls.Add(dateTimePicker1)
form.Controls.Add(monthCalendar1)
form.Name <- "Form1"
form.Text <- "Form1"
form.FormClosed.AddHandler(new System.Windows.Forms.FormClosedEventHandler(Form1_FormClosed))
form.ResumeLayout(false)
form.PerformLayout()

[<EntryPoint;System.STAThread>]
let main _ =
    System.Windows.Forms.Application.EnableVisualStyles()
    // System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false)
    System.Windows.Forms.Application.Run(form)
    0