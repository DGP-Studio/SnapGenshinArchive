# 胡桃 API Client
本仓库包含了与 胡桃API 交互需要的代码
# 胡桃 API
由 DGP Studio 开发部署的一套 **深渊数据统计 API**  
> [参阅 Swagger 文档](https://hutao-api.snapgenshin.com/swagger/index.html)

## 准备工作

```
POST /Auth/Login
Content-Type: text/json; charset=utf-8

{
  "Appid": "your appid",
  "Secret": "your secret"
}
```

``` json
{
    "retcode":0,
    "message":"\u767B\u5F55\u6210\u529F",
    "data":{
        "accessToken":"returned access token"
    }
}
```
> 此处返回的 access token 将用来请求后续的数据接口  
> AccessToken 需要置入后续所有请求的 Auth Header (Jwt/Bearer 均可) 中

## 注意
首先请务必加入 [此页面上的开发群](https://github.com/DGP-Studio/Snap.Genshin) 申请 Id 与 Token  
在取得 appid 与 secret 后才能正常的请求API  