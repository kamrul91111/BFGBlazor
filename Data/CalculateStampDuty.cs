using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BFGBlazor.Data
{
    public class CalculateStampDuty
    {
        public double Calculate(string FirstHomeBuyerUserChoice, string PropertyLocationUserChoice, string TypeOfPropertyUserChoice, double PriceOfProperty, double StampDuty)
        {
            // NSW stamp duty exemptions for first home buyers.
            if (FirstHomeBuyerUserChoice.Equals("Yes") && PropertyLocationUserChoice.Equals("NSW"))
            {
                // If you're a first home buyer purchasing a new home.
                if (TypeOfPropertyUserChoice.Equals("Newly built dwelling"))
                {
                    // If your property is valued at up to $800,000 you pay no transfer duty.
                    if (PriceOfProperty <= 800000)
                    {
                        return StampDuty = 0;
                    }

                    // There are concessions for properties valued between $800,000 - $1,000,000.
                    else if (PriceOfProperty > 800000 && PriceOfProperty <= 1000000)
                    {
                        // To calculate the First Home Buyers Assistance concession on a new home between $800,000 and $1,000,000,
                        // multiply the purchase price by 0.201675 and subtract $161,340.
                        return StampDuty = (double)(PriceOfProperty * 0.201675) - 161340;
                    }

                    // For PriceOfProperty > 1 million.
                    else
                    {
                        return CalStampDuty(PriceOfProperty, StampDuty);
                    }
                }

                //  For first home buyers purchasing existing property.
                else if (TypeOfPropertyUserChoice.Equals("Existing property"))
                {
                    // If your property is valued at less than $650,000 you pay no transfer duty.
                    if (PriceOfProperty <= 650000)
                    {
                        return StampDuty = 0;
                    }

                    // If your property is valued between $650,000 and $800,000 you can receive a discount.
                    else if (PriceOfProperty > 650000 && PriceOfProperty <= 800000)
                    {
                        // To calculate the First Home Buyers Assistance concession on an established home between $650,000 and $800,000,
                        // multiply the purchase price by 20.89% and subtract $135,785.
                        return StampDuty = (double)(PriceOfProperty * (double)(20.89 / 100)) - 135785;
                    }

                    // For PriceOfProperty > 800,000.
                    else
                    {
                        return CalStampDuty(PriceOfProperty, StampDuty);
                    }
                }

                // If you're a first home buyer purchasing a vacant land.
                else if (TypeOfPropertyUserChoice.Equals("Vacant land"))
                {
                    // If your land is valued less than $400,000 you pay no transfer duty.
                    if (PriceOfProperty <= 400000)
                    {
                        return StampDuty = 0;
                    }

                    // There are concessions for properties valued between $400,000 - $500,000.
                    else if (PriceOfProperty > 400000 && PriceOfProperty <= 500000)
                    {
                        // To calculate concession on land between $400,000 and $500,000,
                        // multiply the purchase price by 0.17835 and subtract $71,340.
                        return StampDuty = (double)(PriceOfProperty * 0.17835) - 71340;
                    }

                    // For PriceOfProperty > 500,000.
                    else
                    {
                        return CalStampDuty(PriceOfProperty, StampDuty);
                    }
                }
            }
            // For non first home buyers.
            else
            {
                return CalStampDuty(PriceOfProperty, StampDuty);
            }
            return StampDuty;
        }
        public double CalStampDuty(double PriceOfProperty, double StampDuty)
        {
            // Property value $0 - $14,000.
            if (PriceOfProperty > 0 && PriceOfProperty <= 14000)
            {
                // Transfer duty rate is
                // $1.25 for every $100 (the minimum is $10).
                return StampDuty = 1.25 * (double)(PriceOfProperty / 100);
            }

            // Property value $14,000 - $31,0000.
            else if (PriceOfProperty > 14000 && PriceOfProperty <= 31000)
            {
                // Transfer duty rate is
                // $175 plus $1.50 for every $100 over $14,000
                return StampDuty = (1.5 * (double)((PriceOfProperty - 14000) / 100)) + 175;
            }

            // Property value $31,000 - $83,0000.
            else if (PriceOfProperty > 31000 && PriceOfProperty <= 83000)
            {
                // Transfer duty rate is
                // $430 plus $1.75 for every $100 over $31,000
                return StampDuty = (1.75 * (double)((PriceOfProperty - 31000) / 100)) + 430;
            }

            // Property value $83,000 - $310,0000.
            else if (PriceOfProperty > 83000 && PriceOfProperty <= 310000)
            {
                // Transfer duty rate is
                // $1,340 plus $3.50 for every $100 over $83,000
                return StampDuty = (3.5 * (double)((PriceOfProperty - 83000) / 100)) + 1340;
            }

            // Property value $310,000 - $1,033,000.
            else if (PriceOfProperty > 310000 && PriceOfProperty <= 1033000)
            {
                // Transfer duty rate is
                // $9,285 plus $4.50 for every $100 over $310,000
                return StampDuty = (4.5 * (double)((PriceOfProperty - 310000) / 100)) + 9285;
            }

            // Property value over $1,033,000.
            else if (PriceOfProperty > 1033000)
            {
                // Transfer duty rate is
                // $41,820 plus $5.50 for every $100 over $1,033,000
                return StampDuty = (5.5 * (double)((PriceOfProperty - 1033000) / 100)) + 41820;
            }
            return StampDuty;
        }
    }
}
