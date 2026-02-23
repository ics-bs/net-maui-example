namespace LundUniversity.MauiExample.Navigation;

// Shared key names for values passed during Shell navigation.
// Example usage:
// - sender: GoToAsync(route, new Dictionary<string, object> { [DepartmentId] = 1 })
// - receiver: ApplyQueryAttributes(...) reads the same key
// Keeping keys here avoids string typos across the app.
public static class NavigationParameters
{
	// Key for the selected department's ID.
	public const string DepartmentId = "departmentId";

	// Key for the selected employee's ID.
	public const string EmployeeId = "employeeId";
}
