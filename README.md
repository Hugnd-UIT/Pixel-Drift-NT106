# 🏎️ Pixel Drift - Real-time Multiplayer Racing

<div align="center">

![Image](https://github.com/user-attachments/assets/6a9bd581-b6b8-4be4-9f7f-adb1ec4f36d1)

[![.NET](https://img.shields.io/badge/.NET%208.0-purple?style=for-the-badge&logo=.net)](https://dotnet.microsoft.com/download)
[![Language](https://img.shields.io/badge/Language-C%23-green?style=for-the-badge&logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Security](https://img.shields.io/badge/Security-SIEM%20%26%20SOC-red?style=for-the-badge&logo=elastic)](https://www.elastic.co/)
[![Platform](https://img.shields.io/badge/Platform-Windows%20Forms-blue?style=for-the-badge&logo=windows)](https://docs.microsoft.com/en-us/dotnet/desktop/winforms/)

**Đồ án môn học: Lập trình mạng căn bản (NT106)**
*Khoa Mạng Máy tính & Truyền thông - UIT*

</div>

---

## 📖 Giới thiệu

**Pixel Drift** là tựa game đua xe đối kháng thời gian thực (Real-Time) dành cho 2 người chơi. Dự án được xây dựng trên nền tảng **.NET 8.0**, sử dụng kỹ thuật **Lập Trình Socket** để kết nối và đồng bộ dữ liệu giữa các máy tính trong mạng LAN/VPN, mang lại trải nghiệm thi đấu mượt mà và ổn định.
**Pixel Drift** là dự án tích hợp hai trong một:
1.  **Game đua xe thời gian thực (Racing Car Real-Time):** Xây dựng trên nền tảng **.NET 8.0**, sử dụng kỹ thuật **Lập Trình Socket TCP/UDP** để kết nối và đồng bộ dữ liệu giữa các máy tính trong mạng LAN/VPN, mang lại trải nghiệm thi đấu mượt mà và ổn định.
2.  **Môi trường giả lập SOC (Security Operations Center):** Được thiết kế như một "Honeypot" để thực hành các kỹ thuật tấn công (Red Team) và phòng thủ (Blue Team) mạng. Dự án áp dụng các tiêu chuẩn bảo mật thực tế như **Mã hóa lai (RSA & AES)**, **Chống tấn công phát lại**, **Chống tấn công DoS**,... và tích hợp hệ thống giám sát **ELK Stack**.

---
## 📸 Demo
**Mở Đầu Game**
> ![Image](https://github.com/user-attachments/assets/6a1f0a08-fa5f-4e12-86a8-69a11e5c5b28)

**Đăng Kí**
> ![Image](https://github.com/user-attachments/assets/d31222a7-8305-447f-92c2-18d6762b6189)

**Đăng Nhập**
> ![Image](https://github.com/user-attachments/assets/8b76c7e7-92f6-4446-9c17-5766744eca13)

**Thông Tin Người Dùng**
> ![Image](https://github.com/user-attachments/assets/df7efa71-9583-4644-b585-6c6f8cf5a434)

**Lobby**
> ![Image](https://github.com/user-attachments/assets/7c778e5e-c940-4c55-a157-651f759af5ce)

**Game Play**
> ![Image](https://github.com/user-attachments/assets/3fd301da-926f-4834-8535-a91686722f65)

--- 

## 🛡️ Hệ thống An toàn & Bảo mật (Safety & Security System)

Dự án mô phỏng quy trình phòng thủ chiều sâu (Defense in Depth) với 4 trụ cột chính:

### 1. Kiến trúc Bảo mật (Secure Architecture)
* **Mã hóa lai (Hybrid Encryption):**
    * **RSA-2048:** Trao đổi khóa phiên an toàn khi bắt đầu kết nối.
    * **AES-256:** Mã hóa toàn bộ gói tin game để chống nghe lén.
* **Input Validation:** Kiểm soát chặt chẽ dữ liệu đầu vào tại cả Client và Server để ngăn chặn Injection và Overflow.

### 2. Hệ thống Giám sát (SIEM)
* Server C# tự động đẩy log chuẩn hóa sang **Logstash** qua HTTP. Sử dụng **Kibana** để vẽ biểu đồ tấn công, theo dõi IP nghi vấn và các hành vi bất thường.

> *Minh họa báo cáo*
> ![Kibana Dashboard](https://github.com/user-attachments/assets/a6559db5-315b-4f31-b981-dc9d6aa7638c)

> *Minh họa báo cáo*
> ![Kibana Dashboard](https://github.com/user-attachments/assets/2d30d754-8cd2-445b-88ca-44d1fd051640)

### 3. Giả lập phòng thủ (Blue Teaming)
* **Chống Tấn công Phát lại (Anti-Replay):**
    * Cơ chế: Gắn **Timestamp** vào header của mọi gói tin đã mã hóa.
    * Xử lý: Server tự động từ chối gói tin có độ trễ > 10 giây.

> **Minh họa phòng thủ Replay Attack**
> *Server phát hiện gói tin có Timestamp cũ và tự động ngắt kết nối.*  
> ![Replay Block Log](https://github.com/user-attachments/assets/8d2e5c6c-bfc9-48e7-9c69-bafc18b41620)

* **Chống quá tải & Spam (Anti-DoS):**
    * Cơ chế: Rate Limiting (Giới hạn 20 gói tin/giây/IP).
    * Xử lý: Tự động **Ban IP 5 phút** nếu vi phạm.

> **Minh họa phòng thủ DoS Attack**
> *Server phát hiện IP gửi request liên tục và tự động ngắt kết nối.*  
> ![DoS Block Log](https://github.com/user-attachments/assets/628c3df1-2dc8-4327-96b5-3281aec0c04c)

* **Chống tràn bộ nhớ (Anti-Buffer Overflow):**
    * Cơ chế: Kiểm tra kích thước gói tin đầu vào
    * Xử lý: Ngắt kết nối ngay nếu Payload > 4096 bytes.

> **Minh họa phòng thủ Buffer Overflow Attack**
> *Server phát hiện gói tin có kích thước bất thường và ngắt kết nối để bảo vệ RAM.*  
> ![Overflow Block Log](https://github.com/user-attachments/assets/89a38661-eb9b-4ec9-b3be-b8323c338a97)

### 4. Giả lập tấn công (Red Teaming)
* Bộ công cụ **Python Scripts** đi kèm để giả lập các đợt tấn công thực tế, dùng để kiểm thử tính hiệu quả của hệ thống phòng thủ.

> **Minh họa tấn công Replay:**
> *Script Python gửi gói tin cũ và bị Server ngắt kết nối cưỡng chế*  
> ![Replay Attack Log](https://github.com/user-attachments/assets/b291fcab-6065-4d03-9e67-b5d2ede1fee4)

> **Minh họa tấn công Overflow:**
> *Script Python gửi gói tin lớn và bị Server ngắt kết nối cưỡng chế*  
> ![Overflow Attack Log](https://github.com/user-attachments/assets/37eb924c-f5ae-4f61-b645-308503238ece)

> **Minh họa tấn công DoS:**
> *Script Python gửi gói tin liên tục và bị Server ngắt kết nối cưỡng chế*  
> ![DoS Block Attack Log](https://github.com/user-attachments/assets/b4253932-5d69-4810-9732-62ffbda5bfd6)

---

## ⚔️ Cơ chế Tấn công & Giải pháp Phòng thủ (Attack Mechanism & Defense Solutions)

Chi tiết các kỹ thuật tấn công đã được mô phỏng và ngăn chặn trong dự án:

| Loại Tấn công | Cơ chế Tấn công (Red Team Attack) | Giải pháp Phòng thủ (Blue Team Defense) |
| :--- | :--- | :--- |
| **Replay Attack** | Bắt gói tin hợp lệ và gửi lại liên tục. | Server ngắt kết nối nếu độ lệch thời gian > 3s.  |
| **DoS** | Spam hàng loạt gói tin rác để làm treo Server. | Chặn IP ngay lập tức khi vượt ngưỡng request. |
| **Buffer Overflow** | Gửi chuỗi ký tự khổng lồ để gây tràn RAM. | Server ngắt kết nối nếu gói tin > 4KB. |
| **Brute Force** | Dò mật khẩu đăng nhập liên tục. | Chặn IP ngay lập tức sau 5 lần sai liên tiếp. |
| **Man-in-the-Middle** | Nghe lén nội dung gói tin. | Dữ liệu được mã hóa 2 lớp, hacker không thể đọc nội dung. |

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
* **Sảnh chờ:**
    * Người chơi có thể tạo phòng.
    * Người chơi khác nhập ID để vào phòng.
* **Lưu trữ:**
    * Tự động lưu kết quả sau mỗi trận đấu vào cơ sở dữ liệu.
    * Xem Bảng xếp hạng các người chơi điểm cao.

---

## 📂 Cấu trúc dự án

```bash
Pixel-Drift/
├── 📂 Pixel-Drift-Server/     # Backend 
│   ├── Backend.sln            
│   ├── Pixel_Drift_Server/    
│   │   ├── TCP_Handler.cs     
│   │   ├── Security_Logger.cs 
│   │   └── ...
│
├── 📂 Pixel-Drift-Client/     # Frontend
│   ├── Frontend.sln           
│   ├── Setup_Game/            
│   ├── Pixel_Drift_Client/    
│   │   ├── Network_Handle.cs  
│   │   └── ...
│
├── 📂 Pixel-Drift-SIEM/      
│   ├── docker-compose.yml     
│   └── logstash.conf         
│
├── 📂 Attack-Scripts/         # Bộ công cụ tấn công giả lập
│   ├── DoS_Attack.py         
│   ├── Replay_Attack.py       
│   └── Overflow_Attack.py     
│
└── 📄 README.md                
```

---

## 🛠 Hướng dẫn cài đặt

---

### A. Dành cho Người chơi (Player Mode)
*Mục đích: Chỉ cài đặt game để chơi qua mạng LAN.*
1. Tải file **Setup_Game.zip** trong mục Release
2. Giải nén file ra thư mục
3. Chạy file **setup.exe**
4. Cài đặt **Radmin:** https://www.radmin-vpn.com/
5. Mở Radmin chọn **Network**
6. Chọn **Join Network** 
7. Nhập Network name: **Pixel Drift** và Password: **0123456789**
8. Mở Game và bắt đầu chơi thôi!!!  
  
Lưu ý: Nếu xuất hiện Windows protected thì chọn More info -> Run anyway

--- 

### B. Dành cho SOC Analyst (Developer Mode)
*Mục đích: Chạy Server giám sát, xem log và giả lập tấn công.*

#### Bước 1: Khởi động SIEM
Yêu cầu: Máy tính đã cài **Docker Desktop**.

1. Mở thư mục `Pixel-Drift-SIEM`.
2. Mở Terminal, chạy lệnh sau để dựng hệ thống ELK: `docker-compose up -d`
3. Truy cập Dashboard Kibana tại: `http://localhost:5601`.

#### Bước 2: Khởi động Server 
1. Mở `Backend.sln` bằng Visual Studio.
2. Khởi động Server. Server sẽ tự động kết nối tới Logstash.

#### Bước 3: Giả lập Tấn công
Yêu cầu: Máy tính đã cài **Python**.

1. Mở thư mục `Attack-Scripts`.
2. Mở Terminal và chạy thử các kịch bản tấn công: `DoS_Attack.py`, `Replay_Attack.py`, `Overflow_Attack.py`.
3. Quan sát kết quả bị chặn trên **Server Console** và **Kibana Dashboard**.

---
<div align="center">
  <sub>© 2025 Pixel Drift - UIT</sub>
</div>
