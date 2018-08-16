import socket
import sys
import flatbuffers as fb
from threading import Thread
import time
from ClientProperty import *
class Publisher():

    def __init__(self, pubMethod, rate):
        self.pubMethod = pubMethod
        self.clients = []
        self.clientAddresses = []
        self.rate = rate
        # creat tcp socket
        self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

        # connect
        self.server_address = ('localhost', 10000)
        self.sock.connect(self.server_address)

        # send my info
        buf = self.prepFBmyInfo()
        self.sock.sendall(buf)

        # receive
        self.thread = Thread(target=self.receive)
        self.thread.start()

    def prepFBmyInfo(self):
        b = fb.Builder(0)
        nodeName = b.CreateString('QLearningPy')
        targetNodeName = b.CreateString('')
        myIp = b.CreateString('127.0.0.1')
        ClientPropertyStart(b)
        ClientPropertyAddNodeName(b,nodeName)
        ClientPropertyAddTargetNodeName(b, targetNodeName)
        ClientPropertyAddMyIp(b, myIp)
        ClientPropertyAddIsPublisher(b, 1)
        endLine = ClientPropertyEnd(b)
        b.FinishWithFileIdentifier(endLine, "CLPR".encode())
        buf = b.Output()
        return buf

    def receive(self):
        while 0 < 10:
            print("alaive")
            buf = self.sock.recv(1024)
            if fb.Table.HasFileIdentifier(buf, "CLPR".encode()):
                self.parseClientProperty(buf)
            time.sleep(1)

    def parseClientProperty(self, buf):
        cp = ClientProperty.GetRootAsClientProperty(buf, 0)
        self.topicPort = cp.TopicPort()
        self.publish()

    def publish(self):
        addr = ('127.0.0.1', self.topicPort)
        self.publisher = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.publisher.bind(addr)
        self.publisher.listen()
        Thread(target=self.accept_incoming_connections).start()

    def accept_incoming_connections(self):
        while True:
            client, client_address = self.publisher.accept()
            self.clients.append(client)
            self.clientAddresses.append(client_address)
            print('new subscriber attached')
    #         pubThread = Thread(target=self.handle_client, args=(client,)).start()
    #         print('new subscriber attached')
    #
    # def handle_client(self, client):  # Takes client socket as argument.
    #     res = True
    #     while res:
    #         buf = self.pubMethod()
    #         try :
    #             client.sendall(buf)
    #         except:
    #             res = False
    #             print('gotta stop')
    #             break
    #         time.sleep(1.0/self.rate)
    #     print('end con')

    def BroadCast(self, buf):
        deadClients = []
        for i in range(0, len(self.clients)):
            client = self.clients[i]
            try :
                client.sendall(buf)
            except:
                deadClients.append(i)











#//
