using System;
using System.Collections.Generic;
using zpi_aspnet_test.Enumerators;
using zpi_aspnet_test.Models;

namespace zpi_aspnet_test.Algorithms
{
    public class Algorithm
    {
        public static void CalculateFinalPrice(ProductModel product, StateOfAmericaModel state)
        {
			double tax = GetTax(product, state);
            
            product.FinalPrice = Math.Round(product.PreferredPrice - (product.PreferredPrice * (tax / 100)), 2);
        }

        public static double GetTax(ProductModel product, StateOfAmericaModel state)
        {
            double tax;

            switch ((ProductCategoryEnum)product.CategoryId)
            {
                case ProductCategoryEnum.Groceries:
                    tax = state.Groceries;
                    break;
                case ProductCategoryEnum.PreparedFood:
                    tax = state.PreparedFood;
                    break;
                case ProductCategoryEnum.PrescriptionDrug:
                    tax = state.PrescriptionDrug;
                    break;
                case ProductCategoryEnum.NonPrescriptionDrug:
                    tax = state.NonPrescriptionDrug;
                    break;
                case ProductCategoryEnum.Clothing:
                    tax = state.Clothing;
                    break;
                case ProductCategoryEnum.Intangibles:
                    tax = state.Intangibles;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return tax;
        }

        public static void SetFinalPrices(ProductModel product, List<StateOfAmericaModel> states)
        {
            foreach (var state in states)
            {
                CalculateFinalPrice(product, state);
            }
        }

        public static double CalculateMargin(ProductModel product)
        {
            return Math.Round(product.FinalPrice - product.PurchasePrice, 2);
        }

        public static void ChangeMargin(ProductModel product, double margin)
        {
            product.FinalPrice = margin + product.PurchasePrice;
        }
    }
}