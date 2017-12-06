FROM microsoft/	
ARG source
WORKDIR /app
EXPOSE 80
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "Com.Bateeq.Service.Merchandiser.WebApi.dll"]
