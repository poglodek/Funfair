namespace Funfair.Shared.Api.Hateos;

public record Resource<T>(T Data, List<Link> Links = null);

public record Link(string Href, string Rel, string Method);
