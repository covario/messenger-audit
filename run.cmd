SET CONFIGURATION=Development
SET VERSION_FULL=1.0.0
SET ARTIFACT_PATH=%CD%/Artifact
docker build . -t tradingbuild
docker run -d --name tradingapp -p 5002:80 tradingbuild
pause
docker stop tradingapp
docker rm tradingapp
