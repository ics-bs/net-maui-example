using LundUniversity.MauiExample.Models;

namespace LundUniversity.MauiExample.Services;

public sealed class InMemoryUniversityRepository : IUniversityRepository
{
	private readonly List<Department> _departments;
	private readonly List<Employee> _employees;

	public InMemoryUniversityRepository()
	{
		_departments =
		[
			new Department { DepartmentId = 1, DeptName = "Computer Science", DeptBudget = 500000.00m },
			new Department { DepartmentId = 2, DeptName = "Political Science", DeptBudget = 400000.00m },
			new Department { DepartmentId = 3, DeptName = "Informatics", DeptBudget = 700000.00m },
		];

		_employees =
		[
			new Employee { EmployeeId = 1, EmpNo = "E1", EmpName = "Bob", EmpSalary = 50000.00m, DepartmentId = 1 },
			new Employee { EmployeeId = 2, EmpNo = "E2", EmpName = "Joe", EmpSalary = 60000.00m, DepartmentId = 1 },
			new Employee { EmployeeId = 3, EmpNo = "E3", EmpName = "Sue", EmpSalary = 70000.00m, DepartmentId = 2 },
			new Employee { EmployeeId = 4, EmpNo = "E4", EmpName = "Dan", EmpSalary = 40000.00m, DepartmentId = 3 },
			new Employee { EmployeeId = 5, EmpNo = "E5", EmpName = "Ham", EmpSalary = 60000.00m, DepartmentId = 3 },
			new Employee { EmployeeId = 6, EmpNo = "E6", EmpName = "Sam", EmpSalary = 50000.00m, DepartmentId = 3 },
		];

		ValidateSeedData();
	}

	public async Task<IReadOnlyList<Department>> GetDepartmentsAsync(CancellationToken cancellationToken = default)
	{
		await Task.Delay(250, cancellationToken);

		return _departments
			.OrderBy(department => department.DeptName, StringComparer.OrdinalIgnoreCase)
			.Select(department =>
			{
				var stats = GetDepartmentStats(department.DepartmentId);
				return CloneDepartment(department, stats.EmployeeCount, stats.TotalSalary);
			})
			.ToList();
	}

	public async Task<DepartmentDetail?> GetDepartmentDetailAsync(int departmentId, CancellationToken cancellationToken = default)
	{
		await Task.Delay(250, cancellationToken);

		var department = _departments.FirstOrDefault(candidate => candidate.DepartmentId == departmentId);
		if (department is null)
		{
			return null;
		}

		var employees = _employees
			.Where(employee => employee.DepartmentId == department.DepartmentId)
			.OrderBy(employee => employee.EmpNo, StringComparer.OrdinalIgnoreCase)
			.Select(CloneEmployee)
			.ToList();

		var stats = GetDepartmentStats(department.DepartmentId);
		return new DepartmentDetail(CloneDepartment(department, stats.EmployeeCount, stats.TotalSalary), employees);
	}

	public async Task<Employee?> GetEmployeeAsync(int employeeId, CancellationToken cancellationToken = default)
	{
		await Task.Delay(200, cancellationToken);

		var employee = _employees.FirstOrDefault(candidate => candidate.EmployeeId == employeeId);
		return employee is null ? null : CloneEmployee(employee);
	}

	public async Task<bool> UpdateEmployeeAsync(Employee employee, CancellationToken cancellationToken = default)
	{
		await Task.Delay(200, cancellationToken);

		var existingIndex = _employees.FindIndex(candidate => candidate.EmployeeId == employee.EmployeeId);
		if (existingIndex < 0)
		{
			return false;
		}

		if (!_departments.Any(department => department.DepartmentId == employee.DepartmentId))
		{
			throw new InvalidOperationException($"Department {employee.DepartmentId} does not exist.");
		}

		var hasDuplicateEmployeeNumber = _employees.Any(candidate =>
			candidate.EmployeeId != employee.EmployeeId &&
			string.Equals(candidate.EmpNo, employee.EmpNo, StringComparison.OrdinalIgnoreCase));
		if (hasDuplicateEmployeeNumber)
		{
			throw new InvalidOperationException($"Employee number {employee.EmpNo} is already in use.");
		}

		_employees[existingIndex] = CloneEmployee(employee);
		return true;
	}

	private void ValidateSeedData()
	{
		var uniqueDepartmentNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		foreach (var department in _departments)
		{
			if (!uniqueDepartmentNames.Add(department.DeptName))
			{
				throw new InvalidOperationException($"Duplicate department name in seed data: {department.DeptName}");
			}
		}

		var validDepartmentIds = _departments.Select(department => department.DepartmentId).ToHashSet();
		var uniqueEmployeeNumbers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		foreach (var employee in _employees)
		{
			if (!uniqueEmployeeNumbers.Add(employee.EmpNo))
			{
				throw new InvalidOperationException($"Duplicate employee number in seed data: {employee.EmpNo}");
			}

			if (!validDepartmentIds.Contains(employee.DepartmentId))
			{
				throw new InvalidOperationException(
					$"Employee {employee.EmpNo} has invalid DepartmentId {employee.DepartmentId}.");
			}
		}
	}

	private (int EmployeeCount, decimal TotalSalary) GetDepartmentStats(int departmentId)
	{
		var employees = _employees.Where(employee => employee.DepartmentId == departmentId).ToList();
		var totalSalary = employees.Sum(employee => employee.EmpSalary ?? 0m);
		return (employees.Count, totalSalary);
	}

	private static Department CloneDepartment(Department department, int employeeCount = 0, decimal totalSalary = 0m)
	{
		return new Department
		{
			DepartmentId = department.DepartmentId,
			DeptName = department.DeptName,
			DeptBudget = department.DeptBudget,
			EmployeeCount = employeeCount,
			TotalSalary = totalSalary
		};
	}

	private static Employee CloneEmployee(Employee employee)
	{
		return new Employee
		{
			EmployeeId = employee.EmployeeId,
			EmpNo = employee.EmpNo,
			EmpName = employee.EmpName,
			EmpSalary = employee.EmpSalary,
			DepartmentId = employee.DepartmentId
		};
	}
}
