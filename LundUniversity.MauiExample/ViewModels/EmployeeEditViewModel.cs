using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LundUniversity.MauiExample.Models;
using LundUniversity.MauiExample.Services;

namespace LundUniversity.MauiExample.ViewModels;

// ViewModel for EmployeeEditPage: loads one employee, edits values, and saves changes.
public partial class EmployeeEditViewModel : ObservableObject
{
	private readonly IUniversityRepository _repository;
	private readonly INavigationService _navigationService;
	private int? _employeeId;

	// [ObservableProperty] creates IsBusy and raises PropertyChanged automatically.
	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private string pageTitle = "Edit Employee";

	[ObservableProperty]
	private int departmentId;

	[ObservableProperty]
	private string employeeNumber = string.Empty;

	[ObservableProperty]
	private string employeeName = string.Empty;

	[ObservableProperty]
	private string employeeSalary = string.Empty;

	[ObservableProperty]
	private string? errorMessage;

	// True when ErrorMessage has user-visible text.
	public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

	// Dependencies are injected by DI:
	// - repository reads/updates employee data
	// - navigation service handles route navigation (back after save/cancel)
	public EmployeeEditViewModel(IUniversityRepository repository, INavigationService navigationService)
	{
		_repository = repository;
		_navigationService = navigationService;
	}

	// Called by EmployeeEditPage.ApplyQueryAttributes(...) after Shell navigation.
	// Source of values: ShellNavigationService.GoToEmployeeEditAsync(...), which passes
	// departmentId/employeeId route parameters via GoToAsync(..., parameters).
	public void SetEmployeeContext(int departmentId, int employeeId)
	{
		DepartmentId = departmentId;
		_employeeId = employeeId;
	}

	// [RelayCommand] generates LoadEmployeeCommand used by XAML.
	// Loads employee data and fills editable fields.
	[RelayCommand]
	private async Task LoadEmployeeAsync()
	{
		if (IsBusy || _employeeId is null)
		{
			return;
		}

		try
		{
			IsBusy = true;
			ErrorMessage = null;

			var employee = await _repository.GetEmployeeAsync(_employeeId.Value);
			if (employee is null)
			{
				ErrorMessage = "Employee not found.";
				return;
			}

			PageTitle = $"Edit {employee.EmpNo}";
			EmployeeNumber = employee.EmpNo;
			EmployeeName = employee.EmpName ?? string.Empty;
			EmployeeSalary = employee.EmpSalary?.ToString("0.##", CultureInfo.InvariantCulture) ?? string.Empty;

			// default(int) is 0; we treat 0 as "not set yet".
			// If DepartmentId was not provided, fall back to employee's current department.
			if (DepartmentId == default)
			{
				DepartmentId = employee.DepartmentId;
			}
		}
		catch (Exception ex)
		{
			ErrorMessage = $"Could not load employee: {ex.Message}";
		}
		finally
		{
			IsBusy = false;
		}
	}

	// [RelayCommand] generates SaveEmployeeCommand used by XAML.
	// Validates input, builds an updated Employee object, saves via repository, then navigates back.
	[RelayCommand]
	private async Task SaveEmployeeAsync()
	{
		if (IsBusy || _employeeId is null)
		{
			return;
		}

		if (!TryParseSalary(EmployeeSalary, out var salary))
		{
			ErrorMessage = "Salary must be a number (example: 65000).";
			return;
		}

		try
		{
			IsBusy = true;
			ErrorMessage = null;

			var updated = new Employee
			{
				EmployeeId = _employeeId.Value,
				EmpNo = EmployeeNumber.Trim(),
				EmpName = string.IsNullOrWhiteSpace(EmployeeName) ? null : EmployeeName.Trim(),
				EmpSalary = salary,
				DepartmentId = DepartmentId
			};

			var wasUpdated = await _repository.UpdateEmployeeAsync(updated);
			if (!wasUpdated)
			{
				ErrorMessage = "Employee could not be updated.";
				return;
			}

			await _navigationService.GoBackAsync();
		}
		catch (Exception ex)
		{
			ErrorMessage = $"Could not save employee: {ex.Message}";
		}
		finally
		{
			IsBusy = false;
		}
	}

	// User canceled editing; just navigate back without saving.
	[RelayCommand]
	private Task CancelAsync()
	{
		return _navigationService.GoBackAsync();
	}

	// Accepts salary input in either invariant or current culture format.
	// Empty input is allowed and is treated as null salary.
	private static bool TryParseSalary(string salaryText, out decimal? salary)
	{
		if (string.IsNullOrWhiteSpace(salaryText))
		{
			salary = null;
			return true;
		}

		if (decimal.TryParse(salaryText.Trim(), NumberStyles.Number | NumberStyles.AllowCurrencySymbol, CultureInfo.InvariantCulture, out var invariantValue))
		{
			salary = invariantValue;
			return true;
		}

		if (decimal.TryParse(salaryText.Trim(), NumberStyles.Number | NumberStyles.AllowCurrencySymbol, CultureInfo.CurrentCulture, out var currentValue))
		{
			salary = currentValue;
			return true;
		}

		salary = null;
		return false;
	}

	// ErrorMessage changes should also refresh HasError in the UI.
	partial void OnErrorMessageChanged(string? value)
	{
		OnPropertyChanged(nameof(HasError));
	}
}
