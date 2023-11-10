using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pelican;
using PelicanExample.Pages;

namespace PelicanExample; 

public static class AppCommon {

	public static PelicanRouter2 Router { get; private set; }
	
	public static async void Setup() {
		Router = new PelicanRouter2(AppRoutes.RouteTable());
		await Router.Push(new PelicanRouteSegment(AppRoutes.MENU));
	}


	// public static async Task NavigatePush(string segment) {
	// 	Router.Push(segment);
	// }
}
