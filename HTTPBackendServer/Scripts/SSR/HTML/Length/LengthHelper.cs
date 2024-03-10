using System;
using System.Globalization;
using System.Text.RegularExpressions;


namespace DDUKServer.HTML
{
	public static class LengthHelper
	{
		/// <summary>
		/// 파싱 시도.
		/// </summary>
		public static bool TryParse(string text, out double value, out UnitOfLength unitOfLength)
		{
			value = default;
			unitOfLength = UnitOfLength.Invalid;
			var match = Regex.Match(text, @"^([\d\.]+)([a-z%]+)$", RegexOptions.IgnoreCase);
			if (!match.Success)
				return false;

			var valuePart = match.Groups[1].Value;
			if (!double.TryParse(valuePart, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
				return false;

			var unitPart = match.Groups[2].Value.ToLower();
			switch (unitPart)
			{
				case "pixel":
				case "px":
					{
						unitOfLength = UnitOfLength.Pixel;
						return true;
					}

				case "percent":
				case "%":
					{
						unitOfLength = UnitOfLength.Percentage;
						return true;
					}

				case "em":
					{
						unitOfLength = UnitOfLength.EM;
						return true;
					}

				case "loosyscale":
				case "rem":
					{
						unitOfLength = UnitOfLength.REM;
						return true;
					}

				case "vw":
					{
						unitOfLength = UnitOfLength.VW;
						return true;
					}

				case "vh":
					{
						unitOfLength = UnitOfLength.VH;
						return true;
					}

				case "vmin":
					{
						unitOfLength = UnitOfLength.VMIN;
						return true;
					}

				case "vmax":
					{
						unitOfLength = UnitOfLength.VMAX;
						return true;
					}

				default:
					{
						unitOfLength = UnitOfLength.Invalid;
						return false;
					}
			}
		}

		/// <summary>
		/// 상대 단위로 전환.
		/// </summary>
		public static RelativeLength ConvertAbsoluteLengthToRelativeLength(AbsoluteLength absoluteLength, UnitOfLength unitOfLength, Element element)
		{
			// 요소랑 지지고 볶고 해서 픽셀단위로 변환해야함.
			var originalValue = absoluteLength.Value;
			var finalValue = originalValue;

			return new RelativeLength(finalValue, unitOfLength);
		}

		/// <summary>
		/// 픽셀 단위로 전환.
		/// </summary>
		public static AbsoluteLength ConvertRelativeLengthToAbsoluteLength(RelativeLength relativeLength, Element element)
		{
			var originalValue = relativeLength.Value;
			var originalUnitOfSize = relativeLength.UnitOfLength;

			// 요소랑 지지고 볶고 해서 픽셀단위로 변환해야함.
			var finalValue = originalValue;

			return new AbsoluteLength(finalValue);
		}

		/// <summary>
		/// 길이 단위를 문자열로 변환.
		/// </summary>
		public static string GetUnitOfLengthText(UnitOfLength unitOfLength)
		{
			return unitOfLength switch
			{
				UnitOfLength.Pixel => "px",
				UnitOfLength.Percentage => "%",
				UnitOfLength.EM => "em",
				UnitOfLength.REM => "rem",
				UnitOfLength.VW => "vw",
				UnitOfLength.VH => "vh",
				UnitOfLength.VMIN => "vmin",
				UnitOfLength.VMAX => "vmax",
				_ => throw new ArgumentException(),
			};
		}

		/// <summary>
		/// 길이 단위와 값을 문자열로 변환.
		/// </summary>
		public static string ToString(double value, UnitOfLength unitOfLength)
		{
			string unitOfLengthText = LengthHelper.GetUnitOfLengthText(unitOfLength);
			return $"{value}{unitOfLengthText}";
		}
	}
}