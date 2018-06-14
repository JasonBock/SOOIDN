using Autofac;
using SOOIDN.Api.Logging;

namespace SOOIDN.Api
{
	public sealed class ApiModule
		: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);
			builder.RegisterType<SerilogLogging>().As<ILogger>().SingleInstance();
		}
	}
}
