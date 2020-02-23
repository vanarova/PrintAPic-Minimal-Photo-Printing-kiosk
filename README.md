# Print A Pic 

# What it is? 
A economical mobile/cellphone photos printing solution. Connect any mobile with small windows's based PC tablet and transfer pics wirelessly using wifi, No cables needed.
Features: 
  1. Auto resize photo to A4, A5, Postcard, passport sizes
  2. Prints receipts using thermal printer
  3. Print PDF files
  4. Multi-lingual
  5. Fully wireless printing, can also be used as a print server/Network printer.
  
Demo Video :
https://youtu.be/LsZvIBa5tQA


  
  
# Raspberry installation of Raspap router --
Steps --
1. Install Raspap, steps are given in quick installer section in raspap github.
2. After install, raspberry will become router with a wlan access point.
3. Connect to raspberry pi wifi from pc, Its wifi name is raspap_webui and password is ChangeMe
4. Open browser and enter IP address: 10.3.141.1
Username: admin
Password: secret
Goto connected guest client section and note the IP address of PC assigned. (This can also be done by simply using ipconfig on PC itself)
5. To make raspap forward all requests to a particular IP, go to etc\dnsmasq.conf file and add a line 

address=/print.it/10.3.141.242
address=/www.print.it/10.3.141.242

or
best is assign all ips to redirect to a address
for all addresses rediecetion -
address=/#/10.3.141.242
NOTE: Here 10.3.141.242 is address for machine hosting website.

You can reserve STATIC IP addresses for various clients in your network by writing the reservations in /etc/dnsmasq.conf. The static lease takes the form -
dhcp-host=[mac address],[ip address].

Enter IP of your pc in above lines.
6. On PC, go to IIS server and configure it so that it listens to all 8080 ports and serves website.
7. Now raspberry will redirect all request to website hosted on PC and website will apprear
  


# Post installation notes -- 

# How to enable or disable AutoPlay
This settings supress windows behaviour to auto open USB folder once USB is plugged in.
Use the Windows key + I keyboard shortcut to open the Settings app.
Click Devices.
Click AutoPlay.
Turn on or off the "Use AutoPlay for media and devices".

# Give full access rights to
uploads and thumbs folder inside website.. Else app wont work



#########################################################################
------------------------------------------------------------------------------------------------------------
# References

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

FOR Router configuration
- Install RaspApi
- Set port redirection - > 
The easiest way to come about this is properly installing dnsmasq (which is a DNS cacheing server) then in the folder /etc/dnsmasq.d add a file for each domain you want to redirect.
For instance this is the contents of /etc/dnsmasq.d/hotmail.com on my system:
address=/hotmail.com/127.0.0.1
address=/www.hotmail.com/127.0.0.1
#Add domains which you want to force all urls to an IP address here.
address=/#/192.168.1.245

Also check below link for setting up dnsmasq to redeirect all address to one ip
https://www.stevenrombauts.be/2018/01/use-dnsmasq-instead-of-etc-hosts/.

Notes for OCR --
https://www.truiton.com/2016/11/optical-character-recognition-android-ocr/
https://github.com/priyankvex/Easy-Ocr-Scanner-Android
