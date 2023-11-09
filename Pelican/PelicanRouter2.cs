using System.Collections.Immutable;
using System.ComponentModel;
using Pelican.Utilities;
using Serilog;

namespace Pelican;

public class RouteBuilder {
	
	public PelicanRouteSegment Alias { get; private set; }
	public PelicanRoute RedirectRoute { get; private set; }
	public IPelicanPageModel PageModel { get; private set; }
	public IPelicanPage PageInstance { get; private set; }

	public RouteBuilder Page<PageT, PageModelT>() {
		PageInstance = (IPelicanPage)Activator.CreateInstance(typeof(PageT))!;
		PageModel = (IPelicanPageModel)Activator.CreateInstance(typeof(PageModelT))!;
		PageInstance.DataContext = PageModel;
		return this;
	}

	public RouteBuilder Page<PageT>(IPelicanPageModel pageModel) {
		PageInstance = (IPelicanPage)Activator.CreateInstance(typeof(PageT))!;
		PageModel = pageModel;
		PageInstance.DataContext = PageModel;
		return this;
	}


	public RouteBuilder Page<T>() {
		PageInstance = (IPelicanPage)Activator.CreateInstance(typeof(T))!;
		if (PageInstance.DataContext == null)
			PageInstance.DataContext = PageInstance;
		return this;
	}

	public RouteBuilder AliasTo(PelicanRouteSegment segment) {
		Alias = segment;
		return this;
	}
	
	public RouteBuilder RedirectTo(PelicanRoute route) {
		RedirectRoute = route;
		return this;
	}
}

public class RouteTable2 {
	public Dictionary<string, Func<RouteBuilder, Task<RouteBuilder>>> Routes { get; }

	public RouteTable2(Dictionary<string, Func<RouteBuilder, Task<RouteBuilder>>> routes) {
		Routes = routes;
	}

	public Func<RouteBuilder, Task<RouteBuilder>>? Match(string path) {
		if (Routes.TryGetValue(path, out var redirect))
			return redirect;
		else
			return null;
	}

// 	public async Task<string> ExecuteRedirects(string path) {
// 		while (true) {
// 			var redirectBuilder = Match(path);
// 			if (redirectBuilder == null) {
// 				return path;
// 			} else {
// 				path = await redirectBuilder(path);
// 			}
// 		}
// 	}
//
// 	public async Task<PelicanRoute> ExecuteRedirectsRoute(PelicanRoute route) {
// 		var path = route.ToPath();
// 		var redirected = await ExecuteRedirects(path);
// 		return redirected == path ? route : new PelicanRoute(redirected);
// 	}
// }
}

public class PageStackRecord {
	public PelicanRouteSegment Segment { get; }
	public IPelicanPage Page { get; }
	public IPelicanPageModel? PageModel { get; }

	public PageStackRecord(PelicanRouteSegment segment, IPelicanPage page, IPelicanPageModel? pageModel = null) {
		Segment = segment;
		Page = page;
		PageModel = pageModel;
	}
}

public class OnExitResult {
}


public class PelicanRouter2 : BindableBase {
	
	private PelicanRouterState State { get; }
	public PelicanRoute Route => State.Route;

	public RouteTable2 RouteTable { get; }

	private ImmutableList<PageStackRecord> _pageStack = ImmutableList<PageStackRecord>.Empty;
	private PelicanRoute? _cacheRoute;
	

	//public event PropertyChangedEventHandler? PropertyChanged;
	//
	// protected override void OnPropertyChanged(PropertyChangedEventArgs args) {
	// 	base.OnPropertyChanged(args);
	// }

	// virtual void OnPropertyChanged(string propertyName) {
	// 	PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	// }
	
	public PelicanRouter2(RouteTable2 routeTable) {
		RouteTable = routeTable;
		State = new PelicanRouterState();
		State.PropertyChanged += StateOnPropertyChanged;
	}

	~PelicanRouter2() {
		State.PropertyChanged -= StateOnPropertyChanged;
	}

	private void StateOnPropertyChanged(object? sender, PropertyChangedEventArgs e) {
		if (e.PropertyName == nameof(PelicanRouterState.Route))
			OnRouteChanged();
	}

	private void OnRouteChanged() {
		Log.Debug("OnRouteChanged");
		RaisePropertyChanged(nameof(CanPop));
	}

	// public async Task GoBack() {
	// 	if (CanGoBack()) {
	// 		State.Route = State.Route.PopSegment();
	// 	}
	// 	else {
	// 		throw new InvalidOperationException("No more routes to pop.");
	// 	}
	// }
	//
	// public bool CanGoBack() {
	// 	return State.Route.Segments.Count > 1;
	// }
	
	// public async Task ReplaceCurrentSegment(string segmentPath) {
	// 	var segment = PelicanRouteSegment.FromPathSegment(segmentPath);
	// 	var newRoute = State.Route.PopSegment().PushSegment(segment);
	// 	State.Route = newRoute;
	// }
	//
	//    
	// public async Task ReplaceRoute(PelicanRouterState configuration) {
	// 	var newRoute = await RouteTable.ExecuteRedirectsRoute(configuration.Route);
	// 	if (!newRoute.Equals(configuration.Route)) {
	// 		configuration.Route = newRoute;
	// 		await SetNewRoutePath(configuration);
	// 	}
	// }
	//
	// public async Task SetNewRoutePath(PelicanRouterState configuration) {
	// 	if (configuration.Route.Equals(State.Route))
	// 		return;
	// 	State.Route = configuration.Route;
	// }

	// private Page BuildPage(string toPath, Page buildResultPage) {
	// 	throw new NotImplementedException();
	// }

	// public async Task<IEnumerable<Page>> BuildPages() {
	//     Debug.WriteLine("Router.buildPages");
	//     Debug.WriteLine("_pages is ${_cachePages==null ? 'not' : ''} set");
	//     var pages = new List<Page>();
	//     var useCached = _cacheRoute!=null;
	//     for (var i=0; i<State.Route.Segments.Count; i++) {
	//         var segment = State.Route.Segments[i];
	//         Page page;
	//         if (useCached && _cacheRoute!.Segments.Count>i && segment.Equals(_cacheRoute!.Segments[i])) {
	//             page = _cachePages![i];
	//             Debug.WriteLine("Use cached ${_cacheRoute!.segments[i].toPath()}");
	//         } else {
	//             useCached = false;
	//             var prc = new PelicanRouteContext(State.Route, segment);
	//             var buildResult = await RouteTable.ExecuteSegment(prc);
	//             Debug.WriteLine("build ${segment.toPath()}");
	//             page = BuildPage(segment.ToPath(),buildResult.Page!);
	//         }
	//         pages.Add(page);
	//     }
	//
	//     _cacheRoute = State.Route;
	//     var originalPages = _cachePages;
	//     if (originalPages?.Any() ?? false) {
	//         originalPages!.Reverse().ForEach((page) => {
	//             if (pages.Contains(page))
	//                 return;
	//             // !!! dispose page
	//             // var widget = as<MaterialPage<dynamic>>(page)?.child;
	//             //as<Disposable>(widget)?.onDispose();
	//         });
	//     }
	//     _cachePages = pages.ToImmutableList();
	//     return pages;
	// }


	private IPelicanPage currentPage;
	public IPelicanPage CurrentPage {
		get => currentPage;
		private set => SetProperty(ref currentPage,value);
	}

	private async Task<PageStackRecord?> ExecuteSegment(PelicanRouteSegment pelicanRouteSegment) {
		PelicanRouteSegment currSegment = pelicanRouteSegment;
		while (true) {
			var redirectBuilder = RouteTable.Match(currSegment.ToRoutablePath());
			if (redirectBuilder == null) {
				return null;
			} else {
				var rb = new RouteBuilder();
				var result = await redirectBuilder(rb);
				if (result.Alias != null) {
					currSegment = result.Alias;
				} else {
					return new PageStackRecord(currSegment,result.PageInstance,result.PageModel);
				}
			}
		}
	}

	
	public async Task Push(PelicanRouteSegment segment,object? data = null) {
		var item = await ExecuteSegment(segment);
		if (item == null)
			throw new ArgumentException("Undefined segment " + segment.ToPath());
		var currentItem = _pageStack.LastOrDefault();
		if (currentItem?.PageModel != null) {
			var result = await currentItem.PageModel.OnExit(pushing: true);
		}
		_pageStack = _pageStack.Add(item);
		State.Push(item.Segment);
		_cacheRoute = State.Route;
		if (item.PageModel != null) {
			await item.PageModel.Init(segment:item.Segment,data);
			await item.PageModel.OnEnter(popping: false,segment:item.Segment,data);
		}
		CurrentPage = item.Page;
	}

	
	public async Task<PelicanRouteSegment> Pop(object? data = null) {
		if (!CanPop)
			throw new InvalidOperationException("No more routes to pop.");
		var currentItem = _pageStack.LastOrDefault();
		if (currentItem?.PageModel != null) {
			var result = await currentItem.PageModel.OnExit(pushing: false);
		}
		var segment = State.Pop();
		_cacheRoute = State.Route;
		_pageStack = _pageStack.RemoveAt(_pageStack.Count - 1);
		currentItem = _pageStack.LastOrDefault();
		if (currentItem?.PageModel != null) {
			CurrentPage = currentItem.Page;
			await currentItem.PageModel.OnEnter(popping: true,segment:currentItem.Segment,data);
		}
		return segment;
	}

	bool CanPop => State.Route.Segments.Count > 1;

	// public async Task Goto(String path) {
	//     var newPath = await RouteTable.ExecuteRedirects(path);
	//     var route = new PelicanRoute(newPath);
	//     if (route.Equals(State.Route))
	//         return;
	//     State.Route = route;
	// }
	//
	// void Replace(String segmentPath) {
	//  ReplaceSegment(PelicanRouteSegment.FromPathSegment(segmentPath));
	// }

	void ReplaceSegment(PelicanRouteSegment segment) {
		var route = State.Route.PopSegment();
		route = route.PushSegment(segment);
		State.Route = route;
	}
}

