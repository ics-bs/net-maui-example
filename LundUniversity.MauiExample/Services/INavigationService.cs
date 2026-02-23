namespace LundUniversity.MauiExample.Services;

// Abstraction over app navigation.
// ViewModels depend on this interface instead of directly using Shell.Current,
// which keeps navigation testable and decoupled from MAUI UI types.
public interface INavigationService
{
	// Navigate to the department detail page for the selected department.
	Task GoToDepartmentDetailAsync(int departmentId);

	// Navigate to the employee edit page with the required route parameters.
	Task GoToEmployeeEditAsync(int departmentId, int employeeId);

	// Navigate one step back in the Shell navigation stack.
	Task GoBackAsync();
}
