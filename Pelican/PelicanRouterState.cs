using System.ComponentModel;

namespace Pelican; 

public class PelicanRouterState : INotifyPropertyChanged {
	
	private PelicanRoute _route = new PelicanRoute("");

	public event PropertyChangedEventHandler? PropertyChanged;

	public PelicanRouterState() {
	}

	public PelicanRoute Route
	{
		get => _route;
		set
		{
			if (_route != value)
			{
				_route = value;
				OnPropertyChanged(nameof(Route));
			}
		}
	}
	
	public void Push(string segmentPath)
	{
		var segment = PelicanRouteSegment.FromPath(segmentPath);
		Push(segment);
	}

	public void Push(PelicanRouteSegment segment) {
		Route = Route?.PushSegment(segment) ?? new PelicanRoute(new []{segment});
	}
	
	public void Replace(PelicanRouteSegment segment) {
		Route = Route?.ReplaceSegment(segment) ?? new PelicanRoute(new []{segment});
	}
	
	public PelicanRouteSegment Pop()
	{
		if (!_route?.Segments.Any() ?? true)
			throw new InvalidOperationException("Can't pop when stack is empty");

		var poppedItem = _route.Segments[^1]; // using the ^1 index to get the last item
		Route = _route.PopSegment();
		Console.WriteLine($"pop {poppedItem.ToPath()}");
		OnPropertyChanged(nameof(Route));
		return poppedItem;
	}

	protected virtual void OnPropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

}
