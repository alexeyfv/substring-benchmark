using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using Bogus;

namespace Benchmarks;

[MemoryDiagnoser]
public class SubstringBenchmark
{
    private const int size = 100_000;
    private readonly string[] _data = new string[size];
    private readonly string[] _result = new string[size];
    private readonly char _symbol = '/';
    private readonly string _pattern = @"[^\/]+$";

    [GlobalSetup]
    public void GlobalSetup()
    {
        var faker = new Faker();

        for (int i = 0; i < _data.Length; i++)
        {
            _data[i] = faker.System.FilePath();
        }
    }

    [Benchmark(Baseline = true)]
    public string[] Substring()
    {
        for (int i = 0; i < _data.Length; i++)
        {
            var s = _data[i];
            var start = s.LastIndexOf(_symbol) + 1;
            _result[i] = s.Substring(start);
        }

        return _result;
    }

    [Benchmark]
    public string[] RangeOperator()
    {
        for (int i = 0; i < _data.Length; i++)
        {
            var s = _data[i];
            var start = s.LastIndexOf(_symbol) + 1;
            _result[i] = s[start..];
        }

        return _result;
    }

    [Benchmark]
    public string[] Linq()
    {
        for (int i = 0; i < _data.Length; i++)
        {
            var s = _data[i];
            var start = s.LastIndexOf(_symbol) + 1;
            _result[i] = new string(s.TakeLast(s.Length - start).ToArray());
        }

        return _result;
    }

    [Benchmark]
    public string[] Split()
    {
        for (int i = 0; i < _data.Length; i++)
        {
            var s = _data[i];
            _result[i] = s.Split(_symbol)[^1];
        }

        return _result;
    }

    [Benchmark]
    public string[] RegularExpressions()
    {
        for (int i = 0; i < _data.Length; i++)
        {
            var s = _data[i];
            _result[i] = Regex.Match(s, _pattern).Groups[0].Value;
        }

        return _result;
    }
}