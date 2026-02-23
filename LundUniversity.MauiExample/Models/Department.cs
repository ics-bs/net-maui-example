namespace LundUniversity.MauiExample.Models;

public sealed class Department
{
	public int DepartmentId { get; init; }
	public string DeptName { get; init; } = string.Empty;
	public decimal? DeptBudget { get; init; }
	public int EmployeeCount { get; init; }
	public decimal TotalSalary { get; init; }

	public string DeptBudgetDisplay => DeptBudget.HasValue
		? DeptBudget.Value.ToString("C2")
		: "No budget assigned";

	public string EmployeeCountDisplay => EmployeeCount == 1
		? "1 employee"
		: $"{EmployeeCount} employees";

	public string TotalSalaryDisplay => $"Payroll: {TotalSalary:C2}";
}
