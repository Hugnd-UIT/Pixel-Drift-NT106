import socket
import json
import time

SERVER_IP = "127.0.0.1"
SERVER_PORT = 1111

def connect_to_server():
    try:
        client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        client.settimeout(3.0)
        client.connect((SERVER_IP, SERVER_PORT))
        print(f"[INFO] Connected to {SERVER_IP}:{SERVER_PORT}")
        return client
    except Exception as e:
        print(f"[ERROR] Cannot connect to server: {e}")
        return None

def weak_spam_attack(packet_count=50, delay=0.01):
    print(f"\n--- WEAK SPAM ATTACK ({packet_count} packets) ---")
    client = connect_to_server()
    if not client:
        return

    try:
        payload = json.dumps({"action": "ping"}).encode("utf-8") + b"\n"

        for i in range(packet_count):
            client.send(payload)
            print(f" -> Ping {i+1}/{packet_count}")
            time.sleep(delay)

        print("[INFO] Weak spam attack completed.")
        client.close()
    except Exception as e:
        print(f"[ERROR] {e}")

if __name__ == "__main__":
    try:
        user_input = input("Number of ping packets (default 50): ")
        count = int(user_input) if user_input.isdigit() else 50
    except:
        count = 50
    weak_spam_attack(count)