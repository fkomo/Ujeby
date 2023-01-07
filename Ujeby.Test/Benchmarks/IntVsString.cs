using BenchmarkDotNet.Attributes;
using System;
using System.Linq;

namespace Ujeby.Test.Benchmarks
{
	public class IntVsString
	{
		private int[] _ints = null;
		private const int _maxInt = 100;

		private string[] _strings = null;
		private Random _rnd = null;

		public int _length = 1000;

		public string RandomString => $"{'A' + _rnd.Next('Z' - 'A')}{'A' + _rnd.Next('Z' - 'A')}";

		public IntVsString()
		{
			SetUp();
		}

		[GlobalSetup]
		public void SetUp()
		{
			_ints = new int[_length];
			_strings = new string[_length];
			_rnd = new Random((int)DateTime.Now.Ticks);

			for (var i = 0; i < _length; i++)
			{
				_ints[i] = _rnd.Next(_maxInt);
				_strings[i] = RandomString;
			}
		}

		[Benchmark]
		public bool ArrayContainsInt() => _ints.Contains(_rnd.Next(_maxInt));

		[Benchmark]
		public bool ArrayContainsString() => _strings.Contains(RandomString);
	}
}
