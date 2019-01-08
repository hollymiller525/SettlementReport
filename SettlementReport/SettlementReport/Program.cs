using System;
using System.Collections.Generic;
using System.Linq;


namespace SettlementReport
{
    class Program
    {
        static void Main()
        {
            Console.SetWindowSize(200, 40);
            List<Settlement> settlements = new List<Settlement>();
            Console.WriteLine("Welcome to Settlements Report");
            Console.WriteLine();

            settlements.Add(GetSettlementDetails());

            OptionMenu(settlements);

            Console.Read();
        }

        public static List<Settlement> OptionMenu(List<Settlement> settlements)
        {
            do
            {
                Console.WriteLine("To generate a settlement report, press 1.");
                Console.WriteLine("To add another settlement, press 2.");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        return GenerateReport(settlements);
                    case "2":
                        settlements.Add(GetSettlementDetails());
                        break;
                    default:
                        Console.WriteLine("Sorry this is not a valid option please try again!");
                        return OptionMenu(settlements);
                }
            } while (true);
        }

        public static Settlement GetSettlementDetails()
        {
            Settlement settlement = new Settlement
            {
                Id = Guid.NewGuid(),
                Name = GetSettlementName(),
                Type = GetSettlementType(),
                CurrencyType = GetSettlementCurrency(),
                Units = GetSettlementUnits(),
                InstructionDate = DateTime.Now.ToString("dd MMM yyyy")
            };
            settlement.SettlementDate = GetSettlementDate(settlement.CurrencyType).ToString("dd MMM yyyy");
            return settlement;
        }

        public static string GetSettlementName()
        {
            Console.WriteLine("Please insert a name for your settlement.");
            string settlementName = Console.ReadLine();
            Console.WriteLine();

            if (settlementName != String.Empty)
            {
                Console.WriteLine("You have entered {0}", settlementName);
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Please ensure that a name is inputted for the settlement.");
                GetSettlementName();
            }


            return settlementName;
        }

        public static SettlementTypeEnum GetSettlementType()
        {
            SettlementTypeEnum settlementTypeAgreement = 0;

            Console.WriteLine("Please indicate what type of settlement transaction you are making:");
            Console.WriteLine("- Buying");
            Console.WriteLine("- Selling");
            Console.WriteLine("");

            var settlementType = Console.ReadLine();

            Console.WriteLine("You have entered {0}", settlementType);
            Console.WriteLine();

            switch (settlementType.ToLower())
            {
                case "buying":
                    settlementTypeAgreement = SettlementTypeEnum.Buying;
                    break;
                case "selling":
                    settlementTypeAgreement = SettlementTypeEnum.Selling;
                    break;
                default:
                    Console.WriteLine("You did not enter a valid settlement type, please try again!");
                    GetSettlementType();
                    break;
            }

            return settlementTypeAgreement;
        }

        public static SettlementCurrencyEnum GetSettlementCurrency()
        {
            SettlementCurrencyEnum settlementCurrencyAgreement = 0;

            Console.WriteLine("Please choose the currency you wish to make your settlement in:");
            Console.WriteLine("- SGP");
            Console.WriteLine("- GBP");
            Console.WriteLine("- AED");
            Console.WriteLine();

            var currency = Console.ReadLine();

            Console.WriteLine("You have entered {0}", currency);
            Console.WriteLine();

            switch (currency.ToLower())
            {
                case "sgp":
                    settlementCurrencyAgreement = SettlementCurrencyEnum.SGP;
                    break;
                case "aed":
                    settlementCurrencyAgreement = SettlementCurrencyEnum.AED;
                    break;
                case "gbp":
                    settlementCurrencyAgreement = SettlementCurrencyEnum.GBP;
                    break;
                default:
                    Console.WriteLine("You did not enter a valid currency, please try again!");
                    GetSettlementCurrency();
                    break;
            }

            return settlementCurrencyAgreement;
        }

        public static int GetSettlementUnits()
        {

            // could be good to make sure its a int
            Console.WriteLine("Please indicate the settlement units.");
            Console.WriteLine();
            string settlementUnitsText = Console.ReadLine();
            Console.WriteLine();

            var isNumeric = int.TryParse(settlementUnitsText, out int settlementUnits);
            if (!isNumeric)
            {
                Console.WriteLine("Please insert a valid unit amount.");
                GetSettlementUnits();
            }
            return settlementUnits;
        }

        public static DateTime GetSettlementDate(SettlementCurrencyEnum settlementCurrency)
        {
            Console.WriteLine("Please indicate the date you would like the settlement completed for (dd/mmm/yyyy).");
            DateTime settlementDateValue = new DateTime();
            string settlementDate = Console.ReadLine();
            try
            {
                settlementDateValue = DateTime.Parse(settlementDate);
                if (settlementDateValue <= DateTime.Now)
                {
                    Console.WriteLine("The settlement date cannot be less than or equal to the current date.");
                    GetSettlementDate(settlementCurrency);
                    Console.WriteLine();
                }
                if (settlementCurrency == SettlementCurrencyEnum.SGP || settlementCurrency == SettlementCurrencyEnum.AED)
                {
                    // SUN - THURS
                    if (settlementDateValue.DayOfWeek == DayOfWeek.Friday)
                    {
                        settlementDateValue = settlementDateValue.AddDays(2);
                        Console.WriteLine("The date you selected was a {0}", DayOfWeek.Friday);
                        Console.WriteLine("We have updated your settlement date to: {0}", settlementDateValue.ToString("dd/M/yyyy"));
                        Console.WriteLine();
                    }
                    else if (settlementDateValue.DayOfWeek == DayOfWeek.Saturday)
                    {
                        settlementDateValue = settlementDateValue.AddDays(1);
                        Console.WriteLine("The date you selected was a {0}", DayOfWeek.Saturday);
                        Console.WriteLine("We have updated your settlement date to: {0:dd/M/yyyy}", settlementDateValue);
                        Console.WriteLine();
                    }
                }
                else
                {
                    // MON - FRI
                    if (settlementDateValue.DayOfWeek == DayOfWeek.Saturday)
                    {
                        settlementDateValue = settlementDateValue.AddDays(2);
                        Console.WriteLine("The date you selected was a {0}", DayOfWeek.Saturday);
                        Console.WriteLine("We have updated your settlement date to: {0:dd/M/yyyy}", settlementDateValue);
                        Console.WriteLine();
                    }
                    else if (settlementDateValue.DayOfWeek == DayOfWeek.Sunday)
                    {
                        settlementDateValue = settlementDateValue.AddDays(1);
                        Console.WriteLine("The date you selected was a {0}", DayOfWeek.Sunday);
                        Console.WriteLine("We have updated your settlement date to: {0:dd/M/yyyy}", settlementDateValue);
                        Console.WriteLine();
                    }
                }
            }
            catch
            {
                Console.WriteLine("You did not enter a valid date, please try again using the correct format (dd/mm/yyyy)!");
                GetSettlementDate(settlementCurrency);
                Console.WriteLine();
            }
            return Convert.ToDateTime(settlementDateValue);
        }

        public static List<Settlement> GenerateReport(List<Settlement> settlements)
        {
            string[] columnHeadings = new[] { "Entity", "Buy/Sell", "AgreedFx", "Currency", "InstructionDate", "SettlementDate", "Units", "Price per unit", "Value" };
            IEnumerable<Settlement> incomingSettlements = settlements.Where(x => x.Type == SettlementTypeEnum.Selling);
            IEnumerable<Settlement> outgoingSettlements = settlements.Where(x => x.Type == SettlementTypeEnum.Buying);
            if (incomingSettlements.Any())
            {

                PrintLine();
                PrintRow("---Incoming---");
                PrintLine();
                PrintRow(columnHeadings);
                PrintLine();
                foreach (var settlement in incomingSettlements.OrderByDescending(x => x.Units))
                {
                    var usdIncomingValue = GenerateIncomingReport(settlement);
                    PrintRow(settlement.Name, settlement.Type.ToString(), "0.22", settlement.CurrencyType.ToString(), settlement.InstructionDate, settlement.SettlementDate, settlement.Units.ToString(), "150.5", usdIncomingValue.ToString());
                }
                PrintLine();
            }
            if (outgoingSettlements.Any())
            {

                PrintLine();
                PrintRow("---Outgoing---");
                PrintLine();
                PrintRow(columnHeadings);
                PrintLine();
                foreach (var settlement in outgoingSettlements.OrderByDescending(x => x.Units))
                {
                    var usdOutgoingValue = GenerateOutGoingReport(settlement);
                    PrintRow(settlement.Name, settlement.Type.ToString(), "0.50", settlement.CurrencyType.ToString(), settlement.InstructionDate, settlement.SettlementDate, settlement.Units.ToString(), "100.25", usdOutgoingValue.ToString());
                }
                PrintLine();
            }
            Console.Write("\nDone!\nPress any key to exit...");

            return settlements;
        }

        // selling
        public static int GenerateIncomingReport(Settlement settlement)
        {
            int usdIncomingTotal = 0;

            switch (settlement.CurrencyType)
            {
                case SettlementCurrencyEnum.SGP:
                    usdIncomingTotal = (int)(150.5 * settlement.Units * 0.50);
                    break;

                case SettlementCurrencyEnum.AED:
                    usdIncomingTotal = (int)(150.5 * settlement.Units * 0.22);
                    break;

                case SettlementCurrencyEnum.GBP:
                    usdIncomingTotal = (int)(150.5 * settlement.Units * 0.22);
                    break;
            }

            return usdIncomingTotal;
        }

        // buying
        public static int GenerateOutGoingReport(Settlement settlement)
        {
            int usdIncomingTotal = 0;
            switch (settlement.CurrencyType)
            {
                case SettlementCurrencyEnum.SGP:
                    usdIncomingTotal = (int)(100.25 * settlement.Units * 0.50);
                    break;

                case SettlementCurrencyEnum.AED:
                    usdIncomingTotal = (int)(100.25 * settlement.Units * 0.22);
                    break;

                case SettlementCurrencyEnum.GBP:
                    usdIncomingTotal = (int)(100.25 * settlement.Units * 0.22);
                    break;
            }

            return usdIncomingTotal;
        }


        public class Settlement
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public SettlementTypeEnum Type { get; set; }
            public SettlementCurrencyEnum CurrencyType { get; set; }
            public int Units { get; set; }
            public string InstructionDate { get; set; }
            public string SettlementDate { get; set; }
        }

        public enum SettlementTypeEnum
        {
            Buying = 0,
            Selling = 1
        }

        public enum SettlementCurrencyEnum
        {
            SGP = 0,
            AED = 1,
            GBP = 2
        }
        static int tableWidth = 200;

        static void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth));
        }

        static void PrintRow(params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
    }
}


