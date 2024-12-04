$ docker run -d --name sonarqube -e SONAR_ES_BOOTSTRAP_CHECKS_DISABLE=true -p 9000:9000 sonarqube:latest

 D:\repos\sonar-scanner-8.0.3.99785-net-framework\SonarScanner.MSBuild.exe begin /k:"DBasCache" /d:sonar.host.url="http://127.0.0.1:9000"  /d:sonar.token="sqp_4a5c9b68511ab7c333baf127560e2d31af7dda31"

 dotnet build

 D:\repos\sonar-scanner-8.0.3.99785-net-framework\SonarScanner.MSBuild.exe end /d:sonar.token="sqp_4a5c9b68511ab7c333baf127560e2d31af7dda31"