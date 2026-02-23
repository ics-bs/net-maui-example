namespace LundUniversity.MauiExample;

public partial class App : Application
{
	private readonly AppShell _appShell;

	public App(AppShell appShell)
	{
		InitializeComponent();
		_appShell = appShell;
	}

	// MAUI asks your App to create the top-level window.
	// You return a Window whose root content is AppShell.
	// Shell then handles navigation/routing for the rest of the app.
	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(_appShell);
	}
}
