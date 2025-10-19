﻿using HopDong.Application.Services.IServices;
using HopDong.Domain.Entities;
using HopDong.Domain.Interfaces;
using HopDong.Infrastructure.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using OpenXmlPowerTools;
using System.Text;
using System.Web;
using Xceed.Words.NET;

namespace HopDong.Application.Services;

public class HopDongService : IHopDongService
{
    private readonly IHopDongRepository _repo;
    private readonly IEmailService _emailService;
    private readonly IHostEnvironment _env;

    public HopDongService(IHopDongRepository repo, IEmailService emailService, IHostEnvironment env)
    {
        _repo = repo;
        _emailService = emailService;
        _env = env;
    }

    // API #1 (Cũ, cập nhật lại)
    public async Task<Guid> LuuHopDongVaTaoFileAsync(TaoHopDongDto request)
    {
        var ngayTao = DateTime.UtcNow;
        DateTime? ngayHetHan = null;

        // --- LOGIC TÍNH NGÀY HẾT HẠN NẰM Ở ĐÂY ---
        if (request.DonViThoiHan.ToLower() == "thang")
        {
            ngayHetHan = ngayTao.AddMonths(request.ThoiHanThue);
        }
        else if (request.DonViThoiHan.ToLower() == "ngay")
        {
            ngayHetHan = ngayTao.AddDays(request.ThoiHanThue);
        }

        var hopDongEntity = new HopDongThueXe
        {
            Id = Guid.NewGuid(),
            SoHopDong = request.SoHopDong,
            HoTenBenA = request.BenA.HoTen,
            BienSoXe = request.Xe.BienSo,
            NgayTao = DateTime.UtcNow,
            Status = HopDongStatus.Pending
        };

        //await _repo.AddAsync(hopDongEntity);

        // Bước 2: Tạo file Word từ template
        var templatePath = Path.Combine(_env.ContentRootPath, "Templates", "hopdongthuexe.docx");
        using var document = DocX.Load(templatePath);

        // Thay thế các placeholder
        document.ReplaceText("{{so_hop_dong}}", request.SoHopDong);
        document.ReplaceText("{{ngay_ky}}", request.NgayKy);
        document.ReplaceText("{{thang_ky}}", request.ThangKy);
        document.ReplaceText("{{nam_ky}}", request.NamKy);

        document.ReplaceText("{{HO_TEN_BEN_A}}", request.BenA.HoTen);
        document.ReplaceText("{{nam_sinh_ben_a}}", request.BenA.NamSinh);
        document.ReplaceText("{{cccd_hoac_ho_chieu_ben_a}}", request.BenA.CccdHoacHoChieu);
        document.ReplaceText("{{ho_khau_thuong_tru}}", request.BenA.HoKhauThuongTru);

        document.ReplaceText("{{nhan_hieu}}", request.Xe.NhanHieu);
        document.ReplaceText("{{bien_so}}", request.Xe.BienSo);
        document.ReplaceText("{{loai_xe}}", request.Xe.LoaiXe);
        document.ReplaceText("{{mau_son}}", request.Xe.MauSon);
        document.ReplaceText("{{cho_ngoi}}", request.Xe.ChoNgoi);
        document.ReplaceText("{{xe_dang_ki_han}}", request.Xe.XeDangKiHan);

        document.ReplaceText("{{gplx_hang}}", request.GPLX.Hang);
        document.ReplaceText("{{gplx_so}}", request.GPLX.So);
        document.ReplaceText("{{gplx_han_su_dung}}", request.GPLX.HanSuDung);

        document.ReplaceText("{{thoi_han_thue_so}}", request.ThoiHanThueSo);
        document.ReplaceText("{{thoi_han_thue_chu}}", request.ThoiHanThueChu);

        document.ReplaceText("{{gia_thue_so}}", request.GiaThue.GiaThueSo);
        document.ReplaceText("{{gia_thue_chu}}", request.GiaThue.GiaThueChu);
        document.ReplaceText("{{phuong_thuc_thanh_toan}}", request.GiaThue.PhuongThucThanhToan);
        document.ReplaceText("{{ngay_thanh_toan}}", request.GiaThue.NgayThanhToan);

        var memoryStream = new MemoryStream();
        document.SaveAs(memoryStream);
        memoryStream.Position = 0;
        var filePath = Path.Combine(_env.ContentRootPath, "Storage", $"{hopDongEntity.Id}.docx");
        // Đảm bảo thư mục Storage tồn tại
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        await File.WriteAllBytesAsync(filePath, memoryStream.ToArray()); // Lưu file vật lý

        await _repo.AddAsync(hopDongEntity);
        return hopDongEntity.Id;
    }

    // API Gửi Email
    public async Task GuiEmailXacNhanAsync(Guid hopDongId, string email)
    {
        // Bước 1: Tìm hợp đồng trong DB
        var hopDong = await _repo.GetByIdAsync(hopDongId); // Cần thêm GetByIdAsync vào Repository
        if (hopDong == null || hopDong.Status != HopDongStatus.Pending)
        {
            throw new Exception("Hợp đồng không tồn tại hoặc đã được xử lý.");
        }

        // Bước 2: Tạo token và thời gian hết hạn
        hopDong.ConfirmationToken = Guid.NewGuid().ToString("N");
        hopDong.TokenExpiry = DateTime.UtcNow.AddHours(24); // Token hết hạn sau 24 giờ

        // Bước 3: Cập nhật hợp đồng với token mới
        await _repo.UpdateAsync(hopDong);

        // Bước 4: Tạo link xác nhận
        // Lưu ý: "https://your-frontend.com" là địa chỉ trang frontend của bạn
        var confirmationLink = $"https://your-frontend.com/xac-nhan-hop-dong?token={hopDong.ConfirmationToken}";

        // Bước 5: Gọi EmailService để gửi
        await _emailService.SendConfirmationEmailAsync(email, confirmationLink);
    }

    // API #2: Lấy nội dung để ký
    public async Task<HopDongXacNhanDto> LayHopDongDeXacNhanAsync(string token)
    {
        var hopDong = await _repo.GetByTokenAsync(token);
        if (hopDong == null || hopDong.Status != HopDongStatus.Pending || hopDong.TokenExpiry < DateTime.UtcNow)
        {
            throw new Exception("Link không hợp lệ hoặc đã hết hạn.");
        }

        var docxPath = Path.Combine(_env.ContentRootPath, "Storage", $"{hopDong.Id}.docx");
        var docxBytes = await File.ReadAllBytesAsync(docxPath);

        // Chuyển DOCX sang HTML
        var settings = new WmlToHtmlConverterSettings();
        var wmlDoc = new WmlDocument(docxPath, docxBytes);
        var htmlElement = WmlToHtmlConverter.ConvertToHtml(wmlDoc, settings);
        var htmlString = htmlElement.ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

        return new HopDongXacNhanDto(hopDong.SoHopDong, hopDong.HoTenBenA, hopDong.NgayTao, htmlString);
    }

    // API #3: Xác nhận ký
    public async Task XacNhanKyHopDongAsync(string token)
    {
        var hopDong = await _repo.GetByTokenAsync(token);
        // ... (Kiểm tra token hợp lệ tương tự như trên) ...
        if (hopDong == null)
        {
            throw new Exception("Token xác nhận không tồn tại.");
        }

        if (hopDong.Status == HopDongStatus.Signed)
        {
            throw new Exception("Hợp đồng này đã được ký trước đó.");
        }

        if (hopDong.Status == HopDongStatus.Expired || (hopDong.TokenExpiry.HasValue && hopDong.TokenExpiry < DateTime.UtcNow))
        {
            // Cập nhật trạng thái sang hết hạn nếu cần
            if (hopDong.Status != HopDongStatus.Expired)
            {
                hopDong.Status = HopDongStatus.Expired;
                await _repo.UpdateAsync(hopDong);
            }
            throw new Exception("Link xác nhận đã hết hạn.");
        }

        hopDong.Status = HopDongStatus.Signed;
        hopDong.NgayKy = DateTime.UtcNow;
        hopDong.ConfirmationToken = null; // Vô hiệu hóa token

        await _repo.UpdateAsync(hopDong);
    }

    // API Xóa mềm hợp đồng
    public async Task XoaMemHopDongAsync(Guid id)
    {
        var hopDong = await _repo.GetByIdAsync(id);
        if (hopDong == null)
        {
            throw new Exception("Không tìm thấy hợp đồng.");
        }

        hopDong.IsDeleted = true;
        await _repo.UpdateAsync(hopDong);
    }
    public async Task<HopDongDto?> LayHopDongTheoIdAsync(Guid id)
    {
        var hopDong = await _repo.GetByIdAsync(id); // dùng repo
        if (hopDong == null) return null;

        return new HopDongDto(
            hopDong.Id,
            hopDong.SoHopDong,
            hopDong.HoTenBenA,
            hopDong.NgayTao
            
        );
    }

}
