using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using LundUniversity.MauiExample.Services;
using LundUniversity.MauiExample.ViewModels;
using LundUniversity.MauiExample.Views;

namespace LundUniversity.MauiExample;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Singleton = one shared instance for the full app lifetime.
		// Use for cross-page services and shared stateful collaborators.
		builder.Services.AddSingleton<IUniversityRepository, InMemoryUniversityRepository>();
		builder.Services.AddSingleton<INavigationService, ShellNavigationService>();

		// MainViewModel is shared with the root page in this sample.
		builder.Services.AddSingleton<MainViewModel>();

		// Transient = a new instance every time it is resolved.
		// Use for per-screen ViewModels with short-lived screen state.
		builder.Services.AddTransient<DepartmentDetailViewModel>();
		builder.Services.AddTransient<EmployeeEditViewModel>();

		// Pages are transient so navigation gets a fresh page instance.
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddSingleton<Func<MainPage>>(serviceProvider => () => serviceProvider.GetRequiredService<MainPage>());
		builder.Services.AddTransient<DepartmentDetailPage>();
		builder.Services.AddTransient<EmployeeEditPage>();

		// AppShell is the app-level navigation host, so keep one instance.
		builder.Services.AddSingleton<AppShell>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
