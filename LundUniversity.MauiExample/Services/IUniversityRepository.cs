using LundUniversity.MauiExample.Models;

namespace LundUniversity.MauiExample.Services;

public interface IUniversityRepository
{
	Task<IReadOnlyList<Department>> GetDepartmentsAsync(CancellationToken cancellationToken = default);
	Task<DepartmentDetail?> GetDepartmentDetailAsync(int departmentId, CancellationToken cancellationToken = default);
	Task<Employee?> GetEmployeeAsync(int employeeId, CancellationToken cancellationToken = default);
	Task<bool> UpdateEmployeeAsync(Employee employee, CancellationToken cancellationToken = default);
}
