using Pelican;

namespace PelicanTest;

public class RouteTests {
	
	static string testRouteUrl = "auth_email+auth_link=aHR0cHM6Ly9saW5rcy5odWV5LmNvLz9saW5rPWh0dHBzOi8vbGlua3MuaHVleS5jby9fXy9hdXRoL2FjdGlvbj9hcGlLZXklM0RBSXphU3lBSC0wLUMydDhpbkctZnBHb2FaZmxKS0FXbEpFTVJFdm8lMjZtb2RlJTNEc2lnbkluJTI2b29iQ29kZSUzRFNMaktwNEV3dnM3VzJjNXNnZ20xemJ5b1Q5bm00SWd6WERoMlc0b2JIYWtBQUFGOFdRZk1udyUyNmNvbnRpbnVlVXJsJTNEaHR0cHM6Ly9saW5rcy5odWV5LmNvL2gyby9hY3Rpb25zL2NvbmZpcm1fZW1haWw_ZW1haWwlMjUzREdhcnklMjUyQjZAaHVleS5jbyUyNmxhbmclM0RlbiZhcG49Y28uaHVleS5hbmRyb2lkLmgybyZhbXY9MTImaWJpPWNvLmh1ZXkuaW9zLmgybyZpZmw9aHR0cHM6Ly9saW5rcy5odWV5LmNvL19fL2F1dGgvYWN0aW9uP2FwaUtleSUzREFJemFTeUFILTAtQzJ0OGluRy1mcEdvYVpmbEpLQVdsSkVNUkV2byUyNm1vZGUlM0RzaWduSW4lMjZvb2JDb2RlJTNEU0xqS3A0RXd2czdXMmM1c2dnbTF6YnlvVDlubTRJZ3pYRGgyVzRvYkhha0FBQUY4V1FmTW53JTI2Y29udGludWVVcmwlM0RodHRwczovL2xpbmtzLmh1ZXkuY28vaDJvL2FjdGlvbnMvY29uZmlybV9lbWFpbD9lbWFpbCUyNTNER2FyeSUyNTJCNkBodWV5LmNvJTI2bGFuZyUzRGVu";

	
	[SetUp]
	public void Setup() {
	}

	[Test]
	public void Test1() {
		var route = new PelicanRoute(testRouteUrl);
		Assert.That(route.Segments.Count, Is.EqualTo(1));
		Assert.That(route.Segments[0].Name,Is.EqualTo("auth_email"));
		Assert.That(route.Segments[0].Options.Count,Is.EqualTo(1));
		Assert.That(route.Segments[0].Options["auth_link"]!,Is.Not.Empty);
	}

	[Test]
	public void Test2() {
		Assert.That(
			(new PelicanRouteSegment("page", new Dictionary<string, string> { { "a", "1" }, { "b", "2" } })).ToPath(),
			Is.EqualTo("page;a=1;b=2")
		);
		var segment = new PelicanRouteSegment("page", new Dictionary<string, string> { { "a", "1" }, { "b", "2" } }, new Dictionary<string, string> { { "x", "9" } });
		Assert.That(segment.ToPath(), Is.EqualTo("page;a=1;b=2+x=9"));
		
		segment = new PelicanRouteSegment("page", new Dictionary<string, string>{ {"b", "1"}, {"a", "2"} }, new Dictionary<string, string> {{"y", "10"}, {"x", "9"}});
		Assert.That(segment.ToPath(),Is.EqualTo("page;a=2;b=1+x=9;y=10"));  // sorted order
	}

	[Test]
	public void Test3() {	// PelicanRouteSegment deserialise
		Assert.That(PelicanRouteSegment.FromPath("page;b=2;a=1+y=9;x=10").ToPath(),Is.EqualTo("page;a=1;b=2+x=10;y=9"));
	}
}
