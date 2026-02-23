using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LundUniversity.MauiExample.Models;
using LundUniversity.MauiExample.Services;

namespace LundUniversity.MauiExample.ViewModels;

// ViewModel for DepartmentDetailPage: shows one department and its employees,
// and exposes commands for reload/edit/back actions.
public partial class DepartmentDetailViewModel : ObservableObject
{
	private readonly IUniversityRepository _repository;
	private readonly INavigationService _navigationService;
	private int? _departmentId;

	public ObservableCollection<Employee> Employees { get; } = [];

	// [ObservableProperty] creates IsBusy and raises PropertyChanged automatically.
	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private string pageTitle = "Department Details";

	[ObservableProperty]
	private string departmentName = "Department";

	[ObservableProperty]
	private string departmentBudget = string.Empty;

	[ObservableProperty]
	private string statusMessage = "Select a department to view employees.";

	[ObservableProperty]
	private string? errorMessage;

	// True when ErrorMessage has user-visible text.
	public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

	// Dependencies are injected by DI:
	// - repository fetches department/employee data
	// - navigation service handles Shell routes
	public DepartmentDetailViewModel(IUniversityRepository repository, INavigationService navigationService)
	{
		_repository = repository;
		_navigationService = navigationService;
	}

	// Called by the page after navigation parameters are received.
	public void SetDepartmentId(int departmentId)
	{
		_departmentId = departmentId;
	}

	// [RelayCommand] generates LoadDepartmentCommand used by XAML.
	// Loads one department and all employees for that department.
	[RelayCommand]
	private async Task LoadDepartmentAsync()
	{
		if (IsBusy || _departmentId is null)
		{
			return;
		}

		try
		{
			IsBusy = true;
			ErrorMessage = null;

			var detail = await _repository.GetDepartmentDetailAsync(_departmentId.Value);
			if (detail is null)
			{
				DepartmentName = "Department not found";
				DepartmentBudget = string.Empty;
				StatusMessage = "The selected department no longer exists.";
				Employees.Clear();
				return;
			}

			PageTitle = detail.Department.DeptName;
			DepartmentName = detail.Department.DeptName;
			DepartmentBudget = detail.Department.DeptBudgetDisplay;

			Employees.Clear();
			foreach (var employee in detail.Employees)
			{
				Employees.Add(employee);
			}

			StatusMessage = Employees.Count == 1
				? "1 employee in this department"
				: $"{Employees.Count} employees in this department";
		}
		catch (Exception ex)
		{
			ErrorMessage = $"Could not load department: {ex.Message}";
		}
		finally
		{
			IsBusy = false;
		}
	}

	// Runs when the user taps Edit on an employee row.
	// Navigates to the employee edit route with department/employee IDs.
	[RelayCommand]
	private Task EditEmployeeAsync(Employee? employee)
	{
		if (employee is null || IsBusy)
		{
			return Task.CompletedTask;
		}

		var departmentId = _departmentId ?? employee.DepartmentId;
		return _navigationService.GoToEmployeeEditAsync(departmentId, employee.EmployeeId);
	}

	// Runs when user taps Back in the toolbar.
	[RelayCommand]
	private Task GoBackAsync()
	{
		return _navigationService.GoBackAsync();
	}

	// ErrorMessage changes should also refresh HasError in the UI.
	partial void OnErrorMessageChanged(string? value)
	{
		OnPropertyChanged(nameof(HasError));
	}
}
