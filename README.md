# AspNetCore.TencentSms
[![Travis build status](https://img.shields.io/travis/myvas/AspNetCore.QcloudSms.svg?label=travis-ci&style=flat-square&branch=master)](https://travis-ci.org/myvas/AspNetCore.QcloudSms)
[![AppVeyor build status](https://img.shields.io/appveyor/ci/FrankH/AspNetCore-QcloudSms/master.svg?label=appveyor&style=flat-square)](https://ci.appveyor.com/project/FrankH/AspNetCore-QcloudSms)

## What is this?
An AspNetCore `ISmsSender` implementation via TencentSms (aka QcloudSms). (Windows/Linux works!)

## How to Use
### NuGet
https://www.nuget.org/packages/AspNetCore.TencentSms

### Startup/ConfigureServices()
```csharp
services.AddQcloudSms(options =>
{
    options.SdkAppId = Configuration["TencentSms:SdkAppId"];
    options.AppKey = Configuration["TencentSms:AppKey"];
});
```

### Inject & Invoke:
```csharp
private readonly ISmsSender _smsSender;

public XxxController(ISmsSender smsSender)
{
    _smsSender = smsSender ?? throw new ArgumentNullException(nameof(smsSender);
}

public IActionResult Xxx()
{
    //...
    var result = await _smsSender.SendSmsAsync(mobile, content);
}
```

## How to Build & Deploy
* Use Visual Studio 2017 v15.8.2+ with .NET Core SDK v2.1.403+ (dotnet-sdk-2.1.403) installed.
* Run apps on Windows or Linux with .NET Core Runtime v2.1.5+ (dotnet-runtime-2.1.5) installed.

Download from Microsoft's official website: http://asp.net
