namespace Pelican;

public class RouteTable {
	public Dictionary<string?, Func<RouteBuilder, Task<RouteBuilder>>> Routes { get; }

	public RouteTable(Dictionary<string?, Func<RouteBuilder, Task<RouteBuilder>>> routes) {
		Routes = routes;
	}
	
	public Func<RouteBuilder, Task<RouteBuilder>>? Match(string? path) {
		var segment = PelicanRouteSegment.FromPath(path!);
		return Match(segment);
	}

	public Func<RouteBuilder, Task<RouteBuilder>>? Match(PelicanRouteSegment segment) {
		foreach (var pair in Routes) {
			if (pair.Key == segment.Name)
				return pair.Value;
		}
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
