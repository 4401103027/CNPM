create database QLCV;

use QLCV;

create table LOAICV
(
    MALOAICV varchar(10),
    TENLOAI nvarchar(30),
    constraint pk_maloai primary key (MALOAICV)
);

insert into LOAICV (MALOAICV, TENLOAI) values
('CVHD', N'Công văn hướng dẫn'),
('CVGT', N'Công văn giải thích');

create table BOPHAN
(
    MABP varchar(10),
    TENBP nvarchar(30),
    TENLANHDAO nvarchar(50),
    DIENTHOAI varchar(10),
    constraint pk_mabp primary key (MABP)
);

create table NHANVIEN
(
    MANV varchar(10),
    MABP varchar(10),
    TENNV nvarchar(30),
    MATKHAU varchar(50),
    DIACHI nvarchar(50),
    GIOITINH bit,
    DIENTHOAI varchar(10),
    CHUCVU nvarchar(15),
    constraint pk_manv primary key (MANV),
    constraint fk_nv_mabp foreign key (MABP) references BOPHAN(MABP)
);
insert into BOPHAN (MABP, TENBP, TENLANHDAO, DIENTHOAI) values
('BPHCVT', N'Bộ phân Hành Chính - Văn thư', N'Nguyễn Văn A', 0987654321);
select * from BOPHAN;

insert into NHANVIEN(MANV, MABP, TENNV, MATKHAU, GIOITINH, DIACHI, DIENTHOAI, CHUCVU) values
('nva', 'bpa', N'tennva', '81dc9bdb52d04dc20036dbd8313ed055', 1, N'dca', 0987654321, N'cva');

select * from CVDI;

update NHANVIEN SET GIOITINH = 0 WHERE MANV = 'nv3';

create table COQUAN
(
    MACQ varchar(10),
    TENCQ nvarchar(30),
    DIACHI nvarchar(50),
    DIENTHOAI varchar(10),
    constraint pk_macq primary key (MACQ)
);
insert into COQUAN (MACQ, TENCQ, DIACHI, DIENTHOAI) values
('BGDĐT', N'Bộ Giáo dục và Đào tạo', N'Số 35 Đại Cồ Việt, Hà Nội', 0987654321),
('BYT', N'Bộ Y tế', N'Số 138A Giảng Võ - Ba Đình - Hà Nội', 0987654321),
('BCA', N'Bộ Công an', N'44 Yết Kiêu - Hoàn Kiếm - Hà Nội', 0987654321);

create table CVDEN
(
    MACVDEN varchar(30),
    MALOAI varchar(10),
    MABP varchar(10),
    MACQ varchar(10),
    TENCVDEN nvarchar(50),
    TRICHYEU nvarchar(50),
    NGAYNHAN date,
    NGAYKY date,
    NGUOIKY nvarchar(50),
    constraint pk_macvden primary key (MACVDEN),
    constraint fk_cvden_maloai foreign key (MALOAI) references LOAICV(MALOAI),
    constraint fk_cvden_mabp foreign key (MABP) references BOPHAN(MABP),
    constraint fk_cvden_macq foreign key (MACQ) references COQUAN(MACQ),
    constraint chk_ngay_den_ky check (NGAYKY <= NGAYNHAN)
);

create table LOAIBM
(
	MALOAIBM varchar(10),
	TENLOAIBM nvarchar(30),
	constraint pk_maloaibm primary key (MALOAIBM),
);

insert into LOAIBM (MALOAIBM, TENLOAIBM) values
('KC', N'Khẩn cấp'),
('BT', N'Bình thường');

create table CVDI
(
    MACVDI varchar(10),
    MALOAICV varchar(10),
	MALOAIBM varchar(10),
    MABP varchar(10),
    MACQ varchar(10),
    TENCVDI nvarchar(50),
    TRICHYEU nvarchar(50),
    NGAYGUI date,
    NGAYKY date,
    NGUOIKY nvarchar(50),
    constraint pk_macvdi primary key (MACVDI),
    constraint fk_cvdi_maloaicv foreign key (MALOAICV) references LOAICV(MALOAICV),
	constraint fk_cvdi_maloaibm foreign key (MALOAIBM) references LOAIBM(MALOAIBM),
    constraint fk_cvdi_mabp foreign key (MABP) references BOPHAN(MABP),
    constraint fk_cvdi_macq foreign key (MACQ) references COQUAN(MACQ),
    constraint chk_ngay_di_ky check (NGAYKY <= NGAYGUI)
);

insert into BOPHAN (MABP, TENBP, TENLANHDAO, DIENTHOAI) VALUES
('BPVT', N'Bộ phận Văn Thư', N'Nguyễn Văn A', '0123456789');

insert into LOAICV (MALOAICV, TENLOAI) VALUES
('BC', N'Báo cáo'),
('CT', N'Chỉ thị'),
('HD', N'Hướng dẫn');

insert into COQUAN(MACQ, TENCQ, DIACHI, DIENTHOAI) VALUES
('CP', 'Chính Phủ', '16 Lê Hồng Phong - Ba Đình - Hà Nội',  '080 43162'),
('BGDDT', 'Bộ Giáo Dục và Đào Tạo', null, null);

insert into CVDEN(MACVDEN, MALOAI, MABP, MACQ, TENCVDEN, TRICHYEU, NGAYNHAN, NGAYKY, NGUOIKY) VALUES
('3843/BGDĐT-VP', 'BC', 'BPHC', 'BGDDT', N'Báo cáo thống kê giáo dục kỳ đầu năm học 2020-2021', N'Đây là trích yếu', '2020-10-10', '2020-09-10', N'Nguyễn Văn B');


select * from CVDEN;
