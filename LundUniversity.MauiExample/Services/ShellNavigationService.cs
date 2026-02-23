using LundUniversity.MauiExample.Navigation;

namespace LundUniversity.MauiExample.Services;

// Shell is MAUI's built-in navigation container that manages routes and the navigation stack.
// A route is a string name for a destination page (for example "department-detail").
// The navigation stack is the history of opened pages; "back" pops to the previous page.
// This is a service because navigation is shared infrastructure logic used by multiple ViewModels.
// Keeping it here avoids duplicating navigation code and keeps ViewModels free of direct Shell API calls.
public sealed class ShellNavigationService : INavigationService
{
	public Task GoToDepartmentDetailAsync(int departmentId)
	{
		// Shell.Current is the active Shell navigation host at runtime.
		// If it is null, the app is not in a valid Shell navigation context.
		var shell = Shell.Current
			?? throw new InvalidOperationException("Shell navigation is unavailable.");

		// Route parameters are passed as key/value pairs.
		// The target page reads these keys in IQueryAttributable.ApplyQueryAttributes(...).
		var navigationParameters = new Dictionary<string, object>
		{
			[NavigationParameters.DepartmentId] = departmentId
		};

		// GoToAsync(route, parameters) is built into MAUI Shell:
		// - route: destination page name (here DepartmentDetail)
		// - parameters: key/value data for that page (here departmentId)
		// It performs async navigation and delivers parameters to the target page.
		return shell.GoToAsync(ShellRoutes.DepartmentDetail, navigationParameters);
	}

	public Task GoToEmployeeEditAsync(int departmentId, int employeeId)
	{
		var shell = Shell.Current
			?? throw new InvalidOperationException("Shell navigation is unavailable.");

		var navigationParameters = new Dictionary<string, object>
		{
			[NavigationParameters.DepartmentId] = departmentId,
			[NavigationParameters.EmployeeId] = employeeId
		};

		return shell.GoToAsync(ShellRoutes.EmployeeEdit, navigationParameters);
	}

	public Task GoBackAsync()
	{
		var shell = Shell.Current
			?? throw new InvalidOperationException("Shell navigation is unavailable.");

		// ".." means navigate one level back in Shell's navigation stack.
		return shell.GoToAsync("..");
	}
}
