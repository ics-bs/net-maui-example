using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LundUniversity.MauiExample.Models;
using LundUniversity.MauiExample.Services;

namespace LundUniversity.MauiExample.ViewModels;

// ViewModel for MainPage: holds UI state and user actions (commands).
// It should not directly manipulate UI controls (for example setting Button.Text in code).
// Instead, it changes plain C# properties/collections, and MAUI bindings update the controls.
public partial class MainViewModel : ObservableObject
{
	private readonly IUniversityRepository _repository;
	private readonly INavigationService _navigationService;

	public ObservableCollection<Department> Departments { get; } = [];

	// IsBusy exists to:
	// - prevent duplicate overlapping actions (for example double-tapping refresh)
	// - drive busy UI feedback (ActivityIndicator/RefreshView bindings in XAML)
	// [ObservableProperty] generates a public property (for example IsBusy)
	// and raises PropertyChanged automatically when the value changes.
	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private string pageTitle = "University Departments";

	[ObservableProperty]
	private string totalsSummary = "Pull to refresh and load department data.";

	[ObservableProperty]
	private string? errorMessage;

	// Derived UI state: true only when ErrorMessage contains real text.
	// !string.IsNullOrWhiteSpace(ErrorMessage) means:
	// - false when ErrorMessage is null, empty (""), or only spaces
	// - true when it contains a message we want to show in the UI
	public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

	// Dependencies are injected by DI:
	// - repository handles data access
	// - navigation service handles route navigation
	public MainViewModel(IUniversityRepository repository, INavigationService navigationService)
	{
		_repository = repository;
		_navigationService = navigationService;
	}

	// [RelayCommand] generates LoadDepartmentsCommand for XAML bindings.
	// Manual equivalent (without [RelayCommand]) would be something like:
	// public IAsyncRelayCommand LoadDepartmentsCommand { get; }
	// LoadDepartmentsCommand = new AsyncRelayCommand(LoadDepartmentsAsync);
	// This command loads data asynchronously, updates bound collections/properties,
	// and controls busy/error UI state.
	[RelayCommand]
	private async Task LoadDepartmentsAsync()
	{
		if (IsBusy)
		{
			return;
		}

		try
		{
			IsBusy = true;
			ErrorMessage = null;

			var departments = await _repository.GetDepartmentsAsync();

			Departments.Clear();
			foreach (var department in departments)
			{
				Departments.Add(department);
			}

			var totalBudget = departments.Sum(department => department.DeptBudget ?? 0m);
			var totalPayroll = departments.Sum(department => department.TotalSalary);
			TotalsSummary = $"Departments: {departments.Count}  Budget: {totalBudget:C0}  Payroll: {totalPayroll:C0}";
		}
		catch (Exception ex)
		{
			ErrorMessage = $"Could not load departments: {ex.Message}";
		}
		finally
		{
			IsBusy = false;
		}
	}

	// Runs when the user chooses a department (Open button in XAML).
	// Validates input and navigates to the department detail route.
	[RelayCommand]
	private async Task OpenDepartmentAsync(Department? department)
	{
		if (department is null || IsBusy)
		{
			return;
		}

		await _navigationService.GoToDepartmentDetailAsync(department.DepartmentId);
	}

	// ErrorMessage affects HasError, so notify UI that HasError changed too.
	partial void OnErrorMessageChanged(string? value)
	{
		OnPropertyChanged(nameof(HasError));
	}
}
