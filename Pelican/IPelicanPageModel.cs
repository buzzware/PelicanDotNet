namespace Pelican;

public interface IPelicanPageModel {
	Task Init(PelicanRouteSegment segment, object? data);
	Task OnEnter(bool popping, PelicanRouteSegment segment, object? data);
	Task<OnExitResult?> OnExit(bool pushing);
}
