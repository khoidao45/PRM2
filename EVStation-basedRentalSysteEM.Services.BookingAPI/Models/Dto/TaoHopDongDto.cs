using System.Text.Json.Serialization;

public record ThongTinBenA(
    [property: JsonPropertyName("hoTen")] string HoTen,
    [property: JsonPropertyName("namSinh")] string NamSinh,
    [property: JsonPropertyName("cccdHoacHoChieu")] string CccdHoacHoChieu,
    [property: JsonPropertyName("hoKhauThuongTru")] string HoKhauThuongTru
);

public record ThongTinXe(
    [property: JsonPropertyName("nhanHieu")] string NhanHieu,
    [property: JsonPropertyName("bienSo")] string BienSo,
    [property: JsonPropertyName("loaiXe")] string LoaiXe,
    [property: JsonPropertyName("mauSon")] string MauSon,
    [property: JsonPropertyName("choNgoi")] string ChoNgoi,
    [property: JsonPropertyName("xeDangKiHan")] string XeDangKiHan
);

public record ThongTinGiaThue(
    [property: JsonPropertyName("giaThueSo")] string GiaThueSo,
    [property: JsonPropertyName("giaThueChu")] string GiaThueChu,
    [property: JsonPropertyName("phuongThucThanhToan")] string PhuongThucThanhToan,
    [property: JsonPropertyName("ngayThanhToan")] string NgayThanhToan
);

public record ThongTinGPLX(
    [property: JsonPropertyName("hang")] string Hang,
    [property: JsonPropertyName("so")] string So,
    [property: JsonPropertyName("hanSuDung")] string HanSuDung
);

public record TaoHopDongDto(
    [property: JsonPropertyName("soHopDong")] string SoHopDong,
    [property: JsonPropertyName("ngayKy")] string NgayKy,
    [property: JsonPropertyName("thangKy")] string ThangKy,
    [property: JsonPropertyName("namKy")] string NamKy,
    [property: JsonPropertyName("benA")] ThongTinBenA BenA,
    [property: JsonPropertyName("xe")] ThongTinXe Xe,
    [property: JsonPropertyName("giaThue")] ThongTinGiaThue GiaThue,
    [property: JsonPropertyName("thoiHanThueSo")] string ThoiHanThueSo,
    [property: JsonPropertyName("thoiHanThueChu")] string ThoiHanThueChu,
    [property: JsonPropertyName("thoiHanThue")] int ThoiHanThue,
    [property: JsonPropertyName("donViThoiHan")] string DonViThoiHan,
    [property: JsonPropertyName("gplx")] ThongTinGPLX GPLX
);