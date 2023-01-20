using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kysprime.NttModel;
using System.IO;
using System.Net;

namespace Kysprime
{
    public partial class frmMain : Form
    {
        db_a8ded2_emmenedurEntities db;
        public frmMain()
        {
            InitializeComponent();
            db = new db_a8ded2_emmenedurEntities();
        }
        List<tbl_kullanici> kullist;
        List<tbl_post> postlist;
        String MeUserName;
        private void frmMain_Load(object sender, EventArgs e)
        {
            pnlicerik.Visible = true;
            pnlgonderi.Visible = false;
            pnlfeedback.Visible = false;
            postlisten("");
            UserList("");
            MeUserName = lblname.Text;
            lblpostname.Text = MeUserName + " tarafından gönderiliyor.";
           

        }
        void HomeButton()
        {
            pnlicerik.Visible = true;
            pnlgonderi.Visible = false;
            pnlfeedback.Visible = false;
        }
        void FeedbackButton()
        {
            pnlicerik.Visible = false;
            pnlgonderi.Visible = false;
            pnlfeedback.Visible = true;
        }
        void PostButton()
        {
            pnlicerik.Visible = false;
            pnlgonderi.Visible = true;
            pnlfeedback.Visible = false;
        }
        private void pctBoxHome_Click(object sender, EventArgs e)
        {
            HomeButton();
            postlisten("");
            UserList("");
        }
      
        private void pctBoxaddpost_Click(object sender, EventArgs e)
        {
            PostButton(); 
            UserList("");
        }

        private void pctBoxfeedbox_Click(object sender, EventArgs e)
        {
            FeedbackButton();
            UserList("");
        }

       
        public void UserList(string values)
        {

            pnluser.Controls.Clear();
            int labels = 0;
            int lblsloc = 28;
            int pctloc = 20;
            if (values.Equals(""))
                kullist = db.tbl_kullanici.ToList();

            else
                kullist = db.tbl_kullanici.Where(x => x.isim.Contains(values)).ToList();
            foreach (var item in kullist)
            {
                
                Label lbl = new Label();
                lbl.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(162)));
                lbl.Location = new Point(68, lblsloc);
                lbl.Name = "lbluser" + labels;
                lbl.Size = new Size(127, 25);
                lbl.TabIndex = 2;
                lbl.Text = item.isim;

                PictureBox pct = new PictureBox();
                pct.Image = global::Kysprime.Properties.Resources.Cat_Profile_128px;
                pct.Location = new  Point(22, pctloc);
                pct.Name = "pctuser" +labels;
                pct.Size = new  Size(40, 40);
                pct.SizeMode =  PictureBoxSizeMode.Zoom;
                pct.TabIndex = 3;
                pct.TabStop = false;


                pnluser.Controls.Add(lbl);
                pnluser.Controls.Add(pct);
                lblsloc += 50;
                pctloc += 50;
                labels++;
            }
          

        }
        private int postekle(tbl_post post)
        {
            db.tbl_post.Add(post);
            db.SaveChanges();
            int id = db.tbl_post.ToList().Last().id;
            return id;
        }
        
        public void FtpSend(string values)
        {
            
            foreach (var item in listBox1.Items)
            {
                tbl_post post = new tbl_post();

                    FileInfo folderInfo = new FileInfo(@""+item);

                string ftpAdress = "ftp://" + "ftpServerName" + "/" + folderInfo.Name;

                FtpWebRequest ftpistek = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpAdress));

                ftpistek.Credentials = new NetworkCredential("ftpFileName", "password");
                ftpistek.KeepAlive = false;
                ftpistek.Method = WebRequestMethods.Ftp.UploadFile;
                ftpistek.UseBinary = true;
                ftpistek.ContentLength = folderInfo.Length;

                post.gonderen = MeUserName;
                post.cinsiyet = true;
                post.kalp = 0;

                if (values.Equals(""))
                    postlist = db.tbl_post.ToList();

                else
                    postlist = db.tbl_post.Where(x => x.gonderen.Contains(values)).ToList();
                foreach (var wa in postlist)
                {
                    if (listBox1.Items.Count > 1)
                    {
                        for (int i = 1; i < listBox1.Items.Count; i++)
                        {
                            post.posturl = (wa.id+1).ToString("0000000");
                        }
                    }
                }
               
              
                post.postimg = folderInfo.Name;
                post.sil_id = true;
                int postekle_id = postekle(post);
                db.SaveChanges();
                int bufferlenght = 2048;
                byte[] buff = new byte[10000000];
                int sayi;

                FileStream fs = folderInfo.OpenRead();
                Stream str = ftpistek.GetRequestStream();
                sayi = fs.Read(buff, 0, bufferlenght);

                while (sayi != 0)
                {
                    str.Write(buff, 0, sayi);
                    sayi = fs.Read(buff, 0, bufferlenght);
                }

                str.Close();
                fs.Close();

            }

            HomeButton();

        }
      
        private void btnFileSelect_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            OFDfileselect.Filter = "Png Dosyası |*.png| Jpeg Dosyası|*.jpg |Tüm Dosyalar (*.*)|*.*";
            OFDfileselect.RestoreDirectory = true;
            OFDfileselect.CheckFileExists = false;
            OFDfileselect.Title = "PNG yada Jpeg Dosyası Seçiniz..";
            OFDfileselect.Multiselect = true;
            OFDfileselect.ShowDialog();
            int f = 0;
            int pctploc=34;
            pnlpostView.Controls.Clear();
            foreach (string file in OFDfileselect.FileNames)
                {
               
                PictureBox pctp = new PictureBox();
                pctp.BackColor = Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
                pctp.BorderStyle = BorderStyle.FixedSingle;
                pctp.Image = Image.FromFile(file);
                listBox1.Items.Add(file);
                pctp.Location = new Point(pctploc, 16);
                pctp.Name = "pctBoxpostWiew" + f;
                pctp.Size = new Size(200, 200);
                pctp.SizeMode = PictureBoxSizeMode.Zoom;
                pctp.TabIndex = 1;
                pctp.TabStop = false;
                pnlpostView.Controls.Add(pctp);
                pctploc += 280;
                f++;
                
            }
          
        }

        private void btnpostshare_Click(object sender, EventArgs e)
        {
            FtpSend("");
            postlisten("");
        }

        private int feedbackadd(tbl_geribidirim feed)
        {
            db.tbl_geribidirim.Add(feed);
            db.SaveChanges();
            int id = db.tbl_geribidirim.ToList().Last().id;
            return id;
        }
        private void btnfeedbacksend_Click(object sender, EventArgs e)
        {
            
            tbl_geribidirim feed = new tbl_geribidirim();
            feed.gonderen = MeUserName;
            feed.metin = txttext.Text;
            feed.konu = txtkonu.Text;
            int feedbackadd_id = feedbackadd(feed);
            db.SaveChanges();
            HomeButton();
        }

        PictureBox[] picturepost = new PictureBox[1];
        Button[] buttonpost = new Button[1];
        Label[] labelpost = new Label[1];
        Panel[] panelpost = new Panel[1];
        Label[] labelpostheart = new Label[1];

        public void postlisten(string values)
        {
            int p=0;
            int pnlloc = 12;

            if (values.Equals(""))
                postlist = db.tbl_post.ToList();

            else
                postlist = db.tbl_post.Where(x => x.gonderen.Contains(values)).ToList();
            foreach (var item in postlist)
            {
                panelpost[p] = new Panel();

                labelpost[p] = new Label();

                labelpostheart[p] = new Label();

                picturepost[p] = new PictureBox();

                buttonpost[p] = new Button();

                panelpost[p].BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
                panelpost[p].Location = new Point(101, pnlloc);
                panelpost[p].Name = "pnlpost" + item.posturl;
                panelpost[p].Size = new Size(550, 327);
                panelpost[p].TabIndex = 2;

                pnlicerik.Controls.Add(panelpost[p]);

                picturepost[p].BackColor = Color.Black;
                picturepost[p].BorderStyle = BorderStyle.FixedSingle;
                picturepost[p].ImageLocation = "http://emmenedur-001-site1.itempurl.com/images/" + item.postimg;
                picturepost[p].Location = new Point(217, 11);
                picturepost[p].Name = "pctpost"+item.posturl;
                picturepost[p].Size = new Size(321, 304);
                picturepost[p].SizeMode = PictureBoxSizeMode.Zoom;
                picturepost[p].TabIndex = 1;
                picturepost[p].TabStop = false;

                panelpost[p].Controls.Add(picturepost[p]);

                buttonpost[p].BackColor = Color.Transparent;
                buttonpost[p].BackgroundImage = global::Kysprime.Properties.Resources.HeartWithArrow_128px;
                buttonpost[p].BackgroundImageLayout = ImageLayout.Zoom;
                buttonpost[p].Cursor = Cursors.Default;
                buttonpost[p].FlatStyle = FlatStyle.Popup;
                buttonpost[p].ForeColor = SystemColors.ControlText;
                buttonpost[p].Location = new Point(74, 199);
                buttonpost[p].Name = "btnkalp"+item.posturl;
                buttonpost[p].Size = new Size(75, 75);
                buttonpost[p].TabIndex = 3;
                buttonpost[p].UseVisualStyleBackColor = false;
                buttonpost[p].Click += new EventHandler(btnpostheart_Click);
                panelpost[p].Controls.Add(buttonpost[p]);

                labelpost[p].AutoSize = true;
                labelpost[p].Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(162)));
                labelpost[p].ForeColor = Color.Gold;
                labelpost[p].Location = new Point(13, 11);
                labelpost[p].Name = "lblpost"+item.posturl;
                labelpost[p].Size = new Size(90, 25);
                labelpost[p].TabIndex = 2;
                labelpost[p].Text = item.gonderen;
                panelpost[p].Controls.Add(labelpost[p]);

                labelpostheart[p].AutoSize = true;
                labelpostheart[p].Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular,GraphicsUnit.Point, ((byte)(162)));
                labelpostheart[p].ForeColor = Color.Gold;
                labelpostheart[p].Location = new Point(99, 286);
                labelpostheart[p].Name = "lblheart"+item.posturl;
                labelpostheart[p].Size = new Size(29, 16);
                labelpostheart[p].TabIndex = 4;
                labelpostheart[p].Text = item.kalp.ToString();
                labelpostheart[p].TextAlign =ContentAlignment.MiddleCenter;
                panelpost[p].Controls.Add(labelpostheart[p]);
                Array.Resize(ref panelpost, p + 1);
                Array.Resize(ref picturepost, p + 1);
                Array.Resize(ref buttonpost, p + 1);
                Array.Resize(ref labelpost, p + 1);
                Array.Resize(ref labelpostheart, p + 1);
                pnlloc += 350;
            }
          

        }
        private void btnpostheart_Click(object sender, EventArgs e)
        {
            string posturl = (sender as Button).Name.Substring(7);
            heart("",posturl);
          
           

         }
        void heart(string values,string posturl)
        {
            if (values.Equals(""))
                postlist = db.tbl_post.ToList();

            else
                postlist = db.tbl_post.Where(x => x.gonderen.Contains(values)).ToList();
            foreach (var item in postlist)
            {
                if (item.posturl.Equals(posturl))
                {
                    item.kalp++;
                }
                db.SaveChanges();
            }
        }

    }
}
