using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using vtuber.Properties;

namespace vtuber
{
    public partial class Form1 : Form
    {

        class Architecture
        {
            public Panel panel;
            public PictureBox picturebox;
            public Label LABEL;
            public FlowLayoutPanel flow1;
            public Form from;
            public Label name;
            public Architecture(FlowLayoutPanel flow, Form form)
            {
                panel = new Panel();
                picturebox = new PictureBox();
                LABEL = new Label();
                name = new Label();
                ///////////
                panel.Size = new Size(201, 265);
                // panel.Location = now_panel.Location;
                //////////
                picturebox.Size = new Size(161, 129);
                picturebox.Location = new Point(16, 1);
                /////////
                LABEL.Size = new Size(50, 18);
                LABEL.Location = new Point(9, 133);
                LABEL.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
                LABEL.TabIndex = 1;
                ///////// 
                name.Size = new Size(50, 18);
                name.Location = new Point(9, 166);
                name.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
                name.TabIndex = 1;

                LABEL.AutoSize = true;
                name.AutoSize = true;

                panel.BackColor = Color.White;
                panel.Controls.Add(picturebox);
                panel.Controls.Add(name);
                panel.Controls.Add(LABEL);
                flow.Controls.Add(panel);
                form.Controls.Add(flow);
                flow1 = flow;
                this.from = from;
                picturebox.Click += new System.EventHandler(this.pictureBox1_Click);
            }

            private void pictureBox1_Click(object sender, EventArgs e)
            {
                PictureBox x = (PictureBox)sender;
                System.Diagnostics.Process.Start(x.Name.ToString());
            }

            public void set_up(Live_hollo_vtube live)
            {
                picturebox.Image = Image.FromStream(System.Net.WebRequest.Create(live.jpg).GetResponse().GetResponseStream());
                picturebox.SizeMode = PictureBoxSizeMode.StretchImage;


                LABEL.Text = (live.Live_Time.Equals("直播中~~") == true) ? "直播中" : "直播時間:" + live.Live_Time.ToString();
                string NAME = live.name;
                int i1 = 15;
                for (int i = 0; i < NAME.Length / 15; i++)
                {
                    NAME = NAME.Insert(i1, "\n");
                    i1 = i1 + 15;
                }
                picturebox.Name = live.inter;
                name.Text = NAME;
            }

        }
        List<Live_hollo_vtube> v_tuber_total = new List<Live_hollo_vtube>();
        private delegate void DelShowMessage(FlowLayoutPanel flowLayout_panel, List<Live_hollo_vtube> x);
        private void AddMessage(FlowLayoutPanel flowLayout_panel, List<Live_hollo_vtube> x)
        {
            if (this.InvokeRequired) // 若非同執行緒
            {
                DelShowMessage del = new DelShowMessage(AddMessage); //利用委派執行
                object[] vs=new object[] { flowLayout_panel , x };
                this.Invoke(del, vs);
            }
            else // 同執行緒
            {
                flowLayout_panel.Controls.Clear();

                foreach (var i in x)
                {
                    Architecture tt = new Architecture(flowLayout_panel, this);
                    tt.set_up(i);
                }

            }
        }



        public Form1()
        {
            InitializeComponent();
        }

        List<Live_hollo_vtube> get_vtuber(string html)
        {

            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(html);
            List<string> jpg_vtuber = new List<string>();
            List<string> hash_vtuber = new List<string>();
            List<string> time_vtuber = new List<string>();
            List<string> www = new List<string>();
            List<Live_hollo_vtube> Live_Hollo_Vtubes = new List<Live_hollo_vtube>();
            string name_1 = "";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach (var i in htmlDoc.DocumentNode.SelectNodes("//script"))
            {

                if (i.InnerText.Contains("ownerText") == true)
                {
                    string[] time = i.InnerText.Split('"');
                    for (int j = 0; j < time.Length; j++)
                    {
                        if (time[j] == "ownerText")
                            name_1 = time[j + 6];
                        break;
                    }

                }

                if (i.InnerText.Contains(".jpg") == true)
                {

                    if (i.InnerText.Contains("正在觀看") == false && i.InnerText.Contains("startTime") == false)
                    {
                        break;
                    }

                    /////////////////////////////////////////////////////////////////////////////////////////////
                    int position = 0, j = 0;
                    foreach (var i1 in i.InnerText.Split('"'))
                    {

                        if (i1.Contains(".jpg") == true && -1 == jpg_vtuber.IndexOf(i1))
                        {
                            string[] hash = i1.Split('/');
                            hash[4] = "https://www.youtube.com/embed/" + hash[4];//4代表hash的地方
                            if (-1 == hash_vtuber.IndexOf(hash[4]))
                            {
                                hash_vtuber.Add(hash[4]);
                                jpg_vtuber.Add(i1);
                            }

                        }
                        if (jpg_vtuber.Count > 10)
                        {
                            break;
                        }

                        if (i1.Contains("startTime") == true && i1.Contains("startTimeSeconds") == false)
                        {
                            string[] time = i.InnerText.Split('"');
                            time_vtuber.Add(time[position + 2]);
                            int X = jpg_vtuber.Count - 1;
                            Live_Hollo_Vtubes.Add(new Live_hollo_vtube(jpg_vtuber[X], hash_vtuber[X], time[position + 2]));
                        }
                        if (i1.Contains("正在觀看") && j == 0)
                        {
                            Live_Hollo_Vtubes.Add(new Live_hollo_vtube(jpg_vtuber[0], hash_vtuber[0], ""));
                            j++;
                        }

                        position++;
                    }
                }

            }
            for (int i = 0; i < Live_Hollo_Vtubes.Count; i++)
            {
                for (int j = i + 1; j < Live_Hollo_Vtubes.Count; j++)
                {
                    if (Live_Hollo_Vtubes[i].jpg == Live_Hollo_Vtubes[j].jpg)
                    {
                        Live_Hollo_Vtubes.Remove(Live_Hollo_Vtubes[i]);
                        break;
                    }
                }
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////  
            foreach (var i in Live_Hollo_Vtubes)
            {
                string html1 = i.inter;
                HtmlWeb web1 = new HtmlWeb();
                var htmlDoc1 = web1.Load(html1);
                foreach (var i1 in htmlDoc1.DocumentNode.SelectNodes("//script"))
                {
                    int test = 0;
                    int number = 0;
                    if (i1.InnerText.Contains("\"text\\\""))
                    {
                        foreach (var i2 in i1.InnerText.Split('"'))
                        {
                            if (i2.Contains("text") == true)
                            {
                                if (number == 6)
                                {
                                    string[] vs = i1.InnerText.Split('"');
                                    i.name = vs[test + 2].Remove(vs[test + 2].Length - 1);
                                    break;
                                }
                                number++;
                            }
                            test++;
                        }

                    }
                }
            }//擷取名稱

            foreach (var i in Live_Hollo_Vtubes)
            {
                i.vt_name = name_1;
                string[] hal = i.inter.Split('/');
                i.inter = "https://www.youtube.com/watch?v=" + hal[hal.Length - 1] + "&ab_channel=" + name_1;
            }

            stopwatch.Stop();

            return Live_Hollo_Vtubes;
        }



        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            List<Live_hollo_vtube> hollo_undone;
            v_tuber_total.Clear();
            var sr = new StreamReader("vbuter_網址.txt");
            string vtuber_txt = sr.ReadToEnd();
            sr.Close();
            char[] t = new char[] { '\r', ' ', '\n' };
            string[] vtuber_html = vtuber_txt.Split(t);
            for (int i = 0; i < vtuber_html.Length; i = i + 1)
            {
                if (vtuber_html[i].Length > 10)
                {
                    hollo_undone = get_vtuber(vtuber_html[i]);
                    foreach (var j in hollo_undone)
                    {

                        v_tuber_total.Add(j);
                        //v_tuber_total[v_tuber_total.Count - 1].Live_Time = j.Live_Time;
                    }
                }
            }
            
            AddMessage(flowLayoutPanel1, v_tuber_total);
            //flowLayoutPanel1.Controls.Clear();
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            List<Live_hollo_vtube> hollo_undone;
            var sr = new StreamReader("vbuter_網址.txt");
            string vtuber_txt = sr.ReadToEnd();
            sr.Close();
            char[] t = new char[] { '\r', ' ', '\n' };
            string[] vtuber_html = vtuber_txt.Split(t);
            for (int i = 0; i < vtuber_html.Length; i = i + 1)
            {
                if (vtuber_html[i].Length > 10)
                {
                    hollo_undone = get_vtuber(vtuber_html[i]);
                    foreach (var j in hollo_undone)
                    {
                        Architecture tt = new Architecture(flowLayoutPanel1, this);
                        tt.set_up(j);
                        v_tuber_total.Add(j);
                        //v_tuber_total[v_tuber_total.Count - 1].Live_Time = j.Live_Time;
                    }
                }
            }
        }
    }
}
