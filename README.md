# Print A Pic
Affordable wireless mobile photo printing booth for personal/commercial purpose.



# MobPrntBoooth

Headless pi config steps --
https://hackernoon.com/raspberry-pi-headless-install-462ccabd75d0

- Burn image in raspberry (pi/jamun123)
- Enable remote connections in raspberry config.
- Find PI's IP (by connecting with router and looking into its IP allocation table). For Pi zero, connect with wifi use wpa_supplicant.conf file for wifi credentials. Refer link: https://www.losant.com/blog/getting-started-with-the-raspberry-pi-zero-w-without-a-monitor
- connect to PI via putty, use above ip address.
- Enable vnc server, run command: sudo raspi-config and in interfacing-options, enable vnc
- Install VNC on laptop
- Connect a router to pi, configure static ip in router for pi.
- Connect to router via laptop and connect via VNC, change password
- install cups and follow below -

https://www.howtogeek.com/169679/how-to-add-a-printer-to-your-raspberry-pi-or-other-linux-computer/
For old printer models - http://foo2zjs.rkkda.com/

- install FTP server on raspberry-
https://www.raspberrypi.org/documentation/remote-access/ftp.md
upload/jamun123

if needed------

command line options cups
https://www.cups.org/doc/options.html

python access
- sudo apt-get install libcups2-dev
- pip install pycups

# FOR Router configuration
- Install RaspApi - https://github.com/billz/raspap-webgui
- Set port redirection - > 
The easiest way to come about this is properly installing dnsmasq (which is a DNS cacheing server) then in the folder /etc/dnsmasq.d add a file for each domain you want to redirect.
For instance this is the contents of /etc/dnsmasq.d/hotmail.com on my system:
address=/hotmail.com/127.0.0.1
address=/www.hotmail.com/127.0.0.1
#Add domains which you want to force all urls to an IP address here.
address=/#/192.168.1.245

  
# Notes for OCR --
https://www.truiton.com/2016/11/optical-character-recognition-android-ocr/
https://github.com/priyankvex/Easy-Ocr-Scanner-Android
