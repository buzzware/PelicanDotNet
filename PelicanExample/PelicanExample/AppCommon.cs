using System.Collections.Generic;
using System.Threading.Tasks;
using AvaloniaCrossApp.Pelican;

namespace AvaloniaCrossApp; 

public static class AppCommon {

	public static PelicanRouter2 Router { get; private set; }
	
	public static void Setup() {
		// Router = new PelicanRouter2(
		// 	"menu",
		// 	new RouteTable2(
		// 		new Dictionary<string, SegmentPageBuilder>() {
		// 			{ "menu", async (ctx) => ctx.Page(new MenuPage() {DataContext = new MenuPageModel()}) },
		// 			{ "one", async (ctx) => ctx.Page(new OnePage() {DataContext = new OnePageModel()}) }
		// 		}
		// 	)
		// );
	}


	// public static async Task NavigatePush(string segment) {
	// 	Router.Push(segment);
	// }
}
