import socket
import time

TARGET_IP = "127.0.0.1"
TARGET_PORT = 1111

def attack_buffer_overflow():
    try:
        print(f"[*] Connecting to {TARGET_IP}:{TARGET_PORT}...")
        client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        client.connect((TARGET_IP, TARGET_PORT))

        huge_payload = ("A" * 10000) + "\n"
        
        print(f"[*] Sending huge payload ({len(huge_payload)} bytes)...")
        
        client.send(huge_payload.encode('utf-8'))
        
        data = client.recv(1024)
        if not data:
            print("[+] Server disconnected us immediately (Attack Blocked!)")
        else:
            print(f"[-] Server responded: {data}")

        client.close()
        
    except ConnectionResetError:
        print("[+] SUCCESS: Connection forcibly closed by Server!")
    except Exception as e:
        print(f"[!] Error: {e}")

if __name__ == "__main__":
    attack_buffer_overflow()