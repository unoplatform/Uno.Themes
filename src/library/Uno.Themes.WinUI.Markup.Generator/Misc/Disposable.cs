namespace Uno.Markup.Misc;

public partial class Disposable : IDisposable
{
	private readonly Action? dispose;
	private Disposable(Action? dispose) => this.dispose = dispose;

	public void Dispose() => dispose?.Invoke();
}
public partial class Disposable
{
	public static IDisposable Empty => Create(null);
	public static IDisposable Create(Action? dispose) => new Disposable(dispose);
}
