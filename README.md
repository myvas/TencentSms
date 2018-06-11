# AspNetCore.QcloudSms
[![Travis build status](https://img.shields.io/travis/myvas/AspNetCore.QcloudSms.svg?label=travis-ci&style=flat-square&branch=master)](https://travis-ci.org/myvas/AspNetCore.QcloudSms)
[![AppVeyor build status](https://img.shields.io/appveyor/ci/FrankH/AspNetCore-QcloudSms/master.svg?label=appveyor&style=flat-square)](https://ci.appveyor.com/project/FrankH/AspNetCore-QcloudSms)

## What is this?
An AspNetCore ISmsSender implementation via Tencent Cloud (Qcloud) SMS service. (Windows/Linux works!)

## How to Use
### Startup/ConfigureServices:
```csharp
services.AddQcloudSms(options =>
{
	options.SdkAppId = Configuration["QcloudSms:SdkAppId"];
	options.AppKey = Configuration["QcloudSms:AppKey"];
});
```

### Send the SMS via ISendSms:
```csharp
private readonly ISmsSender _smsSender;
//...
var result = await _smsSender.SendSmsAsync(mobile, content);
```

## How to Build
Use Visual Studio 2017 v15.7.3 and .NET Core 2.1 installed.
Download from Microsoft's official website: http://asp.net
