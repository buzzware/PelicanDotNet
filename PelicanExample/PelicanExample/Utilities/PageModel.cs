using System.Threading.Tasks;
using Pelican;

namespace PelicanExample.Utilities; 

public class PageModel : ViewModelBase, IPelicanPageModel {
	public virtual async Task Init(PelicanRouteSegment segment, object? data) {
	}

	public virtual async Task OnEnter(bool popping, PelicanRouteSegment segment, object? data) {
	}

	public virtual async Task<OnExitResult?> OnExit(bool pushing, object? data) {
		return null;
	}

	public PelicanRouteSegment Segment { get; set; }
}
