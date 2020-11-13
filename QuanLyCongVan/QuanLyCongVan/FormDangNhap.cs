using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Threading;

namespace QuanLyCongVan
{
    public partial class FormDangNhap : Form
    {
        public FormDangNhap()
        {
            InitializeComponent();
        }

        //hàm chuyển mật khẩu sang dạng md5
        public string MD5STRING(string s)
        {
            MD5 md = MD5.Create();
            byte[] inputstr = System.Text.Encoding.ASCII.GetBytes(s);
            byte[] hash = md.ComputeHash(inputstr);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }     
            return sb.ToString();
        }

        //nút đăng nhập
        private void btnLogin_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=NGUYENNGOCBAOTR\SQLEXPRESS;Initial Catalog=QLCV;Integrated Security=True");

            try
            {
                conn.Open();
                //txtAcount.Text = "nva";
                //txtPassword.Text = "1234";
                string tk = txtAcount.Text;
                string mk_md5 = MD5STRING(txtPassword.Text); 
                string sql = "select * from NHANVIEN where MANV='" + tk + "' and MATKHAU='" + mk_md5 + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dta = cmd.ExecuteReader();
                if (dta.Read() == true) 
                {
                    string s = sql;
                    FormChinh formChinh = new FormChinh(txtAcount.Text);
                    this.Hide();
                    formChinh.ShowDialog();
                    this.Close();
                }
                else MessageBox.Show("Đăng nhập thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conn.Close();
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("Lỗi kết nối");
                conn.Close();
            }
        }

        private void opennewform(object obj)
        {
            Application.Run(new FormChinh());
        }

        //nút thoát
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult tb = MessageBox.Show("Bạn có thoát hay không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if(tb == DialogResult.OK)
            {
                Close();
            }
        }
    }
}
