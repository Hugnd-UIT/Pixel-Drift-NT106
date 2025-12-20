# 🏎️ Pixel Drift - Real-time Multiplayer Racing

<div align="center">

![Image](https://github.com/user-attachments/assets/6a9bd581-b6b8-4be4-9f7f-adb1ec4f36d1)

[![.NET](https://img.shields.io/badge/.NET%20Framework-4.7.2-purple?style=for-the-badge&logo=.net)](https://dotnet.microsoft.com/download)
[![Language](https://img.shields.io/badge/Language-C%23-green?style=for-the-badge&logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Platform](https://img.shields.io/badge/Platform-Windows%20Forms-blue?style=for-the-badge&logo=windows)](https://docs.microsoft.com/en-us/dotnet/desktop/winforms/)

**Đồ án môn học: Lập trình mạng căn bản (NT106)**
*Khoa Mạng Máy tính & Truyền thông - UIT*

</div>

---

## 📖 Giới thiệu

**Pixel Drift** là tựa game đua xe đối kháng thời gian thực (Real-time) dành cho 2 người chơi. Dự án được xây dựng trên nền tảng **C# WinForms**, sử dụng kỹ thuật **Lập Trình Socket** để kết nối và đồng bộ dữ liệu giữa các máy tính trong mạng LAN/VPN, mang lại trải nghiệm thi đấu mượt mà và ổn định.

---
## 📸 Demo
**Mở Đầu Game**
> ![Image](https://github.com/user-attachments/assets/a63248c7-7662-4077-848a-42a28d72e457)

**Đăng Kí**
> ![Image](https://github.com/user-attachments/assets/3e91a7a1-ae12-4acf-b359-7f463f4c0153)

**Đăng Nhập**
> ![Image](https://github.com/user-attachments/assets/33cd6a70-e56c-49a3-af62-487817ac4e91)

**Thông Tin Người Dùng**
> ![Image](https://github.com/user-attachments/assets/2e60f4e7-2cb3-470a-938e-a07adae2cbe2)

**Lobby**
> ![Image](https://github.com/user-attachments/assets/a6c05d8e-443e-4954-b1b3-958a3a2be38e)

**Game Play**
> ![Image](https://github.com/user-attachments/assets/ebf0b6a6-9023-4354-8e72-b6651157ebd4)

--- 

## 🎮 Chức năng của Game

### 1. Cơ chế chơi 
* **Luật chơi:**
    * Game dành cho **2 người chơi**.
    * Mỗi ván đấu giới hạn trong **60 giây**.
    * Hết giờ, ai có điểm số cao hơn sẽ thắng.
* **Tương tác trong game:**
    * **Điều khiển:** Di chuyển xe qua trái/phải.
    * **Vật phẩm:** **Buff** (Tăng tốc). **Debuff** (Giảm tốc).
    * **Va chạm:** Va chạm với xe khác sẽ bị giảm tốc độ.

### 2. Chức năng hệ thống
* **Quản lý tài khoản:**
    * Đăng ký.
    * Đăng nhập.
    * Quên mật khẩu.
    * Đổi mật khẩu.
* **Sảnh chờ (Lobby):**
    * Người chơi có thể tạo phòng.
    * Người chơi khác nhập ID để vào phòng.
* **Lưu trữ:**
    * Tự động lưu kết quả sau mỗi trận đấu vào cơ sở dữ liệu.
    * Xem Bảng xếp hạng các người chơi điểm cao.

---

## 📂 Cấu trúc dự án

```bash
Pixel-Drift/
├── 📂 Pixel_Drift_Server/
│   ├── Program.cs                
│   ├── Server_Form.cs            
│   ├── Game_Player.cs            
│   ├── Game_Room.cs              
│   ├── SQL_Helper.cs             
│   └── Qly_Nguoi_Dung.db         
│
├── 📂 Pixel_Drift_Client/
│   ├── Program.cs                
│   ├── Client_Manager.cs         
│   ├── Form_Mo_Dau.cs            
│   ├── Form_Dang_Nhap.cs         
│   ├── Form_Dang_Ki.cs           
│   ├── Form_Quen_Mat_Khau.cs     
│   ├── Form_Doi_Mat_Khau.cs      
│   ├── Lobby.cs                  
│   ├── Form_ID.cs                
│   ├── Form_Game_Play.cs         
│   ├── Form_Scoreboard.cs        
│   ├── Form_Thong_Tin.cs         
│   └── *.wav                     
│
└── 📄 README.md                 
```

---

## 🛠 Hướng dẫn cài đặt

1. Tải file **Setup_Game.zip** trong mục Release.
2. Giải nén file ra thư mục.
3. Chạy file **Setup_Game.exe**.
4. Cài đặt **Radmin:** https://www.radmin-vpn.com/
5. Mở Radmin chọn **Network**
6. Chọn **Join Network** 
7. Nhập Network name: **Pixel Drift** và Password: **0123456789**
8. Mở Game và bắt đầu chơi thôi!!!

---
<div align="center">
  <sub>© 2025 Pixel Drift - UIT</sub>
</div>