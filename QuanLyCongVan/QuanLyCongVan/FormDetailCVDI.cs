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

namespace QuanLyCongVan
{
    public partial class FormDetailCVDI : Form
    {
        public FormDetailCVDI()
        {
            InitializeComponent();
        }

        public int index;
        public string s;
        private void FormDetailCVDI_Load(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source = NGUYENNGOCBAOTR\SQLEXPRESS; Initial Catalog = QLCV; Integrated Security = True");
            con.Open();

            //mã công văn và tên công văn đi
            var cmd = new SqlCommand("select * from CVDI where MACVDI = '"+s+"'", con);
            var dr = cmd.ExecuteReader();
            var dtMaCVDI = new DataTable();
            dtMaCVDI.Load(dr);
            txtMaCV.Text = dtMaCVDI.Rows[0][0].ToString();
            txtTenCV.Text = dtMaCVDI.Rows[0][5].ToString();

            //tên loại công văn
            txtMaLoaiCV.Text = dtMaCVDI.Rows[0][1].ToString();
            cmd = new SqlCommand("select * from LOAICV where MALOAICV = '" + txtMaLoaiCV.Text + "'", con);
            dr = cmd.ExecuteReader();
            var dtTenLoaiCV = new DataTable();
            dtTenLoaiCV.Load(dr);
            txtMaLoaiCV.Text = dtTenLoaiCV.Rows[0][0].ToString();
            txtLoaiCV.Text = dtTenLoaiCV.Rows[0][1].ToString();

            //mã và tên loại bảo mật
            txtBM.Text = dtMaCVDI.Rows[0][2].ToString();
            cmd = new SqlCommand("select * from LOAIBM where MALOAIBM = '" + txtBM.Text + "'", con);
            dr = cmd.ExecuteReader();
            var dtTenLoaiBM = new DataTable();
            dtTenLoaiBM.Load(dr);
            txtMaBM.Text = dtTenLoaiBM.Rows[0][0].ToString();
            txtBM.Text = dtTenLoaiBM.Rows[0][1].ToString();

            //mã bộ phận và tên bộ phận
            txtMaBP.Text = dtMaCVDI.Rows[0][3].ToString();
            cmd = new SqlCommand("select * from BOPHAN where MABP = '" + txtMaBP.Text + "'", con);
            dr = cmd.ExecuteReader();
            var dtBP = new DataTable();
            dtBP.Load(dr);
            txtMaBP.Text = dtBP.Rows[0][0].ToString();
            txtBP.Text = dtBP.Rows[0][1].ToString();

            //mã cơ quan và tên cơ quan
            txtMaCQ.Text = dtMaCVDI.Rows[0][4].ToString();
            cmd = new SqlCommand("select * from COQUAN where MACQ = '" + txtMaCQ.Text + "'", con);
            dr = cmd.ExecuteReader();
            var dtCQ = new DataTable();
            dtCQ.Load(dr);
            txtMaCQ.Text = dtCQ.Rows[0][0].ToString();
            txtCQ.Text = dtCQ.Rows[0][1].ToString();

            //trích yếu
            txtTrichYeu.Text = dtMaCVDI.Rows[0][6].ToString();
            //ngày gửi
            txtNgayGui.Text = dtMaCVDI.Rows[0][7].ToString();
            //ngày ký
            txtNgayKy.Text = dtMaCVDI.Rows[0][8].ToString();
            //người gửi
            txtNguoiKy.Text = dtMaCVDI.Rows[0][9].ToString();
        }
    }
}
