using System.Collections.Immutable;
using System.ComponentModel;
using Pelican.Utilities;
using Serilog;

namespace Pelican;

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
	public bool Allowed = true;
	public object? Data = null;
}


public class PelicanRouter : BindableBase {
	
	private PelicanRouterState State { get; }
	public PelicanRoute Route => State.Route;

	public RouteTable RouteTable { get; }

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
	
	public PelicanRouter(RouteTable routeTable) {
		RouteTable = routeTable;
		State = new PelicanRouterState();
		State.PropertyChanged += StateOnPropertyChanged;
	}

	~PelicanRouter() {
		State.PropertyChanged -= StateOnPropertyChanged;
	}

	private void StateOnPropertyChanged(object? sender, PropertyChangedEventArgs e) {
		if (e.PropertyName == nameof(PelicanRouterState.Route))
			OnRouteChanged();
	}

	private void OnRouteChanged() {
		Log.Debug("OnRouteChanged");
		RaisePropertyChanged(nameof(CanPop));
		RaisePropertyChanged(nameof(CanBack));
	}

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


	private IPelicanPage? currentPage = null;
	public IPelicanPage? CurrentPage => currentPage;

	private IPelicanPageModel? currentPageModel = null;
	public IPelicanPageModel? CurrentPageModel => currentPageModel;

	public void SetCurrentPage(IPelicanPage page, IPelicanPageModel? pageModel = null) {
		SetProperty(ref currentPageModel, pageModel, propertyName: nameof(CurrentPageModel));
		SetProperty(ref currentPage, page, propertyName: nameof(CurrentPage));
	}

	private async Task<PageStackRecord?> ExecuteSegment(PelicanRouteSegment pelicanRouteSegment) {
		PelicanRouteSegment currSegment = pelicanRouteSegment;
		while (true) {
			var redirectBuilder = RouteTable.Match(currSegment);
			if (redirectBuilder == null) {
				return null;
			} else {
				var rb = new RouteBuilder(pelicanRouteSegment);
				var result = await redirectBuilder(rb);
				if (result.Alias != null) {
					currSegment = result.Alias;
				} else {
					return new PageStackRecord(currSegment,result.PageInstance,result.PageModel);
				}
			}
		}
	}

	
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

	void Replace(PelicanRouteSegment segment) {
		var route = State.Route.PopSegment();
		route = route.PushSegment(segment);
		State.Route = route;
	}
	
	
	public async Task Push(PelicanRouteSegment segment,object? data = null) {
		var item = await ExecuteSegment(segment);
		if (item == null)
			throw new ArgumentException("Undefined segment " + segment.ToPath());
		var currentItem = _pageStack.LastOrDefault();
		if (currentItem?.PageModel != null) {
			var result = await currentItem.PageModel.OnExit(pushing: true, data: data);
		}
		_pageStack = _pageStack.Add(item);
		State.Push(item.Segment);
		_cacheRoute = State.Route;
		if (item.PageModel != null) {
			await item.PageModel.Init(segment:item.Segment,data: data);
			await item.PageModel.OnEnter(popping: false,segment: item.Segment,data: data);
		}
		SetCurrentPage(item.Page,item.PageModel);
	}

	public Task Push(string path, object? data = null) {
		return Push(PelicanRouteSegment.FromPath(path),data);
	}

	public async Task Replace(PelicanRouteSegment segment,object? data = null) {
		var item = await ExecuteSegment(segment);
		if (item == null)
			throw new ArgumentException("Undefined segment " + segment.ToPath());
		var currentItem = _pageStack.LastOrDefault();
		if (currentItem?.PageModel != null) {
			var result = await currentItem.PageModel.OnExit(pushing: true, data: data);
		}

		if (_pageStack.Any())
			_pageStack = _pageStack.SetItem(_pageStack.Count - 1, item);
		else
			_pageStack = _pageStack.Add(item);
		
		State.Replace(item.Segment);
		_cacheRoute = State.Route;
		if (item.PageModel != null) {
			await item.PageModel.Init(segment:item.Segment,data);
			await item.PageModel.OnEnter(popping: false,segment:item.Segment,data);
		}
		SetCurrentPage(item.Page,item.PageModel);
	}

	public bool CanPop => State.Route.Segments.Count > 0;
	
	public async Task<PelicanRouteSegment?> Pop(object? data = null) {
		if (!CanPop)
			throw new InvalidOperationException("No more routes to pop.");
		var poppingRecord = _pageStack.Last();
		if (poppingRecord.PageModel != null) {
			var result = await poppingRecord.PageModel.OnExit(pushing: false, data: data);
			if (!(result?.Allowed ?? true))
				return null;
			if (result?.Data!=null)
				data = result.Data;
		}
		var segment = State.Pop();
		_cacheRoute = State.Route;
		_pageStack = _pageStack.Remove(poppingRecord);
		var newCurrentSegment = _pageStack.LastOrDefault();
		if (newCurrentSegment?.PageModel != null) {
			SetCurrentPage(newCurrentSegment.Page,newCurrentSegment.PageModel);
			await newCurrentSegment.PageModel.OnEnter(popping: true,segment:segment,data);
		}
		return segment;
	}
	
	bool CanBack => State.Route.Segments.Count > 1;

	// return true if popped
	public async Task<bool> Back() {
		if (!CanBack)
			return false;
		var result = await Pop();
		return result != null;
	} 
	
	// return true if handled or false to allow further handling ie leave app
	public async Task<bool> HandleHardwareBackButton() {
		await Back();
		return true; // true if handled, otherwise false 
	}
}

