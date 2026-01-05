import socket
import time
import sys

SERVER_IP = '127.0.0.1'
SERVER_PORT = 1111

def test_zombie_connection():
    print("\n--- TEST 1: ZOMBIE CONNECTION (Thử câu giờ 6 giây) ---")
    try:
        client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        client.settimeout(10.0) 
        print(f"[*] Đang kết nối đến {SERVER_IP}:{SERVER_PORT}...")
        client.connect((SERVER_IP, SERVER_PORT))
        print("[*] Kết nối thành công!")

        print("[*] Đang ngủ 6 giây để thử thách kiên nhẫn của Server...")
        time.sleep(6)

        print("[*] Hết giờ ngủ! Đang cố gửi dữ liệu 'Hello'...")
        msg = '{"action":"ping"}\n'
        client.sendall(msg.encode('utf-8'))

        response = client.recv(1024)
        if not response:
            print("[THÀNH CÔNG] ✅ Server đã đóng kết nối (Không nhận được phản hồi).")
        else:
            print(f"[THẤT BẠI] ❌ Server vẫn trả lời: {response.decode('utf-8')}")
            print("   -> Kiểm tra lại xem bạn đã set ReceiveTimeout = 5000 chưa?")

    except (ConnectionResetError, BrokenPipeError):
        print("[THÀNH CÔNG] ✅ Server đã đá đít Client (Connection Reset/Broken Pipe)!")
    except socket.timeout:
        print("[THÀNH CÔNG] ✅ Socket bị ngắt kết nối (Timeout).")
    except Exception as e:
        print(f"[LỖI] {e}")
    finally:
        client.close()

def test_normal_connection():
    print("\n--- TEST 2: KẾT NỐI BÌNH THƯỜNG (Gửi ngay lập tức) ---")
    try:
        client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        client.settimeout(5.0)
        client.connect((SERVER_IP, SERVER_PORT))
        
        print("[*] Gửi lệnh 'ping' ngay lập tức...")
        msg = '{"action":"ping"}\n'
        client.sendall(msg.encode('utf-8'))
        
        print("[*] Đã gửi xong. Nếu không bị lỗi Socket là OK.")
        time.sleep(1) 
        print("[THÀNH CÔNG] ✅ Kết nối vẫn sống.")
        
    except Exception as e:
        print(f"[THẤT BẠI] ❌ Lỗi không mong muốn: {e}")
    finally:
        client.close()

if __name__ == "__main__":
    test_zombie_connection()
    test_normal_connection()