from edcon.edrive.com_modbus import ComModbus
from edcon.edrive.motion_handler import MotionHandler
import socket
import serial
import time
import threading


def get_data(host, port):
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        s.bind((host, port))
        s.listen()

        conn, adr = s.accept()
        with conn:
            data = conn.recv(1024)
            if not data:
                return False
            else:
                return data.decode()


def arduino():
    arduino = serial.Serial('/dev/ttyACM0', 9600, timeout=1)
    host = '0.0.0.0'
    port = 54321

    while True:
        data = get_data(host, port)

        if data:
            if data == "cap":
                print("Capping!")
                arduino.write(b'1')
            elif data == "decap":
                print("Decapping!")
                arduino.write(b'2')
            else:
                print(f"Error when parsing {data}")


def actuator():
    host = '0.0.0.0'
    port = 12345

    while (True):
        com = ComModbus('192.168.10.25')
        if com.connected():
            break

    with MotionHandler(com) as mot:
        mot.acknowledge_faults()
        mot.enable_powerstage()
        mot.referencing_task()

        mot.position_task(-200000, 100, absolute=True)
        time.sleep(2)
        mot.position_task(0, 100, absolute=True)

        while True:
            data = get_data(host, port)

            if data:
                try:
                    position = int(data)
                    if position in range(0, 201):
                        print(f"Going to {position}!")
                        position = -(position * 1000)
                        mot.position_task(position, 100, absolute=True)
                except:
                    print("Data could not be converted to int!")


arduinoThread = threading.Thread(target=arduino)
actuatorThread = threading.Thread(target=actuator)

arduinoThread.start()
actuatorThread.start()
