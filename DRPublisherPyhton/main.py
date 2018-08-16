from Publisher import *
from States import *
import time

def main():
    q = QLearning()
    q.update()


class QLearning():
    def __init__(self):
        self.pub = Publisher(self.update, self.rate);

    def update(self):
        while True:

            time.sleep(1.0/self.rate)






if __name__ == "__main__":
    main()
