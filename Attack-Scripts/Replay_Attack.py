import socket
import json
import time

TARGET_IP = "127.0.0.1"
TARGET_PORT = 1111

def attack_replay():
    try:
        client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        client.connect((TARGET_IP, TARGET_PORT))
        print(f"[*] Connected to {TARGET_IP}:{TARGET_PORT}")

        replay_packet = {
            "action": "ping",
            "timestamp": 123456789,
            "message": "This is a replayed packet"
        }
        
        payload = json.dumps(replay_packet).encode('utf-8') + b'\n'
        client.send(payload)
        print(f"[+] Sent Replay Packet: {replay_packet}")

        time.sleep(1)
        client.close()
        
    except Exception as e:
        print(f"[!] Error: {e}")

if __name__ == "__main__":
    attack_replay()