using LundUniversity.MauiExample.Navigation;
using LundUniversity.MauiExample.ViewModels;

namespace LundUniversity.MauiExample.Views;

// IQueryAttributable tells MAUI Shell this page can receive navigation query data.
// After GoToAsync(route, parameters), Shell calls ApplyQueryAttributes(...) and
// provides the parameter dictionary so we can pass values to the ViewModel.
public partial class DepartmentDetailPage : ContentPage, IQueryAttributable
{
	private readonly DepartmentDetailViewModel _viewModel;

	// MAUI creates this page via DI and injects its ViewModel.
	public DepartmentDetailPage(DepartmentDetailViewModel viewModel)
	{
		// Loads controls from DepartmentDetailPage.xaml.
		InitializeComponent();

		// Sets the data source used by XAML {Binding ...} expressions.
		BindingContext = _viewModel = viewModel;
	}

	// Shell calls this automatically after navigation with parameters.
	// Here we read departmentId from the query dictionary and pass it to the ViewModel.
	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		if (!TryGetDepartmentId(query, out var departmentId))
		{
			return;
		}

		_viewModel.SetDepartmentId(departmentId);
	}

	// Runs whenever this page becomes visible.
	// We load on appearing so data refreshes when returning from edit pages.
	protected override void OnAppearing()
	{
		base.OnAppearing();
		_viewModel.LoadDepartmentCommand.Execute(null);
	}

	// Reads departmentId from query data.
	// Supports both int and string values for safety across navigation scenarios.
	private static bool TryGetDepartmentId(IDictionary<string, object> query, out int departmentId)
	{
		departmentId = default;

		if (!query.TryGetValue(NavigationParameters.DepartmentId, out var parameter))
		{
			return false;
		}

		if (parameter is int id)
		{
			departmentId = id;
			return true;
		}

		return parameter is string text && int.TryParse(text, out departmentId);
	}
}
