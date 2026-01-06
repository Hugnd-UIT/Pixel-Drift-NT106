import socket

SERVER_IP = "127.0.0.1"
SERVER_PORT = 1111

def buffer_overflow_attack():
    print("\n--- BUFFER OVERFLOW ATTACK ---")
    try:
        print(f"[INFO] Connecting to {SERVER_IP}:{SERVER_PORT}...")
        client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        client.connect((SERVER_IP, SERVER_PORT))

        huge_payload = ("A" * 10000) + "\n"
        
        print(f"[INFO] Sending huge payload ({len(huge_payload)} bytes)...")
        client.send(huge_payload.encode('utf-8'))
        
        data = client.recv(1024)
        if not data:
            print("[SUCCESS] Server disconnected immediately (Attack Blocked)")
        else:
            print(f"[FAIL] Server responded: {data}")

        client.close()
        
    except ConnectionResetError:
        print("[SUCCESS] Connection forcibly closed by Server")
    except Exception as e:
        print(f"[ERROR] {e}")

if __name__ == "__main__":
    buffer_overflow_attack()