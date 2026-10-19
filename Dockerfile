FROM mcr.microsoft.com/playwright/dotnet:v1.63.0-noble

WORKDIR /src

COPY . .

RUN dotnet restore DemoQA.sln

CMD ["dotnet", "test", "DemoQA.sln", "--no-restore", "--settings", ".runsettings", "--configuration", "Release", "--logger", "console;verbosity=normal"]
