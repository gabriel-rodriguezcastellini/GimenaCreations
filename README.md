# Gimena Creations

E-commerce .NET Core application.

## Status (GitHub Actions)

[![.NET](https://github.com/gabriel-rodriguezcastellini/GimenaCreations/actions/workflows/dotnet.yml/badge.svg)](https://github.com/gabriel-rodriguezcastellini/GimenaCreations/actions/workflows/dotnet.yml) [![CodeQL](https://github.com/gabriel-rodriguezcastellini/GimenaCreations/actions/workflows/codeql.yml/badge.svg)](https://github.com/gabriel-rodriguezcastellini/GimenaCreations/actions/workflows/codeql.yml)

## Getting Started

Make sure you have installed [memurai](https://www.memurai.com/), [ngrok](https://ngrok.com/) and [seq](https://datalust.co/) in your environment. After that, you can run the below command from the **/ngrok/** executable and replace the first part of the NotificationUrl value of the appsettings.json with the one that is returned by ngrok.

```powershell
ngrok http https://localhost:7217
```

You should be able to browse different components of the application by using the below URLs :

```
Clients : https://localhost:7217/
Admins  :  https://localhost:7217/Admin
Health checks :  https://localhost:7217/hc-ui#/healthchecks
Seq :  http://localhost:5341
```

### Architecture overview

The architecture proposes an all-in-one oriented architecture implementation using HTTPS.
