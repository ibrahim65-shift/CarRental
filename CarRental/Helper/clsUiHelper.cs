using CarRental_Buisness.Helpers;
using CarRental_Buisness.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;

using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Helper
{
    public static class clsUiHelper
    {
        public static async Task<bool> FillComboBoxGenericAsync<T>(ComboBox comboBox , Func<Task<clsServiceResult<List<T>>>> fetchData ,
            string displayMember , string valueMember)
        {
            var result = await fetchData();

            if(!result.Success || result.Data ==null || result.Data.Count == 0)
            {
                comboBox.DataSource = null;
                return false;
            }

            comboBox.DataSource = result.Data;
            comboBox.DisplayMember = displayMember;
            comboBox.ValueMember = valueMember;

            return true;
        }
        public static void FillComboBoxGeneric<T>(ComboBox comboBox , IEnumerable<T> data ,
            string displayMember , string valueMember)
        {
            comboBox.DataSource = data.ToList();
            comboBox.DisplayMember = displayMember;
            comboBox.ValueMember = valueMember;
        }

        public static void FillComboBoxWithEnum<T>(ComboBox comboBox)
            where T :Enum
        {
            var list = Enum.GetValues(typeof(T))
               .Cast<T>()
               .Select(e => new
               {
                   Value = e,
                   Text = e.ToString()
               })
               .ToList();

            comboBox.DataSource = list;
            comboBox.ValueMember = "Value";
            comboBox.DisplayMember = "Text";
        }
        public static void FillComboBoxWithEnumDescriptions<T>(ComboBox comboBox)
            where T : Enum
        {
            var list = Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(e => new
                {
                    Value = e,
                    Text = e.GetType().GetField(e.ToString())
                            ?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? e.ToString()
                }).ToList();

            comboBox.DataSource = list;
            comboBox.ValueMember = "Value";  
            comboBox.DisplayMember = "Text";

            if (comboBox.Items.Count > 0)
                comboBox.SelectedIndex = 0;
        }
        public static string GetEnumDescription<T>(T value)
            where T : Enum
        {
            return value.GetType().GetField(value.ToString())?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? value.ToString();
        }
        public static string Serialize(string name , Color color)
        {
            return $"{name}|{ColorTranslator.ToHtml(color)}";
        }

        public static (string name , Color Color) Deserialize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return ("", Color.Gray);

            var parts = value.Split('|');

            if(parts.Length != 2)
                 return (value, Color.Gray);

            string name = parts[0];
            Color color = ColorTranslator.FromHtml(parts[1]);

            return (name, color);
        }

        public static string ToSAR(decimal value)
        {
            var culture = new CultureInfo("ar-SA");

            culture.NumberFormat.CurrencySymbol = "ريال";
            return value.ToString("C", culture);
        }

        public static readonly clsApplicationSettings applicationSettings = new clsApplicationSettings
        {
            TaxRate = Properties.Settings.Default.TaxRate,
            CurrencyCode = Properties.Settings.Default.CurrencyCode,
        };

    }
}
