namespace EVStation_basedRentalSystem.Services.BookingAPI.Models.DTO
{
    // DTO tạo hợp đồng
    public record HopDongDto(
        string SoHopDong,
        string NgayKy,
        string ThangKy,
        string NamKy,
        ThongTinBenA BenA,
        ThongTinXe Xe,
        ThongTinGiaThue GiaThue,
        string ThoiHanThueSo,
        string ThoiHanThueChu,
        int ThoiHanThue,
        string DonViThoiHan,
        ThongTinGPLX GPLX
    );

    public record ThongTinBenA(
        string HoTen,
        string NamSinh,
        string CccdHoacHoChieu,
        string HoKhauThuongTru
    );

    public record ThongTinXe(
        string NhanHieu,
        string BienSo,
        string LoaiXe,
        string MauSon,
        string ChoNgoi,
        string XeDangKiHan
    );

    public record ThongTinGiaThue(
        string GiaThueSo,
        string GiaThueChu,
        string PhuongThucThanhToan,
        string NgayThanhToan
    );

    public record ThongTinGPLX(
        string Hang,
        string So,
        string HanSuDung
    );
}