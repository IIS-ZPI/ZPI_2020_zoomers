﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Extensions.DependencyInjection;
using zpi_aspnet_test.Controllers;
using zpi_aspnet_test.DataBaseUtilities.DAOs;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;

namespace zpi_aspnet_test
{
	public class MvcApplication : HttpApplication
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
			ControllerBuilder.Current.SetControllerFactory(new ZoomersControllerFactory());
		}

        void Application_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();

            if (exc is HttpUnhandledException)
            {
                // Pass the error on to the error page.
                Server.Transfer("ErrorPage.aspx?handler=Application_Error%20-%20Global.asax", true);
            }
        }

		private void ConfigureServices(IServiceCollection services)
		{
			services.AddScoped<ICategoryDatabaseAccess>(provider => new StandardCategoryDatabaseAccessor());
			services.AddScoped<IStateDatabaseAccess>(provider => new StandardStateDatabaseAccessor());
			services.AddScoped<IProductDatabaseAccess>(provider => new StandardProductDatabaseAccessor());
			services.AddTransient<HomeController>();
			services.AddTransient<CategorySelectionController>();
			services.AddTransient<ProductSelectionController>();
			services.AddTransient<StateSelectionController>();
		}
	}

	internal sealed class ServiceDependencyResolver : IDependencyResolver
	{
		private readonly IDependencyResolver _currentResolver;
		private readonly ServiceProvider _provider;

		public ServiceDependencyResolver(IDependencyResolver currentResolver, ServiceProvider provider)
		{
			_currentResolver = currentResolver;
			_provider = provider;
		}

		public object GetService(Type serviceType) => _provider?.GetService(serviceType) ?? _currentResolver?.GetService(serviceType);


		public IEnumerable<object> GetServices(Type serviceType) =>
			_provider?.GetServices(serviceType) ?? _currentResolver?.GetServices(serviceType) ?? new object[0];
	}

	internal sealed class ZoomersControllerFactory : DefaultControllerFactory
	{
		private readonly IDependencyResolver _resolver;

		public ZoomersControllerFactory()
		{
			_resolver = DependencyResolver.Current;
		}

		protected override IController GetControllerInstance(RequestContext context, Type type)
		{
			return _resolver.GetService(type) as IController;
		}
	}
}
