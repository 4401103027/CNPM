using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Data.SqlClient;

namespace QuanLyCongVan
{
    public partial class FormChinh : Form
    {
        public FormChinh()
        {
            InitializeComponent();
        }

        string CodeLogin;
        public FormChinh(string NameLogin)
        {
            InitializeComponent();
            this.CodeLogin = NameLogin;
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Bạn có muốn quay lại đăng nhập không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dialog == DialogResult.OK)
            {
                Thread th;
                this.Close();
                th = new Thread(openformchinh);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            else
            {
                DialogResult result = MessageBox.Show("Bạn muốn đăng xuất?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    Close();
                }
            }
        }

        private void openformchinh(object obj)
        {
            Application.Run(new FormDangNhap());
        }

        private void tạoTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CodeLogin == "NV_Admin")
            {
                Thread th;
                this.Close();
                th = new Thread(openformAddUser);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            else
                MessageBox.Show("Bạn không có quyền truy cập!", "Cảnh bảo!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private void openformAddUser(object obj)
        {
            Application.Run(new FormTaoTaiKhoan());
        }

        private void FormChinh_Load(object sender, EventArgs e)
        {
            GetNhanVienList(); //dtgNhanVien.CurrentCell.ReadOnly = true;
            dtgCVDi.DataSource = GetCongVanDiList();

            //Tải dữ liệu lên combox Search
            SqlConnection con = new SqlConnection(@"Data Source = NGUYENNGOCBAOTR\SQLEXPRESS; Initial Catalog = QLCV; Integrated Security = True");
            con.Open();
            var cmd = new SqlCommand("select MACVDI from CVDI", con);
            var dr = cmd.ExecuteReader();

            var dtSearch = new DataTable();
            dtSearch.Load(dr);
            cbxMaCV.DisplayMember = "MACVDI";
            cbxMaCV.DataSource = dtSearch;
        }

        private DataTable GetCongVanDiList()
        {
            DataTable dtCongVanDi = new DataTable();
            using (SqlConnection con = new SqlConnection(@"Data Source=NGUYENNGOCBAOTR\SQLEXPRESS;Initial Catalog=QLCV;Integrated Security=True"))
            {
                using (SqlCommand cmd = new SqlCommand("select MACVDI as N'Số công văn', MALOAICV as N'Mã loại công văn', MALOAIBM as 'Mã loại bảo mật', " +
                    "MABP as 'Mã bộ phận', MACQ as 'Mã cơ quan', TENCVDI as 'Tên công văn đi', TRICHYEU as 'Mô tả', NGAYGUI as 'Ngày gửi', NGAYKY as 'Ngày ký', NGUOIKY as 'Người ký' " +
                    "from CVDI", con))
                {
                    try
                    {
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        dtCongVanDi.Load(reader);
                        con.Close();
                        dtgCVDi.DataSource = dtgCVDi;

                        //điều chỉnh độ rộng cột
                        //Exception ex1;
                        //dtgCVDi.Columns[5].Width = 100; //Tên công văn
                    }
                    catch
                    {
                        MessageBox.Show("Lỗi tải dữ liệu");
                        con.Close();
                    }
                }
            }

            return dtCongVanDi;
        }

        private void GetNhanVienList()
        {
            DataTable dtNhanVien = new DataTable();
            using (SqlConnection con = new SqlConnection(@"Data Source=NGUYENNGOCBAOTR\SQLEXPRESS;Initial Catalog=QLCV;Integrated Security=True"))
            {
                using (SqlCommand cmd = new SqlCommand("select MANV as N'Mã nhân viên', MABP as N'Mã bộ phận', TENNV as 'Tên nhân viên', " +
                    "DIACHI as 'Địa chỉ', GIOITINH as 'Giới tính', DIENTHOAI as 'Điện thoại', CHUCVU as 'Chức vụ' from NHANVIEN", con))
                {
                    try
                    { con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        dtNhanVien.Load(reader);
                        con.Close();
                        dtgNhanVien.DataSource = dtNhanVien;
                        
                        //điều chỉnh độ rộng cột
                        dtgNhanVien.Columns[0].Width = 131; //mã nhân viên
                        dtgNhanVien.Columns[1].Width = 120; //mã bộ phận
                        dtgNhanVien.Columns[2].Width = 138; //tên nhân viên
                        dtgNhanVien.Columns[5].Width = 120; //địa chỉ
                    }
                    catch
                    {
                        MessageBox.Show("Lỗi tải dữ liệu");
                        con.Close();
                    }
                }
            }
        }

        int IndexRow;
        public void xóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(CodeLogin == "NV_Admin")
            {
                if (IndexRow >= 0)
                {
                    DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa?", "Cảnh báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (result == DialogResult.OK)
                        try
                        {
                            string MANV = dtgNhanVien.Rows[IndexRow].Cells["Mã nhân viên"].FormattedValue.ToString();
                            SqlConnection con = new SqlConnection(@"Data Source = NGUYENNGOCBAOTR\SQLEXPRESS; Initial Catalog = QLCV; Integrated Security = True");
                            con.Open();
                            SqlCommand cmd = new SqlCommand("Delete NHANVIEN where MANV = '" + MANV + "'", con);
                            int kq = cmd.ExecuteNonQuery();
                            if (kq >= 0)
                            {
                                FormChinh_Load(sender, e);
                                MessageBox.Show("Xóa thành công");
                            }
                            else { MessageBox.Show("Xóa thất bại"); }
                        }
                        catch
                        {
                            MessageBox.Show("Lỗi");
                        }
                }
            }
            else
                MessageBox.Show("Bạn không có quyền truy cập!", "Cảnh bảo!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        string MANVClick, MABPClick, TENNVClick, DIACHIClick, DIENTHOAIClick, CHUCVUClick, GIOITINHClick;

        private void openfrormDetailCVDI(object obj)
        {
            Application.Run(new FormDetailCVDI());
        }

        public int index;
        public DataTable dtDetailCVDI;
        public string MACVDI;
        private void dtgCVDi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            MACVDI = dtgCVDi.Rows[e.RowIndex].Cells[0].Value.ToString();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (CodeLogin == "NV_Admin")
            {
                Thread th;
                this.Close();
                th = new Thread(openformAddCVDI);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            else
                MessageBox.Show("Bạn không có quyền truy cập!", "Cảnh bảo!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private void openformAddCVDI(object obj)
        {
            Application.Run(new FormCVDI());
        }

        private void chiTiếtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDetailCVDI detailCVDI = new FormDetailCVDI();
            detailCVDI.index = this.index;
            if(MACVDI == null)
                MACVDI = dtgCVDi.Rows[0].Cells[0].Value.ToString();
            detailCVDI.s = this.MACVDI;
            detailCVDI.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dtgCVDi.DataSource = GetCongVanDiList();
        }

        private void GetSearchList(object sender, EventArgs e, string sql)
        {
            DataTable dataSearch = new DataTable();

            using (SqlConnection con = new SqlConnection(@"Data Source = NGUYENNGOCBAOTR\SQLEXPRESS; Initial Catalog = QLCV; Integrated Security = True"))
            {
                using (SqlCommand cmd = new SqlCommand("select MACVDI as N'Số công văn', MALOAICV as N'Mã loại công văn', MALOAIBM as 'Mã loại bảo mật', " +
                    "MABP as 'Mã bộ phận', MACQ as 'Mã cơ quan', TENCVDI as 'Tên công văn đi', TRICHYEU as 'Mô tả', NGAYGUI as 'Ngày gửi', NGAYKY as 'Ngày ký', NGUOIKY as 'Người ký'  from CVDI where MACVDI = '" + sql + "'", con))
                {
                    con.Open();
                    SqlDataReader dta = cmd.ExecuteReader();
                    dataSearch.Load(dta);
                    dtgCVDi.DataSource = dataSearch;
                    con.Close();
                }
            }
        }

        private void btnTim_Click_1(object sender, EventArgs e)
        {
            DataTable data = new DataTable();
            string sql = cbxMaCV.Text;
            SqlConnection con = new SqlConnection(@"Data Source = NGUYENNGOCBAOTR\SQLEXPRESS; Initial Catalog = QLCV; Integrated Security = True");
            SqlCommand cmd = new SqlCommand("select * from CVDI where MACVDI = '" + sql + "'", con);
            
            con.Open();
            SqlDataReader dta = cmd.ExecuteReader();
            try
            {
                if (dta.Read() == true)
                {
                    GetSearchList(sender, e, sql);
                    MessageBox.Show("Tim kiem thanh cong");
                }
                else
                    MessageBox.Show("Đăng nhập thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch { MessageBox.Show("Lỗi tải dữ liệu Search", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            con.Close();
        }

        private void dtgNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            IndexRow = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                MANVClick = dtgNhanVien.Rows[e.RowIndex].Cells[0].Value.ToString();
                MABPClick = dtgNhanVien.Rows[e.RowIndex].Cells[1].Value.ToString();
                TENNVClick = dtgNhanVien.Rows[e.RowIndex].Cells[2].Value.ToString();
                DIACHIClick = dtgNhanVien.Rows[e.RowIndex].Cells[3].Value.ToString();
                DIENTHOAIClick = dtgNhanVien.Rows[e.RowIndex].Cells[5].Value.ToString();
                CHUCVUClick = dtgNhanVien.Rows[e.RowIndex].Cells[6].Value.ToString();
                GIOITINHClick = dtgNhanVien.Rows[e.RowIndex].Cells[4].Value.ToString();
            }
            
        }

        private void dtgNhanVien_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if(CodeLogin == "NV_Admin")
            { 
            string MANV, MABP, TENNV, DIACHI, DIENTHOAI, CHUCVU, GIOITINH;

            //kết nối SQL
            SqlConnection con = new SqlConnection(@"Data Source = NGUYENNGOCBAOTR\SQLEXPRESS; Initial Catalog = QLCV; Integrated Security = True");
            con.Open();

            //gán các giá trị sửa đổi
            if (dtgNhanVien.Columns[e.ColumnIndex].HeaderCell.Value.ToString() == "Mã nhân viên")
                MANV = dtgNhanVien.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            else MANV = MANVClick;

            if (dtgNhanVien.Columns[e.ColumnIndex].HeaderCell.Value.ToString() == "Mã bộ phận")
                MABP = dtgNhanVien.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            else MABP = MABPClick;

            if (dtgNhanVien.Columns[e.ColumnIndex].HeaderCell.Value.ToString() == "Tên nhân viên")
                TENNV = dtgNhanVien.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            else TENNV = TENNVClick;

            if (dtgNhanVien.Columns[e.ColumnIndex].HeaderCell.Value.ToString() == "Địa chỉ")
                DIACHI = dtgNhanVien.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            else DIACHI = DIACHIClick;

            if (dtgNhanVien.Columns[e.ColumnIndex].HeaderCell.Value.ToString() == "Giới tính")
                GIOITINH = dtgNhanVien.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            else GIOITINH = GIOITINHClick;

            if (dtgNhanVien.Columns[e.ColumnIndex].HeaderCell.Value.ToString() == "Điện thoại")
                DIENTHOAI = dtgNhanVien.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            else DIENTHOAI = DIENTHOAIClick;

            if (dtgNhanVien.Columns[e.ColumnIndex].HeaderCell.Value.ToString() == "Chức vụ")
                CHUCVU = dtgNhanVien.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            else CHUCVU = CHUCVUClick;

            //thông tin cho người dùng thông tin sẽ bị thay đổi
            DialogResult result;
            if (MANV != MANVClick || MABP != MABPClick || TENNV != TENNVClick || DIACHI != DIACHIClick || GIOITINH != GIOITINHClick || DIENTHOAI != DIENTHOAIClick)
                result = MessageBox.Show("Bạn có chắc muốn thay đổi thông tin?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            else result = DialogResult.Cancel;

                if (result == DialogResult.OK)
                {
                    try
                    {
                        int index = e.ColumnIndex;

                        string s = "";
                        if (e.ColumnIndex == 0) s = "Update NHANVIEN set MANV = '" + MANV + "' where MANV = '" + MANVClick + "'";
                        if (e.ColumnIndex == 1) s = "Update NHANVIEN set MABP = '" + MABP + "' where MANV = '" + MANVClick + "'";
                        if (e.ColumnIndex == 2) s = "Update NHANVIEN set TENNV = '" + TENNV + "' where MANV = '" + MANVClick + "'";
                        if (e.ColumnIndex == 3) s = "Update NHANVIEN set DIACHI = '" + DIACHI + "' where MANV = '" + MANVClick + "'";
                        if (e.ColumnIndex == 4)
                            if (GIOITINH == "True")
                                s = "Update NHANVIEN set GIOITINH = '" + "True" + "' where MANV = '" + MANVClick + "'";
                            else s = "Update NHANVIEN set GIOITINH = '" + "False" + "' where MANV = '" + MANVClick + "'";
                        if (e.ColumnIndex == 5) s = "Update NHANVIEN set DIENTHOAI = '" + DIENTHOAI + "' where MANV = '" + MANVClick + "'";
                        if (e.ColumnIndex == 6) s = "Update NHANVIEN set CHUCVU = '" + CHUCVU + "' where MANV = '" + MANVClick + "'";

                        SqlCommand cmd = new SqlCommand(s, con);
                        int kq = cmd.ExecuteNonQuery();
                        if (kq >= 0)
                        {
                            FormChinh_Load(sender, e);
                            MessageBox.Show("Sửa thành công");
                        }
                        else MessageBox.Show("Sửa thất bại");
                        con.Close();
                    }
                    catch { con.Close(); MessageBox.Show("Lỗi sửa dữ liệu"); }
                }
            }
            else
                MessageBox.Show("Bạn không có quyền truy cập!", "Cảnh bảo!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
    }
}
           

