using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pelican;
using PelicanExample.Pages;

public static class AppRoutes {
	
	public const string MENU = "menu";
	public const string ONE = "one";
	public const string TWO = "two";
	public const string THREE = "three";
	public const string FOUR = "four";
	public const string SETTINGS = "settings";
	
	private static RouteTable2? _routeTable;
	public static RouteTable2 RouteTable() {
		if (_routeTable == null) {
			_routeTable = new RouteTable2(
				new Dictionary<string, Func<RouteBuilder, Task<RouteBuilder>>> {
					[MENU] = async rb => rb.Page<MenuPage>(new MenuPageModel()),
					[ONE] = async rb => rb.Page<OnePage>(new OnePageModel()),
					[TWO] = async rb => rb.Page<TwoPage, TwoPageModel>(),
					[THREE] = async rb => rb.AliasTo(new PelicanRouteSegment(TWO)),
					[SETTINGS] = async rb => rb.Page<SettingsPage>(),
					//["diary"] = async rb => rb.TestPage<DiaryPage>(new DiaryPageModel() { Tab = rb.Pars["tab"] ?? "detail" }),
					[$"/{MENU}/{FOUR}"] = async rb => rb.RedirectTo(new PelicanRoute($"/{MENU}/{ONE}")),
				}
			);
		}
		return _routeTable;
	}
	
	
}
