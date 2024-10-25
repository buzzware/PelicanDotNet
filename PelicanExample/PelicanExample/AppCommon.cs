using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pelican;
using PelicanExample.Pages;

namespace PelicanExample; 

public static class AppCommon {

	public static PelicanRouter Router { get; private set; }
	
	public static async void Setup() {
		Router = new PelicanRouter(AppRoutes.RouteTable());
		await Router.Push(new PelicanRouteSegment(AppRoutes.MENU));
	}
}
