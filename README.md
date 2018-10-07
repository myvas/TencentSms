# AspNetCore.Sms.Tencent
[![Travis build status](https://img.shields.io/travis/myvas/AspNetCore.QcloudSms.svg?label=travis-ci&style=flat-square&branch=master)](https://travis-ci.org/myvas/AspNetCore.QcloudSms)
[![AppVeyor build status](https://img.shields.io/appveyor/ci/FrankH/AspNetCore-QcloudSms/master.svg?label=appveyor&style=flat-square)](https://ci.appveyor.com/project/FrankH/AspNetCore-QcloudSms)

## What is this?
An AspNetCore `ISmsSender` implementation via TencentSms (aka QcloudSms). (Windows/Linux works!)

## How to Use
### NuGet
[AspNetCore.Sms.Tencent](https://www.nuget.org/packages/AspNetCore.Sms.Tencent)

### Startup/ConfigureServices:
```csharp
services.AddQcloudSms(options =>
{
	options.SdkAppId = Configuration["QcloudSms:SdkAppId"];
	options.AppKey = Configuration["QcloudSms:AppKey"];
});
```

### Inject and use it:
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
