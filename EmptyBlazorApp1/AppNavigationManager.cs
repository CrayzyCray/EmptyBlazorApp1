namespace EmptyBlazorApp1;

public class AppNavigationManager {
    private readonly Type _defaultPage;

    private readonly Action<Type, IDictionary<string, object>?>  _onNavigate;
    private readonly Stack<(Type, IDictionary<string, object>?)> _stack = new();

    const int MaxStackSize = 10;

    public AppNavigationManager(Type defaultPage, Action<Type, IDictionary<string, object>?> onNavigate) {
        _defaultPage = defaultPage;
        _onNavigate  = onNavigate;
    }

    public void NavigateTo(Type page, IDictionary<string, object>? parameters = null) {
        if (_stack.Count >= MaxStackSize) {
            _stack.Clear();
        }

        _stack.Push((page, parameters));
        _onNavigate(page, parameters);
    }

    public void GoBack() {
        if (_stack.Count == 1) {
            _stack.Clear();
        }

        if (_stack.Count == 0) {
            NavigateTo(_defaultPage, null);
            return;
        }

        _stack.Pop();
        var page = _stack.Pop();
        NavigateTo(page.Item1, page.Item2);
    }
    
    public void ClearHistory() {
        _stack.Clear();
    }
}