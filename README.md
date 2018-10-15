# TencentSms
An `ISmsSender` implementation for TencentSms. (aka QcloudSms)

## NuGet
[Myvas.AspNetCore.TencentSms](https://www.nuget.org/packages/Myvas.AspNetCore.TencentSms)

## ConfigureServices
```csharp
services.AddTencentSms(options =>
{
    options.SdkAppId = Configuration["TencentSms:SdkAppId"];
    options.AppKey = Configuration["TencentSms:AppKey"];
});
```

## Inject & Invoke
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

## API Implementation Status
### Plan:
To support the TencentSms API, docs here: https://cloud.tencent.com/document/product/382

Dependencies:
- https://www.nuget.org/packages/qcloud.qcloudsms_csharp

### DONE:
- 国内发送短信（发送一条短信）
- 国内群发短信（提交群发短信）
