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
    public partial class frmadmin : Form
    {
        public frmadmin()
        {
            InitializeComponent();
        }

        db_a8ded2_emmenedurEntities db = new db_a8ded2_emmenedurEntities();
        private void btnekle_Click(object sender, EventArgs e)
        {
            kayit();

        }
        private int kullanıcıekle(tbl_kullanici kull)
        {
            db.tbl_kullanici.Add(kull);
            db.SaveChanges();
            int id = db.tbl_kullanici.ToList().Last().id;
            return id;
        }
        public void kayit()
        {
            tbl_kullanici kull = new tbl_kullanici();
            kull.isim = txtusername.Text;
            kull.sifre = txtpasword.Text;
            kull.cevrim = false;
            kull.sil_id = true;
            int kullanıcıekle_id = kullanıcıekle(kull);
            db.SaveChanges();
        }
    }
}
