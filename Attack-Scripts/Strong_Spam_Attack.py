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

def strong_spam_attack(retry_count=6):
    print("\n--- STRONG SPAM ATTACK ---")
    client = connect_to_server()
    if not client:
        return

    try:
        print("\n[STEP 1] Sending Handshake (get_public_key)...")
        handshake_payload = json.dumps({"action": "get_public_key"}) + "\n"
        client.sendall(handshake_payload.encode("utf-8"))

        try:
            handshake_resp = client.recv(4096).decode("utf-8").strip()
            print(f" [HANDSHAKE] Server response: {handshake_resp}")
            
            if "error" in handshake_resp.lower():
                print(" [FAIL] Handshake failed. Stopping attack.")
                client.close()
                return
        except socket.timeout:
            print(" [FAIL] Handshake timeout. Server ignored us.")
            client.close()
            return

        print("\n[STEP 2] Handshake successful. Starting spam loop...")
        
        payload = {
            "action": "create_room",
            "username": "Spam_Tester"
        }

        message = json.dumps(payload) + "\n"

        for i in range(1, retry_count + 1):
            print(f"\n[Attempt {i}] Sending create_room request...")
            client.sendall(message.encode("utf-8"))

            try:
                data = client.recv(4096)
                if not data:
                    print(" [WARN] Server closed connection (No Data).")
                    break
                
                response = data.decode("utf-8").strip()
                print(f" [RECV] {response}")

                if any(x in response.lower() for x in ["error", "spam", "too fast"]):
                    print(" [SUCCESS] Server blocked spam.")
                elif "success" in response.lower():
                    print(" [FAIL] Server allowed request.")
                else:
                    print(" [INFO] Unknown response.")

            except socket.timeout:
                print(" [WARN] Timeout - No response.")
            except ConnectionResetError:
                print(" [WARN] Connection forcibly closed.")
                break

            time.sleep(0.5)

        client.close()
    except Exception as e:
        print(f"[ERROR] {e}")

if __name__ == "__main__":
    strong_spam_attack()