import socket
import time

SERVER_IP = '127.0.0.1'
SERVER_PORT = 1111

def slowloris_attack():
    print("\n--- SLOWLORIS ATTACK ---")
    try:
        client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        client.settimeout(10.0) 
        print(f"[INFO] Connecting to {SERVER_IP}:{SERVER_PORT}...")
        client.connect((SERVER_IP, SERVER_PORT))
        print("[INFO] Connected successfully")

        print("[INFO] Sleeping for 6 seconds to stall server...")
        time.sleep(6)

        print("[INFO] Waking up. Sending data...")
        msg = '{"action":"ping"}\n'
        client.sendall(msg.encode('utf-8'))

        response = client.recv(1024)
        if not response:
            print("[SUCCESS] Server closed connection (No response received)")
        else:
            print(f"[FAIL] Server still responded: {response.decode('utf-8')}")

    except (ConnectionResetError, BrokenPipeError):
        print("[SUCCESS] Server forcibly closed connection")
    except socket.timeout:
        print("[SUCCESS] Socket timeout reached")
    except Exception as e:
        print(f"[ERROR] {e}")
    finally:
        client.close()

if __name__ == "__main__":
    slowloris_attack()