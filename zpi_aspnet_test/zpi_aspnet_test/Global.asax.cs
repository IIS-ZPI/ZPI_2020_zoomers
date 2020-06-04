using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace zpi_aspnet_test
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var services = new ServiceCollection(); 
            
            ConfigureServices(services);

            var provider = services.BuildServiceProvider();
            var currentResolver = DependencyResolver.Current;
            var serverDependencyResolver = new ServiceDependencyResolver(currentResolver, provider);

            DependencyResolver.SetResolver(serverDependencyResolver);
        }

        private void ConfigureServices(IServiceCollection services)
        {
	        
        }
    }

    internal class ServiceDependencyResolver : IDependencyResolver
    {
	    private readonly IDependencyResolver _currentResolver;
	    private readonly ServiceProvider _provider;

	    public ServiceDependencyResolver(IDependencyResolver currentResolver, ServiceProvider provider)
	    {
		    _currentResolver = currentResolver;
		    _provider = provider;
	    }

	    public object GetService(Type serviceType)
	    {
		    var result = _provider.GetService(serviceType);
		    return result;
	    }

	    public IEnumerable<object> GetServices(Type serviceType)
	    {
		    throw new NotImplementedException();
	    }
    }
}
