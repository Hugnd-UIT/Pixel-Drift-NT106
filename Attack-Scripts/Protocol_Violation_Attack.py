import socket
import json

SERVER_IP = '127.0.0.1'
SERVER_PORT = 1111

def protocol_violation_attack():
    print("\n--- PROTOCOL VIOLATION ATTACK ---")
    try:
        client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        client.connect((SERVER_IP, SERVER_PORT))
        
        print("[INFO] Sending 'login' immediately (Violation)...")
        msg = json.dumps({"action": "login", "username": "test", "password": "123"}) + "\n"
        client.sendall(msg.encode('utf-8'))
        
        response = client.recv(1024)
        if not response:
            print("[SUCCESS] Server closed connection immediately")
        else:
            print(f"[FAIL] Server responded: {response.decode('utf-8')}")

    except Exception as e:
        print(f"[SUCCESS] Connection reset: {e}")
    finally:
        client.close()

if __name__ == "__main__":
    protocol_violation_attack()