﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using zpi_aspnet_test.Extensions;
using HtmlAgilityPack;
using zpi_aspnet_test.Enumerators;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test_xpath_parser
{
	public static class Parser
	{
		private static readonly Dictionary<int, ProductCategoryEnum> Dict = new Dictionary<int, ProductCategoryEnum>
		{
			{7, ProductCategoryEnum.Groceries},
			{9, ProductCategoryEnum.PreparedFood},
			{11, ProductCategoryEnum.PrescriptionDrug},
			{13, ProductCategoryEnum.NonPrescriptionDrug},
			{15, ProductCategoryEnum.Clothing},
			{17, ProductCategoryEnum.Intangibles}
		};

		private static NumberFormatInfo _format;

		static Parser() => _format = new NumberFormatInfo {NumberDecimalSeparator = "."};

		public static List<StateOfAmericaModel> GetStatesModelsFromWikipedia()
		{
			var possibleNodes = GetNodes();
			var states = new List<StateOfAmericaModel>();

			if (!(possibleNodes is IEnumerable<HtmlNode> nodes)) throw new ArgumentNullException();

			foreach (var n in nodes)
			{
				var state = new StateOfAmericaModel();
				var rates = new List<TaxModel>();
				for (var index = 1; index < n.ChildNodes.Count; index += 2)
				{
					var childNode = n.ChildNodes[index];
					switch (index)
					{
						case 1:
							state.Name = childNode.InnerText.Trim();
							break;
						case 3:
							var percent = childNode.InnerText.Trim();
							state.BaseSalesTax =
								double.Parse(percent.Substring(0, percent.Length - 1), _format);
							break;
						case 7:
						case 9:
						case 11:
						case 13:
						case 15:
						case 17:
							var type = Dict[index];
							ParseRateForProductType(type, childNode, rates, state);
							break;
					}
				}

				state.TaxRates = rates;
				states.Add(state);
			}

			return states;
		}

		private static void ParseRateForProductType(ProductCategoryEnum type, HtmlNode childNode,
			List<TaxModel> rates, StateOfAmericaModel state)
		{
			var style = childNode.Attributes["style"].Value;
			var kV = ParseStyle(style);
			var innerText = childNode.InnerText.Trim();
			var background = kV.GetValueOrDefault("background", "");
			var foreground = kV.GetValueOrDefault("color", "black");
			switch (background)
			{
				case "#f62b0f":
					rates.Add(new TaxModel {CategoryId = (int) type, MinValue = 0.0, MaxValue = 0.0, TaxRate = 0.0});
					break;
				case "#4ee04e":
					if (innerText.Length > 0)
					{
						var id = innerText.IndexOf('$') + 1;
						innerText = innerText.Substring(id > 0 ? id : 0, id > 0 ? innerText.Length - id : 0);
					}

					var condition = innerText.Length > 0
						? double.Parse(innerText.Trim(), _format)
						: 0;

					rates.Add(new TaxModel
						{CategoryId = (int) type, MinValue = 0.0, MaxValue = condition, TaxRate = 0.0});

					if (condition > 0)
					{
						rates.Add(new TaxModel
						{
							CategoryId = (int) type, MinValue = condition + 0.01, MaxValue = double.MaxValue,
							TaxRate = state.BaseSalesTax
						});
					}

					break;
				case "#7788ff":
					if (innerText.Length > 0)
					{
						var idx = innerText.IndexOf('%');
						innerText = innerText.Substring(0, idx < 0 ? 0 : idx);
					}

					var taxRate = innerText.Length == 0
						? state.BaseSalesTax
						: double.Parse(innerText.Trim(), _format);
					rates.Add(new TaxModel
						{CategoryId = (int) type, MinValue = 0.0, MaxValue = 0.0, TaxRate = taxRate,});
					break;
			}
		}

		private static Dictionary<string, string> ParseStyle(string cssString)
		{
			var dict = new Dictionary<string, string>();
			var strings = cssString.Split(' ');
			foreach (var s in strings)
			{
				var arr = s.Split(':');
				if (arr[1].Contains(';'))
				{
					arr[1] = arr[1].Substring(0, arr[1].Length - 1);
				}

				dict.Add(arr[0], arr[1]);
			}

			return dict;
		}

		private static IEnumerable<HtmlNode> GetNodes()
		{
			var web = new HtmlWeb();
			var html = web.Load(@"https://en.wikipedia.org/wiki/Sales_taxes_in_the_United_States");
			var node = html.DocumentNode.SelectNodes("//table")
			   .FirstOrDefault(htmlNode => htmlNode.Attributes["class"].Value == "wikitable sortable");
			var nodes = node?.SelectSingleNode("./tbody").ChildNodes
			   .Where(htmlNode => htmlNode.ChildNodes.Any(n => n.Name == "td"));
			return nodes;
		}
	}
}