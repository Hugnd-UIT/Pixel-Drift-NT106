import socket
import json
import threading
import time

SERVER_IP = "127.0.0.1"
SERVER_PORT = 1111
THREADS_COUNT = 10
REQUESTS_PER_THREAD = 50 

def attack_simulation(thread_id):
    print(f"[Thread {thread_id}] Bắt đầu gửi dữ liệu...")
    try:
        client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        client.settimeout(5)
        client.connect((SERVER_IP, SERVER_PORT))
        
        for i in range(REQUESTS_PER_THREAD):
            payload = {
                "action": "ping",
                "timestamp": int(time.time() * 10000000)
            }
            message = json.dumps(payload) + "\n"
            
            try:
                client.sendall(message.encode('utf-8'))
            except Exception as e:
                print(f"[Thread {thread_id}] Bị Server ngắt kết nối ở request thứ {i}: {e}")
                break
                
            time.sleep(0.01) 
            
        client.close()
    except Exception as e:
        print(f"[Thread {thread_id}] Không thể kết nối hoặc bị chặn ngay lập tức: {e}")

threads = []
for i in range(THREADS_COUNT):
    t = threading.Thread(target=attack_simulation, args=(i,))
    threads.append(t)
    t.start()

for t in threads:
    t.join()

print("Hoàn tất kiểm thử.")