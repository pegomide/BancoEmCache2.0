# Sonar

Run sonar docker

```bash
docker run -d --name sonarqube -e SONAR_ES_BOOTSTRAP_CHECKS_DISABLE=true -p 9000:9000 sonarqube:latest
```

Start sonar scanner

```bash
SonarScanner.MSBuild.exe begin /k:"DBasCache" /d:sonar.host.url="http://127.0.0.1:9000"  /d:sonar.token="sqp_4a5c9b68511ab7c333baf127560e2d31af7dda31"
```

Build the project

```bash
dotnet build
```

End sonar scanner

```bash
D:\repos\sonar-scanner-8.0.3.99785-net-framework\SonarScanner.MSBuild.exe end /d:sonar.token="sqp_4a5c9b68511ab7c333baf127560e2d31af7dda31"
```