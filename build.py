import subprocess
import os

if __name__ == '__main__':
    subprocess.run(["echo", "building...."] )
    #subprocess.run(["make", "clean"])
    subprocess.run(["make"])