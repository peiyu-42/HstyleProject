namespace HStyleApi.Models.DTOs
{
	public class WeatherDTO
	{
		public class Field
		{
			public string id { get; set; }
			public string type { get; set; }
		}

		public class Location
		{
			public string locationName { get; set; }
			public List<WeatherElement> weatherElement { get; set; }
		}

		public class Parameter
		{
			public string parameterName { get; set; }
		}

		public class Records
		{
			public string datasetDescription { get; set; }
			public List<Location> location { get; set; }
		}

		public class Result
		{
			public string resource_id { get; set; }
			public List<Field> fields { get; set; }
		}

		public class Root
		{
			public string success { get; set; }
			public Result result { get; set; }
			public Records records { get; set; }
		}

		public class Time
		{
			public string startTime { get; set; }
			public string endTime { get; set; }
			public Parameter parameter { get; set; }
		}

		public class WeatherElement
		{
			public string elementName { get; set; }
			public List<Time> time { get; set; }
		}


	}
}
