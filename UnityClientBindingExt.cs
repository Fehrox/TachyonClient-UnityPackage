using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TachyonClientRPC;

public static class UnityClientBindingExt 
{
    public static TService Bind<TService>(this ClientRpc client) where TService : class
    {
        var serviceTypes = typeof(TService).Assembly
            .GetTypes();
        var serviceType = serviceTypes
            .First(t => 
                typeof(TService).IsAssignableFrom(t) &&
                !t.IsInterface );
        var bindMethod = serviceType.GetMethod("Bind");
        
        var service = Activator.CreateInstance(serviceType);
        bindMethod?.Invoke(service, new[] {client});

        return service as TService;
    }

}
