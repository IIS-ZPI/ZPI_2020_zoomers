using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Extensions.DependencyInjection;
using zpi_aspnet_test.Controllers;
using zpi_aspnet_test.DataBaseUtilities.DAOs;
using zpi_aspnet_test.DataBaseUtilities.Interfaces;
using zpi_aspnet_test.ViewModels;

namespace zpi_aspnet_test
{
	[ExcludeFromCodeCoverage]
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

		protected void Application_Error(object sender, EventArgs e)
		{
			var context = ((MvcApplication) sender).Context;

			var exc = Server.GetLastError();
			var controller = DependencyResolver.Current.GetService<ErrorController>();
			var model = new ErrorViewModel();

			switch (exc)
			{
				case HttpException httpException:
					model.ErrorCode = httpException.GetHttpCode().ToString();
					model.ErrorMessage = httpException.GetHtmlErrorMessage();
					break;
				case ArgumentNullException argNullException:
					model.ErrorCode = "404";
					model.ErrorMessage = "Requested site has not been found";
					break;
				default:
					model.ErrorCode = "500";
					model.ErrorMessage = "Internal server error :(";
					break;
			}

			var routeData = new RouteData();
			routeData.Values["controller"] = "Error";
			routeData.Values["action"] = "HandleError";
			context.ClearError();
			context.Response.Clear();
			context.Response.StatusCode = int.Parse(model.ErrorCode);
			context.Response.TrySkipIisCustomErrors = true;
			controller.ViewModel = model;
			((IController) controller).Execute(new RequestContext(new HttpContextWrapper(context), routeData));
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
			services.AddTransient<ErrorController>();
		}
	}

	[ExcludeFromCodeCoverage]
	internal sealed class ServiceDependencyResolver : IDependencyResolver
	{
		private readonly IDependencyResolver _currentResolver;
		private readonly ServiceProvider _provider;

		public ServiceDependencyResolver(IDependencyResolver currentResolver, ServiceProvider provider)
		{
			_currentResolver = currentResolver;
			_provider = provider;
		}

		public object GetService(Type serviceType) =>
			_provider?.GetService(serviceType) ?? _currentResolver?.GetService(serviceType);


		public IEnumerable<object> GetServices(Type serviceType) =>
			_provider?.GetServices(serviceType) ?? _currentResolver?.GetServices(serviceType) ?? new object[0];
	}

	[ExcludeFromCodeCoverage]
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