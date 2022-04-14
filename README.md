
docker run -dt 
-v "C:\Users\y.vorobyev\vsdbg\vs2017u5:/remote_debugger:rw" 
-v "C:\Users\y.vorobyev\AppData\Roaming\Microsoft\UserSecrets:/root/.microsoft/usersecrets:ro" 
-v "C:\Users\y.vorobyev\AppData\Roaming\ASP.NET\Https:/root/.aspnet/https:ro" 
-v "C:\Projects\WavesCS\WavesNft.Api:/app" 
-v "C:\Projects\WavesCS:/src/" 
-v "C:\Users\y.vorobyev\.nuget\packages\:/root/.nuget/fallbackpackages4" 
-v "C:\Program Files (x86)\Microsoft Visual Studio\Shared\NuGetPackages:/root/.nuget/fallbackpackages" 
-v "C:\Program Files (x86)\Microsoft\Xamarin\NuGet\:/root/.nuget/fallbackpackages2" 
-v "C:\Program Files\dotnet\sdk\NuGetFallbackFolder:/root/.nuget/fallbackpackages3" 
-e "ASPNETCORE_LOGGING__CONSOLE__DISABLECOLORS=true" 
-e "ASPNETCORE_ENVIRONMENT=Development" 
-e "ASPNETCORE_URLS=https://+:443;http://+:80" 
-e "DOTNET_USE_POLLING_FILE_WATCHER=1" 
-e "NUGET_PACKAGES=/root/.nuget/fallbackpackages4" 
-e "NUGET_FALLBACK_PACKAGES=/root/.nuget/fallbackpackages;/root/.nuget/fallbackpackages2;/root/.nuget/fallbackpackages3;/root/.nuget/fallbackpackages4" 
-P --name WavesNft.Api --entrypoint tail wavesnftapi:dev -f /dev/null

docker run -dt -p "5038:80" -e "ASPNETCORE_URLS=http://+:80" try1975/wavesnftapi
http://localhost:5038/swagger/index.html

# WavesCS
A C# library for interacting with the Waves blockchain

Supports node interaction, offline transaction signing, Matcher orders, and creating addresses and keys.



## Topic on Waves Forum

Here you we can discuss library usage and further development:

https://forum.wavesplatform.com/t/wavescs-c-client-library-for-waves-api/83

## Getting Started

You can install **WavesPlatform.WavesCS** [NuGet package](https://www.nuget.org/packages/WavesPlatform.WavesCS/) and add it to your project's References and in your code as:
```
using WavesCS;
```

For installation NuGet package from VS Package Manager Console you should use:
```
PM> Install-Package WavesPlatform.WavesCS -Version 1.1.0
```

For installation via UI Package Manager use this [instruction](https://docs.microsoft.com/en-us/nuget/tools/package-manager-ui).

Target framework .NET Framework 4.5.1
## Documentation

The library utilizes classes to represent various Waves data structures and encoding and serialization methods:

- WavesCS.Node
- WavesCS.Order
- WavesCS.OrderBook
- WavesCS.PrivateKeyAccount
- WavesCS.Transaction
- WavesCS.AddressEncoding
- WavesCS.Base58
- WavesCS.Utils


#### Code Example
Code examples are in [WavesCSTests](https://github.com/wavesplatform/WavesCS/tree/master/WavesCSTests) project and [Examples](Examples.md) page.

### Source code
[WavesCS Github repository](https://github.com/wavesplatform/WavesCS
