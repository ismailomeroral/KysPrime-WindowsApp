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
namespace Kysprime
{
    public partial class frmlogin : Form
    {
        db_a8ded2_emmenedurEntities db;
        public frmlogin()
        {
            InitializeComponent();
            db = new db_a8ded2_emmenedurEntities();
        }
        List<tbl_kullanici> kullist;
        private void btndevam_Click(object sender, EventArgs e)
        {
            Login("");
        }
        public void Login(string values)
        {
            String getUserName = txtkullaniciadi.Text;
            String getPassword = txtkullanicisifresi.Text;

            if (txtkullanicisifresi.Text.Equals("") || txtkullaniciadi.Text.Equals(""))
                MessageBox.Show("KullanıcıAdı veya Şifre Boş Geçilemez");
            else{
                if (values.Equals(""))
                    kullist = db.tbl_kullanici.ToList();

                else
                    kullist = db.tbl_kullanici.Where(x => x.isim.Contains(values)).ToList();
                foreach (var item in kullist)
                {

                    if (getUserName.Equals(item.isim) && getPassword.Equals(item.sifre))
                    {
                      
                        if (item.sil_id.Equals(true))
                        {

                            frmMain frm = new frmMain();
                            frm.lblname.Text = getUserName;
                            frm.Show();
                            Hide();
                            txtkullaniciadi.Text = "";
                            txtkullanicisifresi.Text = "";
                        }
                        else if (item.sil_id.Equals(false))
                        {
                            txtkullaniciadi.Text = "";
                            txtkullanicisifresi.Text = "";
                            MessageBox.Show("Hesabınız Kapatılmıştır");
                        }


                    }
                 

                }



            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmadmin fra = new frmadmin();
            fra.Show();
        }
    }
}
