# ---------- build ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 先複製 sln 與各 csproj（加速還原快取）
COPY *.sln ./
COPY HrPortal.Web/HrPortal.Web.csproj HrPortal.Web/
COPY HrPortal.EF/HrPortal.EF.csproj HrPortal.EF/
COPY HrPortal.IoC/HrPortal.IoC.csproj HrPortal.IoC/
COPY HrPortal.Models/HrPortal.Models.csproj HrPortal.Models/
COPY HrPortal.Services/HrPortal.Services.csproj HrPortal.Services/
COPY HrPortal.Shared/HrPortal.Shared.csproj HrPortal.Shared/

RUN dotnet restore HrPortal.Web/HrPortal.Web.csproj

# 再複製所有原始碼 & 發佈
COPY . .
RUN dotnet publish HrPortal.Web/HrPortal.Web.csproj -c Release -o /app/publish /p:UseAppHost=false

# ---------- runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# 只固定服務端口，其餘設定都用 docker run -e 覆蓋
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "HrPortal.Web.dll"]
