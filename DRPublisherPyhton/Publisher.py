import socket
import sys
import flatbuffers as fb
from threading import Thread
import threading
import time
from .ClientProperty import *
class Publisher():

    def __init__(self, name, serverIP="127.0.0.1", serverPort = 10000):
        self.IsTimeToStop = False
        self.name = name
        self.clients = []
        self.clientAddresses = []
        # creat tcp socket
        self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

        # connect
        self.serverIP = serverIP
        self.server_address = (serverIP, 10000)
        self.sock.connect(self.server_address)

        # send my info
        buf = self.prepFBmyInfo()
        self.sock.sendall(buf)

        # receive
        self.thread = Thread(target=self.receive)
        self.thread.start()

    def terminate(self):
        self.IsTimeToStop = True
        self.publisher.close()
        self.sock.close()

    def prepFBmyInfo(self):
        b = fb.Builder(0)
        nodeName = b.CreateString(self.name)
        targetNodeName = b.CreateString('0')
        targetIP = b.CreateString('0.0.0.0')
        myIp = b.CreateString(self.serverIP)
        ClientPropertyStart(b)
        ClientPropertyAddNodeName(b,nodeName)
        ClientPropertyAddTargetIP(b, targetIP)
        ClientPropertyAddTargetNodeName(b, targetNodeName)
        ClientPropertyAddMyIp(b, myIp)
        ClientPropertyAddIsPublisher(b, 1)
        endLine = ClientPropertyEnd(b)
        fileID = self.getByteArr("CLPR")
        b.FinishWithFileIdentifier(endLine, fileID)
        buf = b.Output()
        return buf

    def getByteArr(self,string):
        if sys.version_info < (3, 0):
            fileID = []
            for i in range(0, len(string)):
                fileID.append(ord(string[i]))
        else:
            fileID = "CLPR".encode()
        return fileID

    def receive(self):
        while not self.IsTimeToStop:
            print("alaive")
            try:
                buf = self.sock.recv(1024)
            except:
                self.IsTimeToStop = True
            if fb.Table.HasFileIdentifier(buf, "CLPR".encode()):
                self.parseClientProperty(buf)
            time.sleep(1)
        print('node stopping...')

    def parseClientProperty(self, buf):
        cp = ClientProperty.GetRootAsClientProperty(buf, 0)
        self.topicPort = cp.TopicPort()
        self.publish()

    def publish(self):
        addr = ('', self.topicPort)
        self.publisher = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.publisher.bind(addr)
        self.publisher.listen(100)
        self.pubThread = Thread(target=self.accept_incoming_connections)
        self.pubThread.start()

    def accept_incoming_connections(self):
        while not self.IsTimeToStop:
            try:
                client, client_address = self.publisher.accept()
                self.clients.append(client)
                self.clientAddresses.append(client_address)
                print('new subscriber attached')
            except:
                self.IsTimeToStop = True
        print('listener stopping...')

    def BroadCast(self, buf):
        deadClients = []
        for i in range(0, len(self.clients)):
            client = self.clients[i]
            try :
                client.sendall(buf)
            except:
                deadClients.append(i)

#//
