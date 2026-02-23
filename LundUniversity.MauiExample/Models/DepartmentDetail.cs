namespace LundUniversity.MauiExample.Models;

// Model class (data layer type):
// This class represents domain data only, not UI behavior.
//
// Design intent:
// - Department is a lightweight summary model used in list screens.
// - DepartmentDetail is an expanded model used when full detail is needed.
//   It includes Department + Employees in one payload.
//
// Why not put Employees directly on Department?
// - Not every Department use-case needs the full employee collection.
// - Keeping summary and detail shapes separate avoids "is this list loaded?"
//   ambiguity and keeps repository contracts explicit.
public sealed class DepartmentDetail
{
	public DepartmentDetail(Department department, IReadOnlyList<Employee> employees)
	{
		Department = department;
		Employees = employees;
	}

	// The selected department.
	public Department Department { get; }

	// Employees that belong to the selected department.
	public IReadOnlyList<Employee> Employees { get; }
}
