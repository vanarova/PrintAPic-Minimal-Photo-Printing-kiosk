-------------------------------------------	
For Further Configuration- 
-------------------------------------------	
1. Open config and set values marked with:  <!--Mandatory setting-->
2. To Set up Raspberry pi as router, follow instructions below -

-------------------------------------------	
Configure Raspberry pi as router steps 
-------------------------------------------	

1.Install Raspsp, steps are given in quick installer section in raspap github (https://github.com/billz/raspap-webgui)
2.After install, raspberry will become router with a wlan access point.
3.Connect to raspberry pi wifi from pc, Its wifi name is raspap_webui and password is ChangeMe
4.Open browser and enter IP address: 10.3.141.1 Username: admin Password: secret Goto connected guest client section and note the IP address of PC assigned. (This can also be done by simply using ipconfig on PC itself)
5.To make raspap forward all requests to a particular IP, go to etc\dnsmasq.conf file and add a line
address=/print.it/127.0.0.1 address=/www.print.it/127.0.0.1 for all addresses rediecetion - address=/#/127.0.0.1
Enter IP of your pc in above lines.
6.On PC, go to IIS server and configure it so that it listens to all 8080 ports and serves website.
7.Now raspberry will redirect all request to website hosted on PC and website will apprear


-------------------------------------------
Configure IIS server
-------------------------------------------

1. Install IIS server from windows programs and features.
2. Run Second installation package to install website in IIS wwwroot directory.
3. After website is installed, Goto uploads directory and change it permission to allow read/write by IIS_USER & OWNER user type


-------------------------------------------
Configure Raspberry based router
-------------------------------------------

1. In raspberry, open router panel : Open browser and enter IP address: 10.3.141.1 Username: admin Password: secret.
2. Configure router to have static address for tablet.

