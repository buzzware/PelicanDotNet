using System.Collections.Immutable;

namespace Pelican;

public class PelicanRoute {
	
	private readonly ImmutableList<PelicanRouteSegment> _segments;
	public IReadOnlyList<PelicanRouteSegment> Segments => _segments;

	public PelicanRoute(IEnumerable<PelicanRouteSegment> segments)
	{
		_segments = segments.ToImmutableList();
	}

	public string ToPath() {
		if (!_segments.Any())
			return "/";
		var parts = _segments.Select(s => s.ToPath()).Aggregate((current, next) => $"{current}/{next}");
		return $"/{parts}";
	}

	public PelicanRoute(string path)
	{
		var parts = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
		_segments = parts.Select(PelicanRouteSegment.FromPathSegment).ToImmutableList();
	}

	public PelicanRoute PushSegment(PelicanRouteSegment segment)
	{
		return new PelicanRoute(_segments.Add(segment));
	}

	public PelicanRoute PopSegment()
	{
		if (!_segments.Any())
			throw new Exception("Can't pop when stack is empty");

		return new PelicanRoute(_segments.RemoveAt(_segments.Count - 1));
	}

	public bool Equals(PelicanRoute other, bool ignoreOptions = true)
	{
		return _segments.Count == other._segments.Count && 
		       !_segments.Where((segment, i) => !segment.Equals(other._segments[i], ignoreOptions)).Any();
	}
}
