namespace g.identity.business.Services;

public interface IRandomStringGenerator
{
    public string Generate(int length = 8, bool complex = false);
}