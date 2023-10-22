cd ./EmptyBlazorApp1

scp -r ./app.db root@89.208.105.220:/root/WebApp

cd ./bin/Release/net7.0/linux-x64/publish

scp -r ./* root@89.208.105.220:/root/WebApp