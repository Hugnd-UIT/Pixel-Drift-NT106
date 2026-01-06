import socket
import json
import time

SERVER_IP = "127.0.0.1"
SERVER_PORT = 1111

def replay_attack():
    print("\n--- REPLAY ATTACK ---")
    try:
        client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        client.connect((SERVER_IP, SERVER_PORT))
        print(f"[INFO] Connected to {SERVER_IP}:{SERVER_PORT}")

        replay_packet = {
            "action": "ping",
            "timestamp": 123456789,
            "message": "This is a replayed packet"
        }
        
        payload = json.dumps(replay_packet).encode('utf-8') + b'\n'
        client.send(payload)
        print(f"[INFO] Sent Replay Packet: {replay_packet}")

        time.sleep(1)
        client.close()
        
    except Exception as e:
        print(f"[ERROR] {e}")

if __name__ == "__main__":
    replay_attack()