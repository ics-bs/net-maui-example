using LundUniversity.MauiExample.ViewModels;

namespace LundUniversity.MauiExample.Views;

public partial class MainPage : ContentPage
{
	// Keep a reference to the ViewModel that drives this page's UI state and commands.
	private readonly MainViewModel _viewModel;

	// MAUI creates this page through DI and injects MainViewModel for us.
	public MainPage(MainViewModel viewModel)
	{
		// Loads the controls defined in MainPage.xaml.
		InitializeComponent();

		// Bindings are declared in MainPage.xaml using {Binding ...}.
		// BindingContext tells MAUI which object those bindings should read from.
		// Here, that object is MainViewModel, so XAML can bind to:
		// - data/state properties (for example Departments, IsBusy, ErrorMessage)
		// - command actions (for example LoadDepartmentsCommand, OpenDepartmentCommand)
		BindingContext = _viewModel = viewModel;

		// Triggers the initial load so students see data as soon as the page appears.
		_viewModel.LoadDepartmentsCommand.Execute(null);
	}
}
