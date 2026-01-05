import socket
import json
import time

SERVER_IP = '127.0.0.1'
SERVER_PORT = 1111

def test_wrong_protocol():
    print("--- TEST GỬI SAI QUY TRÌNH ---")
    try:
        client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        client.connect((SERVER_IP, SERVER_PORT))
        
        print("[*] Gửi lệnh 'login' ngay lập tức (Sai quy trình)...")
        msg = json.dumps({"action": "login", "username": "test", "password": "123"}) + "\n"
        client.sendall(msg.encode('utf-8'))
        
        response = client.recv(1024)
        if not response:
            print("[THÀNH CÔNG] ✅ Server đã đóng kết nối ngay lập tức!")
        else:
            print(f"[THẤT BẠI] ❌ Server vẫn trả lời: {response.decode('utf-8')}")

    except Exception as e:
        print(f"[THÀNH CÔNG] ✅ Kết nối bị ngắt: {e}")
    finally:
        client.close()

if __name__ == "__main__":
    test_wrong_protocol()