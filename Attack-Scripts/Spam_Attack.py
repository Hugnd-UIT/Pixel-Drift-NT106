import socket
import json
import time
import sys

SERVER_IP = "127.0.0.1"
SERVER_PORT = 1111

def connect_to_server():
    """Tạo kết nối socket đến server."""
    try:
        client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        client.settimeout(3.0) 
        client.connect((SERVER_IP, SERVER_PORT))
        print(f"\n[INFO] Đã kết nối thành công đến {SERVER_IP}:{SERVER_PORT}")
        return client
    except ConnectionRefusedError:
        print(f"\n[LỖI] Không thể kết nối đến Server! Hãy chắc chắn Server đang chạy.")
        return None
    except Exception as e:
        print(f"\n[LỖI] {e}")
        return None

def test_ping_flood(packet_count=50, delay=0.01):
    print(f"\n--- BẮT ĐẦU PING FLOOD ({packet_count} gói tin) ---")
    client = connect_to_server()
    if not client: return

    try:
        payload = json.dumps({"action": "ping"}).encode('utf-8') + b'\n'
        
        for i in range(packet_count):
            client.send(payload)
            print(f" -> [Ping {i+1}/{packet_count}] Đã gửi gói tin...")
            time.sleep(delay) 

        print(f"\n[HOÀN TẤT] Đã gửi xong {packet_count} gói tin Ping.")
        client.close()
    except Exception as e:
        print(f"[LỖI] Quá trình gửi bị gián đoạn: {e}")

def test_action_spam(retry_count=6):
    print(f"\n--- BẮT ĐẦU KIỂM TRA LOGIC CHẶN SPAM (Create Room) ---")
    client = connect_to_server()
    if not client: return

    try:
        spam_payload = {
            "action": "create_room",
            "username": "Spam_Tester"
        }
        message = json.dumps(spam_payload) + "\n"

        for i in range(1, retry_count + 1):
            print(f"\n[Lần {i}] Đang gửi lệnh 'create_room'...")
            client.sendall(message.encode('utf-8'))

            try:
                response = client.recv(4096).decode('utf-8').strip()
                print(f"   => Server trả lời: {response}")

                if "error" in response.lower() or "too fast" in response.lower() or "spam" in response.lower():
                    print("   ✅ [THÀNH CÔNG] Server đã CHẶN hành động này!")
                elif "success" in response.lower():
                    print("   ❌ [CẢNH BÁO] Server vẫn cho phép tạo phòng!")
                else:
                    print("   ⚠️ [INFO] Phản hồi không xác định.")
            
            except socket.timeout:
                print("   ⚠️ [TIMEOUT] Server không phản hồi (Có thể đã bị Firewall/Blacklist tạm thời).")
            
            time.sleep(0.5)

        client.close()
    except Exception as e:
        print(f"[LỖI] {e}")

if __name__ == "__main__":
    while True:
        print("\n" + "="*40)
        print("   TOOL KIỂM TRA BẢO MẬT SERVER  ")
        print("="*40)
        print("1. Ping Flood (Gửi liên tục không chờ phản hồi - Test TCP Stream)")
        print("2. Action Spam (Gửi lệnh logic game & check phản hồi - Test Rate Limit)")
        print("0. Thoát")
        
        choice = input("\nChọn chế độ (0-2): ").strip()

        if choice == '1':
            count = input("Nhập số lượng gói tin (mặc định 50): ")
            count = int(count) if count.isdigit() else 50
            test_ping_flood(packet_count=count)
        elif choice == '2':
            test_action_spam()
        elif choice == '0':
            print("Đã thoát.")
            sys.exit()
        else:
            print("Lựa chọn không hợp lệ.")