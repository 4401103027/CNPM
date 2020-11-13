using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;

namespace QuanLyCongVan
{
    public partial class FormCVDI : Form
    {
        public FormCVDI()
        {
            InitializeComponent();
        }

        private void FormCVDI_Load(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source = NGUYENNGOCBAOTR\SQLEXPRESS; Initial Catalog = QLCV; Integrated Security = True");
            con.Open();
            var cmd = new SqlCommand("select * from BOPHAN", con);
            var dr = cmd.ExecuteReader();

            var dtbp = new DataTable();
            var dtcq = new DataTable();
            var dtbm = new DataTable();
            var dtloaicv = new DataTable();

            dtbp.Load(dr);
            cbMaBP.DisplayMember = "MABP";
            cbMaBP.DataSource = dtbp;
            
            cmd = new SqlCommand("select * from COQUAN", con);
            dr = cmd.ExecuteReader();
            dtcq.Load(dr);
            cbMaCQ.DisplayMember = "MACQ";
            cbMaCQ.DataSource = dtcq;

            cmd = new SqlCommand("select * from LOAIBM", con);
            dr = cmd.ExecuteReader();
            dtbm.Load(dr);
            cbMaBM.DisplayMember = "MALOAIBM";
            cbMaBM.DataSource = dtbm;

            cmd = new SqlCommand("select * from LOAICV", con);
            dr = cmd.ExecuteReader();
            dtloaicv.Load(dr);
            cbLoaiCV.DisplayMember = "MALOAICV";
            cbLoaiCV.DataSource = dtloaicv;

            con.Close();
        }

        private void Cancel()
        {
            txtMaCV.Text = "";
            txtNguoiKy.Text = "";
            txtTenCV.Text = "";
            txtTrichYeu.Text = "";
            dtpNgayGui.Refresh();
            dtpNgayKy.Refresh();
            cbLoaiCV.Refresh();
            cbMaBP.Refresh();
            cbMaBP.Refresh();
            cbMaCQ.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=NGUYENNGOCBAOTR\SQLEXPRESS;Initial Catalog=QLCV;Integrated Security=True");
            string SOCV, LOAICV, NGAYKY, NGAYGUI, TRICHYEU, DOBM, MABP, MACQ, TENCV, NGUOIKY;

            SOCV = txtMaCV.Text;
            MABP = cbMaBP.Text;
            LOAICV = cbLoaiCV.Text;
            NGAYKY = dtpNgayKy.Text;
            NGAYGUI = dtpNgayGui.Text;
            TRICHYEU = txtTrichYeu.Text;
            DOBM = cbMaBM.Text;
            MACQ = cbMaCQ.Text;
            TENCV = txtTenCV.Text;
            NGUOIKY = txtNguoiKy.Text;

            try
            {
                conn.Open();
                string sql = "insert into CVDI values ('" + SOCV + "', '" + LOAICV + "', '" + DOBM + "', '" + MABP + "', '" + MACQ + "', N'" + TENCV + "', N'" + TRICHYEU + "', '" + NGAYGUI + "', '" + NGAYKY + "', N'" + NGUOIKY + "')";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                int kq = cmd.ExecuteNonQuery();
                if (kq > 0) MessageBox.Show("Thêm thành công");
                else MessageBox.Show("Thêm thất bại");
                conn.Close();
                Cancel();
            }
            catch {
                conn.Close();
                MessageBox.Show("Lỗi thêm dữ liệu");
            }
           
        }

        private void cbMaBM_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView rowView = (DataRowView)cbMaBM.SelectedItem;
            txtBM.Text = rowView.Row["TENLOAIBM"].ToString();
        }

        private void cbMaBP_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView rowView = (DataRowView)cbMaBP.SelectedItem;
            txtBP.Text = rowView.Row["TENBP"].ToString();
        }

        private void cbMaCQ_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView rowView = (DataRowView)cbMaCQ.SelectedItem;
            txtCQ.Text = rowView.Row["TENCQ"].ToString();
        }

        private void cbLoaiCV_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView rowView = (DataRowView)cbLoaiCV.SelectedItem;
            txtLoaiCV.Text = rowView.Row["TENLOAI"].ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        private void FormCVDI_FormClosed(object sender, FormClosedEventArgs e)
        {
                Thread th;
                th = new Thread(openformChinh);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
        }

        private void openformChinh(object obj)
        {
            Application.Run(new FormChinh());
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn thực sự muốn thoát?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                Thread th;
                this.Close();
                th = new Thread(openformChinh);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
        }
    }
}

