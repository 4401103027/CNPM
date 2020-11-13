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
    public partial class FormTaoTaiKhoan : Form
    {
        public FormTaoTaiKhoan()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn muốn thoát?", "Thông báo" ,MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                Thread th; //dùng để đóng form cũ mở form mới
                this.Close();
                th = new Thread(opennewform);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
        }

        private void opennewform(object obj)
        {
            Application.Run(new FormChinh());
        }

        private void Cancel()
        {
            txtAddUeser.Text = "";
            txtMABP.Text = "";
            txtTenNV.Text = "";
            txtPassword.Text = "";
            txtDiaChi.Text = "";
            rdbNam.Checked = false;
            rdbNu.Checked = false;
            txtAddPhone.Text = "";
            cbxChucVu.Text = "Chọn chức vụ";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=NGUYENNGOCBAOTR\SQLEXPRESS;Initial Catalog=QLCV;Integrated Security=True");

            string MANV, MABP, TENNV, PASS, DIACHI, GIOITINH, PHONE, CHUCVU;
            MANV = txtAddUeser.Text;
            MABP = txtMABP.Text;
            TENNV = txtTenNV.Text;
            FormDangNhap flogin = new FormDangNhap();
            PASS = flogin.MD5STRING(txtPassword.Text);
            DIACHI = txtDiaChi.Text;
            if (rdbNam.Checked == true) GIOITINH = "1";
            else GIOITINH = "0";
            PHONE = txtAddPhone.Text;
            CHUCVU = cbxChucVu.Text;

            try
            {
                conn.Open();
                string sql = "insert into NHANVIEN values ('" + MANV + "', '" + MABP + "', N'" + TENNV + "', '" + PASS + "', N'" + DIACHI + "', '" + GIOITINH + "', '" + PHONE + "', N'" + CHUCVU + "')";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                int kq = cmd.ExecuteNonQuery();
                if (kq > 0) MessageBox.Show("Thêm thành công");
                else MessageBox.Show("Thêm thất bại");
                conn.Close();
                Cancel();
            }
            catch
            {
                MessageBox.Show("Lỗi kết nối");
                conn.Close();
            }
        }
    }
}
