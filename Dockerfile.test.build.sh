docker build -f Dockerfile.test.build -t com-bateeq-service-merchandiser-webapi:test-build . 
docker create --name com-bateeq-service-merchandiser-webapi-test-build-container com-bateeq-service-merchandiser-webapi:test-build 
mkdir -p ./bin/publish
docker cp com-bateeq-service-merchandiser-webapi-test-build-container:/out/. ./bin/publish
docker build -f Dockerfile.test -t com-bateeq-service-merchandiser-webapi:test .