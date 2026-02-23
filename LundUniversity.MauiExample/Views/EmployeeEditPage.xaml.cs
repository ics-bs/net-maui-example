using LundUniversity.MauiExample.Navigation;
using LundUniversity.MauiExample.ViewModels;

namespace LundUniversity.MauiExample.Views;

// IQueryAttributable tells MAUI Shell this page can receive navigation query data.
// After GoToAsync(route, parameters), Shell calls ApplyQueryAttributes(...) and
// provides the parameter dictionary so we can pass values to the ViewModel.
public partial class EmployeeEditPage : ContentPage, IQueryAttributable
{
	private readonly EmployeeEditViewModel _viewModel;

	public EmployeeEditPage(EmployeeEditViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel = viewModel;
	}

	// Shell calls this method automatically because this page implements IQueryAttributable.
	// Flow:
	// 1) A ViewModel calls navigation service -> GoToEmployeeEditAsync(departmentId, employeeId)
	// 2) ShellNavigationService calls Shell.Current.GoToAsync(route, parameters)
	// 3) Shell navigates to this page and provides the parameters dictionary here.
	//
	// In this method we:
	// - read expected keys from the query dictionary ("departmentId", "employeeId")
	// - pass those IDs into the ViewModel (SetEmployeeContext)
	// - trigger loading of the employee data (LoadEmployeeCommand)
	//
	// Using shared key constants from NavigationParameters avoids string-typo bugs
	// between sender (navigation service) and receiver (this page).
	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if (!TryGetInt(query, NavigationParameters.DepartmentId, out var departmentId))
		{
			return;
		}

		if (!TryGetInt(query, NavigationParameters.EmployeeId, out var employeeId))
		{
			return;
		}

		_viewModel.SetEmployeeContext(departmentId, employeeId);
		_viewModel.LoadEmployeeCommand.Execute(null);
	}

	private static bool TryGetInt(IDictionary<string, object> query, string key, out int value)
	{
		value = default;
		if (!query.TryGetValue(key, out var parameter))
		{
			return false;
		}

		if (parameter is int id)
		{
			value = id;
			return true;
		}

		return parameter is string text && int.TryParse(text, out value);
	}
}
