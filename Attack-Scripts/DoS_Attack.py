import socket
import time
import json
import threading

TARGET_IP = "127.0.0.1"
TARGET_PORT = 1111

def attack_dos():
    try:
        client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        client.connect((TARGET_IP, TARGET_PORT))
        print(f"[*] Connected to {TARGET_IP}:{TARGET_PORT}")

        payload = json.dumps({"action": "ping"}).encode('utf-8') + b'\n'
        
        for i in range(50):
            client.send(payload)
            print(f"[+] Sent packet {i+1}")
            time.sleep(0.01) 

        client.close()
    except Exception as e:
        print(f"[!] Error: {e}")

if __name__ == "__main__":
    print("--- STARTING DOS ATTACK SIMULATION ---")
    attack_dos()