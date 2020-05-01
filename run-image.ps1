#Runs the monitor and outputs the connectionmonitor.csv file in the c:\temp dir
powershell "docker run -d -it -v c:/temp:/var/log glav/connectionmonitor"