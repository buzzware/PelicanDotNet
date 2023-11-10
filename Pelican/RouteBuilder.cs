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