using LundUniversity.MauiExample.Navigation;
using LundUniversity.MauiExample.Views;

namespace LundUniversity.MauiExample;

public partial class AppShell : Shell
{
	public AppShell(Func<MainPage> mainPageFactory)
	{
		// Loads and wires the controls declared in AppShell.xaml.
		InitializeComponent();

		// ContentTemplate expects a DataTemplate (a page factory), not a page instance.
		// () => mainPageFactory() is a lambda that Shell calls later to create MainPage via DI.
		MainShellContent.ContentTemplate = new DataTemplate(() => mainPageFactory());

		// A route is a string identifier for a navigation destination in Shell.
		// ViewModels navigate by these route names (GoToAsync), and Shell resolves the target page.
		Routing.RegisterRoute(ShellRoutes.DepartmentDetail, typeof(DepartmentDetailPage));
		Routing.RegisterRoute(ShellRoutes.EmployeeEdit, typeof(EmployeeEditPage));
	}
}
