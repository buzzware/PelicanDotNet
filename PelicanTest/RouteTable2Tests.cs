using Pelican;
using Pelican.Utilities;
using Serilog;

namespace PelicanTest; 

// Assume T is a type parameter or a variable holding the type you want to instantiate
// Type genericType = typeof(List<>).MakeGenericType(typeof(T));
// object instance = Activator.CreateInstance(genericType);

// public class Route {
//
// 	public Route() {
// 		string name,
// 		Type 	
// 	}
// 	
// }

// public class Route<PageType,PageModelType> {
// 	
// 	TestPage
// 	
// 	public Route() {
// 		object? instance = Activator.CreateInstance(typeof(PageType));
// 		page = 
// 	}
// }



internal class TestPage : BindableBase, IPelicanPage {
	public object? Content { get; set; }

	public object? DataContext { get; set; }
}

internal class TestPageModel : BindableBase, IPelicanPageModel {

	public List<string> TestEvents = new List<string>();
	public void RecordEvent(string ev) {
		TestEvents.Add(ev);
		Log.Debug(ev);
	}
		
	public async Task Init(PelicanRouteSegment segment, object? data) {
		RecordEvent("Init");
	}

	public async Task OnEnter(bool popping, PelicanRouteSegment segment, object? data) {
		RecordEvent($"OnEnter popping:{popping} segment:{segment.ToPath()} data:{data}");
	}

	public async Task<OnExitResult?> OnExit(bool pushing) {
		RecordEvent($"OnExit pushing:{pushing}");
		return null;
	}
}


internal class LoginPage : TestPage {
}

internal class MenuPage : TestPage {
}

internal class OnePage : TestPage {
}

internal class TwoPage : TestPage {
}

internal class SettingsPage : TestPage {
}

internal class LoginPageModel : TestPageModel {
}

internal class MenuPageModel : TestPageModel {
}

internal class OnePageModel : TestPageModel {
}

internal class TwoPageModel : TestPageModel {
}

public class RouteTable2Tests {
	
	private static RouteTable BuildTable() {
		return new RouteTable(
			new Dictionary<string, Func<RouteBuilder, Task<RouteBuilder>>> {
				["login"] = async rb => rb.Page<LoginPage>(new LoginPageModel()),
				["menu"] = async rb => rb.Page<MenuPage>(new MenuPageModel()),
				["one"] = async rb => rb.Page<OnePage>(new OnePageModel()),
				["two"] = async rb => rb.Page<TwoPage,TwoPageModel>(),
				["three"] = async rb => rb.AliasTo(new PelicanRouteSegment("two")),
				["settings"] = async rb => rb.Page<SettingsPage>(),
				//["diary"] = async rb => rb.TestPage<DiaryPage>(new DiaryPageModel() { Tab = rb.Pars["tab"] ?? "detail" }),
				["/menu/four"] = async rb => rb.RedirectTo(new PelicanRoute("/menu/one")),
			}
		);
	}
	
	
	[SetUp]
	public void Setup() {
	}

	[Test]
	public async Task PushPushPopTest() {
		var table = BuildTable();
		var router = new PelicanRouter(table);
		var currentPageChanges = 0;
		//router.PropertyChanged += (sender, args) => if (args.Name) menuPageChanges++;
		router.PropertyChanged += (sender, args) => {
			if (args.PropertyName=="CurrentPage") 
				currentPageChanges++;
		};
		Assert.That(router.Route.ToPath(),Is.EqualTo("/"));
		Assert.That(router.CurrentPage, Is.Null);
		
		await router.Push(new PelicanRouteSegment("menu"));
		Assert.That(router.Route.ToPath(),Is.EqualTo("/menu"));
		Assert.That(router.CurrentPage, Is.TypeOf<MenuPage>());
		Assert.That(currentPageChanges, Is.EqualTo(1));
		var menuPageModel = router.CurrentPage.DataContext as MenuPageModel;
		Assert.That(menuPageModel,Is.TypeOf<MenuPageModel>());
		Assert.That(menuPageModel.TestEvents[0],Is.EqualTo("Init"));
		Assert.That(menuPageModel.TestEvents[1],Is.EqualTo("OnEnter popping:False segment:menu data:"));
		Assert.That(menuPageModel.TestEvents.Count, Is.EqualTo(2));
		
		await router.Push(new PelicanRouteSegment("one"));
		Assert.That(menuPageModel.TestEvents[2],Is.EqualTo("OnExit pushing:True"));
		Assert.That(router.Route.ToPath(),Is.EqualTo("/menu/one"));
		Assert.That(router.CurrentPage, Is.TypeOf<OnePage>());
		var onePageModel = router.CurrentPage.DataContext as OnePageModel;
		Assert.That(currentPageChanges, Is.EqualTo(2));
		
		Assert.That(onePageModel,Is.TypeOf<OnePageModel>());
		Assert.That(onePageModel.TestEvents[0],Is.EqualTo("Init"));
		Assert.That(onePageModel.TestEvents[1],Is.EqualTo("OnEnter popping:False segment:one data:"));
		Assert.That(onePageModel.TestEvents.Count, Is.EqualTo(2));
		
		await router.Pop();
		Assert.That(onePageModel.TestEvents[2],Is.EqualTo("OnExit pushing:False"));
		Assert.That(onePageModel.TestEvents.Count, Is.EqualTo(3));
		Assert.That(menuPageModel.TestEvents[3],Is.EqualTo("OnEnter popping:True segment:menu data:"));
		Assert.That(menuPageModel.TestEvents.Count, Is.EqualTo(4));
		Assert.That(router.Route.ToPath(),Is.EqualTo("/menu"));
		Assert.That(router.CurrentPage, Is.TypeOf<MenuPage>());
		Assert.That(currentPageChanges, Is.EqualTo(3));
		var menuPageModel2 = router.CurrentPage.DataContext as MenuPageModel;
		Assert.That(menuPageModel2,Is.SameAs(menuPageModel));
	}
}



// public class RedirectRoute {
// }
//
// public class RedirectBuilder {
// 	public RedirectRoute Path(string path) {
// 		throw new NotImplementedException();
// 	}
// }

