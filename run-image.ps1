#Runs the monitor and outputs the connectionmonitor.csv file in the c:\temp dir
powershell "docker run -d -it -v 'c:/development/dev projects/nbnconnectionmonitor:/var/log' glav/connectionmonitor"