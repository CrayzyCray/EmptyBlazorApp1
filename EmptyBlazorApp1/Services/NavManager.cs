using EmptyBlazorApp1.Components.Calendar;
using Microsoft.JSInterop;

namespace EmptyBlazorApp1.Services;

public class NavManager {
    private Action<Type, IDictionary<string, object>?> _onNavigate;
    private DotNetObjectReference<NavManager>          _dotNetObjectReference;
    private List<Page>                                 _pages = new();

    private IJSRuntime _jsRuntime;

    private int  _currentPageId;
    private bool _isInitialized;


    const int MaxStackSize = 10;

    public NavManager(IJSRuntime jsRuntime) {
        _jsRuntime             = jsRuntime;
        _dotNetObjectReference = DotNetObjectReference.Create(this);
    }

    public void Initialize(Type startPage, Action<Type, IDictionary<string, object>?> onNavigate) {
        if (_isInitialized) {
            return;
        }

        _pages.Add(new(startPage, null));
        _jsRuntime.InvokeVoidAsync("initHistoryApi", _dotNetObjectReference, _currentPageId);
        _onNavigate    = onNavigate;
        _isInitialized = true;
    }

    public void NavigateTo(Type pageType) =>
        NavigateTo(pageType, null as IDictionary<string, object>);

    public void NavigateTo(Type pageType, Dictionary<string, object> parameters) =>
        NavigateTo(pageType, parameters as IDictionary<string, object>);

    public void NavigateTo(Type pageType, IDictionary<string, object>? parameters) {
        if (!_isInitialized) {
            return;
        }

        var page = new Page(pageType, parameters);

        if (_currentPageId + 1 < _pages.Count) {
            _pages.RemoveRange(_currentPageId + 1, _pages.Count - 1 - _currentPageId);
        }
        
        _pages.Add(page);
        _currentPageId++;
        
        _onNavigate(pageType, parameters);
        _jsRuntime.InvokeVoidAsync("setPageState", _currentPageId);
    }

    public void GoBack() {
        if (!_isInitialized) {
            return;
        }

        if (_currentPageId <= 0) {
            return;
        }

        _currentPageId--;

        _jsRuntime.InvokeVoidAsync("goBack");
    }

    public bool CanGoBack() {
        if (!_isInitialized) {
            return false;
        }
        return _currentPageId > 0;
    }

    public void SetCurrentPageParameters(Dictionary<string, object> parameters) {
        _pages[_currentPageId].Parameters = parameters;
    }

    [JSInvokable]
    public void JsOnpopstateCallback(int pageId) {
        Console.WriteLine(pageId.ToString());

        _currentPageId = pageId;
        var page = _pages[pageId];

        _onNavigate(page.Type, page.Parameters);
    }

    class Page {
        public Type                         Type;
        public IDictionary<string, object>? Parameters;

        public Page(Type type, IDictionary<string, object>? parameters) {
            Type       = type;
            Parameters = parameters;
        }
    }
}