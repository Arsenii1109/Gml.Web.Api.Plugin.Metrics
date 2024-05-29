using Gml.Web.Api.EndpointSDK;
using Gml.Web.Api.Plugins.Metrics.Dto;
using GmlCore.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Gml.Web.Api.Plugins.Metrics;

[Path("GET", "/api/v1/plugins/users", true)]
public class GetUsers : EndpointHelper, IPluginEndpoint
{
    public async Task Execute(HttpContext context, IGmlManager gmlManager)
    {
        var users = await gmlManager.Users.GetUsers();

        await Ok(context, users, "Пользователи");
    }
}

[Path("POST", "/api/v1/plugins/users/ban", true)]
public class BanUser : EndpointHelper, IPluginEndpoint
{
    public async Task Execute(HttpContext context, IGmlManager gmlManager)
    {
        var body = await ParseDto<UserBanUpdateDto>(context);

        if (body is null || string.IsNullOrEmpty(body.User))
        {
            await BadRequest(context, "Неверный формат данных");
            return;
        }

        var user = await gmlManager.Users.GetUserByName(body.User);

        if (user is null)
        {
            await BadRequest(context, "Данный пользователь не найден");
            return;
        }

        user.IsBanned = body.IsBanned;

        await gmlManager.Users.UpdateUser(user);

        await Ok(context, user, "Пользователи");
    }
}

[Path("GET", "/api/v1/plugins/users/info", true)]
public class ProfileInfo : EndpointHelper, IPluginEndpoint
{
    public async Task Execute(HttpContext context, IGmlManager gmlManager)
    {
        var userName = context.Request.Query["user"];

        if (string.IsNullOrEmpty(userName))
        {
            await BadRequest(context, "Неверный формат данных");
            return;
        }

        var user = await gmlManager.Users.GetUserByName(userName);

        if (user is null)
        {
            await BadRequest(context, "Данный пользователь не найден");
            return;
        }

        await Ok(context, user, "Пользователи");
    }
}