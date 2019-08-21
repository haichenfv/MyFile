using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace NetCoreDemo
{
    public static class AlexAuthorizeExtension
    {
        public static void addAlexAuthorize(this IApplicationBuilder applicationBuilder) {
            applicationBuilder.Use(async (currentContext, nextContext) =>
            {
                if (currentContext.Request.Headers.ContainsKey("Authorization"))
                {
                    var authorize = currentContext.Request.Headers["Authorization"].ToString();
                    if (authorize.Contains("Basic"))//如果是Basic 身份认证
                    {
                        var info = authorize.Replace("Basic ", string.Empty);
                        var bytes = Convert.FromBase64String(info);//反解析Basic 64
                        var contents = Encoding.Default.GetString(bytes);
                        var dd = contents.Split(":").ToArray();
                        var userName = dd[0];//用户名
                        var userPwd = dd[1];//密码
                        if (userName == "alex" && userPwd == "123456")
                        {
                            await currentContext.Response.WriteAsync("验证通过").ConfigureAwait(true);
                            await nextContext?.Invoke();
                            return;
                        }
                    }
                }
                currentContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await currentContext.Response.WriteAsync("See tou tomorrow!").ConfigureAwait(true);
            });
        }
    }
}
