namespace LundUniversity.MauiExample.Models;

public sealed class Employee
{
	public int EmployeeId { get; init; }
	public string EmpNo { get; init; } = string.Empty;
	public string? EmpName { get; init; }
	public decimal? EmpSalary { get; init; }
	public int DepartmentId { get; init; }

	public string EmpNameDisplay => string.IsNullOrWhiteSpace(EmpName)
		? "(No name)"
		: EmpName;

	public string EmpSalaryDisplay => EmpSalary.HasValue
		? EmpSalary.Value.ToString("C2")
		: "No salary assigned";
}
