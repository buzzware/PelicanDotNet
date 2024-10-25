using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace Pelican;

public static class MapExtensions
{
    public static bool LinkedMapsEqual<K, V>(IReadOnlyDictionary<K, V> map1, IReadOnlyDictionary<K, V> map2)
    {
        return map1.Count == map2.Count && !map1.Any() || map1.All(e => map2.ContainsKey(e.Key) && EqualityComparer<V>.Default.Equals(map2[e.Key], e.Value));
    }
}

public class PelicanRouteSegment
{
    public string? Name { get; private set; }
    public IReadOnlyDictionary<string, string> Pars { get; private set; }
    public IReadOnlyDictionary<string, string> Options { get; private set; }

    public PelicanRouteSegment(string? name, IReadOnlyDictionary<string, string>? pars=null, IReadOnlyDictionary<string, string>? options=null)
    {
        Name = name;
        Pars = pars==null ? ImmutableDictionary<string, string>.Empty : pars.ToImmutableDictionary();
        Options = options==null ? ImmutableDictionary<string, string>.Empty : options.ToImmutableDictionary();
    }

    public PelicanRouteSegment CopyWith(string? name, IReadOnlyDictionary<string, string>? pars, IReadOnlyDictionary<string, string>? options) {
        return new PelicanRouteSegment(
            name ?? this.Name,
            pars ?? this.Pars,
            options ?? this.Options
        );
    }

    public static IReadOnlyDictionary<string, string> MapFromValues(string values)
    {
        var pairs = values.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                          .Select(part => part.Split('='))
                          .Where(parts => parts.Length == 2)
                          .ToDictionary(parts => parts[0], parts => parts[1]);
        return new ReadOnlyDictionary<string, string>(pairs);
    }

    public static string GetName(string segment)
    {
        var parts = segment.Split(';');
        if (!parts.Any())
            throw new Exception("segment is empty");
        return parts[0];
    }

    public static PelicanRouteSegment FromPath(string path) {
        var parts = path.Split('+');
        var nameAndPars = parts[0];
        var optionsStr = parts.Length > 1 ? parts[1] : "";
        var nameAndParsParts = nameAndPars.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        var name = nameAndParsParts.Any() ? nameAndParsParts[0] : "";
        nameAndParsParts.RemoveAt(0);
        var pars = MapFromValues(string.Join(";", nameAndParsParts));
        var options = MapFromValues(optionsStr);
        return new PelicanRouteSegment(name, pars, options);
    }

    public string? ToRoutablePath() {
        return ToPath(includeOptions: false);
    }
    
    public string? ToPath(bool includeOptions = true, PelicanRouteSegment definition = null)
    {
        if (definition != null && definition.Name != Name)
            throw new Exception("definition name must match path name");

        var nameAndPars = ImmutableList.Create(Name);
        var ops = ImmutableList<string>.Empty;

        if (definition != null)
        {
            if (Pars.Any())
            {
                foreach (var p in definition.Pars.Keys)
                {
                    if (Pars.ContainsKey(p))
                    {
                        nameAndPars = nameAndPars.Add($"{p}={Pars[p]}");
                    }
                }
            }
            if (includeOptions && Options.Any())
            {
                foreach (var op in definition.Options.Keys)
                {
                    if (Options.ContainsKey(op))
                    {
                        ops = ops.Add($"{op}={Options[op]}");
                    }
                }
            }
        }
        else
        {
            var keys = Pars.Keys.OrderBy(k => k).ToImmutableList();
            foreach (var p in keys)
            {
                nameAndPars = nameAndPars.Add($"{p}={Pars[p]}");
            }
            keys = Options.Keys.OrderBy(k => k).ToImmutableList();
            if (includeOptions) foreach (var op in keys)
            {
                ops = ops.Add($"{op}={Options[op]}");
            }
        }
        return string.Join('+', new[] { string.Join(';', nameAndPars), string.Join(';', ops) }.Where(s => !string.IsNullOrEmpty(s)));
    }

    public bool Equals(PelicanRouteSegment other, bool ignoreOptions = true)
    {
        return Name == other.Name && MapExtensions.LinkedMapsEqual<string,string>(Pars, other.Pars) && (ignoreOptions || MapExtensions.LinkedMapsEqual(Options, other.Options));
    }

}
