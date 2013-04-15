﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using Newtonsoft.Json;

namespace OpenExchangeRates
{
	/// <summary>
	/// Open exchange rates data fetcher.
	/// </summary>
	public class Client
	{
		static readonly string ApiKey = ConfigurationManager.AppSettings["OpenExchangeRates.ApiKey"];
		static readonly string LatestUrl = "https://openexchangerates.org/api/latest.json?app_id=" + ApiKey;
		static readonly string HistoryUrl = "https://openexchangerates.org/api/historical/{0:yyyy-MM-dd}.json?app_id=" + ApiKey;

		/// <summary>
		/// Retrieve the latest exchange rate data structure.
		/// </summary>
		/// <param name="symbols">The currency symbols to return when passed. If null then all available currencies are returned.</param>
		/// <param name="baseCurrency">The currency to use as the base currency on which all rates are based.</param>
		/// <returns>An <see cref="ExchangeRateData"/> instance.</returns>
		public static ExchangeRateData Get(IEnumerator<string> symbols = null, string baseCurrency = null)
		{
			var url = LatestUrl;
			if (symbols != null) url += "&symbols=" + string.Join(",", symbols);
			if (baseCurrency != null) url += "&base=" + baseCurrency;
			using (var client = new WebClient())
			{
				var json = client.DownloadString(url);
				return JsonConvert.DeserializeObject<ExchangeRateData>(json);
			}
		}

		/// <summary>
		/// Retrieve the historical exchange rate data structure for a given date.
		/// </summary>
		/// <param name="date">The historical query date</param>
		/// <param name="symbols">The currency symbols to return when passed. If null then all available currencies are returned.</param>
		/// <param name="baseCurrency">The currency to use as the base currency on which all rates are based.</param>
		/// <returns>An <see cref="ExchangeRateData"/> instance.</returns>
		public static ExchangeRateData Get(DateTime date, IEnumerator<string> symbols = null, string baseCurrency = null)
		{
			var url = string.Format(HistoryUrl, date);
			if (symbols != null) url += "&symbols=" + string.Join(",", symbols);
			if (baseCurrency != null) url += "&base=" + baseCurrency;
			using (var client = new WebClient())
			{
				var json = client.DownloadString(url);
				return JsonConvert.DeserializeObject<ExchangeRateData>(json);
			}
		}
	}
}
