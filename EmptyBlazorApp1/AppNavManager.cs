namespace EmptyBlazorApp1;

public class AppNavManager {
    private readonly Type _defaultPage;

    private readonly Action<Type, IDictionary<string, object>?>  _onNavigate;
    private readonly Stack<(Type, IDictionary<string, object>?)> _stack = new();

    const int MaxStackSize = 10;

    public AppNavManager(Type defaultPage, Action<Type, IDictionary<string, object>?> onNavigate) {
        _defaultPage = defaultPage;
        _onNavigate  = onNavigate;
    }
    
    public void SetCurrentPageParameters(IDictionary<string, object> parameters) {
        var t = _stack.Peek();
        t.Item2 = parameters;
    }

    public void NavigateTo(Type page, IDictionary<string, object>? parameters = null) {
        if (_stack.Count >= MaxStackSize) {
            _stack.Clear();
        }

        _stack.Push((page, parameters));
        _onNavigate(page, parameters);
    }

    public void GoBack() {
        if (_stack.Count <= 1) {
            _stack.Clear();
            NavigateTo(_defaultPage);
            return;
        }

        _stack.Pop();
        var page = _stack.Peek();
        NavigateTo(page.Item1, page.Item2);
    }

    public void ClearHistory() {
        _stack.Clear();
    }
}