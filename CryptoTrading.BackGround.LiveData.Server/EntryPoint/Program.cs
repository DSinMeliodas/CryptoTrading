using CryptoTrading.BackGround.LiveData.Server.Core;
using CryptoTrading.Framework.Util.Services;

using Microsoft.Extensions.Hosting;

var service  = ServiceBuilder.BuildConfiguredService<LiveDataServer>(args);
service.Run();