namespace Pelican;

public class RouteBuilder {
	
	public PelicanRouteSegment Alias { get; private set; }
	public PelicanRoute RedirectRoute { get; private set; }
	public IPelicanPageModel PageModel { get; private set; }
	public IPelicanPage PageInstance { get; private set; }
	public PelicanRouteSegment Segment { get; }

	public RouteBuilder(PelicanRouteSegment segment) {
		Segment = segment;
	}
	
	public RouteBuilder Page<PageT, PageModelT>() where PageT : IPelicanPage where PageModelT : IPelicanPageModel {
		PageInstance = (IPelicanPage)Activator.CreateInstance(typeof(PageT))!;
		PageModel = (IPelicanPageModel)Activator.CreateInstance(typeof(PageModelT))!;
		PageInstance.DataContext = PageModel;
		PageModel.Segment = Segment;
		return this;
	}

	public RouteBuilder Page<PageT>(IPelicanPageModel pageModel) where PageT : IPelicanPage {
		PageInstance = (IPelicanPage)Activator.CreateInstance(typeof(PageT))!;
		PageModel = pageModel;
		PageInstance.DataContext = PageModel;
		PageModel.Segment = Segment;
		return this;
	}
	
	public RouteBuilder Page<PageT>() where PageT : IPelicanPage {
		PageInstance = (IPelicanPage)Activator.CreateInstance(typeof(PageT))!;
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
